using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Exposes methods or properties to set or get workflow invokable entity info collection.
    /// </summary>
    public interface IWorkflowInvokableEntityInfoCollection : IEnumerable<WorkflowInvokableEntityInfo>
    {
        #region Properties

        /// <summary>
        /// Indicates no. of workflow invokable entity info present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the WorkflowInvokableEntityInfoCollection object
        /// </summary>
        /// <returns>Xml string representing the WorkflowInvokableEntityInfoCollection</returns>
        String ToXml();

        #endregion Methods
    }
}
