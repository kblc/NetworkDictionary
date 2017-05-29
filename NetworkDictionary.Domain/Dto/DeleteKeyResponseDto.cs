using System;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to <see cref="DeleteKeyRequestDto"/>
    /// </summary>
    [DataContract]
    public class DeleteKeyResponseDto : IEquatable<DeleteKeyResponseDto>
    {
        /// <summary>
        /// Is value has been deleted
        /// </summary>
        [DataMember(Name = "deleted")]
        public bool Deleted { get; set; }


        public bool Equals(DeleteKeyResponseDto other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Deleted == other.Deleted;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((DeleteKeyResponseDto) obj);
        }

        public override int GetHashCode()
        {
            return Deleted.GetHashCode();
        }
    }
}
