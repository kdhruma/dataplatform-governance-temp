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
    ///This is a test class for ContainerRelationshipTypeEntityTypeMappingBLTest and is intended
    ///to contain all ContainerRelationshipTypeEntityTypeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContainerRelationshipTypeEntityTypeMappingBLTest
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
//            //ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Update(containerRelationshipTypeEntityTypeMapping);
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
//            ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = null;
//            ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL();
//            Collection<ContainerRelationshipTypeEntityTypeMapping> containerRelationshipTypeEntityTypeMappings = new Collection<ContainerRelationshipTypeEntityTypeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<ContainerRelationshipTypeEntityTypeMappingCollection ContainerId = ""-1"" RelationshipTypeId = ""-1"" EntityTypeId = ""-1"">
//	                            <ContainerRelationshipTypeEntityTypeMapping Id = ""22267"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Update"" OrganizationId = ""-1"" OrganizationName = """" ContainerId = ""10113"" ContainerName = ""Test Catalog"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" EntityTypeId = ""6"" EntityTypeName = ""Category"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" DrillDown = ""True"" IsDefaultRelation = ""False"" Excludable = ""False"" ReadOnly = ""False"" ValidationRequired = ""False"" ShowValidFlagInGrid = ""False""/>
//                            </ContainerRelationshipTypeEntityTypeMappingCollection>");
            
//            //Prepare ContainerRelationshipTypeEntityTypeMapping Collection required for processing
//            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerRelationshipTypeEntityTypeMappingCollection/ContainerRelationshipTypeEntityTypeMapping"))
//            {
//                containerRelationshipTypeEntityTypeMapping = new ContainerRelationshipTypeEntityTypeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                containerRelationshipTypeEntityTypeMapping.Id = id;
//                containerRelationshipTypeEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
//                containerRelationshipTypeEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                LocaleEnum locale = LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                containerRelationshipTypeEntityTypeMapping.Locale = locale;

//                containerRelationshipTypeEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 orgId = 0;
//                Int32.TryParse(node.Attributes["OrganizationId"].Value.ToString(), out orgId);
//                containerRelationshipTypeEntityTypeMapping.OrganizationId = orgId;
//                containerRelationshipTypeEntityTypeMapping.OrganizationName = node.Attributes["OrganizationName"].Value.ToString();

//                Int32 contId = 0;
//                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
//                containerRelationshipTypeEntityTypeMapping.ContainerId = contId;
//                containerRelationshipTypeEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                containerRelationshipTypeEntityTypeMapping.RelationshipTypeId = relTypeId;
//                containerRelationshipTypeEntityTypeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                containerRelationshipTypeEntityTypeMapping.EntityTypeId = entTypeId;
//                containerRelationshipTypeEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 parentEntityTypeId = 0;
//                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
//                containerRelationshipTypeEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
//                containerRelationshipTypeEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

//                Boolean drillDown = false;
//                Boolean.TryParse(node.Attributes["DrillDown"].Value.ToString(), out drillDown);
//                containerRelationshipTypeEntityTypeMapping.DrillDown = drillDown;

//                Boolean isDefaultRelation = false;
//                Boolean.TryParse(node.Attributes["IsDefaultRelation"].Value.ToString(), out isDefaultRelation);
//                containerRelationshipTypeEntityTypeMapping.IsDefaultRelation = isDefaultRelation;

//                Boolean excludable = false;
//                Boolean.TryParse(node.Attributes["Excludable"].Value.ToString(), out excludable);
//                containerRelationshipTypeEntityTypeMapping.Excludable = excludable;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                containerRelationshipTypeEntityTypeMapping.ReadOnly = readOnly;

//                Boolean validationRequired = false;
//                Boolean.TryParse(node.Attributes["ValidationRequired"].Value.ToString(), out validationRequired);
//                containerRelationshipTypeEntityTypeMapping.ValidationRequired = validationRequired;

