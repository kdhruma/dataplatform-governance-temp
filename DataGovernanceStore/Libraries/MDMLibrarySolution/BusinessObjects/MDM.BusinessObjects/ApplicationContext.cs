using System;
using System.Web;
using System.Collections.Specialized;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Context for any data model item. This context helps in fetching appropriate data model details based on the context parameters.
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class ApplicationContext : MDMObject, IApplicationContext, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denoting organization id for the application context
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Field denoting organization name for the application context
        /// </summary>
        private String _organizationName = String.Empty;

        /// <summary>
        /// Field denoting organization long name for the application context
        /// </summary>
        private String _organizationLongName = String.Empty;

        /// <summary>
        /// Field denoting container id for the application context
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting container name for the application context
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field denoting container long name for the application context
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field denoting entity type id for the application context
        /// </summary>
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field denoting entity type name for the application context
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field denoting entity type long name for the application context
        /// </summary>
        private String _entityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting relationship type id for the application context
        /// </summary>
        private Int32 _relationshipTypeId = 0;

        /// <summary>
        /// Field denoting relationship type name for the application context
        /// </summary>
        private String _relationshipTypeName = String.Empty;

        /// <summary>
        /// Field denoting relationship type long name for the application context
        /// </summary>
        private String _relationshipTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting category name for the application context
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting category id for the application context
        /// </summary>
        private String _categoryName = String.Empty;

        /// <summary>
        /// Field denoting category long name for the application context
        /// </summary>
        private String _categoryLongName = String.Empty;

        /// <summary>
        /// Field denoting category path for the application context
        /// </summary>
        private String _categoryPath = String.Empty;

        /// <summary>
        /// Field denoting entity id for the application context
        /// </summary>
        private Int64 _entityId = 0;

        /// <summary>
        /// Field denoting entity name for the application context
        /// </summary>
        private String _entityName = String.Empty;

        /// <summary>
        /// Field denoting entity long name for the application context
        /// </summary>
        private String _entityLongName = String.Empty;

        /// <summary>
        /// Field denoting attribute id for the application context
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        /// Field denoting attribute name for the application context
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field denoting attribute long name for the application context
        /// </summary>
        private String _attributeLongName = String.Empty;

        /// <summary>
        /// Field denoting user id for the application context
        /// </summary>
        private Int32 _userId = 0;

        /// <summary>
        /// Field denoting user name for the application context
        /// </summary>
        private String _userName = String.Empty;

        /// <summary>
        /// Field denoting role id for the application context
        /// </summary>
        private Int32 _roleId = 0;

        /// <summary>
        /// Field denoting role name for the application context
        /// </summary>
        private String _roleName = String.Empty;

        /// <summary>
        /// Field denoting role long name for the application context
        /// </summary>
        private String _roleLongName = String.Empty;

        /// <summary>
        /// Field denoting locale for the application context
        /// </summary>
        private String _locale = String.Empty;

        /// <summary>
        /// Field denoting context type for the application context 
        /// </summary>
        private ApplicationContextType _contextTpe = ApplicationContextType.ACNCO;

        /// <summary>
        /// Field denotes the reference Id
        /// </summary>
        private Int64 _referenceId = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ApplicationContext()
        {

        }

        /// <summary>
        /// Create Application context based on object type Id
        /// </summary>
        /// <param name="objectTypeId">Indicates the object type id</param>
        public ApplicationContext(Int32 objectTypeId)
            : base(objectTypeId, String.Empty)
        {

        }

        /// <summary>
        /// Create Application context based on object type Id , name , name in lower case and long name
        /// </summary>
        /// <param name="objectTypeId">Indicates the object type id</param>
        /// <param name="objectType">Indicates the object Type</param>
        /// <param name="nameInLowerCase">Indicates the Name in lower case</param>
        /// <param name="name">Indicates the Name</param>
        /// <param name="longName">Indicates the LongName of an Object</param>
        public ApplicationContext(Int32 objectTypeId, String objectType, String name, String nameInLowerCase, String longName)
            : base(objectTypeId, objectType, name, nameInLowerCase, longName)
        {

        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        /// <param name="organizationId">OrganizationId for ApplicationContext</param>
        /// <param name="containerId">ContainerId for ApplicationContext</param>
        /// <param name="entityTypeId">EntityTypeId for ApplicationContext</param>
        /// <param name="relationshipTypeId">RelationshipTypeId for ApplicationContext</param>
        /// <param name="categoryId">CategoryId for ApplicationContext</param>
        /// <param name="attributeId">AttributeId for ApplicationContext</param>
        /// <param name="locale">Locale for ApplicationContext</param>
        /// <param name="userId">User for ApplicationContext</param>
        /// <param name="roleId">Role for ApplicationContext</param>
        /// <param name="contextType">Type for ApplicationContext</param>
        /// <param name="entityId">EntityId for ApplicationContext</param>
        public ApplicationContext(Int32 organizationId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Int64 entityId, Int32 attributeId, String locale, Int32 userId, Int32 roleId, ApplicationContextType contextType)
        {
            _organizationId = organizationId;
            _containerId = containerId;
            _entityTypeId = entityTypeId;
            _relationshipTypeId = relationshipTypeId;
            _categoryId = categoryId;
            _entityId = entityId;
            _attributeId = attributeId;
            _locale = locale;
            _userId = userId;
            _roleId = roleId;
            _contextTpe = contextType;
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        /// <param name="organizationId">Indicates organization id for application context</param>
        /// <param name="containerId">Indicates container id for application context</param>
        /// <param name="entityTypeId">Indicates entity type id for application context</param>
        /// <param name="relationshipTypeId">Indicates relationship type id for application context</param>
        /// <param name="categoryId">Indicates category id for application context</param>
        /// <param name="attributeId">Indicates attribute id for application context</param>
        /// <param name="locale">Indicates locale for application context</param>
        /// <param name="userId">Indicates user id for application context</param>
        /// <param name="roleId">Indicates role id for application context</param>
        /// <param name="contextType">Indicates type for application context</param>
        /// <param name="entityId">Indicates entity id for application context</param>
        /// <param name="objectTypeId">Indicates object type id for application context</param>
        /// <param name="objectType">Indicates object type for application context</param>
        public ApplicationContext(Int32 organizationId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Int64 entityId, Int32 attributeId, String locale, Int32 userId, Int32 roleId, ApplicationContextType contextType, Int32 objectTypeId, String objectType)
            : base(objectTypeId, objectType)
        {
            _organizationId = organizationId;
            _containerId = containerId;
            _entityTypeId = entityTypeId;
            _relationshipTypeId = relationshipTypeId;
            _categoryId = categoryId;
            _entityId = entityId;
            _attributeId = attributeId;
            _locale = locale;
            _userId = userId;
            _roleId = roleId;
            _contextTpe = contextType;
        }


        /// <summary>
        /// Constructor with all parameters but context type
        /// </summary>
        /// <param name="organizationId">OrganizationId for ApplicationContext</param>
        /// <param name="containerId">ContainerId for ApplicationContext</param>
        /// <param name="entityTypeId">EntityTypeId for ApplicationContext</param>
        /// <param name="relationshipTypeId">RelationshipTypeId for ApplicationContext</param>
        /// <param name="categoryId">CategoryId for ApplicationContext</param>
        /// <param name="entityId">EntityId for ApplicationContext</param>
        /// <param name="attributeId">AttributeId for ApplicationContext</param>
        /// <param name="locale">Locale for ApplicationContext</param>
        /// <param name="userId">User for ApplicationContext</param>
        /// <param name="roleId">Role for ApplicationContext</param>
        public ApplicationContext(Int32 organizationId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Int64 entityId, Int32 attributeId, String locale, Int32 userId, Int32 roleId)
        {
            _organizationId = organizationId;
            _containerId = containerId;
            _entityTypeId = entityTypeId;
            _relationshipTypeId = relationshipTypeId;
            _categoryId = categoryId;
            _entityId = entityId;
            _attributeId = attributeId;
            _locale = locale;
            _userId = userId;
            _roleId = roleId;
        }

        /// <summary>
        /// Constructor with Values as XML format
        /// </summary>
        /// <param name="valuesAsXml">String value in XMl format which has to be set when object is initialized.</param>
        public ApplicationContext(String valuesAsXml)
        {
            LoadApplicationContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor with Values as XML format
        /// </summary>
        /// <param name="valuesAsXml">String value in XMl format which has to be set when object is initialized.</param>
        /// <param name="objectTypeId">Indicates object type id for application context</param>
        public ApplicationContext(String valuesAsXml, Int32 objectTypeId)
            : base(objectTypeId, String.Empty)
        {
            LoadApplicationContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor with parameters collection
        /// </summary>
        /// <param name="parameters">Parameters collection</param>
        /// <param name="appContextParametersNamesPrefix">Parameters names optional prefix</param>
        public ApplicationContext(NameValueCollection parameters, String appContextParametersNamesPrefix)
        {
            appContextParametersNamesPrefix = appContextParametersNamesPrefix ?? String.Empty;

            _organizationId = TryParseInt32Parameter(parameters, appContextParametersNamesPrefix + "OrganizationId", _organizationId);
            _organizationName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "OrganizationName", _organizationName);
            _containerId = TryParseInt32Parameter(parameters, appContextParametersNamesPrefix + "ContainerId", _containerId);
            _containerName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "ContainerName", _containerName);
            _entityTypeId = TryParseInt32Parameter(parameters, appContextParametersNamesPrefix + "EntityTypeId", _entityTypeId);
            _entityTypeName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "EntityTypeName", _entityTypeName);
            _relationshipTypeId = TryParseInt32Parameter(parameters, appContextParametersNamesPrefix + "RelationshipTypeId", _relationshipTypeId);
            _relationshipTypeName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "RelationshipTypeName", _relationshipTypeName);
            _categoryId = TryParseInt64Parameter(parameters, appContextParametersNamesPrefix + "CategoryId", _categoryId);
            _categoryName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "CategoryName", _categoryName);
            _categoryPath = HttpUtility.UrlDecode(TryParseStringParameter(parameters, appContextParametersNamesPrefix + "CategoryPath", _categoryPath));
            _entityId = TryParseInt64Parameter(parameters, appContextParametersNamesPrefix + "EntityId", _entityId);
            _entityName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "EntityName", _entityName);
            _attributeId = TryParseInt32Parameter(parameters, appContextParametersNamesPrefix + "AttributeId", _attributeId);
            _attributeName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "AttributeName", _attributeName);
            _locale = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "Locale", _locale);
            _userId = TryParseInt32Parameter(parameters, appContextParametersNamesPrefix + "UserId", _userId);
            _userName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "UserName", _userName);
            _roleId = TryParseInt32Parameter(parameters, appContextParametersNamesPrefix + "RoleId", _roleId);
            _roleName = TryParseStringParameter(parameters, appContextParametersNamesPrefix + "RoleName", _roleName);

            String contextType = parameters[appContextParametersNamesPrefix + "ContextType"];
            if (!string.IsNullOrWhiteSpace(contextType))
            {
                ApplicationContextType ct = ApplicationContextType.ACNCO;
                Enum.TryParse(contextType, true, out ct);
                _contextTpe = ct;
            }
        }

        /// <summary>
        /// Construct the Application context using Entity, relationship type and role
        /// </summary>
        /// <param name="entity">Indicates the Entity</param>
        /// <param name="objectTypeId">Indicates the object type Id</param>
        /// <param name="relationTypeId">Indicates the relationship type Id</param>
        /// <param name="userRoleId">Indicates the user role Id</param>
        public ApplicationContext(Entity entity, Int32 objectTypeId, Int32 relationTypeId, Int32 userRoleId)
            : base(objectTypeId, String.Empty)
        {
            if (entity != null)
            {
                _organizationId = entity.OrganizationId;
                _containerId = entity.ContainerId;
                _entityTypeId = entity.EntityTypeId;
                _categoryId = entity.CategoryId;
                _entityId = entity.Id;
                _locale = entity.Locale.ToString();
                _relationshipTypeId = relationTypeId;
                _roleId = userRoleId;

                if (entity.Action == ObjectAction.Create)
                {
                    _referenceId = entity.ReferenceId;
                }
                else
                {
                    _referenceId = entity.Id;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denotes the Reference Id
        /// </summary>
        [DataMember]
        public new Int64 ReferenceId
        {
            get
            {
                return _referenceId;
            }
            set
            {
                _referenceId = value;
            }
        }

        /// <summary>
        /// Property denoting organization id for the application context
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get { return _organizationId; }
            set { _organizationId = value; }
        }

        /// <summary>
        /// Property denoting organization Name for the application context
        /// </summary>
        [DataMember]
        public String OrganizationName
        {
            get { return _organizationName; }
            set { _organizationName = value; }
        }

        /// <summary>
        /// Property denoting organization long Name for the application context
        /// </summary>
        [DataMember]
        public String OrganizationLongName
        {
            get { return _organizationLongName; }
            set { _organizationLongName = value; }
        }

        /// <summary>
        /// Property denoting container id for the application context
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Property denoting container name for the application context
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get { return _containerName; }
            set { _containerName = value; }
        }

        /// <summary>
        /// Property denoting container long name for the application context
        /// </summary>
        [DataMember]
        public String ContainerLongName
        {
            get { return _containerLongName; }
            set { _containerLongName = value; }
        }

        /// <summary>
        /// Property denoting entity type id for the application context
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get { return _entityTypeId; }
            set { _entityTypeId = value; }
        }

        /// <summary>
        /// Property denoting entity type name for the application context
        /// </summary>
        [DataMember]
        public String EntityTypeName
        {
            get { return _entityTypeName; }
            set { _entityTypeName = value; }
        }

        /// <summary>
        /// Property denoting entity type long name for the application context
        /// </summary>
        [DataMember]
        public String EntityTypeLongName
        {
            get { return _entityTypeLongName; }
            set { _entityTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting relation type id for the application context
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get { return _relationshipTypeId; }
            set { _relationshipTypeId = value; }
        }

        /// <summary>
        /// Property denoting relationship type name for the application context
        /// </summary>
        [DataMember]
        public String RelationshipTypeName
        {
            get { return _relationshipTypeName; }
            set { _relationshipTypeName = value; }
        }

        /// <summary>
        /// Property denoting relationship type long name for the application context
        /// </summary>
        [DataMember]
        public String RelationshipTypeLongName
        {
            get { return _relationshipTypeLongName; }
            set { _relationshipTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting category id for the application context
        /// </summary>
        [DataMember]
        public Int64 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// Property denoting category name for the application context
        /// </summary>
        [DataMember]
        public String CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        /// <summary>
        /// Property denoting category long name for the application context
        /// </summary>
        [DataMember]
        public String CategoryLongName
        {
            get { return _categoryLongName; }
            set { _categoryLongName = value; }
        }

        /// <summary>
        /// Property denoting category path for the application context
        /// </summary>
        [DataMember]
        public String CategoryPath
        {
            get { return _categoryPath; }
            set { _categoryPath = value; }
        }

        /// <summary>
        /// Property denoting entity id for the application context
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Property denoting entity name for the application context
        /// </summary>
        [DataMember]
        public String EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        /// <summary>
        /// Property denoting entity long name for the application context
        /// </summary>
        [DataMember]
        public String EntityLongName
        {
            get { return _entityLongName; }
            set { _entityLongName = value; }
        }

        /// <summary>
        /// Property denoting attribute id for the application context
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        /// <summary>
        /// Property denoting attribute name for the application context
        /// </summary>
        [DataMember]
        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        /// <summary>
        /// Property denoting attribute long name for the application context
        /// </summary>
        [DataMember]
        public String AttributeLongName
        {
            get { return _attributeLongName; }
            set { _attributeLongName = value; }
        }

        /// <summary>
        /// Property denoting locale for the application context
        /// </summary>
        [DataMember]
        public new String Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        /// <summary>
        /// Property denoting user for the application context
        /// </summary>
        [DataMember]
        public Int32 UserId
        {
            get { return this._userId; }
            set { this._userId = value; }
        }

        /// <summary>
        /// Property denoting user name for the application context
        /// </summary>
        [DataMember]
        public new String UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }

        /// <summary>
        /// Property denoting role for the application context
        /// </summary>
        [DataMember]
        public Int32 RoleId
        {
            get { return this._roleId; }
            set { this._roleId = value; }
        }

        /// <summary>
        /// Property denoting role name for the application context
        /// </summary>
        [DataMember]
        public String RoleName
        {
            get
            {
                return this._roleName;
            }
            set
            {
                this._roleName = value;
            }
        }

        /// <summary>
        /// Property denoting role long name for the application context
        /// </summary>
        [DataMember]
        public String RoleLongName
        {
            get { return _roleLongName; }
            set { _roleLongName = value; }
        }

        /// <summary>
        /// Property denoting context type for the application context
        /// </summary>
        [DataMember]
        public ApplicationContextType ContextType
        {
            get
            {
                return this._contextTpe;
            }
            set
            {
                this._contextTpe = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ApplicationContext)
            {
                ApplicationContext objectToBeCompared = obj as ApplicationContext;

                if (this.OrganizationId != objectToBeCompared.OrganizationId)
                    return false;

                if (this.ContainerId != objectToBeCompared.ContainerId)
                    return false;

                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                    return false;

                if (this.RelationshipTypeId != objectToBeCompared.RelationshipTypeId)
                    return false;

                if (this.CategoryId != objectToBeCompared.CategoryId)
                    return false;

                if (this.EntityId != objectToBeCompared.EntityId)
                    return false;

                if (this.AttributeId != objectToBeCompared.AttributeId)
                    return false;

                if (this.Locale != objectToBeCompared.Locale)
                    return false;

                if (this.UserId != objectToBeCompared.UserId)
                    return false;

                if (this.RoleId != objectToBeCompared.RoleId)
                    return false;

                if (this.ContextType != objectToBeCompared.ContextType)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode = this.OrganizationId.GetHashCode() ^ this.ContainerId.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.EntityId.GetHashCode() ^ this.AttributeId.GetHashCode() ^ this.Locale.GetHashCode() ^ this.RoleId.GetHashCode() ^ this.UserId.GetHashCode() ^ this.ContextType.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of application context
        /// </summary>
        /// <returns>Xml representation of application context</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            //xmlWriter.WriteStartDocument();

            //Child AttributeModel node start
            xmlWriter.WriteStartElement("ApplicationContext");

            #region write application context

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name.ToString());
            xmlWriter.WriteAttributeString("LongName", this.LongName.ToString());

            xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
            xmlWriter.WriteAttributeString("OrganizationName", this.OrganizationName.ToString());
            xmlWriter.WriteAttributeString("OrganizationLongName", this.OrganizationLongName.ToString());

            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this.ContainerName.ToString());
            xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName.ToString());

            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName.ToString());
            xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName.ToString());

            xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeLongName", this.RelationshipTypeLongName.ToString());

            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("CategoryName", this.CategoryName.ToString());
            xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName.ToString());
            xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath.ToString());

            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("EntityName", this.EntityName.ToString());
            xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName.ToString());

            xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
            xmlWriter.WriteAttributeString("AttributeName", this.AttributeName.ToString());
            xmlWriter.WriteAttributeString("AttributeLongName", this.AttributeLongName.ToString());

            xmlWriter.WriteAttributeString("RoleId", this.RoleId.ToString());
            xmlWriter.WriteAttributeString("RoleName", this.RoleName.ToString());
            xmlWriter.WriteAttributeString("RoleLongName", this.RoleLongName.ToString());

            xmlWriter.WriteAttributeString("Locale", this.Locale);
            xmlWriter.WriteAttributeString("UserId", this.UserId.ToString());

            xmlWriter.WriteAttributeString("ContextType", this.ContextType.ToString());
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());

            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            #endregion write application context

            //ApplicationContext node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of application context
        /// </summary>
        /// <param name="serializationType">Specifies type of serialization expected.</param>
        /// <returns>Xml representation of application context</returns>
        public override String ToXml(ObjectSerialization serializationType)
        {
            return this.ToXml();
        }

        /// <summary>
        /// get event subscriber
        /// </summary>
        /// <returns></returns>
        public string GetEventSubscriberName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a cloned instance of the current applicationcontext object
        /// </summary>
        /// <returns>Cloned instance of the current application context object</returns>
        public ApplicationContext Clone()
        {
            ApplicationContext clonedObject = new ApplicationContext(this.ObjectTypeId, String.Empty, this.Name, this.Name, this.LongName);

            clonedObject.Id = this.Id;
            clonedObject.ReferenceId = this.ReferenceId;

            clonedObject.OrganizationId = this.OrganizationId;
            clonedObject.OrganizationName = this.OrganizationName;
            clonedObject.OrganizationLongName = this.OrganizationLongName;

            clonedObject.ContainerId = this.ContainerId;
            clonedObject.ContainerName = this.ContainerName;
            clonedObject.ContainerLongName = this.ContainerLongName;

            clonedObject.CategoryId = this.CategoryId;
            clonedObject.CategoryName = this.CategoryName;
            clonedObject.CategoryLongName = this.CategoryLongName;
            clonedObject.CategoryPath = this.CategoryPath;

            clonedObject.EntityTypeId = this.EntityTypeId;
            clonedObject.EntityTypeName = this.EntityTypeName;
            clonedObject.EntityTypeLongName = this.EntityTypeLongName;

            clonedObject.EntityId = this.EntityId;
            clonedObject.EntityName = this.EntityName;
            clonedObject.EntityLongName = this.EntityLongName;

            clonedObject.AttributeId = this.AttributeId;
            clonedObject.AttributeName = this.AttributeName;
            clonedObject.AttributeLongName = this.AttributeLongName;

            clonedObject.RelationshipTypeId = this.RelationshipTypeId;
            clonedObject.RelationshipTypeName = this.RelationshipTypeName;
            clonedObject.RelationshipTypeLongName = this.RelationshipTypeLongName;

            clonedObject.UserId = this.UserId;
            clonedObject.UserName = this.UserName;

            clonedObject.RoleId = this.RoleId;
            clonedObject.RoleName = this.RoleName;
            clonedObject.RoleLongName = this.RoleLongName;

            clonedObject.ContextType = this.ContextType;

            return clonedObject;
        }

        /// <summary>
        /// Compares ApplicationContext map object with current ApplicationContext map object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public override Boolean IsSuperSetOf(Object obj, Boolean compareIds = false)
        {
            ApplicationContext applicationContext = obj as ApplicationContext;

            if (compareIds)
            {
                if (this.OrganizationId != applicationContext.OrganizationId)
                {
                    return false;
                }

                if (this.ContainerId != applicationContext.ContainerId)
                {
                    return false;
                }

                if (this.CategoryId != applicationContext.CategoryId)
                {
                    return false;
                }
            }

            if (String.Compare(this.OrganizationName, applicationContext.OrganizationName) != 0)
            {
                return false;
            }

            if (String.Compare(this.ContainerName, applicationContext.ContainerName) != 0)
            {
                return false;
            }

            if (String.Compare(this.CategoryName, applicationContext.CategoryName) != 0)
            {
                return false;
            }

            return true;
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Read the input xml and set the values to application context
        /// </summary>
        /// <param name="valuesAsXml">application context as xml</param>
        private void LoadApplicationContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;

            try
            {
                using (reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContext")
                        {
                            #region Read ApplicationContext Properties

                            if (reader.HasAttributes)
                            {
                                #region Metadata

                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._referenceId);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    this.Locale = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContextType"))
                                {
                                    ApplicationContextType contextType = ApplicationContextType.ACNCO;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out contextType);
                                    this.ContextType = contextType;
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                #endregion Metadata

                                #region Organization

                                if (reader.MoveToAttribute("OrganizationId"))
                                {
                                    this.OrganizationId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("OrganizationName"))
                                {
                                    this.OrganizationName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OrganizationLongName"))
                                {
                                    this.OrganizationLongName = reader.ReadContentAsString();
                                }

                                #endregion Organization

                                #region Container

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ContainerName"))
                                {
                                    this.ContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContainerLongName"))
                                {
                                    this.ContainerLongName = reader.ReadContentAsString();
                                }

                                #endregion Container

                                #region Entity Type

                                if (reader.MoveToAttribute("EntityTypeId"))
                                {
                                    this.EntityTypeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("EntityTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityTypeLongName"))
                                {
                                    this.EntityTypeLongName = reader.ReadContentAsString();
                                }

                                #endregion Entity Type

                                #region Relationship Type

                                if (reader.MoveToAttribute("RelationshipTypeId"))
                                {
                                    this.RelationshipTypeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("RelationshipTypeName"))
                                {
                                    this.RelationshipTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RelationshipTypeLongName"))
                                {
                                    this.RelationshipTypeLongName = reader.ReadContentAsString();
                                }

                                #endregion Relationship Type

                                #region Category

                                if (reader.MoveToAttribute("CategoryId"))
                                {
                                    this.CategoryId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("CategoryName"))
                                {
                                    this.CategoryName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CategoryLongName"))
                                {
                                    this.CategoryLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CategoryPath"))
                                {
                                    this.CategoryPath = reader.ReadContentAsString();
                                }

                                #endregion Category

                                #region Entity

                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    this.EntityId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("EntityName"))
                                {
                                    this.EntityName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityLongName"))
                                {
                                    this.EntityLongName = reader.ReadContentAsString();
                                }

                                #endregion Entity

                                #region Attribute

                                if (reader.MoveToAttribute("AttributeId"))
                                {
                                    this.AttributeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("AttributeName"))
                                {
                                    this.AttributeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeLongName"))
                                {
                                    this.AttributeLongName = reader.ReadContentAsString();
                                }

                                #endregion Attribute

                                #region Role

                                if (reader.MoveToAttribute("RoleId"))
                                {
                                    this.RoleId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("RoleName"))
                                {
                                    this.RoleName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RoleLongName"))
                                {
                                    this.RoleLongName = reader.ReadContentAsString();
                                }

                                #endregion Role

                                #region User

                                if (reader.MoveToAttribute("UserId"))
                                {
                                    this.UserId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("UserName"))
                                {
                                    this.UserName = reader.ReadContentAsString();
                                }

                                #endregion User
                            }

                            #endregion Read ApplicationContext Properties
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parameterName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private Int32 TryParseInt32Parameter(NameValueCollection parameters, String parameterName, Int32 defaultValue)
        {
            return ValueTypeHelper.Int32TryParse(parameters[parameterName], defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parameterName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private Int64 TryParseInt64Parameter(NameValueCollection parameters, String parameterName, Int64 defaultValue)
        {
            return ValueTypeHelper.Int64TryParse(parameters[parameterName], defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parameterName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private String TryParseStringParameter(NameValueCollection parameters, String parameterName, String defaultValue)
        {
            return parameters[parameterName] ?? defaultValue;
        }

        #endregion Private Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current applicationcontext object
        /// </summary>
        /// <returns>Cloned instance of the current applicationcontext object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #endregion Methods
    }
}