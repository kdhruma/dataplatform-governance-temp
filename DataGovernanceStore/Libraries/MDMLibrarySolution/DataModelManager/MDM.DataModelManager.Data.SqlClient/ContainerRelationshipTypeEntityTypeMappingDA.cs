using System;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Represents data access logic for Container RelationshipType EntityType mapping
    /// </summary>
    public class ContainerRelationshipTypeEntityTypeMappingDA : SqlClientDataAccessBase
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
        /// <param name="entityTypeId"></param>
        /// <returns>ContainerRelationshipTypeEntityTypeMappingCollection</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection GetMappings(Int32 containerId, Int32 relationshipTypeId, Int32 entityTypeId)
        {
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = new ContainerRelationshipTypeEntityTypeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.GetMappings", MDMTraceSource.DataModel, false);

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeEntityType_Get_ParametersArray");

                storedProcedureName = "usp_DataModelManager_ContainerRelationshipTypeEntityType_Get";

                parameters[0].Value = containerId;
                parameters[1].Value = relationshipTypeId;
                parameters[2].Value = entityTypeId;

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                PopulateContainerRelationshipTypeEntityTypeMappings(reader, containerRelationshipTypeEntityTypeMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.GetMappings", MDMTraceSource.DataModel);
            }

            return containerRelationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cnodeGroupIds"></param>
        /// <param name="catalogId"></param>
        /// <returns>Collection<ContainerRelationshipTypeEntityTypeMapping></returns>
        public Collection<ContainerRelationshipTypeEntityTypeMapping> GetMappingsByCnodes(String user, String cnodeGroupIds, Int32 catalogId)
        {

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Collection<ContainerRelationshipTypeEntityTypeMapping> containerRelationshipTypeEntityTypeMappings = new Collection<ContainerRelationshipTypeEntityTypeMapping>(); ;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.GetMappingsByCnodes", MDMTraceSource.DataModel, false);

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeEntityType_GetMappingsByCnodes_ParametersArray");

                parameters[0].Value = user;
                parameters[1].Value = cnodeGroupIds;
                parameters[2].Value = catalogId;

                storedProcedureName = "usp_CNode_FindBulkNodeTypeRelationshipType";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Int32 relationshipTypeId = 0;
                    String relationshipname = String.Empty;

                    if (reader["PK_RelationshipType"] != null)
                        Int32.TryParse(reader["PK_RelationshipType"].ToString(), out relationshipTypeId);

                    if (reader["ShortName"] != null)
                        relationshipname = reader["ShortName"].ToString();

                    ContainerRelationshipTypeEntityTypeMapping relationshipNodeTypes = new ContainerRelationshipTypeEntityTypeMapping(relationshipTypeId, relationshipname);
                    containerRelationshipTypeEntityTypeMappings.Add(relationshipNodeTypes);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.GetMappingsByCnodes", MDMTraceSource.DataModel);
            }

            return containerRelationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns>output</returns>
        public Int32 Process(ContainerRelationshipTypeEntityTypeMappingCollection listMappings, String loginUser, String programName)
        {
            Int32 output = 0;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    String paramXML = ConvertToXML(listMappings);

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeEntityType_Process_ParametersArray");

                    parameters[0].Value = paramXML;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_ContainerRelationshipTypeEntityType_Process";

                    output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel);
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
        /// <returns>t</returns>
        public void Process(ContainerRelationshipTypeEntityTypeMappingCollection listMappings, DataModelOperationResultCollection operationResults, String loginUser, String programName)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel, false);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    connectionString = AppConfigurationHelper.ConnectionString;

                    String paramXML = ConvertToXML(listMappings);

                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    parameters = generator.GetParameters("DataModelManager_ContainerRelationshipTypeEntityType_Process_ParametersArray");

                    parameters[0].Value = paramXML;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_DataModelManager_ContainerRelationshipTypeEntityType_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                     DataModelHelper.PopulateOperationResult(reader, operationResults, listMappings);

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("ContainerRelationshipTypeEntityTypeMapping Process Failed with {0}", ex.Message);
                PopulateOperationResult(errorMessage, operationResults, listMappings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ContainerRelationshipTypeEntityTypeMappingDA.Process", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listMappings"></param>
        /// <returns></returns>
        private String ConvertToXML(ContainerRelationshipTypeEntityTypeMappingCollection listMappings)
        {
            String xml = "<ContainerRelationshipTypeEntityTypes>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (ContainerRelationshipTypeEntityTypeMapping mapping in listMappings)
            {
                if (mapping.Action == ObjectAction.Read || mapping.Action == ObjectAction.Ignore)
                    continue;

                stringBuilder.Append(mapping.ToXML());
            }

            stringBuilder.Append("</ContainerRelationshipTypeEntityTypes>");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="containerRelationshipTypeEntityTypeMappings"></param>
        /// <returns></returns>
        private void PopulateContainerRelationshipTypeEntityTypeMappings(SqlDataReader reader, ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings)
        {
            while (reader.Read())
            {
                ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = new ContainerRelationshipTypeEntityTypeMapping();

                if (reader["PK_CatalogRelTypeNodeType"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.Id = ValueTypeHelper.Int32TryParse(reader["PK_CatalogRelTypeNodeType"].ToString(), containerRelationshipTypeEntityTypeMapping.Id);
                }

                if (reader["ContainerId"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerRelationshipTypeEntityTypeMapping.ContainerId);
                }

                if (reader["ContainerName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.ContainerName = reader["ContainerName"].ToString();
                }

                if (reader["ContainerLongName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.ContainerLongName = reader["ContainerLongName"].ToString();
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), containerRelationshipTypeEntityTypeMapping.RelationshipTypeId);
                }

                if (reader["RelationshipTypeName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.RelationshipTypeName = reader["RelationshipTypeName"].ToString();
                }

                if (reader["RelationshipTypeLongName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.RelationshipTypeLongName = reader["RelationshipTypeLongName"].ToString();
                }

                if (reader["NodeTypeId"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["NodeTypeId"].ToString(), containerRelationshipTypeEntityTypeMapping.EntityTypeId);
                }

                if (reader["EntityTypeName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.EntityTypeName = reader["EntityTypeName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.EntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["ShowAtChildren"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.DrillDown = ValueTypeHelper.BooleanTryParse(reader["ShowAtChildren"].ToString(), containerRelationshipTypeEntityTypeMapping.DrillDown);
                }

                if (reader["Default"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.IsDefaultRelation = ValueTypeHelper.BooleanTryParse(reader["Default"].ToString(), containerRelationshipTypeEntityTypeMapping.IsDefaultRelation);
                }

                if (reader["Excludable"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.Excludable = ValueTypeHelper.BooleanTryParse(reader["Excludable"].ToString(), containerRelationshipTypeEntityTypeMapping.Excludable);
                }

                if (reader["ReadOnly"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(reader["ReadOnly"].ToString(), containerRelationshipTypeEntityTypeMapping.ReadOnly);
                }

                if (reader["ValidationRequired"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.ValidationRequired = ValueTypeHelper.BooleanTryParse(reader["ValidationRequired"].ToString(), containerRelationshipTypeEntityTypeMapping.ValidationRequired);
                }

                if (reader["ShowValidFlagInGrid"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid = ValueTypeHelper.BooleanTryParse(reader["ShowValidFlagInGrid"].ToString(), containerRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid);
                }

                if (reader["IsSpecialized"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.IsSpecialized = ValueTypeHelper.BooleanTryParse(reader["IsSpecialized"].ToString(), containerRelationshipTypeEntityTypeMapping.IsSpecialized);
                }

                if (reader["OrganizationId"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), containerRelationshipTypeEntityTypeMapping.OrganizationId);
                }

                if (reader["OrganizationName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.OrganizationName = reader["OrganizationName"].ToString();
                }

                if (reader["OrganizationLongName"] != null)
                {
                    containerRelationshipTypeEntityTypeMapping.OrganizationLongName = reader["OrganizationLongName"].ToString();
                }

                containerRelationshipTypeEntityTypeMappings.Add(containerRelationshipTypeEntityTypeMapping);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="operationResults"></param>
        /// <param name="containerRelationshipTypeEntityTypeMappings"></param>
        /// <returns></returns>
        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in containerRelationshipTypeEntityTypeMappings)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(containerRelationshipTypeEntityTypeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }

        #endregion

        #endregion
    }
}