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
    /// Specifies the exportsetting collection object
    /// </summary>
    [DataContract]
    [KnownType(typeof(ExportSetting))]
    public class ExportSettingCollection : InterfaceContractCollection<IExportSetting, ExportSetting>, IExportSettingCollection
    {
        #region Fields

        [DataMember]
        private String _exportSettingName = "ExportSetting";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ExportSettingCollection(String exportSettingName)
        {
            _exportSettingName = exportSettingName;
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="exportSettingName">export setting name</param>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public ExportSettingCollection(String exportSettingName, String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
            : this(exportSettingName)
        {
            LoadExportSettingCollection(valueAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize export setting collection from IList
        /// </summary>
        /// <param name="exportSettingsList">IList of exportsettingcollection</param>
        public ExportSettingCollection(IList<ExportSetting> exportSettingsList)
        {
            this._items = new Collection<ExportSetting>(exportSettingsList);
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
        public ExportSetting GetSetting(String settingName)
        {
            ExportSetting settingToReturn = null;

            foreach (ExportSetting setting in this._items)
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
        /// Represents method for getting ExportSettings strongly typed and formatted value
        /// </summary>
        /// <typeparam name="T">Represents resulting type of the value</typeparam>
        /// <param name="settingName">Represents name of the setting that used as a collection key</param>
        /// <param name="defaultValue">Represents default value for resulting value in case no value was found or it couldn't be converted to T</param>
        /// <returns>Returns strongly typed and formatted value from ExportSettingCollection selected by settingName</returns>
        /// <returns></returns>
        public T GetValue<T>(String settingName, T defaultValue)
        {
            return GetValue(settingName, defaultValue, null);
        }

        /// <summary>
        /// Represents method for getting ExportSettings strongly typed and formatted value
        /// </summary>
        /// <typeparam name="T">Represents resulting type of the value</typeparam>
        /// <param name="settingName">Represents name of the setting that used as a collection key</param>
        /// <param name="defaultValue">Represents default value for resulting value in case no value was found or it couldn't be converted to T</param>
        /// <param name="formatProvider">Represents formatter for ChangeType conversion</param>
        /// <returns>Returns strongly typed and formatted value from ExportSettingCollection selected by settingName</returns>
        public T GetValue<T>(String settingName, T defaultValue, IFormatProvider formatProvider)
        {
            T settingValue = defaultValue;

            foreach (ExportSetting exportSetting in this._items)
            {
                if (exportSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = exportSetting.Value;

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

            foreach (ExportSetting exportSetting in this._items)
            {
                if (exportSetting.Name.Equals(settingName))
                {
                    String settingValueAsString = exportSetting.Value;

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
        /// Get Xml representation of export setting collection object
        /// </summary>
        /// <returns>Xml string representing the export setting collection</returns>
        public String ToXml()
        {
            String exportSettingsXml = String.Empty;

            exportSettingsXml = String.Format("<{0}s>", _exportSettingName);

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExportSetting exportSetting in this._items)
                {
                    exportSettingsXml = String.Concat(exportSettingsXml, exportSetting.ToXml());
                }
            }

            exportSettingsXml = String.Concat(exportSettingsXml, String.Format("</{0}s>", _exportSettingName));

            return exportSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of export setting collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the export setting collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String exportSettingsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            exportSettingsXml = this.ToXml();

            return exportSettingsXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the export setting collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExportSettingCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <ExportSettings>
			        <ExportSetting Name="ExecutionType" Value="" /> <!-- Possible values are "Full" or "Delta". Legacy property name is "Type" -->
			        <ExportSetting Name="FirstTimeAsFull" Value="" />
			        <ExportSetting Name="FromTime" Value="" />
			        <ExportSetting Name="Label" Value="" />
			        <ExportSetting Name="StartWithAllCommonAttributes" Value="" />
			        <ExportSetting Name="StartWithAllCategoryAttributes" Value="" />
			        <ExportSetting Name="StartWithAllSystemAttributes" Value="" />
			        <ExportSetting Name="StartWithAllWorkflowAttributes" Value="" />
		        </ExportSettings>
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
                            #region Read ExportSettings Collection

                            String exportSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(exportSettingsXml))
                            {
                                ExportSetting exportSetting = new ExportSetting(_exportSettingName, exportSettingsXml, objectSerialization);
                                if (exportSetting != null)
                                {
                                    this.Add(exportSetting);
                                }
                            }

                            #endregion Read ExportSetting Collection
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
