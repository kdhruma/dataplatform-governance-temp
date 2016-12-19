using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.BusinessObjects;

namespace MDM.Imports.Interfaces
{
    /// <summary>
    /// Denorm Search calculation
    /// </summary>
    public interface IDNSearch : IBulkInsert
    {
        /// <summary>
        /// Computes the key value and search value for the given entity and attribute.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="mdmVersion"></param>
        void ComputeKeySearchValue(Entity entity, BusinessObjects.Attribute item, string mdmVersion);

        /// <summary>
        /// Adds the computed key and search value.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="keyValue"></param>
        /// <param name="searchValue"></param>
        void AddKeySearchValue(Int64 entityId, string keyValue, string searchValue);

        /// <summary>
        /// Gets the number of entities that have denorm attributes
        /// </summary>
        /// <returns></returns>
        int GetEntityCount();
    }
}
