using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace MDM.WCFServices
{
    using MDM.Core.Exceptions;
    using MDM.WCFServiceInterfaces;
    using MDM.AdminManager.Business;
    using MDM.ExceptionManager;
    using MDM.BusinessObjects;
    using MDM.SecurityManager.Business;

    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuthenticationService : IAuthenticationService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="userLoginName">Indicates the User Login Name</param>
        /// <param name="password">Indicates the User Password</param>
        /// <returns>If it is true means user got authenticated</returns>
        public Boolean AuthenticateUser(String userLoginName, String password)
        {
            bool returnValue = false;
            try
            {
                UserPrincipalBL userPrincipal = new UserPrincipalBL();
                returnValue = userPrincipal.AuthenticateUser(userLoginName, password);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return returnValue;
        }

        /// <summary>
        /// Constructs the SecurityPrincipal based on userName
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>returns SecurityPrincipal</returns>
        public SecurityPrincipal GetSecurityPrincipal(string userName)
        {
            SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
            SecurityPrincipal currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, Core.MDMCenterSystem.Web);

            return currentUserSecurityPrincipal;
        }

        /// <summary>
        //  Processes the user credential request by sending email to the registered email id based on the requested context
        /// </summary>
        /// <param name="userRequestContext">Indicates context for the user credential request</param>
        /// <param name="callerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing detail information of process user credential request</returns>
        public OperationResult ProcessUserCredentialRequest(UserCredentialRequestContext userRequestContext, CallerContext callerContext)
        {
            OperationResult result = new OperationResult();

            try
            {
                UserCredentialRequestBL userCredentialRequestBL = new UserCredentialRequestBL();
                result = userCredentialRequestBL.ProcessUserCredentialRequest(userRequestContext, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Wrap the normal exception into a WCF fault
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The Fault Exception of type WcfException</returns>
        private FaultException<MDMExceptionDetails> WrapException(Exception ex)
        {
            //TODO add the service url in the fault code
            MDMExceptionDetails fault = null;
            FaultReason reason = null;
            FaultException<MDMExceptionDetails> exception = null;

            //Get message code
            String messageCode = String.Empty;
            Object[] messageArguments = null;

            if (ex is MDMOperationException)
            {
                MDMOperationException mdmException = ex as MDMOperationException;

                messageCode = mdmException.MessageCode;
                messageArguments = mdmException.MessageArguments;
            }

            fault = new MDMExceptionDetails(messageCode, ex.Message, ex.StackTrace, ex.Source, ex.TargetSite.ToString(), messageArguments);

            reason = new FaultReason(ex.Message);

            exception = new FaultException<MDMExceptionDetails>(fault, reason);

            return exception;
        }

        /// <summary>
        /// Logs the exception into Event Log
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        private void LogException(Exception ex)
        {
            try
            {
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
            catch
            {
                //Do not throw
            }
        }

        #endregion

        #endregion
    }
}
