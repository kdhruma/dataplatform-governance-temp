using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Globalization;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Interfaces.Exports;
    using MDM.Core;

    /// <summary>
    /// Specifies the translation export profile data object
    /// </summary>
    [DataContract]
    public class TranslationExportProfileData : MDMObject, ITranslationExportProfileData
    {
        #region Fields

        /// <summary>
        /// Field specifying collection of profile settings
        /// </summary>
        private ProfileSettingCollection _profileSettings = new ProfileSettingCollection();

        /// <summary>
        /// Field specifies notification
        /// </summary>
        private Notification _notification = new Notification();

        /// <summary>
        /// Field specifying ScopeSpecification
        /// </summary>
        private ScopeSpecification _scopeSpecification = new ScopeSpecification();

        /// <summary>
        /// Field specifying outputspecification
        /// </summary>
        private OutputSpecification _outputSpecification = new OutputSpecification();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies collection of profile settings
        /// </summary>
        [DataMember]
        public ProfileSettingCollection ProfileSettings
        {
            get
            {
                return _profileSettings;
            }
            set
            {
                _profileSettings = value;
            }
        }

        /// <summary>
        /// Property specifies notification
        /// </summary>
        [DataMember]
        public Notification Notifications
        {
            get
            {
                return _notification;
            }
            set
            {
                _notification = value;
            }
        }

        /// <summary>
        /// Property specifies scopespecification
        /// </summary>
        [DataMember]
        public ScopeSpecification ScopeSpecification
        {
            get
            {
                return _scopeSpecification;
            }
            set
            {
                _scopeSpecification = value;
            }
        }

        /// <summary>
        /// Property specifies outputSpecification
        /// </summary>
        [DataMember]
        public OutputSpecification OutputSpecification
        {
            get
            {
                return _outputSpecification;
            }
            set
            {
                _outputSpecification = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes syndication exportprofiledata object with default parameters
        /// </summary>
        public TranslationExportProfileData() : base() { }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public TranslationExportProfileData(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadTranslationExportProfileData(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents translation exportprofiledata in Xml format
        /// </summary>
        /// <returns>String representation of current translation exportprofiledata object</returns>
        public override String ToXml()
        {
            String translationExportProfileDataXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Auto }))
                {

                    // translation exportprofiledata Item node start
                    xmlWriter.WriteStartElement("TranslationExportProfileData");

                    #region write TranslationExportProfileData properties for full TranslationExportProfileData xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name.ToString());
                    #endregion  write TranslationExportProfileData properties for full TranslationExportProfileData xml

                    #region write notifications xml

                    if (this.Notifications != null)
                        xmlWriter.WriteRaw(this.Notifications.ToXml());

                    #endregion

                    #region write profile settings for Full profileSettings Xml

                    if (this.ProfileSettings != null)
                        xmlWriter.WriteRaw(this.ProfileSettings.ToXml());

                    #endregion write profile settings for Full profileSettings Xml

                    #region write scopespecification for Full scopespecification Xml

                    if (this.ScopeSpecification != null)
                        xmlWriter.WriteRaw(this.ScopeSpecification.ToXml());

                    #endregion write scopespecification for Full scopespecification Xml

                    #region write outputSpecification for Full outputSpecification Xml

                    if (this.OutputSpecification != null)
                        xmlWriter.WriteRaw(this.OutputSpecification.ToXml());

                    #endregion write outputSpecification for Full outputSpecification Xml


                    // translation exportprofiledata Item node end
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    //Get the actual XML
                    translationExportProfileDataXml = sw.ToString();

                }
            }

            return translationExportProfileDataXml;
        }

        /// <summary>
        /// Represents translation exportprofiledata in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current translation exportprofiledata object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String translationExportProfileDataXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Auto }))
                    {
                        // translation exportprofiledata Item node start
                        xmlWriter.WriteStartElement("TranslationExportProfileData");

                        #region write SyndicationExportProfileData properties for full SyndicationExportProfileData xml

                        xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                        xmlWriter.WriteAttributeString("Name", this.Name.ToString());

                        #endregion  write TranslationExportProfileData properties for full SyndicationExportProfileData xml

                        #region write profile settings for Full profileSettings Xml

                        if (this.ProfileSettings != null)
                            xmlWriter.WriteRaw(this.ProfileSettings.ToXml(objectSerialization));

                        #endregion write profile settings for Full profileSettings Xml

                        #region write notifications xml

                        if (this.Notifications != null)
                            xmlWriter.WriteRaw(this.Notifications.ToXml(objectSerialization));

                        #endregion

                        #region write scopespecification for Full scopespecification Xml

                        if (this.ScopeSpecification != null)
                            xmlWriter.WriteRaw(this.ScopeSpecification.ToXml(objectSerialization));

                        #endregion write scopespecification for Full scopespecification Xml

                        #region write outputSpecification for Full outputSpecification Xml

                        if (this.OutputSpecification != null)
                            xmlWriter.WriteRaw(this.OutputSpecification.ToXml(objectSerialization));

                        #endregion write outputSpecification for Full outputSpecification Xml

                        // translation exportprofiledata Item node end
                        xmlWriter.WriteEndElement();

                        xmlWriter.Flush();

                        //Get the actual XML
                        translationExportProfileDataXml = sw.ToString();

                    }
                }
            }
            else
            {
                translationExportProfileDataXml = this.ToXml();
            }

            return translationExportProfileDataXml;
        }

        /// <summary>
        /// Gets default translation export profile data object.
        /// </summary>
        /// <returns>Returns translation export profile data</returns>
        public static TranslationExportProfileData GetDefault()
        {
            TranslationExportProfileData profileData = new TranslationExportProfileData();
            profileData.ProfileSettings.Add(new ProfileSetting { Name = ExportProfileConstants.SUBSCRIBERQUEUELABEL, Value = String.Empty });

            profileData.Notifications.EmailNotifications.Add(new EmailNotification { Action = ExportJobStatus.Complete });
            profileData.Notifications.EmailNotifications.Add(new EmailNotification { Action = ExportJobStatus.Failure });

            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.BATCHSIZE, Value = String.Empty });

            return profileData;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the translation exportprofiledata with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadTranslationExportProfileData(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <TranslationExportProfileData Id="123" Name="Export Product Master">
	                <ProfileSettings>
		                <ProfileSetting Name="BatchSize" Value="10" />
		                <ProfileSetting Name="Localor" Value="" />
		                <ProfileSetting Name="LabelName" Value="" />
		                <ProfileSetting Name="ProfileGroup" Value="1" />
		                <ProfileSetting Name="UseInheritedValues" Value="1" />
	                </ProfileSettings>

	                <ScopeSpecification>
		                <ExportScopes>
			                <ExportScope ObjectType="Container" ObjectId="" ObjectUniqueIdentifier="Product Master" Include="" IsRecursive="false">
				                <SearchAttributeRules/>
			                </ExportScope>
		                </ExportScopes>
	                </ScopeSpecification>

	                <OutputSpecification>
		                <DataFormatters>
			                <DataFormatter Id="" Name="" Type="" AttributeColumnHeaderFormat="" ApplyExportMaskToLookupAttribute="" CategoryPathType=""></DataFormatter>
		                </DataFormatters>
		                <DataSubscribers>
			                <DataSubscriber Id="" Name="" Location="" FileName=""></DataSubscriber>
		                </DataSubscribers>
		                <OutputDataSpecification>
			                <MDMObjectGroups/>
		                </OutputDataSpecification>
	                </OutputSpecification>
                </TranslationExportProfileData>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlReader reader = null;


                using (StringReader stringReader = new StringReader(valuesAsXml))
                {
                    using (reader = XmlReader.Create(stringReader))
                    {
                        while (!reader.EOF)
                        {
                            #region Read translation export profile data

                            if (reader.NodeType == XmlNodeType.Element && String.Equals(reader.Name, "TranslationExportProfileData", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Read translation export profile data Properties

                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = ValueTypeHelper.ConvertToInt32(reader.ReadContentAsString());
                                    }

                                    if (reader.MoveToAttribute("Name"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }

                                }

                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && String.Equals(reader.Name, "Notification", StringComparison.OrdinalIgnoreCase))
                            {
                                this.Notifications = new Notification(reader.ReadInnerXml());
                            }
                            else if (reader.NodeType == XmlNodeType.Element && String.Equals(reader.Name, "ProfileSettings", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Read profile settings collection

                                String profileSettingsXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(profileSettingsXml))
                                {
                                    //Get collection of profilesettings and populate it in ProfileSetting collection of current syndicationExportProfileData object.
                                    ProfileSettingCollection profileSettingCollection = new ProfileSettingCollection(profileSettingsXml);
                                    if (profileSettingCollection != null)
                                    {
                                        foreach (ProfileSetting profileSetting in profileSettingCollection)
                                        {
                                            if (!this.ProfileSettings.Contains(profileSetting))
                                            {
                                                this.ProfileSettings.Add(profileSetting);
                                            }
                                        }
                                    }
                                }

                                #endregion Read profile settings collection
                            }
                            else if (reader.NodeType == XmlNodeType.Element && String.Equals(reader.Name, "ScopeSpecification", StringComparison.OrdinalIgnoreCase))
                            {
                                // Read ScopeSpecification
                                #region Read ScopeSpecification
                                String scopeSpecificationXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(scopeSpecificationXml))
                                {
                                    ScopeSpecification scopeSpecification = new ScopeSpecification(scopeSpecificationXml);
                                    if (scopeSpecification != null)
                                    {
                                        this.ScopeSpecification = scopeSpecification;
                                    }
                                }
                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && String.Equals(reader.Name, "OutputSpecification", StringComparison.OrdinalIgnoreCase))
                            {
                                // Read OutputSpecification
                                #region Read OutputSpecification
                                String outputSpecificationXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(outputSpecificationXml))
                                {
                                    OutputSpecification outputSpecification = new OutputSpecification(outputSpecificationXml);
                                    if (outputSpecification != null)
                                    {
                                        this.OutputSpecification = outputSpecification;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion Read translation export profile data
                        }
                    }

                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
