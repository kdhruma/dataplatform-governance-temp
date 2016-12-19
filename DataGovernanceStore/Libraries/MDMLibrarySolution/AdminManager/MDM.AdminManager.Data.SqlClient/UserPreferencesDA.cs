using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Diagnostics;

namespace MDM.AdminManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;

    public class UserPreferencesDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <returns></returns>
        public UserPreferences GetUserPreferences(String vchrTargetUserLogin, String vchrUserLogin)
        {
            UserPreferences userPreferences = new UserPreferences();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_UserPreferences_GetUserPreferences_ParametersArray");

                parameters[0].Value = vchrTargetUserLogin;
                parameters[1].Value = vchrUserLogin;

                storedProcedureName = "usp_SecurityManager_UserPreferences_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);
                
                //userPreferencesDataXML.Append(reader[0]);
                userPreferences = PopulateUserPreferences(reader,vchrUserLogin);                    
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return userPreferences;
        }

        /// <summary>
        /// Process User Preferences
        /// </summary>
        /// <param name="loginUser">Name of the Login User</param>
        /// <param name="userPreferences">UserPrefereces Object to Process</param>
        /// <param name="command">Contains Connection Properties</param>
        /// <returns>True if Process is successful else false.</returns>
        public Boolean ProcessUserPreferences(String loginUser, UserPreferences userPreferences, DBCommandProperties command)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {                
                SqlParameter[] parameters;                
                String storedProcedureName = String.Empty;
                String connectionString = String.Empty;                
                String userPreferencesXml = userPreferences.ToXml();
                connectionString = AppConfigurationHelper.ConnectionString;
                SqlDataReader reader = null;
                OperationResult userPreferencesOperationResult = new OperationResult();

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                    parameters = generator.GetParameters("AdminManager_UserPreferences_Process_ParametersArray");

                    parameters[0].Value = userPreferencesXml;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = loginUser;

                    storedProcedureName = "usp_SecurityManager_UserPreferences_Process";

                    reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                    #region Update OperationResult

                    PopulateOperationResult(reader, userPreferences, userPreferencesOperationResult);

                    #endregion

                    if (userPreferencesOperationResult.OperationResultStatus == OperationResultStatusEnum.None ||
                        userPreferencesOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "User preferences Process Failed." + exception.Message);
                    userPreferencesOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    transactionScope.Complete();
                }
            }

            return false;
        }

        /// <summary>
        /// Process User Preferences
        /// </summary>
        /// <param name="userPreferencesCollection">user Preferences collection to process</param>
        /// <param name="operationResults">operation result of the user preferences</param>
        /// <param name="command">Contains Connection Properties</param>
        /// <returns>True if Process is successful else false.</returns>
        public void ProcessUserPreferences(UserPreferencesCollection userPreferencesCollection, DataModelOperationResultCollection operationResults, DBCommandProperties command)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;
                String connectionString = String.Empty;
                String userPreferencesXml = String.Empty;
                String loginUser = String.Empty;
                connectionString = AppConfigurationHelper.ConnectionString;
                SqlDataReader reader = null;

                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                foreach(UserPreferences userPreferences in userPreferencesCollection)
                {
                    if (userPreferences.Action == ObjectAction.Read || userPreferences.Action == ObjectAction.Ignore)
                        continue;

                    OperationResult userPreferencesOperationResult = (OperationResult)operationResults.GetByReferenceId(userPreferences.ReferenceId);

                    try
                    {
                        userPreferencesXml = userPreferences.ToXml();
                        loginUser = userPreferences.LoginName;

                        parameters = generator.GetParameters("AdminManager_UserPreferences_Process_ParametersArray");

                        parameters[0].Value = userPreferencesXml;
                        parameters[1].Value = loginUser;
                        parameters[2].Value = loginUser;

                        storedProcedureName = "usp_SecurityManager_UserPreferences_Process";

                        //Int32 returnValue = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);
                        reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                        #region Update OperationResult

                        PopulateOperationResult(reader, userPreferences, userPreferencesOperationResult);

                        #endregion
                    }
                    
                    catch (Exception exception)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "User preferences Process Failed." + exception.Message);
                        userPreferencesOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                    }

                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }

                transactionScope.Complete();
            }
        }

          #region Private Methods

        private void PopulateOperationResult(SqlDataReader reader, UserPreferences userPreferences, OperationResult userPreferencesProcessOperationResult)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String userPreferencesId = String.Empty;

                if (reader["Id"] != null)
                {
                    userPreferencesId = reader["Id"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorMessage"] != null)
                {
                    errorCode = reader["ErrorMessage"].ToString();
                }

                if (hasError & !String.IsNullOrEmpty(errorCode))
                {
                    userPreferencesProcessOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    userPreferencesProcessOperationResult.AddOperationResult("", "User Preferences ID: " + userPreferencesId, OperationResultType.Information);
                    userPreferences.Id = ValueTypeHelper.Int32TryParse(userPreferencesId, -1);
                    userPreferencesProcessOperationResult.ReturnValues.Add(userPreferencesId);
                }
            }
        }

          /// <summary>
        /// Populate UserPreferences Object from Reader.
        /// </summary>
        /// <param name="reader">reader</param>
        /// <param name="loginName">Name of Login User</param>
        /// <returns>UserPreferences Object with  all Properties populated</returns>
        private UserPreferences PopulateUserPreferences(SqlDataReader reader, String loginName)
        {
            UserPreferences userPreferences = new UserPreferences();

            while (reader.Read())
            {
                #region Populate Properties

                if (reader["PK_Security_UserPreferences"] != null)
                    userPreferences.Id = ValueTypeHelper.Int32TryParse(reader["PK_Security_UserPreferences"].ToString(), userPreferences.Id);
                
                if(reader["PK_Security_User"] != null)
                    userPreferences.LoginId = ValueTypeHelper.Int32TryParse(reader["PK_Security_User"].ToString(), userPreferences.LoginId); 

                if (reader["UserType"] != null)
                    userPreferences.UserType = reader["UserType"].ToString();

                if (reader["UserInitials"] != null)
                    userPreferences.UserInitials = reader["UserInitials"].ToString();

                if (reader["FK_Security_Role"] != null)
                    userPreferences.DefaultRoleId = ValueTypeHelper.Int32TryParse(reader["FK_Security_Role"].ToString(), userPreferences.DefaultRoleId);

                if (reader["DefaultRole"] != null)
                    userPreferences.DefaultRoleName = reader["DefaultRole"].ToString();

                if (reader["FK_Org"] != null)
                    userPreferences.DefaultOrgId = ValueTypeHelper.Int32TryParse(reader["FK_Org"].ToString(), userPreferences.DefaultOrgId);

                if (reader["OrgShortName"] != null)
                    userPreferences.DefaultOrgName = reader["OrgShortName"].ToString();

                if (reader["OrgLongName"] != null)
                    userPreferences.DefaultOrgLongName = reader["OrgLongName"].ToString();

                if (reader["DefaultCatalogId"] != null)
                    userPreferences.DefaultContainerId = ValueTypeHelper.Int32TryParse(reader["DefaultCatalogId"].ToString(), userPreferences.DefaultContainerId);

                if (reader["CatalogShortName"] != null)
                    userPreferences.DefaultContainerName = reader["CatalogShortName"].ToString();

                if (reader["CatalogLongName"] != null)
                    userPreferences.DefaultContainerLongName = reader["CatalogLongName"].ToString();

                if (reader["DefaultHierarchyId"] != null)
                    userPreferences.DefaultHierarchyId = ValueTypeHelper.Int32TryParse(reader["DefaultHierarchyId"].ToString(), userPreferences.DefaultHierarchyId);

                if (reader["HierarchyShortName"] != null)
                    userPreferences.DefaultHierarchyName = reader["HierarchyShortName"].ToString();

                if (reader["HierarchyLongName"] != null)
                    userPreferences.DefaultHierarchyLongName = reader["HierarchyLongName"].ToString();

                if (reader["DefaultViewId"] != null)
                    userPreferences.DefaultViewId = ValueTypeHelper.Int32TryParse(reader["DefaultViewId"].ToString(), userPreferences.DefaultViewId);

                if (reader["ExternalEntityID"] != null)
                    userPreferences.ExternalEntityId = ValueTypeHelper.Int32TryParse(reader["ExternalEntityID"].ToString(), userPreferences.ExternalEntityId);

                if (reader["MaxTableRows"] != null)
                    userPreferences.MaxTableRows = ValueTypeHelper.Int32TryParse(reader["MaxTableRows"].ToString(), userPreferences.MaxTableRows);

                if (reader["MaxTablePages"] != null)
                    userPreferences.MaxTablePages = ValueTypeHelper.Int32TryParse(reader["MaxTablePages"].ToString(), userPreferences.MaxTablePages);

                if (reader["FK_TimeZone"] != null)
                    userPreferences.DefaultTimeZoneId = ValueTypeHelper.Int32TryParse(reader["FK_TimeZone"].ToString(), userPreferences.DefaultTimeZoneId);

                if (reader["TimeZoneShortName"] != null)
                    userPreferences.DefaultTimeZoneShortName = reader["TimeZoneShortName"].ToString();

                if (reader["LocaleUI"] != null)
                {
                    String strUILocale = reader["LocaleUI"].ToString();
                    LocaleEnum uiLocale = LocaleEnum.UnKnown;

                    if (!String.IsNullOrWhiteSpace(strUILocale))
                        Enum.TryParse<LocaleEnum>(strUILocale, out uiLocale);                  

                    userPreferences.UILocale = uiLocale;
                }

                if (reader["LocaleData"] != null)
                {
                    String strDataLocale = reader["LocaleData"].ToString();
                    LocaleEnum dataLocale = LocaleEnum.UnKnown;

                    if (!String.IsNullOrWhiteSpace(strDataLocale))
                        Enum.TryParse<LocaleEnum>(strDataLocale, out dataLocale);   

                    userPreferences.DataLocale = dataLocale;
                }

                if (reader["UICultureName"] != null)
                    userPreferences.UICultureName = reader["UICultureName"].ToString();

                if (reader["DataCultureName"] != null)
                    userPreferences.DataCultureName = reader["DataCultureName"].ToString();

                userPreferences.LoginName = loginName;
                userPreferences.Action = ObjectAction.Read;

                #endregion                
            }

            return userPreferences;
        }

        #endregion Private Methods

      
        #endregion 
    }
}
