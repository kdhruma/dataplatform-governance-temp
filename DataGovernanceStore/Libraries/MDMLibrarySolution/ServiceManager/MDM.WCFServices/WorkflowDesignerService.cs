using System;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Activation;

namespace MDM.WCFServices
{
    using MDM.Core;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.ExceptionManager;
    using MDM.WCFServiceInterfaces;
    using MDM.Workflow.Designer.Business;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.AdminManager.Business;
    using MDM.CacheManager.Business;
    using MDM.Utility;
    
    /// <summary>
    /// The class which implements Workflow Designer Service Contract
    /// </summary>
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WorkflowDesignerService : IWorkflowDesignerService
    {
        #region Fields

        #endregion

        #region Constructors

        public WorkflowDesignerService()
        {
            LoadWorkflowDesignerSecurityPrincipal();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the value for the requested AppConfig key
        /// </summary>
        /// <param name="keyName">AppConfig key</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Value of the AppConfig key</returns>
        public String GetAppConfigValue(String keyName, ref MDMBO.OperationResult operationResult)
        {
            String appConfigValue = String.Empty;

            try
            {
                appConfigValue = AppConfigurationHelper.GetAppConfig<String>(keyName);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return appConfigValue;
        }

        /// <summary>
        /// Gets the details required for Execution/Definition view
        /// </summary>
        /// <param name="workflowVersionID"> Id of the workflow version</param>
        /// <param name="instanceGuid">GUID of the Instance</param>
        /// <param name="workflowVersion">Workflow version object. Ref param.</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="trackedActivityCollection">Tracked activity collection. Ref param.</param>
        public void GetWorkflowViewDetails(Int32 workflowVersionID, String instanceGuid, ref MDMBOW.WorkflowVersion workflowVersion, ref Collection<MDMBOW.TrackedActivityInfo> trackedActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            try
            {
                WorkflowViewBL workflowViewBL = new WorkflowViewBL();
                workflowViewBL.GetWorkflowViewDetails(workflowVersionID, instanceGuid, ref workflowVersion, ref trackedActivityCollection, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
        }

        /// <summary>
        /// Process workflows, based on Action
        /// </summary>
        /// <param name="workflows">workflows to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Returns the newly created workflow version</returns>
        public MDMBOW.WorkflowVersion ProcessWorkflows(Collection<MDMBOW.Workflow> workflows, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            MDMBOW.WorkflowVersion workflowVersion = new MDMBOW.WorkflowVersion();

            try
            {
                WorkflowBL workflowBL = new WorkflowBL();
                workflowVersion = workflowBL.Process(workflows, loginUser, callerContext, operationResult);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return workflowVersion;
        }

        /// <summary>
        /// Process workflow activities, based on Action
        /// </summary>
        /// <param name="workflowActivities">workflow activities to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>returns status of the process</returns>
        public Int32 ProcessActivities(Collection<MDMBOW.WorkflowActivity> workflowActivities, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 output = 0;

            try
            {
                WorkflowActivityBL workflowActivityBL = new WorkflowActivityBL();
                output = workflowActivityBL.Process(workflowActivities, loginUser, callerContext, operationResult);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return output;
        }

        /// <summary>
        /// Gets all the workflows, workflow versions and activities in the system
        /// </summary>
        /// <param name="workflowCollection"></param>
        /// <param name="workflowVersionCollection"></param>
        /// <param name="workflowActivityCollection"></param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        public void GetAllWorkflowDetails(ref Collection<MDMBOW.Workflow> workflowCollection, ref Collection<MDMBOW.WorkflowVersion> workflowVersionCollection, ref Collection<MDMBOW.WorkflowActivity> workflowActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                workflowInstanceBL.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Logs the exception into Event Log
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        protected void LogException(Exception ex)
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

        private MDMBO.SecurityPrincipal LoadWorkflowDesignerSecurityPrincipal()
        {
            MDMBO.SecurityPrincipal currentUserSecurityPrincipal = null;

            try
            {
                //Workflow internal would be using username as "system"
                String userName = "cfadmin";

                //Get the message security identity key
                String messageSecurityIdentityKey = AppConfigurationHelper.GetAppConfig<String>("MDM.WCFServices.MessageSecurity.IdentityKey");

                //Cache the message security identity key which will be used as authentication ticket for this security principal
                ICache cacheManager = CacheFactory.GetCache();

                //Set the form auth ticket into cache for further operations..
                cacheManager.Set(MDMBO.CacheKeyGenerator.GetFormAuthenticationTicketCacheKey(userName), messageSecurityIdentityKey, DateTime.Now.AddDays(1));

                //Get security principal..
                //If security principal is not available in the cache, the Get method creates and caches the security principal with the authentication ticket declared above
                SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
                currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, AuthenticationType.Forms, MDMCenterSystem.WcfService, "WithoutTimeStamp");
            }
            catch { }

            return currentUserSecurityPrincipal;
        }

        #endregion

        #endregion
    }
}
