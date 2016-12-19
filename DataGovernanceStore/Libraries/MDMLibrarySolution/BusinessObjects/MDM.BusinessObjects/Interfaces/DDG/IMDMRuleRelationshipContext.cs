using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule relationship context.
    /// </summary>
    public interface IMDMRuleRelationshipContext
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule relationship context object
        /// </summary>
        /// <returns>Xml representation of MDMRule relationship context object</returns>
        String ToXml();

        #endregion
    }
}
