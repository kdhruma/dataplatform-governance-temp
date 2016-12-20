using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;

namespace RS.MDM.Data
{

    /// <summary>
    /// Indicates the source type from which data will be extracted for input to the method invocation
    /// </summary>
    public enum ParameterSourceType
    {
        /// <summary>
        /// Indicates a session variable
        /// </summary>
        SessionVariable,

        /// <summary>
        /// Indicates a session pointer variable
        /// </summary>
        SessionPointerVariable,

        /// <summary>
        /// Indicates a query string parameter
        /// </summary>
        QueryStringParameter,

        /// <summary>
        /// Indicates a master key in the master table
        /// </summary>
        MasterKey,

        /// <summary>
        /// Indicates a column in the master table
        /// </summary>
        ColumnData,

        /// <summary>
        /// Indicates a request parameter
        /// </summary>
        RequestParameter,

        /// <summary>
        /// Indicates default value
        /// </summary>
        DefaultValue
    }

    /// <summary>
    /// Enumerates the sql types that are expected in the shell builder methods.
    /// </summary>
    public enum SqlType
    {
        /// <summary>
        /// A binary value
        /// </summary>
        SqlBinary,

        /// <summary>
        /// A boolean value
        /// </summary>
        SqlBoolean,

        /// <summary>
        /// A byte value
        /// </summary>
        SqlByte,

        /// <summary>
        /// A DateTime value
        /// </summary>
        SqlDateTime,

        /// <summary>
        /// A decimal value
        /// </summary>
        SqlDecimal,

        /// <summary>
        /// A double value
        /// </summary>
        SqlDouble,

        /// <summary>
        /// A Guid value
        /// </summary>
        SqlGuid,

        /// <summary>
        /// A 16bit integer value
        /// </summary>
        SqlInt16,

        /// <summary>
        /// A 32bit integer value
        /// </summary>
        SqlInt32,

        /// <summary>
        /// A 64bit integer value
        /// </summary>
        SqlInt64,

        /// <summary>
        /// A datatype that represents money
        /// </summary>
        SqlMoney,

        /// <summary>
        /// A single precision value
        /// </summary>
        SqlSingle,

        /// <summary>
        /// A string value
        /// </summary>
        SqlString,

        /// <summary>
        /// An xml value
        /// </summary>
        SqlXml,
        
        /// <summary>
        /// A boolean value
        /// </summary>
        Boolean,

        /// <summary>
        /// A byte value
        /// </summary>
        Byte,

        /// <summary>
        /// A Char value
        /// </summary>
        Char,

        /// <summary>
        /// A DateTime value
        /// </summary>
        DateTime,

        /// <summary>
        /// A decimal value
        /// </summary>
        Decimal,

        /// <summary>
        /// A double value
        /// </summary>
        Double,

        /// <summary>
        /// A Guid value
        /// </summary>
        Guid,

        /// <summary>
        /// A 16bit integer value
        /// </summary>
        Int16,

        /// <summary>
        /// A 32bit integer value
        /// </summary>
        Int32,

        /// <summary>
        /// A 64bit integer value
        /// </summary>
        Int64,

        /// <summary>
        /// An Object value
        /// </summary>
        Object,

        /// <summary>
        /// A SByte value
        /// </summary>
        SByte,

        /// <summary>
        /// A single precision value
        /// </summary>
        Single,

        /// <summary>
        /// A string value
        /// </summary>
        String,

        /// <summary>
        /// A 16bit unsigned integer value
        /// </summary>
        UInt16,

        /// <summary>
        /// A 32bit unsigned integer value
        /// </summary>
        UInt32,

        /// <summary>
        /// A 64bit unsigned integer value
        /// </summary>
        UInt64
    }

    /// <summary>
    /// Provides functionality to extract data from different pages and pass as arguments to the methods for fetching data.
    /// </summary>
    [XmlRoot("Parameter")]
    [Serializable()]
    public class Parameter : RS.MDM.Object
    {
        #region Fields

        /// <summary>
        /// field for data type of the parameter
        /// </summary>
        private RS.MDM.Data.SqlType _dataType = SqlType.SqlString;

        /// <summary>
        /// field for the default value
        /// </summary>
        private string _defaultValue = string.Empty;

