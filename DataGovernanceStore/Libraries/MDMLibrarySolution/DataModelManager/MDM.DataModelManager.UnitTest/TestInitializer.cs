using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.DataModelManager.UnitTest
{
    using MDM.UnitTestFramework;

    [TestClass()]
    public class TestInitializer
    {
        [AssemblyInitialize()]
        public static void AssemblyInitialize(TestContext context)
        {
            UnitTestFrameworkUtility utility = new UnitTestFrameworkUtility();

            utility.DeployAndInitializeTestResources(context.DeploymentDirectory);

            utility.LoadSecurityPrincipal("cfadmin");
        }
    }
}


