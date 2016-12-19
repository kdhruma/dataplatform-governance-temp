using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get workflow MDM object related information.
    /// </summary>
    public interface IWorkflowMDMObject : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Id of MDMObject
        /// </summary>
        Int64 MDMObjectId { get; set; }

        /// <summary>
        /// Property denoting Type of a MDMObject
        /// </summary>
        String MDMObjectType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Workflow Instance
        /// </summary>
        /// <returns>Xml representation of Workflow Instance</returns>
        String ToXml();

        #endregion
    }
}
