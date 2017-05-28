using System;

namespace NetworkDictionary.Manager.Models
{
    /// <summary>
    /// Dictionary value
    /// </summary>
    internal class DictionaryValue
    {
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Expire date
        /// </summary>
        public DateTime Expired { get; set; }

        /// <summary>
        /// Request count for current value
        /// </summary>
        public int RequestCount { get; set; }
    }
}
