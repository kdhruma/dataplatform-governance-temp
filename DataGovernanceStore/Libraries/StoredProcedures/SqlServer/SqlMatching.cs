
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
    public class SqlMatching
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private SqlMatching()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetStatusItemCount(SqlXml InputXml, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_Match_GetStatusItemCount", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@InputXml]");
                        }
                        parameter = new SqlParameter("@InputXml", System.Data.SqlDbType.Xml);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!InputXml.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@InputXml] value[" + InputXml.ToString() + "]");
                            }
                            parameter.Value = InputXml.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_Match_GetStatusItemCount]");
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
        public static void MatchJob(SqlInt32 FK_Job, SqlString UserName, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_MatchJob", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Job]");
                        }
                        parameter = new SqlParameter("@FK_Job", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Job.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Job] value[" + FK_Job.ToString() + "]");
                            }
                            parameter.Value = FK_Job.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserName]");
                        }
                        parameter = new SqlParameter("@UserName", System.Data.SqlDbType.VarChar, 100);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserName.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserName] value[" + UserName.ToString() + "]");
                            }
                            parameter.Value = UserName.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_MatchJob]");
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
        public static void DescriptionMatch(SqlXml xml, SqlString vchrUserID, SqlInt32 Type, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_Match_CreateDescriptionMatchJob", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@xml]");
                        }
                        parameter = new SqlParameter("@xml", System.Data.SqlDbType.Xml);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!xml.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@xml] value[" + xml.ToString() + "]");
                            }
                            parameter.Value = xml.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserID]");
                        }
                        parameter = new SqlParameter("@vchrUserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!vchrUserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrUserID] value[" + vchrUserID.ToString() + "]");
                            }
                            parameter.Value = vchrUserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Type]");
                        }
                        parameter = new SqlParameter("@Type", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Type.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Type] value[" + Type.ToString() + "]");
                            }
                            parameter.Value = Type.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_Match_CreateDescriptionMatchJob]");
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
        public static String CreateServicesJob(SqlXml xml, SqlString vchrUserID, SqlInt32 ServiceType, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_CreateServicesJob", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@xml]");
                        }
                        parameter = new SqlParameter("@xml", System.Data.SqlDbType.Xml);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!xml.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@xml] value[" + xml.ToString() + "]");
                            }
                            parameter.Value = xml.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserID]");
                        }
                        parameter = new SqlParameter("@vchrUserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!vchrUserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrUserID] value[" + vchrUserID.ToString() + "]");
                            }
                            parameter.Value = vchrUserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ServiceType]");
                        }
                        parameter = new SqlParameter("@ServiceType", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ServiceType.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Type] value[" + ServiceType.ToString() + "]");
                            }
                            parameter.Value = ServiceType.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_CreateServicesJob]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();
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
        public static void AttributeExtraction(SqlXml xml, SqlString vchrUserID, SqlInt32 Type, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_CreateAttributeExtractionJob", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@xml]");
                        }
                        parameter = new SqlParameter("@xml", System.Data.SqlDbType.Xml);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!xml.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@xml] value[" + xml.ToString() + "]");
                            }
                            parameter.Value = xml.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserID]");
                        }
                        parameter = new SqlParameter("@vchrUserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!vchrUserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrUserID] value[" + vchrUserID.ToString() + "]");
                            }
                            parameter.Value = vchrUserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Type]");
                        }
                        parameter = new SqlParameter("@Type", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Type.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Type] value[" + Type.ToString() + "]");
                            }
                            parameter.Value = Type.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_CreateAttributeExtractionJob]");
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
        public static void CreateNetricsResult(SqlInt32 FK_Job, SqlString Xmldata, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_CreateNetricsResult", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Job]");
                        }
                        parameter = new SqlParameter("@FK_Job", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Job.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Job] value[" + FK_Job.ToString() + "]");
                            }
                            parameter.Value = FK_Job.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Xmldata]");
                        }
                        parameter = new SqlParameter("@Xmldata", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Xmldata.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Xmldata] value[" + Xmldata.ToString() + "]");
                            }
                            parameter.Value = Xmldata.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_CreateNetricsResult]");
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
        public static DataTable GetMatchRule(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DC_GetMatchRule", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIb]");
                        }
                        parameter = new SqlParameter("@JobIb", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobIb.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIb] value[" + JobIb.ToString() + "]");
                            }
                            parameter.Value = JobIb.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@MatchRulesetId]");
                        }
                        parameter = new SqlParameter("@MatchRulesetId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!MatchRulesetId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@MatchRulesetId] value[" + MatchRulesetId.ToString() + "]");
                            }
                            parameter.Value = MatchRulesetId.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
                        }
                        parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserID] value[" + UserID.ToString() + "]");
                            }
                            parameter.Value = UserID.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DC_GetMatchRule]");
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
        public static DataTable GetMatchCnodeAndAttrval(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID, SqlString Cnodes, SqlString SourceAttIds, SqlInt32 CatalogSource, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DC_GetMatchCnodeAndAttrval", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIb]");
                        }
                        parameter = new SqlParameter("@JobIb", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobIb.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIb] value[" + JobIb.ToString() + "]");
                            }
                            parameter.Value = JobIb.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@MatchRulesetId]");
                        }
                        parameter = new SqlParameter("@MatchRulesetId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!MatchRulesetId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@MatchRulesetId] value[" + MatchRulesetId.ToString() + "]");
                            }
                            parameter.Value = MatchRulesetId.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
                        }
                        parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserID] value[" + UserID.ToString() + "]");
                            }
                            parameter.Value = UserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Cnodes]");
                        }
                        parameter = new SqlParameter("@Cnodes", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Cnodes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Cnodes] value[" + Cnodes.ToString() + "]");
                            }
                            parameter.Value = Cnodes.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SourceAttIds]");
                        }
                        parameter = new SqlParameter("@SourceAttIds", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!SourceAttIds.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SourceAttIds] value[" + SourceAttIds.ToString() + "]");
                            }
                            parameter.Value = SourceAttIds.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CatalogSource]");
                        }
                        parameter = new SqlParameter("@CatalogSource", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!CatalogSource.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CatalogSource] value[" + CatalogSource.ToString() + "]");
                            }
                            parameter.Value = CatalogSource.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DC_GetMatchCnodeAndAttrval]");
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
        public static DataTable GetMatchCnodeList(SqlInt32 JobIb, SqlString UserID, SqlString SourceAttrIds, SqlInt32 fk_catalog, SqlInt32 min_fk_cnode, SqlInt32 max_fk_cnode, SqlBoolean deltas, SqlBoolean autoclass, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DC_MatchGetCnodeList", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIb]");
                        }
                        parameter = new SqlParameter("@JobIb", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobIb.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIb] value[" + JobIb.ToString() + "]");
                            }
                            parameter.Value = JobIb.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
                        }
                        parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserID] value[" + UserID.ToString() + "]");
                            }
                            parameter.Value = UserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SourceAttrIds]");
                        }
                        parameter = new SqlParameter("@SourceAttrIds", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!SourceAttrIds.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SourceAttrIds] value[" + SourceAttrIds.ToString() + "]");
                            }
                            parameter.Value = SourceAttrIds.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@fk_catalog]");
                        }
                        parameter = new SqlParameter("@fk_catalog", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!fk_catalog.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@fk_catalog] value[" + fk_catalog.ToString() + "]");
                            }
                            parameter.Value = fk_catalog.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@min_fk_cnode]");
                        }
                        parameter = new SqlParameter("@min_fk_cnode", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!min_fk_cnode.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@min_fk_cnode] value[" + min_fk_cnode.ToString() + "]");
                            }
                            parameter.Value = min_fk_cnode.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@max_fk_cnode]");
                        }
                        parameter = new SqlParameter("@max_fk_cnode", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!max_fk_cnode.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@max_fk_cnode] value[" + max_fk_cnode.ToString() + "]");
                            }
                            parameter.Value = max_fk_cnode.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@deltas]");
                        }
                        parameter = new SqlParameter("@deltas", System.Data.SqlDbType.Bit);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!deltas.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@deltas] value[" + deltas.ToString() + "]");
                            }
                            parameter.Value = deltas.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@autoclass]");
                        }
                        parameter = new SqlParameter("@autoclass", System.Data.SqlDbType.Bit);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!autoclass.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@autoclass] value[" + autoclass.ToString() + "]");
                            }
                            parameter.Value = autoclass.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DC_MatchGetCnodeList]");
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
        public static DataTable GetMatchCnodeAndAttrvalsForInitLoad(SqlInt32 JobIb, SqlString attrList, SqlString cnodelist, SqlString UserID, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DC_MatchSourceAttrval", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIb]");
                        }
                        parameter = new SqlParameter("@JobIb", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobIb.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIb] value[" + JobIb.ToString() + "]");
                            }
                            parameter.Value = JobIb.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@attrList]");
                        }
                        parameter = new SqlParameter("@attrList", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!attrList.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@attrList] value[" + attrList.ToString() + "]");
                            }
                            parameter.Value = attrList.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@cnodelist]");
                        }
                        parameter = new SqlParameter("@cnodelist", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!cnodelist.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@cnodelist] value[" + cnodelist.ToString() + "]");
                            }
                            parameter.Value = cnodelist.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
                        }
                        parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserID] value[" + UserID.ToString() + "]");
                            }
                            parameter.Value = UserID.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DC_MatchSourceAttrval]");
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
        public static string GetMatchingCnodeAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DC_MatchingGetCnodeAttrvalXml", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIb]");
                        }
                        parameter = new SqlParameter("@JobIb", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobIb.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIb] value[" + JobIb.ToString() + "]");
                            }
                            parameter.Value = JobIb.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
                        }
                        parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserID] value[" + UserID.ToString() + "]");
                            }
                            parameter.Value = UserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Cnodes]");
                        }
                        parameter = new SqlParameter("@Cnodes", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Cnodes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Cnodes] value[" + Cnodes.ToString() + "]");
                            }
                            parameter.Value = Cnodes.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SourceAttrIds]");
                        }
                        parameter = new SqlParameter("@SourceAttrIds", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!SourceAttrIds.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SourceAttrIds] value[" + SourceAttrIds.ToString() + "]");
                            }
                            parameter.Value = SourceAttrIds.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DC_MatchingGetCnodeAttrvalXml]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static string GetMatchingCnodeCategoryAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DC_MatchingGetCnodeCategoryAttrvalXml", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIb]");
                        }
                        parameter = new SqlParameter("@JobIb", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobIb.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIb] value[" + JobIb.ToString() + "]");
                            }
                            parameter.Value = JobIb.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
                        }
                        parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserID] value[" + UserID.ToString() + "]");
                            }
                            parameter.Value = UserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Cnodes]");
                        }
                        parameter = new SqlParameter("@Cnodes", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Cnodes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Cnodes] value[" + Cnodes.ToString() + "]");
                            }
                            parameter.Value = Cnodes.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SourceAttrIds]");
                        }
                        parameter = new SqlParameter("@SourceAttrIds", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!SourceAttrIds.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SourceAttrIds] value[" + SourceAttrIds.ToString() + "]");
                            }
                            parameter.Value = SourceAttrIds.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DC_MatchingGetCnodeCategoryAttrvalXml]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static string GetMatchRuleSetTypeDefaultXml(SqlInt32 fk_dc_matchtypes, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_matchRuleSetTypeDefault_GetXml", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@fk_dc_matchtypes]");
                        }
                        parameter = new SqlParameter("@fk_dc_matchtypes", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!fk_dc_matchtypes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@fk_dc_matchtypes] value[" + fk_dc_matchtypes.ToString() + "]");
                            }
                            parameter.Value = fk_dc_matchtypes.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_matchRuleSetTypeDefault_GetXml]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static string GetmatchRuleSetsXml(SqlInt32 fk_dc_matchtypes, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_matchRuleSets_GetXml", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@fk_dc_matchtypes]");
                        }
                        parameter = new SqlParameter("@fk_dc_matchtypes", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!fk_dc_matchtypes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@fk_dc_matchtypes] value[" + fk_dc_matchtypes.ToString() + "]");
                            }
                            parameter.Value = fk_dc_matchtypes.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_matchRuleSets_GetXml]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static DataTable GetmatchRuleSetsDT(SqlInt32 fk_dc_matchtypes, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_matchRuleSets_GetDT", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@fk_dc_matchtypes]");
                        }
                        parameter = new SqlParameter("@fk_dc_matchtypes", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!fk_dc_matchtypes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@fk_dc_matchtypes] value[" + fk_dc_matchtypes.ToString() + "]");
                            }
                            parameter.Value = fk_dc_matchtypes.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_matchRuleSets_GetDT]");
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
        public static string GetMatchRuleSetCnodeAndAttrvalXml(SqlInt32 JobId, SqlInt32 MatchRulesetId, SqlString UserID, SqlString cnodelist, SqlBoolean debug, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DC_GetMatchRuleSetCnodeAndAttrvalXml", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobId]");
                        }
                        parameter = new SqlParameter("@JobId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!JobId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobId] value[" + JobId.ToString() + "]");
                            }
                            parameter.Value = JobId.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@MatchRulesetId]");
                        }
                        parameter = new SqlParameter("@MatchRulesetId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!MatchRulesetId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@MatchRulesetId] value[" + MatchRulesetId.ToString() + "]");
                            }
                            parameter.Value = MatchRulesetId.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
                        }
                        parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 50);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserID.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserID] value[" + UserID.ToString() + "]");
                            }
                            parameter.Value = UserID.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@cnodelist]");
                        }
                        parameter = new SqlParameter("@cnodelist", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!cnodelist.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@cnodelist] value[" + cnodelist.ToString() + "]");
                            }
                            parameter.Value = cnodelist.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@debug]");
                        }
                        parameter = new SqlParameter("@debug", System.Data.SqlDbType.Bit);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!debug.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@debug] value[" + debug.ToString() + "]");
                            }
                            parameter.Value = debug.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DC_GetMatchRuleSetCnodeAndAttrvalXml]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static string GetMatchingKeywordListXml(SqlString UserName, SqlInt32 fk_synonymList, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_Match_GetKeywordListXml", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserName]");
                        }
                        parameter = new SqlParameter("@UserName", System.Data.SqlDbType.VarChar, 100);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserName.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserName] value[" + UserName.ToString() + "]");
                            }
                            parameter.Value = UserName.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@fk_synonymList]");
                        }
                        parameter = new SqlParameter("@fk_synonymList", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!fk_synonymList.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@fk_synonymList] value[" + fk_synonymList.ToString() + "]");
                            }
                            parameter.Value = fk_synonymList.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_Match_GetKeywordListXml]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static string GetMatchingKeyWordAndSynonymsXml(SqlString UserName, SqlInt32 Min_fk_keyword, SqlInt32 Max_fk_keyword, SqlInt32 fk_synonymList, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_Match_GetSynonymsXml", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserName]");
                        }
                        parameter = new SqlParameter("@UserName", System.Data.SqlDbType.VarChar, 100);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!UserName.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserName] value[" + UserName.ToString() + "]");
                            }
                            parameter.Value = UserName.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Min_fk_keyword]");
                        }
                        parameter = new SqlParameter("@Min_fk_keyword", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Min_fk_keyword.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Min_fk_keyword] value[" + Min_fk_keyword.ToString() + "]");
                            }
                            parameter.Value = Min_fk_keyword.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Max_fk_keyword]");
                        }
                        parameter = new SqlParameter("@Max_fk_keyword", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Max_fk_keyword.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Max_fk_keyword] value[" + Max_fk_keyword.ToString() + "]");
                            }
                            parameter.Value = Max_fk_keyword.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@fk_synonymList]");
                        }
                        parameter = new SqlParameter("@fk_synonymList", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!fk_synonymList.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@fk_synonymList] value[" + fk_synonymList.ToString() + "]");
                            }
                            parameter.Value = fk_synonymList.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_Match_GetSynonymsXml]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static DataTable GetServiceTypes(SqlInt32 JobID, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_getServiceTypes", (SqlConnection)connection))
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

                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_getServiceTypes]");
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
        public static DataTable GetRSPLMatchingStatus(SqlString CNodes, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_RSPL_GetMatchingStatus", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodes]");
                        }
                        parameter = new SqlParameter("@CNodes", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!CNodes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodes] value[" + CNodes.ToString() + "]");
                            }
                            parameter.Value = CNodes.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Catalog]");
                        }
                        parameter = new SqlParameter("@FK_Catalog", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Catalog.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Catalog] value[" + FK_Catalog.ToString() + "]");
                            }
                            parameter.Value = FK_Catalog.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_RSPL_GetMatchingStatus]");
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
        public static string AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Locale, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_DQM_ExtractAttribute", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Locale]");
                        }
                        parameter = new SqlParameter("@FK_Locale", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Locale.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Locale] value[" + FK_Locale.ToString() + "]");
                            }
                            parameter.Value = FK_Locale.Value;
                        }

                        StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_DQM_ExtractAttribute]");
                        }

                        using (SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                buffer.Append(dataReader[0]);
                            }
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + buffer.ToString() + "]");
                        }

                        return buffer.ToString();

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
        public static DataTable GetMatchedCnodes(SqlString NodeName, SqlXml DataXML, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_FindMatchedCnodes", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Source_Catalog]");
                        }
                        parameter = new SqlParameter("@NodeName", System.Data.SqlDbType.NVarChar, 100);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!NodeName.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@NodeName] value[" + NodeName.ToString() + "]");
                            }
                            parameter.Value = NodeName.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@DataXML]");
                        }
                        parameter = new SqlParameter("@DataXML", System.Data.SqlDbType.Xml);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!DataXML.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@DataXML] value[" + DataXML.ToString() + "]");
                            }
                            parameter.Value = DataXML.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_FindMatchedCnodes]");
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
        public static DataTable GetTextBasedPartNumberMatchingResults(SqlString MatchingRules, SqlString ContextXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DQM_PNM_RunLoad_Dynamic", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@MatchingRules]");
                        }
                        parameter = new SqlParameter("@MatchingRules", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!MatchingRules.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@MatchingRules] value[" + MatchingRules.ToString() + "]");
                            }
                            parameter.Value = MatchingRules.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ContextXML]");
                        }
                        parameter = new SqlParameter("@ContextXML", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ContextXML.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ContextXML] value[" + ContextXML.ToString() + "]");
                            }
                            parameter.Value = ContextXML.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
                        }
                        parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 150);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!vchrUserLogin.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrUserLogin] value[" + vchrUserLogin.ToString() + "]");
                            }
                            parameter.Value = vchrUserLogin.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DQM_PNM_RunLoad_Dynamic]");
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
        public static void AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Profile_Id, SqlString loginUser, ref SqlInt32 TotalProcessed, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DQM_ProcessPDRProfile", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 300000000;

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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Profile_Id]");
                        }
                        parameter = new SqlParameter("@FK_Profile_Id", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Profile_Id.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Profile_Id] value[" + FK_Profile_Id.ToString() + "]");
                            }
                            parameter.Value = FK_Profile_Id.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@loginUser]");
                        }
                        parameter = new SqlParameter("@loginUser", System.Data.SqlDbType.VarChar, 100);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!loginUser.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@loginUser] value[" + loginUser.ToString() + "]");
                            }
                            parameter.Value = loginUser.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@TotalProcessed]");
                        }
                        parameter = new SqlParameter("@TotalProcessed", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.InputOutput;
                        command.Parameters.Add(parameter);

                        if (!TotalProcessed.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@TotalProcessed] value[" + TotalProcessed.ToString() + "]");
                            }
                            parameter.Value = TotalProcessed.Value;
                        }


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DQM_ProcessPDRProfile]");
                        }

                        dataAdapter.SelectCommand.ExecuteNonQuery();


                        TotalProcessed = new SqlInt32((int)command.Parameters["@TotalProcessed"].Value);

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
