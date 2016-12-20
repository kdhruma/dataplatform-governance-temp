using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using ProtoBuf;

    /// <summary>
    /// Specifies SecurityContext which indicates what all information is to be loaded in Entity object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class SecurityContext : ObjectBase, ISecurityContext
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Int32 _userId = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private String _userLoginName;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        private Int32 _userRoleId = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        private String _userRoleName;

        /// <summary>
        /// 
        /// </summary>
        private IIdentity _identity;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public SecurityContext()
            : base()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public SecurityContext(Int32 userId, String userLoginName, Int32 userRoleId, String userRoleName)
            : base()
        {
            _userId = userId;
            _userLoginName = userLoginName;
            _userRoleId = userRoleId;
            _userRoleName = userRoleName;
        }

        /// <summary>
        ///  Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public SecurityContext(String valuesAsXml)
        {
            LoadSecurityContext(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Int32 UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String UserLoginName
        {
            get
            {
                return _userLoginName;
            }
            set
            {
                _userLoginName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 UserRoleId
        {
            get
            {
                return _userRoleId;
            }
            set
            {
                _userRoleId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String UserRoleName
        {
            get
            {
                return _userRoleName;
            }
            set
            {
                _userRoleName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
            set
            {
                _identity = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public methods

        /// <summary>
        /// Get XML representation of Security Context object
        /// </summary>
        /// <returns>XML representation of Security Context object</returns>
        public string ToXml()
        {
            String securityContextXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Security Context Map node start
            xmlWriter.WriteStartElement("SecurityContext");

            #region Write Security Context properties

            xmlWriter.WriteAttributeString("UserId", this.UserId.ToString());
            xmlWriter.WriteAttributeString("UserLoginName", this.UserLoginName);
            xmlWriter.WriteAttributeString("UserRoleId", this.UserRoleId.ToString());
            xmlWriter.WriteAttributeString("UserRoleName", this.UserRoleName);
            xmlWriter.WriteRaw(IdentityToXml());

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            securityContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return securityContextXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SecurityContext Clone()
        {
            SecurityContext clonedSecurityContext = new SecurityContext();

            clonedSecurityContext._userId = _userId;
            clonedSecurityContext._userLoginName = _userLoginName;
            clonedSecurityContext._userRoleId = _userRoleId;
            clonedSecurityContext._userRoleName = _userRoleName;

            return clonedSecurityContext;
        }

        #endregion Public methods

        #region Private methods

        private void LoadSecurityContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "SecurityContext")
                    {
                        #region Read Security Context Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("UserLoginName"))
                            {
                                this.UserLoginName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("UserId"))
                            {
                                this.UserId = reader.ReadContentAsInt();
                            }
                            if (reader.MoveToAttribute("UserRoleId"))
                            {
                                this.UserRoleId = reader.ReadContentAsInt();
                            }
                            if (reader.MoveToAttribute("UserRoleName"))
                            {
                                this.UserRoleName = reader.ReadContentAsString();
                            }
                        }

                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
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

        private String IdentityToXml()
        {
            String securityContextXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Security Context Map node start
            xmlWriter.WriteStartElement("Identity");

            #region Write Identity properties

            if (this.Identity != null)
            {
                xmlWriter.WriteAttributeString("AuthenticationType", this.Identity.AuthenticationType);
                xmlWriter.WriteAttributeString("IsAuthenticated", this.Identity.IsAuthenticated.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("Name", this.Identity.Name);
            }

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            securityContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return securityContextXml;
        }

        #endregion Private methods

        #endregion Methods

    }
}
