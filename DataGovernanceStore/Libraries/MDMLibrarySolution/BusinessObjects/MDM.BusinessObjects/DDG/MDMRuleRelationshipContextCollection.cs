using System;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of MDMRule relationship context collection
    /// </summary>
    public class MDMRuleRelationshipContextCollection : InterfaceContractCollection<IMDMRuleRelationshipContext, MDMRuleRelationshipContext>, IMDMRuleRelationshipContextCollection
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleRelationshipContextCollection()
            : base()
        {
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule relationship context collection</param>
        public MDMRuleRelationshipContextCollection(String valuesAsXml)
        {
            LoadRelationshipContexts(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule relationship context collection
        /// </param>
        public void LoadRelationshipContexts(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleRelationshipContext")
                        {
                            String context = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(context))
                            {
                                MDMRuleRelationshipContext relationshipContext = new MDMRuleRelationshipContext(context);

                                if (relationshipContext != null)
                                {
                                    this.Add(relationshipContext);
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
        /// Get Xml representation of MDMRule relationship context collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule relationship context collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleRelationshipContexts");

                    if (this._items != null)
                    {
                        foreach (MDMRuleRelationshipContext context in this._items)
                        {
                            xmlWriter.WriteRaw(context.ToXml());
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
        /// Gets the relationship context by relationship type name
        /// </summary>
        /// <param name="relationshipTypeName">Indicates the relationship type name</param>
        /// <returns>Returns the MDMRuleRelationshipContext</returns>
        public MDMRuleRelationshipContext GetRelationshipContextByTypeName(String relationshipTypeName)
        {
            MDMRuleRelationshipContext result= null;
            if (_items != null)
            {
                foreach (MDMRuleRelationshipContext item in _items)
                {
                    if (String.Compare(item.RelationshipTypeName, relationshipTypeName, true) == 0)
                    {
                        result = item;
                        break;
                    }
                }
            }
            return result;
        }

        #endregion Methods
    }
}
