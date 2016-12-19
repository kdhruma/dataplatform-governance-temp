using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Context defining the Workflow Business Rule
    /// </summary>
    [DataContract]
    public class WorkflowBusinessRuleContext: IWorkflowBusinessRuleContext
    {
        #region Fields

        /// <summary>
        /// Field denoting workflow data context
        /// </summary>
        private WorkflowDataContext _workflowDataContext = null;

        /// <summary>
        /// Field denoting current activity short name which is in the form of GUID
        /// </summary>
        private String _currentActivityName = String.Empty;

        /// <summary>
        /// Field denoting current activity long name
        /// </summary>
        private String _currentActivityLongName = String.Empty;

        /// <summary>
        /// Field denoting action context of previous activity
        /// </summary>
        private WorkflowActionContext _previousActivityActionContext = null;

        #endregion

        #region Properties

        /// <summary>
        /// Workflow Data Context participating in the Business Rule
        /// </summary>
        [DataMember]
        public WorkflowDataContext WorkflowDataContext
        {
            get
            {
                return this._workflowDataContext;
            }
            set
            {
                this._workflowDataContext = value;
            }
        }

        /// <summary>
        /// Property denoting current activity short name which is in the form of GUID
        /// </summary>
        [DataMember]
        public String CurrentActivityName
        {
            get
            {
                return _currentActivityName;
            }
            set
            {
                _currentActivityName = value;
            }
        }

        /// <summary>
        /// Property denoting current activity long name
        /// </summary>
        [DataMember]
        public String CurrentActivityLongName
        {
            get
            {
                return _currentActivityLongName;
            }
            set
            {
                _currentActivityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting action context of previous activity
        /// </summary>
        [DataMember]
        public WorkflowActionContext PreviousActivityActionContext
        {
            get
            {
                return _previousActivityActionContext;
            }
            set
            {
                _previousActivityActionContext = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the context with the provided components
        /// </summary>
        /// <param name="workflowDataContext"></param>
        public WorkflowBusinessRuleContext(WorkflowDataContext workflowDataContext)
        {
            this._workflowDataContext = workflowDataContext;
        }

        /// <summary>
        /// Initializes the context with the provided components
        /// </summary>
        /// <param name="workflowDataContext"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="currentActivityLongName"></param>
        /// <param name="previousActivityActionContext"></param>
        public WorkflowBusinessRuleContext(WorkflowDataContext workflowDataContext, String currentActivityName, String currentActivityLongName, WorkflowActionContext previousActivityActionContext)
        {
            this._workflowDataContext = workflowDataContext;
            this._currentActivityName = currentActivityName;
            this._currentActivityLongName = currentActivityLongName;
            this._previousActivityActionContext = previousActivityActionContext;
        }

        #endregion

        #region Methods

        #endregion

        #region IWorkflowBusinessRuleContext Methods

        /// <summary>
        /// Get WorkflowDataContext
        /// </summary>
        /// <returns>WorkflowDataContext</returns>
        public IWorkflowDataContext GetWorkflowDataContext()
        {
            if (this.WorkflowDataContext == null)
            {
                throw new NullReferenceException("WorkflowDataContext is null");
            }
            return (IWorkflowDataContext)this.WorkflowDataContext;
        }

        /// <summary>
        /// Set WorkflowDataContext
        /// </summary>
        /// <param name="iWorkflowDataContext">WorkflowDataContext to be set</param>
        public void SetWorkflowDataContext(IWorkflowDataContext iWorkflowDataContext)
        {
            if (iWorkflowDataContext == null)
                throw new ArgumentNullException("WorkflowDataContext");

            this.WorkflowDataContext = (WorkflowDataContext)iWorkflowDataContext;
        }

        /// <summary>
        /// Get Previous Activity ActionContext
        /// </summary>
        /// <returns>WorkflowActionContext</returns>
        public IWorkflowActionContext GetPreviousActivityActionContext()
        {
            if (this.PreviousActivityActionContext == null)
            {
                throw new NullReferenceException("Previous Activity ActionContext is null");
            }

            return (IWorkflowActionContext)this.PreviousActivityActionContext;
        }

        #endregion
    }
}
