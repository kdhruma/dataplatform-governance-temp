//To be executed only in development environment when you do not want to run Unit Tests in IIS/Web context
//#if DEBUG
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.EntityManager.UnitTest
{
    using MDM.UnitTestFramework;

    [TestClass()]
    public class AssemblyInitializer
    {
        [AssemblyInitialize()]
        public static void Initialize(TestContext context)
        {
            UnitTestFrameworkUtility unitTestFrameWorkUtility = new UnitTestFrameworkUtility();
            unitTestFrameWorkUtility.DeployAndInitializeTestResources(context.DeploymentDirectory);
            unitTestFrameWorkUtility.LoadSecurityPrincipal("system");
        }
    }
}

//#endif
