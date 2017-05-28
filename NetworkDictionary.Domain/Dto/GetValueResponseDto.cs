using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to <see cref="GetValueRequestDto"/>
    /// </summary>
    [DataContract]
    public class GetValueResponseDto
    {
        /// <summary>
        /// Value
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
