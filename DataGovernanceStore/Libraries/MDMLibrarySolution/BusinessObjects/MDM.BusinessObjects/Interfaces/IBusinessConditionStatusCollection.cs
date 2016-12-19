using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get business condition status collection instance.
    /// </summary>
    public interface IBusinessConditionStatusCollection : IEnumerable<BusinessConditionStatus>
    {
        #region Properties

        /// <summary>
        /// Presents no. of business condition present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of the BusinessConditionCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the BusinessConditionCollection</returns>
        String ToXml();

        #endregion Methods
    }
}
