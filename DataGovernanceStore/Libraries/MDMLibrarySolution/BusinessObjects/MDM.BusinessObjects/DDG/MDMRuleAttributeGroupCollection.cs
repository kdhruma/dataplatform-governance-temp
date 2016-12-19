using System;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of MDMRule Attribute Group
    /// </summary>
    public class MDMRuleAttributeGroupCollection : InterfaceContractCollection<IMDMRuleAttribute, MDMRuleAttribute>, IMDMRuleAttributeGroupCollection
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleAttributeGroupCollection()
            : base()
        {
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule attribute group collection</param>
        public MDMRuleAttributeGroupCollection(String valuesAsXml)
        {
            LoadAttributeGroupContexts(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule attribute group collection
        /// </param>
        public void LoadAttributeGroupContexts(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttribute")
                        {
                            String ruleAttributeGroup = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(ruleAttributeGroup))
                            {
                                MDMRuleAttribute attributeGroup = new MDMRuleAttribute(ruleAttributeGroup);

                                if (attributeGroup != null)
                                {
                                    this.Add(attributeGroup);
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
        /// Get Xml representation of MDMRule attribute group collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule attribute group collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleAttributeGroups");

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
        /// <param name="attributeGroupName">Indicates the attribute Group Name</param>
        /// <param name="isRequired">Indicates whether to consider only required attributes or all attributes </param>
        public void AddMDMAttributeGroup(String attributeGroupName, Boolean isRequired = false)
        {
            this.Add(new MDMRuleAttribute() { Name = attributeGroupName, IsRequired = isRequired });
        }

        /// <summary>
        /// Gets all the attribute names
        /// </summary>
        /// <returns>Returns the list of attribute names</returns>
        public Collection<String> GetAttributeGroupNames()
        {
            Collection<String> attributeGroupNames = null;

            if (this._items != null && this._items.Count > 0)
            {
                attributeGroupNames = new Collection<String>();

                foreach (MDMRuleAttribute attributeGroup in this._items)
                {
                    if (!attributeGroupNames.Contains(attributeGroup.Name))
                    {
                        attributeGroupNames.Add(attributeGroup.Name);
                    }
                }
            }

            return attributeGroupNames;
        }

        /// <summary>
        /// Checks requrested attribute group name is exist in the collection or not
        /// </summary>
        /// <returns>Returns true if requested attribute group name is already exist else retuns false</returns>
        public Boolean ContainsAttributeGroupName(String attributeGroupName)
        {
            Boolean result = false;
            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMRuleAttribute attributeGroup in this._items)
                {
                    if (String.Compare(attributeGroup.Name, attributeGroupName, true) == 0)
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
