using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the notification object
    /// </summary>
    [DataContract]
    public class Notification : MDMObject, INotification
    {
        #region Fields

        /// <summary>
        /// Field specifying email notification collection
        /// </summary>
        private EmailNotificationCollection _emailNotifications = new EmailNotificationCollection();

        /// <summary>
        /// Field specifying executionsetting collection
        /// </summary>
        private NotificationSettingCollection _notificationSettings = new NotificationSettingCollection();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies email notification collection
        /// </summary>
        [DataMember]
        public EmailNotificationCollection EmailNotifications
        {
            get
            {
                return _emailNotifications;
            }
            set
            {
                _emailNotifications = value;
            }
        }

        /// <summary>
        /// Property specifies notification settings
        /// </summary>
        [DataMember]
        public NotificationSettingCollection NotificationSettings
        {
            get
            {
                return _notificationSettings;
            }
            set
            {
                _notificationSettings = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes notification object with default parameters
        /// </summary>
        public Notification() : base() { }

        /// <summary>
        /// Initializes notification object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public Notification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadNotification(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents notification in Xml format
        /// </summary>
        /// <returns>String representation of current notification object</returns>
        public override String ToXml()
        {
            String notificationXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //Notification Item node start
                    xmlWriter.WriteStartElement("Notification");

                    #region Write NotificationSettings

                    if (this.NotificationSettings != null)
                    {
                        xmlWriter.WriteRaw(this.NotificationSettings.ToXml());
                    }

                    #endregion

                    #region Write EmailNotifications

                    if (this.EmailNotifications != null)
                    {
                        xmlWriter.WriteRaw(this.EmailNotifications.ToXml());
                    }

                    #endregion

                    //Notification Item node end
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    //Get the actual XML
                    notificationXml = sw.ToString();
                }
            }

            return notificationXml;
        }

        /// <summary>
        /// Represents notification in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current notification object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String notificationXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            notificationXml = this.ToXml();

            return notificationXml;
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
                if (obj is Notification)
                {
                    Notification objectToBeCompared = obj as Notification;

                    if (!this.EmailNotifications.Equals(objectToBeCompared.EmailNotifications))
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
            hashCode = base.GetHashCode() ^ this.EmailNotifications.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the notification with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadNotification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <Notification>
		            <EmailNotifications>
			            <EmailNotification Action="OnBegin" Emails="" />
			            <EmailNotification Action="OnComplete" Emails="" />
			            <EmailNotification Action="OnFailure" Emails="" />
			            <EmailNotification Action="OnSuccess" Emails="" />
		            </EmailNotifications>
                   <NotificationSettings>
                        <NotificationSetting Name="SendOnlyIfItemCountIsMoreThanZero" Value="false"/>
                   </NotificationSettings>
                
	            </Notification>
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
                        #region Read Notification

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EmailNotifications")
                        {
                            // Read email notifications
                            #region Read email notifications
                            String emailNotificationsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(emailNotificationsXml))
                            {
                                EmailNotificationCollection emailNotificationCollection = new EmailNotificationCollection(emailNotificationsXml);
                                if (emailNotificationCollection.Any())
                                {
                                    this.EmailNotifications = emailNotificationCollection;
                                }
                            }
                            #endregion
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "NotificationSettings")
                        {
                            String notificationSettingsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(notificationSettingsXml))
                            {
                                NotificationSettingCollection notificationSettingCollection = new NotificationSettingCollection(notificationSettingsXml);
                                if (notificationSettingCollection.Any())
                                {
                                    this.NotificationSettings = notificationSettingCollection;
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read Notification
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
