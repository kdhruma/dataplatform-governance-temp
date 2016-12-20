using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the data subscriber object
    /// </summary>
    [DataContract]
    public class DataSubscriber : MDMObject, IDataSubscriber
    {
        #region Fields

        /// <summary>
        /// Field specifying data subscriber location which defines where to store file
        /// </summary>
        private String _location = String.Empty;

        /// <summary>
        /// Field specifying data subscriber filename
        /// </summary>
        private String _fileName = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies data subscriber location which defines where to store files.
        /// </summary>
        [DataMember]
        public String Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        /// <summary>
        /// Property specifies data subscriber filename
        /// </summary>
        [DataMember]
        public String FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes datasubscriber object with default parameters
        /// </summary>
        public DataSubscriber() : base() { }

        /// <summary>
        ///  Initializes datasubscriber object with specified parameters
        /// </summary>
        /// <param name="id">id of datasubscriber</param>
        /// <param name="name">name of datasubscriber</param>
        /// <param name="location">rulesetid of datasubscriber</param>
        /// <param name="fileName">fileName of datasubscriber</param>
        public DataSubscriber(Int32 id, String name, String location, String fileName)
            : base(id)
        {
            this.Name = name;
            this._location = location;
            this._fileName = fileName;
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public DataSubscriber(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadDataSubscriber(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents datasubscriber in Xml format
        /// </summary>
        /// <returns>String representation of current datasubscriber object</returns>
        public override String ToXml()
        {
            String dataSubscriberXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Datasubscriber node start
            xmlWriter.WriteStartElement("DataSubscriber");

            #region write DataSubscriber properties for full DataSubscriber xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Location", this.Location);
            xmlWriter.WriteAttributeString("FileName", this.FileName);


            #endregion  write DataSubscriber properties for full DataSubscriber xml

            //Datasubscriber node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            dataSubscriberXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return dataSubscriberXml;
        }

        /// <summary>
        /// Represents datasubscriber in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current datasubscriber object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String dataSubscriberXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            //ObjectSerialization.External is used for Export Profile
            if (objectSerialization == ObjectSerialization.External)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Datasubscriber node start
                xmlWriter.WriteStartElement("DataSubscriber");

                #region write DataSubscriber properties for full DataSubscriber xml

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("Location", this.Location);
                xmlWriter.WriteAttributeString("FileName", this.FileName);
                if (this.ExtendedProperties != null)
                {
                    xmlWriter.WriteStartElement("ExtendedProperties");
                    foreach (String extendedProperty in this.ExtendedProperties.Split('#'))
                    {
                        //ExtendedProperty node start
                        xmlWriter.WriteStartElement("ExtendedProperty");

                        String[] extendedPropertyArray = extendedProperty.Split(',');
                        xmlWriter.WriteAttributeString("Name", extendedPropertyArray[0]);
                        xmlWriter.WriteAttributeString("Value", extendedPropertyArray[1]);

                        //ExtendedProperty node end
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                #endregion  write DataSubscriber properties for full DataSubscriber xml

                //Datasubscriber node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                dataSubscriberXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            else
            {
                dataSubscriberXml = this.ToXml();
            }

            return dataSubscriberXml;
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
                if (obj is DataSubscriber)
                {
                    DataSubscriber objectToBeCompared = obj as DataSubscriber;

                    if (!this.Location.Equals(objectToBeCompared.Location))
                        return false;

                    if (!this.FileName.Equals(objectToBeCompared.FileName))
                        return false;

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
            hashCode = base.GetHashCode() ^ this.Location.GetHashCode() ^ this.FileName.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the datasubscriber with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadDataSubscriber(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <DataSubscriber Id="" Name="" Location="" FileName=""></DataSubscriber>
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
                        #region Read DataSubscriber

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataSubscriber")
                        {
                            #region Read data subscriber Properties

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

                                if (reader.MoveToAttribute("Location"))
                                {
                                    this.Location = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FileName"))
                                {
                                    this.FileName = reader.ReadContentAsString();
                                }

                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperties")
                        {
                            String attributeXml = reader.ReadInnerXml();
                            if (!String.IsNullOrWhiteSpace(attributeXml))
                            {
                                this.ExtendedProperties = LoadExtendedProperties(attributeXml);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read DataSubscriber
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
        /// <summary>
        /// method used for loading extended properties
        /// </summary>
        /// <param name="valuesAsXml">xml as input </param>
        /// <param name="objectSerialization">objectserialization</param>
        /// <returns>extendedproperty string</returns>
        private String LoadExtendedProperties(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            StringBuilder extendedProperties = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperty")
                        {
                            if (reader.HasAttributes)
                            {
                                String name = String.Empty;
                                String value = String.Empty;
                                if (reader.MoveToAttribute("Name"))
                                {
                                    name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Value"))
                                {
                                    value = reader.ReadContentAsString();
                                }
                                if (!String.IsNullOrWhiteSpace(extendedProperties.ToString()))
                                {
                                    extendedProperties.Append("#" + name + "," + value);
                                }
                                else
                                {
                                    extendedProperties.Append(name + "," + value);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
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
            return extendedProperties.ToString();
        }

        #endregion Private Methods
    }
}
