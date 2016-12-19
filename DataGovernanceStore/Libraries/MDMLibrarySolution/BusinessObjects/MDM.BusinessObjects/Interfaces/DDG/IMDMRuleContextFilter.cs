using System;
using System.Collections.ObjectModel;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MDMRule context filter
    /// </summary>
   public interface IMDMRuleContextFilter
    {
        #region Properties
        
        /// <summary>
        /// Property denotes the Entity Id list
        /// </summary>
        Collection<Int64> EntityIds { get; set; }

        /// <summary>
        /// Property denotes the MdMRule Id list
        /// </summary>
        Collection<Int32> MDMRuleIds { get; set; }
        
        /// <summary>
        /// Property denotes the list of MDMRuleNames
        /// </summary>
        Collection<String> MDMRuleNames { get; set; }

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
        /// Set the application context collection for the MDMRule context filter
        /// </summary>
        /// <param name="applicationContexts">Indicates the application contexts</param>
        void SetApplicationContexts(IApplicationContextCollection applicationContexts);

        /// <summary>
        /// Gets Xml representation of MDMRule context filter Object
        /// </summary>
        /// <returns>Returns RuleContextFilter in Xml format</returns>
        String ToXml();

        #endregion Methods
    }
}
