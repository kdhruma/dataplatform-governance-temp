using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

using RS.MDM.Data;
using RS.MDM.Configuration.Data;

using MDM.Interfaces;
using MDM.Core;
using MDM.Services;

namespace RS.MDM.Configuration
{
    /// <summary>
    /// Provides functionality to configure an object
    /// </summary>
    [XmlRoot("Configuration")]
    public class ConfigurationObject : RS.MDM.Object, IConfigurationObject
    {

        #region Fields

        /// <summary>
        /// field for the datasourceset
        /// </summary>
        private DataSourceSet _dataSourceSet = new DataSourceSet();

        /// <summary>
        /// field for the datasource that is used to fetch the configuration objects
        /// </summary>
        private ApplicationContextInternal _applicationContext = new ApplicationContextInternal();

        /// <summary>
        /// field for a configurable object
        /// </summary>
        private RS.MDM.Object _object = null;

        /// <summary>
        /// field for the xml configuration
        /// </summary>
        private string _xmlConfiguration = string.Empty;

        /// <summary>
        /// field for the path of the file 
        /// </summary>
        private string _filePath = string.Empty;

        /// <summary>
        /// Indicates whether current Config has been changed.
        /// </summary>
        public bool _isConfigDirty = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ConfigurationObject()
            : base()
        {
            //Adding context parameters for DataSourceset and Application context
            //Here we are maintaining a single object to sync between Application context and datasourceset
            RS.MDM.Data.Parameter _parameter = new RS.MDM.Data.Parameter();

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Application_ContextDefinition";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "ApplicationConfigTypeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Application_ConfigParent";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "ApplicationConfigParentId";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "ShortName";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "ShortName";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "LongName";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "LongName";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Source";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "EventSourceId";
            _parameter.DefaultValue = "1";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Subscriber";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "EventSubscriberId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_Role";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "SecurityRoleId";
            _parameter.DataType = SqlType.SqlInt32;
            _parameter.DefaultValue = "0";
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_user";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "SecurityUserId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Org";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "OrgId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Catalog";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "CatalogId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Category";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "CategoryId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_CNode";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "CNodeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Attribute";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "AttributeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_NodeType";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "NodeTypeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_RelationshipType";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "RelationshipTypeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _dataSourceSet.Parameters.Add(_applicationContext.Parameters.GetItemByName("FK_Security_Role"));
            _dataSourceSet.Parameters.Add(_applicationContext.Parameters.GetItemByName("FK_Security_user"));

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "ConfigXML";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "ConfigXML";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "Description";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "Description";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "PreCondition";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "PreCondition";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "PostCondition";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "PostCondition";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "XSDSchema";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "XSDSchema";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "SampleXML";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "SampleXML";
            _parameter.DefaultValue = "";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "loginUser";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "loginUser";
            _parameter.DefaultValue = "Configuration Tool";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "userProgram";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.Direction = System.Data.ParameterDirection.Input;
            _parameter.SourceFieldName = "userProgram";
            _parameter.DefaultValue = "Configuration Tool";
            _parameter.DataType = SqlType.SqlString;
            _dataSourceSet.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "PK_Application_Config";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "ApplicationConfigID";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Locale";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.Direction = System.Data.ParameterDirection.Input;
            _parameter.SourceFieldName = "FK_Locale";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);
            _applicationContext.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "RETURN_VALUE";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.Direction = System.Data.ParameterDirection.Output;
            _parameter.SourceFieldName = "RETURN_VALUE";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            _dataSourceSet.Parameters.Add(_parameter);

            this.AddVerb("Open Configuration");
            this.AddVerb("Save Configuration");
            this.AddVerb("Save As Configuration");
            this.AddVerb("Load Child Configurations");
            this.AddVerb("Add Object");
            this.AddVerb("Edit Object");
            this.AddVerb("Generate XML");
            this.AddVerb("Export");
            this.RemoveVerb("Read File");
            this.RemoveVerb("Save File");
            this.RemoveVerb("Load XML");
            this.RemoveVerb("Show XML");
            this.RemoveVerb("Find Changes");
            this.RemoveVerb("Accept Changes");
        }

