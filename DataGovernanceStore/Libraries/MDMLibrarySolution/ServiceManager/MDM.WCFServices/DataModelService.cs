using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using MDM.AttributeDependencyManager.Business;
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.ContainerManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Business;
    using MDM.LookupManager.Business;
    using MDM.RelationshipManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.OrganizationManager.Business;
    using MDM.HierarchyManager.Business;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDM.Interfaces;
    using MDM.CategoryManager.Business;
    using MDM.UomManager.Business;
    using MDM.SearchManager.Business;
    using MDM.ApplicationServiceManager.Business;
    using MDM.EntityModelManager.Business;
    using MDM.AdminManager.Business;
    using MDM.EntityManager.Business;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;
    using MDM.MessageManager.Business;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class DataModelService : MDMWCFBase, IDataModelService
    {
        #region Constructors

        public DataModelService()
            : base(true)
        {

        }

        public DataModelService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region Attribute Model Methods

        /// <summary>
        /// Gets the attribute model for the requested AttributeModelContext
        /// </summary>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Attribute Model objects</returns>
        public AttributeModelCollection GetAttributeModelsByContext(AttributeModelContext attributeModelContext, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributeModelsByContext", MDMTraceSource.AttributeModelGet, false);

            AttributeModelCollection attributeModels = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAttributeModelsByContext' request message.", MDMTraceSource.AttributeModelGet);

                AttributeModelBL attributeModelBL = new AttributeModelBL();
                IEntityModelManager iEntityModelManager = new EntityModelBL();

                attributeModels = attributeModelBL.Get(attributeModelContext, iEntityModelManager, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAttributeModelsByContext' response message.", MDMTraceSource.AttributeModelGet);

            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributeModelsByContext", MDMTraceSource.AttributeModelGet);
            }

            return attributeModels;
        }

        /// <summary>
        /// Gets all Base attribute models
        /// </summary>
        /// <returns>Attribute Model objects</returns>
        public AttributeModelCollection GetAllBaseAttributeModels()
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAllBaseAttributeModels", MDMTraceSource.AttributeModelGet, false);

            AttributeModelCollection attributeModels = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAllBaseAttributeModels' request message.", MDMTraceSource.AttributeModelGet);

                AttributeModelBL attributeModelBL = new AttributeModelBL();
                attributeModels = attributeModelBL.GetAllBaseAttributeModels();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAllBaseAttributeModels' response message.", MDMTraceSource.AttributeModelGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAllBaseAttributeModels", MDMTraceSource.AttributeModelGet);
            }

            return attributeModels;
        }

        /// <summary>
        /// Get Attribute models by received Ids
        /// </summary>
        /// <param name="attributeIds">Indicates which Ids should be used to get Attribute Model Ids</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute Model Collection object</returns>
        public AttributeModelCollection GetBaseAttributeModelsByIds(Collection<Int32> attributeIds, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>("GetAttributeModelsByIds",
                                                  businessLogic =>
                                                      businessLogic.GetBaseAttributeModelsByIds(attributeIds));
        }

        /// <summary>
        /// Gets the attribute model for the requested attribute id
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which model is required</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <param name="callerContext"></param>
        /// <returns>Attribute Model Collection object</returns>
        public AttributeModelCollection GetAttributeModel(Int32 attributeId, AttributeModelContext attributeModelContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>(
                bl => bl.GetById(attributeId, attributeModelContext),
                context =>
                {
                    context.CallDataContext.AttributeIdList.Add(attributeId);
                    if (!context.LegacyMDMTraceSources.Contains(MDMTraceSource.AttributeModelGet))
                    {
                        context.LegacyMDMTraceSources.Add(MDMTraceSource.AttributeModelGet);
                    }
                });
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute ids
        /// </summary>
        /// <param name="attributeIds">Array of attribute ids for which models are required</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Model objects</returns>
        public AttributeModelCollection GetAttributeModelsByIds(Collection<Int32> attributeIds, AttributeModelContext attributeModelContext)
        {
            return MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>(
                bl => bl.GetByIds(attributeIds, attributeModelContext),
                context =>
                {
                    if (attributeIds != null)
                    {
                        context.CallDataContext.AttributeIdList = attributeIds;
                    }
                    if (attributeModelContext.CategoryId > 0)
                    {
                        context.CallDataContext.CategoryIdList.Add(attributeModelContext.CategoryId);
                    }
                    if (attributeModelContext.ContainerId > 0)
                    {
                        context.CallDataContext.ContainerIdList.Add(attributeModelContext.ContainerId);
                    }
                    if (attributeModelContext.EntityId > 0)
                    {
                        context.CallDataContext.EntityIdList.Add(attributeModelContext.EntityId);
                    }
                    if (attributeModelContext.EntityTypeId > 0)
                    {
                        context.CallDataContext.EntityTypeIdList.Add(attributeModelContext.EntityTypeId);
                    }
                    if (attributeModelContext.RelationshipTypeId > 0)
                    {
                        context.CallDataContext.RelationshipTypeIdList.Add(attributeModelContext.RelationshipTypeId);
                    }
                    if (attributeModelContext.Locales != null)
                    {
                        context.CallDataContext.LocaleList = attributeModelContext.Locales;
                    }
                });
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute group ids
        /// </summary>
        /// <param name="attributeGroupIds">Ids of the attribute group for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        public AttributeModelCollection GetAttributeModelsByGroupIds(Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributeModelsByGroupIds", MDMTraceSource.AttributeModelGet, false);

            AttributeModelCollection attributeModelCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAttributeModelsByGroupIds' request message.", MDMTraceSource.AttributeModelGet);

                AttributeModelBL attributeModelBL = new AttributeModelBL();
                attributeModelCollection = attributeModelBL.GetByGroupIds(attributeGroupIds, excludeAttributeIds, attributeModelContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAttributeModelsByGroupIds' response message.", MDMTraceSource.AttributeModelGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributeModelsByGroupIds", MDMTraceSource.AttributeModelGet);
            }

            return attributeModelCollection;
        }

        /// <summary>
        /// Gets the attribute models for the requested custom view id
        /// </summary>
        /// <param name="customViewId">Id of the custom view for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        public AttributeModelCollection GetAttributeModelsByCustomViewId(Int32 customViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributeModelsByCustomViewId", MDMTraceSource.AttributeModelGet, false);

            AttributeModelCollection attributeModelCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAttributeModelsByCustomViewId' request message.", MDMTraceSource.AttributeModelGet);

                AttributeModelBL attributeModelBL = new AttributeModelBL();
                attributeModelCollection = attributeModelBL.GetByCustomViewId(customViewId, excludeAttributeIds, attributeModelContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAttributeModelsByCustomViewId' response message.", MDMTraceSource.AttributeModelGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributeModelsByCustomViewId", MDMTraceSource.AttributeModelGet);
            }

            return attributeModelCollection;
        }

        /// <summary>
        /// Gets the attribute models for the requested state view id
        /// </summary>
        /// <param name="stateViewId">Id of the state view for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        public AttributeModelCollection GetAttributeModelsByStateViewId(Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributeModelsByStateViewId", MDMTraceSource.AttributeModelGet, false);

            AttributeModelCollection attributeModelCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAttributeModelsByStateViewId' request message.", MDMTraceSource.AttributeModelGet);

                AttributeModelBL attributeModelBL = new AttributeModelBL();
                attributeModelCollection = attributeModelBL.GetByStateViewId(stateViewId, excludeAttributeIds, attributeModelContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAttributeModelsByStateViewId' response message.", MDMTraceSource.AttributeModelGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributeModelsByStateViewId", MDMTraceSource.AttributeModelGet);
            }

            return attributeModelCollection;
        }

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
        public AttributeModelCollection GetAttributeModels(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributeModels", MDMTraceSource.AttributeModelGet, false);

            AttributeModelCollection attributeModelCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService has received 'GetAttributeModels' request message.", MDMTraceSource.AttributeModelGet);

                AttributeModelBL attributeModelBL = new AttributeModelBL();
                attributeModelCollection = attributeModelBL.Get(attributeIds, attributeGroupIds, customViewId, stateViewId, excludeAttributeIds, attributeModelContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService has sent 'GetAttributeModels' response message.", MDMTraceSource.AttributeModelGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributeModels", MDMTraceSource.AttributeModelGet);
            }

            return attributeModelCollection;
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Unique identifier for attributre</param>
        /// <param name="locale">denotes locale of attribute model</param>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <returns>Returns Attribute model</returns>
        public AttributeModel GetAttributeModelByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<AttributeModelBL, AttributeModel>("GetAttributeModelByUniqueIdentifier",
                                         businessLogic =>
                                             businessLogic.GetByUniqueIdentifier(attributeUniqueIdentifier, locale, callerContext));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Unique identifier for attributre</param>
        /// <param name="locales">denotes locale of attribute model</param>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <returns>Returns Attribute model</returns>
        public AttributeModelCollection GetAttributeModelsByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, Collection<LocaleEnum> locales, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>("GetAttributeModelsByUniqueIdentifier",
                                         businessLogic =>
                                             businessLogic.GetByUniqueIdentifier(attributeUniqueIdentifier, locales, callerContext));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Unique identifier for attributre</param>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <returns>Returns Attribute model</returns>
        public AttributeModel GetAttributeModelByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<AttributeModelBL, AttributeModel>("GetAttributeModelByUniqueIdentifier",
                                         businessLogic =>
                                             businessLogic.GetByUniqueIdentifier(attributeUniqueIdentifier, callerContext));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attributre collection</param>
        /// <param name="locale"></param>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <returns>Returns Attribute model</returns>
        public AttributeModelCollection GetAttributeModelsByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, LocaleEnum locale, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>("GetAttributeModelsByUniqueIdentifiers",
                                         businessLogic =>
                                             businessLogic.GetByUniqueIdentifiers(attributeUniqueIdentifiers, locale, callerContext));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attributre collection</param>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <returns>Returns Attribute model</returns>
        public AttributeModelCollection GetAttributeModelsByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>("GetAttributeModelsByUniqueIdentifiers",
                                         businessLogic =>
                                             businessLogic.GetByUniqueIdentifiers(attributeUniqueIdentifiers, callerContext));
        }

        /// <summary>
        /// Creates or Updates or Deletes attribute models based on model action flag
        /// </summary>
        /// <param name="attributeModelCollection">Models which needs to be processed</param>
        /// <returns>Result of the processing</returns>
        public AttributeOperationResultCollection ProcessAttributeModels(AttributeModelCollection attributeModelCollection, String programName, CallerContext callerContext)
        {
            //Start Trace Activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.ProcessAttributeModels", MDMTraceSource.AttributeModelProcess, false);

            AttributeOperationResultCollection attributeOperationResultCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'ProcessAttributeModels' request message.", MDMTraceSource.AttributeModelProcess);

                AttributeModelBL attributeModelBL = new AttributeModelBL();
                attributeOperationResultCollection = attributeModelBL.ProcessAttributeModels(attributeModelCollection, attributeOperationResultCollection, programName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'ProcessAttributeModels' response message.", MDMTraceSource.AttributeModelProcess);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop Trace Activity
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.ProcessAttributeModels", MDMTraceSource.AttributeModelProcess);
            }

            return attributeOperationResultCollection;
        }

        /// <summary>
        /// Get All Stateview attributes
        /// </summary>
        /// <param name="locales">Specifies List of Locales</param>
        /// <param name="callerContext">Specifies the Context of the Caller</param>
        /// <returns>Stateview Attribute Models</returns>
        public AttributeModelCollection GetAllStateviewAttributeModels(Collection<LocaleEnum> locales, CallerContext callerContext)
        {
            const Int32 stateviewAttributeGroupId = 95;
            Collection<AttributeModel> stateviewAttributeModels =
                MakeBusinessLogicCall<AttributeModelOperationsBL, Collection<AttributeModel>>("GetAttributeModelsByUniqueIdentifier",
                    businessLogic => businessLogic.GetByAttributeGroup(stateviewAttributeGroupId, locales));

            return new AttributeModelCollection(stateviewAttributeModels);
        }

        #endregion

        #region user management methods

        public Collection<SecurityUser> GetRoleUsers(Int32 roleId)
        {
            Collection<SecurityUser> users = new Collection<SecurityUser>();

            return users;
        }

        public Collection<SecurityUser> GetVendorUsers(Int32 vendorId)
        {
            Collection<SecurityUser> users = new Collection<SecurityUser>();

            return users;
        }



        #endregion

        #region Entity Variant Definition

        /// <summary>
        /// Process entity variant definition based on the application context provided.
        /// </summary>
        /// <param name="entityVariantDefinitions">Indicates the entity hierarchy definition collection to be processed.</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns operation result.</returns>
        public OperationResultCollection ProcessVariantDefinitions(EntityVariantDefinitionCollection entityVariantDefinitions, CallerContext callerContext)
        {
            OperationResultCollection operationResults = new OperationResultCollection();

            try
            {
                EntityVariantDefinitionBL entityVariantDefinitionBL = new EntityVariantDefinitionBL(new AttributeModelBL());
                operationResults = entityVariantDefinitionBL.Process(entityVariantDefinitions, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return operationResults;

        }

        /// <summary>
        /// Get all entity variant definitions based on context
        /// </summary>
        /// <param name="containerId">Indicates the identifier of container, as a part of context<</param>
        /// <param name="categoryId">Indicates the identifier of category, as a part of context</param>
        /// <param name="entityTypeId">Indicates the identifier of base entity type for which definition will be fetched</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns all entity variant definitions based on context</returns>
        public EntityVariantDefinition GetVariantDefinitionByContext(Int32 containerId, Int64 categoryId, Int32 entityTypeId, CallerContext callerContext)
        {

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            ExecutionContext executionContext = new ExecutionContext();

            if (callerContext != null)
            {
                executionContext.CallerContext = callerContext;

                traceSettings = callerContext.TraceSettings;
            }

            if (traceSettings != null && traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    executionContext.CallDataContext = new CallDataContext();
                    executionContext.CallDataContext.ContainerIdList.Add(containerId);
                    executionContext.CallDataContext.CategoryIdList.Add(categoryId);
                    executionContext.CallDataContext.EntityTypeIdList.Add(entityTypeId);

                    executionContext.SecurityContext = new SecurityContext();
                    executionContext.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    diagnosticActivity.Start(executionContext);
                }
                else
                    diagnosticActivity.Start();
            }


            EntityVariantDefinition entityVariantDefinition = null;
            try
            {

                EntityVariantDefinitionBL entityVariantDefinitionBL = new EntityVariantDefinitionBL(new AttributeModelBL());
                entityVariantDefinition = entityVariantDefinitionBL.GetByContext(containerId, categoryId, entityTypeId, callerContext);

            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop trace activity
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityVariantDefinition;
        }

        /// <summary>
        /// Gets entity variant definitions based entity variant definition id
        /// </summary>
        /// <param name="entityVariantDefinitionId">Indicates the identifier of entity variant definition</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns entity variant definitions based on entity variant definition id</returns>
        public EntityVariantDefinition GetVariantDefinitionById(Int32 entityVariantDefinitionId, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings != null && traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    traceSettings = callerContext.TraceSettings;
                }

                diagnosticActivity.Start();
            }

            EntityVariantDefinition entityVariantDefinition = null;
            try
            {
                EntityVariantDefinitionBL entityVariantDefinitionBL = new EntityVariantDefinitionBL(new AttributeModelBL());

                entityVariantDefinition = entityVariantDefinitionBL.GetById(entityVariantDefinitionId, callerContext);
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop trace activity
                if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityVariantDefinition;
        }

        /// <summary>
        /// Get all the entity variant definition collection
        /// </summary>
        /// <returns>Returns all the entity variant definition collection</returns>
        public EntityVariantDefinitionCollection GetAllVariantDefinitions(CallerContext callerContext)
        {
            EntityVariantDefinitionCollection result = new EntityVariantDefinitionCollection();

            try
            {
                EntityVariantDefinitionBL entityVariantDefinitionBL = new EntityVariantDefinitionBL(new AttributeModelBL());
                result = entityVariantDefinitionBL.GetAll(callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return result;
        }

        #endregion

        #region Dynamic Table Schema Methods

        /// <summary>
        /// Process table
        /// </summary>
        /// <param name="dbTable">This parameter is specifying instance of table to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult DynamicTableSchemaProcess(DBTable dbTable, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            try
            {
                DynamicTableSchemaBL _dynamicTableSchemaBL = DynamicTableSchemaBL.GetSingleton();
                operationResult = _dynamicTableSchemaBL.Process(dbTable, dynamicTableType, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return operationResult;
        }

        /// <summary>
        /// Process Multiple tables
        /// </summary>
        /// <param name="dbTables">This parameter is specifying instance of tables to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult DynamicTableSchemaProcesses(DBTableCollection dbTables, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            try
            {
                DynamicTableSchemaBL _dynamicTableSchemaBL = DynamicTableSchemaBL.GetSingleton();
                operationResult = _dynamicTableSchemaBL.Process(dbTables, dynamicTableType, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return operationResult;
        }

        #endregion

        #region Lookup Methods

        /// <summary>
        /// Loads all lookup data
        /// </summary>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Boolean flag which says whether the load is success or not</returns>
        public Boolean LoadLookupData(CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.LoadLookupData", MDMTraceSource.LookupGet, false);

            Boolean isLoadSuccessful = false;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'LoadLookupData' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();
                isLoadSuccessful = lookupManager.Load(callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'LoadLookupData' response message.", MDMTraceSource.LookupGet);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.LoadLookupData", MDMTraceSource.LookupGet);
            }

            return isLoadSuccessful;
        }

        /// <summary>
        /// Gets model of the specified lookup table 
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns lookup model</returns>
        public Lookup GetLookupModel(String lookupTableName, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetLookupModel", MDMTraceSource.LookupGet, false);

            Lookup lookup = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetLookupModel' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();
                lookup = lookupManager.GetModel(lookupTableName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetLookupModel' response message.", MDMTraceSource.LookupGet);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetLookupModel", MDMTraceSource.LookupGet);
            }

            return lookup;
        }

        /// <summary>
        /// Gets lookup data for the requested lookup table and locale
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns lookup data</returns>
        public Lookup GetLookupData(String lookupTableName, LocaleEnum locale, Int32 maxRecordsToReturn, Boolean getLatest, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetLookupData", MDMTraceSource.LookupGet, false);

            DateTime startTime = DateTime.Now;
            Lookup lookup = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetLookupData' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();
                lookup = lookupManager.Get(lookupTableName, locale, maxRecordsToReturn, getLatest, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetLookupData' response message.", MDMTraceSource.LookupGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                DateTime endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Total execution time taken by lookup get : Lookup table name {0}. Time taken is {1} milliseconds", lookupTableName, ts.TotalMilliseconds), MDMTraceSource.LookupGet);
                    if (ts.Milliseconds > 300)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Look up get for table name : {0} has taken {1} milliseconds for execution. This would impact core system performance.", lookupTableName, ts.TotalMilliseconds), MDMTraceSource.LookupGet);
                    }
                    //Stop trace activity
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetLookupData", MDMTraceSource.LookupGet);
                }

            }

            return lookup;
        }

        /// <summary>
        /// Gets the collection of lookup row for the specified lookup table and lookup search rule collection in a locale provided by user
        /// </summary>
        /// <param name="lookupTableName">Indicates the name of the lookup table</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="searchRuleCollection">Indicates the search rules for filtering the lookup row collection</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <returns>Returns the filtered lookup row collection</returns>
        public RowCollection GetLookupRows(String lookupTableName, LocaleEnum locale, LookupSearchRuleCollection searchRuleCollection, Int32 maxRecordsToReturn,
            CallerContext callerContext)
        {
            return MakeBusinessLogicCall<LookupBL, RowCollection>("GetLookupRows",
                businessLogic => businessLogic.GetLookupRows(lookupTableName, locale, searchRuleCollection, maxRecordsToReturn, callerContext));
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which data needs to be get</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <returns>Returns lookup data</returns>
        public Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, CallerContext callerContext)
        {
            return GetAttributeLookupData(attributeId, locale, maxRecordsToReturn, new ApplicationContext(), callerContext);
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id and application context in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="applicationContext">Indicates the current context of the application</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <returns>Returns lookup data</returns>
        public Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext)
        {
            return GetAttributeLookupData(attributeId, locale, maxRecordsToReturn, null, applicationContext, callerContext);
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id and lookup values id in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="lookupValueIdList">Indicates the list of lookup PK Ids to be returned along with requested max number of records to return</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <returns>Returns lookup data</returns>
        public Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Collection<Int32> lookupValueIdList, CallerContext callerContext)
        {
            return GetAttributeLookupData(attributeId, locale, 0, lookupValueIdList, new ApplicationContext(), callerContext);
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id, lookup values id and application context in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">Indicates the list of lookup PK Ids to be returned along with requested max number of records to return</param>
        /// <param name="applicationContext">Indicates the current context of the application</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <returns>Returns lookup data</returns>
        public Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<LookupBL, Lookup>(
                bl => bl.Get(attributeId, locale, maxRecordsToReturn, lookupValueIdList, applicationContext, callerContext, true),
                context =>
                {
                    context.CallDataContext.AttributeIdList.Add(attributeId);
                    context.CallDataContext.LocaleList.Add(locale);
                    if (!context.LegacyMDMTraceSources.Contains(MDMTraceSource.LookupGet))
                    {
                        context.LegacyMDMTraceSources.Add(MDMTraceSource.LookupGet);
                    }
                },
                diagnosticActivity =>
                {
                    TimeSpan ts = DateTime.Now - diagnosticActivity.StartDateTime;
                    if (ts.Milliseconds > 300)
                    {
                        diagnosticActivity.LogWarning(String.Format("Look up get for attribute id : {0} has taken {1} milliseconds for execution. This would impact core system performance.", attributeId, ts.TotalMilliseconds));
                    }
                });
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id, lookup values id, application context and dependent attributes in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">Indicates the list of lookup PK Ids to be returned along with requested max number of records to return</param>
        /// <param name="applicationContext">Indicates the current context of the application</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <param name="isDependent">Indicates whether the attribute is dependent or not</param>
        /// <param name="dependentAttributeCollection">Indicates the dependent attribute collection details</param>
        /// <returns>Returns lookup data</returns>
        public Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean isDependent, DependentAttributeCollection dependentAttributeCollection)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributeLookupData", MDMTraceSource.LookupGet, false);

            Lookup lookup = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAttributeLookupData' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();
                lookup = lookupManager.Get(attributeId, locale, maxRecordsToReturn, lookupValueIdList, applicationContext, callerContext, true, isDependent, dependentAttributeCollection);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAttributeLookupData' response message.", MDMTraceSource.LookupGet);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributeLookupData", MDMTraceSource.LookupGet);
            }

            return lookup;
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute name, attribute parent name, lookup values id, application context and dependent attributes in a locale provided by user
        /// </summary>
        /// <param name="attributeName">Indicates the attribute short name required the identify an attribute</param>
        /// <param name="attributeParentName">Indicates the attribute parent name required the identify an attribute</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">Indicates the list of lookup PK Ids to be returned along with requested max number of records to return</param>
        /// <param name="applicationContext">Indicates the current context of the application</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <param name="isDependent">Indicates whether the attribute is dependent or not</param>
        /// <param name="dependentAttributeCollection">Indicates the dependent attribute collection details</param>
        /// <returns>Returns lookup data</returns>
        public Lookup GetAttributeLookupData(String attributeName, String attributeParentName, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean isDependent, DependentAttributeCollection dependentAttributeCollection)
        {
            return MakeBusinessLogicCall<LookupBL, Lookup>("GetAttributeLookupData", businessLogic => businessLogic.GetAttributeLookupData(attributeName, attributeParentName,
                locale, maxRecordsToReturn, lookupValueIdList, applicationContext, callerContext, true, isDependent, dependentAttributeCollection));
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="searchValue">Value to be searched for the attribute lookup</param>
        /// <param name="applicationContext">Indicates the current context of the application</param>
        /// <param name="callerContext">Indicates the application and module name by which action is being performed</param>
        /// <returns>Returns lookup data</returns>
        public Lookup SearchAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, ApplicationContext applicationContext, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.SearchAttributeLookupData", MDMTraceSource.LookupGet, false);

            Lookup lookup = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'SearchAttributeLookupData' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();
                lookup = lookupManager.Search(attributeId, locale, maxRecordsToReturn, searchValue, applicationContext, callerContext, true);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'SearchAttributeLookupData' response message.", MDMTraceSource.LookupGet);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.SearchAttributeLookupData", MDMTraceSource.LookupGet);
            }

            return lookup;
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Locale for which data needs to be get</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="searchValue">Value to be searched for the attribute lookup.</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        public Lookup SearchAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, ApplicationContext applicationContext, Boolean isDependent, DependentAttributeCollection dependentAttributes, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.SearchAttributeLookupData", MDMTraceSource.LookupGet, false);

            Lookup lookup = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'SearchAttributeLookupData' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();
                lookup = lookupManager.Search(attributeId, locale, maxRecordsToReturn, searchValue, applicationContext, callerContext, true, isDependent, dependentAttributes);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'SearchAttributeLookupData' response message.", MDMTraceSource.LookupGet);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.SearchAttributeLookupData", MDMTraceSource.LookupGet);
            }

            return lookup;
        }

        /// <summary>
        /// Gets lookup data for requested attributes and locale
        /// </summary>
        /// <param name="attributeIds">List of attribute Ids for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        public LookupCollection GetAttributesLookupData(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributesLookupData", MDMTraceSource.LookupGet, false);

            LookupCollection lookupCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAttributesLookupData' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();

                if (applicationContext != null)
                {
                    if (attributeIds.Count > 1)
                    {
                        applicationContext.AttributeId = 0;
                        applicationContext.AttributeName = String.Empty;
                    }
                }

                lookupCollection = lookupManager.Get(attributeIds, locale, maxRecordsToReturn, applicationContext, callerContext, true);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAttributesLookupData' response message.", MDMTraceSource.LookupGet);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributesLookupData", MDMTraceSource.LookupGet);
            }

            return lookupCollection;
        }

        /// <summary>
        /// Gets lookup data for requested attributes and locale
        /// </summary>
        /// <param name="attributeIds">List of attribute Ids for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        public LookupCollection GetAttributesLookupData(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAttributesLookupData", MDMTraceSource.LookupGet, false);

            LookupCollection lookupCollection = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAttributesLookupData' request message.", MDMTraceSource.LookupGet);

                LookupBL lookupManager = new LookupBL();
                lookupCollection = lookupManager.Get(attributeIds, locale, maxRecordsToReturn, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAttributesLookupData' response message.", MDMTraceSource.LookupGet);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAttributesLookupData", MDMTraceSource.LookupGet);
            }

            return lookupCollection;
        }

        /// <summary>
        /// Gets displayValue for specified lookup attributes, locale and contexts
        /// </summary>
        /// <param name="attributeValueRefIdPair"><code>Dictionary&lt;Int32 attributeId, Collection&lt;Int64 RefIds&gt;&gt;</code></param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of each lookup items to return. Setting '-1' returns all record</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Display Values</returns>
        public Dictionary<Int32, Dictionary<Int32, String>> GetLookupAttributeDisplayValue(Dictionary<Int32, Collection<Int32>> attributeValueRefIdPair, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetLookupAttributeDisplayValue", MDMTraceSource.LookupGet, false);

            Dictionary<Int32, Dictionary<Int32, String>> result = null;
            try
            {
                result = new Dictionary<Int32, Dictionary<Int32, String>>(attributeValueRefIdPair.Count);
                if (attributeValueRefIdPair.Count == 0)
                {
                    return result;
                }

                //TODO: Good place for paerallelization
                foreach (KeyValuePair<Int32, Collection<Int32>> attributeQuery in attributeValueRefIdPair)
                {
                    Int32 attributeId = attributeQuery.Key;
                    Collection<Int32> refIds = attributeQuery.Value;
                    Dictionary<Int32, String> attributeResult = new Dictionary<Int32, String>();

                    if (refIds == null || refIds.Count == 0)
                    {
                        result.Add(attributeId, attributeResult);
                        continue;
                    }

                    ApplicationContext preparedAppContext = null;
                    if (applicationContext != null)
                    {
                        preparedAppContext = (ApplicationContext)applicationContext.Clone();
                        applicationContext.AttributeId = attributeId;
                        applicationContext.AttributeName = String.Empty;
                    }

                    Collection<Int32> refIdsTemp = new Collection<Int32>();
                    foreach (Int32 id in refIds)
                    {
                        refIdsTemp.Add(id);
                    }

                    Lookup lookup = GetAttributeLookupData(attributeId, locale, maxRecordsToReturn, refIdsTemp, preparedAppContext, callerContext);

                    if (lookup == null)
                    {
                        ApplicationMessage message = null;
                        ApplicationMessageBL applicationMessageManager = new ApplicationMessageBL();
                        message = applicationMessageManager.GetMessage("60004", "en-WW", attributeId);
                        throw (new Exception(message.Message));
                    }

                    foreach (Int32 requestedRefId in refIds)
                    {
                        if (lookup.GetRecordById(requestedRefId) == null)
                        {
                            Object[] parameters = { requestedRefId, attributeId };
                            LocaleMessage message = new LocaleMessageBL().Get(locale, "114002", parameters, false, callerContext);
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "DataModelService: GetLookupAttributeDisplayValue: " + message.Message);
                            //throw (new Exception(message.Message));
                        }
                        attributeResult[requestedRefId] = lookup.GetDisplayFormatById(requestedRefId);
                    }

                    result.Add(attributeId, attributeResult);
                }
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetLookupAttributeDisplayValue", MDMTraceSource.LookupGet);
            }

            return result;
        }

        /// <summary>
        /// Processes lookup data
        /// </summary>
        /// <param name="lookup">Lookup data which needs to be processed</param>
        /// <param name="programName">Program name which is requesting for process</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="invalidateCache">Flag which indicates whether to invalidate the lookup cache or not.</param>
        /// <returns>Result of the process</returns>
        public OperationResult ProcessLookupData(Lookup lookup, String programName, CallerContext callerContext, Boolean invalidateCache)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.ProcessLookupData", MDMTraceSource.LookupProcess, false);

            OperationResult operationResult = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'ProcessLookupData' request message.", MDMTraceSource.LookupProcess);

                LookupBL lookupManager = new LookupBL();
                operationResult = lookupManager.Process(lookup, programName, callerContext, invalidateCache);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'ProcessLookupData' response message.", MDMTraceSource.LookupProcess);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.ProcessLookupData", MDMTraceSource.LookupProcess);
            }

            return operationResult;
        }

        /// <summary>
        /// Processes lookup data
        /// </summary>
        /// <param name="lookups">Lookup data which needs to be processed</param>
        /// <param name="programName">Program name which is requesting for process</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Result of the process</returns>
        public OperationResult ProcessLookups(LookupCollection lookups, String programName, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.ProcessLookupData", MDMTraceSource.LookupProcess, false);

            OperationResult operationResult = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'ProcessLookupData' request message.", MDMTraceSource.LookupProcess);

                LookupBL lookupManager = new LookupBL();
                operationResult = lookupManager.Process(lookups, programName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'ProcessLookupData' response message.", MDMTraceSource.LookupProcess);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.ProcessLookupData", MDMTraceSource.LookupProcess);
            }

            return operationResult;
        }

        /// <summary>
        /// Gets All Related Lookup Table Names for Current Lookup Table
        /// </summary>
        /// <param name="lookup">Current Lookup Table Name</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Collection of Referrer Lookups </returns>
        public LookupCollection GetRelatedLookups(Lookup lookup, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<LookupBL, LookupCollection>("GetRelatedLookups",
                                        businessLogic =>
                                            businessLogic.GetRelatedLookups(lookup, callerContext));
        }

        /// <summary>
        /// Gets the lookup schema based on requested lookup names.
        /// </summary>
        /// <param name="lookupNames">Indicates list of lookup names</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns the list of lookup schema</returns>
        public LookupCollection GetLookupSchema(Collection<String> lookupNames, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<LookupBL, LookupCollection>("GetLookupSchema",
                                        businessLogic =>
                                            businessLogic.GetLookupSchema(lookupNames, callerContext));
        }

        /// <summary>
        /// Gets the lookup schema based requested lookup name.
        /// </summary>
        /// <param name="lookupName">Indicates the lookup name</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns the lookup schema</returns>
        public Lookup GetLookupSchema(String lookupName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<LookupBL, Lookup>("GetLookupSchema",
                                       businessLogic =>
                                           businessLogic.GetLookupSchema(lookupName, callerContext));
        }

        #endregion

        #region Container Methods

        /// <summary>
        /// API to Create a Container
        /// </summary>
        /// <param name="container">container object</param>
        /// <param name="programName">program name</param>
        /// <param name="callerContext">caller context to to indicate aplication and module</param>
        /// <returns>OperationResult</returns>
        public OperationResult CreateContainer(Container container, String programName, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.CreateContainer", false);

            OperationResult containerOperationResult = new OperationResult();

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'CreateContainer' request message.");

                ContainerBL containerManager = new ContainerBL();
                containerOperationResult = containerManager.Create(container, programName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'CreateContainer' response message.");
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.CreateContainer");
            }

            return containerOperationResult;

        }

        /// <summary>
        /// API to Update a Container
        /// </summary>
        /// <param name="container">container object</param>
        /// <param name="programName">program name</param>
        /// <param name="callerContext">caller context to to indicate aplication and module</param>
        /// <returns>OperationResult</returns>
        public OperationResult UpdateContainer(Container container, String programName, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.UpdateContainer", false);

            OperationResult containerOperationResult = new OperationResult();

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'UpdateContainer' request message.");

                ContainerBL containerManager = new ContainerBL();
                containerOperationResult = containerManager.Update(container, programName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'UpdateContainer' response message.");
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.UpdateContainer");
            }

            return containerOperationResult;

        }

        /// <summary>
        /// API to Delete a Container
        /// </summary>
        /// <param name="container">container object</param>
        /// <param name="programName">program name</param>
        /// <param name="callerContext">caller context to to indicate aplication and module</param>
        /// <returns>OperationResult</returns>
        public OperationResult DeleteContainer(Container container, String programName, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.DeleteContainer", false);

            OperationResult containerOperationResult = new OperationResult();

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'DeleteContainer' request message.");

                ContainerBL containerManager = new ContainerBL();
                containerOperationResult = containerManager.Delete(container, programName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'DeleteContainer' response message.");
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.DeleteContainer");
            }

            return containerOperationResult;

        }

        public OperationResult ProcessContainer(ContainerCollection containerCollection, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.ProcessContainer", false);

            OperationResult containerOperationResult = new OperationResult();

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'ProcessContainer' request message.");

                ContainerBL containerManager = new ContainerBL();
                containerManager.Process(containerCollection, containerOperationResult, callerContext.ProgramName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'ProcessContainer' response message.");
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.ProcessContainer");
            }

            return containerOperationResult;

        }

        /// <summary>
        /// Gets All Containers
        /// </summary>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public ContainerCollection GetAllContainers(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerBL, ContainerCollection>("GetAllContainers", businessLogic => businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Returns Container by Id
        /// </summary>
        /// <param name="containerId">Indicates Id of the requested Container</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public Container GetContainerById(Int32 containerId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerBL, Container>(
                bl => bl.Get(containerId, callerContext),
                context => context.CallDataContext.ContainerIdList.Add(containerId));
        }

        /// <summary>
        /// Returns Container by Name
        /// </summary>
        /// <param name="containerShortName">Indicates ShortName of the requested Container</param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Container GetContainerByName(String containerShortName, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetContainerByName", MDMTraceSource.General, false);

            Container container = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetContainerByShortName' request message.", MDMTraceSource.APIFramework);

                ContainerBL containerManager = new ContainerBL();
                container = containerManager.Get(containerShortName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetContainerByShortName' response message.", MDMTraceSource.APIFramework);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetContainerByName", MDMTraceSource.General);
            }

            return container;
        }

        /// <summary>
        /// Returns Container by Name and Org Name.
        /// </summary>
        /// <param name="containerShortName">Indicates ShortName of the requested Container</param>
        /// <param name="organizationName">Indicates ShortName of the Organization</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public Container GetContainerByContainerNameAndOrgName(String containerShortName, String organizationName, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetByContainerNameAndOrgName", MDMTraceSource.General, false);

            Container container = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetByContainerNameAndOrgName' request message.", MDMTraceSource.APIFramework);

                ContainerBL containerManager = new ContainerBL();
                container = containerManager.Get(containerShortName, organizationName, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetByContainerNameAndOrgName' response message.", MDMTraceSource.APIFramework);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetByContainerNameAndOrgName", MDMTraceSource.General);
            }

            return container;
        }

        /// <summary>
        /// Gets All Containers
        /// </summary>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public ContainerCollection GetAllContainers(ContainerContext containerContext, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetAllContainers", MDMTraceSource.General, false);

            ContainerCollection containers = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetAllContainers' request message.", MDMTraceSource.APIFramework);

                ContainerBL containerManager = new ContainerBL();
                containers = containerManager.GetAll(containerContext, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetAllContainers' response message.", MDMTraceSource.APIFramework);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetAllContainers", MDMTraceSource.General);
            }

            return containers;
        }

        /// <summary>
        /// Returns Container by Id
        /// </summary>
        /// <param name="containerId">Indicates Id of the requested Container</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public Container GetContainerById(Int32 containerId, ContainerContext containerContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerBL, Container>(
                bl => bl.Get(containerId, containerContext, callerContext),
                context => context.CallDataContext.ContainerIdList.Add(containerId));
        }

        /// <summary>
        /// Returns Container by Name
        /// </summary>
        /// <param name="containerShortName">Indicates ShortName of the requested Container</param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Container GetContainerByName(String containerShortName, ContainerContext containerContext, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetContainerByName", MDMTraceSource.General, false);

            Container container = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetContainerByShortName' request message.", MDMTraceSource.APIFramework);

                ContainerBL containerManager = new ContainerBL();
                container = containerManager.Get(containerShortName, containerContext, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetContainerByShortName' response message.", MDMTraceSource.APIFramework);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetContainerByName", MDMTraceSource.General);
            }

            return container;
        }

        /// <summary>
        /// Returns Container by Name and Org Name.
        /// </summary>
        /// <param name="containerShortName">Indicates ShortName of the requested Container</param>
        /// <param name="organizationName">Indicates ShortName of the Organization</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public Container GetContainerByContainerNameAndOrgName(String containerShortName, String organizationName, ContainerContext containerContext, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetByContainerNameAndOrgName", MDMTraceSource.General, false);

            Container container = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetByContainerNameAndOrgName' request message.", MDMTraceSource.APIFramework);

                ContainerBL containerManager = new ContainerBL();
                container = containerManager.Get(containerShortName, organizationName, containerContext, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetByContainerNameAndOrgName' response message.", MDMTraceSource.APIFramework);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetByContainerNameAndOrgName", MDMTraceSource.General);
            }

            return container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns>ContainerCollection</returns>
        public ContainerCollection GetContainerCollectionByOrganization(Int32 orgId)
        {
            ContainerCollection containerCollection = null;
            try
            {
                ContainerBL containerBL = new ContainerBL();
                ContainerContext containerContext = new ContainerContext();
                CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.UIProcess);
                containerCollection = containerBL.GetByOrgId(orgId, containerContext, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return containerCollection;
        }

        /// <summary>
        /// Get hierarchy of child container based on given container identifier and container context
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="containerContext">Indicates context of the container specifying properties like load attributes</param>
        /// <param name="callerContext">Indicates caller of the API specifying application and module name.</param>
        /// <returns>Returns collection of child container based on given container identifier and container context</returns>
        public ContainerCollection GetChildContainersByParentContainerId(Int32 containerId, ContainerContext containerContext, CallerContext callerContext, Boolean loadRecursive)
        {
            return MakeBusinessLogicCall<ContainerBL, ContainerCollection>("GetChildContainers", businessLogic => businessLogic.GetChildContainers(containerId, containerContext, callerContext, loadRecursive));
        }

        /// <summary>
        /// Get hierarchy of child container with requested container identifier's container itself based on given container identifier and container context
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="containerContext">Indicates context of the container specifying properties like load attributes</param>
        /// <param name="callerContext">Indicates caller of the API specifying application and module name.</param>
        /// <param name="loadRecursive">Indicates whether load only immediate child containers or complete hierarchy of child containers.</param>
        /// <returns>Returns collection of child containers with requested container identifier's container itself based on given container identifier and container context</returns>
        public ContainerCollection GetContainerHierarchyByContainerId(Int32 containerId, ContainerContext containerContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerBL, ContainerCollection>("GetContainerHierarchy", businessLogic => businessLogic.GetContainerHierarchy(containerId, containerContext, callerContext));
        }

        #endregion Container Methods

        #region Copy Container Mappings Methods

        /// <summary>
        /// Copies the Mappings from source container to the target Container
        /// </summary>
        /// <param name="sourceContainerId">source container Id</param>
        /// <param name="targetContainerId">target Container Id</param>
        /// <param name="containerTempleteCopyContext">Context specifies which all mappings needs to be copied from source to target</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public OperationResult CopyContainerMappings(Int32 sourceContainerId, Int32 targetContainerId, ContainerTemplateCopyContext containerTempleteCopyContext, CallerContext callerContext)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.CopyContainerMappings", false);

            OperationResult operationResult = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'CopyContainerMappings' request message.");

                ContainerBL containerBL = new ContainerBL();
                operationResult = containerBL.CopyMappings(sourceContainerId, targetContainerId, containerTempleteCopyContext, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'CopyContainerMappings' response message.");
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.CopyContainerMappings");
            }

            return operationResult;
        }

        #endregion Copy Container Mappings Methods

        #region Attribute Mappings Methods

        /// <summary>
        /// Process container - entity type mappings based on action specified in 'ContainerEntityTypeAttributeMapping' object
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">ContainerEntityTypeAttributeMappings which needs to be processed.</param>
        /// <returns>Boolean flag which says whether process is success or not</returns>
        [Obsolete("This method has been obsoleted. Please use ProcessContainerEntityTypeAttributeMappings method instead of this")]
        public Boolean ProcessContainerEntityTypeAttributeMapping(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.ProcessContainerEntityTypeAttributeMapping", MDMTraceSource.DataModel, false);

            Boolean result = false;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'ProcessContainerEntityTypeAttributeMapping' request message.", MDMTraceSource.DataModel);

                ContainerEntityTypeAttributeMappingBL containerEntityTypeAttributeMappingBL = new ContainerEntityTypeAttributeMappingBL(new OrganizationBL(), new ContainerBL(), new EntityTypeBL(), new AttributeModelBL());

                CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.UIProcess);
                result = containerEntityTypeAttributeMappingBL.Process(containerEntityTypeAttributeMappings, callerContext).OperationResultStatus == OperationResultStatusEnum.Successful ? true : false;

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'ProcessContainerEntityTypeAttributeMapping' response message.", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.ProcessContainerEntityTypeAttributeMapping", MDMTraceSource.DataModel);
            }

            return result;
        }

        /// <summary>
        /// Process container - relationshipType - attribute mappings based on action specified in 'ContainerRelationshipTypeAttributeMapping' object
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings">ContainerRelationshipTypeAttributeMappingCollection which needs to be processed.</param>
        /// <returns>Boolean flag which says whether process is success or not</returns>
        [Obsolete("This method has been obsoleted. Please use ProcessContainerRelationshipTypeAttributeMappings method instead of this")]
        public Boolean ProcessContainerRelationshipTypeAttributeMapping(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.ProcessContainerRelationshipTypeAttributeMapping", MDMTraceSource.DataModel, false);

            Boolean result = false;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'ProcessContainerRelationshipTypeAttributeMapping' request message.", MDMTraceSource.DataModel);

                ContainerRelationshipTypeAttributeMappingBL containerRelationshipTypeAttributeMappingBL = new ContainerRelationshipTypeAttributeMappingBL(new OrganizationBL(), new ContainerBL(), new RelationshipTypeBL(), new AttributeModelBL());

                CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.UIProcess);
                result = containerRelationshipTypeAttributeMappingBL.Process(containerRelationshipTypeAttributeMappings, callerContext).OperationResultStatus == OperationResultStatusEnum.Successful ? true : false;

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'ProcessContainerRelationshipTypeAttributeMapping' response message.", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.ProcessContainerRelationshipTypeAttributeMapping", MDMTraceSource.DataModel);
            }

            return result;
        }

        #endregion

        #region EntityType Get

        /// <summary>
        /// Get all entity types in the system
        /// </summary>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <returns>All entity types</returns>
        public EntityTypeCollection GetAllEntityTypes(CallerContext callerContext, Boolean getLatest)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetEntityTypes", MDMTraceSource.DataModel, false);

            EntityTypeCollection entityTypes = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetEntityTypes' request message.", MDMTraceSource.DataModel);

                EntityTypeBL entityTypeBL = new EntityTypeBL();
                entityTypes = entityTypeBL.GetAll(callerContext, getLatest);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetEntityTypes' response message.", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetEntityTypes", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        /// <summary>
        /// Get entity type based on Id
        /// </summary>
        /// <param name="entityTypeId">Entity type id for which data is to be fetched</param>
        /// <param name="iCallerContext">Indicates name of application and module.</param>
        /// <returns>Entity type for given Id</returns>
        public EntityType GetEntityTypeById(Int32 entityTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeBL, EntityType>("GetEntityTypeById", businessLogic => businessLogic.GetById(entityTypeId, callerContext));
        }

        /// <summary>
        /// Get all entity types by list of ids
        /// </summary>
        /// <param name="entityTypeIds">Collection of EntityType Ids to search in the system</param>
        /// <returns>Collection of EntityTypes with specified Ids in the Id list</returns>
        public EntityTypeCollection GetEntityTypesByIds(Collection<Int32> entityTypeIds)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetEntityTypesByIds", MDMTraceSource.DataModel, false);

            EntityTypeCollection entityTypes = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetEntityTypesByIds' request message.", MDMTraceSource.DataModel);

                EntityTypeBL entityTypeBL = new EntityTypeBL();
                entityTypes = entityTypeBL.GetEntityTypesByIds(entityTypeIds);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetEntityTypesByIds' response message.", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetEntityTypesByIds", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        /// <summary>
        /// Get entity type which are mapped to a container
        /// </summary>
        /// <param name="containerId">All entity types mapped to this container will be fetched.</param>
        /// <returns>Collection of EntityType</returns>
        [Obsolete("This method has been obsoleted. Please use GetMappedEntityTypesWithContainer method instead of this")]
        public EntityTypeCollection GetMappedEntityTypes(Int32 containerId)
        {
            //Initialize trace source and start trace activity
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetMappedEntityTypes", MDMTraceSource.DataModel, false);

            EntityTypeCollection entityTypes = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService receives 'GetMappedEntityTypes' request message.", MDMTraceSource.DataModel);

                ContainerEntityTypeMappingBL containerEntityTypeMappingBL = new ContainerEntityTypeMappingBL();
                entityTypes = containerEntityTypeMappingBL.GetMappedEntityTypes(containerId, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling));

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService sends 'GetMappedEntityTypes' response message.", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetMappedEntityTypes", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        /// <summary>
        /// Get entity type based on its unique identifier (Entity type name and parent entity type name)
        /// </summary>
        /// <param name="entityTypeUniqueIdentifier">Entity type unique identifier</param>
        /// <param name="callerContext">caller context</param>
        /// <returns>Returns entity type </returns>

        /// <summary>
        /// Get entity Types based on unique short names provided.        
        /// </summary>
        /// <param name="entityTypeShortNames">Collection of unique short names for an entity type</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Entity type collection</returns>
        public EntityTypeCollection GetEntityTypesByShortNames(Collection<String> entityTypeShortNames, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<EntityTypeBL, EntityTypeCollection>("GetEntityTypesByShortNames",
                                         businessLogic =>
                                             businessLogic.GetByNames(entityTypeShortNames, callerContext));
        }

        /// <summary>
        /// Get entity Type based on unique short name eprovided.
        /// </summary>
        /// <param name="entityTypeShortName">Unique short name for an entity type</param>
        /// <param name="callerContext">caller context</param>
        /// <returns>Entitytype</returns>
        public EntityType GetEntityTypeByShortName(String entityTypeShortName, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<EntityTypeBL, EntityType>("GetEntitytypeByName",
                                         businessLogic =>
                                             businessLogic.GetByShortName(entityTypeShortName, callerContext));
        }
        #endregion

        #region RelationshipType methods

        /// <summary>
        /// Get relationship type based on relationship type name.
        /// </summary>
        /// <param name="relationshipTypeName">name of relationship type</param>
        /// <param name="callerContext">caller context</param>
        /// <returns>returns relationship type object</returns>
        public RelationshipType GetRelationshipTypeByName(String relationshipTypeName, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<RelationshipTypeBL, RelationshipType>("GetRelationshipTypeByName",
                                         businessLogic =>
                                             businessLogic.GetByName(relationshipTypeName, callerContext));
        }

        #endregion

        #region Dependency Attribute Methods

        /// <summary>
        /// Get dependency mapping details for requested attribute.
        /// This method will return the link table details. 
        /// For example if the the attribute is lookup then will return the WSID of the lookup table which mapped to the requested attribute.
        /// If the attribute is non-lookup attribute then will return the dependent values based on the mapping link.
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which dependency details needs to be fetched</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="dependentAttributeCollection">Dependency Attribute mapping details for the attribute</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Collection of string values</returns>
        public Collection<String> GetDependencyMappings(Int32 attributeId, ApplicationContext applicationContext, DependentAttributeCollection dependentAttributeCollection, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetDependencyMappings", MDMTraceSource.Attribute, false);

            Collection<String> result = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService has received 'GetDependencyMappings' request message.", MDMTraceSource.Attribute);

                AttributeDependencyBL attributeDependencyBL = new AttributeDependencyBL();

                result = attributeDependencyBL.GetDependencyMappings(attributeId, applicationContext, dependentAttributeCollection, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService has sent 'GetDependencyMappings' response message.", MDMTraceSource.Attribute);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetDependencyMappings", MDMTraceSource.Attribute);
            }

            return result;
        }

        /// <summary>
        /// Get the dependency Details for the requested attribute.
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which dependency details needs to be fetched</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="includeChildDependency">Indicates whether load children dependency details or not</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection GetDependencyDetails(Int32 attributeId, ApplicationContext applicationContext, Boolean includeChildDependency, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelService.GetDependencyDetails", MDMTraceSource.Attribute, false);

            DependentAttributeCollection dependentAttributes = null;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService has received 'GetDependencyDetails' request message.", MDMTraceSource.Attribute);

                AttributeDependencyBL attributeDependencyBL = new AttributeDependencyBL();

                dependentAttributes = attributeDependencyBL.GetDependencyDetails(attributeId, applicationContext, includeChildDependency, callerContext);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelService has sent 'GetDependencyDetails' response message.", MDMTraceSource.Attribute);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelService.GetDependencyDetails", MDMTraceSource.Attribute);
            }

            return dependentAttributes;
        }

        /// <summary>
        /// Get all dependencies for given attribute.
        /// </summary>
        /// <param name="attributeId">Attribute Id for which dependencies are to be selected</param>
        /// <param name="callerContext">Context indicating application making this API call.</param>
        /// <returns>Attribute Dependencies having collection of parent attribute and context for given attribute id</returns>
        public DependentAttributeCollection GetAttributeDependencies(Int32 attributeId, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, DependentAttributeCollection>("GetAttributeDependencies", businessLogic => businessLogic.GetAttributeDependencies(attributeId, callerContext));
        }

        /// <summary>
        /// Get all child dependent attribute models
        /// </summary>
        /// <param name="modelContext">Attribute model context which indicates what all attribute models to load.</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Dependent child attribute model</returns>
        public AttributeModelCollection GetAllDependentChildAttributeModels(AttributeModelContext modelContext, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>("GetAllDependentChildAttributeModels", businessLogic => businessLogic.GetAllDependentChildAttributeModels(modelContext, callerContext));
        }

        /// <summary>
        /// Get the dependent attribute data for the requested link table
        /// </summary>
        /// <param name="linkTableName">Indicates the link table name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="callerContext">Indicates the caller context information regarding application and module.</param>
        /// <returns>Returns the dependent attribute data mapping collection for the respective link table.</returns>
        public DependentAttributeDataMapCollection GetDependentData(String linkTableName, LocaleEnum locale, CallerContext callerContext)
        {
            AttributeModelBL modelBL = new AttributeModelBL();
            LookupBL lookupBL = new LookupBL();

            return this.MakeBusinessLogicCall<AttributeDependencyBL, DependentAttributeDataMapCollection>("GetDependentData", businessLogic => businessLogic.GetDependentData(linkTableName, locale, modelBL, lookupBL, callerContext));
        }

        #endregion

        #region Dependency Attribute CUD

        /// <summary>
        /// Create attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult CreateAttributeDependency(Int32 attributeId, DependentAttribute dependentAttribute, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResult>("CreateAttributeDependency", businessLogic => businessLogic.Create(attributeId, dependentAttribute, new AttributeModelBL(), callerContext));
        }

        /// <summary>
        /// Update attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult UpdateAttributeDependency(Int32 attributeId, DependentAttribute dependentAttribute, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResult>("UpdateAttributeDependency", businessLogic => businessLogic.Update(attributeId, dependentAttribute, new AttributeModelBL(), callerContext));
        }

        /// <summary>
        /// Delete attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult DeleteAttributeDependency(Int32 attributeId, DependentAttribute dependentAttribute, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResult>("DeleteAttributeDependency", businessLogic => businessLogic.Delete(attributeId, dependentAttribute, new AttributeModelBL(), callerContext));
        }

        /// <summary>
        /// Create - Update or Delete given application context
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be processed</param>
        /// <param name="dependentAttributes">DependentAttribute collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection ProcessAttributeDependencies(Int32 attributeId, DependentAttributeCollection dependentAttributes, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResultCollection>("ProcessAttributeDependencies", businessLogic => businessLogic.Process(attributeId, dependentAttributes, new AttributeModelBL(), callerContext));
        }

        /// <summary>
        /// Create - Update or Delete given application context
        /// </summary>
        /// <param name="dependentAttributeDataMaps">DependentAttribute Data map collection to process</param>
        /// <param name="objectType">Indicates types of object</param>
        /// <param name="userName">Indicates the name of user</param>
        /// <param name="programName">Indicates the name of program</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection ProcessDependentData(DependentAttributeDataMapCollection dependentAttributeDataMaps, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResultCollection>("ProcessDependentData", businessLogic => businessLogic.ProcessDependentData(dependentAttributeDataMaps, callerContext));
        }

        /// <summary>
        /// Create attribute data dependencies 
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult CreateDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResult>("CreateDependentData", businessLogic => businessLogic.CreateDependentData(dependentAttributeDataMap, callerContext));
        }

        /// <summary>
        /// Update attribute data dependency 
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to update</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult UpdateDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResult>("UpdateDependentData", businessLogic => businessLogic.UpdateDependentData(dependentAttributeDataMap, callerContext));
        }

        /// <summary>
        /// Delete attribute data dependency
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to delete</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult DeleteDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResult>("DeleteDependentData", businessLogic => businessLogic.DeleteDependentData(dependentAttributeDataMap, callerContext));
        }

        /// <summary>
        /// Process attribute data dependency 
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to delete</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<AttributeDependencyBL, OperationResult>("DeleteDependentData", businessLogic => businessLogic.ProcessDependentData(dependentAttributeDataMap, callerContext));
        }

        #endregion Dependency Attribute CUD

        #region Organization Get

        /// <summary>
        /// Get organization by Id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="organizationContext">Indicates context containing flag to load attributes or not</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Organization</returns>
        public Organization GetOrganizationById(Int32 organizationId, OrganizationContext organizationContext, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<OrganizationBL, Organization>("GetOrganizationById",
                                         businessLogic =>
                                             businessLogic.GetById(organizationId, organizationContext, callerContext));
        }

        /// <summary>
        /// Get all organizations in system by caller context
        /// </summary>
        /// <param name="organizationContext">Organization Context</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on organizations</returns>
        public OrganizationCollection GetAllOrganizations(OrganizationContext organizationContext,
            CallerContext callerContext)
        {

            return MakeBusinessLogicCall<OrganizationBL, OrganizationCollection>("GetAllOrganizations",
                                         businessLogic =>
                                             businessLogic.GetAll(organizationContext, callerContext));
        }

        /// <summary>
        /// Get organization based on name provided
        /// </summary>
        /// <param name="organizationShortName">Name of the organization</param>
        /// <param name="organizationContext">Organization Context</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Returns organization object</returns>
        public Organization GetOrganizationByName(String organizationShortName, OrganizationContext organizationContext, CallerContext callerContext)
        {

            return MakeBusinessLogicCall<OrganizationBL, Organization>("GetOrganizationByName",
                                         businessLogic =>
                                             businessLogic.GetByName(organizationShortName, organizationContext, callerContext));
        }

        /// <summary>
        /// Get all organization childs in system by caller context
        /// </summary>
        /// <param name="parentOrganizationId">Organization whick childs are requested</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on organizations</returns>
        public MDMObjectInfoCollection GetAllOrganizationDependencies(Int32 parentOrganizationId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<OrganizationBL, MDMObjectInfoCollection>("GetAllOrganizationDependencies",
                                         businessLogic =>
                                             businessLogic.GetAllOrganizationDependencies(parentOrganizationId, callerContext));
        }

        #endregion Organization Get

        #region Hierarchy Get

        /// <summary>
        /// Get Hierarchy based on provided name.
        /// </summary>
        /// <param name="hierarchyShortName">Short name - i.e. unique name of hierarchy</param>
        /// <param name="callerContext">Caller context to dentoe appilaction and module who is calling API</param>
        /// <returns>Returns hierarchy object</returns>
        public Hierarchy GetHierarchyByName(String hierarchyShortName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<HierarchyBL, Hierarchy>("GetHierarchyByName",
                                                                     businessLogic => businessLogic.GetByName(hierarchyShortName, callerContext));
        }

        #endregion

        #region Attribute Group

        /// <summary>
        /// Get attribute group name collection based on provided name and context.
        /// </summary>
        /// <param name="attributeGroupShortName">Short Name of the attribute group</param>
        /// <param name="attributeModelContext">Attribute model context defines context from which we need to get attribute group</param>
        /// <param name="callerContext">Caller context to denote application and module information</param>
        /// <returns>Returns collection of attribute group with all details.</returns>
        public Collection<AttributeGroup> GetAttributeGroupsByName(String attributeGroupShortName, AttributeModelContext attributeModelContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AttributeGroupBL, Collection<AttributeGroup>>("GetAttributeGroupsByName",
                                                                     businessLogic => businessLogic.GetByName(attributeGroupShortName, attributeModelContext, callerContext));
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Makes calls of DataModel Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(string methodName, Func<TBusinessLogic, TResult> call) where TBusinessLogic : BusinessLogicBase, new()
        {
            return MakeBusinessLogicCall(methodName, call, new TBusinessLogic());
        }

        /// <summary>
        /// Makes calls of DataModel Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <param name="businessLogicInstance">Represents instance of business logic</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(string methodName, Func<TBusinessLogic, TResult> call, TBusinessLogic businessLogicInstance) where TBusinessLogic : BusinessLogicBase, new()
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("DataModelService." + methodName, MDMTraceSource.DataModel, false);
            }

            TResult operationResult;

            try
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "DataModelService receives" + methodName + " request message.",
                                              MDMTraceSource.DataModel);

                operationResult = call(businessLogicInstance);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "DataModelService receives" + methodName + " response message.",
                                              MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService." + methodName, MDMTraceSource.DataModel);
                }
            }

            return operationResult;
        }

        #endregion #region Private Methods

        #region EntityType CUD

        /// <summary>
        /// Create operations of entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityType">Entity type to create</param>
        /// <param name="callerContext">Indicates application making the API call</param>
        /// <returns>Indicates result of operation</returns>
        public OperationResult CreateEntityType(EntityType entityType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeBL, OperationResult>("CreateEntityType",
                                         businessLogic =>
                                             businessLogic.Create(entityType, callerContext));
        }

        /// <summary>
        /// Update operations of entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityType">Entity type to update</param>
        /// <param name="callerContext">Indicates application making the API call</param>
        /// <returns>Indicates result of operation</returns>
        public OperationResult UpdateEntityType(EntityType entityType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeBL, OperationResult>("UpdateEntityType",
                                         businessLogic =>
                                             businessLogic.Update(entityType, callerContext));
        }

        /// <summary>
        /// Delete operations of entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityType">Entity type to delete</param>
        /// <param name="callerContext">Indicates application making the API call</param>
        /// <returns>Indicates result of operation</returns>
        public OperationResult DeleteEntityType(EntityType entityType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeBL, OperationResult>("DeleteEntityType",
                                         businessLogic =>
                                             businessLogic.Delete(entityType, callerContext));
        }

        /// <summary>
        /// Process (CRUD) operations with collection on entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityTypes">Collections of Entity types</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="callerContext"></param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResultCollection ProcessEntityTypes(EntityTypeCollection entityTypes, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeBL, OperationResultCollection>("ProcessEntityTypes",
                                         businessLogic =>
                                             businessLogic.Process(entityTypes, callerContext));
        }

        #endregion EntityType CUD

        #region RelationshipType CUD

        /// <summary>
        /// Create new RelationshipType
        /// </summary>
        /// <param name="relationshipType">Represent RelationshipType Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Creation</returns>
        /// <exception cref="ArgumentNullException">If RelationshipType Object is Null</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType LongName is Null or having empty String</exception>
        public OperationResult CreateRelationshipType(RelationshipType relationshipType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeBL, OperationResult>("CreateRelationshipType",
                businessLogic => businessLogic.Create(relationshipType, callerContext));
        }

        /// <summary>
        /// Update RelationshipType
        /// </summary>
        /// <param name="relationshipType">Represent RelationshipType Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Updating</returns>
        /// <exception cref="ArgumentNullException">If RelationshipType Object is Null</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType LongName is Null or having empty String</exception>
        public OperationResult UpdateRelationshipType(RelationshipType relationshipType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeBL, OperationResult>("UpdateRelationshipType",
                businessLogic => businessLogic.Update(relationshipType, callerContext));

        }

        /// <summary>
        /// Delete RelationshipType
        /// </summary>
        /// <param name="relationshipType">Represent RelationshipType Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Updating</returns>
        /// <exception cref="ArgumentNullException">If RelationshipType Object is Null</exception>
        public OperationResult DeleteRelationshipType(RelationshipType relationshipType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeBL, OperationResult>("DeleteRelationshipType",
                businessLogic => businessLogic.Delete(relationshipType, callerContext));

        }

        /// <summary>
        /// Process RelationshipTypes
        /// </summary>
        /// <param name="relationshipTypes">Represent RelationshipType Object collection to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipTypes process</returns>
        /// <exception cref="ArgumentNullException">If RelationshipTypes Object is Null</exception>
        public OperationResultCollection ProcessRelationshipTypes(RelationshipTypeCollection relationshipTypes, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeBL, OperationResultCollection>("ProcessRelationshipTypes",
                businessLogic => businessLogic.Process(relationshipTypes, callerContext));

        }

        #endregion RelationshipType CUD

        #region RelationshipType Get

        /// <summary>
        /// Get all relationshipTypes
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>All Relationship types</returns>
        public RelationshipTypeCollection GetAllRelationshipTypes(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeBL, RelationshipTypeCollection>("GetAllRelationshipTypes",
                businessLogic => businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Get relationshipType by Id
        /// </summary>
        /// <param name="relationshipTypeId">RelationshipTypeId to search</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>Relationship type for given ID</returns>
        public RelationshipType GetRelationshipTypeById(Int32 relationshipTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeBL, RelationshipType>("GetAllRelationshipTypeById",
                businessLogic => businessLogic.GetById(relationshipTypeId, callerContext));
        }

        /// <summary>
        /// Gets all relationship types based on container id and entity type id.
        /// </summary>
        /// <param name="containerId">Specifies container id.</param>
        /// <param name="entityTypeId">Specifies entity type id.</param>
        /// <param name="callerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>collection of relationship types</returns>
        public RelationshipTypeCollection GetRelationshipTypes(Int32 containerId, Int32 entityTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeBL, RelationshipTypeCollection>("GetRelationshipTypes",
                businessLogic => businessLogic.GetRelationshipTypes(containerId, entityTypeId, callerContext));
        }

        #endregion RelationshipType Get

        #region hierarchy Get

        /// <summary>
        /// Get all hierarchies in system by caller context
        /// </summary>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on hierarchies</returns>
        public HierarchyCollection GetAllHierarchies(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<HierarchyBL, HierarchyCollection>("GetAllHierarchies",
                                                                      businessLogic => businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Get hierarchy by id
        /// </summary>
        /// <param name="hierarchyId">Id of the hierarchy</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on hierarchies</returns>
        public Hierarchy GetHierarchyById(Int32 hierarchyId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<HierarchyBL, Hierarchy>("GetHierarchyById",
                                                                      businessLogic => businessLogic.GetById(hierarchyId, callerContext));
        }

        #endregion hierarchy Get

        #region hierarchy CUD

        /// <summary>
        /// Creates entity of type <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="hierarchy">hierarchy</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult CreateHierarchy(Hierarchy hierarchy, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<HierarchyBL, OperationResult>("Create hierarchy",
                                         businessLogic =>
                                             businessLogic.Create(hierarchy, callerContext));
        }

        /// <summary>
        /// Updates entity of type <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="hierarchy">hierarchy</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult UpdateHierarchy(Hierarchy hierarchy, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<HierarchyBL, OperationResult>("Update hierarchy",
                                         businessLogic =>
                                             businessLogic.Update(hierarchy, callerContext));
        }

        /// <summary>
        /// Deletes entity of type <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="hierarchy">hierarchy</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult DeleteHierarchy(Hierarchy hierarchy, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<HierarchyBL, OperationResult>("Delete hierarchy",
                                         businessLogic =>
                                             businessLogic.Delete(hierarchy, callerContext));
        }

        /// <summary>
        /// Process (CRUD) operations with entity of type <see cref="HierarchyCollection"/>
        /// </summary>
        /// <param name="hierarchyCollection">Collection of Hierarchies</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResultCollection ProcessHierarchies(HierarchyCollection hierarchyCollection, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<HierarchyBL, OperationResultCollection>("ProcessHierarchies",
                                         businessLogic =>
                                             businessLogic.Process(hierarchyCollection, callerContext));
        }

        #endregion hierarchy CUD

        #region Organization CUD

        /// <summary>
        /// Creates entity of type <see cref="Organization"/>
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult CreateOrganization(Organization organization, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<OrganizationBL, OperationResult>("CreateOrganization",
                                         businessLogic =>
                                             businessLogic.Create(organization, callerContext));
        }

        /// <summary>
        /// Updates entity of type <see cref="Organization"/>
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult UpdateOrganization(Organization organization, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<OrganizationBL, OperationResult>("UpdateOrganization",
                                         businessLogic =>
                                             businessLogic.Update(organization, callerContext));
        }

        /// <summary>
        /// Deletes entity of type <see cref="Organization"/>
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult DeleteOrganization(Organization organization, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<OrganizationBL, OperationResult>("DeleteOrganization",
                                         businessLogic =>
                                             businessLogic.Delete(organization, callerContext));
        }

        /// <summary>
        /// Process (CRUD) operations with collection on entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="organizationCollection">Collections of Entity types</param>
        /// <param name="callerContext"></param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResultCollection ProcessOrganizations(OrganizationCollection organizationCollection, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<OrganizationBL, OperationResultCollection>("ProcessOrganizations",
                                         businessLogic =>
                                             businessLogic.Process(organizationCollection, callerContext));
        }

        #endregion Organization CUD

        #region Category - Attribute Mapping Get

        /// <summary>
        /// Get  Category - Attribute Mappings
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>
        /// Collection on Category - Attribute Mapping
        /// </returns>
        public CategoryAttributeMappingCollection GetCategoryAttributeMappingsByCategoryId(Int64 categoryId,
                                                                                    CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, CategoryAttributeMappingCollection>("GetCategoryAttributeMappingsByCategoryId",
                                                                      businessLogic => businessLogic.GetByCategoryId(categoryId, callerContext));
        }

        /// <summary>
        /// Get  Category - Attribute Mappings
        /// </summary>
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>
        /// Collection on Category - Attribute Mapping
        /// </returns>
        public CategoryAttributeMappingCollection GetCategoryAttributeMappingsByCatalogId(Int32 catalogId,
                                                                                          CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, CategoryAttributeMappingCollection>("GetCategoryAttributeMappingsByCatalogId",
                                                                      businessLogic => businessLogic.GetByCatalogId(catalogId, callerContext));
        }


        /// <summary>
        /// Get  Category - Attribute Mappings
        /// </summary>
        /// <param name="hierarchyId">The hierarchy identifier.</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>
        /// Collection on Category - Attribute Mapping
        /// </returns>
        public CategoryAttributeMappingCollection GetCategoryAttributeMappingsByHierarchyId(Int32 hierarchyId,
                                                                                            CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, CategoryAttributeMappingCollection>("GetCategoryAttributeMappingsByHierarchyId",
                                                                      businessLogic => businessLogic.GetByHierarchyId(hierarchyId, callerContext));
        }

        #endregion Category - Attribute Mapping Get

        #region Category - Attribute Mapping CUD

        /// <summary>
        /// Creates entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult CreateCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, OperationResult>("CreateCategoryAttributeMapping",
                                         businessLogic =>
                                             businessLogic.Create(categoryAttributeMapping, new EntityBL(), callerContext, new AttributeModelBL()));
        }

        /// <summary>
        /// Updates entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult UpdateCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, OperationResult>("UpdateCategoryAttributeMapping",
                                         businessLogic =>
                                             businessLogic.Update(categoryAttributeMapping, new EntityBL(), callerContext, new AttributeModelBL()));
        }

        /// <summary>
        /// Deletes entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult DeleteCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, OperationResult>("DeleteCategoryAttributeMapping",
                                         businessLogic =>
                                             businessLogic.Delete(categoryAttributeMapping, new EntityBL(), callerContext, new AttributeModelBL()));
        }

        /// <summary>
        /// Inherits entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public OperationResult InheritCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, OperationResult>("InheritCategoryAttributeMapping",
                                         businessLogic =>
                                             businessLogic.Inherit(categoryAttributeMapping, new EntityBL(), callerContext, new AttributeModelBL()));
        }

        /// <summary>
        /// Process (CRUD) operations with entity of type <see cref="CategoryAttributeMappingCollection"/>
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">Collection of Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Collection of the <see cref="OperationResult"/></returns>
        public OperationResultCollection ProcessCategoryAttributeMappings(CategoryAttributeMappingCollection categoryAttributeMappingCollection, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryAttributeMappingBL, OperationResultCollection>("ProcessCategoryAttributeMappings",
                                         businessLogic =>
                                             businessLogic.Process(categoryAttributeMappingCollection, new EntityBL(), callerContext, new AttributeModelBL()));
        }

        #endregion Category - Attribute Mapping CUD

        #region Entity Type Attribute Mapping

        #region Get

        /// <summary>
        /// Gets all EntityType Attribute Mappings from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All EntityType Attribute Mappings</returns>
        public EntityTypeAttributeMappingCollection GetAllEntityTypeAttributeMapping(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeAttributeMappingBL, EntityTypeAttributeMappingCollection>("Get All Entity Type Attribute Mapping",
                                             businessLogic =>
                                                 businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Gets EntityType Attribute Mappings from the system based on given EntityType Id
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which attribute mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>EntityType Attribute Mappings for a specified EntityTypeId</returns>
        public EntityTypeAttributeMappingCollection GetEntityTypeAttributeMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeAttributeMappingBL, EntityTypeAttributeMappingCollection>("Get Entity Type Attribute Mapping By EntityTypeId",
                                             businessLogic =>
                                                 businessLogic.GetMappingsByEntityTypeId(entityTypeId, callerContext));
        }

        /// <summary>
        /// Gets EntityType Attribute Mappings from the system based on given Attribute Id
        /// </summary>
        /// <param name="attributeId">Indicates the Attribute Id for which attribute mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>EntityType Attribute Mappings for a specified AttributeId</returns>
        public EntityTypeAttributeMappingCollection GetEntityTypeAttributeMappingsByAttributeId(Int32 attributeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeAttributeMappingBL, EntityTypeAttributeMappingCollection>("Get Entity Type Attribute Mapping By AttributeId",
                                                 businessLogic =>
                                                     businessLogic.GetMappingsByAttributeId(attributeId, callerContext));
        }

        /// <summary>
        /// Gets EntityType Attribute Mappings from the system based on EntityTypeId and AttributeGroupId
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which attribute mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>EntityType Attribute Mappings for a specified EntityTypeId and AttributeGroupId</returns>
        public EntityTypeAttributeMappingCollection GetEntityTypeAttributeMappings(Int32 entityTypeId, Int32 attributeGroupId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityTypeAttributeMappingBL, EntityTypeAttributeMappingCollection>("Get Entity Type Attribute Mapping By EntityTypeId and AttributeGroupId",
                                                  businessLogic =>
                                                      businessLogic.GetMappingsByEntityTypeIdAndAttributeGroupId(entityTypeId, attributeGroupId));
        }

        #endregion Get

        /// <summary>
        /// Create, Update or Delete EntityType Attribute Mapping 
        /// </summary>
        /// <param name="entityTypeAttributeMappings">EntityTypeAttributeMappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessEntityTypeAttributeMapping(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, CallerContext callerContext)
        {
            EntityTypeAttributeMappingBL businessLogicInstance = new EntityTypeAttributeMappingBL(new EntityTypeBL(), new AttributeModelBL(), new ContainerBL());
            return MakeBusinessLogicCall<EntityTypeAttributeMappingBL, OperationResult>("Process Entity Type Attribute Mapping",
                                                  businessLogic =>
                                                      businessLogic.Process(entityTypeAttributeMappings, callerContext), businessLogicInstance);
        }

        #endregion Entity Type Attribute Mapping

        #region Container Entity Type Attribute Mapping

        /// <summary>
        /// Gets Container Entity Type Attribute Mappings from the system based on container Id, entityType Id , attributeGroup Id and attribute Id
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which attribute mappings needs to be fetched</param>
        /// <param name="entityTypeId">Indicates the EntityType Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the container Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeId">Indicates the attribute Id for which attribute mappings needs to be fetched</param>
        /// <param name="CallerContext">Context which called the application</param>
        /// <returns>Container EntityType Attribute Mappings for a specified ContainerId, EntityTypeId, AttributeId and AttributeGroupId</returns>
        public ContainerEntityTypeAttributeMappingCollection GetContainerEntityTypeAttributeMappings(Int32 containerId, Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerEntityTypeAttributeMappingBL, ContainerEntityTypeAttributeMappingCollection>("Get Container EntityType Attribute Mappings",
                                                              businessLogic =>
                                                                  businessLogic.Get(containerId, entityTypeId, attributeGroupId, attributeId, callerContext));
        }

        /// <summary>
        /// Create, Update or delete Container EntityType Attribute Mappings
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">ContainerEntityTypeAttributeMappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessContainerEntityTypeAttributeMappings(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, CallerContext callerContext)
        {
            ContainerEntityTypeAttributeMappingBL businessLogicInstance = new ContainerEntityTypeAttributeMappingBL(new OrganizationBL(), new ContainerBL(), new EntityTypeBL(), new AttributeModelBL());
            return MakeBusinessLogicCall<ContainerEntityTypeAttributeMappingBL, OperationResult>("Process Container EntityType Attribute Mappings",
                                                              businessLogic =>
                                                                  businessLogic.Process(containerEntityTypeAttributeMappings, callerContext), businessLogicInstance);
        }

        #endregion Container Entity Type Attribute Mapping

        #region Container EntityType Mapping

        /// <summary>
        /// Gets all Container EntityType mappings from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All Container Entity type mappings</returns>
        public ContainerEntityTypeMappingCollection GetAllContainerEntityTypeMappings(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerEntityTypeMappingBL, ContainerEntityTypeMappingCollection>("Get All Container EntityType Mappings",
                                                  businessLogic =>
                                                      businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Gets Container EntityType mappings from the system based on containerId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which Container EntityType mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container EntityType mappings for a specified container Id</returns>
        public ContainerEntityTypeMappingCollection GetContainerEntityTypeMappingsByContainerId(Int32 containerId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerEntityTypeMappingBL, ContainerEntityTypeMappingCollection>("Get Container EntityType Mappings By ContainerId",
                                                  businessLogic =>
                                                      businessLogic.GetMappingsByContainerId(containerId, callerContext));
        }

        /// <summary>
        /// Gets Container EntityType mappings from the system based on EntityTypeId
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which Container EntityType mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container EntityType mappings for a specified EntityType Id</returns>
        public ContainerEntityTypeMappingCollection GetContainerEntityTypeMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerEntityTypeMappingBL, ContainerEntityTypeMappingCollection>("Get Container EntityType Mappings By EntityTypeId",
                                                  businessLogic =>
                                                      businessLogic.GetMappingsByEntityTypeId(entityTypeId, callerContext));
        }

        /// <summary>
        /// Gets mapped EntityTypes from the system based on containerId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for a specified container Id</returns>
        public EntityTypeCollection GetMappedEntityTypesWithContainer(Int32 containerId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerEntityTypeMappingBL, EntityTypeCollection>(
                bl => bl.GetMappedEntityTypes(containerId, callerContext),
                context => context.CallDataContext.ContainerIdList.Add(containerId));
        }

        /// <summary>
        /// Create, Update or Delete Container EntityType Mappings
        /// </summary>
        /// <param name="ContainerEntityTypeMappingCollection">ContainerEntityTypeMappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessContainerEntityTypeMappings(ContainerEntityTypeMappingCollection containerEntityTypeMappings, CallerContext callerContext)
        {
            ContainerEntityTypeMappingBL businessLogicInstance = new ContainerEntityTypeMappingBL();
            return MakeBusinessLogicCall<ContainerEntityTypeMappingBL, OperationResult>("Process Entity Type Attribute Mapping",
                                                  businessLogic =>
                                                      businessLogic.Process(containerEntityTypeMappings, callerContext), businessLogicInstance);
        }

        #endregion Container EntityType Mapping

        #region Container RelationshipType Attribute Mapping

        /// <summary>
        /// Gets Container RelationshipType Attribute mappings from the system based on containerId, relationshipId and attributeGroupId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType Attribute mappings for specified containerId, relationshipTypeId and attributeGroupId</returns>
        public ContainerRelationshipTypeAttributeMappingCollection GetContainerRelationshipTypeAttributeMappings(Int32 containerId, Int32 relationshipTypeId, Int32 attributeGroupId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeAttributeMappingBL, ContainerRelationshipTypeAttributeMappingCollection>("Get Container RelationshipType Attribute Mappings",
                                                      businessLogic =>
                                                          businessLogic.Get(containerId, relationshipTypeId, attributeGroupId, callerContext));
        }

        /// <summary>
        /// Create, Update or Delete Container RelationshipType Attribute Mappings
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings">Container RelationshipType Attribute Mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessContainerRelationshipTypeAttributeMappings(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, CallerContext callerContext)
        {
            ContainerRelationshipTypeAttributeMappingBL businessLogicBase = new ContainerRelationshipTypeAttributeMappingBL(new OrganizationBL(), new ContainerBL(), new RelationshipTypeBL(), new AttributeModelBL());
            return MakeBusinessLogicCall<ContainerRelationshipTypeAttributeMappingBL, OperationResult>("Process Container RelationshipType Attribute Mappings",
                                                          businessLogic =>
                                                              businessLogic.Process(containerRelationshipTypeAttributeMappings, callerContext), businessLogicBase);
        }

        #endregion Container RelationshipType Attribute Mapping

        #region Container RelationshipType EntityType Mapping

        /// <summary>
        /// Gets all container RelationshipType EntityType mappings from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All container RelationshipType EntityType mappings</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection GetAllContainerRelationshipTypeEntityTypeMappings(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeEntityTypeMappingBL, ContainerRelationshipTypeEntityTypeMappingCollection>("Get All Container RelationshipType EntityType Mappings",
                                                  businessLogic =>
                                                      businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Gets container RelationshipType EntityType mappings based on containerId and relationshipId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the relationship type Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType EntityType mappings  for specified containerId and RelationshipTypeId</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection GetContainerRelationshipTypeEntityTypeMappings(Int32 containerId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeEntityTypeMappingBL, ContainerRelationshipTypeEntityTypeMappingCollection>("Get Container RelationshipType EntityType Mappings",
                                                      businessLogic =>
                                                           businessLogic.Get(containerId, relationshipTypeId, callerContext));
        }

        /// <summary>
        /// Gets mapped EntityTypes based on ContainerId and RelationshipTypeId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the relationshipType Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for specified containerId and relationshipTypeId</returns>
        public EntityTypeCollection GetMappedEntityTypesWithContainerAndRelationshipType(Int32 containerId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeEntityTypeMappingBL, EntityTypeCollection>("Get Mapped EntityTypes With Container And RelationshipType",
                                                  businessLogic =>
                                                      businessLogic.GetMappedEntityTypes(containerId, relationshipTypeId, callerContext));
        }

        /// <summary>
        /// Create, Update or Delete Container RelationshipType EntityType Mappings
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCollection">Container RelationshipType EntityType Mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessContainerRelationshipTypeEntityTypeMappings(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMapping, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeEntityTypeMappingBL, OperationResult>("Process Container RelationshipType EntityType Mapping",
                                                  businessLogic =>
                                                      businessLogic.Process(containerRelationshipTypeEntityTypeMapping, callerContext));
        }

        #endregion  Container RelationshipType EntityType Mapping

        #region RelationshipType Attribute Mapping

        /// <summary>
        /// Gets RelationshipType attribute mappings from the system based on relationshpiypeId and attributeGroupId
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType attribute mappings for specified relationshipType Id and attributeGroup Id</returns>
        public RelationshipTypeAttributeMappingCollection GetRelationshipTypeAttributeMappings(Int32 relationshipTypeId, Int32 attributeGroupId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeAttributeMappingBL, RelationshipTypeAttributeMappingCollection>("Get RelationshipType Attribute Mappings",
                                                  businessLogic =>
                                                      businessLogic.Get(relationshipTypeId, attributeGroupId, callerContext));
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType attribute mappings
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings">RelationshipType attribute mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessRelationshipTypeAttributeMappings(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, CallerContext callerContext)
        {
            RelationshipTypeAttributeMappingBL businessLogicInstance = new RelationshipTypeAttributeMappingBL(new RelationshipTypeBL(), new AttributeModelBL(), new ContainerBL());
            return MakeBusinessLogicCall<RelationshipTypeAttributeMappingBL, OperationResult>("Process RelationshipType Attribute Mappings",
                                                  businessLogic =>
                                                      businessLogic.Process(relationshipTypeAttributeMappings), businessLogicInstance);
        }

        #endregion RelationshipType Attribute Mapping

        #region RelationshipType EntityType Mapping

        /// <summary>
        /// Gets all RelationshipType EntityType mappings from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All RelationshipType EntityType mappings</returns>
        public RelationshipTypeEntityTypeMappingCollection GetAllRelationshipTypeEntityTypeMappings(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeEntityTypeMappingBL, RelationshipTypeEntityTypeMappingCollection>("Get All Relationship Type Entity Type Mappings",
                                                  businessLogic =>
                                                      businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Gets RelationshipType EntityType mappings from the system based on RelationshipType Id
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType mappings for specified RelationshipType Id</returns>
        public RelationshipTypeEntityTypeMappingCollection GetRelationshipTypeEntityTypeMappingsByRelationshipTypeId(Int32 relationshipTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeEntityTypeMappingBL, RelationshipTypeEntityTypeMappingCollection>("Get RelationshipType EntityType Mappings By RelationshipTypeId",
                                                  businessLogic =>
                                                      businessLogic.GetMappingsByRelationshipTypeId(relationshipTypeId, callerContext));
        }

        /// <summary>
        /// Gets RelationshipType EntityType mappings from the system from the system based on EntityType Id
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType mappings for specified EntityType Id</returns>
        public RelationshipTypeEntityTypeMappingCollection GetRelationshipTypeEntityTypeMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeEntityTypeMappingBL, RelationshipTypeEntityTypeMappingCollection>("Get RelationshipType EntityType Mappings By EntityTypeId",
                                                  businessLogic =>
                                                      businessLogic.GetMappingsByEntityTypeId(entityTypeId, callerContext));
        }

        /// <summary>
        /// Gets mapped EntityTypes from the system based on RelationshipType Id
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for specified RelationshipType Id</returns>
        public EntityTypeCollection GetMappedEntityTypesWithRelationshipType(Int32 relationshipTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeEntityTypeMappingBL, EntityTypeCollection>("Get Mapped EntityTypes With RelationshipType",
                                                  businessLogic =>
                                                      businessLogic.GetMappedEntityTypes(relationshipTypeId, callerContext));
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType EntityType mappings
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings">RelationshipType EntityType mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult ProcessRelationshipTypeEntityTypeMappings(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMapping, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<RelationshipTypeEntityTypeMappingBL, OperationResult>("Process RelationshipType EntityType Mappings",
                                                  businessLogic =>
                                                      businessLogic.Process(relationshipTypeEntityTypeMapping, callerContext));
        }

        #endregion RelationshipType EntityType Mapping

        #region Container RelationshipType EntityType Mapping Cardinalities

        /// <summary>
        /// Get Container RelationshipType EntityType MappingCardinalities
        /// </summary>
        /// <param name="containerId">Indicates the container id</param>
        /// <param name="fromEntityTypeId">Indicates the from entitytype id</param>
        /// <param name="relationshipTypeId">Indicates the from relationshiptype id</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Collection of ContainerEntityTypeRelationshipTypeCardinalities</returns>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection GetContainerRelationshipTypeEntityTypeMappingCardinalities(Int32 containerId, Int32 fromEntityTypeId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeEntityTypeMappingCardinalityBL, ContainerRelationshipTypeEntityTypeMappingCardinalityCollection>("Get Container RelationshipType EntityType Mapping Cardinalities",
                                                 businessLogic =>
                                                     businessLogic.Get(containerId, fromEntityTypeId, relationshipTypeId, callerContext));
        }


        /// <summary>
        /// Create, Update or Delete Container RelationshipType EntityType MappingCardinalities
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalities">Container RelationshipType EntityType MappingCardinalities to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection ProcessContainerRelationshipTypeEntityTypeMappingCardinalities(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalities, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeEntityTypeMappingCardinalityBL, OperationResultCollection>("Process Container RelationshipType EntityType Mapping Cardinalities",
                                                 businessLogic =>
                                                     businessLogic.Process(containerRelationshipTypeEntityTypeMappingCardinalities, callerContext));
        }

        #endregion Container RelationshipType EntityType Mapping Cardinalities

        #region Category Get/Search

        /// <summary>
        /// Gets the collection of category for the specified hierarchy id in a locale provided by user
        /// </summary>
        /// <param name="hierarchyId">Indicates the Hierarchy Id in which category is requested</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <param name="locale">Indicates the locale in which category is needed</param>
        /// <returns>Returns the collection of category</returns>
        public CategoryCollection GetAllCategories(Int32 hierarchyId, LocaleEnum locale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, CategoryCollection>("GetAllCategories",
                                                  businessLogic =>
                                                      businessLogic.GetAllCategories(hierarchyId, callerContext, locale));
        }

        /// <summary>
        /// Gets the collection of category for the specified container name in a locale provided by user
        /// </summary>
        /// <param name="containerName">">Indicates the container name in which categories are requested</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <param name="locale">Indicates the locale in which category is needed</param>
        /// <returns>Returns the collection of category</returns>
        public CategoryCollection GetAllCategoriesUsingContainerName(String containerName, LocaleEnum locale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, CategoryCollection>("GetAllCategoriesUsingContainerName",
                                                  businessLogic =>
                                                      businessLogic.GetAllCategoriesUsingContainerName(containerName, callerContext, locale));
        }

        /// <summary>
        /// Gets category details for the specified hierarchyid and categoryid
        /// </summary>
        /// <param name="hierarchyId">Indicates the hierarchy id in which category is requested</param>
        /// <param name="categoryId">Indicates the id of the category</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <returns>Returns Category Information</returns>
        public Category GetCategoryById(Int32 hierarchyId, Int64 categoryId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, Category>("GetById",
                                                  businessLogic =>
                                                      businessLogic.GetById(hierarchyId, categoryId, callerContext));
        }

        /// <summary>
        /// Gets categories details for the specified hierarchy and category mapping
        /// </summary>
        /// <param name="mappingCollection">Indicates the collection of hierarchy and category mappings</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <returns>Returns Category Information</returns>
        public CategoryCollection GetCategoriesByIds(HierachyCategoryMappingCollection mappingCollection, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, CategoryCollection>("GetCategoriesByIds",
                                                  businessLogic =>
                                                      businessLogic.GetByIds(mappingCollection, callerContext));
        }

        /// <summary>
        /// Gets the category for the specified hierarchy id and category name
        /// </summary>
        /// <param name="hierarchyId">Indicates the hierarchy id in which category is requested</param>
        /// <param name="categoryName">Indicates the name of the category</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <returns>Returns category information</returns>
        public Category GetCategoryByName(Int32 hierarchyId, String categoryName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, Category>("GetByName",
                                                  businessLogic =>
                                                      businessLogic.GetByName(hierarchyId, categoryName, callerContext));
        }

        /// <summary>
        /// Gets the category for the specified container name and category name
        /// </summary>
        /// <param name="containerName">Indicates the container name in which category is requested</param>
        /// <param name="categoryName">Indicates the name of the category</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <returns>Returns category object</returns>
        public Category GetCategoryByNameUsingContainerName(String containerName, String categoryName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, Category>("GetCategoryByNameUsingContainerName",
                                                  businessLogic =>
                                                      businessLogic.GetByNameUsingContainerName(containerName, categoryName, callerContext));
        }

        /// <summary>
        /// Gets the category for the specified hierarchy id and category path
        /// </summary>
        /// <param name="hierarchyId">Indicates the hierarchy id in which category is requested</param>
        /// <param name="categoryPath">Indicates the path of the category</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <returns>Returns Category Information</returns>
        public Category GetCategoryByPath(Int32 hierarchyId, String categoryPath, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, Category>("GetByPath",
                                                  businessLogic =>
                                                      businessLogic.GetByPath(hierarchyId, categoryPath, callerContext));
        }

        /// <summary>
        /// Gets category for the specified container name and category path
        /// </summary>
        /// <param name="containerName">Indicates the container name in which category is requested</param>
        /// <param name="categoryPath">Indicates the path of the category</param>
        /// <param name="callerContext">Indicates the caller context of the api</param>
        /// <returns>Returns Category Information</returns>
        public Category GetCategoryByPathUsingContainerName(String containerName, String categoryPath, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, Category>("GetCategoryByPathUsingContainerName",
                                                  businessLogic =>
                                                      businessLogic.GetByPathUsingContainerName(containerName, categoryPath, callerContext));
        }

        /// <summary>
        /// Search Categories for requested context
        /// </summary>
        /// <param name="categoryContext">Search Context containing hierarchy id, locale and other criteria. </param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Category</returns>
        public CategoryCollection SearchCategories(CategoryContext categoryContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, CategoryCollection>("Search",
                                                  businessLogic =>
                                                      businessLogic.Search(categoryContext, callerContext));
        }

        #endregion

        #region UOM Get

        /// <summary>
        /// Get UOM  based on UOM context.
        /// </summary>
        /// <param name="uomContext">Specifies UOM context containing short name , UOM type</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>UOM object based on context.</returns>
        public UOM GetUom(UomContext uomContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<UomBL, UOM>("GetUom", businessLogic => businessLogic.GetUom(uomContext, callerContext));
        }

        /// <summary>
        /// Get UOM Conversion rates XML based on UOM context.
        /// </summary>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Conversion rates XML as string.</returns>
        public string GetUomConversionsAsXml(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<UomBL, string>("GetUomConversionsAsXml", businessLogic => businessLogic.GetUomConversionsAsXml(callerContext));
        }

        #endregion

        #region Search Category Methods

        /// <summary>
        /// Search Categories for given search criteria and return list of categories with specified context. 
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned categories.</param>
        /// <param name="callerContext">Provides search criteria.</param>
        /// <param name="searchOperationResult"></param>
        /// <param name="iEntityManager">Indicates IEntityManager</param>
        /// <param name="callerContext">Indicates in which context caller is calling the method</param>
        /// <returns>Search results - collection of entities</returns>
        public EntityCollection SearchCategories(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult searchOperationResult, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategorySearchBL, EntityCollection>("SearchCategories",
                                                  businessLogic =>
                                                      businessLogic.SearchCategories(searchCriteria, searchContext, searchOperationResult, new EntityBL(), callerContext));
        }

        #endregion

        # region Unique Id Methods
        /// <summary>
        /// Generate unique (id / list of ids) based on the context provided
        /// </summary>        
        /// <param name="callerContext">Represents Context indicating the caller of the API</param>
        /// <param name="uniqueIdGenerateContext">Represents Context indicating the unique id of the API</param>
        /// <returns>collection of string having auto ids generated from DB</returns>
        public Collection<String> GenerateUniqueId(CallerContext callerContext, UniqueIdGenerationContext uniqueIdGenerationContext)
        {
            if (uniqueIdGenerationContext.ResolveNameToId)
            {
                return GenerateUniqueIdByNames(callerContext, uniqueIdGenerationContext);
            }
            else
            {
                return MakeBusinessLogicCall<BusinessRuleBL, Collection<String>>("GetUniqueId",
                                             businessLogic =>
                                                 businessLogic.GetUniqueId(uniqueIdGenerationContext.DataModelObjectType, uniqueIdGenerationContext.OrganizationId,
                                                 uniqueIdGenerationContext.ContainerId, uniqueIdGenerationContext.CategoryId, uniqueIdGenerationContext.EntityTypeId,
                                                 uniqueIdGenerationContext.RelationshipTypeId, uniqueIdGenerationContext.Locale, uniqueIdGenerationContext.RoleId,
                                                 uniqueIdGenerationContext.UserId, uniqueIdGenerationContext.NoOfUIdsToGenerate));
            }
        }

        #endregion

        #region Entity Model Methods

        /// <summary>
        /// Gets the entity model details by providing container name, category name, entity type name and parent entity name in entity context
        /// </summary>
        /// <param name="entityContext">Indicates the data context for which entity needs to be fetched</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns entity object</returns>
        public Entity GetEntityModel(EntityContext entityContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityBL, Entity>("GetModel", businessLogic =>
                businessLogic.GetModel(entityContext.EntityTypeId, entityContext.CategoryId, entityContext, callerContext));
        }

        #endregion

        #region Entity Variant Definition Mapping

        /// <summary>
        /// Get all entity variant definition mappings
        /// </summary>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module.</param>
        /// <returns>Returns collection of entity variant definition mappings</returns>
        public EntityVariantDefinitionMappingCollection GetAllEntityVariantDefinitionMappings(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityVariantDefinitionMappingBL, EntityVariantDefinitionMappingCollection>("GetAll", businessLogic =>
                businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Create, Update and Delete entity variant definition mapping 
        /// </summary>
        /// <param name="entityVariantDefinitionMappings">Indicates collection of entity variant definition mappings to be processed.</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module.</param>
        /// <returns>Returns operation result.</returns>
        public OperationResultCollection ProcessEntityVariantDefinitionMappings(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityVariantDefinitionMappingBL, OperationResultCollection>("Process", businessLogic =>
                businessLogic.Process(entityVariantDefinitionMappings, callerContext), new EntityVariantDefinitionMappingBL(new ContainerBL(), new CategoryBL()));
        }

        #endregion Entity Variant Definition Mapping

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="categoryId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public EntityVariantDefinition GetHierarchyDefinitionByCtxt(Int32 containerId, Int64 categoryId, Int32 entityTypeId, CallerContext context = null)
        {
            DiagnosticActivity curActivity = new DiagnosticActivity();
            TraceSettings curSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            ExecutionContext execCtxt = new ExecutionContext();

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
                    execCtxt.CallDataContext.ContainerIdList.Add(containerId);
                    execCtxt.CallDataContext.CategoryIdList.Add(categoryId);
                    execCtxt.CallDataContext.EntityTypeIdList.Add(entityTypeId);

                    execCtxt.SecurityContext = new SecurityContext();
                    execCtxt.SecurityContext.UserLoginName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    curActivity.Start(execCtxt);
                }
                else
                    curActivity.Start();
            }


            EntityVariantDefinition entityHierarchyDefinition = null;
            try
            {

                EntityVariantDefinitionBL entityHierarchyDefinitionBL = new EntityVariantDefinitionBL(new AttributeModelBL());
                entityHierarchyDefinition = entityHierarchyDefinitionBL.GetByContext(containerId, categoryId, entityTypeId, context);

            }
            catch (Exception ex)
            {
                curActivity.LogError(ex.Message);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop trace activity
                if (curSettings.IsBasicTracingEnabled)
                {
                    curActivity.Stop();

                    //ICache cacheManager =  CacheFactory.GetDistributedCache();

                    //if (cacheManager != null)
                    //{
                    //    cacheManager.Set(context.OperationId.ToString(), curActivity, DateTime.Now.AddHours(2));
                    //}
                }
            }

            return entityHierarchyDefinition;
        }

        private Int32 GetRoleId(String roleName, CallerContext callerContext)
        {
            Int32 roleId = 0;
            if (!String.IsNullOrEmpty(roleName))
            {
                SecurityRoleBL securityRoleBl = new SecurityRoleBL();
                SecurityRole roleObj = securityRoleBl.GetByName(roleName, callerContext);
                if (roleObj != null)
                    roleId = roleObj.Id;
            }
            return roleId;
        }

        private Int32 GetUserId(String userName, CallerContext callerContext)
        {
            Int32 userId = 0;
            if (!String.IsNullOrEmpty(userName))
            {
                SecurityUserBL securityUserBl = new SecurityUserBL();
                SecurityUserCollection userObj = securityUserBl.GetUsersByLoginNames(new Collection<string> { userName }, callerContext);
                if (userObj.FirstOrDefault() != null)
                    userId = userObj.FirstOrDefault().Id;
            }
            return userId;
        }

        /// <summary>
        /// Generate unique (id / list of ids) based on the context provided
        /// </summary>
        /// <param name="callerContext">Represents Context indicating the caller of the API</param>
        /// <param name="uniqueIdGenerationContext">Represents Context indicating the unique id of the API</param>
        /// <returns>collection of string having auto ids generated from DB</returns>
        private Collection<String> GenerateUniqueIdByNames(CallerContext callerContext, UniqueIdGenerationContext uniqueIdGenerationContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("DataModelService.GenerateUniqueIdByNames", MDMTraceSource.DataModel, false);
            }

            Collection<String> uniqueIdCollection;

            try
            {
                Int32 userId = GetUserId(uniqueIdGenerationContext.UserName, callerContext);
                Int32 roleId = GetRoleId(uniqueIdGenerationContext.RoleName, callerContext);

                Collection<String> relationshipTypeNames = new Collection<String>();

                if (!String.IsNullOrEmpty(uniqueIdGenerationContext.RelationshipTypeName))
                {
                    relationshipTypeNames.Add(uniqueIdGenerationContext.RelationshipTypeName);
                }
                EntityModelBL entityModelBl = new EntityModelBL();
                EntityModelContext entityModelContext = entityModelBl.GetEntityModelContextByName(callerContext, organizationName: uniqueIdGenerationContext.OrganizationName, containerName: uniqueIdGenerationContext.ContainerName,
                                                                        entityTypeName: uniqueIdGenerationContext.EntityTypeName, categoryPath: uniqueIdGenerationContext.CategoryPath, relationshipTypeNames: relationshipTypeNames);

                BusinessRuleBL businessRuleBl = new BusinessRuleBL();
                uniqueIdCollection = businessRuleBl.GetUniqueId(uniqueIdGenerationContext.DataModelObjectType, entityModelContext.OrganizationId, entityModelContext.ContainerId,
                                                     entityModelContext.CategoryId, entityModelContext.EntityTypeId, entityModelContext.RelationshipTypeIds.FirstOrDefault(),
                                                     uniqueIdGenerationContext.Locale, roleId, userId, uniqueIdGenerationContext.NoOfUIdsToGenerate);
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
                    MDMTraceHelper.StopTraceActivity("DataModelService.GenerateUniqueIdByNames", MDMTraceSource.DataModel);
                }
            }

            return uniqueIdCollection;
        }


        #endregion
    }
}