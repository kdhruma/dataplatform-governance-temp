using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the lookup search rule collection related information.
    /// </summary>
    public interface ILookupSearchRuleCollection: IEnumerable<LookupSearchRule>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get XML representation of LookupSearchRuleCollection object
        /// </summary>
        /// <returns>XML representation of LookupSearchRuleCollection object</returns>
        String ToXml();

        /// <summary>
        /// Add lookup search rule in collection
        /// </summary>
        /// <param name="item">LookupSearchRule to add in collection</param>
        void Add(ILookupSearchRule item);

        #endregion
    }
}
