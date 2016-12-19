using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.DataModelManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Represents data access logic for EntityType Attribute Mapping
    /// </summary>
    public class EntityTypeAttributeMappingDA : SqlClientDataAccessBase
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
        /// <param name="entityTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="attributeId"></param>
        /// <param name="command"></param>
        /// <returns>EntityTypeAttributeMappingCollection</returns>
        public EntityTypeAttributeMappingCollection Get(Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            EntityTypeAttributeMappingCollection entityTypeAttributeMappings = new EntityTypeAttributeMappingCollection(); ;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.EntityTypeAttributeMappingDA.Get", MDMTraceSource.DataModel, false);

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_EntityTypeAttribute_Get_ParametersArray");

                parameters[0].Value = entityTypeId;
                parameters[1].Value = attributeGroupId;
                parameters[2].Value = attributeId;

                storedProcedureName = "usp_DataModelManager_EntityTypeAttribute_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateEntityTypeAttributeMappings(reader, entityTypeAttributeMappings);
 
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.EntityTypeAttributeMappingDA.Get", MDMTraceSource.DataModel);
            }

            return entityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>output</returns>
        public Int32 Process(EntityTypeAttributeMappingCollection listMappings, String loginUser, String programName)
        {
            Int32 output = 0; //success
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.EntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    String paramXML = listMappings.ToXml(ObjectSerialization.ProcessingOnly);

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    parameters = generator.GetParameters("DataModelManager_EntityTypeAttribute_Process_ParametersArray");

                    parameters[0].Value = paramXML;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_EntityTypeAttribute_Process";

                    output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.EntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="operationResults"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public void Process(EntityTypeAttributeMappingCollection listMappings, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.EntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    String paramXML = listMappings.ToXml(ObjectSerialization.ProcessingOnly);

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    parameters = generator.GetParameters("DataModelManager_EntityTypeAttribute_Process_ParametersArray");

                    parameters[0].Value = paramXML;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_EntityTypeAttribute_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    DataModelHelper.PopulateOperationResult(reader, operationResults, listMappings);
  
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("EntityTypeAttributeMapping Process Failed with {0}", ex.Message);
                PopulateOperationResult(errorMessage, operationResults, listMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.EntityTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="operationResults"></param>
        /// <param name="entityTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, EntityTypeAttributeMappingCollection entityTypeAttributeMappings)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(entityTypeAttributeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateEntityTypeAttributeMappings(SqlDataReader reader, EntityTypeAttributeMappingCollection entityTypeAttributeMappings)
        {
            while (reader.Read())
            {
                EntityTypeAttributeMapping entityTypeAttributeMapping = new EntityTypeAttributeMapping();

                if (reader["PK_EntityTypeAttribute"] != null)
                {
                    entityTypeAttributeMapping.Id = ValueTypeHelper.Int32TryParse(reader["PK_EntityTypeAttribute"].ToString(), entityTypeAttributeMapping.Id);
                }

                if (reader["EntityTypeId"] != null)
                {
                    entityTypeAttributeMapping.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), entityTypeAttributeMapping.EntityTypeId);
                }

                if (reader["EntityTypeName"] != null)
                {
                    entityTypeAttributeMapping.EntityTypeName = reader["EntityTypeName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    entityTypeAttributeMapping.EntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["AttributeId"] != null)
                {
                    entityTypeAttributeMapping.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), entityTypeAttributeMapping.AttributeId);
                }

                if (reader["AttributeName"] != null)
                {
                    entityTypeAttributeMapping.AttributeName = reader["AttributeName"].ToString();
                }

                if (reader["AttributeLongName"] != null)
                {
                    entityTypeAttributeMapping.AttributeLongName = reader["AttributeLongName"].ToString();
                }

                if (reader["AttributeParentId"] != null)
                {
                    entityTypeAttributeMapping.AttributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentId"].ToString(), entityTypeAttributeMapping.AttributeParentId);
                }

                if (reader["AttributeParentName"] != null)
                {
                    entityTypeAttributeMapping.AttributeParentName = reader["AttributeParentName"].ToString();
                }

                if (reader["AttributeParentLongName"] != null)
                {
                    entityTypeAttributeMapping.AttributeParentLongName = reader["AttributeParentLongName"].ToString();
                }

                if (reader["AttributeSeq"] != null)
                {
                    entityTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(reader["AttributeSeq"].ToString(), entityTypeAttributeMapping.SortOrder);
                }

                if (reader["Required"] != null)
                {
                    entityTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(reader["Required"].ToString(), false);
                }

                if (reader["isReadOnly"] != null)
                {
                    entityTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(reader["isReadOnly"].ToString(), false);
                }

                if (reader["ShowAtCreation"] != null)
                {
                    entityTypeAttributeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(reader["ShowAtCreation"].ToString(), false);
                }

                if (reader["Extension"] != null)
                {
                    entityTypeAttributeMapping.Extension = reader["Extension"].ToString();
                }

                entityTypeAttributeMappings.Add(entityTypeAttributeMapping);
            }
        }


        #endregion

        #endregion
    }
}