using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.ActivityLogManager.Business
{
    using BusinessObjects;
    using ConfigurationManager.Business;
    using Core;
    using Core.Exceptions;
    using Data;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MessageManager.Business;
    using Utility;

    /// <summary>
    /// Specifies data model activity log
    /// </summary>
    public class DataModelActivityLogBL : BusinessLogicBase
    {
        private DiagnosticActivity activity = new DiagnosticActivity();

        #region Constants

        private const String GetDataModelActivityLogStatusProcessName = "DataModelActivityLogBL.GetDataModelActivityLogStatus";
        private const String GetAllDataModelActivityLogStatusProcessName = "DataModelActivityLogBL.GetAllDataModelActivityLogStatus";

        #endregion

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private DataModelActivityLogDA _DataModelActivityLogDA = new DataModelActivityLogDA();

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// enable tracing for activity log
        /// </summary>
        private TraceSettings _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataModelActivityLogBL()
        {
            //GetSecurityPrincipal();
        }

        #endregion Ctor

        #region Public Methods

        /// <summary>
        ///  Processes the data model activity log object
        /// </summary>
        /// <param name="dataModelActivityLogCollection">Collection of impacted entity log to be processed</param>
        /// <param name="callerContext">Context indicating who called the API</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns></returns>
        public OperationResult Process(DataModelActivityLogCollection dataModelActivityLogCollection, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync)
        {

            #region validations

            DiagnosticActivity activity = new DiagnosticActivity();

            ValidateContext(callerContext, "Process");

            if (dataModelActivityLogCollection == null || dataModelActivityLogCollection.Count <= 0)
            {
                throw new MDMOperationException("dataModelActivityLogCollection cannot be null or empty.");
            }

            #endregion validations

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
                activity.LogVerbose("No. of activity log item to be loaded : " + dataModelActivityLogCollection.Count);
                activity.LogInformation("CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            String loginUser = SecurityPrincipal.CurrentUserName;

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            OperationResult result = _DataModelActivityLogDA.Process(dataModelActivityLogCollection, loginUser, callerContext, command, processingMode);

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }
            return result;
        }

        /// <summary>
        /// Gets all the impacted entity logs based on the log status
        /// * LogType.Current -> get all the records from tb_DataModelActivityLog with IsLoaded = true, IsProcessed = 0
        /// * LogType.Past -> get all the records from tb_DataModelActivityLog_HS
        /// * LogType.Pending -> get all the records from tb_DataModelActivityLog with IsLoaded = false, IsProcessed = 0
        /// </summary>
        /// <param name="processingStatus">LogType.Current,LogType.Past,LogType.Pending</param>
        /// <param name="callerContext">Context indicating who called the API</param>
        /// <returns></returns>
        public DataModelActivityLogCollection Get(ProcessingStatus processingStatus, CallerContext callerContext)
        {
            DataModelActivityLogCollection dataModelActivityLogCollection = Get(processingStatus, 0, 0, callerContext);

            return dataModelActivityLogCollection;
        }

        /// <summary>
        /// Gets all the data model activity logs based on the log status
        /// * LogType.Current -> get all the records from tb_DataModelActivityLog with IsLoaded = true, IsProcessed = 0
        /// * LogType.Past -> get all the records from tb_DataModelActivityLog_HS
        /// * LogType.Pending -> get all the records from tb_DataModelActivityLog with IsLoaded = false, IsProcessed = 0
        /// </summary>
        /// <param name="processingStatus">LogType.Current,LogType.Past,LogType.Pending</param>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="callerContext">Context indicating who called the API</param>
        /// <returns></returns>
        public DataModelActivityLogCollection Get(ProcessingStatus processingStatus, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelActivityLogBL", String.Empty, "GetDataModelActivityLogEntry by status");
            }

            #endregion validations

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            DataModelActivityLogCollection dataModelActivityLogCollection = _DataModelActivityLogDA.Get(processingStatus, fromRecordNumber, toRecordNumber, command);

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }
            return dataModelActivityLogCollection;
        }

        #region Fill DataModelActivity log object Methods

        /// <summary>
        /// Prepares datamodel activity logs for container collection
        /// </summary>
        /// <param name="containerCollection">Indicates container collection</param>
        /// <returns>Returns data model activity log collection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(ContainerCollection containerCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();

            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var container in containerCollection)
                {

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = container.OrganizationId,
                        ContainerId = container.Id,
                        EntityTypeId = -1,
                        RelationshipTypeId = -1,
                        DataModelActivityLogAction = DataModelActivityList.ContainerUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = container.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = container.Attributes.GetAttributeIdList(),
                        ImpactedCount = containerCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// transforms to-be-processed attributemodelcollection object into datamodelactivitylog object for processing
        /// return null if error
        /// </summary>
        /// <param name="hierarchyCollection"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(HierarchyCollection hierarchyCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();
            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var hierarchy in hierarchyCollection)
                {

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = -1,
                        ContainerId = -1,
                        EntityTypeId = -1,
                        RelationshipTypeId = -1,
                        DataModelActivityLogAction = DataModelActivityList.HierarchyUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = hierarchy.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = hierarchy.Attributes.GetAttributeIdList(),
                        ImpactedCount = hierarchyCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// transforms to-be-processed attributemodelcollection object into datamodelactivitylog object for processing
        /// return null if error
        /// </summary>
        /// <param name="organizationCollection"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(OrganizationCollection organizationCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();
            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var organization in organizationCollection)
                {

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = organization.Id,
                        ContainerId = -1,
                        EntityTypeId = -1,
                        RelationshipTypeId = -1,
                        DataModelActivityLogAction = DataModelActivityList.OrganizationUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = organization.OrganizationTypeId,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = organization.Attributes.GetAttributeIdList(),
                        ImpactedCount = organizationCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// transforms to-be-processed attributemodelcollection object into datamodelactivitylog object for processing
        /// return null if error
        /// </summary>
        /// <param name="entityTypeCollection"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(EntityTypeCollection entityTypeCollection)
        {
            #region Initial Setup

            Collection<int> attributeIdList = new Collection<int>();
            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();
            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var entityType in entityTypeCollection)
                {

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = -1,
                        ContainerId = -1,
                        EntityTypeId = entityType.Id,
                        RelationshipTypeId = -1,
                        DataModelActivityLogAction = DataModelActivityList.EntityTypeUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = entityType.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = attributeIdList,
                        ImpactedCount = entityTypeCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// Transforms to-be-processed attribute model collection object into data model activity log object for processing
        /// return null if error 
        /// </summary>
        /// <param name="attributeModelCollection">Indicates attribute model collection</param>
        /// <param name="dataModelObjectType">Indicates object type of data model objects to be processed</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <param name="baseAttributeModels">Indicates original attribute models. 
        /// This is an optional parameter and will be null in case of DMD import process.</param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(AttributeModelCollection attributeModelCollection, ObjectType dataModelObjectType, CallerContext callerContext, Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = null)
        {
            // Concept here is : per attribute, separate entry will be logged in activity log table.
            // But, change context xml will be populated only for the change in metadata properties like Inheritable, IsSerachable and AllowNullSerach .

            #region Initial Setup

            Boolean isMetadataChangeProcessingQualified;
            DataModelActivityLogCollection dataModelActivityLogs = null;
            DataModelActivityLogCollection qualifiedActivityLogs = null;

            #endregion Initial Setup

            try
            {
                if (attributeModelCollection != null && attributeModelCollection.Count > 0)
                {
                    dataModelActivityLogs = new DataModelActivityLogCollection();

                    foreach (var attributeModel in attributeModelCollection)
                    {
                        if (attributeModel.Action == ObjectAction.Read || attributeModel.Action == ObjectAction.Ignore)
                        {
                            continue;
                        }

                        #region Fill Activity Log record

                        DataModelActivityLog activityLog = GetDataModelActivityLog(attributeModel.Action, new Collection<Int32>() { attributeModel.Id });

                        if (attributeModel.Action == ObjectAction.Create)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.AttributeModelCreate;
                        }
                        else if (attributeModel.Action == ObjectAction.Update)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.AttributeModelUpdate;
                        }
                        else if (attributeModel.Action == ObjectAction.Delete)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.AttributeModelDelete;
                        }

                        isMetadataChangeProcessingQualified = IsMetadataChangeProcessingRequiredForAttributeModels(attributeModel, baseAttributeModels);
                        if (isMetadataChangeProcessingQualified)
                        {
                            if (qualifiedActivityLogs == null)
                            {
                                qualifiedActivityLogs = new DataModelActivityLogCollection();
                            }

                            qualifiedActivityLogs.Add(activityLog);
                        }
                        else
                        {
                            dataModelActivityLogs.Add(activityLog);
                        }

                        #endregion Fill Activity Log record
                    }

                    if (qualifiedActivityLogs != null && qualifiedActivityLogs.Count > 0)
                    {
                        DataModelActivityLogUtils.PopulateEntityFamilyChangeContext(qualifiedActivityLogs, dataModelObjectType, callerContext);

                        // Add logs into collection being returned after change context xml population
                        dataModelActivityLogs.AddRange(qualifiedActivityLogs);
                    }
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            return dataModelActivityLogs;
        }

        /// <summary>
        /// Transforms to-be-processed RelationshipTypeCollection object into datamodelactivitylog object for processing
        /// return null if error
        /// </summary>
        /// <param name="relationshipTypeCollection"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(RelationshipTypeCollection relationshipTypeCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();

            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var relationshipType in relationshipTypeCollection)
                {
                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = -1,
                        ContainerId = -1,
                        EntityTypeId = -1,
                        RelationshipTypeId = relationshipType.Id,
                        DataModelActivityLogAction = DataModelActivityList.RelationshipTypeUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = relationshipType.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = null,
                        ImpactedCount = relationshipTypeCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;

        }

        /// <summary>
        /// transforms to-be-processed containerRelationshipTypeEntityTypeMappingCollection object into datamodelactivitylog object for processing
        /// </summary>
        /// <param name="containerEntityTypeMappingCollection"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(ContainerEntityTypeMappingCollection containerEntityTypeMappingCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();

            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var containerEntityTypeMapping in containerEntityTypeMappingCollection)
                {

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = containerEntityTypeMapping.OrganizationId,
                        ContainerId = containerEntityTypeMapping.OrganizationId,
                        EntityTypeId = containerEntityTypeMapping.EntityTypeId,
                        RelationshipTypeId = -1,
                        DataModelActivityLogAction = DataModelActivityList.ContainerRelationshipTypeEntityTypeMappingUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = containerEntityTypeMapping.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = null,
                        ImpactedCount = containerEntityTypeMappingCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// Transforms to-be-processed EntityTypeAttributeMappingCollection object into datamodelactivitylog object for processing
        /// </summary>
        /// <param name="entityTypeAttributeMappings">Indicates EntityTypeAttributeMappingCollection to be processed</param>
        /// <param name="dataModelObjectType">Indicates object type of data model objects to be processed</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, ObjectType dataModelObjectType, CallerContext callerContext)
        {
            #region Initial Setup

            DataModelActivityLogCollection dataModelActivityLogs = null;
            Dictionary<String, Dictionary<ObjectAction, Collection<Int32>>> masterBucket = new Dictionary<String, Dictionary<ObjectAction, Collection<Int32>>>();

            #endregion Initial Setup

            #region fill log object

            try
            {
                #region Prepare attribute mappings based on same context and action

                foreach (EntityTypeAttributeMapping entityTypeAttrMapping in entityTypeAttributeMappings)
                {
                    ObjectAction performedAction = entityTypeAttrMapping.Action;
                    if(performedAction == ObjectAction.Read || performedAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    Int32 entityTypeId = entityTypeAttrMapping.EntityTypeId;
                    Int32 attributeId = entityTypeAttrMapping.AttributeId;

                    FilterAttributeIdsByCommonContext(-1, -1, entityTypeId, -1, -1, attributeId, performedAction, ref masterBucket);
                }

                #endregion Prepare attribute mappings based on same context and action

                #region Preparing activity log collection from dictionary

                foreach (KeyValuePair<String, Dictionary<ObjectAction, Collection<Int32>>> item in masterBucket)
                {
                    String contextKey = item.Key;
                    Collection<Int32> contextIds = ValueTypeHelper.SplitStringToIntCollection(contextKey, '.');

                    foreach (KeyValuePair<ObjectAction, Collection<Int32>> attributeActionKeyValue in item.Value)
                    {
                        ObjectAction attributeActionKey = attributeActionKeyValue.Key;

                        DataModelActivityLog activityLog = GetDataModelActivityLog(attributeActionKey, attributeActionKeyValue.Value);
                        activityLog.OrgId = contextIds[0];
                        activityLog.ContainerId = contextIds[1];
                        activityLog.EntityTypeId = contextIds[2];
                        activityLog.RelationshipTypeId = contextIds[3];

                        #region Set activityLog.DataModelActivityLogAction

                        if (attributeActionKey == ObjectAction.Create)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.EntityTypeAttributeMappingCreate;
                        }
                        else if (attributeActionKey == ObjectAction.Update)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.EntityTypeAttributeMappingUpdate;
                        }
                        else if (attributeActionKey == ObjectAction.Delete)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.EntityTypeAttributeMappingDelete;
                        }

                        #endregion Set activityLog.DataModelActivityLogAction

                        // Only populate change context xml when mapping is created or deleted.
                        if (activityLog.Action == ObjectAction.Create || activityLog.Action == ObjectAction.Delete)
                        {
                            DataModelActivityLogUtils.PopulateEntityFamilyChangeContext(new DataModelActivityLogCollection() { activityLog }, dataModelObjectType, callerContext);
                        }

                        if (dataModelActivityLogs == null)
                        {
                            dataModelActivityLogs = new DataModelActivityLogCollection();
                        }

                        dataModelActivityLogs.Add(activityLog);
                    }

                }

                #endregion Preparing activity log collection from dictionary
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return dataModelActivityLogs;
        }

        /// <summary>
        /// Transforms to-be-processed ContainerEntityTypeAttributeMappingCollection object into DataModelActivityLogCollection for processing
        /// </summary>
        /// <param name="containerETypeAttrMappings">Indicates ContainerEntityTypeAttributeMappingCollection to be processed</param>
        /// <param name="dataModelObjectType">Indicates object type of data model objects to be processed</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(ContainerEntityTypeAttributeMappingCollection containerETypeAttrMappings, ObjectType dataModelObjectType, CallerContext callerContext)
        {
            #region Initial Setup

            DataModelActivityLogCollection dataModelActivityLogs = null;
            Dictionary<String, Dictionary<ObjectAction, Collection<Int32>>> masterBucket = new Dictionary<String, Dictionary<ObjectAction, Collection<Int32>>>();

            #endregion Initial Setup

            try
            {
                #region Prepare attribute mappings based on same context and action

                foreach (ContainerEntityTypeAttributeMapping contETAttrMapping in containerETypeAttrMappings)
                {
                    ObjectAction performedAction = contETAttrMapping.Action;
                    if (performedAction == ObjectAction.Read || performedAction == ObjectAction.Ignore)
                    {
                        continue;
                    }
                    
                    Int32 orgId = contETAttrMapping.OrganizationId;
                    Int32 containerId = contETAttrMapping.ContainerId;
                    Int32 entityTypeId = contETAttrMapping.EntityTypeId;
                    Int32 attributeId = contETAttrMapping.AttributeId;
                    
                    FilterAttributeIdsByCommonContext(orgId, containerId, entityTypeId, -1, -1, attributeId, performedAction, ref masterBucket);
                }

                #endregion Prepare attribute mappings based on same context and action

                #region Preparing activity log collection from dictionary

                foreach (KeyValuePair<String, Dictionary<ObjectAction, Collection<Int32>>> item in masterBucket)
                {
                    String contextKey = item.Key;
                    Collection<Int32> contextIds = ValueTypeHelper.SplitStringToIntCollection(contextKey, '.');

                    foreach (KeyValuePair<ObjectAction, Collection<Int32>> attributeActionKeyValue in item.Value)
                    {
                        ObjectAction attributeActionKey = attributeActionKeyValue.Key;
                        DataModelActivityLog activityLog = GetDataModelActivityLog(attributeActionKey, attributeActionKeyValue.Value);
                        activityLog.OrgId = contextIds[0];
                        activityLog.ContainerId = contextIds[1];
                        activityLog.EntityTypeId = contextIds[2];
                        activityLog.RelationshipTypeId = contextIds[3];

                        #region Set activityLog.DataModelActivityLogAction

                        if (attributeActionKey == ObjectAction.Create)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.ContainerEntityTypeAttributeMappingCreate;
                        }
                        else if (attributeActionKey == ObjectAction.Update)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.ContainerEntityTypeAttributeMappingUpdate;
                        }
                        else if (attributeActionKey == ObjectAction.Delete)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.ContainerEntityTypeAttributeMappingDelete;
                        }

                        #endregion Set activityLog.DataModelActivityLogAction

                        // Only populate change context xml when mapping is created or deleted.
                        if (activityLog.Action == ObjectAction.Create || activityLog.Action == ObjectAction.Delete)
                        {
                            DataModelActivityLogUtils.PopulateEntityFamilyChangeContext(new DataModelActivityLogCollection() { activityLog }, dataModelObjectType, callerContext);
                        }

                        if (dataModelActivityLogs == null)
                        {
                            dataModelActivityLogs = new DataModelActivityLogCollection();
                        }

                        dataModelActivityLogs.Add(activityLog);
                    }

                }

                #endregion Preparing activity log collection from dictionary

            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            return dataModelActivityLogs;
        }

        /// <summary>
        /// Transforms to-be-processed CategoryAttributeMappingCollection object into datamodelactivitylog object for processing
        /// </summary>
        /// <param name="categoryAttrMappings">Indicates CategoryAttributeMappingCollection to be processed</param>
        /// <param name="dataModelObjectType"></param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(CategoryAttributeMappingCollection categoryAttrMappings, ObjectType dataModelObjectType, CallerContext callerContext)
        {
            #region Initial Setup

            DataModelActivityLogCollection dataModelActivityLogs = null;
            Dictionary<String, Dictionary<ObjectAction, Collection<Int32>>> masterBucket = new Dictionary<String, Dictionary<ObjectAction, Collection<Int32>>>();

            #endregion Initial Setup

            #region fill log object

            try
            {
                #region Prepare attribute mappings based on same context and action

                foreach (CategoryAttributeMapping categortAttrMapping in categoryAttrMappings)
                {
                    if(categortAttrMapping.Action == ObjectAction.Read || categortAttrMapping.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }
                    
                    Int32 attributeId = categortAttrMapping.AttributeId;
                    Int64 categoryId = categortAttrMapping.CategoryId;

                    FilterAttributeIdsByCommonContext(-1, -1, -1, -1, categoryId, attributeId, categortAttrMapping.Action, ref masterBucket);
                }

                #endregion Prepare attribute mappings based on same context and action

                #region Preparing activity log collection from dictionary

                foreach (KeyValuePair<String, Dictionary<ObjectAction, Collection<Int32>>> item in masterBucket)
                {
                    String contextKey = item.Key;
                    Collection<Int32> contextIds = ValueTypeHelper.SplitStringToIntCollection(contextKey, '.');

                    foreach (KeyValuePair<ObjectAction, Collection<Int32>> attributeActionKeyValue in item.Value)
                    {
                        ObjectAction attributeActionKey = attributeActionKeyValue.Key;
                        DataModelActivityLog activityLog = GetDataModelActivityLog(attributeActionKey, attributeActionKeyValue.Value);
                        activityLog.OrgId = contextIds[0];
                        activityLog.ContainerId = contextIds[1];
                        activityLog.EntityTypeId = contextIds[2];
                        activityLog.RelationshipTypeId = contextIds[3];

                        // MDMObjectId property is being populated for storing category id only for CategoryAttributeMapping object.
                        // For rest of the objects, it is set to 0 in GetDataModelActivityLog().
                        activityLog.MDMObjectId = contextIds[4]; 

                        #region Set activityLog.DataModelActivityLogAction

                        if (attributeActionKey == ObjectAction.Create)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.CategoryAttributeMappingCreate;
                        }
                        else if (attributeActionKey == ObjectAction.Update)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.CategoryAttributeMappingUpdate;
                        }
                        else if (attributeActionKey == ObjectAction.Delete)
                        {
                            activityLog.DataModelActivityLogAction = DataModelActivityList.CategoryAttributeMappingDelete;
                        }

                        #endregion Set activityLog.DataModelActivityLogAction

                        // Only populate change context xml when mapping is created or deleted.
                        if (activityLog.Action == ObjectAction.Create || activityLog.Action == ObjectAction.Delete)
                        {
                            DataModelActivityLogUtils.PopulateEntityFamilyChangeContext(new DataModelActivityLogCollection() { activityLog }, dataModelObjectType, callerContext);
                        }

                        if (dataModelActivityLogs == null)
                        {
                            dataModelActivityLogs = new DataModelActivityLogCollection();
                        }

                        dataModelActivityLogs.Add(activityLog);
                    }

                }

                #endregion Preparing activity log collection from dictionary
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return dataModelActivityLogs;
        }

        /// <summary>
        /// 
        /// Transforms to-be-processed RelationshipTypeEntityTypeMappingCollection object into datamodelactivitylog object for processing
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings">Indicates RelationshipTypeEntityTypeMappingCollection to be processed</param>
        /// <param name="dataModelObjectType">Indicates object type of data model objects to be processed</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <param name="originalMappings">Indicates original mappings from DB</param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, ObjectType dataModelObjectType, CallerContext callerContext, RelationshipTypeEntityTypeMappingCollection originalMappings = null)
        {
            #region Initial Setup

            Boolean isMetadataChangeProcessingQualified;
            DataModelActivityLogCollection dataModelActivityLogs = null;
            DataModelActivityLogCollection qualifiedActivityLogs = null;

            #endregion Initial Setup

            #region fill log object

            try
            {
                dataModelActivityLogs = new DataModelActivityLogCollection();

                foreach (RelationshipTypeEntityTypeMapping rTETMapping in relationshipTypeEntityTypeMappings)
                {
                    if (rTETMapping.Action == ObjectAction.Read || rTETMapping.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    DataModelActivityLog activityLog = GetDataModelActivityLog(rTETMapping.Action);
                    activityLog.OrgId = -1;
                    activityLog.ContainerId = -1;
                    activityLog.EntityTypeId = rTETMapping.EntityTypeId;
                    activityLog.RelationshipTypeId = rTETMapping.RelationshipTypeId;

                    #region Set activityLog.DataModelActivityLogAction

                    if (activityLog.Action == ObjectAction.Create)
                    {
                        activityLog.DataModelActivityLogAction = DataModelActivityList.RelationshipTypeEntityTypeMappinCreate;
                    }
                    else if (activityLog.Action == ObjectAction.Update)
                    {
                        activityLog.DataModelActivityLogAction = DataModelActivityList.RelationshipTypeEntityTypeMappingUpdate;
                    }
                    else if (activityLog.Action == ObjectAction.Delete)
                    {
                        activityLog.DataModelActivityLogAction = DataModelActivityList.RelationshipTypeEntityTypeMappingDelete;
                    }

                    #endregion Set activityLog.DataModelActivityLogAction

                    // TODO : For Delete Action (UI), EntityTypeId and RelationshipId values are coming as -1. 
                    // So even Get from originalMappings based on -1 values is not returning any matched mapping.
                    // Need to add logic for setting ids.

                    isMetadataChangeProcessingQualified = IsMetadataChangeProcessingRequiredForRTETMappings(rTETMapping, originalMappings);

                    if (isMetadataChangeProcessingQualified)
                    {
                        if (qualifiedActivityLogs == null)
                        {
                            qualifiedActivityLogs = new DataModelActivityLogCollection();
                        }

                        qualifiedActivityLogs.Add(activityLog);
                    }
                    else
                    {
                        dataModelActivityLogs.Add(activityLog);
                    }
                }

                if (qualifiedActivityLogs != null && qualifiedActivityLogs.Count > 0)
                {
                    DataModelActivityLogUtils.PopulateEntityFamilyChangeContext(qualifiedActivityLogs, dataModelObjectType, callerContext);
                    dataModelActivityLogs.AddRange(qualifiedActivityLogs);
                }

            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return dataModelActivityLogs;
        }

        /// <summary>
        /// Transforms to-be-processed containerRelationshipTypeEntityTypeMappingCollection object into datamodelactivitylog object for processing
        /// </summary>
        /// <param name="contRTETMappings">Indicates ContainerRelationshipTypeEntityTypeMappingCollection to be processed</param>
        /// <param name="dataModelObjectType">Indicates object type of data model objects to be processed</param>
        /// <param name="callerContext">Indiacates caller context</param>
        /// <param name="originalMappings">Indicates original mappings from DB</param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(ContainerRelationshipTypeEntityTypeMappingCollection contRTETMappings, ObjectType dataModelObjectType, CallerContext callerContext, ContainerRelationshipTypeEntityTypeMappingCollection originalMappings)
        {
            #region Initial Setup

            DataModelActivityLogCollection dataModelActivityLogs = null;
            DataModelActivityLogCollection qualifiedActivityLogs = null;

            #endregion Initial Setup

            #region fill log object

            try
            {
                dataModelActivityLogs = new DataModelActivityLogCollection();

                foreach (var conRTETMapping in contRTETMappings)
                {
                    if (conRTETMapping.Action == ObjectAction.Read || conRTETMapping.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }
                    
                    DataModelActivityLog activityLog = GetDataModelActivityLog(conRTETMapping.Action);
                    activityLog.OrgId = conRTETMapping.OrganizationId;
                    activityLog.ContainerId = conRTETMapping.ContainerId;
                    activityLog.EntityTypeId = conRTETMapping.EntityTypeId;
                    activityLog.RelationshipTypeId = conRTETMapping.RelationshipTypeId;

                    #region Set activityLog.DataModelActivityLogAction

                    if (activityLog.Action == ObjectAction.Create)
                    {
                        activityLog.DataModelActivityLogAction = DataModelActivityList.ContainerRelationshipTypeEntityTypeMappingCreate;
                    }
                    else if (activityLog.Action == ObjectAction.Update)
                    {
                        activityLog.DataModelActivityLogAction = DataModelActivityList.ContainerRelationshipTypeEntityTypeMappingUpdate;
                    }
                    else if (activityLog.Action == ObjectAction.Delete)
                    {
                        activityLog.DataModelActivityLogAction = DataModelActivityList.ContainerRelationshipTypeEntityTypeMappingDelete;
                    }

                    #endregion Set activityLog.DataModelActivityLogAction

                    // TODO : For Delete Action (UI), ContainerID, EntityTypeId and RelationshipId values are coming as -1. 
                    // So even Get from originalMappings based on -1 values is not returning any matched mapping.
                    // Need to add logic for setting correct values for these ids.

                    Boolean isMetadataChangeProcessingQualified = IsMetadataChangeProcessingRequiredForConRTETMappings(conRTETMapping, originalMappings);

                    if (isMetadataChangeProcessingQualified)
                    {
                        if (qualifiedActivityLogs == null)
                        {
                            qualifiedActivityLogs = new DataModelActivityLogCollection();
                        }

                        qualifiedActivityLogs.Add(activityLog);
                    }
                    else
                    {
                        dataModelActivityLogs.Add(activityLog);
                    }
                }

                if (qualifiedActivityLogs != null && qualifiedActivityLogs.Count > 0)
                {
                    DataModelActivityLogUtils.PopulateEntityFamilyChangeContext(qualifiedActivityLogs, dataModelObjectType, callerContext);
                    dataModelActivityLogs.AddRange(qualifiedActivityLogs);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return dataModelActivityLogs;
        }

        /// <summary>
        /// Transforms to-be-processed relationshipTypeAttributeMappingCollection object into datamodelactivitylog object for processing
        /// </summary>
        /// <param name="relationshipTypeAttributeMappingCollection">Indicates RelationshipTypeAttributeMappingCollection to be processed</param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappingCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();

            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var relationshipTypeAttributeMapping in relationshipTypeAttributeMappingCollection)
                {
                    Collection<int> attributeIdList = new Collection<int>();
                    attributeIdList.Add(relationshipTypeAttributeMapping.AttributeId);

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = -1,
                        ContainerId = -1,
                        EntityTypeId = -1,
                        RelationshipTypeId = relationshipTypeAttributeMapping.RelationshipTypeId,
                        DataModelActivityLogAction = DataModelActivityList.RelationshipTypeAttributeMappingUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = relationshipTypeAttributeMapping.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = attributeIdList,
                        ImpactedCount = relationshipTypeAttributeMappingCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// Transforms to-be-processed ContainerRelationshipTypeAttributeMappingCollection object into DataModelActivityLogCollection for processing
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeAttrMappingCollection">Indicates ContainerEntityTypeAttributeMappingCollection to be processed</param>
        /// <returns>Returns DataModelActivityLogCollection</returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeEntityTypeAttrMappingCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();

            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var containerRelationshipTypeEntityTypeAttrMapping in containerRelationshipTypeEntityTypeAttrMappingCollection)
                {
                    Collection<int> attributeIdList = new Collection<int>();
                    attributeIdList.Add(containerRelationshipTypeEntityTypeAttrMapping.AttributeId);

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = containerRelationshipTypeEntityTypeAttrMapping.OrganizationId,
                        ContainerId = containerRelationshipTypeEntityTypeAttrMapping.ContainerId,
                        EntityTypeId = -1,
                        RelationshipTypeId = containerRelationshipTypeEntityTypeAttrMapping.RelationshipTypeId,
                        DataModelActivityLogAction = DataModelActivityList.ContainerRelationshipTypeEntityTypeMappingUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = containerRelationshipTypeEntityTypeAttrMapping.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = attributeIdList,
                        ImpactedCount = containerRelationshipTypeEntityTypeAttrMappingCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupCollection"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(LookupCollection lookupCollection)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();

            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var lookup in lookupCollection)
                {
                    Collection<int> attributeIdList = new Collection<int>();
                    attributeIdList.Add(lookup.AttributeId);

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = -1,
                        ContainerId = -1,
                        EntityTypeId = -1,
                        RelationshipTypeId = -1,
                        DataModelActivityLogAction = DataModelActivityList.LookupRowUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = lookup.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = attributeIdList,
                        ImpactedCount = lookupCollection.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection FillDataModelActivityLogCollection(DBTableCollection dbTables)
        {
            #region Initial Setup

            DataModelActivityLogCollection returneDataModelActivityLogCollection = new DataModelActivityLogCollection();

            #endregion Initial Setup

            #region fill log object

            try
            {
                foreach (var dbTable in dbTables)
                {
                    Collection<int> attributeIdList = new Collection<int>();
                    attributeIdList.Add(dbTable.AttributeId);

                    DataModelActivityLog dataModelActivityLog = new DataModelActivityLog
                    {
                        OrgId = -1,
                        ContainerId = -1,
                        EntityTypeId = -1,
                        RelationshipTypeId = -1,
                        DataModelActivityLogAction = DataModelActivityList.LookUpTableUpdate,
                        Weightage = 0,
                        IsLoadingInProgress = false,
                        IsLoaded = false,
                        IsProcessed = false,
                        LoadStartTime = null,
                        LoadEndTime = null,
                        ProcessStartTime = null,
                        ProcessEndTime = null,
                        CreatedDateTime = null,
                        ServerId = 0,
                        DataModelActivityLogId = -1,
                        MDMObjectId = dbTable.Id,
                        PerformedAction = PerformedAction.Updated,
                        AttributeIdList = attributeIdList,
                        ImpactedCount = dbTables.Count
                    };
                    returneDataModelActivityLogCollection.Add(dataModelActivityLog);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                return null;
            }

            #endregion fill log object

            return returneDataModelActivityLogCollection;
        }

        #endregion Fill DataModelActivity log object Methods

        #endregion Public Methods

        #region Private Methods

        private void ValidateContext(CallerContext callerContext, String methodName)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelActivityLogBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        private String GetKey(Int32 orgId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId)
        {
            return String.Format("{0}.{1}.{2}.{3}.{4}", orgId, containerId, entityTypeId, relationshipTypeId, categoryId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relatinshipTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="attributeId"></param>
        /// <param name="action"></param>
        /// <param name="masterBucket"></param>
        private void FilterAttributeIdsByCommonContext(Int32 orgId, Int32 containerId, Int32 entityTypeId, Int32 relatinshipTypeId, Int64 categoryId, Int32 attributeId, ObjectAction action, ref Dictionary<String, Dictionary<ObjectAction, Collection<Int32>>> masterBucket)
        {
            Collection<Int32> attributeIds = null;
            Dictionary<ObjectAction, Collection<Int32>> attributesPerAction = null;

            String org_Con_ET_RT_Cat_Key = GetKey(orgId, containerId, entityTypeId, relatinshipTypeId, categoryId);

            masterBucket.TryGetValue(org_Con_ET_RT_Cat_Key, out attributesPerAction);
            if (attributesPerAction == null)
            {
                attributesPerAction = new Dictionary<ObjectAction, Collection<Int32>>();
                masterBucket.Add(org_Con_ET_RT_Cat_Key, attributesPerAction);
            }
            attributesPerAction.TryGetValue(action, out attributeIds);
            if (attributeIds == null)
            {
                attributeIds = new Collection<Int32>();
                attributesPerAction.Add(action, attributeIds);
            }
            attributeIds.Add(attributeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="attributeIdList"></param>
        /// <returns></returns>
        private DataModelActivityLog GetDataModelActivityLog(ObjectAction action, Collection<Int32> attributeIdList = null)
        {
            DataModelActivityLog activityLog = new DataModelActivityLog();

            activityLog.Weightage = 0;
            activityLog.IsLoadingInProgress = false;
            activityLog.IsProcessed = false;
            activityLog.LoadStartTime = null;
            activityLog.LoadEndTime = null;
            activityLog.ProcessStartTime = null;
            activityLog.ProcessEndTime = null;
            activityLog.CreatedDateTime = null;
            activityLog.ServerId = 0;
            activityLog.MDMObjectId = 0;
            activityLog.PerformedAction = PerformedAction.Updated;
            activityLog.Action = action;
            activityLog.ImpactedCount = 0;  // DB will calculate and update it.

            if (attributeIdList != null && attributeIdList.Count > 0)
            {
                activityLog.AttributeIdList = attributeIdList;
            }

            activityLog.ChangedData = String.Empty;

            return activityLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="baseAttributeModels"></param>
        /// <returns></returns>
        private Boolean IsMetadataChangeProcessingRequiredForAttributeModels(AttributeModel attributeModel, Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels)
        {
            if (attributeModel.OriginalAttributeModel == null)
            {
                AttributeModelBaseProperties originalAttributeModel = AttributeModelUtility.GetBaseAttributeModelByKey(baseAttributeModels, attributeModel.Id);
                if (originalAttributeModel != null)
                {
                    if (attributeModel.Inheritable != originalAttributeModel.Inheritable
                        || attributeModel.Searchable != originalAttributeModel.Searchable
                                || attributeModel.AllowNullSearch != originalAttributeModel.AllowNullSearch)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (attributeModel.Inheritable != attributeModel.OriginalAttributeModel.Inheritable
                    || attributeModel.Searchable != attributeModel.OriginalAttributeModel.Searchable
                        || attributeModel.AllowNullSearch != attributeModel.OriginalAttributeModel.AllowNullSearch)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rTETMapping"></param>
        /// <param name="originalMappings"></param>
        /// <returns></returns>
        private Boolean IsMetadataChangeProcessingRequiredForRTETMappings(RelationshipTypeEntityTypeMapping rTETMapping, RelationshipTypeEntityTypeMappingCollection originalMappings)
        {
            RelationshipTypeEntityTypeMapping originalMapping = null;

            if (rTETMapping.OriginalRelationshipTypeEntityTypeMapping == null)
            {
                originalMapping = originalMappings.Get(rTETMapping.EntityTypeId, rTETMapping.RelationshipTypeId);
            }
            else
            {
                originalMapping = rTETMapping.OriginalRelationshipTypeEntityTypeMapping;
            }

            if (originalMapping != null)
            {
                if (rTETMapping.DrillDown != originalMapping.DrillDown) // There is a change
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conRTETMapping"></param>
        /// <param name="originalMappings"></param>
        /// <returns></returns>
        private Boolean IsMetadataChangeProcessingRequiredForConRTETMappings(ContainerRelationshipTypeEntityTypeMapping conRTETMapping, ContainerRelationshipTypeEntityTypeMappingCollection originalMappings)
        {
            ContainerRelationshipTypeEntityTypeMapping originalMapping = null;

            if (conRTETMapping.OriginalContainerRelationshipTypeEntityTypeMapping == null)
            {
                originalMapping = originalMappings.Get(conRTETMapping.ContainerId, conRTETMapping.EntityTypeId, conRTETMapping.RelationshipTypeId);
            }
            else
            {
                originalMapping = conRTETMapping.OriginalContainerRelationshipTypeEntityTypeMapping;
            }

            if (originalMapping != null)
            {
                if (conRTETMapping.DrillDown != originalMapping.DrillDown) // There is a change
                {
                    return true;
                }
            }

            return false;
        }

        #region Properties

        private SecurityPrincipal SecurityPrincipal
        {
            get
            {
                try
                {
                    if (_securityPrincipal == null)
                    {
                        _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                    }

                    return _securityPrincipal;
                }
                catch
                {
                    activity.LogError("Unable to fetch login user");
                }
                return null;
            }
        }

        #endregion

        #endregion

    }
}