using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the strongly typed entity base class
    /// </summary>
    [ProtoContract]
    public class StronglyTypedEntityBase : IStronglyTypedEntity
    {
        /// <summary>
        /// Property denoting the id of an entity
        /// </summary>
        [ProtoMember(1)]
        public Int64 Id { get; set; }

        /// <summary>
        /// Property denoting the name of an entity. Name often refers to the ShortName
        /// </summary>
        [ProtoMember(2)]
        public String Name { get; set; }

        /// <summary>
        /// Property denoting the long name of an entity
        /// </summary>
        [ProtoMember(3)]
        public String LongName { get; set; }

        /// <summary>
        /// Property denoting the path of an entity
        /// </summary>
        [ProtoMember(4)]
        public String Path { get; set; }

        /// <summary>
        /// Property denoting the ID path of an entity
        /// </summary>
        [ProtoMember(5)]
        public String IdPath { get; set; }

        /// <summary>
        /// Property denoting the external id of an entity
        /// </summary>
        [ProtoMember(6)]
        public String ExternalId { get; set; }

        /// <summary>
        /// Property denoting SKU of an Entity
        /// </summary>
        [ProtoMember(7)]
        public String SKU { get; set; }

        /// <summary>
        /// Property denoting the Category path of an entity
        /// </summary>
        [ProtoMember(8)]
        public String CategoryPath { get; set; }

        /// <summary>
        /// Property denoting the Category long name path of an entity
        /// </summary>
        [ProtoMember(9)]
        public String CategoryLongNamePath { get; set; }

        /// <summary>
        /// Property denoting the category id of an entity
        /// </summary>
        [ProtoMember(10)]
        public Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting the category name of an entity
        /// </summary>
        [ProtoMember(11)]
        public String CategoryName { get; set; }

        /// <summary>
        /// Property denoting the container long name of an entity
        /// </summary>
        [ProtoMember(12)]
        public String CategoryLongName { get; set; }

        /// <summary>
        /// Property denoting the container of an entity
        /// </summary>
        [ProtoMember(13)]
        public Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting the container of an entity
        /// </summary>
        [ProtoMember(14)]
        public String ContainerName { get; set; }

        /// <summary>
        /// Property denoting the container long name of an entity
        /// </summary>
        [ProtoMember(15)]
        public string ContainerLongName { get; set; }

        /// <summary>
        /// Indicates the Organization id of an Entity
        /// </summary>
        [ProtoMember(16)]
        public Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting organization name of an entity
        /// </summary>
        [ProtoMember(17)]
        public String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting the type id of an entity
        /// </summary>
        [ProtoMember(18)]
        public Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the type short name of an entity
        /// </summary>
        [ProtoMember(19)]
        public String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting the type long name of an entity
        /// </summary>
        [ProtoMember(20)]
        public String EntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting the parent Id of an entity
        /// </summary>
        [ProtoMember(21)]
        public Int64 ParentEntityId { get; set; }

        /// <summary>
        /// Property denoting the parent short name of this entity object
        /// </summary>
        [ProtoMember(22)]
        public String ParentEntityName { get; set; }

        /// <summary>
        /// Property denoting the parent short name of this entity object
        /// </summary>
        [ProtoMember(23)]
        public String ParentEntityLongName { get; set; }

        /// <summary>
        /// Property denoting the parent entity type id of an entity
        /// </summary>
        [ProtoMember(24)]
        public Int32 ParentEntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the parent extension Id of an entity
        /// </summary>
        [ProtoMember(25)]
        public Int64 ParentExtensionEntityId { get; set; }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [ProtoMember(26)]
        public String ParentExtensionEntityExternalId { get; set; }

        /// <summary>
        /// Property denoting the parent extension name of an entity
        /// </summary>
        [ProtoMember(27)]
        public String ParentExtensionEntityName { get; set; }

        /// <summary>
        /// Property denoting the parent extension long name of an entity
        /// </summary>
        [ProtoMember(28)]
        public String ParentExtensionEntityLongName { get; set; }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [ProtoMember(29)]
        public Int32 ParentExtensionEntityContainerId { get; set; }

        /// <summary>
        /// Property denoting the parent extension container name of an entity
        /// </summary>
        [ProtoMember(30)]
        public String ParentExtensionEntityContainerName { get; set; }

        /// <summary>
        /// Property denoting the parent extension container long name of an entity
        /// </summary>
        [ProtoMember(31)]
        public String ParentExtensionEntityContainerLongName { get; set; }

        /// <summary>
        /// Property denoting the parent extension category id of an entity
        /// </summary>
        [ProtoMember(32)]
        public Int64 ParentExtensionEntityCategoryId { get; set; }

        /// <summary>
        /// Property denoting the parent extension category name of an entity
        /// </summary>
        [ProtoMember(33)]
        public String ParentExtensionEntityCategoryName { get; set; }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [ProtoMember(34)]
        public String ParentExtensionEntityCategoryLongName { get; set; }

        /// <summary>
        /// Property denoting the parent extension category long name of an entity
        /// </summary>
        [ProtoMember(35)]
        public String ParentExtensionEntityCategoryLongNamePath { get; set; }

        /// <summary>
        /// Property denoting the parent extension category path of an entity
        /// </summary>
        [ProtoMember(36)]
        public String ParentExtensionEntityCategoryPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<Int32> GetAttributeIdsToLoad()
        {
            IEnumerable<StronglyTypedMetaDataAttribute> attributes = this.GetType().GetProperties().SelectMany(a => a.GetCustomAttributes(typeof(StronglyTypedMetaDataAttribute), true)).Cast<StronglyTypedMetaDataAttribute>();
            return new Collection<Int32>(attributes.Select(a => a.AttributeId).ToList());
        }

    }
}
