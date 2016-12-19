using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the application context.
    /// </summary>
    public interface IApplicationContext : IMDMObject
    {
        #region Fields
        /// <summary>
        /// Property denoting attribute id for the data model context
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property denoting attribute name for the data model context
        /// </summary>
        String AttributeName { get; set; }

        /// <summary>
        /// Property denoting attribute long name for the data model context
        /// </summary>
        String AttributeLongName { get; set; }
    
        /// <summary>
        /// Property denoting category id for the data model context
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting category name for the data model context
        /// </summary>
        String CategoryName { get; set; }

        /// <summary>
        /// Property denoting category long name for the data model context
        /// </summary>
        String CategoryLongName { get; set; }

		/// <summary>
        /// Property denoting category path for the data model context
		/// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Property denoting container id for the data model context
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting container name for the data model context
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Property denoting container long name for the data model context
        /// </summary>
        String ContainerLongName { get; set; }

        /// <summary>
        /// Property denoting entity id for the data model context
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting entity name for the data model context
        /// </summary>
        String EntityName { get; set; }

        /// <summary>
        /// Property denoting entity long name for the data model context
        /// </summary>
        String EntityLongName { get; set; }

        /// <summary>
        /// Property denoting entity type id for the data model context
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting entity type name for the data model context
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting entity type long name for the data model context
        /// </summary>
        String EntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting locale for the data model context
        /// </summary>
        new String Locale { get; set; }

        /// <summary>
        /// Property denoting organization id for the data model context
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting organization Name for the data model context
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting organization long Name for the data model context
        /// </summary>
        String OrganizationLongName { get; set; }

        /// <summary>
        /// Property denoting relation type id for the data model context
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Property denoting relation type name for the data model context
        /// </summary>
        String RelationshipTypeName { get; set; }

        /// <summary>
        /// Property denoting relationshiptype long name for the data model context
        /// </summary>
        String RelationshipTypeLongName { get; set; }

        /// <summary>
        /// Property denoting role for the data model context
        /// </summary>
        Int32 RoleId { get; set; }

        /// <summary>
        /// Property denoting role name for the data model context
        /// </summary>
        String RoleName { get; set; }

        /// <summary>
        /// Property denoting role long name for the data model context
        /// </summary>
        String RoleLongName { get; set; }

        /// <summary>
        /// Property denoting user for the data model context
        /// </summary>
        Int32 UserId { get; set; }

        /// <summary>
        /// Property denoting user name for the data model context
        /// </summary>
        new String UserName { get; set; }

        /// <summary>
		/// Property denoting context type for the application context
		/// </summary>
        ApplicationContextType ContextType { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// XML Representation of the Object
        /// </summary>
        /// <returns>XML String of an Object</returns>
        String ToXml();

        /// <summary>
        /// Gets the EventSubscriber name
        /// </summary>
        /// <returns>Event Subscriber name as string</returns>
        String GetEventSubscriberName();

        #endregion
    }
}
