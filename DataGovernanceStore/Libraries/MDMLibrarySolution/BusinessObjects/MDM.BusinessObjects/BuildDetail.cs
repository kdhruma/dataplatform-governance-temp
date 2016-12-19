using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Build object
    /// </summary>
    [DataContract]
    public class BuildDetail : MDMObject, IBuildDetail
    {
        #region Fields

        /// <summary>
        /// Field for the id of the Build
        /// </summary>
        private Int32 _id = -1;

       /// <summary>
		/// Field for the BuildVersion of the Build
		/// </summary>
        private String _buildVersion;

       /// <summary>
        /// Field for the BuildType  of the Build
        /// </summary>
        private String _buildType;

       /// <summary>
        /// Field for the BuildServer of the Build
        /// </summary>
        private String _buildServer;

        /// <summary>
        /// Field for the BuildUser of the Build
        /// </summary>
        private String _buildUser;
            
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BuildDetail class.
        /// </summary>
        public BuildDetail()
            : base()
        {

        }
        

        #endregion

        #region Properties

        /// <summary>
        /// Property for the id  of the Build
        /// </summary>
        [DataMember]
        public new Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Field for the BuildVersion of the Build
        /// </summary>
        [DataMember]
        public String BuildVersion
        {
            get
            {
                return _buildVersion;
            }
            set
            {
                _buildVersion = value;
            }
        }

        /// <summary>
        /// Field for the BuildType  of the Build
        /// </summary>
        [DataMember]
        public String BuildType
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
        /// Field for the BuildServer  of the Build
        /// </summary>
        [DataMember]
        public String BuildServer
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

        /// <summary>
        /// Field for the BuildUser  of the Build
        /// </summary>
        [DataMember]
        public String BuildUser
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
       

        #endregion

        #region Private Methods
        #endregion

        
    }
}