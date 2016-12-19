﻿using System;
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
    using RelationshipManager.Business;
    using SearchManager.Business;
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

        #region Ensure Attribute methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeIds"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Entity EnsureAttributes(Entity entity, IEnumerable<Int32> attributeIds, Boolean loadAttributeModels, CallerContext callerContext)
        {
            try
            {
                //No need to store boolean result of calling API,client is taking care of result based on entity object
                EntityBL entityBL = new EntityBL();
                entityBL.EnsureAttributes(entity, attributeIds, loadAttributeModels, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeUniqueIdentifier"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Entity EnsureAttributes(Entity entity, AttributeUniqueIdentifier attributeUniqueIdentifier, Boolean loadAttributeModels, CallerContext callerContext)
        {
            try
            {
                //No need to store boolean result of calling API,client is taking care of result based on entity object
                EntityBL entityBL = new EntityBL();
                entityBL.EnsureAttributes(entity, attributeUniqueIdentifier, loadAttributeModels, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeIds"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection EnsureAttributes(EntityCollection entityCollection, IEnumerable<Int32> attributeIds, Boolean loadAttributeModels, CallerContext callerContext)
        {
            try
            {
                //No need to store boolean result of calling API,client is taking care of result based on entity object
                EntityBL entityBL = new EntityBL();
                entityBL.EnsureAttributes(entityCollection, attributeIds, loadAttributeModels, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return entityCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeUniqueIdentifiers"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection EnsureAttributes(EntityCollection entityCollection, Collection<AttributeUniqueIdentifier> attributeUniqueIdentifiers, Boolean loadAttributeModels, CallerContext callerContext)
        {
            try
            {
                //No need to store boolean result of calling API,client is taking care of result based on entity object
                EntityBL entityBL = new EntityBL();
                entityBL.EnsureAttributes(entityCollection, attributeUniqueIdentifiers, loadAttributeModels, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return entityCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeModelContext"></param>
        /// <param name="loadAttributeModels"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection EnsureAttributes(EntityCollection entityCollection, AttributeModelContext attributeModelContext, Boolean loadAttributeModels, CallerContext callerContext)
        {
            try
            {
                //No need to store boolean result of calling API,client is taking care of result based on entity object
                EntityBL entityBL = new EntityBL();
                entityBL.EnsureAttributes(entityCollection, attributeModelContext, loadAttributeModels, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return entityCollection;
        }

        #endregion

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
        /// Get Where Used Relationships for given entity id
        /// </summary>
        /// <param name="entityId">Provide entity id</param>
        /// <param name="relationshipTypeId">Provide Relationship Type Id. Set to '0' if we want whereused relationships across relationship types.</param>
        /// <param name="attributeIds">Indicates Id of attributes which needs to be loaded with RelatedEntity</param>
        /// <param name="dataLocale">Indicates data locale</param>
        /// <param name="callerContext">Provide caller context</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsedRelationships(Int64 entityId, Int32 relationshipTypeId, Collection<Int32> attributeIds, LocaleEnum dataLocale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipBL, RelationshipCollection>("GetWhereUsedRelationships", businessLogic =>
                   businessLogic.GetWhereUsed(entityId, relationshipTypeId, attributeIds, dataLocale, new EntityBL(), callerContext));
        }

        /// <summary>
        /// Get Where Used Relationships for given entity id
        /// </summary>
        /// <param name="entityIds">Provide entity ids</param>
        /// <param name="relationshipTypeId">Provide Relationship Type Id. Set to '0' if we want whereused relationships across relationship types.</param>
        /// <param name="attributeIds">Indicates Id of attributes which needs to be loaded with RelatedEntity</param>
        /// <param name="dataLocale">Indicates data locale</param>
        /// <param name="callerContext">Provide caller context</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsedRelationships(Collection<Int64> entityIds, Int32 relationshipTypeId, Collection<Int32> attributeIds, LocaleEnum dataLocale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipBL, RelationshipCollection>("GetWhereUsedRelationships", businessLogic =>
                   businessLogic.GetWhereUsed(entityIds, relationshipTypeId, attributeIds, dataLocale, new EntityBL(), callerContext));
        }

        /// <summary>
        /// Get Where Used Relationships for given entity id
        /// </summary>
        /// <param name="entityId">Provide entity id</param>
        /// <param name="relationshipTypeIds">Provide Relationship Type Id. Set to '0' if we want whereused relationships across relationship types.</param>
        /// <param name="attributeIds">Indicates Id of attributes which needs to be loaded with RelatedEntity</param>
        /// <param name="dataLocale">Indicates data locale</param>
        /// <param name="callerContext">Provide caller context</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsedRelationships(Int64 entityId, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, LocaleEnum dataLocale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipBL, RelationshipCollection>("GetWhereUsedRelationships", businessLogic =>
                   businessLogic.GetWhereUsed(entityId, relationshipTypeIds, attributeIds, dataLocale, new EntityBL(), callerContext));
        }

        /// <summary>
        /// Get Where Used Relationships for given entity id
        /// </summary>
        /// <param name="entityIds">Provide entity id</param>
        /// <param name="relationshipTypeIds">Provide Relationship Type Id. Set to '0' if we want whereused relationships across relationship types.</param>
        /// <param name="attributeIds">Indicates Id of attributes which needs to be loaded with RelatedEntity</param>
        /// <param name="dataLocale">Indicates data locale</param>
        /// <param name="callerContext">Provide caller context</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsedRelationships(Collection<Int64> entityIds, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, LocaleEnum dataLocale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipBL, RelationshipCollection>("GetWhereUsedRelationships", businessLogic =>
                   businessLogic.GetWhereUsed(entityIds, relationshipTypeIds, attributeIds, dataLocale, new EntityBL(), callerContext));
        }

        /// <summary>
        /// Get Relationship Model
        /// </summary>
        /// <param name="relationshipTypeId">Provide Relationship Type Id to filter usage for a given relationship type.</param>
        /// <param name="containerId">Indicates Container Id</param>
        /// <param name="locales">Indicates the collection locales</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Relationship object</returns>
        public Relationship GetRelationshipModel(Int32 relationshipTypeId, Int32 containerId, Collection<LocaleEnum> locales, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataService.GetRelationshipModel", false);

            Relationship relationship = new Relationship();

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService receives 'GetRelationshipModel' request message.");

                RelationshipBL relationshipManager = new RelationshipBL();
                relationship = relationshipManager.GetRelationshipModel(relationshipTypeId, containerId, locales, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataService sends 'GetRelationshipModel' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataService.GetRelationshipModel");
            }

            return relationship;
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

        /// <summary>
        /// Ensures the relationship tree denorm for the non-denormalized entities.
        /// </summary>
        /// <param name="entityIdList">Entities for which relationship needs to be denormalized</param>
        /// <param name="entityContext">Specifies EntityContext which indicates what all information is to be loaded in Entity object</param>
        /// <param name="processWhereUsed">Boolean flag to determine if we have to process where used for current entity or not.</param>
        /// <param name="processImpactedExtensions">Boolean flag to determine if we have to process impacted extensions for current entity or not.</param>
        /// <param name="processImpactedHierarchies">Boolean flag to determine if we have to process processImpactedHierarchies for current entity or not</param>
        /// <param name="processRelationshipTree">Boolean flag to determine if we have to process processInheritable for current entity or not</param>
        /// <param name="processInheritable">Boolean flag to determine if we have to process processInheritable for current entity or not</param>
        /// <param name="processRelationshipAttributes">Boolean flag to determine if we have to process processRelationshipAttributes for current entity or not</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns></returns>
        public EntityOperationResultCollection EnsureInheritedEntityRelationships(Collection<Int64> entityIdList, EntityContext entityContext, Boolean processWhereUsed, Boolean processImpactedExtensions, Boolean processImpactedHierarchies, Boolean processRelationshipTree, Boolean processInheritable, Boolean processRelationshipAttributes, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipDenormBL, EntityOperationResultCollection>("EnsureInheritedEntityRelationships", businessLogic =>
                   businessLogic.EnsureInheritedEntityRelationships(entityIdList, entityContext, processWhereUsed, processImpactedExtensions,
                            processImpactedHierarchies, processRelationshipTree, processInheritable, processRelationshipAttributes,
                            new EntityBL(), new ImpactedEntityBL(), callerContext));
        }


        /// <summary>
        /// Queues the specified entity ids for promote process.
        /// </summary>
        /// <param name="entityIds">The entity ids.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        public OperationResultCollection Promote(Collection<Int64> entityIds, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<PromoteBL, OperationResultCollection>("Promote", businessLogic => businessLogic.EnqueueForPromote(entityIds, callerContext));
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

        #region Search Methods

        /// <summary>
        /// Search Entities for given search criteria and return list of entities with specified context.
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned entities.</param>
        /// <param name="searchOperationResult">Contains names of attributes with invalid input in search criteria.</param>
        /// <param name="callerContext">Provides the CallerContext</param>
        /// <returns>Search results - collection of entities</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of parameters like searchCriteria, searchContext, callerContext are null.
        /// </exception>
        public EntityCollection SearchEntities(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult searchOperationResult, CallerContext callerContext)
        {
            EntitySearchBL businessLogicBase = new EntitySearchBL(new EntityBL());

            SearchReadResult searchResult = MakeBusinessLogicCall<EntitySearchBL, SearchReadResult>(bl => bl.SearchEntities(searchCriteria, searchContext, callerContext), businessLogicBase,
                context =>
                {
                    context.CallDataContext.OrganizationIdList.Add(searchCriteria.OrganizationId);
                    if (searchCriteria.ContainerIds != null)
                    {
                        context.CallDataContext.ContainerIdList = searchCriteria.ContainerIds;
                    }
                    if (searchCriteria.CategoryIds != null)
                    {
                        context.CallDataContext.CategoryIdList = searchCriteria.CategoryIds;
                    }
                    if (searchCriteria.EntityTypeIds != null)
                    {
                        context.CallDataContext.EntityTypeIdList = searchCriteria.EntityTypeIds;
                    }
                    if (searchCriteria.RelationshipTypeIds != null)
                    {
                        context.CallDataContext.RelationshipTypeIdList = searchCriteria.RelationshipTypeIds;
                    }
                    if (searchCriteria.Locales != null)
                    {
                        context.CallDataContext.LocaleList = searchCriteria.Locales;
                    }
                });

            searchOperationResult = searchResult.OperationResult;

            return searchResult.Entities;
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
        /// <param name="entityHierarchyLevel"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Table CalculateEntityHierarchyDimensions(EntityVariantLevel entityHierarchyLevel, CallerContext callerContext)
        {
            return CalculateDimensionsLocal(entityHierarchyLevel, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean ProcessEntityHierarchyGenerationRules(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext)
        {
            TraceSettings curSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            ExecutionContext execCtxt = new ExecutionContext();
            Boolean result = false;

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
                    execCtxt.CallDataContext.EntityIdList.Add(entityId);
                    execCtxt.AdditionalContextData = entityHierarchyDefinition.ToXml();

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }


            try
            {
                EntityHierarchyBL entityHierarchyBL = new EntityHierarchyBL();
                result = entityHierarchyBL.ProcessHierarchyGenerationRules(entityId, entityHierarchyDefinition);
            }
            catch (ArgumentException ex)
            {
                currentActivity.LogError(ex.ToString());
                throw base.WrapException(ex);
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

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityVariantDefinition GetEntityHierarchyDimensionValues(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext)
        {
            TraceSettings curSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            ExecutionContext execCtxt = new ExecutionContext();

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (callerContext != null)
            {
                execCtxt.CallerContext = callerContext;

                curSettings = callerContext.TraceSettings;
            }

            if (curSettings != null && curSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    execCtxt.CallDataContext = new CallDataContext();
                    execCtxt.CallDataContext.EntityIdList.Add(entityId);
                    execCtxt.AdditionalContextData = entityHierarchyDefinition.ToXml();

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }

            EntityVariantDefinition retDef = null;

            try
            {
                EntityHierarchyBL entityHierarchyBL = new EntityHierarchyBL();
                retDef = entityHierarchyBL.GetHierarchyDimensionValues(entityId, entityHierarchyDefinition);
            }
            catch (ArgumentException ex)
            {
                currentActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                if (curSettings != null && curSettings.IsTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return retDef;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="ehdef"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Table GetEntityHierarchyMatrix(Int64 entityId, EntityVariantDefinition ehdef, OperationResult operationResult, CallerContext callerContext)
        {
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (callerContext != null && callerContext.TraceSettings.IsBasicTracingEnabled)
            {
                ExecutionContext ec = new ExecutionContext();
                ec.CallerContext = callerContext;

                currentActivity.Start(ec);
            }

            Table matrixTable = null;
            try
            {
                EntityHierarchyBL entityHierarchyBL = new EntityHierarchyBL();
                matrixTable = entityHierarchyBL.GetHierarchyMatrixExtended(entityId, ehdef, operationResult, true, callerContext);
            }
            catch (ArgumentException ex)
            {
                currentActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                if (callerContext != null && callerContext.TraceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return matrixTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="matrixTable"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult ProcessEntityHierarchyMatrix(Int64 entityId, Table matrixTable, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext)
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
                    execCtxt.CallDataContext.EntityIdList.Add(entityId);
                    execCtxt.AdditionalContextData = entityHierarchyDefinition.ToXml();

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }

            OperationResult operationResult = new OperationResult();
            try
            {
                EntityHierarchyBL entityHierarchyBL = new EntityHierarchyBL();
                operationResult = entityHierarchyBL.ProcessHierarchyMatrix(entityId, matrixTable, entityHierarchyDefinition);
            }
            catch (ArgumentException ex)
            {
                currentActivity.LogError(ex.ToString());
                throw base.WrapException(ex);
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

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult GenerateEntityHierarchy(Entity entity, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext)
        {
            ExecutionContext execCtxt = new ExecutionContext();
            DiagnosticActivity currentActivity = new DiagnosticActivity();
            TraceSettings currentSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (callerContext != null && callerContext.TraceSettings.IsBasicTracingEnabled)
            {
                execCtxt.CallerContext = callerContext;
                currentSettings = callerContext.TraceSettings;
            }

            if (currentSettings != null && currentSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    execCtxt.CallDataContext = new CallDataContext();
                    execCtxt.CallDataContext.EntityIdList.Add(entity.Id);
                    execCtxt.AdditionalContextData = entityHierarchyDefinition.ToXml();

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }

            OperationResult operationResult = null;

            try
            {
                EntityHierarchyBL entityManager = new EntityHierarchyBL();
                operationResult = entityManager.GenerateHierarchy(entity, entityHierarchyDefinition, callerContext);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                if (currentSettings != null && currentSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="matrixTable"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult GenerateEntityHierarchy(Entity entity, Table matrixTable, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext)
        {
            ExecutionContext execCtxt = new ExecutionContext();
            DiagnosticActivity currentActivity = new DiagnosticActivity();
            TraceSettings currentSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (callerContext != null && callerContext.TraceSettings.IsBasicTracingEnabled)
            {
                execCtxt.CallerContext = callerContext;
                currentSettings = callerContext.TraceSettings;
            }

            if (currentSettings != null && currentSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    execCtxt.CallDataContext = new CallDataContext();
                    execCtxt.CallDataContext.EntityIdList.Add(entity.Id);
                    execCtxt.AdditionalContextData = entityHierarchyDefinition.ToXml();

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }

            OperationResult operationResult = null;

            try
            {
                EntityHierarchyBL entityManager = new EntityHierarchyBL();
                operationResult = entityManager.GenerateHierarchy(entity, matrixTable, entityHierarchyDefinition, callerContext.Application, callerContext.Module);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                if (currentSettings != null && currentSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="attributeInfoList"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public EntityCollection GetChildEntitiesByEntityType(Int64 entityId, Int32 entityTypeId, Collection<KeyValuePair<Int32, LocaleEnum>> attributeInfoList, MDMCenterApplication application, MDMCenterModules module)
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
                    execCtxt.CallDataContext.EntityIdList.Add(entityId);
                    execCtxt.CallDataContext.EntityTypeIdList.Add(entityTypeId);


                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }


            EntityCollection returnVal = new EntityCollection();
            try
            {
                EntityHierarchyBL entityHierarchyBL = new EntityHierarchyBL();
                returnVal = entityHierarchyBL.GetChildEntitiesByEntityType(entityId, entityTypeId, attributeInfoList, application, module);
            }
            catch (ArgumentException ex)
            {
                currentActivity.LogError(ex.ToString());
                throw base.WrapException(ex);
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

            return returnVal;
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

        /// <summary>
        /// Gets child entities for the requested parent entity Id.
        /// </summary>
        /// <param name="parentEntityId">parent entity id for which child entities are required.</param>
        /// <param name="childEntityTypeId">Indicates child entity type id.</param>
        /// <param name="locale">Indicates locale</param>
        /// <param name="returnAttributeIds">Indicates return attribute id along with child entities.</param>
        /// <param name="getCompleteDetailsOfEntity">Flag saying whether to load complete details of entity.
        /// If flag is true then it returns CategoryId, CategoryName/LongName , Parent EntityId , Parent EntityName/LongName etc. else EntityId,EntityName and EntityLongName.</param>
        /// <param name="maxRecordsToReturn">Max no of child entities to return</param>
        /// <param name="callerContext">If this flag is true, it returns recursive children of requested entity type under a parent.</param>
        /// <param name="getRecursiveChildren">If this flag is true, it recursive children of requested entity type under a parent.</param>
        /// <returns>collection of child entities.</returns>
        public EntityCollection GetChildEntities(Int64 parentEntityId, Int32 childEntityTypeId, LocaleEnum locale, Collection<Int32> returnAttributeIds, Boolean getCompleteDetailsOfEntity, Int32 maxRecordsToReturn, CallerContext callerContext, Boolean getRecursiveChildren = false)
        {
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (callerContext != null && callerContext.TraceSettings.IsBasicTracingEnabled)
            {
                ExecutionContext ec = new ExecutionContext();
                ec.CallerContext = callerContext;

                currentActivity.Start(ec);
            }

            EntityCollection childEntites = null;

            try
            {
                EntityHierarchyBL entityHierarchyBL = new EntityHierarchyBL();
                childEntites = entityHierarchyBL.GetChildEntities(parentEntityId, childEntityTypeId, locale, returnAttributeIds, getCompleteDetailsOfEntity, maxRecordsToReturn, callerContext, getRecursiveChildren);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.Message);

                throw base.WrapException(ex);
            }
            finally
            {
                if (callerContext != null && callerContext.TraceSettings.IsBasicTracingEnabled)
                    currentActivity.Stop();
            }

            return childEntites;
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

        #region Copy Paste Methods

        /// <summary>
        /// Copies requested attributes and relationships from source entity to multiple target entities
        /// </summary>
        /// <param name="entityCopyPasteContext">EntityCopyPasteContext contains source and target entityId, attributes and Relationships that needs to be copied.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns the results of the operation having errors and information, if any</returns>
        public EntityOperationResultCollection CopyPasteEntityContents(EntityCopyPasteContext entityCopyPasteContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityCopyPasteContentBL, EntityOperationResultCollection>("CopyPasteEntityContents", businessLogic =>
                   businessLogic.CopyPasteEntityContents(entityCopyPasteContext, callerContext));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ehlevel"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Table CalculateDimensionsLocal(EntityVariantLevel ehlevel, CallerContext context)
        {
            TraceSettings curSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            ExecutionContext execCtxt = new ExecutionContext();

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
                    execCtxt.CallDataContext.EntityTypeIdList.Add(ehlevel.EntityTypeId);
                    execCtxt.AdditionalContextData = ehlevel.ToXml();

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    currentActivity.Start(execCtxt);
                }
                else
                    currentActivity.Start();
            }

            Table dimensionValues = null;

            try
            {
                EntityHierarchyBL entityHierarchyBL = new EntityHierarchyBL();
                dimensionValues = entityHierarchyBL.CalculateDimensions(ehlevel, context);
            }
            catch (ArgumentException ex)
            {
                currentActivity.LogError(String.Empty, ex.Message);
                throw base.WrapException(ex);
            }
            catch (Exception ex)
            {
                currentActivity.LogError(String.Empty, ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                if (context != null && context.TraceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return dimensionValues;
        }

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