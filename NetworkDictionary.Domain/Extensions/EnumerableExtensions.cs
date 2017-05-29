using System.Linq;

namespace NetworkDictionary.Domain.Extensions
{
    /// <summary>
    /// Extension for any <see cref="System.Collections.IEnumerable"/>
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Check if array1 equals array2
        /// </summary>
        /// <typeparam name="T">Array element type</typeparam>
        /// <param name="array1">First array</param>
        /// <param name="array2">Second array</param>
        /// <returns>True is items are equals, otherwise false</returns>
        public static bool IsArraysEquals<T>(T[] array1, T[] array2)
        {
            if (array1 == array2)
                return true;

            if (array1 == null || array2 == null)
                return false;

            return array1.Select((item, index) => Equals(array2[index], item)).All(i => i);
        }
    }
}
