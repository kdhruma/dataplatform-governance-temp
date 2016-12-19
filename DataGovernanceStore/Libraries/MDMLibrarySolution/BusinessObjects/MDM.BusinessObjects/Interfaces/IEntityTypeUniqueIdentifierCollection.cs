using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Obsolete. Exposes properties used for identifying entity type collection uniquely.
    /// </summary>
    [System.Obsolete("EntityTypeUniqueIdentifier is no longer needed as EntityType short name is enough to identify entityType uniquely.")]    
    public interface IEntityTypeUniqueIdentifierCollection : IEnumerable<EntityTypeUniqueIdentifier>
    {
        #region Fields
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of EntityTypeUniqueIdentifierCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityTypeUniqueIdentifierCollection</returns>
        String ToXml();

        /// <summary>
        /// Add entity type unique identifier in collection
        /// </summary>
        /// <param name="iEntityTypeUniqueIdentifier">Indicates entity type unique identifier to add in collection</param>
        void Add(IEntityTypeUniqueIdentifier iEntityTypeUniqueIdentifier);

        #endregion Methods
    }
}
