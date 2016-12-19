using System;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using System.Diagnostics;
using System.Linq;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Represents data access logic for RelationshipType EntityType mapping
    /// </summary>
    public class RelationshipTypeEntityTypeMappingDA : SqlClientDataAccessBase
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
        /// <param name="relationshipTypeId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns>RelationshipTypeEntityTypeMappingCollection</returns>
        public RelationshipTypeEntityTypeMappingCollection Get(Int32 relationshipTypeId, Int32 entityTypeId)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingDA.Get", MDMTraceSource.DataModel, false);

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_RelationshipTypeEntityType_Get_ParametersArray");

                parameters[0].Value = relationshipTypeId;
                parameters[1].Value = entityTypeId;

                storedProcedureName = "usp_DataModelManager_RelationshipTypeEntityType_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                PopulateRelationshipTypeEntityTypeMappings(reader, relationshipTypeEntityTypeMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingDA.Get", MDMTraceSource.DataModel);
            }

            return relationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>output</returns>
        public Int32 Process(RelationshipTypeEntityTypeMappingCollection listMappings, String loginUser, String programName)
        {
            Int32 output = 0;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    String paramXML = ConvertToXML(listMappings);

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    parameters = generator.GetParameters("DataModelManager_RelationshipTypeEntityType_Process_ParametersArray");

                    parameters[0].Value = paramXML;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_RelationshipTypeEntityType_Process";

                    output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel);
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
        public void Process(RelationshipTypeEntityTypeMappingCollection listMappings, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    parameters = generator.GetParameters("DataModelManager_RelationshipTypeEntityType_Process_ParametersArray");

                    parameters[0].Value = ConvertToXML(listMappings);
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_RelationshipTypeEntityType_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    DataModelHelper.PopulateOperationResult(reader, operationResults, listMappings);

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("RelationshipTypeEntityTypeMapping Process Failed with {0}", ex.Message);
                PopulateOperationResult(errorMessage, operationResults, listMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="relationshipTypeEntityTypeMappings"></param>
        /// <returns></returns>
        private void PopulateRelationshipTypeEntityTypeMappings(SqlDataReader reader, RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings)
        {
            while (reader.Read())
            {
                RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping();

                if (reader["PK_RelationshipTypeEntityType"] != null)
                {
                    relationshipTypeEntityTypeMapping.Id = ValueTypeHelper.Int32TryParse(reader["PK_RelationshipTypeEntityType"].ToString(), relationshipTypeEntityTypeMapping.Id);
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    relationshipTypeEntityTypeMapping.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), relationshipTypeEntityTypeMapping.RelationshipTypeId);
                }

                if (reader["RelationshipTypeName"] != null)
                {
                    relationshipTypeEntityTypeMapping.RelationshipTypeName = reader["RelationshipTypeName"].ToString();
                }

                if (reader["RelationshipTypeLongName"] != null)
                {
                    relationshipTypeEntityTypeMapping.RelationshipTypeLongName = reader["RelationshipTypeLongName"].ToString();
                }

                if (reader["EntityTypeId"] != null)
                {
                    relationshipTypeEntityTypeMapping.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), relationshipTypeEntityTypeMapping.EntityTypeId);
                }

                if (reader["EntityTypeName"] != null)
                {
                    relationshipTypeEntityTypeMapping.EntityTypeName = reader["EntityTypeName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    relationshipTypeEntityTypeMapping.EntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["ShowAtChildren"] != null)
                {
                    relationshipTypeEntityTypeMapping.DrillDown = ValueTypeHelper.BooleanTryParse(reader["ShowAtChildren"].ToString(), relationshipTypeEntityTypeMapping.DrillDown);
                }

                if (reader["isDefaultRelation"] != null)
                {
                    relationshipTypeEntityTypeMapping.IsDefaultRelation = ValueTypeHelper.BooleanTryParse(reader["isDefaultRelation"].ToString(), relationshipTypeEntityTypeMapping.IsDefaultRelation);
                }

                if (reader["isExcludable"] != null)
                {
                    relationshipTypeEntityTypeMapping.Excludable = ValueTypeHelper.BooleanTryParse(reader["isExcludable"].ToString(), relationshipTypeEntityTypeMapping.Excludable);
                }

                if (reader["isReadOnly"] != null)
                {
                    relationshipTypeEntityTypeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(reader["isReadOnly"].ToString(), relationshipTypeEntityTypeMapping.ReadOnly);
                }

                if (reader["ValidationRequired"] != null)
                {
                    relationshipTypeEntityTypeMapping.ValidationRequired = ValueTypeHelper.BooleanTryParse(reader["ValidationRequired"].ToString(), relationshipTypeEntityTypeMapping.ValidationRequired);
                }

                if (reader["ShowValidFlagInGrid"] != null)
                {
                    relationshipTypeEntityTypeMapping.ShowValidFlagInGrid = ValueTypeHelper.BooleanTryParse(reader["ShowValidFlagInGrid"].ToString(), relationshipTypeEntityTypeMapping.ShowValidFlagInGrid);
                }

                relationshipTypeEntityTypeMappings.Add(relationshipTypeEntityTypeMapping);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="operationResults"></param>
        /// <param name="relationshipTypeEntityTypeMappings"></param>
        /// <returns></returns>
        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in relationshipTypeEntityTypeMappings)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(relationshipTypeEntityTypeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <returns></returns>
        private String ConvertToXML(RelationshipTypeEntityTypeMappingCollection listMappings)
        {
            String xml = "<RelationshipTypeEntityTypes>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (RelationshipTypeEntityTypeMapping mapping in listMappings)
            {
                stringBuilder.Append(mapping.ToXML());

            }
            stringBuilder.Append("</RelationshipTypeEntityTypes>");

            return stringBuilder.ToString();
        }

        #endregion

        #endregion
    }
}