
using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using MDM.Core;
using MDM.Utility;

namespace Riversand.StoredProcedures
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlAggregation
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private SqlAggregation()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static void ImportProduct(SqlInt32 FK_JobService, SqlString UserLogin, SqlInt32 FK_Supplier, SqlBoolean ToLoadCategories, SqlBoolean SchemaValidation, IDbConnection connection, IDbTransaction transaction)
        {
            DateTime startDate = DateTime.Now;
            bool internalConnect = false;
            try
            {
                if (connection == null)
                {
                    string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    internalConnect = true;
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    internalConnect = true;
                }
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Import_Product", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 200000000;

                        IDataParameter parameter = null;
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_JobService]");
                        }
                        parameter = new SqlParameter("@FK_JobService", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_JobService.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_JobService] value[" + FK_JobService.ToString() + "]");
                            }
                            parameter.Value = FK_JobService.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserLogin]");
                        }
                        parameter = new SqlParameter("@UserLogin", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserLogin.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserLogin] value[" + UserLogin.ToString() + "]");
                            }
                            parameter.Value = UserLogin.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Supplier]");
                        }
                        parameter = new SqlParameter("@FK_Supplier", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Supplier.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Supplier] value[" + FK_Supplier.ToString() + "]");
                            }
                            parameter.Value = FK_Supplier.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ToLoadCategories]");
                        }
                        parameter = new SqlParameter("@ToLoadCategories", System.Data.SqlDbType.Bit);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ToLoadCategories.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ToLoadCategories] value[" + ToLoadCategories.ToString() + "]");
                            }
                            parameter.Value = ToLoadCategories.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SchemaValidation]");
                        }
                        parameter = new SqlParameter("@SchemaValidation", System.Data.SqlDbType.Bit);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!SchemaValidation.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SchemaValidation] value[" + SchemaValidation.ToString() + "]");
                            }
                            parameter.Value = SchemaValidation.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Import_Product]");
                        }

                        dataAdapter.SelectCommand.ExecuteNonQuery();


                    }
                }
                finally
                {
                    if (internalConnect)
                    {
                        connection.Close();
                    }
                    if (Constants.TRACING_ENABLED)
                    {
                        DateTime endDate = DateTime.Now;
                        TimeSpan span = endDate.Subtract(startDate);
                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Time Spent:" + span.TotalMilliseconds.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageData(SqlString FileNameList, SqlString TableNameList, SqlInt32 JobID, SqlString FieldTerminator, SqlString RowTerminator, IDbConnection connection, IDbTransaction transaction)
        {
            DateTime startDate = DateTime.Now;
            bool internalConnect = false;
            try
            {
                if (connection == null)
                {
                    string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    internalConnect = true;
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    internalConnect = true;
                }
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_import_stagedata_fromfile", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 200000000;

                        IDataParameter parameter = null;
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FileNameList]");
                        }
                        parameter = new SqlParameter("@FileNameList", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FileNameList.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FileNameList] value[" + FileNameList.ToString() + "]");
                            }
                            parameter.Value = FileNameList.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@TableNameList]");
                        }
                        parameter = new SqlParameter("@TableNameList", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!TableNameList.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@TableNameList] value[" + TableNameList.ToString() + "]");
                            }
                            parameter.Value = TableNameList.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobID]");
                        }
                        parameter = new SqlParameter("@JobID", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobID] value[" + JobID.ToString() + "]");
                            }
                            parameter.Value = JobID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FieldTerminator]");
                        }
                        parameter = new SqlParameter("@FieldTerminator", System.Data.SqlDbType.VarChar, 10);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FieldTerminator.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FieldTerminator] value[" + FieldTerminator.ToString() + "]");
                            }
                            parameter.Value = FieldTerminator.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RowTerminator]");
                        }
                        parameter = new SqlParameter("@RowTerminator", System.Data.SqlDbType.VarChar, 10);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!RowTerminator.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@RowTerminator] value[" + RowTerminator.ToString() + "]");
                            }
                            parameter.Value = RowTerminator.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_import_stagedata_fromfile]");
                        }

                        dataAdapter.SelectCommand.ExecuteNonQuery();


                    }
                }
                finally
                {
                    if (internalConnect)
                    {
                        connection.Close();
                    }
                    if (Constants.TRACING_ENABLED)
                    {
                        DateTime endDate = DateTime.Now;
                        TimeSpan span = endDate.Subtract(startDate);
                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Time Spent:" + span.TotalMilliseconds.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageDataFromXML(SqlXml dataXml, SqlInt32 jobId, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            DateTime startDate = DateTime.Now;
            bool internalConnect = false;
            try
            {
                if (connection == null)
                {
                    string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    internalConnect = true;
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    internalConnect = true;
                }
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Import_StageData_FromDSXML", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@dataXml]");
                        }
                        parameter = new SqlParameter("@dataXml", System.Data.SqlDbType.Xml);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!dataXml.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@dataXml] value[" + dataXml.ToString() + "]");
                            }
                            parameter.Value = dataXml.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@jobId]");
                        }
                        parameter = new SqlParameter("@jobId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!jobId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@jobId] value[" + jobId.ToString() + "]");
                            }
                            parameter.Value = jobId.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Import_StageData_FromDSXML]");
                        }

                        dataAdapter.SelectCommand.ExecuteNonQuery();


                        RETURN_VALUE = new SqlInt32((int)command.Parameters["@RETURN_VALUE"].Value);

                    }
                }
                finally
                {
                    if (internalConnect)
                    {
                        connection.Close();
                    }
                    if (Constants.TRACING_ENABLED)
                    {
                        DateTime endDate = DateTime.Now;
                        TimeSpan span = endDate.Subtract(startDate);
                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Time Spent:" + span.TotalMilliseconds.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable sp_help(SqlString objname, IDbConnection connection, IDbTransaction transaction)
        {
            DateTime startDate = DateTime.Now;
            bool internalConnect = false;
            try
            {
                if (connection == null)
                {
                    string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    internalConnect = true;
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    internalConnect = true;
                }
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("sp_help", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@objname]");
                        }
                        parameter = new SqlParameter("@objname", System.Data.SqlDbType.NVarChar, 776);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!objname.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@objname] value[" + objname.ToString() + "]");
                            }
                            parameter.Value = objname.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[sp_help]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + dataSet.GetXml() + "]");
                        }

                        return dataSet.Tables[0];

                    }
                }
                finally
                {
                    if (internalConnect)
                    {
                        connection.Close();
                    }
                    if (Constants.TRACING_ENABLED)
                    {
                        DateTime endDate = DateTime.Now;
                        TimeSpan span = endDate.Subtract(startDate);
                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Time Spent:" + span.TotalMilliseconds.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateStageTableFromTable(SqlString SourceTableName, SqlBoolean dropStaging, IDbConnection connection, IDbTransaction transaction)
        {
            DateTime startDate = DateTime.Now;
            bool internalConnect = false;
            try
            {
                if (connection == null)
                {
                    string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    internalConnect = true;
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    internalConnect = true;
                }
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Import_CreateStageTableBasedOnTable", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SourceTableName]");
                        }
                        parameter = new SqlParameter("@SourceTableName", System.Data.SqlDbType.VarChar, 1000);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!SourceTableName.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SourceTableName] value[" + SourceTableName.ToString() + "]");
                            }
                            parameter.Value = SourceTableName.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@dropStaging]");
                        }
                        parameter = new SqlParameter("@dropStaging", System.Data.SqlDbType.Bit);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!dropStaging.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@dropStaging] value[" + dropStaging.ToString() + "]");
                            }
                            parameter.Value = dropStaging.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Import_CreateStageTableBasedOnTable]");
                        }

                        dataAdapter.SelectCommand.ExecuteNonQuery();


                    }
                }
                finally
                {
                    if (internalConnect)
                    {
                        connection.Close();
                    }
                    if (Constants.TRACING_ENABLED)
                    {
                        DateTime endDate = DateTime.Now;
                        TimeSpan span = endDate.Subtract(startDate);
                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Time Spent:" + span.TotalMilliseconds.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ImportWorkerTableCreation(SqlInt32 WorkerCount, IDbConnection connection, IDbTransaction transaction)
        {
            DateTime startDate = DateTime.Now;
            bool internalConnect = false;
            try
            {
                if (connection == null)
                {
                    string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    internalConnect = true;
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    internalConnect = true;
                }
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Import_Table_Creation_Script", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 6000;

                        IDataParameter parameter = null;
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@WorkerCount]");
                        }
                        parameter = new SqlParameter("@WorkerCount", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!WorkerCount.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@WorkerCount] value[" + WorkerCount.ToString() + "]");
                            }
                            parameter.Value = WorkerCount.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Import_Table_Creation_Script]");
                        }

                        dataAdapter.SelectCommand.ExecuteNonQuery();


                    }
                }
                finally
                {
                    if (internalConnect)
                    {
                        connection.Close();
                    }
                    if (Constants.TRACING_ENABLED)
                    {
                        DateTime endDate = DateTime.Now;
                        TimeSpan span = endDate.Subtract(startDate);
                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Time Spent:" + span.TotalMilliseconds.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
                throw;
            }
        }

    }
}
