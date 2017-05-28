using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to <see cref="GetKeysRequestDto"/>
    /// </summary>
    [DataContract]
    public class GetKeysResponseDto
    {
        /// <summary>
        /// Keys
        /// </summary>
        [DataMember(Name = "keys")]
        public string[] Keys { get; set; }
    }
}
