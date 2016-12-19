using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.EntityManager.Business;
    using MDM.EntityWorkflowManager.Business;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDM.Workflow.Designer.Business;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.Workflow.TrackingManager.Business;
    using MDM.WorkflowRuntimeEngine;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// The class which implements Workflow Service Contract
    /// </summary>
    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class WorkflowService : MDMWCFBase, IWorkflowService
    {
        #region Constructors

        public WorkflowService()
            : base(true)
        {

        }

        public WorkflowService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region IWorkflowService Members

        #region Start Workflow

        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context which will go as part of the instance lifetime</param>
        /// <param name="serviceType">The type of the service which is invoking the workflow</param>
        /// <param name="serviceId">The unique identification of the service</param>
        /// <param name="currentUserName">The user name who is requesting for the start</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>The success count</returns>
        public Int32 StartWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 invokeSuccessCount = 0;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                invokeSuccessCount = workflowRuntimeBL.StartWorkflow(workflowDataContext, serviceType, serviceId, currentUserName, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return invokeSuccessCount;
        }

        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context which will go as part of the instance lifetime</param>
        /// <param name="serviceType">The type of the service which is invoking the workflow</param>
        /// <param name="serviceId">The unique identification of the service</param>
        /// <param name="currentUserName">The user name who is requesting for the start</param>
        /// <param name="workflowInstanceRunOption">The run option which describes the way the workflow needs to be invoked</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>The success count</returns>
        public Int32 StartWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 invokeSuccessCount = 0;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", MDMTraceSource.AdvancedWorkflow, false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.", MDMTraceSource.AdvancedWorkflow);

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                invokeSuccessCount = workflowRuntimeBL.StartWorkflow(workflowDataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.", MDMTraceSource.AdvancedWorkflow);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow", MDMTraceSource.AdvancedWorkflow);
            }

            return invokeSuccessCount;
        }

        #endregion

        #region Resume Workflow

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="commaSeparatedRuntimeInstanceIds">Instance Ids which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(String commaSeparatedRuntimeInstanceIds, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeWorkflow' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                resumeSuccessCount = workflowRuntimeBL.ResumeWorkflow(commaSeparatedRuntimeInstanceIds, actionContext, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeWorkflow");
            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="workflowInstance">The Instance object which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowInstance workflowInstance, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeWorkflow' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                resumeSuccessCount = workflowRuntimeBL.ResumeWorkflow(workflowInstance, actionContext, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeWorkflow");
            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(Collection<MDMBOW.WorkflowInstance> instanceCollection, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeWorkflow' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                resumeSuccessCount = workflowRuntimeBL.ResumeWorkflow(instanceCollection, actionContext, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeWorkflow");
            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="mdmObj">The MDM Object for which workflow needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowMDMObject mdmObj, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeWorkflow' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                resumeSuccessCount = workflowRuntimeBL.ResumeWorkflow(mdmObj, actionContext, ref operationResult, callerContext);

                 if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeWorkflow");
            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context holding MDM Objects for which workflow needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeWorkflow' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                resumeSuccessCount = workflowRuntimeBL.ResumeWorkflow(workflowDataContext, actionContext, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeWorkflow");
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
        /// <returns>The success count</returns>
        public Int32 ResumeAbortedWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, String instanceStatus, String loginUser, String programName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeAbortedWorkflowInstances", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeAbortedWorkflowInstances' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                resumeSuccessCount = workflowRuntimeBL.ResumeAbortedWorkflowInstances(instanceCollection, instanceStatus, loginUser, programName, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeAbortedWorkflowInstances' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeAbortedWorkflowInstances");
            return resumeSuccessCount;
        }

        #endregion

        #region Terminate Workflow

        /// <summary>
        /// Terminates requested workflow instances
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be terminated</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 TerminateWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 terminateSuccessCount = 0;

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.TerminateWorkflowInstances", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'TerminateWorkflowInstances' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                terminateSuccessCount = workflowRuntimeBL.TerminateWorkflowInstances(instanceCollection, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'TerminateWorkflowInstances' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.TerminateWorkflowInstances");
            return terminateSuccessCount;
        }

        #endregion

        #region Promote Workflow

        /// <summary>
        /// Promotes requested workflow instances
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be promoted</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 PromoteWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 promoteSuccessCount = 0;

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.PromoteWorkflowInstances", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'PromoteWorkflowInstances' request message.");

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                promoteSuccessCount = workflowRuntimeBL.PromoteWorkflowInstances(instanceCollection, ref operationResult, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'PromoteWorkflowInstances' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.PromoteWorkflowInstances");
            return promoteSuccessCount;
        }

        #endregion

        #region Find Workflow

        public Collection<MDMBOW.WorkflowInstance> FindWorkflowInstance(Int32 workflowId, MDMBOW.WorkflowMDMObject mdmObj, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstancesForMDMObj = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                workflowInstancesForMDMObj = workflowInstanceBL.GetByMDMObjectInWorkflow(workflowId, mdmObj, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return workflowInstancesForMDMObj;
        }

        public Collection<MDMBOW.WorkflowInstance> FindWorkflowInstance(String activityName, MDMBOW.WorkflowMDMObject mdmObj, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstancesForMDMObj = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                workflowInstancesForMDMObj = workflowInstanceBL.GetByMDMObject(mdmObj, activityName, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return workflowInstancesForMDMObj;
        }

        #endregion

        #region UI Methods

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

        /// <summary>
        /// Gets the summary of the instances as per the filter criteria
        /// </summary>
        /// <param name="workflowType"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="workflowStatus"></param>
        /// <param name="activityShortName"></param>
        /// <param name="roleIds"></param>
        /// <param name="userIds"></param>
        /// <param name="instanceId"></param>
        /// <param name="mdmObjectIds"></param>
        /// <param name="hasEscalation"></param>
        /// <param name="returnSize"></param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Collection of workflow instances</returns>
        public Collection<MDMBOW.WorkflowInstance> GetInstanceSummary(String workflowType, Int32 workflowId, Int32 workflowVersionId, String workflowStatus, String activityShortName, String roleIds, String userIds, String instanceId, String mdmObjectIds, Boolean? hasEscalation, Int32 returnSize, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstanceCollection = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                workflowInstanceCollection = workflowInstanceBL.GetInstanceSummary(workflowType, workflowId, workflowVersionId, workflowStatus, activityShortName, roleIds, userIds, instanceId, mdmObjectIds, hasEscalation, returnSize, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return workflowInstanceCollection;
        }

        /// <summary>
        /// Gets the details of the requested instance
        /// </summary>
        /// <param name="workflowType"></param>
        /// <param name="instanceId"></param>
        /// <param name="runningActivityCollection">Collection of running activities</param>
        /// <param name="mdmObjectCollection">Collection of MDM objects participating in the instance</param>
        /// <param name="escalationCollection">Collection of escalations happened for this instance</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        public void GetInstanceDetails(String workflowType, String instanceId, ref Collection<MDMBOW.WorkflowActivity> runningActivityCollection, ref Collection<MDMBOW.WorkflowMDMObject> mdmObjectCollection, ref Collection<MDMBOW.Escalation> escalationCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                workflowInstanceBL.GetInstanceDetails(workflowType, instanceId, ref runningActivityCollection, ref mdmObjectCollection, ref escalationCollection, callerContext);
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
        /// Gets the statistics
        /// </summary>
        /// <param name="workflowType"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowVersionId"></param>
        /// <returns>Statistics in the format of string</returns>
        /// <param name="operationResult">Object which collects results of the operation</param>
        public String GetWorkflowStatistics(String workflowType, Int32 workflowId, Int32 workflowVersionId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            String strStatistics = String.Empty;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                strStatistics = workflowInstanceBL.GetWorkflowStatistics(workflowType, workflowId, workflowVersionId, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return strStatistics;
        }

        /// <summary>
        /// Updates the instance status and instance's acting user.
        /// </summary>
        /// <param name="instanceGUIDs">Comma separated GUIDs of instances to be updated</param>
        /// <param name="mdmObjectIDs">Comma separated MDM object IDs</param>
        /// <param name="mdmObjectType">Type of the MDM object</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="activityShortName">Short Name of the activity</param>
        /// <param name="instanceStatus">Status which is going to be updated</param>
        /// <param name="loginUser">Login User which is going to be updated as Acting User</param>
        /// <param name="programName">Name of the module which is updating</param>
        /// <param name="operationResult">Object which collects result of operation</param>
        /// <returns>Result of the operation</returns>
        public Boolean UpdateWorkflowInstances(String instanceGUIDs, String mdmObjectIDs, String mdmObjectType, Int32 workflowID, String activityShortName, String instanceStatus, String loginUser, String programName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                result = workflowInstanceBL.UpdateWorkflowInstances(instanceGUIDs, mdmObjectIDs, mdmObjectType, workflowID, activityShortName, instanceStatus, loginUser, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="toolBarButtonXML">Configuration XML for action buttons</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="loginUser">User login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="callerContext">Caller context having name of application and module performing this action</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetActionButtons(Int32 activityId, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            return MakeBusinessLogicCall<WorkflowInstanceBL, DataTable>(
                "GetActionButtons",
                bl => bl.GetActionButtons(activityId, toolBarButtonXML, loginUser, checkAllowedUsersAndRoles, callerContext));
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="loginUser">User login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="callerContext">Caller context having name of application and module performing this action</param>
        /// <returns>Returns action buttons with comments details</returns>
        public MDMBO.Table GetActionButtons(Int32 activityId, String loginUser, Boolean? checkAllowedUsersAndRoles, MDMBO.CallerContext callerContext)
        {
            MDMBO.Table actionButtonsDetails = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                actionButtonsDetails = workflowInstanceBL.GetActionButtons(activityId, loginUser, checkAllowedUsersAndRoles, callerContext);
            }
            catch (Exception ex)
            {
                //TODO:: Wrap exception into MDM exception..
                this.LogException(ex);
            }

            return actionButtonsDetails;
        }

        /// <summary>
        /// Gets the assignment buttons for the requested assignment status
        /// </summary>
        /// <param name="activityId">Activity Id for which assignment buttons has to get</param>
        /// <param name="assignmentStatus">AssignmentStatus</param>
        /// <param name="toolBarButtonXML">Configuration XML for assignment buttons</param>
        /// <param name="loginUser">Logged in User</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetAssignmentButtons(Int32 activityId, String assignmentStatus, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            DataTable toolBarTable = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                toolBarTable = workflowInstanceBL.GetAssignmentButtons(activityId, assignmentStatus, toolBarButtonXML, loginUser, checkAllowedUsersAndRoles, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return toolBarTable;
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="orgId">Id of the organization for which details are required</param>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="getSTFWorkflows">Get STF workflows data</param>
        /// <param name="getWWFWorkflows">Get WWF workflows data</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="callerContext">Caller context having name of application and module performing this action</param>
        /// <returns>Returns details in Table format</returns>
        public MDMBO.Table GetWorkflowPanelDetails(Int32 orgId, Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, MDMBO.CallerContext callerContext)
        {
            MDMBO.Table workflowPanelDetailsTable = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();

                //Added orgID in the parameters of GetWorkflowPanelDetails() for AVS
                workflowPanelDetailsTable = workflowInstanceBL.GetWorkflowPanelDetails(orgId, catalogId, userId, showEmptyItems, showItemsAssignedToOtherUsers, callerContext);
            }
            catch (Exception ex)
            {
                //TODO:: Wrap exception into MDM exception..
                this.LogException(ex);
            }

            return workflowPanelDetailsTable;
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="getSTFWorkflows">Get STF workflows data</param>
        /// <param name="getWWFWorkflows">Get WWF workflows data</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="callerContext">Caller context having name of application and module performing this action</param>
        /// <returns>Returns details in Table format</returns>
        public MDMBO.Table GetWorkflowPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, MDMBO.CallerContext callerContext)
        {
            MDMBO.Table workflowPanelDetailsTable = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                workflowPanelDetailsTable = workflowInstanceBL.GetWorkflowPanelDetails(catalogId, userId, showEmptyItems, showItemsAssignedToOtherUsers, callerContext);
            }
            catch (Exception ex)
            {
                //TODO:: Wrap exception into MDM exception..
                this.LogException(ex);
            }

            return workflowPanelDetailsTable;
        }

        /// <summary>
        /// Gets the details required for the WorkItems UI Panel
        /// </summary>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="getSTFWorkflows">Get STF workflows data</param>
        /// <param name="getWWFWorkflows">Get WWF workflows data</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="showBusinessCondition"></param>
        /// <param name="callerContext">Caller context having name of application and module performing this action</param>
        /// <returns>Returns details in Table format</returns>
        public MDMBO.Table GetWorkItemsPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, Boolean showBusinessCondition, MDMBO.CallerContext callerContext)
        {
            MDMBO.Table workItemPanelDetailsTable = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                workItemPanelDetailsTable = workflowInstanceBL.GetWorkItemsPanelDetails(catalogId, userId, showEmptyItems, showItemsAssignedToOtherUsers, showBusinessCondition, callerContext);
            }
            catch (Exception ex)
            {
                //TODO:: Wrap exception into MDM exception..
                this.LogException(ex);
            }

            return workItemPanelDetailsTable;
        }

        /// <summary>
        /// Gets role based work items details including workflow activities and business conditions for given entity id
        /// </summary>
        /// <param name="entityId">Indicates entityId</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns a Table with all required data</returns>
        public Table GetWorkItemDetails(Int64 entityId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<WorkflowInstanceBL, Table>("GetWorkItemDetails", businessLogic =>
                   businessLogic.GetWorkItemDetails(entityId, callerContext));
        }

        #endregion

        #region Escalation Service

        /// <summary>
        /// Processes Escalation data
        /// </summary>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Returns the records regarding Escalations</returns>
        public Collection<MDMBOW.Escalation> ProcessEscalation(ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Collection<MDMBOW.Escalation> escalationData = new Collection<MDMBOW.Escalation>();

            try
            {
                EscalationBL escalationBL = new EscalationBL();
                escalationData = escalationBL.Process(callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return escalationData;
        }

        /// <summary>
        /// Get the workflow elapsed time details for the requested entities based on the escalation context
        /// </summary>
        /// <param name="escalationContext">Indicates the workflow escalation context</param>
        /// <param name="callerContext">Indicates the callerContext which contains the MDMCenter application and module</param>
        /// <returns>Returns the workflow escalation details as collection. </returns>
        public MDMBOW.WorkflowEscalationDataCollection GetWorkflowEscalationDetails(MDMBOW.WorkflowEscalationContext escalationContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EscalationBL, MDMBOW.WorkflowEscalationDataCollection>("GetWorkflowEscalationDetails", businessLogic =>
                   businessLogic.GetWorkflowEscalationDetails(escalationContext, callerContext));
        }

        /// <summary>
        /// Send an escalation mail to user based on the requested escalation context and email context
        /// </summary>
        /// <param name="escalationContext">Indicates the workflow escalation context</param>
        /// <param name="emailContext">Indicates the email context</param>
        /// <param name="includeAssignedUserAsRecipient">Whether to include assigned user in To Address list in the mail or not.
        /// If value is true then along with the email context's To address list assigned user's email will be added else not.
        /// If no need to notify the assigned user then set the value as false.</param>
        /// <param name="callerContext">Indicates the callerContext which contains the MDMCenter application and module</param>
        /// <returns>Returns operation result based on the result</returns>
        public OperationResultCollection SendMailWithWorkflowEscalationDetails(MDMBOW.WorkflowEscalationContext escalationContext, EmailContext emailContext, Boolean includeAssignedUserAsRecipient, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EscalationBL, OperationResultCollection>("SendMailWithWorkflowEscalationDetails", businessLogic =>
                  businessLogic.SendMailWithWorkflowEscalationDetails(escalationContext, emailContext, includeAssignedUserAsRecipient, callerContext));
        }

        #endregion

        #region Designer Methods

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

        #endregion

        #region Execution View Details Methods

        /// <summary>
        /// Get the Workflow Execution Details based on the Entity Id and the Workflow Long Name
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="workflowName">Specifies long name of Workflow</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <param name="getAll">Specifies whether all activity details or only current activity details are to be retrieved."
        /// <returns>Returns the TrackedActivityInfoCollection object</returns>
        /// <exception cref="MDMOperationException">Thrown when entity id is not provided </exception>
        public MDMBOW.TrackedActivityInfoCollection GetWorkflowExecutionDetails(Int64 entityId, String workflowName, MDMBO.CallerContext callerContext, Boolean getAll = true)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WCFServices.GetWorkflowExecutionDetails", false);

            MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection = null;

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WCFServices receives 'GetWorkflowExecutionDetails' request message.");

                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                trackedActivityInfoCollection = workflowInstanceBL.GetWorkflowExecutionDetails(entityId, workflowName, callerContext, getAll);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WCFServices sends 'GetWorkflowExecutionDetails' response message.");

            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WCFServices.GetWorkflowExecutionDetails");
            }

            return trackedActivityInfoCollection;
        }

        public Collection<DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds,
                                                                Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds,
                                                                Int32 localeId, Int64 countFrom, Int64 countTo, CallerContext callerContext)
        {
            return GetWorkflowDoneReport(userId, userParticipation, catalogIds, entityTypeIds, workflowNames, currentWorkflowActivity, attributeIds, localeId, countFrom, countTo, null, callerContext);
        }

        public Collection<DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds,
                                                                Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds,
                                                                Int32 localeId, Int64 countFrom, Int64 countTo, String attributesDataSource, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WCFServices.GetWorkflowDoneReport", false);

            Collection<DoneReportItem> collection = null;

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WCFServices receives 'GetWorkflowDoneReport' request message.");

                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                Int64 totalCount = 0;
                collection = workflowInstanceBL.GetWorkflowDoneReport(userId, userParticipation, catalogIds, entityTypeIds, workflowNames, currentWorkflowActivity, attributeIds,
                                                                localeId, countFrom, countTo, attributesDataSource, out totalCount, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WCFServices sends 'GetWorkflowDoneReport' response message.");

            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WCFServices.GetWorkflowDoneReport");
            }
            return collection;
        }

        #endregion Execution View Details Methods

        #region Entity workflow methods

        public EntityOperationResultCollection StartWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            EntityOperationResultCollection entityORC = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityORC = entityWorkflowBL.StartWorkflow(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityORC;
        }

        public EntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            EntityOperationResult entityOperationResult = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityOperationResult = entityWorkflowBL.StartWorkflow(entityId, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityOperationResult;
        }

        public EntityOperationResultCollection StartWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            EntityOperationResultCollection entityORC = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityORC = entityWorkflowBL.StartWorkflow(entities, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityORC;
        }

        public EntityOperationResult StartWorkflow(Entity entity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            EntityOperationResult entityOperationResult = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityOperationResult = entityWorkflowBL.StartWorkflow(entity, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityOperationResult;
        }

        public EntityOperationResultCollection StartWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            EntityOperationResultCollection entityORC = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityORC = entityWorkflowBL.StartWorkflow(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityORC;
        }

        public EntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            EntityOperationResult entityOperationResult = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityOperationResult = entityWorkflowBL.StartWorkflow(entityId, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityOperationResult;
        }

        public EntityOperationResultCollection StartWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            EntityOperationResultCollection entityORC = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityORC = entityWorkflowBL.StartWorkflow(entities, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityORC;
        }

        public EntityOperationResult StartWorkflow(Entity entity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            EntityOperationResult entityOperationResult = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.StartWorkflow", false);

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'StartWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityOperationResult = entityWorkflowBL.StartWorkflow(entity, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'StartWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.StartWorkflow");
            return entityOperationResult;
        }

        public EntityOperationResultCollection ResumeWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String currentActivityName, String action, String comments, CallerContext callerContext)
        {
            EntityOperationResultCollection entityORC = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeWorkflow", false);

            try
            {
                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityORC = entityWorkflowBL.ResumeWorkflow(entityIdList, workflowName, workflowVersionId, currentActivityName, action, comments, callerContext);

                 if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeWorkflow");
            return entityORC;
        }

        public EntityOperationResultCollection ResumeWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String currentActivityName, String action, String comments, CallerContext callerContext)
        {
            EntityOperationResultCollection entityORC = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ResumeWorkflow", false);

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ResumeWorkflow' request message.");

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityORC = entityWorkflowBL.ResumeWorkflow(entities, workflowName, workflowVersionId, currentActivityName, action, comments, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ResumeWorkflow' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("WorkflowService.ResumeWorkflow");
            return entityORC;
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
        /// <returns>EntityOperationResultCollection</returns>
        public EntityOperationResultCollection ChangeAssignment(Collection<Int64> entityIds, String currentActivityName, SecurityUser newlyAssignedUser, String assignmentAction, CallerContext callerContext)
        {
            EntityOperationResultCollection EOR = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowService.ChangeAssignment", MDMTraceSource.AdvancedWorkflow, false);

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives 'ChangeAssignment' request message.", MDMTraceSource.AdvancedWorkflow);

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                EOR = entityWorkflowBL.ChangeAssignment(entityIds, currentActivityName, newlyAssignedUser, assignmentAction, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService sends 'ChangeAssignment' response message.", MDMTraceSource.AdvancedWorkflow);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("EntityWorkflowBL.ChangeAssignment", MDMTraceSource.AdvancedWorkflow);
            }

            return EOR;
        }

        #endregion Change Assignment

        #endregion Entity workflow methods

        #region Workflow Details

        /// <summary>
        /// Gets security user details who performs the workflow activity based on the requested inputs. 
        /// If more than one activities found for the result, it returns the first activity's user details.
        /// </summary>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <param name="workflowName">Indicates the workflow short name or long name</param>
        /// <param name="activityName">Indicates the activity short name or long name</param>
        /// <param name="callerContext">Indicates the caller context details</param>
        /// <returns>Returns the security user object</returns>
        public SecurityUser GetWorkflowActivityPerformedUser(Int64 entityId, String workflowName, String activityName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<WorkflowInstanceBL, SecurityUser>("GetWorkflowActivityPerformedUser", businessLogic =>
                   businessLogic.GetWorkflowActivityPerformedUser(entityId, workflowName, activityName, callerContext));
        }

        #endregion 

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
        /// Get All Workflow Information available in the system
        /// </summary>
        /// <param name="callerContext">Indicates the name of the Application and Module that invoked the API</param>
        /// <returns>Workflow Information available in the system</returns>
        public WorkflowInfoCollection GetAllWorkflowInformation(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<WorkflowBL, WorkflowInfoCollection>("GetAllWorkflowInformation", businessLogic => businessLogic.GetAllWorkflowInformation(callerContext));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Makes calls of Workflow Service Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(String methodName, Func<TBusinessLogic, TResult> call) where TBusinessLogic : BusinessLogicBase, new()
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.InitializeTraceSource();
                MDMTraceHelper.StartTraceActivity("WorkflowService." + methodName, false);
            }

            TResult operationResult;

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives" + methodName + " request message.");
                }

                operationResult = call(new TBusinessLogic());

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "WorkflowService receives" + methodName + " response message.");
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("WorkflowService." + methodName);
                }
            }

            return operationResult;
        }


        #endregion
    }
}
