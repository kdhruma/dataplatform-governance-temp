//using Microsoft.Practices.ServiceLocation;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading;
//using System.Transactions;
//using SM = System.ServiceModel;

//namespace MDM.EntityManager.Business.EntityOperations
//{
//    using ActivityLogManager.Business;
//    using BufferManager;
//    using BusinessObjects;
//    using BusinessObjects.Diagnostics;
//    using BusinessObjects.Exports;
//    using ConfigurationManager.Business;
//    using Core;
//    using Core.Exceptions;
//    using Core.Extensions;
//    using Data;
//    using ExceptionManager;
//    using ExportProfileManager.Business;
//    using Helpers;

//    using Interfaces;

//    using KnowledgeManager.Business;
//    using MDM.ApplicationServiceManager.Business;
//    using MDM.BusinessRule.Manager;

//    using MDM.JigsawIntegrationManager;

//    using MessageManager.Business;
//    using ParallelizationManager.Processors;
//    using PermissionManager.Business;
//    using RelationshipManager.Business;
//    using Utility;

//    /// <summary>
//    /// 
//    /// </summary>
//    internal class EntityProcessManager : BusinessLogicBase
//    {
//        #region Fields

//        /// <summary>
//        ///     Field denoting locale Message
//        /// </summary>
//        private LocaleMessage _localeMessage;

//        /// <summary>
//        ///     Field denoting localeMessageBL
//        /// </summary>
//        private readonly LocaleMessageBL _localeMessageBL;

//        /// <summary>
//        ///     Field denoting the action of the operation for ex.: Create, Update
//        /// </summary>
//        private String _operationAction = String.Empty;

//        /// <summary>
//        ///     Field denoting the security principal
//        /// </summary>
//        private readonly SecurityPrincipal _securityPrincipal;

//        /// <summary>
//        ///     Attribute Unique Identifier which is mapped to be the ShortName of the Entity
//        /// </summary>
//        private readonly IAttributeUniqueIdentifier _shortNameMappedAttributeIdentifier = MDMObjectFactory.GetIAttributeUniqueIdentifier(String.Empty, String.Empty);

//        /// <summary>
//        /// 
//        /// </summary>
//        private readonly IEntityManager _entityManager = null;

//        /// <summary>
//        /// Indicates current trace settings
//        /// </summary>
//        private TraceSettings _currentTraceSettings = new TraceSettings();

//        /// <summary>
//        /// 
//        /// </summary>
//        private static Object bulkEditLockObject = new Object();

//        #endregion

//        #region Constructors

//        internal EntityProcessManager()
//        {
//            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings().Clone();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public EntityProcessManager(IEntityManager entityManager)
//            : this()
//        {
//            _entityManager = entityManager;
//            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
//            _localeMessageBL = new LocaleMessageBL();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public EntityProcessManager(IEntityManager entityManager, SecurityPrincipal securityPrincipal)
//            : this(entityManager)
//        {
//            _securityPrincipal = securityPrincipal;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public EntityProcessManager(IEntityManager entityManager, SecurityPrincipal securityPrincipal, String operationAction)
//            : this(entityManager, securityPrincipal)
//        {
//            _operationAction = operationAction;
//        }

//        #endregion

//        #region Properties

//        #endregion

//        #region Methods

//        #region Public Methods

//        /// <summary>
//        ///     Process given list of entities based on their actions
//        /// </summary>
//        /// <param name="entities">Instance of entities to be processed in MDM system</param>
//        /// <param name="entityProcessingOptions">Specifies processing options</param>
//        /// <param name="callerContext">Indicates the Caller Context</param>
//        /// <returns>Results of the operation having errors and information if any</returns>
//        /// <exception cref="MDMOperationException">If Entities collection is null or operation result is null</exception>
//        public EntityOperationResultCollection Process(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
//        {
//            EntityOperationResultCollection entityOperationResults = null;
//            EntityCacheStatusCollection entityCacheStatusCollection = new EntityCacheStatusCollection();
//            EntityCollection allEntities = new EntityCollection();
//            Boolean isInitialLoad = true;

//            #region Step : Diagnostic Activity initialization

//            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
//            ExecutionContext executionContext = null;

//            String traceMessage = String.Empty;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                CallDataContext callDataContext = new CallDataContext();
//                callDataContext.EntityIdList = entities.GetEntityIdList();
//                executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityProcessingOptions.ToXml());
//                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.EntityProcess);
//                diagnosticActivity.Start(executionContext);
//            }

//            #endregion

//            try
//            {
//                #region Step : Fill Missing Ids in Entities

//                if (entityProcessingOptions != null && entityProcessingOptions.ResolveIdsByNames)
//                {
//                    EntityFillHelper.FillIdsInEntityByNames(entities, _entityManager, callerContext);

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        diagnosticActivity.LogDurationInfo("Completed missing ids fill process.");
//                    }
//                }

//                #endregion

//                #region Step : Populate missing Entity Ids by using EntityGuids

//                Collection<Entity> entitiesWithoutId = new Collection<Entity>();

//                foreach (Entity entity in entities)
//                {
//                    if (entity.Action != ObjectAction.Create && entity.Id < 0 && entity.EntityGuid.HasValue)
//                    {
//                        entitiesWithoutId.Add(entity);
//                    }
//                }

//                if (entitiesWithoutId.Count > 0)
//                {
//                    Collection<Int64> entityIds = _entityManager.GetEntityGuidsMap(entitiesWithoutId.Select(entity => entity.EntityGuid.Value).ToCollection(), callerContext);

//                    if (entitiesWithoutId.Count == entityIds.Count)
//                    {
//                        for (Int32 i = 0; i < entitiesWithoutId.Count; i++)
//                        {
//                            entitiesWithoutId[i].Id = entityIds[i];
//                        }
//                    }
//                    else if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        diagnosticActivity.LogWarning("Some entities are missing Id values and couldn't have been populated with GetEntityGuidsMap method.");
//                    }
//                }
//                #endregion

//                #region Step : Prepare Entity Operation Result Schema

//                entityOperationResults = new EntityOperationResultCollection(entities);

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    diagnosticActivity.LogDurationInfo("Prepared entity operation results");
//                }

//                #endregion

//                #region Step : Initial Setup

//                // c# ref types default is null
//                LocaleEnum systemUILocale = LocaleEnum.UnKnown;
//                LocaleEnum systemDataLocale = LocaleEnum.UnKnown;
//                Boolean continueProcess;

//                EntityValidationBL entityValidationManager;
//                RelationshipBL relationshipManager;
//                ExtensionRelationshipBL extensionRelationshipBL;
//                Collection<LocaleEnum> allLocalesAcrossEntities;
//                DBCommandProperties command;
//                Collection<Int64> renamedEntityIdList;
//                Collection<Int64> entityIdsForLifecycleStatusChange = new Collection<Int64>();

//                // c# boolean default is false
//                Boolean validate;
//                Boolean revalidate;
//                Boolean isCacheEnabled;
//                Boolean updateAttributeResults;
//                Boolean populateAuditInfoSystemAttributes;
//                Boolean populateProgramNameSystemAttribute;
//                Boolean isTranslationEnabled;
//                Boolean isRelationshipInheritanceEnabled;
//                Boolean isSourceProcessingEnabled;
//                Boolean isEntityUniqueIdentificationConfigEnabled;
//                Boolean isEntityRevalidateModeEnabled;

//                String loginUser = String.Empty;

//                MDMRuleParams mdmRuleParams = null;

//                var initialSetupConfigurationActivity = new DiagnosticActivity(null, "InitialSetupConfiguration SubActivity");

//                try
//                {
//                #region Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                        initialSetupConfigurationActivity.Start();

//                    String entityIdListAsString = EntityOperationsCommonUtility.GetEntityIdListAsString(entities.GetEntityIdList());
//                    traceMessage = String.Format("Starting process entity main execution flow for Entity id list:{0}, EntityProcessingOptions:{1}, Application:{3} and Service:{3}", entityIdListAsString, entityProcessingOptions.ToXml(), callerContext.Application, callerContext.Module);
//                        initialSetupConfigurationActivity.LogInformation(traceMessage);
//                }

//                #endregion

//                #region Parameter validations

//                if (entities == null || entities.Count < 1)
//                {
//                        initialSetupConfigurationActivity.LogError("111816", String.Empty);
//                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111816", false, callerContext);
//                    throw new MDMOperationException("111816", _localeMessage.Message, "EntityManager", String.Empty, "Process"); //Entities collection is null or empty
//                }

//                #region Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                        initialSetupConfigurationActivity.LogDurationInfo("Parameter validations completed");
//                }

//                #endregion

//                #endregion Parameter valudations

//                //Collection of EntityIds which are renamed via mapped Entity's ShortName as Attribute in MetaDataPage-Config.
//                    renamedEntityIdList = new Collection<Int64>();

//                //Get command
//                    command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

//                //Get system locales
//                    systemUILocale = GlobalizationHelper.GetSystemUILocale();
//                    systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

//                    loginUser = _securityPrincipal.CurrentUserName;

//                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
//                {
//                    callerContext.ProgramName = "EntityManager.EntityBL.Process";
//                }

//                    isEntityRevalidateModeEnabled = (callerContext.Module == MDMCenterModules.Revalidate) ? true : false;

//                    entityValidationManager = new EntityValidationBL();
//                    relationshipManager = new RelationshipBL();
//                    extensionRelationshipBL = new ExtensionRelationshipBL();
//                    allLocalesAcrossEntities = new Collection<LocaleEnum>();

//                #region Get Config Key Values

//                //Get AppConfig value and check whether validation is turned on
//                    validate = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.Entity.Process.Validate", true);
//                    revalidate = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.Entity.Process.Revalidate", true);

//                // Get AppConfig which specify whether cache is enabled for entity or not
//                    isCacheEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.EntityCache.Enabled", true);

//                //Get AppConfig which specify whether entity should be updated with the processed attribute results
//                    updateAttributeResults = AppConfigurationHelper.GetAppConfig("MDMCenter.EntityProcess.ReadAttributeResults", false);

//                //Get AppConfig value that specifies whether or not to evaluate and store AuditInfo SystemAttributes
//                    populateAuditInfoSystemAttributes = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityProcessManager.AuditInfoSystemAttributes.Populate", false);

//                //Get AppConfig value that specifies whether or not to evaluate and store ProgramName AuditInfo SystemAttribute
//                    populateProgramNameSystemAttribute = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityProcessManager.AuditInfoSystemAttributes.PopulateProgramName", false);

//                //Get AppConfig which specify whether Translation needs to be done for entity data
//                //Implemented feature toggle

//                //var isTranslationEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.TranslationManagement.Enabled", true);
//                    isTranslationEnabled = EntityOperationsCommonUtility.IsTranslationEnabled();

//                //Get AppConfig which specify whether relationship inheritance feature is enabled or not.
//                    isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig("RelationshipManager.Relationship.Inheritance.Enabled", true);

//                // DQM modules features checking

//                    isSourceProcessingEnabled = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.DataQualityManagement, "Entity data source tracking", "1");

//                    isEntityUniqueIdentificationConfigEnabled = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.MDMCenter, "EntityUniqueIdentificationConfig", "1");

//                #endregion

//                #region Update Entity.EntityContext with DataLocales if not provided yet, refresh Entity.ChangeContext and Entity.EntityMoveContext..

//                foreach (Entity entity in entities)
//                {
//                    //Refresh entity context based on data..do not trust input entity.EntityContext...
//                    EntityContext entityContext = new EntityContext(entity);

//                    Collection<LocaleEnum> entityContextDataLocales = entityContext.DataLocales;

//                    if (!entityContextDataLocales.Contains(systemDataLocale))
//                    {
//                        entityContextDataLocales.Add(systemDataLocale);
//                    }

//                    if (entityContext.RelationshipContext != null)
//                    {
//                        Collection<LocaleEnum> relationshipContextDataLocales = entityContext.RelationshipContext.DataLocales;

//                        if (relationshipContextDataLocales != null && !relationshipContextDataLocales.Contains(systemDataLocale))
//                    {
//                            relationshipContextDataLocales.Add(systemDataLocale);
//                        }
//                    }

//                    entity.EntityContext = entityContext;

//                    //Create a union all locales for all entities which are coming for processing.
//                    //This is needed for BulkEntityGet(), since we load all entities in 1 Get call, we need all the locales in which data are to be loaded across entities
//                    if (!allLocalesAcrossEntities.Contains(entityContext.Locale))
//                    {
//                        allLocalesAcrossEntities.Add(entityContext.Locale);
//                    }

//                    foreach (LocaleEnum locale in entityContextDataLocales)
//                    {
//                        if (!allLocalesAcrossEntities.Contains(locale))
//                        {
//                            allLocalesAcrossEntities.Add(locale);
//                        }
//                    }

