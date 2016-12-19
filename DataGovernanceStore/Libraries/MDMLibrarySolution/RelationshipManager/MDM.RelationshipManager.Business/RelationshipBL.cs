using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.RelationshipManager.Business
{
    using BusinessObjects.Diagnostics;
    using MDM.AdminManager.Business;
    using MDM.AttributeModelManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.RelationshipManager.Data;
    using MDM.Utility;
    using MDM.EntityProcessorManager.Business;

    /// <summary>
    /// Specifies business logic operations for Relationships
    /// </summary>
    public class RelationshipBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = null;

        /// <summary>
        /// Field denoting system UI locale.
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting system Data locale.
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Specifies list of relationship parent id list for a level.
        /// </summary>
        private Collection<Int64> _relationshipParentIdList = new Collection<Int64>();

        /// <summary>
        /// Specifies current trace setting
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public RelationshipBL()
        {
            GetSecurityPrincipal();
            InitalizeMembers();
        }

        /// <summary>
        /// Instantiates Relationship Business Logic with provided Security Principal
        /// </summary>
        /// <param name="securityPrinicipal"></param>
        public RelationshipBL(SecurityPrincipal securityPrinicipal)
        {
            this._securityPrincipal = securityPrinicipal;
            InitalizeMembers();
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Get and Load Methods

        /// <summary>
        /// Gets relationships for the requested entity id
        /// </summary>
        /// <param name="entityId">Id of the entity for which relationships are required</param>
        /// <param name="containerId">Container Id of Entity for which relationships are to be fetched.</param>
        /// <param name="relationshipContext">Relationship context indicates relationship type, locale etc,</param>
        /// <param name="entityCacheStatus">Specifies the cache status of the entity</param>
        /// <param name="callerContext">Name of application and module which is performing action</param>
        /// <param name="iEntityManager">Specifies instance of Entity Manager.</param>
        /// <param name="updateCache">Specifies whether to update the cache after loading</param>
        /// <returns>collection of relationships for requested entity id.</returns>
        /// <exception cref="ArgumentException">Thrown when entity id, container Id, is not available. Also thrown when RelationshipContext.RelationshipTypeId is not populated</exception>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipContext is null</exception>
        public RelationshipCollection Get(Int64 entityId, Int32 containerId, RelationshipContext relationshipContext, EntityCacheStatus entityCacheStatus, CallerContext callerContext, IEntityManager iEntityManager, Boolean updateCache = false)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            RelationshipCollection relationships = new RelationshipCollection();

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipBL.Get", MDMTraceSource.RelationshipGet, false);
                }

                #region Initial Setup

                Entity entity = new Entity();
                entity.Id = entityId;
                entity.ContainerId = containerId;
                entity.Locale = relationshipContext.Locale;

                #endregion

                ValidateEntity(entity, callerContext, "Get");

                EntityCacheStatusCollection entityCacheStatusCollection = new EntityCacheStatusCollection();
                if (entityCacheStatus != null)
                {
                    entityCacheStatusCollection.Add(entityCacheStatus);
                }

                Boolean isLoaded = GetRelationships(new EntityCollection() { entity }, relationshipContext, entityCacheStatusCollection, callerContext, false, iEntityManager, updateCache);

                if (isLoaded)
                {
                    relationships = entity.Relationships;
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipBL.Get", MDMTraceSource.RelationshipGet);
                }
            }

            return relationships;
        }

        /// <summary>
        /// Loads relationships for the requested entity
        /// </summary>
        /// <param name="entity">Entity for which needs to be loaded</param>
        /// <param name="entityCacheStatus">Cache status of the entity which needs to be loaded</param>
        /// <param name="callerContext">Name of application and module which is performing action</param>
        /// <param name="loadLatest">Load relationship from database if loadLatest is 'True' otherwise load from cache</param>
        /// <param name="iEntityManager">Specifies instance of Entity Manager.</param>
        /// <param name="updateCache">Specifies whether to update the cache after loading</param>
        /// <returns>The Boolean flag which tells whether the load is successful or not</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed entity object is null</exception>
        public Boolean LoadRelationship(Entity entity, EntityCacheStatus entityCacheStatus, CallerContext callerContext, Boolean loadLatest, IEntityManager iEntityManager, Boolean updateCache = false)
        {
            Boolean result = false;

            #region Parameter Validation

            ValidateEntity(entity, callerContext, "Get");

            #endregion

            #region Create RelationshipContext

            RelationshipContext relationshipContext = entity.EntityContext.RelationshipContext;

            relationshipContext.ContainerId = entity.EntityContext.ContainerId;
            relationshipContext.Locale = entity.Locale;
            relationshipContext.DataLocales = entity.EntityContext.DataLocales;

            #endregion

            result = LoadRelationship(entity, relationshipContext, entityCacheStatus, callerContext, loadLatest, iEntityManager, updateCache);

            return result;
        }

        /// <summary>
        /// Loads relationships for the requested entity
        /// </summary>
        /// <param name="entity">Entity for which needs to be loaded</param>
        /// <param name="relationshipContext">Relationship context indicates relationship type, locale etc,</param>
        /// <param name="entityCacheStatus">Specifies the cache status of the entities</param>
        /// <param name="callerContext">Name of application and module which is performing action</param>
        /// <param name="loadLatest">Load relationship from database if loadLatest is 'True' otherwise load from cache.</param>
        /// <param name="iEntityManager">Specifies instance of Entity Manager.</param>
        /// <param name="updateCache">Specifies whether to update the cache after loading</param>
        /// <returns>The Boolean flag which tells whether the load is successful or not</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed entity object is null</exception>
        public Boolean LoadRelationship(Entity entity, RelationshipContext relationshipContext, EntityCacheStatus entityCacheStatus, CallerContext callerContext, Boolean loadLatest, IEntityManager iEntityManager, Boolean updateCache = false)
        {
            return LoadRelationships(new EntityCollection() { entity }, relationshipContext, new EntityCacheStatusCollection() { entityCacheStatus }, callerContext, loadLatest, iEntityManager, updateCache);
        }

        /// <summary>
        /// Loads relationships for the requested entity
        /// </summary>
        /// <param name="entities">Entities for which needs to be loaded</param>
        /// <param name="relationshipContext">Relationship context indicates relationship type, locale etc,</param>
        /// <param name="entityCacheStatusCollection">Specifies the cache status of the entities</param>
        /// <param name="callerContext">Name of application and module which is performing action</param>
        /// <param name="loadLatest">Load relationship from database if loadLatest is 'True' otherwise load from cache.</param>
        /// <param name="iEntityManager">Specifies instance of Entity Manager.</param>
        /// <param name="updateCache">Specifies whether to update the cache after loading</param>
        /// <returns>The Boolean flag which tells whether the load is successful or not</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed entity collection object is null</exception>
        public Boolean LoadRelationships(EntityCollection entities, RelationshipContext relationshipContext, EntityCacheStatusCollection entityCacheStatusCollection, CallerContext callerContext, Boolean loadLatest, IEntityManager iEntityManager, Boolean updateCache = false)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            Boolean result = false;

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipBL.LoadRelationships", MDMTraceSource.RelationshipGet, false);
                }

                result = GetRelationships(entities, relationshipContext, entityCacheStatusCollection, callerContext, loadLatest, iEntityManager, updateCache);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipBL.LoadRelationships", MDMTraceSource.RelationshipGet);
                }
            }

            return result;
        }

        /// <summary>
        /// Loads the relationships for the requested entities based on the relationship context specified
        /// </summary>
        /// <param name="relationshipContextToEntityCollectionMaps">Collection of relationship context based entity collection</param>
        /// <param name="iEntityManager">Represents an instance of the EntityManager</param>
        /// <param name="callerContext">Represents the identity (Module, Application etc) of the caller who invoked the method</param>
        public void LoadRelationships(Collection<KeyValuePair<RelationshipContext, EntityCollection>> relationshipContextToEntityCollectionMaps, IEntityManager iEntityManager, CallerContext callerContext)
        {
            var relationshipDA = new RelationshipDA();

            var maxLevelToReturn = AppConfigurationHelper.GetAppConfig<Int16>("Relationship.RelationshipLevel", 3);

            var isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig("RelationshipManager.Relationship.Inheritance.Enabled", true);

            var command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            Collection<Int32> defaultRelationshipTypeIdList = null;

            foreach (KeyValuePair<RelationshipContext, EntityCollection> keyValuePair in relationshipContextToEntityCollectionMaps)
            {
                var relationshipContext = keyValuePair.Key;
                var entities = keyValuePair.Value;

                #region Step : Get Relationship Types

                if (relationshipContext.RelationshipTypeIdList == null || relationshipContext.RelationshipTypeIdList.Count < 1)
                {
                    if (defaultRelationshipTypeIdList == null)
                    {
                        // Load the default RelationshipContext once
                        PopulateRelationshipTypeIdList(relationshipContext, callerContext);
                        defaultRelationshipTypeIdList = relationshipContext.RelationshipTypeIdList;
                    }
                    else
                    {
                        relationshipContext.RelationshipTypeIdList = defaultRelationshipTypeIdList;
                    }
                }

                #endregion

                var attributeModelsPerRT = GetRelationshipAttributeModelsByContext(relationshipContext);

                #region Step : Fetch Locale list for attribute models

                Collection<LocaleEnum> datalLocaleList = relationshipContext.DataLocales;

                if (datalLocaleList != null && !datalLocaleList.Contains(_systemDataLocale))
                {
                    foreach (AttributeModelCollection attributeModels in attributeModelsPerRT.Values)
                    {
                        if (DoesNonLocalizableAttributeExist(attributeModels))
                        {
                            datalLocaleList.Add(_systemDataLocale);
                            break;
                        }
                    }
                }

                #endregion

                //NOTE : KEY -> EntityId  Value -> Relationship collection
                Dictionary<Int64, RelationshipCollection> relationshipsDictionary = null;

                //NOTE : KEY -> EN.RelationshipId.Locale.ER Value -> Relationship attributes
                Dictionary<String, AttributeCollection> relationshipAttributesDictionary = null;

                relationshipDA.GetRelationships(entities, relationshipContext, attributeModelsPerRT, maxLevelToReturn, callerContext, command, out relationshipsDictionary, out relationshipAttributesDictionary, false, datalLocaleList);

                #region Step : Create blank instances of attribute not found anywhere and is still missing

                //TODO: do we need to consider LoadBlankAttributes flag over here??
                if (relationshipContext.LoadRelationshipAttributes)
                {
                    foreach (Entity entity in entities)
                    {
                        CreateAttributesBlankInstance(entity, entity.Relationships, attributeModelsPerRT, relationshipAttributesDictionary, relationshipContext);
                    }
                }

                #endregion

                #region Step : Verify Denormalized Relationships Status

                if (relationshipContext.LoadDenormalizedRelationshipsStatus && isRelationshipInheritanceEnabled)
                {
                    IsDenormalizedRelationshipsDirty(entities, relationshipContext, callerContext);
                }

                #endregion

                #region Step : Determine Data Model Security

                DetermineDataModelSecurity(entities, relationshipContext);

                #endregion

                #region Step : Load Related Entities

                //Load related entity details only if system has configured common attributes.
                if (!relationshipContext.ReturnRelatedEntityDetails)
                {
                    LoadRelatedEntities(entities, relationshipContext, iEntityManager, false, callerContext);
                }

                #endregion
            }
        }

        #region Get Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipContext"></param>
        /// <returns></returns>
        public Dictionary<Int32, AttributeModelCollection> GetRelationshipAttributeModelsByContext(RelationshipContext relationshipContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT = new Dictionary<Int32, AttributeModelCollection>();

            if (relationshipContext.LoadRelationshipAttributes)
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading attribute models...", MDMTraceSource.RelationshipGet);
                }

                foreach (Int32 relationshipTypeId in relationshipContext.RelationshipTypeIdList)
                {
                    AttributeModelContext attributeModelContext = new AttributeModelContext();
                    attributeModelContext.AttributeModelType = AttributeModelType.Relationship;
                    attributeModelContext.RelationshipTypeId = relationshipTypeId;
                    attributeModelContext.ContainerId = relationshipContext.ContainerId;
                    attributeModelContext.Locales = relationshipContext.DataLocales;

                    AttributeModelBL attributeModelManager = new AttributeModelBL();
                    AttributeModelCollection attributeModelsForThisRT = attributeModelManager.Get(attributeModelContext);

                    attributeModelsPerRT.Add(relationshipTypeId, attributeModelsForThisRT);
                }

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Attribute models loaded.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
                }

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded attribute models.", MDMTraceSource.RelationshipGet);
                }
            }
            else
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute models load would be skipped as RelationshipContext.LoadRelationshipAttributes is set to 'false'", MDMTraceSource.RelationshipGet);
                }
            }

            return attributeModelsPerRT;
        }

        #endregion

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="entityOperationDiagnosticReport"></param>
        /// <returns></returns>
        public Boolean Process(EntityCollection entities, EntityOperationResultCollection entityOperationResults, EntityCacheStatusCollection entityCacheStatusCollection, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions, EntityOperationDiagnosticReport entityOperationDiagnosticReport = null)
        {
            return Process(entities, entityOperationResults, entityCacheStatusCollection, callerContext, entityProcessingOptions, entityOperationDiagnosticReport, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="entityOperationDiagnosticReport"></param>
        /// <param name="relationships"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        public Boolean Process(EntityCollection entities, EntityOperationResultCollection entityOperationResults, EntityCacheStatusCollection entityCacheStatusCollection, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions, EntityOperationDiagnosticReport entityOperationDiagnosticReport = null, Dictionary<Int64, RelationshipCollection> relationships = null, Boolean isRecursive = true)
        {
            Boolean successFlag = false;

            ProcessingMode processingMode = entityProcessingOptions.ProcessingMode;

            #region Diagnostic

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            ExecutionContext executionContext = null;

            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                var callDataContext = new CallDataContext();
                callDataContext.EntityIdList = entities.GetEntityIdList();
                executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityProcessingOptions.ToXml());
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.RelationshipProcess);
                diagnosticActivity.Start(executionContext);
            }

            #endregion Diagnostic

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Starting parameter validation and initial setup for process...");
                }

                #region Parameters Validation

                ValidateEntities(entities, callerContext, "Process");

                #endregion

                #region Initial Setup

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = "RelationshipManager.RelationshipBL.Process";
                }

                if (entityOperationResults == null || entityOperationResults.Count < 1)
                {
                    entityOperationResults = new EntityOperationResultCollection(entities);

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Prepared entity operation results");
                    }
                }

                if (entityCacheStatusCollection == null)
                {
                    entityCacheStatusCollection = new EntityCacheStatusCollection();
                }

                String loginUser = _securityPrincipal.CurrentUserName;

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Parameter validation and initial setup is completed");
                }

                #endregion

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    #region Update in database

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Starting entity relationship process transaction");
                    }

                    RelationshipDA relationshipDataManager = new RelationshipDA();

                    successFlag = relationshipDataManager.Process(entities, relationships, entityOperationResults, entityProcessingOptions.ProcessOnlyRelationships, loginUser, callerContext, command, processingMode, isRecursive);

                    if (!successFlag)
                    {
                        diagnosticActivity.LogError("Entity relationship database call returned with success flag as 'false'");
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Done with entity relationship database process");
                    }

                    #endregion

                    #region Remove cache

                    EntityCacheStatus entityCacheStatus = null;

                    foreach (Entity entity in entities)
                    {
                        entityCacheStatus = entityCacheStatusCollection.GetOrCreateEntityCacheStatus(entity.Id, entity.ContainerId);
                        entityCacheStatus.IsRelationshipCacheDirty = true;
                    }

                    #endregion

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return successFlag;
        }

        #region Process Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityManager"></param>
        /// <param name="allUoms"></param>
        /// <param name="fillOptions"></param>
        /// <param name="callerContext"></param>
        public void FillEntityRelationships(Entity entity, IEntityManager entityManager, UOMCollection allUoms, EntityGetOptions.EntityFillOptions fillOptions, CallerContext callerContext)
        {
            if (entity.Relationships != null && entity.Relationships.Count > 0)
            {
                FillEntityRelationshipDetails(entity.Id, entity.ContainerId, entity.Relationships, entity.EntityContext.Locale, entity.EntityContext.DataLocales, entityManager, allUoms, fillOptions, callerContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="relationshipCreateId"></param>
        /// <param name="stringComparison"></param>
        /// <param name="processingMode"></param>
        public void CompareAndMerge(Entity entity, EntityOperationResult entityOperationResult, CallerContext callerContext, ref Int32 relationshipCreateId, StringComparison stringComparison, ProcessingMode processingMode = ProcessingMode.Async)
        {
            Boolean flushExistingValues = true;
            RelationshipCollection originalRelationships = null;

            //Get AppConfig which specify whether relationship inheritance feature is enabled or not.
            Boolean isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig("RelationshipManager.Relationship.Inheritance.Enabled", true);

            Entity originalEntity = entity.OriginalEntity;
            if (originalEntity != null)
            {
                originalRelationships = originalEntity.Relationships;
            }

            CompareAndMerge(entity, entity.Relationships, originalRelationships, ref relationshipCreateId, flushExistingValues, stringComparison, processingMode, entityOperationResult, callerContext, isRelationshipInheritanceEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updatedRelationships"></param>
        /// <param name="originalRelationships"></param>
        /// <param name="runningRelatonshipCreateId"></param>
        /// <param name="flushExistingValues"></param>
        /// <param name="stringComparison"></param>
        /// <param name="processingMode"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="isRelationshipInheritanceEnabled"></param>
        /// <returns></returns>
        public Boolean CompareAndMerge(Entity entity, RelationshipCollection updatedRelationships, RelationshipCollection originalRelationships, ref Int32 runningRelatonshipCreateId, Boolean flushExistingValues, StringComparison stringComparison, ProcessingMode processingMode, EntityOperationResult entityOperationResult, CallerContext callerContext, Boolean isRelationshipInheritanceEnabled)
        {
            Boolean areEntityRelationshipsChanged = false;
            Boolean isOrigScanNeededForRelSourceFlagChangeVerification = false;

            Boolean isUiProcessCall = callerContext.ProgramName.Equals("__MultiToMultiRelationship.aspx") || callerContext.ProgramName.Equals("AddRelationshipBySearch");

            foreach (Relationship updatedRelationship in updatedRelationships)
            {
                ObjectAction identifiedPerformedAction = ObjectAction.Read;

                Relationship originalRelationship = null;

                #region Identify performed action for updated relationship using entity level action and original relationship object

                if (entity.Id > 0 && entity.Action != ObjectAction.Create && entity.Action != ObjectAction.Delete && originalRelationships != null)
                {
                    #region Identify original relationship

                    foreach (Relationship origRel in originalRelationships)
                    {
                        if ((origRel.FromEntityId == entity.Id || origRel.FromEntityId == updatedRelationship.FromEntityId) && origRel.RelatedEntityId == updatedRelationship.RelatedEntityId && origRel.ContainerId == updatedRelationship.ContainerId &&
                                    origRel.RelationshipTypeId == updatedRelationship.RelationshipTypeId && origRel.Level == updatedRelationship.Level)
                        {
                            originalRelationship = origRel;
                            break;
                        }
                    }

                    #endregion

                    if (originalRelationship != null)
                    {
                        #region Fill values from original relationship to updated relationship

                        updatedRelationship.RowId = updatedRelationship.Id;
                        updatedRelationship.Id = originalRelationship.Id;
                        updatedRelationship.OriginalRelationship = originalRelationship; // set original rel reference

                        //Update Relationship Operation Result
                        RelationshipOperationResult relOperationResult = entityOperationResult.RelationshipOperationResultCollection.GetRelationshipOperationResult(updatedRelationship.RowId);

                        if (relOperationResult != null)
                        {
                            relOperationResult.RelationshipId = updatedRelationship.Id;
                        }

                        #endregion

                        identifiedPerformedAction = ObjectAction.Update;
                    }
                    else
                    {
                        identifiedPerformedAction = ObjectAction.Create;
                    }
                }
                else if (entity.Action == ObjectAction.Create)
                {
                    identifiedPerformedAction = ObjectAction.Create;
                }

                #endregion

                #region Check for delete action passed in updated relationship and respect it

                if (updatedRelationship.Action == ObjectAction.Delete && identifiedPerformedAction != ObjectAction.Read)
                {
                    identifiedPerformedAction = ObjectAction.Delete;
                }

                #endregion

                #region Verify for passed create action updated relationship from MDM UI(Add by search or Add items) and flip the flag accordingly

                if (isUiProcessCall && updatedRelationship.Action == ObjectAction.Create && identifiedPerformedAction != ObjectAction.Create)
                {
                    identifiedPerformedAction = ObjectAction.Create;
                }

                #endregion

                #region Further Merge accordingly to identified performed action

                if (identifiedPerformedAction == ObjectAction.Create)
                {
                    #region Relationship Create

                    SetRelationshipActionAsCreate(new RelationshipCollection() { updatedRelationship }, ref runningRelatonshipCreateId, entityOperationResult, processingMode);

                    if (updatedRelationship.RelationshipCollection != null && updatedRelationship.RelationshipCollection.Count > 0 && processingMode != ProcessingMode.Async)
                    {
                        //Relationship creation is level by level... 
                        //If first level relationships are not yet created.. then second level relationships cannot be processed.
                        //So, ignoring second and next level relationships... second and next level relationships will be processed through Denorm processing
                        SetRelationshipActionAsIgnore(updatedRelationship.RelationshipCollection);
                    }

                    if (!areEntityRelationshipsChanged)
                    {
                        areEntityRelationshipsChanged = true;
                    }

                    if (isRelationshipInheritanceEnabled)
                    {
                        isOrigScanNeededForRelSourceFlagChangeVerification = true;
                    }

                    #endregion
                }
                else if (identifiedPerformedAction == ObjectAction.Delete)
                {
                    #region Relationship Delete

                    if (originalRelationship != null && originalRelationship.RelationshipCollection != null && originalRelationship.RelationshipCollection.Count > 0)
                    {
                        RelationshipCollection clonedChildRelationships = originalRelationship.RelationshipCollection.Clone();

                        //Delete child relationships also when parent is requested for delete..
                        SetRelationshipActionAsDelete(clonedChildRelationships);

                        //Add child relationships which are set as Delete to current relationship
                        updatedRelationship.RelationshipCollection = clonedChildRelationships;
                    }

                    if (!areEntityRelationshipsChanged)
                    {
                        areEntityRelationshipsChanged = true;
                    }

                    #endregion
                }
                else
                {
                    #region Relationship Update / identify no change(read)

                    if (originalRelationship != null)
                    {
                        Boolean hasRelationshipAttributesChanged = false;

                        #region Compare and merge relationship attributes

                        AttributeCollection origAttributes = (AttributeCollection)originalRelationship.GetRelationshipAttributes();
                        AttributeCollection mergedAttributes = new AttributeCollection();

                        if (origAttributes != null && origAttributes.Count > 0)
                        {
                            foreach (Attribute deltaAttribute in updatedRelationship.RelationshipAttributes)
                            {
                                if (deltaAttribute.Action == ObjectAction.Ignore)
                                {
                                    mergedAttributes.Add(deltaAttribute);
                                }
                                else
                                {
                                    IAttribute origAttribute = origAttributes.GetAttribute(deltaAttribute.Id, deltaAttribute.Locale);

                                    if (origAttribute != null && origAttribute.AuditRefId > 0)
                                    {
                                        if (deltaAttribute.IsComplex == false)
                                        {
                                            Boolean isLookup = origAttribute.IsLookup;

                                            //Relationship UI is not filling InvariantVal property for relationship attributes. Please fill them in if they are missing.
                                            ValueCollection values = (ValueCollection)deltaAttribute.GetCurrentValuesInvariant();

                                            //Few data corrections steps:
                                            //1. UI is sending InvariantVal as null so set it up same as AttrVal if it is null
                                            //2. If attribute is not lookup, ValueRefId should be -1 and not 0. UI is sending 0.
                                            //3. UOM should be -1 and not 0
                                            if (values != null)
                                            {
                                                foreach (Value val in values)
                                                {
                                                    if (val.AttrVal != null && val.InvariantVal == null)
                                                        val.InvariantVal = val.AttrVal;

                                                    if (!isLookup)
                                                        val.ValueRefId = -1;

                                                    if (val.UomId < 1)
                                                        val.UomId = -1;
                                                }
                                            }

                                            Attribute mergedAttribute = (Attribute)origAttribute.MergeDelta(deltaAttribute, flushExistingValues, callerContext);

                                            //This is require as core merge delta is setting action to update for exact match collections too..
                                            if (mergedAttribute.IsCollection)
                                            {
                                                FixCollectionMerge((Attribute)origAttribute, mergedAttribute, stringComparison);
                                            }

                                            if (mergedAttribute.GetCurrentValuesInvariant() == null || mergedAttribute.GetCurrentValuesInvariant().Count < 1)
                                            {
                                                mergedAttribute.Action = ObjectAction.Delete;
                                            }

                                            mergedAttributes.Add(mergedAttribute);

                                            if (mergedAttribute.CheckHasChanged())
                                            {
                                                hasRelationshipAttributesChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            mergedAttributes.Add(deltaAttribute);

                                            if (deltaAttribute.CheckHasChanged())
                                            {
                                                hasRelationshipAttributesChanged = true;
                                            }
                                        }
                                    }
                                    else if (deltaAttribute.HasAnyValue())
                                    {
                                        //Attribute value is not available in original relationship attribute..
                                        //Which means attribute is coming for first time, So sending for creation..
                                        //Setting action to Create
                                        deltaAttribute.Action = ObjectAction.Create;
                                        deltaAttribute.GetCurrentValuesInvariant().SetAction(ObjectAction.Create);

                                        mergedAttributes.Add(deltaAttribute);

                                        if (deltaAttribute.CheckHasChanged())
                                        {
                                            hasRelationshipAttributesChanged = true;
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Fix relationship attributes merge for denorm module

                        //During denorm, when the attribute value which is inherited is cleared at parent level,
                        //attribute details for those cleared values will not be available in Updated Relationships.
                        //So, finding out those attribute details and marking them as delete as those values also should be
                        //cleared at this relationship level
                        //TODO:: How to handle complex attribute..?
                        if (callerContext.Module == MDMCenterModules.Denorm)
                        {
                            foreach (Attribute originalAttribute in origAttributes)
                            {
                                if (originalAttribute.SourceFlag == AttributeValueSource.Inherited)
                                {
                                    //Check whether this attribute details are already available in updated relationships
                                    IAttribute deltaAttribute = updatedRelationship.RelationshipAttributes.GetAttribute(originalAttribute.Id, originalAttribute.Locale);

                                    if (deltaAttribute == null)
                                    {
                                        deltaAttribute = originalAttribute.Clone();

                                        //Set action as Delete..
                                        deltaAttribute.Action = ObjectAction.Delete;

                                        ValueCollection inheritedValues = (ValueCollection)deltaAttribute.GetInheritedValuesInvariant();

                                        if (inheritedValues != null && inheritedValues.Count > 0)
                                        {
                                            inheritedValues.SetAction(ObjectAction.Delete);
                                        }

                                        mergedAttributes.Add(deltaAttribute);
                                        hasRelationshipAttributesChanged = true;
                                    }
                                }
                            }
                        }

                        #endregion

                        updatedRelationship.RelationshipAttributes = mergedAttributes;

                        if (updatedRelationship.Status != originalRelationship.Status)
                        {
                            hasRelationshipAttributesChanged = true;
                        }

                        if (hasRelationshipAttributesChanged)
                        {
                            identifiedPerformedAction = ObjectAction.Update;

                            updatedRelationship.Action = identifiedPerformedAction;

                            if (!areEntityRelationshipsChanged)
                            {
                                areEntityRelationshipsChanged = true;
                            }
                        }
                        else
                        {
                            identifiedPerformedAction = ObjectAction.Read;
                            updatedRelationship.Action = identifiedPerformedAction;
                        }

                        #region Compare and Merge for child relationships

                        if (updatedRelationship.RelationshipCollection != null && updatedRelationship.RelationshipCollection.Count > 0)
                        {
                            Boolean areChildRelationshipsChanged = CompareAndMerge(entity, updatedRelationship.RelationshipCollection, (originalRelationship != null) ? originalRelationship.RelationshipCollection : null, ref runningRelatonshipCreateId, flushExistingValues, stringComparison, processingMode, entityOperationResult, callerContext, isRelationshipInheritanceEnabled);

                            if (areChildRelationshipsChanged && updatedRelationship.Action == ObjectAction.Read)
                            {
                                updatedRelationship.Action = ObjectAction.Update;
                            }
                        }

                        #endregion
                    }

                    #endregion
                }

                #endregion
            }

            #region Find deleted inherited relationship while running in async mode and send them for delete

            if (isOrigScanNeededForRelSourceFlagChangeVerification || processingMode == ProcessingMode.Async)
            {
                if (originalRelationships != null && originalRelationships.Count > 0)
                {
                    foreach (Relationship origRel in originalRelationships)
                    {
                        //Full Denorm happens only while processing in an ASync way.. Hence setting deletion as True by default
                        var deleteOldRelationship = processingMode == ProcessingMode.Async;

                        //Check whether the relationship already being exists in updated relationships...
                        foreach (Relationship updatedRel in updatedRelationships)
                        {
                            if (updatedRel.RelatedEntityId == origRel.RelatedEntityId && updatedRel.ContainerId == origRel.ContainerId && updatedRel.RelationshipTypeId == origRel.RelationshipTypeId)
                            {
                                if (updatedRel.FromEntityId == origRel.FromEntityId || entity.Id == origRel.FromEntityId)
                                {
                                    //Relationship already exists in updated relationship...
                                    //So, ignore this relationship... We cannot delete this relationship
                                    deleteOldRelationship = false;
                                    break;
                                }
                                else if (updatedRel.SourceFlag == AttributeValueSource.Overridden && origRel.SourceFlag == AttributeValueSource.Inherited)
                                {
                                    //Same entity has been related as overridden... Delete inherited relationship...
                                    deleteOldRelationship = true;
                                    break;
                                }
                            }
                        }

                        if (deleteOldRelationship)
                        {
                            Relationship origRelationshipTobeDeleted = origRel.Clone();
                            origRelationshipTobeDeleted.Action = ObjectAction.Delete;

                            updatedRelationships.Add(origRelationshipTobeDeleted);

                            if (!areEntityRelationshipsChanged)
                            {
                                areEntityRelationshipsChanged = true;
                            }
                        }
                    }
                }
            }

            #endregion

            return areEntityRelationshipsChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="callerContext"></param>
        public void PopulateDefaultAttributeValues(EntityCollection entities, CallerContext callerContext)
        {
            AttributeModelBL attributeModelManager = new AttributeModelBL();
            Dictionary<String, AttributeModelCollection> attributeModelsDictionary = new Dictionary<String, AttributeModelCollection>();

            foreach (Entity entity in entities)
            {
                if (entity.Relationships != null && entity.Relationships.Count > 0)
                {
                    PopulateDefaultAttributeValues(entity.Relationships, ref attributeModelsDictionary, attributeModelManager, entity.Locale, entity.EntityContext.DataLocales, callerContext);
                }
            }
        }

        #endregion

        #endregion

        #region WhereUsed Methods

        /// <summary>
        /// Get Where Used Relationships for given entity id and relationshipType
        /// </summary>
        /// <param name="entityId">Indicates Entity Id</param>
        /// <param name="relationshipTypeId">Indicates Relationship Type Id. Set to '0' if we want where used relationships across relationship types</param>
        /// <param name="attributeIds">Indicates Attribute Ids which needs to be loaded with relatedEntity</param>
        /// <param name="dataLocale">Indicates requested locale</param>
        /// <param name="iEntityManager">Indicates IEntityManager</param>
        /// <param name="context">Indicates in which context caller is calling the method</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsed(Int64 entityId, Int32 relationshipTypeId, Collection<Int32> attributeIds, LocaleEnum dataLocale, IEntityManager iEntityManager, CallerContext context)
        {
            return GetWhereUsed(new Collection<Int64>() { entityId }, new Collection<Int32>() { relationshipTypeId }, attributeIds, dataLocale, iEntityManager, context);
        }

        /// <summary>
        /// Get Where Used Relationships for given entity ids and relationshipType
        /// </summary>
        /// <param name="entityIds">Indicates Entity Id</param>
        /// <param name="relationshipTypeId">Indicates Relationship Type Id. Set to '0' if we want where used relationships across relationship types</param>
        /// <param name="attributeIds">Indicates attributes id that needs to be loaded</param>
        /// <param name="dataLocale">Indicates requested locale</param>
        /// <param name="iEntityManager">Indicates IEntityManager</param>
        /// <param name="context">Indicates in which context caller is calling the method</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsed(Collection<Int64> entityIds, Int32 relationshipTypeId, Collection<Int32> attributeIds, LocaleEnum dataLocale, IEntityManager iEntityManager, CallerContext context)
        {
            return GetWhereUsed(entityIds, new Collection<Int32>() { relationshipTypeId }, attributeIds, dataLocale, iEntityManager, context);
        }

        /// <summary>
        /// Get Where Used Relationships for given entity id and relationshpTypes
        /// </summary>
        /// <param name="entityId">Indicates Entity Id</param>
        /// <param name="relationshipTypeIds">Indicates Relationship Type Id. Set to '0' if we want where used relationships across relationship types</param>
        /// <param name="attributeIds">Indicates attributes id that needs to be loaded</param>
        /// <param name="dataLocale">Indicates requested locale</param>
        /// <param name="iEntityManager">Indicates IEntityManager</param>
        /// <param name="context">Indicates in which context caller is calling the method</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsed(Int64 entityId, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, LocaleEnum dataLocale, IEntityManager iEntityManager, CallerContext context)
        {
            return GetWhereUsed(new Collection<Int64>() { entityId }, relationshipTypeIds, attributeIds, dataLocale, iEntityManager, context);
        }

        /// <summary>
        /// Get Where Used Relationships for given entity ids and relationshipTypes
        /// </summary>
        /// <param name="entityIds">Indicates Entity Id</param>
        /// <param name="relationshipTypeIds">Indicates Relationship Type Id. Set to '0' if we want where used relationships across relationship types</param>
        /// <param name="attributeIds">Indicates attributes id that needs to be loaded</param>
        /// <param name="currentDataLocale">Indicates requested locale</param>
        /// <param name="iEntityManager">Indicates IEntityManager</param>
        /// <param name="callerContext">Indicates in which context caller is calling the method</param>
        /// <param name="loadCircularRelationship">Indicates whether to load circular relationship or not. Example : Kit is related to SKU and vice versa</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsed(Collection<Int64> entityIds, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, LocaleEnum currentDataLocale, IEntityManager iEntityManager, CallerContext callerContext, Boolean loadCircularRelationship = true)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            RelationshipCollection relationshipCollection = new RelationshipCollection();

            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipBL.GetWhereUsedRelationships", MDMTraceSource.Relationship, false);
            }

            try
            {
                #region Parameter Validation

                if (entityIds == null || entityIds.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111785", false, callerContext);
                    throw new MDMOperationException("111785", _localeMessage.Message, "RelationshipManager.RelationshipBL.GetWhereUsedRelationships", String.Empty, "Get");//Entity Ids are not available
                }

                #endregion

                #region Initial Setup

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                if (currentDataLocale == LocaleEnum.UnKnown)
                {
                    currentDataLocale = systemDataLocale;
                }

                #endregion

                #region Load From DataBase

                relationshipCollection = new RelationshipDA().GetWhereUsed(entityIds, relationshipTypeIds, currentDataLocale, loadCircularRelationship, command);

                #endregion

                #region Load RelatedEntites

                if (relationshipCollection != null && relationshipCollection.Count > 0)
                {
                    Collection<Int64> relatedEntityIdList = relationshipCollection.GetRelatedEntityIdList();

                    if (relatedEntityIdList != null && relatedEntityIdList.Count > 0)
                    {
                        EntityContext entityContext = new EntityContext();
                        entityContext.LoadEntityProperties = true;
                        entityContext.DataLocales.Add(currentDataLocale);

                        if (currentDataLocale != systemDataLocale)
                        {
                            entityContext.DataLocales.Add(systemDataLocale);
                        }

                        if (attributeIds != null && attributeIds.Count > 0)
                        {
                            entityContext.LoadAttributes = true;
                            entityContext.LoadRelationships = true;
                            entityContext.RelationshipContext.RelationshipTypeIdList = relationshipCollection.GetRelationshipTypeIds();
                            entityContext.AttributeIdList = attributeIds;
                        }

                        //Disable PublishEvents and ApplyAVS as such its internal get.
                        EntityCollection relatedEntities = iEntityManager.Get(relatedEntityIdList, entityContext, false, callerContext.Application, callerContext.Module, false, false);

                        if (relatedEntities != null && relatedEntities.Count > 0)
                        {
                            PopulateRelatedEntities(relationshipCollection, relatedEntities);
                            PopulateRelationshipAttributes(relationshipCollection, attributeIds);
                        }
                    }
                }

                #endregion

                #region Load Source Entities

                if (relationshipCollection != null && relationshipCollection.Count > 0)
                {
                    Collection<Int64> sourceEntityIdList = relationshipCollection.GetSourceEntityIdList();

                    if (sourceEntityIdList != null && sourceEntityIdList.Count > 0)
                    {
                        EntityContext entityContext = new EntityContext();
                        entityContext.LoadEntityProperties = true;

                        //Disable PublishEvents and ApplyAVS as such its internal get.
                        EntityCollection sourceEntities = iEntityManager.Get(sourceEntityIdList, entityContext, false, callerContext.Application, callerContext.Module, false, false);

                        if (sourceEntities != null && sourceEntities.Count > 0)
                        {
                            PopulateSourceEntityDetails(relationshipCollection, sourceEntities);
                        }
                    }
                }

                #endregion
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipBL.GetWhereUsedRelationships", MDMTraceSource.Relationship);
                }
            }

            return relationshipCollection;
        }

        #endregion

        #region Get Relationship Model

        /// <summary>
        /// Gets relationship model
        /// </summary>
        /// <param name="relationshipTypeId">Relationship type Id for which model needs to be fetched</param>
        /// <param name="containerId">Container Id under which attributes needs to be fetched</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns model of Relationship object</returns>
        public Relationship GetRelationshipModel(Int32 relationshipTypeId, Int32 containerId, Collection<LocaleEnum> locales, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            Relationship relationship = new Relationship();

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipBL.GetRelationshipModel", MDMTraceSource.RelationshipGet, false);
                }

                #region Initial Setup

                //Populate relationship type
                relationship.RelationshipTypeId = relationshipTypeId;
                relationship.RelationshipTypeName = GetRelationshipTypeNameById(relationshipTypeId, callerContext);

                //Get relationship attribute models
                RelationshipContext relationshipContext = new RelationshipContext();
                relationshipContext.RelationshipTypeIdList.Add(relationshipTypeId);
                relationshipContext.ContainerId = containerId;
                relationshipContext.DataLocales = locales;

                #endregion

                Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT = GetRelationshipAttributeModelsByContext(relationshipContext);

                if (attributeModelsPerRT != null && attributeModelsPerRT.Count > 0)
                {
                    AttributeModelCollection attributeModels = attributeModelsPerRT[relationshipTypeId];

                    relationship.RelationshipAttributes = new AttributeCollection(attributeModels);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipBL.GetRelationshipModel", MDMTraceSource.RelationshipGet);
                }
            }

            return relationship;
        }

        #endregion

        #region Get Cardinality Methods

        /// <summary>
        /// Get Relationship Cardinality for requested entityType and RelationshipType
        /// </summary>
        /// <param name="relationshipTypeId">Indicates relationshipType for which cardinality is requested</param>
        /// <param name="containerId">Indicates container Id</param>
        /// <param name="fromEntityTypeId">Indicates EntityTypeId for which cardinality is requested</param>
        /// <param name="callerContext">Indicates Caller context</param>
        /// <returns>Relationship Cardinalities</returns>
        public RelationshipCardinalityCollection GetRelationshsipCardinalities(Int32 relationshipTypeId, Int32 containerId, Int32 fromEntityTypeId, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            RelationshipCardinalityCollection relationshipCardinalities = new RelationshipCardinalityCollection();
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipBL.GetRelationshsipCardinalities", MDMTraceSource.Relationship, false);
            }

            try
            {
                #region Parameter Validation

                if (containerId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111821", false, callerContext);
                    throw new MDMOperationException("111821", _localeMessage.Message, "RelationshipManager.RelationshipBL.GetRelationshsipCardinalities", String.Empty, "Get");//ContainerId must be greater than 0.
                }

                if (fromEntityTypeId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111820", false, callerContext);
                    throw new MDMOperationException("111820", _localeMessage.Message, "RelationshipManager.RelationshipBL.GetRelationshsipCardinalities", String.Empty, "Get");//EntityTypeId must be greater than 0.
                }

                if (relationshipTypeId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "112137", false, callerContext);
                    throw new MDMOperationException("112137", _localeMessage.Message, "RelationshipManager.RelationshipBL.GetRelationshsipCardinalities", String.Empty, "Get");//RelationshipType cannot be null
                }

                #endregion

                #region IntialSetup

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                #endregion

                #region Load Data from Cache

                MappingBufferManager bufferManager = new MappingBufferManager();
                relationshipCardinalities = bufferManager.FindRelationshipCardinalities(relationshipTypeId, containerId, fromEntityTypeId);

                #endregion Load Data from Cache

                #region Load Data from DB

                if (relationshipCardinalities == null)
                {
                    relationshipCardinalities = new RelationshipDA().GetRelationshipCardinalities(relationshipTypeId, containerId, fromEntityTypeId, command);

                    if (relationshipCardinalities != null)
                    {
                        bufferManager.UpdateRelationshipCardinalities(relationshipCardinalities, relationshipTypeId, containerId, fromEntityTypeId);
                    }
                }
                #endregion Load Data from DB
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipBL.GetRelationshsipCardinalities", MDMTraceSource.Relationship);
                }
            }

            return relationshipCardinalities;
        }

        #endregion

        #endregion

        #region Private Methods

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <returns></returns>
        private Boolean DoesNonLocalizableAttributeExist(AttributeModelCollection attributeModels)
        {
            if (attributeModels != null && attributeModels.Count > 0)
            {
                foreach (AttributeModel attributeModel in attributeModels)
                {
                    if (!attributeModel.IsLocalizable)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origAttribute"></param>
        /// <param name="mergedAttribute"></param>
        /// <param name="stringComparison"></param>
        private void FixCollectionMerge(Attribute origAttribute, Attribute mergedAttribute, StringComparison stringComparison)
        {
            ValueCollection origValues = (ValueCollection)origAttribute.GetCurrentValuesInvariant();
            ValueCollection mergedValues = (ValueCollection)mergedAttribute.GetCurrentValuesInvariant();

            Boolean isAllValuesExactMatched = false;

            if (mergedAttribute.SourceFlag != origAttribute.SourceFlag)
                return;

            if (mergedAttribute.Action == ObjectAction.Update && origValues != null && mergedValues != null)
            {
                isAllValuesExactMatched = true;

                foreach (Value mergedVal in mergedValues)
                {
                    var matchedVals = origValues.Where(v => v.ValueEquals(mergedVal, stringComparison));

                    //If exact value matches..
                    if (!(matchedVals != null && matchedVals.Count() == 1 && mergedVal.Action != ObjectAction.Delete))
                    {
                        isAllValuesExactMatched = false;
                        break;
                    }
                }
            }

            if (isAllValuesExactMatched)
            {
                mergedValues.SetAction(ObjectAction.Read);
                mergedAttribute.Action = ObjectAction.Read;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromEntityId"></param>
        /// <param name="containerId"></param>
        /// <param name="relationships"></param>
        /// <param name="entityContextLocale"></param>
        /// <param name="entityContextDataLocales"></param>
        /// <param name="entityManager"></param>
        /// <param name="allUoms"></param>
        /// <param name="fillOptions"></param>
        /// <param name="callerContext"></param>
        private void FillEntityRelationshipDetails(Int64 fromEntityId, Int32 containerId, RelationshipCollection relationships, LocaleEnum entityContextLocale, Collection<LocaleEnum> entityContextDataLocales, IEntityManager entityManager, UOMCollection allUoms, EntityGetOptions.EntityFillOptions fillOptions, CallerContext callerContext)
        {
            Collection<Int64> relatedEntityIds = new Collection<Int64>();
            EntityCollection relatedEntites = null;

            foreach (Relationship relationship in relationships)
            {
                if (relationship.FromEntityId <= 0)
                    relationship.FromEntityId = fromEntityId < 0 ? 0 : fromEntityId;

                if (relationship.ContainerId <= 0)
                    relationship.ContainerId = containerId;

                if (fillOptions.FillRelationshipProperties)
                {
                    if (String.IsNullOrWhiteSpace(relationship.RelationshipTypeName) && relationship.RelationshipTypeId > 0)
                    {
                        relationship.RelationshipTypeName = GetRelationshipTypeNameById(relationship.RelationshipTypeId, callerContext);
                    }

                    if (relationship.RelatedEntityId > 0
                        && (String.IsNullOrWhiteSpace(relationship.ToExternalId)
                            || String.IsNullOrWhiteSpace(relationship.ToLongName)
                            || String.IsNullOrWhiteSpace(relationship.ToContainerName)
                            || String.IsNullOrWhiteSpace(relationship.ToEntityTypeName)
                            || String.IsNullOrWhiteSpace(relationship.ToCategoryPath)
                            )
                        )
                    {
                        if (relationship.OriginalRelationship != null && relationship.OriginalRelationship.RelatedEntityId.Equals(relationship.RelatedEntityId))
                        {
                            relationship.ToExternalId = relationship.OriginalRelationship.ToExternalId;
                            relationship.ToLongName = relationship.OriginalRelationship.ToLongName;
                            relationship.ToContainerId = relationship.OriginalRelationship.ToContainerId;
                            relationship.ToContainerName = relationship.OriginalRelationship.ToContainerName;
                            relationship.ToEntityTypeName = relationship.OriginalRelationship.ToEntityTypeName;
                            relationship.ToCategoryPath = relationship.OriginalRelationship.ToCategoryPath;
                            relationship.ToEntityTypeId = relationship.OriginalRelationship.ToEntityTypeId;
                        }
                        else if (!relatedEntityIds.Contains(relationship.RelatedEntityId))
                        {
                            relatedEntityIds.Add(relationship.RelatedEntityId);
                        }
                    }
                }
            }

            if (relatedEntityIds.Count > 0)
            {
                EntityContext entityContext = new EntityContext
                    {
                        DataLocales = entityContextDataLocales,
                        Locale = entityContextLocale,
                        LoadEntityProperties = true,
                        LoadAttributes = false
                    };

                relatedEntites = entityManager.Get(relatedEntityIds, entityContext, false, callerContext.Application, callerContext.Module, false, false);
            }

            foreach (Relationship relationship in relationships)
            {
                if (relatedEntites != null && relatedEntites.Count > 0)
                {
                    Entity relatedEntity = relatedEntites.GetEntity(relationship.RelatedEntityId) as Entity;

                    if (relatedEntity != null)
                    {
                        relationship.ToExternalId = relatedEntity.Name;
                        relationship.ToLongName = relatedEntity.LongName;
                        relationship.ToEntityTypeId = relatedEntity.EntityTypeId;
                        relationship.ToContainerId = relatedEntity.ContainerId;
                        relationship.ToContainerName = relatedEntity.ContainerName;
                        relationship.ToEntityTypeName = relatedEntity.EntityTypeName;
                        relationship.ToCategoryPath = relatedEntity.CategoryPath;
                    }
                }

                if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                {
                    FillEntityRelationshipAttributes(relationship, allUoms, fillOptions);
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    FillEntityRelationshipDetails(relationship.RelatedEntityId, containerId, relationship.RelationshipCollection, entityContextLocale, entityContextDataLocales, entityManager, allUoms, fillOptions, callerContext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="allUoms"></param>
        /// <param name="fillOptions"></param>
        private void FillEntityRelationshipAttributes(Relationship relationship, UOMCollection allUoms, EntityGetOptions.EntityFillOptions fillOptions)
        {
            // Note: Lookup display format population is part of EntityFillHelper. This method would not take care of lookup display value format

            var relationshipAttributes = relationship.GetRelationshipAttributes();

            if (relationshipAttributes == null || relationshipAttributes.Count < 1)
                return;

            foreach (Attribute attribute in relationship.RelationshipAttributes)
            {
                //Relationship UI is not filling InvariantVal property for relationship attributes. Please fill them in if they are missing.
                var values = (ValueCollection)attribute.GetCurrentValuesInvariant();

                if (values != null)
                {
                    foreach (Value value in values)
                    {
                        if (value.AttrVal != null && value.InvariantVal == null)
                        {
                            value.InvariantVal = value.AttrVal;
                        }

                        if (fillOptions.FillUOMValues)
                        {
                            if (allUoms != null && allUoms.Count > 0)
                            {
                                if (value.UomId < 1 && !String.IsNullOrWhiteSpace(value.Uom))
                                {
                                    var uomObj = allUoms.FirstOrDefault(uom => String.Compare(uom.Key, value.Uom, StringComparison.InvariantCultureIgnoreCase) == 0);

                                    if (uomObj != null)
                                    {
                                        value.UomId = uomObj.Id;
                                    }
                                }
                                else if (value.UomId > 0 && String.IsNullOrWhiteSpace(value.Uom))
                                {
                                    var uomObj = allUoms.FirstOrDefault(uom => uom.Id == value.UomId);

                                    if (uomObj != null)
                                    {
                                        value.Uom = uomObj.Key;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="attributeModelsDictionary"></param>
        /// <param name="attributeModelManager"></param>
        /// <param name="entityContextLocale"></param>
        /// <param name="entityContextDataLocales"></param>
        /// <param name="callerContext"></param>
        private void PopulateDefaultAttributeValues(RelationshipCollection relationships, ref Dictionary<String, AttributeModelCollection> attributeModelsDictionary, AttributeModelBL attributeModelManager, LocaleEnum entityContextLocale, Collection<LocaleEnum> entityContextDataLocales, CallerContext callerContext)
        {
            foreach (Relationship relationship in relationships)
            {
                if (relationship.Action == ObjectAction.Create || relationship.Action == ObjectAction.Update)
                {
                    AttributeModelCollection relationshipAttributeModels = null; //new AttributeModelCollection();

                    String attributeModelsUniqueKey = String.Format("CON{0}_RT{1}_L{2}", relationship.ContainerId, relationship.RelationshipTypeId, (Int32)entityContextLocale);

                    if (attributeModelsDictionary.ContainsKey(attributeModelsUniqueKey))
                    {
                        attributeModelsDictionary.TryGetValue(attributeModelsUniqueKey, out relationshipAttributeModels);
                    }
                    else
                    {
                        var attributeModelContext = new AttributeModelContext();
                        attributeModelContext.AttributeModelType = AttributeModelType.Relationship;
                        attributeModelContext.RelationshipTypeId = relationship.RelationshipTypeId;
                        attributeModelContext.ContainerId = relationship.ContainerId;
                        attributeModelContext.Locales = entityContextDataLocales;

                        relationshipAttributeModels = attributeModelManager.Get(attributeModelContext);

                        attributeModelsDictionary.Add(attributeModelsUniqueKey, relationshipAttributeModels);
                    }

                    if (relationshipAttributeModels == null || relationshipAttributeModels.Count < 1)
                        continue; //no models found for this RT...continue with next relationship..

                    foreach (AttributeModel attributeModel in relationshipAttributeModels)
                    {
                        LocaleEnum attributeLocale = (attributeModel.IsLocalizable) ? attributeModel.Locale : GlobalizationHelper.GetSystemDataLocale();

                        if (relationship.Action == ObjectAction.Create && !String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
                        {
                            Attribute attribute = null;

                            Attribute existingAttr = (Attribute)relationship.RelationshipAttributes.GetAttribute(attributeModel.Id, attributeLocale);

                            if (existingAttr != null && existingAttr.Action != ObjectAction.Ignore)
                            {
                                continue;
                            }

                            if (existingAttr != null)
                            {
                                attribute = existingAttr;
                            }
                            else
                            {
                                attribute = new Attribute(attributeModel, attributeLocale);
                                relationship.RelationshipAttributes.Add(attribute);
                            }

                            IValue value = MDMObjectFactory.GetIValue();
                            value.Action = ObjectAction.Create;

                            value.AttrVal = EvaluateDefaultValue(attributeModel.DefaultValue, attributeModel.AttributeDataTypeId);

                            if (!String.IsNullOrEmpty(attributeModel.DefaultUOM))
                            {
                                value.Uom = attributeModel.DefaultUOM;
                            }

                            if (attribute.IsLookup)
                            {
                                if (value.AttrVal != null)
                                {
                                    value.ValueRefId = ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), 0);
                                }
                            }

                            if (attribute.IsCollection)
                            {
                                value.Sequence = 0;
                            }

                            attribute.Action = ObjectAction.Create;
                            attribute.OverriddenValues.Add(value);
                        }
                        else if (relationship.Action == ObjectAction.Update && !String.IsNullOrWhiteSpace(attributeModel.DefaultUOM))
                        {
                            Attribute attribute = (Attribute)relationship.RelationshipAttributes.GetAttribute(attributeModel.Id, attributeLocale);

                            if (attribute == null || (attribute.Action != ObjectAction.Create && attribute.Action != ObjectAction.Update) ||
                                attribute.SourceFlag != AttributeValueSource.Overridden || attribute.IsComplex || attribute.IsLookup)
                            {
                                continue;
                            }

                            if (attribute.IsCollection)
                            {
                                IValueCollection attributeValues = attribute.GetOverriddenValuesInvariant();
                                if (attributeValues != null && attributeValues.Count > 0)
                                {
                                    foreach (Value value in attributeValues)
                                    {
                                        if (value != null && String.IsNullOrEmpty(value.Uom) && value.AttrVal != null && !String.IsNullOrEmpty(value.AttrVal.ToString()))
                                        {
                                            value.Uom = attributeModel.DefaultUOM;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Value value = (Value)attribute.GetCurrentValueInstance();
                                if (value != null && String.IsNullOrEmpty(value.Uom) && value.AttrVal != null && !String.IsNullOrEmpty(value.AttrVal.ToString()))
                                {
                                    value.Uom = attributeModel.DefaultUOM;
                                }
                            }
                        }
                    }
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    PopulateDefaultAttributeValues(relationship.RelationshipCollection, ref attributeModelsDictionary, attributeModelManager, entityContextLocale, entityContextDataLocales, callerContext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="dataTypeId"></param>
        /// <returns></returns>
        private String EvaluateDefaultValue(String defaultValue, Int32 dataTypeId)
        {
            //Assign initial value of defaultValue param as return value. 
            //So if coming value is not a variable then whatever is coming in as parameter, it'll be given back as return value.
            String returnValue = defaultValue;

            if (!String.IsNullOrEmpty(defaultValue))
            {
                //DefaultValue = system date
                if (defaultValue.Trim().ToLowerInvariant().Equals("[system date]"))
                {
                    // IF datatype is Date then convert it to the Utc DateTime short date String 
                    if (dataTypeId == 1)
                    {
                        returnValue = DateTime.UtcNow.ToShortDateString();
                    }
                    else if (dataTypeId == 14) // If datatype is DateTime then convert it to the complete Utc DateTime String 
                    {
                        returnValue = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    }
                }
                //DefaultValue = current user's login
                else if (defaultValue.Trim().ToLowerInvariant().Equals("[user login]"))
                {
                    returnValue = _securityPrincipal.CurrentUserName;
                }
                //DefaultValue = current user's email
                else if (defaultValue.Trim().ToLowerInvariant().Equals("[user email]"))
                {
                    returnValue = GetUserDetail("Email");
                }
                //DefaultValue = current user's first name
                else if (defaultValue.Trim().ToLowerInvariant().Equals("[user firstname]"))
                {
                    returnValue = GetUserDetail("FirstName");
                }
                //DefaultValue = current user's last name
                else if (defaultValue.Trim().ToLowerInvariant().Equals("[user lastname]"))
                {
                    returnValue = GetUserDetail("LastName");
                }
                //DefaultValue = current user's fullname (FirstName + " " + LastName)
                else if (defaultValue.Trim().ToLowerInvariant().Equals("[user fullname]"))
                {
                    returnValue = GetUserDetail("FullName");
                }
            }
            else
            {
                returnValue = String.Empty;
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        private void SetRelationshipActionAsIgnore(RelationshipCollection relationships)
        {
            foreach (Relationship relationship in relationships)
            {
                relationship.Action = ObjectAction.Ignore;

                if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                {
                    foreach (Attribute relAttr in relationship.RelationshipAttributes)
                    {
                        relAttr.Action = ObjectAction.Ignore;
                    }
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    SetRelationshipActionAsIgnore(relationship.RelationshipCollection);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        private void SetRelationshipActionAsDelete(RelationshipCollection relationships)
        {
            foreach (Relationship relationship in relationships)
            {
                relationship.Action = ObjectAction.Delete;

                if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                {
                    foreach (Attribute relAttr in relationship.RelationshipAttributes)
                    {
                        relAttr.Action = ObjectAction.Delete;
                    }
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    SetRelationshipActionAsDelete(relationship.RelationshipCollection);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailName"></param>
        /// <returns></returns>
        private String GetUserDetail(String detailName)
        {
            String returnValue = String.Empty;

            SecurityUserBL userBL = new SecurityUserBL();
            SecurityUser user = userBL.GetUser(_securityPrincipal.CurrentUserName);

            if (user != null)
            {
                if (detailName.ToLowerInvariant().Equals("email"))
                {
                    returnValue = user.Smtp;
                }
                else if (detailName.ToLowerInvariant().Equals("firstname"))
                {
                    returnValue = user.FirstName;
                }
                else if (detailName.ToLowerInvariant().Equals("lastname"))
                {
                    returnValue = user.LastName;
                }
                else if (detailName.ToLowerInvariant().Equals("fullname"))
                {
                    //if "detailName" == fullname then concatenate FirstName and LastName attributes of a user.
                    returnValue = String.Concat(user.FirstName, " ", user.LastName);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private RelationshipTypeCollection GetAllRelationshipTypes(CallerContext callerContext)
        {
            RelationshipTypeBL relationshipTypeManager = new RelationshipTypeBL();
            return relationshipTypeManager.GetAll(callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private string GetRelationshipTypeNameById(Int32 relationshipTypeId, CallerContext callerContext)
        {
            RelationshipType relType = null;
            String relationshipTypeName = String.Empty;

            RelationshipTypeCollection allRelationships = GetAllRelationshipTypes(callerContext);

            if (allRelationships == null || allRelationships.Count < 1)
            {
                throw new Exception("Failed to get all relationship types");
            }

            try
            {
                relType = allRelationships.SingleOrDefault(r => r.Id == relationshipTypeId);
            }
            catch (InvalidOperationException)
            {
                throw new Exception("More than one relationship type found for relationship type Id = " + relationshipTypeId);
            }

            if (relType != null)
            {
                relationshipTypeName = relType.LongName;
            }
            else
            {
                throw new Exception(String.Concat("No relationship type with Id = ", relationshipTypeId, " found."));
            }

            return relationshipTypeName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="runningRelatonshipCreateId"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="processingMode"></param>
        private void SetRelationshipActionAsCreate(RelationshipCollection relationships, ref Int32 runningRelatonshipCreateId, EntityOperationResult entityOperationResult, ProcessingMode processingMode)
        {
            foreach (Relationship relationship in relationships)
            {
                relationship.RowId = relationship.Id;

                relationship.Id = runningRelatonshipCreateId--;
                relationship.Action = ObjectAction.Create;

                //Update Relationship Operation Result
                RelationshipOperationResult relOperationResult = entityOperationResult.RelationshipOperationResultCollection.GetRelationshipOperationResult(relationship.RowId);

                if (relOperationResult != null)
                {
                    relOperationResult.RelationshipId = relationship.Id;
                }

                if (relationship.RelationshipAttributes != null)
                {
                    Int32 runningRelationshipAttrValId = -1;

                    foreach (Attribute attr in relationship.RelationshipAttributes)
                    {
                        if (attr.Action != ObjectAction.Read && attr.Action != ObjectAction.Ignore && attr.Action != ObjectAction.Delete)
                        {
                            attr.Action = ObjectAction.Create;
                            var currentValues = (ValueCollection)attr.GetCurrentValuesInvariant();

                            if (currentValues != null && currentValues.Count > 0)
                            {
                                foreach (Value currentValue in currentValues)
                                {
                                    currentValue.Id = runningRelationshipAttrValId--;
                                    currentValue.Action = ObjectAction.Create;
                                }
                            }
                        }
                    }
                }

                if (processingMode == ProcessingMode.Async && relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    SetRelationshipActionAsCreate(relationship.RelationshipCollection, ref runningRelatonshipCreateId, entityOperationResult, processingMode);
                }
            }
        }

        #endregion

        #region Misc Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitalizeMembers()
        {
            _localeMessageBL = new LocaleMessageBL();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
        }

        #endregion

        #region Get Relationships Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="callerContext"></param>
        /// <param name="loadLatest"></param>
        /// <param name="iEntityManager"></param>
        /// <param name="updateCache"></param>
        /// <returns></returns>
        private Boolean GetRelationships(EntityCollection entities, RelationshipContext relationshipContext, EntityCacheStatusCollection entityCacheStatusCollection, CallerContext callerContext, Boolean loadLatest, IEntityManager iEntityManager, Boolean updateCache)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

            #region Step : Parameter Validation

            ValidateEntities(entities, callerContext, "Get");
            ValidateRelationshipContext(relationshipContext, callerContext, "Get");

            #endregion Parameter Validation

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting relationship get logic with following request parameters...", MDMTraceSource.RelationshipGet);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Entity id list:{0}, relationshipContext:{1}, loadLatest:{2}, application:{3} and module:{4}", ValueTypeHelper.JoinCollection(entities.GetEntityIdList(), ","), relationshipContext.ToXml(), loadLatest, callerContext.Application.ToString(), callerContext.Module.ToString()), MDMTraceSource.RelationshipGet);
            }

            #region Step : Initial Setup

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting initial setup for get...", MDMTraceSource.RelationshipGet);
            }

            Boolean result = true;
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            EntityCollection entitiesToBeLoadedFromDB = new EntityCollection();

            Int16 maxLevelToReturn = AppConfigurationHelper.GetAppConfig<Int16>("Relationship.RelationshipLevel", 3);

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AppConfig key 'Relationship.RelationshipLevel' is set to: " + maxLevelToReturn, MDMTraceSource.RelationshipGet);
            }

            //Get AppConfig which specify whether relationship inheritance feature is enabled or not.
            Boolean isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig("RelationshipManager.Relationship.Inheritance.Enabled", true);

            if (relationshipContext.DataLocales == null || relationshipContext.DataLocales.Count < 1)
            {
                relationshipContext.DataLocales = new Collection<LocaleEnum>() { relationshipContext.Locale };
            }

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for initial setup.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with initial setup for get.", MDMTraceSource.RelationshipGet);
            }

            //NOTE : KEY -> EntityId  Value -> Relationship collection.
            Dictionary<Int64, RelationshipCollection> relationshipsDictionary = null;

            //NOTE : KEY -> EN.RelationshipId.Locale.ER Value -> Relationship attributes
            Dictionary<String, AttributeCollection> relationshipAttributesDictionary = null;

            #endregion

            #region Step : Get Relationship Types

            PopulateRelationshipTypeIdList(relationshipContext, callerContext);

            #endregion

            #region Step : Fetch Attribute models

            Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT = GetRelationshipAttributeModelsByContext(relationshipContext);

            #endregion

            #region Step : Fetch Locale list for attribute models

            Collection<LocaleEnum> datalLocaleList = relationshipContext.DataLocales;

            if (!datalLocaleList.Contains(_systemDataLocale))
            {
                foreach (AttributeModelCollection attributeModels in attributeModelsPerRT.Values)
                {
                    if (DoesNonLocalizableAttributeExist(attributeModels))
                    {
                        datalLocaleList.Add(_systemDataLocale);
                        break;
                    }
                }
            }

            #endregion

            #region Step : Calculate Return Related Entity Details

            CalculateReturnRelatedEntityDetailsFlag(relationshipContext, attributeModelsPerRT);

            #endregion

            #region Step : Try to fetch relationships from cache

            GetRelationshipsFromCache(entities, relationshipContext, entityCacheStatusCollection, loadLatest, ref entitiesToBeLoadedFromDB);

            #endregion

            #region Step : Load relationships from the DB

            if (entitiesToBeLoadedFromDB.Count > 0)
            {
                #region Get relationship and attributes from database

                RelationshipDA relationshipDA = new RelationshipDA();

                result = relationshipDA.GetRelationships(entitiesToBeLoadedFromDB, relationshipContext, attributeModelsPerRT, maxLevelToReturn, callerContext, command, out relationshipsDictionary, out relationshipAttributesDictionary, updateCache, datalLocaleList);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to load relationships from database.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);

                #endregion
            }

            #endregion

            #region Step : Create blank instances of attribute not found anywhere and is still missing

            if (relationshipContext.LoadRelationshipAttributes && relationshipContext.LoadBlankRelationshipAttributes)
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting with blank attribute instance creation...", MDMTraceSource.RelationshipGet);

                foreach (Entity entity in entities)
                {
                    CreateAttributesBlankInstance(entity, entity.Relationships, attributeModelsPerRT, relationshipAttributesDictionary, relationshipContext);
                }

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to create attribute blank instances.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with blank attribute instance creation.", MDMTraceSource.RelationshipGet);
                }
            }

            #endregion

            #region Step : Update cache

            UpdateRelationshipsCache(entitiesToBeLoadedFromDB, relationshipsDictionary, relationshipAttributesDictionary, relationshipContext, updateCache);

            #endregion

            #region Step : Verify Denormalized Relationships Status

            if (relationshipContext.LoadDenormalizedRelationshipsStatus && isRelationshipInheritanceEnabled)
            {
                IsDenormalizedRelationshipsDirty(entities, relationshipContext, callerContext);
            }

            #endregion

            #region Step : Determine Data Model Security

            DetermineDataModelSecurity(entities, relationshipContext);

            #endregion

            #region Step : Load Related Entities

            //Load related entity details only if system has configured common attributes.
            if (!relationshipContext.ReturnRelatedEntityDetails)
            {
                LoadRelatedEntities(entities, relationshipContext, iEntityManager, loadLatest, callerContext);
            }

            #endregion

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time taken to get relationships", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipContext"></param>
        /// <param name="callerContext"></param>
        private void PopulateRelationshipTypeIdList(RelationshipContext relationshipContext, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            if (relationshipContext.RelationshipTypeIdList == null || relationshipContext.RelationshipTypeIdList.Count < 1)
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading relationship types...", MDMTraceSource.RelationshipGet);
                }

                relationshipContext.RelationshipTypeIdList = new Collection<Int32>();
                RelationshipTypeCollection relationshipTypes = GetAllRelationshipTypes(callerContext);

                if (relationshipTypes != null && relationshipTypes.Count > 0)
                {
                    relationshipContext.RelationshipTypeIdList = relationshipTypes.GetRelationshipTypeIds();

                    if (isTracingEnabled)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded relationship types.Total relationship types loaded are:{0}", relationshipTypes.Count), MDMTraceSource.RelationshipGet);
                    }
                }
            }
            else
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Relationship types load would be skipped.", MDMTraceSource.RelationshipGet);
                }
            }

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to load relationship types.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationships"></param>
        /// <param name="attributeModelsPerRT"></param>
        /// <param name="relationshipAttributes"></param>
        /// <param name="relationshipContext"></param>
        private void CreateAttributesBlankInstance(Entity entity, RelationshipCollection relationships, Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT, Dictionary<String, AttributeCollection> relationshipAttributes, RelationshipContext relationshipContext)
        {
            if (relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    #region Step : Create blank instances of attribute not found anywhere and is still missing

                    AttributeModelCollection attributeModelsInThisRT = new AttributeModelCollection();
                    Int32 relationshipTypeId = relationship.RelationshipTypeId;

                    attributeModelsPerRT.TryGetValue(relationshipTypeId, out attributeModelsInThisRT);

                    if (relationshipContext.LoadRelationshipAttributeModels)
                    {
                        if (!entity.RelationshipsAttributeModels.ContainsKey(relationshipTypeId))
                        {
                            entity.RelationshipsAttributeModels.Add(relationshipTypeId, attributeModelsInThisRT);
                        }
                    }

                    if (attributeModelsInThisRT != null && attributeModelsInThisRT.Count > 0)
                    {
                        Collection<KeyValuePair<Int32, LocaleEnum>> missingAttributes = new Collection<KeyValuePair<Int32, LocaleEnum>>(attributeModelsInThisRT.GetAttributeIdLocaleList().Except(relationship.RelationshipAttributes.GetAttributeIdLocaleList()).ToList());

                        if (missingAttributes.Count > 0)
                        {
                            //Add the attributes which are not having values..
                            //DB call returns attributes only if they are having values
                            //So, loop through all the attribute models and add attribute with empty value
                            foreach (KeyValuePair<Int32, LocaleEnum> attrInfo in missingAttributes)
                            {
                                AttributeModel attributeModel = (AttributeModel)attributeModelsInThisRT.GetAttributeModel(attrInfo.Key, attrInfo.Value);

                                //Create blank attribute instance only if attribute model type is relationship.
                                if (attributeModel != null && attributeModel.AttributeModelType == AttributeModelType.Relationship)
                                {
                                    Attribute attribute = new Attribute(attributeModel);

                                    if (!attributeModel.IsLocalizable)
                                        attribute.Locale = _systemDataLocale;

                                    relationship.RelationshipAttributes.Add(attribute);

                                    if (relationshipAttributes != null && relationshipContext.DataLocales != null)
                                    {
                                        foreach (LocaleEnum dataLocale in relationshipContext.DataLocales)
                                        {
                                            AttributeCollection attributes = null;
                                            String key = CacheKeyGenerator.GetEntityRelationshipsCacheKey(relationship.Id, dataLocale);

                                            relationshipAttributes.TryGetValue(key, out attributes);

                                            if (attributes == null)
                                            {
                                                relationshipAttributes.Add(key, new AttributeCollection() { attribute });
                                            }
                                            else
                                            {
                                                attributes.Add(attribute);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    CreateAttributesBlankInstance(entity, relationship.RelationshipCollection, attributeModelsPerRT, relationshipAttributes, relationshipContext);

                    #endregion
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="iEntityManager"></param>
        /// <param name="loadLatest"></param>
        /// <param name="callerContext"></param>
        private void LoadRelatedEntities(EntityCollection entities, RelationshipContext relationshipContext, IEntityManager iEntityManager, Boolean loadLatest, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            Collection<Int64> relatedEntitiesIds = GetRelatedEntitiesIdList(entities);
            EntityContext entityContext = new EntityContext();
            entityContext.LoadEntityProperties = true;
            entityContext.LoadAttributes = relationshipContext.LoadRelatedEntitiesAttributes;
            entityContext.AttributeIdList = relationshipContext.RelatedEntitiesAttributeIdList;
            entityContext.LoadLookupDisplayValues = true;
            entityContext.DataLocales = relationshipContext.DataLocales;

            if (relatedEntitiesIds != null && relatedEntitiesIds.Count > 0)
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading related entities...", MDMTraceSource.RelationshipGet);
                }

                Boolean useEntityCacheForRelatedEntities = AppConfigurationHelper.GetAppConfig("MDMCenter.RelationshipManager.RelatedEntitiesLoad.UseEntityCache", false);
                Boolean loadEntitiesFromDB = loadLatest | !useEntityCacheForRelatedEntities;

                Int32 bulkGetBatchSize = AppConfigurationHelper.GetAppConfig("MDMCenter.RelationshipManager.RelatedEntitiesLoad.BulkGetBatchSize", 300);
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Related entities bulk get batch size is set to: {0}", bulkGetBatchSize),
                        MDMTraceSource.RelationshipGet);
                }

                //Disable PublishEvents and ApplyAVS as such its internal get.
                EntityCollection relatedEntities = iEntityManager.Get(relatedEntitiesIds, entityContext, loadEntitiesFromDB, callerContext.Application, callerContext.Module, false, false, true, bulkGetBatchSize);

                if (relatedEntities != null && relatedEntities.Count > 0)
                {
                    foreach (Entity entity in entities)
                    {
                        PopulateRelatedEntities(entity.Relationships, relatedEntities);
                    }

                    if (isTracingEnabled)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded related entities.Total related entities loaded are:{0}", relatedEntities.Count), MDMTraceSource.RelationshipGet);
                    }
                }
            }
            else
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Related Entities load would be skipped.", MDMTraceSource.RelationshipGet);
                }
            }

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to load related entities.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private Collection<Int64> GetRelatedEntitiesIdList(EntityCollection entities)
        {
            Collection<Int64> relatedEntitiesIds = null;

            if (entities != null && entities.Count > 0)
            {
                relatedEntitiesIds = new Collection<Int64>();

                foreach (Entity entity in entities)
                {
                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        Collection<Int64> entityIdList = entity.Relationships.GetRelatedEntityIdList();

                        if (entityIdList != null && entityIdList.Count > 0)
                        {
                            relatedEntitiesIds = ValueTypeHelper.MergeCollections(relatedEntitiesIds, entityIdList);
                        }
                    }
                }
            }

            return relatedEntitiesIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="relatedEntities"></param>
        private void PopulateRelatedEntities(RelationshipCollection relationships, EntityCollection relatedEntities)
        {
            if (relationships != null && relationships.Count > 0 && relatedEntities != null && relatedEntities.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    relationship.RelatedEntity = relatedEntities[relationship.RelatedEntityId];

                    PopulateRelatedEntities(relationship.RelationshipCollection, relatedEntities);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="sourceEntities"></param>
        private void PopulateSourceEntityDetails(RelationshipCollection relationships, EntityCollection sourceEntities)
        {
            if (relationships != null && relationships.Count > 0 && sourceEntities != null && sourceEntities.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship.RelationshipSourceEntityId > 0)
                    {
                        if (sourceEntities.Contains(relationship.RelationshipSourceEntityId))
                        {
                            Entity sourceEntity = sourceEntities[relationship.RelationshipSourceEntityId];

                            if (sourceEntity != null)
                            {
                                relationship.RelationshipSourceEntityName = sourceEntity.Name;
                                relationship.RelationshipSourceEntityLongName = sourceEntity.LongName;
                            }
                        }
                        else if (relationship.RelatedEntityId == relationship.RelationshipSourceEntityId && relationship.RelatedEntity != null)
                        {
                            relationship.RelationshipSourceEntityName = relationship.RelatedEntity.Name;
                            relationship.RelationshipSourceEntityLongName = relationship.RelatedEntity.LongName;
                        }
                    }

                    PopulateSourceEntityDetails(relationship.RelationshipCollection, sourceEntities);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="attributeIds"></param>
        private void PopulateRelationshipAttributes(RelationshipCollection relationships, Collection<Int32> attributeIds)
        {
            if (attributeIds != null && attributeIds.Count > 0 && relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship.RelatedEntity != null && relationship.RelatedEntity.Relationships != null && relationship.RelatedEntity.Relationships.Count > 0 && relationship.RelatedEntity.Relationships.Contains(relationship.FromEntityId))
                    {
                        IAttributeCollection attributes = relationship.RelatedEntity.Relationships[relationship.FromEntityId].GetRelationshipAttributes();

                        if (attributes != null)
                        {
                            foreach (Int32 attributeId in attributeIds)
                            {
                                IAttribute attribute = attributes.GetAttribute(attributeId, relationship.Locale);

                                if (attribute != null)
                                {
                                    relationship.RelationshipAttributes.Add(attribute);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relationshipContext"></param>
        private void DetermineDataModelSecurity(EntityCollection entities, RelationshipContext relationshipContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            #region Initialization of Data Security

            DataSecurityBL dataSecurityBL = null;
            PermissionContext permissionContext = null;
            Boolean applySecurity = relationshipContext.ApplyDMS;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            #endregion

            if (applySecurity)
            {
                //Get the permissions for each relationship and add to the collection only if relationship is having permissions..
                //Prepare permission context.. Keep RoleId as '0' and pass the current user Id.. The Load logic determines the permissions by considering all roles of the current user.
                permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, _securityPrincipal.CurrentUserId, 0);
                dataSecurityBL = new DataSecurityBL(_securityPrincipal, permissionContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for initialization of permission context and data security.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
            }

            var permissionByContext = new Dictionary<String, Collection<UserAction>>();

            foreach (Entity entity in entities)
            {
                String key = String.Format("EntityId:{0}_ContainerId:{1}_CategoryId:{2}", entity.Id, entity.ContainerId, entity.CategoryId);
                Collection<UserAction> permissionSet = new Collection<UserAction>();
                Permission permission = null;

                if (applySecurity)
                {
                    if (permissionByContext.ContainsKey(key))
                    {
                        permissionSet = permissionByContext[key];
                    }
                    else
                    {
                        permissionContext.OrgId = entity.OrganizationId;
                        permissionContext.ContainerId = entity.ContainerId;
                        permissionContext.CategoryId = entity.CategoryId;

                        permission = dataSecurityBL.GetRelationshipPermission(permissionContext);

                        if (permission != null && permission.PermissionSet != null)
                        {
                            permissionSet = permission.PermissionSet;

                            permissionByContext.Add(key, permissionSet);
                        }
                    }
                }
                else
                {
                    //Provide all permissions in case of apply security is set to false.
                    permissionSet.Add(UserAction.View);
                    permissionSet.Add(UserAction.Add);
                    permissionSet.Add(UserAction.Update);
                    permissionSet.Add(UserAction.Delete);
                }

                AttachPermissionSetToRelationships(entity.Relationships, permissionSet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="permissionSet"></param>
        private void AttachPermissionSetToRelationships(RelationshipCollection relationships, Collection<UserAction> permissionSet)
        {
            if (relationships != null)
            {
                relationships.PermissionSet = permissionSet;

                foreach (Relationship relationship in relationships)
                {
                    relationship.PermissionSet = permissionSet;
                    AttachPermissionSetToRelationships(relationship.RelationshipCollection, permissionSet);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipContext"></param>
        /// <param name="attributeModelsPerRT"></param>
        private void CalculateReturnRelatedEntityDetailsFlag(RelationshipContext relationshipContext, Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            //If related entity attributes are not configured then load base meta from database.
            if (relationshipContext.RelatedEntitiesAttributeIdList != null && relationshipContext.RelatedEntitiesAttributeIdList.Count > 0)
            {
                var enumerator = attributeModelsPerRT.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    AttributeModelCollection attributeModels = enumerator.Current.Value;

                    foreach (Int32 relatedEntitiesAttributeId in relationshipContext.RelatedEntitiesAttributeIdList)
                    {
                        AttributeModel attributeModel = (AttributeModel)attributeModels.GetAttributeModel(relatedEntitiesAttributeId, relationshipContext.Locale);

                        if (attributeModel != null && attributeModel.AttributeModelType != AttributeModelType.Relationship)
                        {
                            relationshipContext.ReturnRelatedEntityDetails = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                relationshipContext.ReturnRelatedEntityDetails = true;
            }

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to calculate related entity details flag.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="callerContext"></param>
        private void IsDenormalizedRelationshipsDirty(EntityCollection entities, RelationshipContext relationshipContext, CallerContext callerContext)
        {
            RelationshipDenormBL relationshipDenormBL = new RelationshipDenormBL();
            EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();
            Int32 relationshipTyped = relationshipContext.RelationshipTypeIdList.Count > 1 ? 0 : relationshipContext.RelationshipTypeIdList.FirstOrDefault();

            foreach (Entity entity in entities)
            {
                Boolean isDenormalizedRelationshipsDirty = relationshipDenormBL.IsDenormalizedRelationshipsDirty(entity.Id, entity.ContainerId, relationshipTyped, entity.EntityTreeIdList, callerContext);
                entity.Relationships.IsDenormalizedRelationshipsDirty = isDenormalizedRelationshipsDirty;
            }
        }

        #endregion

        #region Cache Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="loadLatest"></param>
        /// <param name="entitiesToBeLoadedFromDB"></param>
        private void GetRelationshipsFromCache(EntityCollection entities, RelationshipContext relationshipContext, EntityCacheStatusCollection entityCacheStatusCollection, Boolean loadLatest, ref EntityCollection entitiesToBeLoadedFromDB)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            if (!loadLatest)
            {
                RelationshipBufferManager relationshipBufferManager = new RelationshipBufferManager();
                Boolean isCacheDirty = false;
                EntityCacheStatus entityCacheStatus = null;

                foreach (Entity entity in entities)
                {
                    entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);
                    isCacheDirty = (entityCacheStatus != null) ? entityCacheStatus.IsRelationshipCacheDirty : false;

                    if (!isCacheDirty)
                    {
                        RelationshipCollection relationships = relationshipBufferManager.FindRelationships(entity, relationshipContext);

                        if (relationships != null)
                        {
                            entity.Relationships = relationships;
                        }
                        else
                        {
                            entitiesToBeLoadedFromDB.Add(entity);

                            // If Cache status is not dirty and if cache is unavailable, then set for reload.
                            if (entityCacheStatus != null)
                            {
                                entityCacheStatus.IsRelationshipCacheDirty = true;
                            }
                        }
                    }
                    else
                    {
                        entitiesToBeLoadedFromDB.Add(entity);
                    }
                }
            }
            else
            {
                entitiesToBeLoadedFromDB = entities;
            }

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to load relationships from cache.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitiesToBeLoadedFromDB"></param>
        /// <param name="relationshipsDictionary"></param>
        /// <param name="relationshipAttributesDictionary"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="updateCache"></param>
        private void UpdateRelationshipsCache(EntityCollection entitiesToBeLoadedFromDB, Dictionary<Int64, RelationshipCollection> relationshipsDictionary, Dictionary<String, AttributeCollection> relationshipAttributesDictionary, RelationshipContext relationshipContext, Boolean updateCache)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (updateCache)
            {
                DurationHelper durationHelper = new DurationHelper(DateTime.Now);

                if (entitiesToBeLoadedFromDB != null && relationshipsDictionary != null)
                {
                    RelationshipBufferManager relationshipBufferManager = new RelationshipBufferManager();

                    foreach (Entity entity in entitiesToBeLoadedFromDB)
                    {
                        Collection<String> cachedKeys = new Collection<String>();
                        String cachedKey = String.Empty;
                        RelationshipCollection relationships = null;

                        relationshipBufferManager.RemoveCacheKeysForAllLevel(entity);

                        relationshipsDictionary.TryGetValue(entity.Id, out relationships);

                        if (relationships != null)
                        {
                            Dictionary<String, Dictionary<String, Object>> maxLevelKeyValuePairByRelationshipTypeId = new Dictionary<String, Dictionary<String, Object>>();
                            LoadMaxLevelByRelationshipTypeId(relationships, maxLevelKeyValuePairByRelationshipTypeId, 0);
                            Collection<Int32> relationshipTypeIds = new Collection<Int32>(relationshipContext.RelationshipTypeIdList.ToList());

                            var enumerator = maxLevelKeyValuePairByRelationshipTypeId.GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                RelationshipCollection currentRelationships = new RelationshipCollection();

                                Dictionary<String, Object> dictionary = enumerator.Current.Value;

                                Int32 relationshipTypeId = (Int32)dictionary["RelationshipTypeId"];
                                Int16 maxLevel = (Int16)dictionary["MaxLevel"];
                                Int64 relationshipParentId = (Int64)dictionary["RelationshipParentId"];
                                Int64 rootRelationshipParentId = (Int64)dictionary["RootRelationshipParentId"];
                                String relationshipParentIds = (String)dictionary["RelationshipParentIdList"];
                                _relationshipParentIdList = new Collection<Int64>();

                                if (!String.IsNullOrWhiteSpace(relationshipParentIds))
                                {
                                    _relationshipParentIdList = ValueTypeHelper.SplitStringToLongCollection(relationshipParentIds, ',');
                                }

                                if (maxLevel == Constants.RELATIONSHIP_LEVEL_ONE)
                                {
                                    relationshipTypeIds.Remove(relationshipTypeId);
                                }

                                CloneAndLoadRelationshipsForLevel(maxLevel, relationshipTypeId, rootRelationshipParentId, relationships, currentRelationships);

                                cachedKey = relationshipBufferManager.UpdateRelationships(entity, currentRelationships, relationshipTypeId, relationshipParentId, maxLevel, relationshipContext);

                                if (!String.IsNullOrWhiteSpace(cachedKey))
                                {
                                    cachedKeys.Add(cachedKey);
                                }

                                UpdateRelationshipAttributesCache(maxLevel, entity, currentRelationships, relationshipAttributesDictionary, relationshipContext, cachedKeys);
                            }

                            UpdateEmptyRelationshipInstancesInCache(entity, relationshipTypeIds, relationshipContext, cachedKeys);
                        }
                        else
                        {
                            UpdateEmptyRelationshipInstancesInCache(entity, relationshipContext.RelationshipTypeIdList, relationshipContext, cachedKeys);
                        }

                        relationshipBufferManager.UpdateCacheKeysForAllLevel(entity, cachedKeys);
                    }
                }

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to update relationships in the cache.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <param name="entity"></param>
        /// <param name="relationships"></param>
        /// <param name="relationshipAttributesDictionary"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="cachedKeys"></param>
        private void UpdateRelationshipAttributesCache(Int16 currentLevel, Entity entity, RelationshipCollection relationships, Dictionary<String, AttributeCollection> relationshipAttributesDictionary, RelationshipContext relationshipContext, Collection<String> cachedKeys)
        {
            RelationshipBufferManager relationshipBufferManager = new RelationshipBufferManager();
            String cachedKey = String.Empty;

            if (relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship.Level == currentLevel)
                    {
                        if (relationshipContext != null && relationshipContext.DataLocales.Count > 0)
                        {
                            foreach (LocaleEnum dataLocale in relationshipContext.DataLocales)
                            {
                                String key = CacheKeyGenerator.GetEntityRelationshipsCacheKey(relationship.Id, dataLocale);

                                if (relationshipAttributesDictionary != null && relationshipAttributesDictionary.ContainsKey(key))
                                {
                                    AttributeCollection attributes = relationshipAttributesDictionary[key];
                                    cachedKey = relationshipBufferManager.UpdateRelationshipAttributes(entity, attributes, relationship.RelationshipTypeId, relationship.Id, dataLocale);
                                }

                                if (relationshipContext.LoadSources)
                                {
                                    key = CacheKeyGenerator.GetEntityRelationshipsWithSourcesCacheKey(relationship.Id, dataLocale);

                                    if (relationshipAttributesDictionary != null && relationshipAttributesDictionary.ContainsKey(key))
                                    {
                                        AttributeCollection attributes = relationshipAttributesDictionary[key];
                                        relationshipBufferManager.UpdateRelationshipAttributesWithSource(entity, attributes, relationship.RelationshipTypeId, relationship.Id, dataLocale);
                                    }
                                }

                                if (!String.IsNullOrWhiteSpace(cachedKey))
                                {
                                    cachedKeys.Add(cachedKey);
                                }
                            }
                        }
                    }

                    UpdateRelationshipAttributesCache(currentLevel, entity, relationship.RelationshipCollection, relationshipAttributesDictionary, relationshipContext, cachedKeys);
                }
            }
        }

        /// <summary>
        /// This method loads max level by relationship parent id and type id.
        /// </summary>
        /// <param name="relationships">Specifies collection of relationship</param>
        /// <param name="maxLevelKeyValuePairByRelationshipTypeId"></param>
        /// <param name="rootRelationshipParentId">Indicates id of root level relationship for a level.</param>
        private void LoadMaxLevelByRelationshipTypeId(RelationshipCollection relationships, Dictionary<String, Dictionary<String, Object>> maxLevelKeyValuePairByRelationshipTypeId, Int64 rootRelationshipParentId)
        {
            foreach (Relationship relationship in relationships)
            {
                //This key hold data based on relationship parent id and relationship type.
                String key = String.Format("RELPID:{0}_RELTYPEID:{1}", relationship.RelationshipParentId, relationship.RelationshipTypeId);
                Boolean hasChildRelationships = (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0) ? true : false;

                if (relationship.Level == Constants.RELATIONSHIP_LEVEL_ONE)
                {
                    rootRelationshipParentId = relationship.Id;
                }
                else
                {
                    _relationshipParentIdList.Add(relationship.RelationshipParentId);
                }

                if (!maxLevelKeyValuePairByRelationshipTypeId.ContainsKey(key))
                {
                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();

                    //This key indicates relationship type id based on level. In case A->K->S level relationship, it will store K->S relationship type id for level 2.
                    dictionary.Add("RelationshipTypeId", relationship.RelationshipTypeId);

                    //Calculates max level for each relationship type.
                    dictionary.Add("MaxLevel", relationship.Level);

                    //Indicates relationship parent id for particular relationship type id and level.
                    dictionary.Add("RelationshipParentId", relationship.RelationshipParentId);

                    //This key holds root parent id in case of multi level relation. 
                    //Scenario : C->A->K->S. For K->S relationship in context "C" root relationship parent id would be C->A's relationship id.
                    dictionary.Add("RootRelationshipParentId", rootRelationshipParentId);

                    //Calculates list of relationship parent id for a level. 
                    //Example : C->A->K->S relationship , parent id list will be C->A and A->K relationship's id for K->S relationship.
                    dictionary.Add("RelationshipParentIdList", ValueTypeHelper.JoinCollection(_relationshipParentIdList, ","));

                    maxLevelKeyValuePairByRelationshipTypeId.Add(key, dictionary);
                }

                if (hasChildRelationships)
                {
                    LoadMaxLevelByRelationshipTypeId(relationship.RelationshipCollection, maxLevelKeyValuePairByRelationshipTypeId, rootRelationshipParentId);
                }
                else
                {
                    _relationshipParentIdList = new Collection<Int64>();
                }
            }
        }

        /// <summary>
        /// This method calculates all relationships in hierarchy manner for requested level and relationship type id.
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="rootRelationshipParentId"></param>
        /// <param name="relationships"></param>
        /// <param name="currentRelationships"></param>
        private void CloneAndLoadRelationshipsForLevel(Int16 currentLevel, Int32 relationshipTypeId, Int64 rootRelationshipParentId, RelationshipCollection relationships, RelationshipCollection currentRelationships)
        {
            foreach (Relationship relationship in relationships)
            {
                if (relationship.Level <= currentLevel)
                {
                    Relationship clonedRelationship = relationship.CloneBaseProperties();

                    if (clonedRelationship.Level == currentLevel && clonedRelationship.RelationshipCollection != null)
                    {
                        //If relationship is having any child relationships to then clear it out before putting into cache for a level.
                        clonedRelationship.RelationshipCollection = new RelationshipCollection();
                    }
                    else
                    {
                        //If relationship is parent for requested child then clear it out all relationship attributes and child level relationship.
                        clonedRelationship.RelationshipAttributes = new AttributeCollection();
                        clonedRelationship.RelationshipCollection = new RelationshipCollection();
                    }

                    if (clonedRelationship.RelationshipParentId > 0)
                    {
                        if (_relationshipParentIdList.Contains(clonedRelationship.Id) || relationship.Level == currentLevel)
                        {
                            currentRelationships.AddChildRelationshipForParent(clonedRelationship.RelationshipParentId, clonedRelationship);
                        }
                    }
                    else
                    {
                        if (clonedRelationship.RelationshipTypeId == relationshipTypeId || relationship.Id == rootRelationshipParentId)
                        {
                            currentRelationships.Add(clonedRelationship);
                        }
                    }

                    CloneAndLoadRelationshipsForLevel(currentLevel, relationshipTypeId, rootRelationshipParentId, relationship.RelationshipCollection, currentRelationships);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filteredRelationshipTypeIds"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="cachedKeys"></param>
        private void UpdateEmptyRelationshipInstancesInCache(Entity entity, Collection<Int32> filteredRelationshipTypeIds, RelationshipContext relationshipContext, Collection<String> cachedKeys)
        {
            //We don't have any relationships for current entity.Store blank relationship object in distributed cache.
            if (filteredRelationshipTypeIds != null && filteredRelationshipTypeIds.Count > 0)
            {
                RelationshipBufferManager relationshipBufferManager = new RelationshipBufferManager();

                foreach (Int32 relationshipTypeId in filteredRelationshipTypeIds)
                {
                    String cachedKey = relationshipBufferManager.UpdateRelationships(entity, new RelationshipCollection(), relationshipTypeId, 0, 1, relationshipContext);

                    if (!String.IsNullOrWhiteSpace(cachedKey))
                    {
                        cachedKeys.Add(cachedKey);
                    }
                }
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="callerContext"></param>
        /// <param name="targetSite"></param>
        private void ValidateEntities(EntityCollection entities, CallerContext callerContext, String targetSite)
        {
            if (entities == null || entities.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111816", false, callerContext);
                throw new MDMOperationException("111816", _localeMessage.Message, "RelationshipManager.RelationshipBL", String.Empty, targetSite);   //Entities collection is null or empty.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="targetSite"></param>
        private void ValidateRelationshipContext(RelationshipContext relationshipContext, CallerContext callerContext, String targetSite)
        {
            if (relationshipContext == null)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "112887 ", false, callerContext);
                throw new MDMOperationException("112887 ", _localeMessage.Message, "RelationshipManager.RelationshipBL", String.Empty, targetSite);//Relationship context is not available.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="callerContext"></param>
        /// <param name="targetSite"></param>
        private void ValidateEntity(Entity entity, CallerContext callerContext, String targetSite)
        {
            if (entity == null)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111796 ", false, callerContext);
                throw new MDMOperationException("111796", _localeMessage.Message, "RelationshipManager.RelationshipBL", String.Empty, targetSite);//Entity is not available.
            }

            if (entity.Id < 1)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111645 ", false, callerContext);
                throw new MDMOperationException("111645", _localeMessage.Message, "RelationshipManager.RelationshipBL", String.Empty, targetSite);//Entity Id Must be greater than 0.
            }

            if (entity.ContainerId < 1)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "112887 ", false, callerContext);
                throw new MDMOperationException("112887 ", _localeMessage.Message, "RelationshipManager.RelationshipBL", String.Empty, targetSite);//Relationship context is not available.
            }

            if (entity.EntityContext == null)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111786", false, callerContext);
                throw new MDMOperationException("111786", _localeMessage.Message, "RelationshipManager", String.Empty, "LoadRelationship"); //EntityContext is not available.
            }

            ValidateRelationshipContext(entity.EntityContext.RelationshipContext, callerContext, "Get");
        }

        #endregion

        #endregion

        #endregion
    }
}