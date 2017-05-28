using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Packet response item
    /// </summary>
    [DataContract]
    public class PacketResponseItemDto
    {
        /// <summary>
        /// Get key responses
        /// </summary>
        [DataMember(Name = "getValue")]
        public GetValueResponseDto[] GetValueResponses { get; set; }

        /// <summary>
        /// Get keys responses
        /// </summary>
        [DataMember(Name = "getKeys")]
        public GetKeysResponseDto[] GetKeysResponses { get; set; }

        /// <summary>
        /// Delete key responses
        /// </summary>
        [DataMember(Name = "deleteKey")]
        public DeleteKeyResponseDto[] DeleteKeyResponses { get; set; }
    }
}