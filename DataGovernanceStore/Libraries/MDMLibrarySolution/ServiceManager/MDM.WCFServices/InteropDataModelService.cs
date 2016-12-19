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
    public class InteropDataModelService : IInteropDataModelService
    {
        CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);
        #region IInteropDataModelService Methods

        public String GetOrganizationByName(String token, String organizationShortName)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Organization org = dataModelService.GetOrganizationByName(organizationShortName, new OrganizationContext(false), callerContext);

                    if (org != null)
                        return org.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetContainerById(String token, Int32 containerId)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Container container = dataModelService.GetContainerById(containerId, new ContainerContext(false), callerContext);

                    if (container != null)
                        return container.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetContainerByName(String token, String containerShortName)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Container container = dataModelService.GetContainerByName(containerShortName, new ContainerContext(false), callerContext);

                    if (container != null)
                        return container.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetAllCategories(String token, Int32 hierarchyId, LocaleEnum locale)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    CategoryCollection categories = dataModelService.GetAllCategories(hierarchyId, locale, callerContext);

                    if (categories != null && categories.Count > 0)
                        return categories.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetCategoryById(String token, Int32 hierarchyId, Int64 categoryId)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Category category = dataModelService.GetCategoryById(hierarchyId, categoryId, callerContext);

                    if (category != null)
                        return category.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetCategoryByName(String token, Int32 hierarchyId, String categoryName)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Category category = dataModelService.GetCategoryByName(hierarchyId, categoryName, callerContext);

                    if (category != null)
                        return category.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetLookupModel(String token, String lookupTableName)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Lookup lookup = dataModelService.GetLookupModel(lookupTableName, callerContext);

                    if (lookup != null)
                        return lookup.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetLookupData(String token, String lookupTableName, LocaleEnum locale, Int32 maxRecordsToReturn)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Lookup lookup = dataModelService.GetLookupData(lookupTableName, locale, maxRecordsToReturn, false, callerContext);

                    if (lookup != null)
                        return lookup.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetAttributeLookupData(String token, Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    Lookup lookup = dataModelService.GetAttributeLookupData(attributeId, locale, maxRecordsToReturn, callerContext);

                    if (lookup != null)
                        return lookup.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return String.Empty;
        }

        public String GetAllEntityTypes(String token)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    DataModelService dataModelService = new DataModelService();
                    EntityTypeCollection entityTypes = dataModelService.GetAllEntityTypes(callerContext, false);

                    if (entityTypes != null && entityTypes.Count > 0)
                        return entityTypes.ToXml();
                }
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
