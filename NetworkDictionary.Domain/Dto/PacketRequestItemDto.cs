using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace NetworkDictionary.Domain.Dto
{
    /// <summary>
    /// Packet request item
    /// </summary>
    [DataContract]
    public class PacketRequestItemDto : IValidatableObject
    {
        /// <summary>
        /// Get key requests
        /// </summary>
        [DataMember(Name = "getKey")]
        public GetKeysRequestDto[] GetKeyRequests { get; set; }

        /// <summary>
        /// Set key requests
        /// </summary>
        [DataMember(Name = "setKey")]
        public SetValueRequestDto[] SetValueRequests { get; set; }

        /// <summary>
        /// Get keys requests
        /// </summary>
        [DataMember(Name = "getKeys")]
        public GetKeysRequestDto[] GetKeysRequests { get; set; }

        /// <summary>
        /// Delete key requests
        /// </summary>
        [DataMember(Name = "deleteKey")]
        public DeleteKeyRequestDto[] DeleteKeyRequests { get; set; }

        /// <summary>
        /// Set option requests
        /// </summary>
        [DataMember(Name = "setOptions")]
        public SetOptionsRequestDto[] SetOptionsRequests { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var isNotAnyRequest = IsEmpty(GetKeyRequests)
                                  && IsEmpty(SetValueRequests)
                                  && IsEmpty(GetKeysRequests)
                                  && IsEmpty(DeleteKeyRequests)
                                  && IsEmpty(SetOptionsRequests);

            if (isNotAnyRequest)
                yield return new ValidationResult("No one request exists");
        }

        /// <summary>
        /// Check if collection <paramref name="items"/> is empty
        /// </summary>
        /// <typeparam name="T">Collection's element type</typeparam>
        /// <param name="items">Collection</param>
        /// <returns>True if collection is empty, otherwise false</returns>
        private static bool IsEmpty<T>(IEnumerable<T> items) { return items == null || !items.Any(); }
    }
}