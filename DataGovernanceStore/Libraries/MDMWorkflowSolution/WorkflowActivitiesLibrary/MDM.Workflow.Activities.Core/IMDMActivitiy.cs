using System;
using System.Activities;
using MDM.Core;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Any custom activity used in MDM Workflow System must implement this interface
    /// The custom activity which derives from NativeActivityBase or CodeActivityBase does not need to implement this
    /// </summary>
    public interface IMDMActivitiy :   IMDMSystemActivitiy
    {
        #region Properties
        /// <summary>
        /// Flag which indicates whether activity is human interaction activity or not
        /// </summary>
        InArgument<Boolean> IsHumanActivity { get; set; }
        #endregion

    }
}
