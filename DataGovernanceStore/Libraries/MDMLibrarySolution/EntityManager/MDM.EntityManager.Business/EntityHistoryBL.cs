//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using SM = System.ServiceModel;

//namespace MDM.EntityManager.Business
//{
//    using MDM.AttributeModelManager.Business;
//    using MDM.BufferManager;
//    using MDM.BusinessObjects;
//    using MDM.CacheManager.Business;
//    using MDM.ConfigurationManager.Business;
//    using MDM.ContainerManager.Business;
//    using MDM.Core;
//    using MDM.Core.Exceptions;
//    using MDM.DataModelManager.Business;
//    using MDM.DQM.PdfGenerator;
//    using MDM.EntityManager.Data;
//    using MDM.ExceptionManager;
//    using MDM.Interfaces;
//    using MDM.LookupManager.Business;
//    using MDM.MessageManager.Business;
//    using MDM.RelationshipManager.Business;
//    using MDM.Utility;
//    using RS.MDM.Configuration;
//    using RS.MDM.ConfigurationObjects;
//    using BOFile = MDM.BusinessObjects.File;
//    using UomManager = MDM.UomManager.Business;

//    /// <summary>
//    /// Business Logic for Entity History
//    /// </summary>
//    public class EntityHistoryBL : BusinessLogicBase
//    {
//        #region Fields

//        /// <summary>
//        /// Indicates instance of Entity History DA
//        /// </summary>
//        private EntityHistoryDA _entityHistoryDA = new EntityHistoryDA();

//        /// <summary>
//        /// Indicates entity history buffer manager
//        /// </summary>
//        private EntityHistoryBufferManager _entityHistoryBufferManager = new EntityHistoryBufferManager();

//        /// <summary>
//        /// Indicates instance of Locale Message BL
//        /// </summary>
//        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

//        /// <summary>
//        /// Field denoting the security principal
//        /// </summary>
//        private SecurityPrincipal _securityPrincipal = null;

//        /// <summary>
//        /// Object used for thread safety
//        /// </summary>
//        private static Object entityHistoryGetLockObject = new object();

//        #endregion

//        #region Private Properties

//        #endregion

//        #region Constructor

//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        public EntityHistoryBL()
//        {
//            GetSecurityPrincipal();
//        }

//        #endregion Constructor

//        #region Methods

//        #region Public methods

//        /// <summary>
//        /// Get history for a requested entity.
//        /// </summary>
//        /// <param name="entityId">Indicates id of an entity for which history is requested.</param>
//        /// <param name="entityHistoryContext">Indicates context for entity history.</param>
//        /// <param name="callerContext">Indicates caller context.</param>
//        /// <returns>EntityHistory object</returns>
//        public EntityHistory Get(Int64 entityId, EntityHistoryContext entityHistoryContext, CallerContext callerContext)
//        {
//            EntityHistory entityHistory = null;
//            Collection<Int64> entityIds = new Collection<Int64>();
//            entityIds.Add(entityId);

//            EntityCollectionHistory entityCollectionHistory = this.Get(entityIds, entityHistoryContext, callerContext);

//            if (entityCollectionHistory != null && entityCollectionHistory.Count > 0)
//            {
//                entityHistory = (EntityHistory)entityCollectionHistory.GetEntityHistory(entityId);
//            }

//            return entityHistory;
//        }

//        /// <summary>
//        /// Get history for multiple entities for requested entities.
//        /// </summary>
//        /// <param name="entityIds">Ids of entities for which history is requested.</param>
//        /// <param name="entityHistoryContext">Indicates context for entity history.</param>
//        /// <param name="callerContext">Indicates the Caller Context</param>
//        /// <returns>EntityCollectionHistory object</returns>
//        public EntityCollectionHistory Get(Collection<Int64> entityIds, EntityHistoryContext entityHistoryContext, CallerContext callerContext)
//        {
//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHistoryBL.Get", MDMTraceSource.Entity, false);
//            }
//            EntityCollectionHistory entityCollectionHistory = null;
//            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
//            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

//            try
//            {
//                #region Validations

//                ValidateCallerContext(callerContext, "Get");
//                ValidateEntityIds(entityIds, callerContext, "Get");
//                ValidateEntityHistoryContext(entityHistoryContext, callerContext, "Get");

//                #endregion Validations

//                if (Constants.TRACING_ENABLED)
//                {
//                    String strEntityIds = String.Empty;
//                    foreach (Int64 entityId in entityIds)
//                    {
//                        strEntityIds += entityIds + ", ";
//                    }

//                    if (Constants.TRACING_ENABLED)
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.Entity);
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Fetching entity history data for entity ids - " + strEntityIds, MDMTraceSource.Entity);
//                    }
//                }

//                // Get command
//                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

//                EntityCollection entities = GetEntities(entityIds, entityHistoryContext, entityHistoryContext.IsHistoryForCategory, callerContext);

//                entityCollectionHistory = GetEntitiesHistory(entities, entityHistoryContext, command, callerContext);

//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Get Entity History from DB", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);

//                PopulateEntityCollectionHistoryDetails(entityCollectionHistory, entityHistoryContext, entities, callerContext);

//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - PopulateEntityCollectionHistoryDetails", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);
//            }
//            finally
//            {
//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                {
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load requested entityHistory", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);
//                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHistoryBL.Get", MDMTraceSource.Entity);
//                }
//            }

//            return entityCollectionHistory;
//        }

//        /// <summary>
//        /// Get the entity history details template for the specific locale
//        /// </summary>
//        /// <param name="templateLocale">Indicates the locale for which template is required</param>
//        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
//        /// <returns>Returns the collection of entity history templates for the given locale</returns>
//        public EntityHistoryDetailsTemplateCollection GetEntityHistoryDetailsTemplates(LocaleEnum templateLocale, CallerContext callerContext)
//        {
//            if (Constants.TRACING_ENABLED)
//                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHistoryBL.GetEntityHistoryDetailsTemplates", MDMTraceSource.Entity, false);

//            #region Initial Setup

//            EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates = null;

//            //Get Command
//            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

//            #endregion Initial Setup

//            try
//            {
//                #region Validation

//                ValidateCallerContext(callerContext, "GetEntityHistoryDetailsTemplates");

//                #endregion Validation

//                #region Get entity history details templates from Cache if available

//                //Try to get from cache..
//                if (Constants.TRACING_ENABLED)
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Finding entity history details templates for locale:{0} in cache...", templateLocale.ToString()), MDMTraceSource.Entity);

//                entityHistoryDetailsTemplates = _entityHistoryBufferManager.FindEntityHistoryDetailsTemplate(templateLocale);

//                #endregion

//                #region If not found in cache, then load it from the database

//                if (entityHistoryDetailsTemplates == null || entityHistoryDetailsTemplates.Count < 1)
//                {
//                    if (Constants.TRACING_ENABLED)
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("No entity history details template found for locale:{0}. Now templates would be loaded from database.", templateLocale.ToString()), MDMTraceSource.Entity);
//                    }

//                    entityHistoryDetailsTemplates = _entityHistoryDA.GetEntityHistoryDetailsTemplates(command);

//                    if (Constants.TRACING_ENABLED)
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Completed getting templates from database", MDMTraceSource.Entity);
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Started to fill template details with localized template text for locale: {0}", templateLocale.ToString()), MDMTraceSource.Entity);
//                    }

//                    FillEntityHistoryTemplates(templateLocale, entityHistoryDetailsTemplates, callerContext);

//                    #region Cache Entity History Details Templates

//                    if (Constants.TRACING_ENABLED)
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Populated template details with localized template text for locale: {0}", templateLocale.ToString()), MDMTraceSource.Entity);
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Caching entity history details template for locale:{0}...", templateLocale.ToString()), MDMTraceSource.Entity);
//                    }
//                    _entityHistoryBufferManager.UpdateEntityHistoryDetailsTemplate(entityHistoryDetailsTemplates, templateLocale, 3);

//                    if (Constants.TRACING_ENABLED)
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with caching entity history details template for locale:{0}", templateLocale.ToString()), MDMTraceSource.Entity);

//                    #endregion

//                }
//                else
//                {
//                    if (Constants.TRACING_ENABLED)
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Found entity history details templates for locale:{0} in cache", templateLocale.ToString()), MDMTraceSource.Entity);
//                }

//                #endregion
//            }

//            finally
//            {
//                if (Constants.TRACING_ENABLED)
//                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHistoryBL.GetEntityHistoryDetailsTemplates", MDMTraceSource.Entity);
//            }

//            return entityHistoryDetailsTemplates;
//        }

//        /// <summary>
//        ///  Entity history details send to the requested users by mail as a PDF format based on the Entity History Context
//        /// </summary>
//        /// <param name="entityId">Ids of entities for which history is requested</param>
//        /// <param name="entityHistoryContext">Indicates context for entity history</param>
//        /// <param name="columnsToBeConsidered">Indicates the columns collection which has to be consider</param>
//        /// <param name="tomailAddress">Indicates collection of 'toEmail Address'</param>
//        /// <param name="mailSubject">Indicates the mail subject</param>
//        /// <param name="callerContext">Indicates the Caller Context</param>
//        /// <returns>Return the operation result.</returns>
//        public OperationResult SendEntityHistoryDetailsAsMail(Int64 entityId, EntityHistoryContext entityHistoryContext, ColumnCollection columnsToBeConsidered, Collection<String> tomailAddress, String mailSubject, CallerContext callerContext)
//        {
//            OperationResult operationResult = new OperationResult();
//            ColumnCollection validColumns = new ColumnCollection();
//            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
//            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHistoryBL.SendEntityHistoryDetailsAsMail", MDMTraceSource.Entity, false);
//            }

//            try
//            {
//                #region Parameter Validations

//                #region columnsToBeConsider Validations

//                //Valid column filter
//                if (columnsToBeConsidered != null && columnsToBeConsidered.Count > 0)
//                {
//                    foreach (Column column in columnsToBeConsidered)
//                    {
//                        if (!String.IsNullOrWhiteSpace(column.Name))
//                        {
//                            validColumns.Add(column);
//                        }
//                    }
//                }

//                if (validColumns == null || validColumns.Count < 1)
//                {
//                    String errorMessage = this.GetSystemLocaleMessage("112834", callerContext).Message;
//                    throw new MDMOperationException("112834", errorMessage, "EntityHistoryManager", String.Empty, "SendEntityHistoryDetailsAsMail");   //'ColumnsToBeConsider' parameter is 'null or empty' or the column short name is not provided
//                }
//                #endregion

//                #region Mail Address Validations

//                if (tomailAddress == null || tomailAddress.Count < 1)
//                {
//                    String errorMessage = this.GetSystemLocaleMessage("111967", callerContext).Message;
//                    throw new MDMOperationException("111967", errorMessage, "EntityHistoryManager", String.Empty, "SendEntityHistoryDetailsAsMail");   //'To' mail addresses are not provided. Please provide either ‘toMDMUsers' or ‘'toMailIds'
//                }

//                Collection<String> validMailAddress = ValidateAndFilterMailAddresses(tomailAddress, operationResult, callerContext, "toMailIds");

//                if (validMailAddress == null || validMailAddress.Count < 0)     //If there is no 'TO' mail address is valid then return it.
//                {
//                    String errorMessage = this.GetSystemLocaleMessage("111993", callerContext).Message;
//                    throw new MDMOperationException("111993", errorMessage, "EntityHistoryManager", String.Empty, "SendEntityHistoryDetailsAsMail");
//                }

//                #endregion

//                #endregion

//                EntityHistory entityHistory = this.Get(entityId, entityHistoryContext, callerContext);

//                LocaleEnum modelDisplayLocale = GlobalizationHelper.GetModelDisplayLocale(entityHistoryContext.CurrentDataLocale, entityHistoryContext.CurrentUILocale);
//                LocaleEnum dataFormattingLocale = GlobalizationHelper.GetDataFormattingLocale(entityHistoryContext.CurrentDataLocale, entityHistoryContext.CurrentUILocale);
//                SetLocaleDetails(modelDisplayLocale, dataFormattingLocale);

//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                {
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Entity History Get operation completed.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);
//                }

//                #region Prepare PDF file details

//                if (entityHistory != null && entityHistory.Count > 0)
//                {

//                    String documentTitle = this.GetSystemLocaleMessage("112833", entityHistoryContext.CurrentUILocale, callerContext, new Object[] { entityHistory.EntityLongName }).Message;    //{0} History Details
//                    String companyName = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataQualityManagement.PDFReport.CompanyName");
//                    String companyLogoFile = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataQualityManagement.PDFReport.CompanyLogoPath");
//                    String dbServerTimeZone = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.TimeZone.DBServerTimeZone", "UTC");
//                    String currentTimeZone = String.Empty;

//                    List<String> companyAddresses = new List<String>()
//                {
//                    AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataQualityManagement.PDFReport.CompanyAddressLine1"),
//                    AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataQualityManagement.PDFReport.CompanyAddressLine2"),
//                    AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataQualityManagement.PDFReport.CompanyAddressLine3")
//                };

