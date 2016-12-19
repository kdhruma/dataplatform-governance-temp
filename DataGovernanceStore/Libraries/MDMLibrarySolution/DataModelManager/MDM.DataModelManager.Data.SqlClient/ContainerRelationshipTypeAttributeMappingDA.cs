using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;

namespace MDM.DataModelManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Represents data access logic for Container RelationshipType Attribute Mapping
    /// </summary>
    public class ContainerRelationshipTypeAttributeMappingDA : SqlClientDataAccessBase
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
        /// <param name="relationshipTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="attributeId"></param>
        /// <param name="command"></param>
        /// <returns>ContainerRelationshipTypeAttributeMappingCollection</returns>
        public ContainerRelationshipTypeAttributeMappingCollection Get(Int32 containerId, Int32 relationshipTypeId, Int32 attributeGroupId, Int32 attributeId, DBCommandProperties command)
        {
            String storedProcedureName = String.Empty;
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = new ContainerRelationshipTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeAttributeMappingDA.Get", MDMTraceSource.DataModel, false);

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

                parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeAttribute_Get_ParametersArray");

                parameters[0].Value = containerId;
                parameters[1].Value = relationshipTypeId;
                parameters[2].Value = attributeGroupId;
                parameters[3].Value = attributeId;

                storedProcedureName = "usp_DataModelManager_ContainerRelationshipTypeAttribute_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateContainerRelationshipTypeAttributeMappings(reader, containerRelationshipTypeAttributeMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeAttributeMappingDA.Get", MDMTraceSource.DataModel);
            }

            return containerRelationshipTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>output</returns>
        public Int32 Process(ContainerRelationshipTypeAttributeMappingCollection listMappings, String loginUser, String programName)
        {
            Int32 output = 0;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    List<SqlDataRecord> containerRelationshipTypeAttributeMappingTable;

                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeAttribute_Process_ParametersArray");
                    SqlMetaData[] containerRelationshipTypeAttributeMappingMetaData = generator.GetTableValueMetadata("DataModelManager_ContainerRelationshipTypeAttribute_Process_ParametersArray", parameters[0].ParameterName);

                    CreateTableParams(listMappings, containerRelationshipTypeAttributeMappingMetaData, out containerRelationshipTypeAttributeMappingTable);

                    parameters[0].Value = containerRelationshipTypeAttributeMappingTable;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_ContainerRelationshipTypeAttribute_Process";

                    output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public void Process(ContainerRelationshipTypeAttributeMappingCollection listMappings, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {    
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    List<SqlDataRecord> containerRelationshipTypeAttributeMappingTable;

                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeAttribute_Process_ParametersArray");
                    SqlMetaData[] containerRelationshipTypeAttributeMappingMetaData = generator.GetTableValueMetadata("DataModelManager_ContainerRelationshipTypeAttribute_Process_ParametersArray", parameters[0].ParameterName);

                    CreateTableParams(listMappings, containerRelationshipTypeAttributeMappingMetaData, out containerRelationshipTypeAttributeMappingTable);

                    parameters[0].Value = containerRelationshipTypeAttributeMappingTable;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_ContainerRelationshipTypeAttribute_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    DataModelHelper.PopulateOperationResult(reader, operationResults, listMappings);

                    transactionScope.Complete();
                 }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("ContainerRelationshipTypeAttributeMapping Process Failed with {0}", ex.Message);
                PopulateOperationResult(errorMessage, operationResults, listMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #region Private Methods

        
        /// <summary>
        /// Create Container Relationship Type Attribute Mapping TVP for process
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings">Indicates container relationship type mapping collection</param>
        /// <param name="containerRelationshipTypeMappingAttributeMetaData">Metadata of container relationship type mapping collection TVP</param>
        /// <param name="containerRelationshipTypeAttributeMappingTable">Data record of container relationship type mapping collection</param>
        private void CreateTableParams(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, SqlMetaData[] containerRelationshipTypeAttributeMappingMetaData,
                                       out List<SqlDataRecord> containerRelationshipTypeAttributeMappingTable)
        {
            containerRelationshipTypeAttributeMappingTable = new List<SqlDataRecord>();
            
            foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
            {
                if (containerRelationshipTypeAttributeMapping.Action == ObjectAction.Read || containerRelationshipTypeAttributeMapping.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                SqlDataRecord containerRelationshipTypeAttributeMappingRecord = new SqlDataRecord(containerRelationshipTypeAttributeMappingMetaData);
                containerRelationshipTypeAttributeMappingRecord.SetValue(0, ValueTypeHelper.Int32TryParse(containerRelationshipTypeAttributeMapping.ReferenceId, 0));
                containerRelationshipTypeAttributeMappingRecord.SetValue(1, containerRelationshipTypeAttributeMapping.Id);
                containerRelationshipTypeAttributeMappingRecord.SetValue(2, containerRelationshipTypeAttributeMapping.ContainerId);
                containerRelationshipTypeAttributeMappingRecord.SetValue(3, containerRelationshipTypeAttributeMapping.RelationshipTypeId);
                containerRelationshipTypeAttributeMappingRecord.SetValue(4, containerRelationshipTypeAttributeMapping.AttributeId);
                containerRelationshipTypeAttributeMappingRecord.SetValue(5, containerRelationshipTypeAttributeMapping.SortOrder);
                containerRelationshipTypeAttributeMappingRecord.SetValue(6, containerRelationshipTypeAttributeMapping.ReadOnly);
                containerRelationshipTypeAttributeMappingRecord.SetValue(7, containerRelationshipTypeAttributeMapping.Required);
                containerRelationshipTypeAttributeMappingRecord.SetValue(8, containerRelationshipTypeAttributeMapping.ShowAtCreation);
                containerRelationshipTypeAttributeMappingRecord.SetValue(9, containerRelationshipTypeAttributeMapping.ShowInline);
                containerRelationshipTypeAttributeMappingRecord.SetValue(10, containerRelationshipTypeAttributeMapping.AutoPromotable);
                containerRelationshipTypeAttributeMappingRecord.SetValue(11, containerRelationshipTypeAttributeMapping.Action.ToString());

                containerRelationshipTypeAttributeMappingTable.Add(containerRelationshipTypeAttributeMappingRecord);
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="operationResults"></param>
        /// <param name="containerRelationshipTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(containerRelationshipTypeAttributeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateContainerRelationshipTypeAttributeMappings(SqlDataReader reader, ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings)
        {
            while (reader.Read())
            {
                ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = new ContainerRelationshipTypeAttributeMapping();

                if (reader["PK_CatalogRelTypeAttr"] != null)
                {
                    containerRelationshipTypeAttributeMapping.Id = ValueTypeHelper.Int32TryParse(reader["PK_CatalogRelTypeAttr"].ToString(), containerRelationshipTypeAttributeMapping.Id);
                }

                if (reader["OrganizationId"] != null)
                {
                    containerRelationshipTypeAttributeMapping.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), containerRelationshipTypeAttributeMapping.OrganizationId);
                }

                if (reader["OrganizationName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.OrganizationName = reader["OrganizationName"].ToString();
                }

                if (reader["OrganizationLongName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.OrganizationLongName = reader["OrganizationLongName"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    containerRelationshipTypeAttributeMapping.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerRelationshipTypeAttributeMapping.ContainerId);
                }

                if (reader["ContainerName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.ContainerName = reader["ContainerName"].ToString();
                }

                if (reader["ContainerLongName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.ContainerLongName = reader["ContainerLongName"].ToString();
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    containerRelationshipTypeAttributeMapping.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), containerRelationshipTypeAttributeMapping.RelationshipTypeId);
                }

                if (reader["RelationshipTypeName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.RelationshipTypeName = reader["RelationshipTypeName"].ToString();
                }

                if (reader["RelationshipTypeLongName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.RelationshipTypeLongName = reader["RelationshipTypeLongName"].ToString();
                }

                if (reader["AttributeId"] != null)
                {
                    containerRelationshipTypeAttributeMapping.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), containerRelationshipTypeAttributeMapping.AttributeId);
                }

                if (reader["AttributeName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.AttributeName = reader["AttributeName"].ToString();
                }

                if (reader["AttributeLongName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.AttributeLongName = reader["AttributeLongName"].ToString();
                }

                if (reader["AttributeParentId"] != null)
                {
                    containerRelationshipTypeAttributeMapping.AttributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentId"].ToString(), containerRelationshipTypeAttributeMapping.AttributeParentId);
                }

                if (reader["AttributeParentName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.AttributeParentName = reader["AttributeParentName"].ToString();
                }

                if (reader["AttributeParentLongName"] != null)
                {
                    containerRelationshipTypeAttributeMapping.AttributeParentLongName = reader["AttributeParentLongName"].ToString();
                }

                if (reader["AttributeSeq"] != null)
                {
                    containerRelationshipTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(reader["AttributeSeq"].ToString(), containerRelationshipTypeAttributeMapping.SortOrder);
                }

                if (reader["Required"] != null)
                {
                    containerRelationshipTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(reader["Required"].ToString(), false);
                }

                if (reader["isReadOnly"] != null)
                {
                    containerRelationshipTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(reader["isReadOnly"].ToString(), false);
                }

                if (reader["ShowAtCreation"] != null)
                {
                    containerRelationshipTypeAttributeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(reader["ShowAtCreation"].ToString(), false);
                }

                if (reader["ShowAttributeInline"] != null)
                {
                    containerRelationshipTypeAttributeMapping.ShowInline = ValueTypeHelper.BooleanTryParse(reader["ShowAttributeInline"].ToString(), false);
                }

                if (reader["IsSpecialized"] != null)
                {
                    containerRelationshipTypeAttributeMapping.IsSpecialized = ValueTypeHelper.BooleanTryParse(reader["IsSpecialized"].ToString(), false);
                }

                if (reader["AutoPromotable"] != null)
                {
                    containerRelationshipTypeAttributeMapping.AutoPromotable = ValueTypeHelper.BooleanTryParse(reader["AutoPromotable"].ToString(), false);
                }

                containerRelationshipTypeAttributeMappings.Add(containerRelationshipTypeAttributeMapping);
            }
        }

        #endregion

        #endregion
    }
}