using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Exposes class that contains data for user authentication
    /// </summary>
    public class AuthenticationData : IAuthenticationData
    {
        #region Fields

        private String _userName;
        private String _password;
        private String _ip;
        private DateTime _timestamp;
        private String _scheme;
        private String _host;
        private Int32 _port;
        private String _pathAndQuery;

        #endregion

        #region Contructors

        /// <summary>
        /// Constructs AuthenticationData class with provided userName and password
        /// </summary>
        /// <param name="userName">Represents user name</param>
        /// <param name="password">Represents user password</param>
        public AuthenticationData(String userName, String password)
            : this(userName, password, null)
        {
        }

        /// <summary>
        /// Constructs AuthenticationData class with provided userName, password and httpContext
        /// </summary>
        /// <param name="userName">Represents user name</param>
        /// <param name="password">Represents user password</param>
        /// <param name="context">Represents HttpContext</param>
        public AuthenticationData(String userName, String password, HttpContext context)
        {
            UserName = userName;
            Password = password;
            if (context != null)
            {
                Ip = GetUserIp(context);
                Timestamp = context.Timestamp;
                Scheme = context.Request.Url.Scheme;
                Host = context.Request.Url.Host;
                Port = context.Request.Url.Port;
                PathAndQuery = context.Request.Url.PathAndQuery;
            }

        } 

        #endregion

        #region Properties

        /// <summary>
        /// Represents user name
        /// </summary>
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Represents user password
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Represents user ip
        /// </summary>
        public String Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// Represents timestamp
        /// </summary>
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        /// <summary>
        /// Represents scheme
        /// </summary>
        public String Scheme
        {
            get { return _scheme; }
            set { _scheme = value; }
        }

        /// <summary>
        /// Represents host
        /// </summary>
        public String Host
        {
            get { return _host; }
            set { _host = value; }
        }

        /// <summary>
        /// Represents port
        /// </summary>
        public Int32 Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// Represents path and query
        /// </summary>
        public String PathAndQuery
        {
            get { return _pathAndQuery; }
            set { _pathAndQuery = value; }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Converts AuthenticationData to string
        /// </summary>
        /// <returns>Returns a string representation of AuthenticationData</returns>
        public String ToString(Boolean exposePassword)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.AppendFormat("UserName='{0}',", UserName);
            if (exposePassword)
            {
                sb.AppendFormat("Password='{0}',", Password);
            }
            sb.AppendFormat("Ip='{0}',", Ip);
            sb.AppendFormat("TimestampUTC='{0}',", Timestamp.ToUniversalTime());
            sb.AppendFormat("Scheme='{0}',", Scheme);
            sb.AppendFormat("Host='{0}',", Host);
            sb.AppendFormat("Port='{0}',", Port.ToString(CultureInfo.InvariantCulture));
            sb.AppendFormat("PathAndQuery='{0}'", PathAndQuery);

            return sb.ToString();
        }

        /// <summary>
        /// Converts AuthenticationData to string
        /// </summary>
        /// <returns>Returns a string representation of AuthenticationData without password</returns>
        public override String ToString()
        {
            return this.ToString(false);
        }

        /// <summary>
        /// Gets user IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns user ip from HttpContext</returns>
        protected String GetUserIp(HttpContext context)
        {
            if (context == null)
            {
                return String.Empty;
            }

            String ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(ip))
            {
                ip = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ip = ip.Split(',').Last().Trim();
            }

            return ip;
        }
        
        #endregion

    }
}
