using System;
using System.Xml;
using MDM.DataModelManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.DataModelManager.UnitTest
{
    /// <summary>
    ///This is a test class for ContainerEntityTypeAttributeMappingBLTest and is intended
    ///to contain all ContainerEntityTypeAttributeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContainerEntityTypeAttributeMappingBLTest
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
        //            ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping = null;
        //            ContainerEntityTypeAttributeMappingBL target = new ContainerEntityTypeAttributeMappingBL();
        //            Collection<ContainerEntityTypeAttributeMapping> containerEntityTypeAttributeMappings = new Collection<ContainerEntityTypeAttributeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<ContainerEntityTypeAttributeMappingCollection ContainerId = ""-1"" EntityTypeId = ""-1"" AttributeGroupId = ""-1"" AttributeId = ""-1"">
        //	                            <ContainerEntityTypeAttributeMapping Id = ""574308"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Update"" OrganizationId = ""-1"" OrganizationName = """" 
        //                                 ContainerId = ""10113"" ContainerName = """" EntityTypeId = ""7"" EntityTypeName = """" AttributeId = ""109992"" AttributeName = """" AttributeParentId = """" AttributeParentName = """" 
        //                                 Required = ""True"" ReadOnly = ""False"" ShowAtCreation = ""False"" Extension = """" SortOrder = ""0"" IsSpecialized = ""False"" />
        //                             </ContainerEntityTypeAttributeMappingCollection>");

        //            //Prepare ContainerEntityTypeAttributeMapping Collection required for processing
        //            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerEntityTypeAttributeMappingCollection/ContainerEntityTypeAttributeMapping"))
        //            {
        //                containerEntityTypeAttributeMapping = new ContainerEntityTypeAttributeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                containerEntityTypeAttributeMapping.Id = id;
        //                containerEntityTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                containerEntityTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                containerEntityTypeAttributeMapping.Locale = locale;

        //                containerEntityTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 orgId = 0;
        //                Int32.TryParse(node.Attributes["OrganizationId"].Value.ToString(), out orgId);
        //                containerEntityTypeAttributeMapping.OrganizationId = orgId;
        //                containerEntityTypeAttributeMapping.OrganizationName = node.Attributes["OrganizationName"].Value.ToString();

        //                Int32 contId = 0;
        //                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
        //                containerEntityTypeAttributeMapping.ContainerId = contId;
        //                containerEntityTypeAttributeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

        //                Int32 entTypeId = 0;
        //                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
        //                containerEntityTypeAttributeMapping.EntityTypeId = entTypeId;
        //                containerEntityTypeAttributeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

        //                Int32 attrId = 0;
        //                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
        //                containerEntityTypeAttributeMapping.AttributeId = attrId;
        //                containerEntityTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

        //                Int32 attrParentId = 0;
        //                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
        //                containerEntityTypeAttributeMapping.AttributeParentId = attrParentId;
        //                containerEntityTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

        //                Boolean required = false;
        //                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
        //                containerEntityTypeAttributeMapping.Required = required;

        //                Boolean readOnly = false;
        //                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
        //                containerEntityTypeAttributeMapping.ReadOnly = readOnly;

        //                Boolean showAtCreation = false;
        //                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
        //                containerEntityTypeAttributeMapping.ShowAtCreation = showAtCreation;
        //                containerEntityTypeAttributeMapping.Extension = node.Attributes["Extension"].Value.ToString();

        //                Int32 sortOrder = 0;
        //                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
        //                containerEntityTypeAttributeMapping.SortOrder = sortOrder;

        //                containerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMapping);
        //            }

        //            //Process mappings and assert the result
        //            Assert.IsTrue(target.Process(containerEntityTypeAttributeMappings));
        //        }

        //        /// <summary>
        //        ///A test for Get
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetTest()
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping = null;
        //            ContainerEntityTypeAttributeMappingBL target = new ContainerEntityTypeAttributeMappingBL();
        //            Collection<ContainerEntityTypeAttributeMapping> expected = new Collection<ContainerEntityTypeAttributeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<ContainerEntityTypeAttributeMappingCollection ContainerId = ""10113"" EntityTypeId = ""7"" AttributeGroupId = ""109990"" AttributeId = ""-1"">
        //	                            <ContainerEntityTypeAttributeMapping Id = ""574308"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" OrganizationId = ""-1"" OrganizationName = "" 
        //                                 ContainerId = ""10113"" ContainerName = ""Test Catalog"" EntityTypeId = ""7"" EntityTypeName = ""Component"" AttributeId = ""109992"" AttributeName = ""9-Box"" AttributeParentId = ""109990"" AttributeParentName = """" 
        //                                 Required = ""True"" ReadOnly = ""False"" ShowAtCreation = ""False"" Extension = """" SortOrder = ""0"" IsSpecialized = ""False"" />
        //                             </ContainerEntityTypeAttributeMappingCollection>");
        //            XmlNode rootNode = xmlDoc.SelectSingleNode("ContainerEntityTypeAttributeMappingCollection");

        //            //Get Container Id from XML
        //            Int32 containerId = 0;
        //            Int32.TryParse(rootNode.Attributes["ContainerId"].Value.ToString(), out containerId);

        //            //Get Entity Type Id from XML
        //            Int32 entityTypeId = 0;
        //            Int32.TryParse(rootNode.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);

        //            //Get Attribute Group Id from XML
        //            Int32 attributeGroupId = 0;
        //            Int32.TryParse(rootNode.Attributes["AttributeGroupId"].Value.ToString(), out attributeGroupId);

        //            //Get Attribute Id from XML
        //            Int32 attributeId = 0;
        //            Int32.TryParse(rootNode.Attributes["AttributeId"].Value.ToString(), out attributeId);

        //            //Prepare expected ContainerEntityTypeAttributeMapping Collection from XML
        //            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerEntityTypeAttributeMappingCollection/ContainerEntityTypeAttributeMapping"))
        //            {
        //                containerEntityTypeAttributeMapping = new ContainerEntityTypeAttributeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                containerEntityTypeAttributeMapping.Id = id;
        //                containerEntityTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                containerEntityTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                containerEntityTypeAttributeMapping.Locale = locale;

        //                containerEntityTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 orgId = 0;
        //                Int32.TryParse(node.Attributes["OrganizationId"].Value.ToString(), out orgId);
        //                containerEntityTypeAttributeMapping.OrganizationId = orgId;
        //                containerEntityTypeAttributeMapping.OrganizationName = node.Attributes["OrganizationName"].Value.ToString();

        //                Int32 contId = 0;
        //                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
        //                containerEntityTypeAttributeMapping.ContainerId = contId;
        //                containerEntityTypeAttributeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

        //                Int32 entTypeId = 0;
        //                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
        //                containerEntityTypeAttributeMapping.EntityTypeId = entTypeId;
        //                containerEntityTypeAttributeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

        //                Int32 attrId = 0;
        //                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
        //                containerEntityTypeAttributeMapping.AttributeId = attrId;
        //                containerEntityTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

        //                Int32 attrParentId = 0;
        //                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
        //                containerEntityTypeAttributeMapping.AttributeParentId = attrParentId;
        //                containerEntityTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

        //                Boolean required = false;
        //                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
        //                containerEntityTypeAttributeMapping.Required = required;

        //                Boolean readOnly = false;
        //                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
        //                containerEntityTypeAttributeMapping.ReadOnly = readOnly;

        //                Boolean showAtCreation = false;
        //                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
        //                containerEntityTypeAttributeMapping.ShowAtCreation = showAtCreation;
        //                containerEntityTypeAttributeMapping.Extension = node.Attributes["Extension"].Value.ToString();

        //                Int32 sortOrder = 0;
        //                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
        //                containerEntityTypeAttributeMapping.SortOrder = sortOrder;

        //                expected.Add(containerEntityTypeAttributeMapping);
        //            }

        //            //Get actual mappings
        //            Collection<ContainerEntityTypeAttributeMapping> actual;
        //            actual = target.Get(containerId, entityTypeId, attributeGroupId, attributeId);

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expected, actual);
        //        }

        //        /// <summary>
        //        ///A test for ContainerEntityTypeAttributeMappingBL Constructor
        //        ///</summary>
        //        [TestMethod()]
        //        public void ContainerEntityTypeAttributeMappingBLConstructorTest()
        //        {
        //            ContainerEntityTypeAttributeMappingBL target = new ContainerEntityTypeAttributeMappingBL();
        //            Assert.Inconclusive("TODO: Implement code to verify target");
        //        }
    }
}
