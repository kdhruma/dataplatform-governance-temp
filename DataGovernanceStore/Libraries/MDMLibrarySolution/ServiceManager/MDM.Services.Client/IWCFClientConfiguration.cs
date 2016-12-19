using System;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml;
using System.Security.Principal;

namespace MDM.Services
{
    using MDM.Core;

    /// <summary>
    /// Defines client configuration properties for MDM WCF Services
    /// </summary>
    public interface IWCFClientConfiguration
    {
        /// <summary>
        /// Property denoting EndPoint Configuration Name
        /// </summary>
        String EndPointConfigurationName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting EndPoint Address
        /// </summary>
        EndpointAddress EndpointAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting authentication type
        /// </summary>
        AuthenticationType AuthenticationType
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting user userIdentity
        /// </summary>
        IIdentity UserIdentity
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the Current Username 
        /// </summary>
        String UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the User's Password
        /// </summary>
        String Password
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting if delegation is enabled..
        /// </summary>
        Boolean IsDelegationEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting WCF BindingType
        /// </summary>
        WCFBindingType WCFBindingType
        {
            get;
            set;
        }

        /// <summary>
        /// Xml representation of the WCF Client Configuration
        /// </summary>
        /// <returns>Xml representation of the WCF Client Configuration</returns>
        String ToXml();
    }
}
