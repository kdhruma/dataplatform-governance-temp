using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the export setting object
    /// </summary>
    [DataContract]
    public class ExportSetting : ObjectBase, IExportSetting
    {
        #region Fields
        [DataMember]
        private String _exportSettingName = "ExportSetting";

        /// <summary>
        /// Field specifying export setting name
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Field specifying export setting value
        /// </summary>
        private String _value = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies export setting name
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
        /// Property specifies export setting value
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
        /// Initializes exportsetting object with default parameters
        /// </summary>
        public ExportSetting(String exportSettingName) 
        {
            _exportSettingName = exportSettingName;
        }

        /// <summary>
        ///  Initializes exportsetting object with specified parameters
        /// </summary>
        /// <param name="exportSettingName">name of exportsetting</param>
        /// <param name="name">name of exportsetting</param>
        /// <param name="value">value of exportsetting</param>
        public ExportSetting(String exportSettingName,String name, String value):this(exportSettingName)
        {
            this._name = name;
            this._value = value;
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="exportSettingName">name of export setting</param>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public ExportSetting(String exportSettingName, String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full):this(exportSettingName)
        {
            LoadExportSetting(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents Exportsetting in Xml format
        /// </summary>
        /// <returns>String representation of current Exportsetting object</returns>
        public String ToXml()
        {
            String exportSettingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Execution setting Item node start
            xmlWriter.WriteStartElement(_exportSettingName);

            #region write export Setting for Full ExportSetting Xml

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Value", this.Value);

            #endregion write export Setting for Full ExportSetting Xml

            //Execution setting Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            exportSettingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return exportSettingXml;
        }

        /// <summary>
        /// Represents exportsetting in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current exportsetting object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String exportSettingXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            exportSettingXml = this.ToXml();

            return exportSettingXml;
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
                if (obj is ExportSetting)
                {
                    ExportSetting objectToBeCompared = obj as ExportSetting;

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
        /// Loads the exportsetting with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExportSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <ExportSetting Name="ExecutionType" Value="" /> <!-- Possible values are "Full" or "Delta". Legacy property name is "Type" -->
			    <ExportSetting Name="FirstTimeAsFull" Value="" />
			    <ExportSetting Name="FromTime" Value="" />
			    <ExportSetting Name="Label" Value="" />
			    <ExportSetting Name="StartWithAllCommonAttributes" Value="" />
			    <ExportSetting Name="StartWithAllCategoryAttributes" Value="" />
			    <ExportSetting Name="StartWithAllSystemAttributes" Value="" />
			    <ExportSetting Name="StartWithAllWorkflowAttributes" Value="" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == _exportSettingName)
                        {
                            #region Read ExportSetting

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
