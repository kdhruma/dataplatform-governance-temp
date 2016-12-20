using System;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.DataModelManager.UnitTest
{
    using MDM.Core;
    using MDM.DataModelManager.UnitTest.Utility;
    using MDM.Core.Exceptions;
    using MDM.HierarchyManager.Business;
    using MDM.BusinessObjects;

    [TestClass]
    [DeploymentItem(@"Data\GetHierarchyTest.xml")]
    public class HierarchyBL_GetHierarchy
    {
        #region Fields
        private const String FileName = "GetHierarchyTest.xml";
        private CallerContext _callerContext;
        private HierarchyBL _hierarchyBl;
        private HierarchyCollection _expectedHierarchyCollection;
        private String _programmName;
        private Int32 _hierarchyId;
        
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
            _programmName = String.Empty;
        }

        /// <summary>
        /// Verification of GetAll methods
        /// </summary>
        [TestMethod]
        public void Get_All_Hierarchies()
        {
            String methodName = "Get_All_Hierarchies";

            GetInputOutput(methodName, out _programmName, out _hierarchyId, out _expectedHierarchyCollection);

            HierarchyCollection actualHierarchyCollection = _hierarchyBl.GetAll(this._callerContext);

            Assert.AreEqual(_expectedHierarchyCollection.ToXml(), actualHierarchyCollection.ToXml());
        }

        /// <summary>
        /// Verification of GetById method
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Hierarchy Relationship Model")]
        public void Get_Hierarchy_By_Id_Proper()
        {
            String methodName = "Get_Hierarchy_By_Id_Proper";

            GetInputOutput(methodName, out _programmName, out _hierarchyId, out _expectedHierarchyCollection);

            Hierarchy actualHierarchy = _hierarchyBl.GetById(_hierarchyId, this._callerContext);

            Assert.IsTrue(_expectedHierarchyCollection != null && _expectedHierarchyCollection.Any(), "Expected hierarchy collection is empty");

            Assert.AreEqual(_expectedHierarchyCollection.First().ToXml(), actualHierarchy.ToXml());
        }

        /// <summary>
        /// Verification that in case hierarchy is not found null is returned
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Hierarchy Relationship Model")]
        public void Get_Hierarchy_By_Id_Not_Exists()
        {
            String methodName = "Get_Hierarchy_By_Id_Not_Exists";

            GetInputOutput(methodName, out _programmName, out _hierarchyId, out _expectedHierarchyCollection);

            Hierarchy actualHierarchy = _hierarchyBl.GetById(_hierarchyId, this._callerContext);

            Assert.IsNull(actualHierarchy);
        }

        /// <summary>
        /// Verification of exception in case when Id less than 1 is passed
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Hierarchy Relationship Model")]
        public void Get_Hierarchy_By_Id_Less_Than_1()
        {
            String methodName = "Get_Hierarchy_By_Id_Less_Than_1";
            String errorMessageCode = String.Empty;

            GetInputOutput(methodName, out _programmName, out _hierarchyId, out _expectedHierarchyCollection);

            try
            {
                Hierarchy actualProfile = _hierarchyBl.GetById(_hierarchyId,_callerContext);
            }
            catch (Exception ex)
            {
                errorMessageCode = ((MDMOperationException)ex).MessageCode;
            }

            Assert.AreEqual("112198", errorMessageCode);
        }

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="hierarchyId">Id of hierarchy</param>
        /// <param name="outputHierarchyCollection">Expected output value</param>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="programName">Name of the programm</param>
        private void GetInputOutput(String methodName, out String programName, out Int32 hierarchyId,
                                    out HierarchyCollection outputHierarchyCollection)
        {
            programName = String.Empty;
            outputHierarchyCollection = new HierarchyCollection();
            hierarchyId = 0;

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
                                        case "hierarchyid":
                                            hierarchyId = ValueTypeHelper.Int32TryParse(value, -1);
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

                             outputHierarchyCollection = new HierarchyCollection(value);
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
    }
}
