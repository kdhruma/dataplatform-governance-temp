using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Principal;

namespace MDM.Services
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.ExceptionManager;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using WFDSC = MDM.Services.WorkflowDesignerServiceClient;

    ///<summary>
    ///Client consuming Workflow Designer Service 
    ///</summary>
    public class WorkflowDesignerService
    {
        #region Fields

        /// <summary>
        /// Field denoting Configuration Name of EndPoint that Client consumes
        /// </summary>
        private String _endPointConfigurationName = String.Empty;

        /// <summary>
        /// Binding used for Workflow Designer WCF service
        /// </summary>
        private Binding _binding = null;

        /// <summary>
        /// Field denoting the Address of EndPoint
        /// </summary>
        private EndpointAddress _endpointAddress = null;

        /// <summary>
        /// Field denoting the Current Username 
        /// </summary>
        private String _username = String.Empty;

        /// <summary>
        /// Field denoting the User's Password
        /// </summary>
        private String _password = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Current Username 
        /// </summary>
        public String UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// Property denoting the User's Password
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public WorkflowDesignerService()
        {

        }

        /// <summary>
        /// Constructor with Binding and end
        /// </summary>
        /// <param name="binding">Indicates binding</param>
        /// <param name="endPointAddress">Indicates the End point address</param>
        public WorkflowDesignerService(Binding binding, EndpointAddress endPointAddress)
        {
            _binding = binding;
            _endpointAddress = endPointAddress;
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
            WFDSC.WorkflowDesignerServiceClient workflowDesignerServiceClient = null;

            try
            {
                workflowDesignerServiceClient = GetClient();

                appConfigValue = workflowDesignerServiceClient.GetAppConfigValue(keyName, ref operationResult);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(workflowDesignerServiceClient);
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
        /// <param name="callerContext">Caller Information details</param>
        public void GetWorkflowViewDetails(Int32 workflowVersionID, String instanceGuid, ref MDMBOW.WorkflowVersion workflowVersion, ref Collection<MDMBOW.TrackedActivityInfo> trackedActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            WFDSC.WorkflowDesignerServiceClient workflowDesignerServiceClient = null;

            try
            {
                workflowDesignerServiceClient = GetClient();

                workflowDesignerServiceClient.GetWorkflowViewDetails(workflowVersionID, instanceGuid, ref workflowVersion, ref trackedActivityCollection, ref operationResult, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(workflowDesignerServiceClient);
            }
        }

        /// <summary>
        /// Process workflows, based on Action
        /// </summary>
        /// <param name="workflows">workflows to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Returns the newly created workflow version</returns>
        public MDMBOW.WorkflowVersion ProcessWorkflows(Collection<MDMBOW.Workflow> workflows, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            WFDSC.WorkflowDesignerServiceClient workflowDesignerServiceClient = null;
            MDMBOW.WorkflowVersion workflowVersion = new MDMBOW.WorkflowVersion();

            try
            {
                workflowDesignerServiceClient = GetClient();
                workflowVersion = workflowDesignerServiceClient.ProcessWorkflows(workflows, loginUser, ref operationResult, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(workflowDesignerServiceClient);
            }

            return workflowVersion;
        }

        /// <summary>
        /// Process workflow activities, based on Action
        /// </summary>
        /// <param name="workflowActivities">workflow activities to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>returns status of the process</returns>
        public Int32 ProcessActivities(Collection<MDMBOW.WorkflowActivity> workflowActivities, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 output = 0;
            WFDSC.WorkflowDesignerServiceClient workflowDesignerServiceClient = null;

            try
            {
                workflowDesignerServiceClient = GetClient();
                output = workflowDesignerServiceClient.ProcessActivities(workflowActivities, loginUser, ref operationResult, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(workflowDesignerServiceClient);
            }

            return output;
        }

        /// <summary>
        /// Gets all the workflows, workflow versions and activities in the system
        /// </summary>
        /// <param name="workflowCollection">Collection of WorkFlow</param>
        /// <param name="workflowVersionCollection">Collection of Workflow Version</param>
        /// <param name="workflowActivityCollection">Collection of Workflow Activity</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        public void GetAllWorkflowDetails(ref Collection<MDMBOW.Workflow> workflowCollection, ref Collection<MDMBOW.WorkflowVersion> workflowVersionCollection, ref Collection<MDMBOW.WorkflowActivity> workflowActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            WFDSC.WorkflowDesignerServiceClient workflowDesignerServiceClient = null;

            try
            {
                workflowDesignerServiceClient = GetClient();
                workflowDesignerServiceClient.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, ref operationResult, callerContext);
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(workflowDesignerServiceClient);
            }
        }

        #endregion

        #region Private Methods

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

        /// <summary>
        /// Get Workflow Designer Service Client
        /// </summary>
        /// <returns>ServiceClient with UerName and Password</returns>
        private WFDSC.WorkflowDesignerServiceClient GetClient()
        {
            WFDSC.WorkflowDesignerServiceClient client = null;

            if (_binding != null && _endpointAddress != null)
                client = new WFDSC.WorkflowDesignerServiceClient(_binding, _endpointAddress);
            else if (String.IsNullOrEmpty(_endPointConfigurationName) && _endpointAddress == null)
                client = new WFDSC.WorkflowDesignerServiceClient();
            else if (!String.IsNullOrEmpty(_endPointConfigurationName) && _endpointAddress == null)
                client = new WFDSC.WorkflowDesignerServiceClient(_endPointConfigurationName);
            else if (!String.IsNullOrEmpty(_endPointConfigurationName) && _endpointAddress != null)
                client = new WFDSC.WorkflowDesignerServiceClient(_endPointConfigurationName, _endpointAddress);

            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                client.ClientCredentials.UserName.UserName = UserName;
                client.ClientCredentials.UserName.Password = Password;
                client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
            }

            return client;
        }

        private void DisposeClient(WFDSC.WorkflowDesignerServiceClient client)
        {
            if (client != null)
            {
                if (client.State == CommunicationState.Created || client.State == CommunicationState.Opened || client.State == CommunicationState.Opening)
                {
                    client.Close();
                }
                else if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
            }
        }

        #endregion

        #endregion
    }
}