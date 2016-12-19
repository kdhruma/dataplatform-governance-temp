using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get copy-paste context, which indicates information to be loaded for copy-paste of Entity object.
    /// </summary>
    public interface IEntityCopyPasteContext
    {
        #region Properties

        /// <summary>
        /// Property denoting Source Entity Id
        /// </summary>
        Int64 FromEntityId { get; set; }

        /// <summary>
        /// Property denoting Source Container Id
        /// </summary>
        Int32 FromContainerId { get; set; }

        /// <summary>
        /// Property denoting Target Entity Ids
        /// </summary>
        Collection<Int64> ToEntityIds { get; set; }

        /// <summary>
        /// Property denoting Target Container Id
        /// </summary>
        Int32 ToContainerId { get; set; }

        /// <summary>
        /// Property denoting source to target locale mappings
        /// </summary>
        Dictionary<LocaleEnum, LocaleEnum> LocaleMappings { get; set; }

        /// <summary>
        /// Property denoting Ids of Attribute for copy-paste
        /// </summary>
        Collection<Int32> AttributeIds { get; set; }

        /// <summary>
        /// Property denoting Ids of RelationshipType for copy-paste
        /// </summary>
        Collection<Int32> RelationshipTypeIds { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents EntityCopyPasteContext  in Xml format
        /// </summary>
        /// <returns>
        /// EntityCopyPasteContext  in Xml format
        /// </returns>
        String ToXml();

        #endregion
    }
}
