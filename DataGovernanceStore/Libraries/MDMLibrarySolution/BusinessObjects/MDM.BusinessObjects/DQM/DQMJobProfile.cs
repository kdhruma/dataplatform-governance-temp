using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DQM Job Profile settings
    /// </summary>
    [DataContract]
    public class DQMJobProfile : BaseProfile, IDQMJobProfile, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field for Job Source Scope Filter
        /// </summary>
        private JobSourceScopeFilter _sourceScopeFilter = new JobSourceScopeFilter();

        /// <summary>
        /// Field for job profile type
        /// </summary>
        private DQMJobType _jobType = DQMJobType.Unknown;

        /// <summary>
        /// Field for weightage
        /// </summary>
        private Int32 _weightage;

        /// <summary>
        /// Field for enabling status
        /// </summary>
        private Boolean _enabled;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Job Source Scope Filter
        /// </summary>
        [DataMember]
        public JobSourceScopeFilter SourceScopeFilter
        {
            get { return _sourceScopeFilter; }
            set { _sourceScopeFilter = value; }
        }

        /// <summary>
        /// Property denoting profile type
        /// </summary>
        [DataMember]
        public DQMJobType JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }

        /// <summary>
        /// This property will be used by the parallel processor to schedule any match job that uses this profile.
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get { return _weightage; }
            set { _weightage = value; }
        }

        /// <summary>
        /// Profile is enabled or disabled
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Helper property required for xml conversions
        /// </summary>
        [IgnoreDataMember]
        public String JobProfileType { get { return JobType + "Profile"; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DQMJobProfile()
        {
            JobType = DQMJobType.Unknown;
        }

        /// <summary>
        /// Constructs DQMJobProfile for given profile type
        /// </summary>
        public DQMJobProfile(DQMJobType jobType)
        {
            JobType = jobType;
            Weightage = 0;
            Enabled = true;
        }

        /// <summary>
        /// Constructs DQMJobProfile using specified instance data
        /// </summary>
        public DQMJobProfile(DQMJobProfile source)
            : base(source.Id, source.Name, source.LongName, source.Locale, source.AuditRefId, source.ProgramName)
        {
            this.Action = source.Action;

            this.JobType = source.JobType;

            this.SourceScopeFilter = source.SourceScopeFilter == null ? null : (JobSourceScopeFilter)source.SourceScopeFilter.Clone();

            this.Weightage = source.Weightage;
            this.Enabled = source.Enabled;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clones DQMJobProfile
        /// </summary>
        public virtual object Clone()
        {
            DQMJobProfile profile = new DQMJobProfile(this);
            return profile;
        }

        /// <summary>
        /// Get Xml representation of Job Profile properties (excluding MDMObject's properties)
        /// </summary>
        public virtual String PropertiesOnlyToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteRaw(SourceScopeFilter.ToXml());

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Get Xml representation of Job Profile properties (excluding MDMObject's properties) with outer node
        /// </summary>
        public virtual String PropertiesOnlyToXmlWithOuterNode()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement(JobProfileType);

            xmlWriter.WriteRaw(PropertiesOnlyToXml());

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }


        /// <summary>
        /// Loads Job Profile properties (excluding MDMObject's properties) from XML
        /// </summary>
        public virtual void LoadPropertiesOnlyFromXml(XmlNode xmlNode)
        {
            SourceScopeFilter.LoadFromXml(xmlNode);
        }

        /// <summary>
        /// Loads Job Profile properties (excluding MDMObject's properties) from XML with outer node
        /// </summary>
        public virtual void LoadPropertiesOnlyFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlWithOuterNode);
            XmlNode node = doc.SelectSingleNode(JobProfileType);
            if (node != null)
            {
                LoadPropertiesOnlyFromXml(node);
            }
        }

        #endregion

        #endregion
    }
}