//                    if (_securityPrincipal != null && _securityPrincipal.UserPreferences != null)
//                    {
//                        currentTimeZone = _securityPrincipal.UserPreferences.DefaultTimeZoneShortName;
//                    }

//                    if (String.IsNullOrWhiteSpace(currentTimeZone))
//                    {
//                        currentTimeZone = dbServerTimeZone;
//                    }

//                    EntityHistories reportGenerator = new EntityHistories(entityHistoryContext.CurrentUILocale, callerContext)
//                {
//                    DocumentTitle = documentTitle,
//                    DocumentSubject = mailSubject,
//                    CompanyName = companyName,
//                    DocumentName = documentTitle,
//                    CompanyAddresses = companyAddresses,
//                    CompanyLogoFile = companyLogoFile
//                };

//                #endregion

//                    File file = reportGenerator.GenerateEntityHistoryPDFFile(new EntityCollectionHistory() { entityHistory }, entityHistoryContext, validColumns, documentTitle, dbServerTimeZone, currentTimeZone, this.GetDataFormattingLocale());

//                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Entity History PDF file generated", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);
//                    }

//                    if (String.IsNullOrWhiteSpace(mailSubject))
//                    {
//                        mailSubject = documentTitle;
//                    }

//                    this.SendMail(validMailAddress, null, mailSubject, null, file, callerContext);

//                    //Set operation result status to successful
//                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
//                }
//                else
//                {
//                    //What to do.. Add message code here.
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "There is no records to send a mail", MDMTraceSource.Entity);
//                    operationResult.AddOperationResult("", "There is no records to send a mail", OperationResultType.Error);
//                }

//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                {
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Entity History details send to the user as a PDF file", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to Send Entity History Details to the user.", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);
//                }
//            }
//            catch (MDMOperationException ex)
//            {
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.Entity);
//                operationResult.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
//            }
//            catch (System.Net.Mail.SmtpException ex)
//            {
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.Entity);
//                operationResult.AddOperationResult("113405", ex.Message, OperationResultType.Error);
//            }
//            catch (System.Configuration.ConfigurationException ex)
//            {
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.Entity);
//                operationResult.AddOperationResult("113405", ex.Message, OperationResultType.Error);
//            }
//            catch (Exception ex)
//            {
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.Entity);
//                operationResult.AddOperationResult("111995", new Collection<Object>() { ex.Message }, OperationResultType.Error);
//                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
//            }
//            finally
//            {
//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                {
//                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHistoryBL.SendEntityHistoryDetailsAsMail", MDMTraceSource.Entity);
//                }
//            }

//            return operationResult;
//        }

//        #endregion

//        #region Private Methods

//        private void PopulateEntityCollectionHistoryDetails(EntityCollectionHistory entityCollectionHistory, EntityHistoryContext entityHistoryContext, EntityCollection currentEntities, CallerContext callerContext)
//        {
//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHistoryBL.PopulateEntityCollectionHistoryDetails", MDMTraceSource.Entity, false);
//            }

//            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
//            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

//            if (entityCollectionHistory != null && entityCollectionHistory.Count > 0)
//            {
//                #region Initialization

//                Collection<Int64> entityIds = null;
//                Collection<Int64> categoryIds = null;
//                Collection<Int32> attributeIds = null;
//                Dictionary<Int32, String> attributeIdsToValues = null;

//                EntityCollection entities = new EntityCollection();
//                EntityCollection categories = null;
//                ContainerCollection containers = null;
//                EntityTypeCollection entityTypes = null;
//                RelationshipTypeCollection relationshipTypes = null;
//                AttributeModelCollection attributeModels = null;
//                UOMCollection uoms = null;
//                EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates = null;
//                Dictionary<Int32, Lookup> attributeIdToLookup = null;

//                LocaleEnum modelDisplayLocale = GlobalizationHelper.GetModelDisplayLocale(entityHistoryContext.CurrentDataLocale, entityHistoryContext.CurrentUILocale);
//                LocaleEnum dataFormattingLocale = GlobalizationHelper.GetDataFormattingLocale(entityHistoryContext.CurrentDataLocale, entityHistoryContext.CurrentUILocale);
//                SetLocaleDetails(modelDisplayLocale, dataFormattingLocale);

//                #endregion Initialization

//                #region Step 1: Read all Ids

//                entityIds = new Collection<Int64>();
//                categoryIds = new Collection<Int64>();
//                attributeIdsToValues = new Dictionary<Int32, String>();
//                attributeIds = new Collection<Int32>();
//                entityHistoryDetailsTemplates = new EntityHistoryDetailsTemplateCollection();

//                if (currentEntities != null && currentEntities.Count > 0)
//                {
//                    if (entityHistoryContext.IsHistoryForCategory)
//                    {
//                        categories = currentEntities;
//                    }
//                    else
//                    {
//                        entities = currentEntities;
//                    }
//                }

//                foreach (EntityHistory entityHistory in entityCollectionHistory)
//                {
//                    if (entityHistory != null && entityHistory.Count > 0)
//                    {
//                        foreach (EntityHistoryRecord entityHistoryRecord in entityHistory)
//                        {
//                            if (entityHistoryRecord != null)
//                            {
//                                #region Add category id

//                                if (entityHistoryRecord.ChangedData_CategoryId > 0)
//                                {
//                                    if (!categoryIds.Contains(entityHistoryRecord.ChangedData_CategoryId))
//                                    {
//                                        categoryIds.Add(entityHistoryRecord.ChangedData_CategoryId);
//                                    }
//                                }

//                                #endregion

//                                #region Add related entity id

//                                if (entityHistoryRecord.ChangedData_RelatedEntityId > 0)
//                                {
//                                    if (!entityIds.Contains(entityHistoryRecord.ChangedData_RelatedEntityId))
//                                    {
//                                        entityIds.Add(entityHistoryRecord.ChangedData_RelatedEntityId);
//                                    }
//                                }

//                                #endregion

//                                #region Add extension category id

//                                if (entityHistoryRecord.ChangedData_ExtensionCategoryId > 0)
//                                {
//                                    if (!categoryIds.Contains(entityHistoryRecord.ChangedData_ExtensionCategoryId))
//                                    {
//                                        categoryIds.Add(entityHistoryRecord.ChangedData_ExtensionCategoryId);
//                                    }
//                                }

//                                #endregion

//                                #region Add attribute id

//                                if (entityHistoryRecord.ChangedData_AttributeId > 0)
//                                {
//                                    if (!attributeIds.Contains(entityHistoryRecord.ChangedData_AttributeId))
//                                    {
//                                        attributeIds.Add(entityHistoryRecord.ChangedData_AttributeId);
//                                    }

//                                    if (!attributeIdsToValues.ContainsKey(entityHistoryRecord.ChangedData_AttributeId))
//                                    {
//                                        if (!String.IsNullOrWhiteSpace(entityHistoryRecord.ChangedData_AttrVal))
//                                        {
//                                            attributeIdsToValues.Add(entityHistoryRecord.ChangedData_AttributeId, entityHistoryRecord.ChangedData_AttrVal);
//                                        }
//                                    }
//                                    else
//                                    {
//                                        String attrVal = attributeIdsToValues[entityHistoryRecord.ChangedData_AttributeId];

//                                        attrVal += "," + entityHistoryRecord.ChangedData_AttrVal;

//                                        attributeIdsToValues[entityHistoryRecord.ChangedData_AttributeId] = attrVal;
//                                    }
//                                }

//                                #endregion

//                                #region Promote Related
//                                switch (entityHistoryRecord.ChangeType)
//                                {
//                                    case EntityChangeType.Promote:
//                                        {
//                                            String[] promoteMsgParams = entityHistoryRecord.PromoteMessageParams.Split(new String[] { "#@#" }, StringSplitOptions.None);
//                                            if (promoteMsgParams != null && promoteMsgParams.Length > 0)
//                                            {
//                                                Int32 index = 2;
//                                                if (String.Compare(entityHistoryRecord.PromoteMessageCode, "114287") == 0)
//                                                {
//                                                    index = 1;
//                                                }

//                                                entityHistoryRecord.PromotedRootEntityId = ValueTypeHelper.Int64TryParse(promoteMsgParams[index], -1);
//                                                if (entityHistoryRecord.PromotedRootEntityId > 0 && !entityIds.Contains(entityHistoryRecord.PromotedRootEntityId))
//                                                {
//                                                    entityIds.Add(entityHistoryRecord.PromotedRootEntityId);
//                                                }
//                                            }
//                                            break;
//                                        }
//                                    case EntityChangeType.AutoPromote:
//                                    case EntityChangeType.EmergencyPromote:
//                                        {
//                                            String attributeIdsString = entityHistoryRecord.PromoteMessageParams;
//                                            if (!String.IsNullOrWhiteSpace(attributeIdsString))
//                                            {
//                                                Int32[] attrIds = ValueTypeHelper.SplitStringToIntArray(attributeIdsString, ',');

//                                                if (attrIds != null && attrIds.Length > 0)
//                                                {
//                                                    for (Int32 i = 0; i < attrIds.Length; i++)
//                                                    {
//                                                        if (attrIds[i] > 0 && !attributeIds.Contains(attrIds[i]))
//                                                        {
//                                                            attributeIds.Add(attrIds[i]);
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                        }
//                                        break;
//                                }
                                
//                                #endregion Promote Related
//                            }
//                        }
//                    }
//                }

//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Read all ids", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);

//                #endregion Read all ids

//                #region Step 2: Get data from ids

//                #region Get entity history detail template

//                entityHistoryDetailsTemplates = GetEntityHistoryDetailsTemplates(entityHistoryContext.CurrentUILocale, callerContext);

//                #endregion

//                #region Get entities from entity ids

//                EntityCollection relatedEntities = GetEntities(entityIds, entityHistoryContext, false, callerContext);
                
//                if (relatedEntities != null)
//                {
//                    entities.AddRange(relatedEntities);
//                }

//                #endregion

//                #region Get categories from category ids

//                EntityCollection relatedCategories = GetEntities(categoryIds, entityHistoryContext, true, callerContext);

//                if (relatedCategories != null)
//                {
//                    // Update CategoryLongNamePath & Category Path per what History needs
//                    String seperator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " ");

//                    foreach (var category in relatedCategories)
//                    {
//                        if (String.IsNullOrWhiteSpace(category.CategoryPath))
//                        {
//                            category.CategoryPath = category.Name;
//                        }
//                        else
//                        {
//                            category.CategoryPath = String.Concat(category.CategoryPath, seperator, category.Name);
//                        }

//                        if (String.IsNullOrWhiteSpace(category.CategoryLongNamePath))
//                        {
//                            category.CategoryLongNamePath = category.LongName;
//                        }
//                        else
//                        {
//                            category.CategoryLongNamePath = String.Concat(category.CategoryLongNamePath, seperator, category.LongName);
//                        }
//                    }

//                    if (categories != null)
//                    {
//                        categories.AddRange(relatedCategories);
//                    }
//                    else
//                    {
//                        categories = relatedCategories;
//                    }
//                }

//                #endregion

//                #region Get all containers

//                if (entityHistoryContext.LoadExtensionRelationshipsVersionDetails)
//                {
//                    containers = GetAllContainers(callerContext);

//                    if (containers == null)
//                    {
//                        String errorMessage = this.GetSystemLocaleMessage("112818", callerContext).Message;
//                        throw new MDMOperationException("112818", errorMessage, "EntityManager.EntityHistoryBL.Get", String.Empty, String.Empty);
//                    }
//                }

//                #endregion

//                #region Get all entityTypes

//                if (entityHistoryContext.LoadHierarchyRelationshipsVersionDetails)
//                {
//                    entityTypes = GetAllEntityTypes(callerContext);

//                    if (entityTypes == null)
//                    {
//                        String errorMessage = this.GetSystemLocaleMessage("112854", callerContext).Message;
//                        throw new MDMOperationException("112854", errorMessage, "EntityManager.EntityHistoryBL.Get", String.Empty, String.Empty);
//                    }
//                }

//                #endregion

//                #region Get all relationship types

//                if (entityHistoryContext.LoadRelationshipsVersionDetails)
//                {
//                    relationshipTypes = GetAllRelationshipTypes(callerContext);
//                    if (relationshipTypes == null)
//                    {
//                        String errorMessage = this.GetSystemLocaleMessage("112817", callerContext).Message;
//                        throw new MDMOperationException("112817", errorMessage, "EntityManager.EntityHistoryBL.Get", String.Empty, String.Empty);
//                    }
//                }

//                #endregion

//                #region Get Attribute data from attribute ids

//                if (entityHistoryContext.LoadAttributesVersionDetails || entityHistoryContext.LoadRelationshipsVersionDetails)
//                {
//                    #region Get attribute models from atribute ids
//                    if (attributeIds != null && attributeIds.Count > 0)
//                    {
//                        attributeModels = GetAttributeModels(attributeIds);

