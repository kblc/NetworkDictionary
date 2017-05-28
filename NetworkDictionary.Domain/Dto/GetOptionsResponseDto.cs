using System;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Response to request for options
    /// </summary>
    [DataContract]
    public class GetOptionsResponseDto
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
    }
}
