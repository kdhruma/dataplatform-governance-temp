using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace MDM.Core
{ 
    /// <summary>
    /// The base class for all Sql Server Data Access Clients
    /// </summary>
    /// <remarks>Try to reuse these methods before adding new ones, since this is the core project which would affect everyone, approval needs to be obtained</remarks>
    public abstract class SqlClientDataAccessBase
    {
        private SqlCommand PrepareCommand(SqlConnection connection, SqlParameter[] parameters, String storedProcedureOrSqlText, CommandType type)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = storedProcedureOrSqlText;
            command.CommandType = type;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            Int32 dataCommandTimeout = AppConfiguration.GetSettingAsInteger("DataCommandTimeout");
            command.CommandTimeout = dataCommandTimeout;

            return command;
        }

        /// <summary>
        /// Uses a SqlCommand to execute its scalar method
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="parameters">The stored procedure parameters, pass null if there are no parameters</param>
        /// <param name="storedProcedure">The stored procedure name to execute</param>
        /// <returns>The single record with a maximum of 2033 characters. You can use it to get xml values which are less than 2033 characters</returns>
        protected Object ExecuteProcedureScalar(String connectionString, SqlParameter[] parameters, String storedProcedure)
        {
            Object result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = PrepareCommand(connection, parameters, storedProcedure, CommandType.StoredProcedure);

                connection.Open();
                result = command.ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        /// Uses a SqlCommand to execute its Non Query Method
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="parameters">The stored procedure parameters, pass null if there are no parameters</param>
        /// <param name="storedProcedure">The stored procedure name to execute which doesn't return any records</param>
        /// <returns>The number of records affected by the query</returns>
        protected Int32 ExecuteProcedureNonQuery(String connectionString, SqlParameter[] parameters, String storedProcedure)
        {
            Int32 result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = PrepareCommand(connection, parameters, storedProcedure, CommandType.StoredProcedure);
                connection.Open();
                
                result = command.ExecuteNonQuery();

            }
            return result;
        }

        /// <summary>
        /// Uses a SqlCommand and return a single row object array with one or multiple columns without column names
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="parameters">The stored procedure parameters, pass null if there are no parameters</param>
        /// <param name="storedProcedure">The stored procedure name to execute</param>
        /// <returns>An object Array containing the columns in a row</returns>
        protected Object[] ExecuteProcedureSingleRow(String connectionString, SqlParameter[] parameters, String storedProcedure)
        {
            Object[] result = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                command = PrepareCommand(connection, parameters, storedProcedure, CommandType.StoredProcedure);
                connection.Open();
                
                reader = command.ExecuteReader(CommandBehavior.SingleRow);

                while(reader.Read())
                {

                    result = new Object[reader.FieldCount];

                    reader.GetValues(result);
                }
                reader.Close();
            }
            return result;
        }

        /// <summary>
        /// Uses a SqlCommand and returns the SqlDataReader
        /// </summary>
        /// <remarks>Since the SqlDataReader can't work without the SqlConnection the open connection is given along with the reader, it is  user's responsibility to close the reader which would close the connection</remarks>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="parameters">The stored procedure parameters, pass null if there are no parameters</param>
        /// <param name="storedProcedure">The stored procedure name to execute</param>
        /// <returns>The SqlDataReader ready to be read</returns>
        protected SqlDataReader ExecuteProcedureReader(String connectionString, SqlParameter[] parameters, String storedProcedure)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                command = PrepareCommand(connection, parameters, storedProcedure, CommandType.StoredProcedure);
                connection.Open();

                //don't close the connection and return the data reader
                //when the datareader closes, the connection would also be closed
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch
            {
                connection.Dispose();
                throw;
            }            
        }

        /// <summary>
        /// Uses a SqlCommand and returns the DataSet
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="parameters">The stored procedure parameters, pass null if there are no parameters</param>
        /// <param name="storedProcedure">The stored procedure name to execute</param>
        /// <returns>The DataSet ready to be used</returns>
        protected DataSet ExecuteDataSet(String connectionString, SqlParameter[] parameters, String storedProcedure)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {   
                using (SqlCommand command = PrepareCommand(connection, parameters, storedProcedure, CommandType.StoredProcedure))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet);
                    }
                }
            }

            return dataSet;
        }

        /// <summary>
        /// Uses Sql Command and reader to fill a collection with specific objects provided the object has a constructor which accepts an object array
        /// </summary>
        /// <example>
        /// ExecuteProcedureCollection(
        ///        AppConfigurationHelper.ConnectionString,
        ///        parameters,
        ///        procedure,
        ///        values => new BusinessRule( values ) );
        ///        
        /// The BusinessRule Object would have a constructor for 
        /// public BusinessRule(Object[] objectArray)
        /// </example>
        /// <typeparam name="T">The Object to be filled as a collection</typeparam>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="parameters">The stored procedure parameters, pass null if there are no parameters</param>
        /// <param name="storedProcedure">The stored procedure name to execute</param>
        /// <param name="createInstance">The object instance</param>
        /// <returns>The object collection</returns>
        protected Collection<T> ExecuteProcedureCollection<T>( String connectionString, SqlParameter[] parameters, String storedProcedure, Func<Object[], T> createInstance )
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using ( SqlCommand command = PrepareCommand(connection, parameters, storedProcedure, CommandType.StoredProcedure))
                {
                    connection.Open();

                    using ( SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection) )
                    {
                        Collection<T> data = new Collection<T>();
                        while (reader.Read())
                        {
                            Object[] values = new Object[reader.FieldCount];
                            reader.GetValues( values );
                            data.Add( createInstance( values ) );
                        }
                        return data;
                    }
                }
            }
        }


        /// <summary>
        /// Gets and SqlCommand object with the connection open, parameters and storedprocedure set, this is tob used for bulk operation where we change the values of the parameters and perfrom CUD operation on the database without having to close the connection.
        /// </summary>
        /// <remarks>Since the connection is open its the resonsibility of the caller to close it</remarks>
        /// <example>
        /// Here were are trying to insert multiple addresses into the table without closing the connection
        /// <code>
        ///public Int32 CreateMultiple(AddressDTO[] addressesData)
        ///{
        ///    Int32 result = 0;
        ///    SqlCommand command = null;
        ///
        ///    try
        ///    {
        ///        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
        ///        {
        ///            SqlParameter[] parameters;
        ///            String connectionString = String.Empty;
        ///            String storedProcedureName = String.Empty;
        ///
        ///            SqlParametersGenerator generator = new SqlParametersGenerator("CustomerManager_SqlParameters");
        ///
        ///            parameters = generator.GetParameters("MasterManager_Address_Create_ParametersArray");
        ///
        ///            connectionString = AppConfigurationHelper.ConnectionString;
        ///
        ///            storedProcedureName = "usp_MasterManager_Address_Create";
        ///
        ///            command = GetProcedureCommand(connectionString, parameters, storedProcedureName);
        ///
        ///            foreach (AddressDTO addressData in addressesData)
        ///            {
        ///                parameters[0].Value = addressData.AddressLine;
        ///                parameters[1].Value = addressData.City;
        ///                parameters[2].Value = addressData.State;
        ///                parameters[3].Value = addressData.Country;
        ///
        ///                command.ExecuteNonQuery();
        ///            }
        ///            scope.Complete();
        ///        }
        ///
        ///        return result;
        ///    }
        ///    finally
        ///    {
        ///        //dispose the connection and the command
        ///        if (command != null)
        ///        {
        ///            if (command.Connection != null)
        ///            {
        ///                command.Connection.Dispose();
        ///            }
        ///            command.Dispose();
        ///        }
        ///    }
        ///}
        ///</code>
        /// </example>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="parameters">The stored procedure parameters, pass null if there are no parameters</param>
        /// <param name="storedProcedure">The stored procedure name to execute</param>
        /// <returns>The SqlCommandObject </returns>
        protected SqlCommand GetProcedureCommand(String connectionString, SqlParameter[] parameters, String storedProcedure)
        {

            SqlCommand command = null;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                command = PrepareCommand(connection, parameters, storedProcedure, CommandType.StoredProcedure);
                connection.Open();

                //don't close the connection and return the sqlcommand
                return command;
            }
            catch
            {
                connection.Dispose();
                throw;
            }

        }
    }
}