//                    //Refresh EntityMoveContext...
//                    if (entity.Action == ObjectAction.Reclassify)
//                    {
//                        EntityMoveContext moveContext = entity.GetEntityMoveContext() as EntityMoveContext;
//                        if (moveContext != null && moveContext.ReParentType == ReParentTypeEnum.UnKnown)
//                        {
//                            if (moveContext.TargetCategoryId > 0 || !String.IsNullOrWhiteSpace(moveContext.TargetCategoryPath))
//                            {
//                                moveContext.ReParentType = ReParentTypeEnum.CategoryReParent;
//                            }
//                            else if (moveContext.TargetParentEntityId > 0)
//                            {
//                                moveContext.ReParentType = ReParentTypeEnum.HiearchyReParent;
//                            }
//                            else if (moveContext.TargetParentExtensionEntityId > 0)
//                            {
//                                moveContext.ReParentType = ReParentTypeEnum.ExtensionReParent;
//                            }
//                        }
//                    }

//                        if (entity.Attributes.Count > 0)
//                        {
//                            IAttribute lifecycleStatusAttribute = entity.GetAttribute((Int32)SystemAttributes.LifecycleStatus, systemDataLocale);
//                            if (lifecycleStatusAttribute != null && lifecycleStatusAttribute.Action == ObjectAction.Update)
//                            {
//                                entityIdsForLifecycleStatusChange.Add(entity.Id);
//                    }
//                        }

//                        allEntities.Add(entity);
//                    }

//                #endregion Update Entity.EntityContext with DataLocales from Attribute in Entity

//                #region Validate Base properties of entity

//                entityValidationManager.ValidateEntityProperties(entities, entityOperationResults, systemUILocale, _operationAction, entityProcessingOptions.ResolveIdsByNames, callerContext);

//                #region Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    diagnosticActivity.LogDurationInfo("Entity base properties validation completed ");
//                }

//                #endregion

//                #endregion

//                #region Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    diagnosticActivity.LogDurationInfo("Initial setup and config reads completed");
//                }

//                #endregion
//                }
//                finally
//                {
//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        initialSetupConfigurationActivity.Stop();
//                    }
//                }

//                #endregion

//                #region Step : Check lock status for entities

//                if (!isEntityRevalidateModeEnabled)
//                {
//                    VerifyEntityLock(entities, entityOperationResults, entityIdsForLifecycleStatusChange, callerContext);
//                    continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, allEntities, entityOperationResults, callerContext);

//                    if (!continueProcess)
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = "All entities are locked. Entity processing would terminate now";
//                            diagnosticActivity.LogWarning(traceMessage);
//                        }

//                        return entityOperationResults;
//                    }
//                }

//                #endregion Step : Check lock status for entities

//                #region Step : Entity Identification - Pre Process

//                EntityCollection entitiesForUniqueCheck = null;

//                if (isEntityUniqueIdentificationConfigEnabled &&
//                    entityProcessingOptions.ProcessingMode != ProcessingMode.AsyncCreate &&
//                    entityProcessingOptions.ProcessingMode != ProcessingMode.AsyncUpdate &&
//                    entityProcessingOptions.ProcessingMode != ProcessingMode.AsyncDelete)
//                {

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = "MDMCenter.EntityIdentificationConfiguration.Enabled is true, entering Process Entity Identification after BR execution";
//                        diagnosticActivity.LogInformation(traceMessage);
//                    }

//                    //Entity uniqueness check will be performed only if the (module is Import) OR (entity action = create)
//                    if (callerContext != null && callerContext.Module == MDMCenterModules.Import)
//                    {
//                        entitiesForUniqueCheck = entities;
//                    }
//                    else
//                    {
//                        entitiesForUniqueCheck = new EntityCollection();

//                        foreach (Entity entity in entities)
//                        {
//                            if (entity.Action == ObjectAction.Create)
//                            {
//                                entitiesForUniqueCheck.Add(entity);
//                            }
//                        }
//                    }

//                    if (entitiesForUniqueCheck.Count > 0)
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Entity uniqueness check before BR execution will be performed for {0} entities out of {1} entities received for processing.", entitiesForUniqueCheck.Count, entities.Count);
//                            diagnosticActivity.LogInformation(traceMessage);
//                        }

//                        EntityIdentificationManager.ProcessEntityIdentificationData(entitiesForUniqueCheck, entityOperationResults, callerContext, true);
//                        entityOperationResults.RefreshOperationResultsSchema(entitiesForUniqueCheck);
//                    }
//                }

//                #endregion Step : Entity Identification - Pre Process

//                #region Step : Apply AVS security permissions

//                if (entityProcessingOptions.ApplyAVS)
//                {
//                    PermissionBL permissionBL = new PermissionBL();

//                    foreach (Entity entity in entities)
//                    {
//                        if (entity.Action != ObjectAction.Read && entity.Action != ObjectAction.Create)
//                        {
//                            EntityOperationResult entityOR = entityOperationResults[entity.Id];

//                            Permission permisson = permissionBL.GetValueBasedEntityPermission(entity, entityOR, _entityManager, callerContext);

//                            if (!(permisson != null && permisson.PermissionSet != null && permisson.PermissionSet.Count > 0 && permisson.PermissionSet.Contains(UserAction.Update)))
//                            {
//                                diagnosticActivity.LogError(String.Format("User '{0}' is not having edit / view permission for entity '{1}'. Please verify AVS settings for user.", loginUser, entity.LongName));

//                                //If permission set is having only 'View' permission then we need to convert the added Info message into error as entities with View permissions cannot be processed.
//                                //So, checking whether any Info message is there
//                                if (entityOR.HasInformation)
//                                {
//                                    Information information = entityOR.Informations.FirstOrDefault();

//                                    if (information != null)
//                                    {
//                                        entityOR.AddOperationResult(information.InformationCode, information.InformationMessage, OperationResultType.Error);

//                                        //Clear Info messages
//                                        entityOR.Informations.Clear();
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    //Refresh operation result status
//                    entityOperationResults.RefreshOperationResultStatus();

//                    continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, null, entityOperationResults, callerContext);

//                    if (!continueProcess)
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = "AVS failed for all the entities came for processing. Entity processing would be terminated now.";
//                            diagnosticActivity.LogInformation(traceMessage);
//                        }

//                        return entityOperationResults;
//                    }

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = "AVS security validated";
//                        diagnosticActivity.LogDurationInfo(traceMessage);
//                    }
//                }
//                else
//                {
//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = "Skipping AVS security validation as applyAVS flag is set to 'false";
//                        diagnosticActivity.LogDurationInfo(traceMessage);
//                    }
//                }

//                #endregion

//                #region Step : Populate Original Entity objects

//                if (!isEntityRevalidateModeEnabled && !entityProcessingOptions.ProcessOnlyEntities && !entityProcessingOptions.ProcessOnlyRelationships)
//                {
//                    LoadOriginalEntitiesInBulk(entities, entityProcessingOptions, allLocalesAcrossEntities, callerContext);
//                }

//                #endregion

//                #region Step : Cleanse entities

//                if (!entityProcessingOptions.ProcessOnlyEntities && !entityProcessingOptions.ProcessOnlyRelationships)
//                {
//                    foreach (Entity entity in entities)
//                    {
//                        EntityOperationsCommonUtility.CleanseEntity(entity);
//                    }
//                }

//                #region Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    traceMessage = "Entity cleansing process completed";
//                    diagnosticActivity.LogDurationInfo(traceMessage);
//                }

//                #endregion

//                #endregion

//                #region Step : Compare, Merge and Calculate Actions

//                if (!isEntityRevalidateModeEnabled && !entityProcessingOptions.ProcessOnlyEntities && !entityProcessingOptions.ProcessOnlyRelationships)
//                {
//                    EntityCompareAndMergeHelper.CompareAndMerge(entities, entityOperationResults, entityProcessingOptions, callerContext);
//                    EntityOperationsCommonUtility.CalculateEntityAction(entities, entityProcessingOptions);
//                }

//                #region Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    traceMessage = "Entity comparison and action population process completed";
//                    diagnosticActivity.LogDurationInfo(traceMessage);
//                }

//                #endregion

//                #endregion

//                #region Step : Generate EntityGuid for the newly created entities

//                foreach (Entity entity in entities.Where(entity => entity.Action == ObjectAction.Create && !entity.EntityGuid.HasValue))
//                {
//                    entity.EntityGuid = Guid.NewGuid();
//                }

//                if (_currentTraceSettings.IsBasicTracingEnabled && entities.Any(entity => !entity.EntityGuid.HasValue))
//                {
//                    diagnosticActivity.LogWarning("Some entities are missing Id values and couldn't have been populated with GetEntityGuidsMap method.");
//                }

//                #endregion

//                #region Step : Create separate buckets for entities to be created and entities to be updated/deleted

//                EntityCollection entitiesForCreate = new EntityCollection();
//                EntityCollection entitiesForUpdateAndDelete = new EntityCollection();
//                EntityCollection entitiesForDelete = new EntityCollection();
//                EntityCollection entitiesWithReadAction = new EntityCollection();

//                foreach (Entity entity in entities)
//                {
//                    PopulateBucketsByEntityAction(entity, entitiesForCreate, entitiesForUpdateAndDelete, entitiesForDelete, entitiesWithReadAction);
//                }

//                RemoveEntitiesWithReadAction(entitiesWithReadAction, entities, allEntities, entityOperationResults, diagnosticActivity);

//                #region Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    traceMessage = String.Format("Created separate buckets for entities to be created and updated. Total entities to create: {0}, Total entities to update / delete: {1}", entitiesForCreate.Count, entitiesForUpdateAndDelete.Count);
//                    diagnosticActivity.LogDurationInfo(traceMessage);
//                }

//                #endregion

//                #endregion

//                #region Step : Return call if all entities has action = Read

//                if (entities.Count < 1 && allEntities.Count < 1)
//                {
//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = "None of entities have action set to 'Create/Update/Delete'. Entity processing would terminate now";
//                        diagnosticActivity.LogWarning(traceMessage);
//                    }

//                    return entityOperationResults;
//                }

//                #endregion

//                #region Step : Validate references For Entity Delete

//                //Before deleting Entities check whether the entities have children or not.
//                //If category then will check the mapping as well. If there is a children or mapping present should not delete Entities.
//                if (entityProcessingOptions.ValidateEntities && entitiesForDelete != null && entitiesForDelete.Count > 0)
//                {
//                    entityValidationManager.ValidateEntitiesReferencesForDelete(entitiesForDelete, entityOperationResults, callerContext);

//                    continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, allEntities, entityOperationResults, callerContext);

//                    #region Diagnostics and tracing

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = String.Format("Reference check completed for entities marked for 'Delete'");
//                        diagnosticActivity.LogDurationInfo(traceMessage);
//                    }

//                    #endregion

//                    if (!continueProcess)
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = "None of entities now qualified for processing. Entity processing would terminate now";
//                            diagnosticActivity.LogWarning(traceMessage);
//                        }

//                        return entityOperationResults;
//                    }
//                }

//                #endregion

//                #region Step : Verify The action Flags mentioned in import Profile and decide correct actions for attribute

//                if (entities != null && entities.Count > 0)
//                {
//                    if (entityProcessingOptions.AttributeProcessingOptionCollection != null)
//                    {
//                        CheckAttributeOptions(entities, entityProcessingOptions);

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Completed validation of attribute actions as per import profile's allowed actions setup");
//                            diagnosticActivity.LogDurationInfo(traceMessage);
//                        }
//                    }
//                        }

//                #endregion

//                #region Step : Verify if all entities are sibling and having exact same attributes to update(This happens in EH generation scenario)

//                Boolean allSiblingEntitiesWithSameAttributes = EntityOperationsCommonUtility.IsAllSiblingEntitiesWithSameAttributes(entities, MDMTraceSource.EntityProcess);

//                Entity firstEntity = (entities.Count > 0) ? entities.ElementAt(0) : null;

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    traceMessage = String.Format("Identified if all entities to be processed are siblings entities(belongs to same parent entity). Sibling verification status is: {0}", allSiblingEntitiesWithSameAttributes);
//                    diagnosticActivity.LogDurationInfo(traceMessage);
//                }

//                #endregion

//                #region Step : Load attribute models

//                DiagnosticActivity loadAttributeModelsActivity = null;

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    loadAttributeModelsActivity = new DiagnosticActivity(null, "LoadAttributesModel SubActivity");
//                    loadAttributeModelsActivity.Start();
//                }

//                try
//                {
//                    if (!entityProcessingOptions.ProcessOnlyEntities && !entityProcessingOptions.ProcessOnlyRelationships)
//                {
//                        AttributeModelCollection attributeModels;

//                        if (allSiblingEntitiesWithSameAttributes && firstEntity != null)
//                        {
//                            attributeModels = EntityAttributeModelHelper.GetAttributeModels(firstEntity.Id, firstEntity.EntityContext, firstEntity.EntityContext.DataLocales, false, false, firstEntity, null, _currentTraceSettings, entityProcessingOptions.ApplyDMS);

//                            foreach (Entity entity in entities)
//                            {
//                                var doesEntityHasExistingAttributeModels = entity.AttributeModels != null && entity.AttributeModels.Count > 0;

