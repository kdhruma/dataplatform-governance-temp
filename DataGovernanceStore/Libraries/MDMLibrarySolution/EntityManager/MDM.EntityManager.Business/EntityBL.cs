using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.EntityManager.Business
{
    using AdminManager.Business;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using ConfigurationManager.Business;
    using Core;
    using Core.Exceptions;
    using Core.Extensions;
    using Data;
    using EntityOperations;
    using EntityOperations.Helpers;
    using Interfaces;
    using MDM.BusinessObjects.Workflow;
    using MessageManager.Business;
    using RelationshipManager.Business;
    using Utility;

    /// <summary>
    ///     Specifies entity manager
    /// </summary>
    public class EntityBL : BusinessLogicBase, IEntityManager
    {
        #region Fields

        /// <summary>
        ///     Field denoting the action of the operation for ex.: Create, Update
        /// </summary>
        private String _operationAction = String.Empty;

        /// <summary>
        ///     Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        ///     Field denoting localeMessageBL
        /// </summary>
        private readonly LocaleMessageBL _localeMessageBL;

        /// <summary>
        ///     Field denoting the security principal
        /// </summary>
        private readonly SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// 
        /// </summary>
        private EntityGetManager _entityGetManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public EntityBL()
        {
            _localeMessageBL = new LocaleMessageBL();
            //_securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            this._securityPrincipal = new SecurityPrincipal(new UserIdentity("cfadmin", "cfadmin"));
            _entityGetManager = new EntityGetManager(this, _securityPrincipal);
        }

        #endregion

        #region Methods

        #region Entity CUD

        /// <summary>
        ///     Creates new entity in MDM system from the given instance
        /// </summary>
        /// <param name="entity">Instance of entity to be added in MDM system</param>
        /// <param name="programName">Indicates from which program created the entities</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entity is null or operation result is null</exception>
        /// <exception cref="NotImplementedException">
        ///     If more than 1 entities are passed as multiple entity processing is not
        ///     supported currently
        /// </exception>
        public EntityOperationResult Create(Entity entity, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess, false);

            var entityOperationResult = new EntityOperationResult();
            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Create";

            if (entity == null)
            {
                diagnosticActivity.LogError("111815", String.Empty);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Create");
            }

            entity.Action = ObjectAction.Create;

            EntityOperationResultCollection entityOperationResults = Process(new EntityCollection { entity }, programName, application, module);

            if (entityOperationResults != null && entityOperationResults.Count > 0)
                entityOperationResult = entityOperationResults.First();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess);

            return entityOperationResult;
        }

        /// <summary>
        ///     Creates new entity in MDM system from the given instance
        /// </summary>
        /// <param name="entity">Instance of entity to be added in MDM system</param>
        /// <param name="entityProcessingOptions">Specifies processing options</param>
        /// <param name="programName">
        ///     Indicates from which program Process method is being called. This would be used for log
        ///     purpose.
        /// </param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <exception cref="MDMOperationException">If Entity is null or operation result is null</exception>
        /// <returns>Result of the operation having errors and information if any</returns>
        public EntityOperationResult Create(Entity entity, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess, false);

            var entityOperationResult = new EntityOperationResult();
            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Create";

            if (entity == null)
            {
                diagnosticActivity.LogError("111815", String.Empty);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Create");
            }

            entity.Action = ObjectAction.Create;

            EntityOperationResultCollection entityOperationResults = Process(new EntityCollection { entity }, entityProcessingOptions, programName, application, module);

            if (entityOperationResults != null && entityOperationResults.Count > 0)
                entityOperationResult = entityOperationResults.First();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess);

            return entityOperationResult;
        }

        /// <summary>
        ///     Create new entities in MDM system from the given instance
        /// </summary>
        /// <param name="entities">Instance of entities to be added in MDM system</param>
        /// <param name="programName">Indicates from which program created the entities</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <exception cref="MDMOperationException">If Entities are null or operation results is null</exception>
        /// <returns>Results of the operation having errors and information if any</returns>
        public EntityOperationResultCollection Create(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess, false);

            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Create";

            #region Entities null or empty check

            ValidateEntityCollection(entities, callerContext, diagnosticActivity, _operationAction);

            #endregion Entities null or empty check

            foreach (Entity entity in entities)
            {
                entity.Action = ObjectAction.Create;
            }

            EntityOperationResultCollection entityOperationResults = Process(entities, programName, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess);

            return entityOperationResults;
        }

        /// <summary>
        ///     Create new entities in MDM system from the given instance
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Specifies processing options</param>
        /// <param name="programName">
        ///     Indicates from which program Process method is being called. This would be used for log
        ///     purpose.
        /// </param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entities are null or operation results is null</exception>
        public EntityOperationResultCollection Create(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess, false);

            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Create";

            #region Entities null or empty check

            ValidateEntityCollection(entities, callerContext, diagnosticActivity, _operationAction);

            #endregion Entities null or empty check

            foreach (Entity entity in entities)
            {
                entity.Action = ObjectAction.Create;
            }

            EntityOperationResultCollection entityOperationResults = Process(entities, entityProcessingOptions, programName, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess);

            return entityOperationResults;
        }

        /// <summary>
        ///     Create new entity in MDM system from the given instances
        /// </summary>
        /// <param name="entity">Instance of entity to be processed in MDM system</param>
        /// <param name="imageDetails">Indicates Dictionary of "AttributeId + Locale" and "File detail" for that attribute.</param>
        /// <param name="programName">Indicates from which program create method is being called.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public EntityOperationResult Create(Entity entity, Dictionary<KeyValuePair<Int32, LocaleEnum>, File> imageDetails, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess, false);
            var callerContext = new CallerContext(application, module);

            #region Parameter validation

            if (entity == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Create"); //Entity is null
            }

            #endregion Parameter valudation

            entity.Action = ObjectAction.Create;

            EntityOperationResult entityOR;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                var fileManager = new FileBL();

                if (imageDetails.Count > 0)
                {
                    foreach (var pair in imageDetails)
                    {
                        //Pair.Key = KeyValuePair<Int32, LocaleEnum>
                        //Pair.Key.Key = AttributeId
                        //Pair.Key.Value = Locale of that attribute.
                        //Pair.Value = File

                        if (pair.Key.Key > 0 && pair.Value != null)
                        {
                            Int32 fileId = fileManager.Process(pair.Value, new CallerContext(application, module));

                            if (fileId > 0)
                            {
                                if (entity.Attributes[pair.Key.Key, pair.Key.Value] != null && entity.Attributes[pair.Key.Key, pair.Key.Value].SourceFlag == AttributeValueSource.Overridden)
                                {
                                    entity.Attributes[pair.Key.Key, pair.Key.Value].OverriddenValues.Clear();
                                    entity.Attributes[pair.Key.Key, pair.Key.Value].OverriddenValues.Add(new Value(fileId));
                                }
                            }
                        }
                    }
                }

                entityOR = Create(entity, programName, application, module);
                if (entityOR != null && !entityOR.HasError)
                {
                    transactionScope.Complete();
                }
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Create", MDMTraceSource.EntityProcess);

            return entityOR;
        }

        /// <summary>
        ///     Updates entity in MDM system from the instance given.
        /// </summary>
        /// <param name="entity">Instance of entity to be Updated in the MDM.</param>
        /// <param name="programName">Indicates from which program an entity is being Updated</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entity is null or operation result is null</exception>
        public EntityOperationResult Update(Entity entity, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess, false);

            var entityOperationResult = new EntityOperationResult();
            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Update";

            if (entity == null)
            {
                diagnosticActivity.LogError("111815", String.Empty);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Update"); //Entity is null
            }

            if (entity.Action == ObjectAction.Read)
                entity.Action = ObjectAction.Update;

            EntityOperationResultCollection entityOperationResults = Process(new EntityCollection { entity }, programName, application, module);

            if (entityOperationResults != null)
                entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entity.Id);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess);

            return entityOperationResult;
        }

        /// <summary>
        ///     Updates entity in MDM system from the instance given
        /// </summary>
        /// <param name="entity">Instance of entity to be Updated in the MDM</param>
        /// <param name="entityProcessingOptions">Specifies processing options</param>
        /// <param name="programName">Indicates from which program an entity is being Updated</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public EntityOperationResult Update(Entity entity, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess, false);

            var entityOperationResult = new EntityOperationResult();
            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Update";

            if (entity == null)
            {
                diagnosticActivity.LogError("111815", String.Empty);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Update");
            }

            if (entity.Action == ObjectAction.Read)
                entity.Action = ObjectAction.Update;

            EntityOperationResultCollection entityOperationResults = Process(new EntityCollection { entity }, entityProcessingOptions, programName, application, module);

            if (entityOperationResults != null)
                entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entity.Id);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess);

            return entityOperationResult;
        }

        /// <summary>
        ///     Update entities in MDM system from the instance given.
        /// </summary>
        /// <param name="entities">Instances of entities to be Updated in the MDM.</param>
        /// <param name="programName">Indicates from which program an entity is being Updated</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entities are null or operation result is null</exception>
        public EntityOperationResultCollection Update(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess, false);

            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Update";

            #region Entities null or empty check

            ValidateEntityCollection(entities, callerContext, diagnosticActivity, _operationAction);

            #endregion Entities null or empty check

            foreach (Entity entity in entities)
            {
                if (entity.Action == ObjectAction.Read)
                    entity.Action = ObjectAction.Update;
            }

            EntityOperationResultCollection entityOperationResults = Process(entities, programName, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess);

            return entityOperationResults;
        }

        /// <summary>
        ///     Update entities in MDM system from the instance given.
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Specifies processing options</param>
        /// <param name="programName">
        ///     Indicates from which program Process method is being called. This would be used for log
        ///     purpose.
        /// </param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entities collection is null or operation result is null</exception>
        public EntityOperationResultCollection Update(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess, false);

            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            //Set the operation action
            _operationAction = "Update";

            #region Entities null or empty check

            ValidateEntityCollection(entities, callerContext, diagnosticActivity, _operationAction);

            #endregion Entities null or empty check

            foreach (Entity entity in entities)
            {
                if (entity.Action == ObjectAction.Read)
                    entity.Action = ObjectAction.Update;
            }

            EntityOperationResultCollection entityOperationResults = Process(entities, entityProcessingOptions, programName, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess);

            return entityOperationResults;
        }

        /// <summary>
        ///     Update entity in MDM system for the given instance with image/file attribute.
        ///     It will also save file in database.
        /// </summary>
        /// <param name="entity">Instance of entity to be processed in MDM system.</param>
        /// <param name="imageDetails">Indicates Dictionary of AttributeId and File detail for that attribute.</param>
        /// <param name="programName">Indicates from which program update method is being called</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Result of the operation having errors and information if any</returns>
        public EntityOperationResult Update(Entity entity, Dictionary<KeyValuePair<Int32, LocaleEnum>, File> imageDetails, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess, false);
            var callerContext = new CallerContext(application, module);

            #region Parameter validation

            if (entity == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Update"); //Entity is null
            }

            #endregion Parameter valudation

            EntityOperationResult entityOR;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                var fileManager = new FileBL();

                if (imageDetails.Count > 0)
                {
                    foreach (var pair in imageDetails)
                    {
                        //Pair.Key = KeyValuePair<Int32, LocaleEnum>
                        //Pair.Key.Key = AttributeId
                        //Pair.Key.Value = Locale of that attribute.
                        //Pair.Value = File

                        if (pair.Key.Key > 0 && pair.Value != null)
                        {
                            Int32 fileId = fileManager.Process(pair.Value, new CallerContext(application, module));

                            if (fileId > 0)
                            {
                                if (entity.Attributes[pair.Key.Key, pair.Key.Value] != null && entity.Attributes[pair.Key.Key, pair.Key.Value].SourceFlag == AttributeValueSource.Overridden)
                                {
                                    entity.Attributes[pair.Key.Key, pair.Key.Value].OverriddenValues.Clear();
                                    entity.Attributes[pair.Key.Key, pair.Key.Value].OverriddenValues.Add(new Value(fileId));
                                }
                            }
                        }
                    }
                }

                if (entity.Action == ObjectAction.Read)
                    entity.Action = ObjectAction.Update;

                entityOR = Update(entity, programName, application, module);

                if (entityOR != null && !entityOR.HasError)
                {
                    transactionScope.Complete();
                }
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Update", MDMTraceSource.EntityProcess);

            return entityOR;
        }

        /// <summary>
        ///     Deletes an entity from the MDM System.
        /// </summary>
        /// <param name="entity">Entity to be deleted from the MDM System.</param>
        /// <param name="programName">Program name.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public EntityOperationResult Delete(Entity entity, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Delete", MDMTraceSource.EntityProcess, false);

            var entityOperationResult = new EntityOperationResult();
            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            if (String.IsNullOrWhiteSpace(programName))
            {
                programName = "EntityManager.Entity.Delete";
            }

            if (entity == null)
            {
                diagnosticActivity.LogError("111815", String.Empty);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Delete");
            }

            entity.Action = ObjectAction.Delete;

            EntityOperationResultCollection entityOperationResults = Process(new EntityCollection { entity }, programName, application, module);

            if (entityOperationResults != null)
                entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entity.Id);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Delete", MDMTraceSource.EntityProcess);

            return entityOperationResult;
        }

        /// <summary>
        ///     Deletes an entity from the MDM System.
        /// </summary>
        /// <param name="entities">Entities to be deleted from the MDM System.</param>
        /// <param name="programName">Name of application which is performing action</param>
        /// <param name="application">Name of module which is performing action</param>
        /// <param name="module">Results of the operation having errors and information if any</param>
        /// <returns></returns>
        public EntityOperationResultCollection Delete(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Delete", MDMTraceSource.EntityProcess, false);

            var diagnosticActivity = new DiagnosticActivity();
            var callerContext = new CallerContext(application, module);

            #region Entities null or empty check

            ValidateEntityCollection(entities, callerContext, diagnosticActivity, "Delete");

            #endregion Entities null or empty check

            foreach (Entity entity in entities)
            {
                entity.Action = ObjectAction.Delete;
            }

            EntityOperationResultCollection entityOperationResults = Process(entities, programName, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Delete", MDMTraceSource.EntityProcess);

            return entityOperationResults;
        }

        /// <summary>
        ///     Creates new entity in MDM system from the given instance
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="programName">Indicates from which program created the entities</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entities is null or operation result is null</exception>
        public EntityOperationResultCollection Process(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            var entityProcessingOptions = new EntityProcessingOptions(true, true, false, true);
            return Process(entities, entityProcessingOptions, programName, application, module);
        }

        /// <summary>
        ///     Process given list of entities based on their actions
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Specifies processing options</param>
        /// <param name="programName">
        ///     Indicates from which program Process method is being called. This would be used for log
        ///     purpose.
        /// </param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entities collection is null or operation result is null</exception>
        public EntityOperationResultCollection Process(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            var callerContext = new CallerContext { Application = application, Module = module, ProgramName = programName };
            return Process(entities, entityProcessingOptions, callerContext);
        }

        /// <summary>
        ///     Process given list of entities based on their actions
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Specifies processing options</param>
        /// <param name="callerContext">Indicates the Caller Context</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="MDMOperationException">If Entities collection is null or operation result is null</exception>
        public EntityOperationResultCollection Process(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
        {
            return null;
            //return new EntityProcessManager(this, _securityPrincipal, _operationAction).Process(entities, entityProcessingOptions, callerContext);
        }

        /// <summary>
        /// </summary>
        /// <param name="templateEntities"></param>
        /// <param name="entityIdsToProcess"></param>
        /// <param name="actionPerformed"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityOperationResultCollection BulkUpdateEntityAttributes(EntityCollection templateEntities, Collection<Int64> entityIdsToProcess, String actionPerformed, CallerContext callerContext)
        {
            return null;
            //return new EntityProcessManager(this, _securityPrincipal, _operationAction).BulkUpdateEntityAttributes(templateEntities, entityIdsToProcess, actionPerformed, callerContext);
        }

        #endregion

        #region Get Entity

        /// <summary>
        ///     Gets an entity with given id and entity context
        /// </summary>
        /// <param name="entityId">Id of an entity for which information is to be fetched</param>
        /// <param name="entityContext">Specifies what all information is to be loaded for given entity</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Entity having given EntityId and attributes specified in EntityContext</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0</exception>
        /// <exception cref="MDMOperationException">Thrown if EntityContext is null</exception>
        /// <exception cref="MDMOperationException">
        ///     Thrown if none of the EntityContext's properties like LoadEntityProperties,
        ///     LoadAttributes or LoadRelationships is set to true
        /// </exception>
        public Entity Get(Int64 entityId, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet, false);

            //Note:: Always returns entity properties irrespective of EntityContext.LoadEntityProperties flag in order to have
            //entity details to fetch attributes. This is helpful when user does not pass ContainerId or EntityTypeId and CategoryId and 
            //requests for Attributes

            Entity entity = Get(entityId, entityContext, false, application, module, publishEvents, applyAVS);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet);

            return entity;
        }

        /// <summary>
        ///     Gets an entity with given id and entity context
        /// </summary>
        /// <param name="entityId">Id of an entity for which information is to be fetched</param>
        /// <param name="entityContext">Specifies what all information is to be loaded for given entity</param>
        /// <param name="loadLatest">Load entity from database refreshing cache</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <param name="applySecurity">Indicates whether to apply security or not. Default is set to 'true'</param>
        /// <param name="updateCache">Specifies whether to update the cache after loading</param>
        /// <returns>Entity having given EntityId and attributes specified in EntityContext</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0</exception>
        /// <exception cref="MDMOperationException">Thrown if EntityContext is null</exception>
        /// <exception cref="MDMOperationException">
        ///     Thrown if none of the EntityContext's properties like LoadEntityProperties,
        ///     LoadAttributes or LoadRelationships is set to true
        /// </exception>
        public Entity Get(Int64 entityId, EntityContext entityContext, Boolean loadLatest, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true, Boolean applySecurity = true, Boolean updateCache = false)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet, false);

            var callerContext = new CallerContext(application, module, "MDM.EntityManager.Business.EntityBL.Get");
            //Note:: Always returns entity properties irrespective of EntityContext.LoadEntityProperties flag in order to have
            //entity details to fetch attributes. This is helpful when user does not pass ContainerId or EntityTypeId and CategoryId and 
            //requests for Attributes

            #region Parameter Validation

            if (entityId <= 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Entity get request is received without entity id. Get operation is being terminated with exception.", MDMTraceSource.EntityGet);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                throw new MDMOperationException("111795", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //EntityId must be greater than 0
            }

            if (entityContext == null)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Entity get request is received without entity context. Get operation is being terminated with exception.", MDMTraceSource.EntityGet);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111792", false, callerContext);
                throw new MDMOperationException("111792", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //EntityContext
            }

            #endregion Parameter Validation

            var entity = new Entity();

            var entityIds = new Collection<Int64> { entityId };

            var entityGetOptions = new EntityGetOptions { PublishEvents = publishEvents, ApplyAVS = applyAVS, LoadLatestFromDB = loadLatest, ApplySecurity = applySecurity, UpdateCache = updateCache };

            EntityCollection entityCollection = Get(entityIds, entityContext, entityGetOptions, callerContext);

            if (entityCollection != null && entityCollection.Any())
                entity = entityCollection.First();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet);

            return entity;
        }

        /// <summary>
        ///     Gets entities for the requested ids and entity context
        /// </summary>
        /// <param name="entityIdList">Ids of entities for which information needs to be fetched</param>
        /// <param name="entityContext">Specifies what all information is to be loaded for entity</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Collection of entity objects</returns>
        /// <exception cref="MDMOperationException">Thrown when entity ids are not provided</exception>
        /// <exception cref="MDMOperationException">Thrown if EntityContext is null</exception>
        /// <exception cref="MDMOperationException">
        ///     Thrown if none of the EntityContext's properties like LoadEntityProperties,
        ///     LoadAttributes or LoadRelationships is set to true
        /// </exception>
        public EntityCollection Get(Collection<Int64> entityIdList, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet, false);

            EntityCollection entities = Get(entityIdList, entityContext, false, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet);

            return entities;
        }

        /// <summary>
        ///     Gets entities for the requested ids and entity context
        /// </summary>
        /// <param name="entityIdList">Ids of entities for which information needs to be fetched</param>
        /// <param name="entityContext">Specifies what all information is to be loaded for entity</param>
        /// <param name="loadLatest">Load entity from database refreshing cache</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <param name="applySecurity">Indicates whether to apply security or not. Default is set to 'true'</param>
        /// <param name="bulkGetBatchSize">
        ///     Specifies the bulk get batch size. Default is 0, so the value specified in app config
        ///     'MDMCenter.EntityManager.BulkEntityGet.BatchSize' is considered.
        /// </param>
        /// <returns>Collection of entity objects</returns>
        /// <exception cref="MDMOperationException">
        ///     Thrown when entity ids are not provided / if EntityContext is null / if none of
        ///     the EntityContext's properties like LoadEntityProperties, LoadAttributes or LoadRelationships is set to true
        /// </exception>
        public EntityCollection Get(Collection<Int64> entityIdList, EntityContext entityContext, Boolean loadLatest, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true, Boolean applySecurity = true, Int32 bulkGetBatchSize = 0)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet, false);

            var callerContext = new CallerContext(application, module, "MDM.EntityManager.Business.EntityBL.Get");

            #region Parameter Validation

            if (entityIdList == null || entityIdList.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111785", false, callerContext);
                throw new MDMOperationException("111785", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //Entity Ids are not available
            }

            if (entityContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111786", false, callerContext);
                throw new MDMOperationException("111786", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //EntityContext is not available
            }

            #endregion

            var entityGetOptions = new EntityGetOptions { PublishEvents = publishEvents, ApplyAVS = applyAVS, ApplySecurity = applySecurity, BulkGetBatchSize = bulkGetBatchSize };
            EntityCollection entityCollection = Get(entityIdList, entityContext, entityGetOptions, callerContext);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet);

            return entityCollection;
        }

        /// <summary>
        ///     Gets and entity with given id and locale from a given container.
        ///     This method will only load entity metadata. No attributes or relationships will be loaded
        /// </summary>
        /// <param name="entityId">Id of the entity.</param>
        /// <param name="containerId">Container to which the entity belongs to.</param>
        /// <param name="locale">The locale in which the details are required</param>
        /// <param name="dataLocales">Collection of locales indicating attribute values are to be populated in which all locales.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Entity corresponding to the given id, container and locale.</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0</exception>
        /// <exception cref="MDMOperationException">Thrown if locale value is not provided</exception>
        public Entity Get(Int64 entityId, Int32 containerId, LocaleEnum locale, Collection<LocaleEnum> dataLocales, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet, false);

            Entity entity = Get(entityId, containerId, locale, dataLocales, false, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet);

            return entity;
        }

        /// <summary>
        ///     Gets and entity with given id and locale from a given container.
        ///     This method will only load entity metadata. No attributes will be loaded
        /// </summary>
        /// <param name="entityId">Id of the entity.</param>
        /// <param name="containerId">Container id from which to get the entity.</param>
        /// <param name="locale">The locale in which the details are required</param>
        /// <param name="dataLocales">The data locale in which the details are required</param>
        /// <param name="loadCompleteEntity">Indicates if all details needs to be loaded</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Entity corresponding to the given id, container and locale.</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0 / if locale value is not provided</exception>
        public Entity Get(Int64 entityId, Int32 containerId, LocaleEnum locale, Collection<LocaleEnum> dataLocales, Boolean loadCompleteEntity, MDMCenterApplication application, MDMCenterModules module)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet, false);
            var callerContext = new CallerContext(application, module, "MDM.EntityManager.Business.EntityBL.Get");

            #region Parameter Validation

            if (entityId <= 0)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                throw new MDMOperationException("111795", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //EntityId must be greater than 0
            }

            //Container can be 0, if we want to fetch product information. Container is required for getting category information

            if (locale == LocaleEnum.UnKnown)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111793", false, callerContext);
                throw new MDMOperationException("111793", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //Locale value cannot be null or unknown
            }

            #endregion Parameter Validation

            var entityContext = new EntityContext();
            entityContext.ContainerId = containerId;
            entityContext.CategoryId = 0;
            entityContext.EntityTypeId = 0;
            entityContext.Locale = locale;
            entityContext.DataLocales = dataLocales;
            entityContext.LoadEntityProperties = true;
            entityContext.LoadAttributes = loadCompleteEntity;
            entityContext.AttributeModelType = AttributeModelType.All;
            entityContext.AttributeIdList = null;
            entityContext.LoadRelationships = loadCompleteEntity;
            entityContext.LoadCreationAttributes = false;
            entityContext.LoadRequiredAttributes = false;
            entityContext.LoadHierarchyRelationships = loadCompleteEntity;

            Entity entity = Get(entityId, entityContext, application, module);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.Get", MDMTraceSource.EntityGet);

            return entity;
        }

        /// <summary>
        ///     Get Entity By given ExternalId
        /// </summary>
        /// <param name="externalId">ExternalId for which Entity needs to be Populate</param>
        /// <param name="entityContext">entityContext for which Entity needs to be Populate</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Entity Object</returns>
        /// <exception cref="MDMOperationException">Thrown if externalId or entity context is null</exception>
        /// <exception cref="MDMOperationException">Thrown if Entity is Category and EntityContext is having ContainerId Zero</exception>
        public Entity GetByExternalId(String externalId, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            return _entityGetManager.GetByExternalId(externalId, entityContext, callerContext, publishEvents, applyAVS);
        }

        /// <summary>
        /// Gets an entity collection based on the external id(s) and entity context
        /// </summary>
        /// <param name="externalIdList">Lists the external id(s) for which the entities needs to be retrieved</param>
        /// <param name="entityContext">Specifies the entity context for which entities needs to be retrieved</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <param name="publishEvents">Specifies whether to publish events or not</param>
        /// <param name="applyAVS">Specifies whether to apply AVS or not</param>
        /// <returns>Returns the collection of entities</returns>
        public EntityCollection GetEntitiesByExternalIds(Collection<String> externalIdList, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents, Boolean applyAVS)
        {
            return _entityGetManager.GetEntitiesByExternalIds(externalIdList, entityContext, callerContext, publishEvents, applyAVS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Entity Get(Int64 entityId, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            Entity entity = null;

            EntityCollection entities = _entityGetManager.GetEntityMain(new Collection<Int64> { entityId }, entityContext, entityGetOptions, callerContext);

            if (entities != null && entities.Count > 0)
                entity = entities.FirstOrDefault();

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection Get(Collection<Int64> entityIdList, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            return _entityGetManager.GetEntityMain(entityIdList, entityContext, entityGetOptions, callerContext);
        }

        /// <summary>
        /// Gets entity read result based on entityscopes provided
        /// </summary>
        /// <param name="entityScopeCollection">EntityScope list based on which entities to be retrieved</param>
        /// <param name="entityGetOptions">Options to retrieve entities</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>EntityReadResult containing EntityCollection and EntityOperationResultCollection</returns>
        public EntityReadResult Get(EntityScopeCollection entityScopeCollection, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            return new EntityParallelGetManager(this).GetEntities(entityScopeCollection, entityGetOptions, callerContext);
        }

        /// <summary>
        /// Gets Entity Ids Collection for the requested Entity Guids
        /// </summary>
        /// <param name="entityUniqueIdList">Indicates the Entity Guids for which Entity Ids are required</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns collection of Entity Ids</returns>
        public Collection<Int64> GetEntityGuidsMap(Collection<Guid> entityUniqueIdList, CallerContext callerContext)
        {
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Collection<Int64> entityIds = new Collection<Int64>();

            try
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                entityIds = new EntityDA().GetEntityGuidsMap(entityUniqueIdList);
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityIds;
        }

        /// <summary>
        /// Gets entity unique identifier collection based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <param name="entityContexts">Indicates entity context</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns entity unique identifier collection</returns>
        public EntityUniqueIdentifierCollection GetEntityUniqueIdentifiers(Int64 entityId, EntityContextCollection entityContexts, CallerContext callerContext)
        {
            EntityUniqueIdentifierCollection entityUniqueIdentifiers = null;
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                var entityDA = new EntityDA();

                entityUniqueIdentifiers = entityDA.GetEntityUniqueIdentifiers(entityId, entityContexts, command);
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityUniqueIdentifiers;
        }

        #endregion

        #region Get Entity Children

        /// <summary>
        ///     Return the entity collection based on entity type
        /// </summary>
        /// <param name="entityId">Indicates entity Id</param>
        /// <param name="entityTypeId">Indicates entity type Id</param>
        /// <returns>Entity Collection</returns>
        public Collection<Int64> GetChildrenByEntityType(Int64 entityId, Int32 entityTypeId)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.GetChildrenByEntityType", MDMTraceSource.EntityGet, false);

            Collection<Int64> entityIdList = new EntityDA().GetChildrenByEntityType(entityId, entityTypeId);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.GetChildrenByEntityType", MDMTraceSource.EntityGet);

            return entityIdList;
        }

        /// <summary>
        ///     Return the entity collection based on entity type
        /// </summary>
        /// <param name="entityIds">Indicates entity Ids</param>
        /// <param name="entityTypeIds">Indicates entity type Ids</param>
        /// <param name="getOnlyChildren">Indicate if parrentIds is unnecessary</param>
        /// <param name="generateEmptyChildren">Option where if no SKU exists, an 'empty SKU' should be created</param>
        /// <param name="cloneAttributesIds"></param>
        /// <param name="emptySkusCollection"></param>
        /// <returns>Entity Collection</returns>
        public Collection<Int64> GetEntitiesChildrenByEntityTypes(Collection<Int64> entityIds, Collection<Int32> entityTypeIds, bool getOnlyChildren, bool generateEmptyChildren, Collection<Int32> cloneAttributesIds, ref EntityCollection emptySkusCollection)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.GetEntitiesChildrenByEntityTypes", false);

            var entityIdList = new Collection<Int64>();

            var entityDA = new EntityDA();
            foreach (long entityId in entityIds)
            {
                var entitySkuIds = new Collection<Int64>();
                if (!getOnlyChildren)
                {
                    entityIdList.Add(entityId);
                }

                foreach (int entityTypeId in entityTypeIds)
                {
                    Collection<Int64> childEntityIds = entityDA.GetChildrenByEntityType(entityId, entityTypeId);
                    entitySkuIds = ValueTypeHelper.MergeCollections(entitySkuIds, childEntityIds);
                }

                entityIdList = ValueTypeHelper.MergeCollections(entityIdList, entitySkuIds);

                if (generateEmptyChildren && entitySkuIds.Count < 1)
                {
                    var entityContext = new EntityContext();
                    entityContext.LoadAttributes = true;
                    entityContext.LoadEntityProperties = true;
                    entityContext.LoadRelationships = false;
                    entityContext.LoadAttributeModels = true;
                    entityContext.AttributeIdList = cloneAttributesIds;

                    Entity en = Get(entityId, entityContext, MDMCenterApplication.VendorPortal, MDMCenterModules.Entity);
                    Entity skuEntity = en.CloneBasicProperties();
                    skuEntity.ParentEntityId = en.Id;
                    skuEntity.ParentEntityName = en.Name;
                    skuEntity.ParentEntityLongName = en.LongName;
                    skuEntity.ParentEntityTypeId = en.EntityTypeId;
                    skuEntity.EntityTypeId = entityTypeIds.FirstOrDefault();
                    skuEntity.Id = 0;
                    if (!cloneAttributesIds.IsNullOrEmpty())
                    {
                        skuEntity.SetAttributeModels(en.AttributeModels);
                        skuEntity.SetAttributes(en.Attributes);
                    }
                    emptySkusCollection.Add(skuEntity);
                }
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.GetEntitiesChildrenByEntityTypes");

            return entityIdList;
        }

        /// <summary>
        ///     Return the entity collection based on entity type
        /// </summary>
        /// <param name="entityIds">Indicates entity Ids</param>
        /// <param name="entityTypeIds">Indicates entity type Ids</param>
        /// <param name="getOnlyChildren"></param>
        /// <returns>Entity Collection</returns>
        public Collection<Int64> GetEntitiesChildrenByEntityTypes(Collection<Int64> entityIds, Collection<Int32> entityTypeIds, bool getOnlyChildren)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.GetEntitiesChildrenByEntityTypes", false);

            var entityDA = new EntityDA();

            var entityIdList = new List<Int64>();

            foreach (long entityId in entityIds)
            {
                if (!getOnlyChildren)
                {
                    entityIdList.Add(entityId);
                }
                foreach (int entityTypeId in entityTypeIds)
                {
                    entityIdList.AddRange(entityDA.GetChildrenByEntityType(entityId, entityTypeId));
                }
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.GetEntitiesChildrenByEntityTypes");

            return new Collection<Int64>(entityIdList.Distinct().ToArray());
        }

        /// <summary>
        /// Gets the entity object along with its hierarchy based on the id and the entity context collection specified.
        /// </summary>
        /// <param name="entityUniqueIdentifier">Specifies the entity identifer data for which the entities needs to be retrieved</param>
        /// <param name="entityContext">Specifies the entity context collection for which entities needs to be retrieved</param>
        /// <param name="entityGetOptions">Specifies the options to be considered while retrieving entity object</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns entity with its hierarchy entities loaded based on the requested context</returns>
        public Entity GetEntityHierarchy(EntityUniqueIdentifier entityUniqueIdentifier, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            var entityHierarchyGetManager = new EntityHierarchyGetManager(this);
            return entityHierarchyGetManager.GetEntityHierarchy(entityUniqueIdentifier, entityContext, entityGetOptions, callerContext);
        }

        /// <summary>
        /// Load the entity hierarchy data inside provided entity object based  on entity context specified
        /// </summary>
        /// <param name="entity">Specifies entity object for which hierarchy has to be loaded</param>
        /// <param name="entityContext">Specifies the entity context collection for which entities needs to be retrieved</param>
        /// <param name="entityGetOptions">Specifies the options to be considered while retrieving entity object</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns true if load is successful otherwise returns false</returns>
        public Boolean LoadEntityHierarchy(Entity entity, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            var entityHierarchyGetManager = new EntityHierarchyGetManager(this);
            return entityHierarchyGetManager.LoadEntityHierarchy(entity, entityContext, entityGetOptions, callerContext);
        }

        /// <summary>
        /// Gets entity variants level based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity variant level to be fetched</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns collection of key value pair with key as entity type id and value as variant level</returns>
        public Dictionary<Int32, Int32> GetEntityVariantLevel(Int64 entityId, CallerContext callerContext)
        {
            return new EntityOperationsBL().GetEntityVariantLevel(entityId, callerContext);
        }

        #endregion

        #region Get Entity Model

        /// <summary>
        ///     Get the meta modal of the entity object from EntityTypeId, ContainerId, CategoryId and Locale
        /// </summary>
        /// <param name="entityTypeId">The entity type Identifier</param>
        /// <param name="containerId">The container Id the entity belongs to</param>
        /// <param name="categoryId">The category id the entity belongs to</param>
        /// <param name="locale">The locale in which the entity object is required</param>
        /// <param name="dataLocales">Collection of locales for which EntityModel has to be populated</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>An empty entity object</returns>
        /// <exception cref="MDMOperationException">
        ///     Thrown if EntityTypeId, ContainerId, CategoryId or Locale is not having proper
        ///     value
        /// </exception>
        public Entity GetModelById(Int32 entityTypeId, Int32 containerId, Int64 categoryId, LocaleEnum locale, Collection<LocaleEnum> dataLocales, CallerContext callerContext)
        {
            #region Parameter Validation

            if (entityTypeId <= 0)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111820", false, callerContext);
                throw new MDMOperationException("111820", _localeMessage.Message, "EntityManager", String.Empty, "GetModelById"); //EntityTypeId must be greater than 0
            }

            if (containerId <= 0)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111821", false, callerContext);
                throw new MDMOperationException("111821", _localeMessage.Message, "EntityManager", String.Empty, "GetModelById"); //ContainerId must be greater than 0
            }

            if (categoryId <= 0)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111822", false, callerContext);
                throw new MDMOperationException("111822", _localeMessage.Message, "EntityManager", String.Empty, "GetModelById"); //CategoryId must be greater than 0
            }

            if (locale == LocaleEnum.UnKnown)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111793", false, callerContext);
                throw new MDMOperationException("111793", _localeMessage.Message, "EntityManager", String.Empty, "GetModelById"); //Locale value cannot be null or unknown
            }

            if (callerContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111823", false, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity));
                throw new MDMOperationException("111823", _localeMessage.Message, "EntityManager", String.Empty, "GetModelById"); //CallerContext is null or empty
            }

            #endregion Parameter Validation

            var entityContext = new EntityContext();
            entityContext.ContainerId = containerId; //  ContainerId
            entityContext.CategoryId = categoryId; //  CategoryId
            entityContext.EntityTypeId = 0; //  EntitytTypeId
            entityContext.Locale = locale; //  Locale
            entityContext.DataLocales = dataLocales;
            entityContext.LoadEntityProperties = true; //  loadEntityProperties, 
            entityContext.LoadAttributes = true; //  loadAttributes
            entityContext.AttributeGroupIdList = null; //  attributeGroupIdList, 
            entityContext.AttributeIdList = null; //  attributeIdList,
            entityContext.LoadRelationships = false; //  loadRelationships
            entityContext.LoadCreationAttributes = false; //  loadAttributesWithShowAtCreationFlag
            entityContext.AttributeModelType = AttributeModelType.All; //  attributeModelType 

            Entity entityObject = GetModel(entityTypeId, categoryId, categoryId, entityContext, callerContext);

            return entityObject;
        }

        /// <summary>
        ///     Get model of entity based on EntityTypeId, CategoryId and EntityIdentifierContext which indicates what all
        ///     information is to be loaded
        /// </summary>
        /// <param name="entityTypeId">Indicates Entity type ID</param>
        /// <param name="categoryId">Indicates category id</param>
        /// <param name="entityContext">Indicates which indicates what all information is to be loaded</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>The Entity object with model loaded</returns>
        public Entity GetModel(Int32 entityTypeId, Int64 categoryId, EntityContext entityContext, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111823", false, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity));
                throw new MDMOperationException("111823", _localeMessage.Message, "EntityManager", String.Empty, "GetModel"); //CallerContext is null or empty
            }

            return GetModel(entityTypeId, categoryId, categoryId, entityContext, callerContext);
        }

        /// <summary>
        ///     Get model of entity based on EntityTypeId, CategoryId, ParentEntityId and EntityContext which indicates what all
        ///     information is to be loaded
        /// </summary>
        /// <param name="entityTypeId">Indicates Entity type ID</param>
        /// <param name="categoryId">Indicates category id</param>
        /// <param name="parentEntityId">Indicates parent entity type id</param>
        /// <param name="entityContext">Indicates entity data context</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>The Entity object with model loaded</returns>
        /// <exception cref="MDMOperationException">
        ///     Thrown if any of parameters like EntityTypeId, CategoryId, ParentEntityId is having value less than 0.
        ///     <para>
        ///         Also Thrown if EntityContext is null or EntityContext.ContainerId is less than 0.
        ///     </para>
        /// </exception>
        public Entity GetModel(Int32 entityTypeId, Int64 categoryId, Int64 parentEntityId, EntityContext entityContext, CallerContext callerContext)
        {
            #region Parameter Validation

            //entityTypeId, categoryId, parentEntityId, entityContext.ContainerId Validation will happen based on conditions.
            //So it is done in AttributeModel. So not keeping here

            if (entityContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111792", false, callerContext);
                throw new MDMOperationException("111792", _localeMessage.Message, "EntityManager", String.Empty, "GetModel"); //EntityContext is null or empty.
            }

            if (callerContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111823", false, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity));
                throw new MDMOperationException("111823", _localeMessage.Message, "EntityManager", String.Empty, "GetModel"); //CallerContext is null or empty.
            }

            #endregion Parameter Validation

            return new EntityModelGetManager().GetModel(entityTypeId, categoryId, parentEntityId, entityContext, callerContext);
        }

        #endregion

        #region Exists Methods

        /// <summary>
        ///     Check the if Entity exists or not.
        /// </summary>
        /// <param name="entity">Entity object which needs to be checked for existence.</param>
        /// <param name="entityId">Indicates the Component Id </param>
        /// <param name="containerId">Indicates the Catalog Id where to check for the Entity.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="excludeSelfReference">Indicates if need to skip self reference</param>
        /// <returns>1 if given entity exists in given context. 0 otherwise</returns>
        public Boolean Exists(Entity entity, Int64 entityId, Int32 containerId, MDMCenterApplication application, MDMCenterModules module, Boolean excludeSelfReference = false)
        {
            #region Parameter Validation

            var callerContext = new CallerContext(application, module);
            if (entity == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityManager", String.Empty, "Exists"); //Entity is null.
            }

            #endregion Parameter Validation

            Int64 entityParentId = entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE ? entity.ParentEntityId : entity.CategoryId;
            var entityMapManager = new EntityMapBL();
            EntityMap entityMap = entityMapManager.Get("Internal", entity.Name, containerId, entity.EntityTypeId, entityParentId, null, application, module);

            if (entityMap != null)
            {
                if (excludeSelfReference && entityId > 0)
                {
                    return entityMap.InternalId != entityId;
                }
                return true;
            }

            return false;
        }

        #endregion Exists

        #region Ensure Methods

        /// <summary>
        /// Ensure entity data for given entity context
        /// </summary>
        /// <param name="entities">Indicates collection of entities</param>
        /// <param name="entityContext">Indicates entity context</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Return true if object is ensured else false</returns>
        public Boolean EnsureEntityData(EntityCollection entities, EntityContext entityContext, CallerContext callerContext)
        {
            return new EnsureEntityDataManager(this).EnsureEntityData(entities, entityContext, callerContext);
        }
        
        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeIds"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns></returns>
        public Boolean EnsureAttributes(Entity entity, IEnumerable<Int32> attributeIds, Boolean loadAttributeModels, CallerContext callerContext)
        {
            return new EnsureEntityDataManager(this).EnsureAttributes(entity, attributeIds, loadAttributeModels, callerContext);
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
            return new EnsureEntityDataManager(this).EnsureAttributes(entity, attributeUniqueIdentifier, loadAttributeModels, callerContext);
        }

        /// <summary>
        ///     Makes sure that expected attributes are loaded. If they are not there, it loads the remaining.
        /// </summary>
        /// <param name="entityCollection">Indicates Entity objects for which we are checking attributes</param>
        /// <param name="attributeUniqueIdentifiers">Attributes unique identifiers</param>
        /// <param name="loadAttributeModels">Load Attribute Models is not supported currently</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Result of operation</returns>
        /// <exception cref="Exception">Thrown when requested attribute is not available</exception>
        public Boolean EnsureAttributes(EntityCollection entityCollection, IEnumerable<AttributeUniqueIdentifier> attributeUniqueIdentifiers, Boolean loadAttributeModels, CallerContext callerContext)
        {
            return new EnsureEntityDataManager(this).EnsureAttributes(entityCollection, attributeUniqueIdentifiers, loadAttributeModels, callerContext);
        }

        /// <summary>
        ///     Makes sure that expected attributes are loaded. If they are not there, it loads the remaining.
        /// </summary>
        /// <param name="entityCollection">Indicates Entity objects for which we are checking attributes</param>
        /// <param name="loadExistingRelationships">Indicates if we need to load existing relationships of given relationship types</param>
        /// <param name="relationshipTypeNames">Relationship Type Names</param>
        /// <param name="attributeUniqueIdentifiers">Relationship Type attibutes unique identifiers</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Result of operation</returns>
        /// <exception cref="Exception">Thrown when requested attribute is not available</exception>
        public Boolean EnsureRelationships(EntityCollection entityCollection, Boolean loadExistingRelationships, Collection<String> relationshipTypeNames, AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext)
        {
            var relationshipTypeIds = new Collection<Int32>();
            var relationshipTypeBL = new RelationshipTypeBL();

            RelationshipTypeCollection relationshipTypes = null;

            if (relationshipTypeNames != null && relationshipTypeNames.Count > 0)
            {
                relationshipTypes = relationshipTypeBL.GetByNames(relationshipTypeNames, callerContext);
            }
            else //load all relationship types if nothing is provided..
            {
                relationshipTypes = relationshipTypeBL.GetAll(callerContext);
            }

            if (relationshipTypes != null && relationshipTypes.Count > 0)
            {
                foreach (var rel in relationshipTypes)
                {
                    relationshipTypeIds.Add(rel.Id);
                }

                return new EnsureEntityDataManager(this).EnsureRelationships(entityCollection, loadExistingRelationships, relationshipTypeIds, null, attributeUniqueIdentifiers, callerContext);
            }

            return false;
        }

        /// <summary>
        ///     Makes sure that expected attributes are loaded. If they are not there, it loads the remaining.
        /// </summary>
        /// <param name="entityCollection">Indicates Entity objects for which we are checking attributes</param>
        /// <param name="loadExistingRelationships">Indicates if we need to load existing relationships of given relationship types</param>
        /// <param name="relationshipTypeIds">Relationship Type Ids</param>
        /// <param name="attributeIds">Relationship attributes Ids</param>
        /// <param name="attributeUniqueIdentifiers">Relationship Type attributes unique identifiers</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Result of operation</returns>
        /// <exception cref="Exception">Thrown when requested attribute is not available</exception>
        public Boolean EnsureRelationships(EntityCollection entityCollection, Boolean loadExistingRelationships, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext)
        {
            return new EnsureEntityDataManager(this).EnsureRelationships(entityCollection, loadExistingRelationships, relationshipTypeIds, attributeIds, attributeUniqueIdentifiers, callerContext);
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
            return new EnsureEntityDataManager(this).EnsureAttributes(entityCollection, attributeModelContext, loadAttributeModels, callerContext);
        }

        /// <summary>
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeIds"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean EnsureAttributes(EntityCollection entityCollection, IEnumerable<Int32> attributeIds, Boolean loadAttributeModels, CallerContext callerContext)
        {
            return new EnsureEntityDataManager(this).EnsureAttributes(entityCollection, attributeIds, null, loadAttributeModels, callerContext);
        }

        /// <summary>
        ///     Ensures inherited values for the entity to be created and also for existing entity based on parent information
        /// </summary>
        /// <param name="entity">Entity for which inherited values needs to be ensured</param>
        /// <param name="callerContext">Object having caller context details</param>
        /// <returns>Flag indicating where ensure is successful or not</returns>
        public Boolean EnsureInheritedAttributes(Entity entity, CallerContext callerContext)
        {
            return new EnsureEntityDataManager(this).EnsureInheritedAttributes(entity, callerContext);
        }

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
            return new EnsureEntityDataManager(this).EnsureEntityHierarchy(entity, entityHierarchyContext, entityGetOptions, callerContext);
        }

        #endregion

        #region Cache utility methods

        /// <summary>
        ///     Removes entire entity from cache, including all attributes and relationships.
        /// </summary>
        /// <param name="entityCacheInvalidateContexts">EntityCacheInvalidateContextCollection to be removed from cache</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Flag for the status of the operation</returns>
        public Boolean RemoveEntityCache(EntityCacheInvalidateContextCollection entityCacheInvalidateContexts, CallerContext callerContext)
        {
            return EntityCacheHelper.RemoveEntityCache(entityCacheInvalidateContexts, this, callerContext);
        }

        #endregion

        #region Load Workflow Details Methods

        /// <summary>
        /// Load the workflow details as part of entity
        /// </summary>
        /// <param name="entity">Indicates the entity</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns the boolean value based on the result</returns>
        public Boolean LoadWorkflowDetails(Entity entity, CallerContext callerContext)
        {
            return _entityGetManager.LoadWorkflowDetails(entity, callerContext.Application, callerContext.Module);
        }

        /// <summary>
        /// Gets the workflow invokable entity ids.
        /// </summary>
        /// <param name="entityIds">The entity ids.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="workflowShortName">Indicates short name of the workflow</param>
        /// <param name="activityLongName">Indicates long name of activity in the given workflow</param>
        /// <returns>Returns WorkflowInvokableEntityInfoCollection object</returns>
        public WorkflowInvokableEntityInfoCollection GetWorkflowInvokableEntityIds(Collection<Int64> entityIds, CallerContext callerContext, String workflowShortName = "", String activityLongName = "")
        {
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            WorkflowInvokableEntityInfoCollection workflowInvokableEntityInfoColl = null;
            
            try
            {
                #region Diagnostics
                if (isTracingEnabled)
                {
                    CallDataContext callDataContext = new CallDataContext();
                    callDataContext.EntityIdList = entityIds;
                    ExecutionContext executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), String.Empty);
                    executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.Entity);
                    diagnosticActivity.Start(executionContext);
                }
                #endregion Diagnostics

                if (entityIds != null && entityIds.Count > 0)
                {
                    // Perform distinct operation if entity ids are more than 1.
                    if (entityIds.Count > 1)
                    {
                        var uniquePromotedEntityIds = entityIds.Distinct();
                        entityIds = uniquePromotedEntityIds.ToCollection<Int64>();
                    }

                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                    workflowInvokableEntityInfoColl = new EntityDA().GetWorkflowInvokableEntityIds(entityIds, workflowShortName, activityLongName, command, callerContext);

                    if(workflowInvokableEntityInfoColl == null)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "114549", false, callerContext); // Provided entity ids are invalid as they do not exist in the system.
                        throw new MDMOperationException(_localeMessage.Code, _localeMessage.Message, "EntityManager", String.Empty, "GetWorkflowInvokableEntityIds");
                    }
                }
            }
            finally
            {
                #region Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return workflowInvokableEntityInfoColl;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Throws exception if entity collection is null or empty
        /// </summary>
        /// <param name="entities">Indicates collection of entity to validate</param>
        /// <param name="callerContext">Indicates application and module name that requested the action to be performed</param>
        /// <param name="diagnosticActivity">Indicates activity object that logs error, information, warnings, etc</param>
        /// <param name="action">Indicates action being performed</param>
        private void ValidateEntityCollection(EntityCollection entities, CallerContext callerContext, DiagnosticActivity diagnosticActivity, String action)
        {
            if (entities == null || entities.Count < 1)
            {
                diagnosticActivity.LogError("111816", String.Empty);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111816", false, callerContext);
                throw new MDMOperationException("111816", _localeMessage.Message, "EntityManager", String.Empty, action);
            }
        }

        #endregion

        #endregion Methods
    }
}