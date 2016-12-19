using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

namespace MDM.RelationshipManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Specifies Relationship type data access
    /// </summary>
    public class RelationshipTypeDA : SqlClientDataAccessBase
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
        /// Get relationship based on catalogId and nodeType Id.
        /// </summary>
        /// <param name="containerId">This parameter specifying container id. It is optional</param>
        /// <param name="entityTypeId">This parameter specifying entity type id. It is optional</param>
        /// <param name="command">Connection string and other command related property</param>
        /// <returns>collection of relationship type</returns>
        public Tuple<RelationshipTypeCollection, Int32> Get(Int32 containerId, Int32 entityTypeId, DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipTypeDA.GetAll", false);

            SqlDataReader reader = null;
            RelationshipTypeCollection relationshipTypeCollection = new RelationshipTypeCollection();
            Tuple<RelationshipTypeCollection, Int32> relationshipTuple = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("RelationshipManager_RelationshipType_Get_ParametersArray");

                parameters[0].Value = containerId;
                parameters[1].Value = entityTypeId;

                const String storedProcedureName = "usp_RelationshipManager_RelationshipType_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                Int32 id = 0;
                String name = String.Empty;
                String longName = String.Empty;
                Boolean validationRequired = false;
                Boolean showValidFlag = false;
                Boolean readOnly = false;
                Boolean drillDown = false;
                String isDefault = String.Empty;
                Boolean enforceRelatedEntityStateOnSourceEntity = false;
                Boolean checkRelatedEntityPromoteStatusOnPromote = false;

                Int32 relationshipTypeId = 0;

                if (containerId > 0 && entityTypeId > 0)
                {
                    #region Relationship Type based on catalogId & nodeTypeId

                    while (reader.Read())
                    {
                        if (reader["PK_RelationshipType"] != null)
                        {
                            id = ValueTypeHelper.Int32TryParse(reader["PK_RelationshipType"].ToString(), 0);
                        }
                        if (reader["ShortName"] != null)
                        {
                            name = reader["ShortName"].ToString();
                        }
                        if (reader["LongName"] != null)
                        {
                            longName = reader["LongName"].ToString();
                        }
                        if (reader["ValidationRequired"] != null)
                        {
                            validationRequired = ValueTypeHelper.ConvertToBoolean(reader["ValidationRequired"].ToString());
                        }
                        if (reader["ShowValidFlagInGrid"] != null)
                        {
                            showValidFlag = ValueTypeHelper.ConvertToBoolean(reader["ShowValidFlagInGrid"].ToString());
                        }
                        if (reader["ReadOnly"] != null)
                        {
                            readOnly = ValueTypeHelper.ConvertToBoolean(reader["ReadOnly"].ToString());
                        }
                        if (reader["DrillDown"] != null)
                        {
                            drillDown = ValueTypeHelper.ConvertToBoolean(reader["DrillDown"].ToString());
                        }
                        if (reader["IsDefault"] != null)
                        {
                            isDefault = reader["IsDefault"].ToString();
                        }
                        if (reader["EnforceRelatedEntityStateOnSourceEntity"] != null)
                        {
                            enforceRelatedEntityStateOnSourceEntity = ValueTypeHelper.BooleanTryParse(reader["EnforceRelatedEntityStateOnSourceEntity"].ToString(), false);
                        }
                        if (reader["CheckRelatedEntityPromoteStatusOnPromote"] != null)
                        {
                            checkRelatedEntityPromoteStatusOnPromote = ValueTypeHelper.BooleanTryParse(reader["CheckRelatedEntityPromoteStatusOnPromote"].ToString(), false);
                        }

                        RelationshipType relationshipType = new RelationshipType(id, name, longName, validationRequired, showValidFlag, readOnly, drillDown, isDefault, enforceRelatedEntityStateOnSourceEntity, checkRelatedEntityPromoteStatusOnPromote);
                        relationshipTypeCollection.Add(relationshipType);
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (reader["FK_RelationshipType"] != null)
                            {
                                relationshipTypeId = ValueTypeHelper.Int32TryParse(reader["FK_RelationshipType"].ToString(), 0);
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Relationship Type based on catalogId

                    while (reader.Read())
                    {
                        if (reader["PK_RelationshipType"] != null)
                        {
                            id = ValueTypeHelper.Int32TryParse(reader["PK_RelationshipType"].ToString(), 0);
                        }
                        if (reader["ShortName"] != null)
                        {
                            name = reader["ShortName"].ToString();
                        }
                        if (reader["LongName"] != null)
                        {
                            longName = reader["LongName"].ToString();
                        }
                        if (reader["EnforceRelatedEntityStateOnSourceEntity"] != null)
                        {
                            enforceRelatedEntityStateOnSourceEntity = ValueTypeHelper.BooleanTryParse(reader["EnforceRelatedEntityStateOnSourceEntity"].ToString(), false);
                        }
                        if (reader["CheckRelatedEntityPromoteStatusOnPromote"] != null)
                        {
                            checkRelatedEntityPromoteStatusOnPromote = ValueTypeHelper.BooleanTryParse(reader["CheckRelatedEntityPromoteStatusOnPromote"].ToString(), false);
                        }

                        RelationshipType relationshipType = new RelationshipType(id, name, longName, enforceRelatedEntityStateOnSourceEntity, checkRelatedEntityPromoteStatusOnPromote);
                        relationshipTypeCollection.Add(relationshipType);
                    }

                    relationshipTypeId = -1;

                    #endregion
                }
                
                relationshipTuple = new Tuple<RelationshipTypeCollection, Int32>(relationshipTypeCollection, relationshipTypeId);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipTypeDA.GetAll");
            }

            return relationshipTuple;
        }

        /// <summary>
        /// Process relationship type
        /// </summary>
        /// <param name="relationshipType">Relationship type to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of relationship</returns>
        public OperationResult Process(RelationshipType relationshipType, String programName, String userName, DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("RelationshipTypeDA.Process", false);

            SqlDataReader reader = null;
            OperationResult relationshipTypeProcessOperationResult = new OperationResult();

            const String storedProcedureName = "usp_RelationshipManager_RelationshipType_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    SqlParameter[] parameters = BuildInputParameters(relationshipType, userName, programName);

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update relationshipType object with Actual Id in case of create
                    UpdateRelationshipType(reader, relationshipType, relationshipTypeProcessOperationResult);

                    //Need empty information to make sure correct operation result status is calculated.
                    relationshipTypeProcessOperationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);

                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "RelationshipType Process Failed." + exception.Message);
                    relationshipTypeProcessOperationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }

            return relationshipTypeProcessOperationResult;
        }

        /// <summary>
        /// Processes entity type in accordance to operation
        /// </summary>
        /// <param name="relationshipTypes">Collection of relationship types</param>
        /// <param name="operationResults">OperationResult Collection</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="command">DB command</param>
        /// <returns>The result of the operation </returns>
        public void Process(RelationshipTypeCollection relationshipTypes, DataModelOperationResultCollection operationResults, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipTypeDA.Process", false);
            }

            SqlDataReader reader = null;
            const String storedProcedureName = "usp_RelationshipManager_RelationshipType_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                foreach (RelationshipType relationshipType in relationshipTypes)
                {
                    if (relationshipType.Action == ObjectAction.Read || relationshipType.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    OperationResult relationshipTypeOperationResult = (OperationResult)operationResults.GetByReferenceId(relationshipType.ReferenceId);
                    
                    #region Execute SQL Procedure

                    try
                    {
                        SqlParameter[] parameters = BuildInputParameters(relationshipType, userName, programName);

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        //Update relationshipType object with Actual Id in case of create
                        UpdateRelationshipType(reader, relationshipType, relationshipTypeOperationResult);

                        operationResults.RefreshOperationResultStatus();
                    }
                    catch (Exception exception)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "RelationshipType Process Failed." + exception.Message);
                        relationshipTypeOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }

                    #endregion

                }// for all relationship types

                transactionScope.Complete();
            }
        }

        #endregion

        #region Private Methods

        private SqlParameter[] BuildInputParameters(RelationshipType relationshipType, String userName, String programName)
        {
            SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");

            SqlParameter[] parameters = generator.GetParameters("RelationshipManager_RelationshipType_Process_ParametersArray");

            parameters[0].Value = relationshipType.Id;
            parameters[1].Value = relationshipType.Name;
            parameters[2].Value = relationshipType.LongName;
            parameters[3].Value = relationshipType.Action.ToString();
            parameters[4].Value = userName;
            parameters[5].Value = programName;
            parameters[6].Value = relationshipType.EnforceRelatedEntityStateOnSourceEntity;
            parameters[7].Value = relationshipType.CheckRelatedEntityPromoteStatusOnPromote;

            return parameters;
        }

        private void UpdateRelationshipType(SqlDataReader reader, RelationshipType relationshipType, OperationResult relationshipTypeProcessOperationResult)
        {
            Boolean hasError = false;
            String errorCode = String.Empty;

            while (reader.Read())
            {
                if (reader["Id"] != null)
                {
                    relationshipType.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), -1);
                    relationshipTypeProcessOperationResult.Id = relationshipType.Id;
                    relationshipTypeProcessOperationResult.ReferenceId = String.IsNullOrWhiteSpace(relationshipType.ReferenceId) ? relationshipType.Name : relationshipType.ReferenceId;
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
                    relationshipTypeProcessOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    relationshipTypeProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        #endregion Private method

        #endregion
    }
}