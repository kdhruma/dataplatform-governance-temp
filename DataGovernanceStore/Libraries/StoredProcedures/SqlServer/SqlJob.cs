
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
	/// Job API
	/// </summary>
	public class SqlJob
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private SqlJob()
		{
		}
		
		
		/// <summary>
        /// Adds validation results
        /// </summary>
		public static void InsertValidationResult(SqlString jobID, SqlInt32 catalogID, SqlInt32 CNodeParent, SqlInt32 CNode, SqlInt32 customerID, SqlInt32 localeID, SqlString objectType, SqlString validationDescription, SqlInt32 attributeID, SqlString oldValue, SqlString newValue, SqlString message, SqlString creatingUser, SqlString creatingProgram, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_ValidationResultInsert", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@jobID]");
						}
						parameter = new SqlParameter("@jobID", System.Data.SqlDbType.NVarChar, 400);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!jobID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@jobID] value[" + jobID.ToString() + "]");
							}
							parameter.Value = jobID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@catalogID]");
						}
						parameter = new SqlParameter("@catalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!catalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@catalogID] value[" + catalogID.ToString() + "]");
							}
							parameter.Value = catalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeParent]");
						}
						parameter = new SqlParameter("@CNodeParent", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeParent.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeParent] value[" + CNodeParent.ToString() + "]");
							}
							parameter.Value = CNodeParent.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNode]");
						}
						parameter = new SqlParameter("@CNode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNode] value[" + CNode.ToString() + "]");
							}
							parameter.Value = CNode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@customerID]");
						}
						parameter = new SqlParameter("@customerID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!customerID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@customerID] value[" + customerID.ToString() + "]");
							}
							parameter.Value = customerID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@localeID]");
						}
						parameter = new SqlParameter("@localeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!localeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@localeID] value[" + localeID.ToString() + "]");
							}
							parameter.Value = localeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@objectType]");
						}
						parameter = new SqlParameter("@objectType", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!objectType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@objectType] value[" + objectType.ToString() + "]");
							}
							parameter.Value = objectType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@validationDescription]");
						}
						parameter = new SqlParameter("@validationDescription", System.Data.SqlDbType.NVarChar, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!validationDescription.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@validationDescription] value[" + validationDescription.ToString() + "]");
							}
							parameter.Value = validationDescription.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@attributeID]");
						}
						parameter = new SqlParameter("@attributeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!attributeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@attributeID] value[" + attributeID.ToString() + "]");
							}
							parameter.Value = attributeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@oldValue]");
						}
						parameter = new SqlParameter("@oldValue", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!oldValue.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@oldValue] value[" + oldValue.ToString() + "]");
							}
							parameter.Value = oldValue.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@newValue]");
						}
						parameter = new SqlParameter("@newValue", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!newValue.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@newValue] value[" + newValue.ToString() + "]");
							}
							parameter.Value = newValue.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@message]");
						}
						parameter = new SqlParameter("@message", System.Data.SqlDbType.NVarChar, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!message.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@message] value[" + message.ToString() + "]");
							}
							parameter.Value = message.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@creatingUser]");
						}
						parameter = new SqlParameter("@creatingUser", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!creatingUser.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@creatingUser] value[" + creatingUser.ToString() + "]");
							}
							parameter.Value = creatingUser.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@creatingProgram]");
						}
						parameter = new SqlParameter("@creatingProgram", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!creatingProgram.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@creatingProgram] value[" + creatingProgram.ToString() + "]");
							}
							parameter.Value = creatingProgram.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_ValidationResultInsert]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// Gives a result for Matching jobs
        /// </summary>
		public static DataSet GetMatchingJobs(SqlInt32 jobid, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_GetMatchingJobs", (SqlConnection)connection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.Transaction = (SqlTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 1200;

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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@jobid]");
						}
						parameter = new SqlParameter("@jobid", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!jobid.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@jobid] value[" + jobid.ToString() + "]");
							}
							parameter.Value = jobid.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_GetMatchingJobs]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
						
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + dataSet.GetXml() + "]");
						}
						
						return dataSet;
						
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetApprovedMatchingCnode(SqlString MatchingXML, SqlString UpdateFlag, SqlInt32 JobID, ref SqlInt32 intOutput, SqlString vchrUserID, ref SqlString vchrReturnSourceCatalog, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_DC_ApproveMatchingCnode", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@MatchingXML]");
						}
						parameter = new SqlParameter("@MatchingXML", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!MatchingXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@MatchingXML] value[" + MatchingXML.ToString() + "]");
							}
							parameter.Value = MatchingXML.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UpdateFlag]");
						}
						parameter = new SqlParameter("@UpdateFlag", System.Data.SqlDbType.VarChar, 10);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!UpdateFlag.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UpdateFlag] value[" + UpdateFlag.ToString() + "]");
							}
							parameter.Value = UpdateFlag.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOutput]");
						}
						parameter = new SqlParameter("@intOutput", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!intOutput.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOutput] value[" + intOutput.ToString() + "]");
							}
							parameter.Value = intOutput.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserID]");
						}
						parameter = new SqlParameter("@vchrUserID", System.Data.SqlDbType.VarChar, 100);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrReturnSourceCatalog]");
						}
						parameter = new SqlParameter("@vchrReturnSourceCatalog", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!vchrReturnSourceCatalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrReturnSourceCatalog] value[" + vchrReturnSourceCatalog.ToString() + "]");
							}
							parameter.Value = vchrReturnSourceCatalog.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_DC_ApproveMatchingCnode]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
												
						intOutput = new SqlInt32((int)command.Parameters["@intOutput"].Value);
		
						vchrReturnSourceCatalog = new SqlString((string)command.Parameters["@vchrReturnSourceCatalog"].Value);

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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetJobTypeEventSourceMapping(SqlInt32 EventSourceId, SqlInt32 JobId, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_JobTypeEventSourceMapping_Get", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@EventSourceId]");
                        }
                        parameter = new SqlParameter("@EventSourceId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!EventSourceId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@EventSourceId] value[" + EventSourceId.ToString() + "]");
                            }
                            parameter.Value = EventSourceId.Value;
                        }
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


                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_JobTypeEventSourceMapping_Get]");
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
		public static DataSet GetServiceResult(SqlInt32 ServiceID, SqlString JobID_List, SqlString CNodeID_List, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_GetResult", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ServiceID]");
						}
						parameter = new SqlParameter("@ServiceID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ServiceID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ServiceID] value[" + ServiceID.ToString() + "]");
							}
							parameter.Value = ServiceID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobID_List]");
						}
						parameter = new SqlParameter("@JobID_List", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobID_List.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobID_List] value[" + JobID_List.ToString() + "]");
							}
							parameter.Value = JobID_List.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeID_List]");
						}
						parameter = new SqlParameter("@CNodeID_List", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeID_List.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeID_List] value[" + CNodeID_List.ToString() + "]");
							}
							parameter.Value = CNodeID_List.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_Config]");
						}
						parameter = new SqlParameter("@FK_Application_Config", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Application_Config.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_Config] value[" + FK_Application_Config.ToString() + "]");
							}
							parameter.Value = FK_Application_Config.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@GridId]");
						}
						parameter = new SqlParameter("@GridId", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!GridId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@GridId] value[" + GridId.ToString() + "]");
							}
							parameter.Value = GridId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobCNodeStatusID_List]");
						}
						parameter = new SqlParameter("@JobCNodeStatusID_List", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobCNodeStatusID_List.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobCNodeStatusID_List] value[" + JobCNodeStatusID_List.ToString() + "]");
							}
							parameter.Value = JobCNodeStatusID_List.Value;
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
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FilterXML]");
						}
						parameter = new SqlParameter("@FilterXML", System.Data.SqlDbType.Xml);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FilterXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FilterXML] value[" + FilterXML.ToString() + "]");
							}
							parameter.Value = FilterXML.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_GetResult]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
						
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + dataSet.GetXml() + "]");
						}
						
						return dataSet;
						
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessServiceResult(SqlInt32 FK_Event_Source, SqlInt32 FK_Application_Config, SqlXml DataXML, SqlString loginUser, SqlString userProgram, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_ProcessServiceResults", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Source]");
						}
						parameter = new SqlParameter("@FK_Event_Source", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Event_Source.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Source] value[" + FK_Event_Source.ToString() + "]");
							}
							parameter.Value = FK_Event_Source.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_Config]");
						}
						parameter = new SqlParameter("@FK_Application_Config", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Application_Config.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_Config] value[" + FK_Application_Config.ToString() + "]");
							}
							parameter.Value = FK_Application_Config.Value;
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
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@loginUser]");
						}
						parameter = new SqlParameter("@loginUser", System.Data.SqlDbType.NVarChar, 200);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userProgram]");
						}
						parameter = new SqlParameter("@userProgram", System.Data.SqlDbType.NVarChar, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!userProgram.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@userProgram] value[" + userProgram.ToString() + "]");
							}
							parameter.Value = userProgram.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_ProcessServiceResults]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_SchemaValidation(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_GetResult_SchemaValidation", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Source]");
						}
						parameter = new SqlParameter("@FK_Event_Source", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Event_Source.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Source] value[" + FK_Event_Source.ToString() + "]");
							}
							parameter.Value = FK_Event_Source.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIDList]");
						}
						parameter = new SqlParameter("@JobIDList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobIDList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIDList] value[" + JobIDList.ToString() + "]");
							}
							parameter.Value = JobIDList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeList]");
						}
						parameter = new SqlParameter("@CNodeList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeList] value[" + CNodeList.ToString() + "]");
							}
							parameter.Value = CNodeList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_Config]");
						}
						parameter = new SqlParameter("@FK_Application_Config", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Application_Config.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_Config] value[" + FK_Application_Config.ToString() + "]");
							}
							parameter.Value = FK_Application_Config.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@GridId]");
						}
						parameter = new SqlParameter("@GridId", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!GridId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@GridId] value[" + GridId.ToString() + "]");
							}
							parameter.Value = GridId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobCNodeStatusID_List]");
						}
						parameter = new SqlParameter("@JobCNodeStatusID_List", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobCNodeStatusID_List.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobCNodeStatusID_List] value[" + JobCNodeStatusID_List.ToString() + "]");
							}
							parameter.Value = JobCNodeStatusID_List.Value;
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
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FilterXML]");
						}
						parameter = new SqlParameter("@FilterXML", System.Data.SqlDbType.Xml);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FilterXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FilterXML] value[" + FilterXML.ToString() + "]");
							}
							parameter.Value = FilterXML.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_GetResult_SchemaValidation]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_DescMatching(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_GetResult_DescMatching", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Source]");
						}
						parameter = new SqlParameter("@FK_Event_Source", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Event_Source.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Source] value[" + FK_Event_Source.ToString() + "]");
							}
							parameter.Value = FK_Event_Source.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIDList]");
						}
						parameter = new SqlParameter("@JobIDList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobIDList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIDList] value[" + JobIDList.ToString() + "]");
							}
							parameter.Value = JobIDList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeList]");
						}
						parameter = new SqlParameter("@CNodeList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeList] value[" + CNodeList.ToString() + "]");
							}
							parameter.Value = CNodeList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_Config]");
						}
						parameter = new SqlParameter("@FK_Application_Config", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Application_Config.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_Config] value[" + FK_Application_Config.ToString() + "]");
							}
							parameter.Value = FK_Application_Config.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@GridId]");
						}
						parameter = new SqlParameter("@GridId", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!GridId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@GridId] value[" + GridId.ToString() + "]");
							}
							parameter.Value = GridId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobCNodeStatusID_List]");
						}
						parameter = new SqlParameter("@JobCNodeStatusID_List", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobCNodeStatusID_List.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobCNodeStatusID_List] value[" + JobCNodeStatusID_List.ToString() + "]");
							}
							parameter.Value = JobCNodeStatusID_List.Value;
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
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FilterXML]");
						}
						parameter = new SqlParameter("@FilterXML", System.Data.SqlDbType.Xml);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FilterXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FilterXML] value[" + FilterXML.ToString() + "]");
							}
							parameter.Value = FilterXML.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_GetResult_DescMatching]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_AttributeExtraction(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_GetResult_AttributeExtraction", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Source]");
						}
						parameter = new SqlParameter("@FK_Event_Source", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Event_Source.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Source] value[" + FK_Event_Source.ToString() + "]");
							}
							parameter.Value = FK_Event_Source.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIDList]");
						}
						parameter = new SqlParameter("@JobIDList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobIDList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIDList] value[" + JobIDList.ToString() + "]");
							}
							parameter.Value = JobIDList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeList]");
						}
						parameter = new SqlParameter("@CNodeList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeList] value[" + CNodeList.ToString() + "]");
							}
							parameter.Value = CNodeList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_Config]");
						}
						parameter = new SqlParameter("@FK_Application_Config", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Application_Config.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_Config] value[" + FK_Application_Config.ToString() + "]");
							}
							parameter.Value = FK_Application_Config.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@GridId]");
						}
						parameter = new SqlParameter("@GridId", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!GridId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@GridId] value[" + GridId.ToString() + "]");
							}
							parameter.Value = GridId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobCNodeStatusID_List]");
						}
						parameter = new SqlParameter("@JobCNodeStatusID_List", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobCNodeStatusID_List.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobCNodeStatusID_List] value[" + JobCNodeStatusID_List.ToString() + "]");
							}
							parameter.Value = JobCNodeStatusID_List.Value;
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
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FilterXML]");
						}
						parameter = new SqlParameter("@FilterXML", System.Data.SqlDbType.Xml);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FilterXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FilterXML] value[" + FilterXML.ToString() + "]");
							}
							parameter.Value = FilterXML.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_GetResult_AttributeExtraction]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_Normalization(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Job_GetResult_Normalization", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Event_Source]");
						}
						parameter = new SqlParameter("@FK_Event_Source", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Event_Source.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Event_Source] value[" + FK_Event_Source.ToString() + "]");
							}
							parameter.Value = FK_Event_Source.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobIDList]");
						}
						parameter = new SqlParameter("@JobIDList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobIDList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobIDList] value[" + JobIDList.ToString() + "]");
							}
							parameter.Value = JobIDList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeList]");
						}
						parameter = new SqlParameter("@CNodeList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeList] value[" + CNodeList.ToString() + "]");
							}
							parameter.Value = CNodeList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_Config]");
						}
						parameter = new SqlParameter("@FK_Application_Config", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Application_Config.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_Config] value[" + FK_Application_Config.ToString() + "]");
							}
							parameter.Value = FK_Application_Config.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@GridId]");
						}
						parameter = new SqlParameter("@GridId", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!GridId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@GridId] value[" + GridId.ToString() + "]");
							}
							parameter.Value = GridId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@JobCNodeStatusID_List]");
						}
						parameter = new SqlParameter("@JobCNodeStatusID_List", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!JobCNodeStatusID_List.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@JobCNodeStatusID_List] value[" + JobCNodeStatusID_List.ToString() + "]");
							}
							parameter.Value = JobCNodeStatusID_List.Value;
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
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FilterXML]");
						}
						parameter = new SqlParameter("@FilterXML", System.Data.SqlDbType.Xml);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FilterXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FilterXML] value[" + FilterXML.ToString() + "]");
							}
							parameter.Value = FilterXML.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Job_GetResult_Normalization]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

	}
}		
