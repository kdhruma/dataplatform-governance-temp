using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Transactions;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Class containing the methods to communicate to the database related to Promote BL.
    /// </summary>
    public class PromoteDA : SqlClientDataAccessBase
    {
        /// <summary>
        /// Calls the database to perform the promote process on the entity ids provided.
        /// </summary>
        /// <param name="entityHierarchy">The dictionary of entity ids are all the promotable entities along with the hierarchy level</param>
        /// <param name="operationResults">The operation result collection.</param>
        /// <param name="needsApprovedCopy">if set to <c>true</c> [needs approved copy].</param>
        /// <param name="messageCode">The message code.</param>
        /// <param name="messageParams">The message parameters.</param>
        /// <param name="loginUser">The login user.</param>
        /// <param name="command">The command.</param>
        /// <param name="callerContext">The caller context.</param>
        public void ProcessPromote(Dictionary<Int64, Int16> entityHierarchy, EntityOperationResultCollection operationResults, Boolean needsApprovedCopy, String messageCode, String messageParams, String loginUser, DBCommandProperties command, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            SqlDataReader reader = null;

            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    #region Declaration and Initialization

                    List<SqlDataRecord> entityPromoteListTable;
                    String storedProcedureName = "usp_EntityManager_Entity_Promote";
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Promote_ParametersArray");
                    SqlMetaData[] entityPromoteListMetaData = generator.GetTableValueMetadata("EntityManager_Entity_Promote_ParametersArray", parameters[0].ParameterName);

                    #endregion Declaration and Initialization

                    CreateEntityPromoteListTable(entityHierarchy, entityPromoteListMetaData, out entityPromoteListTable);

                    if (entityPromoteListTable != null)
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Preparing TVP from Entity hierarchy Object is completed.");
                            DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityPromoteListTable", entityPromoteListMetaData, entityPromoteListTable);
                        }

                        parameters[0].Value = entityPromoteListTable;
                        parameters[1].Value = (Int32)EntityActivityList.Promote;
                        parameters[2].Value = messageCode;
                        parameters[3].Value = messageParams;
                        parameters[4].Value = needsApprovedCopy;
                        parameters[5].Value = loginUser;
                        parameters[6].Value = callerContext.ProgramName;
                        parameters[7].Value = true;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                        }

                        UpdateOperationResults(reader, operationResults);
                    }

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Calls the database to perform the promote process on the auto promote or emergency promote context provided.
        /// </summary>
        /// <param name="changeContext">The change context.</param>
        /// <param name="operationResults">The operation result collection.</param>
        /// <param name="action">The action.</param>
        /// <param name="loginUser">The login user.</param>
        /// <param name="command">The command.</param>
        /// <param name="callerContext">The caller context.</param>
        public void ProcessAttributePromote(EntityFamilyChangeContext changeContext, EntityOperationResultCollection operationResults, EntityActivityList action, String loginUser, DBCommandProperties command, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            SqlDataReader reader = null;

            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    #region Declaration and Initialization

                    List<SqlDataRecord> entityPromoteListTable;
                    String storedProcedureName = "usp_EntityManager_Entity_Promote";
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Promote_ParametersArray");
                    SqlMetaData[] entityPromoteListMetaData = generator.GetTableValueMetadata("EntityManager_Entity_Promote_ParametersArray", parameters[0].ParameterName);

                    #endregion Declaration and Initialization

                    CreateEntityPromoteListTable(changeContext, entityPromoteListMetaData, operationResults, out entityPromoteListTable);

                    if (entityPromoteListTable != null)
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Preparing TVP from Entity family change context Object is completed.");
                            DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityPromoteListTable", entityPromoteListMetaData, entityPromoteListTable);
                        }

                        String messageCode = String.Empty;

                        if (action == EntityActivityList.AutoPromote)
                        {
                            messageCode = "114282";
                        }
                        else if (action == EntityActivityList.EmergencyPromote)
                        {
                            messageCode = "114283";
                        }

                        parameters[0].Value = entityPromoteListTable;
                        parameters[1].Value = (Int32)action;
                        parameters[2].Value = messageCode;
                        parameters[3].Value = String.Empty;
                        parameters[4].Value = true;
                        parameters[5].Value = loginUser;
                        parameters[6].Value = callerContext.ProgramName;
                        parameters[7].Value = true;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                        }

                        UpdateOperationResults(reader, operationResults);
                    }

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Updates the operation results.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="entityHierarchy">The entity hierarchy.</param>
        /// <param name="operationResults">The operation result.</param>
        private void UpdateOperationResults(SqlDataReader reader, EntityOperationResultCollection operationResults)
        {
            if (reader != null)
            {
                while (reader.Read())
                {
                    Int64 id = 0;
                    Int64 promotedFamilyId = 0;
                    Boolean hasError = false;
                    String errorCode = String.Empty;

                    if (reader["EntityId"] != null)
                    {
                        id = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), id);
                    }
                    if (reader["HasError"] != null)
                    {
                        hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), hasError);
                    }
                    if (reader["ErrorCode"] != null)
                    {
                        errorCode = reader["ErrorCode"].ToString();
                    }
                    if (reader["PromotedEntityFamilyId"] != null)
                    {
                        promotedFamilyId = ValueTypeHelper.Int64TryParse(reader["PromotedEntityFamilyId"].ToString(), promotedFamilyId);
                    }

                    //Get the entity operation result
                    EntityOperationResult result = (EntityOperationResult)operationResults.GetByEntityId(id);
                    if (result != null)
                    {
                        if (hasError)
                        {
                            result.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                            result.OperationResultStatus = OperationResultStatusEnum.Failed;
                        }
                        else
                        {
                            result.OperationResultStatus = OperationResultStatusEnum.Successful;
                            result.AddReturnValue(promotedFamilyId);
                        }
                    }
                }

                operationResults.RefreshOperationResultStatus();
            }
        }

        /// <summary>
        /// Creates the entity promote list table.
        /// </summary>
        /// <param name="entityHierarchy">The entity hierarchy.</param>
        /// <param name="entityPromoteListMetaData">The entity promote list meta data.</param>
        /// <param name="entityPromoteListTable">The entity promote list table.</param>
        private void CreateEntityPromoteListTable(Dictionary<Int64, Int16> entityHierarchy, SqlMetaData[] entityPromoteListMetaData, out List<SqlDataRecord> entityPromoteListTable)
        {
            entityPromoteListTable = new List<SqlDataRecord>();

            foreach (KeyValuePair<Int64, Int16> keyValuePair in entityHierarchy)
            {
                SqlDataRecord entityPromoteListTableRecord = new SqlDataRecord(entityPromoteListMetaData);

                entityPromoteListTableRecord.SetValue(0, keyValuePair.Key);
                entityPromoteListTableRecord.SetValue(2, keyValuePair.Value);

                entityPromoteListTable.Add(entityPromoteListTableRecord);
            }
        }

        /// <summary>
        /// Creates the entity promote list table.
        /// </summary>
        /// <param name="changeContext">The change context.</param>
        /// <param name="entityPromoteListMetaData">The entity promote list meta data.</param>
        /// <param name="entityPromoteListTable">The entity promote list table.</param>
        private void CreateEntityPromoteListTable(EntityFamilyChangeContext changeContext, SqlMetaData[] entityPromoteListMetaData, EntityOperationResultCollection operationResults, out List<SqlDataRecord> entityPromoteListTable)
        {
            entityPromoteListTable = new List<SqlDataRecord>();
            VariantsChangeContext variantsChangeContext = changeContext.VariantsChangeContext;
            PopulateTableForVariantsChangeContext(entityPromoteListMetaData, entityPromoteListTable, variantsChangeContext, operationResults);

            ExtensionChangeContextCollection extensionChangeContexts = changeContext.ExtensionChangeContexts;
            if (extensionChangeContexts != null && extensionChangeContexts.Count > 0)
            {
                foreach (ExtensionChangeContext extensionChangeContext in extensionChangeContexts)
                {
                    variantsChangeContext = extensionChangeContext.VariantsChangeContext;
                    PopulateTableForVariantsChangeContext(entityPromoteListMetaData, entityPromoteListTable, variantsChangeContext, operationResults);
                }
            }
        }

        /// <summary>
        /// Populates the table for variants change context.
        /// </summary>
        /// <param name="entityPromoteListMetaData">The entity promote list meta data.</param>
        /// <param name="entityPromoteListTable">The entity promote list table.</param>
        /// <param name="variantsChangeContext">The variants change context.</param>
        private static void PopulateTableForVariantsChangeContext(SqlMetaData[] entityPromoteListMetaData, List<SqlDataRecord> entityPromoteListTable, VariantsChangeContext variantsChangeContext, EntityOperationResultCollection operationResults)
        {
            if (variantsChangeContext != null && variantsChangeContext.EntityChangeContexts != null && variantsChangeContext.EntityChangeContexts.Count > 0)
            {
                foreach (EntityChangeContext entityChangeContext in variantsChangeContext.EntityChangeContexts)
                {
                    LocaleChangeContextCollection localeContexts = entityChangeContext.LocaleChangeContexts;
                    Int64 entityId = entityChangeContext.EntityId;
                    Int16 variantLevel = entityChangeContext.VariantLevel < 0 ? (Int16)0 : entityChangeContext.VariantLevel;

                    if (localeContexts != null && localeContexts.Count > 0)
                    {
                        operationResults.Add(new EntityOperationResult(entityChangeContext.EntityId, String.Empty));
                        foreach (LocaleChangeContext localeChangeContext in localeContexts)
                        {
                            AttributeChangeContextCollection attributeChangeContexts = localeChangeContext.AttributeChangeContexts;
                            PopulateTableForAttributeChangeContexts(attributeChangeContexts, entityPromoteListMetaData, entityPromoteListTable, entityId, variantLevel, 0, (Int32)localeChangeContext.DataLocale);

                            RelationshipChangeContextCollection relationshipChangeContexts = localeChangeContext.RelationshipChangeContexts;
                            if (relationshipChangeContexts != null && relationshipChangeContexts.Count > 0)
                            {
                                foreach (RelationshipChangeContext relationshipChangeContext in relationshipChangeContexts)
                                {
                                    AttributeChangeContextCollection relationshipAttributeChangeContexts = relationshipChangeContext.AttributeChangeContexts;

                                    PopulateTableForAttributeChangeContexts(relationshipAttributeChangeContexts, entityPromoteListMetaData, entityPromoteListTable, entityId, variantLevel, relationshipChangeContext.RelationshipId, (Int32)localeChangeContext.DataLocale);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Populates the table for attribute change contexts.
        /// </summary>
        /// <param name="attributeChangeContexts">The attribute change contexts.</param>
        /// <param name="entityPromoteListMetaData">The entity promote list meta data.</param>
        /// <param name="entityPromoteListTable">The entity promote list table.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="variantLevel">The variant level.</param>
        /// <param name="relationshipId">The relationship identifier.</param>
        /// <param name="localeId">The locale identifier.</param>
        private static void PopulateTableForAttributeChangeContexts(AttributeChangeContextCollection attributeChangeContexts, SqlMetaData[] entityPromoteListMetaData, List<SqlDataRecord> entityPromoteListTable, Int64 entityId, Int16 variantLevel, Int64 relationshipId, Int32 localeId)
        {
            if (attributeChangeContexts != null && attributeChangeContexts.Count > 0)
            {
                foreach (AttributeChangeContext attributeChangeContext in attributeChangeContexts)
                {
                    if (attributeChangeContext.AttributeInfoCollection != null && attributeChangeContext.AttributeInfoCollection.Count > 0)
                    {
                        foreach (AttributeInfo attributeInfo in attributeChangeContext.AttributeInfoCollection)
                        {
                            SqlDataRecord entityPromoteListTableRecord = new SqlDataRecord(entityPromoteListMetaData);

                            entityPromoteListTableRecord.SetValue(0, entityId);
                            entityPromoteListTableRecord.SetValue(1, attributeInfo.Id);
                            entityPromoteListTableRecord.SetValue(2, variantLevel);
                            entityPromoteListTableRecord.SetValue(3, localeId);
                            entityPromoteListTableRecord.SetValue(4, relationshipId);

                            entityPromoteListTable.Add(entityPromoteListTableRecord);
                        }
                    }
                }
            }
        }
    }
}