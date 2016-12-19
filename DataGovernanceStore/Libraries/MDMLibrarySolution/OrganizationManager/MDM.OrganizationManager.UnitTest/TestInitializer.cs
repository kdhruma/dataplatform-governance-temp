using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.OrganizationManager.UnitTest
{
    using MDM.UnitTestFramework;

    [TestClass]
    public class TestInitializer
    {
        [AssemblyInitialize()]
        public static void AssemblyInitialize(TestContext context)
        {
            UnitTestFrameworkUtility utility = new UnitTestFrameworkUtility();

            //To be executed only in development environment when you do not want to run Unit Tests in IIS/Web context
            #if DEBUG

            utility.DeployAndInitializeTestResources(context.DeploymentDirectory);

            #endif

            utility.LoadSecurityPrincipal("cfadmin");
        }
    }
}