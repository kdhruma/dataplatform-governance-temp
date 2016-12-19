using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule context.
    /// </summary>
    public interface IMDMRuleContext
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule context object
        /// </summary>
        /// <returns>Xml representation of MDMRule context object</returns>
        String ToXml();

        #endregion
    }
}
