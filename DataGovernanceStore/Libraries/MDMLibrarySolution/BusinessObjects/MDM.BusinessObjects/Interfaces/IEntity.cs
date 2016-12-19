using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using Core;
    using BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for providing entity related information.
    /// </summary>
    public interface IEntity : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates the Id of the entity
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Indicates the reference Id of the entity for which results are created
        /// </summary>
        new Int64 ReferenceId { get; set; }

        /// <summary>
        /// Indicates the external Id for the entity object
        /// </summary>
        String ExternalId { get; set; }

        /// <summary>
        /// Indicates the parent external Id for the entity object
        /// </summary>
        String ParentExternalId { get; set; }

        /// <summary>
        /// Indicates the parent Id of the entity
        /// </summary>
        Int64 ParentEntityId { get; set; }

        /// <summary>
        /// Indicates the parent short name of the entity
        /// </summary>
        String ParentEntityName { get; set; }

        /// <summary>
        /// Indicates the parent long name of the entity
        /// </summary>
        String ParentEntityLongName { get; set; }

        /// <summary>
        /// Indicates the parent extension Id of the entity
        /// </summary>
        Int64 ParentExtensionEntityId { get; set; }

        /// <summary>
        /// Indicates the parent extension name of the entity
        /// </summary>
        String ParentExtensionEntityName { get; set; }

        /// <summary>
        /// Indicates the parent extension long name of the entity
        /// </summary>
        String ParentExtensionEntityLongName { get; set; }

        /// <summary>
        /// Indicates the parent extension external Id of the entity
        /// </summary>
        String ParentExtensionEntityExternalId { get; set; }

        /// <summary>
        /// Indicates the parent extension external Id of the entity
        /// </summary>
        Int32 ParentExtensionEntityContainerId { get; set; }

        /// <summary>
        /// Indicates the parent extension external Id of the entity
        /// </summary>
        String ParentExtensionEntityContainerName { get; set; }

        /// <summary>
        /// Indicates the parent extension external Id of the entity
        /// </summary>
        String ParentExtensionEntityContainerLongName { get; set; }

        /// <summary>
        /// Indicates the parent extension external Id of the entity
        /// </summary>
        Int64 ParentExtensionEntityCategoryId { get; set; }

        /// <summary>
        /// Indicates the parent extension external Id of the entity
        /// </summary>
        String ParentExtensionEntityCategoryName { get; set; }

        /// <summary>
        /// Indicates the parent extension external Id of the entity
        /// </summary>
        String ParentExtensionEntityCategoryLongName { get; set; }

        /// <summary>
        /// Indicates the parent extension category short name path of the entity
        /// </summary>
        String ParentExtensionEntityCategoryPath { get; set; }

        /// <summary>
        /// Indicates the parent extension category long name path of the entity
        /// </summary>
        String ParentExtensionEntityCategoryLongNamePath { get; set; }

        /// <summary>
        /// Indicates the category Id of the entity
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Indicates the category path of the entity
        /// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Indicates the category long name path of the entity
        /// </summary>
        String CategoryLongNamePath { get; set; }

        /// <summary>
        /// Indicates the Id path of the entity
        /// </summary>
        String IdPath { get; }

        /// <summary>
        /// Indicates the category name of the entity
        /// </summary>
        String CategoryName { get; set; }

        /// <summary>
        /// Indicates the container long name of the entity
        /// </summary>
        String CategoryLongName { get; set; }

        /// <summary>
        /// Indicates the container of the entity
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Indicates the container of the entity
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Indicates the container long name of the entity
        /// </summary>
        String ContainerLongName { get; set; }

        /// <summary>
        /// Indicates the type Id of the entity
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Indicates the type short name of the entity
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Indicates the type name of the entity
        /// </summary>
        String EntityTypeLongName { get; set; }

        /// <summary>
        /// Indicates the type Id of parent entity
        /// </summary>
        Int32 ParentEntityTypeId { get; }

        /// <summary>
        /// Indicates the Organization Id of the entity
        /// </summary>
        Int32 OrganizationId { get; }

        /// <summary>
        /// Indicates organization name of the entity
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Indicates the path of the entity
        /// </summary>
        String Path { get; }

        /// <summary>
        /// Indicates the branch level of the entity
        /// </summary>
        ContainerBranchLevel BranchLevel { get; }

        /// <summary>
        /// Indicates SKU of the entity
        /// </summary>
        String SKU { get; set; }

        /// <summary>
        ///Indicates the permission set for the current entity
        /// </summary>
        Collection<UserAction> PermissionSet { get; set; }

        /// <summary>
        /// Indicates the source of changes info for entity
        /// </summary>
        SourceInfo SourceInfo { get; set; }

        /// <summary>
        /// Specifies entity family id for a variant tree
        /// </summary>
        Int64 EntityFamilyId { get; set; }

        /// <summary>
        /// Specifies entity family name for a variant tree
        /// </summary>
        String EntityFamilyLongName { get; set; }

        /// <summary>
        /// Specifies entity global family id across parent(including extended families)
        /// </summary>
        Int64 EntityGlobalFamilyId { get; set; }

        /// <summary>
        /// Specifies entity global family name across parent(including extended families)
        /// </summary>
        String EntityGlobalFamilyLongName { get; set; }

        /// <summary>
        /// Indicates the entity hierarchy level
        /// </summary>
        Int16 HierarchyLevel { get; set; }
         
        /// <summary>
        /// Indicates the entity guid
        /// </summary>
        Guid? EntityGuid { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Gets XML representation of entity object
        /// </summary>
        /// <returns>XML representation of entity object</returns>
        String ToXml();

        /// <summary>
        /// Gets XML representation of entity object
        /// </summary>
        /// <param name="objectSerialization">Indicates serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>XML representation of Entity object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Gets XML representation of entity object
        /// </summary>
        /// <param name="objectSerialization">Indicates serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="serializeOnlyRootElements">
        /// Indicates whether to serialize whole entity object including attributes and relationships or serialize only entity metadata
        /// <para>
        /// if true then returns only entity metadata. If false then returns attributes also.
        /// </para>
        /// </param>
        /// <returns>XML representation of Entity object</returns>
        String ToXml(ObjectSerialization objectSerialization, Boolean serializeOnlyRootElements);

        #endregion ToXml methods

        #region Attribute Get for current entity

        /// <summary>
        /// Gets the attributes of the entity 
        /// </summary>
        /// <returns>Attribute collection containing the attributes of the entity</returns>
        IAttributeCollection GetAttributes();

        /// <summary>
        /// Gets the attributes of the entity based on the attribute model type 
        /// </summary>
        /// <param name="attributeModelType">Indicates attribute model type of the attributes</param>
        /// <returns>Attribute collection containing attributes of the entity with the specified attribute model type</returns>
        IAttributeCollection GetAttributes(AttributeModelType attributeModelType);

        /// <summary>
        /// Gets the attributes of the entity in the specificed locale along with nonlocalizable attributes.
        /// If locale is unknown then method doesn't apply filtering
        /// </summary>
        /// <param name="locale">Indicates locale in which attribute collection is required</param>
        /// <returns>Attribute collection containing attributes of the entity in the specified locale</returns>
        IAttributeCollection GetAttributes(LocaleEnum locale);

        /// <summary>
        /// Gets an attribute of the entity based on the attribute Id
        /// </summary>
        /// <param name="attributeId">Indicates identifier of an attribute</param>
        /// <returns>Attribute of the entity with the specified attribute Id</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        IAttribute GetAttribute(Int32 attributeId);

        /// <summary>
        /// Gets an attribute of the entity based on attribute unique identifier
        /// </summary>
        /// <param name="attributeUId">Indicates attribute unique identifier which identifies an attribute uniquely</param>
        /// <returns>Attribute of the entity with the specified attribute unique identifier</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId);

        /// <summary>
        /// Gets an attribute of the entity in the specified locale based on the attribute Id
        /// </summary>
        /// <param name="attributeId">Indicates identifier of an attribute</param>
        /// <param name="locale">Indicates locale in which the attribute is required</param>
        /// <returns>Attribute of the entity with the attribute Id in the specified locale </returns>
        IAttribute GetAttribute(Int32 attributeId, LocaleEnum locale);

        /// <summary>
        /// Gets an attribute of the entity based on attribute unique identifier
        /// </summary>
        /// <param name="attributeUId">Indicates attribute unique identifier which identifies an attribute uniquely</param>
        /// <param name="locale">Indicates the locale in which the attribute is required</param>
        /// <returns>Attribute of the entity with the attribute unique identifier in the specified locale</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="ArgumentException">AttributeUniqueIdentifier.AttributeGroupName and AttributeUniqueIdentifier.AttributeName is either null or empty</exception>
        IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId, LocaleEnum locale);

        /// <summary>
        /// Gets an attribute of the entity based on the specified attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\Attribute\SimpleAttributeSamples.cs" region="Get Attribute By Attribute Short Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Indicates the attribute short name</param>
        /// <returns>Attribute of the entity with the specified short name</returns>
        /// <exception cref="ArgumentException">Attribute name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName);

        /// <summary>
        /// Gets an attribute of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name for Locale"  source="..\MDM.APISamples\EntityManager\Attribute\SimpleAttributeSamples.cs" region="Get Attribute By Attribute Short Name for Locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates the locale in which attribute is required</param>
        /// <returns>Attribute of the entity with the attribute short name in the specified locale</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName, LocaleEnum locale);

        /// <summary>
        /// Gets an attribute of the entity based on the specified attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name And Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\Attribute\SimpleAttributeSamples.cs" region="Get Attribute By Attribute Short Name And Attribute Parent Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <returns>Attribute of the entity with the specified attribute short name and attribute parent short name</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName, String attributeParentName);

        /// <summary>
        /// Gets an attribute of the entity in the specified locale based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name And Attribute Parent Name for Locale"  source="..\MDM.APISamples\EntityManager\Attribute\SimpleAttributeSamples.cs" region="Get Attribute By Attribute Short Name And Attribute Parent Name for Locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates the locale in which attribute is required</param>
        /// <returns>Attribute of the entity with the attribute short name and attribute parent short name in the specified locale </returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName, String attributeParentName, LocaleEnum locale);

        /// <summary>
        /// Gets the common attributes of the entity
        /// </summary>
        /// <returns>Attribute collection containing common attributes of the entity</returns>
        IAttributeCollection GetCommonAttributes();

        /// <summary>
        /// Gets the category specific attributes of the entity
        /// </summary>
        /// <returns>Attribute collection containing category specific attributes of the entity</returns>
        IAttributeCollection GetCategorySpecificAttributes();

        /// <summary>
        /// Gets the system attributes of the entity
        /// </summary>
        /// <returns>Attribute collection containing system specific attributes of the entity </returns>
        IAttributeCollection GetSystemAttributes();

        /// <summary>
        /// Gets the relationships of the entity 
        /// </summary>
        ///  <example>
        ///		<code language="c#" title="Sample: Get entity with relationships" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And Relationships" />
        ///	</example>    
        /// <returns>Relationship collection interface</returns>
        IRelationshipCollection GetRelationships();

        /// <summary>
        /// Sets relationships for the entity 
        /// </summary>
        /// <param name="iRelationshipCollection">Indicates a collection of relationships to be set for the entity</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship collection is null</exception>
        void SetRelationships(IRelationshipCollection iRelationshipCollection);

        #endregion Attribute Get for current entity

        #region Attribute Value Get

        #region Current value

        /// <summary>
        /// Gets the current attribute value of the entity based on the attribute short name 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <returns>Current value of an attribute with the attribute short name from the entity's attributes</returns>
        IValue GetAttributeCurrentValueInstance(String attributeName);

        /// <summary>
        /// Gets the current attribute value of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance By Attribute Short Name in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates the locale in which attribute value should returned </param>
        /// <returns>Current value of an attribute with the attribute short name in the specified locale from the entity's attributes</returns>
        IValue GetAttributeCurrentValueInstance(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the current invariant attribute value of the entity based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Invariant Attribute Value by Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <returns>Current invariant value of an attribute with the specified attribute short name from the entity's attributes</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName);

        /// <summary>
        /// Gets the current invariant attribute value of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Invariant Attribute Value by Attribute Short Name in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <returns>Current invariant value of an attribute with the attribute short name in the specified locale from the entity's attributes</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the current attribute value of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Attribute Value by Attribute Short Name And Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name And Attribute Parent Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        ///  <returns>Current value of an attribute with the specified attribute short name and attribute parent short name from the entity's attributes</returns>
        IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the current attribute value of the entity in the specified locale based on attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance By Attribute Short Name And Attribute Parent Name in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name And Attribute Parent Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <returns>Current value of an attribute with the attribute short name and attribute parent short name in the specified locale from the entity's attributes  </returns>
        IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName, LocaleEnum locale);

        /// <summary>
        /// Gets the current invariant attribute value of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Invariant Attribute Value by Attribute Short Name and Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name And Attribute Parent Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <returns>Current invariant value of an attribute with the specified attribute short name and attribute parent short name from the entity's attributes</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the current invariant attribute value of the entity in the specified locale based on attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Invariant Attribute Value by Attribute Short Name and Attribute Parent Name in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name And Attribute Parent Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <returns>Current invariant value of an attribute with the attribute short name and attribute parent short name in the specified locale from the entity's attributes</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale);

        #region Get Attribute Value as T type

        /// <summary>
        /// Gets the current attribute value of the entity based on the attribute short name and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Current Value By Attribute Short Name In Specified Type"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Current Value By Attribute Short Name In Specified Type"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current value of an attribute with the attribute short name in the specified data type from the entity's attributes </returns>
        T GetAttributeCurrentValue<T>(String attributeName);

        /// <summary>
        /// Gets the current attribute value of the entity based on attribute short name, attribute parent short name, and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Current Value By Attribute Short Name And Attribute Parent Name In Specified Type"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Current Value By Attribute Short Name And Attribute Parent Name In Specified Type"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current value of an attribute with the attribute short name and attribute parent short name in the specified data type from the entity's attributes</returns>
        T GetAttributeCurrentValue<T>(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the current attribute value of the entity in the specified locale based attribute short name and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Current Value By Attribute Short Name In Specified Type in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Current Value By Attribute Short Name In Specified Type in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current value of an attribute with the attribute short name in the specfied locale and data type from the entity's attributes</returns>
        T GetAttributeCurrentValue<T>(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the current attribute value of the entity in the specified locale based on the attribute short name, attribute parent short name, and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Current Value By Attribute Short Name And Attribute Parent Name In Specified Type in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Current Value By Attribute Short Name And Attribute Parent Name In Specified Type in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current value of an attribute with the attribute short name and attribute parent short name in the specfied locale and data type from the entity's attributes</returns>
        T GetAttributeCurrentValue<T>(String attributeName, String attributeParentName, LocaleEnum locale);

        /// <summary>
        /// Gets the current invariant attribute value of the entity based on the attribute short name and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Current Value Invariant By Attribute Short Name In Specified Type"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Current Value Invariant By Attribute Short Name In Specified Type"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current invariant value of an attribute with the attribute short name in the specified data type from the entity's attributes</returns>
        T GetAttributeCurrentValueInvariant<T>(String attributeName);

        /// <summary>
        /// Gets the current invariant attribute value of the entity based on the attribute short name, attribute parent short name, and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Current Value Invariant By Attribute Short Name And Attribute Parent Name In Specified Type"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Current Value Invariant By Attribute Short Name And Attribute Parent Name In Specified Type"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current invariant value of an attribute with the attribute short name and attribute parent short name in the specified data type from the entity's attributes</returns>
        T GetAttributeCurrentValueInvariant<T>(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the current invariant attribute value of the entity in the specified locale based on the attribute short name and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Current Value Invariant By Attribute Short Name In Specified Type in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Current Value Invariant By Attribute Short Name In Specified Type in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current invariant value of an attribute with the attribute short name in the specified data type and locale from the entity's attributes </returns>
        T GetAttributeCurrentValueInvariant<T>(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the current invariant attribute value of the entity in the specified locale based on the attribute short name, attribute parent short name, and data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Invariant By Attribute Short Name And Attribute Parent Name In Specified Type in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute CurrentValue Invariant By Attribute Short Name And Attribute Parent Name In Specified Type in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Current invariant value of an attribute with the attribute short name and attribute parent short name in the specified data type and locale from the entity's attributes</returns>
        T GetAttributeCurrentValueInvariant<T>(String attributeName, String attributeParentName, LocaleEnum locale);

        #endregion Get Attribute Value as T type

        #endregion

        #region Inherited Value

        /// <summary>
        /// Gets the current inherited attribute value of the entity based on the attribute short name 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Inherited Attribute Value by Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <returns>Current inherited value of an attribute with the attribute short name from the entity's attributes </returns>
        IValue GetAttributeInheritedValueInstance(String attributeName);

        /// <summary>
        /// Gets the inherited attribute value of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Inherited Attribute Value by Attribute Short Name and Locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates the locale in which the value should be formatted</param>
        /// <returns>Current inherited value of an attribute with the attribute short name in the specified locale from the entity's attributes</returns>
        IValue GetAttributeInheritedValueInstance(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the inherited invariant attribute value of the entity based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <returns>Current inherited invariant value of an attribute with the attribute short name from the entity's attributes </returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName);

        /// <summary>
        /// Gets the inherited invariant attribute value of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <returns>Current inherited invariant value of an attribute with the attribute short name in the specified locale from the entity's attributes</returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the inherited attribute value of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Inherited Attribute Value by Attribute Short Name and Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name And Attribute Parent Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <returns>Current inherited value of an attribute with the attribute short name and attribute parent short name from the entity's attributes</returns>
        IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the inherited attribute value of the entity in the specified locale based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Inherited Attribute Value by Attribute Short Name and Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name And Attribute Parent Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates the locale in which the value should be formatted</param>
        /// <returns>Current inherited value of an attribute in the specified locale with the attribute short name and attribute parent short name from the entity's attributes</returns>
        IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName, LocaleEnum locale);

        /// <summary>
        /// Gets the inherited invariant attribute value of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name And Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name And Attribute Parent Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <returns>Current inherited invariant value of an attribute with the attribute short name and attribute parent short name from the entity's attributes</returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the inherited invariant attribute value of the entity in the specified locale based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name And Attribute Parent Name in locale"  source="..\MDM.APISamples\EntityManager\AttributeValue\GetAttributeValue.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name And Attribute Parent Name in locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates the locale in which the value is required</param>
        /// <returns>Current inherited invariant value of an attribute in the specified locale with the attribute short name and attribute parent short name from the entity's attributes</returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale);

        #endregion

        #endregion

        #region Complex Attribute get utility methods

        #region Non collection attribute

        /// <summary>
        /// Gets the complex attribute of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex"  source="..\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex attribute of the entity with the attribute short name and attribute parent short name</returns>
        IComplexAttribute GetAttributeAsComplex(String attributeName, String parentName);

        /// <summary>
        /// Gets the complex attribute of the entity in the specified locale based on the attribute short name and attribute parent short name 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex in Specified Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute With Locale"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="locale">Indicates the locale in which complex attribute is required</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex attribute of the entity in the specified locale with the attribute short name and attribute parent short name</returns>
        IComplexAttribute GetAttributeAsComplex(String attributeName, String parentName, LocaleEnum locale);

        /// <summary>
        /// Gets the complex attribute of the entity based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex By Name"  source="..\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute By AtributeName Only"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex attributes of the entity with the attribute short name</returns>
        IComplexAttribute GetAttributeAsComplex(String attributeName);

        /// <summary>
        /// Gets the complex attribute of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex By Name and Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute By AttributeName And Locale Only"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex attributes of the entity in the specified locale with the attribute short name </returns>
        IComplexAttribute GetAttributeAsComplex(String attributeName, LocaleEnum locale);

        #endregion Non collection attribute

        #region Collection Attribute

        /// <summary>
        /// Gets the complex collection attribute of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex Collection"  source="..\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute Collection"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex collection attribute of the entity with the attribute short name and attribute parent short name</returns>
        IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName, String parentName);

        /// <summary>
        /// Gets the complex collection attribute of the entity in the specified locale based on the attribute short name and attribute parent short name 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex Collection By Attribute Name and Parent Name"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute Collection With Locale"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex collection attribute of the entity in the specified locale with the attribute short name and attribute parent short name </returns>
        IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName, String parentName, LocaleEnum locale);

        /// <summary>
        /// Gets complex collection attribute of the entity based on the attribute short name 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex Collection By Attribute Name"  source="..\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute Collection By AtributeName Only"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex collection attribute of the entity with the attribute short name  </returns>
        IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName);

        /// <summary>
        /// Gets complex collection attribute of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Complex Collection By attribute Name and Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get ComplexAttribute Collection By AttributeName And Locale Only"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Complex collection attribute of the entity in the specified locale with the attribute short name </returns>
        IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName, LocaleEnum locale);

        #endregion Collection Attribute

        #endregion Complex Attribute get utility methods

        #region Collection Attribute get utility methods

        /// <summary>
        /// Gets the collection attribute of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Collection Attribute By Attribute Short Name And Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Collection Attribute By Attribute Short Name And Attribute Parent Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Collection attribute of the entity with the attribute short name and attribute parent short name </returns>
        ICollectionAttribute GetAttributeAsCollection(String attributeName, String parentName);

        /// <summary>
        /// Gets the collection attribute of the entity in the specified locale based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Collection Attribute By Attribute Short Name And Attribute Parent Name in Locale"  source="..\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Collection Attribute By Attribute Short Name And Attribute Parent Name in Locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Collection attribute of the entity in the specified locale with the attribute short name and attribute parent short name</returns>
        ICollectionAttribute GetAttributeAsCollection(String attributeName, String parentName, LocaleEnum locale);

        /// <summary>
        /// Gets the collection attribute of the entity based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Collection Attribute By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Collection Attribute By Attribute Short Name"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Collection attribute of the entity with the attribute short name</returns>
        ICollectionAttribute GetAttributeAsCollection(String attributeName);

        /// <summary>
        /// Gets the collection attribute of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Collection Attribute By Attribute Short Name in Locale"  source="..\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Collection Attribute By Attribute Short Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes" source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Collection attribute of the entity in the specified locale with the attribute short name</returns>
        ICollectionAttribute GetAttributeAsCollection(String attributeName, LocaleEnum locale);

        #endregion

        #region Lookup Attribute Get Utility Methods

        #region Non collection attribute

        /// <summary>
        /// Gets the lookup attribute of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>        
        /// <code language="c#" title="Get Attribute As Lookup"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Lookup Attribute with attribute name and parent name"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup attribute of the entity with the attribute short name and attribute parent short name</returns>
        ILookupAttribute GetAttributeAsLookup(String attributeName, String parentName);

        /// <summary>
        /// Gets the lookup attribute of the entity in the specified locale based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Lookup"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Lookup Attribute with attribute name, parent name and locale"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup attribute of the entity in the specified locale with the attribute short name and attribute parent short name</returns>
        ILookupAttribute GetAttributeAsLookup(String attributeName, String parentName, LocaleEnum locale);

        /// <summary>
        /// Gets the lookup attribute of the entity based on the attribute short name 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Lookup"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Lookup Attribute with attribute name"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup attribute of the entity with the attribute short name </returns>
        ILookupAttribute GetAttributeAsLookup(String attributeName);

        /// <summary>
        /// Gets the lookup attribute of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Lookup"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Lookup Attribute with attribute name and locale"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup attribute of the entity in the specified locale with the attribute short name</returns>
        ILookupAttribute GetAttributeAsLookup(String attributeName, LocaleEnum locale);

        #endregion Non collection attribute

        #region Lookup Collection Attribute

        /// <summary>
        /// Gets the lookup collection attribute of the entity based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>        
        /// <code language="c#" title="Get Attribute As Lookup Collection"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Collection Lookup Attribute with attribute name and parent name"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup collection attribute of the entity with the attribute short name and attribute parent short name </returns>
        ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName, String parentName);

        /// <summary>
        /// Gets the lookup collection attribute of the entity in the specified locale based on the attribute short name and attribute parent short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Lookup Collection"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Collection Lookup Attribute with attribute name, parent name and locale"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup collection attribute of the entity in the specified locale with the attribute short name and attribute parent short name</returns>
        ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName, String parentName, LocaleEnum locale);

        /// <summary>
        /// Gets the lookup collection attribute of the entity based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Lookup Collection"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Collection Lookup Attribute with attribute name"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup collection attribute of the entity with the attribute short name</returns>
        ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName);

        /// <summary>
        /// Gets the lookup collection attribute of the entity in the specified locale based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute As Lookup Collection"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Collection Lookup Attribute with attribute name and locale"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="locale">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Lookup collection attribute of the entity in the specified locale with the attribute short name</returns>
        ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName, LocaleEnum locale);

        #endregion Lookup Collection Attribute

        #endregion

        #region Attribute Set

        /// <summary>
        /// Sets the attributes of the Entity
        /// </summary>
        /// <param name="iAttributeCollection">Indicates the attribute collection to be set</param>
        void SetAttributes(IAttributeCollection iAttributeCollection);

        #endregion Attribute Set

        #region Set Attribute Value

        /// <summary>
        /// Sets the attribute value for the attribute with the attribute short name in the current entity's attributes using IValue
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value By Attribute Short Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value By Attribute Short Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute Short Name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValue(String attributeName, IValue value);

        /// <summary>
        /// Sets the attribute value for the attribute with the attribute short name in the current entity's attributes using Object 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value By Attribute Short Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value By Attribute Short Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute Short name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValue(String attributeName, Object value);

        /// <summary>
        /// Sets the attribute value in the specified locale for the attribute with the attribute short name in the current entity's attributes using IValue
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value In The Specified Locale By Attribute Short Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value In The Specified Locale By Attribute Short Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValue(String attributeName, IValue value, LocaleEnum locale);

        /// <summary>
        /// Sets the attribute value in the specified locale for the attribute with the attribute short name in the current entity's attributes using Object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value In The Specified Locale By Attribute Short Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value In The Specified Locale By Attribute Short Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValue(String attributeName, Object value, LocaleEnum locale);

        /// <summary>
        /// Sets the attribute value in the specified locale for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using IValue
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value By Attribute Short Name And Attribute Parent Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value By Attribute Short Name And Attribute Parent Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates theattribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValue(String attributeName, String attributeParentName, IValue value);

        /// <summary>
        /// Sets the attribute value for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using Object 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value By Attribute Short Name And Attribute Parent Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value By Attribute Short Name And Attribute Parent Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValue(String attributeName, String attributeParentName, Object value);

        /// <summary>
        /// Sets the attribute value in the specified locale for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using IValue
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value In The Specified Locale By Attribute Short Name And Attribute Parent Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value In The Specified Locale By Attribute Short Name And Attribute Parent Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValue(String attributeName, String attributeParentName, IValue value, LocaleEnum locale);

        /// <summary>
        /// Sets the attribute value in the specified locale for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using Object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value In The Specified Locale By Attribute Short Name And Attribute Parent Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value In The Specified Locale By Attribute Short Name And Attribute Parent Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValue(String attributeName, String attributeParentName, Object value, LocaleEnum locale);

        /// <summary>
        /// Sets the invariant attribute value for the attribute with the attribute short name in the current entity's attributes using IValue 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant By Attribute Short Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant By Attribute Short Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValueInvariant(String attributeName, IValue value);

        /// <summary>
        /// Sets the invariant attribute value for the attribute with the attribute short name in the current entity's attributes using Object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant By Attribute Short Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant By Attribute Short Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValueInvariant(String attributeName, Object value);

        /// <summary>
        /// Sets the invariant attribute value in the specified locale for the attribute with the attribute short name in the current entity's attributes using IValue
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValueInvariant(String attributeName, IValue value, LocaleEnum locale);

        /// <summary>
        /// Sets the invariant attribute value in the specified locale for the attribute with the attribute short name in the current entity's attributes using Object 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValueInvariant(String attributeName, Object value, LocaleEnum locale);

        /// <summary>
        /// Sets the invariant attribute value for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using IValue
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant By Attribute Short Name And Attribute Parent Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant By Attribute Short Name And Attribute Parent Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates attribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValueInvariant(String attributeName, String attributeParentName, IValue value);

        /// <summary>
        /// Sets the invariant attribute value for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using Object 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant By Attribute Short Name And Attribute Parent Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant By Attribute Short Name And Attribute Parent Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates attribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        void SetAttributeValueInvariant(String attributeName, String attributeParentName, Object value);

        /// <summary>
        /// Sets the invariant attribute value in the specified locale for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using IValue 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And Attribute Parent Name And IValue As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And Attribute Parent Name And IValue As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates attribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValueInvariant(String attributeName, String attributeParentName, IValue value, LocaleEnum locale);

        /// <summary>
        /// Sets the invariant attribute value in the specified locale for the attribute with the attribute short name and attribute parent short name in the current entity's attributes using Object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And Attribute Parent Name And Object As Argument"  source="..\MDM.APISamples\EntityManager\AttributeValue\SetAttributeValue.cs" region="Set Attribute Value Invariant In The Specified Locale By Attribute Short Name And Attribute Parent Name And Object As Argument"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates attribute parent short name</param>
        /// <param name="value">Indicates the value to be set</param>
        /// <param name="locale">Indicates the locale in which the attribute values should be set</param>
        void SetAttributeValueInvariant(String attributeName, String attributeParentName, Object value, LocaleEnum locale);

        #endregion Attribute Set

        #region Get Contexts

        /// <summary>
        /// Gets the entity's context
        /// </summary>
        /// <returns>EntityContext interface</returns>
        /// <exception cref="NullReferenceException">Raised when EntityContext is null</exception>
        IEntityContext GetContext();

        /// <summary>
        /// Gets the entity's change context
        /// </summary>
        /// <param name="calculateLatest">
        /// Indicates whethere to calculate latest change context or not 
        /// (Default = false)
        /// </param>
        /// <returns></returns>
        IEntityChangeContext GetChangeContext(Boolean calculateLatest = false);

        /// <summary>
        /// Gets the entity's move context
        /// </summary>
        /// <returns>EntityMoveContext's Interface</returns>
        /// <exception cref="NullReferenceException">Raised when EntityMoveContext is null</exception>
        IEntityMoveContext GetEntityMoveContext();

        /// <summary>
        /// Gets the related entity's change context
        /// </summary>
        /// <returns>Related Entity Change Context</returns>
        IEntityChangeContext GetRelatedEntityChangeContext();

        #endregion

        #region Hierarchy Relationships methods

        /// <summary>
        /// Gets the hierarchy relationships
        /// </summary>
        ///  <example>
        ///		<code language="c#" title="Sample: Get entity with hierarchy relationships" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And HierarchyRelationships" />
        ///	</example>    	
        /// <returns>Hierarchy Relationship collection interface</returns>
        IHierarchyRelationshipCollection GetHierarchyRelationships();

        /// <summary>
        /// Sets the hierarchy relationships
        /// </summary>
        /// <param name="iHierarchyRelationshipCollection">Hierarchy Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed hierarchy relationship collection is null</exception>
        void SetHierarchyRelationships(IHierarchyRelationshipCollection iHierarchyRelationshipCollection);

        /// <summary>
        /// Gets all direct child entities (Use case: get all colors of style).
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get direct child entities using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entities based on root parent entity name"/>
        /// <code language="c#" title="Sample: Get the direct child entities using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entities based on root parent entity id"/>
        /// </example>
        /// <returns>Entity collection interface presenting all direct child entities</returns>
        IEntityCollection GetChildEntities();

        /// <summary>
        /// Gets direct parent entity (Use case: get color of sku).
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get direct parent entity using child entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity based on child entity name"/>
        /// <code language="c#" title="Sample: Get the direct parent entity using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity based on child entity id"/>
        /// </example>
        /// <returns>Entity interface presenting direct parent entity</returns>
        IEntity GetParentEntity();

        /// <summary>
        /// Gets all parent entities (Use case: get all parent entities color and style of sku).
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get all parent entities using child entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get all parent entities based on child entity name"/>
        /// <code language="c#" title="Sample: Get all parent entities using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get all parent entities based on child entity id"/>
        /// </example>
        /// <returns>Entity collection interface presenting all parent entities at all the levels</returns>
        IEntityCollection GetAllParentEntities();

        /// <summary>
        /// Gets all child entities at all levels (Use case: get all colors and skus of style).
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get all child entities using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get all child entities based on root parent entity name"/>
        /// <code language="c#" title="Sample: Get all child entities using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get all child entities based on root parent entity id"/>
        /// </example>
        /// <returns>Entity collection interface presenting all child entities at all the levels</returns>
        IEntityCollection GetAllChildEntities();

        /// <summary>
        /// Gets all child entities belong to provided entity type id (Use case: get all child entities of entity type id 7).
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get the child entities of given entity type ID using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entities by entity type id based on root parent entity id"/>
        /// </example>
        /// <param name="entityTypeId">Indicates identifier of entity type</param>
        /// <returns>Entity collection interface presenting child entities from all the levels and having provided entity type id</returns>
        IEntityCollection GetChildEntitiesByEntityTypeId(Int32 entityTypeId);

        /// <summary>
        /// Gets all child entities belong to provided entity type name (Use case: get all child entities of entity type "sku").
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get the child entities of given entity type using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entities by entity type name based on root parent entity name"/>
        /// <code language="c#" title="Sample: Get the child entities of given entity type name using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entities by entity type name based on root parent entity id"/>
        /// </example>
        /// <param name="entityTypeName">Indicates name of entity type</param>
        /// <returns>Entity collection interface presenting child entities from all the levels and having provided entity type name</returns>
        IEntityCollection GetChildEntitiesByEntityTypeName(String entityTypeName);

        /// <summary>
        /// Gets particular child entity based on provided child entity id (Use case: get child entity with id "1234").
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get a child entity of given entity ID using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entity by entity id based on root parent entity id"/>
        /// </example>
        /// <param name="entityId">Indicates identifier of child entity</param>
        /// <returns>Entity interface presenting child entity with provided entity id</returns>
        IEntity GetChildEntityByEntityId(Int64 entityId);

        /// <summary>
        /// Gets particular child entity based on provided child entity name / shortname / externalid (Use case: get child entity with name "Polo's Mens Slim Shirt:Red").
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get a child entity of given entity name using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entity by entity name based on root parent entity name"/>
        /// <code language="c#" title="Sample: Get a child entity of given entity name using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entity by entity name based on root parent entity id"/>
        /// </example>
        /// <param name="entityName">Indicates name of child entity</param>
        /// <returns>Entity interface presenting child entity with provided entity name</returns>
        IEntity GetChildEntityByEntityName(String entityName);

        /// <summary>
        /// Gets particular parent entity belong to provided entity type id (Use case: get parent enitty of entity type id 5).
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get the parent entity of given entity type ID using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity by parent entity type id based on child entity id"/>
        /// </example>
        /// <param name="entityTypeId">Indicates identifier of parent entity type</param>
        /// <returns>Entity interface presenting entity having provided entity type id</returns>
        IEntity GetParentEntityByEntityTypeId(Int32 entityTypeId);

        /// <summary>
        /// Gets particular parent entity belong to provided entity type name (Use case: get parent enitty of entity type name "style").
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get parent entity of given entity type using child entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity by parent entity type name based on child entity name"/>
        /// <code language="c#" title="Sample: Get the parent entity of given entity type name using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity by parent entity type name based on child entity id"/>
        /// </example>
        /// <param name="entityTypeName">Indicates name of entity type</param>
        /// <returns>Entity interface presenting entity having provided entity type name</returns>
        IEntity GetParentEntityByEntityTypeName(String entityTypeName);

        /// <summary>
        /// Gets particular parent entity based on provided parent entity id (use case: get parent entity with id "1111").
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get a parent entity of given entity ID using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity by parent entity id based on child entity id"/>
        /// </example>
        /// <param name="entityId">Indicates identifier of parent entity</param>
        /// <returns>Entity interface presenting parent entity with provided entity id</returns>
        IEntity GetParentEntityByEntityId(Int64 entityId);

        /// <summary>
        /// Gets particular parent entity based on provided parent entity name / shortname / externalid (Use case: get parent entity with name "Polo's Mens Slim Shirt").
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get parent entity of given entity name using child entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity by parent entity name based on child entity name"/>
        /// <code language="c#" title="Sample: Get a parent entity of given entity name using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get parent entity by parent entity name based on child entity id"/>
        /// </example>
        /// <param name="entityName">Indicates name of parent entity</param>
        /// <returns>Entity interface presenting parent entity with provided entity name</returns>
        IEntity GetParentEntityByEntityName(String entityName);

        /// <summary>
        /// Gets particular hierarchy entity based on provided filter / find expression.
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get a child entity with a specific attribute value using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entity in the entity hierarchy having a specific value for an attribute based on root parent entity id"/>
        /// </example>
        /// <param name="compareMethod">Indicates comparison expression</param>
        /// <param name="isRecursive">Indicates if filter has to be applied recursively in entity hierarchy tree</param>
        /// <returns>Entity interface presenting related hierarchy entity matching the provided comparison expression</returns>
        IEntity FindHierarchyRelatedEntity(Func<IHierarchyRelationship, Boolean> compareMethod, Boolean isRecursive = false);

        /// <summary>
        /// Gets particular hierarchy entities based on provided filter / find expression.
        /// Current entity must have hierarchy entities loaded using GetEntityHierarchy API before using this method else it would return null object
        /// </summary>
        /// <example>
        /// <code language="c#" title="Sample: Get child entities with specific attribute value using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="Get child entities of an entity in the entity hierarchy having a specific value for an attribute based on root parent entity name"/>
        /// </example>
        /// <param name="compareMethod">Indicates comparison expression</param>
        /// <param name="isRecursive">Indicates if filter has to be applied recursively in entity hierarchy tree</param>
        /// <returns>Entity interface presenting related hierarchy entities matching the provided comparison expression</returns>
        IEntityCollection FindHierarchyRelatedEntities(Func<IHierarchyRelationship, Boolean> compareMethod, Boolean isRecursive = false);

        #endregion

        #region Extension Relationship methods

        /// <summary>
        /// Gets the extension relationships
        /// </summary>
        ///  <example>
        ///		<code language="c#" title="Sample: Get entity with extension relationships" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And Extensions" />
        ///	</example>
        /// <returns>Extension Relationship collection interface</returns>
        IExtensionRelationshipCollection GetExtensionsRelationships();

        /// <summary>
        /// Sets the extension relationships
        /// </summary>
        /// <param name="iExtensionRelationshipCollection">Indicates the extension relationship collection to be set</param>
        /// <exception cref="ArgumentNullException">Raised when passed extension relationship collection is null</exception>
        void SetExtensionRelationships(IExtensionRelationshipCollection iExtensionRelationshipCollection);

        #endregion

        #region PermissionSet Methods

        /// <summary>
        /// Gets permission set for this entity in the current context
        /// </summary>
        /// <returns>Permission collection</returns>
        Collection<UserAction> GetPermissionSet();

        #endregion

        #region AttributeModel Methods

        /// <summary>
        /// Gets the attributeModels of the entity
        /// </summary>
        /// <returns>Attribute model collection of the entity</returns>
        IAttributeModelCollection GetAttributeModels();

        /// <summary>
        /// Sets the attributeModels of the Entity
        /// </summary>
        /// <param name="iAttributeModelCollection">Indicates the attribute model collection to be set</param>
        void SetAttributeModels(IAttributeModelCollection iAttributeModelCollection);

        /// <summary>
        /// Marks the entity object properties (AttributeModels and/or Relationships) as ReadOnly 
        /// If AttributeModels is set to ReadOnly then HierarchyRelationships and ExtensionRelationships are also set to ReadOnly
        /// </summary>
        /// <param name="markAttributeModelsAsReadOnly">Indicates whether to mark attribute models as readonly or not</param>
        /// <param name="markRelationshipsAsReadOnly">Indicates whether to mark relationships as readonly or not</param>
        void MarkAsReadOnly(Boolean markAttributeModelsAsReadOnly = true, Boolean markRelationshipsAsReadOnly = true);

        /// <summary>
        /// Gets the common attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Common AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Common AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale Attribute Models are required</param>
        /// <returns>Common attribute model collection of the entity in the specified locale</returns>
        IAttributeModelCollection GetCommonAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the required common attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Required Common AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Required Common AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale Attribute Models are required</param>
        /// <returns>Required common attribute model collection of the entity in the specified locale</returns>
        IAttributeModelCollection GetRequiredCommonAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets all the required and read only common attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Required ReadOnly Common AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Required ReadOnly Common AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale Attribute Models are required</param>
        /// <returns>Required and read only common attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetRequiredReadOnlyCommonAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the technical attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Technical AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Technical AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale Attribute Models are required</param>
        /// <returns>Technical attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetTechnicalAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the required technical attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Required Technical AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Required Technical AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale Attribute Models are required</param>
        /// <returns>Required technical attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetRequiredTechnicalAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the required and read only technical attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Required ReadOnly Technical AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Required ReadOnly Technical AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale AttributeModels are required</param>
        /// <returns>Required and read only technical attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetRequiredReadOnlyTechnicalAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the relationship attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Relationship AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Relationship AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale AttributeModels are required</param>
        /// <returns>Relationship attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetRelationshipAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the required relationship attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Required Relationship AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Required Relationship AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale AttributeModels are required</param>
        /// <returns>Required relationship attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetRequiredRelationshipAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the required and read only relationship attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Required ReadOnly Relationship AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get Required ReadOnly Relationship AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale AttributeModels are required</param>
        /// <returns>Required and read only relationship attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetRequiredReadOnlyRelationshipAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the ShowAtCreation attribute models of the entity in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get ShowAtCreation AttributeModels"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get ShowAtCreation AttributeModels"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale AttributeModels are required</param>
        /// <returns>ShowAtCreation attribute models of the entity in the specified locale</returns>
        IAttributeModelCollection GetShowAtCreationAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets the attribute models of the entity that have default values configured in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get AttributeModels With DefaultValues"  source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get AttributeModels With DefaultValues"/>
        /// <code language="c#" title="Get Entity In Specified Locale With AttributeModels"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityInSpecifiedLocaleWithAttributeModels"/>
        /// </example>
        /// <param name="locale">Indicates in which locale AttributeModels are required</param>
        /// <returns>Attribute models of the entity that have default values configured in the specified locale</returns>
        IAttributeModelCollection GetAttributeModelsWithDefaultValues(LocaleEnum locale);

        #endregion

        #region WorkflowInformation Methods

        /// <summary>
        /// Gets the entity's current workflow Information
        /// </summary>
        /// <returns>Entity's current workflow Information</returns>
        IWorkflowStateCollection GetWorkflowDetails();

        /// <summary>
        /// Gets the entity's current workflow action context
        /// </summary>
        /// <returns>Entity's current workflow action context</returns>
        IWorkflowActionContext GetWorkflowActionContext();

        /// <summary>
        /// Sets the workflow action context
        /// </summary>
        /// <param name="iWorkflowActionContext">Indicates the workflow action context to be set</param>
        void SetWorkflowActionContext(IWorkflowActionContext iWorkflowActionContext);

        #endregion

        #region Get OriginalEntity

        /// <summary>
        /// Gets the original entity of the current entity
        /// </summary>
        /// <returns>Original entity of the current entity</returns>
        IEntity GetOriginalEntity();

        #endregion

        #region Clone Methods

        /// <summary>
        /// Creates a new entity with basic properties of entity
        /// </summary>
        /// <returns>New entity instance having same properties like current entity</returns>
        Entity CloneBasicProperties();

        /// <summary>
        /// Creates a new entity of the entity
        /// </summary>
        /// <returns>New entity instance like current entity</returns>
        Entity Clone();

        #endregion

        #region   Validation State Methods

        /// <summary>
        ///  Get validation state.
        /// </summary>
        /// <returns>Returns validation state</returns>
        IValidationStates GetValidationStates();

        /// <summary>
        /// Sets the validation state.
        /// </summary>
        /// <param name="validationStates">The validation state.</param>
        void SetValidationStates(IValidationStates validationStates);

        #endregion Validation State Methods

        #region Get BusinessConditionStatusCollection

        /// <summary>
        /// Gets IBusinessConditionStatusCollection for current entity
        /// </summary>
        /// <returns>Returns IBusinessConditionStatusCollection</returns>
        IBusinessConditionStatusCollection GetBusinessConditionStatusCollection();
        
        #endregion Get BusinessConditionStatusCollection

        #region Get FlattenEntities

        /// <summary>
        /// Gets all flatten entities of entire family including variants and extensions including variants
        /// </summary>
        /// <param name="includeSelf">Flag indicates to include self entity or not</param>
        /// <param name="includeVariants">Flag indicates to include self entity or not</param>
        /// <param name="includeExtensionsWithVariants">Flag indicates to include self entity or not</param>
        /// <returns>Returns all flatten entities of entire family including variants and extensions including variants</returns>
        IEntityCollection GetFlattenEntities(Boolean includeSelf = true, Boolean includeVariants = true, Boolean includeExtensionsWithVariants = true);

        #endregion Get FlattenEntities

        #endregion
    }
}