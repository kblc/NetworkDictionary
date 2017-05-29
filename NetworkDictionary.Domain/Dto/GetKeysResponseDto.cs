using System;
using System.Runtime.Serialization;
using NetworkDictionary.Domain.Extensions;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to <see cref="GetKeysRequestDto"/>
    /// </summary>
    [DataContract]
    public class GetKeysResponseDto : IEquatable<GetKeysResponseDto>
    {
        /// <summary>
        /// Keys
        /// </summary>
        [DataMember(Name = "keys")]
        public string[] Keys { get; set; }

        public bool Equals(GetKeysResponseDto other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return EnumerableExtensions.IsArraysEquals(Keys, other.Keys);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((GetKeysResponseDto) obj);
        }

        public override int GetHashCode()
        {
            return (Keys != null ? Keys.GetHashCode() : 0);
        }
    }
}
