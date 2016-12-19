using System;
using System.Transactions;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Represents data access logic for Container EntityType mapping
    /// </summary>
    public class ContainerEntityTypeMappingDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get the container entity type mapping collection based on organization id, container id and entity type id.
        /// </summary>
        /// <param name="organizationId">Indicates the organization identifier</param>
        /// <param name="containerId">Indicates the container identifier</param>
        /// <param name="entityTypeId">Indicates the entity type identifier</param>
        /// <returns>Returns the container entity type mapping collection based on organization id, container id and entity type id.</returns>
        public ContainerEntityTypeMappingCollection Get(Int32 organizationId, Int32 containerId, Int32 entityTypeId)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            ContainerEntityTypeMappingCollection containerEntityTypeMappings = new ContainerEntityTypeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerEntityTypeMappingDA.Get", MDMTraceSource.DataModel, false);

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_ContainerEntityType_Get_ParametersArray");

                parameters[0].Value = organizationId;
                parameters[1].Value = containerId;
                parameters[2].Value = entityTypeId;

                storedProcedureName = "usp_DataModelManager_ContainerEntityType_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);
                PopulateContainerEntityTypeMappings(reader, containerEntityTypeMappings);

            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerEntityTypeMappingDA.Get", MDMTraceSource.DataModel);
            }

            return containerEntityTypeMappings;
        }

        /// <summary>
        /// Process container entity type mapping collection
        /// </summary>
        /// <param name="containerEntityTypeMappings">Indicates the container entity type mapping collection to be processed.</param>
        /// <param name="loginUser">Indicates the login user name</param>
        /// <param name="programName">Indicates the program name which initiates the process operation</param>
        /// <returns>Returns boolean value indicating if the process is successful</returns>
        public Int32 Process(ContainerEntityTypeMappingCollection containerEntityTypeMappings, String loginUser, String programName)
        {
            Int32 output = 0; //success
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerEntityTypeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    List<SqlDataRecord> containerEntityTypeMappingTable;

                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_ContainerEntityType_Process_ParametersArray");
                    SqlMetaData[] containerEntityTypeMappingMetaData = generator.GetTableValueMetadata("DataModelManager_ContainerEntityType_Process_ParametersArray", parameters[0].ParameterName);

                    CreateTableParams(containerEntityTypeMappings, containerEntityTypeMappingMetaData, out containerEntityTypeMappingTable);

                    parameters[0].Value = containerEntityTypeMappingTable;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_ContainerEntityType_Process";

                    output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);
                    
                    transactionScope.Complete();
                }
            } 
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerEntityTypeMappingDA.Process", MDMTraceSource.DataModel);
            }

            return output;
        }

        /// <summary>
        /// Process container entity type mapping collection
        /// </summary>
        /// <param name="containerEntityTypeMappings">Indicates the container entity type mapping collection to be processed.</param>
        /// <param name="operationResults">Indicate the result of the process operation</param>
        /// <param name="loginUser">Indicates the login user name</param>
        /// <param name="programName">Indicates the program name which initiates the process operation</param>
        public void Process(ContainerEntityTypeMappingCollection containerEntityTypeMappings, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerEntityTypeMappingDA.Process", MDMTraceSource.DataModel, false);
                }

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                        connectionString = AppConfigurationHelper.ConnectionString;

                        SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                        List<SqlDataRecord> containerEntityTypeMappingTable;

                        SqlParameter[] parameters = generator.GetParameters("DataModelManager_ContainerEntityType_Process_ParametersArray");
                        SqlMetaData[] containerEntityTypeMappingMetaData = generator.GetTableValueMetadata("DataModelManager_ContainerEntityType_Process_ParametersArray", parameters[0].ParameterName);

                        CreateTableParams(containerEntityTypeMappings, containerEntityTypeMappingMetaData, out containerEntityTypeMappingTable);

                        parameters[0].Value = containerEntityTypeMappingTable;
                        parameters[1].Value = loginUser;
                        parameters[2].Value = programName;

                        storedProcedureName = "usp_DataModelManager_ContainerEntityType_Process";

                        reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);
                        DataModelHelper.PopulateOperationResult(reader, operationResults, containerEntityTypeMappings);

                        transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("ContainerEntityTypeMapping Process Failed with {0}", ex.Message);
                PopulateOperationResult(errorMessage, operationResults, containerEntityTypeMappings);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerEntityTypeMappingDA.Process", MDMTraceSource.DataModel);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create Container Entity Type Mapping TVP for process
        /// </summary>
        /// <param name="containerEntityTypeMappings">Indicates container entity type mapping collection</param>
        /// <param name="containerEntityTypeMappingMetaData">Indicates the metadata of container entity type mapping TVP</param>
        /// <param name="containerEntityTypeMappingTable">Indicates the data record of container entity type mapping</param>
        private void CreateTableParams(ContainerEntityTypeMappingCollection containerEntityTypeMappings, SqlMetaData[] containerEntityTypeMappingMetaData,
                                       out List<SqlDataRecord> containerEntityTypeMappingTable)
        {
            containerEntityTypeMappingTable = new List<SqlDataRecord>();

            foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
            {
                if (containerEntityTypeMapping.Action == ObjectAction.Read || containerEntityTypeMapping.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                SqlDataRecord containerEntityTypeMappingRecord = new SqlDataRecord(containerEntityTypeMappingMetaData);
                containerEntityTypeMappingRecord.SetValue(0, ValueTypeHelper.Int32TryParse(containerEntityTypeMapping.ReferenceId, 0));
                containerEntityTypeMappingRecord.SetValue(1, containerEntityTypeMapping.Id);
                containerEntityTypeMappingRecord.SetValue(2, containerEntityTypeMapping.ContainerId);
                containerEntityTypeMappingRecord.SetValue(3, containerEntityTypeMapping.OrganizationId);
                containerEntityTypeMappingRecord.SetValue(4, containerEntityTypeMapping.EntityTypeId);
                containerEntityTypeMappingRecord.SetValue(5, containerEntityTypeMapping.ShowAtCreation);
                containerEntityTypeMappingRecord.SetValue(6, containerEntityTypeMapping.MinimumExtensions);
                containerEntityTypeMappingRecord.SetValue(7, containerEntityTypeMapping.MaximumExtensions);
                containerEntityTypeMappingRecord.SetValue(8, containerEntityTypeMapping.Action.ToString());

                containerEntityTypeMappingTable.Add(containerEntityTypeMappingRecord);
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (containerEntityTypeMappingTable.Count == 0)
            {
                containerEntityTypeMappingTable = null;
            }
        }

        private void PopulateContainerEntityTypeMappings(SqlDataReader reader, ContainerEntityTypeMappingCollection containerEntityTypeMappings)
        {
            while (reader.Read())
            {
                ContainerEntityTypeMapping containerEntityTypeMapping = new ContainerEntityTypeMapping();

                if (reader["PK_ContainerEntityType"] != null)
                {
                    containerEntityTypeMapping.Id = ValueTypeHelper.Int32TryParse(reader["PK_ContainerEntityType"].ToString(), containerEntityTypeMapping.Id);
                }

                if (reader["OrganizationId"] != null)
                {
                    containerEntityTypeMapping.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), containerEntityTypeMapping.OrganizationId);
                }

                if (reader["OrganizationName"] != null)
                {
                    containerEntityTypeMapping.OrganizationName = reader["OrganizationName"].ToString();
                }

                if (reader["OrganizationLongName"] != null)
                {
                    containerEntityTypeMapping.OrganizationLongName = reader["OrganizationLongName"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    containerEntityTypeMapping.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerEntityTypeMapping.ContainerId);
                }

                if (reader["ContainerName"] != null)
                {
                    containerEntityTypeMapping.ContainerName = reader["ContainerName"].ToString();
                }

                if (reader["ContainerLongName"] != null)
                {
                    containerEntityTypeMapping.ContainerLongName = reader["ContainerLongName"].ToString();
                }

                if (reader["EntityTypeId"] != null)
                {
                    containerEntityTypeMapping.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), containerEntityTypeMapping.EntityTypeId);
                }

                if (reader["EntityTypeName"] != null)
                {
                    containerEntityTypeMapping.EntityTypeName = reader["EntityTypeName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    containerEntityTypeMapping.EntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["ShowAtCreation"] != null)
                {
                   containerEntityTypeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(reader["ShowAtCreation"].ToString(), false);
                }

                if (reader["MinimumExtensions"] != null)
                {
                    containerEntityTypeMapping.MinimumExtensions = ValueTypeHelper.Int32TryParse(reader["MinimumExtensions"].ToString(), 0);
                }

                if (reader["MaximumExtensions"] != null)
                {
                    containerEntityTypeMapping.MaximumExtensions = ValueTypeHelper.Int32TryParse(reader["MaximumExtensions"].ToString(), 0);
                }

                containerEntityTypeMappings.Add(containerEntityTypeMapping);
            }
        }

        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, ContainerEntityTypeMappingCollection containerEntityTypeMappings)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(containerEntityTypeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }

        #endregion

        #endregion
    }
}