using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods used for setting entity level history for multiple entities.
    /// </summary>
    public interface IEntityCollectionHistory
    {
        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iEntityHistory">Indicates entity history object to be added in collection.</param>
        void Add(IEntityHistory iEntityHistory);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="iEntityHistory"> Entity history object of single entity </param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(IEntityHistory iEntityHistory);

        /// <summary>
        /// Removes item
        /// </summary>
        /// <param name="iEntityHistory">item which needs to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(IEntityHistory iEntityHistory);

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        String ToXml();

        /// <summary>
        /// Clones object
        /// </summary>
        /// <returns>Cloned object</returns>
        IEntityCollectionHistory Clone();

        /// <summary>
        /// Gets entity history based on entity id from the history of entity collection.
        /// </summary>
        /// <param name="entityId">id of an entity for which history is needed </param>
        /// <returns>IEntityHistory</returns>
        IEntityHistory GetEntityHistory(Int64 entityId);
    }
}
