using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Data model export profile
    /// </summary>
    [DataContract]
    public class DataModelExportProfile : ExportProfile, IDataModelExportProfile
    {
        #region Fields

        /// <summary>
        /// Indicates data model export profile data object
        /// </summary>
        private DataModelExportProfileData _profileDataObject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataModelExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public DataModelExportProfile()
        { 
        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataModelExportProfile"/> class. 
        /// </summary>
        public DataModelExportProfile(ExportProfile exportProfile)
        {
            this.Id = exportProfile.Id;
            this.Name = exportProfile.Name;
            this.LongName = exportProfile.LongName;
            this.ProfileType = exportProfile.ProfileType;
            this.ProfileData = exportProfile.ProfileData;
            this.TemplateId = exportProfile.TemplateId;
            this.ApplicationConfigId = exportProfile.ApplicationConfigId;
            this.Action = exportProfile.Action;
            this.PermissionSet = exportProfile.PermissionSet;
            this.Locale = exportProfile.Locale;
            this.LegacyProfileId = exportProfile.LegacyProfileId;
            this.IsSystemProfile = exportProfile.IsSystemProfile;
            this.IsPublicProfile = exportProfile.IsPublicProfile;
            this.CreateUser = exportProfile.CreateUser;
            this._profileDataObject = new DataModelExportProfileData(exportProfile.ProfileData);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denoting entity export profile data as object
        /// </summary>
        [DataMember]
        public DataModelExportProfileData ProfileDataObject
        {
            get
            {
                return _profileDataObject;
            }
            set
            {
                _profileDataObject = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Converts data model export profile into xml
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("DataModelExportProfile");

            xmlWriter.WriteStartAttribute("Id");
            xmlWriter.WriteValue(Id);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteStartAttribute("ProfileType");
            xmlWriter.WriteValue(this.ProfileType.ToString());
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteStartAttribute("Name");
            xmlWriter.WriteValue(Name);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteStartAttribute("LongName");
            xmlWriter.WriteValue(LongName);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteStartAttribute("TemplateId");
            xmlWriter.WriteValue(TemplateId);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteStartAttribute("ApplicationConfigId");
            xmlWriter.WriteValue(ApplicationConfigId);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            xmlWriter.WriteRaw(_profileDataObject.ToXml());

            xmlWriter.WriteEndElement();

            string entityExportProfile = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityExportProfile;
        }

        /// <summary>
        /// Gets profile data object
        /// </summary>
        /// <returns>Returns data model export profile data object</returns>
        public IDataModelExportProfileData GetProfileData()
        {
            return this._profileDataObject;
        }

        #endregion Public Methods

        #region Private methods

        /// <summary>
        /// Loads data model export profile from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates data model export profile value as XML string</param>
        private void LoadDataModelExportProfileFromXml(String valuesAsXml)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelExportProfile")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }
                                if (reader.MoveToAttribute("TemplateId"))
                                {
                                    this.TemplateId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("ApplicationConfigId"))
                                {
                                    this.ApplicationConfigId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action;
                                    Enum.TryParse(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProfileData")
                        {
                            string profileData = reader.ReadOuterXml();
                            this._profileDataObject = new DataModelExportProfileData(profileData);
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
                        reader.Close();
                }
            }
        }

        #endregion Private methods

        #endregion Methods
    }
}