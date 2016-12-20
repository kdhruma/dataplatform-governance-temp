using System;
using System.Collections;

namespace MDM.DataModelManager.UnitTest.Utility
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;

    public class CommonUtil
    {
        /// <summary>
        /// Load security principal for user
        /// </summary>
        /// <param name="userName">User name for which security principal is to be loaded.</param>
        public static void LoadSecurityPrincipal(String userName)
        {
            Int32 systemId = 0;

            //the stamp at the time of loging in
            String timeStamp = DateTime.Now.ToString();

            //The cache key to for the user principal to be stored
            string securityPrincipalCacheKey = "SecurityPrincipal_" + userName.ToLower();

            Hashtable loginData = new Hashtable();
            loginData.Add("SystemId", systemId);
            loginData.Add("TimeStamp", timeStamp);

            //obtain the security principal
            SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
            SecurityPrincipal currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, MDM.Core.MDMCenterSystem.Web);

            if (ServiceUserContext.Current == null)
            {
                ServiceUserContext.Initialize(currentUserSecurityPrincipal);
            }
            else
            {
                ServiceUserContext.Current.SecurityPrincipal = currentUserSecurityPrincipal;
            }
        }

     
    }
}
