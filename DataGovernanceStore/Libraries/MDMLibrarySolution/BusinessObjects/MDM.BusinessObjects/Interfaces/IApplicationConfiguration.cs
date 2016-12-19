using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Interface for application configuration
    /// </summary>
    public interface IApplicationConfiguration
    {
        /// <summary>
        /// Gets the list of configurations from DB
        /// </summary>
        void GetConfigurations();

        /// <summary>
        /// Gets the list of configuration objects
        /// </summary>
        /// <returns>List of configuration objects</returns>
        List<IConfigurationObject> GetItems();
    }
}
