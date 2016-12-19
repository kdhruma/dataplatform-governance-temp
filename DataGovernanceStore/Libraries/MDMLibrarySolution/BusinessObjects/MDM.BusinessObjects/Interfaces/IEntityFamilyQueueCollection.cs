using MDM.Interfaces;
using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity family queue collection.
    /// </summary>
    public interface IEntityFamilyQueueCollection : IEnumerable<EntityFamilyQueue>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity family queue present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the EntityFamilyQueueCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the EntityFamilyQueueCollection</returns>
        String ToXml();

        #endregion Methods
    }
}