using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Exposes methods or properties to set or get workflow escalation data collection object.
    /// </summary>
    public interface IWorkflowEscalationDataCollection : IEnumerable<WorkflowEscalationData>
    {
        #region Methods

        /// <summary>
        /// Gets the workflow escalation details based on entityId
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Escalation Details By Entity Id"  source="..\MDM.APISamples\Workflow\Escalation\WorkflowEscalationDataCollectionSamples.cs" region="Get Escalation Details By Entity Id"/>
        /// <code language="c#" title="Get Escalation Details from GetWorkFlowEscalationDetailsSamples"  source="..\MDM.APISamples\Workflow\Escalation\GetWorkFlowEscalationDetailsSamples.cs" region="Get workflow escalation details based on workflow name and activity name for an entity"/>
        /// </example>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        IWorkflowEscalationDataCollection GetEscalationDetailsByEntityId(Int64 entityId);

        /// <summary>
        /// Gets the workflow escalation details based on entity id and workflow short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Escalation Details By Entity Id And Workflow Name"  source="..\MDM.APISamples\Workflow\Escalation\WorkflowEscalationDataCollectionSamples.cs" region="Get Escalation Details By Entity Id And Workflow Name"/>
        /// <code language="c#" title="Get Escalation Details from GetWorkFlowEscalationDetailsSamples"  source="..\MDM.APISamples\Workflow\Escalation\GetWorkFlowEscalationDetailsSamples.cs" region="Get workflow escalation details based on workflow name and activity name for an entity"/>
        /// </example>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <param name="workflowName">Indicates the workflow short name</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        IWorkflowEscalationDataCollection GetEscalationDetailsByEntityIdAndWorkflowName(Int32 entityId, String workflowName);

        /// <summary>
        /// Gets the workflow escalation details based on activity long name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Escalation Details By Activity Name"  source="..\MDM.APISamples\Workflow\Escalation\WorkflowEscalationDataCollectionSamples.cs" region="Get Escalation Details By Activity Name"/>
        /// <code language="c#" title="Get Escalation Details from GetWorkFlowEscalationDetailsSamples"  source="..\MDM.APISamples\Workflow\Escalation\GetWorkFlowEscalationDetailsSamples.cs" region="Get workflow escalation details based on workflow name and activity name for an entity"/>
        /// </example>
        /// <param name="activityLongName">Indicates the activity long name</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        IWorkflowEscalationDataCollection GetEscalationDetailsByActivityName(String activityLongName);

        /// <summary>
        /// Gets the workflow escalation details based on EntityId, Workflow Name and Activity long name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Escalation Details By Activity Name And Workflow Name"  source="..\MDM.APISamples\Workflow\Escalation\WorkflowEscalationDataCollectionSamples.cs" region="Get Escalation Details By Activity Name And Workflow Name"/>
        /// <code language="c#" title="Get Escalation Details from GetWorkFlowEscalationDetailsSamples"  source="..\MDM.APISamples\Workflow\Escalation\GetWorkFlowEscalationDetailsSamples.cs" region="Get workflow escalation details based on workflow name and activity name for an entity"/>
        /// </example>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <param name="workflowShortName">Indicates the workflow short name</param>
        /// <param name="activityLongName">Indicates the activity long Name</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        IWorkflowEscalationDataCollection GetEscalationDetailsByActivityNameAndWorkflowName(Int32 entityId, String workflowShortName, String activityLongName);

        #endregion
    }
}
