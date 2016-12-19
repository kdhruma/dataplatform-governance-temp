
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
	public class SqlSecurity
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private SqlSecurity()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUsersByRole(SqlString nvchrRole, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getUsersByRole_DT", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@nvchrRole]");
						}
						parameter = new SqlParameter("@nvchrRole", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!nvchrRole.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@nvchrRole] value[" + nvchrRole.ToString() + "]");
							}
							parameter.Value = nvchrRole.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getUsersByRole_DT]");
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
        public static void HasPermission( SqlString vchrUserLogin, SqlString ObjectTypeSN, SqlString ActionSN, SqlString ParentObjectTypeSN, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt64 FK_CategoryCNode, SqlInt64 FK_ComponentCNode, SqlInt32 FK_Attribute, SqlInt32 FK_Relationship, SqlBoolean bitForDraft, ref SqlBoolean bitHasPermission, IDbConnection connection, IDbTransaction transaction )
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_HasPermission", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ObjectTypeSN]");
						}
						parameter = new SqlParameter("@ObjectTypeSN", System.Data.SqlDbType.VarChar, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ObjectTypeSN.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ObjectTypeSN] value[" + ObjectTypeSN.ToString() + "]");
							}
							parameter.Value = ObjectTypeSN.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ActionSN]");
						}
						parameter = new SqlParameter("@ActionSN", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ActionSN.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ActionSN] value[" + ActionSN.ToString() + "]");
							}
							parameter.Value = ActionSN.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ParentObjectTypeSN]");
						}
						parameter = new SqlParameter("@ParentObjectTypeSN", System.Data.SqlDbType.VarChar, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ParentObjectTypeSN.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ParentObjectTypeSN] value[" + ParentObjectTypeSN.ToString() + "]");
							}
							parameter.Value = ParentObjectTypeSN.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Org]");
						}
						parameter = new SqlParameter("@FK_Org", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Org.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Org] value[" + FK_Org.ToString() + "]");
							}
							parameter.Value = FK_Org.Value;
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
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_CategoryCNode]");
						}
						parameter = new SqlParameter("@FK_CategoryCNode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_CategoryCNode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_CategoryCNode] value[" + FK_CategoryCNode.ToString() + "]");
							}
							parameter.Value = FK_CategoryCNode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_ComponentCNode]");
						}
						parameter = new SqlParameter("@FK_ComponentCNode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_ComponentCNode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_ComponentCNode] value[" + FK_ComponentCNode.ToString() + "]");
							}
							parameter.Value = FK_ComponentCNode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Attribute]");
						}
						parameter = new SqlParameter("@FK_Attribute", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Attribute.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Attribute] value[" + FK_Attribute.ToString() + "]");
							}
							parameter.Value = FK_Attribute.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Relationship]");
						}
						parameter = new SqlParameter("@FK_Relationship", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Relationship.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Relationship] value[" + FK_Relationship.ToString() + "]");
							}
							parameter.Value = FK_Relationship.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitForDraft]");
						}
						parameter = new SqlParameter("@bitForDraft", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitForDraft.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitForDraft] value[" + bitForDraft.ToString() + "]");
							}
							parameter.Value = bitForDraft.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitHasPermission]");
						}
						parameter = new SqlParameter("@bitHasPermission", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!bitHasPermission.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitHasPermission] value[" + bitHasPermission.ToString() + "]");
							}
							parameter.Value = bitHasPermission.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_HasPermission]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
								
						bitHasPermission = new SqlBoolean((bool)command.Parameters["@bitHasPermission"].Value);

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
		public static void IsSystemUser(SqlString nvchrUserLogin, ref SqlString isSystemUser, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_isSystemUser", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@nvchrUserLogin]");
						}
						parameter = new SqlParameter("@nvchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!nvchrUserLogin.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@nvchrUserLogin] value[" + nvchrUserLogin.ToString() + "]");
							}
							parameter.Value = nvchrUserLogin.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@isSystemUser]");
						}
						parameter = new SqlParameter("@isSystemUser", System.Data.SqlDbType.Char, 1);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!isSystemUser.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@isSystemUser] value[" + isSystemUser.ToString() + "]");
							}
							parameter.Value = isSystemUser.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_isSystemUser]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
								
						isSystemUser = new SqlString((string)command.Parameters["@isSystemUser"].Value);

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
		public static string GetUsers(SqlInt32 intPK_Security_User, SqlInt32 intUserType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getUsers_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intPK_Security_User]");
						}
						parameter = new SqlParameter("@intPK_Security_User", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intPK_Security_User.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intPK_Security_User] value[" + intPK_Security_User.ToString() + "]");
							}
							parameter.Value = intPK_Security_User.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intUserType]");
						}
						parameter = new SqlParameter("@intUserType", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intUserType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intUserType] value[" + intUserType.ToString() + "]");
							}
							parameter.Value = intUserType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountFrom]");
						}
						parameter = new SqlParameter("@intCountFrom", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountFrom.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountFrom] value[" + intCountFrom.ToString() + "]");
							}
							parameter.Value = intCountFrom.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountTo]");
						}
						parameter = new SqlParameter("@intCountTo", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountTo.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountTo] value[" + intCountTo.ToString() + "]");
							}
							parameter.Value = intCountTo.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSortColumn]");
						}
						parameter = new SqlParameter("@vchrSortColumn", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSortColumn.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSortColumn] value[" + vchrSortColumn.ToString() + "]");
							}
							parameter.Value = vchrSortColumn.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchColumn]");
						}
						parameter = new SqlParameter("@vchrSearchColumn", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchColumn.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchColumn] value[" + vchrSearchColumn.ToString() + "]");
							}
							parameter.Value = vchrSearchColumn.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchParameter]");
						}
						parameter = new SqlParameter("@vchrSearchParameter", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchParameter.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchParameter] value[" + vchrSearchParameter.ToString() + "]");
							}
							parameter.Value = vchrSearchParameter.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getUsers_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUsersWithRoles(SqlInt32 intPK_Security_User, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getUsersWithRoles_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intPK_Security_User]");
						}
						parameter = new SqlParameter("@intPK_Security_User", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intPK_Security_User.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intPK_Security_User] value[" + intPK_Security_User.ToString() + "]");
							}
							parameter.Value = intPK_Security_User.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountFrom]");
						}
						parameter = new SqlParameter("@intCountFrom", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountFrom.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountFrom] value[" + intCountFrom.ToString() + "]");
							}
							parameter.Value = intCountFrom.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountTo]");
						}
						parameter = new SqlParameter("@intCountTo", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountTo.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountTo] value[" + intCountTo.ToString() + "]");
							}
							parameter.Value = intCountTo.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchColumn]");
						}
						parameter = new SqlParameter("@vchrSearchColumn", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchColumn.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchColumn] value[" + vchrSearchColumn.ToString() + "]");
							}
							parameter.Value = vchrSearchColumn.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchParameter]");
						}
						parameter = new SqlParameter("@vchrSearchParameter", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchParameter.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchParameter] value[" + vchrSearchParameter.ToString() + "]");
							}
							parameter.Value = vchrSearchParameter.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getUsersWithRoles_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoles(SqlInt32 intPK_Security_Role, SqlInt32 intUserType, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getRoles_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intPK_Security_Role]");
						}
						parameter = new SqlParameter("@intPK_Security_Role", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intPK_Security_Role.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intPK_Security_Role] value[" + intPK_Security_Role.ToString() + "]");
							}
							parameter.Value = intPK_Security_Role.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intUserType]");
						}
						parameter = new SqlParameter("@intUserType", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intUserType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intUserType] value[" + intUserType.ToString() + "]");
							}
							parameter.Value = intUserType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@chrGetPermissionSetOnly]");
						}
						parameter = new SqlParameter("@chrGetPermissionSetOnly", System.Data.SqlDbType.Char, 1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!chrGetPermissionSetOnly.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@chrGetPermissionSetOnly] value[" + chrGetPermissionSetOnly.ToString() + "]");
							}
							parameter.Value = chrGetPermissionSetOnly.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountFrom]");
						}
						parameter = new SqlParameter("@intCountFrom", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountFrom.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountFrom] value[" + intCountFrom.ToString() + "]");
							}
							parameter.Value = intCountFrom.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountTo]");
						}
						parameter = new SqlParameter("@intCountTo", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountTo.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountTo] value[" + intCountTo.ToString() + "]");
							}
							parameter.Value = intCountTo.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSortColumn]");
						}
						parameter = new SqlParameter("@vchrSortColumn", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSortColumn.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSortColumn] value[" + vchrSortColumn.ToString() + "]");
							}
							parameter.Value = vchrSortColumn.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchColumn]");
						}
						parameter = new SqlParameter("@vchrSearchColumn", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchColumn.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchColumn] value[" + vchrSearchColumn.ToString() + "]");
							}
							parameter.Value = vchrSearchColumn.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchParameter]");
						}
						parameter = new SqlParameter("@vchrSearchParameter", System.Data.SqlDbType.NVarChar, 150);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchParameter.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchParameter] value[" + vchrSearchParameter.ToString() + "]");
							}
							parameter.Value = vchrSearchParameter.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitDisplaySystemRole]");
						}
						parameter = new SqlParameter("@bitDisplaySystemRole", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitDisplaySystemRole.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitDisplaySystemRole] value[" + bitDisplaySystemRole.ToString() + "]");
							}
							parameter.Value = bitDisplaySystemRole.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getRoles_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRolesWithUsers(SqlInt32 intPK_Security_Role, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getRolesWithUsers_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intPK_Security_Role]");
						}
						parameter = new SqlParameter("@intPK_Security_Role", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intPK_Security_Role.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intPK_Security_Role] value[" + intPK_Security_Role.ToString() + "]");
							}
							parameter.Value = intPK_Security_Role.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@chrGetPermissionSetOnly]");
						}
						parameter = new SqlParameter("@chrGetPermissionSetOnly", System.Data.SqlDbType.Char, 1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!chrGetPermissionSetOnly.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@chrGetPermissionSetOnly] value[" + chrGetPermissionSetOnly.ToString() + "]");
							}
							parameter.Value = chrGetPermissionSetOnly.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountFrom]");
						}
						parameter = new SqlParameter("@intCountFrom", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountFrom.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountFrom] value[" + intCountFrom.ToString() + "]");
							}
							parameter.Value = intCountFrom.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCountTo]");
						}
						parameter = new SqlParameter("@intCountTo", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCountTo.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCountTo] value[" + intCountTo.ToString() + "]");
							}
							parameter.Value = intCountTo.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchColumn]");
						}
						parameter = new SqlParameter("@vchrSearchColumn", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchColumn.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchColumn] value[" + vchrSearchColumn.ToString() + "]");
							}
							parameter.Value = vchrSearchColumn.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchParameter]");
						}
						parameter = new SqlParameter("@vchrSearchParameter", System.Data.SqlDbType.NVarChar, 150);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchParameter.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchParameter] value[" + vchrSearchParameter.ToString() + "]");
							}
							parameter.Value = vchrSearchParameter.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitDisplaySystemRole]");
						}
						parameter = new SqlParameter("@bitDisplaySystemRole", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitDisplaySystemRole.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitDisplaySystemRole] value[" + bitDisplaySystemRole.ToString() + "]");
							}
							parameter.Value = bitDisplaySystemRole.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getRolesWithUsers_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUsers(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_processUsers", ( SqlConnection )connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!txtXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
							}
                            parameter.Value = txtXML.Value;
						}
                        
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_processUsers]");
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
		public static DataTable ProcessRoles(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_processRoles", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!txtXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
							}
							parameter.Value = txtXML.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_processRoles]");
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
		public static string GetObjectTypeAction(IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getObjectTypeAction_XML", (SqlConnection)connection))
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getObjectTypeAction_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetHierarchyAction(IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getHierarchyAction_XML", (SqlConnection)connection))
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getHierarchyAction_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetPermissions(SqlInt32 intPK_Security_Role, SqlString chrPermissionSet, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getPermissions_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intPK_Security_Role]");
						}
						parameter = new SqlParameter("@intPK_Security_Role", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intPK_Security_Role.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intPK_Security_Role] value[" + intPK_Security_Role.ToString() + "]");
							}
							parameter.Value = intPK_Security_Role.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@chrPermissionSet]");
						}
						parameter = new SqlParameter("@chrPermissionSet", System.Data.SqlDbType.Char, 1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!chrPermissionSet.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@chrPermissionSet] value[" + chrPermissionSet.ToString() + "]");
							}
							parameter.Value = chrPermissionSet.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getPermissions_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessPermissions(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_processPermissions", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!txtXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
							}
							parameter.Value = txtXML.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_processPermissions]");
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
		public static string GetAllOrganizationsAndCatalogs(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getAllOrgCatalogs_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getAllOrgCatalogs_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AuthenticateUser(SqlString vchrUserLogin, SqlString vchrPassword, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_authenticateUser", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrPassword]");
						}
						parameter = new SqlParameter("@vchrPassword", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrPassword.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrPassword] value[" + vchrPassword.ToString() + "]");
							}
							parameter.Value = vchrPassword.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_authenticateUser]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetMenus(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getVisibleMenusByUser_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getVisibleMenusByUser_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserPreferences(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getUserPreferences_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrTargetUserLogin]");
						}
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrTargetUserLogin.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrTargetUserLogin] value[" + vchrTargetUserLogin.ToString() + "]");
							}
							parameter.Value = vchrTargetUserLogin.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getUserPreferences_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoleMenus(SqlInt32 PK_Security_Role, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_getRoleMenus_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Security_Role]");
						}
						parameter = new SqlParameter("@PK_Security_Role", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Security_Role.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Security_Role] value[" + PK_Security_Role.ToString() + "]");
							}
							parameter.Value = PK_Security_Role.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_getRoleMenus_XML]");
						}
						
						using(SqlDataReader dataReader = dataAdapter.SelectCommand.ExecuteReader())
						{
						    while(dataReader.Read())
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUserPreferences(SqlString txtXML, SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_processUserPreferences", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!txtXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
							}
							parameter.Value = txtXML.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrTargetUserLogin]");
						}
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrTargetUserLogin.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrTargetUserLogin] value[" + vchrTargetUserLogin.ToString() + "]");
							}
							parameter.Value = vchrTargetUserLogin.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_processUserPreferences]");
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
		public static void ProcessRoleMenus(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_processRoleMenus", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!txtXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@txtXML] value[" + txtXML.ToString() + "]");
							}
							parameter.Value = txtXML.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.NVarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_processRoleMenus]");
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
		public static void GetDraftID(SqlInt32 productionID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_GetDraftID", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@productionID]");
						}
						parameter = new SqlParameter("@productionID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!productionID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@productionID] value[" + productionID.ToString() + "]");
							}
							parameter.Value = productionID.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_GetDraftID]");
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
			catch(Exception e)
			{
				MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
				throw;
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAppSetting(SqlString optionName, ref SqlString optionValue, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_AppSetting_Get", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@optionName]");
						}
						parameter = new SqlParameter("@optionName", System.Data.SqlDbType.NVarChar, 150);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!optionName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@optionName] value[" + optionName.ToString() + "]");
							}
							parameter.Value = optionName.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@optionValue]");
						}
						parameter = new SqlParameter("@optionValue", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!optionValue.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@optionValue] value[" + optionValue.ToString() + "]");
							}
							parameter.Value = optionValue.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_AppSetting_Get]");
						}
						
						dataAdapter.SelectCommand.ExecuteNonQuery();
						
								
						optionValue = new SqlString((string)command.Parameters["@optionValue"].Value);

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
		public static DataSet GetAppconfigNameVals(IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Admin_AppConfigNameVal", (SqlConnection)connection))
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

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Admin_AppConfigNameVal]");
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

	}
}		