//                Boolean showValidFlagInGrid = false;
//                Boolean.TryParse(node.Attributes["ShowValidFlagInGrid"].Value.ToString(), out showValidFlagInGrid);
//                containerRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid = showValidFlagInGrid;

//                containerRelationshipTypeEntityTypeMappings.Add(containerRelationshipTypeEntityTypeMapping);
//            }

//            //Process mappings and assert the result
//            Assert.IsTrue(target.Process(containerRelationshipTypeEntityTypeMappings));
//        }

//        /// <summary>
//        ///A test for GetMappedEntityTypes
//        ///</summary>
//        [TestMethod()]
//        public void GetMappedEntityTypesTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            EntityType entityType = null;
//            ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL();
//            Collection<EntityType> expected = new Collection<EntityType>();

//            //Load XML
//            xmlDoc.LoadXml(@"<EntityTypeCollection ContainerId = ""10113"" RelationshipTypeId = ""1014"">
//	                            <EntityType Id = ""7"" Name = ""Component"" LongName = ""Component"" Locale = ""en_WW"" Action = ""Read"" ShowAtCreation = ""False"" HasChildNodes = ""False"" CatalogBranchLevel = ""2"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" >
//                                    <ChildEntityTypes/>
//                                </EntityType>
//                                <EntityType Id = ""8"" Name = ""Party"" LongName = ""Party"" Locale = ""en_WW"" Action = ""Read"" ShowAtCreation = ""True"" HasChildNodes = ""False"" CatalogBranchLevel = ""2"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" >
//                                    <ChildEntityTypes/>
//                                </EntityType>
//                            </EntityTypeCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("EntityTypeCollection");

//            //Get Container Id from XML
//            Int32 containerId = 0;
//            Int32.TryParse(rootNode.Attributes["ContainerId"].Value.ToString(), out containerId);

//            //Get RelationshipType Id from XML
//            Int32 relationshipTypeId = 0;
//            Int32.TryParse(rootNode.Attributes["RelationshipTypeId"].Value.ToString(), out relationshipTypeId);

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
//                LocaleEnum locale = LocaleEnum.UnKnown;
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
//                        Enum.TryParse<Core.LocaleEnum>(strLocaleVal, out localeVal);
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

//            //Get actual mappings
//            Collection<EntityType> actual;
//            actual = target.GetMappedEntityTypes(containerId, relationshipTypeId);

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetById
//        ///</summary>
//        [TestMethod()]
//        public void GetByIdTest()
//        {
//            //ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //int id = 0; // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeEntityTypeMapping expected = null; // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeEntityTypeMapping actual;
//            //actual = target.GetById(id);
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
//            ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = null;
//            ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL();
//            Collection<ContainerRelationshipTypeEntityTypeMapping> expected = new Collection<ContainerRelationshipTypeEntityTypeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<ContainerRelationshipTypeEntityTypeMappingCollection ContainerId = ""-1"" RelationshipTypeId = ""-1"" EntityTypeId = ""-1"">
//	                            <ContainerRelationshipTypeEntityTypeMapping Id = ""22267"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" OrganizationId = ""-1"" OrganizationName = """" ContainerId = ""10113"" ContainerName = ""Test Catalog"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" EntityTypeId = ""6"" EntityTypeName = ""Category"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" DrillDown = ""False"" IsDefaultRelation = ""False"" Excludable = ""False"" ReadOnly = ""False"" ValidationRequired = ""False"" ShowValidFlagInGrid = ""False""/>
//                            </ContainerRelationshipTypeEntityTypeMappingCollection>");

//            //Prepare expected ContainerRelationshipTypeEntityTypeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerRelationshipTypeEntityTypeMappingCollection/ContainerRelationshipTypeEntityTypeMapping"))
//            {
//                containerRelationshipTypeEntityTypeMapping = new ContainerRelationshipTypeEntityTypeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                containerRelationshipTypeEntityTypeMapping.Id = id;
//                containerRelationshipTypeEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
//                containerRelationshipTypeEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                containerRelationshipTypeEntityTypeMapping.Locale = locale;

