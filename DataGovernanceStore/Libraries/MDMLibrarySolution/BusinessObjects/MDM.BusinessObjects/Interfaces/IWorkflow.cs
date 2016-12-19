using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;    

    /// <summary>
    /// Exposes methods or properties to set or get workflow related information.
    /// </summary>
    public interface IWorkflow : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting latest Version of Workflow 
        /// </summary>
        Int32 LatestVersion { get; set; }

        /// <summary>
        /// Property denoting type of a workflow
        /// </summary>
        String WorkflowType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of object
        /// </summary>
        /// <returns>Xml representation of the object</returns>
        String ToXML();
        
        #endregion
    }
}
