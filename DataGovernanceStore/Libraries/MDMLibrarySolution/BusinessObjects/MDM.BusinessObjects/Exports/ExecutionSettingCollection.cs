using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the executionsetting collection object
    /// </summary>
    [DataContract]
    public class ExecutionSettingCollection : InterfaceContractCollection<IExecutionSetting, ExecutionSetting>, IExecutionSettingCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public ExecutionSettingCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public ExecutionSettingCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadExecutionSettingCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize execution setting collection from IList
		/// </summary>
        /// <param name="executionSettingsList">IList of executionsettingcollection</param>
        public ExecutionSettingCollection(IList<ExecutionSetting> executionSettingsList)
		{
            this._items = new Collection<ExecutionSetting>(executionSettingsList);
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
        public ExecutionSetting GetSetting(String settingName)
        {
            ExecutionSetting settingToReturn = null;

            foreach (ExecutionSetting setting in this._items)
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
        /// Represents method for getting ExecutionSettings strongly typed and formatted value
        /// </summary>
        /// <typeparam name="T">Represents resulting type of the value</typeparam>
        /// <param name="settingName">Represents name of the setting that used as a collection key</param>
        /// <param name="defaultValue">Represents default value for resulting value in case no value was found or it couldn't be converted to T</param>
        /// <returns>Returns strongly typed and formatted value from ExecutionSettingCollection selected by settingName</returns>
        /// <returns></returns>
        public T GetValue<T>(String settingName, T defaultValue)
        {
            return GetValue(settingName, defaultValue, null);
        }

        /// <summary>
        /// Represents method for getting ExecutionSettings strongly typed and formatted value
        /// </summary>
        /// <typeparam name="T">Represents resulting type of the value</typeparam>
        /// <param name="settingName">Represents name of the setting that used as a collection key</param>
        /// <param name="defaultValue">Represents default value for resulting value in case no value was found or it couldn't be converted to T</param>
        /// <param name="formatProvider">Represents formatter for ChangeType conversion</param>
        /// <returns>Returns strongly typed and formatted value from ExecutionSettingCollection selected by settingName</returns>
        public T GetValue<T>(String settingName, T defaultValue, IFormatProvider formatProvider)
        {
            T settingValue = defaultValue;

            foreach (ExecutionSetting executionSetting in this._items)
            {
                if (executionSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = executionSetting.Value;

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

            foreach (ExecutionSetting executionSetting in this._items)
            {
                if (executionSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = executionSetting.Value;

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
            String executionSettingsXml = String.Empty;

            executionSettingsXml = "<ExecutionSettings>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExecutionSetting executionSetting in this._items)
                {
                    executionSettingsXml = String.Concat(executionSettingsXml, executionSetting.ToXml());
                }
            }

            executionSettingsXml = String.Concat(executionSettingsXml, "</ExecutionSettings>");

            return executionSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of execution setting collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the execution setting collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String executionSettingsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            executionSettingsXml = this.ToXml();

            return executionSettingsXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the execution setting collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExecutionSettingCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <ExecutionSettings>
			        <ExecutionSetting Name="ExecutionType" Value="" /> <!-- Possible values are "Full" or "Delta". Legacy property name is "Type" -->
			        <ExecutionSetting Name="FirstTimeAsFull" Value="" />
			        <ExecutionSetting Name="FromTime" Value="" />
			        <ExecutionSetting Name="Label" Value="" />
			        <ExecutionSetting Name="StartWithAllCommonAttributes" Value="" />
			        <ExecutionSetting Name="StartWithAllCategoryAttributes" Value="" />
			        <ExecutionSetting Name="StartWithAllSystemAttributes" Value="" />
			        <ExecutionSetting Name="StartWithAllWorkflowAttributes" Value="" />
		        </ExecutionSettings>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionSetting")
                        {
                            #region Read ExecutionSettings Collection

                            String executionSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(executionSettingsXml))
                            {
                                ExecutionSetting executionSetting = new ExecutionSetting(executionSettingsXml, objectSerialization);
                                if (executionSetting != null)
                                {
                                    this.Add(executionSetting);
                                }
                            }

                            #endregion Read ExecutionSetting Collection
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
