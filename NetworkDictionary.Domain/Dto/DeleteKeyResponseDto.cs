using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to <see cref="DeleteKeyRequestDto"/>
    /// </summary>
    [DataContract]
    public class DeleteKeyResponseDto
    {
        /// <summary>
        /// Is value has been deleted
        /// </summary>
        [DataMember(Name = "deleted")]
        public bool Deleted { get; set; }
    }
}
