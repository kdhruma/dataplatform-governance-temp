using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using System.Diagnostics;

namespace MDM.DataModelManager.Data
{

    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Represents data access logic for RelationshipType Attribute Mapping 
    /// </summary>
    public class RelationshipTypeAttributeMappingDA : SqlClientDataAccessBase
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
        /// <param name="attributeGroupId"></param>
        /// <param name="attributeId"></param>
        /// <param name="command"></param>
        /// <returns>RelationshipTypeAttributeMappingCollection</returns>
        public RelationshipTypeAttributeMappingCollection Get(Int32 relationshipTypeId, Int32 attributeGroupId, Int32 attributeId, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = new RelationshipTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeAttributeMappingDA.Get", MDMTraceSource.DataModel, false);

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_RelationshipTypeAttribute_Get_ParametersArray");

                parameters[0].Value = relationshipTypeId;
                parameters[1].Value = attributeGroupId;
                parameters[2].Value = attributeId;

                storedProcedureName = "usp_DataModelManager_RelationshipTypeAttribute_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateRelationshipTypeAttributeMappings(reader, relationshipTypeAttributeMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeAttributeMappingDA.Get", MDMTraceSource.DataModel);
            }

            return relationshipTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>output</returns>
        public Int32 Process(RelationshipTypeAttributeMappingCollection listMappings, String loginUser, String programName)
        {
            Int32 output = 0; //success
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    String paramXML = listMappings.ToXml(ObjectSerialization.ProcessingOnly);

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    parameters = generator.GetParameters("DataModelManager_RelationshipTypeAttribute_Process_ParametersArray");

                    parameters[0].Value = paramXML;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_RelationshipTypeAttribute_Process";

                    output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
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
        public void Process(RelationshipTypeAttributeMappingCollection listMappings, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    String paramXML = listMappings.ToXml(ObjectSerialization.ProcessingOnly);

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

                    parameters = generator.GetParameters("DataModelManager_RelationshipTypeAttribute_Process_ParametersArray");

                    parameters[0].Value = paramXML;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_RelationshipTypeAttribute_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    DataModelHelper.PopulateOperationResult(reader, operationResults, listMappings);

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("RelationshipTypeAttributeMapping Process Failed with {0}", ex.Message);
                PopulateOperationResult(errorMessage, operationResults, listMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeAttributeMappingDA.Process", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="operationResults"></param>
        /// <param name="relationshipTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(relationshipTypeAttributeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="relationshipTypeAttributeMappings"></param>
        /// <returns></returns>
        private void PopulateRelationshipTypeAttributeMappings(SqlDataReader reader, RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings)
        {
            while (reader.Read())
            {
                RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = new RelationshipTypeAttributeMapping();

                if (reader["PK_RelationshipTypeAttribute"] != null)
                {
                    relationshipTypeAttributeMapping.Id = ValueTypeHelper.Int32TryParse(reader["PK_RelationshipTypeAttribute"].ToString(), relationshipTypeAttributeMapping.Id);
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    relationshipTypeAttributeMapping.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), relationshipTypeAttributeMapping.RelationshipTypeId);
                }

                if (reader["RelationshipTypeName"] != null)
                {
                    relationshipTypeAttributeMapping.RelationshipTypeName = reader["RelationshipTypeName"].ToString();
                }

                if (reader["RelationshipTypeLongName"] != null)
                {
                    relationshipTypeAttributeMapping.RelationshipTypeLongName = reader["RelationshipTypeLongName"].ToString();
                }

                if (reader["AttributeId"] != null)
                {
                    relationshipTypeAttributeMapping.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), relationshipTypeAttributeMapping.AttributeId);
                }

                if (reader["AttributeName"] != null)
                {
                    relationshipTypeAttributeMapping.AttributeName = reader["AttributeName"].ToString();
                }

                if (reader["AttributeLongName"] != null)
                {
                    relationshipTypeAttributeMapping.AttributeLongName = reader["AttributeLongName"].ToString();
                }

                if (reader["AttributeParentId"] != null)
                {
                    relationshipTypeAttributeMapping.AttributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentId"].ToString(), relationshipTypeAttributeMapping.AttributeParentId);
                }

                if (reader["AttributeParentName"] != null)
                {
                    relationshipTypeAttributeMapping.AttributeParentName = reader["AttributeParentName"].ToString();
                }

                if (reader["AttributeParentLongName"] != null)
                {
                    relationshipTypeAttributeMapping.AttributeParentLongName = reader["AttributeParentLongName"].ToString();
                }

                if (reader["AttributeSeq"] != null)
                {
                    relationshipTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(reader["AttributeSeq"].ToString(), relationshipTypeAttributeMapping.SortOrder);
                }

                if (reader["Required"] != null)
                {
                    relationshipTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(reader["Required"].ToString(), false);
                }

                if (reader["isReadOnly"] != null)
                {
                    relationshipTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(reader["isReadOnly"].ToString(), false);
                }

                if (reader["ShowAtCreation"] != null)
                {
                    relationshipTypeAttributeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(reader["ShowAtCreation"].ToString(), false);
                }

                if (reader["ShowAttributeInline"] != null)
                {
                    relationshipTypeAttributeMapping.ShowInline = ValueTypeHelper.BooleanTryParse(reader["ShowAttributeInline"].ToString(), false);
                }

                relationshipTypeAttributeMappings.Add(relationshipTypeAttributeMapping);
            }
        }

        #endregion

        #endregion
    }
}