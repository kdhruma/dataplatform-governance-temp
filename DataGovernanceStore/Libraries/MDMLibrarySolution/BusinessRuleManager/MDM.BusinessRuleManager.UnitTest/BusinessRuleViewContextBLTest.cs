using MDM.BusinessRuleManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.BusinessRuleManager.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for BusinessRuleViewContextBLTest and is intended
    ///to contain all BusinessRuleViewContextBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessRuleViewContextBLTest
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
        ///A test for ProcessContext
        ///</summary>
        //[TestMethod()]
        //public void ProcessContextTest()
        //{
        //    BusinessRuleViewContextBL target = new BusinessRuleViewContextBL(); // TODO: Initialize to an appropriate value
        //    Collection<BusinessRuleSetRule> businessRuleSetRules = null; // TODO: Initialize to an appropriate value
        //    string loginUser = string.Empty; // TODO: Initialize to an appropriate value
        //    string programName = string.Empty; // TODO: Initialize to an appropriate value
        //    string action = string.Empty; // TODO: Initialize to an appropriate value
        //    target.ProcessContext(businessRuleSetRules, loginUser, programName, action);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}
    }
}
