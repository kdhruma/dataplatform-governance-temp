using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace MDM.AdminManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Specifies data access operations for security user
    /// </summary>
    public class SecurityUserDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="countFrom"></param>
        /// <param name="countTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public Collection<SecurityUser> GetAllUsers(Int32 userType, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, String userLogin)
        {
            SqlDataReader reader = null;
            StringBuilder returnValue = new StringBuilder();
            SecurityUser securityUser = new SecurityUser();
            Collection<SecurityUser> securityUserCollection = new Collection<SecurityUser>();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_getUsers_XML_ParametersArray");

                parameters[0].Value = 0;
                parameters[1].Value = userType;
                parameters[2].Value = countFrom;
                parameters[3].Value = countTo;
                parameters[4].Value = sortColumn;
                parameters[5].Value = searchColumn;
                parameters[6].Value = searchParameter;
                parameters[7].Value = userLogin;

                storedProcedureName = "usp_Sec_Admin_getUsers_XML";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    returnValue.Append(reader.GetString(0));
                }

                XmlDocument objXML = new XmlDocument();
                objXML.LoadXml(returnValue.ToString());

                XmlNodeList userList = objXML.SelectNodes("/Users/User");

                if (userList.Count > 0)
                {
                    foreach (XmlNode user in userList)
                    {
                        securityUser = new SecurityUser(user.OuterXml);
                        securityUserCollection.Add(securityUser);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return securityUserCollection;
        }

        /// <summary>
        /// Get Security User details.
        /// </summary>
        /// <param name="userLogins">Indicates the list of user logins</param>
        /// <param name="userIds">Indicates the list of user ids</param>
        /// <param name="includeDisabledUsers">Indicates whether disabled users should be ignored or not</param>
        /// <returns></returns>
        public SecurityUserCollection GetUsers(Collection<String> userLogins, Collection<Int32> userIds, Boolean includeDisabledUsers = false)
        {
            SqlDataReader reader = null;
            SecurityUserCollection securityUsers = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_SecurityUser_GetUser_ParametersArray");

                #region Populate table value parameters for User Details

                List<SqlDataRecord> userdetails = new List<SqlDataRecord>();

                SqlMetaData[] userData = generator.GetTableValueMetadata("AdminManager_SecurityUser_GetUser_ParametersArray", parameters[0].ParameterName);

                SqlDataRecord userRecord = new SqlDataRecord();

                if (userIds != null && userIds.Count > 0)
                {
                    foreach (Int32 id in userIds)
                    {
                        userRecord = new SqlDataRecord(userData);
                        userRecord.SetValue(0, id);
                        userdetails.Add(userRecord);
                    }
                }

                if (userLogins != null && userLogins.Count > 0)
                {
                    foreach (String userLogin in userLogins)
                    {
                        userRecord = new SqlDataRecord(userData);
                        userRecord.SetValue(1, userLogin);
                        userdetails.Add(userRecord);
                    }
                }

                #endregion Populate table value parameters for User Details

                parameters[0].Value = userdetails;
                parameters[1].Value = includeDisabledUsers;

                storedProcedureName = "usp_AdminManager_SecurityUser_Get_Multiple";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                securityUsers = new SecurityUserCollection();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        SecurityUser securityUser = new SecurityUser();

                        if (reader["Id"] != null)
                        {
                            securityUser.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                        }

                        if (reader["Login"] != null)
                        {
                            securityUser.SecurityUserLogin = reader["Login"].ToString();
                        }

                        if (reader["ManagerId"] != null)
                        {
                            securityUser.ManagerId = ValueTypeHelper.Int32TryParse(reader["ManagerId"].ToString(), -1);
                        }

                        if (reader["ManagerLogin"] != null)
                        {
                            securityUser.ManagerLogin = reader["ManagerLogin"].ToString();
                        }

                        if (reader["smtp"] != null)
                        {
                            securityUser.Smtp = reader["smtp"].ToString();
                        }

                        if (reader["FirstName"] != null)
                        {
                            securityUser.FirstName = securityUser.Name = reader["FirstName"].ToString();
                        }

                        if (reader["LastName"] != null)
                        {
                            securityUser.LastName = securityUser.LongName = reader["LastName"].ToString();
                        }

                        if (reader["Initials"] != null)
                        {
                            securityUser.Initials = reader["Initials"].ToString();
                        }

                        if (reader["AuthenticationType"] != null)
                        {
                            AuthenticationType authenticationType = AuthenticationType.Unknown;
                            Enum.TryParse<AuthenticationType>(reader["AuthenticationType"].ToString(), out authenticationType);

                            securityUser.AuthenticationType = authenticationType;
                        }

                        if (reader["IsSystemCreated"] != null)
                        {
                            securityUser.IsSystemCreatedUser = ValueTypeHelper.BooleanTryParse(reader["IsSystemCreated"].ToString(), false);
                        }

                        if (reader["Disabled"] != null)
                        {
                            securityUser.Disabled = ValueTypeHelper.BooleanTryParse(reader["Disabled"].ToString(), false);
                        }

                        securityUsers.Add(securityUser);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityUsers;
        }

        /// <summary>
        /// Get available Users based on the Role Id.
        /// </summary>
        /// <param name="roleId">Indicates the Role Id</param>
        /// <returns>return a Collection of SecurityUser </returns>
        public SecurityUserCollection Get(String loggedInUserName, Int32 roleId = 0, Int32 userId = 0, SecurityUserType userType = SecurityUserType.Unknown, String userLogin = "")
        {
            SqlDataReader reader = null;
            SecurityUserCollection securityUserCollection = new SecurityUserCollection();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_SecurityUser_GetUsers_ParametersArray");

                parameters[0].Value = roleId;
                parameters[1].Value = userId;
                parameters[2].Value = (Int32)userType;
                parameters[3].Value = userLogin;
                parameters[4].Value = loggedInUserName;

                storedProcedureName = "usp_SecurityManager_SecurityUser_Get";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        SecurityUser user = new SecurityUser();

                        if (reader["SecurityUserId"] != null)
                        {
                            user.Id = ValueTypeHelper.Int32TryParse(reader["SecurityUserId"].ToString(), -1);
                        }

                        if (reader["SecurityRoleId"] != null)
                        {
                            user.SecurityRoleId = ValueTypeHelper.Int32TryParse(reader["SecurityRoleId"].ToString(), -1);
                        }

                        if (reader["SecurityUserTypeId"] != null)
                        {
                            user.SecurityUserTypeID = ValueTypeHelper.Int32TryParse(reader["SecurityUserTypeId"].ToString(), -1);
                        }

                        if (reader["SecurityUserLogin"] != null)
                        {
                            user.SecurityUserLogin = reader["SecurityUserLogin"].ToString();
                        }

                        if (reader["SystemUser"] != null)
                        {
                            user.SystemUser = ValueTypeHelper.BooleanTryParse(reader["SystemUser"].ToString(), false);
                        }

                        if (reader["SMTP"] != null)
                        {
                            user.Smtp = reader["SMTP"].ToString();
                        }

                        if (reader["ExternalEntityId"] != null)
                        {
                            user.ExternalEntityID = ValueTypeHelper.Int32TryParse(reader["ExternalEntityId"].ToString(), -1);
                        }

                        if (reader["ManagerId"] != null)
                        {
                            user.ManagerId = ValueTypeHelper.Int32TryParse(reader["ManagerId"].ToString(), -1);
                        }

                        if (reader["SecurityUserFirstName"] != null)
                        {
                            user.FirstName = user.Name = reader["SecurityUserFirstName"].ToString();
                        }

                        if (reader["SecurityUserLastName"] != null)
                        {
                            user.LastName = user.LongName = reader["SecurityUserLastName"].ToString();
                        }

                        if (reader["ManagerLogin"] != null)
                        {
                            user.ManagerLogin = reader["ManagerLogin"].ToString();
                        }

                        if (reader["SecurityUserInitials"] != null)
                        {
                            user.Initials = reader["SecurityUserInitials"].ToString();
                        }

                        if (reader["SecurityUserDisabled"] != null)
                        {
                            user.Disabled = ValueTypeHelper.BooleanTryParse(reader["SecurityUserDisabled"].ToString(), false);
                        }

                        if (reader["AuthenticationType"] != null)
                        {
                            AuthenticationType authenticationType = AuthenticationType.Unknown;
                            Enum.TryParse<AuthenticationType>(reader["AuthenticationType"].ToString(), out  authenticationType);

                            user.AuthenticationType = authenticationType;
                        }

                        if (reader["AuthenticationToken"] != null)
                        {
                            user.AuthenticationToken = reader["AuthenticationToken"].ToString();
                        }

                        if (reader["IsSystemCreated"] != null)
                        {
                            user.IsSystemCreatedUser = ValueTypeHelper.BooleanTryParse(reader["IsSystemCreated"].ToString(), false);
                        }

                        securityUserCollection.Add(user);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityUserCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <param name="programName"></param>
        /// <param name="userName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult ProcessUser(SecurityUser users, String programName, String userName, CallerContext callerContext)
        {
            MDMTraceHelper.StartTraceActivity("SecurityUserDA.Process", false);

            SqlDataReader reader = null;
            OperationResult userProcessOperationResult = new OperationResult();

            String connectionString = AppConfigurationHelper.ConnectionString;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    List<SqlDataRecord> userTable;
                    List<SqlDataRecord> userRoleTable;
                    SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("AdminManager_SecurityUser_Process_ParametersArray");
                    SqlMetaData[] userMetaData = generator.GetTableValueMetadata("AdminManager_SecurityUser_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] userRoleMetaData = generator.GetTableValueMetadata("AdminManager_SecurityUser_Process_ParametersArray", parameters[1].ParameterName);

                    CreateUserAndRoleTable(users, userMetaData, userRoleMetaData, out userTable, out userRoleTable);


                    parameters[0].Value = userTable;
                    parameters[1].Value = userRoleTable;
                    parameters[2].Value = userName;
                    //parameters[3].Value = programName;

                    const String storedProcedureName = "usp_SecurityManager_User_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    PopulateOperationResult(reader, userProcessOperationResult);

                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    throw new MDMOperationException(exception.Message, exception);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return userProcessOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUsers"></param>
        /// <param name="operationResults"></param>
        /// <param name="programName"></param>
        /// <param name="userName"></param>
        /// <param name="command"></param>
        public void Process(SecurityUserCollection securityUsers, DataModelOperationResultCollection operationResults, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("SecurityUserDA.Process", false);
            }

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");
            const String storedProcedureName = "usp_SecurityManager_User_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                //Here role is single record process in DB. To handle bulk, API need to loop
                foreach (SecurityUser securityUser in securityUsers)
                {
                    if (securityUser.Action == ObjectAction.Read || securityUser.Action == ObjectAction.Ignore)
                        continue;

                    try
                    {
                        OperationResult securityUserOperationResult = (OperationResult)operationResults.GetByReferenceId(securityUser.ReferenceId);

                        List<SqlDataRecord> userTable;
                        List<SqlDataRecord> userRoleTable;

                        SqlParameter[] parameters = generator.GetParameters("AdminManager_SecurityUser_Process_ParametersArray");
                        SqlMetaData[] userMetaData = generator.GetTableValueMetadata("AdminManager_SecurityUser_Process_ParametersArray", parameters[0].ParameterName);
                        SqlMetaData[] userRoleMetaData = generator.GetTableValueMetadata("AdminManager_SecurityUser_Process_ParametersArray", parameters[1].ParameterName);

                        CreateUserAndRoleTable(securityUser, userMetaData, userRoleMetaData, out userTable, out userRoleTable);

                        parameters[0].Value = userTable;
                        parameters[1].Value = userRoleTable;
                        parameters[2].Value = securityUser.UserName;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        PopulateOperationResult(reader, securityUserOperationResult);
                    }
                    catch (Exception exception)
                    {
                        throw new MDMOperationException(exception.Message, exception);
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                    
                }// for all users

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Searches for users with specified LoginId and Email
        /// </summary>
        /// <param name="loginId">Indicates LoginId of the user</param>
        /// <param name="email">Indicates Email of the user</param>
        /// <returns>Collection of users with specified LoginId and Email</returns>
        public SecurityUserCollection SearchUsers(String loginId, String email, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SecurityUserCollection securityUserCollection = new SecurityUserCollection();

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_User_Search_ParametersArray");

                parameters[0].Value = loginId;
                parameters[1].Value = email;

                storedProcedureName = "usp_SecurityManager_User_Search";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        SecurityUser user = new SecurityUser();

                        if (reader["SecurityUserId"] != null)
                        {
                            user.Id = ValueTypeHelper.Int32TryParse(reader["SecurityUserId"].ToString(), -1);
                        }

                        if (reader["SecurityUserLogin"] != null)
                        {
                            user.SecurityUserLogin = reader["SecurityUserLogin"].ToString();
                        }

                        if (reader["ManagerId"] != null)
                        {
                            user.ManagerId = ValueTypeHelper.Int32TryParse(reader["ManagerId"].ToString(), -1);
                        }

                        if (reader["ManagerLogin"] != null)
                        {
                            user.ManagerLogin = reader["ManagerLogin"].ToString();
                        }

                        if (reader["SecurityUserFirstName"] != null)
                        {
                            user.FirstName = user.Name = reader["SecurityUserFirstName"].ToString();
                        }

                        if (reader["SecurityUserLastName"] != null)
                        {
                            user.LastName = user.LongName = reader["SecurityUserLastName"].ToString();
                        }

                        if (reader["SecurityUserTypeId"] != null)
                        {
                            user.SecurityUserTypeID = ValueTypeHelper.Int32TryParse(reader["SecurityUserTypeId"].ToString(), -1);
                        }

                        if (reader["ExternalEntityId"] != null)
                        {
                            user.ExternalEntityID = ValueTypeHelper.Int32TryParse(reader["ExternalEntityId"].ToString(), -1);
                        }

                        if (reader["SystemUser"] != null)
                        {
                            user.SystemUser = ValueTypeHelper.BooleanTryParse(reader["SystemUser"].ToString(), false);
                        }

                        if (reader["SMTP"] != null)
                        {
                            user.Smtp = reader["SMTP"].ToString();
                        }

                        if (reader["SecurityUserInitials"] != null)
                        {
                            user.Initials = reader["SecurityUserInitials"].ToString();
                        }

                        if (reader["SecurityUserDisabled"] != null)
                        {
                            user.Disabled = ValueTypeHelper.BooleanTryParse(reader["SecurityUserDisabled"].ToString(), false);
                        }

                        if (reader["AuthenticationType"] != null)
                        {
                            AuthenticationType authenticationType = AuthenticationType.Unknown;
                            Enum.TryParse<AuthenticationType>(reader["AuthenticationType"].ToString(), out authenticationType);

                            user.AuthenticationType = authenticationType;
                        }

                        if (reader["AuthenticationToken"] != null)
                        {
                            user.AuthenticationToken = reader["AuthenticationToken"].ToString();
                        }

                        if (reader["IsSystemCreated"] != null)
                        {
                            user.IsSystemCreatedUser = ValueTypeHelper.BooleanTryParse(reader["IsSystemCreated"].ToString(), false);
                        }

                        securityUserCollection.Add(user);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityUserCollection;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerCollection"></param>
        /// <param name="containerMetaData"></param>
        /// <param name="attributeMetaData"></param>
        private void CreateUserAndRoleTable(SecurityUser user, SqlMetaData[] userMetaData,
                                         SqlMetaData[] roleMetaData, out List<SqlDataRecord> userTable,
                                        out List<SqlDataRecord> userRoleTable)
        {
            userTable = new List<SqlDataRecord>();
            userRoleTable = new List<SqlDataRecord>();

            SqlDataRecord userRecord = new SqlDataRecord(userMetaData);
            userRecord.SetValue(0, user.Id);
            userRecord.SetValue(1, user.SecurityUserTypeID);
            userRecord.SetValue(2, user.ExternalEntityID);
            userRecord.SetValue(3, user.SecurityUserLogin);
            userRecord.SetValue(4, user.FirstName);
            userRecord.SetValue(5, user.LastName);
            userRecord.SetValue(6, user.Initials);
            userRecord.SetValue(7, user.Disabled);
            userRecord.SetValue(8, user.Smtp);
            userRecord.SetValue(9, user.Password);
            userRecord.SetValue(10, user.Action.ToString());
            userRecord.SetValue(11, String.Empty); //TODO :: Check what is user.Status. This is not available in object right now.
            userRecord.SetValue(12, user.SecurityRoleId);
            userRecord.SetValue(13, user.ManagerId);
            userRecord.SetValue(14, (Int32)user.AuthenticationType);
            userRecord.SetValue(15, -1); //TODO :: check for FK_Locale_UI
            userRecord.SetValue(16, -1); //TODO :: check for FK_Locale_Data
            userRecord.SetValue(17, user.AuthenticationToken);
            userRecord.SetValue(18, user.IsSystemCreatedUser);
            userTable.Add(userRecord);

            if (user.Roles != null && user.Roles.Count > 0)
            {
                foreach (SecurityRole role in user.Roles)
                {
                    SqlDataRecord roleRecord = new SqlDataRecord(roleMetaData);

                    roleRecord.SetValue(0, role.Id);
                    roleRecord.SetValue(1, user.SecurityUserLogin);

                    if (user.Action == ObjectAction.Update || user.Action == ObjectAction.Delete)
                    {
                        roleRecord.SetValue(2, user.Id);
                    }
                    else
                    {
                        roleRecord.SetValue(2, null);  //In the case of create do not know the pK_securityuser. 
                    }

                    userRoleTable.Add(roleRecord);
                }
            }

            if (userTable.Count == 0)
                userTable = null;

            if (userRoleTable.Count == 0)
                userRoleTable = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="userProcessOperationResult"></param>
        private void PopulateOperationResult(SqlDataReader reader, OperationResult userProcessOperationResult)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String userId = String.Empty;

                if (reader["UserId"] != null)
                {
                    userId = reader["UserId"].ToString();
                }
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorCode"] != null)
                    errorCode = reader["ErrorCode"].ToString();

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        userProcessOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                    }
                    userProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    userProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

            } 
        }

        #endregion

        #endregion
    }
}