using System;

namespace MDM.BusinessObjects.Interfaces.Workflow
{
    /// <summary>
    /// Exposes methods or properties to set or get workflow invokable entity info.
    /// </summary>
    public interface IWorkflowInvokableEntityInfo
    {
        #region Properties

        /// <summary>
        /// Indicates current entity id
        /// </summary>
        Int64 EntityId { get; set; }
       
        /// <summary>
        /// Indicates workflow invokable entity id
        /// </summary>
        Int64 WorkflowInvokableEntityId { get; set; }

        /// <summary>
        /// Indicates if current entity is in workflow or not
        /// </summary>
        Boolean IsEntityInWorkflow { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of the WorkflowInvokableEntityInfo object
        /// </summary>
        /// <returns>Xml string representing the WorkflowInvokableEntityInfo</returns>
        String ToXml();

        #endregion Methods
    }
}
