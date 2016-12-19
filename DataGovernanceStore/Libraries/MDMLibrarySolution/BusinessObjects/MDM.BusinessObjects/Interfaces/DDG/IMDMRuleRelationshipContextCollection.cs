using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule relationship context collection.
    /// </summary>
    public interface IMDMRuleRelationshipContextCollection
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule relationship context collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule relationship context collection object</returns>
        String ToXml();

        #endregion Methods
    }
}