//                                foreach (AttributeModel attrModel in attributeModels)
//                                {
//                                    if ((!doesEntityHasExistingAttributeModels) || (!entity.AttributeModels.Contains(attrModel.Id, attrModel.Locale)))
//                                    {
//                                        entity.AttributeModels.Add(attrModel, true);
//                                    }
//                                }
//                            }
//                        }
//                        else
//                        {
//                            foreach (Entity entity in entities)
//                            {
//                                attributeModels = EntityAttributeModelHelper.GetAttributeModels(entity.Id, entity.EntityContext, entity.EntityContext.DataLocales, false, false, entity, null, _currentTraceSettings, entityProcessingOptions.ApplyDMS);

//                                if (entity.AttributeModels != null && entity.AttributeModels.Count > 0)
//                                {
//                                    foreach (AttributeModel attrModel in attributeModels)
//                                    {
//                                        if (!entity.AttributeModels.Contains(attrModel.Id, attrModel.Locale))
//                                        {
//                                            entity.AttributeModels.Add(attrModel, true);
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    entity.AttributeModels = attributeModels;
//                                }
//                            }
//                        }
//                    }
//                }
//                finally
//                {
//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        loadAttributeModelsActivity.Stop();
//                    }
//                }

//                #endregion

//                #region Step : Populate default attribute values for new entities

//                if (entityProcessingOptions.ProcessDefaultValues)
//                {
//                    DiagnosticActivity populateDefaultValuesActivity = null;

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        populateDefaultValuesActivity = new DiagnosticActivity(null, "PopulateDefaultValuesForNewEntities SubActivity");
//                        populateDefaultValuesActivity.Start();
//                    }

//                    try
//                    {
//                        EntityAttributeDefaultValueHelper.PopulateDefaultAttributeValues(entitiesForCreate, entityOperationResults, allSiblingEntitiesWithSameAttributes, firstEntity, _securityPrincipal.CurrentUserName);

//                        EntityAttributeDefaultValueHelper.PopulateEmptyUOMsForNonEmptyAttributesByDefaultUOMs(entitiesForUpdateAndDelete);

//                        //Populate default values for relationship attributes
//                        relationshipManager.PopulateDefaultAttributeValues(entities, callerContext);

//                        entityProcessingOptions.ProcessDefaultValues = false;
//                    }
//                    finally
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            populateDefaultValuesActivity.Stop();
//                        }
//                    }
//                }

//                #endregion

//                #region Step : Fill Entity

//                if (!entityProcessingOptions.ProcessOnlyEntities)
//                {
//                    DiagnosticActivity entityFillProcessActivity = null;

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        entityFillProcessActivity = new DiagnosticActivity(null, "Entity fill process");
//                        entityFillProcessActivity.Start();
//                    }

//                    try
//                    {
//                        EntityGetOptions.EntityFillOptions entityFillOptions = new EntityGetOptions.EntityFillOptions();
//                        entityFillOptions.FillLookupRowWithValues = false; // This is to maintain 7.6 RA behavior. We are yet to decide if we need to open up lookup rows for BRs

//                        EntityFillHelper.FillEntities(entities, entityFillOptions, _entityManager, callerContext);

//                        if (isSourceProcessingEnabled)
//                        {
//                            EntityFillHelper.FillEntitiesSources(entities, entityProcessingOptions, callerContext);
//                            }
//                        }
//                    finally
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            entityFillProcessActivity.Stop();
//                        }
//                    }
//                }

//                #endregion

//                if (entityProcessingOptions.PublishEvents || isEntityRevalidateModeEnabled)
//                {
//                    #region Step : Load Entity Context for Pre load

//                    mdmRuleParams = new MDMRuleParams()
//                    {
//                        Entities = entities,
//                        EntityOperationResults = entityOperationResults,
//                        UserSecurityPrincipal = _securityPrincipal,
//                        CallerContext = callerContext,
//                        DDGCallerModule = (callerContext.Module == MDMCenterModules.Revalidate) ? DDGCallerModule.Revalidate : DDGCallerModule.EntityProcess,
//                        EntityProcessingOptions = entityProcessingOptions
//                    };

//                    EntityContext preLoadEntityContext = PreLoadContextHelper.GetEntityContext(mdmRuleParams, _entityManager);

//                    #endregion Step : Load Entity Context for Pre load

//                    #region Step : Ensure entities data for given entity context

//                    var ensureEntityDataManager = new EnsureEntityDataManager(_entityManager);
//                    ensureEntityDataManager.EnsureEntityData(entities, preLoadEntityContext, callerContext);

//                    //Refresh operation results schema,once pre-load is done.
//                    entityOperationResults.RefreshOperationResultsSchema(entities);

//                    #endregion Step : Ensure entities data for given entity context
//                }

//                #region Step : Data validations

//                DiagnosticActivity validationProcessActivity = null;

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    validationProcessActivity = new DiagnosticActivity(null, "Entity data validation process");
//                    validationProcessActivity.Start();
//                }

//                try
//                {
//                    if (entityProcessingOptions.ValidateEntities && validate)
//                    {
//                        entityValidationManager.Validate(entities, entityOperationResults, callerContext, true, entityProcessingOptions);

//                        //Call custom validations if defined and publish events flag is turned on..
//                        continueProcess = entityProcessingOptions.PublishEvents
//                                              ? EntityOperationsCommonUtility.FireEntityEvent(entities, entities, allEntities,
//                                               MDMEvent.EntityValidate, _entityManager, mdmRuleParams)
//                            : EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, allEntities, entityOperationResults, callerContext);

//                        if (!continueProcess)
//                        {
//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = "None of entities have passed validation process. Entity processing would terminate now";
//                                validationProcessActivity.LogWarning(traceMessage);
//                            }

//                            return entityOperationResults;
//                        }
//                    }
//                    else
//                    {
//                        // if Entity process validation is false then also reclassification should have to be done 
//                        // As we are assigning target category Id only while validation reclassification was not happening if entity process validation is false
//                        // So entities should have to be filled using EntityMoveContext for reclassification
//                        EntityFillHelper.FillEntitiesUsingEntityMoveContext(entities);
//                    }
//                }
//                finally
//                {
//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        validationProcessActivity.Stop();
//                    }
//                }

//                #endregion

//                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(entityProcessingOptions.ProcessingMode)))
//                {
//                    #region Step : Diagnostics and tracing

//                    DurationHelper entityProcessTransactionDuration = null;

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        entityProcessTransactionDuration = new DurationHelper(DateTime.Now);
//                    }

//                    #endregion

//                    try
//                    {
//                        #region Step : Trigger Pre Process events

//                        if (entityProcessingOptions.PublishEvents)
//                        {
//                            #region Step : Trigger creating events

//                            if (entitiesForCreate != null && entitiesForCreate.Count > 0)
//                            {
//                                // do we have attributes
//                                EntityCollection newEntitiesWithAttributes = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "Attributes");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithAttributes, entitiesForCreate, entities, MDMEvent.EntityAttributesCreating, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have relationship
//                                EntityCollection newEntitiesWithRelationShips = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "RelationshipsCreate");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithRelationShips, entitiesForCreate, entities, MDMEvent.EntityRelationShipsCreating, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have hierarchy
//                                EntityCollection newEntitiesWithHierarchy = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "Hierarchy");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithHierarchy, entitiesForCreate, entities, MDMEvent.EntityHierarchyCreating, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have extensions
//                                EntityCollection newEntitiesWithExtensions = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "Extensions");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithExtensions, entitiesForCreate, entities, MDMEvent.EntityExtensionsCreating, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // entity level
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForCreate, entitiesForCreate, entities, MDMEvent.EntityCreating, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }
//                            }

//                            #endregion Step : Trigger creating events

//                            #region Step : Trigger updating events

//                            if (entitiesForUpdateAndDelete != null && entitiesForUpdateAndDelete.Count > 0)
//                            {
//                                EntityCollection reclassificationEntities = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Reclassify");

//                                    if (reclassificationEntities != null && reclassificationEntities.Count > 0)
//                                    {
//                                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                                        {
//                                        traceMessage = String.Format("Loading target category's attributes for entities being reclassified...");
//                                            diagnosticActivity.LogInformation(traceMessage);
//                                        }

//                                        new EntityReclassificationManager().LoadTargetCategoryAttributesForReclasification(reclassificationEntities, entityProcessingOptions, callerContext.Application, callerContext.Module);

//                                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                                        {
//                                        traceMessage = String.Format("Completed target category's attributes load for entities being reclassified");
//                                            diagnosticActivity.LogInformation(traceMessage);
//                                        }

//                                    if (!EntityOperationsCommonUtility.FireEntityEvent(reclassificationEntities, entitiesForUpdateAndDelete, entities, MDMEvent.EntityReclassifying, _entityManager, mdmRuleParams))
//                                        {
//                                            return entityOperationResults;
//                                        }
//                                    }

//                                    // do we have attributes
//                                EntityCollection updateEntitiesWithAttributes = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Attributes");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithAttributes, entitiesForUpdateAndDelete, entities, MDMEvent.EntityAttributesUpdating, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }

//                                    // do we have relationship being created
//                                EntityCollection updateEntitiesWithNewRelationShips = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "RelationshipsCreate");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithNewRelationShips, entitiesForUpdateAndDelete, entities, MDMEvent.EntityRelationShipsCreating, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }

//                                    // do we have relationship being updated
//                                EntityCollection updateEntitiesWithRelationShips = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "RelationshipsUpdateOrDelete");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithRelationShips, entitiesForUpdateAndDelete, entities, MDMEvent.EntityRelationShipsUpdating, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }

//                                    // do we have hierarchy
//                                EntityCollection updateEntitiesWithHierarchy = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Hierarchy");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithHierarchy, entitiesForUpdateAndDelete, entities, MDMEvent.EntityHierarchyUpdating, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }

//                                    // do we have child hierarchy changed?
//                                EntityCollection updateEntitiesWithChildHierarchyChange = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "ChildHierarchyChange");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithChildHierarchyChange, entitiesForUpdateAndDelete, entities, MDMEvent.EntityHierarchyChanged, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }

//                                    // do we have extensions
//                                EntityCollection updateEntitiesWithExtensions = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Extensions");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithExtensions, entitiesForUpdateAndDelete, entities, MDMEvent.EntityExtensionsUpdating, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }

//                                    // do we have child extension changed?
//                                EntityCollection updateEntitiesWithChildExtensionsChange = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "ChildExtensionsChange");
//                                if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithChildExtensionsChange, entitiesForUpdateAndDelete, entities, MDMEvent.EntityExtensionsChanged, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }

//                                if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForUpdateAndDelete, entitiesForUpdateAndDelete, entities, MDMEvent.EntityUpdating, _entityManager, mdmRuleParams))
//                                    {
//                                        return entityOperationResults;
//                                    }
//                                }

//                            #endregion Step : Trigger updating events
//                            }
//                        else if (isEntityRevalidateModeEnabled)
//                        {
//                            #region Step : Trigger events

//                            // do we have revalidate mode enabled
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForUpdateAndDelete, entitiesForUpdateAndDelete, entities, MDMEvent.UnKnown, _entityManager, mdmRuleParams))
//                            {
//                                return entityOperationResults;
//                            }

//                            #endregion Step : Trigger events
//                        }

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Entity pre-process events execution completed");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }

//                        #endregion

//                        #region Step : EH Generation - unique entity name updation

//                        //TODO - Needs to bind it properly with DDG once proper DDG Keyword is getting generated.

//                        if (entitiesForCreate != null && entitiesForCreate.Count > 0)
//                        {
//                            foreach (Entity entity in entitiesForCreate)
//                            {
//                                EntityOperationResult entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(entity.Id);

//                                GenerateChildEntityName(entity, entityOperationResult);
//                            }
//                        }

//                        #endregion Step : EH Generation - unique entity name updateion

//                        #region Step : Re-Populate ids after pre-process events

//                        if (!entityProcessingOptions.ProcessOnlyEntities)
//                        {
//                            EntityGetOptions.EntityFillOptions entityFillOptions = new EntityGetOptions.EntityFillOptions();

//                            entityFillOptions.FillCategoryInfo = false;
//                            entityFillOptions.FillContainerAndOrganizationInfo = false;
//                            entityFillOptions.FillLookupDisplayValues = false;
//                            entityFillOptions.FillEntityTypeInfo = false;
//                            entityFillOptions.FillLookupRowWithValues = false;
//                            entityFillOptions.FillParentExtensionInfo = false;
//                            entityFillOptions.FillParentInfo = false;
//                            entityFillOptions.FillRelationshipProperties = false;
//                            entityFillOptions.FillEntityProperties = false;

//                            entityFillOptions.FillUOMValues = true;

//                            //BR might have changed attribute values...please do Fill Entities again...
//                            EntityFillHelper.FillEntities(entities, entityFillOptions, _entityManager, callerContext);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Re-populated ids after pre-process events completed");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }
//                        }

//                        #endregion Step : Re-Populate ids after pre-process events

//                        #region Step : Compare and merge once again after pre-process events

//                        if (entityProcessingOptions.PublishEvents || isEntityRevalidateModeEnabled)
//                        {
//                            //BR might have changed attribute values...please do compare and merge..
//                            EntityCompareAndMergeHelper.CompareAndMerge(entities, entityOperationResults, entityProcessingOptions, callerContext);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Entity actions re-calculated after pre-process events");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }
//                        }

