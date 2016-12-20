using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties used for providing workflow change context related information.
    /// </summary>
    public interface IWorkflowChangeContext
    {
        #region Properties

        /// <summary>
        /// Specifies workflow activity name of workflow change context
        /// </summary>
        String ActivityName { get; set; }

        /// <summary>
        /// Specifies workflow version of workflow change context
        /// </summary>
        Int64 WorkflowVersion { get; set; }

        /// <summary>
        /// Specifies workflow runtime instance id of workflow change context
        /// </summary>
        String WorkflowRuntimeInstanceId { get; set; }

        #endregion Properties

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of workflow change context object
        /// </summary>
        /// <returns>XML representation of workflow change context object</returns>
        String ToXml();

        #endregion ToXml Methods

        #region Workflow Action Context related methods

        /// <summary>
        /// Gets the action context of workflow change context
        /// </summary>
        /// <returns>Gets the action context of workflow change context</returns>
        IWorkflowActionContext GetWorkflowActionContext();

        /// <summary>
        /// Sets the action context of workflow change context
        /// </summary>
        /// <param name="iWorkflowActionContext">Indicates the workflow action context to be set</param>
        void SetWorkflowActionContext(IWorkflowActionContext iWorkflowActionContext);

        #endregion

        #endregion
    }
}