using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using MDM.BusinessObjects;
using MDM.Core;
using MDM.Core.Exceptions;
using MDM.OrganizationManager.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.OrganizationManager.UnitTest
{
    [TestClass]
    public class OrganizationBL_Get
    {
        #region Fields

        private const String FilePath = @"Data\GetOrganizationTest.xml";
        private const String FileName = "GetOrganizationTest.xml";

        #endregion Fields

        #region Test Methods

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Get_All_Organizations()
        {
            OrganizationCollection expectedOrganizations;
            Int32 organizationId;
            String programName;

            String methodName = MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out expectedOrganizations, out programName, out organizationId);

            var organizationBl = new OrganizationBL();
            var actualOrganizations =
                organizationBl.GetAll(new OrganizationContext(), new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Import, programName));

            Assert.AreEqual(expectedOrganizations.ToXml(), actualOrganizations.ToXml());
        }

        // todo: Create method for Get Organizations with attributes
        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Get_All_Organizations_With_Attributes()
        {

        }

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Get_By_Id_Less_Than_0()
        {
            OrganizationCollection expectedOrganizations;
            Int32 organizationId;
            String errorMessageCode = String.Empty;
            String programName;

            String methodName = MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out expectedOrganizations, out programName, out organizationId);

            try
            {
                var organizationBl = new OrganizationBL();
                var actualOrganization = organizationBl.GetById(organizationId, new OrganizationContext(), 
                    new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Import, programName));
            }
            catch (Exception exc)
            {
                errorMessageCode = ((MDMOperationException) exc).MessageCode;
            }
            Assert.AreEqual("111864", errorMessageCode);
        }

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Get_By_Id_ProperValue()
        {
            OrganizationCollection expectedOrganizations;
            Int32 organizationId;
            String programName;

            String methodName = MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out expectedOrganizations, out programName, out organizationId);

            var organizationBl = new OrganizationBL();
            var actualOrganization = organizationBl.GetById(organizationId, new OrganizationContext(),
                new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Import, programName));

            Assert.AreEqual(expectedOrganizations.First().ToXml(), actualOrganization.ToXml());
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="organizations"></param>
        /// <param name="programName"></param>
        /// <param name="organizationId"></param>
        private void GetInputOutput(String methodName, out OrganizationCollection organizations, out String programName, out Int32 organizationId)
        {
            organizations = new OrganizationCollection();
            organizationId = -1;
            programName = String.Empty;

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
                                        case "organizationid":
                                            organizationId = ValueTypeHelper.Int32TryParse(value, -1);
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
                            //Read output for this method.
                            String organizationsXml = reader.ReadInnerXml();
                            if (String.IsNullOrWhiteSpace(organizationsXml))
                            {
                                throw new Exception(String.Concat("No output value found for method : ", methodName));
                            }
                            else
                            {
                                organizations = new OrganizationCollection(organizationsXml);
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
