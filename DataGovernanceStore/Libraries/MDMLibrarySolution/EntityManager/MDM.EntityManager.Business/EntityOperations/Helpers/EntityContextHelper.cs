using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using AttributeModelManager.Business;
    using BusinessObjects;
    using Core;
    using EntityModelManager.Business;
    using Interfaces;
    using Utility;

    /// <summary>
    /// Class provides utility methods for manipulating entity context object
    /// </summary>
    internal class EntityContextHelper
    {
        /// <summary>
        /// Populates the Id's in the entity context based on the names
        /// </summary>
        /// <param name="entityContext">The entity context for which the name to id resolving has to be performed</param>
        /// <param name="entityManager">Specifies an instance of the entity manager</param>
        /// <param name="callerContext">Specifies the caller context that describes the module and application that invoked the method</param>
        public static void PopulateIdsInEntityContext(EntityContext entityContext, IEntityManager entityManager, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.PopulateIdsInEntityContext", MDMTraceSource.EntityGet, false);
            }

            if (entityContext != null)
            {
                #region Fill Base PropertyIds By Names

                var entityModelContext = new EntityModelContext();
                var entityModelManager = new EntityModelBL();
                Boolean needToLoadIds = false;

                if (entityContext.CategoryId < 1 && !String.IsNullOrWhiteSpace(entityContext.CategoryPath))
                {
                    entityModelContext.CategoryPath = entityContext.CategoryPath;
                    needToLoadIds = true;
                }

                if (entityContext.ContainerId < 1 && !String.IsNullOrWhiteSpace(entityContext.ContainerName))
                {
                    entityModelContext.ContainerName = entityContext.ContainerName;
                    needToLoadIds = true;
                }

                if (entityContext.EntityTypeId < 1 && !String.IsNullOrWhiteSpace(entityContext.EntityTypeName))
                {
                    entityModelContext.EntityTypeName = entityContext.EntityTypeName;
                    needToLoadIds = true;
                }

                if (entityContext.RelationshipContext != null && entityContext.RelationshipContext.RelatedEntitiesAttributeIdList == null && entityContext.RelationshipContext.RelatedEntitiesAttributeNames != null)
                {
                    entityModelContext.RelationshipTypeNames = entityContext.RelationshipContext.RelationshipTypeNames;
                    needToLoadIds = true;
                }

                if (needToLoadIds)
                {
                    entityModelManager.FillEntityModelContextByName(ref entityModelContext, callerContext);

                    PopulateIdsFromEntityModelContext(entityModelContext, ref entityContext);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Basic Property Id population in entity context by name completed.", MDMTraceSource.EntityGet);
                    }
                }

                #endregion

                #region Fill AttributeIds By Names

                if (entityContext.AttributeIdList.Count < 1 && entityContext.AttributeNames != null && entityContext.AttributeNames.Count > 0)
                {
                    entityContext.AttributeIdList = GetAttributeIdList(entityContext.AttributeNames);

                    if (entityContext.AttributeIdList == null || entityContext.AttributeIdList.Count == 0)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve AttributeIdList by Names : {0}", ValueTypeHelper.JoinCollection(entityContext.AttributeNames, "||")));
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AttributeIdList population in entity context by name and parent name completed.", MDMTraceSource.EntityGet);
                    }
                }

                if (entityContext.AttributeGroupNames != null && entityContext.AttributeGroupNames.Count > 0)
                {
                    Collection<Int32> attributeIds = GetAttributeIdsByGroupNames(entityContext.AttributeGroupNames, entityContext.DataLocales);

                    if (attributeIds != null && attributeIds.Count > 0)
                    {
                        if (entityContext.AttributeIdList == null || entityContext.AttributeIdList.Count < 1)
                        {
                            entityContext.AttributeIdList = attributeIds;
                        }
                        else
                        {
                            foreach (Int32 attributeId in attributeIds)
                            {
                                if (entityContext.AttributeIdList.Contains(attributeId) == false)
                                {
                                    entityContext.AttributeIdList.Add(attributeId);
                                }
                            }
                        }
                    }

                    if (entityContext.AttributeIdList == null || entityContext.AttributeIdList.Count == 0)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve AttributeIdList by Names : {0}", ValueTypeHelper.JoinCollection(entityContext.AttributeGroupNames, "||")));
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AttributeGroupId list population in entity context by name completed", MDMTraceSource.EntityGet);
                    }
                }

                if (entityContext.RelationshipContext != null && (entityContext.RelationshipContext.RelatedEntitiesAttributeIdList == null || entityContext.RelationshipContext.RelatedEntitiesAttributeIdList.Count < 1)
                    && entityContext.RelationshipContext.RelatedEntitiesAttributeNames != null && entityContext.RelationshipContext.RelatedEntitiesAttributeNames.Count > 0)
                {
                    entityContext.RelationshipContext.RelatedEntitiesAttributeIdList = GetAttributeIdList(entityContext.RelationshipContext.RelatedEntitiesAttributeNames);

                    if (entityContext.RelationshipContext.RelatedEntitiesAttributeIdList == null || entityContext.RelationshipContext.RelatedEntitiesAttributeIdList.Count == 0)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve RelatedEntitiesAttributeIdList by Names : {0}", ValueTypeHelper.JoinCollection(entityContext.RelationshipContext.RelatedEntitiesAttributeNames, "||")));
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "RelatedEntitiesAttributeIdList  population in entity context by name completed.", MDMTraceSource.EntityGet);
                    }
                }

                #endregion
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.Entity.PopulateIdsInEntityContext", MDMTraceSource.EntityGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityModelContext"></param>
        /// <param name="entityContext"></param>
        private static void PopulateIdsFromEntityModelContext(EntityModelContext entityModelContext, ref EntityContext entityContext)
        {
            if (entityModelContext.CategoryId > 0)
            {
                entityContext.CategoryId = entityModelContext.CategoryId;
            }

            if (entityModelContext.ContainerId > 0)
            {
                entityContext.ContainerId = entityModelContext.ContainerId;
            }

            if (entityModelContext.EntityTypeId > 0)
            {
                entityContext.EntityTypeId = entityModelContext.EntityTypeId;
            }

            if (entityModelContext.RelationshipTypeIds != null && entityContext.RelationshipContext != null)
            {
                entityContext.RelationshipContext.RelationshipTypeIdList = entityModelContext.RelationshipTypeIds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeNames"></param>
        /// <returns></returns>
        private static Collection<Int32> GetAttributeIdList(Collection<String> attributeNames)
        {
            var attributeIdList = new Collection<Int32>();

            if (attributeNames != null && attributeNames.Count > 0)
            {
                attributeIdList = new AttributeModelBL().GetAttributeIdList(attributeNames);
            }

            return attributeIdList;
        }

        private static Collection<Int32> GetAttributeIdsByGroupNames(Collection<String> attributeGroupNames, Collection<LocaleEnum> dataLocales)
        {
            var attributeIdList = new Collection<Int32>();

            var attributeModelCollection = new AttributeModelBL().GetAllBaseAttributeModels();
            IAttributeModelCollection filteredModelCollection = attributeModelCollection.GetAttributeModelsByParentNames(attributeGroupNames, dataLocales);

            if (filteredModelCollection != null && filteredModelCollection.Count > 0)
            {
                attributeIdList = filteredModelCollection.GetAttributeIdList();
            }

            return attributeIdList;
        }
    }
}
