using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity type collection.
    /// </summary>
    public interface IEntityTypeCollection : IEnumerable<EntityType>
    {
        #region Properties
       
        #endregion

        #region Methods

        /// <summary>
        /// Adds entity type to collection
        /// </summary>
        /// <param name="iEntityType">Indicates the entity type to add in the collection</param>
        void Add(IEntityType iEntityType);

        /// <summary>
        /// Gets Xml representation of EntityTypeCollection
        /// </summary>
        /// <returns>Xml representation of EntityTypeCollection</returns>
        String ToXml();

        /// <summary>
        /// Clones EntityTypeCollection
        /// </summary>
        /// <returns>Cloned EntityTypeCollection object</returns>
        IEntityTypeCollection Clone();

        #endregion
    }
}
