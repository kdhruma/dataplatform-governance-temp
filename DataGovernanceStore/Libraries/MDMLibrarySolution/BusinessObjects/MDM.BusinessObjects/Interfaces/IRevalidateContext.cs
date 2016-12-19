using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for providing revalidate context related information.
    /// </summary>
    public interface IRevalidateContext: IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates search criteria to specify the context for dynamic data governance. 
        /// </summary>
        SearchCriteria SearchCriteria { get; set; }

        /// <summary>
        ///  Indicates collection of entity identifiers on which rule to be applied
        /// </summary>
        Collection<Int64> EntityIds { get; set; }

        /// <summary>
        /// Indicates collection of rule map context identifiers which needs to be applied
        /// </summary>
        Collection<Int32> RuleMapContextIds { get; set; }

        /// <summary>
        /// Indicates collection of rule map context names which needs to be applied
        /// </summary>
        Collection<String> RuleMapContextNames { get; set; }

        /// <summary>
        /// Indicates the revalidate mode for triggering the rule
        /// </summary>
        RevalidateMode RevalidateMode { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents revalidate context  in Xml format
        /// </summary>
        /// <returns>
        /// Returns Revalidate context  in Xml format
        /// </returns>
        String ToXml();

        #endregion Methods
    }
}
