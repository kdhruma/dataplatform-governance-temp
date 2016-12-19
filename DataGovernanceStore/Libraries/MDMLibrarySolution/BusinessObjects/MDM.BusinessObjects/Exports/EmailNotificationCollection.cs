using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;

    /// <summary>
    /// Specifies the email notification collection object
    /// </summary>
    [DataContract]
    public class EmailNotificationCollection :InterfaceContractCollection<IEmailNotification,EmailNotification>, IEmailNotificationCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public EmailNotificationCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public EmailNotificationCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadEmailNotificationCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize email notification collection from IList
		/// </summary>
        /// <param name="emailNotificationList">IList of email notification collection</param>
        public EmailNotificationCollection(IList<EmailNotification> emailNotificationList)
		{
            this._items = new Collection<EmailNotification>(emailNotificationList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of email notification collection object
        /// </summary>
        /// <returns>Xml string representing the email notification collection</returns>
        public String ToXml()
        {
            String emailNotificationsXml = String.Empty;

            emailNotificationsXml = "<EmailNotifications>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (EmailNotification emailNotification in this._items)
                {
                    emailNotificationsXml = String.Concat(emailNotificationsXml, emailNotification.ToXml());
                }
            }

            emailNotificationsXml = String.Concat(emailNotificationsXml, "</EmailNotifications>");

            return emailNotificationsXml;
        }

        /// <summary>
        /// Get Xml representation of email notification collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the email notification collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String emailNotificationsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            emailNotificationsXml = this.ToXml();

            return emailNotificationsXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EmailNotification GetByExportStatus(ExportJobStatus exportJobStatus)
        {
            EmailNotification notification = null;

            foreach(EmailNotification emailNotification in this._items)
            {
                if(emailNotification.Action == exportJobStatus)
                {
                    notification = emailNotification;
                    break;
                }
            }

            return notification;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the email notification collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadEmailNotificationCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <EmailNotifications>
			        <EmailNotification Action="OnBegin" Emails="" />
			        <EmailNotification Action="OnComplete" Emails="" />
			        <EmailNotification Action="OnFailure" Emails="" />
			        <EmailNotification Action="OnSuccess" Emails="" />
		        </EmailNotifications>
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
                            #region Read EmailNotification Collection

                            String emailNotificationsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(emailNotificationsXml))
                            {
                                EmailNotification emailNotification = new EmailNotification(emailNotificationsXml, objectSerialization);
                                if (emailNotification != null)
                                {
                                    this.Add(emailNotification);
                                }
                            }

                            #endregion Read EmailNotification Collection
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
