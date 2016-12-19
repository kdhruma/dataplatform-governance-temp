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
    /// Translation Export Profile class
    /// </summary>
    [DataContract]
    [Serializable]
    public class TranslationExportProfile : ExportProfile, ITranslationExportProfile, ICloneable
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private TranslationExportProfileData _dataObject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public TranslationExportProfile()
        {

        }
         /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public TranslationExportProfile(ExportProfile exportProfile)
        {
            this.Id = exportProfile.Id;
            this.Name = exportProfile.Name;
            this.LongName = exportProfile.LongName;
            this.ProfileType = exportProfile.ProfileType;
            this.ProfileData = exportProfile.ProfileData;
            this.ApplicationConfigId = exportProfile.ApplicationConfigId;
            this.Action = exportProfile.Action;
            this.IsSystemProfile = exportProfile.IsSystemProfile;
            LoadProfileDataObject(exportProfile.ProfileData);
        }


        #endregion

        #region Properties

        /// <summary>
        /// Property denoting entity export profile data as object
        /// </summary>
        [DataMember]
        public TranslationExportProfileData DataObject
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
        /// Sets TranslationExportProfile object from input xml string
        /// </summary>
        /// <param name="profileDataAsXml">TranslationExportProfile object in Xml representation.</param>
        public void LoadProfileDataObject(String profileDataAsXml)
        {
            this.ProfileData = profileDataAsXml;
            this._dataObject = new TranslationExportProfileData(profileDataAsXml);
        }

        #endregion

        #region Util methods



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

            xmlWriter.WriteStartElement("TranslationExportProfile");

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
