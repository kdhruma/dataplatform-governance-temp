using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Collections.Generic;

    /// <summary>
    /// Specifies the BuildContext object
    /// </summary>
    [DataContract]
    public class BuildDetailContext : MDMObject, IBuildDetailContext
    {
        #region Fields
        /// <summary>
        /// Field for the id of a Build
        /// </summary>
        private Int32 _buildFeatureId;
        /// <summary>
        /// Field for the FilePath 
        /// </summary>
        private String _filePath;
        /// <summary>
        /// Collection for storing path and hash 
        /// </summary>
        private Dictionary<String, String> _fileHashDetails;
        /// <summary>
        /// Field for the Feature of the Build
        /// </summary>
        private String _feature;
        /// <summary>
        /// Field for the FeatureDescription of the Build
        /// </summary>
        private String _featureDescription;
        /// <summary>
        /// Field for the Version of the Build
        /// </summary>
        private String _version;
        /// <summary>
        /// Field for the CoreError of the Build
        /// </summary>
        private Int32 _coreError;
        /// <summary>
        /// Field for the MdmCenterLog of the Build
        /// </summary>
        private String _mdmCenterLog;
        /// <summary>
        /// Field for the WorkFlowError of the Build
        /// </summary>
        private Int32 _workFlowError;
        /// <summary>
        /// Field for the WorkFlowErrorLog of the Build
        /// </summary>
        private String _workFlowErrorLog;
        /// <summary>
        /// Field for the VpError of the Build
        /// </summary>
        private Int32 _vpError;
        /// <summary>
        /// Field for the VpErrorLog of the Build
        /// </summary>
        private String _vpErrorLog;
        /// <summary>
        /// Field for the BuildType of the Build
        /// </summary>
        private String _buildType;
        /// <summary>
        /// Field for the BuildUser of the Build
        /// </summary>
        private String _buildUser;
        /// <summary>
        /// Field for the BuildServer of the Build
        /// </summary>
        private String _buildServer;


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BuildContext class.
        /// </summary>
        public BuildDetailContext()
            : base()
        {

        }


        #endregion

        #region Properties
        /// <summary>
        /// Property denoting identity of the build
        /// </summary>
        [DataMember]
        public int BuildFeatureId
        {
            get
            {
                return _buildFeatureId;
            }
            set
            {
                _buildFeatureId = value;
            }
        }

        /// <summary>
        /// Property denoting path of the file
        /// </summary>
        [DataMember]
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }

        /// <summary>
        /// Collection for storing file path and checksum
        /// </summary>
        [DataMember]
        public Dictionary<String, String> FileHashDetails
        {
            get
            {
                return _fileHashDetails;
            }
            set
            {
                _fileHashDetails = value;
            }
        }
        
        /// <summary>
        /// Property denoting feature description of build
        /// </summary>
        [DataMember]
        public string FeatureDescription
        {
            get
            {
                return _featureDescription;
            }
            set
            {
                _featureDescription = value;
            }
        }
        
        /// <summary>
        /// Property denoting the feature of build
        /// </summary>
        [DataMember]
        public string Feature
        {
            get
            {
                return _feature;
            }
            set
            {
                _feature = value;
            }
        }
       
        /// <summary>
        /// Property denoting the version of the Build
        /// </summary>
        [DataMember]
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        /// <summary>
        /// Property denoting the core error of the Build
        /// </summary>
        [DataMember]
        public int CoreError
        {
            get
            {
                return _coreError;
            }
            set
            {
                _coreError = value;
            }
        }

        /// <summary>
        /// Property denoting the MDM Center log of the Build
        /// </summary>
        [DataMember]
        public string MdmCenterLog
        {
            get
            {
                return _mdmCenterLog;
            }
            set
            {
                _mdmCenterLog = value;
            }
        }

        /// <summary>
        /// Property denoting workflow error of the Build
        /// </summary>
        [DataMember]
        public int WorkFlowError
        {
            get
            {
                return _workFlowError;
            }
            set
            {
                _workFlowError = value;
            }
        }

        /// <summary>
        /// Property denoting workflow error log of the Build
        /// </summary>
        [DataMember]
        public string WorkFlowErrorLog
        {
            get
            {
                return _workFlowErrorLog;
            }
            set
            {
                _workFlowErrorLog = value;
            }
        }
        
        /// <summary>
        /// Property denoting the VP error of the Build
        /// </summary>
        [DataMember]
        public int VpError
        {
            get
            {
                return _vpError;
            }
            set
            {
                _vpError = value;
            }
        }

        /// <summary>
        /// Property denoting the VP error log of the Build
        /// </summary>
        [DataMember]
        public string VpErrorLog
        {
            get
            {
                return _vpErrorLog;
            }
            set
            {
                _vpErrorLog = value;
            }
        }

        /// <summary>
        /// Property denoting a type of the build
        /// </summary>
        [DataMember]
        public string BuildType
        {
            get
            {
                return _buildType;
            }
            set
            {
                _buildType = value;
            }
        }

        /// <summary>
        /// Property denoting a user of the build
        /// </summary>
        [DataMember]
        public string BuildUser
        {
            get
            {
                return _buildUser;
            }
            set
            {
                _buildUser = value;
            }
        }

        /// <summary>
        /// Property denoting a build server
        /// </summary>
        [DataMember]
        public string BuildServer
        {
            get
            {
                return _buildServer;
            }
            set
            {
                _buildServer = value;
            }
        }

        #endregion

        #region Private Methods
        #endregion
    }
}