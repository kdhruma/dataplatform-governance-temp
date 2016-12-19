using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Data model export profile data base object
    /// </summary>
    [DataContract]
    public class DataModelExportProfileData : MDMObject, IDataModelExportProfileData
    {
        #region Fields

        /// <summary>
        /// Field denoting organization ids for data model export profile
        /// </summary>
        private Collection<Int32> _organizationIds;

        /// <summary>
        /// Field denoting container ids for data model export profile
        /// </summary>
        private Collection<Int32> _containerIds;

        /// <summary>
        /// Field denoting locale ids for data model export profile
        /// </summary>
        private Collection<LocaleEnum> _localeIds;

        /// <summary>
        /// Field denoting sheet ids for data model export profile
        /// </summary>
        private Collection<DataModelSheet> _sheetsIds;

        /// <summary>
        /// Field denoting whether to exclude non translated data for data model export profile
        /// </summary>
        private Boolean _excludeNonTranslatedModelData = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataModelExportProfileData"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public DataModelExportProfileData()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public DataModelExportProfileData(String valuesAsXml)
        {
            LoadDataModelExportProfileData(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denoting the organization ids.
        /// </summary>
        [DataMember]
        public Collection<Int32> OrganizationIds
        {
            get 
            {
                return _organizationIds ?? (_organizationIds = new Collection<Int32>());
            }
            set 
            { 
                _organizationIds = value; 
            }
        }

        /// <summary>
        /// Property denoting the container ids.
        /// </summary>
        [DataMember]
        public Collection<Int32> ContainerIds
        {
            get 
            {
                return _containerIds ?? (_containerIds = new Collection<Int32>());
            }
            set 
            { 
                _containerIds = value; 
            }
        }

        /// <summary>
        /// Property denoting the locale id list.
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> LocaleIds
        {
            get 
            {
                return _localeIds ?? (_localeIds = new Collection<LocaleEnum>());
            }
            set 
            { 
                _localeIds = value; 
            }
        }

        /// <summary>
        /// Property denoting the sheet id list.
        /// </summary>
        [DataMember]
        public Collection<DataModelSheet> SheetIds 
        { 
            get 
            {
                return _sheetsIds ?? (_sheetsIds = new Collection<DataModelSheet>());
            }
            set 
            { 
                _sheetsIds = value; 
            } 
        }

        /// <summary>
        /// Property denoting whether to exclude non translated data or not in data model export profile
        /// </summary>
        [DataMember]
        public Boolean ExcludeNonTranslatedModelData
        {
            get
            {
                return _excludeNonTranslatedModelData;
            }
            set
            {
                _excludeNonTranslatedModelData = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// represents DataModelExportProfileData in XML format 
        /// </summary>
        /// <returns>The result string</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("ProfileData");

            xmlWriter.WriteStartElement("OrgIDs");
            if (_organizationIds != null && _organizationIds.Count > 0)
            {
               xmlWriter.WriteCData(ValueTypeHelper.JoinCollection(_organizationIds, ","));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ContainerIDs");
            if (_containerIds != null && _containerIds.Count > 0)
            {
                xmlWriter.WriteCData(ValueTypeHelper.JoinCollection(_containerIds, ","));

            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("LocaleIDs");
            if (_localeIds != null && _localeIds.Count > 0)
            {
                Collection<Int32> localeIdList = new Collection<Int32>();

                foreach (LocaleEnum locale in _localeIds)
                {
                    localeIdList.Add((Int32)locale);
                }
                xmlWriter.WriteCData(ValueTypeHelper.JoinCollection(localeIdList, ","));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SheetIDs");
            if (_sheetsIds != null && _sheetsIds.Count > 0)
            {
                Collection<Int32> sheetIdList = new Collection<Int32>();

                foreach (DataModelSheet dataModelSheet in _sheetsIds)
                {
                    sheetIdList.Add((Int32)dataModelSheet);
                }
                xmlWriter.WriteCData(ValueTypeHelper.JoinCollection(sheetIdList, ","));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ExcludeNonTranslatedModelData");
            xmlWriter.WriteCData(_excludeNonTranslatedModelData.ToString().ToLowerInvariant());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement(); // End ProfileData

            string profileData = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return profileData;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Populates DataModelExportProfileData object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// <para>
        /// Sample Xml:
        /// <ProfileData>
        ///     <OrgIDs><![CDATA[1,2,3,4,5]]></OrgIDs>
        ///     <ContainerIDs><![CDATA[6,7,8,9]]></ContainerIDs>
        ///     <LocaleIDs><![CDATA[10,11]]></LocaleIDs>
        ///     <SheetIDs><![CDATA[6,7,8,9]]></SheetIDs>
        /// </ProfileData>
        /// </para>
        /// </param>
        private void LoadDataModelExportProfileData(String valuesAsXml)
        {
            #region Sample XML
            /*
                <ProfileData>
                  <OrgIDs />
                  <ContainerIDs />
                  <LocaleIDs />
                  <SheetIDs />
                </ProfileData>
            */
            #endregion Sample XML

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "OrgIDs")
                        {
                            this._organizationIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadElementContentAsString(), ',');
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerIDs")
                        {
                            this._containerIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadElementContentAsString(), ',');
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleIDs")
                        {
                            this._localeIds = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadElementContentAsString(), ',');
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SheetIDs")
                        {
                            this._sheetsIds = ValueTypeHelper.SplitStringToEnumCollection<DataModelSheet>(reader.ReadElementContentAsString(), ',');
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExcludeNonTranslatedModelData")
                        {
                            this._excludeNonTranslatedModelData = reader.ReadElementContentAsBoolean();
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

        #endregion Methods
    }
}
