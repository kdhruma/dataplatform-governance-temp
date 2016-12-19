
using System;
using System.Xml;
using System.Data;
using System.Text;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data.SqlTypes;
using System.Configuration;

namespace Riversand.JobService
{
	/// <summary>
	/// 
	/// </summary>
	public class OracleJobDB
	{
	
		/// <summary>
		/// The logger to use in this class
		/// </summary>
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly log4net.ILog spLog = log4net.LogManager.GetLogger("Database.StoredProcedures");

		/// <summary>
		/// Private constructor
		/// </summary>		
		private OracleJobDB()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobsDT(SqlString P_JOBTYPE, SqlString P_USERNAME, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.sp_GetByType", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBTYPE]");
						}
						parameter = new OracleParameter("P_JOBTYPE", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBTYPE.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBTYPE] value[" + P_JOBTYPE.ToString() + "]");
							}
							parameter.Value = P_JOBTYPE.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_USERNAME]");
						}
						parameter = new OracleParameter("P_USERNAME", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_USERNAME.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_USERNAME] value[" + P_USERNAME.ToString() + "]");
							}
							parameter.Value = P_USERNAME.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[O_JOBS]");
						}
						parameter = new OracleParameter("O_JOBS", OracleDbType.RefCursor);
						parameter.Direction = ParameterDirection.Output;
						command.Parameters.Add(parameter);

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.sp_GetByType]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
						
						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Result:[" + dataSet.GetXml() + "]");
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
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}					
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetJobsXml(SqlBoolean P_INCLUDEINACTIVE, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.sp_GetXml", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_INCLUDEINACTIVE]");
						}
						parameter = new OracleParameter("P_INCLUDEINACTIVE", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_INCLUDEINACTIVE.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_INCLUDEINACTIVE] value[" + P_INCLUDEINACTIVE.ToString() + "]");
							}
							parameter.Value = P_INCLUDEINACTIVE.Value ? 1 : 0;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[O_JOBS]");
						}
						parameter = new OracleParameter("O_JOBS", OracleDbType.RefCursor);
						parameter.Direction = ParameterDirection.Output;
						command.Parameters.Add(parameter);

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.sp_GetXml]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
						
						
						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Result:[" + dataSet.GetXml() + "]");
						}
						
						DataTable table = dataSet.Tables[0];
						return Riversand.Framework.Xml.XmlUtilities.GetXmlExplicit(table);
					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}					
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetJobItemXml(SqlInt32 P_JOBID, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.sp_GetItemXml", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBID]");
						}
						parameter = new OracleParameter("P_JOBID", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBID.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBID] value[" + P_JOBID.ToString() + "]");
							}
							parameter.Value = P_JOBID.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[O_JOBITEM]");
						}
						parameter = new OracleParameter("O_JOBITEM", OracleDbType.RefCursor);
						parameter.Direction = ParameterDirection.Output;
						command.Parameters.Add(parameter);

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.sp_GetItemXml]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
						
						
						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Result:[" + dataSet.GetXml() + "]");
						}
						
						DataTable table = dataSet.Tables[0];
						return Riversand.Framework.Xml.XmlUtilities.GetXmlExplicit(table);
					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}					
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateJobInformation(SqlInt32 P_JOBID, SqlString P_DESCRIPTION, SqlString P_JOBDATA, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;	
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.fn_UpdateInformation", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[RETURN_VALUE]");
						}
						parameter = new OracleParameter("RETURN_VALUE", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.ReturnValue;
						command.Parameters.Add(parameter);
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBID]");
						}
						parameter = new OracleParameter("P_JOBID", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBID.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBID] value[" + P_JOBID.ToString() + "]");
							}
							parameter.Value = P_JOBID.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_DESCRIPTION]");
						}
						parameter = new OracleParameter("P_DESCRIPTION", OracleDbType.Varchar2, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_DESCRIPTION.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_DESCRIPTION] value[" + P_DESCRIPTION.ToString() + "]");
							}
							parameter.Value = P_DESCRIPTION.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBDATA]");
						}
						parameter = new OracleParameter("P_JOBDATA", OracleDbType.Clob, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBDATA.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBDATA] value[" + P_JOBDATA.ToString() + "]");
							}
							parameter.Value = P_JOBDATA.Value;
						}

						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.fn_UpdateInformation]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
											
						RETURN_VALUE = new SqlInt32(Convert.ToInt32(((OracleDecimal)command.Parameters["RETURN_VALUE"].Value).Value));

					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateJobStatus(SqlInt32 P_JOBID, SqlString P_STATUS, SqlString P_USERACTION, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;	
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.fn_UpdateStatus", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[RETURN_VALUE]");
						}
						parameter = new OracleParameter("RETURN_VALUE", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.ReturnValue;
						command.Parameters.Add(parameter);
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBID]");
						}
						parameter = new OracleParameter("P_JOBID", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBID.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBID] value[" + P_JOBID.ToString() + "]");
							}
							parameter.Value = P_JOBID.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_STATUS]");
						}
						parameter = new OracleParameter("P_STATUS", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_STATUS.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_STATUS] value[" + P_STATUS.ToString() + "]");
							}
							parameter.Value = P_STATUS.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_USERACTION]");
						}
						parameter = new OracleParameter("P_USERACTION", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_USERACTION.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_USERACTION] value[" + P_USERACTION.ToString() + "]");
							}
							parameter.Value = P_USERACTION.Value;
						}

						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.fn_UpdateStatus]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
											
						RETURN_VALUE = new SqlInt32(Convert.ToInt32(((OracleDecimal)command.Parameters["RETURN_VALUE"].Value).Value));

					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateJobUserAction(SqlInt32 P_JOBID, SqlString P_USERACTION, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;	
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.fn_UpdateUserAction", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[RETURN_VALUE]");
						}
						parameter = new OracleParameter("RETURN_VALUE", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.ReturnValue;
						command.Parameters.Add(parameter);
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBID]");
						}
						parameter = new OracleParameter("P_JOBID", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBID.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBID] value[" + P_JOBID.ToString() + "]");
							}
							parameter.Value = P_JOBID.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_USERACTION]");
						}
						parameter = new OracleParameter("P_USERACTION", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_USERACTION.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_USERACTION] value[" + P_USERACTION.ToString() + "]");
							}
							parameter.Value = P_USERACTION.Value;
						}

						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.fn_UpdateUserAction]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
											
						RETURN_VALUE = new SqlInt32(Convert.ToInt32(((OracleDecimal)command.Parameters["RETURN_VALUE"].Value).Value));

					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ResetJobUserAction(SqlInt32 P_JOBID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;	
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.fn_ResetUserAction", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[RETURN_VALUE]");
						}
						parameter = new OracleParameter("RETURN_VALUE", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.ReturnValue;
						command.Parameters.Add(parameter);
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBID]");
						}
						parameter = new OracleParameter("P_JOBID", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBID.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBID] value[" + P_JOBID.ToString() + "]");
							}
							parameter.Value = P_JOBID.Value;
						}

						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.fn_ResetUserAction]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
											
						RETURN_VALUE = new SqlInt32(Convert.ToInt32(((OracleDecimal)command.Parameters["RETURN_VALUE"].Value).Value));

					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddJob(SqlString P_JOBTYPE, SqlString P_JOBSUBTYPE, SqlString P_PROFILENAME, SqlString P_SHORTNAME, SqlString P_DESCRIPTION, SqlString P_JOBDATA, SqlString P_STATUS, SqlString P_COMPUTERNAME, SqlString P_USERNAME, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;	
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.fn_Add", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[RETURN_VALUE]");
						}
						parameter = new OracleParameter("RETURN_VALUE", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.ReturnValue;
						command.Parameters.Add(parameter);
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBTYPE]");
						}
						parameter = new OracleParameter("P_JOBTYPE", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBTYPE.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBTYPE] value[" + P_JOBTYPE.ToString() + "]");
							}
							parameter.Value = P_JOBTYPE.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBSUBTYPE]");
						}
						parameter = new OracleParameter("P_JOBSUBTYPE", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBSUBTYPE.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBSUBTYPE] value[" + P_JOBSUBTYPE.ToString() + "]");
							}
							parameter.Value = P_JOBSUBTYPE.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_PROFILENAME]");
						}
						parameter = new OracleParameter("P_PROFILENAME", OracleDbType.Varchar2, 150);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_PROFILENAME.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_PROFILENAME] value[" + P_PROFILENAME.ToString() + "]");
							}
							parameter.Value = P_PROFILENAME.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_SHORTNAME]");
						}
						parameter = new OracleParameter("P_SHORTNAME", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_SHORTNAME.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_SHORTNAME] value[" + P_SHORTNAME.ToString() + "]");
							}
							parameter.Value = P_SHORTNAME.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_DESCRIPTION]");
						}
						parameter = new OracleParameter("P_DESCRIPTION", OracleDbType.Varchar2, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_DESCRIPTION.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_DESCRIPTION] value[" + P_DESCRIPTION.ToString() + "]");
							}
							parameter.Value = P_DESCRIPTION.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBDATA]");
						}
						parameter = new OracleParameter("P_JOBDATA", OracleDbType.Clob, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBDATA.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBDATA] value[" + P_JOBDATA.ToString() + "]");
							}
							parameter.Value = P_JOBDATA.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_STATUS]");
						}
						parameter = new OracleParameter("P_STATUS", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_STATUS.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_STATUS] value[" + P_STATUS.ToString() + "]");
							}
							parameter.Value = P_STATUS.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_COMPUTERNAME]");
						}
						parameter = new OracleParameter("P_COMPUTERNAME", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_COMPUTERNAME.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_COMPUTERNAME] value[" + P_COMPUTERNAME.ToString() + "]");
							}
							parameter.Value = P_COMPUTERNAME.Value;
						}
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_USERNAME]");
						}
						parameter = new OracleParameter("P_USERNAME", OracleDbType.Varchar2, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_USERNAME.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_USERNAME] value[" + P_USERNAME.ToString() + "]");
							}
							parameter.Value = P_USERNAME.Value;
						}

						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.fn_Add]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
											
						RETURN_VALUE = new SqlInt32(Convert.ToInt32(((OracleDecimal)command.Parameters["RETURN_VALUE"].Value).Value));

					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteJob(SqlInt32 P_JOBID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			DateTime startDate = DateTime.Now;	
			bool internalConnect = false;
			try
			{
				if (connection == null)
				{
					string connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
					connection = new OracleConnection(connectionString);
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
					using(OracleDataAdapter dataAdapter = new OracleDataAdapter("pkg_Job.fn_Delete", (OracleConnection)connection))
					{
						OracleCommand command = dataAdapter.SelectCommand;
						/* http://www.c-sharpcorner.com/Code/2004/Feb/ODP.NET04.asp
						The Oracle Database starts a transaction only in the 
						context of a connection. Once a transaction starts, all the successive command 
						execution on that connection run in the context of that transaction. 
						Transactions can only be started on a OracleConnection object and the read-only 
						Transaction property on the OracleCommand object is implicitly set by the 
						OracleConnection object. Therefore, the application cannot set the Transaction 
						property, nor does it need to.
						*/
						//command.Transaction = (OracleTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 120;
						
						IDataParameter parameter = null;
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[RETURN_VALUE]");
						}
						parameter = new OracleParameter("RETURN_VALUE", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.ReturnValue;
						command.Parameters.Add(parameter);
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Adding Parameter: name[P_JOBID]");
						}
						parameter = new OracleParameter("P_JOBID", OracleDbType.Decimal);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!P_JOBID.IsNull)
						{
							if (spLog.IsDebugEnabled)
							{
								spLog.Debug("Setting Parameter: name[P_JOBID] value[" + P_JOBID.ToString() + "]");
							}
							parameter.Value = P_JOBID.Value;
						}

						
						if (spLog.IsDebugEnabled)
						{
							spLog.Debug("Calling:[pkg_Job.fn_Delete]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
											
						RETURN_VALUE = new SqlInt32(Convert.ToInt32(((OracleDecimal)command.Parameters["RETURN_VALUE"].Value).Value));

					}
				}
				finally
				{
					if (internalConnect)
					{
						connection.Close();
					}
					if (spLog.IsDebugEnabled)
					{
						DateTime endDate = DateTime.Now;
						TimeSpan span = endDate.Subtract(startDate);
						spLog.Debug("Time Spent:" + span.TotalMilliseconds.ToString());
					}
				}
			}
			catch(Exception e)
			{
				log.Error(e);
				throw;
			}
		}

	}
}		
