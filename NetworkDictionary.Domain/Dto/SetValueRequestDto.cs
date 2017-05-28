using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Request to set value by key
    /// </summary>
    [DataContract]
    public class SetValueRequestDto
    {
        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "key"), Required(AllowEmptyStrings = false), MaxLength(200)]
        public string Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [DataMember(Name = "value"), MaxLength(1024*1024/2)]
        public string Value { get; set; }

        /// <summary>
        /// Time to live
        /// </summary>
        [DataMember(Name = "ttl")]
        public TimeSpan? TimeToLive { get; set; }
    }
}
