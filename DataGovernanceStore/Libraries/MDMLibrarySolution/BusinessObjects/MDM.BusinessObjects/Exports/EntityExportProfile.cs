using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Entity Export Profile class
    /// </summary>
    [DataContract]
    [KnownType(typeof(EntityExportSyndicationProfileData))]
    [KnownType(typeof(EntityExportUIProfileData))]
    [KnownType(typeof(ExportProfile))]
    [KnownType(typeof(EntityExportProfileData))]
    [Serializable]
    public class EntityExportProfile : ExportProfile, IEntityExportProfile, ICloneable
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private EntityExportProfileData _dataObject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public EntityExportProfile()
        { 
        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public EntityExportProfile(ExportProfileType exportProfileType)
        {
            this.ProfileType = exportProfileType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Create EntityExportProfile object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// The values As Xml.
        /// </param>
        /// <param name="exportProfileType"> Enum which specifies the exportProfileType </param>
        public EntityExportProfile(ExportProfileType exportProfileType, String valuesAsXml)
        {
            this.ProfileType = exportProfileType;
            LoadEntityExportProfileFromXml(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public EntityExportProfile(ExportProfile exportProfile)
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

            LoadProfileDataObject(exportProfile.ProfileData);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting entity export profile data as object
        /// </summary>
        [DataMember]
        public EntityExportProfileData DataObject
        {
            get
            {
                return _dataObject;
            }
            set
            {
                _dataObject = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Profile data methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetProfileDataObject<T>()
        {
            T returnObj = default(T);

            try
            {
                returnObj = (T)Convert.ChangeType(this._dataObject, typeof(T));
            }
            catch { }

            return returnObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileDataAsXml"></param>
        public void LoadProfileDataObject(String profileDataAsXml)
        {
            this.ProfileData = profileDataAsXml;

            switch (this.ProfileType)
            {
                case ExportProfileType.EntityExportUIProfile:
                case ExportProfileType.DynamicGovernanceExportUIProfile:
                    this._dataObject = new EntityExportUIProfileData(profileDataAsXml);
                    break;
                case ExportProfileType.EntityExportSyndicationProfile:
                    this._dataObject = new EntityExportSyndicationProfileData(profileDataAsXml);
                    break;
            }
        }

        #endregion

        #region Util methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Boolean Equals(object obj)
        {
            EntityExportProfile other = obj as EntityExportProfile;
            if (other == null)
                return false;

            if (!this.Id.Equals(other.Id))
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new Object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("EntityExportProfile");

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

            xmlWriter.WriteRaw(_dataObject.ToXml());

            xmlWriter.WriteEndElement();

            string entityExportProfile = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityExportProfile;
        }

        #endregion
        
        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityExportProfileFromXml(String valuesAsXml)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityExportProfile")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                    //this.Id = this.ProfileId;
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }
                                //if (reader.MoveToAttribute("ProfileType"))
                                //{
                                //    ExportProfileType profileType = ExportProfileType.Unknown;
                                //    Enum.TryParse(reader.ReadContentAsString(), out profileType);
                                //    this.ProfileType = profileType;
                                //}
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
                            LoadProfileDataObject(profileData);
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

        #endregion

        #endregion
    }
}
