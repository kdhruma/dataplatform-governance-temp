using MDM.BusinessObjects;
using MDM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get entity scope collection
    /// </summary>
    public interface IEntityScopeCollection : IEnumerable<EntityScope>, ICollection<EntityScope>
    {
        #region Methods

        /// <summary>
        /// Gets Xml representation of EntityScopeCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityScopeCollection</returns>
        String ToXml();

        /// <summary>
        /// Adds EntityScope in collection
        /// </summary>
        /// <param name="iEntityScope">Indicates entityscope to add in collection</param>
        void Add(IEntityScope iEntityScope);

        /// <summary>
        /// Adds EntityScopes in collection
        /// </summary>
        /// <param name="iEntityScopeCollection">Indicates EntityScopes to add in collection</param>
        void AddRange(IEntityScopeCollection iEntityScopeCollection);

        #endregion Methods

    }
}
