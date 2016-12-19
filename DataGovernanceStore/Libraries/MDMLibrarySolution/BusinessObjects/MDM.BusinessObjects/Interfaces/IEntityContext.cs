using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity context, which indicates information to be loaded into the Entity object.
    /// </summary>
    public interface IEntityContext : IEntityDataContext
    {
        #region Properties

        /// <summary>
        /// Property denoting container id to which entity belongs
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting category id to which entity belongs
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting entity type id of current entity
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting state view id.
        /// </summary>
        Int32 StateViewId { get; set; }

        /// <summary>
        /// Property denoting state view name.
        /// </summary>
        String StateViewName { get; set; }

        /// <summary>
        /// Property denoting custom view id.
        /// </summary>
        Int32 CustomViewId { get; set; }

        /// <summary>
        /// Property denoting custom view name.
        /// </summary>
        String CustomViewName { get; set; }

        /// <summary>
        /// Property denoting whether to load only current attribute values (Either Inherited or Overridden based on availability) or not.
        /// </summary>
        Boolean LoadOnlyCurrentValues { get; set; }

        /// <summary>
        /// Property denoting attributes belonging to which group are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificGroupIdList.
        /// To load attributes from group ids given in AttributeGroupIdList, LoadAttributesWithSpecificGroupIdList should be  to true.
        /// </summary>
        Collection<Int32> AttributeGroupIdList { get; set; }

        /// <summary>
        /// Property denoting which attributes are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificAttributeIdList.
        /// To load attributes from attribute ids given in AttributeIdList, LoadAttributesWithSpecificGroupIdList should be  to true.
        /// </summary>
        Collection<Int32> AttributeIdList { get; set; }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as ShowAtCreation
        /// </summary>
        Boolean LoadCreationAttributes { get; set; }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as Required
        /// </summary>
        Boolean LoadRequiredAttributes { get; set; }

        /// <summary>
        /// Property denoting which type of attributes are to be loaded. Possible values <see cref="AttributeModelType"/>
        /// </summary>
        AttributeModelType AttributeModelType { get; set; }

        /// <summary>
        /// Property denoting whether to load AttributeModels or not.
        /// </summary>
        Boolean LoadAttributeModels { get; set; }

        /// <summary>
        /// Property denoting whether to load lookup display values or not.
        /// </summary>
        Boolean LoadLookupDisplayValues { get; set; }

        /// <summary>
        /// Property denoting whether to load dependent attribute or not
        /// </summary>
        Boolean LoadDependentAttributes { get; set; }

        /// <summary>
        /// Property denoting whether to load attributes having blank values / no value
        /// When set to true, Entity Get will return attribute object instances having blank / no value
        /// </summary>
        Boolean LoadBlankAttributes { get; set; }

        /// <summary>
        /// Property denoting shortname of the container
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Property denoting shortname of the category
        /// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Property denoting shortname of the entity type
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting attribute names
        /// </summary>
        Collection<String> AttributeNames { get; set; }

        /// <summary>
        /// Property denoting attribute group names
        /// </summary>
        Collection<String> AttributeGroupNames { get; set; }

        /// <summary>
        /// Property denoting whether to load LookupRow along with Value or not
        /// </summary>
        Boolean LoadLookupRowWithValues { get; set; }
        
        #endregion

        #region Methods

        #region Attribute Methods

        /// <summary>
        /// Adds attribute name in AttributeNames
        /// </summary>
        /// <param name="attributeName">Represents Attribute short name</param>
        void AddAttribute(String attributeName);

        /// <summary>
        /// Adds attribute group name unique identifier in AttributeGroupNames
        /// </summary>
        /// <param name="attributeGroupName">Represents Attribute Group name</param>
        void AddAttributeGroupName(String attributeGroupName);

        #endregion

        #region ToXml Methods

        /// <summary>
        /// Represents EntityContext  in Xml format
        /// </summary>
        /// <returns>
        /// EntityContext  in Xml format
        /// </returns>
        String ToXml();

        #endregion

        #endregion
    }
}