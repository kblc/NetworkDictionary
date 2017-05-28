using System;

namespace NetworkDictionary.Manager.Exceptions
{
    /// <summary>
    /// Exception to use in <see cref="Manager"/>
    /// </summary>
    public class ManagerException : Exception
    {
        /// <summary>
        /// Create instance
        /// </summary>
        public ManagerException() { }

        /// <summary>
        /// Create instance
        /// </summary>
        /// <param name="message">Exception message</param>
        public ManagerException(string message) : base(message) { }

        /// <summary>
        /// Create instance
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
