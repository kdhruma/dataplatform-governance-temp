using System;
using System.Collections.ObjectModel;

namespace MDM.AttributeModelManager.UnitTest
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.AttributeModelManager.Business;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MDM.BusinessObjects;
    
    /// <summary>
    ///This is a test class for AttributeMetaModelBLTest and is intended
    ///to contain all AttributeMetaModelBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttributeModelOperationsBLTest
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
        ///A test for GetAllCommonAttributes
        ///</summary>
        //[TestMethod()]
        //public void GetAllCommonAttributesTest()
        //{
        //    AttributeModelOperationsBL target = new AttributeModelOperationsBL(); // TODO: Initialize to an appropriate value
        //    int localeId = 0; // TODO: Initialize to an appropriate value
        //    //Collection<AttributeModel> expected = null; // TODO: Initialize to an appropriate value
        //    Collection<AttributeModel> actual;
        //    actual = target.GetAllCommonAttributes(localeId);
        //    Assert.AreEqual(383, actual.Count);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetAllTechnicalAttributes
        /////</summary>
        //[TestMethod()]
        //public void GetAllTechnicalAttributesTest()
        //{
        //    AttributeModelOperationsBL target = new AttributeModelOperationsBL(); // TODO: Initialize to an appropriate value
        //    int localeId = 0; // TODO: Initialize to an appropriate value
        //    //Collection<AttributeModel> expected = null; // TODO: Initialize to an appropriate value
        //    Collection<AttributeModel> actual;
        //    actual = target.GetAllTechnicalAttributes(localeId);
        //    Assert.AreEqual(1429, actual.Count);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetByAttributeGroup
        /////</summary>
        //[TestMethod()]
        //public void GetByAttributeGroupTest()
        //{
        //    AttributeModelOperationsBL target = new AttributeModelOperationsBL(); // TODO: Initialize to an appropriate value
        //    int attributeGroupId = 0; // TODO: Initialize to an appropriate value
        //    attributeGroupId = 8263;

        //    //Collection<AttributeModel> expected = null; // TODO: Initialize to an appropriate value
        //    Collection<AttributeModel> actual;
        //    Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
        //    locales.Add(GlobalizationHelper.GetSystemDataLocale());

        //    actual = target.GetByAttributeGroup(attributeGroupId,locales);
        //    Assert.AreEqual(1, actual.Count);
        //   // Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetCommonAttributesByContainerAndEntityType
        /////</summary>
        //[TestMethod()]
        //public void GetCommonAttributesByContainerAndEntityTypeTest()
        //{
        //    AttributeModelOperationsBL target = new AttributeModelOperationsBL(); // TODO: Initialize to an appropriate value
        //    Int32 catalogId = 0; // TODO: Initialize to an appropriate value
        //    Int32 entityTypeId = 0; // TODO: Initialize to an appropriate value

        //    catalogId = 1199;
        //    entityTypeId = 7;
        //    Int32 localeId = (Int32) GlobalizationHelper.GetSystemDataLocale();
        //    //Collection<AttributeModel> expected = null; // TODO: Initialize to an appropriate value
        //    Collection<AttributeModel> actual;
        //    actual = target.GetCommonAttributesByContainerAndEntityType(catalogId, entityTypeId,localeId);
        //    Assert.AreEqual(76, actual.Count);
           
        //}

        ///// <summary>
        /////A test for GetTechAttributesByTaxonomyAndCategory
        /////</summary>
        //[TestMethod()]
        //public void GetTechAttributesByTaxonomyAndCategoryTest()
        //{
        //    AttributeModelOperationsBL target = new AttributeModelOperationsBL(); // TODO: Initialize to an appropriate value
        //    int iCategoryID = 0; // TODO: Initialize to an appropriate value
        //    int iTaxonomyID = 0; // TODO: Initialize to an appropriate value
        //    int iLocaleID = 0; // TODO: Initialize to an appropriate value

        //    iTaxonomyID = 13;
        //    iCategoryID = 11; 
        //    //Collection<AttributeModel> expected = null; // TODO: Initialize to an appropriate value
        //    Collection<AttributeModel> actual;
        //    actual = target.GetTechAttributesByTaxonomyAndCategory(iCategoryID, iTaxonomyID, iLocaleID);
        //    Assert.AreEqual(18, actual.Count);
            
        //}
    }
}
