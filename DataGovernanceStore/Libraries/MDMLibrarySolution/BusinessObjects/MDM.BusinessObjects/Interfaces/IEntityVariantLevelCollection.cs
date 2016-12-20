using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity variant Level collection.
    /// </summary>
    public interface IEntityVariantLevelCollection : IEnumerable<EntityVariantLevel>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Adds entity variant level to collection
        /// </summary>
        /// <param name="iEntityVariantLevel">Indicates the entity variant Level to add in the collection</param>
        void Add(IEntityVariantLevel iEntityVariantLevel);

        /// <summary>
        /// Gets Xml representation of EntityVariantLevelCollection
        /// </summary>
        /// <returns>Xml representation of EntityVariantLevelCollection</returns>
        String ToXml();

        #endregion
    }
}
