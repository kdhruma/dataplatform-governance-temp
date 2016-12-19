using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;

    /// <summary>
    /// Specifies the profilesetting object
    /// </summary>
    [DataContract]
    public class ProfileSetting : ObjectBase, IProfileSetting
    {
        #region Fields

        /// <summary>
        /// Field specifying profilesetting name
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Field specifying profilesetting value
        /// </summary>
        private String _value = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies profilesetting name
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
        /// Property specifies profilesetting value
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
        /// Initializes profilesetting object with default parameters
        /// </summary>
        public ProfileSetting() : base() { }

        /// <summary>
        ///  Initializes profilesetting object with specified parameters
        /// </summary>
        /// <param name="name">name of profilesetting</param>
        /// <param name="value">value of profilesetting</param>
        public ProfileSetting(String name, String value)
        {
            this._name = name;
            this._value = value;
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public ProfileSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadProfileSetting(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents profilesetting in Xml format
        /// </summary>
        /// <returns>String representation of current profilesetting object</returns>
        public String ToXml()
        {
            String profileSettingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Profile setting Item node start
            xmlWriter.WriteStartElement("ProfileSetting");

            #region write profile Setting for Full ProfileSetting Xml

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Value", this.Value);

            #endregion write profile Setting for Full ProfileSetting Xml

            //Profile setting Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            profileSettingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return profileSettingXml;
        }

        /// <summary>
        /// Represents profilesetting in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current profilesetting object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String profileSettingXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            profileSettingXml = this.ToXml();

            return profileSettingXml;
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
                if (obj is ProfileSetting)
                {
                    ProfileSetting objectToBeCompared = obj as ProfileSetting;

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
        /// Loads the profilesetting with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadProfileSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
             * <ProfileSetting Name="Desciption" Value="Exports RS XML 4.1 for " />
		     * <ProfileSetting Name="PromotedCopy" Value="false" /> 
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProfileSetting")
                        {
                            #region Read ProfileSetting

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
