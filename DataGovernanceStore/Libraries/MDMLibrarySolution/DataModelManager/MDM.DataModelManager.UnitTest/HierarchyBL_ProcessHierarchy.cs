using System;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.DataModelManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.DataModelManager.UnitTest.Utility;
    using MDM.HierarchyManager.Business;

    [TestClass]
    [DeploymentItem(@"Data\ProcessHierarchyTest.xml")]
    public class HierarchyBL_ProcessHierarchy
    {
        #region Fields

        private const String FileName = "ProcessHierarchyTest.xml";
        private CallerContext _callerContext;
        private HierarchyBL _hierarchyBl;
        private HierarchyCollection _hierarchyCollectionToProcess;
        private OperationResultCollection _operationResultCollection;
        private String _programName;
        private String _timeString;

        #endregion Fields

        /// <summary>
        /// Initialize test data
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
            CommonUtil.LoadSecurityPrincipal("cfadmin");
            _hierarchyBl = new HierarchyBL();
            _timeString = DateTime.Now.ToShortTimeString();
        }

        /// <summary>
        /// Tests Hierarchy creation
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Hierarchy Relationship Model")]
        public void Create_Hierarchy()
        {
            _callerContext.ProgramName = "create_hierarchy";
            String methodName = "Create_Hierarchy";
            GetInputOutput(methodName, out _hierarchyCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_hierarchyCollectionToProcess != null && _hierarchyCollectionToProcess.Any(), "Collection of hierarchies to process null or empty");

            Hierarchy hierarchy = _hierarchyCollectionToProcess.First();
            CreateHierarchy(hierarchy);
        }

        /// <summary>
        /// Tests Hierarchy updating
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Hierarchy Relationship Model")]
        public void Update_Hierarchy()
        {
            _callerContext.ProgramName = "update_hierarchy";
            String methodName = "update_hierarchy";

            GetInputOutput(methodName, out _hierarchyCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_hierarchyCollectionToProcess != null && _hierarchyCollectionToProcess.Any(), "Collection of hierarchies to process null or empty");

            Hierarchy hierarchy = _hierarchyCollectionToProcess.First();
           
            hierarchy.Id = this.CreateHierarchy(hierarchy);
            hierarchy.Name = "Updated_" + hierarchy.Name;

            OperationResult importProfileProcessOperationResult =
                _hierarchyBl.Update(hierarchy, _callerContext);
            Assert.IsTrue(importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None, "Update failed");
        
        }

        /// <summary>
        /// Tests Hierarchy deletion
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Hierarchy Relationship Model")]
        public void Delete_Hierarchy()
        {
            _callerContext.ProgramName = "delete_hierarchy";
            String methodName = "Delete_Hierarchy";

            GetInputOutput(methodName, out _hierarchyCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_hierarchyCollectionToProcess != null && _hierarchyCollectionToProcess.Any(), "Collection of hierarchies to process null or empty");

            Hierarchy hierarchy = _hierarchyCollectionToProcess.First();
           
            hierarchy.Id = this.CreateHierarchy(hierarchy);

            OperationResult importProfileProcessOperationResult =
                _hierarchyBl.Delete(hierarchy, _callerContext);

            Assert.IsTrue(importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        /// <summary>
        /// Tests hierarchy processing
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Hierarchy Relationship Model")]
        public void Process_Hierarchy()
        {
            _callerContext.ProgramName = "process_hierarchy";
            String methodName = "Process_Hierarchy";
                
            GetInputOutput(methodName, out _hierarchyCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_hierarchyCollectionToProcess != null && _hierarchyCollectionToProcess.Any(), "Collection of hierarchies to process null or empty");

            _hierarchyCollectionToProcess.ToList().ForEach(hierarchy =>
                { 
                    hierarchy.Id = this.CreateHierarchy(hierarchy);
                    hierarchy.Action = ObjectAction.Delete;
                });

            OperationResultCollection orc = _hierarchyBl.Process(_hierarchyCollectionToProcess, this._callerContext);

            Assert.IsTrue(orc.OperationResultStatus == OperationResultStatusEnum.Successful || orc.OperationResultStatus == OperationResultStatusEnum.None, "Process operation failed");

        }

        #region Helper Methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="hierarchyCollectionToProcess">Input value of "appConfigKey" parameter for method from Xml</param>
        /// <param name="operationResultCollection">Expected output value</param>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="programName"></param>
        private void GetInputOutput(String methodName, out HierarchyCollection hierarchyCollectionToProcess,
                                    out String programName, out OperationResultCollection operationResultCollection)
        {
            hierarchyCollectionToProcess = new HierarchyCollection();
            programName = String.Empty;
            operationResultCollection = new OperationResultCollection();

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
                                        case "hierarchies":
                                            hierarchyCollectionToProcess = new HierarchyCollection(value);
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
                              //Read output for this method. Either OperationResult or OperationResultCollection
                              String value = reader.ReadInnerXml();
                              if (String.IsNullOrWhiteSpace(value))
                              {
                                  throw new Exception(String.Concat("No output value found for method : ",
                                                                    methodName));
                              }
                              
                              operationResultCollection = new OperationResultCollection(value);
                              break;
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

        private int CreateHierarchy(Hierarchy hierarchy)
        {
            hierarchy.Name = String.Format("{0}_{1}", hierarchy.Name, _timeString);
            hierarchy.LongName = String.Format("{0}_{1}", hierarchy.LongName, _timeString);
            

            OperationResult operationResult =
                _hierarchyBl.Create(hierarchy, _callerContext);



            Assert.IsTrue(
                operationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                operationResult.OperationResultStatus == OperationResultStatusEnum.None, "Create failed");
            Assert.IsTrue(operationResult.ReturnValues.Any(), "Return value of process operation doesn't contain ID");
            Int32 hierarchyId = operationResult.Id;

            Assert.IsTrue(hierarchyId > 0, "Return value is not integer");
            return hierarchyId;
        }

        #endregion Helper methods
    }
}
