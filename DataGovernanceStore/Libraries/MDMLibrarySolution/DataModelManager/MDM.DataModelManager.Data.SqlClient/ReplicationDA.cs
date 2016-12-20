using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Data;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    /// Specifies Replication Data Access
    /// </summary>
    public class ReplicationDA : SqlClientDataAccessBase
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
        /// Modify Article of replication
        /// </summary>
        /// <param name="tableNames">This parameter is specifying table names to be replicate.</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamicTableType</param>
        /// <param name="action">This parameter is specifying action of table for replication</param>
        /// <param name="moduleId">This parameter is specifying module id</param>
        /// <returns>If Operation performed Successful, returns the Job Name else returns Empty String</returns>
        public String ModifyArticle(Collection<String> tableNames, DynamicTableType dynamicTableType, ReplicationType action, Int32 moduleId)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.ReplicationDA.ModifyArticle", false);

            String jobName = String.Empty;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

                parameters = generator.GetParameters("DataModelManager_Replication_ModifyArticle_ParametersArray");

                #region Populate table value parameters

                List<SqlDataRecord> tableNamesList = null;
                SqlMetaData[] replicationMetadata = generator.GetTableValueMetadata("DataModelManager_Replication_ModifyArticle_ParametersArray", parameters[0].ParameterName);

                SqlDataRecord replicationRecord = null;

                if (tableNames != null && tableNames.Count > 0)
                {
                    tableNamesList = new List<SqlDataRecord>();

                    foreach (String tableName in tableNames)
                    {
                        replicationRecord = new SqlDataRecord(replicationMetadata);
                        replicationRecord.SetValue(0, tableName);
                        tableNamesList.Add(replicationRecord);
                    }
                }

                #endregion

                parameters[0].Value = tableNamesList;
                parameters[1].Value = dynamicTableType.ToString();
                parameters[2].Value = action.ToString();
                parameters[3].Value = moduleId;

                storedProcedureName = "usp_Replication_ModifyArticle_Bulk";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (reader["IsError"] != null)
                    {
                        if (reader["IsError"].ToString() == "0")
                        {
                            if (reader["jobName"] != null)
                                jobName = reader["jobName"].ToString();
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.ReplicationDA.ModifyArticle");
            }

            return jobName;
        }

        /// <summary>
        /// Adds Job to execute on the Distributor in order to replicate Lookup and Complex Attribute Tables to Subscribers
        /// </summary>
        /// <param name="jobName">The Job Name to be run on the Distributor DB</param>
        /// <param name="command">Distributor DB's Connection Details provided as DBCommandProperties Object</param>
        public void AddJobToDistributor(String jobName, DBCommandProperties command)
        {
            String script = "DECLARE @jobname NVARCHAR(300)" +
                            "; SELECT @jobname = jobs.NAME FROM    msdb.dbo.sysjobs jobs (nolock) INNER JOIN msdb.dbo.syscategories categories (nolock) ON jobs.category_id = categories.category_id WHERE  categories.name = 'REPL-Snapshot' AND jobs.name LIKE '" + jobName + "%'" +
                            ";IF  EXISTS (	SELECT jobs.NAME FROM    msdb.dbo.sysjobs jobs (nolock) INNER JOIN msdb.dbo.syscategories categories (nolock) ON jobs.category_id = categories.category_id WHERE  categories.name = 'REPL-Snapshot' AND jobs.name = @jobname)" +
                            " BEGIN " +
                            " IF EXISTS(	SELECT 1 FROM msdb.dbo.sysjobs_view job INNER JOIN msdb.dbo.sysjobactivity activity ON job.job_id = activity.job_id WHERE run_Requested_date IS NOT NULL AND stop_execution_date IS NULL AND job.NAME = @jobname )" +
                            " BEGIN " +
                            " WAITFOR DELAY '00:00:20' " +
                            " END " +
                            " EXEC msdb..sp_start_job @jobname " +
                            " END ";

            ExecuteScript(command.ConnectionString, script, CommandType.Text);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Special method to directly access Distributor DB and run the Replication Job.
        /// Since SqlClientDataAccessBase Class does not provide public method to inherit/use to run SQL Script/Job, written a new private method
        /// within this/ReplicationDA Class itself.
        /// </summary>
        /// <param name="connectionString">Connection String to connect to Distributor DB</param>
        /// <param name="storedProcedureOrSqlText">SQL Script to be run on the Distributor</param>
        /// <param name="type">Command Type of the SQL Script, which is a Text</param>
        private void ExecuteScript(String connectionString, String storedProcedureOrSqlText, CommandType type)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = storedProcedureOrSqlText;
                command.CommandType = type;

                Int32 dataCommandTimeout = AppConfiguration.GetSettingAsInteger("DataCommandTimeout");
                command.CommandTimeout = dataCommandTimeout;

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        #endregion

        #endregion
    }
}
