using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.BusinessObjects.Exports;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for job schedule
    /// </summary>
    [DataContract]
    public class JobSchedule : MDMObject, IJobSchedule
    {
        #region Fields

        /// <summary>
        /// Field denoting
        /// </summary>
        private ScheduleCriteria _scheduleCriteria = new ScheduleCriteria();

        /// <summary>
        /// Field denoting
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        /// Field denoting
        /// </summary>
        private String _lastRunStatus = String.Empty;

        /// <summary>
        /// Field denoting
        /// </summary>
        private DateTime? _lastRunDate = null;

        /// <summary>
        /// Field denoting
        /// </summary>
        private DateTime? _nextRunDate = null;

        /// <summary>
        /// Field denoting
        /// </summary>
        private String _computerName = String.Empty;

        /// <summary>
        /// Indicates profiles to be executed for given schedule
        /// </summary>
        private BaseProfileCollection _profiles = new BaseProfileCollection();

        /// <summary>
        /// Indicates user name who created the profile
        /// </summary>
        private String _createUserName = String.Empty;

        /// <summary>
        /// Indicates when schedule was modified lastly.
        /// </summary>
        private DateTime? _lastModofiedDateTime = null;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public JobSchedule()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// <param name="id">Indicates the Identity of JobSchedule Instance</param>
        /// </summary>
        public JobSchedule(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Initialize subscriber from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Job schedule in xml format</param>
        public JobSchedule(String valuesAsXml)
        {
            LoadJobSchedule(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting
        /// </summary>
        [DataMember]
        public ScheduleCriteria ScheduleCriteria
        {
            get
            {
                return _scheduleCriteria;
            }
            set
            {
                _scheduleCriteria = value;
            }
        }

        /// <summary>
        /// Property denoting
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        /// <summary>
        /// Property denoting
        /// </summary>
        [DataMember]
        public String LastRunStatus
        {
            get
            {
                return _lastRunStatus;
            }
            set
            {
                _lastRunStatus = value;
            }
        }

        /// <summary>
        /// Property denoting
        /// </summary>
        [DataMember]
        public DateTime? LastRunDate
        {
            get
            {
                return _lastRunDate;
            }
            set
            {
                _lastRunDate = value;
            }
        }

        /// <summary>
        /// Property denoting
        /// </summary>
        [DataMember]
        public DateTime? NextRunDate
        {
            get
            {
                return _nextRunDate;
            }
            set
            {
                _nextRunDate = value;
            }
        }

        /// <summary>
        /// Property denoting
        /// </summary>
        [DataMember]
        public String ComputerName
        {
            get
            {
                return _computerName;
            }
            set
            {
                _computerName = value;
            }
        }

        /// <summary>
        /// Indicates profiles to be executed for given schedule
        /// </summary>
        [DataMember]
        public BaseProfileCollection Profiles
        {
            get { return _profiles; }
            set { _profiles = value; }
        }

        /// <summary>
        /// Indicates when schedule was modified lastly.
        /// </summary>
        [DataMember]
        public DateTime? LastModofiedDateTime
        {
            get { return _lastModofiedDateTime; }
            set { _lastModofiedDateTime = value; }
        }

        /// <summary>
        /// Indicates user name who created the profile
        /// </summary>
        [DataMember]
        public String CreateUserName
        {
            get { return _createUserName; }
            set { _createUserName = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents JobSchedule in Xml format
        /// </summary>
        public override String ToXml()
        {
            String jobScheduleXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("JobSchedule");

            #region Write Job schedule properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LastRunStatus", this.LastRunStatus);
            xmlWriter.WriteAttributeString("LastRunDate", this.LastRunDate == null ? String.Empty : this.LastRunDate.ToString());
            xmlWriter.WriteAttributeString("NextRunDate", this.NextRunDate == null ? String.Empty : this.NextRunDate.ToString());
            xmlWriter.WriteAttributeString("ComputerName", this.ComputerName);
            xmlWriter.WriteAttributeString("CreateUserName", this.CreateUserName);
            xmlWriter.WriteAttributeString("LastModofiedDateTime", this.LastModofiedDateTime == null ? String.Empty : this.LastModofiedDateTime.ToString());

            #endregion Write Job schedule properties

            if (this.ScheduleCriteria != null)
            {
                xmlWriter.WriteRaw(this.ScheduleCriteria.ToXml());
            }

            //Write profile information
            xmlWriter.WriteStartElement("Profiles");

            if (this.Profiles != null)
            {
                foreach (BaseProfile profile in this.Profiles)
                {
                    xmlWriter.WriteStartElement("Profile");

                    xmlWriter.WriteAttributeString("Id", profile.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", profile.Name);
                    xmlWriter.WriteAttributeString("LongName", profile.LongName);
                    xmlWriter.WriteAttributeString("ProfileTypeMain", profile.GetProfileType().ToString());
                    if (profile is ExportProfile)
                    {
                        xmlWriter.WriteAttributeString("ProfileType", ((ExportProfile)profile).ProfileType.ToString());
                    }

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            //job schedulenode end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            jobScheduleXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return jobScheduleXml;
        }

        /// <summary>
        /// Represents JobSchedule in Xml format
        /// </summary>
        public override String ToXml(ObjectSerialization serializationType)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Get schedule criteria for given job schedule
        /// </summary>
        /// <returns></returns>
        public IScheduleCriteria GetScheduleCriteria()
        {
            return this.ScheduleCriteria;
        }

        /// <summary>
        /// Get profiles for given schedule
        /// </summary>
        /// <returns>Collection of Base Profiles</returns>
        public BaseProfileCollection GetProfiles()
        {
            return this.Profiles;
        }

        /// <summary>
        /// Add profile 
        /// </summary>
        /// <param name="profile">Profile to add</param>
        public void AddProfile(IBaseProfile profile)
        {
            if (profile != null)
            {
                this.Profiles.Add((BaseProfile)profile);
            }
        }
             
        #endregion Methods

        #region Private Methods

        /// <summary>
        /// Load job schedule object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        private void LoadJobSchedule(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobSchedule" && reader.IsStartElement())
                        {
                            //Read job schedule metadata
                            #region Read job schedule Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = reader.ReadContentAsInt();

                                if (reader.MoveToAttribute("Name"))
                                    this.Name = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("LongName"))
                                    this.LongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Enabled"))
                                    this.Enabled = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("LastRunStatus"))
                                    this.LastRunStatus = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("NextRunDate"))
                                    this.NextRunDate = reader.ReadContentAsDateTime();

                                if (reader.MoveToAttribute("LastRunDate"))
                                    this.LastRunDate = reader.ReadContentAsDateTime();

                                if (reader.MoveToAttribute("ComputerName"))
                                    this.ComputerName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                    {
                                        this.Action = action;
                                    }
                                }

                                if (reader.MoveToAttribute("CreateUserName"))
                                    this.CreateUserName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("LastModofiedDateTime"))
                                    this.LastModofiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ScheduleCriteria")
                        {
                            String criteriaXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(criteriaXml))
                            {
                                this.ScheduleCriteria = new ScheduleCriteria(criteriaXml);
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Profiles")
                        {
                            String profileXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(profileXml))
                            {
                                this.Profiles = this.ReadProfiles(profileXml);
                            }
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

        /// <summary>
        /// Read base profiles from XML.
        /// </summary>
        /// <param name="profileXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///          <Profiles>
        ///             <Profile Id="74" Name="API Brand" LongName="" ProfileType="Riversand.ProductCenter.Integration.Syndication.CatalogContent.Profile" ProfileTypeMain="ProfileType.Export"/> 
        ///             <Profile Id="75" Name="API Web" LongName="" ProfileType="Riversand.ProductCenter.Integration.Syndication.CatalogContent.Profile" ProfileTypeMain="ProfileType.Export"/>
        ///             <Profile Id="76" Name="API Product" LongName="" ProfileType="Riversand.ProductCenter.Integration.Syndication.CatalogContent.Profile" ProfileTypeMain="ProfileType.Export"/>
        ///             <Profile Id="77" Name="Merge Profile" LongName="" ProfileTypeMain="ProfileType.Export"/>
        ///             <Profile Id="78" Name="Validation Profile" LongName="" ProfileTypeMain="ProfileType.Validation"/>
        ///         <Profiles>
        ///     ]]>    
        ///     </para>
        /// </example>
        private BaseProfileCollection ReadProfiles(String profileXml)
        {
            BaseProfileCollection profiles = null;

            if (!String.IsNullOrWhiteSpace(profileXml))
            {
                profiles = new BaseProfileCollection();

                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(profileXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Profile" && reader.IsStartElement())
                        {
                            BaseProfile profile = null;

                            //Read job schedule metadata
                            #region Read job schedule Attributes

                            if (reader.HasAttributes)
                            {
                                ProfileType profileType = ProfileType.Unknown;

                                if (reader.MoveToAttribute("ProfileTypeMain"))
                                    Enum.TryParse(reader.ReadContentAsString(), out profileType);

                                if (profileType != ProfileType.Unknown)
                                {
                                    profile = BaseProfile.CreateProfile(profileType);

                                    if (reader.MoveToAttribute("Id"))
                                        profile.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                    if (reader.MoveToAttribute("Name"))
                                        profile.Name = reader.ReadContentAsString();

                                    if (reader.MoveToAttribute("LongName"))
                                        profile.LongName = reader.ReadContentAsString();

                                    if (profile is ExportProfile && reader.MoveToAttribute("ProfileType"))
                                    {
                                        ExportProfileType exportProfileType = ExportProfileType.Unknown;
                                        Enum.TryParse(reader.ReadContentAsString(), out exportProfileType);
                                        ((ExportProfile)profile).ProfileType = exportProfileType;
                                    }

                                    profiles.Add(profile);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
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

            return profiles;
        }


        #endregion Private Methods
    }
}
