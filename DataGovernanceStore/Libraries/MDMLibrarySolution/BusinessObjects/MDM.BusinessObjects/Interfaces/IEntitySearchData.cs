using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity search data.
    /// </summary>
    public interface IEntitySearchData : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Id of an Entity.
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting Id of Container id
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting value that needs to be searched.
        /// </summary>
        String SearchValue { get; set; }

        /// <summary>
        /// Property denoting Keyvalue for search.
        /// </summary>
        String KeyValue { get; set; }

        /// <summary>
        /// Property denoting IdPath for Serach.
        /// </summary>
        String IdPath { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of EntitySearchData
        /// </summary>
        /// <returns>Xml representation of EntitySearchData</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of EntitySearchData based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of EntitySearchData</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