//                        //Get lookup data for lookup attributes
//                        if (attributeModels != null && attributeModels.Count > 0)
//                        {
//                            attributeIdToLookup = GetAttributeIdToLookup(attributeModels, attributeIdsToValues, callerContext);

//                            //Get all UOMs
//                            uoms = GetAllUOMs(callerContext);
//                        }
//                        else
//                        {
//                            String errorMessage = this.GetSystemLocaleMessage("112815", callerContext).Message;
//                            throw new MDMOperationException("112815", errorMessage, "EntityManager.EntityHistoryBL.Get", String.Empty, String.Empty);
//                        }
//                    }
//                    #endregion

//                }
//                else if (entityHistoryContext.LoadPromoteVersionDetails)
//                {
//                    if (attributeIds != null && attributeIds.Count > 0)
//                    {
//                        attributeModels = GetAttributeModels(attributeIds);
//                    }
//                }
//                #endregion

//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Get data from ids", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);

//                #endregion Get data from ids

//                #region Step 3: populate data in EntityCollectionHistory

//                String dbServerTimeZone = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.TimeZone.DBServerTimeZone", "UTC");
//                String currentTimeZone = _securityPrincipal.UserPreferences.DefaultTimeZoneShortName;
//                currentTimeZone = String.IsNullOrWhiteSpace(currentTimeZone) ? dbServerTimeZone : currentTimeZone;
//                LocaleEnum currentUILocale = entityHistoryContext.CurrentUILocale;

//                Collection<DayGroup> applicableDateGroups = GetApplicableDayGroups(dbServerTimeZone, currentTimeZone, currentUILocale);

//                foreach (EntityHistory entityHistory in entityCollectionHistory)
//                {
//                    #region Fill data to entity history properties

//                    if (entityHistoryContext.IsHistoryForCategory)
//                    {
//                        IEntity iEntity = categories.GetEntity(entityHistory.EntityId);
//                        if (iEntity != null)
//                        {
//                            entityHistory.EntityLongName = iEntity.LongName;
//                            entityHistory.EntityTypeLongName = iEntity.EntityTypeLongName;
//                            entityHistory.EntityCatalogLongName = iEntity.ContainerLongName;
//                        }
//                    }
//                    else
//                    {
//                        IEntity iEntity = entities.GetEntity(entityHistory.EntityId);
//                        if (iEntity != null)
//                        {
//                            entityHistory.EntityLongName = iEntity.LongName;
//                            entityHistory.EntityTypeLongName = iEntity.EntityTypeLongName;
//                            entityHistory.EntityCatalogLongName = iEntity.ContainerLongName;
//                        }
//                    }

//                    #endregion

//                    PopulateEnityHistoryRecord(entityHistory, entityIds, entities, categoryIds, categories, attributeIds, attributeModels,
//                                            containers, entityTypes, relationshipTypes, uoms, entityHistoryDetailsTemplates, attributeIdToLookup,
//                                            dbServerTimeZone, currentTimeZone, currentUILocale, applicableDateGroups);
//                }

//                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - populate data in EntityCollectionHistory", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);

//                #endregion
//            }

//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//            {
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to Populate EntityCollectionHistoryDetails", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.Entity);
//                MDMTraceHelper.StopTraceActivity("EntityManager.EntityHistoryBL.PopulateEntityCollectionHistoryDetails", MDMTraceSource.Entity);
//            }
//        }

//        private void PopulateEnityHistoryRecord(EntityHistory currentEntityHistory, Collection<Int64> entityIds, EntityCollection entities,
//                        Collection<Int64> categoryIds, EntityCollection categories, Collection<Int32> attributeIds, AttributeModelCollection attributeModels,
//                        ContainerCollection containers, EntityTypeCollection entityTypes, RelationshipTypeCollection relationshipTypes, UOMCollection uoms,
//                        EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates, Dictionary<Int32, Lookup> attributeIdToLookup,
//                        String dbServerTimeZone, String currentTimeZone, LocaleEnum currentUILocale, Collection<DayGroup> applicableDateGroups)
//        {
//            //Flag to indicate whether entity history record has to be added in final result or not. 
//            //In case of entity Common,technical, relationship attribute history if attribute is collection then we will have multiple records coming from DB, But
//            //Final result should receive only single record with multiple values merged. for that this flag is maintained.
//            Boolean addHistoryRecordInFinalResult = true;
//            AttributeModel attributeModel = null;
//            Collection<EntityHistoryRecord> entityHistoryRecords = new Collection<EntityHistoryRecord>();
//            Dictionary<String, String> previousRecordValuesDictionary = new Dictionary<String, String>();
//            Dictionary<String, String> previousRecordSourceDictionary = new Dictionary<String, String>();

//            String collectionValueSeparator = AppConfigurationHelper.GetAppConfig<String>("EntityAttributePublish.CollectionValueDelimiter", "|");
//            String uomSeparator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.UomSeparator", "|");

//            #region Fill data in entity history record

//            Int32 entityHistoryRecordsCount = currentEntityHistory.Count;

//            for (Int32 i = (entityHistoryRecordsCount - 1); i > -1; i--)
//            {
//                EntityHistoryRecord entityHistoryRecord = currentEntityHistory.ElementAt(i);

//                attributeModel = null;
//                addHistoryRecordInFinalResult = true;

//                #region Switch case for different change types

//                switch (entityHistoryRecord.ChangeType)
//                {
//                    case EntityChangeType.CommonAttribute:
//                    case EntityChangeType.TechnicalAttribute:
//                    case EntityChangeType.CommonComplexAttribute:
//                    case EntityChangeType.TechnicalComplexAttribute:
//                    case EntityChangeType.CommonHierarchicalAttribute:
//                    case EntityChangeType.TechnicalHierarchicalAttribute:
//                    case EntityChangeType.RelationshipAttribute:
//                        {
//                            attributeModel = (AttributeModel)attributeModels.GetAttributeModel(entityHistoryRecord.ChangedData_AttributeId, base.GetModelDisplayLocale());

//                            if (attributeModel != null)
//                            {
//                                FillAttributeDetails(attributeModel, entityHistoryRecord, attributeIdToLookup, uoms, uomSeparator);

//                                if (attributeModel.IsCollection)
//                                {
//                                    addHistoryRecordInFinalResult = MergeAttributeValues(currentEntityHistory, entityHistoryRecord, entityHistoryRecords, collectionValueSeparator, entityHistoryDetailsTemplates, attributeModel, previousRecordValuesDictionary);
//                                }

//                                if (addHistoryRecordInFinalResult)
//                                {
//                                    if (entityHistoryRecord.ChangeType == EntityChangeType.RelationshipAttribute)
//                                    {
//                                        FillRelatedEntityDetails(entities, entityTypes, entityHistoryRecord);
//                                        FillRelationshipTypeDetails(relationshipTypes, entityHistoryRecord);
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                addHistoryRecordInFinalResult = false;
//                            }

//                            break;
//                        }
//                    case EntityChangeType.ExtensionRelationship:
//                        {
//                            FillRelatedEntityDetails(entities, entityTypes, entityHistoryRecord);
//                            FillExtensionDetails(containers, categories, entityHistoryRecord);

//                            break;
//                        }
//                    case EntityChangeType.Relationship:
//                        {
//                            FillRelatedEntityDetails(entities, entityTypes, entityHistoryRecord);
//                            FillRelationshipTypeDetails(relationshipTypes, entityHistoryRecord);

//                            break;
//                        }
//                    case EntityChangeType.HierarchyRelationship:
//                        {
//                            FillRelatedEntityDetails(entities, entityTypes, entityHistoryRecord);

//                            break;
//                        }
//                    case EntityChangeType.Metadata:
//                        {
//                            FillMetadataDetails(categories, entityHistoryRecord);

//                            break;
//                        }
//                    case EntityChangeType.Metadata_Category:
//                        {
//                            FillMetadataDetails(categories, entityHistoryRecord);

//                            break;
//                        }
//                    case EntityChangeType.Metadata_LongName:
//                        {
//                            break;
//                        }
//                    case EntityChangeType.N_Level_HierarchyRelationship:
//                        {
//                            FillRelatedEntityDetails(entities, entityTypes, entityHistoryRecord);

//                            break;
//                        }
//                    case EntityChangeType.Workflow_Assignment:
//                        {
//                            FillWFActivityActionData(entityHistoryRecord);
//                            break;
//                        }
//                    case EntityChangeType.Workflow_StageTransition:
//                        {
//                            break;
//                        }
//                    case EntityChangeType.Promote:
//                        {
//                            FillPromoteDetails(entities, entityHistoryRecord);
//                            break;
//                        }
//                    case EntityChangeType.AutoPromote:
//                    case EntityChangeType.EmergencyPromote:
//                        {
//                            FillAttributePromoteDetails(attributeModels, entityHistoryRecord);
//                            break;
//                        }
//                }

//                #endregion

//                //Add this current history record into new record collection
//                if (addHistoryRecordInFinalResult)
//                {

//                    // Fill Modified Day
//                    FillModifiedDay(applicableDateGroups, entityHistoryRecord, dbServerTimeZone, currentTimeZone);

//                    //Fill previous value
//                    FillPreviousValue(previousRecordValuesDictionary, previousRecordSourceDictionary, entityHistoryRecord);

//                    //Fill entity history details template.. Current entity history record
//                    FillTemplateDetails(currentEntityHistory, entityHistoryRecord, entityHistoryDetailsTemplates, attributeModel);

//                    entityHistoryRecords.Insert(0, entityHistoryRecord);
//                }
//            }

//            #endregion

//            //Set new entity history records to current entity history
//            currentEntityHistory.SetEntityHistoryRecords(entityHistoryRecords);
//        }


//        #region Helper methods to fill entity history record
        
//        private void FillAttributePromoteDetails(AttributeModelCollection attributeModels, EntityHistoryRecord entityHistoryRecord)
//        {
//            Int32[] attrIds = ValueTypeHelper.SplitStringToIntArray(entityHistoryRecord.PromoteMessageParams, ',');
            
//            if (attrIds != null && attrIds.Length > 0)
//            {
//                List<String> attributeNamesList = new List<String>();
//                foreach (Int32 attrId in attrIds)
//                {
//                    IAttributeModel attrModel = attributeModels.GetAttributeModel(attrId, base.GetModelDisplayLocale());
//                    if (attrModel != null)
//                    {
//                        attributeNamesList.Add(attrModel.LongName);
//                    }
//                }
//                entityHistoryRecord.PromotedAttributesString = String.Join(", ", attributeNamesList);
//            }
//        }

//        private void FillPromoteDetails(EntityCollection entities, EntityHistoryRecord entityHistoryRecord)
//        {
//            if (entities != null && entities.Count > 0)
//            {
//                IEntity rootEntity = entities.GetEntity(entityHistoryRecord.PromotedRootEntityId);
//                if (rootEntity != null)
//                {
//                    entityHistoryRecord.PromotedRootEntityLongName = rootEntity.LongName;
//                    entityHistoryRecord.PromoteRootEntityTypeLongName = rootEntity.EntityTypeLongName;
//                }
//            }
//        }

//        private Collection<DayGroup> GetApplicableDayGroups(String dbServerTimeZone, String currentUITimeZone, LocaleEnum currentUILocale)
//        {
//            Collection<DayGroup> applicableDayGroups = null;
//            //Format in user’s default time zone. It should be the Time Zone Id which is of String type.
//            String wcfTimeZone = TimeZoneInfo.Local.Id;
//            DateTime currentDateTime = FormatHelper.ConvertToTimeZone(DateTime.Now, wcfTimeZone, currentUITimeZone);

//            #region Try to find the Day Group buckets with validations in cache

//            ICache cacheManager = null;
//            String cacheKey = String.Empty;

//            try
//            {
//                cacheManager = CacheFactory.GetCache();
//            }
//            catch
//            {

//            }

//            if (cacheManager != null)
//            {
//                cacheKey = CacheKeyGenerator.GetAvailableModifiedDayGroupsCacheKey(currentDateTime, currentUILocale, currentUITimeZone);
//                Object cachedObject = null;

//                try
//                {
//                    cachedObject = cacheManager.Get(cacheKey);
//                }
//                catch (Exception ex)
//                {
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
//                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
//                }

//                if (cachedObject != null && cachedObject is Collection<DayGroup>)
//                {
//                    applicableDayGroups = (Collection<DayGroup>)cachedObject;
//                }
//            }

//            #endregion

//            if (applicableDayGroups == null)
//            {
//                int timelineIndex = -1;

//                LocaleMessageBL localeManager = new LocaleMessageBL();
//                Collection<String> messageCodes = new Collection<string>() {
//                    "112870",   //Today
//                    "112871",   //Yesterday
//                    "112872",   //Sunday
//                    "112883",   //Monday
//                    "112873",   //Tuesday
//                    "112874",   //Wednesday
//                    "112875",   //Thursday
//                    "112876",   //Friday
//                    "112877",   //Saturday
//                    "112878",   //Last Week
//                    "112879",   //Two Weeks Ago
//                    "112880",   //Three Weeks Ago 
//                    "112881",   //Earlier this Month
//                    "112750",   //Last Month
//                    "112882",   //Older
//                };

