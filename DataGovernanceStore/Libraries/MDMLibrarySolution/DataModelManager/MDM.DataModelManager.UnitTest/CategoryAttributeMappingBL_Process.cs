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
    using MDM.EntityManager.Business;
    using MDM.Interfaces;
    using MDM.AttributeModelManager.Business;

    [TestClass]
    [DeploymentItem(@"Data\ProcessCategoryAttributeMappingTest.xml")]
    public class CategoryAttributeMappingBL_Process
    {
        #region Fields

        private const String FileName = "ProcessCategoryAttributeMappingTest.xml";

        private CallerContext _callerContext;
        private CategoryAttributeMappingBL _categoryAttributeMappingBl;
        private CategoryAttributeMappingCollection _categoryAttributeMappingCollectionToProcess;
        private OperationResultCollection _operationResultCollection;
        private String _programName;
        private String _timeString;
        private IEntityManager _entityManager;
        private IAttributeModelManager _attributeModelManager;
             
        #endregion Fields

        /// <summary>
        /// Initialize test data
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
            CommonUtil.LoadSecurityPrincipal("cfadmin");
            _categoryAttributeMappingBl = new CategoryAttributeMappingBL();
            _timeString = DateTime.Now.ToShortTimeString();
            _entityManager = new EntityBL();
            _attributeModelManager = new AttributeModelBL();
        }

        /// <summary>
        /// Tests CategoryAttributeMapping creation
        /// </summary>
        [TestMethod]
        public void Create_CategoryAttributeMapping()
        {
            _callerContext.ProgramName = "create_category-attribute mapping";
            String methodName = "Create_CategoryAttributeMapping";
            GetInputOutput(methodName, out _categoryAttributeMappingCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_categoryAttributeMappingCollectionToProcess != null && _categoryAttributeMappingCollectionToProcess.Any(), "Collection of category-attribute mappings to process null or empty");

            CategoryAttributeMapping categoryAttributeMapping = _categoryAttributeMappingCollectionToProcess.First();

            CreateCategoryAttributeMapping(categoryAttributeMapping);
        }

        /// <summary>
        /// Tests CategoryAttributeMapping updating
        /// </summary>
        [TestMethod]
        public void Update_CategoryAttributeMapping()
        {
            _callerContext.ProgramName = "update_category-attribute mapping";
            String methodName = "Update_CategoryAttributeMapping";

            GetInputOutput(methodName, out _categoryAttributeMappingCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_categoryAttributeMappingCollectionToProcess != null && _categoryAttributeMappingCollectionToProcess.Any(), "Collection of category-attribute mappings to process null or empty");

            CategoryAttributeMapping categoryAttributeMapping = _categoryAttributeMappingCollectionToProcess.First();

            categoryAttributeMapping.AttributeName = "Updated_" + categoryAttributeMapping.AttributeName;
            categoryAttributeMapping.AllowableValues = "Updated_" + categoryAttributeMapping.AllowableValues;
            categoryAttributeMapping.MaxLength++;
            categoryAttributeMapping.MinLength++;
            categoryAttributeMapping.Required = !categoryAttributeMapping.Required;
            categoryAttributeMapping.AllowableUOM = "Updated_" + categoryAttributeMapping.AllowableUOM;
            categoryAttributeMapping.DefaultUOM = "Updated_" + categoryAttributeMapping.DefaultUOM;
            categoryAttributeMapping.Precision++;
            categoryAttributeMapping.MaxInclusive = "Updated_" + categoryAttributeMapping.MaxInclusive;
            categoryAttributeMapping.MinInclusive = "Updated_" + categoryAttributeMapping.MinInclusive;
            categoryAttributeMapping.MinExclusive = "Updated_" + categoryAttributeMapping.MinExclusive;
            categoryAttributeMapping.MaxExclusive = "Updated_" + categoryAttributeMapping.MaxExclusive;
            categoryAttributeMapping.Definition = "Updated_" + categoryAttributeMapping.Definition;
            categoryAttributeMapping.AttributeExample = "Updated_" + categoryAttributeMapping.AttributeExample;
            categoryAttributeMapping.BusinessRule = "Updated_" + categoryAttributeMapping.BusinessRule;
            categoryAttributeMapping.DefaultValue = "Updated_" + categoryAttributeMapping.DefaultValue;

            categoryAttributeMapping.Id = this.CreateCategoryAttributeMapping(categoryAttributeMapping);

            OperationResult importProfileProcessOperationResult = _categoryAttributeMappingBl.Update(categoryAttributeMapping, _entityManager, _callerContext, _attributeModelManager);
            Assert.IsTrue(importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None, "Update failed");

        }

        /// <summary>
        /// Tests CategoryAttributeMapping deletion
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Attribute Mapping")]
        public void Delete_CategoryAttributeMapping()
        {
            _callerContext.ProgramName = "delete_category-attribute mapping";
            String methodName = "Delete_CategoryAttributeMapping";

            GetInputOutput(methodName, out _categoryAttributeMappingCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_categoryAttributeMappingCollectionToProcess != null && _categoryAttributeMappingCollectionToProcess.Any(), "Collection of category - attribute mappings to process null or empty");

            CategoryAttributeMapping categoryAttributeMapping = _categoryAttributeMappingCollectionToProcess.First();

            categoryAttributeMapping.Id = this.CreateCategoryAttributeMapping(categoryAttributeMapping);

            OperationResult importProfileProcessOperationResult =
                _categoryAttributeMappingBl.Delete(categoryAttributeMapping, _entityManager, _callerContext, _attributeModelManager);

            Assert.IsTrue(importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        /// <summary>
        /// Tests CategoryAttributeMapping inheritance
        /// </summary>
        [TestMethod]
        public void Inherit_CategoryAttributeMapping()
        {
            _callerContext.ProgramName = "inherit_category-attribute mapping";
            String methodName = "Inherit_CategoryAttributeMapping";

            GetInputOutput(methodName, out _categoryAttributeMappingCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_categoryAttributeMappingCollectionToProcess != null && _categoryAttributeMappingCollectionToProcess.Any(), "Collection of category - attribute mappings to process null or empty");

            CategoryAttributeMapping categoryAttributeMapping = _categoryAttributeMappingCollectionToProcess.First();

            categoryAttributeMapping.Id = this.CreateCategoryAttributeMapping(categoryAttributeMapping);

            OperationResult importProfileProcessOperationResult =
                _categoryAttributeMappingBl.Inherit(categoryAttributeMapping, _entityManager, _callerContext, _attributeModelManager);

            Assert.IsTrue(importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || importProfileProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        /// <summary>
        /// Tests categoryAttributeMapping processing
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Attribute Mapping")]
        public void Process_CategoryAttributeMapping()
        {
            _callerContext.ProgramName = "process_category-attribute mapping";
            String methodName = "Process_CategoryAttributeMapping";

            GetInputOutput(methodName, out _categoryAttributeMappingCollectionToProcess, out _programName, out _operationResultCollection);

            Assert.IsTrue(_categoryAttributeMappingCollectionToProcess != null && _categoryAttributeMappingCollectionToProcess.Any(), "Collection of category - attribute mappings to process null or empty");

            _categoryAttributeMappingCollectionToProcess.ToList().ForEach(categoryAttributeMapping =>
            {
                categoryAttributeMapping.Id = this.CreateCategoryAttributeMapping(categoryAttributeMapping);
                categoryAttributeMapping.Action = ObjectAction.Delete;
            });

            OperationResultCollection orc = _categoryAttributeMappingBl.Process(_categoryAttributeMappingCollectionToProcess, _entityManager, this._callerContext, _attributeModelManager);

            Assert.IsTrue(orc.OperationResultStatus == OperationResultStatusEnum.Successful || orc.OperationResultStatus == OperationResultStatusEnum.None, "Process operation failed");

        }

        #region Helper Methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="categoryAttributeMappingCollectionToProcess">Input value of "appConfigKey" parameter for method from Xml</param>
        /// <param name="operationResultCollection">Expected output value</param>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="programName"></param>
        private void GetInputOutput(String methodName, out CategoryAttributeMappingCollection categoryAttributeMappingCollectionToProcess,
                                    out String programName, out OperationResultCollection operationResultCollection)
        {
            categoryAttributeMappingCollectionToProcess = new CategoryAttributeMappingCollection();
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
                                        case "categoryattributemappings":
                                            categoryAttributeMappingCollectionToProcess = new CategoryAttributeMappingCollection(value);
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

        private int CreateCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping)
        {
            categoryAttributeMapping.Name = String.Format("{0}_{1}", categoryAttributeMapping.Name, _timeString);
            categoryAttributeMapping.LongName = String.Format("{0}_{1}", categoryAttributeMapping.LongName, _timeString);

            OperationResult operationResult =
                _categoryAttributeMappingBl.Create(categoryAttributeMapping, new EntityBL(), _callerContext, _attributeModelManager);

            if (operationResult.Errors != null && operationResult.Errors.Count > 0)
            {
                Error error = operationResult.Errors.First();
                Assert.Fail("{0} {1}", error.ErrorCode, error.ErrorMessage);
            }

            Assert.IsTrue(
                operationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                operationResult.OperationResultStatus == OperationResultStatusEnum.None, "Create failed");

            Assert.IsTrue(operationResult.ReturnValues.Any(), "Return value of process operation doesn't contain ID");
            Int32 categoryAttributeMappingId = operationResult.Id;

            Assert.IsTrue(categoryAttributeMappingId > 0, "Return value is not integer");
            return categoryAttributeMappingId;
        }

        #endregion Helper methods
    }
}
