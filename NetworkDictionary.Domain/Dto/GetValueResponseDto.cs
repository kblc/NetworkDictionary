using System;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to <see cref="GetValueRequestDto"/>
    /// </summary>
    [DataContract]
    public class GetValueResponseDto : IEquatable<GetValueResponseDto>
    {
        /// <summary>
        /// Value
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }

        public bool Equals(GetValueResponseDto other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((GetValueResponseDto) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
