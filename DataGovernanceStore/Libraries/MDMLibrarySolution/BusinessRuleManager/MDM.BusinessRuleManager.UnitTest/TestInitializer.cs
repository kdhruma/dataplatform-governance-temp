using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MDM.BusinessObjects;
using MDM.AdminManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.DataModelManager.UnitTest
{
    [TestClass()]
    public class TestInitializer
    {
        [AssemblyInitialize()]
        public static void AssemblyInitialize(TestContext context)
        {
            LoadSecurityPrincipal("cfadmin");
        }

        private static void LoadSecurityPrincipal(String userName)
        {
            Int32 systemId = 0;
            Int32 userTimeOut = 50;

            //the stamp at the time of loging in
            String timeStamp = DateTime.Now.ToString();

            //The cache key to for the user principal to be stored
            string securityPrincipalCacheKey = "SecurityPrincipal_" + userName.ToLower();

            Hashtable loginData = new Hashtable();
            loginData.Add("SystemId", systemId);
            loginData.Add("TimeStamp", timeStamp);

            //obtain the security principal
            SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
            SecurityPrincipal currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, Core.MDMCenterSystem.Web);

            if (HttpContext.Current != null)
            {
                //Remove cache 
                if (HttpContext.Current.Cache[securityPrincipalCacheKey] != null)
                    HttpContext.Current.Cache.Remove(securityPrincipalCacheKey);

                //TODO:: Eventually migrate all the Cache objects into CacheManager
                HttpContext.Current.Cache.Insert(securityPrincipalCacheKey,
                                        currentUserSecurityPrincipal, null,
                                        System.Web.Caching.Cache.NoAbsoluteExpiration,
                                        TimeSpan.FromMinutes(userTimeOut));

                if (currentUserSecurityPrincipal != null && currentUserSecurityPrincipal.UserIdentity.Name == userName)
                {
                    HttpContext.Current.User = currentUserSecurityPrincipal;
                }
            }
        }
    }
}
