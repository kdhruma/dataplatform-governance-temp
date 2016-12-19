using System;
using System.Xml;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MDM.BusinessObjects;
using MDM.DataModelManager.Business;

using MDM.Core;

namespace MDM.DataModelManager.UnitTest
{
    /// <summary>
    ///This is a test class for EntityTypeAttributeMappingBLTest and is intended
    ///to contain all EntityTypeAttributeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EntityTypeAttributeMappingBLTest
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

        /// <summary>
        ///A test for Process
        ///</summary>
//        [TestMethod()]
//        public void ProcessTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            EntityTypeAttributeMapping entityTypeAttributeMapping = null;
//            EntityTypeAttributeMappingBL target = new EntityTypeAttributeMappingBL();
//            Collection<EntityTypeAttributeMapping> entityTypeAttributeMappings = new Collection<EntityTypeAttributeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<EntityTypeAttributeMappingCollection EntityTypeId = ""-1"" AttributeGroupId = ""-1"" AttributeId = ""-1"">
//	                            <EntityTypeAttributeMapping Id = ""-1"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Create"" EntityTypeId = ""6"" EntityTypeName = """" AttributeId = ""109991"" AttributeName = """" AttributeParentId = ""109990"" AttributeParentName = """" Required = ""True"" ReadOnly = ""False"" ShowAtCreation = ""False"" Extension = """" SortOrder = ""0""/>
//                            </EntityTypeAttributeMappingCollection>");

//            //Prepare EntityTypeAttributeMapping Collection required for processing
//            foreach (XmlNode node in xmlDoc.SelectNodes("EntityTypeAttributeMappingCollection/EntityTypeAttributeMapping"))
//            {
//                entityTypeAttributeMapping = new EntityTypeAttributeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                entityTypeAttributeMapping.Id = id;
//                entityTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
//                entityTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                LocaleEnum locale = LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                entityTypeAttributeMapping.Locale = locale;

//                entityTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                entityTypeAttributeMapping.EntityTypeId = entTypeId;
//                entityTypeAttributeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 attrId = 0;
//                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
//                entityTypeAttributeMapping.AttributeId = attrId;
//                entityTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

//                Int32 attrParentId = 0;
//                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
//                entityTypeAttributeMapping.AttributeParentId = attrParentId;
//                entityTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

//                Boolean required = false;
//                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
//                entityTypeAttributeMapping.Required = required;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                entityTypeAttributeMapping.ReadOnly = readOnly;

//                Boolean showAtCreation = false;
//                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
//                entityTypeAttributeMapping.ShowAtCreation = showAtCreation;
//                entityTypeAttributeMapping.Extension = node.Attributes["Extension"].Value.ToString();

//                Int32 sortOrder = 0;
//                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
//                entityTypeAttributeMapping.SortOrder = sortOrder;

//                entityTypeAttributeMappings.Add(entityTypeAttributeMapping);
//            }

//            //Process mappings and assert the result
//            Assert.IsTrue(target.Process(entityTypeAttributeMappings));
//        }

//        /// <summary>
//        ///A test for GetMappingsByEntityTypeIdAndAttributeGroupId
//        ///</summary>
//        [TestMethod()]
//        public void GetMappingsByEntityTypeIdAndAttributeGroupIdTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            EntityTypeAttributeMapping entityTypeAttributeMapping = null;
//            EntityTypeAttributeMappingBL target = new EntityTypeAttributeMappingBL();
//            Collection<EntityTypeAttributeMapping> expected = new Collection<EntityTypeAttributeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<EntityTypeAttributeMappingCollection EntityTypeId = ""6"" AttributeGroupId = ""109990"" AttributeId = ""-1"">
//	                            <EntityTypeAttributeMapping Id = ""3189"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" EntityTypeId = ""6"" EntityTypeName = ""Category"" AttributeId = ""109991"" AttributeName = ""ABC Code"" AttributeParentId = ""-1"" AttributeParentName = """" Required = ""True"" ReadOnly = ""False"" ShowAtCreation = ""False"" Extension = """" SortOrder = ""0""/>
//                            </EntityTypeAttributeMappingCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("EntityTypeAttributeMappingCollection");

//            //Get Entity Type Id from XML
//            Int32 entityTypeId = 0;
//            Int32.TryParse(rootNode.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);

//            //Get Attribute Group Id from XML
//            Int32 attributeGroupId = 0;
//            Int32.TryParse(rootNode.Attributes["AttributeGroupId"].Value.ToString(), out attributeGroupId);

//            //Prepare expected EntityTypeAttributeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("EntityTypeAttributeMappingCollection/EntityTypeAttributeMapping"))
//            {
//                entityTypeAttributeMapping = new EntityTypeAttributeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                entityTypeAttributeMapping.Id = id;
//                entityTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
//                entityTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                entityTypeAttributeMapping.Locale = locale;

//                entityTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                entityTypeAttributeMapping.EntityTypeId = entTypeId;
//                entityTypeAttributeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 attrId = 0;
//                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
//                entityTypeAttributeMapping.AttributeId = attrId;
//                entityTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

//                Int32 attrParentId = 0;
//                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
//                entityTypeAttributeMapping.AttributeParentId = attrParentId;
//                entityTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

//                Boolean required = false;
//                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
//                entityTypeAttributeMapping.Required = required;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                entityTypeAttributeMapping.ReadOnly = readOnly;

//                Boolean showAtCreation = false;
//                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
//                entityTypeAttributeMapping.ShowAtCreation = showAtCreation;
//                entityTypeAttributeMapping.Extension = node.Attributes["Extension"].Value.ToString();

//                Int32 sortOrder = 0;
//                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
//                entityTypeAttributeMapping.SortOrder = sortOrder;

//                expected.Add(entityTypeAttributeMapping);
//            }

//            //Get actual mappings
//            Collection<EntityTypeAttributeMapping> actual;
//            actual = target.GetMappingsByEntityTypeIdAndAttributeGroupId(entityTypeId, attributeGroupId);

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetMappingsByEntityTypeId
//        ///</summary>
//        [TestMethod()]
//        public void GetMappingsByEntityTypeIdTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            EntityTypeAttributeMapping entityTypeAttributeMapping = null;
//            EntityTypeAttributeMappingBL target = new EntityTypeAttributeMappingBL();
//            Collection<EntityTypeAttributeMapping> expected = new Collection<EntityTypeAttributeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<EntityTypeAttributeMappingCollection EntityTypeId = ""6"" AttributeGroupId = ""-1"" AttributeId = ""-1"">
//	                            <EntityTypeAttributeMapping Id = ""3189"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" EntityTypeId = ""6"" EntityTypeName = """" AttributeId = ""109991"" AttributeName = ""ABC Code"" AttributeParentId = ""-1"" AttributeParentName = """" Required = ""True"" ReadOnly = ""False"" ShowAtCreation = ""False"" Extension = """" SortOrder = ""0""/>
//                            </EntityTypeAttributeMappingCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("EntityTypeAttributeMappingCollection");

//            //Get Entity Type Id from XML
//            Int32 entityTypeId = 0;
//            Int32.TryParse(rootNode.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);

//            //Prepare expected EntityTypeAttributeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("EntityTypeAttributeMappingCollection/EntityTypeAttributeMapping"))
//            {
//                entityTypeAttributeMapping = new EntityTypeAttributeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                entityTypeAttributeMapping.Id = id;
//                entityTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
//                entityTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                entityTypeAttributeMapping.Locale = locale;

//                entityTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                entityTypeAttributeMapping.EntityTypeId = entTypeId;
//                entityTypeAttributeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 attrId = 0;
//                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
//                entityTypeAttributeMapping.AttributeId = attrId;
//                entityTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

//                Int32 attrParentId = 0;
//                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
//                entityTypeAttributeMapping.AttributeParentId = attrParentId;
//                entityTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

//                Boolean required = false;
//                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
//                entityTypeAttributeMapping.Required = required;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                entityTypeAttributeMapping.ReadOnly = readOnly;

//                Boolean showAtCreation = false;
//                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
//                entityTypeAttributeMapping.ShowAtCreation = showAtCreation;
//                entityTypeAttributeMapping.Extension = node.Attributes["Extension"].Value.ToString();

//                Int32 sortOrder = 0;
//                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
//                entityTypeAttributeMapping.SortOrder = sortOrder;

//                expected.Add(entityTypeAttributeMapping);
//            }

//            //Get actual mappings
//            Collection<EntityTypeAttributeMapping> actual;
//            actual = target.GetMappingsByEntityTypeId(entityTypeId);

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetMappingsByAttributeId
//        ///</summary>
//        [TestMethod()]
//        public void GetMappingsByAttributeIdTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            EntityTypeAttributeMapping entityTypeAttributeMapping = null;
//            EntityTypeAttributeMappingBL target = new EntityTypeAttributeMappingBL();
//            Collection<EntityTypeAttributeMapping> expected = new Collection<EntityTypeAttributeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<EntityTypeAttributeMappingCollection EntityTypeId = ""-1"" AttributeGroupId = ""-1"" AttributeId = ""109991"">
//	                            <EntityTypeAttributeMapping Id = ""3189"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" EntityTypeId = ""6"" EntityTypeName = ""Category"" AttributeId = ""109991"" AttributeName = ""ABC Code"" AttributeParentId = ""-1"" AttributeParentName = """" Required = ""True"" ReadOnly = ""False"" ShowAtCreation = ""False"" Extension = """" SortOrder = ""0""/>
//                            </EntityTypeAttributeMappingCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("EntityTypeAttributeMappingCollection");

//            //Get Attribute Id from XML
//            Int32 attributeId = 0;
//            Int32.TryParse(rootNode.Attributes["AttributeId"].Value.ToString(), out attributeId);

//            //Prepare expected EntityTypeAttributeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("EntityTypeAttributeMappingCollection/EntityTypeAttributeMapping"))
//            {
//                entityTypeAttributeMapping = new EntityTypeAttributeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                entityTypeAttributeMapping.Id = id;
//                entityTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
//                entityTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                entityTypeAttributeMapping.Locale = locale;

//                entityTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                entityTypeAttributeMapping.EntityTypeId = entTypeId;
//                entityTypeAttributeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 attrId = 0;
//                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
//                entityTypeAttributeMapping.AttributeId = attrId;
//                entityTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

//                Int32 attrParentId = 0;
//                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
//                entityTypeAttributeMapping.AttributeParentId = attrParentId;
//                entityTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

//                Boolean required = false;
//                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
//                entityTypeAttributeMapping.Required = required;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                entityTypeAttributeMapping.ReadOnly = readOnly;

//                Boolean showAtCreation = false;
//                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
//                entityTypeAttributeMapping.ShowAtCreation = showAtCreation;
//                entityTypeAttributeMapping.Extension = node.Attributes["Extension"].Value.ToString();

//                Int32 sortOrder = 0;
//                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
//                entityTypeAttributeMapping.SortOrder = sortOrder;

//                expected.Add(entityTypeAttributeMapping);
//            }

//            //Get actual mappings
//            Collection<EntityTypeAttributeMapping> actual;
//            actual = target.GetMappingsByAttributeId(attributeId);

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetAll
//        ///</summary>
//        [TestMethod()]
//        public void GetAllTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            EntityTypeAttributeMapping entityTypeAttributeMapping = null;
//            EntityTypeAttributeMappingBL target = new EntityTypeAttributeMappingBL();
//            Collection<EntityTypeAttributeMapping> expected = new Collection<EntityTypeAttributeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<EntityTypeAttributeMappingCollection EntityTypeId = """" AttributeId = """">
//	                            <EntityTypeAttributeMapping Id = """" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" EntityTypeId = """" EntityTypeName = """" AttributeId = """" AttributeName = """" AttributeParentId = """" AttributeParentName = """" Required = """" ReadOnly = """" ShowAtCreation = """" Extension = """" SortOrder = """"/>
//                            </EntityTypeAttributeMappingCollection>");

//            //Prepare expected EntityTypeAttributeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("EntityTypeAttributeMappingCollection/EntityTypeAttributeMapping"))
//            {
//                entityTypeAttributeMapping = new EntityTypeAttributeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                entityTypeAttributeMapping.Id = id;
//                entityTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
//                entityTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                entityTypeAttributeMapping.Locale = locale;

//                entityTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                entityTypeAttributeMapping.EntityTypeId = entTypeId;
//                entityTypeAttributeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 attrId = 0;
//                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
//                entityTypeAttributeMapping.AttributeId = attrId;
//                entityTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

//                Int32 attrParentId = 0;
//                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
//                entityTypeAttributeMapping.AttributeParentId = attrParentId;
//                entityTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

//                Boolean required = false;
//                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
//                entityTypeAttributeMapping.Required = required;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                entityTypeAttributeMapping.ReadOnly = readOnly;

//                Boolean showAtCreation = false;
//                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
//                entityTypeAttributeMapping.ShowAtCreation = showAtCreation;
//                entityTypeAttributeMapping.Extension = node.Attributes["Extension"].Value.ToString();

//                Int32 sortOrder = 0;
//                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
//                entityTypeAttributeMapping.SortOrder = sortOrder;

//                expected.Add(entityTypeAttributeMapping);
//            }

//            Collection<EntityTypeAttributeMapping> actual;
//            actual = target.GetAll();

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for EntityTypeAttributeMappingBL Constructor
//        ///</summary>
//        [TestMethod()]
//        public void EntityTypeAttributeMappingBLConstructorTest()
//        {
//            EntityTypeAttributeMappingBL target = new EntityTypeAttributeMappingBL();
//            Assert.Inconclusive("TODO: Implement code to verify target");
//        }
    }
}
