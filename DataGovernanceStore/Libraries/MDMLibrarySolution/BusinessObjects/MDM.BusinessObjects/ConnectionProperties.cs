using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Represent connection properties for storing database connection related information
    /// </summary>
    public class ConnectionProperties : MDMObject, IConnectionProperties
    {
        #region Fields

        /// <summary>
        /// Field denoting server id
        /// </summary>
        private String _serverId = String.Empty;

        /// <summary>
        /// Field denoting user name
        /// </summary>
        private String _userName = String.Empty;

        /// <summary>
        /// Field denoting password
        /// </summary>
        private String _password = String.Empty;

        /// <summary>
        /// Field denoting database name
        /// </summary>
        private String _databaseName = String.Empty;

        /// <summary>
        /// Field denoting connection attributes like application name, pool size etc.
        /// </summary>
        private String _connectionAttribtues = String.Empty;

        /// <summary>
        /// Field denoting whether it is windows authentication
        /// </summary>
        private Boolean _isWindowsAuthentication = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting server id
        /// </summary>
        public String ServerId
        {
            get { return _serverId; }
            set { _serverId = value; }
        }

        /// <summary>
        /// Property denoting user name
        /// </summary>
        public new String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Property denoting password
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Property denoting database name
        /// </summary>
        public String DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        /// <summary>
        /// Property denoting connection attributes like application name, pool size etc.
        /// </summary>
        public String ConnectionAttribtues
        {
            get { return _connectionAttribtues; }
            set { _connectionAttribtues = value; }
        }

        /// <summary>
        /// Property denoting whether it is windows authentication
        /// </summary>
        public Boolean IsWindowsAuthentication
        {
            get { return _isWindowsAuthentication; }
            set { _isWindowsAuthentication = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ConnectionProperties()
            :base()
        {
        }

        /// <summary>
        /// Constructor which takes server id, user name, password, database name, and connection attributes as input parameters
        /// </summary>
        /// <param name="serverId">Indicates the server id of the database to be connected</param>
        /// <param name="userName">Indicates the user name used for connecting to database</param>
        /// <param name="password">Indicates the password used for connecting to database</param>
        /// <param name="databaseName">Indicates the database name to which connection to be done</param>
        /// <param name="connectionAttributes">Indicates the connection attributes required for connection</param>
        public ConnectionProperties(String serverId, String userName, String password, String databaseName, String connectionAttributes)
            : base()
        {
            this.ServerId = serverId;
            this.UserName = userName;
            this.Password = password;
            this.DatabaseName = databaseName;
            this.ConnectionAttribtues = connectionAttributes;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get connection string from connection properties.
        /// </summary>
        /// <returns>Connection string</returns>
        public String GetConnectionString()
        {
            String connectionString = String.Empty;

            //Create connection string only if required info is found.
            if ( !String.IsNullOrWhiteSpace(this.ServerId) && !String.IsNullOrWhiteSpace(this.DatabaseName) )
            {
                connectionString = String.Concat(connectionString, "Data Source=", this.ServerId, ";");
                connectionString = String.Concat(connectionString, "Initial Catalog=", this.DatabaseName, ";");
                if ( this.IsWindowsAuthentication == false )
                {
                    if ( !String.IsNullOrWhiteSpace(this.UserName) && !String.IsNullOrWhiteSpace(this.Password) )
                    {
                        connectionString = String.Concat(connectionString, "User ID=", this.UserName, ";");
                        connectionString = String.Concat(connectionString, "Password=", this.Password, ";");
                    }
                    else
                    {
                        throw new Exception("User Id / Password is not provided. Please check connection configuration.");
                    }
                }
                else
                {
                    this.ConnectionAttribtues = String.Concat(this.ConnectionAttribtues + "Integrated Security=true;");
                }

                if ( !String.IsNullOrWhiteSpace(this.ConnectionAttribtues) )
                {
                    connectionString = String.Concat(connectionString, this.ConnectionAttribtues);
                }
            }
            
            return connectionString;
        }

        #endregion Methods
    }
}
