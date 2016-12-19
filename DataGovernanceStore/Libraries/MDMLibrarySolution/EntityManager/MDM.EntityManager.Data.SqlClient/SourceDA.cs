using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Source data access layer
    /// </summary>
    public class SourceDA : SqlClientDataAccessBase
    {
        #region Constants

        private const String SourceIdResultName = "PK_Source";
        private const String ShortNameResultName = "ShortName";
        private const String LongNameResultName = "LongName";
        private const String DescriptionResultName = "Description";
        private const String IsSystemResultName = "IsSystem";
        private const String AuditRefResultName = "FK_Audit_Ref";


        private const String SQLParametersGeneratorName = "EntityManager_SqlParameters";

        private const String SourceGetAllProcessName = "GetAll";
        private const String SourceCreateUpdateDeleteProcessName = "Process";

        private const String SourceGetAllSPName = "usp_EntityManager_Source_GetAll";
        private const String SourceProcessSPName = "usp_EntityManager_Source_Process";

        private const String SourceGetAllSPParameters = "EntityManager_Source_GetAll_ParametersArray";
        private const String SourceProcessSPParameters = "EntityManager_Source_Process_ParametersArray";

        private const String TracingPrefix = "MDM.EntityManager.Data.SqlClient.SourceDA.";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Getting all Sources
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Collection of Sources</returns>
        public SourceCollection GetAll(DBCommandProperties command)
        {
            SourceCollection result = new SourceCollection();

            StartTraceActivity(SourceGetAllProcessName, MDMTraceSource.EntityGet);
            try
            {
                SqlDataReader reader = null;
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);

                    SqlParameter[] parameters = generator.GetParameters(SourceGetAllSPParameters);

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, SourceGetAllSPName);

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Int32 sourceId = 0;
                            String shortName = String.Empty;
                            String longName = String.Empty;
                            String description = String.Empty;
                            Boolean isSystem = false;
                            Int64 auditRef = -1;

                            if (reader[SourceIdResultName] != null)
                                Int32.TryParse(reader[SourceIdResultName].ToString(), out sourceId);
                            if (reader[ShortNameResultName] != null)
                                shortName = reader[ShortNameResultName].ToString();
                            if (reader[LongNameResultName] != null)
                                longName = reader[LongNameResultName].ToString();
                            if (reader[DescriptionResultName] != null)
                                description = reader[DescriptionResultName].ToString();
                            if (reader[IsSystemResultName] != null)
                                Boolean.TryParse(reader[IsSystemResultName].ToString(), out isSystem);
                            if (reader[AuditRefResultName] != null)
                                Int64.TryParse(reader[AuditRefResultName].ToString(), out auditRef);

                            Source item = new Source
                            {
                                Id = sourceId,
                                Name = shortName,
                                LongName = longName,
                                Description = description,
                                IsInternal = isSystem,
                                AuditRefId = auditRef,
                            };

                            result.Add(item);
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            finally
            {
                StopTraceActivity(SourceGetAllProcessName, MDMTraceSource.EntityGet);
            }

            return result;
        }

        /// <summary>
        /// Insert, Update and Delete operations for Sources
        /// </summary>
        /// <param name="sources">Collection of Sources to process</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">Connection string and operation type</param>
        /// <param name="userLogin">User login</param>
        /// <returns>Operation result</returns>
        public OperationResultCollection Process(SourceCollection sources, CallerContext callerContext, DBCommandProperties command, String userLogin)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            StartTraceActivity(SourceCreateUpdateDeleteProcessName, MDMTraceSource.EntityProcess);
            try
            {

                SqlDataReader reader = null;

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    try
                    {
                        SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);

                        SqlParameter[] parameters = generator.GetParameters(SourceProcessSPParameters);

                        List<SqlDataRecord> preparedSource = new List<SqlDataRecord>();

                        foreach (Source item in sources)
                        {
                            SqlDataRecord preparedItem = new SqlDataRecord(generator.GetTableValueMetadata(SourceProcessSPParameters, parameters[0].ParameterName));

                            preparedItem.SetValue(0, item.Id);
                            preparedItem.SetValue(1, item.Name);
                            preparedItem.SetValue(2, item.LongName);
                            preparedItem.SetValue(3, item.Description);
                            preparedItem.SetValue(4, item.IsInternal);
                            preparedItem.SetValue(5, item.Action.ToString());

                            preparedSource.Add(preparedItem);
                        }

                        if (preparedSource.Any())
                        {
                            parameters[0].Value = preparedSource;
                        }

                        parameters[1].Value = userLogin;
                        parameters[2].Value = callerContext.ProgramName;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, SourceProcessSPName);

                        if (reader != null)
                        {
                            UpdateOperationResult(reader, sources, operationResultCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        OperationResult operationResult = new OperationResult();
                        operationResult.AddOperationResult(String.Empty, ex.Message, OperationResultType.Error);
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

                        operationResultCollection.Add(operationResult);
                        operationResultCollection.OperationResultStatus = OperationResultStatusEnum.Failed;

                        TraceError("Process - Failed. " + ex.Message, MDMTraceSource.EntityProcess);
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }

                    operationResultCollection.RefreshOperationResultStatus();
                    if (operationResultCollection.OperationResultStatus == OperationResultStatusEnum.Successful)
                        transactionScope.Complete();
                }

                operationResultCollection.RefreshOperationResultStatus();

            }
            finally
            {
                StopTraceActivity(SourceCreateUpdateDeleteProcessName, MDMTraceSource.EntityProcess);
            }

            return operationResultCollection;
        }

        #endregion

        #region Private Methods

        private void UpdateOperationResult(SqlDataReader reader, SourceCollection sources, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                Int32 sourceId = 0;
                Int32 resultId = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;

                if (reader["SourceId"] != null)
                    Int32.TryParse(reader["SourceId"].ToString(), out sourceId);
                if (reader["ResultId"] != null)
                    Int32.TryParse(reader["ResultId"].ToString(), out resultId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorMessage"] != null)
                    errorMessage = reader["ErrorMessage"].ToString();

                //Get Source for this result record
                Source source = sources.FirstOrDefault(e => e.Id == sourceId);

                if (source != null)
                {
                    if (source.Action == ObjectAction.Create)
                    {
                        //Update item id with the new value
                        source.Id = resultId;
                    }

                    OperationResult operationResult = new OperationResult();
                    operationResult.Id = source.Id;
                    operationResult.ReferenceId = String.IsNullOrWhiteSpace(source.ReferenceId) ? source.Name : source.ReferenceId;

                    if (hasError)
                    {
                        //Add error
                        operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                    }
                    else
                    {
                        //No errors.. Set status as Successful
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                    operationResultCollection.Add(operationResult);
                }
            }
        }

        private static Boolean StartTraceActivity(String record, MDMTraceSource traceSource)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StartTraceActivity(PopulateTraceRecord(record), traceSource, false) : true;
        }

        private static Boolean StopTraceActivity(String record, MDMTraceSource traceSource)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StopTraceActivity(PopulateTraceRecord(record), traceSource) : true;
        }

        private static Boolean TraceInformation(String record, MDMTraceSource traceSource)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, PopulateTraceRecord(record), traceSource) : true;
        }

        private static Boolean TraceError(String record, MDMTraceSource traceSource)
        {
            return MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, PopulateTraceRecord(record), traceSource);
        }

        private static String PopulateTraceRecord(String record)
        {
            return TracingPrefix + record;
        }

        #endregion
    }
}