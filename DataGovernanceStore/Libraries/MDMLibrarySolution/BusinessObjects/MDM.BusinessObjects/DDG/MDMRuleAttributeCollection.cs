using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of MDMRule Attribute
    /// </summary>
    public class MDMRuleAttributeCollection : InterfaceContractCollection<IMDMRuleAttribute, MDMRuleAttribute>, IMDMRuleAttributeCollection
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleAttributeCollection()
            : base()
        {
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule attribute collection</param>
        public MDMRuleAttributeCollection(String valuesAsXml)
        {
            LoadAttributeContexts(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule attribute collection
        /// </param>
        public void LoadAttributeContexts(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttribute")
                        {
                            String ruleAttribute = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(ruleAttribute))
                            {
                                MDMRuleAttribute attribute = new MDMRuleAttribute(ruleAttribute);

                                if (attribute != null)
                                {
                                    this.Add(attribute);
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Xml representation of MDMRule attribute collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule attribute collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleAttributes");

                    if (this._items != null)
                    {
                        foreach (MDMRuleAttribute ruleAttribute in this._items)
                        {
                            xmlWriter.WriteRaw(ruleAttribute.ToXml());
                        }
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Add new MDMRule attribute into the existing collection
        /// </summary>
        /// <param name="attributeName">Indicates the attribute Name</param>
        public void AddMDMAttribute(String attributeName)
        {
            this.Add(new MDMRuleAttribute() { Name = attributeName });
        }

        /// <summary>
        /// Add MDMRuleCollectiion into the current collection
        /// </summary>
        /// <param name="items">Indicates the MDMRuleCollection </param>
        public void AddRange(MDMRuleAttributeCollection items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item != null && this._items.Contains(item) == false)
                    {
                        this._items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all the attribute names
        /// </summary>
        /// <returns>Returns the list of attribute names</returns>
        public Collection<String> GetAttributeNames()
        {
            Collection<String> attributeNames = null;

            if (this._items != null && this._items.Count > 0)
            {
                attributeNames = new Collection<String>();

                foreach (MDMRuleAttribute attribute in this._items)
                {
                    if (attributeNames.Contains(attribute.Name) == false)
                    {
                        attributeNames.Add(attribute.Name);
                    }
                }
            }

            return attributeNames;
        }

        /// <summary>
        /// Checks requrested attribute name is exist in the collection or not
        /// </summary>
        /// <returns>Returns true if requested attribute name is already exist else retuns false</returns>
        public Boolean ContainsAttributeName(String attributeGroupName)
        {
            Boolean result = false;
            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMRuleAttribute attribute in this._items)
                {
                    if (String.Compare(attribute.Name, attributeGroupName, true) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        #endregion Methods
    }
}
