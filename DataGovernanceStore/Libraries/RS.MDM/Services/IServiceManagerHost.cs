using System;
using System.Collections.Generic;
using System.Text;

namespace RS.MDM.Services
{
    /// <summary>
    /// Defines the functionality of a Service Manager Host
    /// </summary>
    public interface IServiceManagerHost
    {
        /// <summary>
        /// Gets a Service Manager for a given Service Manager Type
        /// </summary>
        ServiceManagerBase GetServiceManager(System.Type serviceManagerType);

    }
}
