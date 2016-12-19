using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.AttributeManager.Business;

namespace MDM.AttributeManager.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for AttributeBLTest and is intended
    ///to contain all AttributeBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttributeBLTest
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


        ///// <summary>
        /////A test for GetCommonAttributes with Specific group id
        /////</summary>
        //[TestMethod()]
        //public void GetCommonAttributesWithSpecificGroupTest()
        //{
        //    AttributeBL target = new AttributeBL(); 

        //    //Input Parameters for Method
        //    Int32 entityId = 0; 
        //    Int32 containerId = 0;
        //    Int32 attributeParentId = 0; 
        //    int[] attributeIds = null; 
        //    String locale = String.Empty; 
        //    Boolean onlyShowAtCreationAttributes = false;

        //    //Input and Output XMLs
        //    String InputXML = @"<InputParameters entityId=""1471831"" containerId=""10110"" attributeParentId=""109832"" attributeIds="""" locale=""en_WW"" onlyShowAtCreationAttributes=""false""/>";
        //    //String OutputXML = @"<Attributes><Attribute Id=""109836"" Name=""Effective From"" LongName=""Effective From"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109837"" Name=""Effective To"" LongName=""Effective To"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109838"" Name=""Product ID"" LongName=""Product ID"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""TST01"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109833"" Name=""SKU"" LongName=""SKU"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""11"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute></Attributes>";
        //    String OutputXML = @"<Attributes><Attribute Id=""109836"" Name=""Effective From"" LongName=""Effective From"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109837"" Name=""Effective To"" LongName=""Effective To"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109838"" Name=""Product ID"" LongName=""Product ID"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""TST01"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109833"" Name=""SKU"" LongName=""SKU"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""11"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute></Attributes>";
        //    AttributeCollection expected = new AttributeCollection();
        //    AttributeCollection actual;

        //    //Read Parameters From InputXML
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(InputXML);
        //    XmlNode rootNode = xmlDoc.SelectSingleNode("InputParameters");
        //    Int32.TryParse(rootNode.Attributes["entityId"].Value, out entityId);
        //    Int32.TryParse(rootNode.Attributes["attributeParentId"].Value, out attributeParentId);
        //    Int32.TryParse(rootNode.Attributes["containerId"].Value, out containerId);
        //    locale = rootNode.Attributes["locale"].Value;
        //    Boolean.TryParse(rootNode.Attributes["onlyShowAtCreationAttributes"].Value, out onlyShowAtCreationAttributes);
        //    String listOfAttributeIds = String.Empty;
        //    listOfAttributeIds = rootNode.Attributes["attributeIds"].Value;
        //    if (!String.IsNullOrEmpty(listOfAttributeIds))
        //    {
        //        attributeIds = MDM.Utility.ValueTypeHelper.SplitStringToIntArray(listOfAttributeIds, ',');
        //    }

        //    //Read the OutputXML to generate the expected result.
        //    XmlTextReader reader = null;
           
        //    reader = new XmlTextReader(OutputXML, XmlNodeType.Element, null);

        //    while (reader.Read()) // Reading <Attributes> root node and moves to <Attribute> node
        //    {
        //        while (reader.Depth == 1) // loop through <Attribute> nodes
        //        {
        //            reader.MoveToElement();
        //            if (!reader.Name.Equals("Attribute"))
        //            {
        //                throw new FormatException("<Attribute> node not found under <Attributes>");
        //            }

        //            String attributeXml = reader.ReadOuterXml(); //Read <Attribute> node xml

        //            MDM.BusinessObjects.Attribute attribute = new MDM.BusinessObjects.Attribute(attributeXml);
        //            expected.Add(attribute);
                    
        //        }
        //    }
        //    if (reader != null)
        //        reader.Close();

        //    //actual = target.GetCommonAttributes(entityId, containerId, attributeParentId, attributeIds, locale, onlyShowAtCreationAttributes);
        //    //Assert.IsTrue(expected.Equals(actual));
        //    Assert.AreEqual<AttributeCollection>(expected, actual);
        //}

        ///// <summary>
        /////A test for GetCommonAttributes with list of attributes
        /////</summary>
        //[TestMethod()]
        //public void GetCommonAttributesWithListOfAttributesTest()
        //{
        //    AttributeBL target = new AttributeBL();

        //    //Input Parameters for Method
        //    Int32 entityId = 0;
        //    Int32 containerId = 0;
        //    Int32 attributeParentId = 0;
        //    int[] attributeIds = null;
        //    String locale = String.Empty;
        //    Boolean onlyShowAtCreationAttributes = false;

        //    //Input and Output XMLs
        //    String InputXML = @"<InputParameters entityId=""1471831"" containerId=""10110"" attributeParentId=""0"" attributeIds=""109836,109837,109838,109833"" locale=""en_WW"" onlyShowAtCreationAttributes=""false""/>";
        //    //String OutputXML = @"<Attributes><Attribute Id=""109836"" Name=""Effective From"" LongName=""Effective From"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109837"" Name=""Effective To"" LongName=""Effective To"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109838"" Name=""Product ID"" LongName=""Product ID"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""TST01"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109833"" Name=""SKU"" LongName=""SKU"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""11"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute></Attributes>";
        //    String OutputXML = @"<Attributes><Attribute Id=""109836"" Name=""Effective From"" LongName=""Effective From"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109837"" Name=""Effective To"" LongName=""Effective To"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""Date"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""08/05/2010"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109838"" Name=""Product ID"" LongName=""Product ID"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""TST01"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute><Attribute Id=""109833"" Name=""SKU"" LongName=""SKU"" AttributeParentId=""109832"" AttributeParentName=""Attribute Group"" AttributeParentLongName=""Attribute Group"" AttributeType=""Attribute"" AttributeDataType=""String"" IsCollection=""false"" IsComplex=""false"" IsLocalizable=""false"" ApplyLocaleFormat=""false"" ApplyTimeZoneConversion=""false"" Precision=""0"" SourceFlag=""O"" Updateable=""Update"" Visible=""View""><Values><Value AttrVal=""11"" Uom="""" ValueRefId=""0"" Locale=""Neutral"" Sequence=""-1"" /></Values></Attribute></Attributes>";
        //    AttributeCollection expected = new AttributeCollection();
        //    AttributeCollection actual;

        //    //Read Parameters From InputXML
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(InputXML);
        //    XmlNode rootNode = xmlDoc.SelectSingleNode("InputParameters");
        //    Int32.TryParse(rootNode.Attributes["entityId"].Value, out entityId);
        //    Int32.TryParse(rootNode.Attributes["attributeParentId"].Value, out attributeParentId);
        //    Int32.TryParse(rootNode.Attributes["containerId"].Value, out containerId);
        //    locale = rootNode.Attributes["locale"].Value;
        //    Boolean.TryParse(rootNode.Attributes["onlyShowAtCreationAttributes"].Value, out onlyShowAtCreationAttributes);
        //    String listOfAttributeIds = String.Empty;
        //    listOfAttributeIds = rootNode.Attributes["attributeIds"].Value;
        //    if (!String.IsNullOrEmpty(listOfAttributeIds))
        //    {
        //        attributeIds = MDM.Utility.ValueTypeHelper.SplitStringToIntArray(listOfAttributeIds, ',');
        //    }

        //    //Read the OutputXML to generate the expected result.
        //    XmlTextReader reader = null;

        //    reader = new XmlTextReader(OutputXML, XmlNodeType.Element, null);

        //    while (reader.Read()) // Reading <Attributes> root node and moves to <Attribute> node
        //    {
        //        while (reader.Depth == 1) // loop through <Attribute> nodes
        //        {
        //            reader.MoveToElement();
        //            if (!reader.Name.Equals("Attribute"))
        //            {
        //                throw new FormatException("<Attribute> node not found under <Attributes>");
        //            }

        //            String attributeXml = reader.ReadOuterXml(); //Read <Attribute> node xml

        //            MDM.BusinessObjects.Attribute attribute = new MDM.BusinessObjects.Attribute(attributeXml);
        //            expected.Add(attribute);

        //        }
        //    }
        //    if (reader != null)
        //        reader.Close();

        //    //actual = target.GetCommonAttributes(entityId, containerId, attributeParentId, attributeIds, locale, onlyShowAtCreationAttributes);
        //    //Assert.IsTrue(expected.Equals(actual));
        //    Assert.AreEqual<AttributeCollection>(expected, actual);
        //}
        /// <summary>
        ///A test for GetCommonAttributeModel
        ///</summary>
        //[TestMethod()]
        //public void GetCommonAttributeModelTest()
        //{
        //    AttributeBL target = new AttributeBL(); // TODO: Initialize to an appropriate value
        //    Int32 containerId = 0; // TODO: Initialize to an appropriate value
        //    Int32 entityTypeId = 0; // TODO: Initialize to an appropriate value
        //    Collection<Int32> attributeIds = null; // TODO: Initialize to an appropriate value
        //    Collection<Int32> attributeGroupIds = null; // TODO: Initialize to an appropriate value
        //    Collection<Int32> excludeAttributeIds = null; // TODO: Initialize to an appropriate value
        //    Collection<LocaleEnum> locale = new Collection<LocaleEnum>(){ LocaleEnum.UnKnown}; // TODO: Initialize to an appropriate value
        //    Boolean onlyShowAtCreationAttributes = false; // TODO: Initialize to an appropriate value
        //    Boolean onlyRequiredAttributes = false; // TODO: Initialize to an appropriate value
        //    Boolean getCompleteDetailsOfAttribute = false; // TODO: Initialize to an appropriate value

        //    AttributeModelContext attributeModelContext = new AttributeModelContext(containerId, entityTypeId, 0, 0, locale,0, Core.AttributeModelType.Common, onlyShowAtCreationAttributes, onlyRequiredAttributes, getCompleteDetailsOfAttribute);

        //    AttributeCollection expected = null; // TODO: Initialize to an appropriate value
        //    AttributeCollection actual;

        //    actual = target.GetAttributesFromModel(attributeIds, attributeGroupIds, excludeAttributeIds, attributeModelContext);

        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetCategoryAttributes
        /////</summary>
        //[TestMethod()]
        //public void GetCategoryAttributesTest()
        //{
        //    AttributeBL target = new AttributeBL(); // TODO: Initialize to an appropriate value
        //    int entityId = 0; // TODO: Initialize to an appropriate value
        //    int containerId = 0; // TODO: Initialize to an appropriate value
        //    int attributeParentId = 0; // TODO: Initialize to an appropriate value
        //    int[] attributeIds = null; // TODO: Initialize to an appropriate value
        //    string locale = string.Empty; // TODO: Initialize to an appropriate value
        //    bool onlyShowAtCreationAttributes = false; // TODO: Initialize to an appropriate value
        //    AttributeCollection expected = null; // TODO: Initialize to an appropriate value
        //    AttributeCollection actual;
        //    actual = target.GetCategoryAttributes(entityId, containerId, attributeParentId, attributeIds, locale, onlyShowAtCreationAttributes);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for GetCategoryAttributeModel
        ///</summary>
        //[TestMethod()]
        //public void GetCategoryAttributeModelTest()
        //{
        //    AttributeBL target = new AttributeBL(); // TODO: Initialize to an appropriate value
        //    Int32 categoryId = 0; // TODO: Initialize to an appropriate value
        //    Collection<Int32> attributeIds = null; // TODO: Initialize to an appropriate value
        //    Collection<Int32> attributeGroupIds = null; // TODO: Initialize to an appropriate value
        //    Collection<Int32> excludeattributeIds = null; // TODO: Initialize to an appropriate value
        //    Collection<LocaleEnum> locale = new Collection<LocaleEnum>() {LocaleEnum.UnKnown}; // TODO: Initialize to an appropriate value
        //    Boolean onlyShowAtCreationAttributes = false; // TODO: Initialize to an appropriate value
        //    Boolean onlyRequiredAttributes = false; // TODO: Initialize to an appropriate value
        //    Boolean getCompleteDetailsOfAttribute = false; // TODO: Initialize to an appropriate value

        //    AttributeModelContext attributeModelContext = new AttributeModelContext(0, 0, 0, categoryId, locale,0, Core.AttributeModelType.Category, onlyShowAtCreationAttributes, onlyRequiredAttributes, getCompleteDetailsOfAttribute);

        //    AttributeCollection expected = null; // TODO: Initialize to an appropriate value
        //    AttributeCollection actual;

        //    actual = target.GetAttributesFromModel(attributeIds, attributeGroupIds, excludeattributeIds, attributeModelContext);

        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for AttributeBL Constructor
        /////</summary>
        //[TestMethod()]
        //public void AttributeBLConstructorTest()
        //{
        //    AttributeBL target = new AttributeBL();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}
    }
}
