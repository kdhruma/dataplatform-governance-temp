
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
    public class SqlCustomSecurity
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private SqlCustomSecurity()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static void CalculateCustomSecurity_GetDT(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_CalculateCustomSecurity_GetDT", (SqlConnection)connection))
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
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@itemList]");
                        }
                        parameter = new SqlParameter("@itemList", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!itemList.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@itemList] value[" + itemList.ToString() + "]");
                            }
                            parameter.Value = itemList.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@permissionLevel]");
                        }
                        parameter = new SqlParameter("@permissionLevel", System.Data.SqlDbType.NVarChar, 10);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!permissionLevel.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@permissionLevel] value[" + permissionLevel.ToString() + "]");
                            }
                            parameter.Value = permissionLevel.Value;
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


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_CalculateCustomSecurity_GetDT]");
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
        public static void CalculateCustomSecurity_GetXML(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_CalculateCustomSecurity_GetXML", (SqlConnection)connection))
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
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@itemList]");
                        }
                        parameter = new SqlParameter("@itemList", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!itemList.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@itemList] value[" + itemList.ToString() + "]");
                            }
                            parameter.Value = itemList.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@permissionLevel]");
                        }
                        parameter = new SqlParameter("@permissionLevel", System.Data.SqlDbType.NVarChar, 10);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!permissionLevel.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@permissionLevel] value[" + permissionLevel.ToString() + "]");
                            }
                            parameter.Value = permissionLevel.Value;
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


                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_CalculateCustomSecurity_GetXML]");
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

    }
}		

