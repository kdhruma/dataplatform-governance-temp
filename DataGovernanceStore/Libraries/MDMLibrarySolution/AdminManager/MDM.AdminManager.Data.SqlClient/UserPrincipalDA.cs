using System;
using System.Xml;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.Utility;

namespace MDM.AdminManager.Data
{
    public class UserPrincipalDA : SqlClientDataAccessBase
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
        /// <param name="vchrUserLogin"></param>
        /// <param name="vchrPassword"></param>
        /// <returns>Returns true if user is authenticated</returns>
        public Boolean AuthenticateUser(String vchrUserLogin, String vchrPassword)
        {
            Int32 userId = 0;
            Boolean isAuthenticated = false;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

            parameters = generator.GetParameters("AdminManager_UserPrincipal_AuthenticateUser_ParametersArray");

            parameters[0].Value = vchrUserLogin;
            parameters[1].Value = vchrPassword;

            storedProcedureName = "usp_Sec_Admin_authenticateUser";

            if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

            Object objUserId = ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);

            if (objUserId != null)
                Int32.TryParse(objUserId.ToString(), out userId);

            if (userId > 0)
                isAuthenticated = true;

            return isAuthenticated;
        }

        #endregion
    }
}
