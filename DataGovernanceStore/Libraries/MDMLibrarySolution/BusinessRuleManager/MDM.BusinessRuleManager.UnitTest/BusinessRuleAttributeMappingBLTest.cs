using MDM.BusinessRuleManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.BusinessRuleManager.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for BusinessRuleAttributeMappingBLTest and is intended
    ///to contain all BusinessRuleAttributeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessRuleAttributeMappingBLTest
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
        ///A test for Process
        ///</summary>
        //[TestMethod()]
        //public void ProcessTest()
        //{
        //    BusinessRuleAttributeMappingBL target = new BusinessRuleAttributeMappingBL(); // TODO: Initialize to an appropriate value
        //    Collection<BusinessRuleAttributeMapping> businessRuleAttributeMappings = null; // TODO: Initialize to an appropriate value
            
        //    target.Process(businessRuleAttributeMappings,"1234");
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for GetByBusinessRuleID
        /////</summary>
        //[TestMethod()]
        //public void GetByBusinessRuleIDTest()
        //{
        //    BusinessRuleAttributeMappingBL target = new BusinessRuleAttributeMappingBL(); // TODO: Initialize to an appropriate value
        //    int businessRuleID = 0; // TODO: Initialize to an appropriate value
        //    Collection<BusinessRuleAttributeMapping> expected = null; // TODO: Initialize to an appropriate value
        //    Collection<BusinessRuleAttributeMapping> actual;
        //    actual = target.GetByBusinessRuleID(businessRuleID);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
