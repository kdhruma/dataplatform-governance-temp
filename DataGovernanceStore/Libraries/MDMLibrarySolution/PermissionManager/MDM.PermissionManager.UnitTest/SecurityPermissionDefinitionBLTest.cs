using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.PermissionManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.PermissionManager.Business;

    /// <summary>
    /// Test class for Security Permission Definition BL
    /// </summary>
    [TestClass]
    public class SecurityPermissionDefinitionBLTest
    {
        #region TestCases

        /// <summary>
        /// Empty test case to check whether the test setting is working fine or not.
        /// </summary>
        //[TestMethod]
        //public void EmptyTest()
        //{
        //    Assert.IsTrue(true);
        //}

        ///// <summary>
        ///// Positive testcase for Get by providing Security Role
        ///// </summary>
        //[TestMethod]
        //public void Get_SecurityPermissionDefinitionCollection_With_ProperValue_SecurityRole_CallerContext()
        //{
        //    #region Object and Variable Decleration

        //    SecurityPermissionDefinitionCollection expectedSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    SecurityPermissionDefinitionCollection actualSecurityPermissions = null;

        //    SecurityRole securityRole = new SecurityRole();

        //    SecurityPermissionDefinitionBL securityPermissionBL = new SecurityPermissionDefinitionBL();

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    #endregion

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    GetSecurityRoleInputAndOutput(methodName, out securityRole, out expectedSecurityPermissions);
        //    actualSecurityPermissions = securityPermissionBL.Get(securityRole, callerContext);

        //    if (actualSecurityPermissions == null)
        //    {
        //        actualSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    }

        //    Assert.AreEqual(expectedSecurityPermissions.ToXml(), actualSecurityPermissions.ToXml());
        //}

        ///// <summary>
        ///// Negative testcase for Get by providing Security Role
        ///// </summary>
        //[TestMethod]
        //public void Get_SecurityPermissionDefinitionCollection_With_ImproperValue_SecurityRole_CallerContext()
        //{
        //    #region Object and Variable Declaration

        //    SecurityPermissionDefinitionCollection expectedSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    SecurityPermissionDefinitionCollection actualSecurityPermissions = null;

        //    SecurityRole securityRole = new SecurityRole();

        //    SecurityPermissionDefinitionBL securityPermissionBL = new SecurityPermissionDefinitionBL();

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    #endregion

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    GetSecurityRoleInputAndOutput(methodName, out securityRole, out expectedSecurityPermissions);
        //    actualSecurityPermissions = securityPermissionBL.Get(securityRole, callerContext);

        //    if (actualSecurityPermissions == null)
        //    {
        //        actualSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    }

        //    Assert.AreEqual(expectedSecurityPermissions.ToXml(), actualSecurityPermissions.ToXml());
        //}

        ///// <summary>
        ///// Positive Testcase for Get Method by providing Application Context
        ///// </summary>
        //[TestMethod]
        //public void Get_SecurityPermissionDefinitionCollection_With_ProperValue_ApplicationContext_CallerContext()
        //{
        //    #region Object and Variable Decleration

        //    SecurityPermissionDefinitionCollection expectedSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    SecurityPermissionDefinitionCollection actualSecurityPermissions = null;

        //    SecurityPermissionDefinitionBL securityPermissionBL = null;
        //    securityPermissionBL = new SecurityPermissionDefinitionBL();

        //    ApplicationContext applicationContext = new ApplicationContext();
        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    #endregion

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    GetApplicationContextInputAndOutput(methodName, out applicationContext, out expectedSecurityPermissions);

        //    actualSecurityPermissions = securityPermissionBL.Get(applicationContext, callerContext);

        //    if (actualSecurityPermissions == null)
        //    {
        //        actualSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    }

        //    Assert.AreEqual(expectedSecurityPermissions.ToXml(), actualSecurityPermissions.ToXml());
        //}

        ///// <summary>
        ///// Negative Testcase for Get Method by providing Application Context
        ///// </summary>
        //[TestMethod]
        //public void Get_SecurityPermissionDefinitionCollection_With_ImproperValue_ApplicationContext_CallerContext()
        //{
        //    #region Object and Variable Decleration

        //    SecurityPermissionDefinitionCollection expectedSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    SecurityPermissionDefinitionCollection actualSecurityPermissions = null;

        //    SecurityPermissionDefinitionBL securityPermissionBL = null;
        //    securityPermissionBL = new SecurityPermissionDefinitionBL();

        //    ApplicationContext applicationContext = new ApplicationContext();
        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    #endregion

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    GetApplicationContextInputAndOutput(methodName, out applicationContext, out expectedSecurityPermissions);

        //    actualSecurityPermissions = securityPermissionBL.Get(applicationContext, callerContext);

        //    if (actualSecurityPermissions == null)
        //    {
        //        actualSecurityPermissions = new SecurityPermissionDefinitionCollection();
        //    }

        //    Assert.AreEqual(expectedSecurityPermissions.ToXml(), actualSecurityPermissions.ToXml());
        //}

        #endregion TestCases

        #region Private Methods

        /// <summary>
        /// Get the actualSecurityPermissions based on Security Role.
        /// </summary>
        /// <param name="methodName">Indicates the method name.</param>
        /// <param name="securityRole">Indicates the security Role instance.</param>
        /// <param name="actualSecurityPermissions">Indicates the actual security permissions instance.</param>
        private void GetSecurityRoleInputAndOutput(String methodName, out SecurityRole securityRole, out SecurityPermissionDefinitionCollection actualSecurityPermissions)
        {
            ApplicationContext applicationContext = null;
            applicationContext = new ApplicationContext();
            GetInputOutput(methodName, out securityRole, out applicationContext, out actualSecurityPermissions);
        }

        /// <summary>
        /// Get the actualSecurityPermissions based on application context.
        /// </summary>
        /// <param name="methodName">Indicates the method name</param>
        /// <param name="applicationContext">Indicates the application context.</param>
        /// <param name="actualSecurityPermissions">Indicates the actual security permissions instance.</param>
        private void GetApplicationContextInputAndOutput(String methodName, out ApplicationContext applicationContext, out SecurityPermissionDefinitionCollection actualSecurityPermissions)
        {
            SecurityRole securityRole = null;
            securityRole = new SecurityRole();
            GetInputOutput(methodName, out securityRole, out applicationContext, out actualSecurityPermissions);
        }

        /// <summary>
        /// Get the input and output parameters and actual security permissions definitions.
        /// </summary>
        /// <param name="methodName">Indicates the method name.</param>
        /// <param name="securityRole">Indicates the security role instance.</param>
        /// <param name="context">Indicates the application context.</param>
        /// <param name="securityPermissionDefinitions">Indicates the actual security permission definitions.</param>
        private void GetInputOutput(String methodName, out SecurityRole securityRole, out ApplicationContext context, out SecurityPermissionDefinitionCollection securityPermissionDefinitions)
        {
            context = null;
            securityRole = null;
            securityPermissionDefinitions = null;

            String fileName = "SecurityPermissionDefinitionBLTest.xml";

            //Get Data Xml from config file for method that we are looking at.
            String dataXml = DataReader.ReadMethodData(fileName, methodName);

            if (!String.IsNullOrWhiteSpace(dataXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(dataXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        //Read input parameters from DataXml
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
                                        case "applicationcontext":
                                            context = new ApplicationContext(value);
                                            break;
                                        case "securityrole":
                                            securityRole = new SecurityRole(value);
                                            break;
                                        default:
                                            throw new Exception(String.Concat("Unknown parameter : ", paramName));
                                    }
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Output")
                        {
                            //Read output for this method.
                            String securityPermissionDefinitionXml = reader.ReadInnerXml();
                            if (String.IsNullOrWhiteSpace(securityPermissionDefinitionXml))
                            {
                                throw new Exception(String.Concat("No output value found for method : ", methodName));
                            }
                            else
                            {
                                securityPermissionDefinitions = new SecurityPermissionDefinitionCollection(securityPermissionDefinitionXml);
                            }
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

        #endregion
    }
}
