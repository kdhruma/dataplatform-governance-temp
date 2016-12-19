using System;
using System.Collections.ObjectModel;

namespace MDM.DataModelManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeModelMappingPropertiesBL
    {
        #region Fields

        /// <summary>
        /// Specifies mapping buffer manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = new MappingBufferManager();

        /// <summary>
        /// Lookup data access object
        /// </summary>
        private ContainerEntityTypeAttributeMappingBL _containerEntityTypeAttributeMappingBL = new ContainerEntityTypeAttributeMappingBL();

        /// <summary>
        /// Lookup data access object
        /// </summary>
        private ContainerRelationshipTypeAttributeMappingBL _containerRelationshipTypeAttributeMappingBL = new ContainerRelationshipTypeAttributeMappingBL();

        /// <summary>
        /// Lookup data access object
        /// </summary>
        private CategoryAttributeMappingBL _categoryAttributeMappingBL = new CategoryAttributeMappingBL();

        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Common Attribute Model Mappings Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetCommonAttributeModelMappingProperties(Int32 containerId, Int32 entityTypeId, Boolean getLatest, CallerContext callerContext)
        {
            #region Initial Setup

            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = null;

            #endregion

            #region Get Mappings from Cache if available

            if (!getLatest)
            {
                attributeModelMappingProperties = _mappingBufferManager.FindContainerEntityTypeAttributeMappingsPartialDetails(containerId, entityTypeId);
            }

            #endregion

            #region Get Mappings from Database

            if (attributeModelMappingProperties == null)
            {
                containerEntityTypeAttributeMappings = _containerEntityTypeAttributeMappingBL.GetContainerEntityTypeAttributeMappings(containerId, entityTypeId, -1, -1, getLatest, callerContext);

                #region Construct AttributeModel Mappig Properties

                if (containerEntityTypeAttributeMappings != null)
                {
                    attributeModelMappingProperties = _mappingBufferManager.ConvertToAttributeModelMappingProperties(containerEntityTypeAttributeMappings);
                }

                #endregion
            }

            #endregion

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetCommonAttributeModelMappingProperties(Int32 attributeId, Int32 attributeGroupId, Int32 containerId, Int32 entityTypeId, Boolean getLatest, CallerContext callerContext)
        {
            return GetCommonAttributeModelMappingProperties(ReturnAsCollection(attributeId), ReturnAsCollection(attributeGroupId), containerId, entityTypeId, getLatest, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetCommonAttributeModelMappingProperties(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int32 containerId, Int32 entityTypeId, Boolean getLatest, CallerContext callerContext)
        {
            return GetAttributeModelMappingProperties(attributeIds, attributeGroupIds, AttributeModelType.Common, containerId, entityTypeId, 0, 0, getLatest, callerContext);
        }

        #endregion

        #region Category Attribute Model Mapping Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetCategoryAttributeModelMappingProperties(Int64 categoryId, Boolean getLatest, CallerContext callerContext)
        {
            #region Initial Setup

            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;
            CategoryAttributeMappingCollection categoryAttributeMappingCollection = null;

            #endregion

            #region Get Mappings from Cache if available

            if (!getLatest)
            {
                attributeModelMappingProperties = _mappingBufferManager.FindCategoryAttributeModelMappingsPartialDetails(categoryId);
            }

            #endregion

            #region Get Mappings from Database

            if (attributeModelMappingProperties == null)
            {
                categoryAttributeMappingCollection = _categoryAttributeMappingBL.GetByCategoryId(categoryId, callerContext);

                #region Clone Mappings data

                if (categoryAttributeMappingCollection != null)
                {
                    attributeModelMappingProperties = _mappingBufferManager.ConvertToAttributeModelMappingProperties(categoryAttributeMappingCollection);
                }

                #endregion
            }

            #endregion

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetCategoryAttributeModelMappingProperties(Int32 attributeId, Int32 attributeGroupId, Int64 categoryId, Boolean getLatest, CallerContext callerContext)
        {
            return GetCategoryAttributeModelMappingProperties(ReturnAsCollection(attributeId), ReturnAsCollection(attributeGroupId), categoryId, getLatest, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetCategoryAttributeModelMappingProperties(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int64 categoryId, Boolean getLatest, CallerContext callerContext)
        {
            return GetAttributeModelMappingProperties(attributeIds, attributeGroupIds, AttributeModelType.Category, 0, 0, 0, categoryId, getLatest, callerContext);
        }

        #endregion

        #region Relationship Attribute Model Mapping Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetRelationshipAttirbuteModelMappingProperties(Int32 containerId, Int32 relationshipTypeId, Boolean getLatest, CallerContext callerContext)
        {
            #region Initial Setup

            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;
            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = null;

            #endregion

            #region Get Mappings from Cache if available

            if (!getLatest)
            {
                attributeModelMappingProperties = _mappingBufferManager.FindContainerRelationshipAttributeModelMappingsPartialDetails(containerId, relationshipTypeId);
            }

            #endregion

            #region Get Mappings from Database

            if (attributeModelMappingProperties == null)
            {
                containerRelationshipTypeAttributeMappings = _containerRelationshipTypeAttributeMappingBL.Get(containerId, relationshipTypeId, -1, getLatest, callerContext);

                #region Construct AttributeModel Mappig Properties

                if (containerRelationshipTypeAttributeMappings != null)
                {
                    attributeModelMappingProperties = _mappingBufferManager.ConvertToAttributeModelMappingProperties(containerRelationshipTypeAttributeMappings);
                }

                #endregion
            }

            #endregion

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetRelationshipAttirbuteModelMappingProperties(Int32 attributeId, Int32 attributeGroupId, Int32 containerId, Int32 relationshipTypeId, Boolean getLatest, CallerContext callerContext)
        {
            return GetRelationshipAttirbuteModelMappingProperties(ReturnAsCollection(attributeId), ReturnAsCollection(attributeGroupId), containerId, relationshipTypeId, getLatest, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetRelationshipAttirbuteModelMappingProperties(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int32 containerId, Int32 relationshipTypeId, Boolean getLatest, CallerContext callerContext)
        {
            return GetAttributeModelMappingProperties(attributeIds, attributeGroupIds, AttributeModelType.Relationship, containerId, 0, relationshipTypeId, 0, getLatest, callerContext);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="attributeGroupIds"></param>
        /// <param name="attributeModelType"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private AttributeModelMappingPropertiesCollection GetAttributeModelMappingProperties(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, AttributeModelType attributeModelType, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Boolean getLatest, CallerContext callerContext)
        {
            AttributeModelMappingPropertiesCollection filteredAttributeModelMappingProperties = null;
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;

            if (attributeModelType == AttributeModelType.Common)
                attributeModelMappingProperties = GetCommonAttributeModelMappingProperties(containerId, entityTypeId, getLatest, callerContext);
            else if (attributeModelType == AttributeModelType.Category)
                attributeModelMappingProperties = GetCategoryAttributeModelMappingProperties(categoryId, getLatest, callerContext);
            else if (attributeModelType == AttributeModelType.Relationship)
                attributeModelMappingProperties = GetRelationshipAttirbuteModelMappingProperties(containerId, relationshipTypeId, getLatest, callerContext);

            if (attributeModelMappingProperties != null && attributeModelMappingProperties.Count > 0)
            {
                filteredAttributeModelMappingProperties = attributeModelMappingProperties.FilterByAttributeIdAndGroupId(attributeIds, attributeGroupIds);
            }

            return filteredAttributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Collection<Int32> ReturnAsCollection(Int32 item)
        {
            Collection<Int32> items = null;

            if (item > 0)
                items = new Collection<Int32>() { item };

            return items;
        }

        #endregion

        #endregion
    }
}