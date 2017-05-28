using System;
using NetworkDictionary.Manager.Models;

namespace NetworkDictionary.Manager.Extensions
{
    /// <summary>
    /// Extensions for <see cref="DictionaryValue"/>
    /// </summary>
    internal static class DictionaryValueExtensions
    {
        /// <summary>
        /// Increment request count
        /// </summary>
        /// <param name="dictionaryValue">Dictionary value</param>
        public static void IncrementRequestCount(this DictionaryValue dictionaryValue)
        {
            if (dictionaryValue == null)
            {
                throw new ArgumentNullException(nameof(dictionaryValue));
            }
            if (dictionaryValue.RequestCount == int.MaxValue)
                return;

            dictionaryValue.RequestCount++;
        }
    }
}
