using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the Notification setting object
    /// </summary>
    [DataContract]
    public class NotificationSetting : ObjectBase, INotificationSetting
    {
        #region Fields

        /// <summary>
        /// Field specifying Notification setting name
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Field specifying Notification setting value
        /// </summary>
        private String _value = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies Notification setting name
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
        /// Property specifies Notification setting value
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
        /// Initializes Notificationsetting object with default parameters
        /// </summary>
        public NotificationSetting() : base() { }

        /// <summary>
        ///  Initializes Notificationsetting object with specified parameters
        /// </summary>
        /// <param name="name">name of Notificationsetting</param>
        /// <param name="value">value of Notificationsetting</param>
        public NotificationSetting(String name, String value)
        {
            this._name = name;
            this._value = value;
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public NotificationSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadNotificationSetting(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents Notificationsetting in Xml format
        /// </summary>
        /// <returns>String representation of current Notificationsetting object</returns>
        public String ToXml()
        {
            String NotificationSettingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Notification setting Item node start
            xmlWriter.WriteStartElement("NotificationSetting");

            #region write Notification Setting for Full NotificationSetting Xml

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Value", this.Value);

            #endregion write Notification Setting for Full NotificationSetting Xml

            //Notification setting Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            NotificationSettingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return NotificationSettingXml;
        }

        /// <summary>
        /// Represents Notificationsetting in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current Notificationsetting object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String NotificationSettingXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            NotificationSettingXml = this.ToXml();

            return NotificationSettingXml;
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
                if (obj is NotificationSetting)
                {
                    NotificationSetting objectToBeCompared = obj as NotificationSetting;

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
        /// Loads the Notificationsetting with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadNotificationSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <NotificationSetting Name="SendOnlyIfItemCountIsMoreThanZero" Value="" />
			  
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "NotificationSetting")
                        {
                            #region Read NotificationSetting

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
