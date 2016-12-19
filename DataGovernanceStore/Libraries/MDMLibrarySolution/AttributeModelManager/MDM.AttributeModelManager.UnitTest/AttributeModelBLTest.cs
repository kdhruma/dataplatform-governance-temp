using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System;

namespace MDM.AttributeModelManager.UnitTest
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.AttributeModelManager.Business;

    /// <summary>
    ///This is a test class for AttributeModelBLTest and is intended
    ///to contain all AttributeModelBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttributeModelBLTest
    {
        #region Test Methods

        /// <summary>
        ///A test for GetAttributeId by giving AttributeName and AttributeGroupName that can be Short Name / Long Name 
        ///</summary>
        //[TestMethod()]
        //public void GetAttributeId_ProperValue()
        //{
        //    AttributeModelBL target = new AttributeModelBL();
        //    String attributeName = String.Empty;
        //    String attributeGroupName = String.Empty;
        //    Int32 expected = 0;
        //    Int32 actual;

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //    GetInputOutput(methodName, out attributeName, out attributeGroupName, out expected);
        //    //Here AttributeName and AttributeGroupName both are ShortName
        //    actual = target.GetAttributeId(attributeName, attributeGroupName);

        //    Assert.AreEqual(expected, actual);
        //}
        
        ///// <summary>
        ///// Test for GetAttributeId by passing Attribute Long Name and AttributeGroupName
        ///// </summary>
        //[TestMethod]
        //public void GetAttributeId_AttributeName_LongName()
        //{
        //    AttributeModelBL target = new AttributeModelBL();
        //    String attributeName = String.Empty;
        //    String attributeGroupName = String.Empty;
        //    Int32 expected = 0;
        //    Int32 actual;

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //    GetInputOutput(methodName, out attributeName, out attributeGroupName, out expected);

        //    actual = target.GetAttributeId(attributeName, attributeGroupName);

        //    Assert.AreEqual(expected, actual);
        //}

        ///// <summary>
        ///// Test for GetAttributeId by passing AttributeGroupName Long Name and AttributeName
        ///// </summary>
        //[TestMethod()]
        //public void GetAttributeId_AttributeGroupName_LongName()
        //{
        //    AttributeModelBL target = new AttributeModelBL();
        //    String attributeName = String.Empty;
        //    String attributeGroupName = String.Empty;
        //    Int32 expected = 0;
        //    Int32 actual;

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //    GetInputOutput(methodName, out attributeName, out attributeGroupName, out expected);

        //    actual = target.GetAttributeId(attributeName, attributeGroupName);

        //    Assert.AreEqual(expected, actual);
        //}

        ///// <summary>
        ///// Test for GetAttributeId by giving Attribute Name and Attribute Group Name
        ///// <summary>
        //[TestMethod()]
        //public void GetAttributeId_MultipleId_NegativeTest()
        //{
        //    AttributeModelBL target = new AttributeModelBL();
        //    String attributeName = String.Empty;
        //    String attributeGroupName = String.Empty;
        //    Int32 expected = 0;
        //    Int32 actual;

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //    GetInputOutput(methodName, out attributeName, out attributeGroupName, out expected);

        //    actual = target.GetAttributeId(attributeName, attributeGroupName);
        //    //Failed :  Multiple attributes found with Attribute ShortName / LongName or Attribute Group ShortName / LongName 
        //    Assert.AreEqual(expected, actual);
        //}
        #endregion

        #region Helper Methods

        /// <summary>
        /// Get Input and Output for Testing the Calling Method
        /// </summary>
        /// <param name="methodName">Calling method Name</param>
        /// <param name="attributeName">AttributeName Which can be Short / Long Name</param>
        /// <param name="attributeGroupName">AttributeGroupName which can be Short / Long Name</param>
        /// <param name="expected">expected AttributeId</param>
        private void GetInputOutput(String methodName, out String attributeName, out String attributeGroupName, out Int32 expected)
        {
            expected = 0;
            attributeName = String.Empty;
            attributeGroupName = String.Empty;
            String fileName = "AttributeModelBLTest.xml";

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
                                        case "attributename":
                                            attributeName = value.ToString();
                                            break;
                                        case "attributegroupname":
                                            attributeGroupName = value.ToString();
                                            break;
                                        default:
                                            throw new Exception(String.Concat("Unknown parameter : ", paramName));
                                    }
                                }

                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Output")
                        {
                            //Read output For this Method.
                            String attributeId = reader.ReadInnerXml();
                            if (String.IsNullOrWhiteSpace(attributeId))
                            {
                                throw new Exception(String.Concat("No output value found for method : ", methodName));
                            }
                            else
                            {
                                expected = ValueTypeHelper.Int32TryParse(attributeId, 0);
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

