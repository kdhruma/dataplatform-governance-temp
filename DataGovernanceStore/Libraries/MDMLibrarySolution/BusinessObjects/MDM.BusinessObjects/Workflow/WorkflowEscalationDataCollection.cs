using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the collection of workflow escalation data object
    /// </summary>
    [DataContract]
    public class WorkflowEscalationDataCollection : InterfaceContractCollection<IWorkflowEscalationData, WorkflowEscalationData>, IWorkflowEscalationDataCollection
    {
        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public WorkflowEscalationDataCollection()
        { 
        
        }

        /// <summary>
        /// Constructor with list of workflow escalation objects
        /// </summary>
        /// <param name="workflowEscalationList">List of workflow escalation items</param>
        public WorkflowEscalationDataCollection(IList<WorkflowEscalationData> workflowEscalationList)
        {
            if (workflowEscalationList != null)
            {
                this._items = new Collection<WorkflowEscalationData>(workflowEscalationList);
            }
        }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Gets the workflow escalation details based on entityId
        /// </summary>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        public IWorkflowEscalationDataCollection GetEscalationDetailsByEntityId(Int64 entityId)
        {
            this.ValidateWorkflowItems();

            return new WorkflowEscalationDataCollection((from wf in this._items
                                                     where wf.EntityId == entityId
                                                     select wf).ToList<WorkflowEscalationData>());
        }

        /// <summary>
        /// Gets the workflow escalation details based on entity id and workflow short name
        /// </summary>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <param name="workflowName">Indicates the workflow short name</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        public IWorkflowEscalationDataCollection GetEscalationDetailsByEntityIdAndWorkflowName(Int32 entityId, String workflowName)
        {
            this.ValidateWorkflowItems();
            this.ValidateWorkflowShortName(workflowName);
           
            return new WorkflowEscalationDataCollection((from wf in this._items
                                                     where wf.EntityId == entityId && wf.WorkflowName.Trim() == workflowName.Trim()
                                                     select wf).ToList<WorkflowEscalationData>());
        }

        /// <summary>
        /// Gets the workflow escalation details based on activity long name
        /// </summary>
        /// <param name="activityLongName">Indicates the activity long name</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        public IWorkflowEscalationDataCollection GetEscalationDetailsByActivityName(String activityLongName)
        {
            this.ValidateWorkflowItems();
            this.ValidateActivityLongName(activityLongName);

            return new WorkflowEscalationDataCollection((from wf in this._items
                                                     where wf.ActivityLongName.Trim() == activityLongName.Trim()
                                                     select wf).ToList<WorkflowEscalationData>());
        }

        /// <summary>
        /// Gets the workflow escalation details based on EntityId, Workflow Name and Activity long name
        /// </summary>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <param name="workflowShortName">Indicates the workflow short name</param>
        /// <param name="activityLongName">Indicates the activity long Name</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        public IWorkflowEscalationDataCollection GetEscalationDetailsByActivityNameAndWorkflowName(Int32 entityId, String workflowShortName, String activityLongName)
        {
            this.ValidateWorkflowItems();
            this.ValidateWorkflowShortName(workflowShortName);
            this.ValidateActivityLongName(activityLongName);

            return new WorkflowEscalationDataCollection((from wf in this._items
                                                     where wf.EntityId == entityId
                                                     && wf.ActivityLongName.Trim() == activityLongName.Trim()
                                                     && wf.WorkflowName.Trim() == workflowShortName.Trim()
                                                     select wf).ToList<WorkflowEscalationData>());
        }

        /// <summary>
        /// Gets the workflow escalation details based on workflow short name
        /// </summary>
        /// <param name="workflowName">Indicates the workflow short name</param>
        /// <returns>Returns the workflow escalation collection object</returns>
        public IWorkflowEscalationDataCollection GetEscalationDetailsByWorkflowName(String workflowName)
        {
            this.ValidateWorkflowItems();
            this.ValidateWorkflowShortName(workflowName);

            return new WorkflowEscalationDataCollection((from wf in this._items
                                                         where wf.WorkflowName == workflowName
                                                         select wf).ToList<WorkflowEscalationData>());
        }

        /// <summary>
        /// Gets the distinct workflow short names from the current workflow escalation data
        /// </summary>
        /// <returns>Returns the list of workflow short names</returns>
        public Collection<String> GetWorkflowNames()
        {
            this.ValidateWorkflowItems();
            return new Collection<String>(this._items.Select(w => w.WorkflowName).Distinct().ToList());

        }

        /// <summary>
        /// Gets the distinct entity ids from the current workflow escalation data
        /// </summary>
        /// <returns>Returns the list of entity Id</returns>
        public Collection<Int64> GetEntityIds()
        {
            this.ValidateWorkflowItems();
            return new Collection<Int64>(this._items.Select(w => w.EntityId).Distinct().ToList());
        }

        #endregion 

        #region Private Methods

        private void ValidateWorkflowItems()
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There is no workflow escalation items found to search.");
            }
        }

        private void ValidateActivityLongName(String activityLongName)
        {
            if (String.IsNullOrWhiteSpace(activityLongName))
            {
                throw new ArgumentNullException("activityLongName parameter cannot be null or empty.");
            }

        }

        private void ValidateWorkflowShortName(String workflowName)
        {
            if (String.IsNullOrWhiteSpace(workflowName))
            {
                throw new ArgumentNullException("WorkflowName parameter cannot be null or empty.");
            }
        }

        #endregion 
    }
}
