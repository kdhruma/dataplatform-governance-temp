using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;    

    /// <summary>
    /// Specifies the User Preferences
    /// </summary>
    [DataContract]
    public class UserPreferences : MDMObject, IUserPreferences, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the collection of user preferences
        /// </summary>
        private UserPreferencesCollection _userPreferencesCollection = new UserPreferencesCollection();

        /// <summary>
        /// Field denoting Login Id
        /// </summary>
        private Int32 _loginId = 0;

        /// <summary>
        /// Field denoting Login Name
        /// </summary>
        private String _loginName = String.Empty;

        /// <summary>
        /// Field denoting type of the User
        /// </summary>
        private String _userType = string.Empty;

        /// <summary>
        /// Field denoting Initials of the user
        /// </summary>
        private String _userInitials = string.Empty;

        /// <summary>
        /// Field denoting default role id of the user
        /// </summary>
        private Int32 _defaultRoleId = 0;

        /// <summary>
        /// Field denoting default Role Name 
        /// </summary>
        private String _defaultRoleName = string.Empty;

        /// <summary>
        /// Field denoting data locale 
        /// </summary>
        private LocaleEnum _dataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting UI locale 
        /// </summary>
        private LocaleEnum _uiLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting culture Name
        /// </summary>
        private String _dataCultureName = String.Empty;

        /// <summary>
        /// Field denoting culture Name
        /// </summary>
        private String _uiCultureName = String.Empty;

        /// <summary>
        /// Field denoting default organization Id
        /// </summary>
        private Int32 _defaultOrgId = 0;

        /// <summary>
        /// Field denoting default organization Name
        /// </summary>
        private String _defaultOrgName = string.Empty;

        /// <summary>
        /// Field denoting default organization Long Name
        /// </summary>
        private String _defaultOrgLongName = string.Empty;

        /// <summary>
        /// Field denoting default container Id
        /// </summary>
        private Int32 _defaultContainerId = 0;

        /// <summary>
        /// Field denoting default Container Name
        /// </summary>
        private String _defaultContainerName = string.Empty;

        /// <summary>
        /// Field denoting default Container long Name
        /// </summary>
        private String _defaultContainerLongName = string.Empty;

        /// <summary>
        /// Field denoting default organization Id
        /// </summary>
        private Int32 _defaultDraftContainerId = 0;

        /// <summary>
        /// Field denoting default Hierarchy Id
        /// </summary>
        private Int32 _defaultHierarchyId = 0;
        
        /// <summary>
        /// Field denoting default Hierarchy Name
        /// </summary>
        private String _defaultHierarchyName = string.Empty;
        
        /// <summary>
        /// Field denoting default Hierarchy Long Name
        /// </summary>
        private String _defaultHierarchyLongName = string.Empty;

        /// <summary>
        /// Field denoting default draft Hierarchy Id
        /// </summary>
        private Int32 _defaultDraftHierarchyId = 0;

        /// <summary>
        /// Field denoting default view Id
        /// </summary>
        private Int32 _defaultViewId = 0;

        /// <summary>
        /// Field denoting External Entity Id
        /// </summary>
        private Int32 _externalEntityId = 0;

        /// <summary>
        /// Field denoting maximum rows of the table
        /// </summary>
        private Int32 _maxTableRows = 0;

        /// <summary>
        /// Field denoting maximum pages of the table
        /// </summary>
        private Int32 _maxTablePages = 0;

        /// <summary>
        /// Field denoting default time zone Id
        /// </summary>
        private Int32 _defaultTimeZoneId = 0;

        /// <summary>
        /// Field denoting short name of the default time zone
        /// </summary>
        private String _defaultTimeZoneShortName = String.Empty;

        /// <summary>
        /// Field Denoting the original user preferences
        /// </summary>
        private UserPreferences _originalUserPreferences = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting user preference key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors
            
         /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public UserPreferences()
            : base()
        {
        }

        /// <summary>
        /// Create UserPreferences object with property values xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML representation for UserPreferences from which object is to be created</param>
        public UserPreferences(String valuesAsXml)
        {
            LoadUserPreferences(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the user preferences of a colelction
        /// </summary>
        [DataMember]
        public UserPreferencesCollection UserPreferencesCollection
        {
            get
            {
                return this._userPreferencesCollection;
            }
            set
            {
                this._userPreferencesCollection = value;
            }
        }

        /// <summary>
        /// Property denoting login name of the User
        /// </summary>
        [DataMember]
        public String LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }

        /// <summary>
        /// Property denoting login Identity of the User
        /// </summary>
        [DataMember]
        public Int32 LoginId
        {
            get { return _loginId; }
            set { _loginId = value;}
        }

        /// <summary>
        /// Property denoting default role Identity of the User
        /// </summary>
        [DataMember]
        public Int32 DefaultRoleId
        {
            get { return _defaultRoleId; }
            set { _defaultRoleId = value; }
        }

        /// <summary>
        /// Property denoting default role name of the User
        /// </summary>
        [DataMember]
        public String DefaultRoleName
        {
            get { return _defaultRoleName; }
            set { _defaultRoleName = value; }
        }

        /// <summary>
        /// Property denoting Type of the user
        /// </summary>
        [DataMember]
        public String UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        /// <summary>
        /// Property denoting Initials of the user
        /// </summary>
        [DataMember]
        public String UserInitials
        {
            get { return _userInitials; }
            set { _userInitials = value; }
        }

        /// <summary>
        /// Property denoting data locale Identity of the user
        /// </summary>
        [DataMember]
        public LocaleEnum DataLocale
        {
            get { return _dataLocale; }
            set { _dataLocale = value; }
        }
        
        /// <summary>
        /// Property denoting UI locale Identity of the user
        /// </summary>
        [DataMember]
        public LocaleEnum UILocale
        {
            get { return _uiLocale; }
            set { _uiLocale = value; }
        }

        /// <summary>
        /// Property denoting Culture name of the User 
        /// </summary>
        [DataMember]
        public String DataCultureName
        {
            get { return _dataCultureName; }
            set { _dataCultureName = value; }
        }

        /// <summary>
        /// Property denoting Culture name of the User 
        /// </summary>
        [DataMember]
        public String UICultureName
        {
            get { return _uiCultureName; }
            set { _uiCultureName = value; }
        }

        /// <summary>
        /// Property denoting default organization Identity of the User 
        /// </summary>
        [DataMember]
        public Int32 DefaultOrgId
        {
            get { return _defaultOrgId; }
            set { _defaultOrgId = value; }
        }

        /// <summary>
        /// Property denoting default organization short name of the User 
        /// </summary>
        [DataMember]
        public String DefaultOrgName
        {
            get { return _defaultOrgName; }
            set { _defaultOrgName = value; }
        }

        /// <summary>
        /// Property denoting default organization Long name of the User 
        /// </summary>
        [DataMember]
        public String DefaultOrgLongName
        {
            get { return _defaultOrgLongName; }
            set { _defaultOrgLongName = value; }
        }

        /// <summary>
        /// Property denoting default Container Identity of the User 
        /// </summary>
        [DataMember]
        public Int32 DefaultContainerId
        {
            get { return _defaultContainerId; }
            set { _defaultContainerId = value; }
        }

        /// <summary>
        /// Property denoting default Container short name of the User 
        /// </summary>
        [DataMember]
        public String DefaultContainerName
        {
            get { return _defaultContainerName; }
            set { _defaultContainerName = value; }
        }

        /// <summary>
        /// Property denoting default container long name of the User 
        /// </summary>
        [DataMember]
        public String DefaultContainerLongName
        {
            get { return _defaultContainerLongName; }
            set { _defaultContainerLongName = value; }
        }

        /// <summary>
        /// Property denoting default draft container Identity of the User 
        /// </summary>
        [DataMember]
        public Int32 DefaultDraftContainerId
        {
            get { return _defaultDraftContainerId; }
            set { _defaultDraftContainerId = value; }
        }

        /// <summary>
        /// Property denoting default hierarchy Identity of the User 
        /// </summary>
        [DataMember]
        public Int32 DefaultHierarchyId
        {
            get { return _defaultHierarchyId; }
            set { _defaultHierarchyId = value; }
        }

        /// <summary>
        /// Property denoting default hierarchy short name of the User 
        /// </summary>
        [DataMember]
        public String DefaultHierarchyName
        {
            get { return _defaultHierarchyName; }
            set { _defaultHierarchyName = value; }
        }

        /// <summary>
        /// Property denoting default hierarchy long name of the User 
        /// </summary>
        [DataMember]
        public String DefaultHierarchyLongName
        {
            get { return _defaultHierarchyLongName; }
            set { _defaultHierarchyLongName = value; }
        }

        /// <summary>
        /// Property denoting default draft hierarchy Identity of the User 
        /// </summary>
        [DataMember]
        public Int32 DefaultDraftHierarchyId
        {
            get { return _defaultDraftHierarchyId; }
            set { _defaultDraftHierarchyId = value; }
        }

        /// <summary>
        /// Property denoting default view Identity of the User 
        /// </summary>
        [DataMember]
        public Int32 DefaultViewId
        {
            get { return _defaultViewId; }
            set { _defaultViewId = value; }
        }

        /// <summary>
        /// Property denoting External Entity Identity of the User 
        /// </summary>
        [DataMember]
        public Int32 ExternalEntityId
        {
            get { return _externalEntityId; }
            set { _externalEntityId = value; }
        }

        /// <summary>
        /// Property denoting maximum rows on the table of the User
        /// </summary>
        [DataMember]
        public Int32 MaxTableRows
        {
            get { return _maxTableRows; }
            set { _maxTableRows = value; }
        }

        /// <summary>
        /// Property denoting maximum pages on the table of the User
        /// </summary>
        [DataMember]
        public Int32 MaxTablePages
        {
            get { return _maxTablePages; }
            set { _maxTablePages = value; }
        }

        /// <summary>
        /// Property denoting default time zone Identity of the User
        /// </summary>
        [DataMember]
        public Int32 DefaultTimeZoneId
        {
            get { return _defaultTimeZoneId; }
            set { _defaultTimeZoneId = value; }
        }

        /// <summary>
        /// Property denoting default time zone's short name of the User
        /// </summary>
        [DataMember]
        public String DefaultTimeZoneShortName
        {
            get { return _defaultTimeZoneShortName; }
            set { _defaultTimeZoneShortName = value; }
        }

        /// <summary>
        /// Property denoting the original user preference
        /// </summary>
        public UserPreferences OriginalUserPreferences
        {
            get
            {
                return _originalUserPreferences;
            }
            set
            {
                this._originalUserPreferences = value;
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
                return MDM.Core.ObjectType.UserPreferences;
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
        /// Get Xml representation of UserPreferences object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override string ToXml()
        {
            String userPreferencesXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("UserPreferences");

            #region Write UserPreferences properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("LoginName", this.LoginName);
            xmlWriter.WriteAttributeString("LoginId", this.LoginId.ToString());
            xmlWriter.WriteAttributeString("DefaultRoleId", this.DefaultRoleId.ToString());
            xmlWriter.WriteAttributeString("DefaultRoleName", this.DefaultRoleName);
            xmlWriter.WriteAttributeString("UserType", this.UserType);
            xmlWriter.WriteAttributeString("UserInitials", this.UserInitials);
            xmlWriter.WriteAttributeString("DataLocale", this.DataLocale.ToString());
            xmlWriter.WriteAttributeString("UILocale", this.UILocale.ToString());
            xmlWriter.WriteAttributeString("UICultureName", this.UICultureName);
            xmlWriter.WriteAttributeString("DataCultureName", this.DataCultureName);
            xmlWriter.WriteAttributeString("DefaultOrgId", this.DefaultOrgId.ToString());
            xmlWriter.WriteAttributeString("DefaultOrgLongName", this.DefaultOrgLongName);
            xmlWriter.WriteAttributeString("DefaultOrgName", this.DefaultOrgName);
            xmlWriter.WriteAttributeString("DefaultContainerId", this.DefaultContainerId.ToString());
            xmlWriter.WriteAttributeString("DefaultContainerName", this.DefaultContainerName);
            xmlWriter.WriteAttributeString("DefaultContainerLongName", this.DefaultContainerLongName);
            xmlWriter.WriteAttributeString("DefaultDraftContainerId", this.DefaultDraftContainerId.ToString());
            xmlWriter.WriteAttributeString("DefaultHierarchyId", this.DefaultHierarchyId.ToString());
            xmlWriter.WriteAttributeString("DefaultHierarchyName", this.DefaultHierarchyName);
            xmlWriter.WriteAttributeString("DefaultHierarchyLongName", this.DefaultHierarchyLongName);
            xmlWriter.WriteAttributeString("DefaultDraftHierarchyId", this.DefaultDraftHierarchyId.ToString());
            xmlWriter.WriteAttributeString("DefaultViewId", this.DefaultViewId.ToString());
            xmlWriter.WriteAttributeString("ExternalEntityId", this.ExternalEntityId.ToString());
            xmlWriter.WriteAttributeString("MaxTableRows", this.MaxTableRows.ToString());
            xmlWriter.WriteAttributeString("MaxTablePages", this.MaxTablePages.ToString());
            xmlWriter.WriteAttributeString("DefaultTimeZoneId", this.DefaultTimeZoneId.ToString());
            xmlWriter.WriteAttributeString("DefaultTimeZoneShortName", this.DefaultTimeZoneShortName);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            #endregion

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            userPreferencesXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return userPreferencesXml;
        }

        /// <summary>
        /// Clone UserPreferences object
        /// </summary>
        /// <returns>Cloned copy of UserPreferences object.</returns>
        public IUserPreferences Clone()
        {
            UserPreferences clonedUserPreferences = new UserPreferences();

            clonedUserPreferences.Id = this.Id;
            clonedUserPreferences.Name = this.Name;
            clonedUserPreferences.LongName = this.LongName;
            clonedUserPreferences.Locale = this.Locale;
            clonedUserPreferences.Action = this.Action;
            clonedUserPreferences.AuditRefId = this.AuditRefId;
            clonedUserPreferences.ExtendedProperties = this.ExtendedProperties;

            clonedUserPreferences.UserPreferencesCollection = (UserPreferencesCollection)this.UserPreferencesCollection.Clone();
            clonedUserPreferences.LoginName = this.LoginName;
            clonedUserPreferences.LoginId = this.LoginId;
            clonedUserPreferences.DefaultRoleId = this.DefaultRoleId;
            clonedUserPreferences.DefaultRoleName = this.DefaultRoleName;
            clonedUserPreferences.UserType = this.UserType;
            clonedUserPreferences.UserInitials = this.UserInitials;
            clonedUserPreferences.DataLocale = this.DataLocale;
            clonedUserPreferences.UILocale = this.UILocale;
            clonedUserPreferences.DataCultureName = this.DataCultureName;
            clonedUserPreferences.UICultureName = this.UICultureName;
            clonedUserPreferences.DefaultOrgId = this.DefaultOrgId;
            clonedUserPreferences.DefaultOrgName = this.DefaultOrgName;
            clonedUserPreferences.DefaultOrgLongName = this.DefaultOrgLongName;
            clonedUserPreferences.DefaultContainerId = this.DefaultContainerId;
            clonedUserPreferences.DefaultContainerName = this.DefaultContainerName;
            clonedUserPreferences.DefaultContainerLongName = this.DefaultContainerLongName;
            clonedUserPreferences.DefaultDraftContainerId = this.DefaultDraftContainerId;
            clonedUserPreferences.DefaultHierarchyId = this.DefaultHierarchyId;
            clonedUserPreferences.DefaultHierarchyName = this.DefaultHierarchyName;
            clonedUserPreferences.DefaultHierarchyLongName = this.DefaultHierarchyLongName;
            clonedUserPreferences.DefaultDraftHierarchyId = this.DefaultDraftHierarchyId;
            clonedUserPreferences.DefaultViewId = this.DefaultViewId;
            clonedUserPreferences.ExternalEntityId = this.ExternalEntityId;
            clonedUserPreferences.MaxTableRows = this.MaxTableRows;
            clonedUserPreferences.MaxTablePages = this.MaxTablePages;
            clonedUserPreferences.DefaultTimeZoneId = this.DefaultTimeZoneId;
            clonedUserPreferences.DefaultTimeZoneShortName = this.DefaultTimeZoneShortName;
        
            return clonedUserPreferences;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetUserPreferences">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(UserPreferences subsetUserPreferences, Boolean compareIds = false)
        {
            if (subsetUserPreferences != null)
            {
                if (base.IsSuperSetOf(subsetUserPreferences, compareIds))
                {

                    if (compareIds)
                    {
                        if (this.LoginId != subsetUserPreferences.LoginId)
                            return false;

                        if (this.DefaultRoleId != subsetUserPreferences.DefaultRoleId)
                            return false;

                        if (this.DefaultOrgId != subsetUserPreferences.DefaultOrgId)
                            return false;

                        if (this.DefaultContainerId != subsetUserPreferences.DefaultContainerId)
                            return false;

                        if (this.DefaultDraftContainerId != subsetUserPreferences.DefaultDraftContainerId)
                            return false;

                        if (this.DefaultHierarchyId != subsetUserPreferences.DefaultHierarchyId)
                            return false;

                        if (this.DefaultDraftHierarchyId != subsetUserPreferences.DefaultDraftHierarchyId)
                            return false;

                        if (this.DefaultViewId != subsetUserPreferences.DefaultViewId)
                            return false;

                        if (this.ExternalEntityId != subsetUserPreferences.ExternalEntityId)
                            return false;

                        if (this.DefaultTimeZoneId != subsetUserPreferences.DefaultTimeZoneId)
                            return false;
                    }

                    if (this.LoginName != subsetUserPreferences.LoginName)
                        return false;

                    if (this.DefaultRoleName != subsetUserPreferences.DefaultRoleName)
                        return false;

                    if (this.UserType != subsetUserPreferences.UserType)
                        return false;

                    if (this.UserInitials != subsetUserPreferences.UserInitials)
                        return false;

                    if (this.DataLocale != subsetUserPreferences.DataLocale)
                        return false;

                    if (this.UILocale != subsetUserPreferences.UILocale)
                        return false;

                    if (this.DataCultureName != subsetUserPreferences.DataCultureName)
                        return false;

                    if (this.UICultureName != subsetUserPreferences.UICultureName)
                        return false;

                    if (this.DefaultOrgName != subsetUserPreferences.DefaultOrgName)
                        return false;

                    if (this.DefaultOrgLongName != subsetUserPreferences.DefaultOrgLongName)
                        return false;

                    if (this.DefaultContainerName != subsetUserPreferences.DefaultContainerName)
                        return false;

                    if (this.DefaultContainerLongName != subsetUserPreferences.DefaultContainerLongName)
                        return false;

                    if (this.DefaultHierarchyName != subsetUserPreferences.DefaultHierarchyName)
                        return false;

                    if (this.DefaultHierarchyLongName != subsetUserPreferences.DefaultHierarchyLongName)
                        return false;

                    if (this.MaxTableRows != subsetUserPreferences.MaxTableRows)
                        return false;

                    if (this.MaxTablePages != subsetUserPreferences.MaxTablePages)
                        return false;

                    if (this.DefaultTimeZoneShortName != subsetUserPreferences.DefaultTimeZoneShortName)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Delta Merge of user preferences
        /// </summary>
        /// <param name="deltaUserPreferences">User Preferences that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged user preferences instance</returns>
        public IUserPreferences MergeDelta(IUserPreferences deltaUserPreferences, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IUserPreferences mergedUserPreferences = (returnClonedObject == true) ? deltaUserPreferences.Clone() : deltaUserPreferences;

            mergedUserPreferences.Action = (mergedUserPreferences.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedUserPreferences;
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

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">UserPreferences object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is UserPreferences)
            {
                UserPreferences objectToBeCompared = obj as UserPreferences;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.LoginName != objectToBeCompared.LoginName)
                    return false;

                if (this.LoginId != objectToBeCompared.LoginId)
                    return false;

                if (this.DefaultRoleId != objectToBeCompared.DefaultRoleId)
                    return false;

                if (this.DefaultRoleName != objectToBeCompared.DefaultRoleName)
                    return false;

                if (this.UserType != objectToBeCompared.UserType)
                    return false;

                if (this.UserInitials != objectToBeCompared.UserInitials)
                    return false;

                if (this.DataLocale != objectToBeCompared.DataLocale)
                    return false;

                if (this.UILocale != objectToBeCompared.UILocale)
                    return false;

                if (this.DataCultureName != objectToBeCompared.DataCultureName)
                    return false;

                if (this.UICultureName != objectToBeCompared.UICultureName)
                    return false;

                if (this.DefaultOrgId != objectToBeCompared.DefaultOrgId)
                    return false;

                if (this.DefaultOrgName != objectToBeCompared.DefaultOrgName)
                    return false;

                if (this.DefaultOrgLongName != objectToBeCompared.DefaultOrgLongName)
                    return false;

                if (this.DefaultContainerId != objectToBeCompared.DefaultContainerId)
                    return false;

                if (this.DefaultContainerName != objectToBeCompared.DefaultContainerName)
                    return false;

                if (this.DefaultContainerLongName != objectToBeCompared.DefaultContainerLongName)
                    return false;

                if (this.DefaultDraftContainerId != objectToBeCompared.DefaultDraftContainerId)
                    return false;

                if (this.DefaultHierarchyId != objectToBeCompared.DefaultHierarchyId)
                    return false;

                if (this.DefaultHierarchyName != objectToBeCompared.DefaultHierarchyName)
                    return false;

                if (this.DefaultHierarchyLongName != objectToBeCompared.DefaultHierarchyLongName)
                    return false;

                if (this.DefaultDraftHierarchyId != objectToBeCompared.DefaultDraftHierarchyId)
                    return false;

                if (this.DefaultViewId != objectToBeCompared.DefaultViewId)
                    return false;

                if (this.ExternalEntityId != objectToBeCompared.ExternalEntityId)
                    return false;

                if (this.MaxTableRows != objectToBeCompared.MaxTableRows)
                    return false;

                if (this.MaxTablePages != objectToBeCompared.MaxTablePages)
                    return false;

                if (this.DefaultTimeZoneId != objectToBeCompared.DefaultTimeZoneId)
                    return false;

                if (this.DefaultTimeZoneShortName != objectToBeCompared.DefaultTimeZoneShortName)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode() ^ this.LoginName.GetHashCode() ^ this.LoginId.GetHashCode() ^ this.DefaultRoleId.GetHashCode() ^ this.DefaultRoleName.GetHashCode() ^ this.UserType.GetHashCode() ^ this.UserInitials.GetHashCode() ^ this.DataLocale.GetHashCode() ^ this.UILocale.GetHashCode() ^ this.DataCultureName.GetHashCode() ^ this.UICultureName.GetHashCode() ^ this.DefaultOrgId.GetHashCode() ^ this.DefaultOrgName.GetHashCode() ^ this.DefaultOrgLongName.GetHashCode() ^ this.DefaultContainerId.GetHashCode() ^ this.DefaultContainerName.GetHashCode() ^ this.DefaultContainerLongName.GetHashCode() ^ this.DefaultDraftContainerId.GetHashCode() ^ this.DefaultHierarchyId.GetHashCode() ^ this.DefaultHierarchyName.GetHashCode() ^ this.DefaultHierarchyLongName.GetHashCode() ^ this.DefaultDraftHierarchyId.GetHashCode() ^ this.DefaultViewId.GetHashCode() ^ this.ExternalEntityId.GetHashCode() ^ this.MaxTableRows.GetHashCode() ^ this.MaxTablePages.GetHashCode() ^ this.DefaultTimeZoneId.GetHashCode() ^ this.DefaultTimeZoneShortName.GetHashCode();

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize UserPreferences object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for User Preferences object</param>
        private void LoadUserPreferences(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "UserPreferences")
                    {
                        #region Read entity type properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                this.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LongName"))
                            {
                                this.LongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LoginName"))
                            {
                                this.LoginName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LoginId"))
                            {
                                this.LoginId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultRoleId"))
                            {
                                this.DefaultRoleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultRoleName"))
                            {
                                this.DefaultRoleName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("UserType"))
                            {
                                this.UserType = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("UserInitials"))
                            {
                                this.UserInitials = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DataLocale"))
                            {
                                String locale = reader.ReadContentAsString();
                                this.DataLocale = (LocaleEnum)Enum.Parse(typeof(LocaleEnum), locale);
                            }

                            if (reader.MoveToAttribute("UILocale"))
                            {
                                String locale = reader.ReadContentAsString();
                                this.UILocale = (LocaleEnum)Enum.Parse(typeof(LocaleEnum), locale);
                            }

                            if (reader.MoveToAttribute("DataCultureName"))
                            {
                                this.DataCultureName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("UICultureName"))
                            {
                                this.UICultureName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DefaultOrgId"))
                            {
                                this.DefaultOrgId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultOrgName"))
                            {
                                this.DefaultOrgName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DefaultOrgLongName"))
                            {
                                this.DefaultOrgLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DefaultContainerId"))
                            {
                                this.DefaultContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultContainerName"))
                            {
                                this.DefaultContainerName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DefaultContainerLongName"))
                            {
                                this.DefaultContainerLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DefaultDraftContainerId"))
                            {
                                this.DefaultDraftContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultHierarchyId"))
                            {
                                this.DefaultHierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultHierarchyName"))
                            {
                                this.DefaultHierarchyName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DefaultHierarchyLongName"))
                            {
                                this.DefaultHierarchyLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("DefaultDraftHierarchyId"))
                            {
                                this.DefaultDraftHierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultViewId"))
                            {
                                this.DefaultViewId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("ExternalEntityId"))
                            {
                                this.ExternalEntityId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("MaxTableRows"))
                            {
                                this.MaxTableRows = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("MaxTablePages"))
                            {
                                this.MaxTablePages = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultTimeZoneId"))
                            {
                                this.DefaultTimeZoneId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("DefaultTimeZoneShortName"))
                            {
                                this.DefaultTimeZoneShortName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                            }
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

        #endregion

        #endregion
    }
}
