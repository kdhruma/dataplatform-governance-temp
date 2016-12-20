using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.ConfigurationManager.UnitTest
{
    using MDM.AdminManager.Business;
    using MDM.Utility;
    using MDM.UnitTestFramework;

    /// <summary>
    /// Assembly Initializer for application configuration test project
    /// </summary>
    [TestClass()]
    public class AssemblyInitializer
    {
        /// <summary>
        /// Initialize the test resource required to run the test project
        /// </summary>
        /// <param name="context">Indicates the test context</param>
        [AssemblyInitialize()]
        public static void Initialize(TestContext context)
        {
            AppConfigurationHelper.InitializeAppConfig(new AppConfigProviderUsingDB(), false);
            UnitTestFrameworkUtility unitTestFrameWorkUtility = new UnitTestFrameworkUtility();
            unitTestFrameWorkUtility.LoadSecurityPrincipal("cfadmin");
            unitTestFrameWorkUtility.DeployAndInitializeTestResources(context.DeploymentDirectory);
        }
    }
}
