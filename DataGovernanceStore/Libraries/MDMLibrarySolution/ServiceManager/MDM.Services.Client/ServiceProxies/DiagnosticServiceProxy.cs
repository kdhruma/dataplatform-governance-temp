using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Services.ServiceProxies
{
    using MDM.Services.DiagnosticServiceClient;

    /// <summary>
    /// 
    /// </summary>
    public class DiagnosticServiceProxy : DiagnosticServiceClient, MDM.WCFServiceInterfaces.IDiagnosticService
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DiagnosticServiceProxy()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        public DiagnosticServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>
        public DiagnosticServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion
    }
}