//                        #endregion Step : Compare and merge once again after pre-process events

//                        #region Step: Calculate entity actions and filter out entities with Read action

//                        if (entityProcessingOptions.PublishEvents || entityProcessingOptions.ValidateEntities)
//                        {
//                            //Reasons to do this filtering -
//                            //- On PublishEvents, events could modify entity data and hence re-calculation of actions could results into 
//                            //  no change for an entity.
//                            //- On ValidateEntities, invalid entity data gets removed in case of Import with ValidationLevel as 'Warning' 
//                            //  which could results into no change for an entity.

//                            EntityOperationsCommonUtility.CalculateEntityAction(entities, entityProcessingOptions);

//                            entitiesWithReadAction = new EntityCollection();

//                            foreach (Entity entity in entities)
//                            {
//                                if (entity.Action == ObjectAction.Read)
//                                {
//                                    entitiesWithReadAction.Add(entity);
//                                }
//                            }

//                            RemoveEntitiesWithReadAction(entitiesWithReadAction, entities, allEntities, entityOperationResults, diagnosticActivity);
//                        }

//                        #endregion Step: Calculate entity actions and filter out entities with Read action

//                        #region Step : Data re-validations after pre-process events

//                        if (revalidate)
//                            {
//                            DiagnosticActivity revalidationProcessActivity = null;

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                revalidationProcessActivity = new DiagnosticActivity(null, "Entity data re-validation process after pre-process events");
//                                revalidationProcessActivity.Start();
//                            }

//                            try
//                            {
//                                if (entityProcessingOptions.ValidateEntities &&
//                                   (entityProcessingOptions.PublishEvents || isEntityRevalidateModeEnabled) &&
//                                    mdmRuleParams.QualifiedRuleMaps.Count > 0)
//                                {
//                                    entityValidationManager.Validate(entities, entityOperationResults, callerContext, true, entityProcessingOptions);

//                                    continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, allEntities, entityOperationResults, callerContext);

//                                    if (!continueProcess)
//                                    {
//                                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                                        {
//                                            traceMessage = "None of entities have passed re-validation process. Entity processing would terminate now";
//                                            revalidationProcessActivity.LogWarning(traceMessage);
//                                    }

//                                        return entityOperationResults;
//                                }
//                                }
//                                else
//                                {
//                                    // if Entity process validation is false then also reclassification should have to be done 
//                                    // As we are assigning target category Id only while validation reclassification was not happening if entity process validation is false
//                                    // So entities should have to be filled using EntityMoveContext for reclassification
//                                    EntityFillHelper.FillEntitiesUsingEntityMoveContext(entities);
//                                }
//                            }
//                            finally
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    revalidationProcessActivity.Stop();
//                                }
//                            }
//                        }
//                        else if (entityProcessingOptions.PublishEvents)
//                        {
//                            DiagnosticActivity revalidationProcessActivity = null;

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                revalidationProcessActivity = new DiagnosticActivity(null, "Entity data re-validation process after pre-process events");
//                                revalidationProcessActivity.Start();
//                            }

//                            try
//                                {
//                                if (entityProcessingOptions.ValidateEntities && mdmRuleParams.QualifiedRuleMaps.Count > 0)
//                                    {
//                                    foreach (Entity entity in entities)
//                                    {
//                                        if (entity.Action == ObjectAction.Unknown)
//                                        {
//                                            //Get entity operation result
//                                            EntityOperationResult entityOperationResult = null;
//                                            entity.Action = ObjectAction.Create;

//                                            try
//                                            {
//                                                entityOperationResult = (EntityOperationResult)entityOperationResults.GetByReferenceId(entity.ReferenceId);
//                                                entityValidationManager.ValidateDuplicateEntity(entity, entityOperationResult, callerContext);
//                                    }
//                                            catch (InvalidOperationException)
//                                            {
//                                                _localeMessage = _localeMessageBL.Get(systemUILocale, "111800", new object[] { entity.Id, entity.ExternalId }, false, callerContext);

//                                                throw new MDMOperationException("111800", _localeMessage.Message, "EntityManager", String.Empty, "Get", ReasonType.ApplicationError, -1); //More than one operation results found for Entity - Id: {0} ExternalId: {1}
//                                }
//                                        }
//                                    }

//                                    continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, allEntities, entityOperationResults, callerContext);

//                                    if (!continueProcess)
//                                    {
//                                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                                        {
//                                            traceMessage = "None of entities have passed re-validation process. Entity processing would terminate now";
//                                            revalidationProcessActivity.LogWarning(traceMessage);
//                            }

//                                        return entityOperationResults;
//                        }
//                                }
//                            }
//                            finally
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    revalidationProcessActivity.Stop();
//                                }
//                            }
//                        }

//                        #endregion Step : Data re-validations after pre-process events

//                        #region Step : Populate Audit Info System Attributes

//                        if (populateAuditInfoSystemAttributes)
//                        {
//                            EntityFillHelper.FillAuditInfoSystemAttributes(entities, loginUser, callerContext.ProgramName, populateProgramNameSystemAttribute, systemDataLocale);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Audit Info System attributes populated");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }
//                        }

//                        #endregion

//                        #region Step : Entity Identification after Pre Process

//                        if (isEntityUniqueIdentificationConfigEnabled &&
//                            entityProcessingOptions.ProcessingMode != ProcessingMode.AsyncCreate &&
//                            entityProcessingOptions.ProcessingMode != ProcessingMode.AsyncUpdate &&
//                            entityProcessingOptions.ProcessingMode != ProcessingMode.AsyncDelete)
//                        {

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = "MDMCenter.EntityIdentificationConfiguration.Enabled is true, entering Process Entity Identification after BR execution";
//                                diagnosticActivity.LogInformation(traceMessage);
//                            }

//                            //Entity uniqueness check will be performed only if the (module is Import) OR (entity action = create)
//                            if (entitiesForUniqueCheck != null && entitiesForUniqueCheck.Count > 0)
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    traceMessage = String.Format("Entity uniqueness check after BR execution will be performed for {0} entities out of {1} entities received for processing.", entitiesForUniqueCheck.Count, entities.Count);
//                                    diagnosticActivity.LogInformation(traceMessage);
//                                }
//                                EntityIdentificationManager.ProcessEntityIdentificationData(entitiesForUniqueCheck, entityOperationResults, callerContext, false);

//                                entityOperationResults.RefreshOperationResultStatus();
//                                //Passing all the entities for the case where the passed in entity collection as mix and match of create and update actions and module is not imports
//                                continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, allEntities, entityOperationResults, callerContext);

//                                if (!continueProcess)
//                                {
//                                    return entityOperationResults;
//                                }
//                            }
//                            }

//                        #endregion Step : Entity Identification after Pre Process

//                        #region Step : Perform entity updates in database

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Starting entity database process...");
//                            diagnosticActivity.LogInformation(traceMessage);
//                        }

//                        if (entities.Count > 0)
//                        {
//                            EntityDA entityDA = new EntityDA();
//                            entityDA.Process(entities, entityProcessingOptions, loginUser, GlobalizationHelper.GetSystemDataLocale(), callerContext.ProgramName, entityOperationResults, command, callerContext, true, updateAttributeResults || isSourceProcessingEnabled);

//                            Boolean canContinue = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entities, allEntities, entityOperationResults, callerContext);

//                            if (!canContinue)
//                            {
//                                return entityOperationResults;
//                            }
//                        }
//                        else
//                        {
//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Entity pre-processing has removed all the entities from process. Skipping EntityDA call, please check business rules if this is not expected.");
//                                diagnosticActivity.LogWarning(traceMessage);
//                            }
//                        }

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Entity database process completed");
//                            diagnosticActivity.LogDurationInfo(traceMessage);
//                            }

//                        #endregion

//                        #region Step : Set AuditInfo System Attributes Action to 'Read'

//                        if (populateAuditInfoSystemAttributes)
//                        {
//                            EntityFillHelper.SetAuditInfoSystemAttributeAction(entities, ObjectAction.Read, populateProgramNameSystemAttribute, systemDataLocale);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Updated audit info system attributes action to Read");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }
//                        }

//                        #endregion

//                        #region Step : Perform entity reclassification for children

//                        EntityCollection entitiesForReclassificationOrRename = FilterEntitiesByType(entities, entityOperationResults, "ReclassifyOrRename");

//                        if (entitiesForReclassificationOrRename != null && entitiesForReclassificationOrRename.Count > 0)
//                        {
//                            //Reclassification of Children of entities who were successfully reclassified. Any errors in entityForReclass
//                            new EntityReclassificationManager().ReclassifyChildren(entitiesForReclassificationOrRename, entityOperationResults, renamedEntityIdList, isCacheEnabled, loginUser, callerContext.ProgramName, callerContext, entityProcessingOptions.ProcessingMode);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Entity children reclassification completed");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }
//                            }

//                        #endregion Reclassification of EntityChildren

//                        #region Step : Perform relationships and extension relationships updates

//                        if (!entityProcessingOptions.ProcessOnlyEntities && entities.Count > 0)
//                        {
//                            #region Process Relationships

//                            EntityCollection entitiesWithRelationships = FilterEntitiesByType(entities, entityOperationResults, "RelationshipsChange");

//                            if (entitiesWithRelationships != null && entitiesWithRelationships.Count > 0)
//                            {
//                                relationshipManager.Process(entities, entityOperationResults, entityCacheStatusCollection, callerContext, entityProcessingOptions, null);
//                            }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    traceMessage = String.Format("None of entity have relationships to process. Skipping relationship processing");
//                                    diagnosticActivity.LogDurationInfo(traceMessage);
//                                }
//                                }

//                            #endregion Process Relationships

//                            #region Process Extensions

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Starting entity extension relationship process...");
//                                diagnosticActivity.LogInformation(traceMessage);
//                            }

//                            if (!entityProcessingOptions.ProcessOnlyRelationships)
//                            {
//                                EntityOperationResultCollection extensionProcessResults = extensionRelationshipBL.Process(entities, entityCacheStatusCollection, callerContext.ProgramName, callerContext.Application, callerContext.Module, entityProcessingOptions.ProcessingMode, false);

//                                //Failure in the extension process will be added as a warning and will not effect the operation status of the entity
//                                if (extensionProcessResults.OperationResultStatus == OperationResultStatusEnum.Failed || extensionProcessResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
//                                {
//                                    foreach (EntityOperationResult extensionOR in extensionProcessResults)
//                                    {
//                                        if (extensionOR.HasError)
//                                        {
//                                            entityOperationResults.AddEntityOperationResult(extensionOR.EntityId, String.Empty, extensionOR.Errors[0].ErrorMessage, OperationResultType.Warning);
//                                            break;
//                                        }
//                                    }
//                                }

//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    traceMessage = String.Format("Entity extension relationship process completed");
//                                    diagnosticActivity.LogDurationInfo(traceMessage);
//                                }
//                            }

//                            #endregion
//                        }

//                        #endregion

//                        #region Step : Perform entity state validation calculation and updates in database

//                        if (allEntities != null && allEntities.Count > 0)
//                        {
//                            EntityStateValidationBL entityStateValidationBL = new EntityStateValidationBL();
//                            entityStateValidationBL.Process(allEntities, entityOperationResults, callerContext);
//                        }

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Entity validation states processing completed");
//                            diagnosticActivity.LogDurationInfo(traceMessage);
//                        }

//                        #endregion Step : Perform entity state validation calculation and updates in database

//                        #region Step : Process Sources

//                        if (isSourceProcessingEnabled && entityProcessingOptions.ProcessSources && entities.Count > 0)
//                        {
//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Starting entity sources process...");
//                                diagnosticActivity.LogInformation(traceMessage);
//                            }

//                            SourceValueBL sourceValueBL = new SourceValueBL();
//                            sourceValueBL.Process(entities, callerContext);

//                            if (entities != null && entities.Count > 0)
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    traceMessage = String.Format("Entity sources process completed");
//                                    diagnosticActivity.LogDurationInfo(traceMessage);
//                                }
//                                }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    traceMessage = String.Format("None of entity have sources to process. Skipping entity sources processing");
//                                    diagnosticActivity.LogDurationInfo(traceMessage);
//                                }
//                                }
//                            }

//                        #endregion Process Relationships

//                        #region Step : Perform entity re-parenting

//                        foreach (Entity entity in entitiesForUpdateAndDelete)
//                        {
//                            //If OriginalEntity ParentExtensionEntityId and TargetParentExtensionEntityId both are same then continue.
//                            if (entity.OriginalEntity != null && entity.OriginalEntity.ParentExtensionEntityId == entity.EntityMoveContext.TargetParentExtensionEntityId)
//                                continue;

//                            //Import Engine populates only EntityMoveContext but not Entity.Action as 'Re-parent'.
//                            if (entity.EntityMoveContext.TargetParentExtensionEntityId > 0 && entity.EntityMoveContext.ReParentType == ReParentTypeEnum.ExtensionReParent)
//                            {
//                                entity.Action = ObjectAction.ReParent;
//                            }

