using System;
using System.Threading.Tasks;
using NetworkDictionary.Domain.Dto;

namespace NetworkDictionary.Dispatcher.Interfaces
{
    /// <summary>
    /// Dispatcher interface
    /// </summary>
    public interface IDispatcher : IDisposable
    {
        /// <summary>
        /// Delete key by request
        /// </summary>
        /// <param name="request">Delete key request</param>
        /// <returns>Delete key response</returns>
        Task<DeleteKeyResponseDto> DeleteKey(DeleteKeyRequestDto request);

        /// <summary>
        /// Delete key by request array
        /// </summary>
        /// <param name="requests">Delete key request array</param>
        /// <returns>Delete key response array</returns>
        Task<DeleteKeyResponseDto[]> DeleteKey(DeleteKeyRequestDto[] requests);

        /// <summary>
        /// Get keys by request
        /// </summary>
        /// <param name="request">Get keys request</param>
        /// <returns>Get key response</returns>
        Task<GetKeysResponseDto> GetKeys(GetKeysRequestDto request);

        /// <summary>
        /// Get keys by request array
        /// </summary>
        /// <param name="requests">Get keys request array</param>
        /// <returns>Get key response array</returns>
        Task<GetKeysResponseDto[]> GetKeys(GetKeysRequestDto[] requests);

        /// <summary>
        /// Get options by request
        /// </summary>
        /// <returns>Get options response</returns>
        Task<GetOptionsResponseDto> GetOptions();

        /// <summary>
        /// Get packet execution result by request
        /// </summary>
        /// <param name="request">Packet execution result request</param>
        /// <returns>Packet execution result response</returns>
        Task<PacketResponseDto> GetPacketExecutionResult(PacketRequestDto request);

        /// <summary>
        /// Get packet execution item result for by request
        /// </summary>
        /// <param name="request">Packet execution item result request</param>
        /// <returns>Packet execution item result response</returns>
        Task<PacketResponseItemDto> GetPacketItemExecutionResult(PacketRequestItemDto request);

        /// <summary>
        /// Get packet execution item result for by request array
        /// </summary>
        /// <param name="requests">Packet execution item result request array</param>
        /// <returns>Packet execution item result response array</returns>
        Task<PacketResponseItemDto[]> GetPacketItemExecutionResult(PacketRequestItemDto[] requests);

        /// <summary>
        /// Get value by request
        /// </summary>
        /// <param name="request">Get value request</param>
        /// <returns>Get value response</returns>
        Task<GetValueResponseDto> GetValue(GetValueRequestDto request);

        /// <summary>
        /// Get value by request array
        /// </summary>
        /// <param name="requests">Get value request array</param>
        /// <returns>Get value response array</returns>
        Task<GetValueResponseDto[]> GetValue(GetValueRequestDto[] requests);

        /// <summary>
        /// Set options by request
        /// </summary>
        /// <param name="request">Set options request</param>
        Task SetOptions(SetOptionsRequestDto request);

        /// <summary>
        /// Set options by request array
        /// </summary>
        /// <param name="requests">Set options request array</param>
        Task SetOptions(SetOptionsRequestDto[] requests);

        /// <summary>
        /// Set value by request
        /// </summary>
        /// <param name="request">Set value request</param>
        Task SetValue(SetValueRequestDto request);

        /// <summary>
        /// Set value by request array
        /// </summary>
        /// <param name="requests">Set value request array</param>
        Task SetValue(SetValueRequestDto[] requests);
    }
}