using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for providing entity variant definition mapping collection information.
    /// </summary>
    public interface IEntityVariantDefinitionMappingCollection : IEnumerable<EntityVariantDefinitionMapping>
    {
        #region Fields

        #endregion Fields

        #region Methods

        /// <summary>
        /// Represents entity variant definition mapping collection in Xml format 
        /// </summary>
        /// <returns>Returns entity variant definition mapping collection in Xml format as string.</returns>
        String ToXml();

        /// <summary>
        /// Adds entity variant definition mapping to the collection
        /// </summary>
        /// <param name="mapping">Indicates the entity variant definition mapping to be added</param>
        void Add(IEntityVariantDefinitionMapping mapping);

        #endregion Methods
    }
}
