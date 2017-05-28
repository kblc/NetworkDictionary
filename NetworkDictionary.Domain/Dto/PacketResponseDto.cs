using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Packet response to <see cref="PacketRequestDto"/>
    /// </summary>
    [DataContract]
    public class PacketResponseDto
    {
        /// <summary>
        /// Results
        /// </summary>
        [DataMember(Name = "results")]
        public PacketResponseItemDto[] Results { get; set; }
    }
}
