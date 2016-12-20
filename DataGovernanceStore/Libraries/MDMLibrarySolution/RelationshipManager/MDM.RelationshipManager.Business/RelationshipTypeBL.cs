using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.RelationshipManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.RelationshipManager.Data;
    using MDM.Utility;
    using MDM.ActivityLogManager.Business;

    /// <summary>
    /// Specifies business logic operations for relationship type
    /// </summary>
    public class RelationshipTypeBL : BusinessLogicBase, IRelationshipTypeManager, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private RelationshipTypeDA _relationshipDA = new RelationshipTypeDA();

        /// <summary>
        /// 
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// 
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// 
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// 
        /// </summary>
        private bool _isDistributedCacheWithNotificationEnabled;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Constructor

        public RelationshipTypeBL()
        {
            GetSecurityPrincipal();
            _isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructor

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// Create new RelationshipType
        /// </summary>
        /// <param name="relationshipType">Represent RelationshipType Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Creation</returns>
        /// <exception cref="ArgumentNullException">If RelationshipType Object is Null</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType LongName is Null or having empty String</exception>
        public OperationResult Create(RelationshipType relationshipType, CallerContext callerContext)
        {
            this.ValidateRelationshipType(relationshipType, "Create");

            relationshipType.Action = Core.ObjectAction.Create;
            return this.Process(relationshipType, callerContext);
        }

        /// <summary>
        /// Update existing relationshipType
        /// </summary>
        /// <param name="relationshipType">Represent RelationshipType Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Updating</returns>
        /// <exception cref="ArgumentNullException">If RelationshipType Object is Null</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If RelationshipType LongName is Null or having empty String</exception>
        public OperationResult Update(RelationshipType relationshipType, CallerContext callerContext)
        {
            this.ValidateRelationshipType(relationshipType, "Update");

            relationshipType.Action = Core.ObjectAction.Update;
            return this.Process(relationshipType, callerContext);
        }

        /// <summary>
        /// Delete relationshipType
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Deletion</returns>
        /// <returns>True if RelationshipType Creation is Successful</returns>
        /// <exception cref="ArgumentNullException">If RelationshipType Object is Null</exception>
        public OperationResult Delete(RelationshipType relationshipType, CallerContext callerContext)
        {
            this.ValidateRelationshipType(relationshipType, "Delete");

            relationshipType.Action = Core.ObjectAction.Delete;
            return this.Process(relationshipType, callerContext);
        }

        /// <summary>
        /// Process relationshipTypes
        /// </summary>
        /// <param name="relationshipTypes">RelationshipTypes to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Creation</returns>
        public OperationResultCollection Process(RelationshipTypeCollection relationshipTypes, CallerContext callerContext)
        {
            #region Parameter Validation

            ValidateInputParameters(relationshipTypes, callerContext);

            #endregion Parameter Validation

            OperationResultCollection operationResults = new OperationResultCollection();

            foreach (RelationshipType relType in relationshipTypes)
            {
                OperationResult relOR = this.Process(relType, callerContext);

                if (relOR != null)
                {
                    operationResults.Add(relOR);
                }
            }

            #region activitylog

            operationResults.RefreshOperationResultStatus();

            if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
            {
                LogDataModelChanges(relationshipTypes, callerContext);
            }

            #endregion activity log

            return operationResults;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Get relationshipType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RelationshipType GetById(Int32 id, CallerContext callerContext)
        {
            RelationshipTypeCollection relationshipTypes = GetAll(callerContext);
            RelationshipType relType = null;

            if (relationshipTypes != null)
            {
                IRelationshipType iRelType = relationshipTypes.GetRelationshipType(id);

                if (iRelType != null)
                {
                    relType = (RelationshipType)iRelType;
                }
            }

            return relType;
        }

        /// <summary>
        /// Get relationshipType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RelationshipTypeCollection GetByIds(Collection<Int32> relationshipTypeIdList, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (relationshipTypeIdList == null || relationshipTypeIdList.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113960", new Object[] { "RelationshipTypeIdList" }, false, callerContext);
                    diagnosticActivity.LogError("113960", _localeMessage.Message);
                    throw new MDMOperationException("113960", _localeMessage.Message, "HierarchyBL.GetByIds", String.Empty, "Get");
                }

                var relationshipTypeCollection = GetAll(callerContext);

                if (relationshipTypeCollection != null && relationshipTypeCollection.Any())
                {
                    foreach (RelationshipType relationshipType in relationshipTypeCollection)
                    {
                        if (relationshipTypeIdList.Contains(relationshipType.Id))
                        {
                            if (_currentTraceSettings.IsBasicTracingEnabled)
                            {
                                diagnosticActivity.LogInformation(String.Format("Requested relationship type id :{0} ", relationshipType.Id));
                            }

                            relationshipTypes.Add(relationshipType);
                        }
                    }
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return relationshipTypes;
        }


        /// <summary>
        /// Get relationship based on container and entity type Id.
        /// </summary>
        /// <param name="containerId">This parameter specifying catalog id.</param>
        /// <param name="entityTypeId">This parameter specifying entity type id.</param>
        /// <returns>collection of relationship type</returns>
        /// <exception cref="ArgumentNullException">Raised when container id is null</exception>
        public Tuple<RelationshipTypeCollection, Int32> Get(Int32 containerId, Int32 entityTypeId, CallerContext callerContext)
        {
            #region Parameter Validation

            if (containerId <= 0)
                throw new ArgumentNullException("CatalogId must be greater than 0.");

            #endregion

            return this.GetAllRelationshipTypes(containerId, entityTypeId, callerContext);
        }

        /// <summary>
        /// Get relationship based on container id and nodeType Id.
        /// </summary>
        /// <param name="containerId">Specifies container id.</param>
        /// <param name="entityTypeId">Specifies entity type id.</param>
        /// <param name="callerContext">Name of application and module which is performing action</param>
        /// <returns>collection of relationship types</returns>
        public RelationshipTypeCollection GetRelationshipTypes(Int32 containerId, Int32 entityTypeId, CallerContext callerContext)
        {
            RelationshipTypeCollection relationshipTypes = null;

            Tuple<RelationshipTypeCollection, Int32> relTypeTuple = GetAllRelationshipTypes(containerId, entityTypeId, callerContext);

            if (relTypeTuple != null)
            {
                relationshipTypes = relTypeTuple.Item1;
            }

            return relationshipTypes;
        }

        /// <summary>
        /// Get all relationshipTypes
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>All Relationship types</returns>
        public RelationshipTypeCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.StartTraceActivity("RelationshipTypeBL.GetAll", false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Start - RelationshipTypeBL.GetAll call");

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "RelationshipTypeBL.GetAll", String.Empty, "GetAll");
            }

            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();

            try
            {
                ICache cache = CacheFactory.GetCache();
                Object cachedRelationshipTypes = cache[CacheKeyGenerator.GetAllRelationshipTypesCacheKey()];

                if (cachedRelationshipTypes != null)
                {
                    relationshipTypes = (RelationshipTypeCollection)cachedRelationshipTypes;
                }
                else
                {
                    Tuple<RelationshipTypeCollection, Int32> relTypeTuple = this.GetAllRelationshipTypes(0, 0, callerContext);

                    if (relTypeTuple != null)
                    {
                        relationshipTypes = relTypeTuple.Item1;
                        DateTime expirationTime = DateTime.Now.AddDays(5);
                        cache.Set(CacheKeyGenerator.GetAllRelationshipTypesCacheKey(), relationshipTypes, expirationTime);

                        //handle distributed cache
                        if (_isDistributedCacheWithNotificationEnabled)
                        {
                            CacheSynchronizationHelper cacheHelper = new CacheSynchronizationHelper();
                            cacheHelper.NotifyLocalCacheInsert(CacheKeyGenerator.GetAllRelationshipTypesCacheKey(), expirationTime, false);
                        }
                    }
                }
            }
            finally
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "End - RelationshipTypeBL.GetAll call");
                MDMTraceHelper.StopTraceActivity("RelationshipTypeBL.GetAll");
            }

            return relationshipTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public RelationshipType GetByName(String relationshipTypeName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeBL.GetByName", MDMTraceSource.DataModel, false);

            #region Parameter Validation
            if (String.IsNullOrEmpty(relationshipTypeName))
            {
                String errorMessage = this.GetSystemLocaleMessage("100059", callerContext).Message;
                throw new MDMOperationException("100059", errorMessage, "RelationshipManager.GetByName", String.Empty, "GetByName");
            }
            #endregion

            RelationshipType relationshipType = null;
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested relationship type name:{0} ", relationshipTypeName), MDMTraceSource.DataModel);

                RelationshipTypeCollection relationshipTypes = GetAll(callerContext);

                if (relationshipTypes != null && relationshipTypes.Count > 0)
                {
                    relationshipType = relationshipTypes.Get(relationshipTypeName);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeBL.GetByName", MDMTraceSource.DataModel);
            }

            return relationshipType;
        }

        /// <summary>
        /// Get Collection of RelationshipType objects based on collection of relationship type names
        /// </summary>
        /// <param name="relationshipTypeNames">Represents a collection of Relationship type names</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns></returns>
        public RelationshipTypeCollection GetByNames(IEnumerable<String> relationshipTypeNames, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeBL.GetByNames", MDMTraceSource.DataModel, false);
                    
            var relationshipTypes = new RelationshipTypeCollection();
            var incorrectNames = new Collection<String>();
            try
            {                
                RelationshipTypeCollection cachedRelationshipTypes = GetAll(callerContext);
                if (cachedRelationshipTypes != null && cachedRelationshipTypes.Count > 0)
                {
                    foreach (var relTypeName in relationshipTypeNames)
                    {
                        if (String.IsNullOrEmpty(relTypeName))
                        {
                            String errorMessage = this.GetSystemLocaleMessage("100059", callerContext).Message;
                            throw new MDMOperationException("100059", errorMessage, "RelationshipManager.GetByName", String.Empty, "GetByName");
                        }
                        var relationshipType = cachedRelationshipTypes.Get(relTypeName);

                        if (relationshipType != null)
                        {
                            relationshipTypes.Add(relationshipType);
                        }
                        else
                        {
                            incorrectNames.Add(relTypeName);                    
                        }                        
                    }

                    if (incorrectNames.Count > 0)
                    {
                        String errorMessage = this.GetSystemLocaleMessage("113771", callerContext).Message;
                        String msg = String.Format(errorMessage, String.Join(", ", incorrectNames));
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), msg, false, callerContext);
                        throw new MDMOperationException("113771", msg, "RelationshipManager.GetByNames", String.Empty, "GetByNames");
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeBL.GetByNames", MDMTraceSource.DataModel);
            }

            return relationshipTypes;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            RelationshipTypeCollection relationshipTypes = iDataModelObjects as RelationshipTypeCollection;

            if (relationshipTypes != null && relationshipTypes.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int32 relatoinshipTypeToBeCreated = -1;

                foreach (RelationshipType relationshipType in relationshipTypes)
                {
                    DataModelOperationResult relationshipTypeOperationResult = new DataModelOperationResult(relationshipType.Id, relationshipType.LongName, relationshipType.ExternalId, relationshipType.ReferenceId);

                    if (String.IsNullOrEmpty(relationshipTypeOperationResult.ExternalId))
                    {
                        relationshipTypeOperationResult.ExternalId = relationshipType.Name;
                    }

                    if (relationshipType.Id < 1)
                    {
                        relationshipType.Id = relatoinshipTypeToBeCreated;
                        relationshipTypeOperationResult.Id = relatoinshipTypeToBeCreated;
                        relatoinshipTypeToBeCreated--;
                    }

                    operationResultCollection.Add(relationshipTypeOperationResult);
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
            ValidateInputParameters(iDataModelObjects as RelationshipTypeCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            RelationshipTypeCollection relationshipTypes = iDataModelObjects as RelationshipTypeCollection;

            if (relationshipTypes != null)
            {
                LoadOriginalRelationshipTypes(relationshipTypes, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillRelationshipTypes(iDataModelObjects as RelationshipTypeCollection, iCallerContext as CallerContext);
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
            CompareAndMerge(iDataModelObjects as RelationshipTypeCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            RelationshipTypeCollection relationshipTypes = iDataModelObjects as RelationshipTypeCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (relationshipTypes.Count > 0)
            {
                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                #region Perform relationship type updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    new RelationshipTypeDA().Process(relationshipTypes, operationResults, callerContext.ProgramName, userName, command);
                    transactionScope.Complete();
                }

                LocalizeErrors(operationResults, callerContext);

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
            foreach (RelationshipType relationshipType in iDataModelObjects as RelationshipTypeCollection)
            {
                InValidateCache(relationshipType.Id);
            }

            ICache cache = CacheFactory.GetCache();
            cache.Remove(CacheKeyGenerator.GetAllRelationshipTypesCacheKey());
        }

        #endregion

        #endregion

        #region Private Methods

        #region Process Methods

        /// <summary>
        /// Process relationshipType
        /// </summary>
        /// <param name="relationshipType">RelationshipType to process</param>
        /// <param name="callerContext">Context indicating who is calling the API</param>
        private OperationResult Process(RelationshipType relationshipType, CallerContext callerContext)
        {
            MDMTraceHelper.StartTraceActivity("RelationshipTypeBL.Process", false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Start - RelationshipTypeBL.Process");

            #region Parameter Validation

            if (relationshipType == null)
            {
                throw new MDMOperationException("112137", "RelationshipType cannot be null", "RelationshipTypeBL.Process", String.Empty, "Process");
            }

            if (String.IsNullOrWhiteSpace(relationshipType.Name) && relationshipType.Action != ObjectAction.Delete)
            {
                throw new MDMOperationException("100059", "Relationship Type Short Name cannot be empty", "RelationshipTypeBL.Process", String.Empty, "Process");
            }

            if (String.IsNullOrWhiteSpace(relationshipType.LongName) && relationshipType.Action != ObjectAction.Delete)
            {
                throw new MDMOperationException("100060", "Relationship Type Long Name cannot be empty", "RelationshipTypeBL.Process", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "RelationshipTypeBL.Process", String.Empty, "Process");
            }

            #endregion

            OperationResult relOR = null;

            try
            {
                String userName = String.Empty;
                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = "MDM.RelationshipManager.Business.RelationshipTypeBL.Process";
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Start - RelationshipTypeDA.Process call");

                    relOR = _relationshipDA.Process(relationshipType, callerContext.ProgramName, userName, command);

                    LocalizeErrors(callerContext, relOR);

                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "End - RelationshipTypeDA.Process call");

                    if (relOR != null && relOR.OperationResultStatus == Core.OperationResultStatusEnum.None || relOR.OperationResultStatus == Core.OperationResultStatusEnum.Successful)
                    {
                        transactionScope.Complete();

                        #region Invalidate cache

                        //If successful, delete cache
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(CacheKeyGenerator.GetAllRelationshipTypesCacheKey());

                        InValidateCache(relationshipType.Id);

                        #endregion Invalidate cache
                        
                    }
                    else
                    {
                        #region Update OR with messages for error code

                        //if there are any error codes coming in OR from db, populate error messages for them
                        foreach (Error error in relOR.Errors)
                        {
                            if (!String.IsNullOrEmpty(error.ErrorCode))
                            {
                                _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                                if (_localeMessage != null)
                                {
                                    error.ErrorMessage = _localeMessage.Message;
                                }
                            }
                        }

                        #endregion Update OR with messages for error code
                    }
                }
            }
            finally
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "End - RelationshipTypeBL.Process");
                MDMTraceHelper.StopTraceActivity("RelationshipTypeBL.Process");
            }

            return relOR;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypes"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(RelationshipTypeCollection relationshipTypes, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (RelationshipType deltaRelationshipType in relationshipTypes)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaRelationshipType.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaRelationshipType.Action == ObjectAction.Read || deltaRelationshipType.Action == ObjectAction.Ignore)
                    continue;

                IRelationshipType origRelationshipType = deltaRelationshipType.OriginalRelationshipType;

                if (origRelationshipType != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaRelationshipType.Action != ObjectAction.Delete)
                    {
                        origRelationshipType.MergeDelta(deltaRelationshipType, callerContext, false);
                    }
                }
                else
                {
                    String errorMessage = String.Empty;
                    if (deltaRelationshipType.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113601", String.Empty, new Object[] {deltaRelationshipType.Name}, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        //If original object is not found then set Action as Create always.
                        deltaRelationshipType.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaRelationshipType.Action;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipId"></param>
        private void InValidateCache(Int32 relationshipId)
        {
            //handle distributed cache
            if (_isDistributedCacheWithNotificationEnabled)
            {
                CacheSynchronizationHelper cacheHelper = new CacheSynchronizationHelper();
                cacheHelper.NotifyLocalCacheRemoval(CacheKeyGenerator.GetAllRelationshipTypesCacheKey());
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Tuple<RelationshipTypeCollection, Int32> GetAllRelationshipTypes(Int32 containerId, Int32 entityTypeId, CallerContext callerContext)
        {
            //Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Start - RelationshipTypeDA.GetAll call");
            Tuple<RelationshipTypeCollection, Int32> relTypeTuple = null;

            relTypeTuple = _relationshipDA.Get(containerId, entityTypeId, command);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "End - RelationshipTypeDA.GetAll call");

            return relTypeTuple;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypes"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalRelationshipTypes(RelationshipTypeCollection relationshipTypes, CallerContext callerContext)
        {
            RelationshipTypeCollection originalRelationshipTypes = GetAll(callerContext);

            if (originalRelationshipTypes != null && originalRelationshipTypes.Count > 0)
            {
                foreach (RelationshipType relationshipType in relationshipTypes)
                {
                    relationshipType.OriginalRelationshipType = originalRelationshipTypes.Get(relationshipType.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypes"></param>
        /// <param name="callerContext"></param>
        private void FillRelationshipTypes(RelationshipTypeCollection relationshipTypes, CallerContext callerContext)
        {
            foreach (RelationshipType relationshipType in relationshipTypes)
            {
                if (relationshipType.Id < 1)
                {
                    relationshipType.Id = (relationshipType.OriginalRelationshipType != null) ? relationshipType.OriginalRelationshipType.Id : relationshipType.Id;
                }
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipType"></param>
        /// <param name="methodName"></param>
        private void ValidateRelationshipType(RelationshipType relationshipType, String methodName)
        {
            if (relationshipType == null)
            {
                throw new MDMOperationException("112137", "RelationshipType cannot be null", "RelationshipTypeBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypes"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(RelationshipTypeCollection relationshipTypes, CallerContext callerContext)
        {
            if (relationshipTypes == null || relationshipTypes.Count < 1)
            {
                throw new MDMOperationException("112137", "RelationshipType cannot be null", "RelationshipTypeBL.Process", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "RelationshipTypeBL.Process", String.Empty, "Process", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypes"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(RelationshipTypeCollection relationshipTypes, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            Collection<String> shortNames = new Collection<String>();

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            if (relationshipTypes == null || relationshipTypes.Count < 1)
            {
                AddOperationResults(operationResults, "112817", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                foreach (RelationshipType relationshipType in relationshipTypes)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(relationshipType.ReferenceId);

                    if (String.IsNullOrWhiteSpace(relationshipType.Name))
                    {
                        AddOperationResult(operationResult, "113993", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (shortNames.Contains(relationshipType.Name))
                        {
                            AddOperationResult(operationResult, "112150", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            shortNames.Add(relationshipType.Name);
                        }
                    }
                    if (String.IsNullOrWhiteSpace(relationshipType.LongName))
                    {
                        AddOperationResult(operationResult, "100060", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// //if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="callerContext">The caller context</param>
        /// <param name="relationshipTypeProcessOperationResult">Operation result to be modified</param>
        private void LocalizeErrors(CallerContext callerContext, OperationResult relationshipTypeProcessOperationResult)
        {
            foreach (Error error in relationshipTypeProcessOperationResult.Errors)
            {
                if (!String.IsNullOrEmpty(error.ErrorCode))
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false,
                                                          callerContext);

                    if (_localeMessage != null)
                    {
                        error.ErrorMessage = _localeMessage.Message;
                    }
                }
            }
        }

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
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Returns Message Object based on message code
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

            return localeMessageBL.Get(_systemUILocale, messageCode, false, callerContext);
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

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="relationshipTypeCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(RelationshipTypeCollection relationshipTypeCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(relationshipTypeCollection);

            #endregion step: populate datamodelactivitylog object

            #region step: make api call

            if (activityLogCollection != null) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion step: make api call
        }

        #endregion

        #endregion

        #endregion
    }
}