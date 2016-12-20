using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.OrganizationManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.OrganizationManager.Business;

    [TestClass]
    public class OrganizationBL_Process
    {
        #region Fields

        private const String FileName = @"ProcessOrganizationTest.xml";
        private const String FilePath = @"Data\ProcessOrganizationTest.xml";

        #endregion Fields

        #region Test Methods

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Process_Organizations_Create_One()
        {
            OrganizationCollection organizationsToProceed;
            OrganizationCollection outputOrganizations;
            String programName;

            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            
            GetInputOutput(methodName, out organizationsToProceed, out programName, out outputOrganizations);

            var organizationBl = new OrganizationBL();

            OperationResult organizationProcessOperationResult = organizationBl.Create(organizationsToProceed.First(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

            Assert.IsTrue(organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Process_Organizations_Create_Many()
        {
            OrganizationCollection organizationsToProceed;
            OrganizationCollection outputOrganizations;
            String programName;

            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            GetInputOutput(methodName, out organizationsToProceed, out programName, out outputOrganizations);

            foreach (var organization in organizationsToProceed)
            {
                organization.Action = ObjectAction.Create;
            }

            var organizationBl = new OrganizationBL();
            OperationResultCollection organizationProcessOperationResult = organizationBl.Process(organizationsToProceed, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

            Assert.IsTrue(organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None);
        }

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Process_Organizations_Update_One()
        {
            OrganizationCollection organizationsToProcess;
            OrganizationCollection outputOrganizations;
            String programName;

            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out organizationsToProcess, out programName, out outputOrganizations);

            if(organizationsToProcess == null)
                Assert.Fail("Failed to load Organization");

            OrganizationBL organizationBl = new OrganizationBL();

            //First create the organization
            OperationResult organizationProcessOperationResult = organizationBl.Create(organizationsToProcess.First(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

            if (organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None)
            {
                //Do Get by Id
                Organization organizationToUpdate = organizationBl.GetById(organizationProcessOperationResult.Id, new OrganizationContext(),  new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

                if (organizationToUpdate != null)
                {
                    organizationToUpdate.Name = organizationToUpdate.Name + " - Updated";

                    OperationResult or = organizationBl.Update(organizationToUpdate, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, "Unit test"));

                    if (or.OperationResultStatus == OperationResultStatusEnum.Successful ||
                        or.OperationResultStatus == OperationResultStatusEnum.None)
                    {
                        Organization updatedOrganization = organizationBl.GetById(or.Id, new OrganizationContext(),  new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));
                        outputOrganizations.FirstOrDefault().Id = updatedOrganization.Id;

                        if (updatedOrganization != null)
                        {
                            Assert.AreEqual(updatedOrganization.ToXml(), outputOrganizations.FirstOrDefault().ToXml());
                        }
                        else
                        {
                            Assert.Fail("Failed to get organization after update");
                        }
                    }
                    else
                    {
                        Assert.Fail("Failed to update the organization");
                    }
                }
                else
                {
                    Assert.Fail("Failed to get organization after creating it");
                }
            }
            else
            {
                Assert.Fail("Create of organization failed");
            }
        }

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Process_Organizations_Update_Many()
        {
            OrganizationCollection organizationsToProcess;
            OrganizationCollection outputOrganizations;
            String programName;

            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out organizationsToProcess, out programName, out outputOrganizations);

            if (organizationsToProcess == null)
                Assert.Fail("Failed to load Organizations");

            OrganizationBL organizationBl = new OrganizationBL();

            foreach (var organization in organizationsToProcess)
            {
                organization.Action = ObjectAction.Create;
            }

            //First create the organization
            OperationResultCollection organizationProcessOperationResult = organizationBl.Process(organizationsToProcess, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

            if (organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None)
            {
                OrganizationCollection organizationsToUpdate = new OrganizationCollection();
                //Do Get by Ids
                var updatedOrganizationIds = GetIds(organizationProcessOperationResult);
                foreach (var id in updatedOrganizationIds)
                {
                    organizationsToUpdate.Add(organizationBl.GetById(id, new OrganizationContext(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName)));
                }

                if (organizationsToUpdate != null)
                {
                    // Update Names
                    foreach (var organization in organizationsToUpdate)
                    {
                        organization.Name = organization.Name + " - Updated";
                        organization.Action = ObjectAction.Update;
                    }

                    OperationResultCollection or = organizationBl.Process(organizationsToUpdate, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

                    if (or.OperationResultStatus == OperationResultStatusEnum.Successful ||
                        or.OperationResultStatus == OperationResultStatusEnum.None)
                    {
                        OrganizationCollection updatedOrganizations = new OrganizationCollection();

                        foreach (var id in updatedOrganizationIds)
                        {
                            updatedOrganizations.Add(organizationBl.GetById(id, new OrganizationContext(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName)));
                        }

                        for (var i = 0; i < updatedOrganizations.Count; i++)
                        {
                            outputOrganizations.ElementAt(i).Id = updatedOrganizations.ElementAt(i).Id;
                        }

                        if (updatedOrganizations != null)
                        {
                            Assert.AreEqual(updatedOrganizations.ToXml(), outputOrganizations.ToXml());
                        }
                        else
                        {
                            Assert.Fail("Failed to get organization after update");
                        }
                    }
                    else
                    {
                        Assert.Fail("Failed to update the organization");
                    }
                }
                else
                {
                    Assert.Fail("Failed to get organization after creating it");
                }
            }
            else
            {
                Assert.Fail("Create of organization failed");
            }
        }

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Process_Organizations_Delete_One()
        {
            OrganizationCollection organizationsToProcess;
            OrganizationCollection outputOrganizations;
            String programName;

            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out organizationsToProcess, out programName, out outputOrganizations);

            if (organizationsToProcess == null)
                Assert.Fail("Failed to load Organization");

            OrganizationBL organizationBl = new OrganizationBL();

            //First create the organization
            OperationResult organizationProcessOperationResult = organizationBl.Create(organizationsToProcess.First(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

            if (organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None)
            {
                //Do Get by Id
                Organization organizationToDelete = organizationBl.GetById(organizationProcessOperationResult.Id, new OrganizationContext(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

                if (organizationToDelete != null)
                {
                    OperationResult or = organizationBl.Delete(organizationToDelete, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, "Unit test"));

                    if (or.OperationResultStatus == OperationResultStatusEnum.Successful ||
                        or.OperationResultStatus == OperationResultStatusEnum.None)
                    {
                        Organization deletedOrganization = organizationBl.GetById(or.Id, new OrganizationContext(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

                        if (deletedOrganization == null)
                        {
                            Assert.IsTrue(true, "Deleted Organization Get didn't return any Organization");
                        }
                        else
                        {
                            Assert.Fail("Failed to get organization after delete");
                        }
                    }
                    else
                    {
                        Assert.Fail("Failed to delete the organization");
                    }
                }
                else
                {
                    Assert.Fail("Failed to get organization after creating it");
                }
            }
            else
            {
                Assert.Fail("Create of organization failed");
            }
        }

        [TestMethod]
        [DeploymentItem(FilePath)]
        public void Process_Organizations_Delete_Many()
        {
            OrganizationCollection organizationsToProcess;
            OrganizationCollection outputOrganizations;
            String programName;

            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out organizationsToProcess, out programName, out outputOrganizations);

            if (organizationsToProcess == null)
                Assert.Fail("Failed to load Organizations");

            OrganizationBL organizationBl = new OrganizationBL();

            foreach (var organization in organizationsToProcess)
            {
                organization.Action = ObjectAction.Create;
            }

            //First create the organization
            OperationResultCollection organizationProcessOperationResult = organizationBl.Process(organizationsToProcess, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

            if (organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None)
            {
                OrganizationCollection organizationsToDelete = new OrganizationCollection();
                //Do Get by Ids
                var deletedOrganizationsIds = GetIds(organizationProcessOperationResult);
                foreach (var id in deletedOrganizationsIds)
                {
                    organizationsToDelete.Add(organizationBl.GetById(id, new OrganizationContext(), new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName)));
                }

                if (organizationsToDelete != null)
                {
                    // Update Names
                    foreach (var organization in organizationsToDelete)
                    {
                        organization.Action = ObjectAction.Delete;
                    }

                    OperationResultCollection or = organizationBl.Process(organizationsToDelete, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));

                    if (or.OperationResultStatus == OperationResultStatusEnum.Successful ||
                        or.OperationResultStatus == OperationResultStatusEnum.None)
                    {
                        OrganizationCollection deletedOrganizations = new OrganizationCollection();

                        foreach (var id in deletedOrganizationsIds)
                        {
                            var deletedOrganization = organizationBl.GetById(id, new OrganizationContext(),
                                new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export, programName));
                            if(deletedOrganization != null)
                                deletedOrganizations.Add(deletedOrganization);
                        }

                        if (deletedOrganizations.Count == 0)
                        {
                            Assert.IsTrue(true, "Deleted Organization Get didn't return any Organization");
                        }
                        else
                        {
                            Assert.Fail("Failed to get organization after delete");
                        }
                    }
                    else
                    {
                        Assert.Fail("Failed to delete the organization");
                    }
                }
                else
                {
                    Assert.Fail("Failed to get organization after creating it");
                }
            }
            else
            {
                Assert.Fail("Create of organization failed");
            }
        }

        // todo: Create test methods which will work with Organizations with Attributes

        #endregion
        
        #region Private methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="organizationsToProcess"></param>
        /// <param name="programName"></param>
        /// <param name="outputOrganizations"></param>
        private void GetInputOutput(String methodName, out OrganizationCollection organizationsToProcess, out String programName, out OrganizationCollection outputOrganizations)
        {
            organizationsToProcess = new OrganizationCollection();
            programName = String.Empty;
            outputOrganizations = new OrganizationCollection();

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
                                        case "organizations":
                                            organizationsToProcess = new OrganizationCollection(value);
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
                                outputOrganizations = new OrganizationCollection(organizationsXml);
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

        private List<Int32> GetIds(OperationResultCollection operationResultCollection)
        {
            return operationResultCollection.Select(operation => operation.Id).ToList();
        }

        #endregion
    }
}