        /// <summary>
        /// field for the value of the parameter at runtime
        /// </summary>
        private object _value = null;

        /// <summary>
        /// field for the direction of the parameter
        /// </summary>
        private ParameterDirection _direction = ParameterDirection.Input;

        /// <summary>
        /// field for the name of the source field 
        /// </summary>
        private string _sourceFieldName = string.Empty;

        /// <summary>
        /// field for the type of the source field
        /// </summary>
        private ParameterSourceType _parameterSourceType = ParameterSourceType.DefaultValue;


        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Parameter()
            : base()
        {
            this.Name = "";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the parameter at runtime
        /// </summary>
        [Description("Indicates the actual value of the Parameter at Runtime")]
        [Category("Runtime Value")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [ReadOnly(true)]
        [XmlIgnore()]
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Gets or sets the data type of the Parameter
        /// </summary>
        [Description("Indicates the data type of the parameter")]
        [XmlAttribute("DataType")]
        public SqlType DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        /// <summary>
        /// Gets or sets the input direction of the parameter
        /// </summary>
        [Description("Indicates the direction of the parameter")]
        [XmlAttribute("Direction")]
        public ParameterDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the source field
        /// </summary>
        [Description("Indicates the name of the field from which the data needs to be read for the parameter")]
        [XmlAttribute("SourceFieldName")]
        [Category("Parameter Input Source")]
        public string SourceFieldName
        {
            get
            {
                return _sourceFieldName;
            }
            set
            {
                _sourceFieldName = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of source field
        /// </summary>
        [Description("Indicates the source from which to read the input for the parameter")]
        [XmlAttribute("ParameterSourceType")]
        [Category("Parameter Input Source")]
        public ParameterSourceType ParameterSourceType
        {
            get
            {
                return _parameterSourceType;
            }
            set
            {
                _parameterSourceType = value;
            }
        }

        /// <summary>
        /// Gets or sets the default value of the parameter
        /// </summary>
        [Description("Indicates the default value of the Parameter")]
        [XmlAttribute("DefaultValue")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Parameter Input Source")]
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value of the parameter
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return this.ConvertToSqlType(this._defaultValue);
        }

        /// <summary>
        /// Gets the type of parameter data
        /// </summary>
        /// <returns>The type of the parameter data</returns>
        public Type GetParameterDataType()
        {
            switch (this._dataType)
            {
                case SqlType.SqlBoolean:
                    return typeof(SqlBoolean);
                case SqlType.SqlByte:
                    return typeof(SqlByte);
                case SqlType.SqlDateTime:
                    return typeof(SqlDateTime);
                case SqlType.SqlDecimal:
                    return typeof(SqlDecimal);
                case SqlType.SqlDouble:
                    return typeof(SqlDouble);
                case SqlType.SqlGuid:
                    return typeof(SqlGuid);
                case SqlType.SqlInt16:
                    return typeof(SqlInt16);
                case SqlType.SqlInt32:
                    return typeof(SqlInt32);
                case SqlType.SqlInt64:
                    return typeof(SqlInt64);
                case SqlType.SqlMoney:
                    return typeof(SqlMoney);
                case SqlType.SqlSingle:
                    return typeof(SqlSingle);
                case SqlType.SqlString:
                    return typeof(SqlString);
                case SqlType.SqlXml:
                    return typeof(SqlXml);
                    
                case SqlType.Boolean:
                    return typeof(Boolean);
                case SqlType.Byte:
                    return typeof(Byte);
                case SqlType.Char:
                    return typeof(Char);
                case SqlType.DateTime:
                    return typeof(DateTime);
                case SqlType.Decimal:
                    return typeof(Decimal);
                case SqlType.Double:
                    return typeof(Double);
                case SqlType.Guid:
                    return typeof(Guid);
                case SqlType.Int16:
                    return typeof(Int16);
                case SqlType.Int32:
                    return typeof(Int32);
                case SqlType.Int64:
                    return typeof(Int64);
                case SqlType.Object:
                    return typeof(Object);
                case SqlType.SByte:
                    return typeof(SByte);
                case SqlType.Single:
                    return typeof(Single);
                case SqlType.String:
                    return typeof(String);
                case SqlType.UInt16:
                    return typeof(UInt16);
                case SqlType.UInt32:
                    return typeof(UInt32);
                case SqlType.UInt64:
                    return typeof(UInt64);
                default:
                    return null;
                    
            }
        }

        /// <summary>
        /// Converts a string value to sql type
        /// </summary>
        /// <param name="input">Indicates an input in string format</param>
        /// <returns>An object that is converted to a sql type</returns>
        public object ConvertToSqlType(string input)
        {
            if (input == null)
            {
                return null;
            }
            switch (this._dataType)
            {
                case SqlType.SqlBoolean:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlBoolean.Null;
                    }
                    else
                    {
                        return SqlBoolean.Parse(input);
                    }
                case SqlType.SqlByte:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlByte.Null;
                    }
                    else
                    {
                        return SqlByte.Parse(input);
                    }
                case SqlType.SqlDateTime:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlDateTime.Null;
                    }
                    else
                    {
                        return SqlDateTime.Parse(input);
                    }
                case SqlType.SqlDecimal:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlDecimal.Null;
                    }
                    else
                    {
                        return SqlDecimal.Parse(input);
                    }
                case SqlType.SqlDouble:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlDouble.Null;
                    }
                    else
                    {
                        return SqlDouble.Parse(input);
                    }
                case SqlType.SqlGuid:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlGuid.Null;
                    }
                    else
                    {
                        return SqlGuid.Parse(input);
                    }
                case SqlType.SqlInt16:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlInt16.Null;
                    }
                    else
                    {
                        return SqlInt16.Parse(input);
                    }
                case SqlType.SqlInt32:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlInt32.Null; 
                    }
                    else
                    {
                        return SqlInt32.Parse(input);
                    }
                case SqlType.SqlInt64:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlInt64.Null;
                    }
                    else
                    {
                        return SqlInt64.Parse(input);
                    }
                case SqlType.SqlMoney:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlMoney.Null;
                    }
                    else
                    {
                        return SqlMoney.Parse(input);
                    }
                case SqlType.SqlSingle:
                    if (string.IsNullOrEmpty(input))
                    {
                        return SqlSingle.Null;
                    }
                    else
                    {
                        return SqlSingle.Parse(input);
                    }
                case SqlType.SqlString:
                    return new SqlString(input);
                case SqlType.SqlXml:
                    System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create(new System.IO.StringReader(input));
                    SqlXml sqlXml = new SqlXml(_xmlReader);

                    _xmlReader.Close();

                    return sqlXml;
                case SqlType.Boolean:
                    if (string.IsNullOrEmpty(input))
                    {
                        return false;
                    }
                    else
                    {
                        return Boolean.Parse(input);
                    }
                case SqlType.Byte:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Byte.Parse(input);
                    }
                case SqlType.Char:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Char.Parse(input);
                    }
                case SqlType.DateTime:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return DateTime.Parse(input);
                    }
                case SqlType.Decimal:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Decimal.Parse(input);
                    }
                case SqlType.Double:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Double.Parse(input);
                    }
                case SqlType.Int16:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Int16.Parse(input);
                    }
                case SqlType.Int32:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Int32.Parse(input);
                    }
                case SqlType.Int64:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Int64.Parse(input);
                    }
                case SqlType.SByte:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return SByte.Parse(input);
                    }
                case SqlType.Single:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return Single.Parse(input);
                    }
                case SqlType.UInt16:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return UInt16.Parse(input);
                    }
                case SqlType.UInt32:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return UInt32.Parse(input);
                    }
                case SqlType.UInt64:
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    else
                    {
                        return UInt64.Parse(input);
                    }
                default:
                    return input;
            }
        }

        /// <summary>
        /// Gets a value by invoking the method
        /// </summary>
        /// <param name="page">A web page that is used to extract input parameters</param>
        /// <param name="keyValuePairs">A keyvalue pair that is used to extract input parameters</param>
        /// <returns></returns>
        public object GetValue(System.Web.UI.Page page, System.Collections.Hashtable keyValuePairs)
        {
            string _inputValue = string.Empty;
            switch (this.ParameterSourceType)
            {
                case ParameterSourceType.MasterKey:
                    if (keyValuePairs != null && keyValuePairs.ContainsKey(this.SourceFieldName))
                    {
                        _inputValue = keyValuePairs[this.SourceFieldName].ToString();
                    }
                    else
                    {
                        _inputValue = this.DefaultValue;
                    }

                    break;
                case ParameterSourceType.QueryStringParameter:
                    try
                    {
                        if (page != null)
                        {
                            if (this.ContainsKey(page.Request.QueryString.AllKeys, this.SourceFieldName))
                            {
                                _inputValue = page.Request.QueryString[this.SourceFieldName];
                            }
                            else
                            {
                                _inputValue = this.DefaultValue;
                            }
                        }
                        else
                        {
                            _inputValue = this.DefaultValue;
                        }
                    }
                    catch
                    {
                        _inputValue = this.DefaultValue;
                    }

                    break;
                case ParameterSourceType.SessionVariable:
                    try
                    {
                        if (page != null)
                        {
                            if (page.Session[this.SourceFieldName] != null)
                                _inputValue = page.Session[this.SourceFieldName].ToString();
                            else
                                _inputValue = this.DefaultValue;
                        }
                        else
                        {
                            _inputValue = this.DefaultValue;
                        }
                    }
                    catch
                    {
                        _inputValue = this.DefaultValue;
                    }
                    break;
                case ParameterSourceType.SessionPointerVariable:
                    try
                    {
                        if (page != null)
                        {
                            string _pointer = string.Empty;
                            if (this.ContainsKey(page.Request.QueryString.AllKeys, this.SourceFieldName))
                            {
                                _pointer = page.Request.QueryString[this.SourceFieldName];
                                if (!string.IsNullOrEmpty(_pointer))
                                {
                                    _inputValue = page.Session[_pointer].ToString();
                                }
                                else
                                {
                                    _inputValue = this.DefaultValue;
                                }
                            }
                            else
                            {
                                _inputValue = this.DefaultValue;
                            }

                        }
                        else
                        {
                            _inputValue = this.DefaultValue;
                        }
                    }
                    catch
                    {
                        _inputValue = this.DefaultValue;
                    }
                    break;
                case ParameterSourceType.ColumnData:
                    if (keyValuePairs != null && keyValuePairs.ContainsKey(this.SourceFieldName))
                    {
                        _inputValue = keyValuePairs[this.SourceFieldName].ToString();
                    }
                    else
                    {
                        _inputValue = this.DefaultValue;
                    }
                    break;
                case ParameterSourceType.RequestParameter:
                    try
                    {
                        if (page != null)
                        {
                            if (this.ContainsKey(page.Request.Form.AllKeys, this.SourceFieldName))
                            {
                                _inputValue = page.Request.Form[this.SourceFieldName];
                            }
                            else
                            {
                                _inputValue = this.DefaultValue;
                            }
                        }
                        else
                        {
                            _inputValue = this.DefaultValue;
                        }
                    }
                    catch
                    {
                        _inputValue = this.DefaultValue;
                    }
                    break;
                case ParameterSourceType.DefaultValue:
                    _inputValue = this.DefaultValue;
                    break;
            }
            return this.ConvertToSqlType(_inputValue);
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

        #endregion

        #region Overrides

        /// <summary>
        /// Get a list of properties that are displayed in the designer
        /// </summary>
        /// <param name="properties">Indicates a superset of properties</param>
        /// <returns>A list of filtered properties that are displayed in the designer</returns>
        protected override PropertyDescriptorCollection GetProperties(PropertyDescriptorCollection properties)
        {
            properties = base.GetProperties(properties);
            PropertyDescriptorCollection _props = new PropertyDescriptorCollection(null);

            if (this.Parent == null)
            {
                return properties;
            }
            else if (this.Parent is RS.MDM.Configuration.Data.DataSourceGet || this.Parent is RS.MDM.Configuration.Data.DataSourceSet)
            {

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
                            case "UniqueIdentifier":
                                continue;
                            default:
                                _props.Add(_prop);
                                break;
                        }
                    }
                }
                return _props;
            }
            else
            {
                return properties;
            }
        }

        /// <summary>
        /// Gets a TreeNode that represents the Parameter
        /// </summary>
        /// <returns>A treenode that represents the parameter</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();
            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "Parameter";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }
            return _treeNode;
        }

        #endregion
    }
}
