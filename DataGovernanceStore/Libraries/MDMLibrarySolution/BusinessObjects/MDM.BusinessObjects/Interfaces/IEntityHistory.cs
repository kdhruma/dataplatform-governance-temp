using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get entity level history for a single entity.
    /// </summary>
    public interface IEntityHistory
    {
        #region Properties

        /// <summary>
        /// Property denoting entity id for current history record 
        /// </summary>
        Int64 EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting entity id for current history record 
        /// </summary>
        String EntityLongName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting current entity's entity type long name for which history is requested.
        /// </summary>
        String EntityTypeLongName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting current entity's catalog long name for which history is requested.        
        /// </summary>
        String EntityCatalogLongName
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iEntityHistoryRecord">Object which implements interface <see cref="IEntityHistoryRecord"/></param>
        void Add(IEntityHistoryRecord iEntityHistoryRecord);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="iEntityHistoryRecord"> Entity History Record object</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(IEntityHistoryRecord iEntityHistoryRecord);

        /// <summary>
        /// Removes item
        /// </summary>
        /// <param name="iEntityHistoryRecord">item which needs to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(IEntityHistoryRecord iEntityHistoryRecord);

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        String ToXml();

        /// <summary>
        /// Clones object
        /// </summary>
        /// <returns>Cloned object</returns>
        IEntityHistory Clone();
        
        #endregion

    }
}