//                            if (entity.Action == ObjectAction.ReParent)
//                            {
//                                if (entity.EntityMoveContext.ReParentType == ReParentTypeEnum.ExtensionReParent)
//                                {
//                                    Boolean reparentSuccess = extensionRelationshipBL.ReParent(entity.EntityMoveContext.TargetParentExtensionEntityId, entity.Id, callerContext, entityProcessingOptions.ProcessingMode);

//                                    if (!reparentSuccess)
//                                    {
//                                        //TODO:: Register this entity as error in EORC
//                                    }
//                                }
//                            }
//                        }

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Entity re-parenting process completed");
//                            diagnosticActivity.LogDurationInfo(traceMessage);
//                        }

//                        #endregion

//                        #region Step : Refresh ChangeContext, entitiesForCreate and entitiesForUpdate after process

//                        entitiesForCreate = new EntityCollection();
//                        entitiesForUpdateAndDelete = new EntityCollection();
//                        entitiesForDelete = new EntityCollection();
//                        entitiesWithReadAction = new EntityCollection();

//                        foreach (Entity entity in entities)
//                        {
//                            PopulateBucketsByEntityAction(entity, entitiesForCreate, entitiesForUpdateAndDelete, entitiesForDelete, entitiesWithReadAction);

//                            //Refresh EntityChangeContext..
//                            //This is needed to get Object Ids created as a part of process and update Id lists of Change Context
//                            IEntityChangeContext changeContext = entity.GetChangeContext(true);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                diagnosticActivity.LogMessageWithData(String.Format("See 'View Data' for changed identified for entity id: {0}.", entity.Id), changeContext.ToXml());
//                            }
//                        }

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Entity change context is updated");
//                            diagnosticActivity.LogDurationInfo(traceMessage);
//                        }

//                        #endregion Refresh buckets entitiesForCreate and entitiesForUpdate after process for triggering  post event businessRules

//                        #region Step : Update Entity buffer cache

//                        var entityCacheStatusUpdateActivity = new DiagnosticActivity(null, "UpdateCurrentEntityCache SubActivity");

//                        try
//                        {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                                entityCacheStatusUpdateActivity.Start();
//                        }

//                        //No need to add/update the entity case of initial load, so updating cache only when process only entities value is false.
//                        //In case of initial load, process only entities value is true. In other case, it is false.
//                        if (!entityProcessingOptions.ProcessOnlyEntities)
//                        {
//                            EntityCacheHelper.UpdateCurrentEntityCache(entities, callerContext, entityCacheStatusCollection, updateAttributeResults);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                    entityCacheStatusUpdateActivity.LogDurationInfo("Entity cache status update object is updated");
//                            }
//                            }
//                        else
//                        {
//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                    entityCacheStatusUpdateActivity.LogDurationInfo("Entity cache is disabled. No entity cache update would be performed.");
//                        }
//                        }
//                        }
//                        finally
//                        {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                                entityCacheStatusUpdateActivity.Stop();
//                            }
//                        }

//                        #endregion

//                        #region Step : Non Initial Load Activities

//                        isInitialLoad = (entityProcessingOptions.ImportMode == ImportMode.InitialLoad || entityProcessingOptions.ImportMode == ImportMode.RelationshipInitialLoad);

//                        //When the entity process is called from import for initial load, that does the search population and no family queue population is required.
//                        if (!isInitialLoad)
//                        {
//                        #region Step : Create Entity Activity Logs

//                            EntityActivityLogBL entityActivityLogManager = new EntityActivityLogBL();
//                            entityActivityLogManager.Process(entities, entityProcessingOptions, callerContext, isRelationshipInheritanceEnabled);

//                        #endregion

//                            #region Step : Create Entity Family Queue Log

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Starting entity family queue log creation process...");
//                                diagnosticActivity.LogInformation(traceMessage);
//                        }

//                            EntityFamilyChangeContextCollection entityFamilyChangeContexts = EntityChangeContextHelper.GetEntityFamilyChangeContexts(entities, callerContext);

//                            if (entityFamilyChangeContexts != null && entityFamilyChangeContexts.Count > 0)
//                            {
//                                EntityFamilyQueueCollection entityFamilyQueues = GetEntityFamilyQueues(entityFamilyChangeContexts);

//                                EntityFamilyQueueBL entityFamilyQueueBL = new EntityFamilyQueueBL();
//                                OperationResultCollection operationResults = entityFamilyQueueBL.Process(entityFamilyQueues, callerContext);

//                                if (operationResults != null && operationResults.OperationResultStatus != OperationResultStatusEnum.Successful &&
//                                    operationResults.OperationResultStatus != OperationResultStatusEnum.None)
//                                {
//                                    entityOperationResults.CopyOperationResults(operationResults);
//                                    entityOperationResults.RefreshOperationResultStatus();

//                                    if (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed ||
//                                        entityOperationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
//                                    {
//                                        return entityOperationResults;
//                                    }
//                                }
//                            }

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Entity family queue log creation process completed.");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }

//                        #endregion
//                        }

//                        #endregion Step : Non Initial Load Activities

//                        #region Step : Create Integration Activity Log for Translation

//                        if (isTranslationEnabled)
//                        {
//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Starting integration activity log creation process...");
//                                diagnosticActivity.LogInformation(traceMessage);
//                            }

//                            EntityOperationsCommonUtility.EnqueueIntegrationActivity(entities, callerContext);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Integration activity log creation process completed");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }
//                        }

//                        #endregion

//                        #region Step : Update Cache Status

//                        if (isCacheEnabled && entityCacheStatusCollection.IsCacheStatusUpdated())
//                        {
//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Starting entity cache status update database rocess...");
//                                diagnosticActivity.LogInformation(traceMessage);
//                            }

//                            EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
//                            entityCacheStatusBL.Process(entityCacheStatusCollection, callerContext);

//                            if (_currentTraceSettings.IsBasicTracingEnabled)
//                            {
//                                traceMessage = String.Format("Entity cache status update database process completed");
//                                diagnosticActivity.LogDurationInfo(traceMessage);
//                            }
//                        }

//                        #endregion

//                        #region Step : Export Qualification for Category

//                        EntityCollection categoryEntities = (EntityCollection)entities.GetEntitiesByEntityTypeId(Constants.CATEGORY_ENTITYTYPE);

//                        if (categoryEntities != null && categoryEntities.Count > 0)
//                        {
//                            EntityExportProfileBL exportProfileBL = new EntityExportProfileBL();
//                            EntityExportProfileCollection exportProfileCollection = exportProfileBL.Get(ExportProfileType.EntityExportSyndicationProfile, callerContext);

//                            if (exportProfileCollection != null && exportProfileCollection.Count > 0)
//                            {
//                                ExportQualificationHelper qualificationHelper = new ExportQualificationHelper();
//                                ExportQueueCollection exportQueueItems = qualificationHelper.GetExportQueueItemsForCategoryExport(categoryEntities, exportProfileCollection, callerContext);
//                                if (exportQueueItems != null && exportQueueItems.Count > 0)
//                                {
//                                    IExportQueueManager exportQueueManager = ServiceLocator.Current.GetInstance(typeof(IExportQueueManager)) as IExportQueueManager;
//                                    OperationResultCollection queueResults = exportQueueManager.Process(exportQueueItems, false, callerContext);

//                                    if (queueResults.OperationResultStatus != OperationResultStatusEnum.Successful)
//                                    {
//                                        throw new MDMOperationException("Failed to save export qualified items.");
//                                    }
//                                }
//                            }
//                        }

//                        #endregion Step : Export Qualification for Category

//                        #region Step : Commit transaction

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Starting entity process transaction commit...");
//                            diagnosticActivity.LogInformation(traceMessage);
//                        }

//                        //Commit transaction
//                        transactionScope.Complete();

//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            traceMessage = String.Format("Entity process transaction commit completed");
//                            diagnosticActivity.LogDurationInfo(traceMessage);
//                        }

//                        #endregion
//                    }
//                    finally
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            diagnosticActivity.LogDurationInfo("Total Entity Process Transaction Duration", entityProcessTransactionDuration.GetDurationInMilliseconds(DateTime.Now));
//                        }
//                    }
//                        }

//                #region Step : Send entities processed to Jigsaw

//                if (JigsawConstants.IsJigsawIntegrationEnabled && allEntities.Count > 0)
//                {
//                    EntityJigsawIntegrationHelper.SendToJigsaw(allEntities, mdmRuleParams, _securityPrincipal, callerContext);

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        diagnosticActivity.LogDurationInfo("Sending Entities to Jigsaw step completed.");
//                    }
//                }

//                #endregion

//                #region Step : Update entity maps

//                // Entity map cache is updated only after entity process transaction scope is complete
//                EntityMapBL entityMapManager = new EntityMapBL();
//                entityMapManager.UpdateEntityMaps(entities);

//                #endregion

//                #region Step : Trigger EntityUpdated event

//                if (entityProcessingOptions.PublishEvents)
//                {
//                    DurationHelper postProcessEventExecutionDuration = null;

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        postProcessEventExecutionDuration = new DurationHelper(DateTime.Now);
//                    }

//                    try
//                    {
//                        #region Step : Trigger created events

//                        if (entitiesForCreate != null && entitiesForCreate.Count > 0)
//                        {
//                            // do we have attributes
//                            EntityCollection newEntitiesWithAttributes = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "Attributes");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithAttributes, entitiesForCreate, entities, MDMEvent.EntityAttributesCreated, _entityManager, mdmRuleParams))
//                            {
//                                return entityOperationResults;
//                            }

//                            // do we have relationship
//                            EntityCollection newEntitiesWithRelationShip = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "RelationshipsCreate");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithRelationShip, entitiesForCreate, entities, MDMEvent.EntityRelationShipsCreated, _entityManager, mdmRuleParams))
//                            {
//                                return entityOperationResults;
//                            }

//                            // do we have hierarchy
//                            EntityCollection newEntitiesWithHierarchy = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "Hierarchy");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithHierarchy, entitiesForCreate, entities, MDMEvent.EntityHierarchyCreated, _entityManager, mdmRuleParams))
//                            {
//                                return entityOperationResults;
//                            }

//                            // do we have extensions
//                            EntityCollection newEntitiesWithExtensions = FilterEntitiesByType(entitiesForCreate, entityOperationResults, "Extensions");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(newEntitiesWithExtensions, entitiesForCreate, entities, MDMEvent.EntityExtensionsCreated, _entityManager, mdmRuleParams))
//                            {
//                                return entityOperationResults;
//                            }

//                            //entity level
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForCreate, entitiesForCreate, entities, MDMEvent.EntityCreated, _entityManager, mdmRuleParams))
//                            {
//                                return entityOperationResults;
//                            }
//                        }

//                        #endregion

//                        #region Step : Trigger updated events

//                        if (entitiesForUpdateAndDelete != null && entitiesForUpdateAndDelete.Count > 0)
//                        {
//                                //do we have entities for reclassification
//                            EntityCollection reclassificationEntities = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Reclassify");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(reclassificationEntities, entitiesForUpdateAndDelete, entities, MDMEvent.EntityReclassified, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have attributes
//                            EntityCollection updateEntitiesWithAttributes = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Attributes");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithAttributes, entitiesForUpdateAndDelete, entities, MDMEvent.EntityAttributesUpdated, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have relationship being created
//                            EntityCollection updateEntitiesWithNewRelationShips = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "RelationshipsCreate");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithNewRelationShips, entitiesForUpdateAndDelete, entities, MDMEvent.EntityRelationShipsCreated, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have relationship being updated
//                            EntityCollection updateEntitiesWithRelationShips = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "RelationshipsUpdateOrDelete");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithRelationShips, entitiesForUpdateAndDelete, entities, MDMEvent.EntityRelationShipsUpdated, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have hierarchy
//                            EntityCollection updateEntitiesWithHierarchy = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Hierarchy");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithHierarchy, entitiesForUpdateAndDelete, entities, MDMEvent.EntityHierarchyUpdated, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                // do we have extensions
//                            EntityCollection updateEntitiesWithExtensions = FilterEntitiesByType(entitiesForUpdateAndDelete, entityOperationResults, "Extensions");
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(updateEntitiesWithExtensions, entitiesForUpdateAndDelete, entities, MDMEvent.EntityExtensionsUpdated, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }

//                                //entity level
//                            if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForUpdateAndDelete, entitiesForUpdateAndDelete, entities, MDMEvent.EntityUpdated, _entityManager, mdmRuleParams))
//                                {
//                                    return entityOperationResults;
//                                }
//                            }

//                        #endregion
//                }
//                    finally
//                    {
//                        if (_currentTraceSettings.IsBasicTracingEnabled)
//                        {
//                            diagnosticActivity.LogDurationInfo("Total Post Process Event Execution Duration", postProcessEventExecutionDuration.GetDurationInMilliseconds(DateTime.Now));
//                        }
//                    }
//                }

//                #endregion

//                #region Step : Entity Relationship Denorm

