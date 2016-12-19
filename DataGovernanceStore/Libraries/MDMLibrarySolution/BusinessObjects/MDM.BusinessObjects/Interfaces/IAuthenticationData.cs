using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes interface that contains data for user authentication
    /// </summary>
    public interface IAuthenticationData
    {
        /// <summary>
        /// Represents user name
        /// </summary>
        String UserName { get; set; }

        /// <summary>
        /// Represents user password
        /// </summary>
        String Password { get; set; }

        /// <summary>
        /// Represents user ip
        /// </summary>
        String Ip { get; set; }

        /// <summary>
        /// Represents timestamp
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// Represents scheme
        /// </summary>
        String Scheme { get; set; }

        /// <summary>
        /// Represents host
        /// </summary>
        String Host { get; set; }

        /// <summary>
        /// Represents port
        /// </summary>
        Int32 Port { get; set; }

        /// <summary>
        /// Represents path and query
        /// </summary>
        String PathAndQuery { get; set; }

    }
}
