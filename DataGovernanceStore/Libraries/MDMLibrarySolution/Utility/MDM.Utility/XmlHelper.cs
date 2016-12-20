using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MDM.ExceptionManager;

namespace MDM.Utility
{
    /// <summary>
    /// Specifies XMLHelper. Has helper methods for XML manipulation
    /// </summary>
    public class XmlHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets value of an Attribute having given name from given XmlNode, value returned as String
        /// </summary>
        /// <param name="xmlNode">XmlNode to look for the Attribute</param>
        /// <param name="attributeName">Attribute name for getting the value of it</param>
        /// <returns>Attribute value returned as String</returns>
        public static string GetXmlAttributeStringValue(XmlNode xmlNode, string attributeName)
        {
            string value = string.Empty;

            if (xmlNode.Attributes[attributeName] != null && xmlNode.Attributes[attributeName].Value != null)
                value = xmlNode.Attributes[attributeName].Value;

            return value;
        }

        /// <summary>
        /// Gets value of an Attribute having given name from given XmlNode, value returned as Integer
        /// </summary>
        /// <param name="xmlNode">XmlNode to look for the Attribute</param>
        /// <param name="attributeName">Attribute name for getting the value of it</param>
        /// <returns>Attribute value returned as Integer</returns>
        public static int GetXmlAttributeIntegerValue(XmlNode xmlNode, string attributeName)
        {
            int value = 0;

            if (xmlNode.Attributes[attributeName] != null && xmlNode.Attributes[attributeName].Value != null)
            {
                Int32.TryParse(xmlNode.Attributes[attributeName].Value, out value);
            }

            return value;
        }

        /// <summary>
        /// Gets value of an Attribute having given name from given XmlNode, value returned as Boolean
        /// </summary>
        /// <param name="xmlNode">XmlNode to look for the Attribute</param>
        /// <param name="attributeName">Attribute name for getting the value of it</param>
        /// <returns>Attribute value returned as String</returns>
        public static bool GetXmlAttributeBooleanValue(XmlNode xmlNode, string attributeName)
        {
            bool value = false;

            if (xmlNode.Attributes[attributeName] != null && xmlNode.Attributes[attributeName].Value != null)
            {
                value = xmlNode.Attributes[attributeName].Value == "1" || xmlNode.Attributes[attributeName].Value.ToLower() == "true" || xmlNode.Attributes[attributeName].Value.ToLower() == "y"  ? true : false;
            }

            return value;
        }

        /// <summary>
        /// Gets value of an Attribute having given name from given XmlNode, value returned as given Type T
        /// </summary>
        /// <typeparam name="T">Value returned will be in this given Type</typeparam>
        /// <param name="reader">XmlReader to be searched for finding Attribute, usually contains a single node</param>
        /// <param name="attributeName">Attribute name for getting the value of it</param>
        /// <returns>Attribute value returned as Type T</returns>
        public static T ReadXmlAttributeValueAs<T>(XmlReader reader, String attributeName)
        {
            T val = default(T);
            if (reader.MoveToAttribute(attributeName))
            {
                try
                {
                    val = (T)reader.ReadContentAs(typeof(T), null);
                }
                catch(Exception ex)
                {
                    ExceptionHandler exHandler = new ExceptionHandler(ex);
                }
            }
            return val;
        }

        #endregion

    }
}
