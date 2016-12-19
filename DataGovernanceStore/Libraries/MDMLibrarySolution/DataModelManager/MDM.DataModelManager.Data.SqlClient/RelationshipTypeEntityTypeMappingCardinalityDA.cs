using System;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using System.Xml;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using System.Diagnostics;

    /// <summary>
    /// Represents data access logic for RelationshipType EntityType Cardinality
    /// </summary>
    public class RelationshipTypeEntityTypeMappingCardinalityDA : SqlClientDataAccessBase
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
        /// Get RelationshipTypeEntityTypeMappingCardinalityCollection based on  RelationshipType Id and  From EntityType Id
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        /// <returns>RelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        public RelationshipTypeEntityTypeMappingCardinalityCollection Get(Int32 entityTypeId, Int32 relationshipTypeId)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalities = new RelationshipTypeEntityTypeMappingCardinalityCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingCardinalityDA.Get", MDMTraceSource.DataModel, false);

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_RelationshipTypeEntityTypeCardinality_Get_ParametersArray");

                parameters[0].Value = entityTypeId;
                parameters[1].Value = relationshipTypeId;

                storedProcedureName = "usp_DataModelManager_RelationshipTypeEntityTypeCardinality_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                PopulateRelationshipTypeEntityTypeMappingCardinalities(reader, relationshipTypeEntityTypeMappingCardinalities);

            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingCardinalityDA.Get", MDMTraceSource.DataModel);

            }

            return relationshipTypeEntityTypeMappingCardinalities;
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType EntityType mappings
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>Result of the operation Collection</returns>
        public OperationResultCollection Process(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys, String loginUser, String programName)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            OperationResult operationResult = null;
            OperationResultCollection operationResults = new OperationResultCollection();

            foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalitys)
            {
                try
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel, false);

                    operationResult = new OperationResult();

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        connectionString = AppConfigurationHelper.ConnectionString;

                        SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                        parameters = generator.GetParameters("DataModelManager_RelationshipTypeEntityTypeCardinality_Process_ParametersArray");

                        parameters[0].Value = GetLegacyXml(relationshipTypeEntityTypeMappingCardinality);
                        parameters[1].Value = loginUser;
                        parameters[2].Value = programName;

                        storedProcedureName = "usp_DataModelManager_RelationshipTypeEntityTypeCardinality_Process";

                        reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                        DataModelHelper.PopulateOperationResult(reader, operationResult);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    String errorMessage = String.Format("RelationshipTypeEntityTypeMappingCardinality Process Failed with {0}", exception.Message);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }
                finally
                {
                    operationResults.Add(operationResult);

                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel);
                }
            }

            return operationResults;
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType EntityType mappings
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="operationResults"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public void Process(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            OperationResult operationResult = null;

            foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalitys)
            {
                try
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StartTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel, false);
     
                    operationResult = (OperationResult)operationResults.GetByReferenceId(relationshipTypeEntityTypeMappingCardinality.ReferenceId);

                     using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        connectionString = AppConfigurationHelper.ConnectionString;

                        SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                        parameters = generator.GetParameters("DataModelManager_RelationshipTypeEntityTypeCardinality_Process_ParametersArray");

                        parameters[0].Value = GetLegacyXml(relationshipTypeEntityTypeMappingCardinality);
                        parameters[1].Value = loginUser;
                        parameters[2].Value = programName;

                        storedProcedureName = "usp_DataModelManager_RelationshipTypeEntityTypeCardinality_Process";

                        reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                        DataModelHelper.PopulateOperationResult(reader, operationResult);

                        transactionScope.Complete();
                    }

                }
                catch (Exception exception)
                {
                    String errorMessage = String.Format("RelationshipTypeEntityTypeMappingCardinality Process Failed with {0}", exception.Message);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("DataModelManager.RelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel);
                }
            }

            if (operationResults.OperationResultStatus != OperationResultStatusEnum.Failed || operationResults.OperationResultStatus != OperationResultStatusEnum.CompletedWithWarnings)
            {
                operationResults.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="relationshipTypeEntityTypeMappingCardinalities"></param>
        private void PopulateRelationshipTypeEntityTypeMappingCardinalities(SqlDataReader reader, RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalities)
        {
            while (reader.Read())
            {
                RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality = new RelationshipTypeEntityTypeMappingCardinality();

                if (reader["PK_RelationshipTypeEntityTypeCardinality"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.Id = ValueTypeHelper.Int32TryParse(reader["PK_RelationshipTypeEntityTypeCardinality"].ToString(), relationshipTypeEntityTypeMappingCardinality.Id);
                }

                if (reader["FK_RelationshipTypeEntityType"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_RelationshipTypeEntityType"].ToString(), relationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId);
                }

                if (reader["FK_NodeType_To"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_NodeType_To"].ToString(), relationshipTypeEntityTypeMappingCardinality.ToEntityTypeId);
                }

                if (reader["NodeTypeToShortName"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName = reader["NodeTypeToShortName"].ToString();
                }

                if (reader["NodeTypeToLongName"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.ToEntityTypeLongName = reader["NodeTypeToLongName"].ToString();
                }

                if (reader["MinRelationships"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.MinRelationships = ValueTypeHelper.Int32TryParse(reader["MinRelationships"].ToString(), relationshipTypeEntityTypeMappingCardinality.MinRelationships);
                }

                if (reader["MaxRelationships"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.MaxRelationships = ValueTypeHelper.Int32TryParse(reader["MaxRelationships"].ToString(), relationshipTypeEntityTypeMappingCardinality.MaxRelationships);
                }

                if (reader["FK_NodeType_From"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_NodeType_From"].ToString(), relationshipTypeEntityTypeMappingCardinality.ToEntityTypeId);
                }

                if (reader["NodeTypeFromShortName"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.EntityTypeName = reader["NodeTypeFromShortName"].ToString();
                }

                if (reader["NodeTypeFromLongName"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.EntityTypeLongName = reader["NodeTypeFromLongName"].ToString();
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), relationshipTypeEntityTypeMappingCardinality.RelationshipTypeId);
                }

                if (reader["RelationshipTypeName"] != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName = reader["RelationshipTypeName"].ToString();
                }

                relationshipTypeEntityTypeMappingCardinalities.Add(relationshipTypeEntityTypeMappingCardinality);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinality"></param>
        /// <returns></returns>
        private String GetLegacyXml(RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality)
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //RelationshipTypeEntityTypeMappingCardinality node start
            xmlWriter.WriteStartElement("RelationshipEntityTypeCardinality");
            xmlWriter.WriteAttributeString("Operation", relationshipTypeEntityTypeMappingCardinality.Action.ToString());
            xmlWriter.WriteAttributeString("FK_NodeType_From", relationshipTypeEntityTypeMappingCardinality.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName_From", relationshipTypeEntityTypeMappingCardinality.EntityTypeName);
            xmlWriter.WriteAttributeString("FK_RelationshipType", relationshipTypeEntityTypeMappingCardinality.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeName", relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName);

            xmlWriter.WriteStartElement("Cardinality");
            xmlWriter.WriteAttributeString("PK_RelationshipTypeEntityTypeCardinality", relationshipTypeEntityTypeMappingCardinality.Id.ToString());
            xmlWriter.WriteAttributeString("FK_RelationshipTypeEntityType", relationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("FK_NodeType_To", relationshipTypeEntityTypeMappingCardinality.ToEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName_To", relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);
            xmlWriter.WriteAttributeString("MinRelationships", relationshipTypeEntityTypeMappingCardinality.MinRelationships.ToString());
            xmlWriter.WriteAttributeString("MaxRelationships", relationshipTypeEntityTypeMappingCardinality.MaxRelationships.ToString());
            xmlWriter.WriteEndElement();

            //ExportSubscriber node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion

        #endregion
    }
}