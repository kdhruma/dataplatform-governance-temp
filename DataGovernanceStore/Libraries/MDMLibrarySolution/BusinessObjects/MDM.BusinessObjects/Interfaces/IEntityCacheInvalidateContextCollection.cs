using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity cache invalidate context collection.
    /// </summary>
    public interface IEntityCacheInvalidateContextCollection
    {
        #region Methods

        /// <summary>
        /// No. of entity cache invalidate context collection
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Add new item into the collection
        /// </summary>
        /// <param name="item">IEntityCacheInvalidateContext</param>
        void Add(IEntityCacheInvalidateContext item);

        /// <summary>
        /// Removes item from collection
        /// </summary>
        /// <param name="item">IEntityCacheInvalidateContext</param>
        /// <returns>'True' if removal was successful, 'False' otherwise</returns>
        Boolean Remove(IEntityCacheInvalidateContext item);

        #region ToXml methods

        /// <summary>
        /// Get XML representation of IEntityCacheInvalidateContext collection object
        /// </summary>
        /// <returns>XML string representing the EntityCacheInvalidateContext collection</returns>
        String ToXml();

        /// <summary>
        /// Get XML representation of IEntityCacheInvalidateContext collection object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different XML representation will be there</param>
        /// <returns>XML representation of object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion ToXml methods

        #endregion Methods
    }
}
