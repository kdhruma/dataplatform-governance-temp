using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the email notification object
    /// </summary>
    [DataContract]
    public class EmailNotification : MDMObject, IEmailNotification
    {
        #region Fields

        /// <summary>
        /// Field specifying email notification action
        /// </summary>
        private ExportJobStatus _action = ExportJobStatus.Unknown;

        /// <summary>
        /// Field specifying email notification emails collection
        /// </summary>
        private Collection<String> _emails = new Collection<String>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies email notification action
        /// </summary>
        [DataMember]
        public new ExportJobStatus Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Property specifies email notification emails collection
        /// </summary>
        [DataMember]
        public Collection<String> Emails
        {
            get
            {
                return _emails;
            }
            set
            {
                _emails = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes emailnotification object with default parameters
        /// </summary>
        public EmailNotification() : base() { }

        /// <summary>
        ///  Initializes emailnotification object with specified parameters
        /// </summary>
        /// <param name="action">action of emailnotification</param>
        /// <param name="emails">collection of emails of emailnotification</param>
        public EmailNotification(ExportJobStatus action, Collection<String> emails)
        {
            this._action = action;
            this._emails = emails;
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public EmailNotification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadEmailNotification(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents emailnotification in Xml format
        /// </summary>
        /// <returns>String representation of current emailnotification object</returns>
        public override String ToXml()
        {
            String emailNotificationXml = String.Empty;
            String emails = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (this.Emails != null)
            {
                emails = ValueTypeHelper.JoinCollection(this.Emails, ",");
            }

            // Email notification Item node start
            xmlWriter.WriteStartElement("EmailNotification");

            #region write emailNotification for Full EmailNotification Xml

            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("Emails", emails);

            #endregion write emailNotification for Full EmailNotification Xml

            // Email notification Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            emailNotificationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return emailNotificationXml;
        }

        /// <summary>
        /// Represents emailnotification in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current emailnotification object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String emailNotificationXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            emailNotificationXml = this.ToXml();

            return emailNotificationXml;
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
                if (obj is EmailNotification)
                {
                    EmailNotification objectToBeCompared = obj as EmailNotification;

                    if (!this.Action.Equals(objectToBeCompared.Action))
                        return false;

                    if (!this.Emails.Equals(objectToBeCompared.Emails))
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
            hashCode = base.GetHashCode() ^ this.Action.GetHashCode() ^ this.Emails.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the emailnotification with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadEmailNotification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
             *  <EmailNotificiation Action="OnBegin" Emails="user@riversand.com,mdmtest@riversand.com" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EmailNotification")
                        {
                            #region Read EmailNotificiation

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ExportJobStatus action = ExportJobStatus.Unknown;
                                    Enum.TryParse<ExportJobStatus>(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }

                                if (reader.MoveToAttribute("Emails"))
                                {
                                    this.Emails = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');   
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
