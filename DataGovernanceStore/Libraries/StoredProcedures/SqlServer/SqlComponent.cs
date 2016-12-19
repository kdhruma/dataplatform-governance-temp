
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
	public class SqlComponent
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private SqlComponent()
		{
		}
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable getEntityTypes(SqlInt32 NodeTypeParent, IDbConnection connection, IDbTransaction transaction)
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
					using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getEntityTypes", (SqlConnection)connection))
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
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@NodeTypeParent]");
						}
						parameter = new SqlParameter("@NodeTypeParent", System.Data.SqlDbType.Int);
						parameter.Direction = ParameterDirection.Input;
						command.Parameters.Add(parameter);

						if (!NodeTypeParent.IsNull)
						{
							if (Constants.TRACING_ENABLED)
							{
								MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Setting Parameter: name[@NodeTypeParent] value[" + NodeTypeParent.ToString() + "]");
							}
							parameter.Value = NodeTypeParent.Value;
						}

						
						//Create a new DataSet to hold the records.
						DataSet dataSet = new DataSet();
		
						if (Constants.TRACING_ENABLED)
						{
							MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getEntityTypes]");
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

        ///// <summary>
        ///// 
        ///// </summary>
        //public static DataTable GetNodeTypesDT(IDbConnection connection, IDbTransaction transaction)
        //{
        //    DateTime startDate = DateTime.Now;
        //    bool internalConnect = false;
        //    try
        //    {
        //        if (connection == null)
        //        {
        //            string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
        //            connection = new SqlConnection(connectionString);
        //            connection.Open();
        //            internalConnect = true;
        //        }
        //        else if (connection.State == ConnectionState.Closed)
        //        {
        //            connection.Open();
        //            internalConnect = true;
        //        }
        //        try
        //        {
        //            using(SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_N_getAllNodeTypes_DT", (SqlConnection)connection))
        //            {
        //                SqlCommand command = dataAdapter.SelectCommand;
        //                command.Transaction = (SqlTransaction)transaction;
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.CommandTimeout = 120;

        //                IDataParameter parameter = null;
        //                if (Constants.TRACING_ENABLED)
        //                {
        //                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Adding Parameter: name[@RETURN_VALUE]");
        //                }
        //                parameter = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int);
        //                parameter.Direction = ParameterDirection.ReturnValue;
        //                command.Parameters.Add(parameter);

						
        //                //Create a new DataSet to hold the records.
        //                DataSet dataSet = new DataSet();
		
        //                if (Constants.TRACING_ENABLED)
        //                {
        //                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Calling:[usp_N_getAllNodeTypes_DT]");
        //                }
						
        //                //Fill the DataSet with the rows that are returned.
        //                dataAdapter.Fill(dataSet, "table");
						
						
        //                if (Constants.TRACING_ENABLED)
        //                {
        //                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Result:[" + dataSet.GetXml() + "]");
        //                }
						
        //                return dataSet.Tables[0];
						
        //            }
        //        }
        //        finally
        //        {
        //            if (internalConnect)
        //            {
        //                connection.Close();
        //            }
        //            if (Constants.TRACING_ENABLED)
        //            {
        //                DateTime endDate = DateTime.Now;
        //                TimeSpan span = endDate.Subtract(startDate);
        //                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Verbose, "Time Spent:" + span.TotalMilliseconds.ToString());
        //            }					
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "Failed with exception - " + e.ToString());
        //        throw;
        //    }
        //}

	}
}		
