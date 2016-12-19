using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Specifies the data access operations for Entity Validation.
    /// </summary>
    public class EntityValidationDA : SqlClientDataAccessBase
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
        /// Data Access operation for Validate Impacted data for Entity Delete.
        /// If entity has children or category has children or 
        /// category has mapped to any technical attribute then for the entity
        /// </summary>
        ///  <param name="entities">Indicates the Entity collections</param>
        /// <param name="entityOperationResults">Indicates the Entity operation results</param>
        /// <param name="command">Indicates the db command properties</param>
        public void CheckForEntitiesReferences(EntityCollection entities, EntityOperationResultCollection entityOperationResults, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityValidationDA.CheckForEntitiesReferences", MDMTraceSource.EntityProcess, false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityValidation_References_Exists_ParametersArray");

                SqlMetaData[] entityMetaData = generator.GetTableValueMetadata("EntityManager_EntityValidation_References_Exists_ParametersArray", parameters[0].ParameterName);

                ContainerBranchLevel containerBranchLevel = ContainerBranchLevel.Component;

                if (entities.First().BranchLevel == ContainerBranchLevel.Node)
                    containerBranchLevel = ContainerBranchLevel.Node;

                Collection<Int64> entityIds = new Collection<Int64>();
                
                if(entities != null)
                    entityIds = entities.GetEntityIdList();

                parameters[0].Value = EntityDataReaderUtility.CreateEntityIdTable(entityIds, entityMetaData);
                parameters[1].Value = containerBranchLevel;

                storedProcedureName = "usp_EntityManager_EntityValidation_References_Exists";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    PopulateResultsForEntitiesReferencesCheck(reader, entityOperationResults);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityValidationDA.CheckForEntitiesReferences", MDMTraceSource.EntityProcess);
            }
        }

        #endregion

        #region Private Methods

        private void PopulateResultsForEntitiesReferencesCheck(SqlDataReader reader, EntityOperationResultCollection entityOperationResults)
        {
            while (reader.Read())
            {
                Int64 entityId = 0;
                Boolean hasError = false;
                String errorCode = String.Empty;
                String errorMessage = String.Empty;

                if (reader["EntityId"] != null)
                {
                    Int64.TryParse(reader["EntityId"].ToString(), out entityId);
                }

                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (reader["ErrorMessage"] != null)
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }

                //Get the entity operation result
                EntityOperationResult entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entityId);

                if (entityOperationResult != null)
                {
                    if (hasError)
                    {
                        //Add error to entity operation results
                        entityOperationResults.AddEntityOperationResult(entityId, errorCode, errorMessage, ReasonType.EntityReferencesCheck, -1, -1, OperationResultType.Error);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
