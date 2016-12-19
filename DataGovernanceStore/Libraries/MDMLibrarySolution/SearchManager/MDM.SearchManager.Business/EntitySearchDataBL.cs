using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;

namespace MDM.SearchManager.Business
{
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.SearchManager.Data;
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.Core.Extensions;
    using MDM.Interfaces;
    using MDM.DataModelManager.Business;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Entity search data manager
    /// </summary>
    public class EntitySearchDataBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private static RegexOptions _options = RegexOptions.IgnoreCase | RegexOptions.RightToLeft | RegexOptions.Compiled;

        /// <summary>
        /// 
        /// </summary>
        private static String _regFormat = String.Format("[{0}{1}{2} .,+-\\/_#\'\"()!@]", @"\n", @"\r", @"\t");

        /// <summary>
        /// 
        /// </summary>
        private IEntityManager _entityManager = null;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        #endregion

        #region Constructors

        public EntitySearchDataBL(IEntityManager entityManager)
        {
            this._entityManager = entityManager;
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Refresh EntitySearchData for given entities.
        /// </summary>
        /// <param name="entityFamilyChangeContext">Indicates family change context</param>
        /// <param name="callerContext">Indicates application and method which called this method</param>
        /// <returns>Returns entity operation result collection</returns>
        public EntityOperationResultCollection RefreshEntitiesSearchData(EntityCollection entities, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext)
        {
            if (entities == null)
            {
                throw new MDMOperationException("111840", "Entity Collection is null or empty.", "EntitySearchManager", String.Empty, "Get");
            }

            #region Step : Initial Setup

            EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection(entities);

            Collection<ObjectAction> actions = new Collection<ObjectAction>() { ObjectAction.Reclassify, ObjectAction.Rename, ObjectAction.Unknown };
            Collection<Int64> filteredEntityIdList = entityFamilyChangeContext.GetEntityIdList(actions);
            Boolean isFullRefreshRequired = (filteredEntityIdList != null && filteredEntityIdList.Count > 0) ? true : false;

            #endregion Step : Initial Setup

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            #endregion

            try
            {
                #region Step : Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Step : Diagnostics & Tracing

                #region Step : Prepare Entity Id List

                Collection<Int64> entityIdList = new Collection<Int64>();

                foreach (Entity entity in entities)
                {
                    entityIdList.Add(entity.Id);

                    //Update entity context for current entity.
                    entity.EntityContext = new EntityContext(entity);
                }

                //Add Deleted entities also to remove records from DN search table
                Collection<Int64> deletedEntityIdList = new Collection<Int64>();

                if (entityFamilyChangeContext != null)
                {
                    deletedEntityIdList = entityFamilyChangeContext.GetEntityIdList(ObjectAction.Delete);
                    entityIdList.AddRange<Int64>(deletedEntityIdList);
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Preparing of entity id list completed.");
                }

                #endregion Step : Prepare Entity Id List

                if (entityIdList != null && entityIdList.Count > 0)
                {
                    #region Step : Apply Data Model Exclusion

                    FilterEntityByDataModelExclusionContexts(entities, callerContext, diagnosticActivity);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Data model exclusion process completed.");
                    }

                    #endregion Step : Apply Data Model Exclusion

                    #region Step : Load Original Entity Search Data

                    EntitySearchDataCollection entitySearchDataCollection = Get(entityIdList, callerContext, diagnosticActivity);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Loading original entity search data process completed.");
                    }

                    #endregion Step : Load Original Entity Search Data

                    #region Step : Update entity search data object for changes

                    foreach (Int64 entityId in entityIdList)
                    {
                        EntitySearchData originalEntitySearchData = (EntitySearchData)entitySearchDataCollection.GetEntitySearchDataByEntityId(entityId);
                        Entity entity = (Entity)entities.GetEntity(entityId);

                        if (entity == null || deletedEntityIdList.Contains(entityId))
                        {
                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogInformation("Entity.Action = Delete. So marking PK_DNSearch : " + originalEntitySearchData.Id + " with action = Delete");
                            }

                            originalEntitySearchData.Action = ObjectAction.Delete;
                        }
                        else
                        {
                            EntityContext entityRefreshContext = new EntityContext()
                            {
                                AttributeModelType = AttributeModelType.All,
                                AttributeIdList = entity.Attributes.GetAttributeIdList(),
                                DataLocales = entity.Attributes.GetLocaleList()
                            };

                            Collection<Int32> attributeIdsToBeDeleted = null;

                            if (entityFamilyChangeContext != null)
                            {
                                attributeIdsToBeDeleted = entityFamilyChangeContext.VariantsChangeContext.GetAttributeIdList(ObjectAction.Delete);
                            }

                            if (attributeIdsToBeDeleted != null && attributeIdsToBeDeleted.Count > 0)
                            {
                                entityRefreshContext.AttributeIdList.AddRange(attributeIdsToBeDeleted);
                            }

                            if (originalEntitySearchData == null)
                            {
                                EntitySearchData entitySearchData = PopulateSearchValAndKeyVal(entity, entityRefreshContext);
                                entitySearchData.Action = ObjectAction.Create;

                                entitySearchDataCollection.Add(entitySearchData);
                            }
                            else if(isFullRefreshRequired)
                            {
                                var removeSuccess = entitySearchDataCollection.Remove(entity.Id);
                                Int64 currEntitySearchDataId = originalEntitySearchData.Id;

                                originalEntitySearchData = PopulateSearchValAndKeyVal(entity, entityRefreshContext);

                                if (removeSuccess && currEntitySearchDataId > 0)
                                {
                                    originalEntitySearchData.Id = currEntitySearchDataId;
                                    originalEntitySearchData.Action = ObjectAction.Update;
                                }
                                else
                                {
                                    originalEntitySearchData.Action = ObjectAction.Create;
                                }

                                entitySearchDataCollection.Add(originalEntitySearchData);
                            }
                            else
                            {
                                //SearchVal can't be empty or null. Why to take chance? Do a full entity get and update the searchVal
                                if (String.IsNullOrWhiteSpace(originalEntitySearchData.SearchValue))
                                {
                                    EntityContext clonedEntityContext = (EntityContext)entityRefreshContext.Clone();

                                    entitySearchDataCollection.Remove(entity.Id);

                                    clonedEntityContext.AttributeIdList.Clear();
                                    clonedEntityContext.LoadAttributes = true;

                                    Entity freshEntity = this._entityManager.Get(entity.Id, clonedEntityContext, false, callerContext.Application, callerContext.Module, false, false);

                                    originalEntitySearchData = PopulateSearchValAndKeyVal(freshEntity, clonedEntityContext);

                                    entitySearchDataCollection.Add(originalEntitySearchData);
                                }
                                else
                                {
                                    RefreshSearchValAndKeyVal(originalEntitySearchData, entity, entityRefreshContext, diagnosticActivity);
                                }
                                if (isTracingEnabled)
                                {
                                    diagnosticActivity.LogInformation("Marking PK_DNSearch : " + originalEntitySearchData.Id + " with action = Update");
                                }

                                originalEntitySearchData.Action = ObjectAction.Update;
                            }

                        }
                    }

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("updating entity search data object changes completed.");
                    }

