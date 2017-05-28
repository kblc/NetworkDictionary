using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Request to delete key
    /// </summary>
    [DataContract]
    public class DeleteKeyRequestDto
    {
        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "key"), Required(AllowEmptyStrings = false), MaxLength(200)]
        public string Key { get; set; }
    }
}
