using System;

namespace MDM.Services.ServiceProxies
{
    using MDM.BusinessObjects;
    using MDM.Services.ConfigurationServiceClient;

    /// <summary>
    /// Represents class for configuration service proxy
    /// </summary>
    public class ConfigurationServiceProxy : ConfigurationServiceClient, MDM.WCFServiceInterfaces.IConfigurationService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ConfigurationServiceProxy()
        { 
        
        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public ConfigurationServiceProxy(String endpointConfigurationName) 
            : base(endpointConfigurationName) 
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address.
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public ConfigurationServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) 
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

    }
}
