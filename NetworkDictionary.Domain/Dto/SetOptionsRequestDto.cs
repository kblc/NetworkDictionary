using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Request to update options
    /// </summary>
    [DataContract]
    public class SetOptionsRequestDto : IValidatableObject
    {
        /// <summary>
        /// Default TTL
        /// </summary>
        [DataMember(Name = "defaultTtl")]
        public TimeSpan? DefaultTtl { get; set; }

        /// <summary>
        /// Max key count for dictionary
        /// </summary>
        [DataMember(Name = "maxKeyCount")]
        public int? MaxKeyCount { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaxKeyCount < 1)
            {
                yield return new ValidationResult("Value must be more or equal to 1", new [] { nameof(MaxKeyCount) });
            }
        }
    }
}
