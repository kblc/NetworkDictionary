using System;
using System.Runtime.Serialization;
using NetworkDictionary.Domain.Extensions;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Packet response item
    /// </summary>
    [DataContract]
    public class PacketResponseItemDto : IEquatable<PacketResponseItemDto>
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

        public bool Equals(PacketResponseItemDto other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return EnumerableExtensions.IsArraysEquals(GetValueResponses, other.GetValueResponses) 
                && EnumerableExtensions.IsArraysEquals(GetKeysResponses, other.GetKeysResponses) 
                && EnumerableExtensions.IsArraysEquals(DeleteKeyResponses, other.DeleteKeyResponses);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((PacketResponseItemDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (GetValueResponses != null ? GetValueResponses.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GetKeysResponses != null ? GetKeysResponses.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DeleteKeyResponses != null ? DeleteKeyResponses.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}