        /// <summary>
        /// Constructor with DataRow List as input parameter
        /// </summary>
        /// <param name="dataRowList">Indicates a DataRow List that contains the serialized configuration objects that constitute inheritance path</param>
        public ConfigurationObject(List<System.Data.DataRow> dataRowList)
            : this()
        {
            //Added the following to populate certain required fields in case XML is not deserialized
            foreach (System.Data.DataRow _dataRow in dataRowList)
            {
                this.Id = int.Parse(_dataRow["Pk_Application_Config"].ToString());
                this.ApplicationContext.EventSource = (RS.MDM.Events.EventSourceList)int.Parse(_dataRow["FK_Event_Source"].ToString());
                this.ApplicationContext.EventSubscriber = (RS.MDM.Events.EventSubscriberList)int.Parse(_dataRow["FK_Event_Subscriber"].ToString());
                this.ApplicationContext.Id = int.Parse(_dataRow["FK_Application_ContextDefinition"].ToString());
                this.XMLConfiguration = _dataRow["ConfigXML"].ToString();
            }
            this.DeserializeObject(dataRowList);
        }

        /// <summary>
        /// Constructor with dataRow as input parameter
        /// </summary>
        /// <param name="dataRow">Indicates a dataRow that contains the serialized configuration object</param>
        public ConfigurationObject(System.Data.DataRow dataRow)
            : this()
        {
            Int32 configId = 0;

            foreach (System.Data.DataColumn _dataColumn in dataRow.Table.Columns)
            {
                object _value = dataRow[_dataColumn.ColumnName];
                Parameter _parameter = this._dataSourceSet[_dataColumn.ColumnName];
                if (_parameter != null)
                {
                    _parameter.DefaultValue = _value.ToString();
                    _parameter.Value = _value;
                }
                _parameter = this._applicationContext[_dataColumn.ColumnName];
                if (_parameter != null)
                {
                    _parameter.DefaultValue = _value.ToString();
                    _parameter.Value = _value;
                }
                switch (_dataColumn.ColumnName)
                {
                    case "PK_Application_Config":
                        Int32.TryParse(_value.ToString(), out configId);
                        this.Id = configId;
                        _dataSourceSet.Id = this.Id;
                        break;
                    case "ShortName":
                        this.ShortName = _value.ToString();
                        break;
                    case "LongName":
                        this.LongName = _value.ToString();
                        break;
                    case "ConfigXML":
                        try
                        {
                            this.XMLConfiguration = _value.ToString();
                            this.Object = RS.MDM.Object.XMLDeserialize(_value.ToString()) as RS.MDM.Object;
                            if (this.Object != null)
                            {
                                this.Name = this.Object.Name;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
                        }
                        break;
                }
            }
            if (this.Object != null)
            {
                this.Object.Id = this.Id;
            }
        }

        /// <summary>
        /// Constructor with object as input parameter
        /// </summary>
        /// <param name="configurationObject">Indicates an object that needs to be configured</param>
        public ConfigurationObject(RS.MDM.Object configurationObject)
        {
            this.Object = configurationObject;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an object that needs to be configurated
        /// </summary>
        [Category("Configuration Object")]
        [Editor(typeof(RS.MDM.ComponentModel.Design.PropertiesTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Indicates the Object that is being configured.")]
        public RS.MDM.Object Object
        {
            get
            {
                this.SetParent();
                return this._object;
            }
            set
            {
                this._object = value;
                this.SetParent();
                if (_object != null)
                {
                    this.Name = _object.Name;
                    _object.NameChangedEvent += new NameChangedEventHandler(_object_NameChangedEvent);
                }
            }
        }

        /// <summary>
        /// Gets or sets an xml configuration string
        /// </summary>
        [Description("Indicates the XML representation of the Configuration Object")]
        [Category("Configuration Object")]
        [Editor(typeof(RS.MDM.ComponentModel.Design.StringTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string XMLConfiguration
        {
            get
            {
                return this._xmlConfiguration;
            }
            set
            {
                this._xmlConfiguration = value;
            }
        }

        /// <summary>
        /// Gets or sets a DataSource that is used to store the configuration in the database
        /// </summary>
        [Category("Data Source")]
        [Description("Indicates the Data Source that is used to create/update the object in the database")]
        public Data.DataSourceSet DataSourceSet
        {
            get
            {
                this.SetParent();
                return this._dataSourceSet;
            }
            set
            {
                this._dataSourceSet = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Gets or sets a datasource to fetch the configuration objects
        /// </summary>
        [Category("Data Source")]
        [Description("Indicates the Data Source that is used to get the configuration objects from the database")]
        public ApplicationContextInternal ApplicationContext
        {
            get
            {
                this.SetParent();
                return this._applicationContext;
            }
            set
            {
                this._applicationContext = value;
                this.SetParent();
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets the current MDM application context
        /// </summary>
        /// <returns>Current MDM Application context as IApplicationContext</returns>
        public IApplicationContext GetApplicationContext()
        {
            return ApplicationContext;
        }

        /// <summary>
        /// Gets the object that is set to be configured
        /// </summary>
        /// <returns>Ojbect that is set to be configured as IObject</returns>
        public IObject GetObject()
        {
            return Object;
        }

        /// <summary>
        /// Opens a specific configuration object from the database based on the parameters defined in the DataSourceGet
        /// </summary>
        public void Open()
        {
            this.OnDesignerVerbClick("Open Configuration", this, null);
        }

        /// <summary>
        /// Saves the configuration object to the database based on the parameters defined in the DataSourceSet
        /// </summary>
        public void Save()
        {
            this.OnDesignerVerbClick("Save Configuration", this, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        private void DeserializeObject(System.Data.DataTable dataTable)
        {
            List<System.Data.DataRow> _dataRowList = new List<System.Data.DataRow>();
            foreach (System.Data.DataRow _dataRow in dataTable.Rows)
            {
                _dataRowList.Add(_dataRow);
            }
            this.DeserializeObject(_dataRowList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRowList"></param>
        private void DeserializeObject(List<System.Data.DataRow> dataRowList)
        {
            this.Object = RS.MDM.Object.XMLDeserialize(dataRowList);

            if (_object != null)
            {
                string _uniqueId = string.Format(@"UniqueIdentifier=""{0}""", _object.UniqueIdentifier);
                foreach (System.Data.DataRow _dataRow in dataRowList)
                {
                    if (_dataRow["ConfigXML"].ToString().Contains(_uniqueId))
                    {
                        foreach (System.Data.DataColumn _dataColumn in _dataRow.Table.Columns)
                        {
                            object _value = _dataRow[_dataColumn.ColumnName];
                            Parameter _parameter = this._dataSourceSet[_dataColumn.ColumnName];
                            if (_parameter != null)
                            {
                                _parameter.DefaultValue = _value.ToString();
                                _parameter.Value = _value;
                            }
                            _parameter = this._applicationContext[_dataColumn.ColumnName];
                            if (_parameter != null)
                            {
                                _parameter.DefaultValue = _value.ToString();
                                _parameter.Value = _value;
                            }
                            if (_dataColumn.ColumnName == "PK_Application_Config")
                            {
                                this.Id = (int)_value;
                                _dataSourceSet.Id = this.Id;
                                this._object.Id = this.Id;
                            }
                            if (_dataColumn.ColumnName == "ShortName")
                            {
                                this.ShortName = _value.ToString();
                            }
                            if (_dataColumn.ColumnName == "LongName")
                            {
                                this.LongName = _value.ToString();
                            }
                        }
                    }
                }
                System.Diagnostics.Trace.WriteLine("Configurations fetched successfully.", RS.MDM.Logging.LogLevel.INFO.ToString());
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
            String treeNodeText = String.Empty;
            TreeView explorerTreeView = null;

            if (text != "Inherit")
            {
                base.OnDesignerVerbClick(text, this, treeView);
            }
            switch (text)
            {
                case "Open Configuration":
                    try
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                        this._object = null;
                        System.Data.DataTable _dataTable = this._applicationContext.GetData(null, null) as System.Data.DataTable;
                        this.DeserializeObject(_dataTable);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
                    }
                    finally
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    }
                    break;
                case "Save As Configuration":
                    {
                        if (this.Object != null)
                        {
                            this.Object.Id = 0;
                            this.XMLConfiguration = RS.MDM.Object.XMLSerialize(this.Object, new UnicodeEncoding(false, false), 4);
                        }
                        this.DataSourceSet["ConfigXML"].Value = this.XMLConfiguration;
                        SaveConfiguration(treeView);
                        break;
                    }
                case "Save Configuration":
                    if (this.Object != null)
                    {
                        this.Object.SetParent();
                        this.Object.FindChanges();
                        this.XMLConfiguration = RS.MDM.Object.XMLSerialize(this.Object, new UnicodeEncoding(false, false), 4);
                    }

                    this.DataSourceSet["ConfigXML"].Value = this.XMLConfiguration;
                    SaveConfiguration(treeView);
                    break;
                case "Load Child Configurations":
                    //Loading immediate child configurations of selected parent
                    try
                    {
                        if (treeView is TreeView)
                            explorerTreeView = treeView as TreeView;

                        //Check whether already child configurations are loaded
                        if (explorerTreeView != null && explorerTreeView.SelectedNode.Nodes.ContainsKey("ChildConfigurations"))
                        {
                            //yes.. child configs are already loaded.
                            //do not do anything
                            return;
                        }

                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                        System.Data.DataTable _dataTable = this._applicationContext.GetChildData(this.Id) as System.Data.DataTable;

                        if (_dataTable != null && _dataTable.Rows.Count > 0)
                        {
                            TreeNode childConfigsNode = new TreeNode("ChildConfigurations");
                            childConfigsNode.ToolTipText = "ChildConfigurations";
                            childConfigsNode.ImageKey = "Configuration";
                            childConfigsNode.Name = "ChildConfigurations";
                            childConfigsNode.Expand();
                            childConfigsNode.SelectedImageKey = childConfigsNode.ImageKey;
                            explorerTreeView.SelectedNode.Nodes.Add(childConfigsNode);

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
                                    childConfigsNode.Nodes.Add(_configurationObject.GetTreeNode());
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sorry.. There are no child configurations for '" + this.ShortName + "'.", "Application Configuration Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("The following exception has occurred while loading child configurations:\n" + ex.Message, "Application Configuration Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
                    }
                    finally
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    }
                    break;
                case "Edit Object":
                    if (this._object == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Please add an Object to configure and try again.", "Edit Object", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                    RS.MDM.ComponentModel.Design.PropertiesEditor _editor = new RS.MDM.ComponentModel.Design.PropertiesEditor();
                    _editor.Parameter = this._object;
                    _editor.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    _editor.Text = this._object.GetType().Name;
                    _editor.Tag = this;
                    _editor.ShowDialog();
                    break;
                case "Add Object":
                    System.Windows.Forms.OpenFileDialog _fileDialog = new System.Windows.Forms.OpenFileDialog();
                    _fileDialog.Title = "Configuration File Assembly";
                    _fileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                    _fileDialog.Filter = "Assembly Files (*.dll)|*.dll|Dll Files (*.exe)|*.exe";
                    _fileDialog.FilterIndex = 0;
                    _fileDialog.Title = "Type File";
                    if (_fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string _assemblyPath = _fileDialog.FileName;
                        List<string> _list = this.GetTypesByBaseClassName(_assemblyPath, typeof(RS.MDM.Object).FullName);
                        RS.MDM.ComponentModel.Design.ListItemSelector _listItemSelector = new RS.MDM.ComponentModel.Design.ListItemSelector();
                        _listItemSelector.ListItems = _list;
                        _listItemSelector.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                        if (_listItemSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string _typeName = _listItemSelector.SelectedItem;
                            this.AddObject(_assemblyPath, _typeName);
                        }
                    }
                    break;
                case "Generate XML":
                    if (this.Object != null)
                    {
                        this.XMLConfiguration = RS.MDM.Object.XMLSerialize(this.Object, 4);
                    }
                    break;
                case "Export":
                    if (this.Object != null)
                    {
                        System.Windows.Forms.SaveFileDialog _saveDialog = new System.Windows.Forms.SaveFileDialog();
                        _saveDialog.ShowDialog();

                        if (!String.IsNullOrEmpty(_saveDialog.FileName.Trim()))
                        {
                            String filename = _saveDialog.FileName;
                        }
                        else
                        {
                            MessageBox.Show("Please enter filename.", "Application Configuration Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    break;
                case "Inherit":
                    if (this._object != null && this.Parent != null)
                    {
                        RS.MDM.Configuration.ConfigurationObject _configurationObject = new ConfigurationObject(this.Parent);
                        this._object.AcceptChanges();
                        _configurationObject._object = RS.MDM.Object.Clone(this._object, true);

                        _configurationObject._dataSourceSet.Parameters.Clear();
                        foreach (Parameter _parameter in this._dataSourceSet.Parameters)
                        {
                            _configurationObject._dataSourceSet.Parameters.Add(RS.MDM.Object.Clone(_parameter, true) as Parameter);
                        }

                        //Modified by Shridhar Moorkhandi to set ConfigParentId of the Inherited config to the Id of the parent

                        Parameter configParent = _configurationObject.DataSourceSet.Parameters.GetItemByName("FK_Application_ConfigParent");
                        if (configParent != null)
                            configParent.DefaultValue = this.Id.ToString();

                        //end of modification

                        _configurationObject._applicationContext.Parameters.Clear();
                        foreach (Parameter _parameter in this._applicationContext.Parameters)
                        {
                            _configurationObject._applicationContext.Parameters.Add(RS.MDM.Object.Clone(_parameter, true) as Parameter);
                        }

                        (this.Parent as ApplicationConfiguration).Items.Add(_configurationObject);
                    }
                    break;
            }
            System.ComponentModel.TypeDescriptor.Refresh(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeView"></param>
        private void SaveConfiguration(object treeView)
        {
            String treeNodeText = String.Empty;
            TreeView explorerTreeView = null;
            try
            {
                #region Service method param population

                Int32 applicationContextDefinationId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[0].Value.ToString(), 0);
                Int32 applicationConfigParentId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[1].Value.ToString(), 0);
                String shortName = this.DataSourceSet.Parameters[2].Value.ToString();
                String longName = this.DataSourceSet.Parameters[3].Value.ToString();
                Int32 eventSourceId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[4].Value.ToString(), 0);
                Int32 eventSubscriberId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[5].Value.ToString(), 0);
                Int32 orgId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[6].Value.ToString(), 0);
                Int32 catalogId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[7].Value.ToString(), 0);
                Int32 categoryId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[8].Value.ToString(), 0);
                Int32 entityId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[9].Value.ToString(), 0);
                Int32 attributeId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[10].Value.ToString(), 0);
                Int32 entityTypeId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[11].Value.ToString(), 0);
                Int32 relationshipTypeId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[12].Value.ToString(), 0);
                Int32 roleId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[13].Value.ToString(), 0);
                Int32 userId = ValueTypeHelper.Int32TryParse(this.DataSourceSet.Parameters[14].Value.ToString(), 0);
                String configXml = Convert.ToString(this.DataSourceSet.Parameters[15].Value);
                String description = Convert.ToString(this.DataSourceSet.Parameters[16].Value);
                String preCondition = Convert.ToString(this.DataSourceSet.Parameters[17].Value);
                String postCondition = Convert.ToString(this.DataSourceSet.Parameters[18].Value);
                String xsdSchema = Convert.ToString(this.DataSourceSet.Parameters[19].Value);
                String sampleXml = Convert.ToString(this.DataSourceSet.Parameters[20].Value);
                String loginUser = Convert.ToString(this.DataSourceSet.Parameters[21].Value);
                String programName = Convert.ToString(this.DataSourceSet.Parameters[22].Value);
                Int32 localeId = ValueTypeHelper.Int32TryParse(Convert.ToString(this.DataSourceSet.Parameters[23].Value), 0);

                #endregion

                var legacyDataAccessService = new LegacyDataAccessService();

                legacyDataAccessService.UpdateApplicationConfigXML(applicationContextDefinationId, applicationConfigParentId, shortName, longName, eventSourceId, eventSubscriberId,
                    orgId, catalogId, categoryId, entityId, attributeId, entityTypeId, relationshipTypeId, roleId, userId, configXml, description, preCondition,
                    postCondition, xsdSchema, sampleXml, loginUser, programName, localeId);

                this._isConfigDirty = false;

                if (treeView is TreeView)
                    explorerTreeView = treeView as TreeView;

                //Get the saved config from database
                DataTable configTable = this.ApplicationContext.GetData(null, null) as DataTable;

                //Updating config Id, shortname, longname
                foreach (DataRow configRow in configTable.Rows)
                {
                    Int32 configId = 0;
                    Int32.TryParse(configRow["FK_Application_ConfigParent"].ToString(), out configId);

                    if ((this.Object.InheritedParent != null && this.Object.InheritedParent.Id == configId) || this.Object.Id == 0)
                    {
                        this.Id = (int)configRow["PK_Application_Config"];
                        _dataSourceSet.Id = this.Id;
                        this._object.Id = this.Id;

                        this.ShortName = configRow["ShortName"].ToString();
                        this.LongName = configRow["LongName"].ToString();

                        if (explorerTreeView != null)
                        {
                            if (!string.IsNullOrEmpty(this.LongName.Trim()) && this.Id > 0)
                                treeNodeText = ((this.Id < 10) ? "0" + this.Id.ToString() : this.Id.ToString()) + " : " + this.LongName;
                            else if (!string.IsNullOrEmpty(this.Name.Trim()))
                                treeNodeText = this.Name;
                            else
                                treeNodeText = this.GetType().Name;

                            explorerTreeView.SelectedNode.Text = treeNodeText;
                            explorerTreeView.SelectedNode.ToolTipText = treeNodeText;
                            explorerTreeView.SelectedNode.Name = treeNodeText;
                        }
                    }
                }

                if (treeView != null)
                    MessageBox.Show("Configuration saved successfully.", "Application Configuration Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Trace.WriteLine("Configuration saved successfully.", RS.MDM.Logging.LogLevel.INFO.ToString());
            }
            catch (Exception ex)
            {
                if (treeView != null && ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.Message, "Application Configuration Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="baseClassName"></param>
        private List<string> GetTypesByBaseClassName(string assemblyPath, string baseClassName)
        {
            List<string> _typeNames = new List<string>();
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
            if (_assembly != null)
            {

                foreach (Type _type in _assembly.GetTypes())
                {
                    if (_type.IsPublic && ((_type.Attributes & TypeAttributes.Abstract) != TypeAttributes.Abstract))
                    {
                        try
                        {
                            for (Type _baseType = _type.BaseType; _baseType.FullName != "System.Object"; _baseType = _baseType.BaseType)
                            {
                                if (_baseType.FullName == baseClassName)
                                {
                                    _typeNames.Add(_type.FullName);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return _typeNames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="typeName"></param>
        private void AddObject(string assemblyPath, string typeName)
        {
            try
            {
                System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
                if (_assembly != null)
                {
                    Type _type = _assembly.GetType(typeName);
                    if (_type != null)
                    {
                        object _instance = _assembly.CreateInstance(typeName);
                        if (_instance != null)
                        {
                            this.Object = _instance as RS.MDM.Object;
                            TypeDescriptor.Refresh(this);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _object_NameChangedEvent(Object sender, NameChangedEventArgs e)
        {
            this.Name = e.NewName;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();
            if (this._object != null)
            {
                _object.GenerateNewUniqueIdentifier();
            }
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            //if (this._object != null)
            //{
            //    this._object.Parent = this;
            //}
            if (this._dataSourceSet != null)
            {
                this._dataSourceSet.Parent = this;
                this._dataSourceSet.InheritedParent = this.InheritedParent;
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
            _treeNode.SelectedImageKey = _treeNode.ImageKey;
            if (this._object != null)
            {
                _treeNode.Nodes.Add(this._object.GetTreeNode());
            }
            if (this._applicationContext != null)
            {
                _treeNode.Nodes.Add(this._applicationContext.GetTreeNode());
            }
            if (this._dataSourceSet != null)
            {
                _treeNode.Nodes.Add(this._dataSourceSet.GetTreeNode());
            }

            return _treeNode;
        }

        #endregion

    }
}
