using System;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    
    public class ContainerRelationshipTypeEntityTypeMappingCardinalityDA : SqlClientDataAccessBase
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
        /// Get ContainerRelationshipTypeEntityTypeMappingCardinalityCollection based on  RelationshipType Id and  From EntityType Id
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        /// <returns>ContainerRelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection Get(Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityDA.Get", MDMTraceSource.DataModel, false);

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeEntityTypeCardinality_Get_ParametersArray");

                parameters[0].Value = containerId;
                parameters[1].Value = entityTypeId;
                parameters[2].Value = relationshipTypeId;

                storedProcedureName = "usp_Relationship_Cardinality_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                PopulateContainerRelationshipTypeEntityTypeMappingCardinalities(reader, containerRelationshipTypeEntityTypeMappingCardinalitys);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityDA.Get", MDMTraceSource.DataModel);
            }

            return containerRelationshipTypeEntityTypeMappingCardinalitys;
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType EntityType mappings
        /// </summary>
        /// <param name="containerContainerRelationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>Result of the operation Collection</returns>
        public OperationResultCollection Process(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerContainerRelationshipTypeEntityTypeMappingCardinalitys, String loginUser, String programName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel, false);

            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            OperationResult operationResult = null;
            OperationResultCollection operationResults = new OperationResultCollection();

            foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerContainerRelationshipTypeEntityTypeMappingCardinality in containerContainerRelationshipTypeEntityTypeMappingCardinalitys)
            {
                try
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel, false);

                    operationResult = new OperationResult();

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        connectionString = AppConfigurationHelper.ConnectionString;

                        SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                        parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeEntityTypeCardinality_Process_ParametersArray");

                        parameters[0].Value = GetLegacyXml(containerContainerRelationshipTypeEntityTypeMappingCardinality);
                        parameters[1].Value = loginUser;
                        parameters[2].Value = programName;

                        storedProcedureName = "usp_Relationship_Cardinality_Set";

                        reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                        DataModelHelper.PopulateOperationResult(reader, operationResult);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    String errorMessage = String.Format("ContainerRelationshipTypeEntityTypeMappingCardinality Process Failed with {0}", exception.Message);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }
                finally
                {
                    operationResults.Add(operationResult);

                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel);
                }
            }
            return operationResults;
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType EntityType mappings
        /// </summary>
        /// <param name="containerContainerRelationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="operationResults"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public void Process(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerContainerRelationshipTypeEntityTypeMappingCardinalitys, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            OperationResult operationResult = null;

            foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerContainerRelationshipTypeEntityTypeMappingCardinality in containerContainerRelationshipTypeEntityTypeMappingCardinalitys)
            {
                try
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel, false);

                    operationResult = (OperationResult)operationResults.GetByReferenceId(containerContainerRelationshipTypeEntityTypeMappingCardinality.ReferenceId);

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        connectionString = AppConfigurationHelper.ConnectionString;

                        SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                        parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeEntityTypeCardinality_Process_ParametersArray");

                        parameters[0].Value = GetLegacyXml(containerContainerRelationshipTypeEntityTypeMappingCardinality);
                        parameters[1].Value = loginUser;
                        parameters[2].Value = programName;

                        storedProcedureName = "usp_Relationship_Cardinality_Set";

                        reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                        DataModelHelper.PopulateOperationResult(reader, operationResult);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    String errorMessage = String.Format("ContainerRelationshipTypeEntityTypeMappingCardinality Process Failed with {0}", exception.Message);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityDA.Process", MDMTraceSource.DataModel);
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
        /// Get Xml representation of RelationshipType EntityType Mapping Cardinality
        /// </summary>
        /// <returns>
        /// Xml representation of RelationshipType EntityType Mapping Cardinality
        /// </returns>
        private String GetLegacyXml(ContainerRelationshipTypeEntityTypeMappingCardinality containerContainerRelationshipTypeEntityTypeMappingCardinality)
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ContainerRelationshipTypeEntityTypeMappingCardinality node start
            xmlWriter.WriteStartElement("RelationshipCardinality");
            xmlWriter.WriteAttributeString("Operation", containerContainerRelationshipTypeEntityTypeMappingCardinality.Action.ToString());
            xmlWriter.WriteAttributeString("FK_Catalog", containerContainerRelationshipTypeEntityTypeMappingCardinality.ContainerId.ToString());
            xmlWriter.WriteAttributeString("CatalogName", containerContainerRelationshipTypeEntityTypeMappingCardinality.ContainerName);
            xmlWriter.WriteAttributeString("FK_NodeType_From", containerContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName_From", containerContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName);
            xmlWriter.WriteAttributeString("FK_RelationshipType", containerContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeName", containerContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName);

            xmlWriter.WriteStartElement("Cardinality");
            xmlWriter.WriteAttributeString("PK_CatalogRelTypeCardinality", containerContainerRelationshipTypeEntityTypeMappingCardinality.Id.ToString());
            xmlWriter.WriteAttributeString("FK_CatalogRelTypeNodeType", containerContainerRelationshipTypeEntityTypeMappingCardinality.ContainerRelationshipTypeEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("FK_NodeType_To", containerContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName_To", containerContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);
            xmlWriter.WriteAttributeString("MinRelationships", containerContainerRelationshipTypeEntityTypeMappingCardinality.MinRelationships.ToString());
            xmlWriter.WriteAttributeString("MaxRelationships", containerContainerRelationshipTypeEntityTypeMappingCardinality.MaxRelationships.ToString());
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="containerContainerRelationshipTypeEntityTypeMappingCardinalities"></param>
        /// <returns></returns>
        private void PopulateContainerRelationshipTypeEntityTypeMappingCardinalities(SqlDataReader reader, ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerContainerRelationshipTypeEntityTypeMappingCardinalities)
        {
            while (reader.Read())
            {
                ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality = new ContainerRelationshipTypeEntityTypeMappingCardinality();

                if (reader["PK_CatalogRelTypeCardinality"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.Id = ValueTypeHelper.Int32TryParse(reader["PK_CatalogRelTypeCardinality"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.Id);
                }

                if (reader["FK_CatalogRelTypeNodeType"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.ContainerRelationshipTypeEntityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_CatalogRelTypeNodeType"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.ContainerRelationshipTypeEntityTypeId);
                }

                if (reader["FK_NodeType_To"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_NodeType_To"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId);
                }

                if (reader["NodeTypeToShortName"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName = reader["NodeTypeToShortName"].ToString();
                }

                if (reader["NodeTypeToLongName"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeLongName = reader["NodeTypeToLongName"].ToString();
                }

                if (reader["MinRelationships"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.MinRelationships = ValueTypeHelper.Int32TryParse(reader["MinRelationships"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.MinRelationships);
                }

                if (reader["MaxRelationships"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.MaxRelationships = ValueTypeHelper.Int32TryParse(reader["MaxRelationships"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.MaxRelationships);
                }

                if (reader["FK_NodeType_From"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_NodeType_From"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId);
                }

                if (reader["NodeTypeFromShortName"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName = reader["NodeTypeFromShortName"].ToString();
                }

                if (reader["NodeTypeFromLongName"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeLongName = reader["NodeTypeFromLongName"].ToString();
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId);
                }

                if (reader["RelationshipTypeName"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName = reader["RelationshipTypeName"].ToString();
                }

                if (reader["OrganizationId"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.OrganizationId);
                }

                if (reader["OrganizationName"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName = reader["OrganizationName"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.ContainerId);
                }

                if (reader["ContainerName"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.ContainerName = reader["ContainerName"].ToString();
                }

                if (reader["IsSpecialized"] != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.IsSpecialized = ValueTypeHelper.BooleanTryParse(reader["IsSpecialized"].ToString(), containerRelationshipTypeEntityTypeMappingCardinality.IsSpecialized);
                }

                containerContainerRelationshipTypeEntityTypeMappingCardinalities.Add(containerRelationshipTypeEntityTypeMappingCardinality);
            }
        }

        #endregion

        #endregion
    }
}