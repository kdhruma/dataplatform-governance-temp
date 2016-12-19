using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.ObjectModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.Core;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Defines operation contracts for MDM Workflow Designer operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IWorkflowDesignerService
    {
        [OperationContract(Name = "GetAppConfigValue")]
        String GetAppConfigValue(String keyName, ref MDMBO.OperationResult operationResult);

        [OperationContract(Name = "GetWorkflowViewDetails")]
        void GetWorkflowViewDetails(Int32 workflowVersionID, String instanceGuid, ref MDMBOW.WorkflowVersion workflowVersion, ref Collection<MDMBOW.TrackedActivityInfo> trackedActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ProcessWorkflows")]
        MDMBOW.WorkflowVersion ProcessWorkflows(Collection<MDMBOW.Workflow> workflows, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ProcessActivities")]
        Int32 ProcessActivities(Collection<MDMBOW.WorkflowActivity> workflowActivities, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetAllWorkflowDetails")]
        void GetAllWorkflowDetails(ref Collection<MDMBOW.Workflow> workflowCollection, ref Collection<MDMBOW.WorkflowVersion> workflowVersionCollection, ref Collection<MDMBOW.WorkflowActivity> workflowActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);
    }
}
