using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.AttributeModelManager.UnitTest
{
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.Core;

    /// <summary>
    ///This is a test class for AttributeGroupBLTest and is intended
    ///to contain all AttributeGroupBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttributeGroupBLTest
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
        ///A test for GetXmlAttributeStringValue
        ///</summary>
//        [TestMethod()]
//        [DeploymentItem("RS.MDM.AttributeModelManager.Business.dll")]
//        public void GetXmlAttributeStringValueTest()
//        {
//            XmlNode xmlNode = null; // TODO: Initialize to an appropriate value
//            string attributeName = string.Empty; // TODO: Initialize to an appropriate value
//            string expected = string.Empty; // TODO: Initialize to an appropriate value
//            string actual;
//            actual = XmlHelper.GetXmlAttributeStringValue(xmlNode, attributeName);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for GetXmlAttributeIntegerValue
//        ///</summary>
//        [TestMethod()]
//        [DeploymentItem("RS.MDM.AttributeModelManager.Business.dll")]
//        public void GetXmlAttributeIntegerValueTest()
//        {
//            XmlNode xmlNode = null; // TODO: Initialize to an appropriate value
//            string attributeName = string.Empty; // TODO: Initialize to an appropriate value
//            int expected = 0; // TODO: Initialize to an appropriate value
//            int actual;
//            actual = XmlHelper.GetXmlAttributeIntegerValue(xmlNode, attributeName);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for GetXmlAttributeBooleanValue
//        ///</summary>
//        [TestMethod()]
//        [DeploymentItem("RS.MDM.AttributeModelManager.Business.dll")]
//        public void GetXmlAttributeBooleanValueTest()
//        {
//            XmlNode xmlNode = null; // TODO: Initialize to an appropriate value
//            string attributeName = string.Empty; // TODO: Initialize to an appropriate value
//            bool expected = false; // TODO: Initialize to an appropriate value
//            bool actual;
//            actual = XmlHelper.GetXmlAttributeBooleanValue(xmlNode, attributeName);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for GetTechnicalAttributeGroups
//        ///</summary>
//        [TestMethod()]
//        public void GetTechnicalAttributeGroupsTest()
//        {
//            //Input parameters for the method
//            String categoryID = String.Empty;
//            String catalogID = String.Empty; 
//            String localeID = String.Empty; 

//            //Input and Output XMLs
//            String InputXML = String.Empty;
//            String OutputXML = String.Empty;

//            InputXML = @"<InputParameters categoryID=""1471830"" catalogID=""10110""  localeID=""1""/>";
////            Actual OutputXML
////            OutputXML = @"<TechnicalAttributeGroupCollection>
////                                <TechnicalAttributeGroup Id=""109818"" LongName=""FH_CMN_Boolean Attribute Group1""/>
////                                <TechnicalAttributeGroup Id=""109813"" LongName=""FH_CMN_Date Attribute Group1""/>
////                                <TechnicalAttributeGroup Id=""109805"" LongName=""FH_CMN_Decimal Attribute Group1""/>
////                                <TechnicalAttributeGroup Id=""109827"" LongName=""FH_CMN_File Attribute Group1""/>
////                          </TechnicalAttributeGroupCollection>";

//            OutputXML = @"<TechnicalAttributeGroupCollection>
//                                <TechnicalAttributeGroup Id=""109818"" LongName=""FH_CMN_Boolean Attribute Group1""/>
//                                <TechnicalAttributeGroup Id=""109813"" LongName=""FH_CMN_Date Attribute Group1""/>
//                                <TechnicalAttributeGroup Id=""109805"" LongName=""FH_CMN_Decimal Attribute Group1""/>
//                                <TechnicalAttributeGroup Id=""109827"" LongName=""FH_CMN_File Attribute Group1""/>
//                          </TechnicalAttributeGroupCollection>";

//            Collection<AttributeGroup> expected = new Collection<AttributeGroup>();
            
//            //Read Input parameters from Input XML
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.LoadXml(InputXML);
//            XmlNode rootNode = xmlDoc.SelectSingleNode("InputParameters");
//            categoryID = rootNode.Attributes["categoryID"].Value;
//            catalogID = rootNode.Attributes["catalogID"].Value;
//            localeID = rootNode.Attributes["localeID"].Value;

//            //Read OutputXML and generate the list of expected Technical Attribute Groups
//            xmlDoc.LoadXml(OutputXML);

//            foreach (XmlNode node in xmlDoc.SelectNodes("TechnicalAttributeGroupCollection/TechnicalAttributeGroup"))
//            {
//                AttributeGroup attributeGroup = new AttributeGroup();
//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value, out id);
//                attributeGroup.Id = id;
//                attributeGroup.LongName = node.Attributes["LongName"].Value;

//                expected.Add(attributeGroup);
//            }
//            Collection<AttributeGroup> actual;
//            actual = AttributeGroupBL.GetTechnicalAttributeGroups(categoryID, catalogID,"en_WW");
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetRelationshipAttributeGroups
//        ///</summary>
//        [TestMethod()]
//        public void GetRelationshipAttributeGroupsTest()
//        {
//            //Input parameters for the method
//            String catalogId = String.Empty;
//            String nodeTypeId = String.Empty;

//            //Input and Output XMLs
//            String InputXML = String.Empty;
//            String OutputXML = String.Empty;

//            InputXML = @"<InputParameters CatalogID=""10110"" NodeTypeID=""10021"" />";

////            Actual OutputXML

////            OutputXML = @"<RelationShipAttributeGroupCollection>
////                                <RelationShipAttributeGroup Id=""1012"" LongName=""Sold To - Invoice To""/>
////                                <RelationShipAttributeGroup Id=""1015"" LongName=""Auto_Testing2""/>
////                          </RelationShipAttributeGroupCollection>";

//            OutputXML = @"<RelationShipAttributeGroupCollection>
//                                <RelationShipAttributeGroup Id=""1012"" Name=""Sold To - Invoice To""/>
//                                <RelationShipAttributeGroup Id=""1015"" Name=""Auto_Testing2""/>
//                          </RelationShipAttributeGroupCollection>";

//            Collection<AttributeGroup> expected = new Collection<AttributeGroup>();

//            //Read Input parameters from Input XML
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.LoadXml(InputXML);
//            XmlNode rootNode = xmlDoc.SelectSingleNode("InputParameters");
//            nodeTypeId = rootNode.Attributes["NodeTypeID"].Value;
//            catalogId = rootNode.Attributes["CatalogID"].Value;

//            //Read OutputXML and generate the list of expected Technical Attribute Groups
//            xmlDoc.LoadXml(OutputXML);

//            foreach (XmlNode node in xmlDoc.SelectNodes("RelationShipAttributeGroupCollection/RelationShipAttributeGroup"))
//            {
//                AttributeGroup attributeGroup = new AttributeGroup();
//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value, out id);
//                attributeGroup.Id = id;
//                attributeGroup.Name = node.Attributes["Name"].Value;

//                expected.Add(attributeGroup);
//            }
//            Collection<AttributeGroup> actual;
//            actual = AttributeGroupBL.GetRelationshipAttributeGroups(catalogId, nodeTypeId);
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetCommonAttributeGroups
//        ///</summary>
//        [TestMethod()]
//        public void GetCommonAttributeGroupsTest()
//        {

//            //Input Parameters for the method
//            String catalogID = String.Empty; 
//            String nodeType = String.Empty;

//            //Input and Output XMLs
//            String InputXML = String.Empty;
//            String OutPutXML = String.Empty;

//            Collection<AttributeGroup> expected = new Collection<AttributeGroup>(); 
//            AttributeGroup attributeGroup = null;

//            InputXML = @"<InputParameters CatalogID=""10110"" NodeTypeID=""10021"" />";


////          Actual Output For above Inputs.
////            OutPutXML = @"<CommonAttributeGroupCollection>
////                                <CommonAttributeGroup Id=""109832"" Name=""Attribute Group"" LongName=""Attribute Group"" ParentAttributeGroupId=""14"" AttributeTypeName=""AttributeGroup""/>
////                                <CommonAttributeGroup Id=""109763"" Name=""FH_CMN_Decimal Attribute Group1"" LongName=""FH_CMN_Decimal Attribute Group1"" ParentAttributeGroupId=""14"" AttributeTypeName=""AttributeGroup""/>
////                                <CommonAttributeGroup Id=""109841"" Name=""Tst"" LongName=""Test"" ParentAttributeGroupId=""14"" AttributeTypeName=""AttributeGroup""/>
////                         </CommonAttributeGroupCollection>";
            
//            OutPutXML = @"<CommonAttributeGroupCollection>
//                                <CommonAttributeGroup Id=""109832"" Name=""Attribute Group"" LongName=""Attribute Group"" ParentAttributeGroupId=""14"" AttributeTypeName=""AttributeGroup""/>
//                                <CommonAttributeGroup Id=""109763"" Name=""FH_CMN_Decimal Attribute Group1"" LongName=""FH_CMN_Decimal Attribute Group1"" ParentAttributeGroupId=""14"" AttributeTypeName=""AttributeGroup""/>
//                                <CommonAttributeGroup Id=""109841"" Name=""Tst"" LongName=""Test"" ParentAttributeGroupId=""14"" AttributeTypeName=""AttributeGroup""/>
//                         </CommonAttributeGroupCollection>";

//            //Read Input parameters From XML
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.LoadXml(InputXML);
//            XmlNode rootNode = xmlDoc.SelectSingleNode("InputParameters");
//            catalogID = rootNode.Attributes["CatalogID"].Value;
//            nodeType = rootNode.Attributes["NodeTypeID"].Value;

//            //Read Output XML and create a list of expected Attribute Groups.
//            xmlDoc.LoadXml(OutPutXML);
//            foreach (XmlNode node in xmlDoc.SelectNodes("CommonAttributeGroupCollection/CommonAttributeGroup"))
//            {
//                attributeGroup = new AttributeGroup();
                
//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value,out id);
//                attributeGroup.Id = id;
//                attributeGroup.Name = node.Attributes["Name"].Value;
//                attributeGroup.LongName = node.Attributes["LongName"].Value;
//                Int32 parentAttributeGroupId = 0;
//                Int32.TryParse(node.Attributes["ParentAttributeGroupId"].Value, out parentAttributeGroupId);
//                attributeGroup.ParentAttributeGroupId = parentAttributeGroupId;
//                attributeGroup.AttributeTypeName = node.Attributes["AttributeTypeName"].Value;
//                expected.Add(attributeGroup);
//            }
            
//            Collection<AttributeGroup> actual;
//            actual = AttributeGroupBL.GetCommonAttributeGroups(catalogID, nodeType, "en_WW");
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetByAttributeType
//        ///</summary>
//        [TestMethod()]
//        public void GetByAttributeTypeTest()
//        {
//            AttributeGroupBL target = new AttributeGroupBL(); // TODO: Initialize to an appropriate value
//            int common = 0; // TODO: Initialize to an appropriate value
//            int technical = 0; // TODO: Initialize to an appropriate value
//            int relationship = 0; // TODO: Initialize to an appropriate value
//            Collection<AttributeGroup> expected = null; // TODO: Initialize to an appropriate value
//            Collection<AttributeGroup> actual;
//            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            
//            //TODO: Need to get current Data Locale as well as System Default Data Locale.
//            locales.Add(LocaleEnum.de_DE);

//            actual = target.GetByAttributeType(common, technical, relationship,locales,LocaleEnum.en_US);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for AttributeGroupBL Constructor
//        ///</summary>
//        [TestMethod()]
//        public void AttributeGroupBLConstructorTest()
//        {
//            AttributeGroupBL target = new AttributeGroupBL();
//            Assert.Inconclusive("TODO: Implement code to verify target");
//        }
    }
}
