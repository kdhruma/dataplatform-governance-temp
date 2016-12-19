using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the Lookup Export Profile object
    /// </summary>
    [DataContract]
    [KnownType(typeof(LookupExportProfileData))]
    [Serializable]
    public class LookupExportProfile : ExportProfile, ICloneable, ILookupExportProfile
    {
        #region Fields

        /// <summary>
        /// Field specifying Lookup export profile data
        /// </summary>
        private LookupExportProfileData _lookupProfileData;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public LookupExportProfile()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportProfile"/> class. 
        /// Constructor with Parameter as export profile Type
        /// <param name="exportProfileType" >Indicates the export profile Type </param>
        /// </summary>
        public LookupExportProfile(ExportProfileType exportProfileType)
        {
            this.ProfileType = exportProfileType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportProfile"/> class. 
        /// Create LookupExportProfile object from xml
        /// </summary>
        /// <param name="exportProfileType">Indicates the export profile Type</param>
        /// <param name="valuesAsXml">The values As Xml. </param>
        /// Xml having values which we want to populate in current object
        public LookupExportProfile(ExportProfileType exportProfileType, String valuesAsXml)
        {
            this.ProfileType = exportProfileType;
            LoadLookupExportProfileFromXml(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportProfile"/> class. 
        /// Constructor with parameter as ExportProfile
        /// </summary>
        public LookupExportProfile(ExportProfile exportProfile)
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
            this.IsSystemProfile = exportProfile.IsSystemProfile;

            LoadProfileDataObject(exportProfile.ProfileData);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Lookup export profile data
        /// </summary>
        [DataMember]
        public LookupExportProfileData LookupProfileData
        {
            get
            {
                return _lookupProfileData;
            }
            set
            {
                _lookupProfileData = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the Profile Data object based on the profile data.
        /// </summary>
        /// <param name="profileDataAsXml">Indicates the profile data as xml format</param>
        public void LoadProfileDataObject(String profileDataAsXml)
        {
            this.ProfileData = profileDataAsXml;

            switch (this.ProfileType)
            {
                case ExportProfileType.LookupExportSyndicationProfile:
                    this._lookupProfileData = new LookupExportSyndicationProfileData(profileDataAsXml);
                    break;
                case ExportProfileType.LookupExportUIProfile:
                    //Future usage...
                    break;
            }
        }

        /// <summary>
        /// Represents LookupExportProfile in Xml format 
        /// </summary>
        /// <returns>LookupExport profile object as string format</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("LookupExportProfile");

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

            xmlWriter.WriteStartElement("ProfileData");
            if (_lookupProfileData != null)
            {
                xmlWriter.WriteRaw(_lookupProfileData.ToXml());
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            String LookupExportProfile = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return LookupExportProfile;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            LookupExportProfile other = obj as LookupExportProfile;
            if (other == null)
                return false;

            if (!this.Id.Equals(other.Id))
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region Private methods

        private void LoadLookupExportProfileFromXml(String valuesAsXml)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupExportProfile")
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
    }
}
