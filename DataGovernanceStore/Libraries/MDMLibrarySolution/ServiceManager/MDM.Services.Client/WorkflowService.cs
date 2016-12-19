using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.ExceptionManager;
    using MDM.Interfaces;
    using MDM.Services.ServiceProxies;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;

    ///<summary>
    /// Workflow Service facilitates to work with MDMCenter workflows. 
    /// This includes initiating workflows, resuming persisted workflows, and performing work actions like assignments and approvals.
    ///</summary>
    public class WorkflowService : ServiceClientBase
    {
        #region Fields

        /// <summary>
        /// Binding used for Workflow WCF service
        /// </summary>
        private Binding _binding = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public WorkflowService()
            : base(typeof(WorkflowService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public WorkflowService(String endPointConfigurationName)
            : base(typeof(WorkflowService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public WorkflowService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates EndPoint Configuration</param>
        /// <param name="userIdentity">Indicates User Identity</param>
        public WorkflowService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public WorkflowService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates EndPoint Configuration Name</param>
        /// <param name="authenticationType">Indicates Type of Authentication</param>
        /// <param name="userIdentity">Indicates User Identity</param>
        /// <param name="userName">Indicates User Name</param>
        /// <param name="password">Indicates Password</param>
        public WorkflowService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates EndPoint Configuration Name</param>
        /// <param name="endPointAddress">Indicates EndPoint Address</param>
        /// <param name="authenticationType">Indicates User Identity</param>
        /// <param name="userIdentity">Indicates User Identity</param>
        /// <param name="userName">Indicates User Name</param>
        /// <param name="password">Indicates Password</param>
        public WorkflowService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with Binding
        /// </summary>
        /// <param name="binding">Indicates binding</param>
        /// <param name="endPointAddress">Indicates the End point address</param>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public WorkflowService(Binding binding, EndpointAddress endPointAddress, IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
            _binding = binding;
            this.EndpointAddress = endPointAddress;
        }

        #endregion

        #region Properties

        #endregion

        #region Public methods

        ///<summary>
        ///Starts the requested workflow
        ///</summary>
        ///<param name="workflowName">Name of WorkFlow</param>
        ///<param name="mdmObjectType">Type of MDMObject</param>
        ///<param name="mdmObjectId">Id of Object</param>
        ///<param name="serviceType">Indicates the Type of the Service</param>
        ///<param name="serviceId">Indicates the Id of the Service</param>
        ///<param name="currentUserName">Indicates User name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        /// <param name="comments" >Indicates comments for invoking workflow</param>
        ///<returns>returns 1 if workFlow starts successfully</returns>    
        public Int32 StartWorkflow(String workflowName, String mdmObjectType, Int64 mdmObjectId, String serviceType, Int32 serviceId, String currentUserName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            Int32 invokeSuccessCount = 0;

            if (String.IsNullOrEmpty(workflowName))
            {
                throw new ArgumentException("workflowName is null or empty");
            }

            // Create workflow object
            MDMBOW.Workflow workflow = new MDMBOW.Workflow();
            workflow.Name = workflowName;

            invokeSuccessCount = StartWorkflow(workflow, mdmObjectType, mdmObjectId, serviceType, serviceId, currentUserName, ref operationResult, callerContext, comments);

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");

            return invokeSuccessCount;
        }

        ///<summary>
        ///Starts the requested workflow
        ///</summary>
        ///<param name="workflow">Indicates the WorkFlow Object</param>
        ///<param name="mdmObjectType">Type of MDMObject</param>
        ///<param name="mdmObjectId">Id of MDM Object</param>
        ///<param name="serviceType">Indicates the Type of the Service</param>
        ///<param name="serviceId">Indicates the Id of the Service</param>
        ///<param name="currentUserName">Indicates User name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<param name="comments" >Indicates comments for invoking workflow</param>
        ///<returns>returns 1 if workFlow starts successfully</returns>    
        public Int32 StartWorkflow(MDMBOW.Workflow workflow, String mdmObjectType, Int64 mdmObjectId, String serviceType, Int32 serviceId, String currentUserName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            Int32 invokeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (workflow == null)
                    throw new ArgumentException("workflow is null");

                // Start new wf
                MDMBOW.WorkflowDataContext dataContext = new MDMBOW.WorkflowDataContext();
                MDMBOW.WorkflowMDMObject mdmObj = new MDMBOW.WorkflowMDMObject();

                mdmObj.MDMObjectId = mdmObjectId;
                mdmObj.MDMObjectType = mdmObjectType;
                dataContext.MDMObjectCollection.Add(mdmObj);
                dataContext.WorkflowComments = comments;

                if (!String.IsNullOrEmpty(workflow.Name))
                    dataContext.WorkflowName = workflow.Name;
                else if (!String.IsNullOrEmpty(workflow.LongName))
                    dataContext.WorkflowName = workflow.LongName;
                else if (workflow.LatestVersion < 1)
                    throw new Exception("Name for the Workflow to be invoked is not specified. Please specify the Workflow ShortName/LongName");

                dataContext.WorkflowVersionId = workflow.LatestVersion;

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        if (String.IsNullOrWhiteSpace(currentUserName))
                            currentUserName = windowsIdentity.Name;

                        invokeSuccessCount = workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, ref operationResult, FillDiagnosticTraces(callerContext));
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    if (String.IsNullOrWhiteSpace(currentUserName))
                        currentUserName = this.UserName;

                    invokeSuccessCount = workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return invokeSuccessCount;
        }

        ///<summary>
        ///Starts the workflow
        ///</summary>
        ///<param name="workflowCollection">Collection of WorkFlow Object</param>
        ///<param name="mdmObjectType">Type of MDMObject</param>
        ///<param name="mdmObjectId">Id of Object</param>
        ///<param name="serviceType">Indicates the Type of the Service</param>
        ///<param name="serviceId">Indicates the Id of the Service</param>
        ///<param name="currentUserName">Indicates User name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<param name="comments" >Indicates comments for invoking workflow</param>
        ///<returns>returns 1 if workFlow starts successfully</returns>
        public Int32 StartWorkflow(Collection<MDMBOW.Workflow> workflowCollection, String mdmObjectType, Int64 mdmObjectId, String serviceType, String currentUserName, Int32 serviceId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            Int32 invokeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                if (workflowCollection != null && workflowCollection.Count > 0)
                {
                    // Start new wf
                    MDMBOW.WorkflowDataContext dataContext = new MDMBOW.WorkflowDataContext();
                    MDMBOW.WorkflowMDMObject mdmObj = new MDMBOW.WorkflowMDMObject();

                    mdmObj.MDMObjectId = mdmObjectId;
                    mdmObj.MDMObjectType = mdmObjectType;
                    dataContext.MDMObjectCollection.Add(mdmObj);
                    dataContext.WorkflowComments = comments;

                    foreach (MDMBOW.Workflow wf in workflowCollection)
                    {
                        if (!String.IsNullOrEmpty(wf.Name))
                            dataContext.WorkflowName = wf.Name;
                        else if (!String.IsNullOrEmpty(wf.LongName))
                            dataContext.WorkflowName = wf.LongName;
                        else if (wf.LatestVersion < 1)
                            continue;

                        dataContext.WorkflowVersionId = wf.LatestVersion;

                        Int32 result = -1;

                        workflowServiceClient = GetClient(); // Get new client all the time..

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                        if (this.AuthenticationType == Core.AuthenticationType.Windows)
                        {
                            WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                            using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                            {
                                if (String.IsNullOrWhiteSpace(currentUserName))
                                    currentUserName = windowsIdentity.Name;

                                result = workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, ref operationResult, FillDiagnosticTraces(callerContext));
                            }
                        }
                        else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                        {
                            if (String.IsNullOrWhiteSpace(currentUserName))
                                currentUserName = this.UserName;

                            result = workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, ref operationResult, FillDiagnosticTraces(callerContext));
                        }

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");

                        if (result > 0)
                            invokeSuccessCount++;
                    }
                }
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return invokeSuccessCount;
        }

        ///<summary>
        ///Starts the requested workflow based on workflow name
        ///</summary>
        ///<example>
        ///<code>
        /// // Used Name Spaces
        ///    RS.MDM.Core
        ///    RS.MDM.Interfaces
        ///    RS.MDM.Services
        ///    RS.MDM.Utility
        ///    
        ///    String currentUserLogin = String.Empty; // Variable declaration for current security user
        /// 
        /// // Workflow Interface object
        ///    IWorkflow iWorkflow = MDMObjectFactory.GetIWorkflow();
        ///    
        /// // Assumption: Entity Id and Service Id are hard coded in this sample
        ///    Int32 serviceId = 1;    // Service Id
        ///    Int64 entityId = 1001;  // Sample entity Id
        ///    IOperationResult iOperationResult = MDMObjectFactory.GetIOperationResult(); //  Operation Result Interface
        ///    IWorkflowMDMObjectCollection iWorkflowMDMObjectCollection = MDMObjectFactory.GetIWorkflowMDMObjectCollection();     // Workflow Object Collection Object
        ///    ICallerContext iCallerContext = MDMObjectFactory.GetICallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);  // Prepare Caller Context
        ///
        ///    ISecurityPrincipal iSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();  // Get the security principal using SecurityPrincipalHelper
        ///   
        ///    if (iSecurityPrincipal != null)
        ///    {
        ///         currentUserLogin = iSecurityPrincipal.CurrentUserName;
        ///    }
        ///
        ///    iWorkflow.LongName = "New Product Introduction";    // Sample workflow name based on Riverworks model
        ///
        ///    IWorkflowMDMObject iMDMObject = MDMObjectFactory.GetIWorkflowMDMObject();
        ///    iMDMObject.MDMObjectId = entityId;  // Populate entity id to workflow object
        ///    iWorkflowMDMObjectCollection.Add(iMDMObject);
        ///
        /// // Initialize Workflow Service
        ///    WorkflowService workflowService = new WorkflowService();
        /// // Make a WCF call to invoke the workflow for requested entity
        ///    workflowService.StartWorkflow(iWorkflow, iWorkflowMDMObjectCollection, "API Sample", serviceId, currentUserLogin, WorkflowInstanceRunOptions.RunAsMultipleInstances, currentUserLogin, ref iOperationResult, iCallerContext);
        ///
        /// // Result: Entity will invoke the Workflow 
        ///
        /// </code>
        /// </example>
        ///<param name="workflowName">Name of the Workflow</param>
        ///<param name="mdmObjectCollection">Collection of mdmObject</param>
        ///<param name="serviceType">Type of Service</param>
        ///<param name="serviceId">Id of Service</param>
        ///<param name="currentUserName">Current User Name</param>
        ///<param name="workflowInstanceRunOption">Indicates how workflow runs as RunAsMultipleInstances/RunAsSingleInstance</param>
        ///<param name="operationResult">Operation Result of workflow </param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<param name="comments" >Indicates comments for invoking workflow</param>
        ///<returns>returns 1 if workFlow starts successfully</returns>
        public Int32 StartWorkflow(String workflowName, MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            String extendedProperties = String.Empty;
            return StartWorkflow(workflowName, mdmObjectCollection, serviceType, serviceId, currentUserName, workflowInstanceRunOption, extendedProperties, ref operationResult, callerContext, comments);
        }

        ///<summary>
        ///Starts the requested workflow based on workflow name
        ///</summary>
        ///<param name="workflowName">Name of the Workflow</param>
        ///<param name="mdmObjectCollection">Collection of mdmObject</param>
        ///<param name="serviceType">Type of Service</param>
        ///<param name="serviceId">Id of Service</param>
        ///<param name="currentUserName">Current User Name</param>
        ///<param name="workflowInstanceRunOption">Indicates how workflow runs as RunAsMultipleInstances/RunAsSingleInstance</param>
        ///<param name="extendedProperties">Provides additional data(or custom data) to be passed to the workflow process</param>
        ///<param name="operationResult">Operation Result of workflow </param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<param name="comments" >Indicates comments for invoking workflow</param>
        ///<returns>returns 1 if workFlow starts successfully</returns>
        public Int32 StartWorkflow(String workflowName, MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, String extendedProperties, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            Int32 invokeSuccessCount = 0;

            if (String.IsNullOrEmpty(workflowName))
                throw new ArgumentException("workflowName is null");

            // Create workflow object
            MDMBOW.Workflow workflow = new MDMBOW.Workflow();
            workflow.Name = workflowName;

            invokeSuccessCount = StartWorkflow(workflow, mdmObjectCollection, serviceType, serviceId, currentUserName, workflowInstanceRunOption, extendedProperties, ref operationResult, callerContext, comments);

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");

            return invokeSuccessCount;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information.
        /// </summary>
        /// <param name="iWorkflow">Indicates the Workflow</param>
        /// <param name="iMDMObjectCollection">Indicates the Collection of mdmObject</param>
        /// <param name="serviceType">Indicates the Type of Service</param>
        /// <param name="serviceId">Indicates the Id of Service</param>
        /// <param name="currentUserName">Indicates the Current User Name</param>
        /// <param name="workflowInstanceRunOption">Indicates how workflow runs as RunAsMultipleInstances/RunAsSingleInstance</param>
        /// <param name="iOperationResult">Indicates the Operation Result of workflow</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        ///<param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns success count which indicates the number of entites for which workflow started successfully</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="Start Workflow By Workflow Interface And MDMObjectCollection" />
        /// <code language="c#" title="Get Multiple Entities"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetMultipleEntities"/>
        /// </example>
        public Int32 StartWorkflow(IWorkflow iWorkflow, IWorkflowMDMObjectCollection iMDMObjectCollection, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref IOperationResult iOperationResult, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            String extendedProperties = String.Empty;
            Int32 count = StartWorkflow(iWorkflow, iMDMObjectCollection, serviceType, serviceId, currentUserName, workflowInstanceRunOption, String.Empty, ref iOperationResult, iCallerContext, comments);

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");

            return count;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information with an option to pass additional information using extended properties.
        /// </summary>
        /// <param name="iWorkflow">Indicates the Workflow</param>
        /// <param name="iMDMObjectCollection">Indicates the Collection of mdmObject</param>
        /// <param name="serviceType">Indicates the Type of Service</param>
        /// <param name="serviceId">Indicates the Id of Service</param>
        /// <param name="currentUserName">Indicates the Current User Name</param>
        /// <param name="workflowInstanceRunOption">Indicates how workflow runs as RunAsMultipleInstances/RunAsSingleInstance</param>
        /// <param name="extendedProperties">Provides additional data(or custom data) to be passed to the workflow process</param>
        /// <param name="iOperationResult">Indicates the Operation Result of workflow</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        ///<param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns success count which indicates the number of entites for which workflow started successfully</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and ExtendedProperties" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="Start Workflow By Workflow Interface And MDMObjectCollection And Extended Properties" />
        /// <code language="c#" title="Get Multiple Entities"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetMultipleEntities"/>
        /// </example>
        public Int32 StartWorkflow(IWorkflow iWorkflow, IWorkflowMDMObjectCollection iMDMObjectCollection, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, String extendedProperties, ref IOperationResult iOperationResult, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            Int32 invokeSuccessCount = 0;

            MDMBOW.Workflow workflow = (MDMBOW.Workflow)iWorkflow;
            MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = (MDMBOW.WorkflowMDMObjectCollection)iMDMObjectCollection;
            MDMBO.CallerContext callerContext = (MDMBO.CallerContext)iCallerContext;
            MDMBO.OperationResult operationResult = (MDMBO.OperationResult)iOperationResult;

            invokeSuccessCount = this.StartWorkflow(workflow, workflowMDMObjectCollection, serviceType, serviceId, currentUserName, workflowInstanceRunOption, extendedProperties, ref operationResult, callerContext, comments);

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");

            return invokeSuccessCount;
        }

        ///<summary>
        ///Starts the requested workflow
        ///</summary>
        ///<param name="workflow">Indicates the  Workflow</param>
        ///<param name="mdmObjectCollection">Collection of mdmObject</param>
        ///<param name="serviceType">Type of Service</param>
        ///<param name="serviceId">Id of Service</param>
        ///<param name="currentUserName">Current User Name</param>
        ///<param name="workflowInstanceRunOption">Indicates how workflow runs as RunAsMultipleInstances/RunAsSingleInstance</param>
        ///<param name="operationResult">Operation Result of workflow </param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<param name="comments" >Indicates comments for invoking workflow</param>
        ///<returns>Returns success count</returns>        
        public Int32 StartWorkflow(MDMBOW.Workflow workflow, MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            String extendedProperties = String.Empty;
            Int32 invokeSuccessCount = 0;

            invokeSuccessCount = StartWorkflow(workflow, mdmObjectCollection, serviceType, serviceId, currentUserName, workflowInstanceRunOption, extendedProperties, ref operationResult, callerContext, comments);

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");

            return invokeSuccessCount;
        }

        ///<summary>
        ///Starts the requested workflow
        ///</summary>
        ///<param name="workflow">Indicates the  Workflow</param>
        ///<param name="mdmObjectCollection">Collection of mdmObject</param>
        ///<param name="serviceType">Type of Service</param>
        ///<param name="serviceId">Id of Service</param>
        ///<param name="currentUserName">Current User Name</param>
        ///<param name="workflowInstanceRunOption">Indicates how workflow runs as RunAsMultipleInstances/RunAsSingleInstance</param>
        ///<param name="extendedProperties">Provides additional data(or custom data) to be passed to the workflow process</param>
        ///<param name="operationResult">Operation Result of workflow </param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<param name="comments" >Indicates comments for invoking workflow</param>
        ///<returns>Returns success count</returns>        
        public Int32 StartWorkflow(MDMBOW.Workflow workflow, MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, String extendedProperties, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", MDMTraceSource.AdvancedWorkflow, false);

            Int32 batchSize = 0;
            Int32 invokeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (workflow == null)
                    throw new ArgumentException("workflow is null");

                if (mdmObjectCollection == null || mdmObjectCollection.Count == 0)
                {
                    throw new ArgumentException("mdmObjectCollection is null or empty");
                }

                // Prepare workflow data context
                MDMBOW.WorkflowDataContext dataContext = new MDMBOW.WorkflowDataContext();

                dataContext.Application = callerContext.Application;
                dataContext.Module = callerContext.Module;
                dataContext.ExtendedProperties = extendedProperties;
                dataContext.WorkflowComments = comments;

                if (!String.IsNullOrEmpty(workflow.Name))
                    dataContext.WorkflowName = workflow.Name;
                else if (!String.IsNullOrEmpty(workflow.LongName))
                    dataContext.WorkflowName = workflow.LongName;
                else if (workflow.LatestVersion < 1)
                    throw new Exception("Name for the Workflow to be invoked is not specified. Please specify the Workflow ShortName/LongName");

                dataContext.WorkflowVersionId = workflow.LatestVersion;

                #region Commenting RunAsSingleInstance option

                // Commenting this code because we are going to support RunAsMultipleInstances always
                // RunAsSingleInstance has been obsoleted

                //if (workflowInstanceRunOption == WorkflowInstanceRunOptions.RunAsSingleInstance)
                //{
                //    dataContext.MDMObjectCollection = mdmObjectCollection;

                //    if (isTracingEnabled)
                //        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.", MDMTraceSource.AdvancedWorkflow);

                //    if (this.AuthenticationType == Core.AuthenticationType.Windows)
                //    {
                //        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                //        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                //        {
                //            if (String.IsNullOrWhiteSpace(currentUserName))
                //                currentUserName = windowsIdentity.Name;

                //            invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, FillDiagnosticTraces(callerContext));
                //        }
                //    }
                //    else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                //    {
                //        if (String.IsNullOrWhiteSpace(currentUserName))
                //            currentUserName = this.UserName;

                //        invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, FillDiagnosticTraces(callerContext));
                //    }

                //    if (isTracingEnabled)
                //        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.", MDMTraceSource.AdvancedWorkflow);
                //}

                #endregion Commenting RunAsSingleInstance option

                //Get the Workflow.LowPerf.Saturation.Batchsize(AppConfig)
                String strBatchSize = GetAppConfigValue("Workflow.LowPerf.Saturation.Batchsize", ref operationResult);

                try
                {
                    batchSize = Convert.ToInt32(strBatchSize);
                }
                catch
                {
                    throw new Exception("Workflow.LowPerf.Saturation.Batchsize AppConfig key value is not in a valid format.");
                }

                //Invoke workflows in batches as per the batch size
                MDMBOW.WorkflowMDMObjectCollection nextBatchMDMObjects = new MDMBOW.WorkflowMDMObjectCollection();

                foreach (MDMBOW.WorkflowMDMObject mdmObject in mdmObjectCollection)
                {
                    //Check whether item is duplicate..
                    //If yes, ignore the item
                    if (nextBatchMDMObjects.Contains(mdmObject))
                        continue;

                    //Create the batch
                    nextBatchMDMObjects.Add(mdmObject);

                    if (nextBatchMDMObjects.Count == batchSize)
                    {
                        //batch is ready..
                        //Invoke
                        dataContext.MDMObjectCollection = nextBatchMDMObjects;

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.", MDMTraceSource.AdvancedWorkflow);

                        if (this.AuthenticationType == Core.AuthenticationType.Windows)
                        {
                            WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                            using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                            {
                                if (String.IsNullOrWhiteSpace(currentUserName))
                                    currentUserName = windowsIdentity.Name;

                                invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                            }
                        }
                        else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                        {
                            if (String.IsNullOrWhiteSpace(currentUserName))
                                currentUserName = this.UserName;

                            invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                        }

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.", MDMTraceSource.AdvancedWorkflow);

                        //Clear the batch object
                        nextBatchMDMObjects = new MDMBOW.WorkflowMDMObjectCollection();
                    }
                }

                //nextBatchMDMObjects count is more than zero, then invoke for those objects
                if (nextBatchMDMObjects.Count > 0)
                {
                    dataContext.MDMObjectCollection = nextBatchMDMObjects;

                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.", MDMTraceSource.AdvancedWorkflow);

                    if (this.AuthenticationType == Core.AuthenticationType.Windows)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            if (String.IsNullOrWhiteSpace(currentUserName))
                                currentUserName = windowsIdentity.Name;

                            invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                        }
                    }
                    else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                    {
                        if (String.IsNullOrWhiteSpace(currentUserName))
                            currentUserName = this.UserName;

                        invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                    }

                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.", MDMTraceSource.AdvancedWorkflow);
                }
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.AdvancedWorkflow);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow", MDMTraceSource.AdvancedWorkflow);
            }

            return invokeSuccessCount;
        }

        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowCollection">Collection of Workflow Objects</param>
        /// <param name="mdmObjectCollection">Collection of mdmObject</param>
        /// <param name="serviceType">Type of Service</param>
        /// <param name="serviceId">Id of Service</param>
        /// <param name="currentUserName">Current User Name</param>
        /// <param name="workflowInstanceRunOption">Indicates how workflow runs as RunAsMultipleInstances/RunAsSingleInstance</param>
        /// <param name="operationResult">Operation Result of workflow</param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        ///<param name="comments" >Indicates comments for invoking workflow</param>
        /// <returns>returns 1 if workFlow starts successfully</returns>
        public Int32 StartWorkflow(Collection<MDMBOW.Workflow> workflowCollection, MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            Int32 batchSize = 0;
            Int32 invokeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                if (workflowCollection == null || workflowCollection.Count == 0)
                    throw new ArgumentException("workflowCollection is null or empty");

                if (mdmObjectCollection == null || mdmObjectCollection.Count == 0)
                {
                    throw new ArgumentException("mdmObjectCollection is null or empty");
                }

                base.ValidateContext();

                //Instantiate Client
                workflowServiceClient = GetClient();

                #region Commenting RunAsSingleInstance option

                // Commenting this code because we are going to support RunAsMultipleInstances always
                // RunAsSingleInstance has been obsoleted

                //if (workflowInstanceRunOption == WorkflowInstanceRunOptions.RunAsSingleInstance)
                //{
                //    // Prepare workflow data context
                //    MDMBOW.WorkflowDataContext dataContext = new MDMBOW.WorkflowDataContext();
                //    dataContext.MDMObjectCollection = mdmObjectCollection;
                //    dataContext.Application = callerContext.Application;
                //    dataContext.Module = callerContext.Module;
                //    dataContext.WorkflowComments = comments;

                //    foreach (MDMBOW.Workflow wf in workflowCollection)
                //    {
                //        if (!String.IsNullOrEmpty(wf.Name))
                //            dataContext.WorkflowName = wf.Name;
                //        else if (!String.IsNullOrEmpty(wf.LongName))
                //            dataContext.WorkflowName = wf.LongName;
                //        else if (wf.LatestVersion < 1)
                //            continue;

                //        dataContext.WorkflowVersionId = wf.LatestVersion;

                //        if (isTracingEnabled)
                //            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                //        if (this.AuthenticationType == Core.AuthenticationType.Windows)
                //        {
                //            if (this.UserIdentity is WindowsIdentity)
                //            {
                //                WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                //                using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                //                {
                //                    if (String.IsNullOrWhiteSpace(currentUserName))
                //                        currentUserName = windowsIdentity.Name;

                //                    invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, FillDiagnosticTraces(callerContext));
                //                }
                //            }
                //        }
                //        else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                //        {
                //            if (String.IsNullOrWhiteSpace(currentUserName))
                //                currentUserName = this.UserName;

                //            invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, FillDiagnosticTraces(callerContext));
                //        }

                //        if (isTracingEnabled)
                //            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
                //    }
                //}

                #endregion Commenting RunAsSingleInstance option

                //Get the Workflow.LowPerf.Saturation.Batchsize(AppConfig)
                String strBatchSize = GetAppConfigValue("Workflow.LowPerf.Saturation.Batchsize", ref operationResult);

                try
                {
                    batchSize = Convert.ToInt32(strBatchSize);
                }
                catch
                {
                    throw new Exception("Workflow.LowPerf.Saturation.Batchsize AppConfig key value is not in a valid format.");
                }

                foreach (MDMBOW.Workflow wf in workflowCollection)
                {
                    // Prepare workflow data context
                    MDMBOW.WorkflowDataContext dataContext = new MDMBOW.WorkflowDataContext();

                    if (!String.IsNullOrEmpty(wf.Name))
                        dataContext.WorkflowName = wf.Name;
                    else if (!String.IsNullOrEmpty(wf.LongName))
                        dataContext.WorkflowName = wf.LongName;
                    else if (wf.LatestVersion < 1)
                        continue;

                    dataContext.WorkflowVersionId = wf.LatestVersion;

                    //Invoke workflows in batches as per the batch size
                    MDMBOW.WorkflowMDMObjectCollection nextBatchMDMObjects = new MDMBOW.WorkflowMDMObjectCollection();

                    foreach (MDMBOW.WorkflowMDMObject mdmObject in mdmObjectCollection)
                    {
                        //Check whether item is duplicate..
                        //If yes, ignore the item
                        if (nextBatchMDMObjects.Contains(mdmObject))
                            continue;

                        //Create the batch
                        nextBatchMDMObjects.Add(mdmObject);

                        if (nextBatchMDMObjects.Count == batchSize)
                        {
                            //batch is ready..
                            //Invoke
                            dataContext.MDMObjectCollection = nextBatchMDMObjects;

                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                            if (this.AuthenticationType == Core.AuthenticationType.Windows)
                            {
                                if (this.UserIdentity is WindowsIdentity)
                                {
                                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                                    {
                                        if (String.IsNullOrWhiteSpace(currentUserName))
                                            currentUserName = windowsIdentity.Name;

                                        invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                                    }
                                }
                            }
                            else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                            {
                                if (String.IsNullOrWhiteSpace(currentUserName))
                                    currentUserName = this.UserName;

                                invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                            }

                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");

                            //Clear the batch object
                            nextBatchMDMObjects = new MDMBOW.WorkflowMDMObjectCollection();
                        }
                    }

                    //nextBatchMDMObjects count is more than zero, then invoke for those objects
                    if (nextBatchMDMObjects.Count > 0)
                    {
                        dataContext.MDMObjectCollection = nextBatchMDMObjects;

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                        if (this.AuthenticationType == Core.AuthenticationType.Windows)
                        {
                            if (this.UserIdentity is WindowsIdentity)
                            {
                                WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                                using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                                {
                                    if (String.IsNullOrWhiteSpace(currentUserName))
                                        currentUserName = windowsIdentity.Name;

                                    invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                                }
                            }
                        }
                        else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                        {
                            if (String.IsNullOrWhiteSpace(currentUserName))
                                currentUserName = this.UserName;

                            invokeSuccessCount += workflowServiceClient.StartWorkflow(dataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
                        }

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
                    }
                }
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return invokeSuccessCount;
        }

        ///<summary>
        ///Resumes workflows
        ///</summary>
        ///<param name="mdmObject">The MDM Object for which workflow needs to be resumed</param>
        ///<param name="currentActivityName">Currently executing activity.</param>
        ///<param name="action">Represents what action is being performed on the entity and based on the action, what action is required on the workflow.</param>
        ///<param name="comments">Comment for the particular action on the workflow.</param>
        ///<param name="actingUserName">Represents which user is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="actingUserId">Represents which user(id) is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="operationResult">Operation result</param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<returns>The success count</returns>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowMDMObject mdmObject, String currentActivityName, String action, String comments, String actingUserName, Int32 actingUserId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ResumeWorkflow", false);

            Int32 resumeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                actionContext.UserAction = action;
                actionContext.Comments = comments;
                actionContext.CurrentActivityName = currentActivityName;
                actionContext.ActingUserName = actingUserName;
                actionContext.ActingUserId = actingUserId;

                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ResumeWorkflowInstanceByMDMObject' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            resumeSuccessCount = workflowServiceClient.ResumeWorkflow(mdmObject, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    resumeSuccessCount = workflowServiceClient.ResumeWorkflow(mdmObject, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ResumeWorkflowInstanceByMDMObject' response message.");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ResumeWorkflow");
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <example>
        /// <code>
        /// WorkflowDataContext workflowDataContext = new WorkflowDataContext();
        /// String currentActivityName = "Marketing Enrichment";    // Based on Riverworks Samples
        /// String userAction = "Resume"; // User action 
        /// String comments = "Resume Workflow";   // User comments
        /// Int32 actingUserId = -1;
        ///
        /// String actingUserName = String.Empty; // Variable declared for current security user
        /// OperationResult operationResult = new OperationResult(); //  Operation Result Interface
        /// CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);  // Prepare Caller Context
        ///
        /// workflowDataContext.WorkflowName = "New Product Introduction";  // Based on Riverworks samples
        /// workflowDataContext.WorkflowVersionId = 1;  // Workflow Version number
        /// workflowDataContext.WorkflowComments = comments;
        ///
        /// ISecurityPrincipal iSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();  // Get the security principal using SecurityPrincipalHelper
        ///
        /// if (iSecurityPrincipal != null)
        ///    {
        ///      actingUserName = iSecurityPrincipal.CurrentUserName;
        ///      actingUserId = iSecurityPrincipal.CurrentUserId;
        ///    }
        ///
        /// // Initialize Workflow Service
        ///    WorkflowService workflowService = new WorkflowService();
        /// // Make a WCF call to Start workflow
        ///    Int32 successCount = workflowService.ResumeWorkflow(workflowDataContext, currentActivityName, userAction, comments, actingUserName, actingUserId, ref operationResult, callerContext);
        ///
        /// //  OutPut: 
        /// //  OperationResult determines whether the workflow is resumed or not
        /// //  If the operation result is successful then workflow gets resumed
        /// 
        /// </code>
        /// </example>
        /// <param name="workflowDataContext">The data context holding MDM Objects for which workflow needs to be resumed</param>
        /// <param name="currentActivityName">Currently executing activity.</param>
        ///<param name="action">Represents what action is being performed on the entity and based on the action, what action is required on the workflow.</param>
        ///<param name="comments">Comment for the particular action on the workflow.</param>
        ///<param name="actingUserName">Represents which user is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="actingUserId">Represents which user(id) is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="operationResult">Operation result</param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<returns>The success count</returns>
        ///<exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        ///<exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String currentActivityName, String action, String comments, String actingUserName, Int32 actingUserId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ResumeWorkflow", false);

            Int32 resumeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                actionContext.UserAction = action;
                actionContext.Comments = comments;
                actionContext.CurrentActivityName = currentActivityName;
                actionContext.ActingUserName = actingUserName;
                actionContext.ActingUserId = actingUserId;

                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ResumeMultipleWorkflowInstancesByMDMObjects' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            resumeSuccessCount = workflowServiceClient.ResumeWorkflow(workflowDataContext, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    resumeSuccessCount = workflowServiceClient.ResumeWorkflow(workflowDataContext, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ResumeMultipleWorkflowInstancesByMDMObjects' response message.");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ResumeWorkflow");
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// resumes workflow
        /// </summary>
        ///<param name="workflowInstances">The Instance object which needs to be resumed</param>
        ///<param name="currentActivityName">Currently executing activity.</param>
        ///<param name="action">Represents what action is being performed on the entity and based on the action, what action is required on the workflow.</param>
        ///<param name="comments">Comment for the particular action on the workflow.</param>
        ///<param name="actingUserName">Represents which user is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="actingUserId">Represents which user(id) is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="operationResult">Operation result</param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<returns>The success count</returns>
        ///<exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        ///<exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        public Int32 ResumeWorkflow(Collection<MDMBOW.WorkflowInstance> workflowInstances, String currentActivityName, String action, String comments, String actingUserName, Int32 actingUserId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ResumeWorkflow", false);

            Int32 resumeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                actionContext.UserAction = action;
                actionContext.Comments = comments;
                actionContext.CurrentActivityName = currentActivityName;
                actionContext.ActingUserName = actingUserName;
                actionContext.ActingUserId = actingUserId;

                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ResumeMultipleWorkflowInstances' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            resumeSuccessCount = workflowServiceClient.ResumeWorkflow(workflowInstances, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    resumeSuccessCount = workflowServiceClient.ResumeWorkflow(workflowInstances, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ResumeMultipleWorkflowInstances' response message.");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ResumeWorkflow");
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes WorkFlow
        /// </summary>
        /// <param name="commaSeparatedRuntimeInstanceId">Instance Ids which needs to be resumed</param>
        /// <param name="currentActivityName">Currently executing activity.</param>
        ///<param name="action">Represents what action is being performed on the entity and based on the action, what action is required on the workflow.</param>
        ///<param name="comments">Comment for the particular action on the workflow.</param>
        ///<param name="actingUserName">Represents which user is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="actingUserId">Represents which user(id) is working on the entity and thus indirectly on the workflow.</param>
        ///<param name="operationResult">Operation result</param>
        ///<param name="callerContext">Indicates Context of the Caller</param>
        ///<returns>The success count</returns>
        public Int32 ResumeWorkflow(String commaSeparatedRuntimeInstanceId, String currentActivityName, String action, String comments, String actingUserName, Int32 actingUserId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ResumeWorkflow", false);

            Int32 resumeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                actionContext.UserAction = action;
                actionContext.Comments = comments;
                actionContext.CurrentActivityName = currentActivityName;
                actionContext.ActingUserName = actingUserName;
                actionContext.ActingUserId = actingUserId;

                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ResumeWorkflowInstancesByRuntimeInstanceIds' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            resumeSuccessCount = workflowServiceClient.ResumeWorkflow(commaSeparatedRuntimeInstanceId, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    resumeSuccessCount = workflowServiceClient.ResumeWorkflow(commaSeparatedRuntimeInstanceId, actionContext, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ResumeWorkflowInstancesByRuntimeInstanceIds' response message");

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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ResumeWorkflow");
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes Aborted Instances
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be resumed</param>
        /// <param name="instanceStatus">The status to which instances needs to be updated</param>
        /// <param name="loginUser">The user who is resuming the workflow</param>
        /// <param name="programName">The name of the program requesting for resume</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        /// <returns>The success count</returns>
        public Int32 ResumeAbortedWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, String instanceStatus, String loginUser, String programName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ResumeAbortedWorkflowInstances", false);

            Int32 resumeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ResumeAbortedWorkflowInstances' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            resumeSuccessCount = workflowServiceClient.ResumeAbortedWorkflowInstances(instanceCollection, instanceStatus, loginUser, programName, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    resumeSuccessCount = workflowServiceClient.ResumeAbortedWorkflowInstances(instanceCollection, instanceStatus, loginUser, programName, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ResumeAbortedWorkflowInstances' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ResumeAbortedWorkflowInstances");
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Terminates the requested workflow instances
        /// </summary>
        /// <example>
        /// <code>
        /// // Set base success count to zero
        /// Int32 terminateSuccessCount = 0;
        /// 
        /// // Get new instance of Collection<![CDATA[<WorkflowInstance>]]> 
        /// Collection<![CDATA[<WorkflowInstance>]]> instanceCollection = new Collection<![CDATA[<WorkflowInstance>]]>();
        /// 
        /// // Set caller context for getting Attribute Model
        /// CallerContext callerContext = new CallerContext();
        /// callerContext.Application = MDMCenterApplication.WindowsWorkflow;
        /// callerContext.Module = MDMCenterModules.WindowsWorkflow;
        /// 
        /// // Set empty operation result
        /// OperationResult operationResult = new OperationResult();
        /// WorkflowService workflowService = GetMDMWorkflowService();
        /// // Create workflow object
        /// WorkflowMDMObject workflowMdmObject = new WorkflowMDMObject(1, "MDM.BusinessObjects.Entity");
        /// 
        /// // Get the collection of Workflow Instances based on the Workflow Id
        /// instanceCollection = workflowService.FindWorkflowInstance(1, workflowMdmObject, ref operationResult, callerContext);
        /// 
        /// // Make a WCF call to workflowService.TerminateWorkflowInstances
        /// terminateSuccessCount = workflowService.TerminateWorkflowInstances(instanceCollection, ref operationResult, callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="workflowInstances">Indicates the collection of Instance objects which needs to be terminated</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the success count</returns>
        public Int32 TerminateWorkflowInstances(Collection<MDMBOW.WorkflowInstance> workflowInstances, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.TerminateWorkflowInstances", false);

            Int32 resumeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'TerminateWorkflowInstances' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            resumeSuccessCount = workflowServiceClient.TerminateWorkflowInstances(workflowInstances, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    resumeSuccessCount = workflowServiceClient.TerminateWorkflowInstances(workflowInstances, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'TerminateWorkflowInstances' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.TerminateWorkflowInstances");
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Promotes requested workflow instances
        /// </summary>
        /// <param name="workflowInstances">The collection of Instance objects which needs to be promoted</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        /// <returns>The success count</returns>
        public Int32 PromoteWorkflowInstances(Collection<MDMBOW.WorkflowInstance> workflowInstances, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.PromoteWorkflowInstances", false);

            Int32 resumeSuccessCount = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity == null)
                        throw new ArgumentException("UserIdentitiy is null.");

                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'PromoteWorkflowInstances' request message.");

                    //Windows identitiy..
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            resumeSuccessCount = workflowServiceClient.PromoteWorkflowInstances(workflowInstances, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    resumeSuccessCount = workflowServiceClient.PromoteWorkflowInstances(workflowInstances, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'PromoteWorkflowInstances' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.PromoteWorkflowInstances");
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Gets all the Workflow, Workflow versions and activities in the system
        /// </summary>
        /// <example>
        /// <code>
        /// // Used Name Spaces
        ///    RS.MDM.BusinessObjects
        ///    RS.MDM.BusinessObjects.Workflow;
        ///    RS.MDM.Interfaces
        ///    RS.MDM.Services
        ///   
        /// // Variable declaration for Workflow data 
        ///    <![CDATA[Collection<Workflow>]]>workflowCollection = null;
        /// // Variable declaration for Workflow version data 
        ///    <![CDATA[Collection<WorkflowVersion>]]> workflowVersionCollection = null;
        /// // Variable declaration for Workflow Activities data
        ///    <![CDATA[Collection<WorkflowActivity>]]> workflowActivityCollection = null;
        ///    
        /// // Initialize Operation Result object
        ///    OperationResult operationResult = new OperationResult();
        /// // Prepare Caller Context
        ///    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);
        ///
        /// // Initialize Workflow Service
        ///    WorkflowService workflowService = new WorkflowService();
        /// // Make a WCF call to get all Workflow Details
        ///    workflowService.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, ref operationResult, callerContext);
        ///
        /// // Output: WorkflowCollection contains all the Workflows that are present in the MDMCenter System
        /// //         WorkflowVersionCollection contains all the Workflow versions that are present in the MDMCenter System
        /// //         WorkflowActivityCollection contains all activity details
        /// //         operationResult will be updated based on the result
        ///
        /// </code>
        /// </example>
        /// <param name="workflowCollection">Collection of WorkFlow</param>
        /// <param name="workflowVersionCollection">Collection of Workflow Version</param>
        /// <param name="workflowActivityCollection">Collection of Workflow Activity</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        public void GetAllWorkflowDetails(ref Collection<MDMBOW.Workflow> workflowCollection, ref Collection<MDMBOW.WorkflowVersion> workflowVersionCollection, ref Collection<MDMBOW.WorkflowActivity> workflowActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.GetAllWorkflowDetails", false);

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'GetAllWorkflowDetails' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            workflowServiceClient.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    workflowServiceClient.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'GetAllWorkflowDetails' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.GetAllWorkflowDetails");
            }
        }

        /// <summary>
        /// Get Done report for current user
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <param name="userParticipation">Participation of the user</param>
        /// <param name="catalogIds">Category Ids</param>
        /// <param name="entityTypeIds">EntityType Ids</param>
        /// <param name="workflowNames">Name of all Workflows</param>
        /// <param name="currentWorkflowActivity">Current WorkFlow Activity</param>
        /// <param name="attributeIds"></param>
        /// <param name="localeId"></param>
        /// <param name="countFrom"></param>
        /// <param name="countTo"></param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        /// <returns>Collection of Entity</returns>
        public Collection<MDMBO.DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds,
                                                                      Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds, Int32 localeId, Int64 countFrom,
                                                                      Int64 countTo, MDMBO.CallerContext callerContext)
        {
            return GetWorkflowDoneReport(userId, userParticipation, catalogIds, entityTypeIds, workflowNames, currentWorkflowActivity, attributeIds, localeId, countFrom, countTo, null, callerContext);
        }

        /// <summary>
        /// Get Done report for current user
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <param name="userParticipation">Participation of the user</param>
        /// <param name="catalogIds">Category Ids</param>
        /// <param name="entityTypeIds">EntityType Ids</param>
        /// <param name="workflowNames">Name of all Workflows</param>
        /// <param name="currentWorkflowActivity">Current WorkFlow Activity</param>
        /// <param name="attributeIds"></param>
        /// <param name="localeId"></param>
        /// <param name="countFrom"></param>
        /// <param name="attributesDataSource">Specifies attributes data source. Allowed values: 'DNTables' (default if not specified) and 'CoreTables'.</param>
        /// <param name="countTo"></param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        /// <returns>Collection of Entity</returns>
        public Collection<MDMBO.DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds,
                Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds, Int32 localeId, Int64 countFrom, Int64 countTo,
                String attributesDataSource, MDMBO.CallerContext callerContext)
        {
            return MakeServiceCall("GetWorkflowDoneReport",
                                   "GetWorkflowDoneReport",
                                   service => service.GetWorkflowDoneReport(userId,
                                                                            userParticipation,
                                                                            catalogIds,
                                                                            entityTypeIds,
                                                                            workflowNames,
                                                                            currentWorkflowActivity,
                                                                            attributeIds,
                                                                            localeId,
                                                                            countFrom,
                                                                            countTo,
                                                                            attributesDataSource,
                                                                            FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the summary of the instances as per the filter criteria
        /// </summary>
        /// <example>
        /// <code>
        /// // Get nulled result collection
        /// Collection<![CDATA[<WorkflowInstance>]]> workflowInstances = null;
        /// 
        /// // Pupulating filter criteria
        /// String workflowType = "WWF"; 
        /// Int32 workflowId = 1;
        /// Int32 workflowVersionId = 1;
        /// String workflowStatus = "Running";
        /// String activityShortName = "EDC0D414-FEFA-4388-AED6-9DDBB1548AE5";       
        /// String roleIds = "1,2"; // CSV format
        /// String userIds = "cfadmin";
        /// String instanceId = "ABC0D414-FEFA-4388-AED6-9DDBB1548AE5";
        /// String mdmObjectIds = "1,2,3"; // CSV format
        /// Boolean? hasEscalation = false;
        /// Int32 returnSize = 100;
        /// // Please note that this information should exist in the database
        /// 
        /// // Set empty operation result
        /// OperationResult operationResult = new OperationResult();
        /// 
        /// // Set caller context for getting Attribute Model
        /// CallerContext callerContext = new CallerContext();
        /// callerContext.Application = MDMCenterApplication.WindowsWorkflow;
        /// callerContext.Module = MDMCenterModules.WindowsWorkflow;
        /// 
        /// WorkflowService workflowService = GetMDMWorkflowService();
        /// 
        /// // Make a WCF call to workflowService.TerminateWorkflowInstances
        /// workflowInstances = workflowService.GetInstanceSummary(workflowType, workflowId, workflowVersionId, workflowStatus, activityShortName, roleIds,
        /// userIds, instanceId, mdmObjectIds, hasEscalation, returnSize, ref operationResult, callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="workflowType">Indicates type of Workflow</param>
        /// <param name="workflowId">Indicates the Workflow Id</param>
        /// <param name="workflowVersionId">Indicates the Workflow verssion Id</param>
        /// <param name="workflowStatus">Indicated the Workflow status</param>
        /// <param name="activityShortName">Indicates the workflow short name</param>
        /// <param name="roleIds">Indicates the login role Id</param>
        /// <param name="userIds">Indicates the login User Id</param>
        /// <param name="instanceId">Indicates the workflow Instance id</param>
        /// <param name="mdmObjectIds">Indicates the MDM Object Ids</param>
        /// <param name="hasEscalation">Indicates the whether need to do Escalation or not</param>
        /// <param name="returnSize">Indicates the return size</param>
        /// <param name="operationResult">Indicates the object which collects results of the operation</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the collection of workflow instances</returns>
        public Collection<MDMBOW.WorkflowInstance> GetInstanceSummary(String workflowType, Int32 workflowId, Int32 workflowVersionId, String workflowStatus, String activityShortName, String roleIds, String userIds, String instanceId, String mdmObjectIds, Boolean? hasEscalation, Int32 returnSize, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.GetInstanceSummary", false);

            IWorkflowService workflowServiceClient = null;
            Collection<MDMBOW.WorkflowInstance> workflowInstanceCollection = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'GetInstanceSummary' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            workflowInstanceCollection = workflowServiceClient.GetInstanceSummary(workflowType, workflowId, workflowVersionId, workflowStatus, activityShortName, roleIds, userIds, instanceId, mdmObjectIds, hasEscalation, returnSize, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    workflowInstanceCollection = workflowServiceClient.GetInstanceSummary(workflowType, workflowId, workflowVersionId, workflowStatus, activityShortName, roleIds, userIds, instanceId, mdmObjectIds, hasEscalation, returnSize, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'GetInstanceSummary' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.GetInstanceSummary");
            }

            return workflowInstanceCollection;
        }

        /// <summary>
        /// Gets the details of the requested instance
        /// </summary>
        /// <param name="workflowType">Indicates type of Workflow</param>
        /// <param name="instanceId">Indicates the Instance Id</param>
        /// <param name="runningActivityCollection">Collection of running activities</param>
        /// <param name="mdmObjectCollection">Collection of MDM objects participating in the instance</param>
        /// <param name="escalationCollection">Collection of escalations happened for this instance</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Caller Information details</param>
        public void GetInstanceDetails(String workflowType, String instanceId, ref Collection<MDMBOW.WorkflowActivity> runningActivityCollection, ref Collection<MDMBOW.WorkflowMDMObject> mdmObjectCollection, ref Collection<MDMBOW.Escalation> escalationCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.GetInstanceDetails", false);

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'GetInstanceDetails' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            workflowServiceClient.GetInstanceDetails(workflowType, instanceId, ref runningActivityCollection, ref mdmObjectCollection, ref escalationCollection, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    workflowServiceClient.GetInstanceDetails(workflowType, instanceId, ref runningActivityCollection, ref mdmObjectCollection, ref escalationCollection, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'GetInstanceDetails' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.GetInstanceDetails");

            }
        }

        /// <summary>
        /// Gets the statistics
        /// </summary>
        /// <param name="workflowType">Indicates type of Workflow</param>
        /// <param name="workflowId">Indicates the workflow Id</param>
        /// <param name="workflowVersionId">Indicates the workflow Version Id</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Statistics in the format of string</returns>
        public String GetWorkflowStatistics(String workflowType, Int32 workflowId, Int32 workflowVersionId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.GetWorkflowStatistics", false);

            String strStatistics = String.Empty;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'GetWorkflowStatistics' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            strStatistics = workflowServiceClient.GetWorkflowStatistics(workflowType, workflowId, workflowVersionId, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    strStatistics = workflowServiceClient.GetWorkflowStatistics(workflowType, workflowId, workflowVersionId, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'GetWorkflowStatistics' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.GetWorkflowStatistics");
            }

            return strStatistics;
        }

        /// <summary>
        /// Updates the instance status and instance's acting user.
        /// </summary>
        /// <example>
        /// <code>
        /// // Used Name Spaces
        ///    RS.MDM.BusinessObjects
        ///    RS.MDM.Core
        ///    RS.MDM.Interfaces
        ///    RS.MDM.Services
        ///    RS.MDM.Utility
        ///
        ///    String currentUserLogin = String.Empty; // Variable declared for current security user
        ///    OperationResult operationResult = new OperationResult(); //  Operation Result Interface
        ///    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);  // Prepare Caller Context
        ///
        ///    ISecurityPrincipal iSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();  // Get the security principal using SecurityPrincipalHelper
        ///
        ///    if (iSecurityPrincipal != null)
        ///    {
        ///         currentUserLogin = iSecurityPrincipal.CurrentUserName;
        ///    }
        ///
        ///    String instanceGUID = "d8d461ff-c096-45af-b846-f57d4c254038";   // Sample Workflow instance id
        ///    String mdmObjectIds = "1001";   // Sample entity id 
        ///    Int32 workflowId = 1;   // Sample Workflow id
        ///    String activityShortName = "a0b77d5c-23b4-4730-905e-701e93bb04c5";  // Sample Workflow activity short name
        ///
        /// // Initialize Workflow Service
        ///    WorkflowService workflowService = new WorkflowService();
        /// // Make a WCF call to Start Workflow
        ///    Boolean result= workflowService.UpdateWorkflowInstances(instanceGUID, mdmObjectIds, "Sample API", workflowId, activityShortName, "Update", currentUserLogin, "API Sample", ref operationResult, callerContext);
        ///    
        /// // Result: If the result is true, then the Workflow activity will be updated based on the input, else it will fail. The operation result will be populated with failed reason
        ///
        /// </code>
        /// </example>
        /// <param name="instanceGUIDs">Comma separated GUIDs of instances to be updated</param>
        /// <param name="mdmObjectIDs">Comma separated MDM object IDs</param>
        /// <param name="mdmObjectType">Type of the MDM object</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="activityShortName">Short Name of the activity</param>
        /// <param name="instanceStatus">Status which is going to be updated</param>
        /// <param name="loginUser">Login User which is going to be updated as Acting User</param>
        /// <param name="programName">Name of the module which is updating</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <param name="operationResult">Object which collects result of operation</param>
        /// <returns>Result of the operation</returns>
        public Boolean UpdateWorkflowInstances(String instanceGUIDs, String mdmObjectIDs, String mdmObjectType, Int32 workflowID, String activityShortName, String instanceStatus, String loginUser, String programName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.UpdateWorkflowInstances", false);

            Boolean result = false;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'UpdateWorkflowInstances' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            result = workflowServiceClient.UpdateWorkflowInstances(instanceGUIDs, mdmObjectIDs, mdmObjectType, workflowID, activityShortName, instanceStatus, loginUser, programName, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    result = workflowServiceClient.UpdateWorkflowInstances(instanceGUIDs, mdmObjectIDs, mdmObjectType, workflowID, activityShortName, instanceStatus, loginUser, programName, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'UpdateWorkflowInstances' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.UpdateWorkflowInstances");
            }

            return result;
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="toolBarButtonXML">Configuration XML for action buttons</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetWorkflowActionButtons(Int32 activityId, String toolBarButtonXML, MDMBO.CallerContext callerContext)
        {
            return GetWorkflowActionButtons(activityId, toolBarButtonXML, null, null, callerContext);
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="toolBarButtonXML">Configuration XML for action buttons</param>
        /// <param name="loginUser">User login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetWorkflowActionButtons(Int32 activityId, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.GetWorkflowActionButtons", false);

            DataTable toolBarButtonTable = new DataTable();

            IWorkflowService workflowServiceClient = null;
            MDMBO.OperationResult operationResult = new MDMBO.OperationResult();

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'GetWorkflowActionButtons' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            toolBarButtonTable = workflowServiceClient.GetActionButtons(activityId, toolBarButtonXML, loginUser, checkAllowedUsersAndRoles, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    toolBarButtonTable = workflowServiceClient.GetActionButtons(activityId, toolBarButtonXML, loginUser, checkAllowedUsersAndRoles, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'GetWorkflowActionButtons' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.GetWorkflowActionButtons");
            }

            return toolBarButtonTable;
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Returns action buttons with comments details</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <exception cref="Exception">Thrown when method call fails to get the data</exception>
        public MDMBO.Table GetActivityActionButtons(Int32 activityId, MDMCenterApplication application, MDMCenterModules module)
        {
            return GetActivityActionButtons(activityId, null, null, application, module);
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="loginUser">User login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Returns action buttons with comments details</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <exception cref="Exception">Thrown when method call fails to get the data</exception>
        public MDMBO.Table GetActivityActionButtons(Int32 activityId, String loginUser, Boolean? checkAllowedUsersAndRoles, MDMCenterApplication application, MDMCenterModules module)
        {
            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(application, module);
            return MakeServiceCall("GetActivityActionButtons",
                                   "GetActionButtons",
                                   service => service.GetActionButtons(activityId,
                                                                       loginUser,
                                                                       checkAllowedUsersAndRoles,
                                                                       FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the assignment buttons for the requested assignment status
        /// </summary>
        /// <param name="activityId">Activity Id for which assignment buttons has to get</param>
        /// <param name="assignmentStatus">AssignmentStatus</param>
        /// <param name="toolBarButtonXML">Configuration XML for assignment buttons</param>
        /// <param name="loginUser">Logged in User</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetWorkflowAssignmentButtons(Int32 activityId, String assignmentStatus, String toolBarButtonXML, String loginUser, MDMBO.CallerContext callerContext)
        {
            return GetWorkflowAssignmentButtons(activityId, assignmentStatus, toolBarButtonXML, loginUser, null, callerContext);
        }

        /// <summary>
        /// Gets the assignment buttons for the requested assignment status
        /// </summary>
        /// <param name="activityId">Activity Id for which assignment buttons has to get</param>
        /// <param name="assignmentStatus">AssignmentStatus</param>
        /// <param name="toolBarButtonXML">Configuration XML for assignment buttons</param>
        /// <param name="loginUser">Logged in User</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetWorkflowAssignmentButtons(Int32 activityId, String assignmentStatus, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, MDMBO.CallerContext callerContext)
        {
            MDMBO.OperationResult operationResult = new MDMBO.OperationResult();
            return MakeServiceCall("GetWorkflowAssignmentButtons",
                                   "GetAssignmentButtons",
                                   service => service.GetAssignmentButtons(activityId,
                                                                           assignmentStatus,
                                                                           toolBarButtonXML,
                                                                           loginUser,
                                                                           checkAllowedUsersAndRoles,
                                                                           ref operationResult,
                                                                           FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        ///  Get the collection of Workflow Instances based on the Workflow Id
        /// </summary>
        /// <param name="workflowId">Indicates the Workflow Id</param>
        /// <param name="mdmObj">Indicates the MDM object</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Returns collections of Workflow instance</returns>
        public Collection<MDMBOW.WorkflowInstance> FindWorkflowInstance(Int32 workflowId, MDMBOW.WorkflowMDMObject mdmObj, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.FindWorkflowInstance", false);

            IWorkflowService workflowServiceClient = null;
            Collection<MDMBOW.WorkflowInstance> instancesInWorkflow = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'FindWorkflowInstance' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            instancesInWorkflow = workflowServiceClient.FindWorkflowInstance(workflowId, mdmObj, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    instancesInWorkflow = workflowServiceClient.FindWorkflowInstance(workflowId, mdmObj, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'FindWorkflowInstance' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.FindWorkflowInstance");
            }

            return instancesInWorkflow;
        }

        ///<summary>
        /// Get the collection of Workflow Instances based on the workflow activity short Name
        ///</summary>
        ///<param name="activityShortName">Workflow activity short name</param>
        ///<param name="mdmObj">Indicates the MDM object</param>
        ///<param name="operationResult">Object which collects results of the operation</param>
        ///<param name="callerContext">Caller Information details</param>
        ///<returns>Returns collections of Workflow instance</returns>
        public Collection<MDMBOW.WorkflowInstance> FindWorkflowInstance(String activityShortName, MDMBOW.WorkflowMDMObject mdmObj, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.FindWorkflowInstance", false);

            IWorkflowService workflowServiceClient = null;
            Collection<MDMBOW.WorkflowInstance> instancesInWorkflow = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'FindWorkflowInstance' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            instancesInWorkflow = workflowServiceClient.FindWorkflowInstance(activityShortName, mdmObj, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    instancesInWorkflow = workflowServiceClient.FindWorkflowInstance(activityShortName, mdmObj, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'FindWorkflowInstance' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.FindWorkflowInstance");
            }

            return instancesInWorkflow;
        }

        /// <summary>
        /// Processes Escalation data
        /// </summary>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <returns>Returns the records regarding Escalations</returns>
        public Collection<MDMBOW.Escalation> ProcessEscalation(ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ProcessEscalation", false);

            IWorkflowService workflowServiceClient = null;
            Collection<MDMBOW.Escalation> escalationData = new Collection<MDMBOW.Escalation>();

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ProcessEscalation' request message");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            escalationData = workflowServiceClient.ProcessEscalation(ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    escalationData = workflowServiceClient.ProcessEscalation(ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ProcessEscalation' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ProcessEscalation");
            }

            return escalationData;
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
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ProcessWorkflows", false);

            IWorkflowService workflowServiceClient = null;
            MDMBOW.WorkflowVersion workflowVersion = new MDMBOW.WorkflowVersion();

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ProcessWorkflows' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            workflowVersion = workflowServiceClient.ProcessWorkflows(workflows, loginUser, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    workflowVersion = workflowServiceClient.ProcessWorkflows(workflows, loginUser, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ProcessWorkflows' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ProcessWorkflows");
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
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ProcessActivities", false);

            Int32 output = 0;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ProcessActivities' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            output = workflowServiceClient.ProcessActivities(workflowActivities, loginUser, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    output = workflowServiceClient.ProcessActivities(workflowActivities, loginUser, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ProcessActivities' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ProcessActivities");
            }

            return output;
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
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.GetWorkflowViewDetails", false);

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'GetWorkflowViewDetails' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;
                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            workflowServiceClient.GetWorkflowViewDetails(workflowVersionID, instanceGuid, ref workflowVersion, ref trackedActivityCollection, ref operationResult, FillDiagnosticTraces(callerContext));
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    workflowServiceClient.GetWorkflowViewDetails(workflowVersionID, instanceGuid, ref workflowVersion, ref trackedActivityCollection, ref operationResult, FillDiagnosticTraces(callerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'GetWorkflowViewDetails' response message");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.GetWorkflowViewDetails");
            }
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="orgId">Id of the organization for which details are required</param>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="showEmptyItems">Flag to indicate whether to show empty items or not.</param>
        /// <param name="showItemsAssignedToOtherUsers">Flag to indicate whether to include data related to workflows assigned to other users</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Returns details in Table format</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <exception cref="Exception">Thrown when method call fails to get the data</exception>
        public MDMBO.Table GetWorkflowPanelDetails(Int32 orgId, Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, MDMCenterApplication application, MDMCenterModules module)
        {
            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(application, module);
            return MakeServiceCall("GetWorkflowPanelDetails",
                                   "GetWorkflowPanelDetails",
                                   service => service.GetWorkflowPanelDetails(orgId,
                                                                              catalogId,
                                                                              userId,
                                                                              showEmptyItems,
                                                                              showItemsAssignedToOtherUsers,
                                                                              FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="orgId">Id of the organization for which details are required</param>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Returns details in Table format</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <exception cref="Exception">Thrown when method call fails to get the data</exception>
        public MDMBO.Table GetWorkflowPanelDetails(Int32 orgId, Int32 catalogId, Int32 userId, MDMCenterApplication application, MDMCenterModules module)
        {
            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(application, module);
            return MakeServiceCall("GetWorkflowPanelDetails",
                                   "GetWorkflowPanelDetails",
                                   service => service.GetWorkflowPanelDetails(orgId,
                                                                              catalogId,
                                                                              userId,
                                                                              false,
                                                                              false,
                                                                              FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Get current Workflow Execution Details based on the Entity Id
        /// </summary>
        /// <example>
        /// <code>
        /// // Used Name Spaces
        ///    RS.MDM.BusinessObjects
        ///    RS.MDM.Core
        ///    RS.MDM.Interfaces
        ///    RS.MDM.Services
        ///
        /// // Assumption: Entity Id is hard coded in this sample
        ///    Int64 entityId = 1001; 
        /// // Prepare Caller Context
        ///    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);
        ///
        /// // Initialize Workflow Service
        ///    WorkflowService workflowService = new WorkflowService();
        ///
        /// // Make a WCF call to get Workflow execution details for the requested Entity
        ///    ITrackedActivityInfoCollection entityExecutionDetails = workflowService.GetCurrentWorkflowsExecutionDetails(entityId, callerContext);
        ///
        /// // Output: This will return requested Entity's current execution details in the format of 'TrackedActivityInfo Interface collection'
        /// // Sample Output based on Riverworks model
        /// //  <TrackedActivityInfoCollection><TrackedActivityInfo Id="-1" WorkflowVersionId="2" WorkflowDefinitionActivityId="6" ActivityShortName="a0b77d5c-23b4-4730-905e-701e93bb04c5" ActivityLongName="Content Enrichment" RuntimeInstanceId="2efa33e0-fa44-4bb3-8fbb-ee432e78003d" ExtendedProperties="" Status="Executing" ActingUserId="29" ActedUserId="0" ActivityComments="" Variables="" Arguments="" CustomData="" AssignedUsers="" AssignedRoles="" AssignementType="0" SortOrder="0" IsHumanActivity="False" WorkflowId="1" WorkflowName="New Product Introduction" IsExecuting="False" PerformedAction="" WorkflowVersionName="2.0" ActedUser="" EventDate="12/17/2013 7:05:01 PM" ActingUser="mdmuser" UserMailAddress="mdmuser@domain.com" PreviousActivityShortName="" LastActivityComments="" WorkflowComments=""><WorkflowMDMObjects></WorkflowMDMObjects></TrackedActivityInfo></TrackedActivityInfoCollection>
        ///
        /// </code>
        /// </example>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="iCallerContext">Caller Information details</param>
        /// <returns>Returns the ITrackedActivityInfoCollection object</returns>
        /// <exception cref="MDMOperationException">Thrown when entity id is not provided </exception>
        public ITrackedActivityInfoCollection GetCurrentWorkflowsExecutionDetails(Int64 entityId, ICallerContext iCallerContext)
        {
            return GetWorkflowExecutionDetails(entityId, String.Empty, iCallerContext, false);
        }

        /// <summary>
        /// Get the Workflow Execution Details based on the Entity Id and the Workflow Long Name
        /// </summary>
        /// <example>
        /// <code>
        /// // Used Name Spaces
        ///    RS.MDM.BusinessObjects
        ///    RS.MDM.Core
        ///    RS.MDM.Interfaces
        ///    RS.MDM.Services
        ///
        /// // Assumption: Entity Id is hard coded in this sample
        ///    Int64 entityId = 1001; 
        /// // Prepare Caller Context
        ///    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);
        ///
        /// // Initialize Workflow Service
        ///    WorkflowService workflowService = new WorkflowService();
        ///
        /// // Make a WCF call to get Workflow execution details for the requested Entity
        ///    ITrackedActivityInfoCollection entityExecutionDetails = workflowService.GetCurrentWorkflowsExecutionDetails(entityId, callerContext);
        ///
        /// // Output: This will return requested Entity's current execution details in the format of 'TrackedActivityInfo Interface collection'
        /// // Sample Output based on Riverworks model
        /// //  <TrackedActivityInfoCollection><TrackedActivityInfo Id="-1" WorkflowVersionId="2" WorkflowDefinitionActivityId="6" ActivityShortName="a0b77d5c-23b4-4730-905e-701e93bb04c5" ActivityLongName="Content Enrichment" RuntimeInstanceId="2efa33e0-fa44-4bb3-8fbb-ee432e78003d" ExtendedProperties="" Status="Executing" ActingUserId="29" ActedUserId="0" ActivityComments="" Variables="" Arguments="" CustomData="" AssignedUsers="" AssignedRoles="" AssignementType="0" SortOrder="0" IsHumanActivity="False" WorkflowId="1" WorkflowName="New Product Introduction" IsExecuting="False" PerformedAction="" WorkflowVersionName="2.0" ActedUser="" EventDate="12/17/2013 7:05:01 PM" ActingUser="mdmuser" UserMailAddress="mdmuser@domain.com" PreviousActivityShortName="" LastActivityComments="" WorkflowComments=""><WorkflowMDMObjects></WorkflowMDMObjects></TrackedActivityInfo></TrackedActivityInfoCollection>
        ///
        /// </code>
        /// </example>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="workflowName">Specifies long name of Workflow</param>
        /// <param name="iCallerContext">Caller Information details</param>
        /// <param name="getAll">Specifies whether all activity details or only current activity details to be retrieved."</param>
        /// <returns>Returns the ITrackedActivityInfoCollection object</returns>
        /// <exception cref="MDMOperationException">Thrown when entity id is not provided </exception>
        public ITrackedActivityInfoCollection GetWorkflowExecutionDetails(Int64 entityId, String workflowName, ICallerContext iCallerContext, Boolean getAll = true)
        {
            return MakeServiceCall("GetWorkflowExecutionDetails",
                                   "GetWorkflowExecutionDetails",
                                   service => service.GetWorkflowExecutionDetails(entityId,
                                                                                  workflowName,
                                                                                  FillDiagnosticTraces(iCallerContext),
                                                                                  getAll));
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Returns details in Table format</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <exception cref="Exception">Thrown when method call fails to get the data</exception>
        public MDMBO.Table GetWorkflowPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, MDMCenterApplication application, MDMCenterModules module)
        {
            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(application, module);
            return MakeServiceCall("GetWorkflowPanelDetails",
                                   "GetWorkflowPanelDetails",
                                   service => service.GetWorkflowPanelDetails(catalogId,
                                                                              userId,
                                                                              showEmptyItems,
                                                                              showItemsAssignedToOtherUsers,
                                                                              FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the details required for the WorkItems UI Panel
        /// </summary>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="showBusinessConditions">Include business conditions or not</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Returns details in Table format</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <exception cref="Exception">Thrown when method call fails to get the data</exception>
        public MDMBO.Table GetWorkItemsPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, Boolean showBusinessConditions, MDMCenterApplication application, MDMCenterModules module)
        {
            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(application, module);
            return MakeServiceCall("GetWorkflowPanelDetails",
                                   "GetWorkflowPanelDetails",
                                   service => service.GetWorkItemsPanelDetails(catalogId,
                                                                              userId,
                                                                              showEmptyItems,
                                                                              showItemsAssignedToOtherUsers,
                                                                              showBusinessConditions,
                                                                              FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets role based work items details including workflow activities and business conditions for given entity id
        /// </summary>
        /// <param name="entityId">Indicates entityId</param>
        /// <param name="callerContext">Indicates caller information</param>
        /// <returns>Returns a Table with all required data</returns>
        public Table GetWorkItemDetails(Int64 entityId, CallerContext callerContext)
        {
            return MakeServiceCall("GetWorkItemDetails",
                                   "GetWorkItemDetails",
                                   service => service.GetWorkItemDetails(entityId,  FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the value for the requested AppConfig key
        /// </summary>
        /// <param name="keyName">AppConfig key</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Value of the AppConfig key</returns>
        public String GetAppConfigValue(String keyName, ref MDMBO.OperationResult operationResult)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.GetAppConfigValue", false);

            String appConfigValue = String.Empty;
            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowService sends 'GetAppConfigValue' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity is WindowsIdentity)
                    {
                        WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                        using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                        {
                            appConfigValue = workflowServiceClient.GetAppConfigValue(keyName, ref operationResult);
                        }
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    appConfigValue = workflowServiceClient.GetAppConfigValue(keyName, ref operationResult);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowService receives 'GetAppConfigValue' response message.");
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
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowService.GetAppConfigValue");
            }

            return appConfigValue;
        }

        /// <summary>
        /// Get All Workflow Information available in the system
        /// </summary>
        /// <param name="callerContext">Indicates the name of the Application and Module that invoked the API</param>
        /// <returns>Workflow Information available in the system</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IWorkflowInfoCollection GetAllWorkflowInformation(ICallerContext callerContext)
        {
            return MakeServiceCall<IWorkflowInfoCollection>("GetAllWorkflowInformation", "GetAllWorkflowInformation", client => client.GetAllWorkflowInformation(FillDiagnosticTraces(callerContext)));
        }

        #region Entity Workflow service

        /// <summary>
        /// Starts the requested workflow by passing workflow information and collection of an entity Ids.
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of entity Ids for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and EntityId List" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By EntityId List" />
        /// </example>
        public IEntityOperationResultCollection StartWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId,
            String serviceType, Int32 serviceId, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResultCollection entityORC = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityORC = workflowServiceClient.StartWorkflow(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityORC = workflowServiceClient.StartWorkflow(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityORC == null)
                    entityORC = new MDMBO.EntityOperationResultCollection();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityORC.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResultCollection)entityORC;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information and entity id.
        /// </summary>
        /// <param name="entityId">Indicates the entity id for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and Entity Id" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By Entity Id" />
        /// </example>
        public IEntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId,
            ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResult entityOperationResult = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityOperationResult = workflowServiceClient.StartWorkflow(entityId, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityOperationResult = workflowServiceClient.StartWorkflow(entityId, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityOperationResult == null)
                    entityOperationResult = new MDMBO.EntityOperationResult();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityOperationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResult)entityOperationResult;
        }

        /// <summary>
        /// Start the requested workflow by passing workflow information and entity collection.
        /// </summary>
        /// <param name="iEntities">Indicates the entity collection for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and Entity Collection" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By Entity Collection" />
        /// </example>
        public IEntityOperationResultCollection StartWorkflow(IEntityCollection iEntities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResultCollection entityORC = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (iEntities == null)
                    throw new ArgumentException("iEntities are null");

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityORC = workflowServiceClient.StartWorkflow((MDMBO.EntityCollection)iEntities, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityORC = workflowServiceClient.StartWorkflow((MDMBO.EntityCollection)iEntities, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityORC == null)
                    entityORC = new MDMBO.EntityOperationResultCollection();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityORC.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResultCollection)entityORC;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information and entity.
        /// </summary>
        /// <param name="iEntity">Indicates the entity for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and Entity" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By Entity" />
        /// </example>
        public IEntityOperationResult StartWorkflow(IEntity iEntity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResult entityOperationResult = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (iEntity == null)
                    throw new ArgumentException("iEntity is null");

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityOperationResult = workflowServiceClient.StartWorkflow((MDMBO.Entity)iEntity, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityOperationResult = workflowServiceClient.StartWorkflow((MDMBO.Entity)iEntity, workflowName, workflowVersionId, serviceType, serviceId, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityOperationResult == null)
                    entityOperationResult = new MDMBO.EntityOperationResult();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityOperationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResult)entityOperationResult;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information, collection of an entity ids
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of entity Ids for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates the option to indicate whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and EntityId List With WorkflowInstanceRunOptions" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By EntityId List With WorkflowInstanceRunOptions" />
        /// </example>
        public IEntityOperationResultCollection StartWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResultCollection entityORC = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityORC = workflowServiceClient.StartWorkflow(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityORC = workflowServiceClient.StartWorkflow(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityORC == null)
                    entityORC = new MDMBO.EntityOperationResultCollection();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityORC.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResultCollection)entityORC;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information, entity id
        /// </summary>
        /// <param name="entityId">Indicates the entity Id for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates the option to indicate whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and Entity Id With WorkflowInstanceRunOptions" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By Entity Id With WorkflowInstanceRunOptions" />
        /// </example>
        public IEntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResult entityOperationResult = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityOperationResult = workflowServiceClient.StartWorkflow(entityId, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityOperationResult = workflowServiceClient.StartWorkflow(entityId, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityOperationResult == null)
                    entityOperationResult = new MDMBO.EntityOperationResult();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityOperationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResult)entityOperationResult;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information, collection of entity and workflowinstancerunoptions.
        /// </summary>
        /// <param name="iEntities">Indicates the Collection of entity for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates the option to indicate whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and Entity Collection With WorkflowInstanceRunOptions" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By Entity Collection With WorkflowInstanceRunOptions" />
        /// </example>
        public IEntityOperationResultCollection StartWorkflow(IEntityCollection iEntities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResultCollection entityORC = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (iEntities == null)
                    throw new ArgumentException("entities are null");

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityORC = workflowServiceClient.StartWorkflow((MDMBO.EntityCollection)iEntities, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityORC = workflowServiceClient.StartWorkflow((MDMBO.EntityCollection)iEntities, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityORC == null)
                    entityORC = new MDMBO.EntityOperationResultCollection();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityORC.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResultCollection)entityORC;
        }

        /// <summary>
        /// Starts the requested workflow by passing workflow information, entity and workflowinstancerunoptions.
        /// </summary>
        /// <param name="iEntity">Indicates the entity for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates the Name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates the Workflow version to invoke</param>
        /// <param name="serviceType">Indicates the Service type which triggered workflow</param>
        /// <param name="serviceId">Indicates the Service type Id which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates the option to indicate whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Operation result indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="StartWorkflow based on Workflow Details and Entity With WorkflowInstanceRunOptions" source="..\MDM.APISamples\Workflow\StartWorkflow\StartWorkflow.cs" region="StartWorkflow By Entity With WorkflowInstanceRunOptions" />
        /// </example>
        public IEntityOperationResult StartWorkflow(IEntity iEntity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, ICallerContext iCallerContext, String comments = "")
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.StartWorkflow", false);

            MDMBO.EntityOperationResult entityOperationResult = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (iEntity == null)
                    throw new ArgumentException("entity is null");

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'StartWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityOperationResult = workflowServiceClient.StartWorkflow((MDMBO.Entity)iEntity, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityOperationResult = workflowServiceClient.StartWorkflow((MDMBO.Entity)iEntity, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, FillDiagnosticTraces(iCallerContext), comments);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'StartWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityOperationResult == null)
                    entityOperationResult = new MDMBO.EntityOperationResult();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityOperationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.StartWorkflow");
            }

            return (IEntityOperationResult)entityOperationResult;
        }

        /// <summary>
        /// Resume workflow for the given entities. This will trigger business rule before and after resuming workflow.
        /// </summary>
        /// <param name="entityIdList">Indicates IDs of the entities for which workflow is resumed</param>
        /// <param name="workflowName">Indicates name of the workflow that is resumed</param>
        /// <param name="workflowVersionId">Indicates version of the workflow to be resumed</param>
        /// <param name="currentActivityName">Indicates name of activity from where workflow is resumed.</param>
        /// <param name="action">Indicates an action performed by user for an activity from where workflow is resumed</param>
        /// <param name="comments">Indicates comments entered by user while resuming workflow</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns OperationResultCollection indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Resume Workflow for specified entity IDs" source="..\MDM.APISamples\Workflow\ResumeWorkflow.cs" region="ResumeWorkflowForEntityIds" />
        /// </example>
        public IEntityOperationResultCollection ResumeWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId,
            String currentActivityName, String action, String comments, ICallerContext iCallerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ResumeWorkflow", false);

            MDMBO.EntityOperationResultCollection entityORC = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                //if(String.IsNullOrWhiteSpace(workflowName))
                //    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ResumeWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityORC = workflowServiceClient.ResumeWorkflow(entityIdList, workflowName, workflowVersionId, currentActivityName, action, comments, FillDiagnosticTraces(iCallerContext));
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityORC = workflowServiceClient.ResumeWorkflow(entityIdList, workflowName, workflowVersionId, currentActivityName, action, comments, FillDiagnosticTraces(iCallerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ResumeWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityORC == null)
                    entityORC = new MDMBO.EntityOperationResultCollection();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityORC.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ResumeWorkflow");
            }

            return (IEntityOperationResultCollection)entityORC;
        }

        /// <summary>
        /// Resume workflow for the given entities. This will trigger business rule before and after resuming workflow.
        /// </summary>
        /// <param name="iEntities">Indicates entities for which workflow is resumed</param>
        /// <param name="workflowName">Indicates name of the workflow that is resumed</param>
        /// <param name="workflowVersionId">Indicates version of the workflow to be resumed</param>
        /// <param name="currentActivityName">Indicates name of activity from where workflow is resumed.</param>
        /// <param name="action">Indicates an action performed by user for an activity from where workflow is resumed</param>
        /// <param name="comments">Indicates comments entered by user while resuming workflow</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns OperationResultCollection indicating business rule and workflow invocation result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Resume Workflow for specified entity collection" source="..\MDM.APISamples\Workflow\ResumeWorkflow.cs" region="ResumeWorkflowForEntityCollection" />
        /// <code language="c#" title="Get Multiple Entities"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetMultipleEntities"/>
        /// </example>
        public IEntityOperationResultCollection ResumeWorkflow(IEntityCollection iEntities, String workflowName, Int32 workflowVersionId,
            String currentActivityName, String action, String comments, ICallerContext iCallerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient.ResumeWorkflow", false);

            MDMBO.EntityOperationResultCollection entityORC = null;

            IWorkflowService workflowServiceClient = null;

            try
            {
                base.ValidateContext();

                workflowServiceClient = GetClient();

                if (iEntities == null)
                    throw new ArgumentException("iEntities is null");

                if (String.IsNullOrWhiteSpace(workflowName))
                    throw new ArgumentException("workflowName is null");

                if (iCallerContext == null)
                    throw new ArgumentException("iCallerContext is null");

                // Start new wf
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends 'ResumeWorkflow' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                    using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                    {
                        entityORC = workflowServiceClient.ResumeWorkflow((MDMBO.EntityCollection)iEntities, workflowName, workflowVersionId, currentActivityName, action, comments, FillDiagnosticTraces(iCallerContext));
                    }
                }
                else if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    entityORC = workflowServiceClient.ResumeWorkflow((MDMBO.EntityCollection)iEntities, workflowName, workflowVersionId, currentActivityName, action, comments, FillDiagnosticTraces(iCallerContext));
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives 'ResumeWorkflow' response message.");
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                if (entityORC == null)
                    entityORC = new MDMBO.EntityOperationResultCollection();

                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                entityORC.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                //base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient.ResumeWorkflow");
            }

            return (IEntityOperationResultCollection)entityORC;
        }

        #region Change Assignment

        /// <summary>
        /// Change the workflow activity's ownership assignment from one user to another user.
        /// </summary>
        /// <param name="entityIds">Indicates the list of Entity Ids</param>
        /// <param name="currentActivityName">Indicates the current activity name</param>
        /// <param name="newlyAssignedUser">Indicates the Newly assigned security user</param>
        /// <param name="assignmentAction">Indicates the Assignment Action</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <returns>IEntityOperationResultCollection</returns>
        public IEntityOperationResultCollection ChangeAssignment(Collection<Int64> entityIds, String currentActivityName, MDMBO.SecurityUser newlyAssignedUser, String assignmentAction, MDMBO.CallerContext callerContext)
        {
            return MakeServiceCall("ChangeAssignment",
                                   "ChangeAssignment",
                                   service => service.ChangeAssignment(entityIds,
                                                                       currentActivityName,
                                                                       newlyAssignedUser,
                                                                       assignmentAction,
                                                                       FillDiagnosticTraces(callerContext)));
        }

        #endregion Change Assignment

        #endregion Entity workflow service

        #region Workflow Activity Detail

        /// <summary>
        /// Gets the details of the user who performs the workflow activity based on the requested inputs. 
        /// If more than one activity is found, the method returns the user details of the first activity.
        /// </summary>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <param name="workflowName">Indicates the workflow short name or long name</param>
        /// <param name="activityName">Indicates the activity short name or long name</param>
        /// <param name="callerContext">Indicates the caller context details</param>
        /// <example> 
        /// <code language="c#" title="Get the user details who performed the workflow activity" source="..\MDM.APISamples\Workflow\GetWorkflowActivity.cs" region="GetWorkflowActivityPerformedUser"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the security user details</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ISecurityUser GetWorkflowActivityPerformedUser(Int64 entityId, String workflowName, String activityName, ICallerContext callerContext)
        {
            return this.MakeServiceCall<ISecurityUser>("GetWorkflowActivityPerformedUser", "GetWorkflowActivityPerformedUser",
               client => client.GetWorkflowActivityPerformedUser(entityId, workflowName, activityName, FillDiagnosticTraces(callerContext)));
        }

        #endregion #region Workflow Activity Detail

        #region Workflow Escalation Service

        /// <summary>
        /// Gets the workflow escalation details for the requested entities based on the escalation context
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get workflow escalation details based on workflow name and activity name for an entity"  source="..\MDM.APISamples\Workflow\Escalation\GetWorkflowEscalationDetailsSamples.cs" region="Get workflow escalation details based on workflow name and activity name for an entity"/>
        /// <code language="c#" title="Get workflow escalation details based on the workflow name for an entity"  source="..\MDM.APISamples\Workflow\Escalation\GetWorkflowEscalationDetailsSamples.cs" region="Get workflow escalation details based on the workflow name for an entity"/>
        /// <code language="c#" title="Get the entity from EntityUtility" source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity" />
        /// </example>
        /// <param name="escalationContext">Indicates the workflow escalation context</param>
        /// <param name="callerContext">Indicates the callerContext which contains the MDMCenter application and module</param>
        /// <returns>Returns the workflow escalation details as collection </returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IWorkflowEscalationDataCollection GetWorkflowEscalationDetails(IWorkflowEscalationContext escalationContext, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IWorkflowEscalationDataCollection>("GetWorkflowEscalationDetails", "GetWorkflowEscalationDetails",
              client => client.GetWorkflowEscalationDetails(escalationContext as MDMBOW.WorkflowEscalationContext, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Sends an escalation email to user based on the specified escalation context and email context
        /// </summary>
        /// <example>
        /// <code language="c#" title="Send email with workflow escalation details"  source="..\MDM.APISamples\Workflow\Escalation\GetWorkflowEscalationDetailsSamples.cs" region="Send email with workflow escalation details"/>
        /// <code language="c#" title="Get the entity from EntityUtility" source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity" />
        /// </example>
        /// <param name="escalationContext">Indicates the workflow escalation context</param>
        /// <param name="emailContext">Indicates the email context</param>
        /// <param name="includeAssignedUserAsRecipient">Indicates to include assigned user in "To" address list in the email or not
        /// If the value is "true", then along with the email context "To" address list assigned user's in the email will be added
        /// If no need to notify the assigned user, then set the value as "false"</param>
        /// <param name="callerContext">Indicates the caller context, which contains the MDMCenter application and module</param>
        /// <returns>Results whether the email has been sent to an recipient or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResultCollection SendMailWithWorkflowEscalationDetails(IWorkflowEscalationContext escalationContext, IEmailContext emailContext, Boolean includeAssignedUserAsRecipient, ICallerContext callerContext)
        {
            return this.MakeServiceCall<MDMBO.OperationResultCollection>("SendMailWithWorkflowEscalationDetails", "SendMailWithWorkflowEscalationDetails",
              client => client.SendMailWithWorkflowEscalationDetails(escalationContext as MDMBOW.WorkflowEscalationContext, emailContext as MDMBO.EmailContext, includeAssignedUserAsRecipient, FillDiagnosticTraces(callerContext)), MDMTraceSource.AdvancedWorkflow);
        }

        #endregion

        #endregion

        #region Private methods

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
        /// 
        /// </summary>
        /// <returns></returns>
        private IWorkflowService GetClient()
        {
            IWorkflowService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IWorkflowService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                WorkflowServiceProxy workflowServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    workflowServiceProxy = new WorkflowServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    workflowServiceProxy = new WorkflowServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    workflowServiceProxy = new WorkflowServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    workflowServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    workflowServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    workflowServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = workflowServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IWorkflowService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(WorkflowServiceProxy)))
            {
                WorkflowServiceProxy serviceClient = (WorkflowServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }

        /// <summary>
        /// Makes the workflow service client call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<IWorkflowService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.AdvancedWorkflow)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            //Start trace activity
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("WorkflowServiceClient." + clientMethodName, traceSource, false);
            }

            TResult result = default(TResult);
            IWorkflowService workflowServiceClient = null;

            try
            {
                workflowServiceClient = GetClient();

                ValidateContext();

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient sends '" + serverMethodName + "' request message.", traceSource);
                }

                result = Impersonate(() => call(workflowServiceClient));

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "WorkflowServiceClient receives '" + serverMethodName + "' response message.", traceSource);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(workflowServiceClient);

                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("WorkflowServiceClient." + clientMethodName, traceSource);
                }
            }

            return result;
        }


        #endregion
    }
}
