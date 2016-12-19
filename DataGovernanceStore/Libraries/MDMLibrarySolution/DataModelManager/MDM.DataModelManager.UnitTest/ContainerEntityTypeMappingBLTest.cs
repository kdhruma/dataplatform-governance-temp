using System;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MDM.BusinessObjects;
using MDM.DataModelManager.Business;
using MDM.AdminManager.Business;
using MDM.Core;

namespace MDM.DataModelManager.UnitTest
{
    /// <summary>
    ///This is a test class for ContainerEntityTypeMappingBLTest and is intended
    ///to contain all ContainerEntityTypeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContainerEntityTypeMappingBLTest
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
        //            ContainerEntityTypeMapping containerEntityTypeMapping = null;
        //            ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL();
        //            Collection<ContainerEntityTypeMapping> containerEntityTypeMappings = new Collection<ContainerEntityTypeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<ContainerEntityTypeMappingCollection ContainerId = """" EntityTypeId = """">
        //	                            <ContainerEntityTypeMapping Id = ""1603"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Delete"" ContainerId = ""-1"" ContainerName = """" EntityTypeId = ""-1"" EntityTypeName = """" ParentEntityTypeId = ""-1"" ParentEntityTypeName = """" />
        //                                <ContainerEntityTypeMapping Id = ""-1"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Create"" ContainerId = ""10113"" ContainerName = """" EntityTypeId = ""7"" EntityTypeName = """" ParentEntityTypeId = ""-1"" ParentEntityTypeName = """" />
        //                            </ContainerEntityTypeMappingCollection>");

        //            //Prepare ContainerEntityTypeMapping Collection required for processing
        //            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerEntityTypeMappingCollection/ContainerEntityTypeMapping"))
        //            {
        //                containerEntityTypeMapping = new ContainerEntityTypeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                containerEntityTypeMapping.Id = id;
        //                containerEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                containerEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                LocaleEnum locale = LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                containerEntityTypeMapping.Locale = locale;

        //                containerEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 contId = 0;
        //                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
        //                containerEntityTypeMapping.ContainerId = contId;
        //                containerEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

        //                Int32 entityTypeId = 0;
        //                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);
        //                containerEntityTypeMapping.EntityTypeId = entityTypeId;
        //                containerEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

        //                Int32 parentEntityTypeId = 0;
        //                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
        //                containerEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
        //                containerEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

        //                containerEntityTypeMappings.Add(containerEntityTypeMapping);
        //            }

        //            //Process mappings and assert the result
        //            Assert.IsTrue(target.Process(containerEntityTypeMappings));
        //        }

        //        /// <summary>
        //        ///A test for GetMappingsByEntityTypeId
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetMappingsByEntityTypeIdTest()
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            ContainerEntityTypeMapping containerEntityTypeMapping = null;
        //            ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL();
        //            Collection<ContainerEntityTypeMapping> expected = new Collection<ContainerEntityTypeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<ContainerEntityTypeMappingCollection ContainerId = ""-1"" EntityTypeId = ""7"">
        //	                            <ContainerEntityTypeMapping Id = ""5951"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" ContainerId = ""102"" ContainerName = ""GTN WEB"" EntityTypeId = ""7"" EntityTypeName = ""Product"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" />
        //                            </ContainerEntityTypeMappingCollection>");
        //            XmlNode rootNode = xmlDoc.SelectSingleNode("ContainerEntityTypeMappingCollection");

        //            //Get entity type Id from XML
        //            Int32 entityTypeId = 0;
        //            Int32.TryParse(rootNode.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);

        //            //Prepare expected ContainerEntityTypeMapping Collection from XML
        //            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerEntityTypeMappingCollection/ContainerEntityTypeMapping"))
        //            {
        //                containerEntityTypeMapping = new ContainerEntityTypeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                containerEntityTypeMapping.Id = id;
        //                containerEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                containerEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                containerEntityTypeMapping.Locale = locale;

        //                containerEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 contId = 0;
        //                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
        //                containerEntityTypeMapping.ContainerId = contId;
        //                containerEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

        //                Int32 entTypeId = 0;
        //                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
        //                containerEntityTypeMapping.EntityTypeId = entTypeId;
        //                containerEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

