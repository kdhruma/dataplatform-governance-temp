using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Core;
using MDM.Interfaces.Exports;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.Exports
{

    /// <summary>
    ///  Export Profile class
    /// </summary>
    [DataContract]
    [KnownType(typeof(EntityExportProfile))]
    [KnownType(typeof(LookupExportProfile))]
    [KnownType(typeof(TranslationExportProfile))]
    public class ExportProfile : BaseProfile, IExportProfile, ICloneable
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private ExportProfileType _profileType;

        /// <summary>
        /// 
        /// </summary>
        private String _profileData;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _templateId;

        /// <summary>
        /// 
        /// </summary>
        private Int64 _applicationConfigId;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isSystemProfile;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _legacyProfileId;

        /// <summary>
        /// The create user
        /// </summary>
        private String _createUser;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public ExportProfile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Create EntityExportProfile object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// The values As Xml.
        /// </param>
        /// Xml having values which we want to populate in current object
        public ExportProfile(String valuesAsXml)
        {
            LoadExportProfileFromXml(valuesAsXml);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Property denoting profile type
        /// </summary>
        [DataMember]
        public new ExportProfileType ProfileType
        {
            get
            {
                return _profileType;
            }
            set
            {
                _profileType = value;
            }
        }

        /// <summary>
        /// Property denoting Profile Data of the profile
        /// </summary>
        [DataMember]
        public String ProfileData
        {
            get
            {
                return _profileData;
            }
            set
            {
                _profileData = value;
            } 
        }

        /// <summary>
        /// Property denoting Template Id for the profile
        /// </summary>
        [DataMember]
        public Int32 TemplateId
        {
            get
            {
                return _templateId;
            }
            set
            {
                _templateId = value;
            }
        }

        /// <summary>
        /// Property denoting Application Config Id for the profile
        /// </summary>
        [DataMember]
        public Int64 ApplicationConfigId
        {
            get
            {
                return _applicationConfigId;
            }
            set
            {
                _applicationConfigId = value;
            }
        }

        /// <summary>
        /// Property denoting Is System Profile for the profile
        /// </summary>
        [DataMember]
        public Boolean IsSystemProfile
        {
            get
            {
                return _isSystemProfile;
            }
            set
            {
                _isSystemProfile = value;
            }
        }

        /// <summary>
        /// Property denoting that profile is accessible for all Users
        /// </summary>
        [DataMember]
        public Boolean IsPublicProfile { get; set; }

        /// <summary>
        /// Property denoting legacy nhibernate based profile id
        /// </summary>
        [DataMember]
        public Int32 LegacyProfileId
        {
            get
            {
                return _legacyProfileId;
            }
            set
            {
                _legacyProfileId = value;
            }
        }

        /// <summary>
        /// Property denoting the create user
        /// </summary>
        [DataMember]
        public String CreateUser
        {
            get
            {
                return _createUser;
            }
            set
            {
                _createUser = value;
            }
        }

        #endregion

        #region Methods

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Boolean Equals(object obj)
        {
            ExportProfile other = obj as ExportProfile;
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
        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Represents syndication ExportProfile in Xml format
        /// </summary>
        /// <returns>String representation of current exportProfile object</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("ExportProfile");

            xmlWriter.WriteStartAttribute("Id");
            xmlWriter.WriteValue(Id);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteStartAttribute("Name");
            xmlWriter.WriteValue(Name);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteStartAttribute("ProfileType");
            xmlWriter.WriteValue(this.ProfileType.ToString());
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

            xmlWriter.WriteStartAttribute("CreateUser");
            xmlWriter.WriteValue(CreateUser);
            xmlWriter.WriteEndAttribute();

            xmlWriter.WriteRaw(ProfileData);

            xmlWriter.WriteEndElement();

            string entityExportProfile = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityExportProfile;
        }

        #endregion

        #region Private methods

        private void LoadExportProfileFromXml(String valuesAsXml)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportProfile")
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
                                if (reader.MoveToAttribute("ProfileType"))
                                {
                                    ExportProfileType profileType = ExportProfileType.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out profileType);
                                    this.ProfileType = profileType;
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
                                if (reader.MoveToAttribute("CreateUser"))
                                {
                                    this.CreateUser = reader.ReadContentAsString();
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
                            this.ProfileData = reader.ReadOuterXml();
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