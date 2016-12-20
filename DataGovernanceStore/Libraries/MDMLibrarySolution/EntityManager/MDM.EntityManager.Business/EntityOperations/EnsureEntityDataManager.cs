using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using MDM.RelationshipManager.Business;

namespace MDM.EntityManager.Business.EntityOperations
{
    using Core;
    using Core.Exceptions;
    using Interfaces;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using AttributeModelManager.Business;
    using ConfigurationManager.Business;
    using Data;
    using Helpers;
    using Core.Extensions;
    using MessageManager.Business;
    using Utility;

    /// <summary>
    /// 
    /// </summary>
    internal class EnsureEntityDataManager
    {
        #region Fields

        /// <summary>
        ///     Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        ///     Field denoting localeMessageBL
        /// </summary>
        private readonly LocaleMessageBL _localeMessageBL;

        /// <summary>
        /// 
        /// </summary>
        private readonly IEntityManager _entityManager;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EnsureEntityDataManager(IEntityManager entityManager)
        {
            _entityManager = entityManager;
            _localeMessageBL = new LocaleMessageBL();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Methods

        #region Ensure entity based on entity context

        /// <summary>
        /// Ensure entity data for given entity context
        /// </summary>
        /// <param name="entities">Indicates collection of entities</param>
        /// <param name="entityContext">Indicates entity context</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Return true if object is ensured else false</returns>
        public Boolean EnsureEntityData(EntityCollection entities, EntityContext entityContext, CallerContext callerContext)
        {
            Boolean result = true;

            EntityContextHelper.PopulateIdsInEntityContext(entityContext, _entityManager, callerContext);

            if (entityContext.AttributeIdList.Count > 0)
            {
                result = result && EnsureAttributes(entities, entityContext.AttributeIdList, entityContext.DataLocales, entityContext.LoadAttributeModels, callerContext);
            }

            if (entityContext.RelationshipContext.RelationshipTypeIdList.Count > 0)
            {
                result = result && EnsureRelationships(entities, true, entityContext.RelationshipContext.RelationshipTypeIdList, null, null, callerContext);
            }

            return result;
        }

        #endregion Ensure entity based on entity context

        #region Ensure entity attribute methods

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeIds"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns></returns>
        public Boolean EnsureAttributes(Entity entity, IEnumerable<Int32> attributeIds, Boolean loadAttributeModels, CallerContext callerContext)
        {
            return EnsureEntityAttributes(new EntityCollection { entity }, attributeIds != null ? new Collection<Int32>(attributeIds.ToList()) : null, null, null, loadAttributeModels, callerContext);
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeUniqueIdentifier"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean EnsureAttributes(Entity entity, AttributeUniqueIdentifier attributeUniqueIdentifier, Boolean loadAttributeModels, CallerContext callerContext)
        {
            return EnsureEntityAttributes(new EntityCollection { entity }, null, null, new AttributeUniqueIdentifierCollection { attributeUniqueIdentifier }, loadAttributeModels, callerContext);
        }

        /// <summary>
        ///     Makes sure that expected attributes are loaded. If they are not there, it loads the remaining.
        /// </summary>
        /// <param name="entityCollection">Indicates Entity objects for which we are checking attributes</param>
        /// <param name="attributeUniqueIdentifiers">Attributes unique identifiers</param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Result of operation</returns>
        /// <exception cref="Exception">Thrown when requested attribute is not available</exception>
        public Boolean EnsureAttributes(EntityCollection entityCollection, IEnumerable<AttributeUniqueIdentifier> attributeUniqueIdentifiers, Boolean loadAttributeModels, CallerContext callerContext)
        {
            AttributeUniqueIdentifierCollection uniqueIdentifiers = null;

            if (attributeUniqueIdentifiers != null)
            {
                uniqueIdentifiers = new AttributeUniqueIdentifierCollection();

                foreach (var attributeUniqueIdentifier in attributeUniqueIdentifiers)
                {
                    uniqueIdentifiers.Add(attributeUniqueIdentifier);
                }
            }

            return EnsureEntityAttributes(entityCollection, null, null, uniqueIdentifiers, loadAttributeModels, callerContext);
        }

        /// <summary>
        ///     Ensuring Attributes based on attribute Model context.
        /// </summary>
        /// <param name="entityCollection">Indicates Entity objects for which we are checking attributes</param>
        /// <param name="attributeModelContext">Indicates attributeModelContext</param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Result of operation</returns>
        /// <exception cref="MDMOperationException">Thrown when Entity or AttributeModelContext is not available</exception>
        public Boolean EnsureAttributes(EntityCollection entityCollection, AttributeModelContext attributeModelContext, Boolean loadAttributeModels, CallerContext callerContext)
        {
            Boolean result = true;

            #region Parameter Validation

            if (attributeModelContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111801", false, callerContext);
                throw new MDMOperationException("111801", _localeMessage.Message, "EntityManager", String.Empty, "EnsureAttributeValues"); //Attribute Model Context is not available.
            }

            #endregion

            var attributeModelBL = new AttributeModelBL();

            Collection<Int32> attributeIdList = attributeModelBL.GetAttributeIdList(null, attributeModelContext);

            if (entityCollection.Count > 0 && attributeIdList != null && attributeIdList.Count > 0)
            {
                result = EnsureEntityAttributes(entityCollection, attributeIdList, null, null, loadAttributeModels, callerContext);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeIds"></param>
        /// <param name="dataLocales"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean EnsureAttributes(EntityCollection entityCollection, IEnumerable<Int32> attributeIds, Collection<LocaleEnum> dataLocales, Boolean loadAttributeModels, CallerContext callerContext)
        {
            return EnsureEntityAttributes(entityCollection, attributeIds != null ? new Collection<Int32>(attributeIds.ToList()) : null, dataLocales, null, loadAttributeModels, callerContext);
        }

        /// <summary>
        ///     Ensures inherited values for the entity to be created and also for existing entity based on parent information
        /// </summary>
        /// <param name="entity">Entity for which inherited values needs to be ensured</param>
        /// <param name="callerContext">Object having caller context details</param>
        /// <returns>Flag indicating where ensure is successful or not</returns>
        public Boolean EnsureInheritedAttributes(Entity entity, CallerContext callerContext)
        {
            Boolean result = false;
            AttributeModelCollection attributeModels = null;

            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                #region STEP : Validation

                if (entity == null)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                    throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Update"); //Entity is null
                }

                if (entity.EntityContext == null)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111792", false, callerContext);
                    throw new MDMOperationException("111792", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //EntityContext
                }

                #endregion

                #region STEP : Initial Setup

                //Get command properties
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                #endregion

                #region STEP : Load Attribute Models

                if (entity.OriginalEntity != null && entity.OriginalEntity.AttributeModels != null && entity.OriginalEntity.AttributeModels.Count > 0)
                {
                    //Get attribute models from original entity
                    attributeModels = entity.OriginalEntity.AttributeModels;
                }
                else if (entity.EntityContext != null)
                {
                    //Get All Attribute models as per the entityContext
                    var attributeModelManager = new AttributeModelBL();

                    //Prepare Attribute Model Context
                    var attributeModelContext = new AttributeModelContext
                    {
                        ContainerId = entity.EntityContext.ContainerId,
                        CategoryId = entity.EntityContext.CategoryId,
                        EntityTypeId = entity.EntityContext.EntityTypeId,
                        Locales = entity.EntityContext.DataLocales,
                        AttributeModelType = AttributeModelType.All,
                        ApplyAttributeDependency = false
                    };

                    attributeModels = attributeModelManager.Get(attributeModelContext);
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Loaded inheritable attribute models");
                }

                #endregion

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    var inheritableAttributeModels = new AttributeModelCollection();
                    var inheritableAttrIdsToBeEnsured = new Collection<Int32>();

                    #region STEP : Find inheritable attributes and prepare inheritable attribute Id list which needs to be ensured

                    foreach (AttributeModel attributeModel in attributeModels)
                    {
                        if (attributeModel.Inheritable)
                        {
                            Attribute attribute = null;

                            AttributeCollection entityAttributes = entity.Attributes;

                            if (entityAttributes != null && entityAttributes.Count > 0)
                            {
                                attribute = (Attribute)entity.GetAttribute(attributeModel.Id, attributeModel.Locale);
                            }

                            if (attribute == null)
                            {
                                if (!inheritableAttrIdsToBeEnsured.Contains(attributeModel.Id))
                                {
                                    inheritableAttrIdsToBeEnsured.Add(attributeModel.Id);
                                }

                                inheritableAttributeModels.Add(attributeModel, true);
                            }
                        }
                    }

                    #endregion

                    #region STEP : Get Inherited Values from DB

                    if (inheritableAttrIdsToBeEnsured.Count > 0)
                    {
                        var entityDA = new EntityDA();
                        entityDA.EnsureInheritedValues(entity, inheritableAttrIdsToBeEnsured, inheritableAttributeModels, GlobalizationHelper.GetSystemDataLocale(), command);
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity inherited attribute values loaded from database");
                    }

                    #endregion

                    #region STEP: Update ensured attribute models for inherited values loaded by DB call

                    if (inheritableAttributeModels.Count > 0)
                    {
                        var entityAttributeModels = entity.AttributeModels;

                        foreach (var inheritableAttrModel in inheritableAttributeModels)
                        {
                            if (!entityAttributeModels.Contains(inheritableAttrModel.Id, inheritableAttrModel.Locale))
                            {
                                entityAttributeModels.Add(inheritableAttrModel, true);
                            }
                        }
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Ensured inheritable attribute models in entity");
                    }

                    #endregion

                    #region STEP: Load Lookup DisplayValue and ExportValue in the data

                    var entityFillOptions = new EntityGetOptions.EntityFillOptions();

                    entityFillOptions.SetAllEntityPropertiesLevelFillOptionsTo(false);
                    entityFillOptions.FillLookupDisplayValues = true;
                    entityFillOptions.FillLookupRowWithValues = false;

                    EntityFillHelper.FillEntity(entity, entityFillOptions, new EntityBL(), callerContext);

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Loaded lookup display values for ensured inherited attribute");
                    }

                    #endregion

                    result = true;
                }
                else
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        String traceMessage = String.Format("No attribute modes found for given context {0}. Inherited values cannot be ensured.", entity.EntityContext.ToXml());
                        diagnosticActivity.LogInformation(traceMessage);
                    }
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return result;
        }

