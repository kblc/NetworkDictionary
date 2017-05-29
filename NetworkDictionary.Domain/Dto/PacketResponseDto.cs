using System;
using System.Runtime.Serialization;
using NetworkDictionary.Domain.Extensions;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Packet response to <see cref="PacketRequestDto"/>
    /// </summary>
    [DataContract]
    public class PacketResponseDto : IEquatable<PacketResponseDto>
    {
        /// <summary>
        /// Results
        /// </summary>
        [DataMember(Name = "results")]
        public PacketResponseItemDto[] Results { get; set; }

        public bool Equals(PacketResponseDto other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return EnumerableExtensions.IsArraysEquals(Results, other.Results);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((PacketResponseDto) obj);
        }

        public override int GetHashCode()
        {
            return (Results != null ? Results.GetHashCode() : 0);
        }
    }
}
