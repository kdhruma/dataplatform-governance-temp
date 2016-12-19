using MDM.UomManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MDM.BusinessObjects;
using System.Collections;
using MDM.AdminManager.Business;
using System.Web;
using MDM.CacheManager.Business;

namespace MDM.UomManager.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for UomBLTest and is intended
    ///to contain all UomBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UomBLTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetUom
        ///</summary>
        [TestMethod()]
        public void GetUomTest()
        {
           // LoadSecurityPrincipal("cfadmin");
            UomBL target = new UomBL(); // TODO: Initialize to an appropriate value
            UomContext uomContext = new UomContext();
            uomContext.UomId = 6;
            CallerContext callerContext = new CallerContext(); // TODO: Initialize to an appropriate value
            UOM expected = null; // TODO: Initialize to an appropriate value
            UOM actual;
            actual = target.GetUom(uomContext, callerContext);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        private static void LoadSecurityPrincipal(String userName)
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
            SecurityPrincipal currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, Core.MDMCenterSystem.Web);

            ICache cacheMananger = CacheFactory.GetCache();
            String secPrincipalCacheKeyForSystemUser = "SecurityPrincipal_cfadmin_WithoutTimeStamp";
            cacheMananger.Set(secPrincipalCacheKeyForSystemUser, currentUserSecurityPrincipal, DateTime.MaxValue);

            int userTimeOut = 50;
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