//                containerRelationshipTypeEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 orgId = 0;
//                Int32.TryParse(node.Attributes["OrganizationId"].Value.ToString(), out orgId);
//                containerRelationshipTypeEntityTypeMapping.OrganizationId = orgId;
//                containerRelationshipTypeEntityTypeMapping.OrganizationName = node.Attributes["OrganizationName"].Value.ToString();

//                Int32 contId = 0;
//                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
//                containerRelationshipTypeEntityTypeMapping.ContainerId = contId;
//                containerRelationshipTypeEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                containerRelationshipTypeEntityTypeMapping.RelationshipTypeId = relTypeId;
//                containerRelationshipTypeEntityTypeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                containerRelationshipTypeEntityTypeMapping.EntityTypeId = entTypeId;
//                containerRelationshipTypeEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 parentEntityTypeId = 0;
//                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
//                containerRelationshipTypeEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
//                containerRelationshipTypeEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

//                Boolean drillDown = false;
//                Boolean.TryParse(node.Attributes["DrillDown"].Value.ToString(), out drillDown);
//                containerRelationshipTypeEntityTypeMapping.DrillDown = drillDown;

//                Boolean isDefaultRelation = false;
//                Boolean.TryParse(node.Attributes["IsDefaultRelation"].Value.ToString(), out isDefaultRelation);
//                containerRelationshipTypeEntityTypeMapping.IsDefaultRelation = isDefaultRelation;

//                Boolean excludable = false;
//                Boolean.TryParse(node.Attributes["Excludable"].Value.ToString(), out excludable);
//                containerRelationshipTypeEntityTypeMapping.Excludable = excludable;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                containerRelationshipTypeEntityTypeMapping.ReadOnly = readOnly;

//                Boolean validationRequired = false;
//                Boolean.TryParse(node.Attributes["ValidationRequired"].Value.ToString(), out validationRequired);
//                containerRelationshipTypeEntityTypeMapping.ValidationRequired = validationRequired;

//                Boolean showValidFlagInGrid = false;
//                Boolean.TryParse(node.Attributes["ShowValidFlagInGrid"].Value.ToString(), out showValidFlagInGrid);
//                containerRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid = showValidFlagInGrid;

//                expected.Add(containerRelationshipTypeEntityTypeMapping);
//            }

//            //Get actual mappings
//            Collection<ContainerRelationshipTypeEntityTypeMapping> actual;
//            actual = target.GetAll();

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for Get
//        ///</summary>
//        [TestMethod()]
//        public void GetTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = null;
//            ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL();
//            Collection<ContainerRelationshipTypeEntityTypeMapping> expected = new Collection<ContainerRelationshipTypeEntityTypeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<ContainerRelationshipTypeEntityTypeMappingCollection ContainerId = ""10113"" RelationshipTypeId = ""1014"" EntityTypeId = ""6"">
//	                            <ContainerRelationshipTypeEntityTypeMapping Id = ""22267"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" OrganizationId = ""-1"" OrganizationName = """" ContainerId = ""10113"" ContainerName = ""Test Catalog"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" EntityTypeId = ""6"" EntityTypeName = ""Category"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" DrillDown = ""False"" IsDefaultRelation = ""False"" Excludable = ""False"" ReadOnly = ""False"" ValidationRequired = ""False"" ShowValidFlagInGrid = ""False""/>
//                            </ContainerRelationshipTypeEntityTypeMappingCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("ContainerRelationshipTypeEntityTypeMappingCollection");

//            //Get Container Id from XML
//            Int32 containerId = 0;
//            Int32.TryParse(rootNode.Attributes["ContainerId"].Value.ToString(), out containerId);

//            //Get RelationshipType Id from XML
//            Int32 relationshipTypeId = 0;
//            Int32.TryParse(rootNode.Attributes["RelationshipTypeId"].Value.ToString(), out relationshipTypeId);

