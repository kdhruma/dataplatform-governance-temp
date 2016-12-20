using System;
using System.Xml;
using MDM.DataModelManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.DataModelManager.UnitTest
{
    /// <summary>
    ///This is a test class for RelationshipTypeAttributeMappingBLTest and is intended
    ///to contain all RelationshipTypeAttributeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RelationshipTypeAttributeMappingBLTest
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
        //       [TestMethod()]
        //        public void UpdateTest()
        //        {
        //            //RelationshipTypeAttributeMappingBL target = new RelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
        //            //RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = null; // TODO: Initialize to an appropriate value
        //            //bool expected = false; // TODO: Initialize to an appropriate value
        //            //bool actual;
        //            //actual = target.Update(relationshipTypeAttributeMapping);
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
        //            RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = null;
        //            RelationshipTypeAttributeMappingBL target = new RelationshipTypeAttributeMappingBL();
        //            Collection<RelationshipTypeAttributeMapping> relationshipTypeAttributeMappings = new Collection<RelationshipTypeAttributeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<RelationshipTypeAttributeMappingCollection RelationshipTypeId = """" AttributeGroupId = """" AttributeId = """">
        //	                            <RelationshipTypeAttributeMapping Id = ""166"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Update"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" AttributeId = ""109991"" 
        //                                 AttributeName = ""ABC Code"" AttributeParentId = ""109990"" AttributeParentName = ""Classification"" Required = ""False"" ReadOnly = ""True"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False""/>
        //                                <RelationshipTypeAttributeMapping Id = ""167"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Delete"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" AttributeId = ""109992"" 
        //                                 AttributeName = ""9-Box"" AttributeParentId = ""109990"" AttributeParentName = ""Classification"" Required = ""False"" ReadOnly = ""False"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False""/>
        //                             </RelationshipTypeAttributeMappingCollection>");

        //            //Prepare RelationshipTypeAttributeMapping Collection required for processing
        //            foreach (XmlNode node in xmlDoc.SelectNodes("RelationshipTypeAttributeMappingCollection/RelationshipTypeAttributeMapping"))
        //            {
        //                relationshipTypeAttributeMapping = new RelationshipTypeAttributeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                relationshipTypeAttributeMapping.Id = id;
        //                relationshipTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                relationshipTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                relationshipTypeAttributeMapping.Locale = locale;

        //                relationshipTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 relTypeId = 0;
        //                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
        //                relationshipTypeAttributeMapping.RelationshipTypeId = relTypeId;
        //                relationshipTypeAttributeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

        //                Int32 attrId = 0;
        //                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
        //                relationshipTypeAttributeMapping.AttributeId = attrId;
        //                relationshipTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

        //                Int32 attrParentId = 0;
        //                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
        //                relationshipTypeAttributeMapping.AttributeParentId = attrParentId;
        //                relationshipTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

        //                Boolean required = false;
        //                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
        //                relationshipTypeAttributeMapping.Required = required;

        //                Boolean readOnly = false;
        //                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
        //                relationshipTypeAttributeMapping.ReadOnly = readOnly;

        //                Boolean showAtCreation = false;
        //                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
        //                relationshipTypeAttributeMapping.ShowAtCreation = showAtCreation;

        //                Int32 sortOrder = 0;
        //                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
        //                relationshipTypeAttributeMapping.SortOrder = sortOrder;

        //                Boolean showInline = false;
        //                Boolean.TryParse(node.Attributes["ShowInline"].Value.ToString(), out showInline);
        //                relationshipTypeAttributeMapping.ShowInline = showInline;

        //                relationshipTypeAttributeMappings.Add(relationshipTypeAttributeMapping);
        //            }

        //            //Process mappings and assert the result
        //            Assert.IsTrue(target.Process(relationshipTypeAttributeMappings));
        //        }

        //        /// <summary>
        //        ///A test for GetById
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetByIdTest()
        //        {
        //            //RelationshipTypeAttributeMappingBL target = new RelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
        //            //int id = 0; // TODO: Initialize to an appropriate value
        //            //RelationshipTypeAttributeMapping expected = null; // TODO: Initialize to an appropriate value
        //            //RelationshipTypeAttributeMapping actual;
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
        //            RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = null;
        //            RelationshipTypeAttributeMappingBL target = new RelationshipTypeAttributeMappingBL();
        //            Collection<RelationshipTypeAttributeMapping> expected = new Collection<RelationshipTypeAttributeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<RelationshipTypeAttributeMappingCollection RelationshipTypeId = ""1014"" AttributeGroupId = ""109990"" AttributeId = ""-1"">
        //	                            <RelationshipTypeAttributeMapping Id = ""166"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" AttributeId = ""109991"" 
        //                                 AttributeName = ""ABC Code"" AttributeParentId = ""109990"" AttributeParentName = ""Classification"" Required = ""False"" ReadOnly = ""False"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False""/>
        //                                <RelationshipTypeAttributeMapping Id = ""167"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" AttributeId = ""109992"" 
        //                                 AttributeName = ""9-Box"" AttributeParentId = ""109990"" AttributeParentName = ""Classification"" Required = ""False"" ReadOnly = ""False"" ShowAtCreation = ""False"" SortOrder = ""0"" ShowInline = ""False""/>
        //                             </RelationshipTypeAttributeMappingCollection>");
        //            XmlNode rootNode = xmlDoc.SelectSingleNode("RelationshipTypeAttributeMappingCollection");

        //            //Get Relationship Type Id from XML
        //            Int32 relationshipTypeId = 0;
        //            Int32.TryParse(rootNode.Attributes["RelationshipTypeId"].Value.ToString(), out relationshipTypeId);

        //            //Get Attribute Group Type Id from XML
        //            Int32 attributeGroupId = 0;
        //            Int32.TryParse(rootNode.Attributes["AttributeGroupId"].Value.ToString(), out attributeGroupId);

        //            //Prepare expected RelationshipTypeAttributeMapping Collection from XML
        //            foreach (XmlNode node in xmlDoc.SelectNodes("RelationshipTypeAttributeMappingCollection/RelationshipTypeAttributeMapping"))
        //            {
        //                relationshipTypeAttributeMapping = new RelationshipTypeAttributeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                relationshipTypeAttributeMapping.Id = id;
        //                relationshipTypeAttributeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                relationshipTypeAttributeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                relationshipTypeAttributeMapping.Locale = locale;

        //                relationshipTypeAttributeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 relTypeId = 0;
        //                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
        //                relationshipTypeAttributeMapping.RelationshipTypeId = relTypeId;
        //                relationshipTypeAttributeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

        //                Int32 attrId = 0;
        //                Int32.TryParse(node.Attributes["AttributeId"].Value.ToString(), out attrId);
        //                relationshipTypeAttributeMapping.AttributeId = attrId;
        //                relationshipTypeAttributeMapping.AttributeName = node.Attributes["AttributeName"].Value.ToString();

        //                Int32 attrParentId = 0;
        //                Int32.TryParse(node.Attributes["AttributeParentId"].Value.ToString(), out attrParentId);
        //                relationshipTypeAttributeMapping.AttributeParentId = attrParentId;
        //                relationshipTypeAttributeMapping.AttributeParentName = node.Attributes["AttributeParentName"].Value.ToString();

        //                Boolean required = false;
        //                Boolean.TryParse(node.Attributes["Required"].Value.ToString(), out required);
        //                relationshipTypeAttributeMapping.Required = required;

        //                Boolean readOnly = false;
        //                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
        //                relationshipTypeAttributeMapping.ReadOnly = readOnly;

        //                Boolean showAtCreation = false;
        //                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
        //                relationshipTypeAttributeMapping.ShowAtCreation = showAtCreation;

        //                Int32 sortOrder = 0;
        //                Int32.TryParse(node.Attributes["SortOrder"].Value.ToString(), out sortOrder);
        //                relationshipTypeAttributeMapping.SortOrder = sortOrder;

        //                Boolean showInline = false;
        //                Boolean.TryParse(node.Attributes["ShowInline"].Value.ToString(), out showInline);
        //                relationshipTypeAttributeMapping.ShowInline = showInline;

        //                expected.Add(relationshipTypeAttributeMapping);
        //            }

        //            //Get actual mappings
        //            Collection<RelationshipTypeAttributeMapping> actual;
        //            actual = target.Get(relationshipTypeId, attributeGroupId);

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expected, actual);
        //        }

        //        /// <summary>
        //        ///A test for Delete
        //        ///</summary>
        //        [TestMethod()]
        //        public void DeleteTest()
        //        {
        //            //RelationshipTypeAttributeMappingBL target = new RelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
        //            //RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = null; // TODO: Initialize to an appropriate value
        //            //bool expected = false; // TODO: Initialize to an appropriate value
        //            //bool actual;
        //            //actual = target.Delete(relationshipTypeAttributeMapping);
        //            //Assert.AreEqual(expected, actual);
        //            Assert.Inconclusive("Target Method is not implemented.");
        //        }

        //        /// <summary>
        //        ///A test for Create
        //        ///</summary>
        //        [TestMethod()]
        //        public void CreateTest()
        //        {
        //            //RelationshipTypeAttributeMappingBL target = new RelationshipTypeAttributeMappingBL(); // TODO: Initialize to an appropriate value
        //            //RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = null; // TODO: Initialize to an appropriate value
        //            //bool expected = false; // TODO: Initialize to an appropriate value
        //            //bool actual;
        //            //actual = target.Create(relationshipTypeAttributeMapping);
        //            //Assert.AreEqual(expected, actual);
        //            Assert.Inconclusive("Target Method is not implemented.");
        //        }

        //        /// <summary>
        //        ///A test for RelationshipTypeAttributeMappingBL Constructor
        //        ///</summary>
        //        [TestMethod()]
        //        public void RelationshipTypeAttributeMappingBLConstructorTest()
        //        {
        //            RelationshipTypeAttributeMappingBL target = new RelationshipTypeAttributeMappingBL();
        //            Assert.Inconclusive("TODO: Implement code to verify target");
        //        }
    }
}
