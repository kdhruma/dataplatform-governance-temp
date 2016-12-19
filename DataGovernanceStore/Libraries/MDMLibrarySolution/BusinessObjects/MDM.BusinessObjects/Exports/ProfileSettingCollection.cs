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
    /// Specifies the profile setting collection object
    /// </summary>
    [DataContract]
    public class ProfileSettingCollection : InterfaceContractCollection<IProfileSetting, ProfileSetting>, IProfileSettingCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public ProfileSettingCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public ProfileSettingCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadProfileSettingCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize profile setting collection from IList
		/// </summary>
        /// <param name="profileSettingsList">IList of profilesettingcollection</param>
        public ProfileSettingCollection(IList<ProfileSetting> profileSettingsList)
		{
            this._items = new Collection<ProfileSetting>(profileSettingsList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of profile setting collection object
        /// </summary>
        /// <returns>Xml string representing the profile setting collection</returns>
        public String ToXml()
        {
            String profileSettingsXml = String.Empty;

            profileSettingsXml = "<ProfileSettings>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ProfileSetting profileSetting in this._items)
                {
                    profileSettingsXml = String.Concat(profileSettingsXml, profileSetting.ToXml());
                }
            }

            profileSettingsXml = String.Concat(profileSettingsXml, "</ProfileSettings>");

            return profileSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of profile setting collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the profile setting collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String profileSettingsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            profileSettingsXml = this.ToXml();

            return profileSettingsXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetValue<T>(String settingName, T defaultValue)
        {
            T settingValue = defaultValue;

            foreach (ProfileSetting setting in this._items)
            {
                if (setting.Name.Equals(settingName))
                {
                    String settingValueAsString = setting.Value;

                    try
                    {
                        // If T is of type string, allow empty values to be returned. 
                        if (!String.IsNullOrWhiteSpace(settingValueAsString) || typeof(T) == typeof(String))
                        {
                            settingValue = (T)Convert.ChangeType(settingValueAsString, typeof(T));
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

            foreach (ProfileSetting setting in this._items)
            {
                if (setting.Name.Equals(settingName))
                {
                    String settingValueAsString = setting.Value;

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
        /// 
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public ProfileSetting GetSetting(String settingName)
        {
            ProfileSetting settingToReturn = null;

            foreach (ProfileSetting setting in this._items)
            {
                if (setting.Name.Equals(settingName))
                {
                    settingToReturn = setting;
                    break;
                }
            }

            return settingToReturn;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the profile setting collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadProfileSettingCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
             * <ProfileSettings>
                 * <ProfileSetting Name="Description" Value="Export  RS XML" /> 
		         * <ProfileSetting Name="PromotedCopy" Value="false" />
             * </ProfileSettings>
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
                            #region Read ProfileSetting Collection

                            String profileSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(profileSettingsXml))
                            {
                                ProfileSetting profileSetting = new ProfileSetting(profileSettingsXml, objectSerialization);
                                if (profileSetting != null)
                                {
                                    this.Add(profileSetting);
                                }
                            }

                            #endregion Read ProfileSettings Collection
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
