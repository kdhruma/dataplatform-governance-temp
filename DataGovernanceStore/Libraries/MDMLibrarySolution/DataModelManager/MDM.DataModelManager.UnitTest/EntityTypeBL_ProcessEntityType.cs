using System;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.DataModelManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.DataModelManager.Business;
    using MDM.DataModelManager.UnitTest.Utility;

    [TestClass]
    public class EntityTypeBL_ProcessEntityType
    {
        #region Fields

        private const String FilePath = @"Data\ProcessEntityTypeTest.xml";
        private const String FileName = "ProcessEntityTypeTest.xml";
        private CallerContext _callerContext = null;
        private EntityTypeBL _entityTypeBl = null;
        private EntityType entityTypeToProcess;
        private EntityType outputEntityType;
        private String programName;

        #endregion Fields

        /// <summary>
        /// Initialize test data
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
             CommonUtil.LoadSecurityPrincipal("cfadmin");
            _entityTypeBl = new EntityTypeBL();
        }

        /// <summary>
        /// Tests Entity type creation
        /// </summary>
        [TestMethod]
        public void Create_EntityType()
        {
            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out entityTypeToProcess, out programName, out outputEntityType);
            
            _callerContext.ProgramName = programName;

            OperationResult importProfileProcessOperationResult =
                _entityTypeBl.Create(entityTypeToProcess,_callerContext);
            Assert.IsTrue(importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        /// <summary>
        /// Tests Entity type updation
        /// </summary>
        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Update_EntityType()
        {
            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out entityTypeToProcess, out programName, out outputEntityType);

            _callerContext.ProgramName = programName;
            OperationResult createOR = _entityTypeBl.Create(entityTypeToProcess,_callerContext);

            if (createOR.OperationResultStatus == OperationResultStatusEnum.Successful
                || createOR.OperationResultStatus == OperationResultStatusEnum.None)
            {
                if (!createOR.ReturnValues.Any())
                {
                    Assert.Fail("Return value of process operation doesn't contain ID");
                }

                entityTypeToProcess.Id = Convert.ToInt32(createOR.ReturnValues[0]);
                entityTypeToProcess.Name = entityTypeToProcess.Name + " - Updated";
                entityTypeToProcess.Action = ObjectAction.Update;

                OperationResult updateOR = _entityTypeBl.Update(entityTypeToProcess, _callerContext);

                if (updateOR.OperationResultStatus == OperationResultStatusEnum.Successful
               || updateOR.OperationResultStatus == OperationResultStatusEnum.None)
                {
                    Assert.IsTrue(true, "Update successful");
                }
                else
                {
                    Assert.Fail("Update failed");
                }
            }
            else
            {
                Assert.Fail("Create Entity Type failed");
            }
        }


        /// <summary>
        /// Tests entity type's deletion
        /// </summary>
        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Delete_EntityType()
        {
            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out entityTypeToProcess, out programName, out outputEntityType);

            _callerContext.ProgramName = programName;
            OperationResult createOR = _entityTypeBl.Create(entityTypeToProcess,_callerContext);
            

            if (createOR.OperationResultStatus == OperationResultStatusEnum.Successful
                || createOR.OperationResultStatus == OperationResultStatusEnum.None)
            {
                if (!createOR.ReturnValues.Any())
                {
                    Assert.Fail("Return value of process operation doesn't contain ID");
                }
                entityTypeToProcess.Id = Convert.ToInt32(createOR.ReturnValues[0]);
                entityTypeToProcess.Action = ObjectAction.Delete;
                OperationResult deleteOR = _entityTypeBl.Update(entityTypeToProcess,_callerContext);

                if (deleteOR.OperationResultStatus == OperationResultStatusEnum.Successful
               || deleteOR.OperationResultStatus == OperationResultStatusEnum.None)
                {
                    Assert.IsTrue(true, "Delete successful");
                }
                else
                {
                    Assert.Fail("Delete failed");
                }
            }
            else
            {
                Assert.Fail("Create Entity Type failed");
            }
        }

        #region Helper methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="entityTypeToProcess">Input value of "appConfigKey" parameter for method from Xml</param>
        /// <param name="outputEntityType">Expected output value</param>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="programName"></param>
        private void GetInputOutput(String methodName, out EntityType entityTypeToProcess, out String programName, out EntityType outputEntityType)
        {
            entityTypeToProcess = new EntityType();
            programName = String.Empty;
            outputEntityType = new EntityType();

            //Get Data Xml from config file for method that we are looking at.
            String dataXml = DataReader.ReadMethodData(FileName, methodName);

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
                                        case "entitytype":
                                            entityTypeToProcess = new EntityType(value);
                                            break;
                                        case "programname":
                                            programName = value;
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
                            String entityTypeXML = reader.ReadInnerXml();
                            if (String.IsNullOrWhiteSpace(entityTypeXML))
                            {
                                throw new Exception(String.Concat("No output value found for method : ", methodName));
                            }
                            else
                            {
                                outputEntityType = new EntityType(entityTypeXML);
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
                        reader.Close();
                }
            }
        }

        #endregion Helper methods
    }
}
