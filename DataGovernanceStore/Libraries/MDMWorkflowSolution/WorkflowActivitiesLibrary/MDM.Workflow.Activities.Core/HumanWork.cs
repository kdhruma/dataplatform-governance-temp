using System;
using System.Drawing;
using System.ComponentModel;

using MDM.Workflow.Activities.Designer;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Activity which indicates performing 
    /// </summary>
    [Designer(typeof(HumanWorkDesigner))]
    [ToolboxBitmap(typeof(HumanWorkDesigner), "Images.HumanWork.bmp")]
    public class HumanWork : MDMNativeActivitiyBase
    {
        #region Fields

        private String _actions = "Done,None,";

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
