using System;
using System.Threading.Tasks;

namespace NetworkDictionary.Manager.Interfaces
{
    /// <summary>
    /// Cache dictionary manager interface
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Add value to dictionary
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <param name="value">Value</param>
        /// <param name="ttl">Time to live</param>
        Task SetValue(string key, string value, TimeSpan? ttl = null);

        /// <summary>
        /// Get value from dictionary
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <returns>Value (null if key does not exist)</returns>
        Task<string> GetValue(string key);

        /// <summary>
        /// Delete key and value from dictionary
        /// </summary>
        /// <param name="key">>Dictionary key</param>
        /// <returns>True if key was existed otherwise False</returns>
        Task<bool> DeleteValue(string key);

        /// <summary>
        /// Get all dictionary keys
        /// </summary>
        /// <returns>Existed dictionary keys</returns>
        Task<string[]> GetKeys();
    }
}
