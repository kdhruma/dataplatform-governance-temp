using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get connection properties.
    /// </summary>
    public interface IConnectionProperties : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting connection attributes like application name, pool size etc.
        /// </summary>
        string ConnectionAttribtues { get; set; }

        /// <summary>
        /// Property denoting database name
        /// </summary>
        string DatabaseName { get; set; }
        
        /// <summary>
        /// Property denoting whether it is windows authentication
        /// </summary>
        bool IsWindowsAuthentication { get; set; }
        
        /// <summary>
        /// Property denoting password
        /// </summary>
        string Password { get; set; }
        
        /// <summary>
        /// Property denoting server id
        /// </summary>
        string ServerId { get; set; }
        
        /// <summary>
        /// Property denoting user name
        /// </summary>
        new string UserName { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get connection string from connection properties.
        /// </summary>
        /// <returns>Connection string</returns>
        string GetConnectionString();

        #endregion Methods
    }
}
