
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
	public class SqlErrorHandling
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private SqlErrorHandling()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static void SetErrorLog(SqlString FileName, SqlString MessageID, SqlString Status, SqlString ObjectType, SqlInt32 ItemNumber, SqlString ExternalID, SqlInt32 InternalID, SqlString Description, SqlInt32 ErrorCode, SqlString ItemType, SqlString ItemName, SqlString ItemParentName, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_ErrorHandling_ErrorLog_Set", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FileName]");
						}
						parameter = new SqlParameter("@FileName", System.Data.SqlDbType.NVarChar, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FileName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FileName] value[" + FileName.ToString() + "]");
							}
							parameter.Value = FileName.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@MessageID]");
						}
						parameter = new SqlParameter("@MessageID", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!MessageID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@MessageID] value[" + MessageID.ToString() + "]");
							}
							parameter.Value = MessageID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Status]");
						}
						parameter = new SqlParameter("@Status", System.Data.SqlDbType.VarChar, 10);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!Status.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Status] value[" + Status.ToString() + "]");
							}
							parameter.Value = Status.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ObjectType]");
						}
						parameter = new SqlParameter("@ObjectType", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ObjectType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ObjectType] value[" + ObjectType.ToString() + "]");
							}
							parameter.Value = ObjectType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ItemNumber]");
						}
						parameter = new SqlParameter("@ItemNumber", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ItemNumber.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ItemNumber] value[" + ItemNumber.ToString() + "]");
							}
							parameter.Value = ItemNumber.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ExternalID]");
						}
						parameter = new SqlParameter("@ExternalID", System.Data.SqlDbType.NVarChar, 900);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ExternalID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ExternalID] value[" + ExternalID.ToString() + "]");
							}
							parameter.Value = ExternalID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@InternalID]");
						}
						parameter = new SqlParameter("@InternalID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!InternalID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@InternalID] value[" + InternalID.ToString() + "]");
							}
							parameter.Value = InternalID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Description]");
						}
						parameter = new SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 4000);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!Description.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Description] value[" + Description.ToString() + "]");
							}
							parameter.Value = Description.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ErrorCode]");
						}
						parameter = new SqlParameter("@ErrorCode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ErrorCode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ErrorCode] value[" + ErrorCode.ToString() + "]");
							}
							parameter.Value = ErrorCode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ItemType]");
						}
						parameter = new SqlParameter("@ItemType", System.Data.SqlDbType.VarChar, 20);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ItemType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ItemType] value[" + ItemType.ToString() + "]");
							}
							parameter.Value = ItemType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ItemName]");
						}
						parameter = new SqlParameter("@ItemName", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ItemName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ItemName] value[" + ItemName.ToString() + "]");
							}
							parameter.Value = ItemName.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ItemParentName]");
						}
						parameter = new SqlParameter("@ItemParentName", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ItemParentName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ItemParentName] value[" + ItemParentName.ToString() + "]");
							}
							parameter.Value = ItemParentName.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_ErrorHandling_ErrorLog_Set]");
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

	}
}		
