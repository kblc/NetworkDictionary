using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Packet response for execution
    /// </summary>
    [DataContract]
    public class PacketRequestDto
    {
        /// <summary>
        /// Actions
        /// </summary>
        [DataMember(Name = "actions"), Required, MinLength(1)]
        public PacketRequestItemDto[] Actions { get; set; }
    }
}