//                LocaleMessageCollection localeMessages = localeManager.Get(currentUILocale, messageCodes, false, new CallerContext { Application = MDMCenterApplication.PIM, Module = MDMCenterModules.UIProcess });

//                #region Today
//                applicableDayGroups = new Collection<DayGroup>();

//                DayGroup today = new DayGroup
//                {
//                    DateGroupOperator = DayGroupOperatorEnum.EqualsTo,
//                    EqualsToValue = currentDateTime.Date,
//                    DispalyVaue = localeMessages[0].Message //"Today"
//                };
//                applicableDayGroups.Add(today);
//                #endregion

//                #region yesterday
//                DayGroup yesterday = new DayGroup
//                {
//                    DateGroupOperator = DayGroupOperatorEnum.EqualsTo,
//                    EqualsToValue = currentDateTime.AddDays(timelineIndex).Date,
//                    DispalyVaue = localeMessages[1].Message //"Yesterday"
//                };
//                applicableDayGroups.Add(yesterday);
//                #endregion

//                #region Weekdays
//                switch (currentDateTime.DayOfWeek)
//                {
//                    case DayOfWeek.Tuesday:
//                    case DayOfWeek.Wednesday:
//                    case DayOfWeek.Thursday:
//                    case DayOfWeek.Friday:
//                    case DayOfWeek.Saturday:
//                        for (int i = (int)currentDateTime.DayOfWeek - 1; i > 0; i--)
//                        {
//                            timelineIndex--;
//                            DateTime lastDateTime = currentDateTime.AddDays(timelineIndex);
//                            DayGroup weekday = new DayGroup
//                            {
//                                DateGroupOperator = DayGroupOperatorEnum.EqualsTo,
//                                EqualsToValue = lastDateTime.Date,
//                                DispalyVaue = localeMessages[(int)lastDateTime.DayOfWeek + 2].Message
//                            };

//                            applicableDayGroups.Add(weekday);
//                        }
//                        break;
//                }
//                #endregion

//                #region Last Week
//                timelineIndex--;
//                DateTime lastWeekMaxDT = currentDateTime.AddDays(timelineIndex);
//                int countOfLastWeek = currentDateTime.DayOfWeek == DayOfWeek.Sunday ? 5 : 6;
//                timelineIndex = timelineIndex - countOfLastWeek;
//                DateTime lastWeekMinDT = currentDateTime.AddDays(timelineIndex);
//                DayGroup lastWeek = new DayGroup
//                {
//                    DateGroupOperator = DayGroupOperatorEnum.Range,
//                    MinRangeValue = lastWeekMinDT.Date,
//                    MaxRangeValue = lastWeekMaxDT.Date,
//                    DispalyVaue = localeMessages[9].Message //"Last Week"
//                };
//                applicableDayGroups.Add(lastWeek);
//                #endregion

//                #region Two Weeks Ago
//                timelineIndex--;
//                DateTime twoWeeksAgoMaxDT = currentDateTime.AddDays(timelineIndex);
//                timelineIndex = timelineIndex - 6;
//                DateTime twoWeeksAgoMinDT = currentDateTime.AddDays(timelineIndex);
//                DayGroup twoWeeksAgo = new DayGroup
//                {
//                    DateGroupOperator = DayGroupOperatorEnum.Range,
//                    MinRangeValue = twoWeeksAgoMinDT.Date,
//                    MaxRangeValue = twoWeeksAgoMaxDT.Date,
//                    DispalyVaue = localeMessages[10].Message //"Two Weeks Ago"
//                };
//                applicableDayGroups.Add(twoWeeksAgo);
//                #endregion

//                #region Three Weeks Ago
//                timelineIndex--;
//                DateTime threeWeeksAgoMaxDT = currentDateTime.AddDays(timelineIndex);
//                timelineIndex = timelineIndex - 6;
//                DateTime threeWeeksAgoMinDT = currentDateTime.AddDays(timelineIndex);
//                DayGroup threeWeeksAgo = new DayGroup
//                {
//                    DateGroupOperator = DayGroupOperatorEnum.Range,
//                    MinRangeValue = threeWeeksAgoMinDT.Date,
//                    MaxRangeValue = threeWeeksAgoMaxDT.Date,
//                    DispalyVaue = localeMessages[11].Message //"Three Weeks Ago"
//                };
//                applicableDayGroups.Add(threeWeeksAgo);
//                #endregion

//                #region Earlier This Month
//                timelineIndex--;
//                // If few days are there in this month
//                if (currentDateTime.Day >= (0 - timelineIndex))
//                {
//                    DateTime earlierThisMonthMaxDT = currentDateTime.AddDays(timelineIndex);
//                    timelineIndex = timelineIndex - (currentDateTime.Day + timelineIndex);
//                    DateTime earlierThisMonthMinDT = currentDateTime.AddDays(timelineIndex);
//                    DayGroup earlierThisMonth = new DayGroup
//                    {
//                        DateGroupOperator = DayGroupOperatorEnum.Range,
//                        MinRangeValue = earlierThisMonthMinDT.Date,
//                        MaxRangeValue = earlierThisMonthMaxDT.Date,
//                        DispalyVaue = localeMessages[12].Message //"Earlier this Month"
//                    };
//                    applicableDayGroups.Add(earlierThisMonth);
//                }
//                #endregion

//                #region Last Month
//                timelineIndex--;
//                DateTime lastMonthMaxDT = currentDateTime.AddDays(timelineIndex);
//                timelineIndex = timelineIndex - lastMonthMaxDT.Day;
//                DateTime lastMonthMinDT = currentDateTime.AddDays(timelineIndex);
//                DayGroup lastMonth = new DayGroup
//                {
//                    DateGroupOperator = DayGroupOperatorEnum.Range,
//                    MinRangeValue = lastMonthMinDT.Date,
//                    MaxRangeValue = lastMonthMaxDT.Date,
//                    DispalyVaue = localeMessages[13].Message //"Last Month"
//                };
//                applicableDayGroups.Add(lastMonth);
//                #endregion

//                #region Older
//                timelineIndex--;
//                DateTime olderMinDT = currentDateTime.AddDays(timelineIndex);
//                DayGroup older = new DayGroup
//                {
//                    DateGroupOperator = DayGroupOperatorEnum.LessThan,
//                    MinRangeValue = olderMinDT.Date,
//                    DispalyVaue = localeMessages[14].Message //"Older"
//                };
//                applicableDayGroups.Add(older);
//                #endregion

//                #region Store Applicable Day Group into cache

//                if (cacheManager != null)
//                {
//                    cacheManager.Set(cacheKey, applicableDayGroups, DateTime.Now.AddHours(24));
//                }

//                #endregion
//            }

//            return applicableDayGroups;
//        }

//        private void FillModifiedDay(Collection<DayGroup> applicableDayGroups, EntityHistoryRecord entityHistoryRecord, String dbServerTimeZone, String currentTimeZone)
//        {
//            if (entityHistoryRecord.ModifiedDateTime != null)
//            {
//                DateTime dateTimeValue = ((DateTime)entityHistoryRecord.ModifiedDateTime).Date;

//                if (currentTimeZone != dbServerTimeZone)
//                {
//                    dateTimeValue = FormatHelper.ConvertToTimeZone(entityHistoryRecord.ModifiedDateTime.ToString(), dbServerTimeZone, currentTimeZone).Date;
//                }

//                foreach (DayGroup dateGroup in applicableDayGroups)
//                {
//                    Boolean isInGroup = false;

//                    switch (dateGroup.DateGroupOperator)
//                    {
//                        case DayGroupOperatorEnum.EqualsTo:
//                            isInGroup = dateTimeValue == dateGroup.EqualsToValue;
//                            break;
//                        case DayGroupOperatorEnum.Range:
//                            isInGroup = dateTimeValue >= dateGroup.MinRangeValue && dateTimeValue <= dateGroup.MaxRangeValue;
//                            break;
//                        case DayGroupOperatorEnum.LessThan:
//                            isInGroup = dateTimeValue <= dateGroup.MinRangeValue;
//                            break;
//                    }

//                    if (isInGroup)
//                    {
//                        entityHistoryRecord.ModifiedDay = dateGroup.DispalyVaue;
//                        return;
//                    }
//                }
//            }
//        }

//        private void FillAttributeDetails(AttributeModel attributeModel, EntityHistoryRecord entityHistoryRecord, Dictionary<Int32, Lookup> attributeIdToLookup, UOMCollection uoms, String uomSeparator)
//        {
//            entityHistoryRecord.ChangedData_AttributeLongName = attributeModel.LongName;
//            entityHistoryRecord.ChangedData_AttributeParentLongName = attributeModel.AttributeParentLongName;

//            if (attributeModel.IsHierarchical)
//            {
//                if (attributeModel.AttributeModelType == AttributeModelType.Common)
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.CommonHierarchicalAttribute;
//                }
//                else if (attributeModel.AttributeModelType == AttributeModelType.Category)
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.TechnicalHierarchicalAttribute;
//                }
//            }
//            else if (attributeModel.IsComplex)
//            {
//                if (attributeModel.AttributeModelType == AttributeModelType.Common)
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.CommonComplexAttribute;
//                }
//                else if (attributeModel.AttributeModelType == AttributeModelType.Category)
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.TechnicalComplexAttribute;
//                }

//            }

//            if (!entityHistoryRecord.IsInvalidData)
//            {
//                #region Perform formatting for image, File, URL, ImageURL Attributes

//                if (attributeModel.AttributeModelType == AttributeModelType.Common && (attributeModel.AttributeDataTypeName == AttributeDataType.Image.ToString() || attributeModel.AttributeDataTypeName == AttributeDataType.File.ToString()))
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.CommonFileOrImageAttribute;
//                }

//                if (attributeModel.AttributeModelType == AttributeModelType.Common && (attributeModel.AttributeDataTypeName == AttributeDataType.ImageURL.ToString() || attributeModel.AttributeDataTypeName == AttributeDataType.URL.ToString()))
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.CommonURLAttribute;
//                }

//                if (attributeModel.AttributeModelType == AttributeModelType.Category && (attributeModel.AttributeDataTypeName == AttributeDataType.Image.ToString() || attributeModel.AttributeDataTypeName == AttributeDataType.File.ToString()))
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.TechnicalFileOrImageAttribute;
//                }

//                if (attributeModel.AttributeModelType == AttributeModelType.Category && (attributeModel.AttributeDataTypeName == AttributeDataType.ImageURL.ToString() || attributeModel.AttributeDataTypeName == AttributeDataType.URL.ToString()))
//                {
//                    entityHistoryRecord.ChangeType = EntityChangeType.TechnicalURLAttribute;
//                }

//                #endregion

//                #region Update Complex attribute data
//                if (attributeModel.IsComplex)
//                {
//                    if (entityHistoryRecord.ChangedData_AttrVal.Equals(Constants.COMPLEX_ATTRIBUTE_EMPTY_INSTANCE_VALUE_REF_ID.ToString())) // This is the case when complex attribute values are Cleared. 
//                    {
//                        entityHistoryRecord.ChangedData_AttrVal = String.Empty;
//                    }
//                }
//                #endregion

//                #region Update Lookup value WSID to display format

//                if (attributeModel.IsLookup && attributeIdToLookup != null && attributeIdToLookup.Count > 0)
//                {
//                    if (attributeIdToLookup.ContainsKey(attributeModel.Id))
//                    {
//                        Lookup lookup = attributeIdToLookup[attributeModel.Id];

//                        if (lookup != null)
//                        {
//                            Int32 valueId = ValueTypeHelper.Int32TryParse(entityHistoryRecord.ChangedData_AttrVal, 0);
//                            entityHistoryRecord.ChangedData_InstanceRefId = valueId;
//                            if (valueId > 0)
//                            {
//                                entityHistoryRecord.ChangedData_AttrVal = lookup.GetDisplayFormatById(valueId);
//                            }
//                            else
//                            {
//                                entityHistoryRecord.ChangedData_AttrVal = String.Empty;
//                            }
//                        }
//                    }
//                }

//                #endregion

//                #region Data formatting for Date, DateTime and Decimal attributes
//                if (!string.IsNullOrEmpty(entityHistoryRecord.ChangedData_AttrVal) && entityHistoryRecord.ChangedData_AttrVal != "NA")
//                {
//                if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("date"))
//                {
//                    entityHistoryRecord.ChangedData_AttrVal = MDM.Core.FormatHelper.FormatDateOnly(entityHistoryRecord.ChangedData_AttrVal, LocaleEnum.en_US.GetCultureName(), base.GetDataFormattingLocale().GetCultureName());
//                }

//                if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("datetime"))
//                {
//                    entityHistoryRecord.ChangedData_AttrVal = MDM.Core.FormatHelper.FormatDate(entityHistoryRecord.ChangedData_AttrVal, LocaleEnum.en_US.GetCultureName(), base.GetDataFormattingLocale().GetCultureName());
//                }