        #endregion

        #region Ensure Entity Relationships and Relationship attribute methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="loadExistingRelationships"></param>
        /// <param name="relationshipTypeIds"></param>
        /// <param name="attributeIds"></param>
        /// <param name="attributeUniqueIdentifiers"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean EnsureRelationships(EntityCollection entities, Boolean loadExistingRelationships, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext)
        {
            return EnsureRelationshipsData(entities, loadExistingRelationships, relationshipTypeIds, attributeIds, attributeUniqueIdentifiers, callerContext);
        }

        #endregion

        #region Ensure Entity Hierarchy

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityHierarchyContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean EnsureEntityHierarchy(Entity entity, EntityHierarchyContext entityHierarchyContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            return EnsureEntityHierarchyMain(entity, entityHierarchyContext, entityGetOptions, callerContext);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeIds"></param>
        /// <param name="dataLocales"></param>
        /// <param name="attributeUniqueIdentifiers"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean EnsureEntityAttributes(EntityCollection entityCollection, Collection<Int32> attributeIds, Collection<LocaleEnum> dataLocales, AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, Boolean loadAttributeModels, CallerContext callerContext)
        {
            Boolean result = true;

            var diagnosticActivity = new DiagnosticActivity();

            try
            {
                #region Step: Validations and setup

                if (entityCollection == null || entityCollection.Count < 1)
                {
                    diagnosticActivity.LogError("EnsureEntityAttributes method has been called with null or empty entity collection. Operation is terminating with argument null exception");

                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111796", false, callerContext);
                    throw new MDMOperationException("111796", _localeMessage.Message, "EntityManager", String.Empty, "EnsureAttributeValues"); //Entity is not available.
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                    diagnosticActivity.LogMessageWithData(String.Format("See 'View Data' for entity data at the start of EnsureEntityAttributes call for all requested entities"), entityCollection.ToXml());
                }

                #endregion

                #region Step: Resolve attribute unique identifiers and get list of attribute ids to be ensured

                var entityAndAttributeIdListPairs = new Collection<KeyValuePair<Entity, Collection<Int32>>>();

                if (attributeUniqueIdentifiers != null && attributeUniqueIdentifiers.Count > 0)
                {
                    var resolvedAttributeIds = ResolveAttributeUniqueIdentifiers(attributeUniqueIdentifiers);

                    if (resolvedAttributeIds != null && resolvedAttributeIds.Count > 0)
                    {
                        if (attributeIds == null)
                        {
                            attributeIds = new Collection<Int32>();
                        }

                        attributeIds = ValueTypeHelper.MergeCollections(attributeIds, resolvedAttributeIds);
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Resolved attribute unique identifiers into attribute ids");
                    }
                }

                #endregion

                foreach (Entity entity in entityCollection)
                {
                    Collection<LocaleEnum> dataLocaleList = new Collection<LocaleEnum>();
                    dataLocaleList.AddRange<LocaleEnum>(entity.EntityContext.DataLocales);
                    dataLocaleList.AddRange<LocaleEnum>(dataLocales);

                    AttributeCollection attributes = entity.Attributes;
                    AttributeModelCollection attributeModels = entity.AttributeModels;
                    AttributeCollection originalAttributes = null;
                    AttributeModelCollection originalAttributeModels = null;

                    if (entity.OriginalEntity != null)
                    {
                        originalAttributes = entity.OriginalEntity.Attributes;
                        originalAttributeModels = entity.OriginalEntity.AttributeModels;
                    }

                    var missingAttrIds = FillAndIdentifyMissingAttributes(attributeIds, dataLocaleList, attributes, originalAttributes, attributeModels, originalAttributeModels);

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Fill and identify missing attributes completed for entity id:{0}", entity.Id));
                    }

                    if (missingAttrIds.Count > 0)
                    {
                        var pair = new KeyValuePair<Entity, Collection<Int32>>(entity, missingAttrIds);
                        entityAndAttributeIdListPairs.Add(pair);
                    }
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Fill and identify missing attributes completed for all entities"));
                }

                if (entityAndAttributeIdListPairs.Count > 0)
                {
                    result = LoadMissingEntityAttributes(entityAndAttributeIdListPairs, dataLocales, loadAttributeModels, callerContext);
                }

                #region Complex attributes child attributes processing

                foreach (Entity entity in entityCollection)
                {
                    if (entity.AttributeModels == null || entity.AttributeModels.Count == 0 || entity.Attributes == null)
                    {
                        continue;
                    }
                    if (attributeIds != null && attributeIds.Any())
                    {
                        foreach (Int32 attrId in attributeIds)
                        {
                            foreach (LocaleEnum locale in entity.EntityContext.DataLocales)
                            {
                                AttributeModel model = (AttributeModel)entity.AttributeModels.GetAttributeModel(attrId, locale);
                                if (model == null || !model.IsComplex || model.AttributeModels == null)
                                {
                                    continue;
                                }
                                Attribute attribute = (Attribute)entity.Attributes.GetAttribute(attrId, locale);
                                if (attribute == null || attribute.Attributes == null)
                                {
                                    continue;
                                }
                                foreach (Attribute complexAttrInstance in attribute.Attributes)
                                {
                                    if (complexAttrInstance.Attributes == null)
                                    {
                                        complexAttrInstance.Attributes = new AttributeCollection();
                                    }
                                    foreach (AttributeModel complexChildAttrModel in model.AttributeModels)
                                    {
                                        if (!complexAttrInstance.Attributes.Contains(complexChildAttrModel.Id, complexAttrInstance.Locale))
                                        {
                                            complexAttrInstance.Attributes.Add(new Attribute(complexChildAttrModel, complexAttrInstance.Locale));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Loaded missing entity attributes by making GetEntity call"));
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("EnsureEntityAttributes failed with exception: {0}", ex.Message));
                throw;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData(String.Format("See 'View Data' for entity data after EnsureEntityAttributes call is completed"), entityCollection.ToXml());
                    diagnosticActivity.Stop();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityAndAttributeIdListPairs"></param>
        /// <param name="dataLocales"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean LoadMissingEntityAttributes(IEnumerable<KeyValuePair<Entity, Collection<Int32>>> entityAndAttributeIdListPairs,Collection<LocaleEnum> dataLocales, Boolean loadAttributeModels, CallerContext callerContext)
        {
            Boolean result = true;

            foreach (var pair in entityAndAttributeIdListPairs)
            {
                Entity entity = pair.Key;
                Collection<Int32> attributeIdList = pair.Value;

                if (entity == null)
                {
                    LocaleMessage localeMessage = new LocaleMessageBL().Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                    throw new MDMOperationException("111815", localeMessage.Message, "EntityManager", String.Empty, "LoadAttributes"); //Entity is null. 
                }

                var ensuredAttributes = new AttributeCollection();
                var ensuredAttributeModels = new AttributeModelCollection();

                var entityContext = new EntityContext();
                entityContext.LoadEntityProperties = false;
                //Some information are to be copied from entity 
                //These information are not supposed to change for an entity after it is loaded initially
                entityContext.ContainerId = entity.ContainerId;
                entityContext.EntityTypeId = entity.EntityTypeId;
                entityContext.CategoryId = entity.CategoryId;
                entityContext.Locale = entity.Locale;
                entityContext.DataLocales = dataLocales;
                entityContext.LoadAttributes = true;
                entityContext.AttributeIdList = attributeIdList;
                entityContext.LoadAttributeModels = loadAttributeModels;

                if (entity.EntityContext != null)
                {
                    entityContext.LoadDependentAttributes = entity.EntityContext.LoadDependentAttributes;
                }

                //LoadLookupDisplayValues flag has to be true here as BR calls this method by EnsureAttribute flow
                entityContext.LoadLookupDisplayValues = true;
                entityContext.LoadLookupRowWithValues = true;

                if (entity.Id > 0)
                {
                    var entityIds = new Collection<Int64> { entity.Id };

                    var entityGetOptions = new EntityGetOptions { PublishEvents = false, ApplyAVS = false, ApplySecurity = false };

                    EntityCollection returnedEntities = _entityManager.Get(entityIds, entityContext, entityGetOptions, callerContext);

                    Entity returnedEntity = returnedEntities != null && returnedEntities.Count > 0 ? returnedEntities.FirstOrDefault() : null;

                    if (returnedEntity != null)
                    {
                        if (entity.OriginalEntity == null)
                        {
                            entity.OriginalEntity = returnedEntity;
                        }

                        ensuredAttributes = returnedEntity.Attributes;
                        ensuredAttributeModels = returnedEntity.AttributeModels;

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

                            entity.OriginalEntity.EntityContext = entity.EntityContext; //Update the original Entity.EntityContext
                        }
                    }
                }
                else
                {
                    AttributeCollection attributeCollection;
                    AttributeModelCollection attributeModelCollection;

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to load attribute models", MDMTraceSource.EntityGet);

                    var attributeModelContext = new AttributeModelContext(entity.ContainerId, entity.EntityTypeId, 0, entity.CategoryId, dataLocales, 0, entityContext.AttributeModelType, entityContext.LoadCreationAttributes, false /*only required attributes*/, false /*load complete details*/);
                    attributeModelContext.ApplySorting = false;
                    attributeModelContext.ApplySecurity = false;

                    EntityAttributeModelHelper.LoadAttributeModelsWithBlankAttributeInstances(attributeIdList, null, null, attributeModelContext, out attributeCollection, out attributeModelCollection);

                    if (entityContext.LoadAttributes)
                    {
                        ensuredAttributes = attributeCollection;
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of attributes loaded for model: " + entity.Attributes.Count, MDMTraceSource.EntityGet);
                    }

                    if (entityContext.LoadAttributeModels)
                    {
                        ensuredAttributeModels = attributeModelCollection;

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of attribute models loaded: " + entity.AttributeModels.Count, MDMTraceSource.EntityGet);
                    }
                }

                if (ensuredAttributes != null && ensuredAttributes.Count > 0)
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
                    //Failed to get missing attributes
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="loadExistingRelationships"></param>
        /// <param name="relationshipTypeIds"></param>
        /// <param name="attributeIds"></param>
        /// <param name="attributeUniqueIdentifiers"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean EnsureRelationshipsData(EntityCollection entities, Boolean loadExistingRelationships, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext)
        {
            Boolean result = true;

            #region Diagnostics and Tracing

            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                var executionContext = new ExecutionContext(callerContext, new CallDataContext(), null, String.Empty, new Collection<MDMTraceSource> { MDMTraceSource.EntityGet });
                diagnosticActivity.Start(executionContext);
                diagnosticActivity.LogMessageWithData(String.Format("See 'View Data' for entity data at the start of EnsureRelationshipsData call for all requested entities"), entities.ToXml());
            }

            #endregion

            #region Step: Validations

            //Entity is not available.
            if (entities == null || entities.Count < 1)
            {
                diagnosticActivity.LogError("EnsureRelationshipsData method has been called with null or empty entity collection. Operation is terminating with argument null exception");

                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111796", false, callerContext);
                throw new MDMOperationException("111796", _localeMessage.Message, "EntityManager", String.Empty, "EnsureRelationships");
            }

            #endregion

            try
            {
                #region Step: Resolve attribute unique identifiers

                Collection<Int32> requestedAttributeIds = null;

                if (attributeIds != null && attributeIds.Count > 0)
                {
                    requestedAttributeIds = attributeIds;
                }
                else
                {
                    requestedAttributeIds = new Collection<Int32>();
                }

                if (attributeUniqueIdentifiers != null && attributeUniqueIdentifiers.Count > 0)
                {
                    var resolvedAttributeIds = ResolveAttributeUniqueIdentifiers(attributeUniqueIdentifiers);

                    if (resolvedAttributeIds != null && resolvedAttributeIds.Count > 0)
                    {
                        requestedAttributeIds = ValueTypeHelper.MergeCollections(requestedAttributeIds, resolvedAttributeIds);
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Resolved attribute unique identifiers into attribute ids");
                    }
                }

                #endregion

                foreach (var entity in entities)
                {
                    #region Step: Initial setup and declarations

                    var dataLocales = entity.EntityContext.DataLocales;

                    if (dataLocales == null)
                    {
                        dataLocales = new Collection<LocaleEnum>();
                        entity.EntityContext.DataLocales = dataLocales;
                    }

                    if (dataLocales.Count < 1)
                    {
                        dataLocales.Add(entity.Locale);
                    }

                    var isGetApiCallRequired = false;
                    var relationshipTypeModelToBeLoaded = new Collection<Int32>();

                    var modelRelationshipsByRelationshipType = new Dictionary<Int32, Relationship>();

                    var relationships = entity.Relationships;

                    RelationshipCollection existingRelationships = null;
                    RelationshipCollection originalRelationships = null;

                    if (entity.OriginalEntity != null)
                    {
                        originalRelationships = entity.OriginalEntity.Relationships;
                    }

                    var relationshipContext = new RelationshipContext
                    {
                        ContainerId = entity.ContainerId,
                        DataLocales = dataLocales,
                        Level = 1, //Ensure relationship works only on LEVEL 1 relationships as of 7.6.7
                        LoadRelationshipAttributes = true,
                        RelationshipTypeIdList = relationshipTypeIds
                    };

                    var relationshipLoadEntityContext = new EntityContext
                    {
                        LoadEntityProperties = false,
                        ContainerId = entity.ContainerId,
                        EntityTypeId = entity.EntityTypeId,
                        CategoryId = entity.CategoryId,
                        Locale = entity.Locale,
                        DataLocales = dataLocales,
                        LoadRelationships = true,
                        RelationshipContext = relationshipContext
                    };

                    #endregion

                    #region Step: Fill and identify missing attributes / relationship type models to be loaded

                    if (relationships != null && relationships.Count > 0)
                    {
                        foreach (var relationship in relationships)
                        {
                            if (!relationshipTypeIds.Contains(relationship.RelationshipTypeId))
                                continue;

                            var originalRelationship = relationship.OriginalRelationship;

                            var attributes = relationship.RelationshipAttributes;
                            var originalAttributes = originalRelationship != null ? originalRelationship.RelationshipAttributes : null;

                            Collection<Int32> missingAttrIds = null;

                            if (requestedAttributeIds.Count > 0)
                            {
                                missingAttrIds = FillAndIdentifyMissingAttributes(requestedAttributeIds, dataLocales, attributes, originalAttributes, null, null);
                            }

                            if (missingAttrIds != null && missingAttrIds.Count > 0 && entity.Id > 0 && relationship.Id > 0)
                            {
                                isGetApiCallRequired = true;
                            }
                            else if (!relationshipTypeModelToBeLoaded.Contains(relationship.RelationshipTypeId) && relationship.Id < 1)
                            {
                                relationshipTypeModelToBeLoaded.Add(relationship.RelationshipTypeId);
                            }
                        }
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Fill and identify missing attributes / relationship type models completed for entity id:{0}", entity.Id));
                    }

                    #endregion

                    #region Step: Load Existing Relationships using GetEntity API call if needed

                    if (entity.Id > 0 && (isGetApiCallRequired || loadExistingRelationships))
                    {
                        var entityGetOptions = new EntityGetOptions { PublishEvents = false, ApplyAVS = false, ApplySecurity = false };

                        var returnedEntity = _entityManager.Get(entity.Id, relationshipLoadEntityContext, entityGetOptions, callerContext);

                        if (returnedEntity != null)
                        {
                            existingRelationships = returnedEntity.Relationships;
                        }

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Loaded existing relationships by making GetEntity call for entity id:{0}", entity.Id));
                        }
                    }

                    #endregion

                    #region Step: Load model relationships for relationship type models

                    if (relationshipTypeModelToBeLoaded.Count > 0)
                    {
                        var relationshipManager = new RelationshipBL();
                        var containerId = entity.ContainerId;

                        foreach (var relationshipTypeId in relationshipTypeModelToBeLoaded)
                        {
                            var modelRelationship = relationshipManager.GetRelationshipModel(relationshipTypeId, containerId, dataLocales, callerContext);

                            if (modelRelationship != null)
                            {
                                modelRelationshipsByRelationshipType.Add(relationshipTypeId, modelRelationship);
                            }
                        }

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Loaded relationship models by making GetRelationshipModel call for entity id:{0}", entity.Id));
                        }
                    }

                    #endregion

                    #region Step: Loop through each existing relationship and fill missing attributes from existing relationship / model relationship

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
                            var attributes = relationship.RelationshipAttributes;
                            var ensuredAttributes = ensuredRelationship.RelationshipAttributes;
                            AttributeCollection originalAttributes = null;

                            if (relationship.OriginalRelationship != null)
                            {
                                originalAttributes = relationship.OriginalRelationship.RelationshipAttributes;
                            }

                            if (ensuredAttributes != null && ensuredAttributes.Count > 0)
                            {
                                var filteredEnsuredAttributes = new AttributeCollection();

                                if (!loadExistingRelationships && requestedAttributeIds.Count > 0)
                                {
                                    foreach (var ensuredAttribute in ensuredAttributes)
                                    {
                                        if (requestedAttributeIds.Contains(ensuredAttribute.Id))
                                        {
                                            filteredEnsuredAttributes.Add(ensuredAttribute.Clone());
                                        }
                                    }
                                }
                                else
                                {
                                    filteredEnsuredAttributes = (AttributeCollection)ensuredAttributes.Clone();
                                }

                                FillEnsuredAttributes(filteredEnsuredAttributes, null, attributes, originalAttributes, null, null);
                            }
                        }
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Loaded missing relationship attributes by using original entity / existing relationships / relationship models for entity id:{0}", entity.Id));
                    }

                    #endregion

                    #region Step: Add all missing relationships if loadAllExistingRelationships is set to true

                    if (entity.Id > 0 && loadExistingRelationships && existingRelationships != null && existingRelationships.Count > 0)
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

                            if (originalRelationships != null)
                            {
                                if(!originalRelationships.Contains(existingRelationship.FromEntityId, existingRelationship.RelatedEntityId, 
                                                        existingRelationship.ContainerId, existingRelationship.RelationshipTypeId))
                                {
                                    originalRelationships.Add(existingRelationship.Clone());
                                }
                            }
                        }

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Loaded missing relationships from existing relationships for entity id:{0}", entity.Id));
                        }
                    }

                    #endregion

                    #region Step: Update Entity context

                    relationshipLoadEntityContext.UpdateContext(relationshipLoadEntityContext);

                    if (entity.OriginalEntity != null)
                    {
                        entity.OriginalEntity.EntityContext.UpdateContext(relationshipLoadEntityContext);
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Updated entity context for all the ensured data for entity id:{0}", entity.Id));
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("EnsureRelationshipsData failed with exception: {0}", ex.Message));
                throw;
            }
            finally
            {
                #region Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData(String.Format("See 'View Data' for entity data after EnsureRelationshipsData call is completed"), entities.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityHierarchyContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean EnsureEntityHierarchyMain(Entity entity, EntityHierarchyContext entityHierarchyContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();

            Boolean successFlag = true;

            try
            {
                #region Step: Parameter Validations

                if (entity == null)
                {
                    diagnosticActivity.LogError("EnsureEntityHierarchyMain method has been called with null entity. Operation is terminating with argument null exception");

                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111796", false, callerContext);
                    throw new MDMOperationException("111796", _localeMessage.Message, "EntityManager", String.Empty, "EnsureEntityHierarchyMain");
                }

                if (entity.Id <= 0)
                {
                    diagnosticActivity.LogMessage(MessageClassEnum.Error, "EntityId must be greater than 0");
                    throw new MDMOperationException("111795", "EntityId must be greater than 0", "EntityManager", String.Empty, "EnsureEntityHierarchyMain");
                }

                #endregion

                #region Step: Initial Setup

                callerContext.ProgramName = String.Format("{0}:{1}", callerContext.ProgramName, "EnsureEntityHierarchy");

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                    diagnosticActivity.LogMessageWithData(String.Format("See 'View Data' for entity data at the start of EnsureEntityHierarchy call for all requested entities"), entity.ToXml());
                }

                #endregion

                #region Step: Create main entity context

                var entityGetContext = new EntityContext
                {
                    ContainerId = entity.ContainerId,
                    ContainerName = entity.ContainerName,
                    EntityTypeId = entity.EntityTypeId,
                    EntityTypeName = entity.EntityTypeName,
                    CategoryId = entity.CategoryId,
                    CategoryPath = entity.CategoryPath,

                    Locale = entity.Locale,
                    DataLocales = entity.EntityContext.DataLocales,

                    EntityHierarchyContext = entityHierarchyContext,
                    ResolveIdsByName = true,

                    LoadEntityProperties = false,
                    LoadAttributes = false,
                    LoadRelationships = false,
                    LoadHierarchyRelationships = true,
                    LoadExtensionRelationships = false,
                    LoadLookupDisplayValues = false,
                    LoadLookupRowWithValues = false,
                    LoadAttributeModels = false
                };

                #endregion

                #region Step: Consolidate and fix EntityHierarchyContext properties

                var childEntityLoadContexts = entityHierarchyContext.GetEntityContexts();

                if (childEntityLoadContexts != null && childEntityLoadContexts.Count > 0)
                {
                    #region Step: Set LoadAttributes to true if we have any attribute id list / attribute name list given in the child contexts

                    foreach (var childEntityContext in childEntityLoadContexts)
                    {
                        if ((!childEntityContext.LoadAttributes) && (childEntityContext.AttributeIdList.Count > 0 || childEntityContext.AttributeNames.Count > 0))
                        {
                            childEntityContext.LoadAttributes = true;
                        }

                        if ((!childEntityContext.LoadRelationships) && (childEntityContext.RelationshipContext.RelationshipTypeIdList.Count > 0 || childEntityContext.RelationshipContext.RelationshipTypeNames.Count > 0))
                        {
                            childEntityContext.LoadRelationships = true;
                        }
                    }

                    #endregion
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Consolidated and fixed entity hierarchy context properties");
                }

                #endregion

                #region Step: Call Get entity with Hierarchy relationships API

                successFlag = _entityManager.LoadEntityHierarchy(entity, entityGetContext, entityGetOptions, callerContext);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Loaded entity with hierarchy child entities");
                }

                #endregion
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("EnsureEntityHierarchy failed with exception: {0}", ex.Message));
                throw;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData(String.Format("See 'View Data' for entity data after EnsureEntityHierarchy call is completed"), entity.ToXml());
                    diagnosticActivity.Stop();
                }
            }