        //                Int32 parentEntityTypeId = 0;
        //                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
        //                containerEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
        //                containerEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

        //                expected.Add(containerEntityTypeMapping);
        //            }

        //            //Get actual mappings
        //            Collection<ContainerEntityTypeMapping> actual;
        //            actual = target.GetMappingsByEntityTypeId(entityTypeId);

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expected, actual);
        //        }

        //        /// <summary>
        //        ///A test for GetMappingsByContainerId
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetMappingsByContainerIdTest()
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            ContainerEntityTypeMapping containerEntityTypeMapping = null;
        //            ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL();
        //            Collection<ContainerEntityTypeMapping> expected = new Collection<ContainerEntityTypeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<ContainerEntityTypeMappingCollection ContainerId = ""10113"" EntityTypeId = ""-1"">
        //	                            <ContainerEntityTypeMapping Id = ""1597"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" ContainerId = ""0"" ContainerName = ""All"" EntityTypeId = ""8"" EntityTypeName = ""Party"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" />
        //                                <ContainerEntityTypeMapping Id = ""1604"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" ContainerId = ""10113"" ContainerName = ""Test Catalog"" EntityTypeId = ""7"" EntityTypeName = ""Component"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" />
        //                            </ContainerEntityTypeMappingCollection>");
        //            XmlNode rootNode = xmlDoc.SelectSingleNode("ContainerEntityTypeMappingCollection");

        //            //Get container Id from XML
        //            Int32 containerId = 0;
        //            Int32.TryParse(rootNode.Attributes["ContainerId"].Value.ToString(), out containerId);

        //            //Prepare expected ContainerEntityTypeMapping Collection from XML
        //            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerEntityTypeMappingCollection/ContainerEntityTypeMapping"))
        //            {
        //                containerEntityTypeMapping = new ContainerEntityTypeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                containerEntityTypeMapping.Id = id;
        //                containerEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                containerEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                containerEntityTypeMapping.Locale = locale;

        //                containerEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 contId = 0;
        //                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
        //                containerEntityTypeMapping.ContainerId = contId;
        //                containerEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

        //                Int32 entityTypeId = 0;
        //                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);
        //                containerEntityTypeMapping.EntityTypeId = entityTypeId;
        //                containerEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

        //                Int32 parentEntityTypeId = 0;
        //                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
        //                containerEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
        //                containerEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

        //                expected.Add(containerEntityTypeMapping);
        //            }

        //            //Get actual mappings
        //            Collection<ContainerEntityTypeMapping> actual;
        //            actual = target.GetMappingsByContainerId(containerId);

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expected, actual);
        //        }

        //        /// <summary>
        //        ///A test for Get Mappings for 'AllOrganization' and 'AllContainer' request
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetMappingsForAllContainerRequestTest()
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            ContainerEntityTypeMapping containerEntityTypeMapping = null;
        //            ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL();
        //            Collection<ContainerEntityTypeMapping> expected = new Collection<ContainerEntityTypeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<ContainerEntityTypeMappingCollection ContainerId = ""0"" EntityTypeId = ""-1"">
        //	                            <ContainerEntityTypeMapping Id = ""1597"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" ContainerId = ""0"" ContainerName = ""All"" EntityTypeId = ""8"" EntityTypeName = ""Party"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" />
        //                            </ContainerEntityTypeMappingCollection>");
        //            XmlNode rootNode = xmlDoc.SelectSingleNode("ContainerEntityTypeMappingCollection");

        //            //Get container Id from XML
        //            Int32 containerId = 0;
        //            Int32.TryParse(rootNode.Attributes["ContainerId"].Value.ToString(), out containerId);

        //            //Prepare expected ContainerEntityTypeMapping Collection from XML
        //            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerEntityTypeMappingCollection/ContainerEntityTypeMapping"))
        //            {
        //                containerEntityTypeMapping = new ContainerEntityTypeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                containerEntityTypeMapping.Id = id;
        //                containerEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                containerEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                containerEntityTypeMapping.Locale = locale;

        //                containerEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 contId = 0;
        //                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
        //                containerEntityTypeMapping.ContainerId = contId;
        //                containerEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

        //                Int32 entityTypeId = 0;
        //                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);
        //                containerEntityTypeMapping.EntityTypeId = entityTypeId;
        //                containerEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

        //                Int32 parentEntityTypeId = 0;
        //                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
        //                containerEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
        //                containerEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

        //                expected.Add(containerEntityTypeMapping);
        //            }

        //            //Get actual mappings
        //            Collection<ContainerEntityTypeMapping> actual;
        //            actual = target.GetMappingsByContainerId(containerId);

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expected, actual);
        //        }

        //        /// <summary>
        //        ///A test for GetMappedEntityTypes
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetMappedEntityTypesTest()
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            EntityType entityType = null;
        //            ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL();
        //            Collection<EntityType> expected = new Collection<EntityType>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<EntityTypeCollection ContainerId = ""10113"">
        //	                            <EntityType Id = ""7"" Name = ""Component"" LongName = ""Component"" Locale = ""en_WW"" Action = ""Read"" ShowAtCreation = ""False"" HasChildNodes = ""False"" CatalogBranchLevel = ""2"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" >
        //                                    <ChildEntityTypes>
        //                                        <EntityType Id = ""10022"" Name = ""Tst"" LongName = ""Test"" Locale = ""en_WW"" Action = ""Read"" ShowAtCreation = ""False"" HasChildNodes = ""False"" CatalogBranchLevel = ""2"" ParentEntityTypeId = ""7"" ParentEntityTypeName = ""Component"" />
        //                                    </ChildEntityTypes>
        //                                </EntityType>
        //                                <EntityType Id = ""8"" Name = ""Party"" LongName = ""Party"" Locale = ""en_WW"" Action = ""Read"" ShowAtCreation = ""True"" HasChildNodes = ""False"" CatalogBranchLevel = ""2"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" >
        //                                    <ChildEntityTypes/>
        //                                </EntityType>
        //                            </EntityTypeCollection>");
        //            XmlNode rootNode = xmlDoc.SelectSingleNode("EntityTypeCollection");

        //            //Get container Id from XML
        //            Int32 containerId = 0;
        //            Int32.TryParse(rootNode.Attributes["ContainerId"].Value.ToString(), out containerId);

        //            //Prepare expected EntityType Collection from XML
        //            foreach (XmlNode node in xmlDoc.SelectNodes("EntityTypeCollection/EntityType"))
        //            {
        //                entityType = new EntityType();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                entityType.Id = id;
        //                entityType.Name = node.Attributes["Name"].Value.ToString();
        //                entityType.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                entityType.Locale = locale;

        //                entityType.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Boolean showAtCreation = false;
        //                Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
        //                entityType.ShowAtCreation = showAtCreation;

        //                Boolean hasChildNodes = false;
        //                Boolean.TryParse(node.Attributes["HasChildNodes"].Value.ToString(), out hasChildNodes);
        //                entityType.HasChildNodes = hasChildNodes;

        //                Int32 catalogBranchLevel = 0;
        //                Int32.TryParse(node.Attributes["CatalogBranchLevel"].Value.ToString(), out catalogBranchLevel);
        //                entityType.CatalogBranchLevel = catalogBranchLevel;

        //                Int32 parentEntityTypeId = 0;
        //                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
        //                entityType.ParentEntityTypeId = parentEntityTypeId;
        //                entityType.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

        //                //Check whether entity type is having child entity types
        //                XmlNode childNodeCollection = node.FirstChild;
        //                if (childNodeCollection != null && childNodeCollection.HasChildNodes)
        //                {
        //                    //Yes.. there are child entity types
        //                    //Prepare child entity type collection
        //                    foreach (XmlNode childNode in childNodeCollection.ChildNodes)
        //                    {
        //                        EntityType childEntityType = new EntityType();

        //                        id = 0;
        //                        Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                        childEntityType.Id = id;
        //                        childEntityType.Name = node.Attributes["Name"].Value.ToString();
        //                        childEntityType.LongName = node.Attributes["LongName"].Value.ToString();

        //                        String strLocaleVal = node.Attributes["Locale"].Value.ToString();
        //                        Core.LocaleEnum localeVal = Core.LocaleEnum.UnKnown;
        //                        Enum.TryParse<Core.LocaleEnum>(strLocale, out localeVal);
        //                        childEntityType.Locale = localeVal;

        //                        childEntityType.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                        showAtCreation = false;
        //                        Boolean.TryParse(node.Attributes["ShowAtCreation"].Value.ToString(), out showAtCreation);
        //                        childEntityType.ShowAtCreation = showAtCreation;

        //                        hasChildNodes = false;
        //                        Boolean.TryParse(node.Attributes["HasChildNodes"].Value.ToString(), out hasChildNodes);
        //                        childEntityType.HasChildNodes = hasChildNodes;

        //                        catalogBranchLevel = 0;
        //                        Int32.TryParse(node.Attributes["CatalogBranchLevel"].Value.ToString(), out catalogBranchLevel);
        //                        childEntityType.CatalogBranchLevel = catalogBranchLevel;

        //                        parentEntityTypeId = 0;
        //                        Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
        //                        childEntityType.ParentEntityTypeId = parentEntityTypeId;
        //                        childEntityType.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

        //                        entityType.EntityTypes.Add(childEntityType);
        //                    }
        //                }

        //                expected.Add(entityType);
        //            }

        //            //Get actual entity types mapped to container Id
        //            Collection<EntityType> actual;
        //            actual = target.GetMappedEntityTypes(containerId);

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expected, actual);
        //        }

        //        /// <summary>
        //        ///A test for GetById
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetByIdTest()
        //        {
        //            //ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
        //            //int id = 0; // TODO: Initialize to an appropriate value
        //            //ContainerEntityTypeMapping expected = null; // TODO: Initialize to an appropriate value
        //            //ContainerEntityTypeMapping actual;
        //            //actual = target.GetById(id); // Not Implemented
        //            //Assert.AreEqual(expected, actual);
        //            Assert.Inconclusive("Target Method is not implemented.");
        //        }

        //        /// <summary>
        //        ///A test for GetAll
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetAllTest()
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            ContainerEntityTypeMapping containerEntityTypeMapping = null;
        //            ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL();
        //            Collection<ContainerEntityTypeMapping> expected = new Collection<ContainerEntityTypeMapping>();

        //            //Load XML
        //            xmlDoc.LoadXml(@"<ContainerEntityTypeMappingCollection ContainerId = ""-1"" EntityTypeId = ""-1"">
        //	                            <ContainerEntityTypeMapping Id = ""5951"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" ContainerId = ""102"" ContainerName = ""GTN WEB"" EntityTypeId = ""7"" EntityTypeName = ""Product"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" />
        //                            </ContainerEntityTypeMappingCollection>");
        //            XmlNode rootNode = xmlDoc.SelectSingleNode("ContainerEntityTypeMappingCollection");

        //            //Prepare expected ContainerEntityTypeMapping Collection from XML
        //            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerEntityTypeMappingCollection/ContainerEntityTypeMapping"))
        //            {
        //                containerEntityTypeMapping = new ContainerEntityTypeMapping();

        //                Int32 id = 0;
        //                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
        //                containerEntityTypeMapping.Id = id;
        //                containerEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
        //                containerEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

        //                String strLocale = node.Attributes["Locale"].Value.ToString();
        //                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
        //                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
        //                containerEntityTypeMapping.Locale = locale;

        //                containerEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

        //                Int32 contId = 0;
        //                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
        //                containerEntityTypeMapping.ContainerId = contId;
        //                containerEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

        //                Int32 entityTypeId = 0;
        //                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);
        //                containerEntityTypeMapping.EntityTypeId = entityTypeId;
        //                containerEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

        //                Int32 parentEntityTypeId = 0;
        //                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
        //                containerEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
        //                containerEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

        //                expected.Add(containerEntityTypeMapping);
        //            }

        //            //Get actual mappings
        //            Collection<ContainerEntityTypeMapping> actual;
        //            actual = target.GetAll();

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expected, actual);
        //        }

        //        /// <summary>
        //        ///A test for ContainerEntityTypeMappingBL Constructor
        //        ///</summary>
        //        [TestMethod()]
        //        public void ContainerEntityTypeMappingBLConstructorTest()
        //        {
        //            ContainerEntityTypeMappingBL target = new ContainerEntityTypeMappingBL();
        //            Assert.Inconclusive("TODO: Implement code to verify target");
        //        }
    }
}
