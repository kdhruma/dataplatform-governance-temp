using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml;
using MDM.UnitTestUtility;

namespace MDM.RelationshipManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.RelationshipManager.Business;

    /// <summary>
    /// Unit test class for RelationshipType get
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Data\RelationshipTypeBL_GetAll.xml")]
    public class RelationshipTypeBL_GelAllRelationshipTypes
    {
        #region Fields

        const String _fileName = @"RelationshipTypeBL_GetAll.xml";

        #endregion Fields

        #region Test methods

        /// <summary>
        /// Empty method to check if fwx is running
        /// </summary>
        [TestMethod]
        public void EmptyTest()
        {
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Get all relationshipTypes in system
        /// </summary>
        [TestMethod]
        [TestCategory(UnitTestCategoryTypes.BVTTestList)]
        [TestCategory(UnitTestCategoryTypes.BusinessLayerTest)]
        public void GetAllRelationshipTypes_ProperValue()
        {
            RelationshipTypeCollection expectedRelationshipTypes;
            Int32 relationshipTypeId;
            String name;

            String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            GetInputOutput(methodName, out expectedRelationshipTypes, out relationshipTypeId, out name);

            RelationshipTypeBL relationshipTypeBL = new RelationshipTypeBL();
            RelationshipTypeCollection actualRelationshipTypes = relationshipTypeBL.GetAll(new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling));

            Assert.IsNotNull(actualRelationshipTypes, "Relationship types cannot not be null");
            Assert.IsTrue(actualRelationshipTypes.Count > 0, "No Relationship Types were found");

            RelationshipType expectedRelationshipType = expectedRelationshipTypes.FirstOrDefault();

            Assert.IsNotNull(expectedRelationshipType, "No input Relationship Type found");

            IRelationshipType existingRelationshipType = actualRelationshipTypes.Get(expectedRelationshipType.Name);

            Assert.IsNotNull(existingRelationshipType, "No Expected Relationship Type found in database");
        }

        #endregion Test methods

        #region Helper methods

        /// <summary>
        /// Read input and output value for method from Xml
        /// </summary>
        /// <param name="methodName">Name of method for which we are looking for input and output value from Xml</param>
        /// <param name="relationshipTypes">Relationship Types which are configured in file</param>
        /// <param name="relationshipTypeId">Relationship Type Id which is configured in file</param>
        /// <param name="name"></param>
        private void GetInputOutput(String methodName, out RelationshipTypeCollection relationshipTypes, out Int32 relationshipTypeId, out String name)
        {
            relationshipTypes = new RelationshipTypeCollection();
            relationshipTypeId = -1;
            name = String.Empty;

            //Get Data Xml from config file for method that we are looking at.
            String dataXml = DataReader.ReadMethodData(_fileName, methodName);

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
                                        case "relationshipTypeid":
                                            relationshipTypeId = ValueTypeHelper.Int32TryParse(value, -1);
                                            break;
                                        case "name":
                                            name = value;
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
                            String relationshipTypesXml = reader.ReadInnerXml();
                            if (String.IsNullOrWhiteSpace(relationshipTypesXml))
                            {
                                throw new Exception(String.Concat("No output value found for method : ", methodName));
                            }
                            else
                            {
                                relationshipTypes = new RelationshipTypeCollection(relationshipTypesXml);
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
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion Helper methods
    }
}
