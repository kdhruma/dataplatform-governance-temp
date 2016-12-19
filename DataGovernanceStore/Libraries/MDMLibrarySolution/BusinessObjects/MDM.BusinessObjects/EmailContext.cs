using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies WorkflowEmailContext which indicates what all users who should be sent an email 
    /// </summary>
    [DataContract]
    public class EmailContext : ObjectBase, IEmailContext
    {
        #region Fields

        /// <summary>
        /// Field denoting Collection of user logins to send the email
        /// </summary>
        private Collection<String> _toMDMUserLoginIds;

        /// <summary>
        /// Field denoting Collection of email ids to send the email
        /// </summary>
        private Collection<String> _toEmailIds;

        /// <summary>
        /// Field denoting Collection of role names to send the email
        /// </summary>
        private Collection<String> _toMDMRoleNames;

        /// <summary>
        /// Field denoting Collection of user login ids to CC the email
        /// </summary>
        private Collection< String> _ccMDMUserLoginIds;

        /// <summary>
        /// Field denoting Collection of email ids to CC the email
        /// </summary>
        private	Collection<String> _ccEmailIds;

        /// <summary>
        /// Field denoting Collection of role names ids to CC the email
        /// </summary>
        private Collection<String> _ccMDMRoleNames;

        /// <summary>
        /// Field denoting the template name which is to used to send the email
        /// </summary>
        private String _templateName;

        /// <summary>
        /// Field denoting Dictionary of (Key,Value) pairs to fill in the template data
        /// </summary>
        private Dictionary<String, String> _templateData;

        /// <summary>
        /// Field denoting whether to send one email to everyone or separate emails to each one
        /// </summary>
        private Boolean _sendMailPerEmailId = false;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EmailContext()
            : base()
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting Collection of user logins to send the email
        /// </summary>
        [DataMember]
        public Collection<String> ToMDMUserLoginIds
        {
            get
            {
                if (_toMDMUserLoginIds == null)                    
                {
                    _toMDMUserLoginIds = new Collection<String>();                    
                }
                return _toMDMUserLoginIds;
            }
            set
            {
                _toMDMUserLoginIds = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of email ids to send the email
        /// </summary>
        [DataMember]
        public Collection<String> ToEmailIds
        {
            get
            {
                if (_toEmailIds == null)
                {
                    _toEmailIds = new Collection<String>();
                }
                return _toEmailIds;
            }
            set
            {
                _toEmailIds = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of role names to send the email
        /// </summary>
        [DataMember]
        public Collection<String> ToMDMRoleNames
        {
            get
            {
                if (_toMDMRoleNames == null)
                {
                    _toMDMRoleNames = new Collection<String>();
                }
                return _toMDMRoleNames;
            }
            set
            {
                _toMDMRoleNames = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of user login ids to CC the email
        /// </summary>
        [DataMember]
        public Collection<String> CCMDMUserLoginIds
        {
            get
            {
                if (_ccMDMUserLoginIds == null)
                {
                    _ccMDMUserLoginIds = new Collection<String>();
                }
                return _ccMDMUserLoginIds;
            }
            set
            {
                _ccMDMUserLoginIds = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of email ids to CC the email
        /// </summary>
        [DataMember]
        public Collection<String> CCEmailIds
        {
            get
            {
                if (_ccEmailIds == null)
                {
                    _ccEmailIds = new Collection<String>();
                }
                return _ccEmailIds;
            }
            set
            {
                _ccEmailIds = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of role names to CC the email
        /// </summary>
        [DataMember]
        public Collection<String> CCMDMRoleNames
        {
            get
            {
                if (_ccMDMRoleNames == null)
                {
                    _ccMDMRoleNames = new Collection<String>();
                }
                return _ccMDMRoleNames;
            }
            set
            {
                _ccMDMRoleNames = value;
            }
        }

        /// <summary>
        /// Property denoting the template name which is to used to send the email
        /// </summary>
        [DataMember]
        public String TemplateName
        {
            get
            {
                return _templateName;
            }
            set
            {
                _templateName = value;
            }
        }

        /// <summary>
        /// Property denoting Dictionary of (Key,Value) pairs to fill in the template data
        /// </summary>
        [DataMember]
        public Dictionary<String, String> TemplateData
        {
            get
            {
                if (_templateData == null)
                {
                    _templateData = new Dictionary<String, String>();
                }
                return _templateData;
            }
            set
            {
                _templateData = value;
            }
        }

        /// <summary>
        /// Property denoting whether to send one email to everyone or separate emails to each one
        /// </summary>
        [DataMember]
        public Boolean SendMailPerEmailId
        {
            get
            {
                return _sendMailPerEmailId;
            }
            set
            {
                _sendMailPerEmailId = value;
            }
        }

        #endregion Properties

        #region Public methods

                
        #endregion Public methods
    }
}
