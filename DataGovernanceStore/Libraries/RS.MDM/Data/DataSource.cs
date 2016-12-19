using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Drawing.Design;
using RS.MDM.Validations;
using RS.MDM.Configuration;
using System.Windows.Forms;

namespace RS.MDM.Data
{
    /// <summary>
    /// Provides configuration functionality for the Grid and Toolbar functionality
    /// </summary>
    [XmlRoot("DataSource")]
    [Serializable()]
    public class DataSource : RS.MDM.Object
    {

        #region Fields

        /// <summary>
        /// field for the assembly of the data source
        /// </summary>
        private string _assembly = string.Empty;

        /// <summary>
        /// field for the type in the assembly
        /// </summary>
        private string _typeName = string.Empty;

        /// <summary>
        /// field for the method in the type
        /// </summary>
        private string _methodName = string.Empty;

        /// <summary>
        /// field for the method type
        /// </summary>
        private System.Reflection.BindingFlags _methodType = System.Reflection.BindingFlags.Static;

        /// <summary>
        /// field for the parameters in the method
        /// </summary>
        private RS.MDM.Collections.Generic.List<Parameter> _parameters = new RS.MDM.Collections.Generic.List<Parameter>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataSource()
            : base()
        {
            this.AddVerb("Add Parameter");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the assembly that contains the api to invoke to fetch data
        /// </summary>
        [Description("Indicates the name of the assembly that contains the type and method to fetch the data")]
        [XmlAttribute("Assembly")]
        [Category("Shell Builder")]
        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Assembly
        {
            get
            {
                return _assembly;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._assembly = value;
                }
                else
                {
                    if (value.LastIndexOf(@"\") >= 0)
                    {
                        this._assembly = (value.Substring(value.LastIndexOf(@"\") + 1));
                    }
                    else
                    {
                        this._assembly = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of type that contains the method to invoke to fetch the data
        /// </summary>
        [Description("Indicates the name of the Type that contains the method to fetch the data")]
        [XmlAttribute("TypeName")]
        [Category("Shell Builder")]
        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the method that is used to invoke and fetch the data
        /// </summary>
        [Description("Indicates the name of the method to fetch the data")]
        [XmlAttribute("MethodName")]
        [Category("Shell Builder")]
        public string MethodName
        {
            get
            {
                return _methodName;
            }
            set { _methodName = value; }
        }

        /// <summary>
        /// Gets or sets the type of method
        /// </summary>
        [Description("Indicates the type of Method in the Type")]
        [XmlAttribute("MethodType")]
        [Category("Shell Builder")]
        public System.Reflection.BindingFlags MethodType
        {
            get
            {
                return _methodType;
            }
            set
            {
                _methodType = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameters required as input to the method to fetch the data
        /// </summary>
        [Description("Indicates the input parameters of the method for fetching the data")]
        [Category("Shell Builder")]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<RS.MDM.Data.Parameter> Parameters
        {
            get
            {
                this.SetParent();
                return this._parameters;
            }
            set
            {
                this._parameters = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Gets or Sets a parameter by its name
        /// </summary>
        /// <param name="parameterName">Indicates the name of the parameter</param>
        /// <returns></returns>
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter this[string parameterName]
        {
            get
            {
                foreach (Parameter _parameter in this.Parameters)
                {
                    if (_parameter.Name == parameterName)
                    {
                        return _parameter;
                    }
                }
                return null;
            }
            set
            {
                if (value.Name != parameterName)
                {
                    throw new ArgumentException("The argument name is not valid");
                }
                this.Parameters.Remove(this[parameterName]);
                this.Parameters.Add(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data by invoking the method
        /// </summary>
        /// <param name="inputParameters">Indicates the input parameters to invoke the method</param>
        /// <returns>An object</returns>
        public object GetData(object[] inputParameters)
        {
            string _assemblyPath = null;
            if (System.Web.HttpContext.Current != null)
            {
                _assemblyPath = System.Web.HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + @"bin\" + this.Assembly;
            }
            else
            {
                _assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + this.Assembly;
                if (!System.IO.File.Exists(_assemblyPath))
                {
                    _assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + @"bin\" + this.Assembly;
                }
            }
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFrom(_assemblyPath);
            if (_assembly != null)
            {
                Type _type = _assembly.GetType(this._typeName);
                if (_type != null)
                {
                    if (this._methodType == System.Reflection.BindingFlags.Static)
                    {
                        System.Reflection.MethodInfo _methodInfo = _type.GetMethod(this._methodName, this._methodType | System.Reflection.BindingFlags.Public, null, this.GetInputParameterTypes(), null);
                        if (_methodInfo != null)
                        {
                            try
                            {
                                return _methodInfo.Invoke(null, inputParameters);
                            }
                            catch ( Exception ex )
                            {
                                if ( ex.InnerException != null )
                                {
                                    throw ex.InnerException;
                                }
                                else
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                    else
                    {
                        object _instance = _assembly.CreateInstance(this._typeName);
                        if (_instance != null)
                        {
                            System.Reflection.MethodInfo _methodInfo = _instance.GetType().GetMethod(this._methodName, this._methodType | System.Reflection.BindingFlags.Public, null, this.GetInputParameterTypes(), null);
                            if (_methodInfo != null)
                            {
                                try
                                {
                                    return _methodInfo.Invoke(_instance, inputParameters);
                                }
                                catch ( Exception ex )
                                {
                                    if ( ex.InnerException != null )
                                    {
                                        throw ex.InnerException;
                                    }
                                    else
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the data by invoking the method
        /// </summary>
        /// <param name="page">Indicates a web page that is used to extract data for input parameters</param>
        /// <param name="keyValuePairs">Indicates keyvalue pairs that will also be used to extract data for input parameters</param>
        /// <returns>An object</returns>
        public object GetData(System.Web.UI.Page page, System.Collections.Hashtable keyValuePairs)
        {
            List<object> _inputValues = new List<object>();
            foreach (Parameter _parameter in this.Parameters)
            {
                _inputValues.Add(_parameter.GetValue(page, keyValuePairs));
            }
            object[] inputParameters = _inputValues.ToArray();

            object _data = this.GetData(inputParameters);

            int parameterCount = 0;
            foreach (Parameter _parameter in this.Parameters)
            {
                if (_parameter.Direction == ParameterDirection.Output)
                {
                    _parameter.Value = inputParameters[parameterCount];
                }

                parameterCount++;
            }

            return _data;
        }

        public object GetChildData(System.Data.SqlTypes.SqlInt32 parentConfigId)
        {
            string _assemblyPath = null;
            List<Type> _types = new List<Type>();
            List<object> _inputValues = new List<object>();

            _types.Add(typeof(System.Data.SqlTypes.SqlInt32));
            _inputValues.Add(parentConfigId);

            if (System.Web.HttpContext.Current != null)
            {
                _assemblyPath = System.Web.HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + @"bin\" + this.Assembly;
            }
            else
            {
                _assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + this.Assembly;
                if (!System.IO.File.Exists(_assemblyPath))
                {
                    _assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + @"bin\" + this.Assembly;
                }
            }
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFrom(_assemblyPath);
            if (_assembly != null)
            {
                Type _type = _assembly.GetType(this._typeName);
                if (_type != null)
                {
                    if (this._methodType == System.Reflection.BindingFlags.Static)
                    {
                        System.Reflection.MethodInfo _methodInfo = _type.GetMethod("GetChildApplicationConfigsXML", this._methodType | System.Reflection.BindingFlags.Public, null,_types.ToArray() ,null);
                        if (_methodInfo != null)
                        {
                            return _methodInfo.Invoke(null, _inputValues.ToArray());
                        }
                    }
                    else
                    {
                        object _instance = _assembly.CreateInstance(this._typeName);
                        if (_instance != null)
                        {
                            System.Reflection.MethodInfo _methodInfo = _instance.GetType().GetMethod(this._methodName, this._methodType | System.Reflection.BindingFlags.Public);
                            if (_methodInfo != null)
                            {
                                return _methodInfo.Invoke(_instance, _inputValues.ToArray());
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Type[] GetInputParameterTypes()
        {
            List<Type> _types = new List<Type>();
            foreach (Parameter _parameter in this.Parameters)
            {
                Type _type = _parameter.GetParameterDataType();
                if (_parameter.Direction == ParameterDirection.Input)
                {
                    _types.Add(_type);
                }
                else
                {
                    _types.Add(_type.MakeByRefType());
                }
            }
            return _types.ToArray();
        }

        /// <summary>
        /// Executes logic for a given verb
        /// </summary>
        /// <param name="text">Indicates the verb text that needs to be executed</param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        public override void OnDesignerVerbClick(string text, Object configObject, object treeView)
        {
            Configuration.ConfigurationObject configurationObject = null;
            base.OnDesignerVerbClick(text, configObject, treeView);

            switch (text)
            {
                case "Add Parameter":
                    this._parameters.Add(new Parameter());

                    if (configObject != null && configObject is Configuration.ConfigurationObject)
                    {
                        configurationObject = configObject as Configuration.ConfigurationObject;
                        configurationObject._isConfigDirty = true;
                    }
                    break;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a dictionary of datasources for a given xml data
        /// </summary>
        /// <param name="xmlConfiguration">Indicates xml string of serialized datasources</param>
        /// <returns>A dictionary of DataSources</returns>
        public static Dictionary<string, DataSource> CreateDataSources(string xmlConfiguration)
        {
            Dictionary<string, DataSource> _dataSources = new Dictionary<string, DataSource>();
            System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();
            _xmlDoc.LoadXml(xmlConfiguration);
            System.Xml.XmlNodeList _dataSourcesXMLNodeList = _xmlDoc.DocumentElement.SelectNodes("//DataSource");
            foreach (System.Xml.XmlNode _dataSourceXMLNode in _dataSourcesXMLNodeList)
            {
                DataSource _dataSource = (DataSource)Object.XMLDeserialize(_dataSourceXMLNode.OuterXml, typeof(DataSource));
                if (_dataSource != null)
                {
                    _dataSources.Add(_dataSource.Name, _dataSource);
                }
            }
            return _dataSources;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Generates new unique identifier for the DataSource
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();
            foreach (Parameter _parameter in this._parameters)
            {
                if (_parameter != null)
                {
                    _parameter.GenerateNewUniqueIdentifier();
                }
            }
        }

        /// <summary>
        /// Sets the Parent (Container) for the DataSource
        /// </summary>
        public override void SetParent()
        {
            if (this._parameters != null)
            {
                foreach (Parameter _parameter in this._parameters)
                {
                    if (_parameter != null)
                    {
                        _parameter.Parent = this;
                        _parameter.InheritedParent = this.InheritedParent;
                    }
                }
            }
        }

        /// <summary>
        /// Accepts the property changes.
        /// </summary>
        public override void AcceptChanges()
        {
            base.AcceptChanges();
            if (this._parameters != null && this._parameters.Count > 0)
            {
                for (int i = _parameters.Count - 1; i > -1; i--)
                {
                    Parameter _parameter = _parameters[i];
                    if (_parameter != null)
                    {
                        if (_parameter.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._parameters.Remove(_parameter);
                        }
                        else
                        {
                            _parameter.AcceptChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Find Changes wrt to inherited parent (instance)
        /// </summary>
        public override void FindChanges()
        {
            base.FindChanges();
            if (this._parameters != null)
            {
                foreach (Parameter _parameter in this._parameters)
                {
                    if (_parameter != null)
                    {
                        _parameter.FindChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Find deleted items in an inherited child
        /// </summary>
        /// <param name="inheritedChild">Indicates an inherited child</param>
        public override void FindDeletes(Object inheritedChild)
        {
            base.FindDeletes(inheritedChild);
            if (this._parameters != null)
            {
                string _previousSibling = string.Empty;
                foreach (Parameter _parameter in this._parameters)
                {
                    if (_parameter != null)
                    {
                        List<RS.MDM.Object> _parametesLocal = inheritedChild.FindChildren(_parameter.UniqueIdentifier, true);
                        if (_parametesLocal.Count == 0)
                        {
                            Parameter _parameterClone = RS.MDM.Object.Clone(_parameter, false) as Parameter;
                            _parameterClone.PropertyChanges.Items.Clear();
                            _parameterClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((DataSource)inheritedChild).Parameters.InsertAfter(_previousSibling, _parameterClone);
                        }
                        else
                        {
                            _parameter.FindDeletes(_parametesLocal[0]);
                        }
                        _previousSibling = _parameter.UniqueIdentifier;
                    }
                }
            }
        }


        /// <summary>
        /// Finds children for a given uniqueidentifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates an unique identifier that needs to be used for the search</param>
        /// <param name="includeDeletedItems">Indicates if deleted items to be included in the search</param>
        /// <returns>A list of objects that match the unique identifier</returns>
        public override List<RS.MDM.Object> FindChildren(string uniqueIdentifier, bool includeDeletedItems)
        {
            List<RS.MDM.Object> _list = new List<Object>();
            _list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));
            foreach (Parameter _parameter in this._parameters)
            {
                _list.AddRange(_parameter.FindChildren(uniqueIdentifier, includeDeletedItems));
            }
            return _list;
        }

        /// <summary>
        /// Inherits a parent object
        /// </summary>
        /// <param name="inheritedParent">Indicates an object that needs to be inherited</param>
        public override void InheritParent(RS.MDM.Object inheritedParent)
        {
            if (inheritedParent != null)
            {
                base.InheritParent(inheritedParent);
                DataSource _inheritedParent = inheritedParent as DataSource;
                string _previousSibling = string.Empty;

                // Apply all the changes
                foreach (Parameter _parameter in this._parameters)
                {
                    switch (_parameter.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Parameter _parameterClone = RS.MDM.Object.Clone(_parameter, false) as Parameter;
                            _inheritedParent.Parameters.InsertAfter(_previousSibling, _parameterClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Parameter _inheritedChild = _inheritedParent.Parameters.GetItem(_parameter.UniqueIdentifier);
                            _parameter.InheritParent(_inheritedChild);
                            break;
                    }
                    _previousSibling = _parameter.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (Parameter _parameter in this._parameters)
                {
                    if (_parameter.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Parameters.Remove(_parameter.UniqueIdentifier);
                    }
                    else
                    {
                        _inheritedParent.Parameters.GetItem(_parameter.UniqueIdentifier).PropertyChanges.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a treenode that represents the datasource and its aggregated children
        /// </summary>
        /// <returns>A treenode</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            TreeNode _treeNode = base.GetTreeNode();
            if (this.PropertyChanges.ObjectStatus == InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "Data";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }
            TreeNode _parameterNodes = new TreeNode("Parameters");
            _parameterNodes.ImageKey = "Items";
            _parameterNodes.ToolTipText = "Parameters";
            _parameterNodes.SelectedImageKey = _parameterNodes.ImageKey;
            _parameterNodes.Tag = this.Parameters;
            _treeNode.Nodes.Add(_parameterNodes);
            foreach (Parameter _parameter in this._parameters)
            {
                if (_parameter != null)
                {
                    _parameterNodes.Nodes.Add(_parameter.GetTreeNode());
                }
            }
            return _treeNode;
        }

        #endregion

        #region Validations

        /// <summary>
        /// Validates the DataSource for errors
        /// </summary>
        /// <param name="validationErrors">Indicates a validation error collection that will be populated with errors</param>
        public override void Validate(ref ValidationErrorCollection validationErrors)
        {
            if (validationErrors == null)
            {
                validationErrors = new RS.MDM.Validations.ValidationErrorCollection();
            }
            if (string.IsNullOrEmpty(this._assembly))
            {
                validationErrors.Add("The Assembly Name is not set", ValidationErrorType.Error, "Assembly", this);
            }
            if (string.IsNullOrEmpty(this._typeName))
            {
                validationErrors.Add("The Type Name is not set", ValidationErrorType.Error, "TypeName", this);
            }
            if (string.IsNullOrEmpty(this._methodName))
            {
                validationErrors.Add("The Method Name is not set", ValidationErrorType.Error, "MethodName", this);
            }
            foreach (Parameter _parameter in this._parameters)
            {
                if (_parameter != null)
                {
                    _parameter.Validate(ref validationErrors);
                }
            }
        }

        #endregion

    }
}
