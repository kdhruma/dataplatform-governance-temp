using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the Notificationsetting collection object
    /// </summary>
    [DataContract]
    public class NotificationSettingCollection : InterfaceContractCollection<INotificationSetting, NotificationSetting>, INotificationSettingCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public NotificationSettingCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public NotificationSettingCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadNotificationSettingCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize Notification setting collection from IList
		/// </summary>
        /// <param name="NotificationSettingsList">IList of Notificationsettingcollection</param>
        public NotificationSettingCollection(IList<NotificationSetting> NotificationSettingsList)
		{
            this._items = new Collection<NotificationSetting>(NotificationSettingsList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public NotificationSetting GetSetting(String settingName)
        {
            NotificationSetting settingToReturn = null;

            foreach (NotificationSetting setting in this._items)
            {
                if (setting.Name.Equals(settingName))
                {
                    settingToReturn = setting;
                    break;
                }
            }

            return settingToReturn;
        }

        /// <summary>
        /// Represents method for getting NotificationSettings strongly typed and formatted value
        /// </summary>
        /// <typeparam name="T">Represents resulting type of the value</typeparam>
        /// <param name="settingName">Represents name of the setting that used as a collection key</param>
        /// <param name="defaultValue">Represents default value for resulting value in case no value was found or it couldn't be converted to T</param>
        /// <returns>Returns strongly typed and formatted value from NotificationSettingCollection selected by settingName</returns>
        /// <returns></returns>
        public T GetValue<T>(String settingName, T defaultValue)
        {
            return GetValue(settingName, defaultValue, null);
        }

        /// <summary>
        /// Represents method for getting NotificationSettings strongly typed and formatted value
        /// </summary>
        /// <typeparam name="T">Represents resulting type of the value</typeparam>
        /// <param name="settingName">Represents name of the setting that used as a collection key</param>
        /// <param name="defaultValue">Represents default value for resulting value in case no value was found or it couldn't be converted to T</param>
        /// <param name="formatProvider">Represents formatter for ChangeType conversion</param>
        /// <returns>Returns strongly typed and formatted value from NotificationSettingCollection selected by settingName</returns>
        public T GetValue<T>(String settingName, T defaultValue, IFormatProvider formatProvider)
        {
            T settingValue = defaultValue;

            foreach (NotificationSetting NotificationSetting in this._items)
            {
                if (NotificationSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = NotificationSetting.Value;

                    try
                    {
                        // If T is of type string, allow empty values to be returned. 
                        if (!String.IsNullOrWhiteSpace(settingValueAsString) || typeof(T) == typeof(String))
                        {
                            settingValue = (T)Convert.ChangeType(settingValueAsString, typeof(T), formatProvider);
                        }
                    }
                    catch { }

                    break;
                }
            }

            return settingValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetValueAsEnum<T>(String settingName, T defaultValue) where T : struct
        {
            T settingValue = defaultValue;

            foreach (NotificationSetting NotificationSetting in this._items)
            {
                if (NotificationSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = NotificationSetting.Value;

                    try
                    {
                        // If T is of type string, allow empty values to be returned. 
                        if (!String.IsNullOrWhiteSpace(settingValueAsString))
                        {
                            Enum.TryParse<T>(settingValueAsString, true, out settingValue);
                        }
                    }
                    catch { }

                    break;
                }
            }

            return settingValue;
        }

        /// <summary>
        /// Get Xml representation of Notification setting collection object
        /// </summary>
        /// <returns>Xml string representing the Notification setting collection</returns>
        public String ToXml()
        {
            String NotificationSettingsXml = String.Empty;

            NotificationSettingsXml = "<NotificationSettings>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (NotificationSetting NotificationSetting in this._items)
                {
                    NotificationSettingsXml = String.Concat(NotificationSettingsXml, NotificationSetting.ToXml());
                }
            }

            NotificationSettingsXml = String.Concat(NotificationSettingsXml, "</NotificationSettings>");

            return NotificationSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of Notification setting collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the Notification setting collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String NotificationSettingsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            NotificationSettingsXml = this.ToXml();

            return NotificationSettingsXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the Notification setting collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadNotificationSettingCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <NotificationSettings>
			        <NotificationSetting Name="SendOnlyIfItemCountIsMoreThanZero" Value="false" />
			        
		        </NotificationSettings>
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
                            #region Read NotificationSettings Collection

                            String NotificationSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(NotificationSettingsXml))
                            {
                                NotificationSetting NotificationSetting = new NotificationSetting(NotificationSettingsXml, objectSerialization);
                                if (NotificationSetting != null)
                                {
                                    this.Add(NotificationSetting);
                                }
                            }

                            #endregion Read NotificationSetting Collection
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
