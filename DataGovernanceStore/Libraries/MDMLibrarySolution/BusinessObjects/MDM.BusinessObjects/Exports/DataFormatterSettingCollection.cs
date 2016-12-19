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
    /// Specifies the executionsetting collection object
    /// </summary>
    [DataContract]
    public class DataFormatterSettingCollection : InterfaceContractCollection<IDataFormatterSetting, DataFormatterSetting>, IDataFormatterSettingCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public DataFormatterSettingCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public DataFormatterSettingCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadDataFormatterSettingCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize execution setting collection from IList
		/// </summary>
        /// <param name="dataFormatterSettingsList">IList of executionsettingcollection</param>
        public DataFormatterSettingCollection(IList<DataFormatterSetting> dataFormatterSettingsList)
		{
            this._items = new Collection<DataFormatterSetting>(dataFormatterSettingsList);
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
        public DataFormatterSetting GetSetting(String settingName)
        {
            DataFormatterSetting settingToReturn = null;

            foreach (DataFormatterSetting setting in this._items)
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
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetValue<T>(String settingName, T defaultValue)
        {
            T settingValue = defaultValue;

            foreach (DataFormatterSetting dataFormatterSetting in this._items)
            {
                if (dataFormatterSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = dataFormatterSetting.Value;

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

            foreach (DataFormatterSetting dataFormatterSetting in this._items)
            {
                if (dataFormatterSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = dataFormatterSetting.Value;

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
        /// Get Xml representation of execution setting collection object
        /// </summary>
        /// <returns>Xml string representing the execution setting collection</returns>
        public String ToXml()
        {
            String dataFormatterSettingsXml = String.Empty;

            dataFormatterSettingsXml = "<DataFormatterSettings>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DataFormatterSetting dataFormatterSetting in this._items)
                {
                    dataFormatterSettingsXml = String.Concat(dataFormatterSettingsXml, dataFormatterSetting.ToXml());
                }
            }

            dataFormatterSettingsXml = String.Concat(dataFormatterSettingsXml, "</DataFormatterSettings>");

            return dataFormatterSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of execution setting collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the execution setting collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String dataFormatterSettingsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            dataFormatterSettingsXml = this.ToXml();

            return dataFormatterSettingsXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the execution setting collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadDataFormatterSettingCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <DataFormatterSettings>
			        <DataFormatterSetting Name="ExecutionType" Value="" /> <!-- Possible values are "Full" or "Delta". Legacy property name is "Type" -->
			        <DataFormatterSetting Name="FirstTimeAsFull" Value="" />
			        <DataFormatterSetting Name="FromTime" Value="" />
			        <DataFormatterSetting Name="Label" Value="" />
			        <DataFormatterSetting Name="StartWithAllCommonAttributes" Value="" />
			        <DataFormatterSetting Name="StartWithAllCategoryAttributes" Value="" />
			        <DataFormatterSetting Name="StartWithAllSystemAttributes" Value="" />
			        <DataFormatterSetting Name="StartWithAllWorkflowAttributes" Value="" />
		        </DataFormatterSettings>
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
                            #region Read DataFormatterSettings Collection

                            String dataFormatterSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(dataFormatterSettingsXml))
                            {
                                DataFormatterSetting dataFormatterSetting = new DataFormatterSetting(dataFormatterSettingsXml, objectSerialization);
                                if (dataFormatterSetting != null)
                                {
                                    this.Add(dataFormatterSetting);
                                }
                            }

                            #endregion Read DataFormatterSetting Collection
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
