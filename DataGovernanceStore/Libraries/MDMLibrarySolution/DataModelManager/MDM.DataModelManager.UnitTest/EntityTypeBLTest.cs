using MDM.DataModelManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.EntityManager.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for EntityTypeBLTest and is intended
    ///to contain all EntityTypeBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EntityTypeBLTest
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
        ///A test for GetChildEntityTypes
        ///</summary>
        //[TestMethod()]
        //public void GetChildEntityTypesTest()
        //{
        //    EntityTypeBL target = new EntityTypeBL(); // TODO: Initialize to an appropriate value
        //    int parentEntityTypeId = 0; // TODO: Initialize to an appropriate value

        //    parentEntityTypeId = 7;
        //    //Collection<EntityType> expected = null; // TODO: Initialize to an appropriate value
        //    Collection<EntityType> actual;
        //    actual = target.GetChildEntityTypes(parentEntityTypeId);
        //    Assert.AreEqual(1, actual.Count);
        //   // Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
