using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Workflow.Designer.Business
{
    using MDM.Utility;
    using MDMBO=MDM.BusinessObjects;
    using MDMBOW=MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.Workflow.Designer.Data;
    using MDM.Core;

    /// <summary>
    /// Business Logic for Workflow View
    /// </summary>
    public class WorkflowViewBL : BusinessLogicBase
    {
        /// <summary>
        /// Gets the details required for Execution/Definition view
        /// </summary>
        /// <param name="workflowVersionID"> Id of the workflow version</param>
        /// <param name="instanceGuid">GUID of the Instance</param>
        /// <param name="workflowVersion">Workflow version object. Ref param.</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <param name="trackedActivityCollection">Tracked activity collection. Ref param.</param>
        public void GetWorkflowViewDetails(Int32 workflowVersionID, String instanceGuid, ref MDMBOW.WorkflowVersion workflowVersion, ref Collection<MDMBOW.TrackedActivityInfo> trackedActivityCollection, MDMBO.CallerContext callerContext )
        {
            //Get Command
            MDMBO.DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            WorkflowViewDA workflowViewDA = new WorkflowViewDA();
            workflowViewDA.GetWorkflowViewDetails(workflowVersionID, instanceGuid, ref workflowVersion, ref trackedActivityCollection);
        }
    }
}