//                if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("decimal"))
//                {
//                    entityHistoryRecord.ChangedData_AttrVal = MDM.Core.FormatHelper.FormatNumber(entityHistoryRecord.ChangedData_AttrVal, base.GetDataFormattingLocale().GetCultureName(), attributeModel.Precision, attributeModel.IsPrecisionArbitrary);
//                }
//                }
//                #endregion

//                #region Update UOM value ID to Uom long name

//                if (uoms != null && entityHistoryRecord.ChangedData_UOMId > 0)
//                {
//                    UOM uom = uoms.GetUOMById(entityHistoryRecord.ChangedData_UOMId);

//                    if (uom != null)
//                    {
//                        entityHistoryRecord.ChangedData_UOM = uom.Name;
//                        entityHistoryRecord.ChangedData_AttrVal += uomSeparator + entityHistoryRecord.ChangedData_UOM;
//                    }
//                }

//                #endregion
//            }
//        }

//        private void FillRelatedEntityDetails(EntityCollection entities, EntityTypeCollection entityTypes, EntityHistoryRecord entityHistoryRecord)
//        {
//            if (entities != null)
//            {
//                IEntity iRelationshipEntity = entities.GetEntity(entityHistoryRecord.ChangedData_RelatedEntityId);

//                if (iRelationshipEntity != null)
//                {
//                    entityHistoryRecord.ChangedData_RelatedEntityLongName = iRelationshipEntity.LongName;
//                    entityHistoryRecord.ChangedData_RelatedEntityTypeId = iRelationshipEntity.EntityTypeId;
//                    entityHistoryRecord.ChangedData_RelatedEntityTypeLongName = iRelationshipEntity.EntityTypeLongName;


//                    if (entityHistoryRecord.ChangeType == EntityChangeType.N_Level_HierarchyRelationship)
//                    {
//                        IEntityType iEntityType = entityTypes.Get(iRelationshipEntity.ParentEntityTypeId);

//                        if (iEntityType != null)
//                        {
//                            entityHistoryRecord.ChangedData_RelatedEntityParentLongName = iRelationshipEntity.ParentEntityLongName;
//                            entityHistoryRecord.ChangedData_RelatedEntityParentId = iRelationshipEntity.ParentEntityId;
//                            entityHistoryRecord.ChangedData_RelatedEntityParentEntityTypeLongName = iEntityType.LongName;
//                        }
//                    }
//                }
//            }
//        }

//        private void FillMetadataDetails(EntityCollection categories, EntityHistoryRecord entityHistoryRecord)
//        {
//            if (categories != null)
//            {
//                IEntity category = categories.GetEntity(entityHistoryRecord.ChangedData_CategoryId);

//                if (category != null)
//                {
//                    entityHistoryRecord.ChangedData_CategoryLongName = category.LongName;
//                    entityHistoryRecord.ChangedData_CategoryLongNamePath = category.CategoryLongNamePath;
//                }
//            }
//        }

//        private void FillRelationshipTypeDetails(RelationshipTypeCollection relationshipTypes, EntityHistoryRecord entityHistoryRecord)
//        {
//            IRelationshipType iRelationshipType = relationshipTypes.GetRelationshipType(entityHistoryRecord.ChangedData_RelationshipTypeId);

//            if (iRelationshipType != null)
//            {
//                entityHistoryRecord.ChangedData_RelationshipTypeLongName = iRelationshipType.LongName;
//            }
//        }

//        private void FillExtensionDetails(ContainerCollection containers, EntityCollection categories, EntityHistoryRecord entityHistoryRecord)
//        {
//            IContainer iExtendedContainer = containers.GetContainer(entityHistoryRecord.ChangedData_ExtensionContainerId);
//            if (iExtendedContainer != null)
//            {
//                entityHistoryRecord.ChangedData_ExtensionContainerLongName = iExtendedContainer.LongName;
//            }

//            if (categories != null)
//            {
//                IEntity iExtendedCategory = categories.GetEntity(entityHistoryRecord.ChangedData_ExtensionCategoryId);
//                if (iExtendedCategory != null)
//                {
//                    entityHistoryRecord.ChangedData_ExtensionCategoryLongName = iExtendedCategory.LongName;
//                    entityHistoryRecord.ChangedData_ExtensionCategoryLongNamePath = iExtendedCategory.CategoryLongNamePath;
//                }
//            }
//        }
        
//        private void FillPreviousValue(Dictionary<String, String> previousRecordValuesDictionary, Dictionary<String, String> previousRecordSourceDictionary, EntityHistoryRecord entityHistoryRecord)
//        {
//            String previousRecordValueKey = GetPreviousRecordValuesDictionaryKey(entityHistoryRecord);
            
//            String currentRecordValue = String.Empty;
//            if (entityHistoryRecord.Action == ObjectAction.Delete && entityHistoryRecord.ChangeType != EntityChangeType.Relationship && entityHistoryRecord.ChangeType != EntityChangeType.ExtensionRelationship && entityHistoryRecord.ChangeType != EntityChangeType.HierarchyRelationship)
//            {
//                currentRecordValue = "[RS_DELETE]";
//            }
//            else
//            {
//                currentRecordValue = GetChangedValueOfHistoryRecord(entityHistoryRecord);
//            }
//            String currentRecordSource = (entityHistoryRecord == null) ? "" :  entityHistoryRecord.Source;

//            if (previousRecordValuesDictionary.ContainsKey(previousRecordValueKey))
//            {
//                entityHistoryRecord.PreviousVal = previousRecordValuesDictionary[previousRecordValueKey];
               
//                //Update dictionary with current value which acts as previous value for next history record
//                previousRecordValuesDictionary[previousRecordValueKey] = currentRecordValue;
                
//            }
//            else
//            {
//                if (entityHistoryRecord.ChangeType == EntityChangeType.Metadata)
//                {
//                    if (!previousRecordValuesDictionary.ContainsKey("Metadata_LongName"))
//                    {
//                        previousRecordValuesDictionary.Add("Metadata_LongName", entityHistoryRecord.ChangedData_EntityLongName);
//                    }

//                    if (!previousRecordValuesDictionary.ContainsKey("Metadata_Category"))
//                    {
//                        previousRecordValuesDictionary.Add("Metadata_Category", entityHistoryRecord.ChangedData_CategoryLongNamePath);
//                    }
//                }
//                else
//                {
//                    //Add current value
//                    previousRecordValuesDictionary.Add(previousRecordValueKey, currentRecordValue);
//                }
//            }

//            if(previousRecordSourceDictionary.ContainsKey(previousRecordValueKey))
//            {
//                entityHistoryRecord.PreviousSource = previousRecordSourceDictionary[previousRecordValueKey];

//                //Update dictionary with current source which acts as previous source for next history record
//                previousRecordSourceDictionary[previousRecordValueKey] = currentRecordSource;
//            }
//            else
//            {
//                previousRecordSourceDictionary.Add(previousRecordValueKey, currentRecordSource);
//            }
//        }

//        private void FillWFActivityActionData(EntityHistoryRecord entityHistoryRecord)
//        {
//            if (!entityHistoryRecord.WorkflowPreviousAssignedUser.Equals("NA") && !String.IsNullOrWhiteSpace(entityHistoryRecord.WorkflowPreviousAssignedUser) && (entityHistoryRecord.WorkflowCurrentAssignedUser.Equals("NA") || String.IsNullOrWhiteSpace(entityHistoryRecord.WorkflowCurrentAssignedUser)))
//            {
//                // If Previous assigned user is available but current assigned user is not available then action taken is - Release Ownership
//                entityHistoryRecord.WorkflowActivityActionTaken = "Release Ownership";
//            }
//            else if (!entityHistoryRecord.WorkflowPreviousAssignedUser.Equals("NA") && !String.IsNullOrWhiteSpace(entityHistoryRecord.WorkflowPreviousAssignedUser) && !entityHistoryRecord.WorkflowCurrentAssignedUser.Equals("NA") && !String.IsNullOrWhiteSpace(entityHistoryRecord.WorkflowCurrentAssignedUser))
//            {
//                // If Previous assigned user is available and current assigned user is also available then action taken is - Change Ownership
//                entityHistoryRecord.WorkflowActivityActionTaken = "Change Ownership";
//            }
//            else if ((entityHistoryRecord.WorkflowPreviousAssignedUser.Equals("NA") || String.IsNullOrWhiteSpace(entityHistoryRecord.WorkflowPreviousAssignedUser)) && !entityHistoryRecord.WorkflowCurrentAssignedUser.Equals("NA") && !String.IsNullOrWhiteSpace(entityHistoryRecord.WorkflowCurrentAssignedUser))
//            {
//                // If Previous assigned user is not available but current assigned user is available then action taken is - Take Ownership
//                entityHistoryRecord.WorkflowActivityActionTaken = "Take Ownership";
//            }
//        }

//        private Boolean UpdatePreviousRecordValuesDictionary(Dictionary<String, String> previousRecordValuesDictionary, EntityHistoryRecord entityHistoryRecord)
//        {
//            Boolean isUpdateSuccessful = false;

//            String previousRecordValueKey = GetPreviousRecordValuesDictionaryKey(entityHistoryRecord);
//            String currentRecordValue = GetChangedValueOfHistoryRecord(entityHistoryRecord);

//            if (previousRecordValuesDictionary.ContainsKey(previousRecordValueKey))
//            {
//                //Update dictionary with current value which acts as previous value for next history record
//                previousRecordValuesDictionary[previousRecordValueKey] = currentRecordValue;
//            }

//            return isUpdateSuccessful;
//        }

//        private String GetPreviousRecordValuesDictionaryKey(EntityHistoryRecord entityHistoryRecord)
//        {
//            String key = String.Empty;

//            switch (entityHistoryRecord.ChangeType)
//            {
//                case EntityChangeType.Metadata_LongName:
//                    key = "Metadata_LongName";
//                    break;
//                case EntityChangeType.Metadata_Category:
//                    key = "Metadata_Category";
//                    break;
//                case EntityChangeType.CommonAttribute:
//                case EntityChangeType.CommonComplexAttribute:
//                case EntityChangeType.TechnicalAttribute:
//                case EntityChangeType.TechnicalComplexAttribute:
//                case EntityChangeType.TechnicalHierarchicalAttribute:
//                case EntityChangeType.CommonHierarchicalAttribute:
//                case EntityChangeType.CommonFileOrImageAttribute:
//                case EntityChangeType.CommonURLAttribute:
//                case EntityChangeType.TechnicalFileOrImageAttribute:
//                case EntityChangeType.TechnicalURLAttribute:
//                    key = String.Format("Attribute_Id{0}_L{1}", entityHistoryRecord.ChangedData_AttributeId, entityHistoryRecord.Locale.ToString());
//                    break;
//                case EntityChangeType.RelationshipAttribute:
//                    key = String.Format("Attribute_Id{0}_RelTypeId{1}_RelEntityId{2}_L{3}", entityHistoryRecord.ChangedData_AttributeId, entityHistoryRecord.ChangedData_RelationshipTypeId, entityHistoryRecord.ChangedData_RelatedEntityId, entityHistoryRecord.Locale.ToString());
//                    break;
//            }

//            return key;
//        }

//        private String GetChangedValueOfHistoryRecord(EntityHistoryRecord entityHistoryRecord)
//        {
//            String value = String.Empty;

//            switch (entityHistoryRecord.ChangeType)
//            {
//                case EntityChangeType.Metadata_LongName:
//                    value = entityHistoryRecord.ChangedData_EntityLongName;
//                    break;
//                case EntityChangeType.Metadata_Category:
//                    value = entityHistoryRecord.ChangedData_CategoryLongNamePath;
//                    break;
//                case EntityChangeType.CommonAttribute:
//                case EntityChangeType.TechnicalAttribute:
//                case EntityChangeType.RelationshipAttribute:
//                case EntityChangeType.CommonFileOrImageAttribute:
//                case EntityChangeType.CommonURLAttribute:
//                case EntityChangeType.TechnicalFileOrImageAttribute:
//                case EntityChangeType.TechnicalURLAttribute:
//                    value = entityHistoryRecord.ChangedData_AttrVal;
//                    break;
//                case EntityChangeType.CommonComplexAttribute:
//                case EntityChangeType.TechnicalComplexAttribute:
//                case EntityChangeType.CommonHierarchicalAttribute:
//                case EntityChangeType.TechnicalHierarchicalAttribute:
//                    value = entityHistoryRecord.AuditRefId.ToString();
//                    break;
//            }

//            return value;
//        }

