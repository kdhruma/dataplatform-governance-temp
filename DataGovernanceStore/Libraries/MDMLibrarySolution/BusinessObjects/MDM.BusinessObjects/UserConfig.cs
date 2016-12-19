using System;
using System.Xml;
using System.Runtime.Serialization;

using MDM.Core;
using MDM.Interfaces;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the UserConfig
    /// </summary>
    [DataContract]
    public class UserConfig : MDMObject,IUserConfig
    {
        #region Fields

        /// <summary>
        /// field denoting UserConfig Type Id
        /// </summary>
        private Int32 _UserConfigTypeId = -1;

        /// <summary>
        /// field denoting Security User Id
        /// </summary>
        private Int32 _securityUserId = -1;

        /// <summary>
        /// field denoting Organization Id
        /// </summary>
        private Int32 _orgId = -1;

        /// <summary>
        /// field denoting Configuration Xml
        /// </summary>
        private String _configXml = String.Empty;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the UserConfigTypeId of the User
        /// </summary>
        [DataMember]
        public Int32 UserConfigTypeId
        {
            get { return this._UserConfigTypeId; }
            set { this._UserConfigTypeId = value; }
        }

        /// <summary>
        /// Property denoting the SecurityId of User
        /// </summary>
        [DataMember]
        public Int32 SecurityUserId
        {
            get { return this._securityUserId; }
            set { this._securityUserId = value; }
        }

        /// <summary>
        /// Property denoting the Organization Id of the User
        /// </summary>
        [DataMember]
        public Int32 OrgId
        {
            get { return this._orgId; }
            set { this._orgId = value; }
        }


        /// <summary>
        /// Property denoting the configXml of the User
        /// </summary>
        [DataMember]
        public String ConfigXml
        {
            get { return this._configXml; }
            set { this._configXml = value; }
        }

        #endregion
    }
}

