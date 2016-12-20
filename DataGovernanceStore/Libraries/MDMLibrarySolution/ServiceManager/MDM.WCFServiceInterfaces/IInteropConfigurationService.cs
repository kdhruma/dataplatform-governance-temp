using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects;
using MDM.Core;

namespace MDM.WCFServiceInterfaces
{
    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IInteropConfigurationService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAllAppConfig();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAppConfig(String configKey);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetLocaleMessage(LocaleEnum locale, String messageCode);
    }
}
