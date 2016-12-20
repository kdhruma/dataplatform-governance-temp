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
    public class InteropConfigurationService : IInteropConfigurationService
    {
        CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);
        #region IInteropConfiguration Methods

        public String GetAllAppConfig()
        {
            try
            {
                ConfigurationService configService = new ConfigurationService();
                AppConfigCollection appConfigCollection = configService.GetAllAppConfigs(callerContext);

                if (appConfigCollection != null && appConfigCollection.Count > 0)
                    return appConfigCollection.ToXml();
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetAppConfig(String configKey)
        {
            try
            {
                ConfigurationService configService = new ConfigurationService();
                String  configValue = configService.GetAppConfig(configKey);
                return configValue;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
        }

        public String GetLocaleMessage(LocaleEnum locale, String messageCode)
        {
            try
            {
                ConfigurationService configService = new ConfigurationService();
                Collection<String> messageCollection = new Collection<string>();
                messageCollection.Add(messageCode);
                LocaleMessageCollection messages = configService.GetLocaleMessages(locale, messageCollection, false, callerContext);

                if ( messages != null && messages.Count > 0)
                    return messages.ToXml();
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
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



    }
}
