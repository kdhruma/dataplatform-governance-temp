using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms;
using MDM.Core;
using MDM.BusinessObjects;
using MDM.Utility;
using ILocalizable = MDM.Interfaces.ILocalizable;
using MDM.Interfaces;
using MDM.Services;

namespace RS.MDM.Configuration
{
    /// <summary>
    /// Provides functionality to configure a collection of objects
    /// </summary>
    [XmlRoot("Configurations")]
    public class ApplicationConfiguration : RS.MDM.Object, IApplicationConfiguration, ILocalizable
    {
        #region Fields

        /// <summary>
        /// field for a list of configuration objects
        /// </summary>
        List<RS.MDM.Configuration.ConfigurationObject> _configurationObjects = new List<ConfigurationObject>();

        /// <summary>
        /// field for the datasource that is used to fetch the configuration objects
        /// </summary>
        private ApplicationContextInternal _configurationContext = new ApplicationContextInternal(1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, String.Empty, String.Empty);

        /// <summary>
        /// fields to be set to be passed to legacy service
        /// </summary>
        private int _eventSourceId;
        private int _eventSubscriberId;
        private int _securityRoleId;
        private int _securityUserId;
        private int _orgId;
        private int _catalogId;
        private long _categoryId;
        private long _cNodeId;
        private int _attributeId;
        private int _nodeTypeId;
        private int _relationshipTypeId;
        private int _localId;
        private int _applicationConfigId;
        private string _lookupName = String.Empty;
        private string _categoryPath = String.Empty;
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ApplicationConfiguration()
            : base()
        {
            this.AddVerb("Open Configurations");
            this.AddVerb("Add Configuration");
            this.RemoveVerb("Read File");
            this.RemoveVerb("Save File");
            this.RemoveVerb("Load XML");
            this.RemoveVerb("Show XML");
            this.RemoveVerb("Inherit");
            this.RemoveVerb("Find Changes");
            this.RemoveVerb("Accept Changes");
            this.Name = "Application Configuration - " + (new Random()).Next(1000, 9999).ToString();
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="EventSourceId">Indicates an Id of the event source</param>
        /// <param name="EventSubscriberId">Indicates an Id of the event subscriber</param>
        /// <param name="SecurityRoleId">Indicates an id of a security role</param>
        /// <param name="SecurityUserId">Indicates an id of a security user</param>
        /// <param name="OrgId">Indicates an Id of an organization</param>
        /// <param name="CatalogId">Indicates an Id of a catalog</param>
        /// <param name="CategoryId">Indicates an Id of a category</param>
        /// <param name="CNodeId">Indicates an Id of a CNode</param>
        /// <param name="AttributeId">Indicates an Id of an attribute</param>
        /// <param name="NodeTypeId">Indicates an Id of a node type</param>
        /// <param name="RelationshipTypeId">Indicates an Id of a relationship type</param>
        /// <param name="LocaleId">Indicates an Id of a locale</param>
        public ApplicationConfiguration(int EventSourceId, int EventSubscriberId, int SecurityRoleId, int SecurityUserId, int OrgId, int CatalogId, Int64 CategoryId, Int64 CNodeId, int AttributeId, int NodeTypeId, int RelationshipTypeId, int LocalId, int ApplicationConfigId)
            : this()
        {
            _eventSourceId = EventSourceId;
            _eventSubscriberId = EventSubscriberId;
            _securityRoleId = SecurityRoleId;
            _securityUserId = SecurityUserId;
            _orgId = OrgId;
            _catalogId = CatalogId;
            _categoryId = CategoryId;
            _cNodeId = CNodeId;
            _attributeId = AttributeId;
            _nodeTypeId = NodeTypeId;
            _relationshipTypeId = RelationshipTypeId;
            _localId = LocalId;
            _applicationConfigId = ApplicationConfigId;
            _categoryPath = GetCategoryPath();
            _configurationContext = new ApplicationContextInternal(EventSourceId, EventSubscriberId, SecurityRoleId, SecurityUserId, OrgId, CatalogId, CategoryId, CNodeId, AttributeId, NodeTypeId, RelationshipTypeId, LocalId, ApplicationConfigId, _categoryPath, _lookupName);

        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="EventSourceId">Indicates an Id of the event source</param>
        /// <param name="EventSubscriberId">Indicates an Id of the event subscriber</param>
        /// <param name="SecurityRoleId">Indicates an id of a security role</param>
        /// <param name="SecurityUserId">Indicates an id of a security user</param>
        /// <param name="OrgId">Indicates an Id of an organization</param>
        /// <param name="CatalogId">Indicates an Id of a catalog</param>
        public ApplicationConfiguration(int EventSourceId, int EventSubscriberId, int SecurityRoleId, int SecurityUserId, int OrgId, int CatalogId)
            : this()
        {
            this._configurationContext = new ApplicationContextInternal(EventSourceId, EventSubscriberId, SecurityRoleId, SecurityUserId, OrgId, CatalogId, 0, 0, 0, 0, 0, 0, 0, String.Empty, String.Empty);
            _eventSourceId = EventSourceId;
            _eventSubscriberId = EventSubscriberId;
            _securityRoleId = SecurityRoleId;
            _securityUserId = SecurityUserId;
            _orgId = OrgId;
            _catalogId = CatalogId;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="EventSourceId">Indicates an Id of the event source</param>
        /// <param name="EventSubscriberId">Indicates an Id of the event subscriber</param>
        /// <param name="SecurityRoleId">Indicates an id of a security role</param>
        /// <param name="SecurityUserId">Indicates an id of a security user</param>
        /// <param name="lookupName">Indicates the lookupName </param>

        public ApplicationConfiguration(int EventSourceId, int EventSubscriberId, int SecurityRoleId, int SecurityUserId, String lookupName)
            : this()
        {
            this._configurationContext = new ApplicationContextInternal(EventSourceId, EventSubscriberId, SecurityRoleId, SecurityUserId, 0, 0, 0, 0, 0, 0, 0, 0, 0, String.Empty, lookupName);
            _eventSourceId = EventSourceId;
            _eventSubscriberId = EventSubscriberId;
            _securityRoleId = SecurityRoleId;
            _securityUserId = SecurityUserId;
            _lookupName = lookupName;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a list of configuration object
        /// </summary>
        [Category("Configuration Objects")]
        [Description("Indicates the DataSource Items in the collection")]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        [XmlElement("Configuration")]
        public List<RS.MDM.Configuration.ConfigurationObject> Items
        {
            get
            {
                this.SetParent();
                return this._configurationObjects;
            }
            set
            {
                this._configurationObjects = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Gets or sets a datasource to fetch the configuration objects
        /// </summary>
        [Category("Data Source")]
        [Description("Indicates the context that is used to get the configuration objects from the database")]
        public ApplicationContextInternal ConfigurationContext
        {
            get
            {
                this.SetParent();
                return this._configurationContext;
            }
            set
            {
                this._configurationContext = value;
                this.SetParent();
            }
        }
        
        /// <summary>
        /// Represent UILocale property implementation for ILocalizable interface
        /// </summary>
        [XmlIgnore()]
        public new LocaleEnum UILocale
        {
            get { return Enum.IsDefined(typeof (LocaleEnum), _localId) ? (LocaleEnum) _localId : LocaleEnum.UnKnown; }
            set { _localId = (Int32) value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the list of configuration objects
        /// </summary>
        /// <returns>List of configuration objects</returns>
        public List<IConfigurationObject> GetItems()
        {
            List<IConfigurationObject> items = new List<IConfigurationObject>();
            foreach (ConfigurationObject configObject in Items)
            {
                items.Add(configObject);
            }

            return items;
        }

        /// <summary>
        /// Loads configuration objects based on the ApplicationContext parameter
        /// </summary>
        public void Open()
        {
            this.OnDesignerVerbClick("Open Configurations", null, null);
        }

        /// <summary>
        /// Gets an Object from a list for a given object name
        /// </summary>
        /// <param name="objectName">Indicates the name of an object that needs to be fetched</param>
        /// <returns>An object of a given name</returns>
        public object GetObject(string objectName)
        {
            foreach (ConfigurationObject _configObject in this.Items)
            {
                if (_configObject != null)
                {
                    if (_configObject.Object != null)
                    {
                        if (_configObject.Object.Name == objectName)
                        {
                            return _configObject.Object;
                        }
                    }
                    else
                    {
                        if (_configObject.Name == objectName)
                        {
                            return _configObject.XMLConfiguration;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets Configuration XML from a list for a given object name
        /// </summary>
        /// <param name="objectName">Indicates the name of an object that needs to be fetched</param>
        /// <returns>Configuration XML for a given object</returns>
        public string GetConfigurationXML(string objectName)
        {
            foreach (ConfigurationObject _configObject in this.Items)
            {
                if (_configObject != null)
                {
                    if (_configObject.Object != null)
                    {
                        if (_configObject.Object.Name == objectName)
                        {
                            return _configObject.XMLConfiguration;
                        }
                    }
                    else
                    {
                        if (_configObject.Name == objectName)
                        {
                            return _configObject.XMLConfiguration;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets an Object from a list for a given object identifier
        /// </summary>
        /// <param name="objectId">Indicates the Id of an object that needs to be fetched</param>
        /// <returns>An object of a given identifier</returns>
        public object GetObject(int objectId)
        {
            foreach (ConfigurationObject _configObject in this.Items)
            {
                if (_configObject != null)
                {
                    if (_configObject.Object != null)
                    {
                        if (_configObject.Object.Id == objectId)
                        {
                            return _configObject.Object;
                        }
                    }
                    else
                    {
                        if (_configObject.Id == objectId)
                        {
                            return _configObject.XMLConfiguration;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets an Object from a list for a given object identifier
        /// </summary>
        /// <param name="objectId">Indicates the Id of an object that needs to be fetched</param>
        /// <returns>An object of a given identifier</returns>
        public object GetObjectByType(string objectId)
        {
            foreach (ConfigurationObject _configObject in this.Items)
            {
                if (_configObject != null)
                {
                    if (_configObject.Object != null)
                    {
                        if (_configObject.Object.GetType().Name == objectId)
                        {
                            return _configObject.Object;
                        }
                    }
                    else
                    {
                        if (_configObject.GetType().Name == objectId)
                        {
                            return _configObject.XMLConfiguration;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets an Object from a list for a given object identifier
        /// </summary>
        /// <param name="objectId">Indicates the Id of an object that needs to be fetched</param>
        /// <returns>An object of a given identifier</returns>
        public object GetObjectByApplicationContextDefinitionId(int applicationContextDefinitionId)
        {
            foreach (ConfigurationObject _configObject in this.Items)
            {
                if (_configObject != null)
                {
                    if (_configObject.ApplicationContext != null)
                    {
                        if (_configObject.ApplicationContext.Id == applicationContextDefinitionId)
                        {
                            return _configObject.Object;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a list of configurations from the database
        /// </summary>
        public void GetConfigurations()
        {
            this.GetConfigurations(null, null, true);
        }

        /// <summary>
        /// Gets a list of configurations from the  service for the legacy code or from the database if actually invoked by the  service
        /// </summary>
        /// <param name="page">Indicates a web page that is used to extract parameter inputs</param>
        /// <param name="keyValuePairs">Indicates a key value pairs that is used to extract parameter inputs</param>
        /// <param name="skipServiceCall">Indicates to skip service call and hit database directly if the method is being actually called from the service</param>
        public void GetConfigurations(System.Web.UI.Page page, System.Collections.Hashtable keyValuePairs, bool skipServiceCall = false)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StartTraceActivity(MDMTraceSource.Configuration);
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                this.Items.Clear();

                System.Data.DataTable _dataTable = null;

                if (skipServiceCall)
                {
                    _dataTable = this._configurationContext.GetData(page, keyValuePairs) as System.Data.DataTable;

                }
                else
                {
                    //For the localId the source fieldname is query string so just check if we need to override the default one
                    if (page != null && this.ContainsKey(page.Request.QueryString.AllKeys, "LocaleID"))
                    {
                        _localId = ValueTypeHelper.Int32TryParse(page.Request.QueryString["LocaleID"], _localId);
                    }

                    var legacyService = new LegacyDataAccessService();
                    
                    _dataTable = legacyService.GetApplicationConfigData(_eventSourceId, _eventSubscriberId, _securityRoleId, _securityUserId, _orgId, _catalogId,
                                                        ValueTypeHelper.ConvertToInt32(_categoryId), ValueTypeHelper.ConvertToInt32(_cNodeId), _attributeId, _nodeTypeId, _relationshipTypeId, _localId, _applicationConfigId, _categoryPath, String.Empty);
                }
                if (_dataTable != null && _dataTable.Rows.Count > 0)
                {
                    List<List<System.Data.DataRow>> _dataRowListList = new List<List<System.Data.DataRow>>();
                    System.Data.DataRow[] _rootDataRows = _dataTable.Select("FK_Application_ConfigParent is Null");
                    if (_rootDataRows != null && _rootDataRows.Length > 0)
                    {
                        foreach (System.Data.DataRow _rootDataRow in _rootDataRows)
                        {
                            List<System.Data.DataRow> _dataRowList = new List<System.Data.DataRow>();
                            _dataRowListList.Add(_dataRowList);
                            _dataRowList.Add(_rootDataRow);
                            this.GroupConfigurations(_dataTable, _dataRowListList, _dataRowList, _rootDataRow);
                        }
                        foreach (List<System.Data.DataRow> _dataRowList in _dataRowListList)
                        {
                            ConfigurationObject _configurationObject = new ConfigurationObject(_dataRowList);
                            this.Items.Add(_configurationObject);
                        }
                    }
                }

                //Sort configurations based on Long Name
                if (this.Items != null && this.Items.Count > 0)
                {
                    this.Items = this.Items.OrderBy(i => i.LongName).ToList();
                }

                System.Diagnostics.Trace.WriteLine("Configurations fetched successfully.", RS.MDM.Logging.LogLevel.INFO.ToString());
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.ToString());
                System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }
        }

        /// <summary>
        /// Validates an object for errors
        /// </summary>
        /// <param name="validationErrors">Indicates validationerror collection is used to aggregate all the errors</param>
        public override void Validate(ref RS.MDM.Validations.ValidationErrorCollection validationErrors)
        {
            if (validationErrors == null)
            {
                validationErrors = new RS.MDM.Validations.ValidationErrorCollection();
            }
            base.Validate(ref validationErrors);
            foreach (ConfigurationObject _configurationObject in this.Items)
            {
                _configurationObject.Validate(ref validationErrors);
            }
        }

        /// <summary>
        /// Execute logic related to a given verb
        /// </summary>
        /// <param name="text">Indicate the text that represents a supported verb</param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        public override void OnDesignerVerbClick(string text, Object configObject, object treeView)
        {
            base.OnDesignerVerbClick(text, configObject, treeView);
            switch (text)
            {
                case "Open Configurations":
                    this.GetConfigurations(null, null, true);
                    break;
                case "Add Configuration":
                    ConfigurationObject _configurationObject = new ConfigurationObject();
                    _configurationObject.Parent = this;
                    this.Items.Add(_configurationObject);
                    break;

            }
            System.ComponentModel.TypeDescriptor.Refresh(this);
        }

        /// <summary>
        /// Determines if a key exists in an array of keys
        /// </summary>
        /// <param name="keys">An array of keys that need to be searched</param>
        /// <param name="key">A key that is used for searching</param>
        /// <returns>A boolean value that indicates if the key exists in the collection</returns>
        private bool ContainsKey(string[] keys, string key)
        {
            foreach (string _key in keys)
            {
                if (_key != null && _key.ToLower() == key.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String GetCategoryPath()
        {
            String categoryPath = String.Empty;
            MDMFeatureConfig mdmFeatureConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(MDMCenterApplication.MDMCenter, "ApplicationConfigCategoryInheritance", "7.6.5");
            if (mdmFeatureConfig != null)
            {
                bool isCategoryInheritanceEnabled = mdmFeatureConfig.IsEnabled;

                if (isCategoryInheritanceEnabled)
                {
                    if (_categoryId > 0)
                    {
                        Int64 objectId = 0;

                        // If the cNodeId is greater than  0 then do GetEntity based on cNodeId else based on category Id
                        if (_cNodeId > 0)
                        {
                            objectId = _cNodeId;
                        }
                        else
                        {
                            objectId = _categoryId;
                        }

                        var entity = new DataService().GetEntity(objectId, new EntityContext { CategoryId = _categoryId, ContainerId = _catalogId }, MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
                        categoryPath = entity.CategoryPath;
                    }
                }
            }
            return categoryPath;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();
            foreach (ConfigurationObject _configurationObject in this._configurationObjects)
            {
                if (_configurationObject != null)
                {
                    _configurationObject.GenerateNewUniqueIdentifier();
                }
            }
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._configurationObjects != null)
            {
                foreach (ConfigurationObject _configurationObject in this._configurationObjects)
                {
                    if (_configurationObject != null)
                    {
                        _configurationObject.Parent = this;
                        _configurationObject.InheritedParent = this.InheritedParent;
                    }
                }
            }
            if (this._configurationContext != null)
            {
                this._configurationContext.Parent = this;
                this._configurationContext.InheritedParent = this.InheritedParent;
            }
        }

        /// <summary>
        /// Gets the filtered properties of an object
        /// </summary>
        /// <param name="properties">Indicates the superset of properties collection</param>
        /// <returns>A set of filtered properties of an object that needs to be displayed in the designer</returns>
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
                        case "Parent":
                        case "Id":
                        case "Description":
                        case "UniqueIdentifier":
                        case "InheritedParent":
                        case "ObjectStatus":
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
        /// Get a tree node that reprents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            TreeNode _treeNode = base.GetTreeNode();
            _treeNode.ImageKey = "Configuration";
            _treeNode.Expand();
            _treeNode.SelectedImageKey = _treeNode.ImageKey;
            _treeNode.Tag = this;

            TreeNode _itemsNode = new TreeNode("ConfigurationObjects");
            _itemsNode.ToolTipText = "ConfigurationObjects";
            _itemsNode.ImageKey = "Items";
            _itemsNode.Expand();
            _itemsNode.SelectedImageKey = _itemsNode.ImageKey;
            _treeNode.Nodes.Add(_itemsNode);

            foreach (ConfigurationObject _configurationObject in this._configurationObjects)
            {
                if (_configurationObject != null)
                {
                    _itemsNode.Nodes.Add(_configurationObject.GetTreeNode());
                }
            }

            if (this._configurationContext != null)
            {
                _treeNode.Nodes.Add(this._configurationContext.GetTreeNode());
            }

            return _treeNode;
        }

        #endregion

    }
}
