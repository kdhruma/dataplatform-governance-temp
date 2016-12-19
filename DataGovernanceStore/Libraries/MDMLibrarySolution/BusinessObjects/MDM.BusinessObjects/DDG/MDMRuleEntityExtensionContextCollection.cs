using System;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies collection of MDMRule entity extension context 
    /// </summary>
    public class MDMRuleEntityExtensionContextCollection : InterfaceContractCollection<IMDMRuleEntityExtensionContext, MDMRuleEntityExtensionContext>, IMDMRuleEntityExtensionContextCollection
    {
         #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleEntityExtensionContextCollection()
            : base()
        {
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule entity extension context collection</param>
        public MDMRuleEntityExtensionContextCollection(String valuesAsXml)
        {
            LoadEntityExtensionContexts(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule entity extension context collection
        /// </param>
        public void LoadEntityExtensionContexts(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleEntityExtensionContext")
                        {
                            String context = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(context))
                            {
                                MDMRuleEntityExtensionContext extensionContext = new MDMRuleEntityExtensionContext(context);

                                if (extensionContext != null)
                                {
                                    this.Add(extensionContext);
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
        /// Get Xml representation of MDMRule entity extension context collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule entity extension context collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleEntityExtensionContexts");

                    if (this._items != null)
                    {
                        foreach (MDMRuleEntityExtensionContext context in this._items)
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

        #endregion Methods
    }
}
