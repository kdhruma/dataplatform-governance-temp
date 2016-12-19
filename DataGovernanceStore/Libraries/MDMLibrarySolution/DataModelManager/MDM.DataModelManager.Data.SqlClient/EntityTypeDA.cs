using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;

    public class EntityTypeDA : SqlClientDataAccessBase
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
        /// Get Entity Types based on the Id list
        /// </summary>
        /// <param name="Ids">Indicates the Id lists</param>
        /// <returns></returns>
        public Dictionary<Int32, EntityType> GetAll(DBCommandProperties command)
        {
            Dictionary<Int32, EntityType> entityTypes = new Dictionary<Int32, EntityType>();

            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_EntityType_Get_ParametersArray");

                storedProcedureName = "usp_DataModelManager_EntityType_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateEntityTypes(reader, entityTypes);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityTypes;
        }

        /// <summary>
        /// Processes entity type in accordance to operation
        /// </summary>
        /// <param name="entityType">Collection of entity types</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="command">DB command</param>
        /// <returns>The result of the operation </returns>
        public OperationResult Process(EntityType entityType, String programName, String userName, DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("EntityTypeDA.Process", false);

            SqlDataReader reader = null;
            OperationResult entityProcessOperationResult = new OperationResult();

            SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

            const String storedProcedureName = "usp_DataModelManager_EntityType_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_EntityType_Process_ParametersArray");

                    parameters[0].Value = entityType.Id;
                    parameters[1].Value = entityType.Name;
                    parameters[2].Value = entityType.LongName;
                    parameters[3].Value = userName;
                    parameters[4].Value = programName;
                    parameters[5].Value = entityType.Action;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Need empty information to make sure correct operation result status is calculated.
                    entityProcessOperationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);

                    PopulateOperationResult(reader, entityType, entityProcessOperationResult);
                    transactionScope.Complete();

                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Entity Type Process Failed." + exception.Message);
                    entityProcessOperationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return entityProcessOperationResult;
        }

        /// <summary>
        /// Processes entity type in accordance to operation
        /// </summary>
        /// <param name="entityTypes">Collection of entity types</param>
        /// <param name="operationResults">OperationResult Collection</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="command">DB command</param>
        /// <returns>The result of the operation </returns>
        public void Process(EntityTypeCollection entityTypes, DataModelOperationResultCollection operationResults, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityTypeDA.Process", false);
            }

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
            const String storedProcedureName = "usp_DataModelManager_EntityType_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                foreach (EntityType entityType in entityTypes)
                {
                    if (entityType.Action == ObjectAction.Read || entityType.Action == ObjectAction.Ignore)
                        continue;

                    OperationResult entityTypeOperationResult = (OperationResult)operationResults.GetByReferenceId(entityType.ReferenceId);
                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_EntityType_Process_ParametersArray");

                    #region Execute SQL Procedure

                    try
                    {
                        parameters[0].Value = entityType.Id;
                        parameters[1].Value = entityType.Name;
                        parameters[2].Value = entityType.LongName;
                        parameters[3].Value = userName;
                        parameters[4].Value = programName;
                        parameters[5].Value = entityType.Action;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        #region Update OperationResult

                        PopulateOperationResult(reader, entityType, entityTypeOperationResult);

                        operationResults.RefreshOperationResultStatus();

                        #endregion
                    }
                    catch (Exception exception)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Entity Type Process Failed." + exception.Message);
                        entityTypeOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }

                    #endregion

                }// for all entity types

                transactionScope.Complete();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityType"></param>
        /// <param name="entityTypeProcessOperationResult"></param>
        private void PopulateOperationResult(SqlDataReader reader, EntityType entityType, OperationResult entityTypeProcessOperationResult)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String entityTypeId = String.Empty;

                if (reader["Id"] != null)
                {
                    entityTypeId = reader["Id"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorMessage"] != null)
                {
                    errorCode = reader["ErrorMessage"].ToString();
                }

                if (hasError & !String.IsNullOrEmpty(errorCode))
                {
                    entityTypeProcessOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    entityTypeProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    //entityTypeProcessOperationResult.AddOperationResult("", "Entity Type ID: " + entityTypeId, OperationResultType.Information);
                    entityType.Id = ValueTypeHelper.Int32TryParse(entityTypeId, -1);
                    entityTypeProcessOperationResult.ReturnValues.Add(entityTypeId);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityTypes"></param>
        private void PopulateEntityTypes(SqlDataReader reader, Dictionary<Int32, EntityType> entityTypes)
        {
            while (reader.Read())
            {
                EntityType entityType = new EntityType();

                if (reader["PK_NodeType"] != null)
                {
                    entityType.Id = ValueTypeHelper.Int32TryParse(reader["PK_NodeType"].ToString(), entityType.Id);
                }

                if (reader["ShortName"] != null)
                {
                    entityType.Name = reader["ShortName"].ToString();
                }

                if (reader["LongName"] != null)
                {
                    entityType.LongName = reader["LongName"].ToString();
                }

                if (reader["FK_CatalogBranchLevel"] != null)
                {
                    entityType.CatalogBranchLevel = ValueTypeHelper.Int32TryParse(reader["FK_CatalogBranchLevel"].ToString(), entityType.CatalogBranchLevel);
                }


                entityTypes.Add(entityType.Id, entityType);
            }
        }

        #endregion Private Methods

        #endregion
    }
}