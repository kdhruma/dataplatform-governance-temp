using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.Web;
using System.Collections;

using RS.MDM.Data;
using RS.MDM.ComponentModel;
using MDM.Interfaces;
using MDM.BusinessObjects;
//using MDM.AdminManager.Business;
using MDM.Core;

namespace RS.MDM.Configuration
{
    /// <summary>
    /// Defines the application context for a particular configuration
    /// </summary>
    public class ApplicationContextInternal : RS.MDM.Data.DataSource, RS.MDM.ComponentModel.IDictionaryProperties, IApplicationContext
    {
        #region Fields

        /// <summary>
        /// Dictionary of organizations with Org Id as key and Org LongName as value
        /// </summary>
        internal static Dictionary<int, string> _orgDictionary = null;

        /// <summary>
        /// Dictionary of entity types with EntityType Id as key and EntityType LongName as value
        /// </summary>
        private static Dictionary<int, string> _entityTypeDictionary = null;

        /// <summary>
        /// Dictionary of relationship types with RelationshipType Id as key and RelationshipType LongName as value
        /// </summary>
        private static Dictionary<int, string> _relationshipTypeDictionary = null;

        /// <summary>
        /// Dictionary of attributes with attribute Id as key and attribute LongName as value
        /// </summary>
        private static Dictionary<int, string> _attributeDictionary = null;

        /// <summary>
        /// Dictionary of Locales with Locale Id as key and Locale LongName as value
        /// </summary>
        private static Dictionary<int, string> _localeDictionary = null;

        /// <summary>
        /// Dictionary of Roles with Role Id as key and Role LongName as value
        /// </summary>
        private static Dictionary<int, string> _roleDictionary = null;

