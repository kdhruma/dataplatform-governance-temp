using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.EntityModelManager.Business
{
    using MDM.BusinessObjects;
    using MDM.CategoryManager.Business;
    using MDM.ContainerManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Business;
    using MDM.HierarchyManager.Business;
    using MDM.Interfaces;
    using MDM.OrganizationManager.Business;
    using MDM.RelationshipManager.Business;
    using MDM.Utility;    

    /// <summary>
    /// Class provides methods to retrieve and process entity model data.
    /// </summary>
    public class EntityModelBL : IEntityModelManager
    {
        #region Public Methods

        /// <summary>
        /// Builds and returns an entity model context based on the various model names provided
        /// </summary>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <param name="organizationName">Represents the short name of the organization</param>
        /// <param name="containerName">Represents the short name of the container</param>
        /// <param name="hierarchyName">Represents the short name of the hierarchy</param>
        /// <param name="entityTypeName">Represents the short name of the entity type</param>
        /// <param name="categoryPath">Represents the short name path of the category</param>
        /// <param name="relationshipTypeNames">Represents the short name of the relationship type</param>
        /// <returns>An entity model context object</returns>
        public EntityModelContext GetEntityModelContextByName(CallerContext callerContext, String organizationName = null, String containerName = null, String hierarchyName = null, String entityTypeName = null, String categoryPath = null, Collection<String> relationshipTypeNames = null)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityModelBL.GetEntityModelContextByName", MDMTraceSource.DataModel, false);
            }

            EntityModelContext entityModelContext = null;

            try
            {
                entityModelContext = new EntityModelContext()
                {
                    OrganizationName = organizationName,
                    ContainerName = containerName,
                    HierarchyName = hierarchyName,
                    EntityTypeName = entityTypeName,
                    CategoryPath = categoryPath,
                    RelationshipTypeNames = relationshipTypeNames
                };

                FillEntityModelContextByName(ref entityModelContext, callerContext);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityModelBL.GetEntityModelContextByName", MDMTraceSource.DataModel);
                }
            }

            return entityModelContext;
        }

        /// <summary>
        /// Fills the entity model context based on the model names present in the context
        /// </summary>
        /// <param name="entityModelContext">Represents the entity model context to be filled</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        public void FillEntityModelContextByName(ref EntityModelContext entityModelContext, CallerContext callerContext)
        {
            DurationHelper durationHelper = null;

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityModelBL.FillEntityModelContextByName", MDMTraceSource.DataModel, false);
                durationHelper = new DurationHelper(DateTime.Now);
            }

            try
            {
                if (entityModelContext == null)
                {
                    throw new ArgumentNullException("entityModelContext");
                }

                if (!String.IsNullOrWhiteSpace(entityModelContext.OrganizationName))
                {
                    entityModelContext.OrganizationId = GetEntityModelIdByName(GetOrganizationIdByName, entityModelContext.OrganizationName, callerContext, entityModelContext.OrganizationId);
                }

                if (!String.IsNullOrWhiteSpace(entityModelContext.EntityTypeName))
                {
                    entityModelContext.EntityTypeId = GetEntityModelIdByName(GetEntityTypeIdByName, entityModelContext.EntityTypeName, callerContext, entityModelContext.EntityTypeId);
                }

                if (!String.IsNullOrWhiteSpace(entityModelContext.HierarchyName))
                {
                    entityModelContext.HierarchyId = GetEntityModelIdByName(GetHierarchyIdByName, entityModelContext.HierarchyName, callerContext, entityModelContext.HierarchyId);
                }

                if (!String.IsNullOrWhiteSpace(entityModelContext.ContainerName))
                {
                    PopulateContainerAndHierarchyId(ref entityModelContext, callerContext);
                }

                if (entityModelContext.RelationshipTypeNames != null)
                {
                    entityModelContext.RelationshipTypeIds = GetRelationshipTypeIdByNames(entityModelContext.RelationshipTypeNames, callerContext);
                }

                if (entityModelContext.HierarchyId > 0 && !String.IsNullOrWhiteSpace(entityModelContext.CategoryPath))
                {
                    entityModelContext.CategoryId = GetCategoryIdByPath(entityModelContext.HierarchyId, entityModelContext.CategoryPath, callerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    if (durationHelper != null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} ms: Time take to fill EntityModelContext by name",
                            durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.DataModel);
                    }

                    MDMTraceHelper.StopTraceActivity("EntityModelBL.FillEntityModelContextByName", MDMTraceSource.DataModel);
                }
            }
        }

        /// <summary>
        /// Gets the Organization Id based on organization name.
        /// </summary>
        /// <param name="organizationName">Represents the short name of the organization</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The organization id</returns>
        public Int32 GetOrganizationIdByName(String organizationName, CallerContext callerContext)
        {
            return GetModelIdUsingBusinessLogic<OrganizationBL, Organization>(businessLogic => businessLogic.GetByName(organizationName, new OrganizationContext(), callerContext),
                "Organization", organizationName, "GetOrganizationIdByName");
        }

        /// <summary>
        /// Gets the Container Id based on container name.
        /// </summary>
        /// <param name="containerName">Represents the short name of the container</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The container id</returns>
        public Int32 GetContainerIdByName(String containerName, CallerContext callerContext)
        {
            return GetModelIdUsingBusinessLogic<ContainerBL, Container>(businessLogic => businessLogic.Get(containerName, callerContext, false),
                "Container", containerName, "GetContainerIdByName");
        }

        /// <summary>
        /// Gets the hierarchy Id based on Hierarchy name.
        /// </summary>
        /// <param name="hierarchyName">Represents the short name of the hierarchy</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The hierarchy id</returns>
        public Int32 GetHierarchyIdByName(String hierarchyName, CallerContext callerContext)
        {
            return GetModelIdUsingBusinessLogic<HierarchyBL, Hierarchy>(businessLogic => businessLogic.GetByName(hierarchyName, callerContext),
                "Hierarchy", hierarchyName, "GetHierarchyIdByName");
        }

        /// <summary>
        /// Gets the entity type Id based on EntityType name.
        /// </summary>
        /// <param name="entityTypeName">Represents the short name of the entity type</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The entity type id</returns>
        public Int32 GetEntityTypeIdByName(String entityTypeName, CallerContext callerContext)
        {
            return GetModelIdUsingBusinessLogic<EntityTypeBL, EntityType>(businessLogic => businessLogic.GetByShortName(entityTypeName, callerContext),
                "EntityType", entityTypeName, "GetEntityTypeIdByName");            
        }

        /// <summary>
        /// Gets the relationship type Id based on relationship name.
        /// </summary>
        /// <param name="relationshipTypeName">Represents the short name of the entity type</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The relationship type id</returns>
        public Int32 GetRelationshipTypeIdByName(String relationshipTypeName, CallerContext callerContext)
        {
            return GetModelIdUsingBusinessLogic<RelationshipTypeBL, RelationshipType>(businessLogic => businessLogic.GetByName(relationshipTypeName, callerContext),
                "RelationshipType", relationshipTypeName, "GetRelationshipTypeIdByName");
        }

        /// <summary>
        /// Get relationship type Ids based on relationship type names.
        /// </summary>
        /// <param name="relationshipTypeNames">Represents the short names of the relationship type</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The relationship type id</returns>
        public Collection<Int32> GetRelationshipTypeIdByNames(Collection<String> relationshipTypeNames, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityModelBL.GetRelationshipTypeIdByNames", MDMTraceSource.DataModel, false);
            }

            Collection<Int32> relationshipTypeIds = new Collection<Int32>();

            try
            {
                RelationshipTypeCollection relationshipTypes = new RelationshipTypeBL().GetByNames(relationshipTypeNames, callerContext);

                if (relationshipTypes != null)
                {
                    foreach (RelationshipType relationshipType in relationshipTypes)
                    {
                        relationshipTypeIds.Add(relationshipType.Id);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityModelBL.GetRelationshipTypeIdByNames", MDMTraceSource.DataModel);
                }
            }
            return relationshipTypeIds;
        }

        /// <summary>
        /// Gets the category Id based on hierarchy id and category name.
        /// </summary>
        /// <param name="hierarchyId">Represents the hierarchy id of the category</param>
        /// <param name="categoryName">Represents the short name of the category</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The category id</returns>
        public Int64 GetCategoryIdByName(Int32 hierarchyId, String categoryName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityModelBL.GetCategoryIdByName", MDMTraceSource.DataModel, false);
            }
            
            Int64 categoryId = 0;
            try
            {
                Category category = new CategoryBL().GetByName(hierarchyId, categoryName, callerContext);
                if (category != null)
                {
                    categoryId = category.Id;
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Category with hierarchy id '{0}', name '{1}' does not exist", hierarchyId, categoryName), MDMTraceSource.DataModel);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityModelBL.GetCategoryIdByName", MDMTraceSource.DataModel);
                }
            }
            return categoryId;
        }

        /// <summary>
        /// Gets the category Id based on hierarchy id and category path.
        /// </summary>
        /// <param name="hierarchyId">Represents the hierarchy id of the category</param>
        /// <param name="categoryPath">Represents the path of the category</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The category id</returns>
        public Int64 GetCategoryIdByPath(Int32 hierarchyId, String categoryPath, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityModelBL.GetCategoryIdByPath", MDMTraceSource.DataModel, false);
            }

            Int64 categoryId = 0;

            try
            {
                Category category = new CategoryBL().GetByPath(hierarchyId, categoryPath, callerContext);

                if (category != null)
                {
                    categoryId = category.Id;
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Category with hierarchy id '{0}', path '{1}' does not exist", hierarchyId, categoryPath), MDMTraceSource.DataModel);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityModelBL.GetCategoryIdByPath", MDMTraceSource.DataModel);
                }
            }
            return categoryId;
        }

        /// <summary>
        /// Gets Category By the Category shortname path
        /// </summary>
        /// <param name="containerName">Specifies Container name in which category should be searched</param>
        /// <param name="categoryPath">Specifies Category Short Name Path</param>
        /// <param name="callerContext">Specifies the Context of the Caller</param>
        /// <returns>Category</returns>
        public Category GetCategoryByPath(String containerName, String categoryPath, CallerContext callerContext)
        {
            Category category = null;

            if (!String.IsNullOrWhiteSpace(containerName))
            {
                Container container = new ContainerBL().Get(containerName, callerContext);

                if (container != null)
                {
                    category = new CategoryBL().GetByPath(container.HierarchyId, categoryPath, callerContext);
                }
            }

            return category;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBusinessLogic"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method"></param>
        /// <param name="modelTypeName"></param>
        /// <param name="shortName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private Int32 GetModelIdUsingBusinessLogic<TBusinessLogic, TResult>(Func<TBusinessLogic, TResult> method, String modelTypeName, String shortName, String methodName)
            where TBusinessLogic : BusinessLogicBase, new() where TResult : MDMObject
        {
            Int32 modelId = 0;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(String.Format("EntityModelBL.{0}", methodName), MDMTraceSource.DataModel, false);
            }
            try
            {
                MDMObject mdmObject = method(new TBusinessLogic());
                if (mdmObject != null)
                {
                    modelId = mdmObject.Id;
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("{0} with name '{1}' does not exist", modelTypeName, shortName), MDMTraceSource.DataModel);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(String.Format("EntityModelBL.{0}", methodName), MDMTraceSource.DataModel);
                }
            }
            return modelId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="shortName"></param>
        /// <param name="callerContext"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private T GetEntityModelIdByName<T>(Func<String, CallerContext, T> method, String shortName, CallerContext callerContext, T defaultValue)
        {
            T result = defaultValue;
            if (!String.IsNullOrWhiteSpace(shortName))
            {
                result = method.Invoke(shortName, callerContext);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityModelContext"></param>
        /// <param name="callerContext"></param>
        private void PopulateContainerAndHierarchyId(ref EntityModelContext entityModelContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityModelBL.PopulateContainerAndHierarchyId", MDMTraceSource.DataModel, false);
            }

            try
            {
                Container container = new ContainerBL().Get(entityModelContext.ContainerName, callerContext);

                if (container != null)
                {
                    entityModelContext.ContainerId = container.Id;

                    if (String.IsNullOrWhiteSpace(entityModelContext.HierarchyName))
                    {
                        entityModelContext.HierarchyId = container.HierarchyId;
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityModelBL.PopulateContainerAndHierarchyId", MDMTraceSource.DataModel);
                }
            }
        }

        #endregion
    }
}
