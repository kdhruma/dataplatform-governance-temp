using System;
using System.Xml;
using MDM.DataModelManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.DataModelManager.UnitTest
{
    /// <summary>
    ///This is a test class for RelationshipTypeEntityTypeMappingBLTest and is intended
    ///to contain all RelationshipTypeEntityTypeMappingBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RelationshipTypeEntityTypeMappingBLTest
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
//            //RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Update(relationshipTypeEntityTypeMapping);
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
//            RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = null;
//            RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL();
//            Collection<RelationshipTypeEntityTypeMapping> relationshipTypeEntityTypeMappings = new Collection<RelationshipTypeEntityTypeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<RelationshipTypeEntityTypeMappingCollection RelationshipTypeId = ""-1"" EntityTypeId = ""-1"">
//	                            <RelationshipTypeEntityTypeMapping Id = ""102"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Update"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" EntityTypeId = ""6"" EntityTypeName = ""Category"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" DrillDown = ""True"" IsDefaultRelation = ""False"" Excludable = ""False"" ReadOnly = ""False"" ValidationRequired = ""False"" ShowValidFlagInGrid = ""False""/>
//                             </RelationshipTypeEntityTypeMappingCollection>");

//            //Prepare RelationshipTypeEntityTypeMapping Collection required for processing
//            foreach (XmlNode node in xmlDoc.SelectNodes("RelationshipTypeEntityTypeMappingCollection/RelationshipTypeEntityTypeMapping"))
//            {
//                relationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                relationshipTypeEntityTypeMapping.Id = id;
//                relationshipTypeEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
//                relationshipTypeEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                relationshipTypeEntityTypeMapping.Locale = locale;

//                relationshipTypeEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                relationshipTypeEntityTypeMapping.RelationshipTypeId = relTypeId;
//                relationshipTypeEntityTypeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                relationshipTypeEntityTypeMapping.EntityTypeId = entTypeId;
//                relationshipTypeEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 parentEntityTypeId = 0;
//                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
//                relationshipTypeEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
//                relationshipTypeEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

//                Boolean drillDown = false;
//                Boolean.TryParse(node.Attributes["DrillDown"].Value.ToString(), out drillDown);
//                relationshipTypeEntityTypeMapping.DrillDown = drillDown;

//                Boolean isDefaultRelation = false;
//                Boolean.TryParse(node.Attributes["IsDefaultRelation"].Value.ToString(), out isDefaultRelation);
//                relationshipTypeEntityTypeMapping.IsDefaultRelation = isDefaultRelation;

//                Boolean excludable = false;
//                Boolean.TryParse(node.Attributes["Excludable"].Value.ToString(), out excludable);
//                relationshipTypeEntityTypeMapping.Excludable = excludable;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                relationshipTypeEntityTypeMapping.ReadOnly = readOnly;

//                Boolean validationRequired = false;
//                Boolean.TryParse(node.Attributes["ValidationRequired"].Value.ToString(), out validationRequired);
//                relationshipTypeEntityTypeMapping.ValidationRequired = validationRequired;

//                Boolean showValidFlagInGrid = false;
//                Boolean.TryParse(node.Attributes["ShowValidFlagInGrid"].Value.ToString(), out showValidFlagInGrid);
//                relationshipTypeEntityTypeMapping.ShowValidFlagInGrid = showValidFlagInGrid;

//                relationshipTypeEntityTypeMappings.Add(relationshipTypeEntityTypeMapping);
//            }

//            //Process mappings and assert the result
//            Assert.IsTrue(target.Process(relationshipTypeEntityTypeMappings));
//        }

//        /// <summary>
//        ///A test for GetMappingsByRelationshipTypeId
//        ///</summary>
//        [TestMethod()]
//        public void GetMappingsByRelationshipTypeIdTest()
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = null;
//            RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL();
//            Collection<RelationshipTypeEntityTypeMapping> expected = new Collection<RelationshipTypeEntityTypeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<RelationshipTypeEntityTypeMappingCollection RelationshipTypeId = ""1014"" EntityTypeId = ""6"">
//	                            <RelationshipTypeEntityTypeMapping Id = ""102"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" EntityTypeId = ""6"" EntityTypeName = ""Category"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" DrillDown = ""False"" IsDefaultRelation = ""False"" Excludable = ""False"" ReadOnly = ""False"" ValidationRequired = ""False"" ShowValidFlagInGrid = ""False""/>
//                             </RelationshipTypeEntityTypeMappingCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("RelationshipTypeEntityTypeMappingCollection");

//            //Get RelationshipType Id from XML
//            Int32 relationshipTypeId = 0;
//            Int32.TryParse(rootNode.Attributes["RelationshipTypeId"].Value.ToString(), out relationshipTypeId);

//            //Prepare expected RelationshipTypeEntityTypeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("RelationshipTypeEntityTypeMappingCollection/RelationshipTypeEntityTypeMapping"))
//            {
//                relationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                relationshipTypeEntityTypeMapping.Id = id;
//                relationshipTypeEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
//                relationshipTypeEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                relationshipTypeEntityTypeMapping.Locale = locale;

//                relationshipTypeEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                relationshipTypeEntityTypeMapping.RelationshipTypeId = relTypeId;
//                relationshipTypeEntityTypeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                relationshipTypeEntityTypeMapping.EntityTypeId = entTypeId;
//                relationshipTypeEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 parentEntityTypeId = 0;
//                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
//                relationshipTypeEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
//                relationshipTypeEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

//                Boolean drillDown = false;
//                Boolean.TryParse(node.Attributes["DrillDown"].Value.ToString(), out drillDown);
//                relationshipTypeEntityTypeMapping.DrillDown = drillDown;

//                Boolean isDefaultRelation = false;
//                Boolean.TryParse(node.Attributes["IsDefaultRelation"].Value.ToString(), out isDefaultRelation);
//                relationshipTypeEntityTypeMapping.IsDefaultRelation = isDefaultRelation;

//                Boolean excludable = false;
//                Boolean.TryParse(node.Attributes["Excludable"].Value.ToString(), out excludable);
//                relationshipTypeEntityTypeMapping.Excludable = excludable;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                relationshipTypeEntityTypeMapping.ReadOnly = readOnly;

//                Boolean validationRequired = false;
//                Boolean.TryParse(node.Attributes["ValidationRequired"].Value.ToString(), out validationRequired);
//                relationshipTypeEntityTypeMapping.ValidationRequired = validationRequired;

//                Boolean showValidFlagInGrid = false;
//                Boolean.TryParse(node.Attributes["ShowValidFlagInGrid"].Value.ToString(), out showValidFlagInGrid);
//                relationshipTypeEntityTypeMapping.ShowValidFlagInGrid = showValidFlagInGrid;

//                expected.Add(relationshipTypeEntityTypeMapping);
//            }

//            //Get actual mappings
//            Collection<RelationshipTypeEntityTypeMapping> actual;
//            actual = target.GetMappingsByRelationshipTypeId(relationshipTypeId);

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
//            RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = null;
//            RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL();
//            Collection<RelationshipTypeEntityTypeMapping> expected = new Collection<RelationshipTypeEntityTypeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<RelationshipTypeEntityTypeMappingCollection RelationshipTypeId = ""1014"" EntityTypeId = ""6"">
//	                            <RelationshipTypeEntityTypeMapping Id = ""102"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" EntityTypeId = ""6"" EntityTypeName = ""Category"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" DrillDown = ""False"" IsDefaultRelation = ""False"" Excludable = ""False"" ReadOnly = ""False"" ValidationRequired = ""False"" ShowValidFlagInGrid = ""False""/>
//                             </RelationshipTypeEntityTypeMappingCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("RelationshipTypeEntityTypeMappingCollection");

//            //Get Entity Type Id from XML
//            Int32 entityTypeId = 0;
//            Int32.TryParse(rootNode.Attributes["EntityTypeId"].Value.ToString(), out entityTypeId);

//            //Prepare expected RelationshipTypeEntityTypeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("RelationshipTypeEntityTypeMappingCollection/RelationshipTypeEntityTypeMapping"))
//            {
//                relationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                relationshipTypeEntityTypeMapping.Id = id;
//                relationshipTypeEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
//                relationshipTypeEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                relationshipTypeEntityTypeMapping.Locale = locale;

//                relationshipTypeEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                relationshipTypeEntityTypeMapping.RelationshipTypeId = relTypeId;
//                relationshipTypeEntityTypeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                relationshipTypeEntityTypeMapping.EntityTypeId = entTypeId;
//                relationshipTypeEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 parentEntityTypeId = 0;
//                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
//                relationshipTypeEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
//                relationshipTypeEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

//                Boolean drillDown = false;
//                Boolean.TryParse(node.Attributes["DrillDown"].Value.ToString(), out drillDown);
//                relationshipTypeEntityTypeMapping.DrillDown = drillDown;

//                Boolean isDefaultRelation = false;
//                Boolean.TryParse(node.Attributes["IsDefaultRelation"].Value.ToString(), out isDefaultRelation);
//                relationshipTypeEntityTypeMapping.IsDefaultRelation = isDefaultRelation;

//                Boolean excludable = false;
//                Boolean.TryParse(node.Attributes["Excludable"].Value.ToString(), out excludable);
//                relationshipTypeEntityTypeMapping.Excludable = excludable;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                relationshipTypeEntityTypeMapping.ReadOnly = readOnly;

//                Boolean validationRequired = false;
//                Boolean.TryParse(node.Attributes["ValidationRequired"].Value.ToString(), out validationRequired);
//                relationshipTypeEntityTypeMapping.ValidationRequired = validationRequired;

//                Boolean showValidFlagInGrid = false;
//                Boolean.TryParse(node.Attributes["ShowValidFlagInGrid"].Value.ToString(), out showValidFlagInGrid);
//                relationshipTypeEntityTypeMapping.ShowValidFlagInGrid = showValidFlagInGrid;

//                expected.Add(relationshipTypeEntityTypeMapping);
//            }

//            //Get actual mappings
//            Collection<RelationshipTypeEntityTypeMapping> actual;
//            actual = target.GetMappingsByEntityTypeId(entityTypeId);

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
//            RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL();
//            Collection<EntityType> expected = new Collection<EntityType>();

//            //Load XML
//            xmlDoc.LoadXml(@"<EntityTypeCollection RelationshipTypeId = ""10113"">
//	                            <EntityType Id = ""7"" Name = ""Component"" LongName = ""Component"" Locale = ""en_WW"" Action = ""Read"" ShowAtCreation = ""False"" HasChildNodes = ""False"" CatalogBranchLevel = ""2"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" >
//                                    <ChildEntityTypes/>
//                                </EntityType>
//                                <EntityType Id = ""8"" Name = ""Party"" LongName = ""Party"" Locale = ""en_WW"" Action = ""Read"" ShowAtCreation = ""True"" HasChildNodes = ""False"" CatalogBranchLevel = ""2"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" >
//                                    <ChildEntityTypes/>
//                                </EntityType>
//                            </EntityTypeCollection>");
//            XmlNode rootNode = xmlDoc.SelectSingleNode("EntityTypeCollection");

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
//            actual = target.GetMappedEntityTypes(relationshipTypeId);

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetById
//        ///</summary>
//        [TestMethod()]
//        public void GetByIdTest()
//        {
//            //RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //int id = 0; // TODO: Initialize to an appropriate value
//            //RelationshipTypeEntityTypeMapping expected = null; // TODO: Initialize to an appropriate value
//            //RelationshipTypeEntityTypeMapping actual;
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
//            RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = null;
//            RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL();
//            Collection<RelationshipTypeEntityTypeMapping> expected = new Collection<RelationshipTypeEntityTypeMapping>();

//            //Load XML
//            xmlDoc.LoadXml(@"<RelationshipTypeEntityTypeMappingCollection RelationshipTypeId = ""1014"" EntityTypeId = ""6"">
//	                            <RelationshipTypeEntityTypeMapping Id = ""102"" Name = """" LongName = """" Locale = ""en_WW"" Action = ""Read"" RelationshipTypeId = ""1014"" RelationshipTypeName = ""Auto_Testing1"" EntityTypeId = ""6"" EntityTypeName = ""Category"" ParentEntityTypeId = ""0"" ParentEntityTypeName = """" DrillDown = ""False"" IsDefaultRelation = ""False"" Excludable = ""False"" ReadOnly = ""False"" ValidationRequired = ""False"" ShowValidFlagInGrid = ""False""/>
//                             </RelationshipTypeEntityTypeMappingCollection>");
            
//            //Prepare expected RelationshipTypeEntityTypeMapping Collection from XML
//            foreach (XmlNode node in xmlDoc.SelectNodes("RelationshipTypeEntityTypeMappingCollection/RelationshipTypeEntityTypeMapping"))
//            {
//                relationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping();

//                Int32 id = 0;
//                Int32.TryParse(node.Attributes["Id"].Value.ToString(), out id);
//                relationshipTypeEntityTypeMapping.Id = id;
//                relationshipTypeEntityTypeMapping.Name = node.Attributes["Name"].Value.ToString();
//                relationshipTypeEntityTypeMapping.LongName = node.Attributes["LongName"].Value.ToString();

//                String strLocale = node.Attributes["Locale"].Value.ToString();
//                Core.LocaleEnum locale = Core.LocaleEnum.UnKnown;
//                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
//                relationshipTypeEntityTypeMapping.Locale = locale;

//                relationshipTypeEntityTypeMapping.Action = (MDM.Core.ObjectAction)Enum.Parse(typeof(MDM.Core.ObjectAction), node.Attributes["Action"].Value.ToString());

//                Int32 relTypeId = 0;
//                Int32.TryParse(node.Attributes["RelationshipTypeId"].Value.ToString(), out relTypeId);
//                relationshipTypeEntityTypeMapping.RelationshipTypeId = relTypeId;
//                relationshipTypeEntityTypeMapping.RelationshipTypeName = node.Attributes["RelationshipTypeName"].Value.ToString();

//                Int32 entTypeId = 0;
//                Int32.TryParse(node.Attributes["EntityTypeId"].Value.ToString(), out entTypeId);
//                relationshipTypeEntityTypeMapping.EntityTypeId = entTypeId;
//                relationshipTypeEntityTypeMapping.EntityTypeName = node.Attributes["EntityTypeName"].Value.ToString();

//                Int32 parentEntityTypeId = 0;
//                Int32.TryParse(node.Attributes["ParentEntityTypeId"].Value.ToString(), out parentEntityTypeId);
//                relationshipTypeEntityTypeMapping.ParentEntityTypeId = parentEntityTypeId;
//                relationshipTypeEntityTypeMapping.ParentEntityTypeName = node.Attributes["ParentEntityTypeName"].Value.ToString();

//                Boolean drillDown = false;
//                Boolean.TryParse(node.Attributes["DrillDown"].Value.ToString(), out drillDown);
//                relationshipTypeEntityTypeMapping.DrillDown = drillDown;

//                Boolean isDefaultRelation = false;
//                Boolean.TryParse(node.Attributes["IsDefaultRelation"].Value.ToString(), out isDefaultRelation);
//                relationshipTypeEntityTypeMapping.IsDefaultRelation = isDefaultRelation;

//                Boolean excludable = false;
//                Boolean.TryParse(node.Attributes["Excludable"].Value.ToString(), out excludable);
//                relationshipTypeEntityTypeMapping.Excludable = excludable;

//                Boolean readOnly = false;
//                Boolean.TryParse(node.Attributes["ReadOnly"].Value.ToString(), out readOnly);
//                relationshipTypeEntityTypeMapping.ReadOnly = readOnly;

//                Boolean validationRequired = false;
//                Boolean.TryParse(node.Attributes["ValidationRequired"].Value.ToString(), out validationRequired);
//                relationshipTypeEntityTypeMapping.ValidationRequired = validationRequired;

//                Boolean showValidFlagInGrid = false;
//                Boolean.TryParse(node.Attributes["ShowValidFlagInGrid"].Value.ToString(), out showValidFlagInGrid);
//                relationshipTypeEntityTypeMapping.ShowValidFlagInGrid = showValidFlagInGrid;

//                expected.Add(relationshipTypeEntityTypeMapping);
//            }

//            //Get actual mappings
//            Collection<RelationshipTypeEntityTypeMapping> actual;
//            actual = target.GetAll();

//            //Check whether expected is matching to actual and assert the result
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for Delete
//        ///</summary>
//        [TestMethod()]
//        public void DeleteTest()
//        {
//            //RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Delete(relationshipTypeEntityTypeMapping);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for Create
//        ///</summary>
//        [TestMethod()]
//        public void CreateTest()
//        {
//            //RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL(); // TODO: Initialize to an appropriate value
//            //RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = null; // TODO: Initialize to an appropriate value
//            //bool expected = false; // TODO: Initialize to an appropriate value
//            //bool actual;
//            //actual = target.Create(relationshipTypeEntityTypeMapping);
//            //Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Target Method is not implemented.");
//        }

//        /// <summary>
//        ///A test for RelationshipTypeEntityTypeMappingBL Constructor
//        ///</summary>
//        [TestMethod()]
//        public void RelationshipTypeEntityTypeMappingBLConstructorTest()
//        {
//            RelationshipTypeEntityTypeMappingBL target = new RelationshipTypeEntityTypeMappingBL();
//            Assert.Inconclusive("TODO: Implement code to verify target");
//        }
    }
}
