using System;
using System.ServiceModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.Interfaces;

    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IAuthenticationService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean AuthenticateUser(String userLoginName, String password);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityPrincipal GetSecurityPrincipal(String userLoginName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessUserCredentialRequest(UserCredentialRequestContext userRequestContext, CallerContext callerContext);
    }
}
