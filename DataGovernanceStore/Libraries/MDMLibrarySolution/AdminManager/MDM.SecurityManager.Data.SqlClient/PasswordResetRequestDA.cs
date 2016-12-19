using System;
using System.Data.SqlClient;


namespace MDM.SecurityManager.Data.SqlClient
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// /// Contains data access logic for password reset request
    /// </summary>
    public class PasswordResetRequestDA : SqlClientDataAccessBase
    {
        #region Methods

        /// <summary>
        /// Get Password Reset Request by Token
        /// </summary>
        /// <param name="token">Token for a password reset request</param>
        /// <param name="command">Database command properties for provided caller context</param>
        /// <returns>PasswordResetRequest object for a given token</returns>
        public PasswordResetRequest GetByToken(String token, DBCommandProperties command)
        {
            PasswordResetRequest passwordResetRequest = null;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("SecurityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("SecurityManager_PasswordResetRequest_Get_ParametersArray");

                parameters[0].Value = token;

                const String storedProcedureName = "usp_SecurityManager_PasswordResetRequest_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    passwordResetRequest = ReadPasswordResetRequest(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return passwordResetRequest;
        }

        /// <summary>
        /// Process password reset request
        /// </summary>
        /// <param name="passwordResetRequest">Password reset request to be processed</param>
        /// <param name="command">Database command properties for provided caller context</param>
        public void Process(PasswordResetRequest passwordResetRequest, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            SqlParametersGenerator generator = new SqlParametersGenerator("SecurityManager_SqlParameters");
            parameters = generator.GetParameters("SecurityManager_PasswordResetRequest_Process_ParametersArray");
            parameters[0].Value = passwordResetRequest.SecurityUserId;
            parameters[1].Value = passwordResetRequest.Token;
            parameters[2].Value = passwordResetRequest.RequestedDateTime;

            storedProcedureName = "usp_SecurityManager_PasswordResetRequest_Process";

            this.ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);
        }

        /// <summary>
        /// Update security user password for a given token
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="password">New password</param>
        /// <param name="command">Database command properties for provided caller context</param>
        public void PasswordReset(String token, String password, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            SqlParametersGenerator generator = new SqlParametersGenerator("SecurityManager_SqlParameters");
            parameters = generator.GetParameters("SecurityManager_PasswordResetRequest_PasswordReset_ParametersArray");
            parameters[0].Value = token;
            parameters[1].Value = password;

            storedProcedureName = "usp_SecurityManager_PasswordResetRequest_PasswordReset";

            this.ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);
        }

        #endregion

        #region Helper Methods

        private PasswordResetRequest ReadPasswordResetRequest(SqlDataReader reader)
        {

            PasswordResetRequest passwordResetRequest = null;

            while (reader.Read())
            {
                Int32 id = 0;
                Int32 userId = 0;
                String token = String.Empty;
                DateTime requestedDateTime = DateTime.MinValue;
                Boolean isPasswordReset = false;

                if (reader["PK_Security_PasswordResetRequest"] != null)
                    id = ValueTypeHelper.Int32TryParse(reader["PK_Security_PasswordResetRequest"].ToString(), 0);

                if (reader["FK_UserId"] != null)
                    userId = ValueTypeHelper.Int32TryParse(reader["FK_UserId"].ToString(), 0);

                if (reader["PasswordRequestId"] != null)
                    token = reader["PasswordRequestId"].ToString();

                if (reader["RequestedTime"] != null)
                    requestedDateTime = ValueTypeHelper.ConvertToDateTime(reader["RequestedTime"].ToString());

                if (reader["IsPasswordReset"] != null)
                    isPasswordReset = ValueTypeHelper.ConvertToBoolean(reader["IsPasswordReset"].ToString());

                passwordResetRequest = new PasswordResetRequest()
                {
                    Id = id,
                    SecurityUserId = userId,
                    Token = token,
                    RequestedDateTime = requestedDateTime,
                    IsPasswordReset = isPasswordReset
                };
            }

            return passwordResetRequest;
        }

        #endregion
    }
}