        /// <summary>
        /// Dictionary of Users with User Id as key and User LongName as value
        /// </summary>
        private static Dictionary<int, string> _userDictionary = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ApplicationContextInternal()
            : base()
        {
            //GetApplicationConfigXML(
            //    SqlInt32 FK_Event_Source, 
            //    SqlInt32 FK_Event_Subscriber, 
            //    SqlInt32 FK_Security_Role, 
            //    SqlInt32 FK_Security_user, 
            //    SqlInt32 FK_Org, 
            //    SqlInt32 FK_Catalog, 
            //    SqlInt32 FK_Category, 
            //    SqlInt32 FK_CNode, 
            //    SqlInt32 FK_Attribute, 
            //    SqlInt32 FK_NodeType, 
            //    SqlInt32 FK_RelationshipType,
            //    SqlInt32 PK_Application_Config)

            this.Assembly = "StoredProcedures.dll";
            this.TypeName = "Riversand.StoredProcedures.Events";
            this.MethodName = "GetApplicationConfigXML";
            this.MethodType = System.Reflection.BindingFlags.Static;

            this.RemoveVerb("Add Parameter");


        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="eventSourceId">Indicates an id of an event source</param>
        /// <param name="eventSubscriberId">Indicates an id of an event subscriber</param>
        /// <param name="securityRoleId">Indicates an id of the security role</param>
        /// <param name="securityUserId">Indicates an id of the security user</param>
        /// <param name="orgId">Indicates an id of an organization</param>
        /// <param name="catalogId">Indicates an id of a catalog</param>
        /// <param name="categoryId">Indicates an id of a category</param>
        /// <param name="cNodeId">Indicates an id of a cnode</param>
        /// <param name="attributeId">Indicates an id of an attribute</param>
        /// <param name="nodeTypeId">Indicates an id of a node type</param>
        /// <param name="relationshipTypeId">Indicates an id of relationship type</param>
        /// <param name="applicationConfigId">Indicates an id of application configuration needed, if available</param>
        public ApplicationContextInternal(int eventSourceId, int eventSubscriberId, int securityRoleId, int securityUserId, int orgId, int catalogId, Int64 categoryId, Int64 cNodeId, int attributeId, int nodeTypeId, int relationshipTypeId, int localeId, int applicationConfigId, String categoryPath, String lookupName)
            : base()
        {
            //GetApplicationConfigXML(
            //    SqlInt32 FK_Event_Source, 
            //    SqlInt32 FK_Event_Subscriber, 
            //    SqlInt32 FK_Security_Role, 
            //    SqlInt32 FK_Security_user, 
            //    SqlInt32 FK_Org, 
            //    SqlInt32 FK_Catalog, 
            //    SqlInt32 FK_Category, 
            //    SqlInt32 FK_CNode, 
            //    SqlInt32 FK_Attribute, 
            //    SqlInt32 FK_NodeType, 
            //    SqlInt32 FK_RelationshipType,
            //    SqlInt32 PK_Application_Config)

            this.Assembly = "StoredProcedures.dll";
            this.TypeName = "Riversand.StoredProcedures.Events";
            this.MethodName = "GetApplicationConfigXML";
            this.MethodType = System.Reflection.BindingFlags.Static;

            RS.MDM.Data.Parameter _parameter = null;

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Source";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "EventSourceId";
            _parameter.DefaultValue = eventSourceId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Subscriber";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "EventSubscriberId";
            _parameter.DefaultValue = eventSubscriberId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_Role";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "SecurityRoleId";
            _parameter.DataType = SqlType.SqlInt32;
            _parameter.DefaultValue = securityRoleId.ToString();
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_user";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "SecurityUserId";
            _parameter.DefaultValue = securityUserId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Org";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "OrgId";
            _parameter.DefaultValue = orgId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Catalog";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "CatalogId";
            _parameter.DefaultValue = catalogId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Category";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "CategoryId";
            _parameter.DefaultValue = categoryId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_CNode";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "CNodeId";
            _parameter.DefaultValue = cNodeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Attribute";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "AttributeId";
            _parameter.DefaultValue = attributeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_NodeType";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "NodeTypeId";
            _parameter.DefaultValue = nodeTypeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_RelationshipType";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "RelationshipTypeId";
            _parameter.DefaultValue = relationshipTypeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "PK_Application_Config";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "ApplicationConfigID";
            _parameter.DefaultValue = applicationConfigId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Locale";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "LocaleID";
            _parameter.DefaultValue = localeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "CategoryPath";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "CategoryPath";
            _parameter.DefaultValue = categoryPath;
            _parameter.DataType = SqlType.SqlString;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "LookupName";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "LookupName";
            _parameter.DefaultValue = lookupName;
            _parameter.DataType = SqlType.SqlString;
            this.Parameters.Add(_parameter);
            this.RemoveVerb("Add Parameter");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an Event Source
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        public RS.MDM.Events.EventSourceList EventSource
        {
            get
            {
                return (RS.MDM.Events.EventSourceList)int.Parse(this["FK_Event_Source"].DefaultValue);
            }
            set
            {
                this["FK_Event_Source"].DefaultValue = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Gets or sets an Event Subscriber
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        public RS.MDM.Events.EventSubscriberList EventSubscriber
        {
            get
            {
                return (RS.MDM.Events.EventSubscriberList)int.Parse(this["FK_Event_Subscriber"].DefaultValue);
            }
            set
            {
                this["FK_Event_Subscriber"].DefaultValue = ((int)value).ToString();
            }

        }

        /// <summary>
        /// Gets or sets a Security Role
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string SecurityRole
        {
            get
            {
                return this.GetValue("SecurityRole");
            }
            set
            {
                this.SetValue("SecurityRole", value);
            }
        }

        /// <summary>
        /// Gets or sets a Security User
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string SecurityUser
        {
            get
            {
                return this.GetValue("SecurityUser");
            }
            set
            {
                this.SetValue("SecurityUser", value);
            }
        }

        /// <summary>
        /// Gets or sets an organization
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string Organization
        {
            get
            {
                return this.GetValue("Organization");
            }
            set
            {
                this.SetValue("Organization", value);
            }
        }

        /// <summary>
        /// Gets or sets a catalog
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string Catalog
        {
            get
            {
                return this.GetValue("Catalog");
            }
            set
            {
                this.SetValue("Catalog", value);
            }
        }

        /// <summary>
        /// Gets or sets a category
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string Category
        {
            get
            {
                return this.GetValue("Category");
            }
            set
            {
                this.SetValue("Category", value);
            }
        }

        /// <summary>
        /// Gets or sets a CNode
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string CNode
        {
            get
            {
                return this.GetValue("CNode");
            }
            set
            {
                this.SetValue("CNode", value);
            }
        }

        /// <summary>
        /// Gets or sets an Attribute
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string Attribute
        {
            get
            {
                return this.GetValue("Attribute");
            }
            set
            {
                this.SetValue("Attribute", value);
            }
        }

        /// <summary>
        /// Gets or sets a nodetype
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string NodeType
        {
            get
            {
                return this.GetValue("NodeType");
            }
            set
            {
                this.SetValue("NodeType", value);
            }
        }

        /// <summary>
        /// Gets or sets a Relationship Type
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string RelationshipType
        {
            get
            {
                return this.GetValue("RelationshipType");
            }
            set
            {
                this.SetValue("RelationshipType", value);
            }
        }

        /// <summary>
        /// Gets or sets a Locale
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string Locale
        {
            get
            {
                return this.GetValue("Locale");
            }
            set
            {
                this.SetValue("Locale", value);
            }
        }

        /// <summary>
        /// Gets or sets a Application Configuration ID
        /// </summary>
        [Category("Application Context")]
        [XmlIgnore()]
        [TypeConverter(typeof(DictionaryConvertor))]
        public string ApplicationConfigID
        {
            get
            {
                return this.GetValue("ApplicationConfigID");
            }
            set
            {
                this.SetValue("ApplicationConfigID", value);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets the event subscriber name
        /// </summary>
        /// <returns>Event subscriber name as string</returns>
        public String GetEventSubscriberName()
        {
            return EventSubscriber.ToString();
        }
        #endregion

        #region Overrides

        /// <summary>
        /// Gets a filtered list of properties that are displayed in the designer
        /// </summary>
        /// <param name="properties">Indicates a superset of properties</param>
        /// <returns>A filtered list of properties that are displayed in the designer</returns>
        protected override PropertyDescriptorCollection GetProperties(PropertyDescriptorCollection properties)
        {
            properties = base.GetProperties(properties);
            PropertyDescriptorCollection _props = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor _prop in properties)
            {
                if (_prop != null)
                {
                    switch (_prop.Name)
                    {
                        case "ObjectStatus":
                        case "InheritedParent":
                        case "Parent":
                        case "Id":
                        case "Description":
                        case "UniqueIdentifier":
                        case "Name":
                            continue;
                        default:
                            _props.Add(_prop);
                            break;
                    }
                }
            }
            return _props;
        }

        /// <summary>
        /// Get a tree node that represents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();
            _treeNode.ImageKey = "DataSourceGet";
            _treeNode.SelectedImageKey = _treeNode.ImageKey;
            _treeNode.Nodes.Clear();
            return _treeNode;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Loads the list of all following objects from data base
        /// Organization, EntityType, RelationshipType, Attribute, Locale, Role, User
        /// Used by Application Configuration tool to populate dropdown lists
        /// </summary>
        public static void LoadContextData()
        {
            //since Data is almost static in nature, dictionaries are loaded only once.
            //Dictionary variables are defined as static.
            //Load dictionaries only if it is null  
            if (_orgDictionary == null)
                _orgDictionary = ApplicationContextData.GetAllOrganizations();
            if (_entityTypeDictionary == null)
                _entityTypeDictionary = ApplicationContextData.GetAllEntityTypes();
            if (_relationshipTypeDictionary == null)
                _relationshipTypeDictionary = ApplicationContextData.GetAllRelationshipTypes();
            if (_attributeDictionary == null)
                _attributeDictionary = ApplicationContextData.GetAllAttributes();
            if (_localeDictionary == null)
                _localeDictionary = ApplicationContextData.GetAllLocale();
            if (_roleDictionary == null)
                _roleDictionary = ApplicationContextData.GetAllRoles();
            if (_userDictionary == null)
                _userDictionary = ApplicationContextData.GetAllUsers();
        }

        #endregion Static Methods

        #region IDictionaryProperties Members

        /// <summary>
        /// Gets the string value for a given property
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <returns>A string value that denotes the property value</returns>
        public string GetValue(string propName)
        {
            return this.GetValue(propName, this.GetKey(propName));
        }

        /// <summary>
        /// Set the value of a property
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <param name="value">Indicates the value of the property</param>
        public void SetValue(string propName, string value)
        {
            int _key = this.GetKey(propName, value);
            this[this.GetDatabaseFieldName(propName)].DefaultValue = _key.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        private string GetDatabaseFieldName(string propName)
        {
            switch (propName)
            {
                case "SecurityRole":
                    return "FK_Security_Role";
                case "SecurityUser":
                    return "FK_Security_user";
                case "Organization":
                    return "FK_Org";
                case "Catalog":
                    return "FK_Catalog";
                case "CNode":
                    return "FK_CNode";
                case "Category":
                    return "FK_Category";
                case "Attribute":
                    return "FK_Attribute";
                case "NodeType":
                    return "FK_NodeType";
                case "RelationshipType":
                    return "FK_RelationshipType";
                case "Locale":
                    return "FK_Locale";
                case "ApplicationConfigID":
                    return "PK_Application_Config";
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the value for a given property and given key
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <param name="key">Indicates the key for the property</param>
        /// <returns>A string value that denotes the property value for a given key</returns>
        public string GetValue(string propName, int key)
        {
            Dictionary<int, string> _dictionary = this.GetDictionary(propName);
            if (_dictionary != null && _dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the key for a given property
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <returns>An integer value that indicates the key for a given property</returns>
        public int GetKey(string propName)
        {
            int _key = -1;
            int.TryParse(this[this.GetDatabaseFieldName(propName)].DefaultValue, out _key);
            return _key;
        }

        /// <summary>
        /// Gets the key for a given property and value
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <param name="value">Indicates the value of the property</param>
        /// <returns>An integer value that indicates the key for a given property and value</returns>
        public int GetKey(string propName, string value)
        {
            Dictionary<int, string> _dictionary = this.GetDictionary(propName);
            if (_dictionary != null && _dictionary.ContainsValue(value))
            {
                foreach (int _key in _dictionary.Keys)
                {
                    if (_dictionary[_key] == value)
                    {
                        return _key;
                    }
                }
                return -1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Get a dictionary for a given property name
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <returns>A dictionary that contains possible key-value pairs for a given property name</returns>
        public Dictionary<int, string> GetDictionary(string propName)
        {
            Dictionary<int, string> _dictionary = new Dictionary<int, string>();

            switch (propName)
            {
                case "Attribute":
                    _dictionary = _attributeDictionary;
                    break;
                case "SecurityRole":
                    _dictionary = _roleDictionary;
                    break;
                case "SecurityUser":
                    _dictionary = _userDictionary;
                    break;
                case "Organization":
                    _dictionary = _orgDictionary;
                    break;
                case "Catalog":
                    _dictionary = ApplicationContextData.GetCatalogsByOrganization(this.GetKey("Organization"));
                    break;
                case "CNode":
                    _dictionary.Add(0, "--No Data Available--");
                    break;
                case "Category":
                    _dictionary = ApplicationContextData.GetAllCategoriesByCatalog(this.GetKey("Catalog"));
                    break;
                case "NodeType":
                    _dictionary = _entityTypeDictionary;
                    break;
                case "RelationshipType":
                    _dictionary = _relationshipTypeDictionary;
                    break;
                case "Locale":
                    _dictionary = _localeDictionary;
                    break;
                case "ApplicationConfigID":
                default:
                    _dictionary.Add(0, "--No Data Available--");
                    break;
            }

            return _dictionary;
        }

        /// <summary>
        /// Cleans context dictionaries
        /// </summary>
        public static void CleanDictionaries()
        {
            _orgDictionary = null;
            _entityTypeDictionary = null;
            _relationshipTypeDictionary = null;
            _roleDictionary = null;
            _userDictionary = null;
            _localeDictionary = null;
            _attributeDictionary = null;
        }

        #endregion

        #region MDM application context level data fields - Not used in the context of Configuration Context

        /// <summary>
        /// 
        /// </summary>
        public Int32 AttributeId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String AttributeName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AttributeLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 CategoryId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String CategoryName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String CategoryPath
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 ContainerId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ContainerName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContainerLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 EntityId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String EntityName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EntityLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 EntityTypeId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String EntityTypeName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EntityTypeLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 OrganizationId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String OrganizationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OrganizationLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 RelationshipTypeId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String RelationshipTypeName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RelationshipTypeLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 RoleId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String RoleName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RoleLongName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 UserId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String UserName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ApplicationContextType ContextType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        LocaleEnum IMDMObject.Locale
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObjectAction Action
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 AuditRefId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ProgramName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ReferenceId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

    }
}
