using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetworkDictionary.Manager.Exceptions;
using NetworkDictionary.Manager.Extensions;
using NetworkDictionary.Manager.Interfaces;
using NetworkDictionary.Manager.Models;

namespace NetworkDictionary.Manager
{
    /// <summary>
    /// Cache dictionary manager
    /// </summary>
    internal class Manager : IManager
    {
        #region Data

        /// <summary>
        /// Dictionary
        /// </summary>
        private readonly Dictionary<string, DictionaryValue> _dictionary = new Dictionary<string, DictionaryValue>();

        /// <summary>
        /// Lock object for implementation as signle thread
        /// </summary>
        private readonly object _oneThreadLockObject = new object();

        /// <summary>
        /// Task factory
        /// </summary>
        private readonly TaskFactory _taskFactory;

        /// <summary>
        /// Clear timer
        /// </summary>
        private readonly Timer _clearTimer;

        /// <summary>
        /// Frequince decrement timer
        /// </summary>
        private readonly Timer _frequinceDecrementTimer;

        /// <summary>
        /// Object already disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Options
        /// </summary>
        private readonly ManagerOptions _options;

        /// <summary>
        /// Cancellation token source for fabric
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region .ctor

        /// <summary>
        /// Create new instace of <see cref="Manager"/>
        /// </summary>
        /// <param name="options">Manager options</param>
        public Manager(ManagerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _taskFactory = Task.Factory;
            _cancellationTokenSource = new CancellationTokenSource();
            _taskFactory = new TaskFactory(_cancellationTokenSource.Token, TaskCreationOptions.None, TaskContinuationOptions.PreferFairness, TaskScheduler.Default);
            _clearTimer = new Timer(ClearTimerCallback, null, options.ClearExpiredValuesPeriod, options.ClearExpiredValuesPeriod);
            _frequinceDecrementTimer = new Timer(FrequinceDecrementTimerCallback, null, options.DecreaseValueFrequincePeriod, options.DecreaseValueFrequincePeriod);
        }

        #endregion

        /// <inheritdoc />
        public Task SetValue(string key, string value, TimeSpan? ttl = null)
        {
            return value == null
                ? DeleteValue(key)
                : CreateSingleThreadTaskFromAction(() =>
                    {
                        if (_dictionary.TryGetValue(key, out DictionaryValue existedItem))
                        {
                            SetDictionaryItemValue(existedItem, value, ttl ?? _options.DefaultTtl);
                        }
                        else
                        {
                            var newValue = SetDictionaryItemValue(new DictionaryValue(), value, ttl ?? _options.DefaultTtl);
                            AllocateKeyForNewOne(_options.MaxKeyCount);
                            _dictionary.Add(key, newValue);
                        }
                    });
        }

        /// <inheritdoc />
        public Task<string> GetValue(string key)
        {
            return CreateSingleThreadTaskFromFunction(() =>
            {
                if (!_dictionary.TryGetValue(key, out DictionaryValue existedItem) || existedItem.Expired < DateTime.UtcNow)
                    return null;

                existedItem.IncrementRequestCount();
                return existedItem.Value;
            });
        }

        /// <inheritdoc />
        public Task<bool> DeleteValue(string key)
        {
            return CreateSingleThreadTaskFromFunction(() =>
            {
                if (!_dictionary.ContainsKey(key))
                    return false;

                _dictionary.Remove(key);
                return true;
            });
        }

        /// <inheritdoc />
        public Task<string[]> GetKeys()
        {
            return CreateSingleThreadTaskFromFunction(() => _dictionary.Keys.ToArray());
        }

        /// <summary>
        /// Set value and other parameters for dictionary value
        /// </summary>
        /// <param name="item">Dictionary value</param>
        /// <param name="value">Value</param>
        /// <param name="tll">Time to live</param>
        /// <returns>The same dictionary value to produce fluent syntax</returns>
        private static DictionaryValue SetDictionaryItemValue(DictionaryValue item, string value, TimeSpan tll)
        {
            item.Value = value;
            item.Expired = tll == Timeout.InfiniteTimeSpan ? DateTime.MaxValue : DateTime.UtcNow + tll;
            item.IncrementRequestCount();
            return item;
        }

        /// <summary>
        /// Release one key if needed (Most frequince use strategy)
        /// </summary>
        /// <param name="maxKeyCount">Max key value</param>
        private void AllocateKeyForNewOne(int maxKeyCount)
        {
            if (_dictionary.Count == maxKeyCount)
            {
                var keyToDelete = _dictionary
                    .OrderByDescending(kvp => kvp.Value.RequestCount)
                    .ThenBy(kvp => kvp.Value.Expired)
                    .Select(kvp => kvp.Key)
                    .FirstOrDefault();

                if (keyToDelete == null)
                    throw new ManagerException("The key for release have not been founded");

                _dictionary.Remove(keyToDelete);
            }
        }

        /// <summary>
        /// Execute function in task as single thread
        /// </summary>
        /// <typeparam name="T">Function return type</typeparam>
        /// <param name="function">Function with return <typeparamref name="T"/></param>
        /// <returns>Typed task of <typeparamref name="T"/></returns>
        private Task<T> CreateSingleThreadTaskFromFunction<T>(Func<T> function)
        {
            ThrowIfDisposed();
            return _taskFactory.StartNew(() =>
            {
                lock (_oneThreadLockObject)
                {
                    return function();
                }
            });
        }

        /// <summary>
        /// Execute action in task as single thread
        /// </summary>
        /// <param name="action">Action to execute</param>
        /// <returns>Task with action</returns>
        private Task CreateSingleThreadTaskFromAction(Action action)
        {
            ThrowIfDisposed();
            return _taskFactory.StartNew(() =>
            {
                lock (_oneThreadLockObject)
                {
                    action();
                }
            });
        }

        /// <summary>
        /// Callback for clear timer
        /// </summary>
        /// <param name="state">State object (not used)</param>
        private void ClearTimerCallback(object state)
        {
            CreateSingleThreadTaskFromAction(() =>
            {
                var currentDate = DateTime.UtcNow;
                var expiredKeys = _dictionary.Where(kvp => kvp.Value.Expired < currentDate).Select(kvp => kvp.Key).ToArray();
                foreach (var expiredKey in expiredKeys)
                {
                    _dictionary.Remove(expiredKey);
                }
            });
        }

        /// <summary>
        /// Callback for frequince decrementation timer
        /// </summary>
        /// <param name="state">State object (not used)</param>
        private void FrequinceDecrementTimerCallback(object state)
        {
            CreateSingleThreadTaskFromAction(() =>
            {
                foreach (var dictionaryValue in _dictionary.Values)
                {
                    dictionaryValue.DecrementRequestCount();
                }
            });
        }

        #region IDisposable

        /// <summary>
        /// Dispose from IDisposable.Dispose()
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        /// <param name="disposing">Is called by IDisposable.Dispose()</param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            _cancellationTokenSource.Cancel();
            _clearTimer.Dispose();
            _frequinceDecrementTimer.Dispose();

            if (disposing)
            {
                _dictionary.Clear();
            }
        }

        /// <summary>
        /// Destroy from GC
        /// </summary>
        ~Manager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Throw an exception <see cref="ObjectDisposedException"/> if this object has been already disposed
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Manager));
        }

        #endregion
    }
}
