using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// Specifies DDG JobProcessingOptions
    /// </summary>
    [DataContract]
    public class DDGJobProcessingOptions : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Indicates batch size for the processing
        /// </summary>
        private Int32 _batchSize = -1;

        /// <summary>
        /// Indicates ProcessingType
        /// </summary>
        private ImportProcessingType _processingType = ImportProcessingType.ValidateAndProcess;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Indicates the Procesing type of the Import job
        /// </summary>
        [DataMember]
        public ImportProcessingType DDGImportProcessingType
        {
            get { return _processingType; }
            set { _processingType = value; }
        }

        /// <summary>
        /// Indicates the batch size for the DDG record thread.
        /// </summary>
        [DataMember]
        public Int32 BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DDGJobProcessingOptions()
            : base()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">DDGJobProcessingOptions Object having xml values</param>
        public DDGJobProcessingOptions(String valuesAsXml)
        {
            LoadDDGJobProcessingOptions(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents DDG JobProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current DDG JobProcessingOptions object instance</returns>
        public String ToXml()
        {
            #region Sample Xml

            // <DDGJobProcessingOptions ImportProcessingType="ValidateAndProcess" BatchSize="100" />

            #endregion Sample Xml

            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("DDGJobProcessingOptions");

            xmlWriter.WriteAttributeString("BatchSize", this.BatchSize.ToString());
            xmlWriter.WriteAttributeString("ImportProcessingType", this._processingType.ToString());

            //JobProcessingOptions end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadDDGJobProcessingOptions(String valuesAsXml)
        {
            #region Sample Xml

            // <DDGJobProcessingOptions ImportProcessingType="ValidateAndProcess" BatchSize="100" />

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DDGJobProcessingOptions")
                        {
                            #region Read JobProcessingOptions Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("BatchSize"))
                                {
                                    this.BatchSize = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ImportProcessingType"))
                                {
                                    ImportProcessingType importProcessingType = ImportProcessingType.ValidateAndProcess;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out importProcessingType);
                                    this.DDGImportProcessingType = importProcessingType;
                                }
                            }

                            #endregion Read JobProcessingOptions Properties
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
