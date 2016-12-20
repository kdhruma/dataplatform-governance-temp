using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Transactions;
using System.Linq;
using System.Collections.Generic;
using System.Xml;

namespace MDM.EntityManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.EntityManager.Data;
    using MDM.Utility;
    using MDM.ConfigurationManager.Business;
    using MDM.Interfaces;
    using MDM.BusinessObjects.DataModel;
    using MDM.MessageManager.Business;
    using MDM.HierarchyManager.Business;
    using MDM.CategoryManager.Business;
    using MDM.BufferManager;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies Entity Locale Manager
    /// </summary>
    public class EntityLocaleBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting system UI locale..
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Category BL.
        /// </summary>
        public EntityLocaleBL()
            : base()
        {
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        ///  Process given list of entities based on their actions and data locales
        /// </summary>
        /// <param name="dataXml">XML representing Entity - ID, Locale and action
        /// <example>
        ///  <Entities>
        ///            <Entity Id="190" EntityId = “134” Name="AP" LongName="Apparel" Locale="1" Action="Update" ></Entity>
        ///            <Entity Id="-1" EntityId = “134” Name="AP" LongName="Kleider" Locale="21" Action="Create" ></Entity>
        ///            <Entity Id="-2" EntityId = “134” Name="AP" LongName="服飾" Locale="48" Action="Create" ></Entity>
        ///     </Entities>
        /// </example>
        /// </param>
        /// <param name="systemDataLocale">Indicates the system data locale</param>
        /// <param name="returnResult">Whether need to return result or not </param>
        /// <param name="callerContext">Indicates the CallerContext</param>
        /// <returns>Returns EntityOperationResultCollection</returns>
        public EntityOperationResultCollection Process(String dataXml, String systemDataLocale, Boolean returnResult, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityLocale.Process", MDMTraceSource.EntityProcess, false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityManager.EntityLocale.Process..", MDMTraceSource.EntityProcess);
            }

            #region Step : Initial Setup

            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Create);

            EntityOperationResultCollection entityOperationResultCollection = null;
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            String programName = "EntityLocale Process";
            CategoryLocalePropertiesCollection categoryLocaleProperties = null;

            #endregion

            #region Step : Populate CategoryLocaleProperties from dataXMl

            categoryLocaleProperties = CreateCategoryLocaleProperties(dataXml);

            #endregion Populate CategoryLocaleProperties

            #region Step : Process Category in DB

            EntityLocaleDA entityLocaleDA = new EntityLocaleDA();
            entityOperationResultCollection = entityLocaleDA.Process(categoryLocaleProperties, userName, programName, systemDataLocale, returnResult, command);

            #endregion Process Category in DB

            #region Step : Invalidate the cache

            if (entityOperationResultCollection != null && entityOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Successful)
            {
                InvalidateCache(categoryLocaleProperties as IDataModelObjectCollection, null, callerContext);
            }

            #endregion

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityLocale.Process", MDMTraceSource.EntityProcess);
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
        public EntityCollection Get(Int64 entityId, Collection<Locale> datalocales, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityLocale.Get", MDMTraceSource.EntityGet, false);
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityManager.EntityLocale.Get..", MDMTraceSource.EntityGet);

            #region Step : Initial Setup

            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            EntityCollection entities = null;
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            Int32 systemDataLocaleId = (Int32)MDM.Utility.GlobalizationHelper.GetSystemUILocale();

            #endregion

            EntityLocaleDA entityLocaleDA = new EntityLocaleDA();
            entities = entityLocaleDA.Get(entityId, datalocales, systemDataLocaleId, command);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityLocale.Get", MDMTraceSource.EntityGet);

            return entities;
        }

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = iDataModelObjects as CategoryLocalePropertiesCollection;

            if (categoryLocalePropertiesCollection != null && categoryLocalePropertiesCollection.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int32 categoryLocalePropertiesToBeCreated = -1;

                foreach (CategoryLocaleProperties categoryLocaleProperties in categoryLocalePropertiesCollection)
                {
                    DataModelOperationResult categoryLocalePropertiesOperationResult = new DataModelOperationResult(categoryLocaleProperties.Id, categoryLocaleProperties.LongName, categoryLocaleProperties.ExternalId, categoryLocaleProperties.ReferenceId);

                    if (String.IsNullOrEmpty(categoryLocalePropertiesOperationResult.ExternalId))
                    {
                        categoryLocalePropertiesOperationResult.ExternalId = categoryLocaleProperties.Name;
                    }

                    if (categoryLocaleProperties.Id < 1)
                    {
                        categoryLocaleProperties.Id = categoryLocalePropertiesToBeCreated;
                        categoryLocalePropertiesOperationResult.Id = categoryLocalePropertiesToBeCreated;
                        categoryLocalePropertiesToBeCreated--;
                    }

                    operationResultCollection.Add(categoryLocalePropertiesOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ValidateInputParameters(iDataModelObjects as CategoryLocalePropertiesCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = iDataModelObjects as CategoryLocalePropertiesCollection;

            if (categoryLocalePropertiesCollection != null)
            {
                LoadOriginalCategoryLocaleProperties(categoryLocalePropertiesCollection, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillCategoryLocaleProperties(iDataModelObjects as CategoryLocalePropertiesCollection, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CompareAndMerge(iDataModelObjects as CategoryLocalePropertiesCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = iDataModelObjects as CategoryLocalePropertiesCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (categoryLocalePropertiesCollection.Count > 0)
            {
                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                #region Perform category locale properties updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    new EntityLocaleDA().Process(categoryLocalePropertiesCollection, operationResults, userName, callerContext.ProgramName, GlobalizationHelper.GetSystemDataLocale().ToString(), true, command);
                    transactionScope.Complete();
                }

                //LocalizeErrors(operationResults, callerContext);

                #endregion
            }
        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            if (iDataModelObjects is CategoryLocalePropertiesCollection)
            {
                CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = iDataModelObjects as CategoryLocalePropertiesCollection;
                Dictionary<Int32, Collection<LocaleEnum>> hierachyIdBasedLocalesDictionary = new Dictionary<Int32, Collection<LocaleEnum>>();

                if (categoryLocalePropertiesCollection.Count > 0)
                {
                    foreach (CategoryLocaleProperties categoryLocaleProperties in categoryLocalePropertiesCollection)
                    {
                        Collection<LocaleEnum> dataLocales = null;
                        Int32 hierarchyId = categoryLocaleProperties.HierarchyId;

                        hierachyIdBasedLocalesDictionary.TryGetValue(hierarchyId, out dataLocales);

                        if (dataLocales == null)
                        {
                            hierachyIdBasedLocalesDictionary.Add(hierarchyId, new Collection<LocaleEnum>() { categoryLocaleProperties.Locale });
                        }
                        else
                        {
                            dataLocales.Add(categoryLocaleProperties.Locale);
                        }
                    }

                    if (hierachyIdBasedLocalesDictionary.Count > 0)
                    {
                        CategoryBufferManager categoryBufferManager = new CategoryBufferManager();

                        foreach (Int32 hierarchyId in hierachyIdBasedLocalesDictionary.Keys)
                        {
                            categoryBufferManager.RemoveCategoryLocaleProperties(hierachyIdBasedLocalesDictionary[hierarchyId], hierarchyId);
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryLocalePropertiesCollection"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalCategoryLocaleProperties(CategoryLocalePropertiesCollection categoryLocalePropertiesCollection, CallerContext callerContext)
        {
            Int32 systemDataLocale = (Int32)GlobalizationHelper.GetSystemDataLocale();
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            Dictionary<Int32, CategoryCollection> baseCatgoriesByHierarchyId = new Dictionary<Int32, CategoryCollection>();

            //Since this is internal hierarchy get , pass apply security as false.
            HierarchyCollection hierarchies = new HierarchyBL().GetAll(callerContext, false);
            CategoryBL categoryBL = new CategoryBL();

            foreach (CategoryLocaleProperties categoryLocaleProperties in categoryLocalePropertiesCollection)
            {
                //Find a hierarchy from collection with give hierarchy name.
                Hierarchy hierarchy = hierarchies.Get(categoryLocaleProperties.HierarchyName);
                CategoryCollection baseCategories = null;

                if (hierarchy != null)
                {
                    baseCatgoriesByHierarchyId.TryGetValue(hierarchy.Id, out baseCategories);

                    if (baseCategories == null)
                    {
                        baseCategories = categoryBL.GetAllBaseCategories(hierarchy.Id, callerContext);

                        if (!baseCategories.IsNullOrEmpty())
                        {
                            baseCatgoriesByHierarchyId.Add(hierarchy.Id, baseCategories);
                        }
                    }

                    Category category = baseCategories.Get(hierarchy.Id, categoryLocaleProperties.Path);

                    if (category != null)
                    {
                        CategoryLocalePropertiesCollection originalCategoryLocalePropertiesCollection = new EntityLocaleDA().GetCategoryLocaleProperties(category.Id, new Collection<LocaleEnum>() { categoryLocaleProperties.Locale }, systemDataLocale, command);

                        if (originalCategoryLocalePropertiesCollection != null && originalCategoryLocalePropertiesCollection.Count > 0)
                        {
                            categoryLocaleProperties.OriginalCategoryLocaleProperties = originalCategoryLocalePropertiesCollection.ElementAt(0);
                        }

                        categoryLocaleProperties.CategoryId = category.Id;
                        categoryLocaleProperties.HierarchyId = hierarchy.Id;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryLocalePropertiesCollection"></param>
        /// <param name="callerContext"></param>
        private void FillCategoryLocaleProperties(CategoryLocalePropertiesCollection categoryLocalePropertiesCollection, CallerContext callerContext)
        {
            foreach (CategoryLocaleProperties categoryLocaleProperties in categoryLocalePropertiesCollection)
            {
                if (categoryLocaleProperties.Id < 1)
                {
                    categoryLocaleProperties.Id = (categoryLocaleProperties.OriginalCategoryLocaleProperties != null) ? categoryLocaleProperties.OriginalCategoryLocaleProperties.Id : categoryLocaleProperties.Id;
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryLocalePropertiesCollection"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(CategoryLocalePropertiesCollection categoryLocalePropertiesCollection, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (CategoryLocaleProperties deltaCategoryLocaleProperties in categoryLocalePropertiesCollection)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaCategoryLocaleProperties.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaCategoryLocaleProperties.Action == ObjectAction.Read || deltaCategoryLocaleProperties.Action == ObjectAction.Ignore)
                    continue;

                CategoryLocaleProperties origCategoryLocaleProperties = deltaCategoryLocaleProperties.OriginalCategoryLocaleProperties;

                if (origCategoryLocaleProperties != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaCategoryLocaleProperties.Action != ObjectAction.Delete)
                    {
                        origCategoryLocaleProperties.MergeDelta(deltaCategoryLocaleProperties, callerContext, false);
                    }
                }
                else
                {
                    String errorMessage = String.Empty;

                    if (deltaCategoryLocaleProperties.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113692", "Delete action is invalid for the locale: {0} of category: {1}", new Object[] { deltaCategoryLocaleProperties.Locale, deltaCategoryLocaleProperties.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (deltaCategoryLocaleProperties.CategoryId < 1)
                        {
                            AddOperationResult(operationResult, "VP000321", String.Empty, new Object[] { deltaCategoryLocaleProperties.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        //If original object is not found then set Action as Create always.
                        deltaCategoryLocaleProperties.Action = ObjectAction.Create;
                    }
                    operationResult.PerformedAction = deltaCategoryLocaleProperties.Action;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataXml"></param>
        /// <returns></returns>
        private CategoryLocalePropertiesCollection CreateCategoryLocaleProperties(String dataXml)
        {
            XmlTextReader reader = null;
            CategoryLocalePropertiesCollection categoryLocaleProperties = new CategoryLocalePropertiesCollection();

            try
            {
                reader = new XmlTextReader(dataXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                    {
                        #region Read category locale properties

                        if (reader.HasAttributes)
                        {
                            CategoryLocaleProperties categoryLocalePropertiesRecord = new CategoryLocaleProperties();

                            if (reader.MoveToAttribute("Id"))
                            {
                                categoryLocalePropertiesRecord.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("EntityId"))
                            {
                                categoryLocalePropertiesRecord.CategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("HierarchyId"))
                            {
                                categoryLocalePropertiesRecord.HierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                categoryLocalePropertiesRecord.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LongName"))
                            {
                                categoryLocalePropertiesRecord.LongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Locale"))
                            {
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out locale);
                                categoryLocalePropertiesRecord.Locale = locale;
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                ObjectAction action = ObjectAction.Unknown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out action);
                                categoryLocalePropertiesRecord.Action = action;
                            }

                            categoryLocaleProperties.Add(categoryLocalePropertiesRecord);
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return categoryLocaleProperties;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryLocalePropertiesCollection"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(CategoryLocalePropertiesCollection categoryLocalePropertiesCollection, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            Collection<String> duplicateCategoryLocales = new Collection<String>();

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            if (categoryLocalePropertiesCollection == null || categoryLocalePropertiesCollection.Count < 1)
            {
                AddOperationResults(operationResults, "113591", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                foreach (CategoryLocaleProperties categoryLocaleProperties in categoryLocalePropertiesCollection)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(categoryLocaleProperties.ReferenceId);

                    if (String.IsNullOrWhiteSpace(categoryLocaleProperties.HierarchyName))
                    {
                        AddOperationResult(operationResult, "112688", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    if (String.IsNullOrWhiteSpace(categoryLocaleProperties.Name))
                    {
                        AddOperationResult(operationResult, String.Empty, "Category name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(categoryLocaleProperties.HierarchyName))
                        {
                            String duplicateCategoryLocale = String.Format("{0}-{1}-{2}-{3}", categoryLocaleProperties.HierarchyName.GetHashCode(), categoryLocaleProperties.Name.GetHashCode(), categoryLocaleProperties.Path.GetHashCode(), categoryLocaleProperties.Locale.GetHashCode());

                            if (duplicateCategoryLocales.Contains(duplicateCategoryLocale))
                            {
                                AddOperationResult(operationResult, "112016", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else
                            {
                                duplicateCategoryLocales.Add(duplicateCategoryLocale);
                            }
                        }
                    }
                    if (String.IsNullOrWhiteSpace(categoryLocaleProperties.LongName))
                    {
                        AddOperationResult(operationResult, String.Empty, "Category long name is empty or not specified", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        /// <param name="callerContext">CallerContext</param>
        private void LocalizeErrors(DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                foreach (Error error in operationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode))
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResults(DataModelOperationResultCollection operationResults, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResult(IDataModelOperationResult operationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}