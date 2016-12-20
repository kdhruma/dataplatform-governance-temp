using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml;
using MDM.UnitTestUtility;

namespace MDM.RelationshipManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.RelationshipManager.Business;

    /// <summary>
    /// Unit test class for RelationshipType get
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Data\RelationshipTypeBL_Process.xml")]
    public class RelationshipTypeBL_ProcessRelationshipTypes
    {
        #region Fields

        const String _fileName = @"RelationshipTypeBL_Process.xml";

        private RelationshipTypeBL _relationshipTypeManager;

        private String _programName;
        private RelationshipTypeCollection _relationshipTypesToProcess;
        private RelationshipTypeCollection _outputRelationshipTypes;

        private RelationshipType _expectedRelationshipType;
        private RelationshipType _relationshipTypeToTest;
        
        #endregion Fields

        #region Initialization

        /// <summary>
        /// Initialize test data
        /// </summary>
        public RelationshipTypeBL_ProcessRelationshipTypes()
        {
            _relationshipTypeManager = new RelationshipTypeBL();
        }

        /// <summary>
        /// Initialize test data
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            if (_relationshipTypesToProcess == null)
            {
                return;
            }

            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling, _programName);
            RelationshipTypeCollection relationshipTypesInDatabase = _relationshipTypeManager.GetAll(callerContext);

            RelationshipTypeCollection relationshipTypesToRemove = new RelationshipTypeCollection();

            if (relationshipTypesInDatabase.Any())
            {
                foreach (RelationshipType testingRelationshipType in _relationshipTypesToProcess)
                {
                    IRelationshipType testingRelationshipTypeInDatabase = relationshipTypesInDatabase.GetRelationshipType(testingRelationshipType.Id);
                    if (testingRelationshipTypeInDatabase != null)
                    {
                        testingRelationshipTypeInDatabase.Action = ObjectAction.Delete;
                        relationshipTypesToRemove.Add(testingRelationshipTypeInDatabase);
                    }
                }
            }

            if (relationshipTypesToRemove.Count > 0)
            {
                OperationResultCollection relationshipTypeProcessOperationResult = _relationshipTypeManager.Process(relationshipTypesToRemove, callerContext);

                Assert.IsTrue(relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None, "Cleanup was not successfull");
            }
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Empty method to check if fwx is running
        /// </summary>
        [TestMethod]
        public void EmptyTest()
        {
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Create 1 relationshipType
        /// </summary>
        [TestMethod]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        public void Create_RelationshipType()
        {
            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName);

            _relationshipTypeToTest = _relationshipTypesToProcess.FirstOrDefault();

            OperationResult relationshipTypeProcessOperationResult = _relationshipTypeManager.Create(_relationshipTypeToTest, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling, _programName));

            Console.Write(relationshipTypeProcessOperationResult.ToXml());
            Assert.IsTrue(relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        /// <summary>
        /// Create 1 relationshipType
        /// </summary>
        [TestMethod]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        public void Create_Bulk_RelationshipType()
        {
            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName);

            OperationResultCollection relationshipTypeProcessOperationResult = _relationshipTypeManager.Process(_relationshipTypesToProcess, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling, _programName));

            Console.Write(relationshipTypeProcessOperationResult.ToXml());
            Assert.IsTrue(relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        /// <summary>
        /// Create 1 relationshipType
        /// </summary>
        [TestMethod]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        public void Update_RelationshipTypes()
        {
            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName);

            ProcessRelationshipType();

            Assert.IsNotNull(_expectedRelationshipType, "No expected RelationshipType found in input data");

            //Do Get by name
            RelationshipTypeCollection relationshipTypes = _relationshipTypeManager.GetAll(new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling));

            IRelationshipType relationshipTypeToUpdate = relationshipTypes.GetRelationshipType(_relationshipTypeToTest.Id);

            Assert.IsNotNull(relationshipTypeToUpdate, "Failed to get relationshipType after creating it");

            relationshipTypeToUpdate.Name = relationshipTypeToUpdate.Name + " - Updated";
            OperationResult or = _relationshipTypeManager.Update((RelationshipType) relationshipTypeToUpdate, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling, _programName));

            Assert.IsTrue(or.OperationResultStatus == OperationResultStatusEnum.Successful || or.OperationResultStatus == OperationResultStatusEnum.None, "Failed to update the relationshipType");

            relationshipTypes = _relationshipTypeManager.GetAll(new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling));

            IRelationshipType updatedRelationshipType = relationshipTypes.GetRelationshipType(_relationshipTypeToTest.Id);

            _expectedRelationshipType.Id = updatedRelationshipType.Id;

            Assert.AreEqual(updatedRelationshipType.ToXml(), _expectedRelationshipType.ToXml(), "Actual and expected Objects are not equal");
        }

        /// <summary>
        /// Create 1 relationshipType
        /// </summary>
        [TestMethod]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        public void Delete_Using_Only_Id_RelationshipType()
        {
            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName);

            ProcessRelationshipType();

            //Do Get by name
            RelationshipTypeCollection relationshipTypes = _relationshipTypeManager.GetAll(new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling));

            IRelationshipType relationshipType = relationshipTypes.GetRelationshipType(_relationshipTypeToTest.Id);

            Assert.IsNotNull(relationshipType, "Failed to get relationship type after creating it");

            RelationshipType relationshipTypeToDelete = new RelationshipType {Id = relationshipType.Id};

            OperationResult or = _relationshipTypeManager.Delete(relationshipTypeToDelete, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling, "Unit test - Delete_RelationshipType"));

            Assert.IsTrue(or.OperationResultStatus == OperationResultStatusEnum.Successful || or.OperationResultStatus == OperationResultStatusEnum.None, "Failed to delete Relationship Type");

            relationshipTypes =_relationshipTypeManager.GetAll(new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling));
            IRelationshipType deletedRelationshipType = relationshipTypes.GetRelationshipType(relationshipTypeToDelete.Id);

            Assert.IsNull(deletedRelationshipType, "Relationship Type was not deleted");
        }

        #endregion Test methods

        #region Helper methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        private void GetInputOutput(String methodName)
        {
            _relationshipTypesToProcess = new RelationshipTypeCollection();
            _programName = String.Empty;
            _outputRelationshipTypes = new RelationshipTypeCollection();

            //Get Data Xml from config file for method that we are looking at.
            String dataXml = DataReader.ReadMethodData(_fileName, methodName);

            if (!String.IsNullOrWhiteSpace(dataXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(dataXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        //Read input parameters from DataXml
                        //Here our parameter name is "appConfigKey". Only 1 parameter to read
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Parameter")
                        {
                            if (reader.HasAttributes)
                            {
                                String paramName = reader.GetAttribute("Name");
                                if (!String.IsNullOrWhiteSpace(paramName))
                                {
                                    String value = reader.ReadInnerXml();
                                    switch (paramName.ToLower())
                                    {
                                        case "relationshiptypes":
                                            _relationshipTypesToProcess = new RelationshipTypeCollection(value);
                                            break;
                                        case "programname":
                                            _programName = value.Trim();
                                            break;
                                        default:
                                            throw new Exception(String.Concat("Unknown parameter : ", paramName));
                                    }
                                }

                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Output")
                        {
                            //Read output for this method.
                            String relationshipTypesXml = reader.ReadInnerXml();
                            if (String.IsNullOrWhiteSpace(relationshipTypesXml))
                            {
                                throw new Exception(String.Concat("No output value found for method : ", methodName));
                            }
                            else
                            {
                                _outputRelationshipTypes = new RelationshipTypeCollection(relationshipTypesXml);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Used to create relationship type in database for test purpose
        /// </summary>
        private void ProcessRelationshipType()
        {
            _relationshipTypeToTest = _relationshipTypesToProcess.FirstOrDefault();

            Assert.IsNotNull(_relationshipTypeToTest, "No RelationshipType to test found in input data");

            _expectedRelationshipType = _outputRelationshipTypes.FirstOrDefault();

            //First create the relationshipType
            OperationResult relationshipTypeProcessOperationResult = _relationshipTypeManager.Create(_relationshipTypeToTest, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling, _programName));

            Assert.IsTrue(relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || relationshipTypeProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None, "Create of relationshipType failed");
        }

        #endregion Helper methods
    }
}