                    #endregion Step :  Update entity search data object for changes

                    #region Step: Updates entity search data

                    Process(entitySearchDataCollection, entityOperationResults, callerContext, diagnosticActivity, ProcessingMode.Async);

                    #endregion Step: Updates entity search data
                }
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity Search Data processing is completed.");
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return entityOperationResults;
        }

        #endregion

        #region Private Methods

        #region Get Methods

        /// <summary>
        /// Get EntitySearchData for given entities.
        /// </summary>
        /// <param name="entityIdList">Provides Collection of entity ids for which searchData needs to be found.</param>
        /// <param name="callerContext">Indicates application and method which called this method</param>
        /// <param name="diagnosticActivity">Indicates diagnosticActivity</param>
        /// <returns>EntitySearchData Collection</returns>
        private EntitySearchDataCollection Get(Collection<Int64> entityIdList, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            EntitySearchDataCollection entitySearchDataCollection = new EntitySearchDataCollection();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                if (entityIdList == null || entityIdList.Count < 1)
                {
                    throw new MDMOperationException("111840", "Entity Collection is null or empty.", "EntitySearchManager", String.Empty, "Get");
                }

                foreach (Int64 entityId in entityIdList)
                {
                    if (entityId < 1)
                    {
                        throw new MDMOperationException("111795", "EntityId must be greater than 0.", "EntitySearchManager", String.Empty, "Get");
                    }
                }

                #endregion

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Search);

                //Get Data from DataBase
                EntitySearchDataDA entitySearchDataDA = new EntitySearchDataDA();
                entitySearchDataCollection = entitySearchDataDA.Get(entityIdList, command);

                if (entitySearchDataCollection == null || entitySearchDataCollection.Count < 1)
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("No SearchData found for given entities.");
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entitySearchDataCollection;
        }

        #endregion Get Methods

        #region Process Methods

        /// <summary>
        /// Process Entity Search Data Collection.
        /// </summary>
        /// <param name="entitySearchDataCollection">Provides SearchData to be processed.</param>
        /// <param name="entityOperationResults">Indicates entityOperationResult.</param>
        /// <param name="callerContext">Indicates application and method which called this method.</param>
        /// <param name="diagnosticActivity">Indicates diagnosticActivity</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns>True if serachData have been processed successfully.</returns>
        private Boolean Process(EntitySearchDataCollection entitySearchDataCollection, EntityOperationResultCollection entityOperationResults, CallerContext callerContext, DiagnosticActivity diagnosticActivity, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            Boolean result = false;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                if (entitySearchDataCollection == null || entitySearchDataCollection.Count < 1)
                {
                    throw new MDMOperationException("111839", "EntitySearchData Collection is null or empty.", "EntitySearchManager", String.Empty, "Process");
                }

                foreach (EntitySearchData entitySearchData in entitySearchDataCollection)
                {
                    if (entitySearchData.EntityId <= 0)
                    {
                        throw new MDMOperationException("111795", "EntityId must be greater than 0.", "EntitySearchManager", String.Empty, "Process");
                    }

                    if (entitySearchData.ContainerId <= 0)
                    {
                        throw new MDMOperationException("111821", "ContainerId must be greater than 0.", "EntitySearchManager", String.Empty, "Process");
                    }

                    if (String.IsNullOrWhiteSpace(entitySearchData.SearchValue))
                    {
                        throw new MDMOperationException("111837", "SearchValue cannot be null.", "EntitySearchManager", String.Empty, "Process");
                    }

                    if (String.IsNullOrWhiteSpace(entitySearchData.KeyValue))
                    {
                        throw new MDMOperationException("111838", "KeyValue cannot be null.", "EntitySearchManager", String.Empty, "Process");
                    }
                }

                #endregion

                #region Prepare Entity OperationResult object for missing ones

                foreach (EntitySearchData entitySearchData in entitySearchDataCollection)
                {
                    EntityOperationResult entityOperationResult = entityOperationResults.GetEntityOperationResult(entitySearchData.EntityId);

                    if (entityOperationResult == null)
                    {
                        entityOperationResult = new EntityOperationResult();
                        entityOperationResult.EntityId = entitySearchData.EntityId;
                        entityOperationResults.Add(entityOperationResult);
                    }
                }

                #endregion

                #region Updates DN search to database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    //Get command
                    DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Search);

                    //Get Data from DataBase
                    EntitySearchDataDA entitySearchDataDA = new EntitySearchDataDA();
                    result = entitySearchDataDA.Process(entitySearchDataCollection, entityOperationResults, command, processingMode);

                    transactionScope.Complete();
                }

                #endregion Updates DN search to database
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return result;
        }

        #endregion

        #region Data Model Exclusion Methods

        /// <summary>
        /// Filters entity by data model exclusion context
        /// </summary>
        /// <param name="entities">Indicates collection of entities</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <param name="diagnosticActivity">Indicates diagnosticActivity</param>
        private void FilterEntityByDataModelExclusionContexts(EntityCollection entities, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                foreach (Entity entity in entities)
                {
                    EntityContext entityContext = entity.EntityContext;
                    DataModelExclusionContextBL dataModelExtendedPropertiesManager = new DataModelExclusionContextBL();

                    DataModelExclusionContextCollection dataModelExclusionContexts = dataModelExtendedPropertiesManager.Get(entity.OrganizationId, entity.ContainerId, entity.EntityTypeId, entityContext.DataLocales, callerContext);

                    if (dataModelExclusionContexts != null && dataModelExclusionContexts.Count > 0)
                    {
                        #region Get AttributeModels

                        GetAttributeModels(entity, entityContext);

                        #endregion

                        #region Filter by exclusion context

                        foreach (DataModelExclusionContext dataModelExclusionContext in dataModelExclusionContexts)
                        {
                            if (dataModelExclusionContext.Locale != LocaleEnum.UnKnown)
                            {
                                FilterEntityByExclusionContextAttributeId(entity, dataModelExclusionContext.Locale, dataModelExclusionContext.AttributeId, dataModelExclusionContext.ServiceType);
                            }
                            else
                            {
                                foreach (LocaleEnum locale in entityContext.DataLocales)
                                {
                                    FilterEntityByExclusionContextAttributeId(entity, locale, dataModelExclusionContext.AttributeId, dataModelExclusionContext.ServiceType);
                                }
                            }
                        }

                        #endregion
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freshEntity"></param>
        /// <param name="locale"></param>
        /// <param name="attributeId"></param>
        /// <param name="serviceType"></param>
        private void FilterEntityByExclusionContextAttributeId(Entity freshEntity, LocaleEnum locale, Int32 attributeId, MDMServiceType serviceType)
        {
            //NOTE : AttributeId = 0 means ALL common and technical
            //       AttributeId = -1 means Common Attributes
            //       AttributeId = -2 means Technical Attributes   
            //       AttributeId > 0 means Particular Attribute   

            if (freshEntity.AttributeModels != null && freshEntity.AttributeModels.Count > 0)
            {
                switch (attributeId)
                {
                    case 0:
                        {
                            //NOTE : ALL means common and technical attributes should get excluded
                            AttributeModelCollection attributesToRemove = new AttributeModelCollection();
                            AttributeModelCollection commonAttributes = freshEntity.AttributeModels.GetCommonAttributeModels(locale) as AttributeModelCollection;
                            AttributeModelCollection categorySpecificAttributes = freshEntity.AttributeModels.GetTechnicalAttributeModels(locale) as AttributeModelCollection;

                            if (commonAttributes != null && commonAttributes.Count > 0)
                            {
                                attributesToRemove.AddRange(commonAttributes);
                            }

                            if (categorySpecificAttributes != null && categorySpecificAttributes.Count > 0)
                            {
                                attributesToRemove.AddRange(categorySpecificAttributes);
                            }

                            UpdateAttributeModesOfEntityByServiceType(attributesToRemove, locale, serviceType);
                            break;
                        }
                    case -1:
                        {
                            AttributeModelCollection commonAttributeModels = freshEntity.AttributeModels.GetCommonAttributeModels(locale) as AttributeModelCollection;

                            if (commonAttributeModels != null && commonAttributeModels.Count > 0)
                            {
                                UpdateAttributeModesOfEntityByServiceType(commonAttributeModels, locale, serviceType);
                            }
                            break;
                        }
                    case -2:
                        {
                            AttributeModelCollection categorySpecificAttributeModels = freshEntity.AttributeModels.GetTechnicalAttributeModels(locale) as AttributeModelCollection;

                            if (categorySpecificAttributeModels != null && categorySpecificAttributeModels.Count > 0)
                            {
                                UpdateAttributeModesOfEntityByServiceType(categorySpecificAttributeModels, locale, serviceType);
                            }
                            break;
                        }
                    default:
                        {
                            AttributeModel attributeModel = (AttributeModel)freshEntity.AttributeModels.GetAttributeModel(attributeId, locale);

                            if (attributeModel != null)
                            {
                                if (serviceType == MDMServiceType.Flattened)
                                {
                                    attributeModel.EnableHistory = false;
                                }
                                else if (serviceType == MDMServiceType.Searchable)
                                {
                                    attributeModel.Searchable = false;
                                }
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <param name="locale"></param>
        /// <param name="serviceType"></param>
        private void UpdateAttributeModesOfEntityByServiceType(IAttributeModelCollection attributeModels, LocaleEnum locale, MDMServiceType serviceType)
        {
            if (attributeModels != null && attributeModels.Count > 0)
            {
                foreach (AttributeModel attributeModel in attributeModels)
                {
                    if (attributeModel.Locale == locale)
                    {
                        if (serviceType == MDMServiceType.Flattened)
                        {
                            attributeModel.EnableHistory = false;
                        }
                        else if (serviceType == MDMServiceType.Searchable)
                        {
                            attributeModel.Searchable = false;
                        }
                    }
                }
            }
        }

        #endregion Data Model Exclusion Methods

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitySearchData"></param>
        /// <param name="entity"></param>
        /// <param name="entityRefreshContext"></param>
        /// <param name="diagnosticActivity">Indicates diagnosticActivity</param>
        private void RefreshSearchValAndKeyVal(EntitySearchData entitySearchData, Entity entity, EntityContext entityRefreshContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                var attributeModels = GetAttributeModels(entity, entityRefreshContext);

                Regex trimmer = new Regex(@"\s\s+", RegexOptions.Compiled);
                String keyValHeader = String.Format("{0} {1}", entity.Name, entity.LongName);

                var isSearchValueChanged = false;

                #region Remove search value for unmapped attributes. Attributes not found in entity but there in entityRefreshContext will be unmapped attributes

                if (entityRefreshContext != null && entityRefreshContext.AttributeIdList != null && attributeModels != null && attributeModels.Count > 0)
                {
                    foreach (Int32 attributeId in entityRefreshContext.AttributeIdList)
                    {
                        //Remove value for all the given locales.
                        foreach (LocaleEnum locale in entityRefreshContext.DataLocales)
                        {
                            AttributeModel attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, locale);

                            if (attributeModel == null)
                            {
                                String searchCriteria = String.Format("q{0}L{1}", attributeId, (Int32)locale);
                                entitySearchData.SearchValue = ReplaceString(entitySearchData.SearchValue, searchCriteria, String.Empty);

                                EntitySearchValuesBuilder entitySearchValuesBuilder = new EntitySearchValuesBuilder();
                                entitySearchValuesBuilder.AddSearchValue(entitySearchData.SearchValue);

                                //remove excess whitespace
                                entitySearchData.SearchValue = trimmer.Replace(entitySearchValuesBuilder.SearchValue, " ");
                                isSearchValueChanged = true;
                            }
                        }
                    }
                }

                #endregion Remove search value for unmapped attributes. Attributes not found in entity but there in entityRefreshContext will be unmapped attributes

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    foreach (AttributeModel attributeModel in attributeModels)
                    {
                        //NOTE : If Attribute is not searchable, don't do anything.....
                        if (attributeModel.AttributeModelType == AttributeModelType.MetaDataAttribute || !attributeModel.Searchable)
                            continue;

                        var entitySearchValuesBuilder = new EntitySearchValuesBuilder();

                        #region Remove all search value for this attribute in the searchVal

                        String replacementString = String.Format(" q{0}L{1}z", attributeModel.Id, (Int32)attributeModel.Locale);

                        String searchCriteria = String.Format("q{0}L{1}", attributeModel.Id, (Int32)attributeModel.Locale);
                        entitySearchData.SearchValue = ReplaceString(entitySearchData.SearchValue, searchCriteria, String.Empty);
                        entitySearchValuesBuilder.AddSearchValue(entitySearchData.SearchValue);

                        #endregion

                        Attribute attribute = (Attribute)entity.GetAttribute(attributeModel.Id, attributeModel.Locale);

                        if (attributeModel.AllowNullSearch && (attribute == null || (attribute.GetCurrentValues() == null || attribute.GetCurrentValues().Count == 0)))
                        {
                            if (attributeModel.Locale == systemDataLocale) // WE populate NULLz only for SDL locale..
                            {
                                entitySearchValuesBuilder.AddSearchValue(String.Format("q{0}L{1}NULLz", attributeModel.Id, (Int32)attributeModel.Locale));
                                isSearchValueChanged = true;
                            }
                        }
                        else
                        {
                            if (attribute == null)
                                continue;

                            // For Integer & Decimal we will populate only null values in search table
                            if ((attribute.AttributeDataType == AttributeDataType.Decimal || attribute.AttributeDataType == AttributeDataType.Integer))
                                continue;

                            foreach (Value value in attribute.GetCurrentValues())
                            {
                                String attrValue;

                                if (attribute.AttributeDataType == AttributeDataType.DateTime || attribute.AttributeDataType == AttributeDataType.Date)
                                {
                                    attrValue = GetFormattedDateTime(value.DateVal, attribute.AttributeDataType);
                                }
                                else
                                {
                                    attrValue = value.GetStringValue();
                                }

                                if (String.IsNullOrEmpty(attrValue))
                                {
                                    entitySearchValuesBuilder.AddSearchValue(String.Format("q{0}L{1}BLANKz", attribute.Id, (Int32)attribute.Locale));
                                    isSearchValueChanged = true;

                                    if (isTracingEnabled)
                                    {
                                        diagnosticActivity.LogInformation("Updating SearchVal : AttributeId = " + attribute.Id + " || Attribute Value is blank");
                                    }
                                }
                                else
                                {
                                    String cleanedAttrValue = Regex.Replace(attrValue, _regFormat, replacementString, _options);
                                    String searchValue = String.Format("q{0}L{1}z{2}", attribute.Id, (Int32)attribute.Locale, cleanedAttrValue);

                                    if (isTracingEnabled)
                                    {
                                        diagnosticActivity.LogInformation("Updating SearchVal : AttributeId = " + attribute.Id + " || Attribute value = " + value.GetStringValue() + " || SearchValue = " + searchValue);
                                    }

                                    entitySearchValuesBuilder.AddSearchValue(searchValue);
                                    isSearchValueChanged = true;
                                }
                            }
                        }

                        //remove excess whitespace
                        entitySearchData.SearchValue = trimmer.Replace(entitySearchValuesBuilder.SearchValue, " ");
                    }
                }

                if (isSearchValueChanged)
                {
                    String keyValue = GenerateKeyValue(keyValHeader, entitySearchData.SearchValue, diagnosticActivity);
                    entitySearchData.KeyValue = trimmer.Replace(keyValue, " ");
                }

            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateValue"></param>
        /// <param name="attributeDataType"></param>
        /// <returns></returns>
        private String GetFormattedDateTime(DateTime? dateValue, AttributeDataType attributeDataType)
        {
            if (dateValue != null)
            {
                String format = (attributeDataType == AttributeDataType.DateTime) ? "yyyyMMddTHHmmss" : "yyyyMMdd";
                return dateValue.Value.ToString(format);
            }
            else
                return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        private AttributeModelCollection GetAttributeModels(Entity entity, EntityContext entityContext)
        {
            var attributeModels = entity.AttributeModels;

            if (attributeModels == null || attributeModels.Count < 1)
            {
                var attributeModelBL = new AttributeModelBL();

                var attributeModelContext = new AttributeModelContext
                    {
                        ContainerId = entity.ContainerId,
                        EntityTypeId = entity.EntityTypeId,
                        CategoryId = entity.CategoryId,
                        AttributeModelType = entityContext.AttributeModelType,
                        ApplySecurity = false,
                        ApplySorting = false,
                        Locales = entityContext.DataLocales
                    };

                Collection<Int32> attributeIds = entityContext.AttributeIdList;

                if (attributeIds != null && attributeIds.Count > 0)
                {
                    attributeModels = attributeModelBL.Get(attributeIds, null, null, attributeModelContext);
                }
                else
                {
                    attributeModels = attributeModelBL.Get(attributeModelContext);
                }
            }

            return attributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValueHeader"></param>
        /// <param name="searchValue"></param>
        /// <param name="diagnosticActivity"></param>
        /// <returns></returns>
        private String GenerateKeyValue(String keyValueHeader, String searchValue, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            var entitySearchValuesBuilder = new EntitySearchValuesBuilder();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                entitySearchValuesBuilder.AddKeyValue(keyValueHeader);

                //split the search value based on whitespace and take each value and fetch only the attribute value
                //search val header is ignored
                String[] searchValArray = searchValue.Split();

                foreach (var searchVal in searchValArray)
                {
                    if (!(searchVal.StartsWith("q1z") || searchVal.StartsWith("q3z") || searchVal.StartsWith("q22z") || searchVal.StartsWith("q23z") || searchVal.StartsWith("q24z") || searchVal.StartsWith("q27z")))
                    {
                        String[] result = searchVal.Split(new[] { 'z' }, 2);
                        if (result.Count() > 1)
                            entitySearchValuesBuilder.AddKeyValue(result[1]);
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entitySearchValuesBuilder.KeyValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityRefreshContext"></param>
        /// <returns></returns>
        private EntitySearchData PopulateSearchValAndKeyVal(Entity entity, EntityContext entityRefreshContext)
        {
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            EntitySearchData entitySearchData = new EntitySearchData();
            entitySearchData.EntityId = entity.Id;
            entitySearchData.ContainerId = entity.ContainerId;
            entitySearchData.IdPath = entity.IdPath;
            entitySearchData.ProgramName = "EntitySearchDataBL";

            Regex trimmer = new Regex(@"\s\s+", RegexOptions.Compiled);

            EntitySearchValuesBuilder entitySearchValuesBuilder = PopulateSearchValKeyValHeader(entity);

            var attributeModels = GetAttributeModels(entity, entityRefreshContext);

            if (attributeModels != null && attributeModels.Count > 0)
            {
                foreach (AttributeModel attributeModel in attributeModels)
                {
                    //If attribute is meta-data attribute or not searchable or inheritable only, then do not add the attribute in dn_search table.
                    if (attributeModel.AttributeModelType == AttributeModelType.MetaDataAttribute || !attributeModel.Searchable || attributeModel.InheritableOnly)
                        continue;

                    Attribute attribute = (Attribute)entity.GetAttribute(attributeModel.Id, attributeModel.Locale);

                    if (attributeModel.AllowNullSearch && (attribute == null || (attribute.GetCurrentValues() == null || attribute.GetCurrentValues().Count == 0)))
                    {
                        if (attributeModel.Locale == systemDataLocale) // WE populate NULLz only for SDL locale..
                        {
                            entitySearchValuesBuilder.AddSearchValue(String.Format("q{0}L{1}NULLz", attributeModel.Id, (Int32)attributeModel.Locale));
                        }
                    }
                    else
                    {
                        if (attribute == null)
                        {
                            continue;
                        }

                        // For Integer & Decimal we will populate only null values in search table
                        if ((attribute.AttributeDataType == AttributeDataType.Decimal || attribute.AttributeDataType == AttributeDataType.Integer))
                        {
                            continue;
                        }

                        String replacementString = String.Format(" q{0}L{1}z", attribute.Id, (Int32)attribute.Locale);

                        foreach (Value value in attribute.GetCurrentValues())
                        {
                            String attrValue = String.Empty;
                            String searchValue = String.Empty;

                            if (attribute.AttributeDataType == AttributeDataType.DateTime || attribute.AttributeDataType == AttributeDataType.Date)
                            {
                                attrValue = GetFormattedDateTime(value.DateVal, attribute.AttributeDataType);
                            }
                            else
                            {
                                attrValue = value.GetStringValue();
                            }


                            if (String.IsNullOrWhiteSpace(attrValue))
                            {
                                searchValue = String.Format("q{0}L{1}BLANKz", attribute.Id, (Int32)attribute.Locale);
                            }
                            else
                            {
                                var cleanedAttrValue = Regex.Replace(attrValue, _regFormat, replacementString, _options);
                                searchValue = String.Format("q{0}L{1}z{2}", attribute.Id, (Int32)attribute.Locale, cleanedAttrValue);
                            }

                            entitySearchValuesBuilder.AddSearchValue(searchValue);

                            if (!String.IsNullOrWhiteSpace(attrValue))
                            {
                                entitySearchValuesBuilder.AddKeyValue(Regex.Replace(attrValue, _regFormat, " ", _options));
                            }
                        }
                    }
                }
            }

            //remove excess whitespace
            entitySearchData.SearchValue = trimmer.Replace(entitySearchValuesBuilder.SearchValue, " ");
            entitySearchData.KeyValue = trimmer.Replace(entitySearchValuesBuilder.KeyValue, " ");

            return entitySearchData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private EntitySearchValuesBuilder PopulateSearchValKeyValHeader(Entity entity)
        {
            EntitySearchValuesBuilder entitySearchValuesBuilder = new EntitySearchValuesBuilder();
            String idPath = String.Empty;

            idPath = entity.IdPath.Trim().Replace(" ", "c q1z") + "c";

            String shortName = Regex.Replace(entity.Name, _regFormat, " q22z", _options);
            String longName = Regex.Replace(entity.LongName, _regFormat, " q23z", _options);
            String searchValHeader = String.Format("q1z{0} q3z{1} q27z{2} q22z{3} q23z{4} q4z{5}", idPath, entity.EntityTypeId, entity.Id, shortName, longName, entity.ContainerId);
            String keyValHeader = String.Format("{0} {1}", entity.Name, entity.LongName);

            entitySearchValuesBuilder.AddSearchValue(searchValHeader);
            entitySearchValuesBuilder.AddKeyValue(keyValHeader);

            return entitySearchValuesBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="replaceString"></param>
        /// <returns></returns>
        private String ReplaceString(String inputString, String searchCriteria, String replaceString)
        {
            String pattern = searchCriteria + @"(.*?)+\s";
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled;

            foreach (Match match in Regex.Matches(inputString, pattern, options))
            {
                if (!String.IsNullOrEmpty(inputString))
                {
                    inputString = inputString.Replace(match.Value, replaceString);
                }
            }

            return inputString;
        }

        #endregion Helper Methods

        #endregion

        #endregion
    }
}