using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get MDMRule map detail
    /// </summary>
    public interface IMDMRuleMapDetail : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denotes the event id
        /// </summary>
        Int32 EventId { get; set; }

        /// <summary>
        /// Property denotes the name of the event
        /// </summary>
        String EventName { get; set; }

        /// <summary>
        /// Property denotes the sequence
        /// </summary>
        Int32 Sequence { get; set; }

        /// <summary>
        /// Property denotes whether the rule mapped under the current application context and event is enabled or not
        /// </summary>
        Boolean IsRuleMapEnabled { get; set; }

        /// <summary>
        /// Property denotes whether the rule should be executed Async mode or Sync mode
        /// </summary>
        Boolean IsAsyncRule { get; set; }

        /// <summary>
        /// Property denotes the workflow name
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denotes the workflow activity id
        /// </summary>
        Int32 WorkflowActivityId { get; set; }

        /// <summary>
        /// Property denotes the workflow activity name
        /// </summary>
        String WorkflowActivityName { get; set; }

        /// <summary>
        /// Property denotes the workflow action
        /// </summary>
        String WorkflowAction { get; set; }

        /// <summary>
        /// Indicates the MDMRule type
        /// </summary>
        MDMRuleType RuleType { get; set; }

        /// <summary>
        /// Property denotes the MDMEvent Type
        /// </summary>
        MDMEventType EventType { get; set; }

        /// <summary>
        /// Property denotes parent rule name (BC rule name)
        /// </summary>
        String ParentRuleName { get; }


        /// <summary>
        /// Property denotes the Business condition rule Id
        /// </summary>
        Int32 ParentRuleId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get XML representation of MDMRule map detail object
        /// </summary>
        /// <returns>XML representation of MDMRule map detail object</returns>
        String ToXml();

        /// <summary>
        /// Set the application context to MDMRule map
        /// </summary>
        /// <param name="applicationContext">Indicates the application context</param>
        void SetApplicationContext(IApplicationContext applicationContext);

        /// <summary>
        /// Set the MDMRule to MDMRule map
        /// </summary>
        /// <param name="mdmRule">Indicates the MDMRule</param>
        void SetMDMRule(IMDMRule mdmRule);

        /// <summary>
        /// Gets the MDMRule
        /// </summary>
        /// <returns>MDMRule</returns>
        IMDMRule GetMdmRule();

        #endregion Methods
    }
}
