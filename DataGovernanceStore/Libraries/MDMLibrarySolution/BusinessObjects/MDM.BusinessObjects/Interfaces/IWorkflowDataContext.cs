using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the context for workflow data. 
    /// </summary>
    public interface IWorkflowDataContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the Name of Workflow
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denoting the Long Name of Workflow
        /// </summary>
        String WorkflowLongName { get; set; }

        /// <summary>
        /// Property denoting VersionId of a Workflow
        /// </summary>
        Int32 WorkflowVersionId { get; set; }

        /// <summary>
        ///  Represents the data which user wants to send to workflow runtime which is not predefined.
        /// The data should be in the format of XML.
        /// <!--<ExtendedProperties><Property Key = "" Value = "" /></ExtendedProperties>-->
        /// </summary>
        String ExtendedProperties { get; set; }

        /// <summary>
        /// Name of application which is performing action
        /// </summary>
        MDMCenterApplication Application { get; set; }

        /// <summary>
        /// Name of module which is performing action
        /// </summary>
        MDMCenterModules Module { get; set; }

        /// <summary>
        /// Property denoting the comments entered for the Workflow.
        /// </summary>
        String WorkflowComments { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get WorkflowMDMObjectCollection
        /// </summary>
        /// <returns>IWorkflowMDMObjectCollection</returns>
        IWorkflowMDMObjectCollection GetWorkflowMDMObjectCollection();

        /// <summary>
        /// Set WorkflowMDMObjectCollection
        /// </summary>
        /// <param name="iWorkflowMDMObjectCollection">WorkflowMDMObjectCollection which needs to be set</param>
        void SetWorkflowMDMObjectCollection(IWorkflowMDMObjectCollection iWorkflowMDMObjectCollection);

        #endregion
    }
}