//                //And If RelationshipManager.Relationship.Inheritance.Enabled == true, then only refresh Denorm data.
//                if (!isInitialLoad && isRelationshipInheritanceEnabled)
//                    {
//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = String.Format("Starting entity relationship denorm process...");
//                        diagnosticActivity.LogInformation(traceMessage);
//                    }

//                    //Do we have any relationship change
//                    EntityCollection entitiesWithRelationshipChange = FilterEntitiesByType(entities, entityOperationResults, "RelationshipsChange");

//                    if (entitiesWithRelationshipChange != null && entitiesWithRelationshipChange.Count > 0)
//                    {
//                        RelationshipDenormBL relationshipDenormManager = new RelationshipDenormBL();
//                        relationshipDenormManager.ProcessDenormalizedRelationships(entitiesWithRelationshipChange, entityProcessingOptions.ProcessingMode, _entityManager, callerContext);
//                    }

//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = String.Format("Entity relationship denorm process completed");
//                        diagnosticActivity.LogDurationInfo(traceMessage);
//                }
//                }
//                else
//                {
//                    if (_currentTraceSettings.IsBasicTracingEnabled)
//                    {
//                        traceMessage = String.Format("Entity relationship denorm process would be skipped as following flag is set to false or it is part of initial load. Is Relationship Inheritance Enabled: {0}", isRelationshipInheritanceEnabled);
//                        diagnosticActivity.LogInformation(traceMessage);
//                    }
//                }

//                #endregion

//                #region Step : Populate Entity Operation Result status

//                //Previous to Post-Process, possible values for operation status could be CompletedWithErrors or Success. If operation status is 'Failed' then Post-Process would have not been called.
//                //If all entities fails in Post-Process then the operation status becomes Failed.
//                if (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed)
//                {
//                    return entityOperationResults;
//                }

//                EntityOperationsCommonUtility.UpdateEntityOperationResultStatus(entities, entityOperationResults, _operationAction);

//                #endregion

//                #region Step : Diagnostics and tracing

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    traceMessage = String.Format("Entity process completed");
//                    diagnosticActivity.LogInformation(traceMessage);
//                }

//                #endregion
//            }

//                #region Step : Exception Handling

//            catch (TransactionAbortedException ex)
//            {
//                const String exceptionMessageFormat = "Error occurred while processing entity. Check and retry again. If the problem persists, contact your system administrator.  Internal Error: {0}";
//                const String exceptionMessageCode = "111957";
//                HandleException(ex, entities, entityOperationResults, diagnosticActivity, exceptionMessageFormat, exceptionMessageCode, callerContext);
//            }

//            catch (PermissionsNotFoundException ex)
//            {
//                const String exceptionMessageFormat = "Internal Error: {0}";
//                const String exceptionMessageCode = "113994";
//                HandleException(ex, entities, entityOperationResults, diagnosticActivity, exceptionMessageFormat, exceptionMessageCode, callerContext);
//            }

//            catch (Exception ex)
//            {
//                const String exceptionMessageFormat = "Error occurred while processing entity. Check and retry again. If the problem persists, contact your system administrator.  Internal Error: {0}";
//                const String exceptionMessageCode = "111957";
//                HandleException(ex, entities, entityOperationResults, diagnosticActivity, exceptionMessageFormat, exceptionMessageCode, callerContext);
//            }
//            finally
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                    diagnosticActivity.Stop();
//            }

//            #endregion

//            return entityOperationResults;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="templateEntities"></param>
//        /// <param name="entityIdsToProcess"></param>
//        /// <param name="actionPerformed"></param>
//        /// <param name="callerContext"></param>
//        /// <returns></returns>
//        public EntityOperationResultCollection BulkUpdateEntityAttributes(EntityCollection templateEntities, Collection<Int64> entityIdsToProcess, String actionPerformed, CallerContext callerContext)
//        {
//            EntityOperationResultCollection masterEntityOperationResult = new EntityOperationResultCollection();

//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.StartTraceActivity("EntityManager.BulkUpdateEntityAttributes", MDMTraceSource.EntityProcess, false);
//            }

//            try
//            {
//                //Get BaseEntitiies
//                //Clone attributes from template entity to base entities
//                //Process all entities

//                EntityCollection entitiesToProcess = new EntityCollection();
//                DurationHelper durationHelper = new DurationHelper(DateTime.Now);
//                EntityOperationResultCollection batchEntityOperationResult = new EntityOperationResultCollection();

//                if (templateEntities != null && templateEntities.Count > 0)
//                {
//                    if (entityIdsToProcess != null && entityIdsToProcess.Count > 0)
//                    {
//                        Int32 countOfEntitiesToProcess = entityIdsToProcess.Count;

//                        //Prepare entity context...
//                        EntityContext entityContext = new EntityContext();
//                        entityContext.LoadEntityProperties = true;
//                        entityContext.LoadAttributes = false;
//                        entityContext.Locale = GlobalizationHelper.GetSystemDataLocale();

//                        Entity firstEntity = templateEntities.FirstOrDefault();
//                        AttributeCollection attributesToBeUpdated = firstEntity.Attributes;
//                        DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

//                        //Read batch size from MDMCenter.Entity.BulkEdit.BatchSize
//                        Int32 batchSize = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.Entity.BulkEdit.BatchSize", 50);

//                        if (Constants.TRACING_ENABLED)
//                        {
//                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDMCenter.Entity.BulkEdit.BatchSize setting is - '{0}'", batchSize), MDMTraceSource.EntityProcess);
//                        }

//                        if (countOfEntitiesToProcess < batchSize)
//                        {
//                            if (Constants.TRACING_ENABLED)
//                            {
//                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total number of entities to be processed is less than batchsize. Skipping batching and processing all entities together...", MDMTraceSource.EntityProcess);
//                            }

//                            //Entity Id's count is lesser than batch size... so skip batching and process entities directly...
//                            masterEntityOperationResult = BulkUpdateEntityAttributesBatch(entityIdsToProcess, entityContext, attributesToBeUpdated, command, callerContext);
//                        }
//                        else
//                        {
//                            Int32 startIndex = 0;
//                            Int32 batchNumber = 0;
//                            Collection<Dictionary<String, Object>> threadContexts = new Collection<Dictionary<String, Object>>();
//                            Object objOperationContext = (Object)SM.OperationContext.Current;

//                            //Get Bulk Edit threading configurations..
//                            Boolean isParallelProcessingEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.Entity.BulkEdit.ParallelProcessing.Enabled", false);
//                            Int32 threadPoolSize = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.Entity.BulkEdit.ThreadPoolSize", 5);

//                            if (Constants.TRACING_ENABLED)
//                            {
//                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Staring entities batching...", MDMTraceSource.EntityProcess);
//                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
//                                    String.Format("Bulk Edit threading configurations: MDMCenter.Entity.BulkEdit.ParallelProcessing.Enabled - '{0}'; MDMCenter.Entity.BulkEdit.ThreadPoolSize - '{1}'", isParallelProcessingEnabled.ToString(), threadPoolSize),
//                                    MDMTraceSource.EntityProcess);

//                                if (isParallelProcessingEnabled)
//                                {
//                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Parallel processing enabled. Processing entity batches in parallel...", MDMTraceSource.EntityProcess);
//                                }
//                                else
//                                {
//                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Parallel processing enabled. Processing entity batches in sequence...", MDMTraceSource.EntityProcess);
//                                }
//                            }

//                            //Start Batching
//                            while (startIndex < countOfEntitiesToProcess)
//                            {
//                                batchNumber++;
//                                Int32 endIndex = (startIndex + batchSize) - 1;

//                                if (endIndex >= countOfEntitiesToProcess)
//                                {
//                                    endIndex = countOfEntitiesToProcess - 1;
//                                }

//                                Collection<Int64> batchEntityIds = new Collection<Int64>();

//                                for (Int32 i = startIndex; i <= endIndex; i++)
//                                {
//                                    batchEntityIds.Add(entityIdsToProcess[i]);
//                                }

//                                if (isParallelProcessingEnabled)
//                                {
//                                    //Parallel Processing is enabled... Creating threading context for parallel processing
//                                    Dictionary<String, Object> threadContext = new Dictionary<String, Object>();
//                                    threadContext.Add("BatchEntityIds", batchEntityIds);
//                                    threadContext.Add("MasterEORs", masterEntityOperationResult);
//                                    threadContext.Add("EntityContext", entityContext);
//                                    threadContext.Add("AttributesToBeUpdated", attributesToBeUpdated);
//                                    threadContext.Add("BatchNumber", batchNumber);
//                                    threadContext.Add("DBCommand", command);
//                                    threadContext.Add("CallerContext", callerContext);
//                                    threadContext.Add("OperationContext", objOperationContext);

//                                    threadContexts.Add(threadContext);
//                                }
//                                else
//                                {
//                                    //Parallel Processing is not enabled... Process batches sequentially
//                                    ProcessBatchEntities(batchEntityIds, entityContext, attributesToBeUpdated, masterEntityOperationResult, batchNumber, command, callerContext);
//                                }

//                                //change start index
//                                startIndex = startIndex + batchSize;
//                            }

//                            if (isParallelProcessingEnabled)
//                            {
//                                //Process batches in parallel...
//                                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
//                                new ParallelTaskProcessor().RunInParallel(threadContexts, ProcessParallelThread, cancellationTokenSource, threadPoolSize);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "No Entity Ids provided for process", MDMTraceSource.EntityProcess);
//                    }
//                }
//                else
//                {
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "TemplateEntity is null. Cannot perform BulkUpdate for entity", MDMTraceSource.EntityProcess);
//                }
//            }
//            finally
//            {
//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                {
//                    MDMTraceHelper.StopTraceActivity("EntityManager.BulkUpdateEntityAttributes", MDMTraceSource.EntityProcess);
//                }
//            }

//            return masterEntityOperationResult;
//        }

//        #endregion

//        #region Private Methods

//        #region ProcessEntity Support Methods

//        /// <summary>
//        /// Filters the type of the entities by.
//        /// </summary>
//        /// <param name="sourceEntities">Defines the source entities.</param>
//        /// <param name="entityOperationResults">Defines the entity operation results.</param>
//        /// <param name="filterType">Defines the type of the filter.</param>
//        /// <returns></returns>
//        private EntityCollection FilterEntitiesByType(EntityCollection sourceEntities, EntityOperationResultCollection entityOperationResults, String filterType)
//        {
//            // do we have attributes
//            EntityCollection filteredEntities = new EntityCollection();
//            foreach (Entity entity in sourceEntities)
//            {
//                EntityOperationResult entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entity.Id);

//                // Check the meta data validation of the entity,Only passed entity need to be processed 
//                if (entityOperationResult != null && !entityOperationResult.HasError)
//                {
//                    EntityChangeContext entityChangeContext = (EntityChangeContext)entity.GetChangeContext();

//            switch (filterType)
//            {
//                case "Attributes":
//                        if (entityChangeContext.IsAttributesChanged)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "RelationshipsChange":
//                            if (entityChangeContext.IsRelationshipsChanged)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "RelationshipsCreate":

//                            if (entityChangeContext.IsRelationshipsCreated)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "RelationshipsUpdateOrDelete":
//                            if (entityChangeContext.IsRelationshipsUpdated || entityChangeContext.IsRelationshipsDeleted)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "Hierarchy":
//                        if (entityChangeContext.IsHierarchyChanged)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "ChildHierarchyChange":
//                        if (entity.HierarchyRelationships != null && entity.HierarchyRelationships.Action == ObjectAction.Update)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "Extensions":
//                        if (entityChangeContext.IsExtensionsChanged)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "ChildExtensionsChange":
//                        if (entity.ExtensionRelationships != null && entity.ExtensionRelationships.Action == ObjectAction.Update)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "Reclassify":
//                        if (entity.Action == ObjectAction.Reclassify)
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "Rename":
//                        if (entity.Action == ObjectAction.Rename ||
//                            (_shortNameMappedAttributeIdentifier != null &&
//                             entityChangeContext.AttributeNameList.Contains(_shortNameMappedAttributeIdentifier.AttributeName)))
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                    break;
//                case "ReclassifyOrRename":

//                        if (entity.Action == ObjectAction.Reclassify || entity.Action == ObjectAction.Rename ||
//                                (_shortNameMappedAttributeIdentifier != null &&
//                                (entityChangeContext.AttributeNameList != null && entityChangeContext.AttributeNameList.Contains(_shortNameMappedAttributeIdentifier.AttributeName))))
//                        {
//                            filteredEntities.Add(entity);
//                        }
//                            break;
//                    }
//                    }
//            }

//            return filteredEntities;
//        }

//        private void ProcessParallelThread(Dictionary<String, Object> threadContext)
//        {
//            Collection<Int64> batchEntityIds = (Collection<Int64>)threadContext["BatchEntityIds"];
//            EntityOperationResultCollection masterEORs = (EntityOperationResultCollection)threadContext["MasterEORs"];
//            EntityContext entityContext = (EntityContext)threadContext["EntityContext"];
//            AttributeCollection attributesToBeUpdated = (AttributeCollection)threadContext["AttributesToBeUpdated"];
//            Int32 batchNumber = (Int32)threadContext["BatchNumber"];
//            DBCommandProperties command = (DBCommandProperties)threadContext["DBCommand"];
//            CallerContext callerContext = (CallerContext)threadContext["CallerContext"];
//            Object operationContext = threadContext["OperationContext"];

