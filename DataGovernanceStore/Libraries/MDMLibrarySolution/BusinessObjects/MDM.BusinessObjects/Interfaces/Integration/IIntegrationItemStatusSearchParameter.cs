using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get key and its values for integration item status search.
    /// </summary>
    public interface IIntegrationItemStatusSearchParameter
    {
        #region Properties

        /// <summary>
        /// Indicates key for search. Typically Id of MDMObjectType or ExternalObjectType or DimensionType
        /// </summary>
        Int32 SearchKey{ get; set; }

        /// <summary>
        /// Indicates collection of values for given key. Typically Ids of MDMObject or ExternalId or StatusType (Error, info etc).
        /// </summary>
        Collection<String> SearchValues { get; set; }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Represents object in xml format
        /// </summary>
        /// <returns>Xml representation of IIntegrationItemStatusSearchParameter</returns>
        String ToXml();

        /// <summary>
        /// Return new object with the values same as current one
        /// </summary>
        /// <returns>New object with same values as current one</returns>
        IIntegrationItemStatusSearchParameter Clone();

        #endregion Public methods
    }
}
