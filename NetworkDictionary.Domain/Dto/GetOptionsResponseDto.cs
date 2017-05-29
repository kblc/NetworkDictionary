using System;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to request for options
    /// </summary>
    [DataContract]
    public class GetOptionsResponseDto : IEquatable<GetOptionsResponseDto>
    {
        /// <summary>
        /// Default TTL
        /// </summary>
        [DataMember(Name = "defaultTtl")]
        public TimeSpan DefaultTtl { get; set; }

        /// <summary>
        /// Max key count for dictionary
        /// </summary>
        [DataMember(Name = "maxKeyCount")]
        public int MaxKeyCount { get; set; }

        public bool Equals(GetOptionsResponseDto other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return DefaultTtl.Equals(other.DefaultTtl) && MaxKeyCount == other.MaxKeyCount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((GetOptionsResponseDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (DefaultTtl.GetHashCode() * 397) ^ MaxKeyCount;
            }
        }
    }
}