//            SM.OperationContext.Current = (SM.OperationContext)operationContext;

//            ProcessBatchEntities(batchEntityIds, entityContext, attributesToBeUpdated, masterEORs, batchNumber, command, callerContext);
//        }

//        private void ProcessBatchEntities(Collection<Int64> batchEntityIds, EntityContext entityContext, AttributeCollection attributesToBeUpdated, EntityOperationResultCollection masterEORs, Int32 batchNumber, DBCommandProperties command, CallerContext callerContext)
//        {
//            DurationHelper durationHelper = null;
//            EntityOperationResultCollection batchEntityOperationResults = null;

//            if (Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Batch {0}: Processing started...", batchNumber), MDMTraceSource.EntityProcess);
//            }

//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//            {
//                //Instantiating only when tracing is enabled in order to avoid multiple duartionHelper instantiation as this method 
//                //executes in multi thread environment...
//                durationHelper = new DurationHelper(DateTime.Now);
//            }

//            batchEntityOperationResults = BulkUpdateEntityAttributesBatch(batchEntityIds, entityContext, attributesToBeUpdated, command, callerContext);

//            //Add all EntityOperationResults from batchEntityOperationResult to masterEntityOperationResult
//            if (batchEntityOperationResults != null)
//            {
//                foreach (EntityOperationResult entityOperationResult in batchEntityOperationResults)
//                {
//                    lock (bulkEditLockObject)
//                    {
//                        masterEORs.Add(entityOperationResult);
//                    }
//                }

//                //Copy over Errors from batchEntityOperationResult to masterEntityOperationResult
//                if (batchEntityOperationResults.Errors != null)
//                {
//                    foreach (Error error in batchEntityOperationResults.Errors)
//                    {
//                        lock (bulkEditLockObject)
//                        {
//                            masterEORs.Errors.Add(error);
//                        }
//                    }
//                }

//                if (batchEntityOperationResults.Informations != null)
//                {
//                    foreach (Information info in batchEntityOperationResults.Informations)
//                    {
//                        lock (bulkEditLockObject)
//                        {
//                            masterEORs.Informations.Add(info);
//                        }
//                    }
//                }

//                if (batchEntityOperationResults.Warnings != null)
//                {
//                    foreach (Warning warning in batchEntityOperationResults.Warnings)
//                    {
//                        lock (bulkEditLockObject)
//                        {
//                            masterEORs.Warnings.Add(warning);
//                        }
//                    }
//                }
//            }

//            if (Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Batch {0}: Processing completed.", batchNumber), MDMTraceSource.EntityProcess);
//            }

//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//            {
//                if (durationHelper != null)
//                {
//                    //Instantiating only when tracing is enabled...
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Batch {1} processing done.", durationHelper.GetDurationInMilliseconds(DateTime.Now), batchNumber), MDMTraceSource.EntityProcess);
//                }
//            }
//        }

//        private EntityOperationResultCollection BulkUpdateEntityAttributesBatch(Collection<Int64> batchEntityIds, EntityContext entityContext, AttributeCollection attributesToBeUpdated, DBCommandProperties command, CallerContext callerContext)
//        {
//            //Create a sub EntityCollection with number of entities equal to batch size.
//            EntityCollection batchEntities = new EntityCollection();
//            String entityLongNameList = String.Empty;
//            EntityOperationResultCollection batchEntityOperationResults = new EntityOperationResultCollection();

//            //take entities between startIndex and endIndex from ArrayList
//            foreach (Int64 entityId in batchEntityIds)
//            {
//                Entity baseEntity = _entityManager.Get(entityId, entityContext, true, callerContext.Application, callerContext.Module);
//                //Entity baseEntity = LoadBaseEntity(entityId, entityContext, true, callerContext, command);

//                if (baseEntity != null)
//                {
//                    IAttributeCollection changedAttributesWithValues = attributesToBeUpdated.Clone(); //TODO:: how do we manage source class data for each attribute object being cloned?
//                    baseEntity.Attributes.AddRange(changedAttributesWithValues);
//                    baseEntity.Action = ObjectAction.Update;

//                    batchEntities.Add(baseEntity);

//                    //Generate comma separated list of entity short names failed in current batch
//                    entityLongNameList = String.Concat(entityLongNameList, baseEntity.LongName, ",");
//                }
//                else
//                {
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("LoadBaseEntity returned null for EntityId = '{0}'", entityId), MDMTraceSource.EntityProcess);
//                }
//            }

//            //Remove the comma at end.
//            if (entityLongNameList.EndsWith(","))
//            {
//                entityLongNameList = entityLongNameList.Remove(entityLongNameList.LastIndexOf(","), 1);
//            }

//            //Make service call to update a batch of entities
//            try
//            {
//                if (batchEntities == null)
//                {
//                    throw new ArgumentNullException("Entities", "No entities found in Entity collection.");
//                }

//                EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions(true, true, false, true);
//                batchEntityOperationResults = Process(batchEntities, entityProcessingOptions, callerContext);
//            }
//            catch (Exception ex)
//            {
//                //Add error to masterOperationResult with all short names 
//                batchEntityOperationResults.AddOperationResult("", "Error occurred while processing these entities: " + entityLongNameList, OperationResultType.Error);
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Error in processing batch : EX = " + ex.Message, MDMTraceSource.EntityProcess);
//            }

//            return batchEntityOperationResults;
//        }

//        private void PopulateBucketsByEntityAction(Entity entity, EntityCollection entitiesForCreate, EntityCollection entitiesForUpdateAndDelete, EntityCollection entitiesForDelete, EntityCollection entitiesWithReadAction)
//        {
//            if (entity.Action == ObjectAction.Create)
//            {
//                entitiesForCreate.Add(entity);
//            }
//            else if (entity.Action == ObjectAction.Update || entity.Action == ObjectAction.Delete || entity.Action == ObjectAction.Reclassify || entity.Action == ObjectAction.Rename || entity.Action == ObjectAction.ReParent)
//            {
//                entitiesForUpdateAndDelete.Add(entity);

//                if (entity.Action == ObjectAction.Delete)
//                {
//                    entitiesForDelete.Add(entity);
//                }
//            }
//            else if (entity.Action == ObjectAction.Read)
//            {
//                entitiesWithReadAction.Add(entity);
//            }
//        }

//        private void HandleException(Exception caughtException, EntityCollection entities, EntityOperationResultCollection entityOperationResults, DiagnosticActivity diagnosticActivity, String exceptionMessageFormat, String exceptionMessageCode, CallerContext callerContext)
//        {
//            Boolean addExceptionIntoOperationResult = true;
//            String exceptionMessage = String.Empty;
//            String caughtExceptionMessage = String.Empty;

//            if (caughtException is TransactionAbortedException)
//            {
//                caughtExceptionMessage = (_localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "114023", false, callerContext)).Message; //The operation has aborted.

//                if (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors ||
//                    entityOperationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings ||
//                    entityOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed)
//                {
//                    addExceptionIntoOperationResult = false;
//                }
//            }
//            else
//            {
//                caughtExceptionMessage = caughtException.Message;
//            }

//            exceptionMessage = String.Format(exceptionMessageFormat, caughtExceptionMessage);
//            String exceptionDetails = String.Format(exceptionMessageFormat, caughtException.ToString());
//            ApplicationException appException = new ApplicationException(exceptionMessage, caughtException);
//            new ExceptionHandler(appException);

//            diagnosticActivity.LogError(exceptionDetails);

//            if (addExceptionIntoOperationResult)
//            {
//                Object[] parameters = { caughtExceptionMessage };
//                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), exceptionMessageCode, parameters, false, callerContext);

//                //Create dummy locale message and continue..
//                if (_localeMessage == null || _localeMessage.MessageClass == MessageClassEnum.UnKnown)
//                {
//                    _localeMessage = new LocaleMessage { Code = String.Empty, Message = exceptionMessage, MessageClass = MessageClassEnum.Error };
//                }

//                //Update this exception to each entity in the transaction
//                EntityOperationsCommonUtility.UpdateEntityOperationResults(entities, entityOperationResults, _localeMessage, caughtExceptionMessage, ReasonType.SystemError);
//            }

//            ClearEntitiesAndEntityMapCacheOnError(entities);

//            EntityStateValidationBL entityStateValidationBL = new EntityStateValidationBL();
//            entityStateValidationBL.ProcessForSystemException(entities, entityOperationResults, callerContext);
//        }

//        private void ClearEntitiesAndEntityMapCacheOnError(EntityCollection entities)
//        {
//            LocaleBL localeBL = new LocaleBL();
//            LocaleCollection localeCollection = localeBL.GetAvailableLocales();

//            //Clear the cache as cache is dirty now, since transaction is roll backed..
//            new EntityBufferManager().RemoveEntities(entities, localeCollection.LocalCollectionValue);

//            //Clear entity map cache as cache is corrupted now due to transaction rollback..
//            EntityMapBL entityMapManager = new EntityMapBL();
//            entityMapManager.RemoveEntityMapCache(entities);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="entitiesWithReadAction"></param>
//        /// <param name="entities"></param>
//        /// <param name="allEntities"></param>
//        /// <param name="entityOperationResults"></param>
//        /// <param name="diagnosticActivity"></param>
//        private void RemoveEntitiesWithReadAction(EntityCollection entitiesWithReadAction, EntityCollection entities, EntityCollection allEntities, EntityOperationResultCollection entityOperationResults, DiagnosticActivity diagnosticActivity)
//        {
//            if (entitiesWithReadAction != null && entitiesWithReadAction.Count > 0)
//            {
//                LocaleMessage readEntityLocaleMessage = new LocaleMessage { Code = "114446", Message = "Entity is not having any changed or valid data. Hence it has been removed from processing.", MessageClass = MessageClassEnum.Warning };

//                EntityOperationsCommonUtility.UpdateEntityOperationResults(entitiesWithReadAction, entityOperationResults, readEntityLocaleMessage, readEntityLocaleMessage.Message);

//                foreach (Entity entityWithReadAction in entitiesWithReadAction)
//                {
//                    entities.Remove(entityWithReadAction);

//                    if (entityWithReadAction.Id < 1)
//                    {
//                        allEntities.Remove(entityWithReadAction);
//                    }
//                }

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    String traceMessage = String.Format("{0}. Entities with Read action are: {1}", readEntityLocaleMessage, EntityOperationsCommonUtility.GetEntityIdListAsString(entitiesWithReadAction.GetEntityIdList()));
//                    diagnosticActivity.LogWarning(traceMessage);
//                }
//            }
//        }

//        #endregion

//        #region Compare, merge and populate actions

//        /// <summary>
//        /// Checking Attribute Option based on Apptribute Option flags in import profile
//        /// </summary>
//        /// <param name="entities"></param>
//        /// <param name="entityProcessingOptions"></param>
//        private void CheckAttributeOptions(EntityCollection entities, EntityProcessingOptions entityProcessingOptions)
//        {
//            foreach (Entity entity in entities)
//            {
//                if (entity.Action != ObjectAction.Delete) //Handle the Update and Create flow here
//                {
//                    if (entityProcessingOptions.AttributeProcessingOptionCollection != null && entityProcessingOptions.AttributeProcessingOptionCollection.Count > 0)
//                    {
//                        foreach (AttributeProcessingOptions attributeWithOptions in entityProcessingOptions.AttributeProcessingOptionCollection)
//                        {
//                            IAttribute attribute = entity.Attributes.GetAttribute(attributeWithOptions.AttributeId, attributeWithOptions.Locale);
//                            if (attribute != null)
//                            {
//                                if (attribute.Action == ObjectAction.Update) // Assuming the Compare and Merge has set the appropriate action
//                                {
//                                    if (!attributeWithOptions.CanUpdateAttribute)
//                                        attribute.Action = ObjectAction.Read;

//                                }
//                                else if (attribute.Action == ObjectAction.Create)
//                                {
//                                    if (!attributeWithOptions.CanAddAttribute)
//                                        attribute.Action = ObjectAction.Read;

//                                }
//                                else if ((attribute.Action == ObjectAction.Delete) && (entity.Action != ObjectAction.Create)) //check for Attribute delete status only when the enity action is Update
//                                {
//                                    if (!attributeWithOptions.CanDeleteAttribute)
//                                        attribute.Action = ObjectAction.Read;
//                                }
//            }

//        }
//                    }
//                }
//            }
//        }

//        #endregion

//        #region Load original entities

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="entities"></param>
//        /// <param name="entityProcessingOptions"></param>
//        /// <param name="allLocalesAcrossEntities"></param>
//        /// <param name="callerContext"></param>
//        /// <returns></returns>
//        private Boolean LoadOriginalEntitiesInBulk(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, Collection<LocaleEnum> allLocalesAcrossEntities, CallerContext callerContext)
//            {
//            #region Initial Setup

//            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                diagnosticActivity.Start();
//            }

