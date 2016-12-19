using System;
namespace MDM.Interfaces
{

    /// <summary>
    /// Exposes methods used for defining server information collection.
    /// </summary>
    public interface IServerInfoCollection
    {
        #region Methods

        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iServerInfo">Object which implements interface <see cref="IServerInfo"/></param>
        void Add(IServerInfo iServerInfo);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="iServerInfo"> server info object</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(IServerInfo iServerInfo);

        /// <summary>
        /// Removes item
        /// </summary>
        /// <param name="iServerInfo">item which needs to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(IServerInfo iServerInfo);

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        String ToXml();

        /// <summary>
        /// Clones object
        /// </summary>
        /// <returns>Cloned object</returns>
        IServerInfoCollection Clone();

        #endregion
    }
}
