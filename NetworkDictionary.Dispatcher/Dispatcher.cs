using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NetworkDictionary.Dispatcher.Interfaces;
using NetworkDictionary.Domain.Dto;
using NetworkDictionary.Manager.Interfaces;

namespace NetworkDictionary.Dispatcher
{
    /// <summary>
    /// Dispatch requests to <see cref="Manager"/>
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        #region Data

        /// <summary>
        /// Another code manage manager lifetime
        /// </summary>
        private readonly bool _doNotDisposeManager;

        /// <summary>
        /// Manager to dispatch
        /// </summary>
        private readonly IManager _manager;

        /// <summary>
        /// Is object disposed
        /// </summary>
        private bool _disposed;

        #endregion

        #region .ctor

        /// <summary>
        /// Create new instance of <see cref="Dispatcher"/>
        /// <param name="manager">Manager</param>
        /// <param name="doNotDisposeManager">Another code manage manager lifetime</param>
        /// </summary>
        public Dispatcher(IManager manager, bool doNotDisposeManager = false)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _doNotDisposeManager = doNotDisposeManager;
        }

        #endregion

        public async Task<GetValueResponseDto> GetValue(GetValueRequestDto request)
        {
            ThrowIfDisposed();

            if (request == null)
                return null;

            var result = await _manager.GetValue(request.Key);
            return new GetValueResponseDto
            {
                Value = result
            };
        }

        public async Task<GetValueResponseDto[]> GetValue(GetValueRequestDto[] requests)
        {
            ThrowIfDisposed();

            if (requests == null)
                return null;

            var result = new GetValueResponseDto[requests.Length];
            for (var i = 0; i < requests.Length; i++)
            {
                result[i] = await GetValue(requests[i]);
            }
            return result;
        }

        public async Task SetValue(SetValueRequestDto request)
        {
            ThrowIfDisposed();

            if (request == null)
                return;

            await _manager.SetValue(request.Key, request.Value, request.TimeToLive);
        }

        public async Task SetValue(SetValueRequestDto[] requests)
        {
            ThrowIfDisposed();
            if (requests == null)
                return;

            foreach (var t in requests)
            {
                await SetValue(t);
            }
        }

        public async Task<DeleteKeyResponseDto> DeleteKey(DeleteKeyRequestDto request)
        {
            ThrowIfDisposed();

            if (request == null)
                return null;

            var result = await _manager.DeleteValue(request.Key);
            return new DeleteKeyResponseDto
            {
                Deleted = result
            };
        }

        public async Task<DeleteKeyResponseDto[]> DeleteKey(DeleteKeyRequestDto[] requests)
        {
            ThrowIfDisposed();
            if (requests == null)
                return null;

            var result = new DeleteKeyResponseDto[requests.Length];
            for (var i = 0; i < requests.Length; i++)
            {
                result[i] = await DeleteKey(requests[i]);
            }
            return result;
        }

        public async Task<GetKeysResponseDto> GetKeys(GetKeysRequestDto request)
        {
            ThrowIfDisposed();

            if (request == null)
                return null;

            var filteredFunc = string.IsNullOrEmpty(request.Filter) ? null : new Func<string,bool>(i => i.Contains(request.Filter));
            var result = await _manager.GetKeys(filteredFunc);

            return new GetKeysResponseDto
            {
                Keys = result
            };
        }

        public async Task<GetKeysResponseDto[]> GetKeys(GetKeysRequestDto[] requests)
        {
            ThrowIfDisposed();
            if (requests == null)
                return null;

            var result = new GetKeysResponseDto[requests.Length];
            for (var i = 0; i < requests.Length; i++)
            {
                result[i] = await GetKeys(requests[i]);
            }
            return result;
        }

        public async Task SetOptions(SetOptionsRequestDto request)
        {
            ThrowIfDisposed();
            await Task.Factory.StartNew(() =>
            {
                if (request.MaxKeyCount.HasValue)
                {
                    _manager.Options.MaxKeyCount = request.MaxKeyCount.Value;
                }

                if (request.DefaultTtl.HasValue)
                {
                    _manager.Options.DefaultTtl = request.DefaultTtl.Value;
                }
            });
        }

        public async Task SetOptions(SetOptionsRequestDto[] requests)
        {
            ThrowIfDisposed();
            if (requests == null)
                return;

            foreach (var t in requests)
            {
                await SetOptions(t);
            }
        }

        public async Task<GetOptionsResponseDto> GetOptions() {
            ThrowIfDisposed();
            return await Task.Factory.StartNew(() => new GetOptionsResponseDto
            {
                MaxKeyCount = _manager.Options.MaxKeyCount,
                DefaultTtl = _manager.Options.DefaultTtl
            });
        }

        public async Task<PacketResponseDto> GetPacketExecutionResult(PacketRequestDto request)
        {
            ThrowIfDisposed();
            return request == null ? null : new PacketResponseDto {Results = await GetPacketItemExecutionResult(request.Actions)};
        }

        public async Task<PacketResponseItemDto> GetPacketItemExecutionResult(PacketRequestItemDto request)
        {
            ThrowIfDisposed();

            if (request == null)
                return null;

            var getValueResponses = await GetValue(request.GetValueRequests);
            await SetValue(request.SetValueRequests);
            var getKeysResponses = await GetKeys(request.GetKeysRequests);
            var deleteKeyResponses = await DeleteKey(request.DeleteKeyRequests);
            await SetOptions(request.SetOptionsRequests);

            return new PacketResponseItemDto
            {
                GetValueResponses = getValueResponses,
                GetKeysResponses = getKeysResponses,
                DeleteKeyResponses = deleteKeyResponses
            };
        }

        public async Task<PacketResponseItemDto[]> GetPacketItemExecutionResult(PacketRequestItemDto[] requests)
        {
            ThrowIfDisposed();
            if (requests == null)
                return null;

            var result = new PacketResponseItemDto[requests.Length];
            for (var i = 0; i < requests.Length; i++)
            {
                result[i] = await GetPacketItemExecutionResult(requests[i]);
            }
            return result;
        }

        #region IDisposable

        /// <summary>
        /// Dispose called by IDispatcher.Dispose()
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Dispose items
        /// </summary>
        /// <param name="disposing">Is method called by IDispatcher.Dispose()</param>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (!_doNotDisposeManager)
                _manager.Dispose();
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Dispatcher()
        {
            Dispose(false);
        }

        /// <summary>
        /// Throw an exception <see cref="ObjectDisposedException"/> if this object has been already disposed
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Dispatcher));
        }

        #endregion
    }
}
