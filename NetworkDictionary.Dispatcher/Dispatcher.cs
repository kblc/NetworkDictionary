using System;
using System.Linq;
using System.Threading.Tasks;
using NetworkDictionary.Domain.Dto;
using NetworkDictionary.Manager.Interfaces;

namespace NetworkDictionary.Dispatcher
{
    /// <summary>
    /// Dispatch requests to <see cref="Manager"/>
    /// </summary>
    public class Dispatcher : IDisposable
    {
        /// <summary>
        /// Another code manage manager lifetime
        /// </summary>
        private readonly bool _ownManagerFromOutside;

        /// <summary>
        /// Manager to dispatch
        /// </summary>
        private readonly IManager _manager;

        /// <summary>
        /// Is object disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Create new instance of <see cref="Dispatcher"/>
        /// <param name="manager">Manager</param>
        /// <param name="ownManagerFromOutside">Another code manage manager lifetime</param>
        /// </summary>
        public Dispatcher(IManager manager, bool ownManagerFromOutside = false)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _ownManagerFromOutside = ownManagerFromOutside;
        }

        public async Task<GetValueResponseDto> GetValue(GetValueRequestDto request)
        {
            ThrowIfDisposed();
            var result = await _manager.GetValue(request.Key);
            return new GetValueResponseDto
            {
                Value = result
            };
        }

        public async Task SetValue(SetValueRequestDto request)
        {
            ThrowIfDisposed();
            await _manager.SetValue(request.Key, request.Value, request.TimeToLive);
        }

        public async Task<DeleteKeyResponseDto> DeleteKey(DeleteKeyRequestDto request)
        {
            ThrowIfDisposed();
            var result = await _manager.DeleteValue(request.Key);
            return new DeleteKeyResponseDto
            {
                Deleted = result
            };
        }

        public async Task<GetKeysResponseDto> GetKeys(GetKeysRequestDto request)
        {
            ThrowIfDisposed();
            var result = await _manager.GetKeys();
            var filteredResult = request.Filter == null ? result : result.Where(i => i.Contains(request.Filter)).ToArray();

            return new GetKeysResponseDto
            {
                Keys = filteredResult
            };
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

        public async Task<GetOptionsResponseDto> GetOptions() {
            ThrowIfDisposed();
            return await Task.Factory.StartNew(() =>
            {
                return new GetOptionsResponseDto
                {
                    MaxKeyCount = _manager.Options.MaxKeyCount,
                    DefaultTtl = _manager.Options.DefaultTtl
                };
            });
        }

        public async Task<PacketResponseDto> GetPacketExecutionResult(PacketRequestDto request)
        {
            ThrowIfDisposed();
            var result = new PacketResponseDto
            {
                Results = new PacketResponseItemDto[request.Actions.Length]
            };

            for (var i = 0; i < request.Actions.Length; i++)
            {
                result.Results[i] = await GetPacketItemExecutionResult(request.Actions[i]);
            }

            return result;
        }

        private async Task<PacketResponseItemDto> GetPacketItemExecutionResult(PacketRequestItemDto request)
        {
            var result = new PacketResponseItemDto
            {
                DeleteKeyResponses = request.DeleteKeyRequests == null ? null : new DeleteKeyResponseDto[request.DeleteKeyRequests.Length],
                GetValueResponses = request.GetValueRequests == null ? null : new GetValueResponseDto[request.GetValueRequests.Length],
                GetKeysResponses = request.GetKeysRequests == null ? null : new GetKeysResponseDto[request.GetKeysRequests.Length],
            };

            if (request.GetValueRequests != null)
            {
                for (var i = 0; i < request.GetValueRequests.Length; i++)
                {
                    result.GetValueResponses[i] = await GetValue(request.GetValueRequests[i]);
                }
            }

            if (request.SetValueRequests != null)
            {
                for (var i = 0; i < request.SetValueRequests.Length; i++)
                {
                    await SetValue(request.SetValueRequests[i]);
                }
            }

            if (request.SetValueRequests != null)
            {
                for (var i = 0; i < request.SetValueRequests.Length; i++)
                {
                    await SetValue(request.SetValueRequests[i]);
                }
            }

            if (request.GetKeysRequests != null)
            {
                for (var i = 0; i < request.GetKeysRequests.Length; i++)
                {
                    result.GetKeysResponses[i] = await GetKeys(request.GetKeysRequests[i]);
                }
            }

            if (request.DeleteKeyRequests != null)
            {
                for (var i = 0; i < request.DeleteKeyRequests.Length; i++)
                {
                    result.DeleteKeyResponses[i] = await DeleteKey(request.DeleteKeyRequests[i]);
                }
            }

            if (request.SetOptionsRequests != null)
            {
                for (var i = 0; i < request.SetOptionsRequests.Length; i++)
                {
                    await SetOptions(request.SetOptionsRequests[i]);
                }
            }

            return result;
        }

        #region IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (!_ownManagerFromOutside)
                _manager.Dispose();
        }

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
