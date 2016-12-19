
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
	public class SqlCatalog
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private SqlCatalog()
		{
		}
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable DQMSearch(SqlInt32 FK_Catalog, SqlInt32 PK_Application_Config, SqlString vchrUserLogin, SqlInt32 userRole, SqlString dqmSearchXML, SqlString keyWordSearch, ref SqlString totalCount, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Search_DQM", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Application_Config]");
						}
						parameter = new SqlParameter("@PK_Application_Config", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Application_Config.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Application_Config] value[" + PK_Application_Config.ToString() + "]");
							}
							parameter.Value = PK_Application_Config.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserLogin]");
						}
						parameter = new SqlParameter("@vchrUserLogin", System.Data.SqlDbType.VarChar, 100);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@userRole]");
						}
						parameter = new SqlParameter("@userRole", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!userRole.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@userRole] value[" + userRole.ToString() + "]");
							}
							parameter.Value = userRole.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@dqmSearchXML]");
						}
						parameter = new SqlParameter("@dqmSearchXML", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!dqmSearchXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@dqmSearchXML] value[" + dqmSearchXML.ToString() + "]");
							}
							parameter.Value = dqmSearchXML.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@keyWordSearch]");
						}
						parameter = new SqlParameter("@keyWordSearch", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!keyWordSearch.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@keyWordSearch] value[" + keyWordSearch.ToString() + "]");
							}
							parameter.Value = keyWordSearch.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@totalCount]");
						}
						parameter = new SqlParameter("@totalCount", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!totalCount.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@totalCount] value[" + totalCount.ToString() + "]");
							}
							parameter.Value = totalCount.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Search_DQM]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
								
						totalCount = new SqlString((string)command.Parameters["@totalCount"].Value);

						
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
		public static DataTable GuidedSearch(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlString vchrCurrentAssignmentStatus, SqlInt32 FK_Status, SqlString FK_Attributes, ref SqlString totalCount, SqlInt32 DisplayRows, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Search_Guided", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrLoadCategory]");
						}
						parameter = new SqlParameter("@vchrLoadCategory", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrLoadCategory.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrLoadCategory] value[" + vchrLoadCategory.ToString() + "]");
							}
							parameter.Value = vchrLoadCategory.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrLoadType]");
						}
						parameter = new SqlParameter("@vchrLoadType", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrLoadType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrLoadType] value[" + vchrLoadType.ToString() + "]");
							}
							parameter.Value = vchrLoadType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrCurrentAssignmentStatus]");
						}
						parameter = new SqlParameter("@vchrCurrentAssignmentStatus", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrCurrentAssignmentStatus.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrCurrentAssignmentStatus] value[" + vchrCurrentAssignmentStatus.ToString() + "]");
							}
							parameter.Value = vchrCurrentAssignmentStatus.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Status]");
						}
						parameter = new SqlParameter("@FK_Status", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Status.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Status] value[" + FK_Status.ToString() + "]");
							}
							parameter.Value = FK_Status.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Attributes]");
						}
						parameter = new SqlParameter("@FK_Attributes", System.Data.SqlDbType.VarChar, 1000);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Attributes.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Attributes] value[" + FK_Attributes.ToString() + "]");
							}
							parameter.Value = FK_Attributes.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@totalCount]");
						}
						parameter = new SqlParameter("@totalCount", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!totalCount.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@totalCount] value[" + totalCount.ToString() + "]");
							}
							parameter.Value = totalCount.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@DisplayRows]");
						}
						parameter = new SqlParameter("@DisplayRows", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!DisplayRows.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@DisplayRows] value[" + DisplayRows.ToString() + "]");
							}
							parameter.Value = DisplayRows.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Search_Guided]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
								
						totalCount = new SqlString((string)command.Parameters["@totalCount"].Value);

						
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
		public static string GetLoadsGuidedSearchXML(SqlInt32 CatalogID, SqlString vchrLoadType, SqlInt32 PK_Load, SqlString vchrLoadsList, SqlString vchrSearchString, SqlString vchrUserLogin, SqlString vchrUserRole, SqlInt32 intCountFrom, SqlInt32 intCountTo, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Search_Guided_GetLoadsXML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CatalogID]");
						}
						parameter = new SqlParameter("@CatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CatalogID] value[" + CatalogID.ToString() + "]");
							}
							parameter.Value = CatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrLoadType]");
						}
						parameter = new SqlParameter("@vchrLoadType", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrLoadType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrLoadType] value[" + vchrLoadType.ToString() + "]");
							}
							parameter.Value = vchrLoadType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Load]");
						}
						parameter = new SqlParameter("@PK_Load", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Load.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Load] value[" + PK_Load.ToString() + "]");
							}
							parameter.Value = PK_Load.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrLoadsList]");
						}
						parameter = new SqlParameter("@vchrLoadsList", System.Data.SqlDbType.VarChar, 500);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrLoadsList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrLoadsList] value[" + vchrLoadsList.ToString() + "]");
							}
							parameter.Value = vchrLoadsList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchString]");
						}
						parameter = new SqlParameter("@vchrSearchString", System.Data.SqlDbType.VarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchString.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchString] value[" + vchrSearchString.ToString() + "]");
							}
							parameter.Value = vchrSearchString.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserRole]");
						}
						parameter = new SqlParameter("@vchrUserRole", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrUserRole.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrUserRole] value[" + vchrUserRole.ToString() + "]");
							}
							parameter.Value = vchrUserRole.Value;
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Search_Guided_GetLoadsXML]");
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
		public static DataTable GetRelationshipTypeHierarchy(SqlInt32 FK_Catalog, SqlInt32 FK_RelationshipType_Top, SqlInt32 MaxLevel, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Catalog_RelationshipTypeHierarchy_Get", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_RelationshipType_Top]");
						}
						parameter = new SqlParameter("@FK_RelationshipType_Top", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_RelationshipType_Top.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_RelationshipType_Top] value[" + FK_RelationshipType_Top.ToString() + "]");
							}
							parameter.Value = FK_RelationshipType_Top.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@MaxLevel]");
						}
						parameter = new SqlParameter("@MaxLevel", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!MaxLevel.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@MaxLevel] value[" + MaxLevel.ToString() + "]");
							}
							parameter.Value = MaxLevel.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Catalog_RelationshipTypeHierarchy_Get]");
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
		public static string ExtractCatalogByIDLocalRel(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrRelAttrList, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getCatalogByID_LocalRel_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intExtSystemID]");
						}
						parameter = new SqlParameter("@intExtSystemID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intExtSystemID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intExtSystemID] value[" + intExtSystemID.ToString() + "]");
							}
							parameter.Value = intExtSystemID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.Text, 2147483647);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrRelAttrList]");
						}
						parameter = new SqlParameter("@vchrRelAttrList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrRelAttrList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrRelAttrList] value[" + vchrRelAttrList.ToString() + "]");
							}
							parameter.Value = vchrRelAttrList.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getCatalogByID_LocalRel_XML]");
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
        public static string ExtractBulkAttributeMetadata(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, SqlInt32 localeId, IDbConnection connection, IDbTransaction transaction, SqlBoolean ignoreComplexAttributes)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getBulkAttributeMetaData", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.Text, 2147483647);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax]");
						}
						parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDraftTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
							}
							parameter.Value = bitUseDraftTax.Value;
						}

						StringBuilder buffer = new StringBuilder();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LocaleId]");
                        }
                        parameter = new SqlParameter("@LocaleId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!localeId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LocaleId] value[" + localeId.ToString() + "]");
                            }
                            parameter.Value = localeId.Value;
                        }

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IgnoreComplexAttributes]");
                        }
                        parameter = new SqlParameter("@IgnoreComplexAttributes", System.Data.SqlDbType.Bit);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ignoreComplexAttributes.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IgnoreComplexAttributes] value[" + ignoreComplexAttributes.ToString() + "]");
                            }
                            parameter.Value = ignoreComplexAttributes.Value;
                        }

						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getBulkAttributeMetaData]");
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
		public static string ExtractBulkAttributeMetadataRel(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax,SqlInt32 localeId, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getBulkAttributeMetaDataRel", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.Text, 2147483647);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax]");
						}
						parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDraftTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
							}
							parameter.Value = bitUseDraftTax.Value;
						}
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LocaleId]");
                        }
                        parameter = new SqlParameter("@LocaleId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!localeId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LocaleId] value[" + localeId.ToString() + "]");
                            }
                            parameter.Value = localeId.Value;
                        }

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getBulkAttributeMetaDataRel]");
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
		public static string ExtractAttributes(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_ExtractAttributes", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.Text, 2147483647);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax]");
						}
						parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDraftTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
							}
							parameter.Value = bitUseDraftTax.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_ExtractAttributes]");
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
		public static DataTable GetCategoryAttributeMap(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCategoryAttributeMap", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCategoryID]");
						}
						parameter = new SqlParameter("@intCategoryID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCategoryID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCategoryID] value[" + intCategoryID.ToString() + "]");
							}
							parameter.Value = intCategoryID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intLocaleID]");
						}
						parameter = new SqlParameter("@intLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intLocaleID] value[" + intLocaleID.ToString() + "]");
							}
							parameter.Value = intLocaleID.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCategoryAttributeMap]");
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
		public static DataTable GetUserVisibleCatalogsDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getVisibleCatalogs_DT", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
						}
						parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Catalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
							}
							parameter.Value = PK_Catalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeTaxonomy] value[" + IncludeTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDynamicTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeDynamicTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDynamicTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDynamicTaxonomy] value[" + IncludeDynamicTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeDynamicTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeCatalog]");
						}
						parameter = new SqlParameter("@IncludeCatalog", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeCatalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeCatalog] value[" + IncludeCatalog.ToString() + "]");
							}
							parameter.Value = IncludeCatalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeView]");
						}
						parameter = new SqlParameter("@IncludeView", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeView.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeView] value[" + IncludeView.ToString() + "]");
							}
							parameter.Value = IncludeView.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeProduction]");
						}
						parameter = new SqlParameter("@IncludeProduction", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeProduction.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeProduction] value[" + IncludeProduction.ToString() + "]");
							}
							parameter.Value = IncludeProduction.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDraft]");
						}
						parameter = new SqlParameter("@IncludeDraft", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDraft.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDraft] value[" + IncludeDraft.ToString() + "]");
							}
							parameter.Value = IncludeDraft.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getVisibleCatalogs_DT]");
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
		public static string GetCharacteristicTemplate(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID, SqlBoolean bitUseDraftTax, SqlInt32 bitUsesChilds, SqlInt32 intOrgID, SqlBoolean ExcludeSearchable, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCharacteristicTemplate", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCategoryID]");
						}
						parameter = new SqlParameter("@intCategoryID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCategoryID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCategoryID] value[" + intCategoryID.ToString() + "]");
							}
							parameter.Value = intCategoryID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intLocaleID]");
						}
						parameter = new SqlParameter("@intLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intLocaleID] value[" + intLocaleID.ToString() + "]");
							}
							parameter.Value = intLocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax]");
						}
						parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDraftTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
							}
							parameter.Value = bitUseDraftTax.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUsesChilds]");
						}
						parameter = new SqlParameter("@bitUsesChilds", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUsesChilds.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUsesChilds] value[" + bitUsesChilds.ToString() + "]");
							}
							parameter.Value = bitUsesChilds.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgID]");
						}
						parameter = new SqlParameter("@intOrgID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgID] value[" + intOrgID.ToString() + "]");
							}
							parameter.Value = intOrgID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ExcludeSearchable]");
						}
						parameter = new SqlParameter("@ExcludeSearchable", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ExcludeSearchable.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ExcludeSearchable] value[" + ExcludeSearchable.ToString() + "]");
							}
							parameter.Value = ExcludeSearchable.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCharacteristicTemplate]");
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
		public static DataTable GetCharacteristicTemplateDT(SqlInt32 intCategoryID, SqlInt32 intLocaleID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCharacteristicTemplateDT", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCategoryID]");
						}
						parameter = new SqlParameter("@intCategoryID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCategoryID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCategoryID] value[" + intCategoryID.ToString() + "]");
							}
							parameter.Value = intCategoryID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intLocaleID]");
						}
						parameter = new SqlParameter("@intLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intLocaleID] value[" + intLocaleID.ToString() + "]");
							}
							parameter.Value = intLocaleID.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCharacteristicTemplateDT]");
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
		public static string GetUserVisibleCatalogs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getVisibleCatalogs_XML", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
						}
						parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Catalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
							}
							parameter.Value = PK_Catalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeTaxonomy] value[" + IncludeTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDynamicTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeDynamicTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDynamicTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDynamicTaxonomy] value[" + IncludeDynamicTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeDynamicTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeCatalog]");
						}
						parameter = new SqlParameter("@IncludeCatalog", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeCatalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeCatalog] value[" + IncludeCatalog.ToString() + "]");
							}
							parameter.Value = IncludeCatalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeView]");
						}
						parameter = new SqlParameter("@IncludeView", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeView.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeView] value[" + IncludeView.ToString() + "]");
							}
							parameter.Value = IncludeView.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeProduction]");
						}
						parameter = new SqlParameter("@IncludeProduction", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeProduction.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeProduction] value[" + IncludeProduction.ToString() + "]");
							}
							parameter.Value = IncludeProduction.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDraft]");
						}
						parameter = new SqlParameter("@IncludeDraft", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDraft.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDraft] value[" + IncludeDraft.ToString() + "]");
							}
							parameter.Value = IncludeDraft.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getVisibleCatalogs_XML]");
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
		public static string GetCatalogPermissionsByOrg(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCatalogPermissionsByOrg_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
						}
						parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Catalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
							}
							parameter.Value = PK_Catalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeTaxonomy] value[" + IncludeTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDynamicTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeDynamicTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDynamicTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDynamicTaxonomy] value[" + IncludeDynamicTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeDynamicTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeCatalog]");
						}
						parameter = new SqlParameter("@IncludeCatalog", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeCatalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeCatalog] value[" + IncludeCatalog.ToString() + "]");
							}
							parameter.Value = IncludeCatalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeView]");
						}
						parameter = new SqlParameter("@IncludeView", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeView.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeView] value[" + IncludeView.ToString() + "]");
							}
							parameter.Value = IncludeView.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeProduction]");
						}
						parameter = new SqlParameter("@IncludeProduction", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeProduction.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeProduction] value[" + IncludeProduction.ToString() + "]");
							}
							parameter.Value = IncludeProduction.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDraft]");
						}
						parameter = new SqlParameter("@IncludeDraft", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDraft.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDraft] value[" + IncludeDraft.ToString() + "]");
							}
							parameter.Value = IncludeDraft.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCatalogPermissionsByOrg_XML]");
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
		public static DataTable GetCatalogDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCatalog_DT", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
						}
						parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Catalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
							}
							parameter.Value = PK_Catalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeTaxonomy] value[" + IncludeTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDynamicTaxonomy]");
						}
						parameter = new SqlParameter("@IncludeDynamicTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDynamicTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDynamicTaxonomy] value[" + IncludeDynamicTaxonomy.ToString() + "]");
							}
							parameter.Value = IncludeDynamicTaxonomy.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeCatalog]");
						}
						parameter = new SqlParameter("@IncludeCatalog", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeCatalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeCatalog] value[" + IncludeCatalog.ToString() + "]");
							}
							parameter.Value = IncludeCatalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeView]");
						}
						parameter = new SqlParameter("@IncludeView", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeView.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeView] value[" + IncludeView.ToString() + "]");
							}
							parameter.Value = IncludeView.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeProduction]");
						}
						parameter = new SqlParameter("@IncludeProduction", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeProduction.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeProduction] value[" + IncludeProduction.ToString() + "]");
							}
							parameter.Value = IncludeProduction.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeDraft]");
						}
						parameter = new SqlParameter("@IncludeDraft", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeDraft.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeDraft] value[" + IncludeDraft.ToString() + "]");
							}
							parameter.Value = IncludeDraft.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCatalog_DT]");
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
		public static string GetCatalogsByOrg(SqlString orgId, SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getCatalogs_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@orgId]");
						}
						parameter = new SqlParameter("@orgId", System.Data.SqlDbType.VarChar, 500);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!orgId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@orgId] value[" + orgId.ToString() + "]");
							}
							parameter.Value = orgId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrTargetUserLogin]");
						}
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getCatalogs_XML]");
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
		public static DataTable ProcessCatalogs(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_processCatalogs", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Org]");
						}
						parameter = new SqlParameter("@PK_Org", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Org.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Org] value[" + PK_Org.ToString() + "]");
							}
							parameter.Value = PK_Org.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_processCatalogs]");
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
		public static string GetCatalogLocaleByID(SqlInt32 PK_Catalog, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCatalogLocaleByID_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
						}
						parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Catalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
							}
							parameter.Value = PK_Catalog.Value;
						}
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCatalogLocaleByID_XML]");
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
		public static void ProcessCatalogLocales(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_processCatalogLocales", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Org]");
						}
						parameter = new SqlParameter("@PK_Org", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Org.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Org] value[" + PK_Org.ToString() + "]");
							}
							parameter.Value = PK_Org.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_processCatalogLocales]");
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
		public static string GetNodePermissions(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 FK_ParentCNode, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_CNode, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory, SqlString ToolTipAttributeList, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCNodePermissions_XML", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
						}
						parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Catalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
							}
							parameter.Value = PK_Catalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_ParentCNode]");
						}
						parameter = new SqlParameter("@FK_ParentCNode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_ParentCNode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_ParentCNode] value[" + FK_ParentCNode.ToString() + "]");
							}
							parameter.Value = FK_ParentCNode.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Customer]");
						}
						parameter = new SqlParameter("@FK_Customer", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Customer.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Customer] value[" + FK_Customer.ToString() + "]");
							}
							parameter.Value = FK_Customer.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_CNode]");
						}
						parameter = new SqlParameter("@PK_CNode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_CNode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_CNode] value[" + PK_CNode.ToString() + "]");
							}
							parameter.Value = PK_CNode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitIncludeComponents]");
						}
						parameter = new SqlParameter("@bitIncludeComponents", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitIncludeComponents.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitIncludeComponents] value[" + bitIncludeComponents.ToString() + "]");
							}
							parameter.Value = bitIncludeComponents.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitEnableComponentMapping]");
						}
						parameter = new SqlParameter("@bitEnableComponentMapping", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitEnableComponentMapping.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitEnableComponentMapping] value[" + bitEnableComponentMapping.ToString() + "]");
							}
							parameter.Value = bitEnableComponentMapping.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDrafTax]");
						}
						parameter = new SqlParameter("@bitUseDrafTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDrafTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDrafTax] value[" + bitUseDrafTax.ToString() + "]");
							}
							parameter.Value = bitUseDrafTax.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitEnableUnassignedCategory]");
						}
						parameter = new SqlParameter("@bitEnableUnassignedCategory", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitEnableUnassignedCategory.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitEnableUnassignedCategory] value[" + bitEnableUnassignedCategory.ToString() + "]");
							}
							parameter.Value = bitEnableUnassignedCategory.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ToolTipAttributeList]");
						}
						parameter = new SqlParameter("@ToolTipAttributeList", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ToolTipAttributeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ToolTipAttributeList] value[" + ToolTipAttributeList.ToString() + "]");
							}
							parameter.Value = ToolTipAttributeList.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCNodePermissions_XML]");
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
		public static string GetNodePermissionsByCNode(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 CnodeId, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCNodePermissionsByCNode_XML", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
						}
						parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Catalog.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
							}
							parameter.Value = PK_Catalog.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CnodeId]");
						}
						parameter = new SqlParameter("@CnodeId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CnodeId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CnodeId] value[" + CnodeId.ToString() + "]");
							}
							parameter.Value = CnodeId.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Customer]");
						}
						parameter = new SqlParameter("@FK_Customer", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Customer.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Customer] value[" + FK_Customer.ToString() + "]");
							}
							parameter.Value = FK_Customer.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitIncludeComponents]");
						}
						parameter = new SqlParameter("@bitIncludeComponents", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitIncludeComponents.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitIncludeComponents] value[" + bitIncludeComponents.ToString() + "]");
							}
							parameter.Value = bitIncludeComponents.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitEnableComponentMapping]");
						}
						parameter = new SqlParameter("@bitEnableComponentMapping", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitEnableComponentMapping.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitEnableComponentMapping] value[" + bitEnableComponentMapping.ToString() + "]");
							}
							parameter.Value = bitEnableComponentMapping.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDrafTax]");
						}
						parameter = new SqlParameter("@bitUseDrafTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDrafTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDrafTax] value[" + bitUseDrafTax.ToString() + "]");
							}
							parameter.Value = bitUseDrafTax.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitEnableUnassignedCategory]");
						}
						parameter = new SqlParameter("@bitEnableUnassignedCategory", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitEnableUnassignedCategory.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitEnableUnassignedCategory] value[" + bitEnableUnassignedCategory.ToString() + "]");
							}
							parameter.Value = bitEnableUnassignedCategory.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCNodePermissionsByCNode_XML]");
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
        public static string GetCoreAttrByGroup( SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlInt64 intCNodeID, SqlInt64 intCNodeParentID, SqlInt32 intGroupID, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlInt32 intBackLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlBoolean ShowAtCreation, SqlString AttrIDList, IDbConnection connection, IDbTransaction transaction )
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCoreAttrByGroup_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intLocaleID]");
						}
						parameter = new SqlParameter("@intLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intLocaleID] value[" + intLocaleID.ToString() + "]");
							}
							parameter.Value = intLocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCustomerID]");
						}
						parameter = new SqlParameter("@intCustomerID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCustomerID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCustomerID] value[" + intCustomerID.ToString() + "]");
							}
							parameter.Value = intCustomerID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCNodeID]");
						}
						parameter = new SqlParameter("@intCNodeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCNodeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCNodeID] value[" + intCNodeID.ToString() + "]");
							}
							parameter.Value = intCNodeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCNodeParentID]");
						}
						parameter = new SqlParameter("@intCNodeParentID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCNodeParentID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCNodeParentID] value[" + intCNodeParentID.ToString() + "]");
							}
							parameter.Value = intCNodeParentID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intGroupID]");
						}
						parameter = new SqlParameter("@intGroupID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intGroupID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intGroupID] value[" + intGroupID.ToString() + "]");
							}
							parameter.Value = intGroupID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgID]");
						}
						parameter = new SqlParameter("@intOrgID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgID] value[" + intOrgID.ToString() + "]");
							}
							parameter.Value = intOrgID.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intBackLocaleID]");
						}
						parameter = new SqlParameter("@intBackLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intBackLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intBackLocaleID] value[" + intBackLocaleID.ToString() + "]");
							}
							parameter.Value = intBackLocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrViewPath]");
						}
						parameter = new SqlParameter("@vchrViewPath", System.Data.SqlDbType.VarChar, 500);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrViewPath.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrViewPath] value[" + vchrViewPath.ToString() + "]");
							}
							parameter.Value = vchrViewPath.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax]");
						}
						parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDraftTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
							}
							parameter.Value = bitUseDraftTax.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ShowAtCreation]");
						}
						parameter = new SqlParameter("@ShowAtCreation", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ShowAtCreation.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ShowAtCreation] value[" + ShowAtCreation.ToString() + "]");
							}
							parameter.Value = ShowAtCreation.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@AttrIDList]");
						}
						parameter = new SqlParameter("@AttrIDList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!AttrIDList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@AttrIDList] value[" + AttrIDList.ToString() + "]");
							}
							parameter.Value = AttrIDList.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCoreAttrByGroup_XML]");
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
        public static void ProcessCoreAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_ProcessCoreAttr", (SqlConnection)connection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.Transaction = (SqlTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 2000000000;

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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgID]");
						}
						parameter = new SqlParameter("@intOrgID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgID] value[" + intOrgID.ToString() + "]");
							}
							parameter.Value = intOrgID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserID]");
						}
						parameter = new SqlParameter("@vchrUserID", System.Data.SqlDbType.NVarChar, 600);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrProgramName]");
						}
						parameter = new SqlParameter("@vchrProgramName", System.Data.SqlDbType.NVarChar, 600);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrProgramName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrProgramName] value[" + vchrProgramName.ToString() + "]");
							}
							parameter.Value = vchrProgramName.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LocaleId]");
						}
						parameter = new SqlParameter("@LocaleId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!LocaleId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LocaleId] value[" + LocaleId.ToString() + "]");
							}
							parameter.Value = LocaleId.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_ProcessCoreAttr]");
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
		public static void ProcessRelAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_ProcessRelAttr", (SqlConnection)connection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.Transaction = (SqlTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 2000000000;

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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgID]");
						}
						parameter = new SqlParameter("@intOrgID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgID] value[" + intOrgID.ToString() + "]");
							}
							parameter.Value = intOrgID.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrProgramName]");
						}
						parameter = new SqlParameter("@vchrProgramName", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrProgramName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrProgramName] value[" + vchrProgramName.ToString() + "]");
							}
							parameter.Value = vchrProgramName.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_ProcessRelAttr]");
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
        public static void ProcessTechAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_ProcessTechAttr", (SqlConnection)connection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.Transaction = (SqlTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 2000000000;

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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgID]");
						}
						parameter = new SqlParameter("@intOrgID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgID] value[" + intOrgID.ToString() + "]");
							}
							parameter.Value = intOrgID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserID]");
						}
						parameter = new SqlParameter("@vchrUserID", System.Data.SqlDbType.NVarChar, 600);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrProgramName]");
						}
						parameter = new SqlParameter("@vchrProgramName", System.Data.SqlDbType.NVarChar, 600);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrProgramName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrProgramName] value[" + vchrProgramName.ToString() + "]");
							}
							parameter.Value = vchrProgramName.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LocaleId]");
						}
						parameter = new SqlParameter("@LocaleId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!LocaleId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LocaleId] value[" + LocaleId.ToString() + "]");
							}
							parameter.Value = LocaleId.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_ProcessTechAttr]");
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
		public static string GetTechAttr(SqlInt32 intCnodeID, SqlInt32 intCnodeParentID, SqlInt32 intCatalogID, SqlInt32 intGroupID, SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlString vchrUserID, SqlInt32 intBackupLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlString AttrIDList, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getTechAttr_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCnodeID]");
						}
						parameter = new SqlParameter("@intCnodeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCnodeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCnodeID] value[" + intCnodeID.ToString() + "]");
							}
							parameter.Value = intCnodeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCnodeParentID]");
						}
						parameter = new SqlParameter("@intCnodeParentID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCnodeParentID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCnodeParentID] value[" + intCnodeParentID.ToString() + "]");
							}
							parameter.Value = intCnodeParentID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intGroupID]");
						}
						parameter = new SqlParameter("@intGroupID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intGroupID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intGroupID] value[" + intGroupID.ToString() + "]");
							}
							parameter.Value = intGroupID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intLocaleID]");
						}
						parameter = new SqlParameter("@intLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intLocaleID] value[" + intLocaleID.ToString() + "]");
							}
							parameter.Value = intLocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCustomerID]");
						}
						parameter = new SqlParameter("@intCustomerID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCustomerID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCustomerID] value[" + intCustomerID.ToString() + "]");
							}
							parameter.Value = intCustomerID.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intBackupLocaleID]");
						}
						parameter = new SqlParameter("@intBackupLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intBackupLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intBackupLocaleID] value[" + intBackupLocaleID.ToString() + "]");
							}
							parameter.Value = intBackupLocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrViewPath]");
						}
						parameter = new SqlParameter("@vchrViewPath", System.Data.SqlDbType.VarChar, 500);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrViewPath.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrViewPath] value[" + vchrViewPath.ToString() + "]");
							}
							parameter.Value = vchrViewPath.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTax]");
						}
						parameter = new SqlParameter("@bitUseDraftTax", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDraftTax.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDraftTax] value[" + bitUseDraftTax.ToString() + "]");
							}
							parameter.Value = bitUseDraftTax.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@AttrIDList]");
						}
						parameter = new SqlParameter("@AttrIDList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!AttrIDList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@AttrIDList] value[" + AttrIDList.ToString() + "]");
							}
							parameter.Value = AttrIDList.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getTechAttr_XML]");
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
		public static DataTable GetStatuses(IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getStatuses", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getStatuses]");
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
		public static DataTable GetCatalogByName(SqlString ShortName, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCatalogByName", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ShortName]");
						}
						parameter = new SqlParameter("@ShortName", System.Data.SqlDbType.NVarChar, 150);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ShortName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ShortName] value[" + ShortName.ToString() + "]");
							}
							parameter.Value = ShortName.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCatalogByName]");
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
		public static string GetAllUOMsByUOMType(IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getAllUOMsByUOMType_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getAllUOMsByUOMType_XML]");
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
		public static void SchemaValidationRulesExecution(SqlInt32 JobId, SqlString UserID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_ValidateSchema_RuleExecution", (SqlConnection)connection))
					{
						SqlCommand command = dataAdapter.SelectCommand;
						command.Transaction = (SqlTransaction)transaction;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandTimeout = 600;

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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserID]");
						}
						parameter = new SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 100);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_ValidateSchema_RuleExecution]");
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
		public static string GetCnodeAttachments(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intCNodeParentID, SqlInt32 intCNodeID, SqlString vchrViewPath, SqlInt32 intLocaleID, SqlInt32 intBackLocaleID, SqlInt32 intCustomerID, SqlString nvchrUserID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCnodeAttachments_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgID]");
						}
						parameter = new SqlParameter("@intOrgID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgID] value[" + intOrgID.ToString() + "]");
							}
							parameter.Value = intOrgID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCNodeParentID]");
						}
						parameter = new SqlParameter("@intCNodeParentID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCNodeParentID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCNodeParentID] value[" + intCNodeParentID.ToString() + "]");
							}
							parameter.Value = intCNodeParentID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCNodeID]");
						}
						parameter = new SqlParameter("@intCNodeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCNodeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCNodeID] value[" + intCNodeID.ToString() + "]");
							}
							parameter.Value = intCNodeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrViewPath]");
						}
						parameter = new SqlParameter("@vchrViewPath", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrViewPath.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrViewPath] value[" + vchrViewPath.ToString() + "]");
							}
							parameter.Value = vchrViewPath.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intLocaleID]");
						}
						parameter = new SqlParameter("@intLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intLocaleID] value[" + intLocaleID.ToString() + "]");
							}
							parameter.Value = intLocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intBackLocaleID]");
						}
						parameter = new SqlParameter("@intBackLocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intBackLocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intBackLocaleID] value[" + intBackLocaleID.ToString() + "]");
							}
							parameter.Value = intBackLocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCustomerID]");
						}
						parameter = new SqlParameter("@intCustomerID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCustomerID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCustomerID] value[" + intCustomerID.ToString() + "]");
							}
							parameter.Value = intCustomerID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@nvchrUserID]");
						}
						parameter = new SqlParameter("@nvchrUserID", System.Data.SqlDbType.NVarChar, 150);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!nvchrUserID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@nvchrUserID] value[" + nvchrUserID.ToString() + "]");
							}
							parameter.Value = nvchrUserID.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCnodeAttachments_XML]");
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
		public static string GetColumnPreference(SqlString user, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_getColumnPref_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@user]");
						}
						parameter = new SqlParameter("@user", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!user.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@user] value[" + user.ToString() + "]");
							}
							parameter.Value = user.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_getColumnPref_XML]");
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
		public static void UpdataColumnPreference(SqlString colXml, SqlString user, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_updateColumnPref", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@colXml]");
						}
						parameter = new SqlParameter("@colXml", System.Data.SqlDbType.NText, 1073741823);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!colXml.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@colXml] value[" + colXml.ToString() + "]");
							}
							parameter.Value = colXml.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@user]");
						}
						parameter = new SqlParameter("@user", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!user.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@user] value[" + user.ToString() + "]");
							}
							parameter.Value = user.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_updateColumnPref]");
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
		public static string GetVisibleComponents(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 intOrgId, SqlInt32 intCatalogId, SqlInt32 intNodeId, SqlBoolean bitRecursive, SqlBoolean bitUseDraftTaxonomy, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getVisibleComponents_XML", (SqlConnection)connection))
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
						parameter = new SqlParameter("@vchrTargetUserLogin", System.Data.SqlDbType.VarChar, 50);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgId]");
						}
						parameter = new SqlParameter("@intOrgId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgId] value[" + intOrgId.ToString() + "]");
							}
							parameter.Value = intOrgId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogId]");
						}
						parameter = new SqlParameter("@intCatalogId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogId] value[" + intCatalogId.ToString() + "]");
							}
							parameter.Value = intCatalogId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intNodeId]");
						}
						parameter = new SqlParameter("@intNodeId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intNodeId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intNodeId] value[" + intNodeId.ToString() + "]");
							}
							parameter.Value = intNodeId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitRecursive]");
						}
						parameter = new SqlParameter("@bitRecursive", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitRecursive.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitRecursive] value[" + bitRecursive.ToString() + "]");
							}
							parameter.Value = bitRecursive.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@bitUseDraftTaxonomy]");
						}
						parameter = new SqlParameter("@bitUseDraftTaxonomy", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!bitUseDraftTaxonomy.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@bitUseDraftTaxonomy] value[" + bitUseDraftTaxonomy.ToString() + "]");
							}
							parameter.Value = bitUseDraftTaxonomy.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getVisibleComponents_XML]");
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
        public static string GetCNode( SqlInt32 FK_Catalog, SqlInt64 PK_CNode, SqlString ViewPath, IDbConnection connection, IDbTransaction transaction )
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getCNode_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_CNode]");
						}
						parameter = new SqlParameter("@PK_CNode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_CNode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_CNode] value[" + PK_CNode.ToString() + "]");
							}
							parameter.Value = PK_CNode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ViewPath]");
						}
						parameter = new SqlParameter("@ViewPath", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ViewPath.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ViewPath] value[" + ViewPath.ToString() + "]");
							}
							parameter.Value = ViewPath.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getCNode_XML]");
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
		public static string GetTaxonomyId(SqlInt32 intCatalogId, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getTaxonomyId", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogId]");
						}
						parameter = new SqlParameter("@intCatalogId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogId] value[" + intCatalogId.ToString() + "]");
							}
							parameter.Value = intCatalogId.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getTaxonomyId]");
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
		public static void UpdateCategoryAttribute(SqlInt32 CategoryID, SqlInt32 CatalogID, SqlString Action, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_UpdateCategoryAttribute", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CategoryID]");
						}
						parameter = new SqlParameter("@CategoryID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CategoryID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CategoryID] value[" + CategoryID.ToString() + "]");
							}
							parameter.Value = CategoryID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CatalogID]");
						}
						parameter = new SqlParameter("@CatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CatalogID] value[" + CatalogID.ToString() + "]");
							}
							parameter.Value = CatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Action]");
						}
						parameter = new SqlParameter("@Action", System.Data.SqlDbType.VarChar, 10);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!Action.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Action] value[" + Action.ToString() + "]");
							}
							parameter.Value = Action.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_UpdateCategoryAttribute]");
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
		public static DataTable GetAttrLookup(SqlInt32 AttributeID, SqlInt32 Cnode, SqlInt32 LocaleID, ref SqlBoolean IsCategory, ref SqlString LKTableName, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_AttrLookup_Get", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@AttributeID]");
						}
						parameter = new SqlParameter("@AttributeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!AttributeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@AttributeID] value[" + AttributeID.ToString() + "]");
							}
							parameter.Value = AttributeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Cnode]");
						}
						parameter = new SqlParameter("@Cnode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!Cnode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Cnode] value[" + Cnode.ToString() + "]");
							}
							parameter.Value = Cnode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LocaleID]");
						}
						parameter = new SqlParameter("@LocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!LocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LocaleID] value[" + LocaleID.ToString() + "]");
							}
							parameter.Value = LocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IsCategory]");
						}
						parameter = new SqlParameter("@IsCategory", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!IsCategory.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IsCategory] value[" + IsCategory.ToString() + "]");
							}
							parameter.Value = IsCategory.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LKTableName]");
						}
						parameter = new SqlParameter("@LKTableName", System.Data.SqlDbType.VarChar, 300);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!LKTableName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LKTableName] value[" + LKTableName.ToString() + "]");
							}
							parameter.Value = LKTableName.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_AttrLookup_Get]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
								
						IsCategory = new SqlBoolean((bool)command.Parameters["@IsCategory"].Value);
		
						LKTableName = new SqlString((string)command.Parameters["@LKTableName"].Value);

						
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
		public static DataTable WhereUsed(SqlInt64 CNodeID, SqlString RelationshipType, SqlString AttributeList, SqlInt32 TotalRows, SqlString CatalogFilter, SqlInt32 CustomerID, SqlInt32 LocaleID, ref SqlInt32 TotalCount, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_RelationshipManager_Relationship_WhereUsed_Get_OLD", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeID]");
						}
						parameter = new SqlParameter("@CNodeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeID] value[" + CNodeID.ToString() + "]");
							}
							parameter.Value = CNodeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RelationshipType]");
						}
						parameter = new SqlParameter("@RelationshipType", System.Data.SqlDbType.NVarChar, 150);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!RelationshipType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@RelationshipType] value[" + RelationshipType.ToString() + "]");
							}
							parameter.Value = RelationshipType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@AttributeList]");
						}
						parameter = new SqlParameter("@AttributeList", System.Data.SqlDbType.NVarChar, 1000);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!AttributeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@AttributeList] value[" + AttributeList.ToString() + "]");
							}
							parameter.Value = AttributeList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@TotalRows]");
						}
						parameter = new SqlParameter("@TotalRows", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!TotalRows.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@TotalRows] value[" + TotalRows.ToString() + "]");
							}
							parameter.Value = TotalRows.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CatalogFilter]");
						}
						parameter = new SqlParameter("@CatalogFilter", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CatalogFilter.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CatalogFilter] value[" + CatalogFilter.ToString() + "]");
							}
							parameter.Value = CatalogFilter.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CustomerID]");
						}
						parameter = new SqlParameter("@CustomerID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CustomerID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CustomerID] value[" + CustomerID.ToString() + "]");
							}
							parameter.Value = CustomerID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@LocaleID]");
						}
						parameter = new SqlParameter("@LocaleID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!LocaleID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@LocaleID] value[" + LocaleID.ToString() + "]");
							}
							parameter.Value = LocaleID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@TotalCount]");
						}
						parameter = new SqlParameter("@TotalCount", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!TotalCount.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@TotalCount] value[" + TotalCount.ToString() + "]");
							}
							parameter.Value = TotalCount.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_RelationshipManager_Relationship_WhereUsed_Get]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
												
						TotalCount = new SqlInt32((int)command.Parameters["@TotalCount"].Value);

						
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
		public static DataTable ComplianceCheckAttribute(IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Relationship_ComplianceCheck_Attribute", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Relationship_ComplianceCheck_Attribute]");
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
		public static DataSet GetCatalogAttributes(SqlString UserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Attr_CatalogAttrAllByUser_GetDT", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserLogin]");
						}
						parameter = new SqlParameter("@UserLogin", System.Data.SqlDbType.VarChar, 50);
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

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Attr_CatalogAttrAllByUser_GetDT]");
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
		public static void CopyCatalogAttributes(SqlInt32 intFromCatalogId, SqlInt32 intToCatalogId, SqlString CreateProgram, SqlString CreateUser, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Attr_CatalogNodeTypeAttr_Copy", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intFromCatalogId]");
						}
						parameter = new SqlParameter("@intFromCatalogId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intFromCatalogId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intFromCatalogId] value[" + intFromCatalogId.ToString() + "]");
							}
							parameter.Value = intFromCatalogId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intToCatalogId]");
						}
						parameter = new SqlParameter("@intToCatalogId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intToCatalogId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intToCatalogId] value[" + intToCatalogId.ToString() + "]");
							}
							parameter.Value = intToCatalogId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CreateProgram]");
						}
						parameter = new SqlParameter("@CreateProgram", System.Data.SqlDbType.NVarChar, 300);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CreateProgram.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CreateProgram] value[" + CreateProgram.ToString() + "]");
							}
							parameter.Value = CreateProgram.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CreateUser]");
						}
						parameter = new SqlParameter("@CreateUser", System.Data.SqlDbType.NVarChar, 300);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CreateUser.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CreateUser] value[" + CreateUser.ToString() + "]");
							}
							parameter.Value = CreateUser.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Attr_CatalogNodeTypeAttr_Copy]");
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
        public static DataTable GetAllCategories(SqlInt32 PK_Catalog, SqlInt32 FK_Locale, SqlString Filter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_getAllCategories", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Catalog]");
                        }
                        parameter = new SqlParameter("@PK_Catalog", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PK_Catalog.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Catalog] value[" + PK_Catalog.ToString() + "]");
                            }
                            parameter.Value = PK_Catalog.Value;
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Filter]");
                        }
                        parameter = new SqlParameter("@Filter", System.Data.SqlDbType.VarChar, 20);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!Filter.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Filter] value[" + Filter.ToString() + "]");
                            }
                            parameter.Value = Filter.Value;
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_getAllCategories]");
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
		public static DataTable GetNameValCollection(SqlString IdList, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Syndication_GetNameValCollection", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IdList]");
						}
						parameter = new SqlParameter("@IdList", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IdList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IdList] value[" + IdList.ToString() + "]");
							}
							parameter.Value = IdList.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Syndication_GetNameValCollection]");
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
		public static string GetCollectionValues(SqlInt32 FK_CNode, SqlInt32 ParentId, SqlInt32 FK_Catalog, SqlInt32 FK_Customer, SqlInt32 FK_Locale, SqlString InheritanceMode, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Catalog_GetCollectionValues_Xml", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_CNode]");
						}
						parameter = new SqlParameter("@FK_CNode", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_CNode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_CNode] value[" + FK_CNode.ToString() + "]");
							}
							parameter.Value = FK_CNode.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ParentId]");
						}
						parameter = new SqlParameter("@ParentId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ParentId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ParentId] value[" + ParentId.ToString() + "]");
							}
							parameter.Value = ParentId.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Customer]");
						}
						parameter = new SqlParameter("@FK_Customer", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Customer.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Customer] value[" + FK_Customer.ToString() + "]");
							}
							parameter.Value = FK_Customer.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@InheritanceMode]");
						}
						parameter = new SqlParameter("@InheritanceMode", System.Data.SqlDbType.Char, 1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!InheritanceMode.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@InheritanceMode] value[" + InheritanceMode.ToString() + "]");
							}
							parameter.Value = InheritanceMode.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Catalog_GetCollectionValues_Xml]");
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
		public static string GetCatalogNodeTypeAttributesXML(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlString nvchrNodeType, SqlInt32 intBranchLevel, SqlBoolean IncludeComplexAttrChildren, SqlBoolean ExcludeSearchable, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_Attr_CatalogNodeTypeAttr_GetXML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intOrgID]");
						}
						parameter = new SqlParameter("@intOrgID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intOrgID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intOrgID] value[" + intOrgID.ToString() + "]");
							}
							parameter.Value = intOrgID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@nvchrNodeType]");
						}
						parameter = new SqlParameter("@nvchrNodeType", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!nvchrNodeType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@nvchrNodeType] value[" + nvchrNodeType.ToString() + "]");
							}
							parameter.Value = nvchrNodeType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intBranchLevel]");
						}
						parameter = new SqlParameter("@intBranchLevel", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intBranchLevel.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intBranchLevel] value[" + intBranchLevel.ToString() + "]");
							}
							parameter.Value = intBranchLevel.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeComplexAttrChildren]");
						}
						parameter = new SqlParameter("@IncludeComplexAttrChildren", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeComplexAttrChildren.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeComplexAttrChildren] value[" + IncludeComplexAttrChildren.ToString() + "]");
							}
							parameter.Value = IncludeComplexAttrChildren.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ExcludeSearchable]");
						}
						parameter = new SqlParameter("@ExcludeSearchable", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ExcludeSearchable.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ExcludeSearchable] value[" + ExcludeSearchable.ToString() + "]");
							}
							parameter.Value = ExcludeSearchable.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_Attr_CatalogNodeTypeAttr_GetXML]");
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
		public static string GetSystemAttributes(IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_Attr_ObjectAttrVal_GetXML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_Attr_ObjectAttrVal_GetXML]");
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
		public static void PromoteItems(SqlString ExternalID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_WS_Promote_ByWF", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ExternalID]");
						}
						parameter = new SqlParameter("@ExternalID", System.Data.SqlDbType.NVarChar, 255);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_WS_Promote_ByWF]");
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
		public static void DemoteItems(SqlString ExternalID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_WS_Demote_ByWF", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ExternalID]");
						}
						parameter = new SqlParameter("@ExternalID", System.Data.SqlDbType.NVarChar, 255);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_WS_Demote_ByWF]");
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
		public static void ProcessSearchCriteria(SqlString Action, SqlInt32 PK_Security_SearchCriteria, SqlString CriteriaName, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog, SqlBoolean IsGlobalSearch, SqlString SearchCriteriaXml, SqlString loginUser, SqlString UserProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Search_ProcessSearchCriteria", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Action]");
						}
						parameter = new SqlParameter("@Action", System.Data.SqlDbType.NVarChar, 500);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!Action.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@Action] value[" + Action.ToString() + "]");
							}
							parameter.Value = Action.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Security_SearchCriteria]");
						}
						parameter = new SqlParameter("@PK_Security_SearchCriteria", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Security_SearchCriteria.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Security_SearchCriteria] value[" + PK_Security_SearchCriteria.ToString() + "]");
							}
							parameter.Value = PK_Security_SearchCriteria.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CriteriaName]");
						}
						parameter = new SqlParameter("@CriteriaName", System.Data.SqlDbType.NVarChar, 500);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CriteriaName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CriteriaName] value[" + CriteriaName.ToString() + "]");
							}
							parameter.Value = CriteriaName.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Security_User]");
						}
						parameter = new SqlParameter("@FK_Security_User", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Security_User.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Security_User] value[" + FK_Security_User.ToString() + "]");
							}
							parameter.Value = FK_Security_User.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IsGlobalSearch]");
						}
						parameter = new SqlParameter("@IsGlobalSearch", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IsGlobalSearch.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IsGlobalSearch] value[" + IsGlobalSearch.ToString() + "]");
							}
							parameter.Value = IsGlobalSearch.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SearchCriteriaXml]");
						}
						parameter = new SqlParameter("@SearchCriteriaXml", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!SearchCriteriaXml.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SearchCriteriaXml] value[" + SearchCriteriaXml.ToString() + "]");
							}
							parameter.Value = SearchCriteriaXml.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserProgram]");
						}
						parameter = new SqlParameter("@UserProgram", System.Data.SqlDbType.NVarChar, 200);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!UserProgram.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@UserProgram] value[" + UserProgram.ToString() + "]");
							}
							parameter.Value = UserProgram.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Search_ProcessSearchCriteria]");
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
		public static DataTable GetSearchCriterias(SqlInt32 PK_Security_SearchCriteria, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Search_GetSearchCriterias", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Security_SearchCriteria]");
						}
						parameter = new SqlParameter("@PK_Security_SearchCriteria", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Security_SearchCriteria.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Security_SearchCriteria] value[" + PK_Security_SearchCriteria.ToString() + "]");
							}
							parameter.Value = PK_Security_SearchCriteria.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Security_User]");
						}
						parameter = new SqlParameter("@FK_Security_User", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Security_User.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Security_User] value[" + FK_Security_User.ToString() + "]");
							}
							parameter.Value = FK_Security_User.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Search_GetSearchCriterias]");
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
        public static DataTable QuickSearch(SqlInt32 CatalogID, SqlString SearchValue, SqlInt32 PK_CNode, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Search_QuickSearch", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CatalogID]");
						}
						parameter = new SqlParameter("@CatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CatalogID] value[" + CatalogID.ToString() + "]");
							}
							parameter.Value = CatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SearchValue]");
						}
						parameter = new SqlParameter("@SearchValue", System.Data.SqlDbType.NVarChar, 1000);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!SearchValue.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SearchValue] value[" + SearchValue.ToString() + "]");
							}
							parameter.Value = SearchValue.Value;
						}

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_CNode]");
                        }
                        parameter = new SqlParameter("@PK_CNode", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!PK_CNode.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_CNode] value[" + PK_CNode.ToString() + "]");
                            }
                            parameter.Value = PK_CNode.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Search_QuickSearch]");
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
        /// Get the catalog 
        /// </summary>
		public static DataTable GetStagingCatalogId(SqlInt32 FK_Org, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_getStagingCatalog", (SqlConnection)connection))
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

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_getStagingCatalog]");
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
        /// Update the items with status Ready For Promote statging catalog
        /// </summary>
		public static DataTable MarkItemComplete(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlString CnodeList, SqlString vchrUserLogin, SqlString vchrUserRole, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_markItemComplete", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CnodeList]");
						}
						parameter = new SqlParameter("@CnodeList", System.Data.SqlDbType.NVarChar, 4000);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CnodeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CnodeList] value[" + CnodeList.ToString() + "]");
							}
							parameter.Value = CnodeList.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrUserRole]");
						}
						parameter = new SqlParameter("@vchrUserRole", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrUserRole.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrUserRole] value[" + vchrUserRole.ToString() + "]");
							}
							parameter.Value = vchrUserRole.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_markItemComplete]");
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
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static DataTable Get_CatalogCNodeOrgInfo(SqlInt32 CatalogID, SqlInt64 CNodeID, SqlString SKU, SqlString FindWhat, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_Catalog_CNode_GetInfo", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CatalogID]");
						}
						parameter = new SqlParameter("@CatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CatalogID] value[" + CatalogID.ToString() + "]");
							}
							parameter.Value = CatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeID]");
						}
						parameter = new SqlParameter("@CNodeID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!CNodeID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@CNodeID] value[" + CNodeID.ToString() + "]");
							}
							parameter.Value = CNodeID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SKU]");
						}
						parameter = new SqlParameter("@SKU", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!SKU.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SKU] value[" + SKU.ToString() + "]");
							}
							parameter.Value = SKU.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FindWhat]");
						}
						parameter = new SqlParameter("@FindWhat", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FindWhat.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FindWhat] value[" + FindWhat.ToString() + "]");
							}
							parameter.Value = FindWhat.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_Catalog_CNode_GetInfo]");
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
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static string SetBOMMatchingRelationship(SqlInt32 FK_StagingCatalogID, SqlString matchingXML, SqlString vchrProgramName, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_BOM_DoMatching_AddRelationship", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_StagingCatalogID]");
						}
						parameter = new SqlParameter("@FK_StagingCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_StagingCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_StagingCatalogID] value[" + FK_StagingCatalogID.ToString() + "]");
							}
							parameter.Value = FK_StagingCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@matchingXML]");
						}
						parameter = new SqlParameter("@matchingXML", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!matchingXML.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@matchingXML] value[" + matchingXML.ToString() + "]");
							}
							parameter.Value = matchingXML.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrProgramName]");
						}
						parameter = new SqlParameter("@vchrProgramName", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrProgramName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrProgramName] value[" + vchrProgramName.ToString() + "]");
							}
							parameter.Value = vchrProgramName.Value;
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

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_BOM_DoMatching_AddRelationship]");
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
        /// Return All the Detail of Given SKU and CNode
        /// </summary>
		public static DataTable GetALLCatalogCnodeDetailBySKU(SqlString txtXML, SqlString PassedType, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("Usp_CatalogDetail_By_Cnode_SKU", (SqlConnection)connection))
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
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NVarChar, -1);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PassedType]");
						}
						parameter = new SqlParameter("@PassedType", System.Data.SqlDbType.NVarChar, 10);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PassedType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PassedType] value[" + PassedType.ToString() + "]");
							}
							parameter.Value = PassedType.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[Usp_CatalogDetail_By_Cnode_SKU]");
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
		public static DataSet GetUserConfigMetadata(SqlXml ConfigXml, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Get_UserConfigType", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ConfigXml]");
						}
						parameter = new SqlParameter("@ConfigXml", System.Data.SqlDbType.Xml);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ConfigXml.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ConfigXml] value[" + ConfigXml.ToString() + "]");
							}
							parameter.Value = ConfigXml.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Get_UserConfigType]");
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
		public static DataSet GetUserConfigContextData(SqlInt32 FK_Application_ContextType, ref SqlString SeqDataTableforUI, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Get_UserConfigContextData", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_Application_ContextType]");
						}
						parameter = new SqlParameter("@FK_Application_ContextType", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_Application_ContextType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_Application_ContextType] value[" + FK_Application_ContextType.ToString() + "]");
							}
							parameter.Value = FK_Application_ContextType.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@SeqDataTableforUI]");
						}
						parameter = new SqlParameter("@SeqDataTableforUI", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.InputOutput;
						command.Parameters.Add(parameter);

						if (!SeqDataTableforUI.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@SeqDataTableforUI] value[" + SeqDataTableforUI.ToString() + "]");
							}
							parameter.Value = SeqDataTableforUI.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Get_UserConfigContextData]");
						}
						
						//Fill the DataSet with the rows that are returned.
						dataAdapter.Fill(dataSet, "table");
								
						SeqDataTableforUI = new SqlString((string)command.Parameters["@SeqDataTableforUI"].Value);

						
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
		public static DataTable GetApplicationConfig(SqlInt32 EventSourceID, SqlInt32 EventSubscriberID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Get_ApplicationConfig_Pick", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@EventSourceID]");
						}
						parameter = new SqlParameter("@EventSourceID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!EventSourceID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@EventSourceID] value[" + EventSourceID.ToString() + "]");
							}
							parameter.Value = EventSourceID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@EventSubscriberID]");
						}
						parameter = new SqlParameter("@EventSubscriberID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!EventSubscriberID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@EventSubscriberID] value[" + EventSubscriberID.ToString() + "]");
							}
							parameter.Value = EventSubscriberID.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Get_ApplicationConfig_Pick]");
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
		public static DataTable GetRelationshipCardinality(SqlInt32 FK_Catalog, SqlInt32 FK_NodeType_From, SqlInt32 FK_RelationshipType, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Relationship_Cardinality_Get", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_NodeType_From]");
						}
						parameter = new SqlParameter("@FK_NodeType_From", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_NodeType_From.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_NodeType_From] value[" + FK_NodeType_From.ToString() + "]");
							}
							parameter.Value = FK_NodeType_From.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@FK_RelationshipType]");
						}
						parameter = new SqlParameter("@FK_RelationshipType", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!FK_RelationshipType.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@FK_RelationshipType] value[" + FK_RelationshipType.ToString() + "]");
							}
							parameter.Value = FK_RelationshipType.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Relationship_Cardinality_Get]");
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
		public static void ProcessRelationshipCardinality(SqlString txtXML, SqlString UserName, SqlString ProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Relationship_Cardinality_Set", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.NVarChar, -1);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@UserName]");
						}
						parameter = new SqlParameter("@UserName", System.Data.SqlDbType.NVarChar, 100);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ProgramName]");
						}
						parameter = new SqlParameter("@ProgramName", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ProgramName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ProgramName] value[" + ProgramName.ToString() + "]");
							}
							parameter.Value = ProgramName.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Relationship_Cardinality_Set]");
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
		public static string ExtractCatalogByIDLocal(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrCoreAttrList, SqlString vchrTechAttrList, SqlInt32 FK_Locale, SqlString ProgramName, SqlBoolean IncludeInheritedValues, SqlBoolean ComputeInheritedValues, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getCatalogByID_Local_XML", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intExtSystemID]");
						}
						parameter = new SqlParameter("@intExtSystemID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intExtSystemID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intExtSystemID] value[" + intExtSystemID.ToString() + "]");
							}
							parameter.Value = intExtSystemID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@txtXML]");
						}
						parameter = new SqlParameter("@txtXML", System.Data.SqlDbType.Text, 2147483647);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrCoreAttrList]");
						}
						parameter = new SqlParameter("@vchrCoreAttrList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrCoreAttrList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrCoreAttrList] value[" + vchrCoreAttrList.ToString() + "]");
							}
							parameter.Value = vchrCoreAttrList.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrTechAttrList]");
						}
						parameter = new SqlParameter("@vchrTechAttrList", System.Data.SqlDbType.VarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrTechAttrList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrTechAttrList] value[" + vchrTechAttrList.ToString() + "]");
							}
							parameter.Value = vchrTechAttrList.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ProgramName]");
						}
						parameter = new SqlParameter("@ProgramName", System.Data.SqlDbType.VarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ProgramName.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ProgramName] value[" + ProgramName.ToString() + "]");
							}
							parameter.Value = ProgramName.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@IncludeInheritedValues]");
						}
						parameter = new SqlParameter("@IncludeInheritedValues", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!IncludeInheritedValues.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@IncludeInheritedValues] value[" + IncludeInheritedValues.ToString() + "]");
							}
							parameter.Value = IncludeInheritedValues.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ComputeInheritedValues]");
						}
						parameter = new SqlParameter("@ComputeInheritedValues", System.Data.SqlDbType.Bit);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ComputeInheritedValues.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ComputeInheritedValues] value[" + ComputeInheritedValues.ToString() + "]");
							}
							parameter.Value = ComputeInheritedValues.Value;
						}

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getCatalogByID_Local_XML]");
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
		public static void DeleteSearchCriteria(SqlInt32 PK_Security_SearchCriteria, SqlString loginUser, SqlString ModProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Search_DeleteSearchCriteria", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@PK_Security_SearchCriteria]");
						}
						parameter = new SqlParameter("@PK_Security_SearchCriteria", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!PK_Security_SearchCriteria.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@PK_Security_SearchCriteria] value[" + PK_Security_SearchCriteria.ToString() + "]");
							}
							parameter.Value = PK_Security_SearchCriteria.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@loginUser]");
						}
						parameter = new SqlParameter("@loginUser", System.Data.SqlDbType.NVarChar, 100);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ModProgram]");
						}
						parameter = new SqlParameter("@ModProgram", System.Data.SqlDbType.NVarChar, 50);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ModProgram.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ModProgram] value[" + ModProgram.ToString() + "]");
							}
							parameter.Value = ModProgram.Value;
						}

						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Search_DeleteSearchCriteria]");
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
		public static DataTable GetStageTransitionButtons(SqlString ContextXml, SqlInt32 StageId, SqlString CNodeList, SqlString ToolbarButtonXml, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Get_StageTransitionButtons_DT", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ContextXml]");
						}
						parameter = new SqlParameter("@ContextXml", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ContextXml.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ContextXml] value[" + ContextXml.ToString() + "]");
							}
							parameter.Value = ContextXml.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@StageId]");
						}
						parameter = new SqlParameter("@StageId", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!StageId.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@StageId] value[" + StageId.ToString() + "]");
							}
							parameter.Value = StageId.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeList]");
						}
						parameter = new SqlParameter("@CNodeList", System.Data.SqlDbType.NVarChar, -1);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ToolbarButtonXml]");
						}
						parameter = new SqlParameter("@ToolbarButtonXml", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ToolbarButtonXml.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ToolbarButtonXml] value[" + ToolbarButtonXml.ToString() + "]");
							}
							parameter.Value = ToolbarButtonXml.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Get_StageTransitionButtons_DT]");
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
		public static DataTable GetAssignmentButtons(SqlString AssignmentStatus, SqlString CNodeList, SqlString ToolbarButtonXml, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Get_AssignmentButtons_DT", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@AssignmentStatus]");
						}
						parameter = new SqlParameter("@AssignmentStatus", System.Data.SqlDbType.NVarChar, 100);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!AssignmentStatus.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@AssignmentStatus] value[" + AssignmentStatus.ToString() + "]");
							}
							parameter.Value = AssignmentStatus.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@CNodeList]");
						}
						parameter = new SqlParameter("@CNodeList", System.Data.SqlDbType.NVarChar, -1);
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ToolbarButtonXml]");
						}
						parameter = new SqlParameter("@ToolbarButtonXml", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ToolbarButtonXml.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ToolbarButtonXml] value[" + ToolbarButtonXml.ToString() + "]");
							}
							parameter.Value = ToolbarButtonXml.Value;
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Get_AssignmentButtons_DT]");
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
		public static DataTable GetWorkflowPanel(SqlInt32 ContainerID, SqlInt32 loginUserID, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Catalog_GetWorkflowPanel", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ContainerID]");
						}
						parameter = new SqlParameter("@ContainerID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!ContainerID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ContainerID] value[" + ContainerID.ToString() + "]");
							}
							parameter.Value = ContainerID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@loginUserID]");
						}
						parameter = new SqlParameter("@loginUserID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!loginUserID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@loginUserID] value[" + loginUserID.ToString() + "]");
							}
							parameter.Value = loginUserID.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Catalog_GetWorkflowPanel]");
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
        public static string GetSearchCategoriesByCriteria(SqlString vchrSearchValue, SqlInt32 intCatalogID, SqlInt32 intParentID, SqlString toolTipAttributeList, SqlString vchrUserLogin,SqlInt32 dataLocale, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Sec_Category_Search", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@vchrSearchValue]");
						}
						parameter = new SqlParameter("@vchrSearchValue", System.Data.SqlDbType.NVarChar, 300);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!vchrSearchValue.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@vchrSearchValue] value[" + vchrSearchValue.ToString() + "]");
							}
							parameter.Value = vchrSearchValue.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intCatalogID]");
						}
						parameter = new SqlParameter("@intCatalogID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intCatalogID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intCatalogID] value[" + intCatalogID.ToString() + "]");
							}
							parameter.Value = intCatalogID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@intParentID]");
						}
						parameter = new SqlParameter("@intParentID", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!intParentID.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@intParentID] value[" + intParentID.ToString() + "]");
							}
							parameter.Value = intParentID.Value;
						}
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@toolTipAttributeList]");
						}
						parameter = new SqlParameter("@toolTipAttributeList", System.Data.SqlDbType.NVarChar, -1);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!toolTipAttributeList.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@toolTipAttributeList] value[" + toolTipAttributeList.ToString() + "]");
							}
							parameter.Value = toolTipAttributeList.Value;
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Locale]");
                        }
                        parameter = new SqlParameter("@Locale", System.Data.SqlDbType.NVarChar, 150);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!dataLocale.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ocale] value[" + dataLocale.ToString() + "]");
                            }
                            parameter.Value = dataLocale.Value;
                        }

						StringBuilder buffer = new StringBuilder();
						
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Sec_Category_Search]");
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
        public static DataTable GetCategoryNavPanel(SqlInt32 catalogId, SqlString sysAttrSelectionXml, SqlString categorySearchColumn, SqlString categorySearchString, SqlInt64 ParentCategoryId, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrUserLogin, SqlInt32 dataLocale, IDbConnection connection, IDbTransaction transaction)
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
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_Catalog_GetCategoryNavPanel", (SqlConnection)connection))
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@catalogId]");
                        }
                        parameter = new SqlParameter("@catalogId", System.Data.SqlDbType.Int);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!catalogId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@catalogId] value[" + catalogId.ToString() + "]");
                            }
                            parameter.Value = catalogId.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@sysAttrSelectionXml]");
                        }
                        parameter = new SqlParameter("@sysAttrSelectionXml", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!sysAttrSelectionXml.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@sysAttrSelectionXml] value[" + sysAttrSelectionXml.ToString() + "]");
                            }
                            parameter.Value = sysAttrSelectionXml.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@categorySearchColumn]");
                        }
                        parameter = new SqlParameter("@categorySearchColumn", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!categorySearchColumn.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@categorySearchColumn] value[" + categorySearchColumn.ToString() + "]");
                            }
                            parameter.Value = categorySearchColumn.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@categorySearchString]");
                        }
                        parameter = new SqlParameter("@categorySearchString", System.Data.SqlDbType.NVarChar, -1);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!categorySearchString.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@categorySearchString] value[" + categorySearchString.ToString() + "]");
                            }
                            parameter.Value = categorySearchString.Value;
                        }
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@ParentCategoryId]");
                        }
                        parameter = new SqlParameter("@ParentCategoryId", System.Data.SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!ParentCategoryId.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ParentCategoryId] value[" + ParentCategoryId.ToString() + "]");
                            }
                            parameter.Value = ParentCategoryId.Value;
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
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@Locale]");
                        }
                        parameter = new SqlParameter("@Locale", System.Data.SqlDbType.NVarChar, 150);
                        parameter.Direction = ParameterDirection.Input;
                        command.Parameters.Add(parameter);

                        if (!dataLocale.IsNull)
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@ocale] value[" + dataLocale.ToString() + "]");
                            }
                            parameter.Value = dataLocale.Value;
                        }

                        //Create a new DataSet to hold the records.
                        DataSet dataSet = new DataSet();

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_Catalog_GetCategoryNavPanel]");
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

	}
}		
