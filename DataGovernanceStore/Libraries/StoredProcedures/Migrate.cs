
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
	public class Migrate
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Migrate()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSessionData(SqlString userLogin, SqlString vchrSessionType, ref SqlString sessionID)
		{
			DateTime startDate = DateTime.Now;
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_getSessionData", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSessionType] value[" + vchrSessionType.ToString() + "]");
						}
						if (!vchrSessionType.IsNull)
						{
							parameter = new SqlParameter("@vchrSessionType",  System.Data.SqlDbType.VarChar, 20);
							parameter.Value = vchrSessionType;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						parameter = new SqlParameter("@sessionID",  System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_getSessionData]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
								
						sessionID = new SqlString((string)command.Parameters["@sessionID"].Value);

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + dataSet.GetXml() + "]");
						}
						
						return dataSet.Tables[0];
						
					}
				}
				finally
				{
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AbortSession(SqlString sessionID)
		{
			DateTime startDate = DateTime.Now;	
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_abortSession", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
					
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@sessionID] value[" + sessionID.ToString() + "]");
						}
						if (!sessionID.IsNull)
						{
							parameter = new SqlParameter("@sessionID",  System.Data.SqlDbType.VarChar, 100);
							parameter.Value = sessionID;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_abortSession]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
						
					}
				}
				finally
				{
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ValidateDraft(SqlString txtXML, SqlString userLogin, SqlBoolean bitLockAll, SqlBoolean bitUseDraftTax, SqlString vchrSessionType, ref SqlString sessionID)
		{
			DateTime startDate = DateTime.Now;
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_ValidateDraft", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
						}
						if (!txtXML.IsNull)
						{
							parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText);
							parameter.Value = txtXML;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitLockAll] value[" + bitLockAll.ToString() + "]");
						}
						if (!bitLockAll.IsNull)
						{
							parameter = new SqlParameter("@bitLockAll", System.Data.SqlDbType.Bit);
							parameter.Value = bitLockAll;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
						}
						if (!bitUseDraftTax.IsNull)
						{
							parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
							parameter.Value = bitUseDraftTax;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSessionType] value[" + vchrSessionType.ToString() + "]");
						}
						if (!vchrSessionType.IsNull)
						{
							parameter = new SqlParameter("@vchrSessionType",  System.Data.SqlDbType.VarChar, 20);
							parameter.Value = vchrSessionType;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						parameter = new SqlParameter("@sessionID",  System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_ValidateDraft]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
								
						sessionID = new SqlString((string)command.Parameters["@sessionID"].Value);

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + dataSet.GetXml() + "]");
						}
						
						return dataSet.Tables[0];
						
					}
				}
				finally
				{
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ValidateSession(SqlString SessionID, SqlString userLogin, SqlBoolean bitLockAll, SqlBoolean bitUseDraftTax)
		{
			DateTime startDate = DateTime.Now;
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_ValidateSession", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SessionID] value[" + SessionID.ToString() + "]");
						}
						if (!SessionID.IsNull)
						{
							parameter = new SqlParameter("@SessionID",  System.Data.SqlDbType.VarChar, 100);
							parameter.Value = SessionID;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitLockAll] value[" + bitLockAll.ToString() + "]");
						}
						if (!bitLockAll.IsNull)
						{
							parameter = new SqlParameter("@bitLockAll", System.Data.SqlDbType.Bit);
							parameter.Value = bitLockAll;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
						}
						if (!bitUseDraftTax.IsNull)
						{
							parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
							parameter.Value = bitUseDraftTax;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_ValidateSession]");
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
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable PromoteDraft(SqlString txtXML, SqlString vchrSessionType, SqlString userLogin, ref SqlString sessionID)
		{
			DateTime startDate = DateTime.Now;
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_PromoteDraft", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
						}
						if (!txtXML.IsNull)
						{
							parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText);
							parameter.Value = txtXML;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSessionType] value[" + vchrSessionType.ToString() + "]");
						}
						if (!vchrSessionType.IsNull)
						{
							parameter = new SqlParameter("@vchrSessionType",  System.Data.SqlDbType.VarChar, 20);
							parameter.Value = vchrSessionType;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						parameter = new SqlParameter("@sessionID",  System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_PromoteDraft]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
								
						sessionID = new SqlString((string)command.Parameters["@sessionID"].Value);

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + dataSet.GetXml() + "]");
						}
						
						return dataSet.Tables[0];
						
					}
				}
				finally
				{
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable PromoteSession(SqlString sessionID, SqlString userLogin)
		{
			DateTime startDate = DateTime.Now;
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_PromoteSession", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@sessionID] value[" + sessionID.ToString() + "]");
						}
						if (!sessionID.IsNull)
						{
							parameter = new SqlParameter("@sessionID",  System.Data.SqlDbType.VarChar, 100);
							parameter.Value = sessionID;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_PromoteSession]");
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
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DiscardChanges(SqlString txtXML, SqlString userLogin, SqlString vchrSessionType, SqlBoolean bitLockAll)
		{
			DateTime startDate = DateTime.Now;	
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_DiscardChanges", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
					
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
						}
						if (!txtXML.IsNull)
						{
							parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText);
							parameter.Value = txtXML;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSessionType] value[" + vchrSessionType.ToString() + "]");
						}
						if (!vchrSessionType.IsNull)
						{
							parameter = new SqlParameter("@vchrSessionType",  System.Data.SqlDbType.VarChar, 20);
							parameter.Value = vchrSessionType;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitLockAll] value[" + bitLockAll.ToString() + "]");
						}
						if (!bitLockAll.IsNull)
						{
							parameter = new SqlParameter("@bitLockAll", System.Data.SqlDbType.Bit);
							parameter.Value = bitLockAll;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_DiscardChanges]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
						
					}
				}
				finally
				{
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UndoCheckOut(SqlString txtXML, SqlString userLogin, SqlString vchrSessionType, SqlBoolean bitLockAll)
		{
			DateTime startDate = DateTime.Now;	
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_UndoCheckOut", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
					
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
						}
						if (!txtXML.IsNull)
						{
							parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText);
							parameter.Value = txtXML;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSessionType] value[" + vchrSessionType.ToString() + "]");
						}
						if (!vchrSessionType.IsNull)
						{
							parameter = new SqlParameter("@vchrSessionType",  System.Data.SqlDbType.VarChar, 20);
							parameter.Value = vchrSessionType;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitLockAll] value[" + bitLockAll.ToString() + "]");
						}
						if (!bitLockAll.IsNull)
						{
							parameter = new SqlParameter("@bitLockAll", System.Data.SqlDbType.Bit);
							parameter.Value = bitLockAll;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_UndoCheckOut]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
						
					}
				}
				finally
				{
					sqlConnection.Close();
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
				throw e;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CheckInItems(SqlString txtXML, SqlString userLogin, SqlString vchrSessionType, SqlBoolean bitLockAll)
		{
			DateTime startDate = DateTime.Now;	
			try
			{
				string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
				SqlConnection sqlConnection = new SqlConnection(connectionString);
				try
				{
					sqlConnection.Open();
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Draft_checkinItems", sqlConnection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
					
						
						SqlParameter parameter = null;

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
						}
						if (!txtXML.IsNull)
						{
							parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText);
							parameter.Value = txtXML;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userLogin] value[" + userLogin.ToString() + "]");
						}
						if (!userLogin.IsNull)
						{
							parameter = new SqlParameter("@userLogin",  System.Data.SqlDbType.NVarChar, 50);
							parameter.Value = userLogin;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSessionType] value[" + vchrSessionType.ToString() + "]");
						}
						if (!vchrSessionType.IsNull)
						{
							parameter = new SqlParameter("@vchrSessionType",  System.Data.SqlDbType.VarChar, 20);
							parameter.Value = vchrSessionType;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitLockAll] value[" + bitLockAll.ToString() + "]");
						}
						if (!bitLockAll.IsNull)
						{
							parameter = new SqlParameter("@bitLockAll", System.Data.SqlDbType.Bit);
							parameter.Value = bitLockAll;
							parameter.Direction = ParameterDirection.Input;
							command.Parameters.Add(parameter);
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Draft_checkinItems]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
						
					}
				}
				finally
				{
					sqlConnection.Close();
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
				throw e;
			}
		}

	}
}		
