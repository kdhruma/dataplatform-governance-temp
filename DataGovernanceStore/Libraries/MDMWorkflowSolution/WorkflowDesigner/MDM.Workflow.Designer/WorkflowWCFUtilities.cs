using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Security.Principal;

namespace MDM.Workflow.Designer
{
    using MDM.BusinessObjects;
    using MDM.Services;
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// WCF utility class for Workflow. Contains methods to make WCF call for WF services.
    /// </summary>
    public class WorkflowWCFUtilities
    {
        /// <summary>
        /// Gets the value for the requested AppConfig key
        /// </summary>
        /// <param name="keyName">AppConfig key</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Value of the AppConfig key</returns>
        public String GetAppConfigValue(String keyName, ref OperationResult operationResult)
        {
            String result = String.Empty;

            WorkflowDesignerService workflowDesignerService = new WorkflowDesignerService(App.WCFBinding, App.WCFRemoteAddress);
            result = workflowDesignerService.GetAppConfigValue(keyName, ref operationResult);

            return result;
        }

        /// <summary>
        /// Gets the details required for Execution/Definition view
        /// </summary>
        /// <param name="workflowVersionID"> Id of the workflow version</param>
        /// <param name="instanceGuid">GUID of the Instance</param>
        /// <param name="workflowVersion">Workflow version object. Ref param.</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <param name="trackedActivityCollection">Tracked activity collection. Ref param.</param>
        public void GetWorkflowViewDetails(Int32 workflowVersionID, String instanceGuid, ref WorkflowVersion workflowVersion, ref Collection<TrackedActivityInfo> trackedActivityCollection, ref OperationResult operationResult, CallerContext callerContext)
        {
            WorkflowDesignerService workflowDesignerService = new WorkflowDesignerService(App.WCFBinding, App.WCFRemoteAddress);
            workflowDesignerService.GetWorkflowViewDetails(workflowVersionID, instanceGuid, ref workflowVersion, ref trackedActivityCollection, ref operationResult, callerContext);
        }

        /// <summary>
        /// Process workflows, based on Action
        /// </summary>
        /// <param name="workflows">workflows to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Returns the newly created workflow version</returns>
        public WorkflowVersion ProcessWorkflows(Collection<Workflow> workflows, String loginUser, ref OperationResult operationResult, CallerContext callerContext)
        {
            WorkflowVersion version = null;

            WorkflowDesignerService workflowDesignerService = new WorkflowDesignerService(App.WCFBinding, App.WCFRemoteAddress);
            version = workflowDesignerService.ProcessWorkflows(workflows, loginUser, ref operationResult, callerContext);

            return version;
        }

        /// <summary>
        /// Process workflow activities, based on Action
        /// </summary>
        /// <param name="workflowActivities">workflow activities to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>returns status of the process</returns>
        public Int32 ProcessActivities(Collection<WorkflowActivity> workflowActivities, String loginUser, ref OperationResult operationResult, CallerContext callerContext)
        {
            Int32 result = 0;

            WorkflowDesignerService workflowDesignerService = new WorkflowDesignerService(App.WCFBinding, App.WCFRemoteAddress);
            result = workflowDesignerService.ProcessActivities(workflowActivities, loginUser, ref operationResult, callerContext);

            return result;
        }

        /// <summary>
        /// Gets all the workflows, workflow versions and activities in the system
        /// </summary>
        /// <param name="workflowCollection"></param>
        /// <param name="workflowVersionCollection"></param>
        /// <param name="workflowActivityCollection"></param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        public void GetAllWorkflowDetails(ref Collection<Workflow> workflowCollection, ref Collection<WorkflowVersion> workflowVersionCollection, ref Collection<WorkflowActivity> workflowActivityCollection, ref OperationResult operationResult, CallerContext callerContext)
        {
            WorkflowDesignerService workflowDesignerService = new WorkflowDesignerService(App.WCFBinding, App.WCFRemoteAddress);
            workflowDesignerService.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, ref operationResult, callerContext);
        }
    }
}
