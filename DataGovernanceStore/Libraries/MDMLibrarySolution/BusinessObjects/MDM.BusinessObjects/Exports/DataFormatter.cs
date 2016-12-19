using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the data formatter object
    /// </summary>
    [DataContract]
    public class DataFormatter : MDMObject, IDataFormatter
    {
        #region Fields

        /// <summary>
        /// Field specifying data formatter type like RsXml, RsExcel etc.
        /// </summary>
        private String _type = String.Empty;

        /// <summary>
        /// Stores the file extension of the data formatter
        /// </summary>
        private String _fileExtension = String.Empty;

        /// <summary>
        /// Field specifying dataformatter setting collection
        /// </summary>
        private DataFormatterSettingCollection _dataFormatterSettings = new DataFormatterSettingCollection();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies data formatter type
        /// </summary>
        [DataMember]
        public String Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// Property specifies data formatter file extension
        /// </summary>
        [DataMember]
        public String FileExtension
        {
            get
            {
                return _fileExtension;
            }
            set
            {
                _fileExtension = value;
            }
        }

        /// <summary>
        /// Property specifies dataformatter setting collection
        /// </summary>
        [DataMember]
        public DataFormatterSettingCollection DataFormatterSettings
        {
            get
            {
                return _dataFormatterSettings;
            }
            set
            {
                _dataFormatterSettings = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes dataformatter object with default parameters
        /// </summary>
        public DataFormatter() : base() { }

        /// <summary>
        /// Initializes dataformatter object using export data formatter
        /// </summary>
        /// <param name="exportDataFormatter">Indicates the export data formatter</param>
        public DataFormatter(ExportDataFormatter exportDataFormatter)
        {
            Name = exportDataFormatter.Name;
            Type = exportDataFormatter.Type;
            Id = exportDataFormatter.Id;
            FileExtension = exportDataFormatter.FileExtension;
        }

        /// <summary>
        ///  Initializes dataformatter object with specified parameters
        /// </summary>
        /// <param name="id">id of dataformatter</param>
        /// <param name="name">name of dataformatter</param>
        /// <param name="type">type of dataformatter</param>
        public DataFormatter(Int32 id, String name, String type)
            : base(id)
        {
            this.Name = name;
            this._type = type;
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public DataFormatter(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadDataFormatter(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents dataformatter in Xml format
        /// </summary>
        /// <returns>String representation of current dataformatter object</returns>
        public override String ToXml()
        {
            String dataFormatterXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Dataformatter node start
            xmlWriter.WriteStartElement("DataFormatter");

            #region write DataFormatter properties for full DataFormatter xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Type", this.Type);
            xmlWriter.WriteAttributeString("FileExtension", this.FileExtension);

            #endregion  write DataFormatter properties for full DataFormatter xml

            if (this.DataFormatterSettings != null)
            {
                xmlWriter.WriteRaw(this.DataFormatterSettings.ToXml());
            }

            //Dataformatter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            dataFormatterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return dataFormatterXml;
        }

        /// <summary>
        /// Represents dataformatter in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current dataformatter object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String dataFormatterXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            dataFormatterXml = this.ToXml();

            return dataFormatterXml;
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
                if (obj is DataFormatter)
                {
                    DataFormatter objectToBeCompared = obj as DataFormatter;

                    if (!this.Id.Equals(objectToBeCompared.Id))
                    {
                        return false;
                    }

                    if (!this.Name.Equals(objectToBeCompared.Name))
                    {
                        return false;
                    }

                    if (!this.Type.Equals(objectToBeCompared.Type))
                    {
                        return false;
                    }

                    if (!this.FileExtension.Equals(objectToBeCompared.FileExtension))
                    {
                        return false;
                    }

                    if (!this.DataFormatterSettings.Equals(objectToBeCompared.DataFormatterSettings))
                    {
                        return false;
                    }

                    return true;
                }
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
            hashCode = base.GetHashCode() ^ this.Type.GetHashCode() ^ this.DataFormatterSettings.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the dataformatter with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadDataFormatter(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
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

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataFormatter")
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

                                if (reader.MoveToAttribute("Type"))
                                {
                                    this.Type = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FileExtension"))
                                {
                                    this.FileExtension = reader.ReadContentAsString();
                                }
                            }

                            #endregion
                        }
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataFormatterSettings")
                        {
                            // Read dataformatter setting
                            #region Read dataformatter setting
                            String dataFormatterSettingsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dataFormatterSettingsXml))
                            {
                                DataFormatterSettingCollection dataFormatterSettingCollection = new DataFormatterSettingCollection(dataFormatterSettingsXml);
                                if (dataFormatterSettingCollection != null)
                                {
                                    this.DataFormatterSettings = dataFormatterSettingCollection;
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

        #endregion Private Methods
    }
}
