using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;

using MDM.Workflow.Activities.Core;

namespace MDM.Workflow.Activities.Custom
{
    /// <summary>
    /// Sample Human Activity
    /// </summary>
    [Designer(typeof(MDM.Workflow.Activities.Custom.Designers.SampleHumanActivityDesigner))]
    //TODO::Replace the ImageName with the actual Image Name of this activity here..
    [ToolboxBitmap(typeof(MDM.Workflow.Activities.Custom.Designers.SampleHumanActivityDesigner), "Images.SampleActivityImage.bmp")]
    public class SampleHumanActivity : MDMNativeActivitiyBase
    {
        #region Fields

        //TODO::Define fields used for this activity here

        private String _actions = "Done,None";

        #endregion

        #region Properties

        //TODO::Define the Input Arguments, Output Arguments and Properties for this Activity here

        /// <summary>
        /// Sample Input Argument
        /// </summary>
        [DisplayName("Sample Input")]
        [Category("Input Arguments")]
        public InArgument<String> SampleInput { get; set; }

        /// <summary>
        /// Sample Output Argument
        /// </summary>
        [DisplayName("Sample Output")]
        [Category("Output Arguments")]
        public OutArgument<String> SampleOutput { get; set; }

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

        //TODO::If this activity requires some logic to be executed then override the Execute method, implement the logic and call the base Execute method as shown below..

        //protected override void Execute(NativeActivityContext context)
        //{
        //    //Implement logic here

        //    base.Execute(context);
        //}

        #endregion
    }
}
