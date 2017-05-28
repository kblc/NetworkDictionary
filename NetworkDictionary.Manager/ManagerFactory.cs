using NetworkDictionary.Manager.Interfaces;

namespace NetworkDictionary.Manager
{
    /// <summary>
    /// Factory for <see cref="IManager"/>
    /// </summary>
    public static class ManagerFactory
    {
        /// <summary>
        /// Create new manager
        /// </summary>
        /// <param name="options">Manager options</param>
        /// <returns></returns>
        public static IManager CreateManager(ManagerOptions options)
        {
            return new Manager(options);
        }
    }
}