//            //Prepare expected ContainerRelationshipTypeEntityTypeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("ContainerRelationshipTypeEntityTypeMappingCollection/ContainerRelationshipTypeEntityTypeMapping"))
//            {
//                containerRelationshipTypeEntityTypeMapping = new ContainerRelationshipTypeEntityTypeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                containerRelationshipTypeEntityTypeMapping.Id = id;
//                containerRelationshipTypeEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
//                containerRelationshipTypeEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                containerRelationshipTypeEntityTypeMapping.Locale = locale;

//                containerRelationshipTypeEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 orgId = 0;
//                Int32.TryParse(node.Attributes["OrganizationId"].Value.ToString(), out orgId);
//                containerRelationshipTypeEntityTypeMapping.OrganizationId = orgId;
//                containerRelationshipTypeEntityTypeMapping.OrganizationName = node.Attributes["OrganizationName"].Value.ToString();

//                Int32 contId = 0;
//                Int32.TryParse(node.Attributes["ContainerId"].Value.ToString(), out contId);
//                containerRelationshipTypeEntityTypeMapping.ContainerId = contId;
//                containerRelationshipTypeEntityTypeMapping.ContainerName = node.Attributes["ContainerName"].Value.ToString();

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                containerRelationshipTypeEntityTypeMapping.RelationshipTypeId = relTypeId;
//                containerRelationshipTypeEntityTypeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                containerRelationshipTypeEntityTypeMapping.EntityTypeId = entTypeId;
//                containerRelationshipTypeEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 parentEntityTypeId = 0;
//                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
//                containerRelationshipTypeEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
//                containerRelationshipTypeEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

//                Boolean drillDown = false;
//                Boolean.TryParse(node.Attributes["DrillDown"].Value.ToString(), out drillDown);
//                containerRelationshipTypeEntityTypeMapping.DrillDown = drillDown;

//                Boolean isDefaultRelation = false;
//                Boolean.TryParse(node.Attributes["IsDefaultRelation"].Value.ToString(), out isDefaultRelation);
//                containerRelationshipTypeEntityTypeMapping.IsDefaultRelation = isDefaultRelation;

//                Boolean excludable = false;
//                Boolean.TryParse(node.Attributes["Excludable"].Value.ToString(), out excludable);
//                containerRelationshipTypeEntityTypeMapping.Excludable = excludable;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                containerRelationshipTypeEntityTypeMapping.ReadOnly = readOnly;

//                Boolean validationRequired = false;
//                Boolean.TryParse(node.Attributes["ValidationRequired"].Value.ToString(), out validationRequired);
//                containerRelationshipTypeEntityTypeMapping.ValidationRequired = validationRequired;

//                Boolean showValidFlagInGrid = false;
//                Boolean.TryParse(node.Attributes["ShowValidFlagInGrid"].Value.ToString(), out showValidFlagInGrid);
//                containerRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid = showValidFlagInGrid;

//                expected.Add(containerRelationshipTypeEntityTypeMapping);
//            }

//            //Get actual mappings
//            Collection<ContainerRelationshipTypeEntityTypeMapping> actual;
//            actual = target.Get(containerId, relationshipTypeId);

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for Delete
//        ///</summary>
//        [TestMethod()]
//        public void DeleteTest()
//        {
//            //ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Delete(containerRelationshipTypeEntityTypeMapping);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for Create
//        ///</summary>
//        [TestMethod()]
//        public void CreateTest()
//        {
//            //ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Create(containerRelationshipTypeEntityTypeMapping);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for ContainerRelationshipTypeEntityTypeMappingBL Constructor
//        ///</summary>
//        [TestMethod()]
//        public void ContainerRelationshipTypeEntityTypeMappingBLConstructorTest()
//        {
//            ContainerRelationshipTypeEntityTypeMappingBL target = new ContainerRelationshipTypeEntityTypeMappingBL();
//            Assert.Inconclusive("TODO: Implement code to verify target");
//        }
    }
}