            return successFlag;
        }

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeUniqueIdentifiers"></param>
        /// <returns></returns>
        private Collection<Int32> ResolveAttributeUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers)
        {
            var attributeIds = new Collection<Int32>();

            if (attributeUniqueIdentifiers != null && attributeUniqueIdentifiers.Count > 0)
            {
                var attributeModelManager = new AttributeModelBL();
                attributeIds = attributeModelManager.GetAttributeIdList(attributeUniqueIdentifiers);
            }

            return attributeIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiredAttributeIds"></param>
        /// <param name="requiredDataLocales"></param>
        /// <param name="attributes"></param>
        /// <param name="originalAttributes"></param>
        /// <param name="attributeModels"></param>
        /// <param name="originalAttributeModels"></param>
        /// <returns></returns>
        private Collection<Int32> FillAndIdentifyMissingAttributes(Collection<Int32> requiredAttributeIds, Collection<LocaleEnum> requiredDataLocales, AttributeCollection attributes, AttributeCollection originalAttributes, AttributeModelCollection attributeModels, AttributeModelCollection originalAttributeModels)
        {
            var missingAttributeIds = new Collection<Int32>();

            if (requiredAttributeIds == null || requiredAttributeIds.Count < 1)
            {
                return missingAttributeIds;
            }

            if (requiredDataLocales == null || requiredDataLocales.Count < 1)
            {
                return missingAttributeIds;
            }

            if (attributes == null || attributes.Count < 1)
            {
                return requiredAttributeIds;
            }

            foreach (var attrId in requiredAttributeIds)
            {
                foreach (var dataLocale in requiredDataLocales)
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
                                            if (!missingAttributeIds.Contains(attrId))
                                            {
                                                missingAttributeIds.Add(attrId);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!missingAttributeIds.Contains(attrId))
                                {
                                    missingAttributeIds.Add(attrId);
                                }
                            }
                        }
                        else
                        {
                            if (!missingAttributeIds.Contains(attrId))
                            {
                                missingAttributeIds.Add(attrId);
                            }
                        }
                    }

                    if (attributeModels != null && !attributeModels.Contains(attrId, dataLocale))
                    {
                        if (!missingAttributeIds.Contains(attrId))
                        {
                            missingAttributeIds.Add(attrId);
                        }

                        break;
                    }
                }
            }

            return missingAttributeIds;
        }

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
        private void FillEnsuredAttributes(AttributeCollection ensuredAttributes, AttributeModelCollection ensuredAttributeModels, AttributeCollection attributes, AttributeCollection originalAttributes, AttributeModelCollection attributeModels, AttributeModelCollection originalAttributeModels)
        {
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
        }

        #endregion

        #endregion

        #endregion
    }
}