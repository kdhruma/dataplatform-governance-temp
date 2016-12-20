using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace MDM.ApplicationServiceManager.UnitTest
{
    using MDM.ApplicationServiceManager.Business;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.UnitTestFramework;
    using MDM.UnitTestUtility;
    using MDM.Utility;

    ///<summary>
    /// A BL test class for creating an Application Context
    ///</summary>
    [TestClass]
    [DeploymentItem(@"Data\ApplicationContextBL_Process.xml")]
    public class ApplicationContextBL_Process
    {
        #region Initilialization
        private const String _fileName = "ApplicationContextBL_Process.xml";

        /// <summary>
        /// Indicates unittestframework utility class
        /// </summary>
        private BusinessLayerUtility _businessLayerUtility = new BusinessLayerUtility();

        #endregion Initialization

        #region Methods
        #region TestMethod
        /// <summary>
        /// Create single Application Context
        /// </summary>
        /// Ignoring this test currently as this need to be rewritten properly
        [Ignore]
        [TestMethod()]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        [TestCategory(UnitTestCategoryTypes.DynamicDataGovernance + "\\213523")]
        public void CreateAndGet_AppContext()
        {
            _businessLayerUtility.CreateAndGetAppContext(_fileName, "CreateAndGet_AppContext");
        }

        /// <summary>
        /// Create single Application Context with existing Application Context's name
        /// Type : Negative
        /// </summary>
        /// Ignoring this test currently as this need to be rewritten properly
        [Ignore]
        [TestMethod()]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        [TestCategory(UnitTestCategoryTypes.DynamicDataGovernance + "\\213582")]
        public void CreateAndGet_AppContext_Exist()
        {
            _businessLayerUtility.CreateAndGetAppContext(_fileName, "CreateAndGet_AppContext_Exist");
        }

        /// <summary>
        /// Updating Application Context
        /// Type : Negative
        /// </summary>
        /// Ignoring this test currently as this need to be rewritten properly
        [Ignore]
        [TestMethod()]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        [TestCategory(UnitTestCategoryTypes.DynamicDataGovernance + "\\213569")]
        public void CreateAndGet_Update_Exist()
        {
            _businessLayerUtility.CreateAndGetAppContext(_fileName, "CreateAndGet_Update_Exist");
        }

        /// <summary>
        /// Deleting Application Context
        /// Type : Negative
        /// </summary>
        /// Ignoring this test currently as this need to be rewritten properly
        [Ignore]
        [TestMethod()]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        [TestCategory(UnitTestCategoryTypes.DynamicDataGovernance + "\\213576")]
        public void CreateAndGet_Delete_Exist()
        {
            _businessLayerUtility.CreateAndGetAppContext(_fileName, "CreateAndGet_Delete_Exist");
        }
        #endregion TestMethod
        #endregion Methods
    }
}
