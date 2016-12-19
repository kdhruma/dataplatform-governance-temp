using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;
    using MDM.ContainerManager.Business;
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// Class containing the methods relating to export qualification for collaboration and approved containers.
    /// </summary>
    internal sealed class ExportQualificationHelper
    {
        #region Fields
        #endregion Fields

        #region Constructors
        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the export queue items based on the profile scope and trigger context.
        /// </summary>
        /// <param name="entityCollection">The entity collection.</param>
        /// <param name="exportProfiles">The export profiles.</param>
        /// <param name="isPromoted">if set to <c>true</c> [is promoted].</param>
        /// <param name="callerContext">The caller context.</param>
        public ExportQueueCollection GetExportQueueItems(EntityCollection entityCollection, ExportProfileCollection filteredExportProfiles, CallerContext callerContext)
        {
            ExportQueueCollection queueCollection = new ExportQueueCollection();

            #region Parameter Validation

            if (entityCollection == null || entityCollection.Count < 1)
            {
                return queueCollection;
            }

            #endregion Parameter Validation

            if (filteredExportProfiles.Count < 1)
            {
                return queueCollection;
            }

            #region Qualification

            Int32 i = -1;

            ContainerCollection containers = GetAllContainers(callerContext);

            foreach (EntityExportProfile filteredProfile in filteredExportProfiles)
            {
                EntityExportSyndicationProfileData profileData = (EntityExportSyndicationProfileData)filteredProfile.DataObject;

                EntityCollection qualifiedEntities = GetScopeQualifiedItems(entityCollection, profileData, containers, callerContext);

                foreach (Entity qualifiedEntity in qualifiedEntities)
                {
                    ExportQueue exportQueueItem = new ExportQueue()
                    {
                        Id = i--,
                        EntityId = qualifiedEntity.Id,
                        EntityFamilyId = qualifiedEntity.EntityFamilyId,
                        EntityGlobalFamilyId = qualifiedEntity.EntityGlobalFamilyId,
                        HierarchyLevel = qualifiedEntity.HierarchyLevel,
                        ContainerId = qualifiedEntity.ContainerId,
                        EntityTypeId = qualifiedEntity.EntityTypeId,
                        ExportProfileId = filteredProfile.Id,
                        Action = ObjectAction.Create
                    };

                    queueCollection.Add(exportQueueItem);
                }
            }

            #endregion Qualification

            return queueCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="exportProfiles"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public ExportQueueCollection GetExportQueueItemsForCategoryExport(EntityCollection entityCollection, EntityExportProfileCollection exportProfiles, CallerContext callerContext)
        {
            ExportQueueCollection queueCollection = new ExportQueueCollection();
            ExportProfileCollection filteredExportProfiles = new ExportProfileCollection();

            #region Parameter Validation

            if (entityCollection == null || entityCollection.Count < 1)
            {
                return queueCollection;
            }

            if (exportProfiles == null || exportProfiles.Count < 1)
            {
                return queueCollection;
            }
            #endregion Parameter Validation

            #region Filter Delta Profiles Based on Approved or Collaboration

            filteredExportProfiles =  GetFilteredExportProfiles(exportProfiles, true);

            #endregion Filter Delta Profiles Based on Approved or Collaboration

            if (filteredExportProfiles.Count < 1)
            {
                return queueCollection;
            }

            #region Qualification

            ContainerCollection containers = GetAllContainers(callerContext);

            foreach (EntityExportProfile filteredProfile in filteredExportProfiles)
            {
                EntityExportSyndicationProfileData profileData = (EntityExportSyndicationProfileData)filteredProfile.DataObject;

                Collection<Int32> scopeEntityTypeIdList = null;
                Collection<Int32> scopeContainerIdList = null;

                ScopeSpecification scopeData = profileData.ScopeSpecification;

                GetProfileScopeItems(scopeData, false, containers, callerContext, out scopeContainerIdList, out scopeEntityTypeIdList);
                
                foreach (Entity entity in entityCollection)
                {
                    if (scopeContainerIdList.Contains(entity.ContainerId))
                    {
                        ExportQueue exportQueueItem = new ExportQueue()
                        {
                            EntityId = entity.Id,
                            EntityFamilyId = entity.EntityFamilyId,
                            EntityGlobalFamilyId = entity.EntityGlobalFamilyId,
                            HierarchyLevel = entity.HierarchyLevel,
                            ContainerId = entity.ContainerId,
                            ExportProfileId = filteredProfile.Id,
                            Action = ObjectAction.Create
                        };

                        queueCollection.Add(exportQueueItem);
                    }
                }
            }

            #endregion Qualification

            return queueCollection;
        }

        #region Private Methods

        /// <summary>
        /// Gets the filtered export profiles.
        /// </summary>
        /// <param name="exportProfiles">The export profiles.</param>
        /// <param name="filteredExportProfiles">The filtered export profiles.</param>
        /// <param name="isCategoryExport">Flag mentioning if it is category export.</param>
        public ExportProfileCollection GetFilteredExportProfiles(EntityExportProfileCollection exportProfiles, Boolean isCategoryExport)
        {
            ExportProfileCollection filteredExportProfiles = new ExportProfileCollection();

            foreach (EntityExportProfile profile in exportProfiles)
            {
                EntityExportSyndicationProfileData profileData = (EntityExportSyndicationProfileData)profile.DataObject;
                if (profileData == null)
                {
                    continue;
                }
                
                ProfileSetting categoryExportSetting = profileData.ProfileSettings.GetSetting(ExportProfileConstants.IS_CATEGORY_EXPORT);
                if (categoryExportSetting != null)
                {
                    Boolean categoryExport = ValueTypeHelper.BooleanTryParse(categoryExportSetting.Value, false);

                    if (categoryExport != isCategoryExport)
                    {
                        continue;
                    }
                }

                Boolean canContinue = false;
                if (!isCategoryExport)
                {
                    ProfileSetting promotedCopySetting = profileData.ProfileSettings.GetSetting(ExportProfileConstants.APPROVEDCOPY);
                    if (promotedCopySetting != null)
                    {
                        canContinue = ValueTypeHelper.BooleanTryParse(promotedCopySetting.Value, false);
                    }
                    else
                    {
                        canContinue = false;
                    }
                }
                else
                {
                    canContinue = true;
                }

                if (canContinue)
                {
                    ExecutionSpecification executionSpecification = profileData.ExecutionSpecification;
                    if (executionSpecification != null && executionSpecification.ExecutionSettings != null && executionSpecification.ExecutionSettings.Count > 0)
                    {
                        ExecutionSetting deltaSetting = executionSpecification.ExecutionSettings.GetSetting("ExecutionMode");
                        ExportExecutionMode exportMode = ExportExecutionMode.Unknown;
                        if (deltaSetting != null)
                        {
                            ValueTypeHelper.EnumTryParse<ExportExecutionMode>(deltaSetting.Value, true, out exportMode);
                        }

                        if (exportMode == ExportExecutionMode.Delta)
                        {
                            filteredExportProfiles.Add(profile);
                        }
                    }
                }
            }

            return filteredExportProfiles;
        }

        /// <summary>
        /// Determines whether if the scope qualified the specified entity collection.
        /// </summary>
        /// <param name="entityCollection">The entity collection.</param>
        /// <param name="profileData">The profile data.</param>
        /// <returns></returns>
        private EntityCollection GetScopeQualifiedItems(EntityCollection entityCollection, EntityExportSyndicationProfileData profileData, ContainerCollection containers, CallerContext callerContext)
        {
            Collection<Int32> entityTypeIdList = entityCollection.GetEntityTypeIdList();
            Collection<Int32> containerIdList = entityCollection.GetContainerIdList();
            EntityCollection filteredEntities = new EntityCollection();

            if (profileData != null && profileData.ScopeSpecification != null)
            {
                Collection<Int32> scopeEntityTypeIdList = null;
                Collection<Int32> scopeContainerIdList = null;

                ScopeSpecification scopeData = profileData.ScopeSpecification;

                GetProfileScopeItems(scopeData, true, containers, callerContext, out scopeContainerIdList, out scopeEntityTypeIdList);

                //Check if at least one container scope matches
                if (!ValueTypeHelper.CheckIfAnyMatches(scopeContainerIdList, containerIdList))
                {
                    return filteredEntities;
                }

                //Check if at least one entity type scope matches
                if (!ValueTypeHelper.CheckIfAnyMatches(scopeEntityTypeIdList, entityTypeIdList))
                {
                    return filteredEntities;
                }

                EntityCollection scopeFilteredEntities = GetFilteredEntities(entityCollection, scopeContainerIdList, scopeEntityTypeIdList);

                //Loop through the scope collection to check the rules, states and business conditions
                ExportScopeCollection containerRuleScopes = scopeData.ExportScopes;
                if (containerRuleScopes != null && containerRuleScopes.Count > 0)
                {
                    foreach (Entity entity in scopeFilteredEntities)
                    {
                        foreach (ExportScope containerRuleScope in containerRuleScopes)
                        {
                            Boolean hasCategoryQualified = false;
                            Boolean hasStatesQualified = true;
                            Boolean hasBizConditionsQualified = true;
                            Boolean hasAttributeFilterQualified = true;

                            //Category Filters
                            hasCategoryQualified = QualifyCategory(entity, containerRuleScope);

                            if (hasCategoryQualified)
                            {
                                //Validation State rules
                                if (containerRuleScope.SearchValidationStatesRuleGroup != null)
                                {
                                    foreach (var statesRule in containerRuleScope.SearchValidationStatesRuleGroup.SearchValidationStatesRules)
                                    {
                                        ValidityStateValue validityStateValue = ValidityStateValue.NotChecked;

                                        if (entity.ValidationStates != null)
                                        {
                                            validityStateValue = entity.ValidationStates.GetStateValue((Int32)statesRule.AttributeId);

                                            if (validityStateValue != statesRule.Value)
                                            {
                                                hasStatesQualified = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (!hasStatesQualified)
                                    {
                                        break;
                                    }
                                }
                                
                                //Business Conditions
                                if (containerRuleScope.MDMRuleGroup != null)
                                {
                                    BusinessConditionStatusCollection businessConditions = null;
                                    if (entity.BusinessConditions != null && entity.BusinessConditions.Count > 0)
                                    {
                                        businessConditions = entity.BusinessConditions;
                                    }

                                    foreach (var businessCondition in containerRuleScope.MDMRuleGroup.SearchMDMRuleRules)
                                    {
                                        if (businessConditions != null)
                                        {
                                            BusinessConditionStatus conditionStatus = businessConditions.GetById(businessCondition.MDMRule.Id);
                                            ValidityStateValue conditionValue = ValidityStateValue.NotChecked;
                                            ValueTypeHelper.EnumTryParse(businessCondition.Conditions[0], true, out conditionValue);

                                            if (conditionStatus != null && conditionStatus.Status != conditionValue)
                                            {
                                                hasBizConditionsQualified = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (!hasBizConditionsQualified)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (hasCategoryQualified && hasStatesQualified && hasBizConditionsQualified)
                            {
                                SearchAttributeRuleGroupCollection searchAttributeRuleGroups = containerRuleScope.SearchAttributeRuleGroups;
                                if (searchAttributeRuleGroups != null)
                                {
                                    SearchAttributeRuleGroup searchAttributeRuleGroup = searchAttributeRuleGroups.FirstOrDefault();
                                    if (searchAttributeRuleGroup != null)
                                    {
                                        SearchAttributeRuleCollection searchAttributeRules = searchAttributeRuleGroup.SearchAttributeRules;
                                        if (searchAttributeRules != null)
                                        {
                                            hasAttributeFilterQualified = QualifyAttributes(entity, searchAttributeRules);
                                        }
                                    }
                                }

                                if (hasAttributeFilterQualified)
                                {
                                    filteredEntities.Add(entity);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    filteredEntities.AddRange(scopeFilteredEntities);
                }
            }

            return filteredEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="containerRuleScope"></param>
        /// <returns></returns>
        private static Boolean QualifyCategory(Entity entity, ExportScope containerRuleScope)
        {
            Boolean hasCategoryQualified = false;

            if (containerRuleScope.CategoryGroup != null && containerRuleScope.CategoryGroup.Categories != null && containerRuleScope.CategoryGroup.Categories.Count > 0)
            {
                Collection<Int64> scopeCategoryIds = containerRuleScope.CategoryGroup.Categories.GetCategoryIdList();
                if (scopeCategoryIds != null && scopeCategoryIds.Count > 0)
                {
                    if (scopeCategoryIds.Contains(entity.CategoryId))
                    {
                        hasCategoryQualified = true;
                    }
                    else
                    {

                        Collection<Int64> entityPathIds = ValueTypeHelper.SplitStringToLongCollection(entity.IdPath, ' ');

                        if (entityPathIds != null && entityPathIds.Count > 0)
                        {
                            foreach(var categoryId in scopeCategoryIds)
                            {
                                if(entityPathIds.Contains(categoryId))
                                {
                                    hasCategoryQualified = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    hasCategoryQualified = true;
                }
            }
            else
            {
                hasCategoryQualified = true;
            }

            return hasCategoryQualified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="searchAttrRules"></param>
        /// <returns></returns>
        private static Boolean QualifyAttributes(Entity entity, SearchAttributeRuleCollection searchAttrRules)
        {
            Boolean hasAttributeFilterQualified = false;

            if (searchAttrRules != null && searchAttrRules.Count > 0)
            {
                AttributeCollection attributes = entity.Attributes;
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                foreach (SearchAttributeRule searchAttrRule in searchAttrRules)
                {
                    if (attributes != null && attributes.Count > 0)
                    {
                        Attribute attribute = (Attribute)attributes.GetAttribute(searchAttrRule.Attribute.Id, systemDataLocale);
                        ValueCollection searchAttrValues = (ValueCollection)searchAttrRule.Attribute.GetOverriddenValues();

                        if (attribute == null)
                        {
                            break;
                        }
                        else
                        {
                            if (attribute.IsComplex || attribute.IsHierarchical || searchAttrValues == null)
                            {
                                continue;
                            }
                            else
                            {
                                ValueCollection values = (ValueCollection)attribute.GetOverriddenValues();

                                object formattedAttrValues = null;
                                object formattedSearchAttrValues = null;

                                if (values != null)
                                {

                                    switch (attribute.AttributeDataType)
                                    {
                                        case AttributeDataType.Integer:
                                        case AttributeDataType.Decimal:
                                            formattedAttrValues = GetFormattedValues<Decimal>(values, attribute.AttributeDataType);
                                            formattedSearchAttrValues = GetFormattedValues<Decimal>(searchAttrValues, attribute.AttributeDataType);
                                            hasAttributeFilterQualified = isAttributeFilterQualified<Decimal>((Collection<Decimal>)formattedAttrValues, searchAttrRule.Operator, (Collection<Decimal>)formattedSearchAttrValues);
                                            break;

                                        case AttributeDataType.Date:
                                        case AttributeDataType.DateTime:
                                            formattedAttrValues = GetFormattedValues<DateTime>(values, attribute.AttributeDataType);
                                            formattedSearchAttrValues = GetFormattedValues<DateTime>(searchAttrValues, attribute.AttributeDataType);
                                            hasAttributeFilterQualified = isAttributeFilterQualified<DateTime>((Collection<DateTime>)formattedAttrValues, searchAttrRule.Operator, (Collection<DateTime>)formattedSearchAttrValues);
                                            break;

                                        case AttributeDataType.String:
                                            formattedAttrValues = GetFormattedValues<String>(values, attribute.AttributeDataType);
                                            formattedSearchAttrValues = GetFormattedValues<String>(searchAttrValues, attribute.AttributeDataType);
                                            hasAttributeFilterQualified = isAttributeFilterQualified<String>((Collection<String>)formattedAttrValues, searchAttrRule.Operator, (Collection<String>)formattedSearchAttrValues);
                                            break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        if (!hasAttributeFilterQualified)
                        {
                            break;
                        }
                    }
                }
            }

            return hasAttributeFilterQualified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="attributeDataType"></param>
        /// <returns></returns>
        private static Collection<T> GetFormattedValues<T>(ValueCollection values, AttributeDataType attributeDataType)
        {
            Collection<T> attrValues = new Collection<T>();

            foreach(Value value in values)
            {
                attrValues.Add((T)Convert.ChangeType(value.AttrVal.ToString(), typeof(T)));
            }

            return attrValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="searchOperator"></param>
        /// <param name="searchValues"></param>
        /// <returns></returns>
        private static Boolean isAttributeFilterQualified<T>(Collection<T> values, SearchOperator searchOperator, Collection<T> searchValues)
        {
            Boolean isQualified = false;

            if (searchValues != null && values != null)
            {
                List<String> searchVal = null;
                List<String> val = null;

                switch (searchOperator)
                {
                    case SearchOperator.EqualTo:

                        foreach (T value in values)
                        {
                            if (Comparer<T>.Default.Compare(value, searchValues.FirstOrDefault()) == 0)
                            {
                                isQualified = true;
                            }
                        }

                        break;

                    case SearchOperator.GreaterThan:

                        foreach (T value in values)
                        {
                            if (Comparer<T>.Default.Compare(value, searchValues.FirstOrDefault()) > 0)
                            {
                                isQualified = true;
                            }
                        }

                        break;

                    case SearchOperator.GreaterThanOrEqualTo:

                        foreach (T value in values)
                        {
                            if (Comparer<T>.Default.Compare(value, searchValues.FirstOrDefault()) >= 0)
                            {
                                isQualified = true;
                            }
                        }

                        break;

                    case SearchOperator.LessThan:

                        foreach (T value in values)
                        {
                            if (Comparer<T>.Default.Compare(value, searchValues.FirstOrDefault()) < 0)
                            {
                                isQualified = true;
                            }
                        }

                        break;

                    case SearchOperator.LessThanOrEqualTo:

                        foreach (T value in values)
                        {
                            if (Comparer<T>.Default.Compare(value, searchValues.FirstOrDefault()) >= 0)
                            {
                                isQualified = true;
                            }
                        }

                        break;

                    case SearchOperator.In:
                        
                        searchVal = searchValues.FirstOrDefault().ToString().Split(',').ToList();
                        val = values.Cast<String>().ToList();

                        if (searchVal != null && val != null)
                        {
                            if (val.Intersect(searchVal).Count() > 0)
                            {
                                isQualified = true;
                            }
                        }

                        break;

                    case SearchOperator.NotIn:

                        searchVal = searchValues.FirstOrDefault().ToString().Split(',').ToList();
                        val = values.Cast<String>().ToList();

                        if (searchVal != null && val != null)
                        {
                            if (val.Intersect(searchVal).Count() <= 0)
                            {
                                isQualified = true;
                            }
                        }

                        break;
                }
            }

            return isQualified;
        }

        /// <summary>
        /// Gets the profile scope items.
        /// </summary>
        /// <param name="scopeData">The scope data.</param>
        /// <param name="containerIdList">The container identifier list.</param>
        /// <param name="entityTypeIdList">The entity type identifier list.</param>
        private void GetProfileScopeItems(ScopeSpecification scopeData, Boolean requireApprovedContainers, ContainerCollection containers, CallerContext callerContext, out Collection<Int32> containerIdList, out Collection<Int32> entityTypeIdList)
        {
            containerIdList = new Collection<Int32>();
            entityTypeIdList = new Collection<Int32>();

            MDMObjectGroupCollection objectGroups = scopeData.MDMObjectGroups;

            if (objectGroups != null && objectGroups.Count > 0)
            { 
                foreach (MDMObjectGroup group in objectGroups)
                {
                    if (group.ObjectType == ObjectType.EntityType)
                    {
                        entityTypeIdList.AddRange(group.GetMDMObjectIds());
                    }
                    else if (group.ObjectType == ObjectType.Catalog)
                    {
                        Collection<Int32> containerIds = group.GetMDMObjectIds();
                        
                        if (containerIds != null && containerIds.Count > 0 && requireApprovedContainers)
                        {
                            Container approvedContainer = null;

                            foreach (var item in containerIds)
                            {
                                approvedContainer = containers.GetContainerByCrossReferenceId(item);

                                if (approvedContainer != null)
                                {
                                    containerIdList.Add(approvedContainer.Id);
                                }
                            }
                        }
                        else if (containerIds != null && containerIds.Count > 0)
                        {
                            containerIdList.AddRange(containerIds);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the filtered entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="containerIds">The container ids.</param>
        /// <param name="entityTypeIds">The entity type ids.</param>
        /// <returns></returns>
        private EntityCollection GetFilteredEntities(EntityCollection entities, Collection<Int32> containerIds, Collection<Int32> entityTypeIds)
        {
            EntityCollection filteredEntities = new EntityCollection();

            foreach (Entity entity in entities)
            {
                if (entityTypeIds.Contains(entity.EntityTypeId) && containerIds.Contains(entity.ContainerId))
                {
                    filteredEntities.Add(entity);
                }
            }

            return filteredEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private static ContainerCollection GetAllContainers(CallerContext callerContext)
        {
            ContainerContext containerContext = new ContainerContext() { ApplySecurity = false, IncludeApproved = true, LoadAttributes = false };
            return new ContainerBL().GetAll(containerContext, callerContext);
        }

        #endregion Private Methods

        #endregion Methods
    }
}