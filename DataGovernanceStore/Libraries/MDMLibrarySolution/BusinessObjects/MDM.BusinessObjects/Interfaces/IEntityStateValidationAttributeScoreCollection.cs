using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity state validation attribute score collection instance.
    /// </summary>
    public interface IEntityStateValidationAttributeScoreCollection : IEnumerable<EntityStateValidationAttributeScore>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity state validation attribute score present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of the EntityStateValidationAttributeScoreCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the EntityStateValidationAttributeScoreCollection</returns>
        String ToXml();

        #endregion Methods
    }
}