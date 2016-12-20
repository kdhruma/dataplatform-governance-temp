using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get MdmRule attribute context.
    /// </summary>
    public interface IMDMRuleAttributeContext
    {
        #region Properties

        /// <summary>
        /// Property denotes data locale.
        /// </summary>
        LocaleEnum DataLocale
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule attribute context object
        /// </summary>
        /// <returns>Xml representation of MDMRule attribute context object</returns>
        String ToXml();

        #endregion Methods
    }
}