//        /// <summary>
//        /// Merges multiple entity history record vales to return single record. 
//        /// In case of attribute related changes if attribute is collection entity history should contain single record with multiple value. So to merge that this method helps.
//        /// </summary>
//        /// <param name="entityHistory"></param>
//        /// <param name="currentEntityHistoryRecord"></param>
//        /// <param name="mergedEntityHistoryRecords"></param>
//        /// <param name="collectionValueSeparator"></param>
//        /// <param name="entityHistoryDetailsTemplates"></param>
//        /// <param name="attributeModel"></param>
//        /// <param name="previousRecordValuesDictionary"></param>
//        /// <returns></returns>
//        private Boolean MergeAttributeValues(EntityHistory entityHistory, EntityHistoryRecord currentEntityHistoryRecord, Collection<EntityHistoryRecord> mergedEntityHistoryRecords, String collectionValueSeparator, EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates, AttributeModel attributeModel, Dictionary<String, String> previousRecordValuesDictionary)
//        {
//            Boolean addHistoryRecordInFinalResult = false;

//            EntityHistoryRecord resultEntityHistoryRecord = null;

//            if (currentEntityHistoryRecord.ChangeType == EntityChangeType.RelationshipAttribute)
//            {
//                resultEntityHistoryRecord = GetEntityHistoryRecord(mergedEntityHistoryRecords, currentEntityHistoryRecord.AuditRefId, currentEntityHistoryRecord.ChangedData_AttributeId, currentEntityHistoryRecord.ChangedData_RelatedEntityId);                
//            }
//            else
//            {
//                resultEntityHistoryRecord = GetEntityHistoryRecord(mergedEntityHistoryRecords, currentEntityHistoryRecord.AuditRefId, currentEntityHistoryRecord.ChangedData_AttributeId, 0);
//            }

//            if (resultEntityHistoryRecord != null)
//            {
//                if (String.IsNullOrWhiteSpace(resultEntityHistoryRecord.ChangedData_AttrVal))
//                {
//                    resultEntityHistoryRecord.ChangedData_AttrVal = currentEntityHistoryRecord.ChangedData_AttrVal;
//                }
//                else
//                {
//                    if (!String.IsNullOrWhiteSpace(currentEntityHistoryRecord.ChangedData_AttrVal))
//                    {
//                        resultEntityHistoryRecord.ChangedData_AttrVal += collectionValueSeparator + currentEntityHistoryRecord.ChangedData_AttrVal;
//                    }
//                }

//                UpdatePreviousRecordValuesDictionary(previousRecordValuesDictionary, resultEntityHistoryRecord);

//                FillTemplateDetails(entityHistory, resultEntityHistoryRecord, entityHistoryDetailsTemplates, attributeModel);
//            }
//            else
//            {
//                addHistoryRecordInFinalResult = true;
//            }

//            return addHistoryRecordInFinalResult;
//        }

//        #endregion

//        #region Helper methods to Get models

//        private EntityCollection GetEntities(Collection<Int64> entityIds, EntityHistoryContext entityHistoryContext, Boolean isCategory, CallerContext callerContext)
//        {
//            if (entityIds.Count > 0)
//            {
//                EntityBL entityBL = new EntityBL();
//                EntityContext entityContext = new EntityContext();
//                entityContext.Locale = base.GetModelDisplayLocale();
//                entityContext.ContainerId = entityHistoryContext.ContainerId;
//                entityContext.IgnoreEntityStatus = true;

//                if (isCategory)
//                {
//                    entityContext.EntityTypeId = 6;
//                }

//                return entityBL.Get(entityIds, entityContext, false, callerContext.Application, callerContext.Module, false);
//            }
//            else
//                return null;
//        }

//        private ContainerCollection GetAllContainers(CallerContext callerContext)
//        {
//            ContainerBL containerBL = new ContainerBL();
//            return containerBL.GetAll(callerContext);
//        }

//        private EntityTypeCollection GetAllEntityTypes(CallerContext callerContext)
//        {
//            EntityTypeBL entityTypeBL = new EntityTypeBL();
//            return entityTypeBL.GetAll(callerContext);
//        }

//        private RelationshipTypeCollection GetAllRelationshipTypes(CallerContext callerContext)
//        {
//            RelationshipTypeBL relationshipTypeBL = new RelationshipTypeBL();
//            return relationshipTypeBL.GetAll(callerContext);
//        }

//        private UOMCollection GetAllUOMs(CallerContext callerContext)
//        {
//            UomManager.UomBL uomBL = new UomManager.UomBL();
//            return uomBL.GetAllUoms(new UomContext(), callerContext);
//        }

//        private AttributeModelCollection GetAttributeModels(Collection<Int32> attributeIds)
//        {
//            if (attributeIds.Count > 0)
//            {
//                AttributeModelBL attributeModelBL = new AttributeModelBL();

//                Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
//                LocaleEnum locale = base.GetModelDisplayLocale();
//                locales.Add(locale);

//                AttributeModelContext attributeModelContext = new AttributeModelContext();
//                attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
//                attributeModelContext.Locales = locales;

//                return attributeModelBL.GetByIds(attributeIds, attributeModelContext);
//            }
//            else
//                return null;
//        }

//        private Dictionary<Int32, Lookup> GetAttributeIdToLookup(AttributeModelCollection attributeModels, Dictionary<Int32, String> attributeIdsToValues, CallerContext callerContext)
//        {
//            Dictionary<Int32, Lookup> attributeIdToLookup = new Dictionary<Int32, Lookup>();

//            foreach (AttributeModel attributeModel in attributeModels)
//            {
//                if (attributeModel.IsLookup)
//                {
//                    if (attributeIdsToValues.ContainsKey(attributeModel.Id))
//                    {
//                        String attrValues = attributeIdsToValues[attributeModel.Id];
//                        Collection<Int32> valueIds = ValueTypeHelper.SplitStringToIntCollection(attrValues, ',');

//                        LookupBL lookupBL = new LookupBL();
//                        Lookup lookup = lookupBL.Get(attributeModel.Id, base.GetModelDisplayLocale(), -1, valueIds, new MDM.BusinessObjects.ApplicationContext(), callerContext, false);

//                        if (lookup != null)
//                        {
//                            attributeIdToLookup.Add(attributeModel.Id, lookup);
//                        }
//                        else
//                        {
//                            String errorMessage = this.GetSystemLocaleMessage("112816", callerContext).Message;
//                            throw new MDMOperationException("112816", errorMessage, "EntityManager.EntityHistoryBL.Get", String.Empty, String.Empty);
//                        }
//                    }
//                }
//            }

//            return attributeIdToLookup;
//        }

//        private EntityHistoryRecord GetEntityHistoryRecord(Collection<EntityHistoryRecord> entityHistoryRecords, Int64 auditRefId, Int32 modifiedAttributeId, Int64 relatedEntityId)
//        {
//            EntityHistoryRecord entityHistoryRecord = null;

//            if (entityHistoryRecords != null)
//            {
//                if (relatedEntityId > 0)
//                {
//                    foreach (EntityHistoryRecord record in entityHistoryRecords)
//                    {
//                        if (record.AuditRefId == auditRefId && record.ChangedData_AttributeId == modifiedAttributeId && record.ChangedData_RelatedEntityId == relatedEntityId)
//                        {
//                            entityHistoryRecord = record;
//                            break;
//                        }
//                    }
//                }
//                else
//                {
//                    foreach (EntityHistoryRecord record in entityHistoryRecords)
//                    {
//                        if (record.AuditRefId == auditRefId && record.ChangedData_AttributeId == modifiedAttributeId)
//                        {
//                            entityHistoryRecord = record;
//                            break;
//                        }
//                    }
//                }
//            }

//            return entityHistoryRecord;
//        }

//        #endregion

//        #region Helper methods for Parameter Validation

//        /// <summary>
//        /// Returns Message Object based on message code
//        /// </summary>
//        /// <param name="messageCode">Message Code</param>
//        /// <param name="callerContext">caller context indicating who called the API</param>
//        /// <returns></returns>
//        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
//        {
//            return _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
//        }

//        private void ValidateCallerContext(CallerContext callerContext, String methodName)
//        {
//            String errorMessage = String.Empty;

//            if (callerContext == null)
//            {
//                errorMessage = this.GetSystemLocaleMessage("111846", callerContext).Message;
//                throw new MDMOperationException("111846", errorMessage, "EntityManager.EntityHistoryBL." + methodName, String.Empty, methodName);
//            }
//        }

//        private void ValidateEntityHistoryContext(EntityHistoryContext entityHistoryContext, CallerContext callerContext, String methodName)
//        {
//            String errorMessage = String.Empty;

//            if (entityHistoryContext == null)
//            {
//                errorMessage = this.GetSystemLocaleMessage("112792", callerContext).Message;
//                throw new MDMOperationException("112792", errorMessage, "DataModelManager.EntityTypeBL." + methodName, String.Empty, String.Empty);
//            }

//            if (entityHistoryContext.ContainerId < 1)
//            {
//                errorMessage = this.GetSystemLocaleMessage("112793", callerContext).Message;
//                throw new MDMOperationException("112793", errorMessage, "DataModelManager.EntityTypeBL." + methodName, String.Empty, String.Empty);
//            }
//        }

//        private void ValidateEntityIds(Collection<Int64> entityIds, CallerContext callerContext, String methodName)
//        {
//            String errorMessage = String.Empty;

//            if (entityIds == null || entityIds.Count < 1)
//            {
//                errorMessage = this.GetSystemLocaleMessage("112557", callerContext).Message;
//                throw new MDMOperationException("112557", errorMessage, "EntityManager.EntityHistoryBL." + methodName, String.Empty, methodName);
//            }
//            else
//            {
//                foreach (Int64 entityId in entityIds)
//                {
//                    if (entityId < 1)
//                    {
//                        errorMessage = this.GetSystemLocaleMessage("111645", callerContext).Message;
//                        throw new MDMOperationException("111645", errorMessage, "EntityManager.EntityHistoryBL." + methodName, String.Empty, methodName);
//                    }
//                }
//            }
//        }

//        #endregion

//        #region Helper methods for Entity History Templates

//        private void FillEntityHistoryTemplates(LocaleEnum locale, EntityHistoryDetailsTemplateCollection entityHistoryTemplates, CallerContext callerContext)
//        {
//            if (entityHistoryTemplates != null && entityHistoryTemplates.Count > 0)
//            {
//                foreach (EntityHistoryDetailsTemplate entityHistoryDetailTemplate in entityHistoryTemplates)
//                {
//                    LocaleMessage localeMessage = null;
//                    String templateMessage = String.Empty;

//                    localeMessage = _localeMessageBL.Get(locale, entityHistoryDetailTemplate.TemplateCode, true, callerContext);

//                    if (localeMessage != null)
//                    {
//                        templateMessage = localeMessage.Message;
//                    }

//                    entityHistoryDetailTemplate.TemplateText = templateMessage;
//                    entityHistoryDetailTemplate.Locale = locale;
//                }
//            }
//        }

//        private void FillTemplateDetails(EntityHistory entityHistory, EntityHistoryRecord entityHistoryRecord, EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates, AttributeModel attributeModel)
//        {
//            String templateText = String.Empty;

//            if (entityHistoryDetailsTemplates != null)
//            {
//                EntityChangeType changeType = EntityChangeType.Unknown;

//                if (entityHistoryRecord.ChangeType == EntityChangeType.TechnicalFileOrImageAttribute || entityHistoryRecord.ChangeType == EntityChangeType.TechnicalURLAttribute)
//                {
//                    changeType = EntityChangeType.TechnicalAttribute;
//                }
//                else if (entityHistoryRecord.ChangeType == EntityChangeType.CommonFileOrImageAttribute || entityHistoryRecord.ChangeType == EntityChangeType.CommonURLAttribute)
//                {
//                    changeType = EntityChangeType.CommonAttribute;
//                }
//                else if (entityHistoryRecord.ChangeType == EntityChangeType.Promote)
//                {
//                    if (String.Compare(entityHistoryRecord.PromoteMessageCode, "114280") == 0)
//                    {
//                        changeType = EntityChangeType.Promote_Workflow;
//                    }
//                    else if (String.Compare(entityHistoryRecord.PromoteMessageCode, "114281") == 0)
//                    {
//                        changeType = EntityChangeType.Promote_DDG;
//                    }
//                    else if (String.Compare(entityHistoryRecord.PromoteMessageCode, "114287") == 0)
//                    {
//                        changeType = EntityChangeType.PromoteQualificationFailure;
//                    }
//                }
//                else
//                {
//                    changeType = entityHistoryRecord.ChangeType;
//                }

//                EntityHistoryDetailsTemplate entityHistoryDetailsTemplate = null;
//                if (String.Compare(entityHistoryRecord.PreviousVal, "[RS_DELETE]", StringComparison.InvariantCultureIgnoreCase) == 0 )
//                {
//                    entityHistoryRecord.Action = ObjectAction.Create;
//                }

//                entityHistoryDetailsTemplate = entityHistoryDetailsTemplates.GetEntityHistoryDetailsTemplate(changeType, entityHistoryRecord.Action);

//                if (entityHistoryDetailsTemplate != null)
//                {
//                    templateText = entityHistoryDetailsTemplate.TemplateText;

