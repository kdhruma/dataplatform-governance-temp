using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.EntityManager.Business.EntityOperations
{
    using Core;
    using Core.Exceptions;
    using BusinessObjects;
    using AttributeModelManager.Business;
    using ContainerManager.Business;
    using DataModelManager.Business;
    using MessageManager.Business;
    using Utility;
    using CategoryManager.Business;
    using Helpers;

    /// <summary>
    /// 
    /// </summary>
    internal class EntityModelGetManager : BusinessLogicBase
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="parentEntityId"></param>
        /// <param name="entityContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Entity GetModel(Int32 entityTypeId, Int64 categoryId, Int64 parentEntityId, EntityContext entityContext, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                LocaleMessage localeMessage = new LocaleMessageBL().Get(GlobalizationHelper.GetSystemUILocale(), "111823", false, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity));
                throw new MDMOperationException("111823", localeMessage.Message, "EntityManager", String.Empty, "GetEntityModel"); //CallerContext is null or empty
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "GetEntityModel for entityTypeId: " + entityTypeId + " categoryId: " + categoryId + " parentEntityId: " + parentEntityId + " starting...", MDMTraceSource.EntityGet);

            var entity = new Entity();

            entity.EntityTypeId = entityTypeId;
            entity.ContainerId = entityContext.ContainerId;
            entity.CategoryId = categoryId;
            entity.ParentEntityId = parentEntityId;

            //Check locale detail in EntityContext and populate System Default Data Locale if no locale detail is provided.
            EntityOperationsCommonUtility.ValidateAndUpdateEntityContextForLocales(entityContext, callerContext.Application);

            // Check locale in context, if not available use default locale of an user making request
            LocaleEnum locale = entityContext.Locale;
            if (locale == LocaleEnum.UnKnown)
            {
                locale = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().UserPreferences.DataLocale;
                entityContext.Locale = locale;
            }

            //Check Data Locales.. If not available, populate entity context locale..
            if (entityContext.DataLocales == null || entityContext.DataLocales.Count < 1)
            {
                entityContext.DataLocales = new Collection<LocaleEnum> { locale };
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Locale for GetEntityModel: " + locale, MDMTraceSource.EntityGet);

            entity.Locale = locale;
            entity.EntityContext = entityContext;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LoadAttributes flag for GetEntityModel is set to: " + entityContext.LoadAttributes, MDMTraceSource.EntityGet);

            if (entityContext.LoadAttributes || entityContext.LoadAttributeModels)
            {
                Collection<Int32> attributeIdList = entityContext.AttributeIdList;
                Collection<Int32> attributeGroupIdList = entityContext.AttributeGroupIdList;

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to load attribute models", MDMTraceSource.EntityGet);

                //Prepare AttributeModelContext
                var attributeModelContext = new AttributeModelContext(entity.ContainerId, entity.EntityTypeId, 0, entity.CategoryId, entityContext.DataLocales, 0, entityContext.AttributeModelType, entityContext.LoadCreationAttributes, false /*only required attributes*/, false /*load complete details*/);
                attributeModelContext.ApplySorting = false;
                AttributeCollection attributeCollection;
                AttributeModelCollection attributeModelCollection;
                EntityAttributeModelHelper.LoadAttributeModelsWithBlankAttributeInstances(attributeIdList, attributeGroupIdList, null, attributeModelContext, out attributeCollection, out attributeModelCollection);

                if (entityContext.LoadAttributes)
                {
                    entity.Attributes = attributeCollection;
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of attributes loaded for model: " + entity.Attributes.Count, MDMTraceSource.EntityGet);
                }

                if (entityContext.LoadAttributeModels)
                {
                    var attributeModelManager = new AttributeModelBL();
                    AttributeModelCollection sortedAttributeModels = attributeModelManager.SortAttributeModels(attributeModelCollection); //This is required as GetEntityModel call comes from the UI
                    entity.AttributeModels = sortedAttributeModels;

                    //When UI was asking for GetEntityModel then we need to return attribute models only for entity.Locale instead of entityContext.DataLocales
                    if (entityContext.DataLocales.Count > 1)
                    {
                        var filteredAttributeModels = new AttributeModelCollection();

                        IEnumerable<AttributeModel> attributeModels = entity.AttributeModels.Where(attrModel => attrModel.Locale == entity.Locale).ToList();

                        foreach (AttributeModel attrModel in attributeModels)
                        {
                            filteredAttributeModels.Add(attrModel);
                        }

                        entity.AttributeModels = filteredAttributeModels;
                    }

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of attribute models loaded: " + entity.AttributeModels.Count, MDMTraceSource.EntityGet);
                }
            }

            var containerManager = new ContainerBL();
            var containerContext = new ContainerContext(false);
            Container entityContainer = containerManager.Get(entity.ContainerId, containerContext, callerContext);

            var entityTypeManager = new EntityTypeBL();
            EntityType entityEntityType = entityTypeManager.GetById(entity.EntityTypeId, callerContext);

            Int32 hierarchyId = 0;

            if (entityEntityType != null && entityContainer != null)
            {
                hierarchyId = entityContainer.HierarchyId;
                entity.EntityTypeName = entityEntityType.Name;
                entity.EntityTypeLongName = entityEntityType.LongName;
                entity.ContainerName = entityContainer.Name;
                entity.ContainerLongName = entityContainer.LongName;
                entity.OrganizationId = entityContainer.OrganizationId;
                entity.OrganizationName = entityContainer.OrganizationShortName;
                entity.OrganizationLongName = entityContainer.OrganizationLongName;
            }

            //populating category details
            if (categoryId > 0 && hierarchyId > 0)
            {
                var baseEntityContext = new EntityContext();
                baseEntityContext.ContainerId = entityContext.ContainerId;
                baseEntityContext.LoadEntityProperties = true;
                baseEntityContext.LoadAttributes = false;

                var categoryManager = new CategoryBL();
                var category = categoryManager.GetById(hierarchyId, categoryId, locale, callerContext, false);
                
                if (category != null)
                {
                    entity.CategoryLongNamePath = category.LongNamePath;
                    entity.CategoryPath = category.Path;
                }
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "GetEntityModel for entityTypeId: " + entityTypeId + " categoryId: " + categoryId + " parentEntityId: " + parentEntityId + " completed...", MDMTraceSource.EntityGet);

            return entity;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}