using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace MDM.Services
{
    using Core;
    using Core.Exceptions;
    using Interfaces;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using MDM.Utility;

    /// <summary>
    /// Utility Methods for Ensure Entity API
    /// </summary>
    internal class EnsureEntityUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiredAttributeIds"></param>
        /// <param name="requiredAttributeGroupIds"></param>
        /// <param name="requiredDataLocales"></param>
        /// <param name="attributes"></param>
        /// <param name="originalAttributes"></param>
        /// <param name="attributeModels"></param>
        /// <param name="originalAttributeModels"></param>
        /// <param name="missingAttributeIds"></param>
        /// <returns></returns>
        public static Boolean FillAndIdentifyMissingAttributes(Collection<Int32> requiredAttributeIds, Collection<Int32> requiredAttributeGroupIds, Collection<LocaleEnum> requiredDataLocales, AttributeCollection attributes, AttributeCollection originalAttributes, AttributeModelCollection attributeModels, AttributeModelCollection originalAttributeModels, Collection<Int32> missingAttributeIds)
        {
            #region Diagnostics & Tracing 

            Boolean isGetAPIRequiredForEntityData = false;
            DiagnosticActivity activity = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var traceEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;

            if (traceEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            #endregion

            try
            {
                #region Validate Input Params

                if ((requiredAttributeIds == null || requiredAttributeIds.Count < 1) && (requiredAttributeGroupIds == null || requiredAttributeGroupIds.Count < 1))
            {
                return false;
            }

            if (requiredDataLocales == null || requiredDataLocales.Count < 1)
            {
                return false;
            }

            if (attributes == null || attributes.Count < 1)
            {
                // TODO verify if this is enough if loadattributemodels is true
                missingAttributeIds = requiredAttributeIds;
                return true;
            }

                #endregion Validate Input Params

                #region Step: Fill And Identify Missing Attributes

                if (requiredAttributeGroupIds != null && requiredAttributeGroupIds.Count > 0)
                {
                    if (attributeModels != null)
                    {
                        foreach (Int32 attributeGroupId in requiredAttributeGroupIds)
                        {
                            IAttributeModelCollection requiredAttributeModels = attributeModels.GetAttributeModelsByParentId(attributeGroupId, requiredDataLocales);

                            if (requiredAttributeModels != null && requiredAttributeModels.Count > 0)
                            {
                                foreach (AttributeModel attributeModel in requiredAttributeModels)
                                {
                                    if (!requiredAttributeIds.Contains(attributeModel.Id))
                                    {
                                        requiredAttributeIds.Add(attributeModel.Id);
                                    }
                                }
                            }
                            else
                            {
                                isGetAPIRequiredForEntityData = true;
                            }
                        }
                    }
                    else
            {
                        isGetAPIRequiredForEntityData = true;
                    }
                }

                foreach (Int32 attrId in requiredAttributeIds)
                {
                    foreach (LocaleEnum dataLocale in requiredDataLocales)
                {
                    if ((!attributes.Contains(attrId, dataLocale)))
                    {
                        if (originalAttributes != null)
                        {
                            var originalAttribute = (Attribute)originalAttributes.GetAttribute(attrId, dataLocale);

                            if (originalAttribute != null)
                            {
                                attributes.Add(originalAttribute.Clone(), true);

                                if (attributeModels != null && originalAttributeModels != null)
                                {
                                    if (!attributeModels.Contains(attrId, dataLocale))
                                    {
                                        var originalAttributeModel = (AttributeModel)originalAttributeModels.GetAttributeModel(attrId, dataLocale);

                                        if (originalAttributeModel != null)
                                        {
                                            attributeModels.Add(originalAttributeModel, true);
                                        }
                                        else
                                        {
                                            missingAttributeIds.Add(attrId);
                                            break; //break data locale loop
                                        }
                                    }
                                }
                            }
                            else
                            {
                                missingAttributeIds.Add(attrId);
                                break; //break data locale loop
                            }
                        }
                        else
                        {
                            missingAttributeIds.Add(attrId);
                            break; //break data locale loop
                        }
                    }

                    if (attributeModels != null && !attributeModels.Contains(attrId, dataLocale))
                    {
                        missingAttributeIds.Add(attrId);
                        break;
                    }
                    }
                }

                if (missingAttributeIds != null && missingAttributeIds.Count > 0)
                {
                    isGetAPIRequiredForEntityData = true;
                }

                #endregion Step: Fill And Identify Missing Attributes
            }
            finally
            {
                #region Finalize Diagnostics & Tracing

                if (traceEnabled)
                {
                    activity.Stop();
                }

                #endregion
            }

            return isGetAPIRequiredForEntityData;
        }

        /// <summary>
        /// Identifies missing RelationshipTypeModels to be loaded.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="relationshipTypeModelToBeLoaded"></param>
        public static void IdentifyMissingRelationshipData(Entity entity, IEntityContext entityContext, IRelationshipContext relationshipContext, Collection<Int32> relationshipTypeModelToBeLoaded)
        {
            #region Diagnostics, Tracing & Local Variables Initialization

            DiagnosticActivity activity = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var traceEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;

            if (traceEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            #endregion

            try
            {
                #region Validate Input Params

                ValidateNullReferenceParameter(entity, "entity");

                ValidateNullReferenceParameter(entityContext, "entityContext");

                ValidateNullReferenceParameter(relationshipContext, "relationshipContext");

                ValidateNullReferenceParameter(relationshipTypeModelToBeLoaded, "relationshipTypeModelToBeLoaded");

                #endregion

                #region Step: Identify Missing RelationshipTypeModels and Attributes

                Collection<Int32> relationshipTypeIds = null;

                var relationships = entity.Relationships;

                if (relationships != null && relationships.Count > 0)
                {
                    relationshipTypeIds = relationshipContext.RelationshipTypeIdList != null ? relationshipContext.RelationshipTypeIdList : new Collection<Int32>();

                    foreach (var relationship in relationships)
                    {
                        if (!relationshipTypeIds.Contains(relationship.RelationshipTypeId))
                        {
                            continue;
                        }

                        if (!relationshipTypeModelToBeLoaded.Contains(relationship.RelationshipTypeId) && relationship.Id < 1)
                        {
                            relationshipTypeModelToBeLoaded.Add(relationship.RelationshipTypeId);
                        }

                        // When we are calling FillAndIdentifyMissingAttributes, requiredAttributeIds is always null.
                        // So there is no need to make this call as of now, till we support RelationshipContext, R

                        //if (relationshipContext.LoadRelationshipAttributes)
                        //{
                        //    var missingRelationshipAttributeIds = new Collection<Int32>();
                        //    var originalRelationship = relationship.OriginalRelationship;
                        //    var relationshipAttributes = relationship.RelationshipAttributes;
                        //    var originalrelationshipAttributes = originalRelationship != null ? originalRelationship.RelationshipAttributes : null;

                        //    isGetApiCallRequired = EnsureEntityUtility.FillAndIdentifyMissingAttributes(null, entityContext.DataLocales, relationshipAttributes, originalrelationshipAttributes, null, null, missingRelationshipAttributeIds);
                        //}
                    }
                }

                #endregion Step: Identify Missing RelationshipTypeModels and Attributes
            }
            finally
            {
                #region Finalize Diagnostics & Tracing

                if (traceEnabled)
                {
                    activity.Stop();
                }

                #endregion
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="loadedEntity"></param>
        /// <param name="attributeGroupIdList"></param>
        /// <param name="attributeIdList"></param>
        /// <param name="loadAttributes"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        public static void FillMissingEntityAttributeData(Entity entity, Entity loadedEntity, Collection<Int32> attributeGroupIdList, Collection<Int32> attributeIdList, Boolean loadAttributes, Boolean loadAttributeModels, ICallerContext callerContext)
        {
            #region Diagnostics & Tracing

            DiagnosticActivity activity = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var traceEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;

            if (traceEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            #endregion

            try
        {

            #region Validate Input Params

            ValidateNullReferenceParameter(entity, "entity");

            #endregion

                #region Step Initialize loacal variables and local entityContext

            var ensuredAttributes = new AttributeCollection();
            var ensuredAttributeModels = new AttributeModelCollection();

            var entityContext = new EntityContext();

            //Some information are to be copied from entity 
            //These information are not supposed to change for an entity after it is loaded initially
            entityContext.ContainerId = entity.ContainerId;
            entityContext.EntityTypeId = entity.EntityTypeId;
            entityContext.CategoryId = entity.CategoryId;
            entityContext.Locale = entity.Locale;
            entityContext.LoadAttributes = loadAttributes;
            entityContext.AttributeIdList = attributeIdList;
            entityContext.LoadAttributeModels = loadAttributeModels;
                entityContext.AttributeGroupIdList = attributeGroupIdList;

            if (entity.EntityContext != null)
            {
                entityContext.DataLocales = entity.EntityContext.DataLocales;
                entityContext.LoadDependentAttributes = entity.EntityContext.LoadDependentAttributes;
            }

                #endregion

                #region Step: Find missing attributes and models to be ensured

            if (entity.Id > 0)
            {
                    #region Find missing Attributes and Models for entity with id.

                if (loadedEntity != null)
                {
                    if (entity.OriginalEntity == null)
                    {
                        entity.OriginalEntity = loadedEntity;
                    }

                    ensuredAttributes = loadedEntity.Attributes;
                    ensuredAttributeModels = loadedEntity.AttributeModels;

                    //If attributes are found for given context then append information in Entity.EntityContext with new context specified in parameter
                    if (ensuredAttributes != null && ensuredAttributes.Count > 0)
                    {
                        if (entity.EntityContext != null)
                        {
                            entity.EntityContext.UpdateContext(entityContext);
                        }
                        else
                        {
                            entity.EntityContext = entityContext;
                        }

                        entity.OriginalEntity.EntityContext = entity.EntityContext; //Update the original Entity Entity context
                    }
                }

                    #endregion Find missing Attributes and Models for entity with id.
            }
            else
            {
                    #region Find missing Attributes and Models for entity without id.

                    if (traceEnabled)
                {
                        activity.LogInformation("Trying to load attribute models");
                }

                    LoadEntityIdsByName(entity, entityContext, callerContext);

                    #region Make GetEntityModel call

                var dataService = new DataService();

                    var returnedEntity = dataService.GetEntityModel(entityContext, MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);

                    #endregion

                    #region Load Attributes and Attribute Models

                if (entityContext.LoadAttributes)
                {
                        ensuredAttributes = (AttributeCollection)returnedEntity.GetAttributes();

                        if (traceEnabled)
                    {
                            activity.LogInformation("Number of attributes loaded for model: " + entity.Attributes.Count);
                    }
                }

                if (entityContext.LoadAttributeModels)
                {
                        ensuredAttributeModels = (AttributeModelCollection)returnedEntity.GetAttributeModels();

                        if (traceEnabled)
                    {
                            activity.LogInformation("Number of attribute models loaded: " + entity.AttributeModels.Count);
                    }
                }

                    #endregion Load Attributes and Attribute Models

                    #endregion Load Attribute Models
            }

                #endregion Step Find missing attributes and models to be ensured

                #region Step: Ensure missing attributes/models

            if ((ensuredAttributes != null && ensuredAttributes.Count > 0) || (ensuredAttributeModels != null && ensuredAttributeModels.Count > 0))
            {
                AttributeCollection attributes = entity.Attributes;
                AttributeModelCollection attributeModels = entity.AttributeModels;
                AttributeCollection originalAttributes = null;
                AttributeModelCollection originalAttributeModels = null;

                if (entity.OriginalEntity != null)
                {
                    originalAttributes = entity.OriginalEntity.Attributes;
                    originalAttributeModels = entity.OriginalEntity.AttributeModels;
                }

                FillEnsuredAttributes(ensuredAttributes, ensuredAttributeModels, attributes, originalAttributes, attributeModels, originalAttributeModels);
            }
            else
            {
                    if (traceEnabled)
                    {
                        activity.LogInformation(String.Format("Attributes were not ensured. {0}", String.IsNullOrEmpty(entity.ExternalId) ? String.Empty : entity.ExternalId));
                    }
                }

                #endregion
            }
            finally
            {
                #region Finalize Diagnostics & Tracing

                if (traceEnabled)
                {
                    activity.Stop();
                }

                #endregion
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="loadedEntity"></param>
        /// <param name="entityContext"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="relationshipTypeModelToBeLoaded"></param>
        /// <param name="iCallerContext"></param>
        public static void FillMissingRelationshipData(Entity entity, Entity loadedEntity, IEntityContext entityContext, IRelationshipContext relationshipContext, Collection<Int32> relationshipTypeModelToBeLoaded, ICallerContext iCallerContext)
        {
            #region Diagnostics & Tracing

            DiagnosticActivity activity = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var traceEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;

            if (traceEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            #endregion

            try
            {
            #region Validate Input Params

            ValidateNullReferenceParameter(entity, "entity");

            ValidateNullReferenceParameter(entityContext, "entityContext");

            ValidateNullReferenceParameter(relationshipContext, "relationshipContext");

            ValidateNullReferenceParameter(relationshipTypeModelToBeLoaded, "relationshipTypeModelToBeLoaded");

            #endregion

                #region Step: Load model relationships for relationship type models
            
            var relationships = entity.Relationships;

            var modelRelationshipsByRelationshipType = new Dictionary<Int32, Relationship>();

            if (relationshipTypeModelToBeLoaded != null && relationshipTypeModelToBeLoaded.Count > 0)
            {
                var containerId = entity.ContainerId;
                    var dataService = new DataService();

                foreach (var relationshipTypeId in relationshipTypeModelToBeLoaded)
                {
                    var modelRelationship = (Relationship)dataService.GetRelationshipModel(relationshipTypeId, containerId, entityContext.DataLocales, iCallerContext);

                    if (modelRelationship != null)
                    {
                        modelRelationshipsByRelationshipType.Add(relationshipTypeId, modelRelationship);
                    }
                }
            }

            #endregion

            #region Step: Loop through each existing relationship and fill missing attributes from existing relationship / model relationship

            RelationshipCollection existingRelationships = null;

            if (loadedEntity != null)
            {
                existingRelationships = loadedEntity.Relationships;
            }

            foreach (var relationship in relationships)
            {
                Relationship ensuredRelationship = null;

                if (relationship.Id > 0 && existingRelationships != null && existingRelationships.Count > 0)
                {
                    ensuredRelationship = (Relationship)existingRelationships.GetRelationship(relationship.RelatedEntityId, relationship.RelationshipTypeId);
                }
                else
                {
                    modelRelationshipsByRelationshipType.TryGetValue(relationship.RelationshipTypeId, out ensuredRelationship);
                }

                if (ensuredRelationship != null)
                {
                    var relationshipAttributes = relationship.RelationshipAttributes;
                    var ensuredRelationshipAttributes = ensuredRelationship.RelationshipAttributes;

                    AttributeCollection originalRelationshipAttributes = null;

                    if (relationship.OriginalRelationship != null)
                    {
                        originalRelationshipAttributes = relationship.OriginalRelationship.RelationshipAttributes;
                    }

                    if (ensuredRelationshipAttributes != null && ensuredRelationshipAttributes.Count > 0)
                    {
                        var filteredEnsuredAttributes = new AttributeCollection();

                        if (relationshipContext.LoadRelationshipAttributes)
                        {
                            filteredEnsuredAttributes = (AttributeCollection)ensuredRelationshipAttributes.Clone();
                        }

                        EnsureEntityUtility.FillEnsuredAttributes(filteredEnsuredAttributes, null, relationshipAttributes, originalRelationshipAttributes, null, null);
                    }
                }
            }

            #endregion

            #region Step: Add all missing relationships if loadAllExistingRelationships is set to true

            if (entity.Id > 0 && existingRelationships != null && existingRelationships.Count > 0)
            {
                foreach (var existingRelationship in existingRelationships)
                {
                    if (relationships == null)
                    {
                        relationships = new RelationshipCollection();
                    }

                    var relationship = relationships.GetRelationship(existingRelationship.RelatedEntityId, existingRelationship.RelationshipTypeId);

                    if (relationship == null)
                    {
                        relationships.Add(existingRelationship);
                    }
                }
            }

            #endregion
        }
            finally
            {
                #region Finalize Diagnostics & Tracing

                if (traceEnabled)
                {
                    activity.Stop();
                }

                #endregion
            }
            
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="loadedEntity"></param>
        /// <param name="callerContext"></param>
        public static void EnsureHierarchyRelationships(Entity entity, Entity loadedEntity, CallerContext callerContext)
        {
            #region Diagnostics & Tracing

            DiagnosticActivity activity = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var traceEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;

            if (traceEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            #endregion

            try
        {
            #region Validate Input Params

            ValidateNullReferenceParameter(entity, "entity");

            #endregion

                #region Step: Ensure Hierarchy Relationships

            //  Redundant as Get call is instantiating on null. But when this behavior needs to be fixed, below condition will come into effect.
                if (loadedEntity == null || loadedEntity.GetHierarchyRelationships() == null)
            {
                return;
            }

            //  Redundant as Property accessor is instantiating on null. But when this behavior needs to be fixed, below condition will come into effect.
            if (entity.HierarchyRelationships == null)
            {
                entity.HierarchyRelationships = new HierarchyRelationshipCollection();
            }

            if (entity.HierarchyRelationships.Count < 1)
            {
                    entity.HierarchyRelationships = loadedEntity.HierarchyRelationships;
            }
            else
            {
                    EnsureHierarchyRelationshipsRecursive(entity.HierarchyRelationships, loadedEntity.HierarchyRelationships);
                }

                #endregion Step: Ensure Hierarchy Relationships
            }
            finally
            {
                #region Finalize Diagnostics & Tracing

                if (traceEnabled)
                {
                    activity.Stop();
            }

                #endregion
        }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="loadedEntity"></param>
        /// <param name="callerContext"></param>
        public static void EnsureExtensionRelationships(Entity entity, Entity loadedEntity, CallerContext callerContext)
        {
            #region Diagnostics & Tracing

            DiagnosticActivity activity = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var traceEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;

            if (traceEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            #endregion

            try
            {

            #region Validate Input Params

            ValidateNullReferenceParameter(entity, "entity");

            #endregion

                #region Step: Ensure Extension Relationships

            //  Redundant as Get call is instantiating on null. But when this behavior needs to be fixed, below condition will come into effect.
                if (loadedEntity == null || loadedEntity.GetExtensionsRelationships() == null)
            {
                return;
            }

            //  Redundant as Property accessor is instantiating on null. But when this behavior needs to be fixed, below condition will come into effect.
            if (entity.ExtensionRelationships == null)
            {
                entity.ExtensionRelationships = new ExtensionRelationshipCollection();
            }

            if (entity.ExtensionRelationships.Count < 1)
            {
                    entity.ExtensionRelationships = loadedEntity.ExtensionRelationships;
            }
            else
            {
                    EnsureExtensionRelationshipsRecursive(entity.ExtensionRelationships, loadedEntity.ExtensionRelationships);
                }

                #endregion Step: Ensure Extension Relationships
            }
            finally
            {
                #region Finalize Diagnostics & Tracing

                if (traceEnabled)
                {
                    activity.Stop();
                }

                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="loadedEntity"></param>
        public static void FillEntityData(Entity entity, Entity loadedEntity)
        {
            if (loadedEntity != null)
            {
                entity.ExternalId = loadedEntity.ExternalId;
                entity.Name = loadedEntity.Name;
                entity.LongName = loadedEntity.LongName;
                entity.CategoryPath = loadedEntity.CategoryPath;
                entity.CategoryLongNamePath = loadedEntity.CategoryLongNamePath;
                entity.ParentEntityId = loadedEntity.ParentEntityId;
                entity.ParentEntityTypeId = loadedEntity.ParentEntityTypeId;
                entity.ParentExternalId = loadedEntity.ParentExternalId;
                entity.ParentEntityName = loadedEntity.ParentEntityName;
                entity.ParentEntityLongName = loadedEntity.ParentEntityLongName;
                entity.ParentExtensionEntityId = loadedEntity.ParentExtensionEntityId;
                entity.ParentExtensionEntityName = loadedEntity.ParentExtensionEntityName;
                entity.ParentExtensionEntityLongName = loadedEntity.ParentExtensionEntityLongName;
                entity.ParentExtensionEntityExternalId = loadedEntity.ParentExtensionEntityExternalId;
                entity.ParentExtensionEntityContainerId = loadedEntity.ParentExtensionEntityContainerId;
                entity.ParentExtensionEntityContainerName = loadedEntity.ParentExtensionEntityContainerName;
                entity.ParentExtensionEntityContainerLongName = loadedEntity.ParentExtensionEntityContainerLongName;
                entity.ParentExtensionEntityCategoryId = loadedEntity.ParentExtensionEntityCategoryId;
                entity.ParentExtensionEntityCategoryName = loadedEntity.ParentExtensionEntityCategoryName;
                entity.ParentExtensionEntityCategoryLongName = loadedEntity.ParentExtensionEntityCategoryLongName;
                entity.ParentExtensionEntityCategoryPath = loadedEntity.ParentExtensionEntityCategoryPath;
                entity.ParentExtensionEntityCategoryLongNamePath = loadedEntity.ParentExtensionEntityCategoryLongNamePath;
                entity.CategoryId = loadedEntity.CategoryId;
                entity.CategoryName = loadedEntity.CategoryName;
                entity.CategoryLongName = loadedEntity.CategoryLongName;
                entity.ContainerId = loadedEntity.ContainerId;
                entity.ContainerName = loadedEntity.ContainerName;
                entity.ContainerLongName = loadedEntity.ContainerLongName;
                entity.OrganizationId = loadedEntity.OrganizationId;
                entity.OrganizationName = loadedEntity.OrganizationName;
                entity.OrganizationLongName = loadedEntity.OrganizationLongName;
                entity.EntityTypeId = loadedEntity.EntityTypeId;
                entity.EntityTypeName = loadedEntity.EntityTypeName;
                entity.EntityTypeLongName = loadedEntity.EntityTypeLongName;
            }
        }

        #region Private Methods

        #region Validation Methods
        
        // TODO Consolidate validation methods

        /// <summary>
        /// Validate NullReference Parameter
        /// </summary>
        /// <param name="referenceObject"></param>
        /// <param name="parameterName"></param>
        /// <param name="memberName"></param>
        private static void ValidateNullReferenceParameter(Object referenceObject, String parameterName, [CallerMemberName] string memberName = null)
        {
            if (referenceObject == null)
            {
                throw new MDMOperationException("111565", String.Format("{0} is null", parameterName), "DataService", String.Empty, memberName, parameterName);
            }
        }

        #endregion


        #region Fill helper methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ensuredAttributes"></param>
        /// <param name="ensuredAttributeModels"></param>
        /// <param name="attributes"></param>
        /// <param name="originalAttributes"></param>
        /// <param name="attributeModels"></param>
        /// <param name="originalAttributeModels"></param>
        /// <returns></returns>
        private static void FillEnsuredAttributes(AttributeCollection ensuredAttributes, AttributeModelCollection ensuredAttributeModels, AttributeCollection attributes, AttributeCollection originalAttributes, AttributeModelCollection attributeModels, AttributeModelCollection originalAttributeModels)
        {
            #region Ensure Attributes

            if (ensuredAttributes != null && ensuredAttributes.Count > 0)
            {
                if (attributes == null)
                {
                    attributes = new AttributeCollection();
                }

                foreach (Attribute ensuredAttribute in ensuredAttributes)
                {
                    if (!attributes.Contains(ensuredAttribute.Id, ensuredAttribute.Locale))
                    {
                        attributes.Add(ensuredAttribute, true);
                    }

                    if (originalAttributes != null)
                    {
                        if (!originalAttributes.Contains(ensuredAttribute.Id, ensuredAttribute.Locale))
                        {
                            originalAttributes.Add(ensuredAttribute.Clone(), true);
                        }
                    }
                }
            }

            #endregion

            #region Ensure AttributeModels

            if (ensuredAttributeModels != null && ensuredAttributeModels.Count > 0)
            {
                if (attributeModels == null)
                {
                    attributeModels = new AttributeModelCollection();
                }

                foreach (AttributeModel ensuredAttributeModel in ensuredAttributeModels)
                {
                    if (!attributeModels.Contains(ensuredAttributeModel.Id, ensuredAttributeModel.Locale))
                    {
                        attributeModels.Add(ensuredAttributeModel, true);
                    }

                    if (originalAttributeModels != null)
                    {
                        if (!originalAttributeModels.Contains(ensuredAttributeModel.Id, ensuredAttributeModel.Locale))
                        {
                            originalAttributeModels.Add((AttributeModel)ensuredAttributeModel.Clone(), true);
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        /// <param name="callerContext"></param>
        private static void LoadEntityIdsByName(Entity entity, EntityContext entityContext, ICallerContext callerContext)
        {
            #region Diagnostics & Tracing

            DiagnosticActivity activity = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var traceEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;

            if (traceEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            #endregion

            try
            {
                var dataService = new DataService();

                if (entity.ContainerId < 1 || entity.CategoryId < 1 || entity.EntityTypeId < 1)
                {
                    #region Load ContainerId

                    var dataModelService = new DataModelService();

                    if (entity.ContainerId < 1)
                    {
                        if (!String.IsNullOrEmpty(entity.ContainerName))
                        {
                            var container = dataModelService.GetContainerByName(entity.ContainerName, callerContext);

                            if (container != null)
                            {
                                entity.ContainerId = container.Id;
                                entity.OrganizationId = container.OrganizationId;
                            }
                            else
                            {
                                if (traceEnabled)
                                {
                                    activity.LogWarning(String.Format("ContainerId could not be populated as ContainerName {2} was not found for entity with id [{0}] name[{1}]", entity.Id, entity.ExternalId, entity.ContainerName));
                                }
                            }
                        }
                        else
                        {
                            if (traceEnabled)
                            {
                                activity.LogWarning(String.Format("ContainerId could not be populated as ContainerName was not provided for entity with id [{0}] name[{1}]", entity.Id, entity.ExternalId));
                            }
                        }
                    }

                    entityContext.ContainerId = entity.ContainerId;

                    #endregion Load ContainerId

                    #region Load CategoryId

                    if (entity.CategoryId < 1 && !String.IsNullOrEmpty(entity.CategoryPath))
                    {
                        if (!String.IsNullOrEmpty(entity.ContainerName))
                        {
                            var categoryPathSeparator = String.Empty;

                            try
                            {
                                categoryPathSeparator = AppConfigurationHelper.GetAppConfig("Catalog.Category.PathSeparator", categoryPathSeparator);
                            }
                            catch
                            {
                                if (traceEnabled)
                                {
                                    activity.LogInformation("Could not load AppConfig: Catalog.Category.PathSeparator");
                                }
                            }

                            entity.CategoryPath = entity.CategoryPath.Replace("#@#", categoryPathSeparator);
                            entity.CategoryPath = entity.CategoryPath.Replace("//", categoryPathSeparator);

                            var category = dataModelService.GetCategoryByPath(entity.ContainerName, entity.CategoryPath, callerContext);

                            if (category != null)
                            {
                                entity.CategoryId = category.Id;
                                entity.CategoryName = category.Name;
                                entity.CategoryLongName = category.LongName;
                            }
                            else
                            {
                                if (traceEnabled)
                                {
                                    activity.LogWarning(String.Format("CatagoryId could not be populated as CategoryPath {2} was not found for entity with id [{0}] name[{1}]", entity.Id, entity.ExternalId, entity.CategoryPath));
                                }
                            }
                        }
                        else
                        {
                            if (traceEnabled)
                            {
                                activity.LogWarning(String.Format("CatagoryId could not be populated as ContainerName was empty for entity with id [{0}] name[{1}]", entity.Id, entity.ExternalId));
                            }
                        }
                    }

                    entityContext.CategoryId = entity.CategoryId;

                    #endregion Load CategoryId

                    #region Load EntityTypeId

                    if (entity.EntityTypeId < 1)
                    {
                        if (!String.IsNullOrEmpty(entity.EntityTypeName))
                        {
                            var entityType = dataModelService.GetEntityTypeByShortName(entity.EntityTypeName, callerContext);

                            if (entityType != null)
                            {
                                entity.EntityTypeId = entityType.Id;
                                entityContext.EntityTypeId = entity.EntityTypeId;
                            }
                            else
                            {
                                if (traceEnabled)
                                {
                                    activity.LogWarning(String.Format("EntityTypeId could not be populated as EntityTypeName {2} was not found for entity with id [{0}] name[{1}]", entity.Id, entity.ExternalId, entity.EntityTypeName));
                                }
                            }
                        }
                        else
                        {
                            if (traceEnabled)
                            {
                                activity.LogWarning(String.Format("EntityTypeId could not be populated as EntityTypeName was not provided for entity with id [{0}] name[{1}]", entity.Id, entity.ExternalId));
                            }
                        }
                    }

                    entityContext.EntityTypeId = entity.EntityTypeId;

                    #endregion Load EntityTypeId
                }
            }
            finally
            {
                #region Finalize Diagnostics & Tracing

                if (traceEnabled)
                {
                    activity.Stop();
                }

                #endregion
            }
        }
        #endregion

        /// <summary>
        /// Ensure Hierarchy Relationships Recursive
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="ensuredRelationships"></param>
        private static void EnsureHierarchyRelationshipsRecursive(HierarchyRelationshipCollection relationships, HierarchyRelationshipCollection ensuredRelationships)
        {
            if (relationships == null || relationships.Count < 1)
            {
                relationships = ensuredRelationships;
            }
            else if (ensuredRelationships != null && ensuredRelationships.Count > 0)
            {
                foreach (var ensuredRelationship in ensuredRelationships)
                {
                    var relationship = relationships.FindHierarchyRelationshipByRelatedEntityId(ensuredRelationship.RelatedEntityId);

                    if (relationship == null)
                    {
                        relationships.Add(ensuredRelationship);
                    }
                    else
                    {
                        var ensuredChildHierarchyRelationships = (HierarchyRelationshipCollection)ensuredRelationship.GetRelationships();

                        if (ensuredChildHierarchyRelationships != null && ensuredChildHierarchyRelationships.Count > 0)
                        {
                            EnsureHierarchyRelationshipsRecursive(relationship.RelationshipCollection, ensuredChildHierarchyRelationships);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ensure Extension Relationships Recursive
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="ensuredRelationships"></param>
        private static void EnsureExtensionRelationshipsRecursive(ExtensionRelationshipCollection relationships, ExtensionRelationshipCollection ensuredRelationships)
        {
            if (relationships == null || relationships.Count < 1)
            {
                relationships = ensuredRelationships;
            }
            else if (ensuredRelationships != null && ensuredRelationships.Count > 0)
            {
                foreach (var ensuredRelationship in ensuredRelationships)
                {
                    var relationship = (ExtensionRelationship)relationships.FindByRelatedEntityId(ensuredRelationship.RelatedEntityId);

                    if (relationship == null)
                    {
                        relationship = (ExtensionRelationship)relationships.FindByToExternalIdContainerIdCategoryId(ensuredRelationship.ExternalId, ensuredRelationship.ContainerId, ensuredRelationship.CategoryId);
                    }

                    if (relationship == null)
                    {
                        relationship = (ExtensionRelationship)relationships.FindByToExternalIdContainerNameCategoryPath(ensuredRelationship.ExternalId, ensuredRelationship.ContainerName, ensuredRelationship.CategoryPath);
                    }

                    if (relationship == null)
                    {
                        relationships.Add(ensuredRelationship);
                    }
                    else
                    {
                        var ensuredChildExtensionRelationships = (ExtensionRelationshipCollection)ensuredRelationship.GetRelationships();

                        if (ensuredChildExtensionRelationships != null && ensuredChildExtensionRelationships.Count > 0)
                        {
                            EnsureExtensionRelationshipsRecursive(relationship.RelationshipCollection, ensuredChildExtensionRelationships);
                        }
                    }
                }
            }
        }
      
        #endregion
    }
}
