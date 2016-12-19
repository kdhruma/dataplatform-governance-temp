using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Specifies interface providing Attribute Model Manager.
    /// </summary>
    public interface IAttributeModelManager
    {
        /// <summary>
        /// Get Attribute Model Context collection based on mappings
        /// </summary>
        /// <param name="attributeIdList">Collection of attribute ids for which models are required</param>
        /// <returns>Return KeyValue parameter. Key as Attribute ids and value as attribute model context</returns>
        Collection<KeyValuePair<Int32, Collection<AttributeModelContext>>> GetWhereUsed(Collection<Int32> attributeIdList);

        /// <summary>
        /// Gets the attribute models for the requested attribute ids
        /// </summary>
        /// <param name="attributeIds">Array of attribute ids for which models are required</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Model objects</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'attributeIds' not provided</exception>
        AttributeModelCollection GetByIds(Collection<Int32> attributeIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the attribute model for the requested attribute id
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which model is required</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Attribute Model Collection object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        AttributeModelCollection GetById(Int32 attributeId, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the Id of Attribute coming under requested Attribute Group and having Attribute Name
        /// </summary>
        /// <param name="attributeName">Attribute short name</param>
        /// <param name="attributeParentName">Attribute parent name</param>
        /// <returns>Id of Attribute</returns>
        Int32 GetAttributeId(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets Attribute id list coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <returns>List of attribute ids</returns>
        Collection<Int32> GetAttributeIdList(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers);

        /// <summary>
        /// Gets Attribute model coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model</returns>
        AttributeModel GetByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, CallerContext callerContext);

        /// <summary>
        /// Gets Attribute model coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="locale">Locale of attribute model</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model</returns>
        AttributeModel GetByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, CallerContext callerContext);

        /// <summary>
        /// Gets Attribute models coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model collection</returns>
        AttributeModelCollection GetByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext);

        /// <summary>
        /// Gets Attribute models coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="locale">Lcale for attribute models</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model collection</returns>
        AttributeModelCollection GetByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, LocaleEnum locale, CallerContext callerContext);

        /// <summary>
        /// Gets the attribute models for the requested ContainerId, CategoryId, EntityTypeId, AttributeModelType.
        /// It will load all attribute models.
        /// </summary>
        /// <param name="containerId">Indicates container Id for which attribute models are to be loaded</param>
        /// <param name="categoryId">Indicates category Id for which attribute models are to be loaded</param>
        /// <param name="entityTypeId">Indicates entity type Id for which attribute models are to be loaded</param>
        /// <param name="locales">List of locales in which AttributeModels are to be loaded.</param>
        /// <param name="attributeModelType">Indicates which type of attributes are to be loaded. If no value is given then AttributeModelType.All will be taken</param>
        /// <returns>Collection of Attribute Models object qualified for given context</returns>
        /// <exception cref="ArgumentException">Thrown if either of containerId , categoryId or entityTypeId value is less than 0</exception>
        AttributeModelCollection Get(Int32 containerId, Int64 categoryId, Int32 entityTypeId, Collection<LocaleEnum> locales, AttributeModelType attributeModelType = AttributeModelType.All);

        /// <summary>
        /// Gets the attribute models context
        /// </summary>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when attributeModelContext parameters are not provided</exception>
        AttributeModelCollection Get(AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the attribute models for the requested attribute ids, attribute group ids, custom view id and state view id based on attribute model context
        /// </summary>
        /// <param name="attributeIds">Ids of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Ids of attribute groups for which models are required</param>
        /// <param name="customViewId">Custom view id of which models are required</param>
        /// <param name="stateViewId">State view id of which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'attributeIds' and 'attributeGroupIds' and 'customiewId' and 'stateViewId' are not provided</exception>
        AttributeModelCollection Get(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the attribute models for the requested attribute ids and attribute group ids based on attribute model context
        /// </summary>
        /// <param name="attributeIds">Ids of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Ids of attribute groups for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when either 'attributeIds' or 'attributeGroupIds' are not provided</exception>
        AttributeModelCollection Get(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the attribute models from the requested entity context
        /// </summary>
        /// <param name="entityContext">entity context</param>
        /// <returns>attribute model collection for given entity context</returns>
        AttributeModelCollection Get(EntityContext entityContext);

        /// <summary>
        /// Gets the attribute models for the requested attribute group id
        /// </summary>
        /// <param name="attributeGroupId">Id of the attribute group for which model is required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Model objects</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        AttributeModelCollection GetByGroupId(Int32 attributeGroupId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the attribute models for the requested attribute group ids
        /// </summary>
        /// <param name="attributeGroupIds">Ids of the attribute group for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'attributeGroupIds' not provided</exception>
        AttributeModelCollection GetByGroupIds(Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the attribute models for the requested custom view id
        /// </summary>
        /// <param name="customViewId">Id of the custom view for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'customViewId' not provided</exception>
        AttributeModelCollection GetByCustomViewId(Int32 customViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the attribute models for the requested state view id
        /// </summary>
        /// <param name="stateViewId">Id of the state view for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'stateViewId' not provided</exception>
        AttributeModelCollection GetByStateViewId(Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the ids of attributes coming under requested attribute groups, custom views or state views 
        /// This method try to loads attribute ids directly from database. It would be recommended to use other overload method to gain performance.
        /// </summary>
        /// <param name="attributeGroupIds"> Comma separated list of attribute group ids</param>
        /// <param name="customViewId">Custom view id</param>
        /// <param name="stateViewId">State view id</param>
        /// <param name="attributeModelContext">The data context for which ids needs to be fetched</param>
        /// <returns>collection of attribute ids.</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        Collection<Int32> GetAttributeIdList(Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, AttributeModelContext attributeModelContext);

        /// <summary>
        /// Gets the ids of attributes coming under requested attribute groups or based on attribute model context
        /// This method try to loads attribute ids from cache if its available.
        /// </summary>
        /// <param name="attributeGroupIds">collection of attribute group ids.</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetch.</param>
        /// <returns>collection of attribute ids.</returns>
        Collection<Int32> GetAttributeIdList(Collection<Int32> attributeGroupIds, AttributeModelContext attributeModelContext);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AttributeModelCollection GetAllBaseAttributeModels();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AttributeModelCollection GetAllBaseRelationshipAttributeModels();

        /// <summary>
        /// Creates or Updates or Deletes attribute models based on model action flag
        /// </summary>
        /// <param name="attributeModelCollection"></param>
        /// <param name="attributeOperationResultCollection"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        AttributeOperationResultCollection ProcessAttributeModels(AttributeModelCollection attributeModelCollection, AttributeOperationResultCollection attributeOperationResultCollection, String programName, CallerContext callerContext);

        /// <summary>
        /// Sort Attribute Models based on sort order recursively.If sort order value is 0 then it will be always last.
        /// Sort AttributeModels applies for child attribute model also if it has any.
        /// </summary>
        /// <param name="attributeModels">This parameter is specifying attribute models</param>
        /// <returns>sorted attribute model based on sort order</returns>
        AttributeModelCollection SortAttributeModels(AttributeModelCollection attributeModels);

        /// <summary>
        /// Get all child dependent attribute models
        /// </summary>
        /// <param name="modelContext">Attribute model context which indicates what all attribute models to load.</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Dependent child attribute model</returns>
        AttributeModelCollection GetAllDependentChildAttributeModels(AttributeModelContext modelContext, CallerContext callerContext);
    }
}