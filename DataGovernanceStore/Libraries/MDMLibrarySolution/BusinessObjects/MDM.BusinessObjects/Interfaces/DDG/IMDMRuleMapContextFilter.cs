using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get MDMRule map context filter
    /// </summary>
    public interface IMDMRuleMapContextFilter
    {
        #region Properties

        /// <summary>
        /// Indicates the event id list
        /// </summary>
        Collection<Int32> MDMEventIds { get; set; }

        /// <summary>
        /// Indicates the MDMRule map id list
        /// </summary>
        Collection<Int32> MDMRuleMapIds { get; set; }

        /// <summary>
        /// Indicates the workflow short name
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Indicates the workflow activity long name
        /// </summary>
        String WorkflowActivityLongName { get; set; }

        /// <summary>
        /// Indicates the workflow action
        /// </summary>
        String WorkflowAction { get; set; }

        /// <summary>
        /// Indicates the workflow activity short name
        /// </summary>
        String WorkflowActivityName { get; set; }

        /// <summary>
        /// Property denotes the Entity Id list
        /// </summary>
        Collection<Int64> EntityIds { get; set; }

        /// <summary>
        /// Property denotes the MdMRule Id list
        /// </summary>
        Collection<Int32> MDMRuleIds { get; set; }

        /// <summary>
        /// Property denotes the MDMRuleType
        /// </summary>
        MDMRuleType MDMRuleType { get; set; }

        /// <summary>
        /// Whether to load Business condition's validation rules or not.
        /// This applicable if Business condition rules are available.
        /// </summary>
        Boolean LoadValidationRules { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Set the application context collection for the MDMRule map context filter
        /// </summary>
        /// <param name="applicationContexts">Indicates the application context</param>
        void SetApplicationContexts(IApplicationContextCollection applicationContexts);

        /// <summary>
        /// Gets Xml representation of MDMRule map context filter Object
        /// </summary>
        /// <returns>Returns RuleMapContextFilter in Xml format</returns>
        String ToXml();

        #endregion Methods

    }
}
