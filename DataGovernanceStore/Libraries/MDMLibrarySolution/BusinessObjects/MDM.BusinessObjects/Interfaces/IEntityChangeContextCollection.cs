using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity change context collection.
    /// </summary>
    public interface IEntityChangeContextCollection : IEnumerable<EntityChangeContext>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity change context present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the EntityChangeContextCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityChangeContextCollection</returns>
        String ToXml();

        #endregion Methods
    }
}