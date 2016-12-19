using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies a SecurityUser
    /// </summary>
    [DataContract]
    [KnownType(typeof(SecurityRoleCollection))]
    [KnownType(typeof(UserPreferences))]
    public class SecurityUser : MDMObject, ISecurityUser, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting Security_UserType of current user
        /// </summary>
        private Int32 _securityUserTypeId = -1;

        /// <summary>
        /// Field denoting Login of current user
        /// </summary>
        private String _securityUserLogin = String.Empty;

        /// <summary>
        /// Field denoting security role id
        /// </summary>
        private Int32 _securityRoleId = -1;

        /// <summary>
        /// Field denoting if current user is a SystemUser
        /// </summary>
        private Boolean _systemUser = false;

        /// <summary>
        /// Field denoting SMTP address
        /// </summary>
        private String _smtp = String.Empty;

        /// <summary>
        /// Field denoting Password of current user
        /// </summary>
        private String _password = String.Empty;

        /// <summary>
        /// Field denoting ExternalEntityID
        /// </summary>
        private Int32 _externalEntityId = -1;

        /// <summary>
        /// Field denoting manager Id of current user
        /// </summary>
        private Int32 _managerId = -1;

        /// <summary>
        /// Field denoting current user's manager's login
        /// </summary>
        private String _managerLogin = String.Empty;

        /// <summary>
        /// Field denoting First Name of current user
        /// </summary>
        private String _firstName = String.Empty;

        /// <summary>
        /// Field denoting Last Name of current user
        /// </summary>
        private String _lastName = String.Empty;

        /// <summary>
        /// Field denoting Initials of current user
        /// </summary>
        private String _initials = String.Empty;

        /// <summary>
        /// Field denoting whether current user is Disabled
        /// </summary>
        private Boolean _disabled = false;

        /// <summary>
        /// Field denoting the mapped security roles for the security user
        /// </summary>
        private SecurityRoleCollection _roles = new SecurityRoleCollection();

        /// <summary>
        /// Field denoting the current user authentication type
        /// </summary>
        private AuthenticationType _authenticationType = AuthenticationType.Unknown;

        /// <summary>
        /// Field denoting whether the user is created by system or not
        /// </summary>
        private Boolean _isSystemCreated = false;

        /// <summary>
        /// Field denoting the SAML authentication token
        /// </summary>
        private String _authenticationToken = String.Empty;

        /// <summary>
        /// Field Denoting the original security user
        /// </summary>
        private SecurityUser _originalSecurityUser = null;

        /// <summary>
        /// Field denoting user preference
        /// </summary>
        private UserPreferences _userPreference = new UserPreferences();

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting security user key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// 
        public SecurityUser()
            : base()
        {

        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a SecurityUser</param>
        public SecurityUser(Int32 id)
            : base(id)
        {

        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for SecurityUser. </param>
        public SecurityUser(object[] objectArray)
        {
            int intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);
            this.Id = intId;

            if (objectArray[1] != null)
                this.SecurityUserLogin = objectArray[1].ToString();

            if (objectArray[2] != null)
                this.Smtp = objectArray[2].ToString();

            if (objectArray[3] != null)
                this.FirstName = objectArray[3].ToString();

            if (objectArray[4] != null)
                this.LastName = objectArray[4].ToString();

            if (objectArray[5] != null)
                this.Initials = objectArray[5].ToString();

            if (objectArray.Length > 6)
            {
                if (objectArray[6] != null)
                {
                    Enum.TryParse<AuthenticationType>(objectArray[6].ToString(), out _authenticationType);
                    this.AuthenticationType = _authenticationType;
                }
            }
        }

        /// <summary>
        /// Constructor with XML as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML representing the value of SecurityUser</param>
        /// <example>
        /// Sample XML:
        /// <para>
        ///     &lt;User 
        ///         PK_Security_User="1" 
        ///         UserType="1" 
        ///         Login="cfadmin" 
        ///         FirstName="ProductCenter" 
        ///         LastName="SystemAdmin" 
        ///         Initials="PCADMIN" 
        ///         Disabled="False" 
        ///         ExternalEntityID="0" 
        ///         SystemUser="N" 
        ///         SMTP="cfadmin@dm.com" 
        ///         PK_Security_Role="2" 
        ///         Action="Display" /&gt;
        /// </para>
        /// </example>
        public SecurityUser(String valuesAsXml)
        {
            this.LoadSecurityUser(valuesAsXml);
        }

        /// <summary>
        ///  Constructor with Id, First Name, Last Name, Login Name and System User as Parameters.
        /// </summary>
        /// <param name="userId">Indicates the Identity of a Security User</param>
        /// <param name="userLogin">Indicates the System user of a Security User</param>
        /// <param name="smtp">Indicates the SMTP of a Security User</param>
        /// <param name="firstName">Indicates the First name of a Security User</param>
        /// <param name="lastName">Indicates the Last name of a Security User</param>
        /// <param name="initials">Indicates the Initials of a Security User</param>
        /// <param name="isWindowsAuthentication">Indicates whether it is windows authentication or not</param>
        public SecurityUser(Int32 userId, String userLogin, String smtp, String firstName, String lastName, String initials, Boolean isWindowsAuthentication)
            : base(userId, firstName, lastName)
        {
            this._securityUserLogin = userLogin;
            this._smtp = smtp;
            this._initials = initials;
            this.IsWindowsAuthentication = isWindowsAuthentication;
            this._firstName = firstName;
            this._lastName = lastName;
        }

        /// <summary>
        ///  Constructor with Id, First Name, Last Name, Login Name and System User as Parameters.
        /// </summary>
        /// <param name="SecurityUserId">Indicates the Identity of a Security User</param>
        /// <param name="securityUserTypeId">Indicates the Type Id of a Security User</param>
        /// <param name="securityUserLogin">Indicates the Login of a Security User</param>
        /// <param name="securityRoleId">Indicates the Role Id of a Security User</param>
        /// <param name="systemUser">Indicates the System user of a Security User</param>
        /// <param name="smtp">Indicates the SMTP of a Security User</param>
        /// <param name="externalEntityId">Indicates the External Entity Id of a Security User</param>
        /// <param name="managerId">Indicates the Manager Id of a Security User</param>
        /// <param name="managerLogin">Indicates the Manager login of a Security User</param>
        /// <param name="firstName">Indicates the First name of a Security User</param>
        /// <param name="lastName">Indicates the Last name of a Security User</param>
        /// <param name="initials">Indicates the Initials of a Security User</param>
        /// <param name="disabled">Indicates the disabled of a Security User</param>
        /// <param name="isWindowsAuthentication">Indicates if its windows authentication or not.</param>
        public SecurityUser(Int32 SecurityUserId, Int32 securityUserTypeId, String securityUserLogin, Int32 securityRoleId, Boolean systemUser,
                            String smtp, Int32 externalEntityId, Int32 managerId, String managerLogin, String firstName,
                            String lastName, String initials, Boolean disabled, Boolean isWindowsAuthentication)
            : base(SecurityUserId, firstName, lastName)
        {
            this._securityRoleId = securityRoleId;
            this._securityUserLogin = securityUserLogin;
            this._firstName = firstName;
            this._lastName = lastName;
            this._systemUser = systemUser;
            this._securityUserTypeId = securityUserTypeId;
            this._smtp = smtp;
            this._externalEntityId = externalEntityId;
            this.ManagerId = managerId;
            this.ManagerLogin = managerLogin;
            this._initials = initials;
            this._disabled = disabled;
            this.IsWindowsAuthentication = isWindowsAuthentication;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "User";
            }
        }

        /// <summary>
        /// Property denoting Security_UserType of current user
        /// </summary>
        [DataMember]
        public Int32 SecurityUserTypeID
        {
            get { return this._securityUserTypeId; }
            set { this._securityUserTypeId = value; }
        }

        /// <summary>
        /// Property denoting Login of current user
        /// </summary>
        [DataMember]
        public String SecurityUserLogin
        {
            get { return this._securityUserLogin; }
            set { this._securityUserLogin = value; }
        }

        /// <summary>
        /// Property denoting security role id
        /// </summary>
        [DataMember]
        public Int32 SecurityRoleId
        {
            get { return this._securityRoleId; }
            set { this._securityRoleId = value; }
        }

        /// <summary>
        /// Property denoting if current user is a SystemUser
        /// </summary>
        [DataMember]
        public Boolean SystemUser
        {
            get { return this._systemUser; }
            set { this._systemUser = value; }
        }

        /// <summary>
        /// Property denoting SMTP address
        /// </summary>
        [DataMember]
        public String Smtp
        {
            get { return this._smtp; }
            set { this._smtp = value; }
        }

        /// <summary>
        /// Property denoting Password of current user
        /// </summary>
        [DataMember]
        public String Password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        /// <summary>
        /// Property denoting ExternalEntityID
        /// </summary>
        [DataMember]
        public Int32 ExternalEntityID
        {
            get { return this._externalEntityId; }
            set { this._externalEntityId = value; }
        }

        /// <summary>
        /// Property denoting manager Id of current user
        /// </summary>
        [DataMember]
        public Int32 ManagerId
        {
            get { return this._managerId; }
            set { this._managerId = value; }
        }

        /// <summary>
        /// Property denoting current user's manager's login
        /// </summary>
        [DataMember]
        public String ManagerLogin
        {
            get { return this._managerLogin; }
            set { this._managerLogin = value; }
        }

        /// <summary>
        /// Property denoting First Name of current user
        /// </summary>
        [DataMember]
        public String FirstName
        {
            get { return this._firstName; }
            set { this._firstName = value; }
        }

        /// <summary>
        /// Property denoting Last Name of current user
        /// </summary>
        [DataMember]
        public String LastName
        {
            get { return this._lastName; }
            set { this._lastName = value; }
        }

        /// <summary>
        /// Property denoting Initials of current user
        /// </summary>
        [DataMember]
        public String Initials
        {
            get { return this._initials; }
            set { this._initials = value; }
        }

        /// <summary>
        /// Property denoting whether current user is Disabled
        /// </summary>
        [DataMember]
        public Boolean Disabled
        {
            get { return this._disabled; }
            set { this._disabled = value; }
        }

        /// <summary>
        /// Property denoting Authentication of User whether it is WindowsAuthentication or not
        /// </summary>
        [DataMember]
        public Boolean IsWindowsAuthentication
        {
            get
            {
                if (this.AuthenticationType == AuthenticationType.Windows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    this.AuthenticationType = AuthenticationType.Windows;
                }
                else
                {
                    this.AuthenticationType = AuthenticationType.Forms;    //It could be claims/Forms. For supporting the backward compatibility making as forms & Here Claims authentication is new and this property has to be removed later.
                }
            }
        }

        /// <summary>
        /// Property denoting the roles which are mapped to the security user.
        /// </summary>
        [DataMember]
        public SecurityRoleCollection Roles
        {
            get { return this._roles; }
            set { this._roles = value; }
        }

        /// <summary>
        /// Represents the type of authentication for the current user
        /// </summary>
        [DataMember]
        public AuthenticationType AuthenticationType
        {
            get { return this._authenticationType; }
            set
            {
                this._authenticationType = value;
            }
        }

        /// <summary>
        /// Represents the whether the user is created by system or not.
        /// </summary>
        [DataMember]
        public Boolean IsSystemCreatedUser
        {
            get { return this._isSystemCreated; }
            set { this._isSystemCreated = value; }
        }

        /// <summary>
        /// Represents the the SAML authentication token.
        /// </summary>
        [DataMember]
        public String AuthenticationToken
        {
            get { return this._authenticationToken; }
            set { this._authenticationToken = value; }
        }

        /// <summary>
        /// Property denoting the original security user
        /// </summary>
        public SecurityUser OriginalSecurityUser
        {
            get
            {
                return _originalSecurityUser;
            }
            set
            {
                this._originalSecurityUser = value;
            }
        }

        /// <summary>
        /// Property denoting the user preference
        /// </summary>
        [DataMember]
        public UserPreferences UserPreference
        {
            get
            {
                return _userPreference;
            }
            set
            {
                this._userPreference = value;
            }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.User;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Xml representation of Security User object
        /// </summary>
        /// <returns>Xml representation of security user object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Role node start
            xmlWriter.WriteStartElement("User");

            xmlWriter.WriteAttributeString("PK_Security_User", this.Id.ToString());
            xmlWriter.WriteAttributeString("UserType", this.SecurityUserTypeID.ToString());
            xmlWriter.WriteAttributeString("Login", this.SecurityUserLogin.ToString());
            xmlWriter.WriteAttributeString("FirstName", this.Name);
            xmlWriter.WriteAttributeString("LastName", this.LongName);
            xmlWriter.WriteAttributeString("Initials", this.Initials.ToString());
            xmlWriter.WriteAttributeString("Disabled", this.Disabled.ToString());
            xmlWriter.WriteAttributeString("ExternalEntityID", this.ExternalEntityID.ToString());
            //xmlWriter.WriteAttributeString("SystemUser", this.SystemUser.ToString());
            xmlWriter.WriteAttributeString("SMTP", this.Smtp.ToString());
            xmlWriter.WriteAttributeString("PK_Security_Role", this.SecurityRoleId.ToString());
            xmlWriter.WriteAttributeString("FK_Security_User_Manager", this.ManagerId.ToString());
            xmlWriter.WriteAttributeString("Manager", this.ManagerLogin.ToString());
            xmlWriter.WriteAttributeString("WindowsAuthentication", this.IsWindowsAuthentication.ToString());
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("AuthenticationType", this.AuthenticationType.ToString());
            xmlWriter.WriteAttributeString("AuthenticationToken", this.AuthenticationToken);
            xmlWriter.WriteAttributeString("IsSystemCreatedUser", this.IsSystemCreatedUser.ToString());

            if (this.SystemUser == true)
            {
                xmlWriter.WriteAttributeString("SystemUser", "Y");
            }
            else
            {
                xmlWriter.WriteAttributeString("SystemUser", "N");
            }

            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            if (this.Roles != null)
            {
                xmlWriter.WriteRaw(this.Roles.ToXml());
            }

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Xml representation of User object
        /// </summary>
        /// <param name="serialization">Type of serialization</param>
        /// <returns>Xml representation of security user object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetObj">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean IsSuperSetOf(Object subsetObj, Boolean compareIds = false)
        {
            if (base.IsSuperSetOf(subsetObj, compareIds))
            {
                if (subsetObj is SecurityUser)
                {
                    SecurityUser objectToBeCompared = subsetObj as SecurityUser;

                    if (compareIds)
                    {
                        if (this.SecurityUserTypeID != objectToBeCompared.SecurityUserTypeID)
                            return false;

                        if (this.SecurityRoleId != objectToBeCompared.SecurityRoleId)
                            return false;

                        if (this.ExternalEntityID != objectToBeCompared.ExternalEntityID)
                            return false;

                        if (this.ManagerId != objectToBeCompared.ManagerId)
                            return false;
                    }

                    if (this.SystemUser != objectToBeCompared.SystemUser)
                        return false;

                    if (this.Smtp != objectToBeCompared.Smtp)
                        return false;

                    if (this.Password != objectToBeCompared.Password)
                        return false;

                    if (this.ManagerLogin != objectToBeCompared.ManagerLogin)
                        return false;

                    if (this.Initials != objectToBeCompared.Initials)
                        return false;

                    if (this.Disabled != objectToBeCompared.Disabled)
                        return false;

                    if (this.IsWindowsAuthentication != objectToBeCompared.IsWindowsAuthentication)
                        return false;

                    if (this.AuthenticationType != objectToBeCompared.AuthenticationType)
                        return false;

                    if (this.IsSystemCreatedUser != objectToBeCompared.IsSystemCreatedUser)
                        return false;

                    if (this.AuthenticationToken != objectToBeCompared.AuthenticationToken)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the mapped roles for the securityUser
        /// </summary>
        /// <returns>Return the collection of security role interface object which are mapped to the security user</returns>
        public ISecurityRoleCollection GetRoles()
        {
            return this._roles;
        }

        /// <summary>
        /// Set the mapped roles to the security user.
        /// </summary>
        /// <param name="roles">Indicates the security role collection.</param>
        public void SetRoles(ISecurityRoleCollection roles)
        {
            if (roles != null)
            {
                this.Roles = (SecurityRoleCollection)roles;
            }
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">EntityType object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SecurityUser)
            {
                SecurityUser objectToBeCompared = obj as SecurityUser;

                if (!base.Equals(objectToBeCompared))
                {
                    return false;
                }

                if (this.Smtp != objectToBeCompared.Smtp)
                {
                    return false;
                }

                if (this.FirstName != objectToBeCompared.FirstName)
                {
                    return false;
                }

                if (this.LastName != objectToBeCompared.LastName)
                {
                    return false;
                }

                if (this.Initials != objectToBeCompared.Initials)
                {
                    return false;
                }

                if (this.Password != objectToBeCompared.Password)
                {
                    return false;
                }

                if (this.AuthenticationType != objectToBeCompared.AuthenticationType)
                {
                    return false;
                }

                // Compare roles collection
                var matchedUsers = from p in this.Roles
                                   join q in objectToBeCompared.Roles
                                   on p.GetHashCode() equals q.GetHashCode()
                                   select p;

                if (matchedUsers.Count() != this.Roles.Count)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Clone security user object
        /// </summary>
        /// <returns>cloned copy of SecurityUser object.</returns>
        public ISecurityUser Clone()
        {
            SecurityUser clonedSecurityUser = new SecurityUser();

            clonedSecurityUser.Id = this.Id;
            clonedSecurityUser.Name = this.Name;
            clonedSecurityUser.LongName = this.LongName;
            clonedSecurityUser.Locale = this.Locale;
            clonedSecurityUser.Action = this.Action;
            clonedSecurityUser.AuditRefId = this.AuditRefId;
            clonedSecurityUser.ExtendedProperties = this.ExtendedProperties;

            clonedSecurityUser.Roles = (SecurityRoleCollection)this.Roles.Clone();
            clonedSecurityUser.SecurityUserTypeID = this.SecurityUserTypeID;
            clonedSecurityUser.SecurityUserLogin = this.SecurityUserLogin;
            clonedSecurityUser.SecurityRoleId = this.SecurityRoleId;
            clonedSecurityUser.SystemUser = this.SystemUser;
            clonedSecurityUser.Smtp = this.Smtp;
            clonedSecurityUser.Password = this.Password;
            clonedSecurityUser.ExternalEntityID = this.ExternalEntityID;
            clonedSecurityUser.ManagerId = this.ManagerId;
            clonedSecurityUser.ManagerLogin = this.ManagerLogin;
            clonedSecurityUser.FirstName = this.FirstName;
            clonedSecurityUser.LastName = this.LastName;
            clonedSecurityUser.Initials = this.Initials;
            clonedSecurityUser.Disabled = this.Disabled;
            clonedSecurityUser.IsWindowsAuthentication = this.IsWindowsAuthentication;
            clonedSecurityUser.AuthenticationType = this.AuthenticationType;
            clonedSecurityUser.IsSystemCreatedUser = this.IsSystemCreatedUser;
            clonedSecurityUser.AuthenticationToken = this.AuthenticationToken;

            return clonedSecurityUser;
        }

        /// <summary>
        /// Delta Merge of security user
        /// </summary>
        /// <param name="deltaSecurityUser">Security user that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged security user instance</returns>
        public ISecurityUser MergeDelta(ISecurityUser deltaSecurityUser, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            ISecurityUser mergedSecurityUser = (returnClonedObject == true) ? deltaSecurityUser.Clone() : deltaSecurityUser;

            mergedSecurityUser.Action = (mergedSecurityUser.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedSecurityUser;
        }

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        /// <summary>
        ///  Serves as a hash function for SecurityUser
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this._authenticationToken.GetHashCode() 
                ^ this._authenticationType.GetHashCode() 
                ^ this._disabled.GetHashCode() 
                ^ this._externalEntityId.GetHashCode()
                ^ this._externalId.GetHashCode() 
                ^ this._firstName.GetHashCode() 
                ^ this._initials.GetHashCode() 
                ^ this._isSystemCreated.GetHashCode() 
                ^ this._lastName.GetHashCode() 
                ^ this._managerId.GetHashCode()
                ^ this._managerLogin.GetHashCode() 
                ^ this._password.GetHashCode() 
                ^ this._roles.GetHashCode() 
                ^ this._securityRoleId.GetHashCode()
                ^ this._securityUserLogin.GetHashCode() 
                ^ this._securityUserTypeId.GetHashCode() 
                ^ this._smtp.GetHashCode() 
                ^ this._systemUser.GetHashCode() 
                ^ this._userPreference.GetHashCode();

            if (this._originalSecurityUser != null)
            {
                hashCode = hashCode ^ this._originalSecurityUser.GetHashCode();
            }

            return hashCode;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Load securityUser from the input xml
        /// </summary>
        /// <param name="valuesAsXml">XML representing the value of SecurityUser</param>
        /// <example>
        /// Sample XML:
        /// <para>
        ///     &lt;User 
        ///         PK_Security_User="1" 
        ///         UserType="1" 
        ///         Login="cfadmin" 
        ///         FirstName="ProductCenter" 
        ///         LastName="SystemAdmin" 
        ///         Initials="PCADMIN" 
        ///         Disabled="False" 
        ///         ExternalEntityID="0" 
        ///         SystemUser="N" 
        ///         SMTP="cfadmin@dm.com" 
        ///         PK_Security_Role="2" 
        ///         Action="Display" /&gt;
        /// </para>
        /// </example>
        private void LoadSecurityUser(String valuesAsXml)
        {
            /*
                * Sample:
                * <User PK_Security_User="1" UserType="1" Login="cfadmin" FirstName="ProductCenter" LastName="SystemAdmin" Initials="PCADMIN" Disabled="False" ExternalEntityID="0" SystemUser="N" SMTP="cfadmin@dm.com" PK_Security_Role="2" Action="Display" />
                */

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.Name == "User")
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("PK_Security_User"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }
                                    else if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("UserType"))
                                    {
                                        this.SecurityUserTypeID = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("Login"))
                                    {
                                        this.SecurityUserLogin = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Password"))
                                    {
                                        this.Password = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("FirstName"))
                                    {
                                        this.FirstName = reader.ReadContentAsString();
                                    }

                                    this.Name = this.FirstName;

                                    if (!String.IsNullOrWhiteSpace(this.Name))
                                        this.NameInLowerCase = this.Name.ToLowerInvariant();

                                    if (reader.MoveToAttribute("LastName"))
                                    {
                                        this.LastName = reader.ReadContentAsString();
                                        this.LongName = this.LastName;
                                    }

                                    if (reader.MoveToAttribute("Disabled"))
                                    {
                                        this.Disabled = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                    }

                                    if (reader.MoveToAttribute("Initials"))
                                    {
                                        this.Initials = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("ExternalEntityID"))
                                    {
                                        this.ExternalEntityID = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("FK_Security_User_Manager"))
                                    {
                                        this.ManagerId = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("Manager"))
                                    {
                                        this.ManagerLogin = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("SystemUser"))
                                    {
                                        String systemUser = reader.ReadContentAsString();
                                        this.SystemUser = systemUser != "N";
                                    }
                                    if (reader.MoveToAttribute("SMTP"))
                                    {
                                        this.Smtp = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("PK_Security_Role"))
                                    {
                                        this.SecurityRoleId = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("WindowsAuthentication"))
                                    {
                                        this.IsWindowsAuthentication = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("AuthenticationType"))
                                    {
                                        Enum.TryParse<AuthenticationType>(reader.ReadContentAsString(), out _authenticationType);

                                        this.AuthenticationType = _authenticationType;
                                    }

                                    if (reader.MoveToAttribute("IsSystemCreatedUser"))
                                    {
                                        this.IsSystemCreatedUser = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("AuthenticationToken"))
                                    {
                                        this.AuthenticationToken = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Action"))
                                    {
                                        this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                    }
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SecurityRoles")
                        {

                            String rolesXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(rolesXml))
                            {
                                SecurityRoleCollection roles = new SecurityRoleCollection(rolesXml);
                                if (roles != null)
                                {
                                    this.Roles = roles;
                                }
                            }
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
        }

        #endregion

        #endregion
    }
}