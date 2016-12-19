using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.DataModelManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.DataModelManager.Business;
    using MDM.DataModelManager.UnitTest.Utility;

    [TestClass]
    [DeploymentItem(@"Data\GetCategoryAttributeMappingTest.xml")]
    public class CategoryAttributeMappingBL_Get
    {
        #region Fields
        private const String FileName = "GetCategoryAttributeMappingTest.xml";
        private CallerContext _callerContext;
        private CategoryAttributeMappingBL _categoryAttributeMappingBl;
        private CategoryAttributeMappingCollection _expectedCategoryAttributeMappingCollection;
        private String _programmName;
        private Int32 _categoryId;
        private Int32 _catalogId;
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
            _categoryAttributeMappingBl = new CategoryAttributeMappingBL();
            _programmName = String.Empty;
        }

        #region Get By Category Id

        /// <summary>
        /// Verification of GetByCategoryId method
        /// </summary>
        [TestMethod]
        public void Get_CategoryAttributeMappings_By_CategoryId()
        {
            String methodName = "Get_CategoryAttributeMappings_By_CategoryId";

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            CategoryAttributeMappingCollection actualCategoryAttributeMappingCollection = _categoryAttributeMappingBl.GetByCategoryId(_categoryId, this._callerContext );

            Assert.AreEqual(_expectedCategoryAttributeMappingCollection.ToXml(), actualCategoryAttributeMappingCollection.ToXml());
        }

        /// <summary>
        /// Verification that in case category-attribute mapping is not found null is returned
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Category")]
        public void Get_CategoryAttributeMapping_By_CategoryId_Not_Exists()
        {
            String methodName = "Get_CategoryAttributeMapping_By_CategoryId_Not_Exists";

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            CategoryAttributeMappingCollection actualCategoryAttributeMappingCollection = _categoryAttributeMappingBl.GetByCategoryId(_categoryId, this._callerContext);

            Assert.IsTrue(actualCategoryAttributeMappingCollection == null || actualCategoryAttributeMappingCollection.Count == 0);
        }

        /// <summary>
        /// Verification of exception in case when Id less than 0 is passed
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Category")]
        public void Get_CategoryAttributeMapping_By_CategoryId_Less_Than_0()
        {
            String methodName = "Get_CategoryAttributeMapping_By_CategoryId_Less_Than_0";
            String errorMessageCode = String.Empty;

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            try
            {
                CategoryAttributeMappingCollection actualAttributeMapping = _categoryAttributeMappingBl.GetByCategoryId(_categoryId, this._callerContext);
            }
            catch (Exception ex)
            {
                errorMessageCode = ((MDMOperationException)ex).MessageCode;
            }

            Assert.AreEqual("112227", errorMessageCode);
        }


        #endregion Get By Category Id

        #region Get By Hierarchy Id

        /// <summary>
        /// Verification of GetByHierarchyId method
        /// </summary>
        [TestMethod]
        public void Get_CategoryAttributeMappings_By_HierarchyId()
        {
            String methodName = "Get_CategoryAttributeMappings_By_HierarchyId";

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            CategoryAttributeMappingCollection actualCategoryAttributeMappingCollection = _categoryAttributeMappingBl.GetByHierarchyId(_hierarchyId, this._callerContext);

            Assert.AreEqual(_expectedCategoryAttributeMappingCollection.ToXml(), actualCategoryAttributeMappingCollection.ToXml());
        }

        /// <summary>
        /// Verification that in case category-attribute mapping is not found null is returned
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Category")]
        public void Get_CategoryAttributeMapping_By_HierarchyId_Not_Exists()
        {
            String methodName = "Get_CategoryAttributeMapping_By_HierarchyId_Not_Exists";

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            CategoryAttributeMappingCollection actualCategoryAttributeMappingCollection = _categoryAttributeMappingBl.GetByHierarchyId(_hierarchyId, this._callerContext);

            Assert.IsTrue(actualCategoryAttributeMappingCollection == null || actualCategoryAttributeMappingCollection.Count == 0);
        }

        /// <summary>
        /// Verification of exception in case when HierarchyId less than 0 is passed
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Category")]
        public void Get_CategoryAttributeMapping_By_HierarchyId_Less_Than_0()
        {
            String methodName = "Get_CategoryAttributeMapping_By_HierarchyId_Less_Than_0";
            String errorMessageCode = String.Empty;

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            try
            {
                CategoryAttributeMappingCollection actualAttributeMapping = _categoryAttributeMappingBl.GetByHierarchyId(_hierarchyId, this._callerContext);
            }
            catch (Exception ex)
            {
                errorMessageCode = ((MDMOperationException)ex).MessageCode;
            }

            Assert.AreEqual("112198", errorMessageCode);
        }

        #endregion Get By Hierarchy Id

        #region Get By Catalog Id

        /// <summary>
        /// Verification of GetByHierarchyId method
        /// </summary>
        [TestMethod]
        public void Get_CategoryAttributeMappings_By_CatalogId()
        {
            String methodName = "Get_CategoryAttributeMappings_By_CatalogId";

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            CategoryAttributeMappingCollection actualCategoryAttributeMappingCollection = _categoryAttributeMappingBl.GetByCatalogId(_catalogId, this._callerContext);

            Assert.AreEqual(_expectedCategoryAttributeMappingCollection.ToXml(), actualCategoryAttributeMappingCollection.ToXml());
        }

        /// <summary>
        /// Verification that in case category-attribute mapping is not found null is returned
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Category")]
        public void Get_CategoryAttributeMapping_By_CatalogId_Not_Exists()
        {
            String methodName = "Get_CategoryAttributeMapping_By_CatalogId_Not_Exists";

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            CategoryAttributeMappingCollection actualCategoryAttributeMappingCollection = _categoryAttributeMappingBl.GetByCatalogId(_catalogId, this._callerContext);

            Assert.IsTrue(actualCategoryAttributeMappingCollection == null || actualCategoryAttributeMappingCollection.Count == 0);
        }

        /// <summary>
        /// Verification of exception in case when HierarchyId less than 0 is passed
        /// </summary>
        [TestMethod]
        [TestCategory("BVTTest")]
        [TestCategory("EntityModel")]
        [TestCategory("EntityModel\\Category")]
        public void Get_CategoryAttributeMapping_By_CatalogId_Less_Than_0()
        {
            String methodName = "Get_CategoryAttributeMapping_By_CatalogId_Less_Than_0";
            String errorMessageCode = String.Empty;

            GetInputOutput(methodName, out _programmName, out _categoryId, out _hierarchyId, out _catalogId, out _expectedCategoryAttributeMappingCollection);

            try
            {
                CategoryAttributeMappingCollection actualAttributeMapping = _categoryAttributeMappingBl.GetByCatalogId(_catalogId, this._callerContext);
            }
            catch (Exception ex)
            {
                errorMessageCode = ((MDMOperationException)ex).MessageCode;
            }

            Assert.AreEqual("112228", errorMessageCode);
        }

        #endregion Get By Catalog Id

        #region Private Methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="programName">Name of the programm</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="hierarchyId">The hierarchy identifier.</param>
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="outputHCategoryAttributeMappingCollection">Expected output value</param>
        /// <exception cref="System.Exception">
        /// </exception>
        private void GetInputOutput(String methodName, out String programName, out Int32 categoryId, out Int32 hierarchyId, out Int32 catalogId,
                                    out CategoryAttributeMappingCollection outputHCategoryAttributeMappingCollection)
        {
            programName = String.Empty;
            outputHCategoryAttributeMappingCollection = new CategoryAttributeMappingCollection();
            categoryId = 0;
            catalogId = 0;
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
                                        case "categoryid":
                                            categoryId = ValueTypeHelper.Int32TryParse(value, -1);
                                            break;
                                        case "hierarchyid":
                                            hierarchyId = ValueTypeHelper.Int32TryParse(value, -1);
                                            break;
                                        case "catalogid":
                                            catalogId = ValueTypeHelper.Int32TryParse(value, -1);
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

                            outputHCategoryAttributeMappingCollection = new CategoryAttributeMappingCollection(value);
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

        #endregion Private Methods

    }
}
