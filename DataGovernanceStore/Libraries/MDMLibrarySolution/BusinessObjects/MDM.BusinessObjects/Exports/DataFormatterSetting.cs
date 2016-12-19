﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the execution setting object
    /// </summary>
    [DataContract]
    public class DataFormatterSetting : ObjectBase, IDataFormatterSetting
    {
        #region Fields

        /// <summary>
        /// Field specifying execution setting name
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Field specifying execution setting value
        /// </summary>
        private String _value = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies execution setting name
        /// </summary>
        [DataMember]
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Property specifies execution setting value
        /// </summary>
        [DataMember]
        public String Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes executionsetting object with default parameters
        /// </summary>
        public DataFormatterSetting() : base() { }

        /// <summary>
        ///  Initializes executionsetting object with specified parameters
        /// </summary>
        /// <param name="name">name of executionsetting</param>
        /// <param name="value">value of executionsetting</param>
        public DataFormatterSetting(String name, String value)
        {
            this._name = name;
            this._value = value;
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public DataFormatterSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadDataFormatterSetting(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents executionsetting in Xml format
        /// </summary>
        /// <returns>String representation of current executionsetting object</returns>
        public String ToXml()
        {
            String dataFormatterSettingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Execution setting Item node start
            xmlWriter.WriteStartElement("DataFormatterSetting");

            #region write execution Setting for Full DataFormatterSetting Xml

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Value", this.Value);

            #endregion write execution Setting for Full DataFormatterSetting Xml

            //Execution setting Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            dataFormatterSettingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return dataFormatterSettingXml;
        }

        /// <summary>
        /// Represents executionsetting in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current executionsetting object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String dataFormatterSettingXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            dataFormatterSettingXml = this.ToXml();

            return dataFormatterSettingXml;
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
                if (obj is DataFormatterSetting)
                {
                    DataFormatterSetting objectToBeCompared = obj as DataFormatterSetting;

                    if (!this.Name.Equals(objectToBeCompared.Name))
                        return false;

                    if (!this.Value.Equals(objectToBeCompared.Value))
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
            hashCode = base.GetHashCode() ^ this.Name.GetHashCode() ^ this.Value.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the executionsetting with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadDataFormatterSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <DataFormatterSetting Name="ExecutionType" Value="" /> <!-- Possible values are "Full" or "Delta". Legacy property name is "Type" -->
			    <DataFormatterSetting Name="FirstTimeAsFull" Value="" />
			    <DataFormatterSetting Name="FromTime" Value="" />
			    <DataFormatterSetting Name="Label" Value="" />
			    <DataFormatterSetting Name="StartWithAllCommonAttributes" Value="" />
			    <DataFormatterSetting Name="StartWithAllCategoryAttributes" Value="" />
			    <DataFormatterSetting Name="StartWithAllSystemAttributes" Value="" />
			    <DataFormatterSetting Name="StartWithAllWorkflowAttributes" Value="" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataFormatterSetting")
                        {
                            #region Read DataFormatterSetting

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.Value = reader.ReadContentAsString();
                                }

                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
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
        }

        #endregion Private Methods
    }
}