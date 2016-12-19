using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get business condition status instance.
    /// </summary>
    public interface IBusinessConditionStatus : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates status for the business condition 
        /// </summary>
        ValidityStateValue Status { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get XMl representation of business condition
        /// </summary>
        /// <returns>XMl string representation of business condition</returns>
        String ToXml();

        #endregion Methods
    }
}
