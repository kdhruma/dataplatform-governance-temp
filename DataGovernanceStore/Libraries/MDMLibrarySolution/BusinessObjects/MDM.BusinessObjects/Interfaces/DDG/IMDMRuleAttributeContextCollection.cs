using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule attribute context collection.
    /// </summary>
    public interface IMDMRuleAttributeContextCollection
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule attribute context collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule attribute context collection object</returns>
        String ToXml();

        #endregion Methods
    }
}
