using System;
using System.Xml;
using MDM.DataModelManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.DataModelManager.UnitTest
{
    /// <summary>
    ///This is a test class for ContainerRelationshipTypeAttributeMappingBLTest and is intended
    ///to contain all ContainerRelationshipTypeAttributeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContainerRelationshipTypeAttributeMappingBLTest
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
        ///A test for Update
        ///</summary>
//        [TestMethod()]
//        public void UpdateTest()
//        {
//            //ContainerRelationshipTypeAttributeMappingBL target = new ContainerRelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Update(containerRelationshipTypeAttributeMapping);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for Process
//        ///</summary>
//        [TestMethod()]
//        public void ProcessTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = null;
//            ContainerRelationshipTypeAttributeMappingBL target = new ContainerRelationshipTypeAttributeMappingBL();
//            Collection<ContainerRelationshipTypeAttributeMapping> containerRelationshipTypeAttributeMappings = new Collection<ContainerRelationshipTypeAttributeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<ContainerRelationshipTypeAttributeMappingCollection ContainerId = ""-1"" RelationshipTypeId = ""-1"" AttributeGroupId = ""-1"" AttributeId = ""-1"">
//	                            <ContainerRelationshipTypeAttributeMapping Id = ""-1"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Create"" OrganizationId = ""-1"" OrganizationName = """" ContainerId = ""10113"" ContainerName = """" RelationshipTypeId = ""1014"" RelationshipTypeName = """" AttributeId = ""109991"" AttributeName = """" AttributeParentId = ""109990"" 	AttributeParentName = """" Required = ""False"" ReadOnly = ""False"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False"" IsSpecialized = ""False""/>
//                                <ContainerRelationshipTypeAttributeMapping Id = ""118965"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Update"" OrganizationId = ""-1"" OrganizationName = """" ContainerId = ""10113"" ContainerName = ""Test Catalog"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" AttributeId = ""109992"" AttributeName = ""9-Box"" AttributeParentId = ""109990"" 	AttributeParentName = ""Classification"" Required = ""True"" ReadOnly = ""False"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False"" IsSpecialized = ""True""/>
//                             </ContainerRelationshipTypeAttributeMappingCollection>");

//            //Prepare ContainerRelationshipTypeAttributeMapping Collection required for processing
//            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerRelationshipTypeAttributeMappingCollection/ContainerRelationshipTypeAttributeMapping"))
//            {
//                containerRelationshipTypeAttributeMapping = new ContainerRelationshipTypeAttributeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                containerRelationshipTypeAttributeMapping.Id = id;
//                containerRelationshipTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
//                containerRelationshipTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                LocaleEnum locale = LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                containerRelationshipTypeAttributeMapping.Locale = locale;

//                containerRelationshipTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 orgId = 0;
//                Int32.TryParse(node.Attributes["OrganizationId"].Value.ToString(), out orgId);
//                containerRelationshipTypeAttributeMapping.OrganizationId = orgId;
//                containerRelationshipTypeAttributeMapping.OrganizationName = node.Attributes["OrganizationName"].Value.ToString();

//                Int32 contId = 0;
//                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
//                containerRelationshipTypeAttributeMapping.ContainerId = contId;
//                containerRelationshipTypeAttributeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                containerRelationshipTypeAttributeMapping.RelationshipTypeId = relTypeId;
//                containerRelationshipTypeAttributeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 attrId = 0;
//                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
//                containerRelationshipTypeAttributeMapping.AttributeId = attrId;
//                containerRelationshipTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

//                Int32 attrParentId = 0;
//                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
//                containerRelationshipTypeAttributeMapping.AttributeParentId = attrParentId;
//                containerRelationshipTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

//                Boolean required = false;
//                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
//                containerRelationshipTypeAttributeMapping.Required = required;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                containerRelationshipTypeAttributeMapping.ReadOnly = readOnly;

//                Boolean showAtCreation = false;
//                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
//                containerRelationshipTypeAttributeMapping.ShowAtCreation = showAtCreation;

//                Int32 sortOrder = 0;
//                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
//                containerRelationshipTypeAttributeMapping.SortOrder = sortOrder;

//                Boolean showInline = false;
//                Boolean.TryParse(node.Attributes["ShowInline"].Value.ToString(), out showInline);
//                containerRelationshipTypeAttributeMapping.ShowInline = showInline;

//                containerRelationshipTypeAttributeMappings.Add(containerRelationshipTypeAttributeMapping);
//            }

//            //Process mappings and assert the result
//            Assert.IsTrue(target.Process(containerRelationshipTypeAttributeMappings));
//        }

//        /// <summary>
//        ///A test for GetById
//        ///</summary>
//        [TestMethod()]
//        public void GetByIdTest()
//        {
//            //ContainerRelationshipTypeAttributeMappingBL target = new ContainerRelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
//            //int id = 0; // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeAttributeMapping expected = null; // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeAttributeMapping actual;
//            //actual = target.GetById(id);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for Get
//        ///</summary>
//        [TestMethod()]
//        public void GetTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = null;
//            ContainerRelationshipTypeAttributeMappingBL target = new ContainerRelationshipTypeAttributeMappingBL(); 
//            Collection<ContainerRelationshipTypeAttributeMapping> expected = new Collection<ContainerRelationshipTypeAttributeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<ContainerRelationshipTypeAttributeMappingCollection ContainerId = ""10113"" RelationshipTypeId = ""1014"" AttributeGroupId = ""109990"" AttributeId = ""-1"">
//	                            <ContainerRelationshipTypeAttributeMapping Id = ""118960"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" OrganizationId = ""-1"" OrganizationName = """" ContainerId = ""10113"" ContainerName = ""Test Catalog"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" AttributeId = ""109991"" AttributeName = ""ABC Code"" AttributeParentId = ""109990"" 	AttributeParentName = ""Classification"" Required = ""False"" ReadOnly = ""False"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False"" IsSpecialized = ""False""/>
//                                <ContainerRelationshipTypeAttributeMapping Id = ""118965"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" OrganizationId = ""-1"" OrganizationName = """" ContainerId = ""10113"" ContainerName = ""Test Catalog"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" AttributeId = ""109992"" AttributeName = ""9-Box"" AttributeParentId = ""109990"" 	AttributeParentName = ""Classification"" Required = ""False"" ReadOnly = ""False"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False"" IsSpecialized = ""False""/>
//                             </ContainerRelationshipTypeAttributeMappingCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("ContainerRelationshipTypeAttributeMappingCollection");

//            //Get Container Id from XML
//            Int32 containerId = 0;
//            Int32.TryParse(rootNode.Attributes["ContainerId"].Value.ToString(), out containerId);

//            //Get Relationship Type Id from XML
//            Int32 relationshipTypeId = 0;
//            Int32.TryParse(rootNode.Attributes["RelationshipTypeId"].Value.ToString(), out relationshipTypeId);

//            //Get Attribute Group Type Id from XML
//            Int32 attributeGroupId = 0;
//            Int32.TryParse(rootNode.Attributes["AttributeGroupId"].Value.ToString(), out attributeGroupId);

//            //Prepare expected ContainerRelationshipTypeAttributeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerRelationshipTypeAttributeMappingCollection/ContainerRelationshipTypeAttributeMapping"))
//            {
//                containerRelationshipTypeAttributeMapping = new ContainerRelationshipTypeAttributeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                containerRelationshipTypeAttributeMapping.Id = id;
//                containerRelationshipTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
//                containerRelationshipTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                LocaleEnum locale = LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                containerRelationshipTypeAttributeMapping.Locale = locale;

//                containerRelationshipTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 orgId = 0;
//                Int32.TryParse(node.Attributes["OrganizationId"].Value.ToString(), out orgId);
//                containerRelationshipTypeAttributeMapping.OrganizationId = orgId;
//                containerRelationshipTypeAttributeMapping.OrganizationName = node.Attributes["OrganizationName"].Value.ToString();

//                Int32 contId = 0;
//                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
//                containerRelationshipTypeAttributeMapping.ContainerId = contId;
//                containerRelationshipTypeAttributeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                containerRelationshipTypeAttributeMapping.RelationshipTypeId = relTypeId;
//                containerRelationshipTypeAttributeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 attrId = 0;
//                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
//                containerRelationshipTypeAttributeMapping.AttributeId = attrId;
//                containerRelationshipTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

//                Int32 attrParentId = 0;
//                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
//                containerRelationshipTypeAttributeMapping.AttributeParentId = attrParentId;
//                containerRelationshipTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

//                Boolean required = false;
//                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
//                containerRelationshipTypeAttributeMapping.Required = required;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                containerRelationshipTypeAttributeMapping.ReadOnly = readOnly;

//                Boolean showAtCreation = false;
//                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
//                containerRelationshipTypeAttributeMapping.ShowAtCreation = showAtCreation;

//                Int32 sortOrder = 0;
//                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
//                containerRelationshipTypeAttributeMapping.SortOrder = sortOrder;

//                Boolean showInline = false;
//                Boolean.TryParse(node.Attributes["ShowInline"].Value.ToString(), out showInline);
//                containerRelationshipTypeAttributeMapping.ShowInline = showInline;

//                expected.Add(containerRelationshipTypeAttributeMapping);
//            }

//            //Get actual mappings
//            Collection<ContainerRelationshipTypeAttributeMapping> actual;
//            actual = target.Get(containerId, relationshipTypeId, attributeGroupId);

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for Delete
//        ///</summary>
//        [TestMethod()]
//        public void DeleteTest()
//        {
//            //ContainerRelationshipTypeAttributeMappingBL target = new ContainerRelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Delete(containerRelationshipTypeAttributeMapping);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for Create
//        ///</summary>
//        [TestMethod()]
//        public void CreateTest()
//        {
//            //ContainerRelationshipTypeAttributeMappingBL target = new ContainerRelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Create(containerRelationshipTypeAttributeMapping);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for ContainerRelationshipTypeAttributeMappingBL Constructor
//        ///</summary>
//        [TestMethod()]
//        public void ContainerRelationshipTypeAttributeMappingBLConstructorTest()
//        {
//            ContainerRelationshipTypeAttributeMappingBL target = new ContainerRelationshipTypeAttributeMappingBL();
//            Assert.Inconclusive("TODO: Implement code to verify target");
//        }
    }
}
