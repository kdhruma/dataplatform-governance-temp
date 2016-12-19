using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace MDM.IntegrationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies integration error log type data access
    /// </summary>
    public class IntegrationItemStatusDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.IntegrationItemStatusDA.Process";
        private const String _searchMethodName = "MDM.IntegrationManager.Data.IntegrationItemStatusDA.SearchIntegrationItemStatus";

        /// <summary>
        /// Specifies type of search criteria for IntegrationItemStatus
        /// </summary>
        private enum IntegrationItemStatusSearchObjectType
        {
            /// <summary>
            /// Where to search is not know. Ignore this kind of search parameter.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Search in MDMObject.
            /// </summary>
            MDMObjectType = 1,

            /// <summary>
            /// Search in ExternalObject
            /// </summary>
            ExternalObjectType = 2,

            /// <summary>
            /// Search in Dimension type/value.
            /// </summary>
            DimensionValue = 3,

            /// <summary>
            /// Search in ItemStatusComments.
            /// </summary>
            StatusComments = 4,

            /// <summary>
            /// Search in ItemDimensionComments.
            /// </summary>
            DimensionComments = 4,

            /// <summary>
            /// Search in status type
            /// </summary>
            StatusType = 5,

            /// <summary>
            /// Indicates which status value to search.
            /// </summary>
            StatusValue = 6
        }

        #endregion Fields

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Process integration error log
        /// </summary>
        /// <param name="integrationItemStatus">IntegrationItemStatus to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration error log</returns>
        public void Process(IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection, String programName, String userName, OperationResultCollection operationResults, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_IntegrationItemStatus_Process";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationItemStatus_Process_ParametersArray");

            SqlMetaData[] sqlMetadataItemStatus = generator.GetTableValueMetadata("IntegrationManager_IntegrationItemStatus_Process_ParametersArray", parameters[0].ParameterName);
            SqlMetaData[] sqlMetadataDimensions = generator.GetTableValueMetadata("IntegrationManager_IntegrationItemStatus_Process_ParametersArray", parameters[1].ParameterName);

            List<SqlDataRecord> itemStatusRecords = null;
            List<SqlDataRecord> itemStatusDimensions = null;

            PopulateSqlRecords(integrationItemStatusInternalCollection, out itemStatusRecords, sqlMetadataItemStatus, out itemStatusDimensions, sqlMetadataDimensions);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;

                try
                {
                    parameters[0].Value = itemStatusRecords;
                    parameters[1].Value = itemStatusDimensions;
                    parameters[2].Value = userName;
                    parameters[3].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update item status with actual id
                    //UpdateIntegrationItemStatuss(reader, integrationItemStatusInternalCollection, operationResults);

                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integrationItemStatusSearchCriteria"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public IntegrationItemStatusInternalCollection SearchIntegrationItemStatus(IntegrationItemStatusSearchCriteria integrationItemStatusSearchCriteria, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_searchMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_IntegrationItemStatus_Search";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationItemStatus_Search_ParametersArray");

            SqlMetaData[] sqlMetadataStatusSearchCriteria = generator.GetTableValueMetadata("IntegrationManager_IntegrationItemStatus_Search_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> statusSearchCriteriaRecords = GetSqlRecords(integrationItemStatusSearchCriteria, sqlMetadataStatusSearchCriteria);

            IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection = new IntegrationItemStatusInternalCollection();

            try
            {
                parameters[0].Value = statusSearchCriteriaRecords;
                parameters[1].Value = integrationItemStatusSearchCriteria.ConnectorId;
                parameters[2].Value = integrationItemStatusSearchCriteria.IncludeHistoryData;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                //Read status
                integrationItemStatusInternalCollection = ReadIntegrationItemStatusInternal(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_searchMethodName, MDMTraceSource.Integration);
                }
            }

            return integrationItemStatusInternalCollection;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integrationItemStatusInternalCollection"></param>
        /// <param name="sqlMetadataItemStatus"></param>
        /// <returns></returns>
        private void PopulateSqlRecords(IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection, out List<SqlDataRecord> itemStatusRecords, SqlMetaData[] sqlMetadataItemStatus, out List<SqlDataRecord> itemStatusDimensions, SqlMetaData[] sqlMetadataDimensions)
        {
            Int32 referenceId = -1;
            itemStatusRecords = new List<SqlDataRecord>();
            itemStatusDimensions = new List<SqlDataRecord>();

            foreach (IntegrationItemStatusInternal integrationItemStatusInternal in integrationItemStatusInternalCollection)
            {
                SqlDataRecord integrationItemStatusInternalRecord = new SqlDataRecord(sqlMetadataItemStatus);

                integrationItemStatusInternalRecord.SetValue(0, referenceId);
                integrationItemStatusInternalRecord.SetValue(1, integrationItemStatusInternal.MDMObjectId);
                integrationItemStatusInternalRecord.SetValue(2, integrationItemStatusInternal.MDMObjectTypeId);
                integrationItemStatusInternalRecord.SetValue(3, integrationItemStatusInternal.ExternalId);
                integrationItemStatusInternalRecord.SetValue(4, integrationItemStatusInternal.ExternalObjectTypeId);
                integrationItemStatusInternalRecord.SetValue(5, integrationItemStatusInternal.ConnectorId);
                integrationItemStatusInternalRecord.SetValue(6, integrationItemStatusInternal.Status);
                integrationItemStatusInternalRecord.SetValue(7, integrationItemStatusInternal.StatusType);
                integrationItemStatusInternalRecord.SetValue(8, integrationItemStatusInternal.IsExternalStatus);
                integrationItemStatusInternalRecord.SetValue(9, integrationItemStatusInternal.Comments);

                if (integrationItemStatusInternal.StatusDimensionInternalCollection != null && integrationItemStatusInternal.StatusDimensionInternalCollection.Count > 0)
                {
                    foreach (IntegrationItemStatusDimensionInternal dimensionStatusInternal in integrationItemStatusInternal.StatusDimensionInternalCollection)
                    {
                        SqlDataRecord integrationItemDimension = new SqlDataRecord(sqlMetadataDimensions);
                        integrationItemDimension.SetValue(0, referenceId);
                        integrationItemDimension.SetValue(1, dimensionStatusInternal.IntegrationItemDimensionTypeId);
                        integrationItemDimension.SetValue(2, dimensionStatusInternal.IntegrationItemDimensionValue);

                        itemStatusDimensions.Add(integrationItemDimension);
                    }
                }
                itemStatusRecords.Add(integrationItemStatusInternalRecord);

                referenceId--;
            }
            

            if (itemStatusRecords.Count < 1)
            {
                itemStatusRecords = null;
            }
            if (itemStatusDimensions.Count < 1)
            {
                itemStatusDimensions = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integrationItemStatusSearchCriteria"></param>
        /// <param name="sqlMetadataSearchCriterial"></param>
        /// <returns></returns>
        private List<SqlDataRecord> GetSqlRecords(IntegrationItemStatusSearchCriteria integrationItemStatusSearchCriteria, SqlMetaData[] sqlMetadataSearchCriterial)
        {
            List<SqlDataRecord> sqlRecords = new List<SqlDataRecord>();

            if (integrationItemStatusSearchCriteria != null)
            {
                #region MDMObjectType search param

                foreach (IntegrationItemStatusSearchParameter searchParam in integrationItemStatusSearchCriteria.MDMObjectValues)
                {
                    SqlDataRecord record = new SqlDataRecord(sqlMetadataSearchCriterial);

                    record.SetValue(0, IntegrationItemStatusSearchObjectType.MDMObjectType.ToString());
                    record.SetValue(1, searchParam.SearchKey);
                    record.SetValue(2, ValueTypeHelper.JoinCollection<String>(searchParam.SearchValues, ","));
                    record.SetValue(3, SearchOperator.In.ToString());

                    sqlRecords.Add(record);
                }

                #endregion MDMObjectType search param

                #region ExternalObjectType search param

                foreach (IntegrationItemStatusSearchParameter searchParam in integrationItemStatusSearchCriteria.ExternalObjectValues)
                {
                    SqlDataRecord record = new SqlDataRecord(sqlMetadataSearchCriterial);

                    record.SetValue(0, IntegrationItemStatusSearchObjectType.ExternalObjectType.ToString());
                    record.SetValue(1, searchParam.SearchKey);
                    record.SetValue(2, ValueTypeHelper.JoinCollection<String>(searchParam.SearchValues, ","));
                    record.SetValue(3, SearchOperator.In.ToString());

                    sqlRecords.Add(record);
                }

                #endregion ExternalObjectType search param

                #region DimensionValue search param

                foreach (IntegrationItemStatusSearchParameter searchParam in integrationItemStatusSearchCriteria.DimensionValues)
                {
                    SqlDataRecord record = new SqlDataRecord(sqlMetadataSearchCriterial);

                    record.SetValue(0, IntegrationItemStatusSearchObjectType.DimensionValue.ToString());
                    record.SetValue(1, searchParam.SearchKey);
                    record.SetValue(2, ValueTypeHelper.JoinCollection<String>(searchParam.SearchValues, ","));
                    record.SetValue(3, SearchOperator.In.ToString());

                    sqlRecords.Add(record);
                }

                #endregion DimensionValue search param

                #region StatusType search param

                if (integrationItemStatusSearchCriteria.StatusTypes != null && integrationItemStatusSearchCriteria.StatusTypes.Count > 0)
                {
                    String commaSeparatedStatusTypeIds = String.Empty;

                    foreach (OperationResultType statusType in integrationItemStatusSearchCriteria.StatusTypes)
                    {
                        if (!String.IsNullOrWhiteSpace(commaSeparatedStatusTypeIds))
                        {
                            commaSeparatedStatusTypeIds = String.Concat(commaSeparatedStatusTypeIds, ",");
                        }

                        commaSeparatedStatusTypeIds = String.Concat(commaSeparatedStatusTypeIds, (Int32)statusType);
                    }

                    SqlDataRecord recordStatusType = new SqlDataRecord(sqlMetadataSearchCriterial);

                    recordStatusType.SetValue(0, IntegrationItemStatusSearchObjectType.StatusType.ToString());
                    recordStatusType.SetValue(1, -1);
                    recordStatusType.SetValue(2, commaSeparatedStatusTypeIds);
                    recordStatusType.SetValue(3, SearchOperator.In.ToString());

                    sqlRecords.Add(recordStatusType);
                }

                #endregion StatusType search param

                #region ItemStatusValue search param

                foreach (IntegrationItemStatusSearchParameter searchParam in integrationItemStatusSearchCriteria.StatusValues)
                {
                    SqlDataRecord record = new SqlDataRecord(sqlMetadataSearchCriterial);

                    record.SetValue(0, IntegrationItemStatusSearchObjectType.StatusValue.ToString());
                    record.SetValue(1, -1);
                    record.SetValue(2, ValueTypeHelper.JoinCollection<String>(searchParam.SearchValues, ","));
                    record.SetValue(3, SearchOperator.In.ToString());

                    sqlRecords.Add(record);
                }

                #endregion ItemStatusValue search param

                #region ItemStatusComments search param


                if (integrationItemStatusSearchCriteria.ItemStatusComments.SearchValues != null && integrationItemStatusSearchCriteria.ItemStatusComments.SearchValues.Count > 0)
                {
                    SqlDataRecord recordItemStatusComments = new SqlDataRecord(sqlMetadataSearchCriterial);

                    recordItemStatusComments.SetValue(0, IntegrationItemStatusSearchObjectType.StatusComments.ToString());
                    recordItemStatusComments.SetValue(1, -1);
                    recordItemStatusComments.SetValue(2, integrationItemStatusSearchCriteria.ItemStatusComments.SearchValues.FirstOrDefault());
                    recordItemStatusComments.SetValue(3, Utility.GetOperatorString(integrationItemStatusSearchCriteria.ItemStatusComments.Operator));

                    sqlRecords.Add(recordItemStatusComments);
                }

                #endregion ItemStatusComments  search param
            }
            if (sqlRecords.Count < 1)
            {
                sqlRecords = null;
            }
            return sqlRecords;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private IntegrationItemStatusInternalCollection ReadIntegrationItemStatusInternal(SqlDataReader reader)
        {
            IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection = new IntegrationItemStatusInternalCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int64 itemStatusId = -1;
                    Int64 mdmObjectId = -1;
                    Int16 mdmObjectTypeId = -1;
                    String mdmObjectTypeName = String.Empty;
                    String externalId = String.Empty;
                    Int16 externalObjectTypeId = -1;
                    String externalObjectTypeName = String.Empty;
                    Int16 connectorId = -1;
                    String connectorLongName = String.Empty;
                    String comments = String.Empty;
                    OperationResultType statusType = OperationResultType.Information;
                    String status = String.Empty;
                    DateTime auditTimestamp = DateTime.MinValue;

                    #endregion Declaratioin

                    #region Read values

                    if (reader["PK_Integration_ItemStatus"] != null)
                    {
                        itemStatusId = ValueTypeHelper.Int64TryParse(reader["PK_Integration_ItemStatus"].ToString(), -1);
                    }

                    if (reader["MDMObjectId"] != null)
                    {
                        mdmObjectId = ValueTypeHelper.Int64TryParse(reader["MDMObjectId"].ToString(), -1);
                    }

                    if (reader["FK_MDMObjectType"] != null)
                    {
                        mdmObjectTypeId = ValueTypeHelper.Int16TryParse(reader["FK_MDMObjectType"].ToString(), -1);
                    }

                    if (reader["MDMObjectTypeName"] != null)
                    {
                        mdmObjectTypeName = reader["MDMObjectTypeName"].ToString();
                    }

                    if (reader["ExternalId"] != null)
                    {
                        externalId = reader["ExternalId"].ToString();
                    }

                    if (reader["FK_ExternalObjectType"] != null)
                    {
                        externalObjectTypeId = ValueTypeHelper.Int16TryParse(reader["FK_ExternalObjectType"].ToString(), -1);
                    }

                    if (reader["ExternalObjectTypeName"] != null)
                    {
                        externalObjectTypeName = reader["ExternalObjectTypeName"].ToString();
                    }

                    if (reader["FK_Connector"] != null)
                    {
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);
                    }

                    if (reader["ConnectorLongName"] != null)
                    {
                        connectorLongName = reader["ConnectorLongName"].ToString();
                    }

                    if (reader["Comments"] != null)
                    {
                        comments = reader["Comments"].ToString();
                    }

                    if (reader["StatusType"] != null)
                    {
                        statusType = (OperationResultType)ValueTypeHelper.Int32TryParse(reader["StatusType"].ToString(), 2);
                    }

                    if (reader["Status"] != null)
                    {
                        status = reader["Status"].ToString();
                    }

                    if (reader["ModDateTime"] != null)
                    {
                        String val = reader["ModDateTime"].ToString();
                        if (!String.IsNullOrWhiteSpace(val))
                        {
                            auditTimestamp = ValueTypeHelper.ConvertToDateTime(val);
                        }
                    }

                    #endregion Read values

                    #region Populate object

                    IntegrationItemStatusInternal itemStatusInternal = new IntegrationItemStatusInternal();

                    itemStatusInternal.Id = itemStatusId;
                    itemStatusInternal.MDMObjectId = mdmObjectId;
                    itemStatusInternal.MDMObjectTypeId = mdmObjectTypeId;
                    itemStatusInternal.MDMObjectTypeName = mdmObjectTypeName;
                    itemStatusInternal.ExternalId = externalId;
                    itemStatusInternal.ExternalObjectTypeId = externalObjectTypeId;
                    itemStatusInternal.ExternalObjectTypeName = externalObjectTypeName;
                    itemStatusInternal.ConnectorId = connectorId;
                    itemStatusInternal.ConnectorLongName = connectorLongName;
                    itemStatusInternal.Comments = comments;
                    itemStatusInternal.StatusType = statusType;
                    itemStatusInternal.Status = status;
                    itemStatusInternal.AuditTimeStamp = auditTimestamp;

                    //itemStatusInternal.StatusDimensionCollection.Add(dimensionStatus);
                    integrationItemStatusInternalCollection.Add(itemStatusInternal);

                    #endregion Populate object
                }

                //Read item status dimension value table.
                ReadIntegrationItemStatusDimensionInternalCollection(reader, integrationItemStatusInternalCollection);
            }
            return integrationItemStatusInternalCollection;
        }

        private void ReadIntegrationItemStatusDimensionInternalCollection(SqlDataReader reader, IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection)
        {
            if (integrationItemStatusInternalCollection != null)
            {
                //Move reader to IntegrationItemStatusDimensionInternalCollection set
                reader.NextResult();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        #region Declaration

                        Int64 itemStatusId = -1;
                        Int64 itemStatusDimensionId = -1;
                        Int32 integrationItemDimensionTypeId = -1;
                        String integrationItemDimensionTypeName = String.Empty;
                        String integrationItemDimensionTypeLongName = String.Empty;
                        String dimensionValue = String.Empty;

                        #endregion Declaratioin

                        #region Read values

                        if (reader["FK_Integration_ItemStatus"] != null)
                        {
                            itemStatusId = ValueTypeHelper.Int64TryParse(reader["FK_Integration_ItemStatus"].ToString(), -1);
                        }

                        if (reader["PK_Integration_ItemStatus_Dimensions"] != null)
                        {
                            itemStatusDimensionId = ValueTypeHelper.Int64TryParse(reader["PK_Integration_ItemStatus_Dimensions"].ToString(), -1);
                        }

                        if (reader["FK_Integration_ItemDimensionType"] != null)
                        {
                            integrationItemDimensionTypeId = ValueTypeHelper.Int32TryParse(reader["FK_Integration_ItemDimensionType"].ToString(),-1);
                        }

                        if (reader["ItemDimensionTypeName"] != null)
                        {
                            integrationItemDimensionTypeName = reader["ItemDimensionTypeName"].ToString();
                        }

                        if (reader["ItemDimensionTypeLongName"] != null)
                        {
                            integrationItemDimensionTypeLongName = reader["ItemDimensionTypeLongName"].ToString();
                        }

                        if (reader["DimensionValue"] != null)
                        {
                            dimensionValue = reader["DimensionValue"].ToString();
                        }
                        
                        #endregion Read values

                        #region Populate object

                        IntegrationItemStatusDimensionInternal integrationItemStatusDimensionInternal = new IntegrationItemStatusDimensionInternal();

                        integrationItemStatusDimensionInternal.Id = itemStatusDimensionId;
                        integrationItemStatusDimensionInternal.IntegrationItemDimensionTypeId = integrationItemDimensionTypeId;
                        integrationItemStatusDimensionInternal.IntegrationItemDimensionTypeName = integrationItemDimensionTypeName;
                        integrationItemStatusDimensionInternal.IntegrationItemDimensionTypeLongName = integrationItemDimensionTypeLongName;
                        integrationItemStatusDimensionInternal.IntegrationItemDimensionValue = dimensionValue;

                        IntegrationItemStatusInternal integrationItemStatusInternal = (IntegrationItemStatusInternal)integrationItemStatusInternalCollection.GetIntegrationItemStatusInternal(itemStatusId);

                        if (integrationItemStatusInternal != null)
                        {
                            if (integrationItemStatusInternal.StatusDimensionInternalCollection == null)
                            {
                                integrationItemStatusInternal.StatusDimensionInternalCollection = new IntegrationItemStatusDimensionInternalCollection();
                            }

                            integrationItemStatusInternal.StatusDimensionInternalCollection.Add(integrationItemStatusDimensionInternal);
                        }

                        #endregion Populate object
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}