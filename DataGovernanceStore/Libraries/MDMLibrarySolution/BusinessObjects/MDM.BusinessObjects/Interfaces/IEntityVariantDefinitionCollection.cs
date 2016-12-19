using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity variant definition collection.
    /// </summary>
    public interface IEntityVariantDefinitionCollection : IEnumerable<EntityVariantDefinition>
    {
        #region Properties
       
        #endregion

        #region Methods

        /// <summary>
        /// Adds entity type to collection
        /// </summary>
        /// <param name="iEntityVariantDefinition">Indicates the entity variant definition to add in the collection</param>
        void Add(IEntityVariantDefinition iEntityVariantDefinition);

        /// <summary>
        /// Gets Xml representation of EntityVariantDefinitionCollection
        /// </summary>
        /// <returns>Xml representation of EntityVariantDefinitionCollection</returns>
        String ToXml();

        ///// <summary>
        ///// Clones EntityVariantDefinitionCollection
        ///// </summary>
        ///// <returns>Cloned EntityVariantDefinitionCollection object</returns>
        //IEntityVariantDefinitionCollection Clone();

        #endregion
    }
}
