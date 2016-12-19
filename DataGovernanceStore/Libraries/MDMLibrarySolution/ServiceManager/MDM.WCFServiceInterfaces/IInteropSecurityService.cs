using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects;

namespace MDM.WCFServiceInterfaces
{
    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IInteropSecurityService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAuthenticationToken(String userLoginName, String password);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetUserPreference(String token, String userLoginName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetUserRoles(String token, int LoginId, String userLoginName);
    }
}
