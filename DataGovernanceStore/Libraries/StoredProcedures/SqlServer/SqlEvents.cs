
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Riversand.StoredProcedures
{
    using MDM.Utility;

    /// <summary>
    /// 
    /// </summary>
    public class SqlEvents
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private SqlEvents()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetApplicationConfigXML(SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 PK_Application_Config, SqlInt32 FK_Locale, SqlString categoryPath, SqlString lookupName, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Application_Config_Get", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Source]");
                        }
                        parameter = new SqlParameter("@FK_Event_Source", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Event_Source.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Source] value[" + FK_Event_Source.ToString() + "]");
                            }
                            parameter.Value = FK_Event_Source.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Subscriber]");
                        }
                        parameter = new SqlParameter("@FK_Event_Subscriber", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Event_Subscriber.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Subscriber] value[" + FK_Event_Subscriber.ToString() + "]");
                            }
                            parameter.Value = FK_Event_Subscriber.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Security_Role]");
                        }
                        parameter = new SqlParameter("@FK_Security_Role", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Security_Role.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Security_Role] value[" + FK_Security_Role.ToString() + "]");
                            }
                            parameter.Value = FK_Security_Role.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Security_user]");
                        }
                        parameter = new SqlParameter("@FK_Security_user", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Security_user.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Security_user] value[" + FK_Security_user.ToString() + "]");
                            }
                            parameter.Value = FK_Security_user.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Org]");
                        }
                        parameter = new SqlParameter("@FK_Org", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Org.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Org] value[" + FK_Org.ToString() + "]");
                            }
                            parameter.Value = FK_Org.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Catalog]");
                        }
                        parameter = new SqlParameter("@FK_Catalog", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Catalog.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Catalog] value[" + FK_Catalog.ToString() + "]");
                            }
                            parameter.Value = FK_Catalog.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Category]");
                        }
                        parameter = new SqlParameter("@FK_Category", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Category.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Category] value[" + FK_Category.ToString() + "]");
                            }
                            parameter.Value = FK_Category.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_CNode]");
                        }
                        parameter = new SqlParameter("@FK_CNode", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_CNode.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_CNode] value[" + FK_CNode.ToString() + "]");
                            }
                            parameter.Value = FK_CNode.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Attribute]");
                        }
                        parameter = new SqlParameter("@FK_Attribute", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Attribute.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Attribute] value[" + FK_Attribute.ToString() + "]");
                            }
                            parameter.Value = FK_Attribute.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_NodeType]");
                        }
                        parameter = new SqlParameter("@FK_NodeType", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_NodeType.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_NodeType] value[" + FK_NodeType.ToString() + "]");
                            }
                            parameter.Value = FK_NodeType.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_RelationshipType]");
                        }
                        parameter = new SqlParameter("@FK_RelationshipType", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_RelationshipType.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_RelationshipType] value[" + FK_RelationshipType.ToString() + "]");
                            }
                            parameter.Value = FK_RelationshipType.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Application_Config]");
                        }
                        parameter = new SqlParameter("@PK_Application_Config", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PK_Application_Config.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Application_Config] value[" + PK_Application_Config.ToString() + "]");
                            }
                            parameter.Value = PK_Application_Config.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Locale]");
                        }
                        parameter = new SqlParameter("@FK_Locale", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Locale.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Locale] value[" + FK_Locale.ToString() + "]");
                            }
                            parameter.Value = FK_Locale.Value;
                        }

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ObjectName]");
                        }
                        parameter = new SqlParameter("@ObjectName", System.Data.SqlDbType.Text);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!lookupName.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ObjectName] value[" + lookupName.Value + "]");
                            }
                            parameter.Value = lookupName.Value;
                        }

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CategoryPath]");
                        }
                        parameter = new SqlParameter("@CategoryPath", System.Data.SqlDbType.Text);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!categoryPath.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CategoryPath] value[" + categoryPath.Value + "]");
                            }
                            parameter.Value = categoryPath.Value;
                        }

                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Application_Config_Get]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (isTracingEnabled)
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
                    if (isTracingEnabled)
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
        public static DataTable GetChildApplicationConfigsXML(SqlInt32 FK_Application_ConfigParent, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Application_ChildConfig_Get", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_ConfigParent]");
                        }
                        parameter = new SqlParameter("@FK_Application_ConfigParent", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Application_ConfigParent.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_ConfigParent] value[" + FK_Application_ConfigParent.ToString() + "]");
                            }
                            parameter.Value = FK_Application_ConfigParent.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Application_ChildConfig_Get]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (isTracingEnabled)
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
                    if (isTracingEnabled)
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
        public static DataTable GetEventSources(SqlInt32 PK_Event_Source, SqlString EventSourceName, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Event_Source_Get", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Event_Source]");
                        }
                        parameter = new SqlParameter("@PK_Event_Source", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PK_Event_Source.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Event_Source] value[" + PK_Event_Source.ToString() + "]");
                            }
                            parameter.Value = PK_Event_Source.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@EventSourceName]");
                        }
                        parameter = new SqlParameter("@EventSourceName", System.Data.SqlDbType.NVarChar, 200);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!EventSourceName.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@EventSourceName] value[" + EventSourceName.ToString() + "]");
                            }
                            parameter.Value = EventSourceName.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Event_Source_Get]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (isTracingEnabled)
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
                    if (isTracingEnabled)
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
        public static DataTable GetEventSubscribers(SqlInt32 PK_Event_Subscriber, SqlString EventSubscriberName, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Event_Subscriber_Get", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Event_Subscriber]");
                        }
                        parameter = new SqlParameter("@PK_Event_Subscriber", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PK_Event_Subscriber.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Event_Subscriber] value[" + PK_Event_Subscriber.ToString() + "]");
                            }
                            parameter.Value = PK_Event_Subscriber.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@EventSubscriberName]");
                        }
                        parameter = new SqlParameter("@EventSubscriberName", System.Data.SqlDbType.NVarChar, 200);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!EventSubscriberName.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@EventSubscriberName] value[" + EventSubscriberName.ToString() + "]");
                            }
                            parameter.Value = EventSubscriberName.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Event_Subscriber_Get]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (isTracingEnabled)
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
                    if (isTracingEnabled)
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
        public static DataTable GetApplicationConfigTypes(SqlInt32 PK_Application_ConfigType, SqlString ApplicationConfigTypeName, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Application_ConfigType_Get", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Application_ConfigType]");
                        }
                        parameter = new SqlParameter("@PK_Application_ConfigType", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PK_Application_ConfigType.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Application_ConfigType] value[" + PK_Application_ConfigType.ToString() + "]");
                            }
                            parameter.Value = PK_Application_ConfigType.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ApplicationConfigTypeName]");
                        }
                        parameter = new SqlParameter("@ApplicationConfigTypeName", System.Data.SqlDbType.NVarChar, 200);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ApplicationConfigTypeName.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ApplicationConfigTypeName] value[" + ApplicationConfigTypeName.ToString() + "]");
                            }
                            parameter.Value = ApplicationConfigTypeName.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Application_ConfigType_Get]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (isTracingEnabled)
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
                    if (isTracingEnabled)
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
        public static void UpdateApplicationConfigXML(SqlInt32 FK_Application_ContextDefinition, SqlInt32 FK_Application_ConfigParent, SqlString ShortName, SqlString LongName, SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlString ConfigXML, SqlString Description, SqlString PreCondition, SqlString PostCondition, SqlString XSDSchema, SqlString SampleXML, SqlString loginUser, SqlString userProgram, SqlInt32 FK_Locale, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Application_Config_Set", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_ContextDefinition]");
                        }
                        parameter = new SqlParameter("@FK_Application_ContextDefinition", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Application_ContextDefinition.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_ContextDefinition] value[" + FK_Application_ContextDefinition.ToString() + "]");
                            }
                            parameter.Value = FK_Application_ContextDefinition.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_ConfigParent]");
                        }
                        parameter = new SqlParameter("@FK_Application_ConfigParent", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Application_ConfigParent.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_ConfigParent] value[" + FK_Application_ConfigParent.ToString() + "]");
                            }
                            parameter.Value = FK_Application_ConfigParent.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ShortName]");
                        }
                        parameter = new SqlParameter("@ShortName", System.Data.SqlDbType.NVarChar, 200);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ShortName.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ShortName] value[" + ShortName.ToString() + "]");
                            }
                            parameter.Value = ShortName.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LongName]");
                        }
                        parameter = new SqlParameter("@LongName", System.Data.SqlDbType.NVarChar, 200);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!LongName.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LongName] value[" + LongName.ToString() + "]");
                            }
                            parameter.Value = LongName.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Source]");
                        }
                        parameter = new SqlParameter("@FK_Event_Source", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Event_Source.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Source] value[" + FK_Event_Source.ToString() + "]");
                            }
                            parameter.Value = FK_Event_Source.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Subscriber]");
                        }
                        parameter = new SqlParameter("@FK_Event_Subscriber", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Event_Subscriber.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Subscriber] value[" + FK_Event_Subscriber.ToString() + "]");
                            }
                            parameter.Value = FK_Event_Subscriber.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Org]");
                        }
                        parameter = new SqlParameter("@FK_Org", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Org.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Org] value[" + FK_Org.ToString() + "]");
                            }
                            parameter.Value = FK_Org.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Catalog]");
                        }
                        parameter = new SqlParameter("@FK_Catalog", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Catalog.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Catalog] value[" + FK_Catalog.ToString() + "]");
                            }
                            parameter.Value = FK_Catalog.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Category]");
                        }
                        parameter = new SqlParameter("@FK_Category", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Category.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Category] value[" + FK_Category.ToString() + "]");
                            }
                            parameter.Value = FK_Category.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_CNode]");
                        }
                        parameter = new SqlParameter("@FK_CNode", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_CNode.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_CNode] value[" + FK_CNode.ToString() + "]");
                            }
                            parameter.Value = FK_CNode.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Attribute]");
                        }
                        parameter = new SqlParameter("@FK_Attribute", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Attribute.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Attribute] value[" + FK_Attribute.ToString() + "]");
                            }
                            parameter.Value = FK_Attribute.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_NodeType]");
                        }
                        parameter = new SqlParameter("@FK_NodeType", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_NodeType.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_NodeType] value[" + FK_NodeType.ToString() + "]");
                            }
                            parameter.Value = FK_NodeType.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_RelationshipType]");
                        }
                        parameter = new SqlParameter("@FK_RelationshipType", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_RelationshipType.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_RelationshipType] value[" + FK_RelationshipType.ToString() + "]");
                            }
                            parameter.Value = FK_RelationshipType.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Security_Role]");
                        }
                        parameter = new SqlParameter("@FK_Security_Role", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Security_Role.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Security_Role] value[" + FK_Security_Role.ToString() + "]");
                            }
                            parameter.Value = FK_Security_Role.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Security_user]");
                        }
                        parameter = new SqlParameter("@FK_Security_user", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Security_user.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Security_user] value[" + FK_Security_user.ToString() + "]");
                            }
                            parameter.Value = FK_Security_user.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ConfigXML]");
                        }
                        parameter = new SqlParameter("@ConfigXML", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ConfigXML.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ConfigXML] value[" + ConfigXML.ToString() + "]");
                            }
                            parameter.Value = ConfigXML.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Description]");
                        }
                        parameter = new SqlParameter("@Description", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Description.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Description] value[" + Description.ToString() + "]");
                            }
                            parameter.Value = Description.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PreCondition]");
                        }
                        parameter = new SqlParameter("@PreCondition", System.Data.SqlDbType.NVarChar, 500);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PreCondition.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PreCondition] value[" + PreCondition.ToString() + "]");
                            }
                            parameter.Value = PreCondition.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PostCondition]");
                        }
                        parameter = new SqlParameter("@PostCondition", System.Data.SqlDbType.NVarChar, 500);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PostCondition.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PostCondition] value[" + PostCondition.ToString() + "]");
                            }
                            parameter.Value = PostCondition.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@XSDSchema]");
                        }
                        parameter = new SqlParameter("@XSDSchema", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!XSDSchema.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@XSDSchema] value[" + XSDSchema.ToString() + "]");
                            }
                            parameter.Value = XSDSchema.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SampleXML]");
                        }
                        parameter = new SqlParameter("@SampleXML", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!SampleXML.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SampleXML] value[" + SampleXML.ToString() + "]");
                            }
                            parameter.Value = SampleXML.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@loginUser]");
                        }
                        parameter = new SqlParameter("@loginUser", System.Data.SqlDbType.NVarChar, 200);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!loginUser.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@loginUser] value[" + loginUser.ToString() + "]");
                            }
                            parameter.Value = loginUser.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userProgram]");
                        }
                        parameter = new SqlParameter("@userProgram", System.Data.SqlDbType.NVarChar, 200);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!userProgram.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@userProgram] value[" + userProgram.ToString() + "]");
                            }
                            parameter.Value = userProgram.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Locale]");
                        }
                        parameter = new SqlParameter("@FK_Locale", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Locale.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Locale] value[" + FK_Locale.ToString() + "]");
                            }
                            parameter.Value = FK_Locale.Value;
                        }


                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Application_Config_Set]");
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
                    if (isTracingEnabled)
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
        public static DataTable GetMatchinRuleSets(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 eventSource, SqlInt32 eventSubscriber, SqlInt32 fkSecurityUser, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_getMatchingRuleSets", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Org]");
                        }
                        parameter = new SqlParameter("@FK_Org", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Org.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Org] value[" + FK_Org.ToString() + "]");
                            }
                            parameter.Value = FK_Org.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Catalog]");
                        }
                        parameter = new SqlParameter("@FK_Catalog", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!FK_Catalog.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Catalog] value[" + FK_Catalog.ToString() + "]");
                            }
                            parameter.Value = FK_Catalog.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@eventSource]");
                        }
                        parameter = new SqlParameter("@eventSource", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!eventSource.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@eventSource] value[" + eventSource.ToString() + "]");
                            }
                            parameter.Value = eventSource.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@eventSubscriber]");
                        }
                        parameter = new SqlParameter("@eventSubscriber", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!eventSubscriber.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@eventSubscriber] value[" + eventSubscriber.ToString() + "]");
                            }
                            parameter.Value = eventSubscriber.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@fkSecurityUser]");
                        }
                        parameter = new SqlParameter("@fkSecurityUser", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!fkSecurityUser.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@fkSecurityUser] value[" + fkSecurityUser.ToString() + "]");
                            }
                            parameter.Value = fkSecurityUser.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_getMatchingRuleSets]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (isTracingEnabled)
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
                    if (isTracingEnabled)
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
        public static DataTable GetTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, IDbConnection connection, IDbTransaction transaction)
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

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                try
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Lookup_getTranslationMemory", (SqlConnection)connection))
                    {
                        SqlCommand command = dataAdapter.SelectCommand;
                        command.Transaction = (SqlTransaction)transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        IDataParameter parameter = null;
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
                        }
                        parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@OrigLocale]");
                        }
                        parameter = new SqlParameter("@OrigLocale", System.Data.SqlDbType.NVarChar, 10);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!OrigLocale.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@OrigLocale] value[" + OrigLocale.ToString() + "]");
                            }
                            parameter.Value = OrigLocale.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@TransLocale]");
                        }
                        parameter = new SqlParameter("@TransLocale", System.Data.SqlDbType.NVarChar, 10);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!TransLocale.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@TransLocale] value[" + TransLocale.ToString() + "]");
                            }
                            parameter.Value = TransLocale.Value;
                        }
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@OrigText]");
                        }
                        parameter = new SqlParameter("@OrigText", System.Data.SqlDbType.NVarChar, 400);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!OrigText.IsNull)
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@OrigText] value[" + OrigText.ToString() + "]");
                            }
                            parameter.Value = OrigText.Value;
                        }


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Lookup_getTranslationMemory]");
                        }

                        //Fill the DataSet with the rows that are returned.
                        dataAdapter.Fill(dataSet, "table");


                        if (isTracingEnabled)
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
                    if (isTracingEnabled)
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
