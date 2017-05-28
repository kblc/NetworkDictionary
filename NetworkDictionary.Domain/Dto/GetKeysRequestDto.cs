using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Request to get keys from dictionary
    /// </summary>
    [DataContract]
    public class GetKeysRequestDto
    {
        /// <summary>
        /// Filter for keys
        /// </summary>
        [DataMember(Name = "filter"), MaxLength(200)]
        public string Filter { get; set; }
    }
}
