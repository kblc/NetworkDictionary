using System;

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
        /// <param name="clearPeriod">Period to clear data with expired period</param>
        /// <param name="frequinceDecreasePeriod">Period to clear data with expired period</param>
        /// <param name="defaultTtl">Default TTL (Time to live) for key values</param>
        /// <param name="maxKeyCount">Max key count for dictionary</param>
        public ManagerOptions(TimeSpan clearPeriod, TimeSpan frequinceDecreasePeriod, TimeSpan defaultTtl, int maxKeyCount)
        {
            if (clearPeriod < _minClearPeriod)
                throw new ArgumentOutOfRangeException(nameof(clearPeriod), $"Value should be more or equals to {_minClearPeriod}");

            if (frequinceDecreasePeriod < _minFrequinceDecreasePeriod)
                throw new ArgumentOutOfRangeException(nameof(frequinceDecreasePeriod), $"Value should be more or equals to {_minFrequinceDecreasePeriod}");

            ClearPeriod = clearPeriod;
            FrequinceDecreasePeriod = frequinceDecreasePeriod;
            DefaultTtl = defaultTtl;
            MaxKeyCount = maxKeyCount;
        }

        /// <summary>
        /// Default TTL (Time to live) for key values
        /// </summary>
        public TimeSpan DefaultTtl { get; set; }

        /// <summary>
        /// Max key count for dictionary
        /// </summary>
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
        public TimeSpan ClearPeriod { get; }

        /// <summary>
        /// Period to clear data with expired period
        /// </summary>
        public TimeSpan FrequinceDecreasePeriod { get; }
    }
}
