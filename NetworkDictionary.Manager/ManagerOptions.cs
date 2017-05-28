using System;
using System.Diagnostics.CodeAnalysis;

namespace NetworkDictionary.Manager
{
    /// <summary>
    /// Options for <see cref="Manager"/>
    /// </summary>
    public class ManagerOptions
    {
        private static readonly TimeSpan _minClearPeriod = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan _minFrequinceDecreasePeriod = TimeSpan.FromSeconds(0.5);

        /// <summary>
        /// Max key count for dictionary
        /// </summary>
        private int _maxKeyCount;

        /// <summary>
        /// Create new instance
        /// </summary>
        /// <param name="clearExpiredValuesPeriod">Period to clear data with expired period</param>
        /// <param name="decreaseValueFrequincePeriod">Period to clear data with expired period</param>
        /// <param name="defaultTtl">Default TTL (Time to live) for key values</param>
        /// <param name="maxKeyCount">Max key count for dictionary</param>
        public ManagerOptions(TimeSpan clearExpiredValuesPeriod, TimeSpan decreaseValueFrequincePeriod, TimeSpan defaultTtl, int maxKeyCount)
        {
            if (clearExpiredValuesPeriod < _minClearPeriod)
                throw new ArgumentOutOfRangeException(nameof(clearExpiredValuesPeriod), $"Value should be more or equals to {_minClearPeriod}");

            if (decreaseValueFrequincePeriod < _minFrequinceDecreasePeriod)
                throw new ArgumentOutOfRangeException(nameof(decreaseValueFrequincePeriod), $"Value should be more or equals to {_minFrequinceDecreasePeriod}");

            ClearExpiredValuesPeriod = clearExpiredValuesPeriod;
            DecreaseValueFrequincePeriod = decreaseValueFrequincePeriod;
            DefaultTtl = defaultTtl;
            MaxKeyCount = maxKeyCount;
        }

        /// <summary>
        /// Default TTL (Time to live) for key values
        /// </summary>
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public TimeSpan DefaultTtl { get; set; }

        /// <summary>
        /// Max key count for dictionary
        /// </summary>
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public int MaxKeyCount
        {
            get => _maxKeyCount;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(MaxKeyCount));
                _maxKeyCount = value;
            }
        }

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
