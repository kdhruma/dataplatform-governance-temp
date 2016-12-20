using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;

using MDM.Workflow.Activities.Core;

namespace MDM.Workflow.Activities.Custom
{
    /// <summary>
    /// Sample Code Activity
    /// </summary>
    [Designer(typeof(MDM.Workflow.Activities.Custom.Designers.SampleCodeActivityDesigner))]
    //TODO::Replace the ImageName with the actual Image Name of this activity here..
    [ToolboxBitmap(typeof(MDM.Workflow.Activities.Custom.Designers.SampleCodeActivityDesigner), "Images.SampleActivityImage.bmp")]
    public class SampleCodeActivity:MDMCodeActivitiyBase<Boolean>
    {
        #region Fields

        //TODO::Define fields used for this activity here

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

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Boolean Execute(CodeActivityContext context)
        {
            Boolean isSuccess = false;

            //TODO::Implement the Activity logic here

            return isSuccess;
        }

        #endregion
    }
}
