using System;
using System.Collections.Generic;
using System.Text;

namespace RS.MDM.Configuration
{
    /// <summary>
    /// Provides functionality to load configuration into an object using xml
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Loads the configuration configuration
        /// </summary>
        /// <param name="xmlConfiguration">Indicates the configuration of the object in xml format</param>
        void LoadConfiguration(string xmlConfiguration);
    }
}
