using System;
using System.Transactions;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;
    using MDM.Interfaces;
    
    /// <summary>
    /// Represents data access logic for Container EntityType Attribute Mapping
    /// </summary>
    public class ContainerEntityTypeAttributeMappingDA : SqlClientDataAccessBase
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
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="attributeId"></param>
        /// <param name="command"></param>
        /// <returns>ContainerEntityTypeAttributeMappingCollection</returns>
        public ContainerEntityTypeAttributeMappingCollection Get(Int32 containerId, Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerEntityTypeAttributeMappingDA.Get", MDMTraceSource.DataModel, false);

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_ContainerEntityTypeAttribute_Get_ParametersArray");

                parameters[0].Value = containerId;
                parameters[1].Value = entityTypeId;
                parameters[2].Value = attributeGroupId;
                parameters[3].Value = attributeId;

                storedProcedureName = "usp_DataModelManager_ContainerEntityTypeAttribute_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateContainerEntityTypeAttributeMappings(reader, containerEntityTypeAttributeMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerEntityTypeAttributeMappingDA.Get", MDMTraceSource.DataModel);
            }
            return containerEntityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>output</returns>
        public Int32 Process(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, String loginUser, String programName)
        {

            Int32 output = 0; //success
          
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {   
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerEntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

                    List<SqlDataRecord> containerEntityTypeAttributeMappingTable;

                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_ContainerEntityTypeAttribute_Process_ParametersArray");
                    SqlMetaData[] containerEntityTypeAttributeMappingMetaData = generator.GetTableValueMetadata("DataModelManager_ContainerEntityTypeAttribute_Process_ParametersArray", parameters[0].ParameterName);

                    CreateTableParams(containerEntityTypeAttributeMappings, containerEntityTypeAttributeMappingMetaData, out containerEntityTypeAttributeMappingTable);

                    parameters[0].Value = containerEntityTypeAttributeMappingTable;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_ContainerEntityTypeAttribute_Process";

                    output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerEntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public void Process(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerEntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {

                    connectionString = AppConfigurationHelper.ConnectionString;

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    List<SqlDataRecord> containerEntityTypeAttributeMappingTable;

                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_ContainerEntityTypeAttribute_Process_ParametersArray");
                    SqlMetaData[] containerEntityTypeAttributeMappingMetaData = generator.GetTableValueMetadata("DataModelManager_ContainerEntityTypeAttribute_Process_ParametersArray", parameters[0].ParameterName);

                    CreateTableParams(containerEntityTypeAttributeMappings, containerEntityTypeAttributeMappingMetaData, out containerEntityTypeAttributeMappingTable);

                    //Need to convert it to TVP
                    parameters[0].Value = containerEntityTypeAttributeMappingTable;
                    //parameters[0].Value = containerEntityTypeAttributeMappings.ToXml();

                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_ContainerEntityTypeAttribute_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    DataModelHelper.PopulateOperationResult(reader, operationResults, containerEntityTypeAttributeMappings);
                   
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("ContainerEntityTypeAttributeMapping Process Failed with {0}", ex.Message);
                PopulateOperationResult(errorMessage, operationResults, containerEntityTypeAttributeMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerEntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create Container Entity Type Attribute Mapping TVP for process
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">Indicates container entity type mapping collection</param>
        /// <param name="containerEntityTypeMappingAttributeMetaData">Metadata of container entity type mapping collection TVP</param>
        /// <param name="containerEntityTypeAttributeMappingTable">Data record of container entity type mapping collection</param>
        private void CreateTableParams(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, SqlMetaData[] containerEntityTypeAttributeMappingMetaData,
                                       out List<SqlDataRecord> containerEntityTypeAttributeMappingTable)
        {
            containerEntityTypeAttributeMappingTable = new List<SqlDataRecord>();
            Int32 inheritanceModeId = 2;

            foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
            {
                if (containerEntityTypeAttributeMapping.Action == ObjectAction.Read || containerEntityTypeAttributeMapping.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                SqlDataRecord containerEntityTypeAttributeMappingRecord = new SqlDataRecord(containerEntityTypeAttributeMappingMetaData);
                containerEntityTypeAttributeMappingRecord.SetValue(0, ValueTypeHelper.Int32TryParse(containerEntityTypeAttributeMapping.ReferenceId, 0));
                containerEntityTypeAttributeMappingRecord.SetValue(1, containerEntityTypeAttributeMapping.Id);
                containerEntityTypeAttributeMappingRecord.SetValue(2, containerEntityTypeAttributeMapping.ContainerId);
                containerEntityTypeAttributeMappingRecord.SetValue(3, containerEntityTypeAttributeMapping.EntityTypeId);
                containerEntityTypeAttributeMappingRecord.SetValue(4, containerEntityTypeAttributeMapping.AttributeId);
                containerEntityTypeAttributeMappingRecord.SetValue(5, inheritanceModeId);
                containerEntityTypeAttributeMappingRecord.SetValue(6, containerEntityTypeAttributeMapping.SortOrder);
                containerEntityTypeAttributeMappingRecord.SetValue(7, containerEntityTypeAttributeMapping.ReadOnly);
                containerEntityTypeAttributeMappingRecord.SetValue(8, containerEntityTypeAttributeMapping.Required);
                containerEntityTypeAttributeMappingRecord.SetValue(9, containerEntityTypeAttributeMapping.ShowAtCreation);
                containerEntityTypeAttributeMappingRecord.SetValue(10, containerEntityTypeAttributeMapping.Extension);
                containerEntityTypeAttributeMappingRecord.SetValue(11, containerEntityTypeAttributeMapping.InheritableOnly);
                containerEntityTypeAttributeMappingRecord.SetValue(12, containerEntityTypeAttributeMapping.AutoPromotable);
                containerEntityTypeAttributeMappingRecord.SetValue(13, containerEntityTypeAttributeMapping.Action.ToString());

                containerEntityTypeAttributeMappingTable.Add(containerEntityTypeAttributeMappingRecord);
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (containerEntityTypeAttributeMappingTable.Count == 0)
            {
                containerEntityTypeAttributeMappingTable = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="operationResults"></param>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(containerEntityTypeAttributeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateContainerEntityTypeAttributeMappings(SqlDataReader reader, ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings)
        {
            while (reader.Read())
            {
                ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping = new ContainerEntityTypeAttributeMapping();

                if (reader["PK_CatalogNodeTypeAttr"] != null)
                {
                    containerEntityTypeAttributeMapping.Id = ValueTypeHelper.Int32TryParse(reader["PK_CatalogNodeTypeAttr"].ToString(), containerEntityTypeAttributeMapping.Id);
                }

                if (reader["OrganizationId"] != null)
                {
                    containerEntityTypeAttributeMapping.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), containerEntityTypeAttributeMapping.OrganizationId);
                }

                if (reader["OrganizationName"] != null)
                {
                    containerEntityTypeAttributeMapping.OrganizationName = reader["OrganizationName"].ToString();
                }

                if (reader["OrganizationLongName"] != null)
                {
                    containerEntityTypeAttributeMapping.OrganizationLongName = reader["OrganizationLongName"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    containerEntityTypeAttributeMapping.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerEntityTypeAttributeMapping.ContainerId);
                }

                if (reader["ContainerName"] != null)
                {
                    containerEntityTypeAttributeMapping.ContainerName = reader["ContainerName"].ToString();
                }

                if (reader["ContainerLongName"] != null)
                {
                    containerEntityTypeAttributeMapping.ContainerLongName = reader["ContainerLongName"].ToString();
                }

                if (reader["EntityTypeId"] != null)
                {
                    containerEntityTypeAttributeMapping.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), containerEntityTypeAttributeMapping.EntityTypeId);
                }

                if (reader["EntityTypeName"] != null)
                {
                    containerEntityTypeAttributeMapping.EntityTypeName = reader["EntityTypeName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    containerEntityTypeAttributeMapping.EntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["AttributeId"] != null)
                {
                    containerEntityTypeAttributeMapping.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), containerEntityTypeAttributeMapping.AttributeId);
                }

                if (reader["AttributeName"] != null)
                {
                    containerEntityTypeAttributeMapping.AttributeName = reader["AttributeName"].ToString();
                }

                if (reader["AttributeLongName"] != null)
                {
                    containerEntityTypeAttributeMapping.AttributeLongName = reader["AttributeLongName"].ToString();
                }

                if (reader["AttributeParentId"] != null)
                {
                    containerEntityTypeAttributeMapping.AttributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentId"].ToString(), containerEntityTypeAttributeMapping.AttributeParentId);
                }

                if (reader["AttributeParentName"] != null)
                {
                    containerEntityTypeAttributeMapping.AttributeParentName = reader["AttributeParentName"].ToString();
                }

                if (reader["AttributeParentLongName"] != null)
                {
                    containerEntityTypeAttributeMapping.AttributeParentLongName = reader["AttributeParentLongName"].ToString();
                }

                if (reader["AttributeSeq"] != null)
                {
                    containerEntityTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(reader["AttributeSeq"].ToString(), containerEntityTypeAttributeMapping.SortOrder);
                }

                if (reader["Required"] != null)
                {
                    containerEntityTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(reader["Required"].ToString(), false);
                }

                if (reader["isReadOnly"] != null)
                {
                    containerEntityTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(reader["isReadOnly"].ToString(), false);
                }

                if (reader["ShowAtCreation"] != null)
                {
                    containerEntityTypeAttributeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(reader["ShowAtCreation"].ToString(), false);
                }

                if (reader["Extension"] != null)
                {
                    containerEntityTypeAttributeMapping.Extension = reader["Extension"].ToString();
                }

                if (reader["IsSpecialized"] != null)
                {
                    containerEntityTypeAttributeMapping.IsSpecialized = ValueTypeHelper.BooleanTryParse(reader["IsSpecialized"].ToString(), false);
                }

                if (reader["InheritableOnly"] != null)
                {
                    containerEntityTypeAttributeMapping.InheritableOnly = ValueTypeHelper.BooleanTryParse(reader["InheritableOnly"].ToString(), false);
                }

                if (reader["AutoPromotable"] != null)
                {
                    containerEntityTypeAttributeMapping.AutoPromotable = ValueTypeHelper.BooleanTryParse(reader["AutoPromotable"].ToString(), false);
                }

                containerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMapping);
            }
        }

        #endregion

        #endregion
    }
}