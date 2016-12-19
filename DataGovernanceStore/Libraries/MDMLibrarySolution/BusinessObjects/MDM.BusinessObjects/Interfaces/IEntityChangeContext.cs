using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity change context, which indicates information that has changed under the entity object.
    /// </summary>
    public interface IEntityChangeContext
    {
        #region Properties

        /// <summary>
        /// Property denoting list of AttributeId
        /// </summary>
        Collection<Int32> AttributeIdList
        {
            get;
        }

        /// <summary>
        /// Property denoting list of attribute Name
        /// </summary>
        Collection<String> AttributeNameList
        {
            get;
        }

        /// <summary>
        /// Property denoting list of relationshipTypeId 
        /// </summary>
        Collection<Int32> RelationshipTypeIdList
        {
            get;
        }

        /// <summary>
        /// Property denoting list of relationshipTypeName 
        /// </summary>
        Collection<String> RelationshipTypeNameList
        {
            get;
        }

        /// <summary>
        /// Property denoting list of relationship attribute id
        /// </summary>
        Collection<Int32> RelationshipAttributeIdList
        {
            get;
        }

        /// <summary>
        /// Property denoting list of Relationship attribute name
        /// </summary>
        Collection<String> RelationshipAttributeNameList
        {
            get;
        }

        /// <summary>
        /// Property denoting category id
        /// </summary>
        Int64 CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting category name
        /// </summary>
        String CategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Category path
        /// </summary>
        String CategoryPath
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Hierarchy changed or not
        /// </summary>
        Boolean IsHierarchyChanged
        {
            get;
        }

        /// <summary>
        /// Property denoting attributes changed or not
        /// </summary>
        Boolean IsAttributesChanged
        {
            get;
        }

        /// <summary>
        /// Property denoting relationships changed or not
        /// </summary>
        Boolean IsRelationshipsChanged
        {
            get;
        }

        /// <summary>
        /// Property denoting extension changed or not
        /// </summary>
        Boolean IsExtensionsChanged
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Represents IEntityChangeContext instance in Xml format
        /// </summary>
        /// <returns>IEntityChangeContext instance in Xml format</returns>
        String ToXml();

        #endregion
    }
}
