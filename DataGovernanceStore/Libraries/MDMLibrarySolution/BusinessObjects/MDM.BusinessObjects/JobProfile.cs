using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Job Profile
    /// </summary>
    [DataContract]
    public class JobProfile : MDMObject
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        private String _profileDomain = String.Empty;

        /// <summary>
        /// field denoting profile data xml of job
        /// </summary>
        private String _profileDataXml = String.Empty;
       
        /// <summary>
        /// 
        /// </summary>
        private String _fileType = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// 
        /// </summary>
        private String _organizationName = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private Int32 _containerId  = 0;

        /// <summary>
        ///
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Indicates whether profile is system
        /// </summary>
        private Boolean _isSystemProfile = false;

        /// <summary>
        /// Indicates the job type of profile
        /// </summary>
        private JobType _jobType = JobType.UnKnown;

        /// <summary>
        /// Indicates last modified date and time for profile
        /// </summary>
        private DateTime? _lastModified = new DateTime();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public JobProfile()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public JobProfile(String valuesAsXml)
        {
            LoadJobProfile(valuesAsXml);
        }
        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the job profile domain
        /// </summary>
        [DataMember]
        public String ProfileDomain
        {
            get
            {
                return this._profileDomain;
            }
            set
            {
                this._profileDomain = value;
            }
        }

        /// <summary>
        ///  
        /// </summary>
        [DataMember]
        public String ProfileDataXml
        {
            get
            {
                return this._profileDataXml;
            }
            set
            {
                this._profileDataXml = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String FileType
        {
            get
            {
                return this._fileType;
            }
            set
            {
                this._fileType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                this._containerId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get
            {
                return this._containerName;
            }
            set
            {
                this._containerName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get
            {
                return this._organizationId;
            }
            set
            {
                this._organizationId = value;
            }
        }

        /// <summary>
        ///  Property denoting the Next Run date of the Job schedule
        /// </summary>
        [DataMember]
        public String OrganizationName
        {
            get
            {
                return this._organizationName;
            }
            set
            {
                this._organizationName = value;
            }
        }

        /// <summary>
        /// Indicates whether profile is system's one
        /// </summary>
        [DataMember]
        public Boolean IsSystemProfile
        {
            get
            {
                return this._isSystemProfile;
            }
            set
            {
                this._isSystemProfile = value;
            }
        }

        /// <summary>
        /// Indicates the job type of profile
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
        /// Indicates last modified date and time for profile
        /// </summary>
        [DataMember]
        public DateTime? LastModified
        {
            get
            {
                return _lastModified;
            }
            set
            {
                _lastModified = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load job profile from Xml
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadJobProfile(String valuesAsXml)
        {
            
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of JobProfile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            throw new NotImplementedException();       
        }

        /// <summary>
        /// Get Xml representation of JobProfile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            // No serialization impemented for now...
            return this.ToXml();
        }

        #endregion
      
        #endregion
    }
}
