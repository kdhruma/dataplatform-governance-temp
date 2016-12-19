using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MDM.Core;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the syndication lookup export profile data object
    /// </summary>
    [DataContract]
    public class LookupExportSyndicationProfileData : LookupExportProfileData, ILookupExportSyndicationProfileData
    {
        #region Fields

        /// <summary>
        /// Field specifying collection of profile settings
        /// </summary>
        private ProfileSettingCollection _profileSettings = new ProfileSettingCollection();

        /// <summary>
        /// Field specifying notification
        /// </summary>
        private Notification _notification = new Notification();

        /// <summary>
        /// Field specifying Lookup Export Scope
        /// </summary>
        private LookupExportScopeCollection _lookupExportScopes = new LookupExportScopeCollection();

        /// <summary>
        /// Field specifying outputspecification
        /// </summary>
        private OutputSpecification _outputSpecification = new OutputSpecification();

        /// <summary>
        /// Field specifying executionspecification
        /// </summary>
        private ExecutionSpecification _executionSpecification = new ExecutionSpecification();

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="LookupExportSyndicationProfileData"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public LookupExportSyndicationProfileData()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportSyndicationProfileData"/> class. 
        /// with value as Constructor
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public LookupExportSyndicationProfileData(String valuesAsXml)
        {
            LoadLookupExportSyndicationProfileData(valuesAsXml);
        }

        #endregion

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
        public Notification Notification
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

        /// <summary>
        /// Property specifies LookupExportScopes
        /// </summary>
        [DataMember]
        public LookupExportScopeCollection LookupExportScopes
        {
            get
            {
                return _lookupExportScopes;
            }
            set
            {
                _lookupExportScopes = value;
            }
        }

        /// <summary>
        /// Property specifies ExecutionSpecification
        /// </summary>
        [DataMember]
        public ExecutionSpecification ExecutionSpecification
        {
            get
            {
                return _executionSpecification;
            }
            set
            {
                _executionSpecification = value;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Represents lookup export syndication profiledata in Xml format
        /// </summary>
        /// <returns>String representation of current syndication exportprofiledata object</returns>
        public override String ToXml()
        {
            String syndicationExportProfileDataXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("LookupSyndicationExportProfileData");

            #region write SyndicationExportProfileData properties for full SyndicationExportProfileData xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name.ToString());

            #endregion  write SyndicationExportProfileData properties for full SyndicationExportProfileData xml

            #region write profile settings for Full profileSettings Xml

            if (this.ProfileSettings != null)
                xmlWriter.WriteRaw(this.ProfileSettings.ToXml());

            #endregion write profile settings for Full profileSettings Xml

            #region write notification for Full notification Xml

            if (this.Notification != null)
                xmlWriter.WriteRaw(this.Notification.ToXml());

            #endregion write notification for Full notification Xml

            #region write scope specification for Full scope specification Xml

            if (this.LookupExportScopes != null)
                xmlWriter.WriteRaw(this.LookupExportScopes.ToXml());

            #endregion write scopespecification for Full scopespecification Xml

            #region write outputSpecification for Full outputSpecification Xml

            if (this.OutputSpecification != null)
                xmlWriter.WriteRaw(this.OutputSpecification.ToXml());

            #endregion write outputSpecification for Full outputSpecification Xml

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            syndicationExportProfileDataXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return syndicationExportProfileDataXml;
        }

        /// <summary>
        /// Represents Lookup export syndication profiledata in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current Lookup export syndication profiledata object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String syndicationExportProfileDataXml = String.Empty;
            if (objectSerialization != ObjectSerialization.Full)
            {
                syndicationExportProfileDataXml = this.ToXml();
            }
            //Todo::Implement other serializations

            return syndicationExportProfileDataXml;
        }

        /// <summary>
        /// Set the execution specification for the current lookup export profile.
        /// </summary>
        /// <param name="executionSpecification">Indicates the execution specification object</param>
        public void SetExecutionSpecification(IExecutionSpecification executionSpecification)
        {
            if (executionSpecification != null)
            {
                this.ExecutionSpecification = (ExecutionSpecification)executionSpecification;
            }
        }

        /// <summary>
        /// Get the execution specification for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of execution specification object.</returns>
        public IExecutionSpecification GetExecutionSpecification()
        {
            return this.ExecutionSpecification;
        }

        /// <summary>
        /// Set the output specification for the current lookup export profile.
        /// </summary>
        /// <param name="outputSpecification">Indicates the output specification object</param>
        public void SetOutputSpecification(IOutputSpecification outputSpecification)
        {
            if (outputSpecification != null)
            {
                this.OutputSpecification = (OutputSpecification)outputSpecification;
            }
        }

        /// <summary>
        /// Get the output specification for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of output specification object.</returns>
        public IOutputSpecification GetOutputSpecification()
        {
            return this.OutputSpecification;
        }

        /// <summary>
        /// Set the notification details for the current lookup export profile.
        /// </summary>
        /// <param name="iNotification">Indicates the notification object</param>
        public void SetNotification(INotification iNotification)
        {
            if (iNotification != null)
            {
                this.Notification = (Notification)iNotification;
            }
        }

        /// <summary>
        /// Get the Notification details for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of Notification object.</returns>
        public INotification GetNotification()
        {
            return this.Notification;
        }

        /// <summary>
        /// Set the Profile details for the current lookup export profile.
        /// </summary>
        /// <param name="iProfileSettingCollection">Indicates the ProfileSettingCollection object</param>
        public void SetProfileSettings(IProfileSettingCollection iProfileSettingCollection)
        {
            if (iProfileSettingCollection != null)
            {
                this.ProfileSettings = (ProfileSettingCollection)iProfileSettingCollection;
            }
        }

        /// <summary>
        /// Get the Profile details for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of ProfileSettings object.</returns>
        public IProfileSettingCollection GetProfileSettings()
        {
            return this.ProfileSettings;
        }

        /// <summary>
        /// Set the lookup export scopes for the current lookup export profile.
        /// </summary>
        /// <param name="iLookupExportScopes">Indicates the LookupExportScopeCollection object</param>
        public void SetLookupExportScopes(ILookupExportScopeCollection iLookupExportScopes)
        {
            if (iLookupExportScopes != null)
            {
                this.LookupExportScopes = (LookupExportScopeCollection)iLookupExportScopes;
            }
        }

        /// <summary>
        /// Get the lookup export scopes for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of LookupExportScopeCollection object.</returns>
        public ILookupExportScopeCollection GetLookupExportScopes()
        {
            return this.LookupExportScopes;
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// Loads the lookup export syndication profiledata with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadLookupExportSyndicationProfileData(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read syndication export profile data

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupSyndicationExportProfileData")
                        {
                            #region Read syndication export profile data Properties

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
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupExportScopes")
                        {
                            #region Read LookupExportScopes collection

                            String lookupExportScopeXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(lookupExportScopeXml))
                            {
                                this.LookupExportScopes = new LookupExportScopeCollection(lookupExportScopeXml);
                            }

                            #endregion Read LookupExportScopes
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProfileSettings")
                        {
                            #region Read profile settings collection

                            String profileSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(profileSettingsXml))
                            {
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
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Notification")
                        {
                            // Read Notification
                            #region Read Notification
                            String notificationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(notificationXml))
                            {
                                Notification notification = new Notification(notificationXml);
                                if (notification != null)
                                {
                                    this.Notification = notification;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "OutputSpecification")
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

                        #endregion Read syndication export profile data
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

        #endregion

    }
}
