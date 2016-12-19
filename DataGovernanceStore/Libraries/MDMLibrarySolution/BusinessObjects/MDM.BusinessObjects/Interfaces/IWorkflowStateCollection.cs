using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of workflow states.
    /// </summary>
    public interface IWorkflowStateCollection : IEnumerable<WorkflowState>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of IWorkflowStateCollection object
        /// </summary>
        /// <returns>Xml string representing the IWorkflowStateCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of the object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
