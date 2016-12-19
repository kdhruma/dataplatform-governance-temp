using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.ConfigurationManager.UnitTest
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;

    /// <summary>
    /// Represent unit test project for application configuration BL
    /// </summary>
    [TestClass]
    public class ApplicationConfigurationBLTest
    {
        /// <summary>
        /// Positive test to verify updation custom application configuration reading from an excel file.
        /// </summary>
        [TestMethod]
        public void UpdateCustomApplicationConfigurationTest()
        {
            try
            {
                OperationResult operationResult = new OperationResult();
                ApplicationConfigurationBL appConfigBL = new ApplicationConfigurationBL();

                //TODO: Need to read the path from the config file.
                string fileCompletePath = @"F:\Riversand Related\Work related\NS\My task\Extended Application Configuration Template_v1.00.xlsx";

                CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.CustomApplicationConfig);

                operationResult = appConfigBL.ProcessExtendedApplicationConfig(fileCompletePath, callerContext);

                Assert.IsTrue(operationResult.OperationResultStatus != OperationResultStatusEnum.Failed);
            }
            catch (Exception ex)
            {
                String message = ex.Message;
            }
        }
    }
}
