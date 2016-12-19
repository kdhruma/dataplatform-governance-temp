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
    /// Specifies a SecurityRole
    /// </summary>
    [DataContract]
    public class SecurityRole : MDMObject, ISecurityRole, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting security user type
        /// </summary>
        private SecurityUserType _securityUserType = SecurityUserType.Internal;

        /// <summary>
        /// Field denoting set of permissions for role
        /// </summary>
        private Boolean _permissionSet = false;

        /// <summary>
        /// Field denoting whether current role is a system role
        /// </summary>
        private Boolean _isSystemRole = false;

        /// <summary>
        /// field denoting whether current role is a private role.
        /// </summary>
        private Boolean _isPrivateRole = false;

        /// <summary>
        /// Indicates users under this role
        /// </summary>
        private SecurityUserCollection _users = new SecurityUserCollection();

        /// <summary>
        /// Field Denoting the original security role
        /// </summary>
        private SecurityRole _originalSecurityRole = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting Organization key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// 
        public SecurityRole()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a SecurityRole</param>
        public SecurityRole(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a SecurityRole as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a SecurityRole</param>
        /// <param name="name">Indicates the ShortName of a SecurityRole </param>
        /// <param name="longName">Indicates the LongName of a SecurityRole </param>
        public SecurityRole(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with XML as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML having value for SecurityRole object</param>
        /// <example>
        /// Sample XML
        /// <para>
        ///     &lt;Role 
        ///         ShortName="Administrator" 
        ///         PK_Security_Role="4" 
        ///         UserType="1" 
        ///         LongName="Administrator" 
        ///         SystemRole="N" 
        ///         PrivateRole="N" 
        ///         PermissionSet="N" /&gt;
        /// </para>
        /// </example>
        public SecurityRole(String valuesAsXml)
        {
            /*
             * Sample:
             * <Role ShortName="Administrator" PK_Security_Role="4" UserType="1" LongName="Administrator" SystemRole="N" PrivateRole="N" PermissionSet="N" />
             */

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.Name == "Role")
                        {
                            if (reader.IsStartElement())
                            {
                                #region Read role metadata

                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("ShortName"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }
                                    else if (reader.MoveToAttribute("Name"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("PK_Security_Role"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }
                                    else if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("UserType"))
                                    {
                                        this.SecurityUserType = (SecurityUserType) reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("LongName"))
                                    {
                                        this.LongName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("SystemRole"))
                                    {
                                        this.IsSystemRole = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsSystemRole);
                                    }

                                    if (reader.MoveToAttribute("PrivateRole"))
                                    {
                                        this.IsPrivateRole = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsPrivateRole);
                                    }

                                    if (reader.MoveToAttribute("PermissionSet"))
                                    {
                                        this.PermissionSet = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.PermissionSet);
                                    }

                                    if (reader.MoveToAttribute("Action"))
                                    {
                                        ObjectAction act = ObjectAction.Read;
                                        if (Enum.TryParse(reader.ReadContentAsString(), out act))
                                        {
                                            this.Action = act;
                                        }
                                    }
                                }

                                #endregion Read role metadata
                            }
                        }
                        else if (reader.Name == "Users")
                        {
                            String usersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(usersXml))
                            {
                                SecurityUserCollection users = new SecurityUserCollection(usersXml);
                                if (users != null)
                                {
                                    this.Users = users;
                                }
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

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "Role";
            }
        }

        /// <summary>
        /// Property denoting the SecurityUserTypeID of SecurityRole
        /// </summary>
        [DataMember]
        public SecurityUserType SecurityUserType
        {
            get { return this._securityUserType; }
            set { this._securityUserType = value; }
        }

        /// <summary>
        /// Property denoting the PermissionSet of SecurityRole
        /// </summary>
        [DataMember]
        public new Boolean PermissionSet
        {
            get { return this._permissionSet; }
            set { this._permissionSet = value; }
        }

        /// <summary>
        /// Property denoting whether current SecurityRole is SystemRole 
        /// </summary>
        [DataMember]
        public Boolean IsSystemRole
        {
            get { return this._isSystemRole; }
            set { this._isSystemRole = value; }
        }

        /// <summary>
        /// Property denoting whether current SecurityRole is PrivateRole 
        /// </summary>
        [DataMember]
        public Boolean IsPrivateRole
        {
            get { return this._isPrivateRole; }
            set { this._isPrivateRole = value; }
        }

        /// <summary>
        /// Indicates users under this role
        /// </summary>
        [DataMember]
        public SecurityUserCollection Users
        {
            get { return _users; }
            set { _users = value; }
        }

        /// <summary>
        /// Property denoting the original security role
        /// </summary>
        public SecurityRole OriginalSecurityRole
        {
            get
            {
                return _originalSecurityRole;
            }
            set
            {
                this._originalSecurityRole = value;
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
                return MDM.Core.ObjectType.Role;
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
        /// Determines whether two Object instances are equal.        
        /// </summary>
        /// <param name="subsetSecurityRole">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(SecurityRole subsetSecurityRole, Boolean compareIds = false)
        {
            if (subsetSecurityRole != null)
            {
                if (base.IsSuperSetOf(subsetSecurityRole, compareIds))
                {
                    if (!compareIds)
                    {
                        if (this.SecurityUserType != subsetSecurityRole.SecurityUserType)
                            return false;

                        if (this.ObjectType != subsetSecurityRole.ObjectType)
                            return false;

                        if (this.PermissionSet != subsetSecurityRole.PermissionSet)
                            return false;

                        if (this.IsSystemRole != subsetSecurityRole.IsSystemRole)
                            return false;

                        if (this.IsPrivateRole != subsetSecurityRole.IsPrivateRole)
                            return false;

                        if (!this.Users.IsSuperSetOf(subsetSecurityRole.Users))
                            return false;
                    }

                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Xml representation of Role object
        /// </summary>
        /// <returns>Xml format of role</returns>
        public override String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Role node start
            xmlWriter.WriteStartElement("Role");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("SecurityUserType", this.SecurityUserType.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            // Users start
            xmlWriter.WriteStartElement("Users");

            if (this.Users != null && this.Users.Count > 0)
            {
                foreach (SecurityUser user in this.Users)
                {
                    xmlWriter.WriteStartElement("User");

                    xmlWriter.WriteAttributeString("Id", user.Id.ToString());
                    xmlWriter.WriteAttributeString("FirstName", user.FirstName);
                    xmlWriter.WriteAttributeString("LastName", user.LastName);
                    xmlWriter.WriteAttributeString("Login", user.SecurityUserLogin);

                    xmlWriter.WriteEndElement();
                }
            }

            // Users end
            xmlWriter.WriteEndElement();

            //Role node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Xml representation of Role object
        /// </summary>
        /// <param name="serialization">Type of serialization</param>
        /// <returns>Xml format of role</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Get users for current role
        /// </summary>
        /// <returns>User collection for current role</returns>
        public ISecurityUserCollection GetUsers()
        {
            ISecurityUserCollection users = null;
            if (this.Users != null)
            {
                users = ( ISecurityUserCollection )this.Users;
            }
            return users;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">EntityType object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SecurityRole)
            {
                SecurityRole objectToBeCompared = obj as SecurityRole;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.SecurityUserType != objectToBeCompared.SecurityUserType)
                    return false;

                if (this.IsSystemRole != objectToBeCompared.IsSystemRole)
                    return false;

                if (this.IsPrivateRole != objectToBeCompared.IsPrivateRole)
                    return false;

                // Compare security user collection
                var matchedUsers = from p in this.Users
                                   join q in objectToBeCompared.Users
                                   on p.GetHashCode() equals q.GetHashCode()
                                   select p;

                if (matchedUsers.Count() != this.Users.Count)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Clone security role object
        /// </summary>
        /// <returns>cloned copy of security role object.</returns>
        public ISecurityRole Clone()
        {
            SecurityRole clonedSecurityRole = new SecurityRole();

            clonedSecurityRole.Id = this.Id;
            clonedSecurityRole.Name = this.Name;
            clonedSecurityRole.LongName = this.LongName;
            clonedSecurityRole.Locale = this.Locale;
            clonedSecurityRole.Action = this.Action;
            clonedSecurityRole.AuditRefId = this.AuditRefId;
            clonedSecurityRole.ExtendedProperties = this.ExtendedProperties;

            clonedSecurityRole.Users = (SecurityUserCollection)this.Users.Clone();
            clonedSecurityRole.SecurityUserType = this.SecurityUserType;
            clonedSecurityRole.PermissionSet = this.PermissionSet;
            clonedSecurityRole.IsSystemRole = this.IsSystemRole;
            clonedSecurityRole.IsPrivateRole = this.IsPrivateRole;

            return clonedSecurityRole;
        }

        /// <summary>
        /// Delta Merge of security role
        /// </summary>
        /// <param name="deltaSecurityRole">Security role that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged security role instance</returns>
        public ISecurityRole MergeDelta(ISecurityRole deltaSecurityRole, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            ISecurityRole mergedSecurityRole = (returnClonedObject == true) ? deltaSecurityRole.Clone() : deltaSecurityRole;

            mergedSecurityRole.Action = (mergedSecurityRole.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedSecurityRole;
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
        ///  Serves as a hash function for SecurityRole
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ _externalId.GetHashCode() ^ _isPrivateRole.GetHashCode() ^ _isSystemRole.GetHashCode() 
                ^ _permissionSet.GetHashCode() ^ _securityUserType.GetHashCode() ^ _users.GetHashCode();

            if (_originalSecurityRole != null)
            {
                hashCode = hashCode ^ _originalSecurityRole.GetHashCode();
            }
            return hashCode;
        }
        #endregion

        #region Private Methods
        #endregion

        #endregion Methods
    }
}