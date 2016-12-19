using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Interface for configuration object
    /// </summary>
    public interface IConfigurationObject
    {
        /// <summary>
        /// Gets the object that is set to be configured
        /// </summary>
        /// <returns>Object that is set to be configured as IObject</returns>
        IObject GetObject();

        /// <summary>
        /// Gets the current MDM application context
        /// </summary>
        /// <returns>Current MDM application context as IApplicationContext</returns>
        IApplicationContext GetApplicationContext();
    }
}
