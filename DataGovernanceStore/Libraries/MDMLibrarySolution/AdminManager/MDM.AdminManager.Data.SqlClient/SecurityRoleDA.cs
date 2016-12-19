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
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Specifies data access operations for security role
    /// </summary>
    public class SecurityRoleDA : SqlClientDataAccessBase
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
        /// <param name="getPermissionSetOnly"></param>
        /// <param name="countFrom"></param>
        /// <param name="countTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <param name="displaySystemRole"></param>
        /// <returns></returns>
        public Collection<SecurityRole> GetAllRoles(Int32 userType, Char getPermissionSetOnly, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, String userLogin, Boolean displaySystemRole)
        {
            SqlDataReader reader = null;
            StringBuilder returnValue = new StringBuilder();
            SecurityRole securityRole = new SecurityRole();
            Collection<SecurityRole> securityRoleCollection = new Collection<SecurityRole>();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_getRoles_XML_ParametersArray");

                parameters[0].Value = 0;
                parameters[1].Value = userType;
                parameters[2].Value = getPermissionSetOnly;
                parameters[3].Value = countFrom;
                parameters[4].Value = countTo;
                parameters[5].Value = sortColumn;
                parameters[6].Value = searchColumn;
                parameters[7].Value = searchParameter;
                parameters[8].Value = userLogin;
                parameters[9].Value = displaySystemRole;

                storedProcedureName = "usp_Sec_Admin_getRoles_XML";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    returnValue.Append(reader.GetString(0));
                }

                XmlDocument objXML = new XmlDocument();
                objXML.LoadXml(returnValue.ToString());

                XmlNodeList roleList = objXML.SelectNodes("/Roles/Role");

                if (roleList.Count > 0)
                {
                    foreach (XmlNode role in roleList)
                    {
                        securityRole = new SecurityRole(role.OuterXml);
                        securityRoleCollection.Add(securityRole);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityRoleCollection;
        }

        /// <summary>
        /// Get available Roles based on the Role Id.
        /// </summary>
        /// <param name="roleId">Indicates the Role Id</param>
        /// <returns>return a Collection of SecurityRole </returns>
        public SecurityRoleCollection Get(String loggedInUserName, Int32 roleId = 0, SecurityUserType userType = SecurityUserType.Unknown, String roleShortName = "",  Boolean returnPrivateRole = false, Boolean permissionSet = false)
        {
            SqlDataReader reader = null;
            SecurityRoleCollection securityRoleCollection = new SecurityRoleCollection();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_SecurityRole_GetRoles_ParametersArray");

                parameters[0].Value = roleId;
                parameters[1].Value = ( Int32 )userType;
                parameters[2].Value = roleShortName;
                parameters[3].Value = ValueTypeHelper.ConvertBooleanToString(permissionSet);
                parameters[4].Value = ValueTypeHelper.ConvertBooleanToString(returnPrivateRole);
                parameters[5].Value = loggedInUserName;

                storedProcedureName = "usp_SecurityManager_SecurityRole_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        #region Local variables

                        Int32 securityRoleId = 0;
                        SecurityUserType securityUserType = SecurityUserType.Unknown;
                        String shortName = String.Empty;
                        String longName = String.Empty;
                        Boolean isSystemRole = false;
                        Boolean isPrivateRole = false;
                        Boolean isPermissionSet = false;

                        #endregion Local variables

                        #region Read values

                        if (reader["PK_Security_Role"] != null)
                            securityRoleId = ValueTypeHelper.Int32TryParse(reader["PK_Security_Role"].ToString(), -1);

                        if (reader["FK_Security_UserType"] != null)
                        {
                            Int32 type = ValueTypeHelper.Int32TryParse(reader["FK_Security_UserType"].ToString(), 1);
                            securityUserType = ( SecurityUserType )type;
                        }

                        if (reader["ShortName"] != null)
                            shortName = reader["ShortName"].ToString();

                        if (reader["LongName"] != null)
                            longName = reader["LongName"].ToString();

                        if (reader["SystemRole"] != null)
                            isSystemRole = ValueTypeHelper.ConvertToBooleanFromShortString(reader["SystemRole"].ToString());

                        if (reader["PermissionSet"] != null)
                            isPermissionSet = ValueTypeHelper.ConvertToBooleanFromShortString(reader["PermissionSet"].ToString());

                        if (reader["PrivateRole"] != null)
                            isPrivateRole = ValueTypeHelper.ConvertToBooleanFromShortString(reader["PrivateRole"].ToString());

                        #endregion Read values

                        #region Create object

                        SecurityRole role = new SecurityRole();
                        role.Id = securityRoleId;
                        role.SecurityUserType = securityUserType;
                        role.Name = shortName;
                        role.LongName = longName;
                        role.IsSystemRole = isSystemRole;
                        role.IsPrivateRole = isPrivateRole;
                        role.PermissionSet = isPermissionSet;

                        securityRoleCollection.Add(role);

                        #endregion Create object
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityRoleCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public SecurityRoleCollection GetUserRoles(Int32 userID, String userLoginName)
        {
            SecurityRole securityRole = new SecurityRole();
            SecurityRoleCollection securityRoles = new SecurityRoleCollection();
            StringBuilder roles = new StringBuilder();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_getUsersWithRoles_XML_ParametersArray");

                parameters[0].Value = userID;
                parameters[1].Value = 0;
                parameters[2].Value = 0;
                parameters[3].Value = "";
                parameters[4].Value = "";
                parameters[5].Value = userLoginName;

                storedProcedureName = "usp_Sec_Admin_getUsersWithRoles_XML";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        roles.Append(reader.GetString(0));
                    }
                }

                XmlDocument objXML = new XmlDocument();
                objXML.LoadXml(roles.ToString());

                XmlNodeList roleList = objXML.SelectNodes("/Users/User/Roles/Role");

                if (roleList != null && roleList.Count > 0)
                {
                    foreach (XmlNode role in roleList)
                    {
                        securityRole = new SecurityRole(role.OuterXml);
                        securityRoles.Add(securityRole);
                    }
                }
            }
            finally
            {
                // close the connection.
                if (reader != null)
                    reader.Close();
            }

            return securityRoles;
        }

        /// <summary>
        /// Process (Create/Update/Delete) given roles.
        /// </summary>
        /// <param name="role">Roles to be processed</param>
        /// <param name="programName">Name of program which made change</param>
        /// <param name="userName">Name of user who is performing operation</param>
        /// <param name="callerContext">Context indicating who trigger the change</param>
        /// <returns>OperationResult status of current process</returns>
        public OperationResult ProcessRole(SecurityRole role, String programName, String userName, CallerContext callerContext)
        {
            MDMTraceHelper.StartTraceActivity("SecurityRoleDA.Process", false);

            SqlDataReader reader = null;
            OperationResult roleProcessOperationResult = new OperationResult();

            String connectionString = AppConfigurationHelper.ConnectionString;
            const String storedProcedureName = "usp_SecurityManager_SecurityRole_Process";

            //Here role is single record process in DB. To handle bulk, API need to loop
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    List<SqlDataRecord> userTable;
                    SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("AdminManager_SecurityRole_Process_ParametersArray");
                    SqlMetaData[] userMetaData = generator.GetTableValueMetadata("AdminManager_SecurityRole_Process_ParametersArray", parameters[6].ParameterName);

                    CreateUsersTable(role, userMetaData, out userTable);

                    parameters[0].Value = role.Id;
                    parameters[1].Value = (Int32)role.SecurityUserType;
                    parameters[2].Value = role.Name;
                    parameters[3].Value = role.LongName;
                    parameters[4].Value = ValueTypeHelper.ConvertBooleanToString(role.PermissionSet);
                    parameters[5].Value = ValueTypeHelper.ConvertBooleanToString(role.IsPrivateRole);
                    parameters[6].Value = userTable;
                    parameters[7].Value = role.Action.ToString();
                    parameters[8].Value = userName;
                    parameters[9].Value = programName;
                    parameters[10].Value = true;

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    PopulateOperationResult(reader,role, roleProcessOperationResult);

                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Role Process Failed." + exception.Message);
                    roleProcessOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

            }

            return roleProcessOperationResult;
        }

        /// <summary>
        /// Process (Create/Update/Delete) given roles.
        /// </summary>
        /// <param name="securityRoles">Roles to be processed</param>
        /// <param name="operationResults">OperationResult status of current process</param>
        /// <param name="programName">Name of program which made change</param>
        /// <param name="userName">Name of user who is performing operation</param>
        /// <param name="command">DB command</param>
        public void Process(SecurityRoleCollection securityRoles, DataModelOperationResultCollection operationResults, String programName, String userName,DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("SecurityRoleDA.Process", false);

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");
            const String storedProcedureName = "usp_SecurityManager_SecurityRole_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                //Here role is single record process in DB. To handle bulk, API need to loop
                foreach (SecurityRole securityRole in securityRoles)
                {
                    if (securityRole.Action == ObjectAction.Read || securityRole.Action == ObjectAction.Ignore)
                        continue;

                    try
                    {
                        OperationResult securityRoleOperationResult = (OperationResult)operationResults.GetByReferenceId(securityRole.ReferenceId);

                        List<SqlDataRecord> userTable;
                        SqlParameter[] parameters = generator.GetParameters("AdminManager_SecurityRole_Process_ParametersArray");
                        SqlMetaData[] userMetaData = generator.GetTableValueMetadata("AdminManager_SecurityRole_Process_ParametersArray", parameters[6].ParameterName);

                        CreateUsersTable(securityRole, userMetaData, out userTable);

                        parameters[0].Value = securityRole.Id;
                        parameters[1].Value = (Int32)securityRole.SecurityUserType;
                        parameters[2].Value = securityRole.Name;
                        parameters[3].Value = securityRole.LongName;
                        parameters[4].Value = ValueTypeHelper.ConvertBooleanToString(securityRole.PermissionSet);
                        parameters[5].Value = ValueTypeHelper.ConvertBooleanToString(securityRole.IsPrivateRole);
                        parameters[6].Value = userTable;
                        parameters[7].Value = securityRole.Action.ToString();
                        parameters[8].Value = securityRole.UserName;
                        parameters[9].Value = programName;
                        parameters[10].Value = true;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        PopulateOperationResult(reader, securityRole, securityRoleOperationResult);

                        if (operationResults.OperationResultStatus != OperationResultStatusEnum.Failed || operationResults.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                        {
                            operationResults.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                    }
                    catch (Exception exception)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Role Process Failed." + exception.Message);
                        operationResults.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                } // for all security roles.

                transactionScope.Complete();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="role"></param>
        /// <param name="roleProcessOperationResult"></param>
        private void PopulateOperationResult(SqlDataReader reader, SecurityRole role, OperationResult roleProcessOperationResult)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String roleId = String.Empty;

                if (reader["SecurityRoleID"] != null)
                {
                    roleId = reader["SecurityRoleID"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError & !String.IsNullOrEmpty(errorCode))
                {
                    roleProcessOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    role.Id = ValueTypeHelper.Int32TryParse(roleId, -1);
                    roleProcessOperationResult.Id = role.Id;
                    roleProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    roleProcessOperationResult.ReferenceId = !String.IsNullOrWhiteSpace(role.ReferenceId) ? role.ReferenceId : role.Name;
                }

            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerCollection"></param>
        /// <param name="containerMetaData"></param>
        /// <param name="attributeMetaData"></param>
        private void CreateUsersTable(SecurityRole role, SqlMetaData[] userMetaData, out List<SqlDataRecord> userTable)
        {
            userTable = new List<SqlDataRecord>();

            foreach (SecurityUser user in role.Users)
            {
                SqlDataRecord userRecord = new SqlDataRecord(userMetaData);
                userRecord.SetValue(0, user.Id);

                userTable.Add(userRecord);
            }

            if (userTable.Count == 0)
                userTable = null;

        }

        #endregion

        #endregion
    }
}