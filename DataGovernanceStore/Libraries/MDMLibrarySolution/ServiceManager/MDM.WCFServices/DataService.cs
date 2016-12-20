using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using Core;
    using Core.Exceptions;
    using WCFServiceInterfaces;
    using BusinessObjects;
    using BusinessObjects.Imports;
    using BusinessObjects.Diagnostics;
    using AdminManager.Business;
    using EntityManager.Business;
    using Utility;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class DataService : MDMWCFBase, IDataService
    {
        #region Constructors

        public DataService()
            : base(true)
        {

        }

        public DataService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region Entity Methods

        /// <summary>
        /// Get model of entity based on EntityTypeId, CategoryId, ParentEntityId and EntityContext which indicates what all information is to be loaded
        /// </summary>
        /// <param name="entityContext">Indicates entity data context</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>The Entity object with model loaded</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of parameters like EntityTypeId, CategoryId, ParentEntityId is having value less than 0.
        /// <para>
        /// Also Thrown if EntityContext is null or EntityContext.ContainerId is less than 0.
        /// </para>
        /// </exception>
        public Entity GetEntityModel(EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.GetEntityModel", MDMTraceSource.EntityGet, false);

            if (entityContext == null)
            {
                throw new ArgumentNullException("entityContext");
            }

            Entity entityObject = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'GetEntityModel' request message.", MDMTraceSource.EntityGet);

                EntityBL entityManager = new EntityBL();
                entityObject = entityManager.GetModel(entityContext.EntityTypeId, entityContext.CategoryId, entityContext.CategoryId, entityContext, new CallerContext(application, module));

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'GetEntityModel' response message.", MDMTraceSource.EntityGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.GetEntityModel", MDMTraceSource.EntityGet);
            }

            return entityObject;
        }

        /// <summary>
        /// Obtain an Entity from the MDM system
        /// </summary>
        /// <param name="entityId">The entity identifier</param>
        /// <param name="entityContext">The data context for which entity needs to be fetched</param>
        /// <param name="entityGetOptions">Indicates the options available while retrieving entity data</param>
        /// <param name="callerContext">Specifies caller context</param>
        /// <returns>Entity having given EntityId and attributes, relationship, hierarchy relationships baesd on EntityContext</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0</exception>
        /// <exception cref="MDMOperationException">Thrown if EntityContext is null</exception>
        /// <exception cref="MDMOperationException">Thrown if none of the EntityContext's properties like LoadEntityProperties, LoadAttributes or LoadRelationships is set to true</exception>
        public Entity GetEntity(Int64 entityId, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityBL, Entity>("GetEntity",
                businessLogicBase => businessLogicBase.Get(entityId, entityContext, entityGetOptions, callerContext));
        }

        /// <summary>
        /// Obtain an Entity from the MDM system
        /// </summary>
        /// <param name="entityId">The entity identifier</param>
        /// <param name="entityContext">The data context for which entity needs to be fetched</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Entity having given EntityId and attributes, relationship, hierarchy relationships baesd on EntityContext</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0</exception>
        /// <exception cref="MDMOperationException">Thrown if EntityContext is null</exception>
        /// <exception cref="MDMOperationException">Thrown if none of the EntityContext's properties like LoadEntityProperties, LoadAttributes or LoadRelationships is set to true</exception>
        public Entity GetEntity(Int64 entityId, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            return MakeBusinessLogicCall<EntityBL, Entity>(
                bl => bl.Get(entityId, entityContext, application, module, publishEvents, applyAVS),
                context =>
                {
                    context.CallerContext.Application = application;
                    context.CallerContext.Module = module;
                    context.CallDataContext.EntityIdList.Add(entityId);
                    FillFromEntityContext(context.CallDataContext, entityContext);
                });
        }

        /// <summary>
        /// Gets entity objects for the requested entity ids
        /// </summary>
        /// <param name="entityIdList">Entity ids for which entity objects are required</param>
        /// <param name="entityContext">The data context for which entity needs to be fetched</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>EntityCollection for given Entity Ids and thier attributes, relationship, hierarchy relationships baesd on EntityContext</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0</exception>
        /// <exception cref="MDMOperationException">Thrown if EntityContext is null</exception>
        /// <exception cref="MDMOperationException">Thrown if none of the EntityContext's properties like LoadEntityProperties, LoadAttributes or LoadRelationships is set to true</exception>
        public EntityCollection GetEntities(Collection<Int64> entityIdList, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            Boolean loadLatest = false;
            return MakeBusinessLogicCall<EntityBL, EntityCollection>(
                bl => bl.Get(entityIdList, entityContext, loadLatest, application, module, publishEvents, applyAVS),
                context =>
                {
                    context.CallerContext.Application = application;
                    context.CallerContext.Module = module;
                    if (entityIdList != null)
                    {
                        context.CallDataContext.EntityIdList = entityIdList;
                    }
                    FillFromEntityContext(context.CallDataContext, entityContext);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection GetEntities(Collection<Int64> entityIdList, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityBL, EntityCollection>(
                bl => bl.Get(entityIdList, entityContext, entityGetOptions, callerContext),
                context =>
                {
                    if (entityIdList != null)
                    {
                        context.CallDataContext.EntityIdList = entityIdList;
                    }
                    FillFromEntityContext(context.CallDataContext, entityContext);
                });
        }

        /// <summary>
        /// Create and entity object after getting the model from GetEntityModel
        /// </summary>
        /// <param name="entity">The filled entity object without ids</param>
        /// <param name="entityProcessingOptions">Instance of Processing Option for Processing Entities</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="programName">Indicates from which program Process method is being called. This would be used for log purpose.</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>The operation result for the create</returns>
        public EntityOperationResult CreateEntity(Entity entity, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.CreateEntity", MDMTraceSource.EntityProcess, false);

            EntityOperationResult createOperationResult = null;

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'Create Entity' request message.", MDMTraceSource.EntityProcess);

                EntityBL entityManager = new EntityBL();
                createOperationResult = entityManager.Create(entity, entityProcessingOptions, programName, application, module);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'Create Entity' response message.", MDMTraceSource.EntityProcess);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop trace activity
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.CreateEntity", MDMTraceSource.EntityProcess);
            }

            return createOperationResult;
        }

        /// <summary>
        /// Create new entities in MDM system from the given instance
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Instance of Processing Option for Processing Entities</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="programName">Indicates from which program Process method is being called. This would be used for log purpose.</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public EntityOperationResultCollection CreateEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.CreateEntities", MDMTraceSource.EntityProcess, false);

            EntityOperationResultCollection createOperationResults = null;

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'Create Entities' request message.", MDMTraceSource.EntityProcess);

                EntityBL entityManager = new EntityBL();
                createOperationResults = entityManager.Create(entities, entityProcessingOptions, programName, application, module);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'Create Entities' response message.", MDMTraceSource.EntityProcess);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop trace activity
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.CreateEntity", MDMTraceSource.EntityProcess);
            }

            return createOperationResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="context"></param>
        /// <param name="processingOptions">Instance of Processing Option for Processing Entities</param>
        /// <returns></returns>
        public EntityOperationResult UpdateEntityByContext(Entity entity, CallerContext context, EntityProcessingOptions processingOptions)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity curActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                if (context != null)
                {
                    ExecutionContext ctxt = new ExecutionContext();
                    ctxt.CallerContext = context;
                    curActivity.Start(ctxt);
                }
                else
                    curActivity.Start();
            }

            EntityOperationResult updateOperationResult = null;

            try
            {
                EntityBL entityManager = new EntityBL();
                updateOperationResult = entityManager.Update(entity, processingOptions, context.ProgramName, context.Application, context.Module);
            }
            catch (Exception ex)
            {
                curActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    curActivity.Stop();
                }
            }

            return updateOperationResult;
        }

        /// <summary>
        /// Update an already existing entity, this requires the entity be obtained
        /// first by calling the GetById or GetByEntityUniqueIdentifier
        /// </summary>
        /// <param name="entity">The entity object to be updated </param>
        /// <param name="entityProcessingOptions">Instance of Processing Option for Processing Entities</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="programName">The program which has requested for update</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>The operation result for the update</returns>
        public EntityOperationResult UpdateEntity(Entity entity, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            TraceSettings curSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            ExecutionContext executionContext = new ExecutionContext();

            //TODO: Wiring has to be done with context.
            CallerContext context = new CallerContext();

            context.ProgramName = "EntityHierarchy";
            context.Module = MDMCenterModules.Entity;
            context.Application = MDMCenterApplication.PIM;

            //End TODO

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (context != null)
            {
                executionContext.CallerContext = context;

                curSettings = context.TraceSettings;
            }

            if (curSettings != null && curSettings.IsTracingEnabled)
            {
                if (context != null)
                {
                    executionContext.CallDataContext = new CallDataContext();
                    executionContext.CallDataContext.EntityIdList.Add(entity.Id);

                    executionContext.SecurityContext = new SecurityContext();
                    executionContext.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(executionContext);
                }
                else
                    currentActivity.Start();
            }

            EntityOperationResult updateOperationResult = null;

            try
            {
                EntityBL entityManager = new EntityBL();
                updateOperationResult = entityManager.Update(entity, entityProcessingOptions, programName, application, module);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.ToString());
                throw base.WrapException(ex);
            }
            finally
            {
                if (curSettings != null && curSettings.IsBasicTracingEnabled)
                    currentActivity.Stop();
            }

            return updateOperationResult;
        }

        /// <summary>
        /// Update entities in MDM system from the given instance
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Instance of Processing Option for Processing Entities</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="programName">Indicates from which program Process method is being called. This would be used for log purpose.</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public EntityOperationResultCollection UpdateEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeBusinessLogicCall<EntityBL, EntityOperationResultCollection>(
                bl => bl.Update(entities, entityProcessingOptions, programName, application, module),
                context =>
                {
                    context.CallerContext.ProgramName = programName;
                    context.CallerContext.Application = application;
                    context.CallerContext.Module = module;
                    if (entities != null)
                    {
                        foreach (Entity entity in entities)
                        {
                            FillFromEntity(context.CallDataContext, entity);
                        }
                    }
                });
        }

        /// <summary>
        /// Delete an already existing entity.
        /// </summary>
        /// <param name="entity">The entity object to be deleted </param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="programName">The program which has requested for delete</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>The operation result for the delete</returns>
        public EntityOperationResult DeleteEntity(Entity entity, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            TraceSettings curSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            ExecutionContext execCtxt = new ExecutionContext();

            //TODO: Wiring has to be done with context.
            CallerContext context = new CallerContext();

            context.ProgramName = "EntityHierarchy";
            context.Module = MDMCenterModules.Entity;
            context.Application = MDMCenterApplication.PIM;

            //End TODO

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (context != null)
            {
                execCtxt.CallerContext = context;

                curSettings = context.TraceSettings;
            }

            if (curSettings != null && curSettings.IsTracingEnabled)
            {
                if (context != null)
                {
                    execCtxt.CallDataContext = new CallDataContext();
                    execCtxt.CallDataContext.EntityIdList.Add(entity.Id);

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }

            EntityOperationResult deleteOperationResult = null;

            try
            {
                EntityBL entityManager = new EntityBL();
                deleteOperationResult = entityManager.Delete(entity, programName, application, module);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.ToString());
                throw base.WrapException(ex);
            }
            finally
            {
                if (curSettings != null && curSettings.IsBasicTracingEnabled)
                    currentActivity.Stop();
            }

            return deleteOperationResult;
        }

        /// <summary>
        /// Delete an already existing entities.
        /// </summary>
        /// <param name="entities">The entities objects to be deleted </param>
        /// <param name="programName">The program which has requested for delete</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>The operation result for the delete</returns>
        public EntityOperationResultCollection DeleteEntities(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.DeleteEntities", MDMTraceSource.EntityProcess, false);

            EntityOperationResultCollection deleteOperationResultCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'Delete Entities' request message.", MDMTraceSource.EntityProcess);

                EntityBL entityManager = new EntityBL();
                deleteOperationResultCollection = entityManager.Delete(entities, programName, application, module);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'Delete Entities' response message.", MDMTraceSource.EntityProcess);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop trace activity
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.DeleteEntities", MDMTraceSource.EntityProcess);
            }

            return deleteOperationResultCollection;
        }

        /// <summary>
        /// Process given list of entities based on their actions
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Instance of Processing Option for Processing Entities</param>
        /// <param name="programName">Indicates from which program Process method is being called. This would be used for log purpose.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="ArgumentNullException">If Entities collection is null or operation result is null</exception>
        public EntityOperationResultCollection ProcessEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext() { Application = application, Module = module, ProgramName = programName };
            return this.ProcessEntities(entities, entityProcessingOptions, callerContext);
        }

        /// <summary>
        /// Process given list of entities based on their actions
        /// </summary>
        /// <param name="entities">Instance of entities to be processed in MDM system</param>
        /// <param name="entityProcessingOptions">Instance of Processing Option for Processing Entities</param>
        /// <param name="callerContext">Instance of processing option for Processing entities</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="ArgumentNullException">If Entities collection is null or operation result is null</exception>
        public EntityOperationResultCollection ProcessEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
        {
            Boolean isTracingEnabled = false;
            DiagnosticActivity diagnosticActivity = null;

            EntityOperationResultCollection entityOperationResultCollection = null;

            try
            {
                isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsTracingEnabled;

                if (isTracingEnabled)
                {
                    diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.APIFramework);
                    foreach (Entity entity in entities)
                    {
                        FillFromEntity(diagnosticActivity.ExecutionContext.CallDataContext, entity);
                    }
                    diagnosticActivity.Start();
                    diagnosticActivity.LogInformation("DataService receives 'Process Entities' request message.");
                }

                EntityBL entityManager = new EntityBL();
                entityOperationResultCollection = entityManager.Process(entities, entityProcessingOptions, callerContext);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData(
                        "DataService sends 'Process Entities' response message. See 'View Data' for operation result details.",
                        entityOperationResultCollection != null ? entityOperationResultCollection.ToXml() : "EntityOperationResultCollection is null",
                        MessageClassEnum.Information);
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityOperationResultCollection;
        }
        
        /// <summary>
        /// Get Entity By given ExternalId
        /// </summary>
        /// <param name="externalId">ExternalId for which Entity needs to be Populate</param>
        /// <param name="entityContext">entityContext for which Entity needs to be Populated</param>
        /// <param name="context">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Entity Object</returns>
        public Entity GetEntityByExternalId(String externalId, EntityContext entityContext, CallerContext context)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.GetEntityByExternalId", MDMTraceSource.EntityGet, false);

            if (String.IsNullOrWhiteSpace(externalId))
            {
                throw new ArgumentNullException("externalId");
            }

            Entity entityObject = null;

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'GetEntityByExternalId' request message.", MDMTraceSource.EntityGet);

                EntityBL entityManager = new EntityBL();
                entityObject = entityManager.GetByExternalId(externalId, entityContext, context);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'GetEntityByExternalId' response message.", MDMTraceSource.EntityGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.GetEntityByExternalId", MDMTraceSource.EntityGet);
            }

            return entityObject;
        }

        /// <summary>
        /// Get Entity By given ExternalId
        /// </summary>
        /// <param name="externalId">ExternalId for which Entity needs to be Populate</param>
        /// <param name="entityContext">entityContext for which Entity needs to be Populated</param>
        /// <param name="context">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Entity Object</returns>
        public Entity GetEntityByExternalId(String externalId, EntityContext entityContext, CallerContext context, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.GetEntityByExternalId", MDMTraceSource.EntityGet, false);

            if (String.IsNullOrWhiteSpace(externalId))
            {
                throw new ArgumentNullException("externalId");
            }

            Entity entityObject = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'GetEntityByExternalId' request message.", MDMTraceSource.EntityGet);

                EntityBL entityManager = new EntityBL();
                entityObject = entityManager.GetByExternalId(externalId, entityContext, context, publishEvents, applyAVS);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'GetEntityByExternalId' response message.", MDMTraceSource.EntityGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.GetEntityByExternalId", MDMTraceSource.EntityGet);
            }

            return entityObject;
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
            return MakeBusinessLogicCall<EntityBL, EntityCollection>("GetEntitiesByExternalIds",
                businessLogicBase => businessLogicBase.GetEntitiesByExternalIds(externalIdList, entityContext, callerContext, publishEvents, applyAVS));
        }
        
        /// <summary>
        /// Update bulk entity attributes
        /// </summary>
        /// <param name="templateEntities">Indicates template entities</param>
        /// <param name="entityIdsToProcess">Indicates entity ids to be processed</param>
        /// <param name="actionPerformed">Indicates which action should be performed</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns entity operation result after updating all entities attributes</returns>
        public EntityOperationResultCollection BulkUpdateEntityAttributes(EntityCollection templateEntities, Collection<Int64> entityIdsToProcess, String actionPerformed, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityBL, EntityOperationResultCollection>(
                bl => bl.BulkUpdateEntityAttributes(templateEntities, entityIdsToProcess, actionPerformed, callerContext),
                context =>
                {
                    if (templateEntities != null)
                    {
                        foreach (Entity entity in templateEntities)
                        {
                            FillFromEntity(context.CallDataContext, entity);
                        }
                    }
                    context.CallDataContext.EntityIdList = entityIdsToProcess;
                });
        }
        
        #endregion

        #region Entity Locale Methods

        /// <summary>
        ///  Process given list of entities based on their actions and data locales
        /// </summary>
        /// <param name="dataXml">XML representing Entity - ID, Locale and action</param>
        /// <param name="systemDataLocale">Indicates the system data locale</param>
        /// <param name="returnResult">Whether need to return result or not </param>
        /// <param name="callerContext">Indicates the CallerContext</param>
        /// <returns>Returns EntityOperationResultCollection</returns>
        public EntityOperationResultCollection ProcessEntityLocale(String dataXml, String systemDataLocale, Boolean returnResult, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.ProcessEntityLocale", false);


            EntityOperationResultCollection entityOperationResultCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'ProcessEntityLocale' request message.");

                EntityLocaleBL entityLocaleBL = new EntityLocaleBL();
                entityOperationResultCollection = entityLocaleBL.Process(dataXml, systemDataLocale, returnResult, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'ProcessEntityLocale' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.ProcessEntityLocale");
            }

            return entityOperationResultCollection;

        }

        /// <summary>
        /// Updating the entity operation result collection
        /// </summary>
        /// <param name="entityId">Indicates the Entity Id</param>
        /// <param name="datalocales">Indicates the data locales</param>
        /// <param name="callerContext">Indicates the CallerContext</param>
        /// <returns>Returns EntityCollection</returns>
        public EntityCollection GetEntityLocale(Int64 entityId, Collection<Locale> datalocales, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.GetEntityLocale", false);

            EntityCollection entities = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'GetEntityLocale' request message.");

                EntityLocaleBL entityLocaleBL = new EntityLocaleBL();
                entities = entityLocaleBL.Get(entityId, datalocales, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'GetEntityLocale' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.GetEntityLocale");
            }

            return entities;
        }

        #endregion
        
        #region Entity Hierarchy Methods

        /// <summary>
        /// Gets the entity object along with its hierarchy based on the entity identifier and the entity context specified.
        /// </summary>
        /// <param name="entityUniqueIdentifier">Specifies the entity unique identifier for which entity hierarchy needs to be retrieved</param>
        /// <param name="entityContext">Specifies the entity context collection for which entities needs to be retrieved</param>
        /// <param name="entityGetOptions">Specifies the options to be considered while retrieving entity object</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns entity with its hierarchy entities loaded based on the requested context</returns>
        public Entity GetEntityHierarchy(EntityUniqueIdentifier entityUniqueIdentifier, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityBL, Entity>("GetEntityHierarchy", businessLogicBase => businessLogicBase.GetEntityHierarchy(entityUniqueIdentifier, entityContext, entityGetOptions, callerContext));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="childEntityTypeIds"></param>
        /// <returns></returns>
        public Collection<Int64> GetChildEntitiesIdsByEntityType(Collection<Int64> entityIds, Collection<Int32> childEntityTypeIds)
        {
            return MakeBusinessLogicCall<EntityBL, Collection<Int64>>(
                bl => bl.GetEntitiesChildrenByEntityTypes(entityIds, childEntityTypeIds, true),
                context =>
                {
                    context.CallDataContext.EntityIdList = entityIds;
                    context.CallDataContext.EntityTypeIdList = childEntityTypeIds;
                });
        }
        
        #endregion Entity Hierarchy Methods

        #region File Methods

        /// <summary>
        /// Gets a file object based on given file id
        /// </summary>
        /// <param name="fileId">file id</param>
        /// <param name="getOnlyFileDetails">a flag to indicate if only file details are required(true) or file data is also required along with details(false)</param>
        /// <returns></returns>
        public File GetFile(Int32 fileId, Boolean getOnlyFileDetails)
        {
            File file = null;

            try
            {
                FileBL fileManager = new FileBL();
                file = fileManager.GetFile(fileId, getOnlyFileDetails);
            }
            catch (ArgumentNullException ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            catch (ArgumentException ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return file;
        }

        /// <summary>
        /// Returns files using specified filter <paramref name="fileIdsFilter"/>.
        /// </summary>
        /// <param name="fileIdsFilter">Indicates ids of files. All files will be returned if filter is empty.</param>
        /// <param name="getOnlyFileDetails">Indicates file content requesting status. Please set to True if you want only files metadata information except file content.</param>
        /// <returns>Returns collection of files</returns>
        public FileCollection GetFiles(Collection<Int32> fileIdsFilter, Boolean getOnlyFileDetails)
        {
            return MakeBusinessLogicCall<FileBL, FileCollection>("GetFiles", businessLogic => businessLogic.GetFiles(fileIdsFilter, getOnlyFileDetails));
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="file">Indicates the  File details</param>
        /// <returns>The process status</returns>
        public Int32 ProcessFile(File file)
        {
            int processStatus = 0;
            FileBL fileBL = new FileBL();

            CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.UIProcess);
            processStatus = fileBL.Process(file, callerContext);
            return processStatus;
        }

        #endregion File Methods

        #region EntityMap Contracts

        /// <summary>
        /// Gets entity map for requested external Id
        /// </summary>
        /// <param name="systemId">External system Id</param>
        /// <param name="externalId">Entity external Id</param>
        /// <param name="containerId">Container Id of the entity</param>
        /// <param name="entityTypeId">Entity type Id of the entity</param>
        /// <param name="categoryId">Category Id of the entity</param>
        /// <param name="entityIdentificationMap">Entity Identification Mappings</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Entity Map object</returns>
        public EntityMap GetEntityMap(String systemId, String externalId, Int32 containerId, Int32 entityTypeId, Int64 categoryId, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            EntityMap entityMap = null;

            try
            {
                EntityMapBL entityMapManager = new EntityMapBL();
                entityMap = entityMapManager.Get(systemId, externalId, containerId, entityTypeId, categoryId, entityIdentificationMap, application, module);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return entityMap;
        }

        /// <summary>
        /// Loads entity maps with the entity internals
        /// </summary>
        /// <param name="entityMapCollection">Collection of entity maps which needs to be loaded</param>
        /// <param name="entityIdentificationMap">Entity Identification Mappings</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>collection of entity maps</returns>
        public EntityMapCollection LoadInternalDetails(EntityMapCollection entityMapCollection, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            try
            {
                EntityMapBL entityMapManager = new EntityMapBL();
                entityMapManager.LoadInternalDetails(entityMapCollection, entityIdentificationMap, application, module);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return entityMapCollection;
        }

        #endregion

        #region Cache Management Methods

        /// <summary>
        /// Removes entire entity from cache, including all Attributes and relationships.
        /// </summary>
        /// <param name="entityCacheInvalidateContexts">EntityCacheInvalidateContextCollection to be removed from cache</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Flag for the status of the operation</returns>
        public Boolean RemoveEntityCache(EntityCacheInvalidateContextCollection entityCacheInvalidateContexts, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityBL, Boolean>("RemoveEntityCache", businessLogic =>
                businessLogic.RemoveEntityCache(entityCacheInvalidateContexts, callerContext));
        }

        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// Makes calls of Data Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(String methodName, Func<TBusinessLogic, TResult> call) where TBusinessLogic : BusinessLogicBase, new()
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("DataService." + methodName, false);
            }

            TResult operationResult;

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                                  "DataService receives" + methodName + " request message.");
                }

                operationResult = call(new TBusinessLogic());

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                                  "DataService receives" + methodName + " response message.");
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("DataService." + methodName);
                }
            }

            return operationResult;
        }

        private void FillFromEntity(CallDataContext callDataContext, Entity entity)
        {
            callDataContext.EntityIdList.Add(entity.Id);

            if (entity.CategoryId > 0 && !callDataContext.CategoryIdList.Contains(entity.CategoryId))
            {
                callDataContext.CategoryIdList.Add(entity.CategoryId);
            }

            if (entity.ContainerId > 0 && !callDataContext.ContainerIdList.Contains(entity.ContainerId))
            {
                callDataContext.ContainerIdList.Add(entity.ContainerId);
            }

            if (entity.EntityTypeId > 0 && !callDataContext.EntityTypeIdList.Contains(entity.EntityTypeId))
            {
                callDataContext.EntityTypeIdList.Add(entity.EntityTypeId);
            }

            if (entity.OrganizationId > 0 && !callDataContext.OrganizationIdList.Contains(entity.OrganizationId))
            {
                callDataContext.OrganizationIdList.Add(entity.OrganizationId);
            }

            if (!callDataContext.LocaleList.Contains(entity.Locale))
            {
                callDataContext.LocaleList.Add(entity.Locale);
            }
            if (entity.Attributes != null)
            {
                foreach (Attribute attribute in entity.Attributes)
                {
                    if (!callDataContext.AttributeIdList.Contains(attribute.Id))
                    {
                        callDataContext.AttributeIdList.Add(attribute.Id);
                    }
                }
            }
        }

        private void FillFromEntityContext(CallDataContext callDataContext, EntityContext entityContext)
        {
            if (entityContext != null)
            {
                callDataContext.LocaleList.Add(entityContext.Locale);
                if (entityContext.CategoryId > 0)
                {
                    callDataContext.CategoryIdList.Add(entityContext.CategoryId);
                }
                if (entityContext.ContainerId > 0)
                {
                    callDataContext.ContainerIdList.Add(entityContext.ContainerId);
                }
                if (entityContext.EntityTypeId > 0)
                {
                    callDataContext.EntityTypeIdList.Add(entityContext.EntityTypeId);
                }
                if (entityContext.AttributeIdList != null)
                {
                    callDataContext.AttributeIdList = entityContext.AttributeIdList;
                }
            }
        }

        #endregion #region Private Methods

    }
}