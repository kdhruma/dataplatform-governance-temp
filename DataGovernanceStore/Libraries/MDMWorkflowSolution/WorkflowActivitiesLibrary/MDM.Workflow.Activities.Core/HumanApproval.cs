using System;
using System.Drawing;
using System.ComponentModel;

using MDM.Workflow.Activities.Designer;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Activity used for review of Human Work 
    /// </summary>
    [Designer(typeof(HumanApprovalDesigner))]
    [ToolboxBitmap(typeof(HumanApprovalDesigner), "Images.HumanApproval.bmp")]
    public class HumanApproval : MDMNativeActivitiyBase
    {
        #region Fields

        private String _actions = "Approve,Optional,;Reject,Mandatory,";

        #endregion

        #region Properties

        /// <summary>
        /// Possible actions for this activity
        /// Ex: Approve,Reject
        /// </summary>
        [DisplayName("Actions")]
        [Category("Actions")]
        public override String Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }

        #endregion

        #region Methods

        #endregion
    }
}
