using System;
using NetworkDictionary.Manager.Interfaces;

namespace NetworkDictionary.Service.Options
{
    /// <summary>
    /// Stored configuration for <see cref="IManager"/>
    /// </summary>
    public class ManagerConfiguration
    {
        /// <summary>
        /// Default TTL (Time to live) for key values
        /// </summary>
        public TimeSpan DefaultTtl { get; set; }

        /// <summary>
        /// Max key count for dictionary
        /// </summary>
        public int MaxKeyCount { get; set; }

        /// <summary>
        /// Period to clear data with expired period
        /// </summary>
        public TimeSpan ClearExpiredValuesPeriod { get; }

        /// <summary>
        /// Period to clear data with expired period
        /// </summary>
        public TimeSpan DecreaseValueFrequincePeriod { get; }
    }
}
