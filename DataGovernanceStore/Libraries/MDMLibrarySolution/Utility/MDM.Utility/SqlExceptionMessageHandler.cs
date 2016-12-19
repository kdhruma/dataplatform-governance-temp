using System;
using System.Data.SqlClient;
namespace MDM.Utility
{
    /// <summary>
    /// SqlException Error Code Message Handler 
    /// This exposes the error codes as user friendly messages
    /// </summary>
    public class SqlExceptionMessageHandler
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public SqlExceptionMessageHandler()
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get the sql exception message code.
        /// </summary>
        /// <param name="ex">The sql exception.</param>
        /// <returns>The user friendly error message for database.</returns>
        public static String GetMessage(Exception ex)
        {
            String message = String.Empty;
            SqlException sqlEx = null;

            if (ex is SqlException)
            {
                sqlEx = ex as SqlException;


                //TODO get the values from resource files
                switch (sqlEx.Number)
                {
                    case -1:
                    case 53:
                        message = "Cannot connect to database server; check if server exists";
                        break;
                    case 4060:
                        message = "Cannot open database; check database exists";
                        break;
                    case 18456:
                        message = "The username or password is not correct for the database requested";
                        break;
                    default:
                        message = "Database error occurred";
                        break;
                }
            }

            return message;

        }

        #endregion
    }
}
