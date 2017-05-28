using System;

namespace NetworkDictionary.Manager
{
    /// <summary>
    /// Options for <see cref="Manager"/>
    /// </summary>
    public class ManagerOptions
    {
        /// <summary>
        /// Max key count for dictionary
        /// </summary>
        private int _maxKeyCount = 100000;

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
    }
}
