using MDM.BusinessRuleManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;
using System.Xml;
using System;

namespace MDM.BusinessRuleManager.UnitTest
{


    /// <summary>
    ///This is a test class for BusinessRuleBLTest and is intended
    ///to contain all BusinessRuleBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessRuleBLTest
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
        ///A test for Process
        ///</summary>
        //        [TestMethod()]
        //        public void ProcessTest()
        //        {
        //            BusinessRuleBL target = new BusinessRuleBL(); // TODO: Initialize to an appropriate value
        //            Collection<BusinessRule> businessRules = null; // TODO: Initialize to an appropriate value
        //            string loginUser = string.Empty; // TODO: Initialize to an appropriate value
        //            string programName = string.Empty; // TODO: Initialize to an appropriate value
        //            string action = string.Empty; // TODO: Initialize to an appropriate value
        //            target.Process(businessRules, loginUser, programName, action);
        //            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //        }

        //        /// <summary>
        //        ///A test for GetAllByUser
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetAllByUserTest()
        //        {
        //            BusinessRuleBL target = new BusinessRuleBL(); // TODO: Initialize to an appropriate value
        //            string LoginUser = "cfadmin"; // TODO: Initialize to an appropriate value
        //            //Collection<BusinessRule> expected = new Collection<BusinessRule>(); // TODO: Initialize to an appropriate value

        //            Collection<BusinessRule> actual;
        //            actual = target.GetAllByUser(LoginUser);
        //            Assert.AreEqual(6, actual.Count);

        //        }
        //        /// <summary>
        //        ///A test for GetByContext
        //        ///</summary>
        //        [TestMethod()]
        //        public void GetByContextTest()
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            String InputXML = String.Empty;
        //            String ExpectedOutputXML = String.Empty;
        //            BusinessRule businessRule = null;
        //            Collection<BusinessRule> expectedCollection = new Collection<BusinessRule>();

        //            // The xml with inputs and expected output

        //            InputXML = @"<BusinessRulesInput EventSourceID=""23""
        //                                                      EventSubscriberID=""1""
        //                                                      LoginUserID=""104""
        //                                                      LoginUserRole=""0""
        //                                                      OrgID=""155""
        //                                                      ContainerID=""10110""
        //                                                      EntityTypeID=""10021"" 
        //                                                      BusinessRuleTypeIDs=""11,12,13,14""/>";


        //            ExpectedOutputXML = @"</BusinessRulesCollection>
        //                                    <BusinessRule BusinessRuleTypeId = ""12"" BusinessRuleTypeName = """" RuleXML = """" Description = """" PreCondition = """" PostCondition = """" XsdSchema = """" SampleXml = """" RuleValue = """" ActiveFlag = ""false"" CreateUser = """" />
        //                                    <BusinessRule BusinessRuleTypeId = ""11"" BusinessRuleTypeName = """" RuleXML = """" Description = """" PreCondition = """" PostCondition = """" XsdSchema = """" SampleXml = """" RuleValue = """" ActiveFlag = ""false"" CreateUser = """" />                            
        //                                </BusinessRulesCollection>";

        //            //Read the Input Parameters from the input xml
        //            xmlDoc.LoadXml(InputXML);

        //            XmlNode rootNode = xmlDoc.SelectSingleNode("BusinessRulesInput");

        //            Int32 EventSourceID = 0;
        //            Int32.TryParse(rootNode.Attributes["EventSourceID"].Value, out EventSourceID);

        //            Int32 EventSubscriberID = 0;
        //            Int32.TryParse(rootNode.Attributes["EventSubscriberID"].Value, out EventSubscriberID);

        //            Int32 LoginUserID = 0;
        //            Int32.TryParse(rootNode.Attributes["LoginUserID"].Value, out LoginUserID);

        //            Int32 LoginUserRole = 0;
        //            Int32.TryParse(rootNode.Attributes["LoginUserRole"].Value, out LoginUserRole);

        //            Int32 OrgID = 0;
        //            Int32.TryParse(rootNode.Attributes["OrgID"].Value, out OrgID);

        //            Int32 ContainerID = 0;
        //            Int32.TryParse(rootNode.Attributes["ContainerID"].Value, out ContainerID);

        //            Int32 EntityTypeID = 0;
        //            Int32.TryParse(rootNode.Attributes["EntityTypeID"].Value, out EntityTypeID);

        //            String BusinessRuleTypeIDs = String.Empty;
        //            BusinessRuleTypeIDs = rootNode.Attributes["BusinessRuleTypeIDs"].Value;

        //            //Create the list of BusinessRule Objects from the xml
        //            xmlDoc.LoadXml(ExpectedOutputXML);

        //            foreach (XmlNode node in xmlDoc.SelectNodes("BusinessRulesCollection/BusinessRule"))
        //            {
        //                businessRule = new BusinessRule();

        //                Int32 businessRuleTypeId = 0;
        //                Int32.TryParse(node.Attributes["BusinessRuleTypeId"].Value,out businessRuleTypeId);
        //                businessRule.BusinessRuleTypeId = businessRuleTypeId;
        //                businessRule.BusinessRuleTypeName = node.Attributes["BusinessRuleTypeName"].Value;
        //                businessRule.RuleXML = node.Attributes["RuleXML"].Value;
        //                businessRule.Description = node.Attributes["Description"].Value;
        //                businessRule.PreCondition = node.Attributes["PreCondition"].Value;
        //                businessRule.PostCondition = node.Attributes["PostCondition"].Value;
        //                businessRule.XsdSchema = node.Attributes["XsdSchema"].Value;
        //                businessRule.SampleXml = node.Attributes["SampleXml"].Value;
        //                businessRule.RuleValue = node.Attributes["RuleValue"].Value;
        //                Boolean activeFlag = false;
        //                Boolean.TryParse(node.Attributes["ActiveFlag"].Value, out activeFlag);
        //                businessRule.ActiveFlag = activeFlag;
        //                businessRule.CreateUser = node.Attributes["CreateUser"].Value;

        //                expectedCollection.Add(businessRule);

        //            }

        //            //Get the actual BusinessRule Objects
        //            Collection<BusinessRule> actual;
        //            actual = BusinessRuleBL.GetByContext(EventSourceID,EventSubscriberID,LoginUserID,LoginUserRole,OrgID,ContainerID,EntityTypeID,BusinessRuleTypeIDs);

        //            //Check whether expected is matching to actual and assert the result
        //            CollectionAssert.AreEqual(expectedCollection, actual);
        //        }

    }
}