//                    if (entityHistoryRecord.ChangeType != EntityChangeType.TechnicalFileOrImageAttribute && entityHistoryRecord.ChangeType != EntityChangeType.TechnicalURLAttribute && entityHistoryRecord.ChangeType != EntityChangeType.CommonFileOrImageAttribute && entityHistoryRecord.ChangeType != EntityChangeType.CommonURLAttribute)
//                    {
//                        templateText = templateText.Replace("##AttributeValue##", entityHistoryRecord.ChangedData_AttrVal);
//                        templateText = templateText.Replace("##OldValue##", entityHistoryRecord.PreviousVal);
//                    }

//                    templateText = templateText.Replace("##Source##", entityHistoryRecord.Source);
//                    templateText = templateText.Replace("##PreviousSource##", entityHistoryRecord.PreviousSource);
//                    templateText = templateText.Replace("##AttributeLongName##", entityHistoryRecord.ChangedData_AttributeLongName);
//                    templateText = templateText.Replace("##AttributeParentLongName##", entityHistoryRecord.ChangedData_AttributeParentLongName);
//                    templateText = templateText.Replace("##RelationshipTypeLongName##", entityHistoryRecord.ChangedData_RelationshipTypeLongName);
//                    templateText = templateText.Replace("##RelatedEntityTypeLongName##", entityHistoryRecord.ChangedData_RelatedEntityTypeLongName);
//                    templateText = templateText.Replace("##RelatedEntityLongName##", entityHistoryRecord.ChangedData_RelatedEntityLongName);
//                    templateText = templateText.Replace("##EntityTypeLongName##", entityHistory.EntityTypeLongName);

//                    Boolean isSourceTrackingEnabled = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.DataQualityManagement, "Entity data source tracking", "1", false);
//                    if (!isSourceTrackingEnabled)
//                    {
//                        //Remove the source and previous sources from template text.
//                        templateText = ModifyTemplateText(templateText, "@@", "@@");
//                    }
//                    else
//                    {
//                        //Remove the extra @@ from the template text
//                        templateText = templateText.Replace("@@", "");
//                    }

//                    String entityLongName = "NA";
//                    if (entityHistoryRecord.ChangeType == EntityChangeType.Metadata_LongName || entityHistoryRecord.ChangeType == EntityChangeType.Metadata || entityHistoryRecord.ChangeType == EntityChangeType.Metadata_Category)
//                    {
//                        entityLongName = entityHistoryRecord.ChangedData_EntityLongName;
//                    }
//                    else
//                    {
//                        entityLongName = entityHistory.EntityLongName;
//                    }

//                    templateText = templateText.Replace("##EntityLongName##", entityLongName);
//                    templateText = templateText.Replace("##EntityCategoryLongNamePath##", entityHistoryRecord.ChangedData_CategoryLongNamePath);
//                    templateText = templateText.Replace("##EntityCatalogLongName##", entityHistory.EntityCatalogLongName);
//                    templateText = templateText.Replace("##RelatedEntityCatalogLongName##", entityHistoryRecord.ChangedData_ExtensionContainerLongName);
//                    templateText = templateText.Replace("##RelatedEntityCategoryLongNamePath##", entityHistoryRecord.ChangedData_ExtensionCategoryLongNamePath);
//                    templateText = templateText.Replace("##N_Level_RelatedEntityTypeLongName##", entityHistoryRecord.ChangedData_RelatedEntityTypeLongName);
//                    templateText = templateText.Replace("##N_Level_RelatedEntityLongName##", entityHistoryRecord.ChangedData_RelatedEntityLongName);
//                    templateText = templateText.Replace("##(N-1)_Level_RelatedEntityTypeLongName##", entityHistoryRecord.ChangedData_RelatedEntityParentEntityTypeLongName);
//                    templateText = templateText.Replace("##(N-1)_Level_RelatedEntityLongName##", entityHistoryRecord.ChangedData_RelatedEntityParentLongName);
//                    templateText = templateText.Replace("##WorkflowLongName##", entityHistoryRecord.WorkflowName);
//                    templateText = templateText.Replace("##WorkflowVersionLongName##", entityHistoryRecord.WorkflowVersionName);
//                    templateText = templateText.Replace("##ActivityLongName##", entityHistoryRecord.WorkflowActivityLongName);
//                    templateText = templateText.Replace("##PerformedAction##", entityHistoryRecord.WorkflowActivityActionTaken);
//                    templateText = templateText.Replace("##Comments##", entityHistoryRecord.WorkflowComments);
//                    templateText = templateText.Replace("##PreviousAssignedUser##", entityHistoryRecord.WorkflowPreviousAssignedUser);
//                    templateText = templateText.Replace("##CurrentAssignedUser##", entityHistoryRecord.WorkflowCurrentAssignedUser);

//                    if (changeType == EntityChangeType.AutoPromote || changeType == EntityChangeType.EmergencyPromote)
//                    {
//                        templateText = templateText.Replace("##PromoteAttributes##", entityHistoryRecord.PromotedAttributesString);
//                    }
//                    else if (changeType == EntityChangeType.Promote_Workflow || changeType == EntityChangeType.Promote_DDG || changeType == EntityChangeType.PromoteQualificationFailure) //Promote by workflow or BR
//                    {
//                        templateText = templateText.Replace("##PromoteRootEntityTypeLongName##", entityHistoryRecord.PromoteRootEntityTypeLongName);
//                        templateText = templateText.Replace("##PromoteRootEntityLongName##", entityHistoryRecord.PromotedRootEntityLongName);
//                        String[] messageParams = entityHistoryRecord.PromoteMessageParams.Split(new String[] { "#@#" }, StringSplitOptions.None);

//                        if (changeType == EntityChangeType.PromoteQualificationFailure)
//                        {
//                            templateText = String.Format(templateText, messageParams[1]);
//                        }
//                        else
//                        {
//                            templateText = String.Format(templateText, messageParams[0], messageParams[1]);
//                        }
//                    }
//                }
//            }

//            entityHistoryRecord.Details = templateText;
//        }

//        #endregion

//        #region SendMail Helper Methods

//        private void SendMail(Collection<String> toAddresses, Collection<String> ccAddresses, String messageSubject, String messageBody, BOFile file, CallerContext callerContext, MailPriority mailPriority = MailPriority.Normal)
//        {

//            MailMessage message = new MailMessage();

//            //Convert addresses into comma separated values
//            String toAddressesString = ValueTypeHelper.JoinCollection(toAddresses, ",");
//            message.To.Add(toAddressesString);

//            if (ccAddresses != null && ccAddresses.Count > 0)
//            {
//                String ccAddressesString = ValueTypeHelper.JoinCollection(ccAddresses, ",");

//                message.CC.Add(ccAddressesString);
//            }

//            message.IsBodyHtml = true;
//            message.Subject = messageSubject;
//            message.Body = messageBody;
//            message.Priority = mailPriority;

//            if (file != null)
//            {
//                Stream st = new MemoryStream(file.FileData);
//                String fileName = file.Name;
//                message.Attachments.Add(new Attachment(st, fileName));
//            }

//            MailConfigBL mailConfigBL = new MailConfigBL();
//            MailConfig mailConfig = mailConfigBL.Get(callerContext);

//            using (SmtpClient smtpClient = new SmtpClient())
//            {
//                if (mailConfig != null && !String.IsNullOrWhiteSpace(mailConfig.From) && !String.IsNullOrWhiteSpace(mailConfig.UserName))
//                {
//                    message.From = new MailAddress(mailConfig.From);

//                    smtpClient.Host = mailConfig.Host;
//                    smtpClient.Port = mailConfig.Port;
//                    smtpClient.EnableSsl = mailConfig.EnableSSL;
//                    smtpClient.UseDefaultCredentials = false;
//                    smtpClient.Credentials = new NetworkCredential(mailConfig.UserName, mailConfig.Password);
//                }
//                try
//                {
//                    smtpClient.Send(message);
//                }
//                catch
//                {
//                    throw new System.Net.Mail.SmtpException();
//                }
//            }
//        }

//        private Collection<String> ValidateAndFilterMailAddresses(Collection<String> emailIdLists, OperationResult operationresult, CallerContext callerContext, String addressParameterName)
//        {
//            //Logic:
//            //If email address is valid then will add to result mail list.
//            //

//            Collection<String> validMailIds = new Collection<String>();
//            String errorMessage = String.Empty;
//            Collection<Object> parameter = null;

//            if (emailIdLists != null && emailIdLists.Count > 0)
//            {
//                foreach (String email in emailIdLists)
//                {
//                    try
//                    {
//                        MailAddress address = new MailAddress(email);   // Validate Email address

//                        validMailIds.Add(email);
//                    }
//                    catch (FormatException) //If there is a format exception then email address is not a valid format.
//                    {
//                        parameter = new Collection<Object>();
//                        parameter.Add(email);
//                        parameter.Add(addressParameterName);

//                        errorMessage = this.GetSystemLocaleMessage("111968", callerContext).Message;

//                        operationresult.AddOperationResult("111968", errorMessage, parameter, OperationResultType.Warning); //Mail Id '{0}' provided in '{1}' list is not valid and hence ignored.
//                    }
//                }
//            }

//            return validMailIds;
//        }

//        /// <summary>
//        /// Returns Message Object based on message code
//        /// </summary>
//        /// <param name="messageCode">Message Code</param>
//        /// <param name="locale">Indicates teh locale</param>
//        /// <param name="callerContext">caller context indicating who called the API</param>
//        /// <param name="param">Indicates the parameters</param>
//        /// <returns></returns>
//        private LocaleMessage GetSystemLocaleMessage(String messageCode, LocaleEnum locale, CallerContext callerContext, Object[] param)
//        {
//            return _localeMessageBL.Get(locale, messageCode, param, false, callerContext);
//        }

//        #endregion

//        #region Helper methods for Exclude list functionality

//        private EntityCollectionHistory GetEntitiesHistory(EntityCollection entities, EntityHistoryContext entityHistoryContext, DBCommandProperties command, CallerContext callerContext)
//        {
//            EntityCollectionHistory entityCollectionHistory = null;

//            if (entities != null)
//            {
//                Int32 numberOfEntitiesThreads = 1;
//                Int32 numberofEntities = entities.Count;
//                Int32 bulkEntityHistoryGetBatchSize = 1;

//                if (numberofEntities > 1)
//                {
//                    #region Bulk Entity History Get

//                    //Create parallel get here with configured batch size. Assuming a default of 3 tasks
//                    numberOfEntitiesThreads = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.EntityManager.ParallelEntityHistoryGet.ThreadPoolSize", 3);
//                    bulkEntityHistoryGetBatchSize = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.EntityManager.BulkEntityHistoryGet.BatchSize", 1);

//                    if (Constants.TRACING_ENABLED)
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "App config key 'MDMCenter.EntityManager.ParallelEntityHistoryGet.ThreadPoolSize' is set to: " + numberOfEntitiesThreads, MDMTraceSource.Entity);
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "App config key 'MDMCenter.EntityManager.BulkEntityHistoryGet.BatchSize' is set to: " + bulkEntityHistoryGetBatchSize, MDMTraceSource.Entity);
//                    }

//                    //Reconfigure the thread count based on the batches required.
//                    Int32 noOfBatchesRequired = numberofEntities / bulkEntityHistoryGetBatchSize;

//                    if (numberofEntities % bulkEntityHistoryGetBatchSize > 0)
//                        noOfBatchesRequired++;

//                    numberOfEntitiesThreads = Math.Min(numberOfEntitiesThreads, noOfBatchesRequired);

//                    if (Constants.TRACING_ENABLED)
//                    {
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting entity batch creation...", MDMTraceSource.Entity);
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of threads to be created: " + numberOfEntitiesThreads, MDMTraceSource.Entity);
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of entities to get: " + numberofEntities, MDMTraceSource.Entity);
//                    }

//                    if (numberOfEntitiesThreads > 1)
//                    {
//                        #region Parallel Get

//                        //Equally distribute the available entities across the available tasks..The left over is given to the last one
//                        Int32 entitybatchPerThread = (numberofEntities) / numberOfEntitiesThreads;

//                        if (Constants.TRACING_ENABLED)
//                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of entities per thread: " + entitybatchPerThread, MDMTraceSource.Entity);

//                        // Split the entities..This is the same logic used in Import engine...Need to move this to a UTILITY method..
//                        Dictionary<Int32, Int32> entitySplit = SplitEntiesForThreads(numberOfEntitiesThreads, 0, numberofEntities - 1, entitybatchPerThread);

//                        if (Constants.TRACING_ENABLED)
//                        {
//                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with entity batch creation.", MDMTraceSource.Entity);
//                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting entity history parallel get...", MDMTraceSource.Entity);
//                        }

//                        SM.OperationContext operationContext = SM.OperationContext.Current;

//                        // Simple parallel..loop
//                        Parallel.For(0, numberOfEntitiesThreads, i =>
//                        {
//                            SM.OperationContext.Current = operationContext;

//                            for (Int32 j = entitySplit[i]; j < entitySplit[i + 1]; j++)
//                            {
//                                EntityCollectionHistory currentEntityCollectionHistory = GetEntityHistory(entities.ElementAt(j), entityHistoryContext, command, callerContext);

