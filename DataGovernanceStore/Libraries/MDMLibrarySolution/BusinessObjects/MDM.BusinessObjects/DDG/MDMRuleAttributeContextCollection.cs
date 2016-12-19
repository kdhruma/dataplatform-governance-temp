using System;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of MDMRule Attribute context
    /// </summary>
    public class MDMRuleAttributeContextCollection : InterfaceContractCollection<IMDMRuleAttributeContext, MDMRuleAttributeContext>, IMDMRuleAttributeContextCollection
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleAttributeContextCollection()
            : base()
        {
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule attribute context collection</param>
        public MDMRuleAttributeContextCollection(String valuesAsXml)
        {
            LoadAttributeContexts(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule attribute context collection
        /// </param>
        public void LoadAttributeContexts(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttributeContext")
                        {
                            String context = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(context))
                            {
                                MDMRuleAttributeContext attributeContext = new MDMRuleAttributeContext(context);

                                if (attributeContext != null)
                                {
                                    this.Add(attributeContext);
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
        /// Get Xml representation of MDMRule attribute context collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule attribute context collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleAttributeContexts");

                    if (this._items != null)
                    {
                        foreach (MDMRuleAttributeContext context in this._items)
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
        /// Gets the attribute context by locale
        /// </summary>
        /// <param name="locale">Indicates the locale</param>
        /// <returns>Returns the attribute context object</returns>
        public MDMRuleAttributeContext GetAttributeContextByLocale(LocaleEnum locale)
        {
            foreach (MDMRuleAttributeContext context in this._items)
            {
                if (context.DataLocale == locale)
                {
                    return context;
                }
            }
            return null;
        }

        #endregion Methods
    }
}