//            Boolean returnFlag = true;
//            Boolean loadLatestFromDB = false;
//            EntityContext originalEntityContext = null;
//            EntityCollection originalEntities = null;
//            Collection<Int64> entitiesToLoad = new Collection<Int64>();

//            Boolean isAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.AttributeDependency.Enabled", false);
//            Boolean bulkEditLoadOriginalEntityFromDB = AppConfigurationHelper.GetAppConfig("MDMCenter.Entity.BulkEdit.ProcessEntity.LoadOriginalEntityFromDB", true);
//            Boolean loadAllRelationshipsForOriginalEntity = AppConfigurationHelper.GetAppConfig("MDMCenter.EntityProcessManager.OriginalEntity.LoadAllRelationships", false);

//            if (callerContext.ProgramName.ToLowerInvariant().Contains("bulkedit") && bulkEditLoadOriginalEntityFromDB)
//            {
//                //Here, we would also load only modified attributes
//                loadLatestFromDB = true;
//            }

//            EntityGetOptions entityGetOptions = new EntityGetOptions { PublishEvents = false, ApplyAVS = false, LoadLatestFromDB = loadLatestFromDB, ApplySecurity = false, UpdateCacheStatusInDB = false };
//            entityGetOptions.LoadAttributePermissions = entityProcessingOptions.ApplyDMS;

//            EntityGetOptions.EntityFillOptions entityFillOptions = entityGetOptions.FillOptions;
//            entityFillOptions.FillLookupRowWithValues = false; // This is to maintain 7.6 RA behavior. We are yet to decide if we need to open up lookup rows for BRs

//            //// All the filled additional info is only used for BR executions, if publish event itself is false, avoid to do all unnecessary filling of data
//            if (!entityProcessingOptions.PublishEvents)
//            {
//                entityFillOptions.SetAllEntityPropertiesLevelFillOptionsTo(false);
//                entityFillOptions.FillRelationshipProperties = false;
//                entityFillOptions.FillLookupDisplayValues = false;
//            }

//            #endregion

//            #region Original get for category

//            foreach (Entity entity in entities)
//            {
//                //If Entity id is present then populate the original Entity.
//                //Which means other than entity create operation, keep the backup of the entity object for the further process
//                if (entity.Id > 0 && entity.Action != ObjectAction.Create && entity.Action != ObjectAction.Delete)
//                {
//                    if (entity.EntityContext.EntityTypeId != Constants.CATEGORY_ENTITYTYPE)
//                    {
//                        //Getting entity context from first entity in collection.
//                        //TODO::Add the logic to Merge Entity Contexts of all entities...
//                        //Currently for attributes and relationships it has been done... we need to consider extensions also..
//                        if (originalEntityContext == null)
//                        {
//                            originalEntityContext = (EntityContext)entity.EntityContext.Clone();
//                        }
//                        else
//                        {
//                            if (entity.EntityContext.LoadAttributes && !originalEntityContext.LoadAttributes)
//                            {
//                                originalEntityContext.LoadAttributes = true;
//                                originalEntityContext.AttributeIdList = new Collection<Int32>(); //Attribute load is requested.. Load all attributes for original entity.
//                            }

//                            if (entity.EntityContext.LoadRelationships && !originalEntityContext.LoadRelationships)
//                            {
//                                originalEntityContext.LoadRelationships = true; //Relationships load is requested.. Load relationships for original entity.
//                    }
//                        }

//                        entitiesToLoad.Add(entity.Id);
//                    }
//                    else
//                    {
//                        EntityContext originalCategoryContext = (EntityContext)entity.EntityContext.Clone();
//                        PopulateOriginalEntityContext(originalCategoryContext, isAttributeDependencyEnabled, loadAllRelationshipsForOriginalEntity);
//                        originalCategoryContext.LoadSources = entityProcessingOptions.ProcessSources;

//                        originalEntities = _entityManager.Get(new Collection<Int64> { entity.Id }, originalCategoryContext, entityGetOptions, callerContext);

//                        if (originalEntities != null && originalEntities.Count > 0)
//                        {
//                            entity.OriginalEntity = originalEntities.FirstOrDefault();
//            }
//                    }
//                }
//            }

//            #endregion

//            #region Original get for entities without categories

//            if (entitiesToLoad.Count > 0)
//            {
//                PopulateOriginalEntityContext(originalEntityContext, isAttributeDependencyEnabled, loadAllRelationshipsForOriginalEntity);

//                if (originalEntityContext != null)
//                {
//                    originalEntityContext.ContainerId = 0;
//                    originalEntityContext.CategoryId = 0;
//                    originalEntityContext.EntityTypeId = 0;
//                    originalEntityContext.LoadSources = entityProcessingOptions.ProcessSources;
//                    originalEntityContext.DataLocales = allLocalesAcrossEntities;
//                    originalEntityContext.LoadStateValidationAttributes = true;

//                    //Get entities.
//                    originalEntities = _entityManager.Get(entitiesToLoad, originalEntityContext, entityGetOptions, callerContext);

//                    if (originalEntities != null && originalEntities.Count > 0)
//                    {
//                        foreach (Entity originalEntity in originalEntities)
//                        {
//                            //Find entity from incoming collection
//                            Entity entity = entities.GetEntity(originalEntity.Id) as Entity;

//                            if (entity != null)
//                            {
//                                entity.OriginalEntity = originalEntity;
//                            }
//                        }
//                    }
//                }
//            }

//            #endregion

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                diagnosticActivity.Stop();
//            }

//            return returnFlag;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="entityContext"></param>
//        /// <param name="isAttributeDependencyEnabled"></param>
//        /// <param name="loadAllRelationshipsForOriginalEntity"></param>
//        private void PopulateOriginalEntityContext(EntityContext entityContext, Boolean isAttributeDependencyEnabled, Boolean loadAllRelationshipsForOriginalEntity)
//        {
//            entityContext.LoadLookupDisplayValues = true;

//            RelationshipContext relationshipContext = (RelationshipContext)entityContext.GetRelationshipContext();

//            //If 'MDMCenter.AttributeDependency.Enabled' flag set as false then no need to load dependent attributes and 
//            //attribute model as part of the original entity. 
//            if (isAttributeDependencyEnabled)
//            {
//                entityContext.LoadDependentAttributes = true;
//                entityContext.LoadAttributeModels = true;
//            }

//            if (entityContext.AttributeIdList.Count > 0) // this means load all attributes for entity when process is not called from Bulk edit
//            {
//                entityContext.AttributeIdList = new Collection<Int32>();
//            }

//            if (relationshipContext != null)
//            {
//                relationshipContext.ApplyDMS = false;

//                if (loadAllRelationshipsForOriginalEntity && entityContext.LoadRelationships && relationshipContext.RelationshipTypeIdList != null && relationshipContext.RelationshipTypeIdList.Count > 0)
//                {
//                    entityContext.RelationshipContext.RelationshipTypeIdList = new Collection<Int32>();
//                }
//            }
//        }

//        #endregion

//        #region Entity Family Methods

//        /// <summary>
//        /// Gets collection of entity family queue.
//        /// </summary>
//        /// <param name="entityFamilyChangeContexts">Indicates the collection of entity change context</param>
//        /// <returns>Collection of entity family queue collection</returns>
//        private EntityFamilyQueueCollection GetEntityFamilyQueues(EntityFamilyChangeContextCollection entityFamilyChangeContexts)
//        {
//            EntityFamilyQueueCollection entityFamilyQueues = null;

//            if (entityFamilyChangeContexts != null && entityFamilyChangeContexts.Count > 0)
//            {
//                entityFamilyQueues = new EntityFamilyQueueCollection();

//                foreach (EntityFamilyChangeContext entityFamilyChangeContext in entityFamilyChangeContexts)
//            {
//                    EntityFamilyQueue entityFamilyQueue = new EntityFamilyQueue();
//                    entityFamilyQueue.EntityFamilyId = entityFamilyChangeContext.EntityFamilyId;
//                    entityFamilyQueue.EntityGlobalFamilyId = entityFamilyChangeContext.EntityGlobalFamilyId;
//                    entityFamilyQueue.EntityFamilyChangeContexts.Add(entityFamilyChangeContext);
//                    entityFamilyQueue.ContainerId = entityFamilyChangeContext.ContainerId;
//                    entityFamilyQueue.EntityActivityList = EntityActivityList.VariantsChange;
//                    entityFamilyQueue.Action = ObjectAction.Create;

//                    entityFamilyQueues.Add(entityFamilyQueue);
//                }
//            }

//            return entityFamilyQueues;
//        }

//        /// <summary>
//        /// Verifies the entity lock.
//        /// </summary>
//        /// <param name="entities">The entities.</param>
//        /// <param name="entityOperationResults">The entity operation results.</param>
//        /// <param name="entityIdsForLifecycleStatusChange">The entity ids for lifecycle status change.</param>
//        /// <param name="callerContext">The caller context.</param>
//        private void VerifyEntityLock(EntityCollection entities, EntityOperationResultCollection entityOperationResults, Collection<Int64> entityIdsForLifecycleStatusChange, CallerContext callerContext)
//        {
//            if (entities != null && entities.Count > 0)
//                {
//                Collection<Int64> entityGlobalFamilyIds = new Collection<Int64>();

//                foreach (Entity entity in entities)
//                    {
//                    if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
//                    {
//                        continue;
//                    }

//                    if (!entityIdsForLifecycleStatusChange.Contains(entity.Id) && entity.EntityGlobalFamilyId > 0 && !entityGlobalFamilyIds.Contains(entity.EntityGlobalFamilyId))
//                {
//                        entityGlobalFamilyIds.Add(entity.EntityGlobalFamilyId);
//                }
//                }

//                if (entityGlobalFamilyIds.Count > 0)
//                {
//                    Dictionary<Int64, LockType> entitiesLockStatus = new EntityStateValidationBL().GetEntityLocks(entityGlobalFamilyIds, callerContext);

//                    if (entitiesLockStatus != null && entitiesLockStatus.Count > 0)
//                    {
//                        foreach (Entity entity in entities)
//                        {
//                            LockType lockType = LockType.Unknown;
//                            entitiesLockStatus.TryGetValue(entity.EntityGlobalFamilyId, out lockType);

//                            if (lockType == LockType.Promote || lockType == LockType.Revalidate)
//                {
//                                EntityOperationResult entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(entity.Id);
//                                String messageCode = String.Empty;

//                                if (lockType == LockType.Revalidate)
//                {
//                                    messageCode = "114343"; // Unable to perform the requested operation, as the entity family is locked for Re-validate.
//                }
//                                else if (lockType == LockType.Promote)
//                                {
//                                    messageCode = "114344"; // 	Unable to perform the requested operation, as the entity family is locked for Promote.
//            }

//                                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
//                                entityOperationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, ReasonType.EntityLocked, -1, -1, OperationResultType.Error);
//        }
//            }

//                        //Update entity operation results status
//                        entityOperationResults.RefreshOperationResultStatus();
//                }
//            }
//        }
//        }

//        #endregion Entity Family Methods

//        #region EH Generation Helper Methods

//        //TODO - Needs to move this function from here once proper DDG Keyword is getting generated.
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <param name="entityOperationResult"></param>
//        private void GenerateChildEntityName(Entity entity, EntityOperationResult entityOperationResult)
//        {
//            if (Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.StartTraceActivity("BusinessRule.GenerateChildEntityName", false);
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Inside GenerateChildEntityName business rule - Entity Id: {0}, Entity Name: {1}, Entity LongName: {2}", entity.Id.ToString(), entity.Name, entity.LongName));
//            }

//            try
//            {
//                if (String.IsNullOrEmpty(entity.Name.Trim()))
//                {
//                    if (entity.EntityTypeId != 6)
//                    {
//                        ObjectType objectType = ObjectType.Component;

//                        Int32 userId = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserId;

//                        BusinessRuleBL businessRuleBL = new BusinessRuleBL();
//                        Collection<String> uniqueIds = businessRuleBL.GetUniqueId(objectType, entity.OrganizationId, entity.ContainerId, entity.CategoryId, entity.EntityTypeId, 0, entity.Locale.ToString(), 0, userId, 1);

//                        if (uniqueIds != null && uniqueIds.Count > 0)
//                        {
//                            String entityUId = uniqueIds.FirstOrDefault();
//                            entity.Name = entityUId;
//                            entity.LongName = entityUId;
//                            entity.SKU = entityUId;
//                        }
//                    }
//                    else
//                {
//                        if (Constants.TRACING_ENABLED)
//                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Current entity type is of Category. Name cannot be generated. Skipping rule..");
//                    }
//                }
//                else
//                {
//                    if (Constants.TRACING_ENABLED)
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity name is already available. Exiting rule..");
//                }
//                }
//            catch (Exception ex)
//            {
//                entityOperationResult.AddOperationResult("", ex.Message, OperationResultType.Error);
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message);
//            }
//            finally
//            {
//                if (Constants.TRACING_ENABLED)
//                    MDMTraceHelper.StopTraceActivity("BusinessRule.GenerateChildEntityName");
//            }
//        }

//        #endregion EH Generation Helper Methods

//        #endregion

//        #endregion
//    }
//}