using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity state validation score collection instance.
    /// </summary>
    public interface IEntityStateValidationScoreCollection : IEnumerable<EntityStateValidationScore>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity validation score present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of the EntityStateValidationScoreCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the EntityStateValidationScoreCollection</returns>
        String ToXml();

        /// <summary>
        /// Get entity validation score for specified entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <returns>Returns entity validation score</returns>
        IEntityStateValidationScore GetByEntityId(Int64 entityId);

        #endregion Methods
    }
}