//                                if (currentEntityCollectionHistory != null)
//                                {
//                                    lock (entityHistoryGetLockObject)
//                                    {
//                                        if (entityCollectionHistory != null)
//                                        {
//                                            entityCollectionHistory.AddRange(currentEntityCollectionHistory);
//                                        }
//                                        else
//                                        {
//                                            entityCollectionHistory = currentEntityCollectionHistory;
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        );

//                        #endregion
//                    }
//                    else if (numberOfEntitiesThreads == 1)
//                    {
//                        foreach (Entity entity in entities)
//                        {
//                            EntityCollectionHistory currentEntityCollectionHistory = GetEntityHistory(entity, entityHistoryContext, command, callerContext);

//                            if (currentEntityCollectionHistory != null)
//                            {
//                                if (entityCollectionHistory != null)
//                                {
//                                    entityCollectionHistory.AddRange(currentEntityCollectionHistory);
//                                }
//                                else
//                                {
//                                    entityCollectionHistory = currentEntityCollectionHistory;
//                                }
//                            }
//                        }
//                    }

//                    #endregion
//                }
//                else if (numberofEntities == 1)
//                {
//                    entityCollectionHistory = GetEntityHistory(entities.ElementAt(0), entityHistoryContext, command, callerContext);
//                }
//            }

//            return entityCollectionHistory;
//        }

//        private EntityCollectionHistory GetEntityHistory(Entity entity, EntityHistoryContext entityHistoryContext, DBCommandProperties command, CallerContext callerContext)
//        {
//            EntityCollectionHistory entityCollectionHistory = null;
//            ICache cache = CacheFactory.GetCache();
//            EntityHistoryExcludeListConfig entityHistoryExcludeListConfig = null;

//            String excludeAttributeIds = String.Empty;
//            String excludeExtensionRelationshipContainerIds = String.Empty;
//            String excludeRelationshipTypeIds = String.Empty;
//            String excludeWorkflowIds = String.Empty;
//            String excludeChildEntityTypeIds = String.Empty;

//            List<KeyValuePair<Int32, Int32>> excludeRelationshipTypeIdToAttributeList = new List<KeyValuePair<Int32, Int32>>();

//            #region Get Application config for exclude list

//            String cacheKey = CacheKeyGenerator.GetEntityHistoryExcludeListConfigCacheKey(
//                                                _securityPrincipal.UserPreferences.DefaultRoleId,
//                                                _securityPrincipal.CurrentUserId,
//                                                entity.OrganizationId,
//                                                entity.ContainerId,
//                                                entity.CategoryId,
//                                                entity.EntityTypeId,
//                                                entity.Id);

//            if (cache != null)
//            {
//                entityHistoryExcludeListConfig = (EntityHistoryExcludeListConfig)cache.Get(cacheKey);
//            }

//            if (entityHistoryExcludeListConfig == null)
//            {
//                Int32 eventSourceId = (Int32)EventSource.EntityHistoryView;
//                Int32 eventSubscriberId = (Int32)EventSubscriber.EntityHistoryViewPageExcludeList;

//                if (callerContext.Application == MDMCenterApplication.VendorPortal)
//                {
//                    eventSourceId = (Int32)EventSource.VPEntityHistoryView;
//                    eventSubscriberId = (Int32)EventSubscriber.VPEntityHistoryViewPageExcludeList;
//                }

//                ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration(
//                                                        eventSourceId,
//                                                        eventSubscriberId,
//                                                        _securityPrincipal.UserPreferences.DefaultRoleId,
//                                                        _securityPrincipal.CurrentUserId,
//                                                        entity.OrganizationId,
//                                                        entity.ContainerId,
//                                                        entity.CategoryId,
//                                                        entity.Id,//EntityId
//                                                        0,//AttributeId
//                                                        entity.EntityTypeId,
//                                                        0,//RelationshipTypeId
//                                                        0,//LoacaleId
//                                                        0);//ApplicationConfigId

//                applicationConfiguration.GetConfigurations();

//                if (Constants.TRACING_ENABLED)
//                {
//                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Application Config get API call completed.", MDMTraceSource.Entity);
//                }

//                entityHistoryExcludeListConfig = (EntityHistoryExcludeListConfig)applicationConfiguration.GetObject("EntityHistoryViewPage-ExcludeList");

//                if (entityHistoryExcludeListConfig != null)
//                {
//                    if (Constants.TRACING_ENABLED)
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Caching of EntityHistoryViewPage-ExcludeList started...", MDMTraceSource.Entity);

//                    cache.Set(cacheKey, entityHistoryExcludeListConfig, DateTime.Now.AddDays(10));

//                    if (Constants.TRACING_ENABLED)
//                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Caching of EntityHistoryViewPage-ExcludeList completed.", MDMTraceSource.Entity);
//                }
//            }

//            #endregion

//            #region Read all exclude list parameters and values(Values)

//            if (entityHistoryExcludeListConfig != null && entityHistoryExcludeListConfig.EntityHistoryExcludeElements != null && entityHistoryExcludeListConfig.EntityHistoryExcludeElements.Count > 0)
//            {
//                foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in entityHistoryExcludeListConfig.EntityHistoryExcludeElements)
//                {
//                    if (entityHistoryExcludeElement != null && entityHistoryExcludeElement.EntityHistoryExcludeSubElements != null && entityHistoryExcludeElement.EntityHistoryExcludeSubElements.Count > 0)
//                    {
//                        if (entityHistoryExcludeElement.EntityHistoryExcludeSubElements.Count == 1)
//                        {
//                            #region Reading exclude list data for simple combination

//                            EntityHistoryExcludeSubElement entityHistoryExcludeSubElement = entityHistoryExcludeElement.EntityHistoryExcludeSubElements[0];

//                            if (!String.IsNullOrWhiteSpace(entityHistoryExcludeSubElement.Ids))
//                            {
//                                switch (entityHistoryExcludeSubElement.ChangeType)
//                                {
//                                    case EntityHistoryExcludeChangeType.CommonAttribute:
//                                    case EntityHistoryExcludeChangeType.TechnicalAttribute:
//                                    case EntityHistoryExcludeChangeType.RelationshipAttribute:
//                                        {
//                                            if (String.IsNullOrWhiteSpace(excludeAttributeIds))
//                                            {
//                                                excludeAttributeIds = entityHistoryExcludeSubElement.Ids;
//                                            }
//                                            else
//                                            {
//                                                excludeAttributeIds += "," + entityHistoryExcludeSubElement.Ids;
//                                            }

//                                            break;
//                                        }
//                                    case EntityHistoryExcludeChangeType.ExtensionRelationship_Container:
//                                        {
//                                            if (String.IsNullOrWhiteSpace(excludeExtensionRelationshipContainerIds))
//                                            {
//                                                excludeExtensionRelationshipContainerIds = entityHistoryExcludeSubElement.Ids;
//                                            }
//                                            else
//                                            {
//                                                excludeExtensionRelationshipContainerIds += "," + entityHistoryExcludeSubElement.Ids;
//                                            }

//                                            break;
//                                        }
//                                    case EntityHistoryExcludeChangeType.RelationshipType:
//                                        {
//                                            if (String.IsNullOrWhiteSpace(excludeRelationshipTypeIds))
//                                            {
//                                                excludeRelationshipTypeIds = entityHistoryExcludeSubElement.Ids;
//                                            }
//                                            else
//                                            {
//                                                excludeRelationshipTypeIds += "," + entityHistoryExcludeSubElement.Ids;
//                                            }

//                                            break;
//                                        }
//                                    case EntityHistoryExcludeChangeType.Workflow:
//                                        {
//                                            if (String.IsNullOrWhiteSpace(excludeWorkflowIds))
//                                            {
//                                                excludeWorkflowIds = entityHistoryExcludeSubElement.Ids;
//                                            }
//                                            else
//                                            {
//                                                excludeWorkflowIds += "," + entityHistoryExcludeSubElement.Ids;
//                                            }

//                                            break;
//                                        }
//                                    case EntityHistoryExcludeChangeType.HierarchyRelationship_ChildEntityType:
//                                        {
//                                            if (String.IsNullOrWhiteSpace(excludeChildEntityTypeIds))
//                                            {
//                                                excludeChildEntityTypeIds = entityHistoryExcludeSubElement.Ids;
//                                            }
//                                            else
//                                            {
//                                                excludeChildEntityTypeIds += "," + entityHistoryExcludeSubElement.Ids;
//                                            }

//                                            break;
//                                        }
//                                }
//                            }

//                            #endregion
//                        }
//                        else
//                        {
//                            #region Reading exclude list data for complex combination

//                            //NOTE: Currently exclude list combination based on context is supported for RelationshipTypes and RelationshipAttributeIds.
//                            //      The below code has been written considering this assumption.

//                            String[] relationshipTypeIds = null;
//                            String[] relationshipAttributeIds = null;

//                            foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in entityHistoryExcludeElement.EntityHistoryExcludeSubElements)
//                            {
//                                if (entityHistoryExcludeSubElement.ChangeType == EntityHistoryExcludeChangeType.RelationshipType)
//                                {
//                                    String[] currentRelationshipTypeIds = entityHistoryExcludeSubElement.Ids.Split(',');

//                                    if (relationshipTypeIds == null)
//                                    {
//                                        relationshipTypeIds = currentRelationshipTypeIds;
//                                    }
//                                    else
//                                    {
//                                        relationshipTypeIds.CopyTo(currentRelationshipTypeIds, (currentRelationshipTypeIds.GetLength(1)));
//                                    }
//                                }
//                                else if (entityHistoryExcludeSubElement.ChangeType == EntityHistoryExcludeChangeType.RelationshipAttribute)
//                                {
//                                    String[] currentRelationshipAttributeIds = entityHistoryExcludeSubElement.Ids.Split(',');

//                                    if (relationshipAttributeIds == null)
//                                    {
//                                        relationshipAttributeIds = currentRelationshipAttributeIds;
//                                    }
//                                    else
//                                    {
//                                        relationshipAttributeIds.CopyTo(currentRelationshipAttributeIds, (currentRelationshipAttributeIds.GetLength(1)));
//                                    }
//                                }
//                            }

//                            if (relationshipTypeIds != null && relationshipAttributeIds != null)
//                            {
//                                foreach (String relationshipTypeId in relationshipTypeIds)
//                                {
//                                    foreach (String relationshipAttribute in relationshipAttributeIds)
//                                    {
//                                        KeyValuePair<Int32, Int32> relationshipTypeAttributePair = new KeyValuePair<Int32, Int32>(ValueTypeHelper.Int32TryParse(relationshipTypeId, 0), ValueTypeHelper.Int32TryParse(relationshipAttribute, 0));
//                                        excludeRelationshipTypeIdToAttributeList.Add(relationshipTypeAttributePair);
//                                    }
//                                }
//                            }

//                            #endregion
//                        }
//                    }
//                }
//            }

//            #endregion

//            entityCollectionHistory = _entityHistoryDA.GetEntityCollectionHistory(new Collection<Int64>() { entity.Id }, entityHistoryContext, excludeAttributeIds, excludeExtensionRelationshipContainerIds, excludeRelationshipTypeIds, excludeWorkflowIds, excludeChildEntityTypeIds, excludeRelationshipTypeIdToAttributeList, command);

//            return entityCollectionHistory;
//        }

//        private Dictionary<Int32, Int32> SplitEntiesForThreads(Int32 numberOfThreads, Int32 start, Int32 end, Int32 entitybatchPerThread)
//        {
//            Dictionary<Int32, Int32> threadBatchStart = new Dictionary<Int32, Int32>(numberOfThreads + 1);

//            for (Int32 i = 0; i < numberOfThreads; i++)
//            {
//                threadBatchStart[i] = start + i * entitybatchPerThread;
//            }

//            // the last thread gets its batch and the spill over..
//            threadBatchStart[numberOfThreads] = end + 1;

//            return threadBatchStart;
//        }

//        private String ModifyTemplateText(String templateText, String begin, String end)
//        {
//            Regex regex = new Regex(String.Format("\\{0}.*?\\{1}", begin, end));
//            return regex.Replace(templateText, string.Empty);
//        }

//        #endregion

//        /// <summary>
//        /// Get the security principal
//        /// </summary>
//        private void GetSecurityPrincipal()
//        {
//            if (_securityPrincipal == null)
//            {
//                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
//            }
//        }

//        #endregion

//        #endregion

//        #region Internal Classes

//        /// <summary>
//        /// Represents 
//        /// </summary>
//        internal enum DayGroupOperatorEnum
//        {
//            EqualsTo,
//            Range,
//            LessThan
//        }

//        /// <summary>
//        /// Represents Day Group
//        /// </summary>
//        internal class DayGroup
//        {
//            public DayGroupOperatorEnum DateGroupOperator;

//            public String DispalyVaue;

//            public DateTime MinRangeValue;

//            public DateTime MaxRangeValue;

//            public DateTime EqualsToValue;
//        }

//        #endregion
//    }
//}
