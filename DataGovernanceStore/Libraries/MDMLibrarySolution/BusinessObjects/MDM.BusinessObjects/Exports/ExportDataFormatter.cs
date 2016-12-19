using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{


    using MDM.Core;
    using MDM.BusinessObjects.Interfaces.Exports;

    /// <summary>
    /// Caures the tb Formatter
    /// </summary>
    [DataContract]
    public class ExportDataFormatter : DataFormatter, IExportDataFormatter
    {

        #region Fields

        ExportFormatterExportType _exportType = ExportFormatterExportType.Unknown;
        String _displayName = String.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Formatter Export Type
        /// </summary>
        [DataMember]
        public ExportFormatterExportType ExportType
        {
            get
            {
                return _exportType;
            }
            set
            {
                _exportType = value;
            }
        }

        /// <summary>
        /// Formatter Display Name
        /// </summary>
        [DataMember]
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
               _displayName = value;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes exportDataformatter object with default parameters
        /// </summary>
        public ExportDataFormatter() : base() { }

        /// <summary>
        ///  Initializes exportDataformatter object with specified parameters
        /// </summary>
        /// <param name="id">id of exportDataformatter</param>
        /// <param name="name">name of exportDataformatter</param>
        /// <param name="type">type of exportDataformatter</param>
        public ExportDataFormatter(Int32 id, String name, String type):base(id,name,type)
        {
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public ExportDataFormatter(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadExportDataFormatter(valuesAsXml, objectSerialization);
        }

        #endregion Constructors
        #region Public Methods

        /// <summary>
        /// Represents dataformatter in Xml format
        /// </summary>
        /// <returns>String representation of current dataformatter object</returns>
        public override String ToXml()
        {
            String exportDataFormatterXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Dataformatter node start
            xmlWriter.WriteStartElement("ExportDataFormatter");

            #region write DataFormatter properties for full ExportDataFormatter xml
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Description", this.LongName);
            xmlWriter.WriteAttributeString("Type", this.Type);
            xmlWriter.WriteAttributeString("ExportType", this.ExportType.ToString());
            xmlWriter.WriteAttributeString("DisplayName", this.DisplayName);
            xmlWriter.WriteAttributeString("FileExtension", this.FileExtension);
            #endregion  write DataFormatter properties for full ExportDataFormatter xml
             
            //Dataformatter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            exportDataFormatterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return exportDataFormatterXml;
        }

        /// <summary>
        /// Represents dataformatter in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current dataformatter object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ExportDataFormatter objectToBeCompared = obj as ExportDataFormatter;

                if (objectToBeCompared == null)
                {
                    return false;
                }

                if (!this.Id.Equals(objectToBeCompared.Id))
                {
                    return false;
                }

                if (!this.Name.Equals(objectToBeCompared.Name))
                {
                    return false;
                }

                if (!this.LongName.Equals(objectToBeCompared.LongName))
                {
                    return false;
                }

                if (!this.Type.Equals(objectToBeCompared.Type))
                {
                    return false;
                }

                if (!this.ExportType.Equals(objectToBeCompared.ExportType))
                {
                    return false;
                }

                if (!this.FileExtension.Equals(objectToBeCompared.FileExtension))
                {
                    return false;
                }

                if (!this.DisplayName.Equals(objectToBeCompared.DisplayName))
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode = base.GetHashCode() ^ this.Type.GetHashCode() ^ this.ExportType.GetHashCode() ^ this.FileExtension.GetHashCode() ^ this.DisplayName.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the ExportDataformatter with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExportDataFormatter(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <DataFormatter Id="" Name="" Type="" ><DataFormatterSettings/></DataFormatter>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read DataFormatter

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportDataFormatter")
                        {
                            #region Read data formatter Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Type"))
                                {
                                    this.Type = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ExportType"))
                                {
                                    ExportFormatterExportType exportType = ExportFormatterExportType.Unknown;
                                    if (ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out exportType))
                                    {
                                        this.ExportType = exportType;
                                    }
                                }

                                if (reader.MoveToAttribute("FileExtension"))
                                {
                                    this.FileExtension = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DisplayName"))
                                {
                                    this.DisplayName = reader.ReadContentAsString();
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read DataFormatter
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
        #endregion
    }
}
