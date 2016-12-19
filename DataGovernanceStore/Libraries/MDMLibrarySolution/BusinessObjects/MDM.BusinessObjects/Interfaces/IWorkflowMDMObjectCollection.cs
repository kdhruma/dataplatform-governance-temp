using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects.Workflow;
    
    /// <summary>
    /// Exposes methods or properties to set or get the collection of workflow MDM objects.
    /// </summary>
    public interface IWorkflowMDMObjectCollection : IEnumerable<WorkflowMDMObject>
    {
        #region properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of IWorkflowMDMObjectCollection Instance
        /// </summary>
        /// <returns>Xml representation of IWorkflowMDMObjectCollection Instance</returns>
        String ToXml();

        /// <summary>
        /// Add WorkflowMDMObject Instance in collection
        /// </summary>
        /// <param name="item">WorkflowMDMObject to add in collection</param>
        void Add(IWorkflowMDMObject item);

        /// <summary>
        /// Removes the first occurrence of a specific object from the Workflow MDM Object collection
        /// </summary>
        /// <param name="item">The object to remove from the Workflow MDM object collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        Boolean Remove(IWorkflowMDMObject item);
        
        #endregion
    }
}
