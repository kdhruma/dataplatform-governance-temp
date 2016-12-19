using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using MDM.AdminManager.Business;
using MDM.BusinessObjects;
using MDM.Core;
using MDM.Core.Exceptions;
using MDM.EntityManager.Business;
using MDM.ExceptionManager;
using MDM.SearchManager.Business;
using MDM.WCFServiceInterfaces;

namespace MDM.WCFServices
{
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class InteropSecurityService : IInteropSecurityService
    {
        public String GetAuthenticationToken(String userLoginName, String password)
        {
            String token = "My Token";
            try
            {
                UserPrincipalBL userPrincipal = new UserPrincipalBL();
                bool returnValue = userPrincipal.AuthenticateUser(userLoginName, password);
                if (returnValue)
                {
                    token = FormsAuthentication.GetAuthCookie(userLoginName, false).Value;
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return token;
        }

        public String GetUserPreference(String token, String userLoginName)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    UserPreferences userPreferences = new UserPreferences();
                    UserPreferencesBL userPreferencesBL = new UserPreferencesBL();
                    userPreferences = userPreferencesBL.GetUserPreferences(userLoginName);
                    return userPreferences.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return string.Empty;
        }

        public String GetUserRoles(String token, int LoginId, String userLoginName)
        {
            SecurityRoleCollection securityRoleCollection = new SecurityRoleCollection();

            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    SecurityRoleBL securityRoleManager = new SecurityRoleBL();
                    securityRoleCollection = securityRoleManager.GetUserRoles(LoginId, userLoginName);
                    return securityRoleCollection.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }
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



    }
}
