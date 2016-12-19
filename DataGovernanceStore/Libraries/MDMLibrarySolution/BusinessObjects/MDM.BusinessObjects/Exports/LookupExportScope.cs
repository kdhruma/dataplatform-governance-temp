using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the Lookup Export scope object
    /// </summary>
    [DataContract]
    public class LookupExportScope : MDMObject,ILookupExportScope
    {
        #region Fields
        String _exportMask = String.Empty;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportScope"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public LookupExportScope()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportScope"/> class. 
        /// with value as Constructor
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public LookupExportScope(String valuesAsXml)
        {
            LoadLookupExportScope(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        /// <summary>
        /// Export Mask Attributes for lookups
        /// </summary>
        [DataMember]
        public String ExportMask
        {
            get
            {
                return _exportMask;
            }
            set
            {
                _exportMask = value;
            } 
        }
        #region Public Methods

        /// <summary>
        /// Represents LookupExportScope in Xml format
        /// </summary>
        /// <returns>String representation of current LookupExportScope object</returns>
        public override String ToXml()
        {
            String lookupExportScopeXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("LookupExportScope");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ExportMask", this.ExportMask);

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            lookupExportScopeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return lookupExportScopeXml;
        }

        /// <summary>
        /// Loads the lookup export scopes with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        private void LoadLookupExportScope(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read syndication export profile data

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupExportScope")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ExportMask"))
                                {
                                    this.ExportMask = reader.ReadContentAsString();
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read syndication export profile data
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

        #endregion Public Methods
    }
}
