using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get application configuration collection of <see cref="IAppConfig"/> elements.
    /// </summary>
    public interface IAppConfigCollection : ICollection<AppConfig>
    {
        /// <summary>
        /// Determines whether the IAppConfigCollection contains a specific value.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if item is found in the IAppConfigCollection; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(int id);

        /// <summary>
        /// Removes the first occurrence of a specific object from the IAppConfigCollectio.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns><c>true</c> if item was successfully removed from the IAppConfigCollectio; otherwise, <c>false</c>. 
        /// This method also returns false if item is not found in the original IAppConfigCollectio.</returns>
        bool Remove(int id);

        /// <summary>
        /// Get Xml representation of IAppConfigCollection
        /// </summary>
        /// <returns>Xml representation of IAppConfigCollection</returns>
        string ToXml();

        /// <summary>
        /// Get Xml representation of IAppConfigCollection
        /// </summary>
        /// <param name="serialization">The serialization options.</param>
        /// <returns>Xml representation of IAppConfigCollection</returns>
        string ToXml(ObjectSerialization serialization);
    }
}
