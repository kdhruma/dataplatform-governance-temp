using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get service status.
    /// </summary>
    public interface IServiceStatus
    {
        #region Properties

        /// <summary>
        /// Property denoting Name of the Server
        /// </summary>
        String Server { get; set; }

        /// <summary>
        /// Property denoting Type of service
        /// </summary>
        MDMServiceType Service { get; set; }

        /// <summary>
        /// Property denoting subType of Service
        /// </summary>
        MDMServiceSubType ServiceSubType { get; set; }

        /// <summary>
        /// Property denoting status of the server in form of XML
        /// </summary>
        String ServiceStatusXML { get; set; }

        /// <summary>
        /// Property denoting Modified Datetime of the Server
        /// </summary>
        DateTime ModifiedDateTime { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// XML representation of ServiceStatus
        /// </summary>
        /// <returns>ServiceStatus XML</returns>
        String ToXml();

        #endregion
    }
}
