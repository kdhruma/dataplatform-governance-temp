using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for job
    /// </summary>
    [DataContract]
    public class Job : MDMObject, IJob
    {
        #region Fields

        /// <summary>
        /// Field denoting description of Job Status.
        /// </summary>
        private String _description = String.Empty;

        /// <summary>
        /// Field denoting priority of Job Status.
        /// </summary>
        private Int32 _priority = 0;

        /// <summary>
        /// Field denoting name of the Job profile id
        /// </summary>
        private Int32 _profileId = 0;

        /// <summary>
        /// Field denoting name of the Job profile.
        /// </summary>
        private String _profileName = String.Empty;

        /// <summary>
        /// Field denoting Type of the Job profile.
        /// </summary>
        private String _profileType = String.Empty;

        /// <summary>
        /// Field denoting computer name which has created job.
        /// </summary>
        private String _computerName = String.Empty;

        /// <summary>
        /// Field denoting file name which has created job.
        /// </summary>
        private String _fileName = String.Empty;

        /// <summary>
        /// Field denoting created User name.
        /// </summary>
        private String _createdUser = String.Empty;

        /// <summary>
        /// Field denoting create date of the Job.
        /// </summary>
        private String _createdDateTime = String.Empty;

        /// <summary>
        /// Field denoting modified date of the Job.
        /// </summary>
        private String _modifiedDateTime = String.Empty;

        /// <summary>
        /// Field denoting modified next run date of the Job schedule.
        /// </summary>
        private String _nextRunDate = String.Empty;

        /// <summary>
        /// Field denoting whether it is enable or not.
        /// </summary>
        private Boolean _isEnable = false;

        /// <summary>
        /// Field denoting job data as xml
        /// </summary>
        private String _jobDataXml = String.Empty;

        /// <summary>
        /// Field denoting job action
        /// </summary>
        private JobType _jobType = JobType.UnKnown;

        /// <summary>
        /// Field denoting job sub type
        /// </summary>
        private JobSubType _jobSubType = JobSubType.UnKnown;

        /// <summary>
        /// Field denoting job action
        /// </summary>
        private JobAction _jobAction = JobAction.None;

        /// <summary>
        /// Field denoting job status
        /// </summary>
        private JobStatus _jobStatus = JobStatus.UnKnown;

        /// <summary>
        /// Field denoting job data
        /// </summary>
        private JobData _jobData = new JobData();

        /// <summary>
        /// Field denoting parent job identifier
        /// </summary>
        private int _parentJobId;

        /// <summary>
        /// Field denoting file identifier
        /// </summary>
        private int _fileId;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Job()
            : base()
        {
        }

        /// <summary>
        /// Constructor with id, short name, long name , profile name, description, priority, job type, job subtype, created by, created date time, 
        /// modified date time, and status.
        /// </summary>
        /// <param name="id">Indicates the identity of job schedule</param>
        /// <param name="shortName">Indicates the name of job schedule</param>
        /// <param name="longName">Indicates the long name of job schedule</param>
        /// <param name="profilename">Indicates the name of profile</param>
        /// <param name="description">Indicates the description about the job schedule</param>
        /// <param name="priority">Indicates the priority of job</param>
        /// <param name="jobType">Indicates the type of job</param>
        /// <param name="jobSubType">Indicates the sub type of job</param>
        /// <param name="createdBy">Indicates the user name who created the job</param>
        /// <param name="createdDateTime">Indicates the date time information when job is created</param>
        /// <param name="modifiedDateTime">Indicates the date time information when job is modified</param>
        /// <param name="status">Indicates the status of job</param>
        public Job(Int32 id, String shortName, String longName, String profilename, String description, Int32 priority, String jobType, String jobSubType, String createdBy, String createdDateTime, String modifiedDateTime, String status)
            : base(id, shortName, longName)
        {
            this._profileName = profilename;
            this._description = description;
            this._priority = priority;
            this._createdUser = createdBy;
            this._modifiedDateTime = modifiedDateTime;
            this._createdDateTime = createdDateTime;
            this._jobStatus = GetJobStatus(status);
            this._jobType = GetJobType(jobType);
            this._jobSubType = GetJobSubType(jobSubType);
        }

        /// <summary>
        /// Constructor with profile name
        /// </summary>
        /// <param name="profilename">Indicates the profile Name</param>
        public Job(String profilename)
        {
            this._profileName = profilename;
        }

        /// <summary>
        /// Constructor with Id , short name , long name , content,Is enable ,modified date,created by,next run date and last run date.
        /// </summary>
        /// <param name="id">Indicates the Identity of the job schedule id</param>
        /// <param name="shortName">Indicates the name of the job schedule name</param>
        /// <param name="longName">Indicates the long name of the job schedule long name</param>
        /// <param name="content">Indicates the content of the job schedule data</param>
        /// <param name="isEnable"></param>
        /// <param name="modifiedDateTime">Indicates the modified date of the job schedule</param>
        /// <param name="createdBy">Indicates the create user of the job schedule</param>
        /// <param name="nextRunDate">Indicates the next run date of the job schedule</param>
        /// <param name="lastRunstatus">Indicates the last run status of the job schedule</param>
        /// <param name="computerName">Indicates the name of computer which processed the job</param>
        public Job(Int32 id, String shortName, String longName, String content, Boolean isEnable, String modifiedDateTime, String createdBy, String nextRunDate, String lastRunstatus, String computerName)
            : base(id, shortName, longName)
        {
            this._jobDataXml = content;
            this._isEnable = isEnable;
            this._modifiedDateTime = modifiedDateTime;
            this._createdUser = createdBy;
            this._nextRunDate = nextRunDate;
            this._jobStatus = GetJobStatus(lastRunstatus);
            this._computerName = computerName;
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the file name which has created job
        /// </summary>
        [DataMember]
        public String FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
            }
        }

        /// <summary>
        ///  Property denoting the description of the Job
        /// </summary>
        [DataMember]
        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        ///  Property denoting the priority of the Job
        /// </summary>
        [DataMember]
        public Int32 Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        /// <summary>
        ///  Property denoting the id of the Job profile.
        /// </summary>
        [DataMember]
        public Int32 ProfileId
        {
            get
            {
                return this._profileId;
            }
            set
            {
                this._profileId = value;
            }
        }

        /// <summary>
        ///  Property denoting the Name of the Job profile.
        /// </summary>
        [DataMember]
        public String ProfileName
        {
            get
            {
                return this._profileName;
            }
            set
            {
                this._profileName = value;
            }
        }

        /// <summary>
        ///  Property denoting the Name of the Job profile.
        /// </summary>
        [DataMember]
        public String ProfileType
        {
            get
            {
                return this._profileType;
            }
            set
            {
                this._profileType = value;
            }
        }

        /// <summary>
        ///  Property denoting the computer name which has created job
        /// </summary>
        [DataMember]
        public String ComputerName
        {
            get
            {
                return this._computerName;
            }
            set
            {
                this._computerName = value;
            }
        }

        /// <summary>
        ///  Property denoting the create date of the Job
        /// </summary>
        [DataMember]
        public String CreatedUser
        {
            get
            {
                return this._createdUser;
            }
            set
            {
                this._createdUser = value;
            }
        }

        /// <summary>
        ///  Property denoting the created date of the Job
        /// </summary>
        [DataMember]
        public String CreatedDateTime
        {
            get
            {
                return this._createdDateTime;
            }
            set
            {
                this._createdDateTime = value;
            }
        }

        /// <summary>
        ///  Property denoting the modified date of the Job
        /// </summary>
        [DataMember]
        public String ModifiedDateTime
        {
            get
            {
                return this._modifiedDateTime;
            }
            set
            {
                this._modifiedDateTime = value;
            }
        }

        /// <summary>
        ///  Property denoting the Next Run date of the Job schedule
        /// </summary>
        [DataMember]
        public String NextRunDate
        {
            get
            {
                return this._nextRunDate;
            }
            set
            {
                this._nextRunDate = value;
            }
        }

        /// <summary>
        ///  Property denoting the Content Data.
        /// </summary>
        [DataMember]
        public String JobDataXml
        {
            get
            {
                return this._jobDataXml;
            }
            set
            {
                this._jobDataXml = value;
            }
        }

        /// <summary>
        ///  Property denoting the whether it is enable or not
        /// </summary>
        [DataMember]
        public Boolean IsEnable
        {
            get
            {
                return this._isEnable;
            }
            set
            {
                this._isEnable = value;
            }
        }

        /// <summary>
        ///  Property denoting the Type of the Job
        /// </summary>
        [DataMember]
        public JobType JobType
        {
            get
            {
                return this._jobType;
            }
            set
            {
                this._jobType = value;
            }
        }

        /// <summary>
        ///  Property denoting the sub type of the Job
        /// </summary>
        [DataMember]
        public JobSubType JobSubType
        {
            get
            {
                return this._jobSubType;
            }
            set
            {
                this._jobSubType = value;
            }
        }

        /// <summary>
        ///  Property denoting job action
        /// </summary>
        [DataMember]
        public JobAction JobAction
        {
            get
            {
                return this._jobAction;
            }
            set
            {
                this._jobAction = value;
            }
        }

        /// <summary>
        ///  Property denoting job status
        /// </summary>
        [DataMember]
        public JobStatus JobStatus
        {
            get
            {
                return this._jobStatus;
            }
            set
            {
                this._jobStatus = value;
            }
        }

        /// <summary>
        ///  Property denoting job data
        /// </summary>
        [DataMember]
        public JobData JobData
        {
            get
            {
                return this._jobData;
            }
            set
            {
                this._jobData = value;
            }
        }

        /// <summary>
        /// Property denoting parent job
        /// </summary>
        [DataMember]
        public int ParentJobId
        {
            get
            {
                return this._parentJobId;
            }
            set
            {
                this._parentJobId = value;
            }
        }

        /// <summary>
        /// Property denoting file
        /// </summary>
        [DataMember]
        public int FileId
        {
            get
            {
                return this._fileId;
            }
            set
            {
                this._fileId = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load JobParameter object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadJob(String valuesAsXml)
        {

        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String jobXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //jobXml node start
            xmlWriter.WriteStartElement("Job");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("Priority", this.Priority.ToString());
            xmlWriter.WriteAttributeString("JobType", this.JobType.ToString());
            xmlWriter.WriteAttributeString("JobStatus", this.JobStatus.ToString());
            xmlWriter.WriteAttributeString("CreatedBy", this.CreatedUser.ToString());
            xmlWriter.WriteAttributeString("CreatedDateTime", this.CreatedDateTime.ToString());

            #endregion

            xmlWriter.WriteRaw(this.JobData.ToXml());

            //Job node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            jobXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return jobXml;
        }

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            String jobXml = String.Empty;

            switch (serialization)
            {
                case ObjectSerialization.UIRender:
                    StringWriter sw = new StringWriter();
                    XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                    //jobXml node start
                    xmlWriter.WriteStartElement("Job");


                    #region Write Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("name", this.Name);
                    xmlWriter.WriteAttributeString("type", this.JobType.ToString());
                    xmlWriter.WriteAttributeString("subtype", this.JobStatus.ToString());
                    xmlWriter.WriteAttributeString("description", this.Description);
                    xmlWriter.WriteAttributeString("status", this.JobStatus.ToString());
                    xmlWriter.WriteAttributeString("userAction", this.Action.ToString());
                    xmlWriter.WriteAttributeString("computerName", this.ComputerName.ToString());
                    xmlWriter.WriteAttributeString("username", this.CreatedUser.ToString());

                    #endregion

                    xmlWriter.WriteStartElement("data");

                    xmlWriter.WriteRaw(String.IsNullOrWhiteSpace(this.JobDataXml) ? this.JobData.ToXml() : this.JobDataXml);

                    //data node end
                    xmlWriter.WriteEndElement();
                    //Job node end
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    //get the actual XML
                    jobXml = sw.ToString();

                    xmlWriter.Close();
                    sw.Close();

                    break;
                default:
                    jobXml = this.ToXml();
                    break;
            }


            return jobXml;
        }

        #endregion

        #region IJob Methods

        /// <summary>
        /// Clone hierarchy object
        /// </summary>
        /// <returns>cloned copy of hierarchy object.</returns>
        public IJob Clone()
        {
            Job job = (Job)MemberwiseClone();
            return job;
        }

        /// <summary>
        /// Gets job data
        /// </summary>
        /// <returns>Job Data object</returns>
        /// <exception cref="NullReferenceException">Thrown when JobData is null</exception>
        public IJobData GetJobData()
        {
            if (this.JobData == null)
            {
                throw new NullReferenceException("JobData is null");
            }

            return (IJobData)this.JobData;
        }

        /// <summary>
        /// Sets job data
        /// </summary>
        /// <param name="iJobData">Job data object</param>
        /// <returns>Result of the operation</returns>
        public Boolean SetJobData(IJobData iJobData)
        {
            if (iJobData == null)
            {
                throw new ArgumentNullException("iJobData");
            }

            this.JobData = (JobData)iJobData;

            return true;
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Get job status based on status
        /// </summary>
        /// <param name="status">Indicates the status based on which job status is returned</param>
        /// <returns>Returns the job status based on status</returns>
        public static JobStatus GetJobStatus(String status)
        {
            JobStatus jobStatus = Core.JobStatus.UnKnown;
            Enum.TryParse<JobStatus>(status, out jobStatus);
            return jobStatus;
        }

        /// <summary>
        /// Get job type based on database job type
        /// </summary>
        /// <param name="dbJobType">Indicates the database job type based on which job type is returned</param>
        /// <returns>Returns job type based on database job type</returns>
        public static JobType GetJobType(String dbJobType)
        {
            JobType jobType = JobType.UnKnown;
            switch (dbJobType.ToLower())
            {
                case "import":
                    jobType = JobType.Import;
                    break;
                case "entityimport":
                    jobType = JobType.EntityImport;
                    break;
                case "catalogexport":
                    jobType = JobType.EntityExport;
                    break;
                case "attributematching":
                    jobType = JobType.AttributeMatching;
                    break;
                case "lookupimport":
                    jobType = JobType.LookupImport;
                    break;
                case "lookupexport":
                    jobType = JobType.LookupExport;
                    break;
                case "datamodelimport":
                    jobType = JobType.DataModelImport;
                    break;
                case "custom":
                    jobType = JobType.Custom;
                    break;
                case "diagnosticreportexport":
                    jobType = JobType.DiagnosticReportExport;
                    break;
                case "ddgimport":
                    jobType = JobType.DDGImport;
                    break;
                default:
                    break;
            }
            return jobType;
        }

        /// <summary>
        /// Get the job type name in database based on the job type enumeration
        /// </summary>
        /// <param name="jobType">Indicates the type of job</param>
        /// <returns>Returns the job type name in database based on job type enum</returns>
        public static String GetDBJobType(JobType jobType)
        {
            String type = String.Empty;

            switch (jobType)
            {
                case JobType.Import:
                    type = "Import";
                    break;
                case JobType.EntityImport:
                    type = "EntityImport";
                    break;
                case JobType.EntityExport:
                    type = "CatalogExport";
                    break;
                case JobType.AttributeMatching:
                    type = "AttributeMatching";
                    break;
                case JobType.LookupImport:
                    type = "LookupImport";
                    break;
                case JobType.Custom:
                    type = "Custom";
                    break;
                case JobType.BulkOperation:
                    type = "BulkOperation";
                    break;
                case JobType.LookupExport:
                    type = "LookupExport";
                    break;
                case JobType.DataModelImport:
                    type = "DataModelImport";
                    break;
                case JobType.DiagnosticReportExport:
                    type = "DiagnosticReportExport";
                    break;
                case JobType.DDGImport:
                    type = "DDGImport";
                    break;
                default:
                    break;
            }
            return type;
        }

        /// <summary>
        /// Get job subtype based on the subtype specified
        /// </summary>
        /// <param name="subType">Indicates the subtype based on which job sub type is fetched</param>
        /// <returns>Returns the job subtype based on the subtype specified</returns>
        public static JobSubType GetJobSubType(String subType)
        {
            JobSubType jobSubType = JobSubType.UnKnown;
            Enum.TryParse<JobSubType>(subType, out jobSubType);
            return jobSubType;
        }

        #endregion

        #endregion
    }
}
