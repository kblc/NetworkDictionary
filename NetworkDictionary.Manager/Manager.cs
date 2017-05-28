﻿using System;
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
    public class Manager : IManager, IDisposable
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
        /// Object already disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Options
        /// </summary>
        private readonly ManagerOptions _options;


        #endregion

        #region .ctor

        /// <summary>
        /// Create new instace with dfault TTL (Time to live)
        /// </summary>
        /// <param name="options">Options</param>
        public Manager(ManagerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _taskFactory = Task.Factory;
            _clearTimer = new Timer(ClearTimerCallback, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        #endregion

        /// <inheritdoc />
        public Task SetValue(string key, string value, TimeSpan? ttl = null)
        {
            return CreateSingleThreadTaskFromAction(() =>
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
                if (!_dictionary.TryGetValue(key, out DictionaryValue existedItem))
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
            item.Expired = DateTime.UtcNow + tll;
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
        /// <param name="state"></param>
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

        #region IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (disposing)
            {
                _dictionary.Clear();
            }
            _clearTimer.Dispose();
        }

        ~Manager()
        {
            Dispose(false);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Manager));
        }

        #endregion
    }
}