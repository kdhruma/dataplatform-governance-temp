using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity family change context collection.
    /// </summary>
    public interface IEntityFamilyChangeContextCollection : IEnumerable<EntityFamilyChangeContext>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity family change context present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the EntityFamilyChangeContextCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityFamilyChangeContextCollection</returns>
        String ToXml();

        #endregion Methods
    }
}