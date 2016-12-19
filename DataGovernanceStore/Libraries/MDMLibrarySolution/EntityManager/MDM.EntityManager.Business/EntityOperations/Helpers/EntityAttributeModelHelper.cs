using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using Core;
    using BusinessObjects;
    using AttributeModelManager.Business;
    using Utility;
    using AttributeManager.Business;
    using BusinessObjects.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityAttributeModelHelper
    {
        #region Methods

        /// <summary>
        /// Gets the attribute models.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <param name="locales"></param>
        /// <param name="applySecurity"></param>
        /// <param name="applySorting"></param>
        /// <param name="entity"></param>
        /// <param name="attributeModelManager"></param>
        /// <param name="traceSettings"></param>
        /// <returns></returns>
        public static AttributeModelCollection GetAttributeModels(Int64 entityId, EntityContext entityContext, Collection<LocaleEnum> locales, Boolean applySecurity = true, Boolean applySorting = false, Entity entity = null, AttributeModelBL attributeModelManager = null, TraceSettings traceSettings = null, Boolean loadAttributePermission = false)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            var attributeModels = new AttributeModelCollection();

            if (attributeModelManager == null)
            {
                attributeModelManager = new AttributeModelBL();
            }

            var attributeIds = new Collection<Int32>(entityContext.AttributeIdList.ToList());

            var ignoreDuplicateCheckInCollection = true;
            var metadataAttrMaxId = 100;

            #region Prepare AttributModel Context

            var attributeModelContext = new AttributeModelContext();

            attributeModelContext.ContainerId = entityContext.ContainerId;
            attributeModelContext.AttributeModelType = entityContext.AttributeModelType;

            if (attributeModelContext.AttributeModelType == AttributeModelType.Common
                || attributeModelContext.AttributeModelType == AttributeModelType.All)
            {
                attributeModelContext.EntityTypeId = entityContext.EntityTypeId;
                attributeModelContext.CategoryId = entityContext.CategoryId;
            }

            if (attributeModelContext.AttributeModelType == AttributeModelType.Category
                || attributeModelContext.AttributeModelType == AttributeModelType.All)
            {
                attributeModelContext.CategoryId = entityContext.CategoryId;
            }

            RelationshipContext relationshipContext = entityContext.RelationshipContext;

            if ((attributeModelContext.AttributeModelType == AttributeModelType.Relationship
                 || attributeModelContext.AttributeModelType == AttributeModelType.All)
                && relationshipContext.RelationshipTypeIdList != null
                && relationshipContext.RelationshipTypeIdList.Count > 0)
            {
                attributeModelContext.RelationshipTypeId = relationshipContext.RelationshipTypeIdList.FirstOrDefault(); //TODO:: what to do when multiple RelationshipTypes are requested?
            }

            //This is because when we are editing category, and if it is first level category, it will get CategoryId = 0.
            //In this case AttributeModel get SP is not returning attribute models.
            //In this case we need to pass category Id as EntityId ( in a away categoryId only).
            //But again this should happen only when it is a top level category. For child categories, category id will be there.
            if (attributeModelContext.CategoryId == 0 && attributeModelContext.EntityTypeId == 6)
            {
                attributeModelContext.CategoryId = entityId;
            }

            attributeModelContext.EntityId = entityId;
            attributeModelContext.GetCompleteDetailsOfAttribute = true;
            attributeModelContext.Locales = locales;
            attributeModelContext.GetOnlyShowAtCreationAttributes = entityContext.LoadCreationAttributes;
            attributeModelContext.GetOnlyRequiredAttributes = entityContext.LoadRequiredAttributes;
            attributeModelContext.ApplySorting = applySorting;
            attributeModelContext.ApplySecurity = applySecurity;
            attributeModelContext.LoadPermissions = loadAttributePermission;
            attributeModelContext.LoadStateValidationAttributes = entityContext.LoadStateValidationAttributes;

            #endregion

            if ((entityContext.AttributeIdList != null && entityContext.AttributeIdList.Count > 0)
                    || (entityContext.AttributeGroupIdList != null && entityContext.AttributeGroupIdList.Count > 0))
            {
                if (entity != null && entity.OriginalEntity != null && entity.OriginalEntity.AttributeModels != null && entity.OriginalEntity.AttributeModels.Count > 0)
                {
                    attributeModels = new AttributeModelCollection();
                    AttributeModelCollection originalEntityAttributeModels = entity.OriginalEntity.AttributeModels;

                    if (entityContext.AttributeIdList != null)
                    {
                        foreach (Int32 attributeId in entityContext.AttributeIdList)
                        {
                            Boolean isAttributeFoundInAllLocales = true;

                            foreach (LocaleEnum locale in locales)
                            {
                                var attributeModel = (AttributeModel)originalEntityAttributeModels.GetAttributeModel(attributeId, locale);

                                if (attributeModel != null && attributeModel.Id > 0)
                                {
                                    if (attributeModel.InheritableOnly)
                                    {
                                        if(entityContext.LoadInheritableOnlyAttributes)
                                        {
                                            attributeModels.Add(attributeModel);
                                        }
                                    }
                                    else
                                    {
                                        attributeModels.Add(attributeModel);
                                    }
                                }
                                else
                                {
                                    isAttributeFoundInAllLocales = false;
                                }
                            }

                            if (isAttributeFoundInAllLocales)
                                attributeIds.Remove(attributeId);
                        }
                    }
                }

                if ((attributeIds.Count > 0) || (entityContext.AttributeGroupIdList != null && entityContext.AttributeGroupIdList.Count > 0))
                {
                    var returnedAttributeModels = attributeModelManager.Get(attributeIds, entityContext.AttributeGroupIdList, 0, 0, null, attributeModelContext);

                    if (returnedAttributeModels != null && returnedAttributeModels.Count > 0)
                    {
                        foreach (AttributeModel attrModel in returnedAttributeModels)
                        {
                            if (attrModel.Id <= metadataAttrMaxId)
                                continue;

                            if (attrModel.InheritableOnly)
                            {
                                if (entityContext.LoadInheritableOnlyAttributes)
                                {
                                    attributeModels.Add(attrModel, ignoreDuplicateCheckInCollection);
                                }
                            }
                            else
                            {
                                attributeModels.Add(attrModel, ignoreDuplicateCheckInCollection);
                            }
                        }
                    }
                }
            }
            else
            {
                var returnedAttributeModels = attributeModelManager.Get(attributeModelContext);

                foreach (AttributeModel attrModel in returnedAttributeModels)
                {
                    if (attrModel.Id <= metadataAttrMaxId)
                        continue;

                    if (attrModel.InheritableOnly)
                    {
                        if (entityContext.LoadInheritableOnlyAttributes)
                        {
                            attributeModels.Add(attrModel, ignoreDuplicateCheckInCollection);
                        }
                    }
                    else
                    {
                        attributeModels.Add(attrModel, ignoreDuplicateCheckInCollection);
                    }
                }
            }

            if (entityContext.LoadDependentAttributes)
            {
                Boolean isAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.AttributeDependency.Enabled", false);

                if (isAttributeDependencyEnabled)
                {
                    //If LoadDependentAttributes flag set as 'true' and 'MCenter.AttributeDependency.Enabled' flag is enabled then load the dependency attribute models as part of entity.
                    LoadDependencyAttributeModel(attributeModels, attributeModelContext, entityContext.CategoryId);
                }
            }

            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return attributeModels;
        }

        /// <summary>
        /// Loads the dependency attribute model.
        /// </summary>
        /// <param name="attributeModels">The attribute models.</param>
        /// <param name="attributeModelContext">The attribute model context.</param>
        /// <param name="categoryId">The category identifier.</param>
        private static void LoadDependencyAttributeModel(AttributeModelCollection attributeModels, AttributeModelContext attributeModelContext, Int64 categoryId)
        {
            if (attributeModels != null && attributeModels.Count > 0)
            {
                var attributeModelBL = new AttributeModelBL();

                //check the list of attribute which contains dependency and check whether those or part of this attribute model.
                // if not load dependent attribute then load missing dependent attribute models.
                AttributeModelCollection dependencyAttributeModels = attributeModels.GetDependentAttributeAttributeModels();

                //If dependency attribute models are present get the list of dependency attribute id list.
                if (dependencyAttributeModels != null && dependencyAttributeModels.Any())
                {
                    var dependencyAttributeIdlist = new List<Int32>();
                    Collection<Int32> loadedAttributeModelIdList = attributeModels.GetAttributeIdList();

                    Boolean isCollectionAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.AttributeDependency.CollectionAttribute.Enabled", false);

                    foreach (AttributeModel attrmodel in dependencyAttributeModels)
                    {
                        if (attrmodel.IsCollection && !isCollectionAttributeDependencyEnabled)
                        {
                            //If the dependent attribute is collection and flag is disable so no need to load the attribute model.
                            continue;
                        }

                        dependencyAttributeIdlist.AddRange(attrmodel.GetDepencyAttributeIdList());
                    }

                    if (dependencyAttributeIdlist.Count > 0)
                    {
                        //It might be duplicate ids present in the list. So filter the id list.
                        dependencyAttributeIdlist = dependencyAttributeIdlist.Distinct().ToList();

                        // Filter the missing attribute model id list. This will return the dependency attribute id list which are required to be loaded using attribute model get.
                        List<Int32> missingIds = (from id in dependencyAttributeIdlist where !loadedAttributeModelIdList.Contains(id) select id).ToList();

                        if (missingIds.Count > 0)
                        {
                            var tempAttributeModelContext = new AttributeModelContext(attributeModelContext.ToXml()) { AttributeModelType = AttributeModelType.All, CategoryId = categoryId };

                            AttributeModelCollection dependentAttributeModels = attributeModelBL.Get(new Collection<Int32>(missingIds), null, 0, 0, null, tempAttributeModelContext);

                            if (dependentAttributeModels != null && dependencyAttributeModels.Count > 0)
                            {
                                attributeModels.AddRange(dependentAttributeModels);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the attributes and attribute models
        /// </summary>
        /// <param name="attributeIds">Ids of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Ids of attribute groups for which models are required</param>
        /// <param name="excludeAttributeIds">Ids to be excluded from the requested models</param>
        /// <param name="attributeModelContext">The attribute model context for which models needs to be fetched</param>
        /// <param name="attributeCollection">Resulting attributes collection</param>
        /// <param name="attributeModelCollection">Resulting attributes models collection</param>
        public static void LoadAttributeModelsWithBlankAttributeInstances(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext, out AttributeCollection attributeCollection, out AttributeModelCollection attributeModelCollection)
        {
            var attributeManager = new AttributeBL();
            attributeManager.GetAttributesAndModels(attributeIds, attributeGroupIds, excludeAttributeIds, attributeModelContext, out attributeCollection, out attributeModelCollection);
        }

        /// <summary>
        /// Gets the state validation attribute models
        /// </summary>
        /// <param name="traceSettings">Indicates trace settings</param>
        /// <returns>Resulting attribute model collection</returns>
        public static AttributeModelCollection GetStateValidationAttributeModels(TraceSettings traceSettings = null)
        {
            AttributeModelCollection attributeModels = null;
            var diagnosticActivity = new DiagnosticActivity();

            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            var attributeModelManager = new AttributeModelBL();

            #region Prepare AttributModel Context

            var attributeModelContext = new AttributeModelContext();

            attributeModelContext.GetCompleteDetailsOfAttribute = true;
            attributeModelContext.ApplySorting = false;
            attributeModelContext.ApplySecurity = false;
            attributeModelContext.LoadPermissions = false;
            attributeModelContext.LoadStateValidationAttributes = true;
            attributeModelContext.AttributeModelType = AttributeModelType.System;

            Collection<Int32> stateValidationAttributeIdList = EntityOperationsHelper.GetStateValidationAttributes();

            #endregion Prepare AttributModel Context

            attributeModels = attributeModelManager.Get(stateValidationAttributeIdList, null, 0, 0, null, attributeModelContext);

            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return attributeModels;
        }

        #endregion
    }
}
