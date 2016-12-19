using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the business context for workflow.
    /// </summary>
    public interface IWorkflowBusinessRuleContext
    {
        #region Properties

        /// <summary>
        /// Property denoting current activity short name which is in the form of GUID
        /// </summary>
        String CurrentActivityName { get; set; }

        /// <summary>
        /// Property denoting current activity long name
        /// </summary>
        String CurrentActivityLongName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get WorkflowDataContext
        /// </summary>
        /// <returns>WorkflowDataContext</returns>
        IWorkflowDataContext GetWorkflowDataContext();

        /// <summary>
        /// Set WorkflowDataContext
        /// </summary>
        /// <param name="iWorkflowDataContext">WorkflowDataContext to be set</param>
        void SetWorkflowDataContext(IWorkflowDataContext iWorkflowDataContext);

        /// <summary>
        /// Get Previous Activity ActionContext
        /// </summary>
        /// <returns>WorkflowActionContext</returns>
        IWorkflowActionContext GetPreviousActivityActionContext();

        #endregion 
    }
}
