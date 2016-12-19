using System;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents the type of the data field
    /// </summary>
    public enum DataFieldTypeEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Attribute = 1,

        /// <summary>
        /// 
        /// </summary>
        DataColumn = 2
    }

    /// <summary>
    /// Represents the
    /// </summary>
    public enum DataTypeEnum
    {

        /// <summary>
        /// Text
        /// </summary>
        Text = 1,

        /// <summary>
        /// Integer
        /// </summary>
        Int = 2,

        /// <summary>
        /// Decimal
        /// </summary>
        Float = 3,

        /// <summary>
        /// DateTime
        /// </summary>
        Date = 4
    }

    /// <summary>
    /// Represents a column of the data grid
    /// </summary>
    [XmlRoot("DataColumn")]
    [Serializable()]
    public sealed class DataColumn : Object
    {
        #region Fields

        /// <summary>
        /// Represents the index of the data column
        /// </summary>
        private String _index = String.Empty;

        /// <summary>
        /// Represents the type of the data field
        /// </summary>
        private DataFieldTypeEnum _dataFieldType = DataFieldTypeEnum.DataColumn;

        /// <summary>
        /// Represents the data field. Attribute Id in case of attribute data field type.
        /// </summary>
        private String _dataField = String.Empty;

        /// <summary>
        /// Represents whether the column is hidden or visible
        /// </summary>
        private Boolean _hidden = false;

        /// <summary>
        /// Represents the Width of the data column
        /// </summary>
        private Int32 _width = 0;

        /// <summary>
        /// Represents the Sortable property of data column
        /// </summary>
        private Boolean _sortable = false;

        /// <summary>
        /// Represents the event handler name on click of the column
        /// </summary>
        private String _onClick = String.Empty;

        /// <summary>
        /// Represents the URL of data column
        /// </summary>
        private String _url = String.Empty;

        /// <summary>
        /// Indicates the type of Column. Default is text
        /// </summary>
        private DataTypeEnum _dataType = DataTypeEnum.Text;

        /// <summary>
        /// Represents the column data formatting rules for future displaying to user
        /// </summary>
        private String _dataFormattingRules = String.Empty;

        /// <summary>
        /// Represents the column data parsing rules
        /// </summary>
        private String _dataParsingRules = String.Empty;


        /// <summary>
        /// Represents the column data formatter name
        /// </summary>
        private String _formatter = String.Empty;

        /// <summary>
        /// Represents the parameter list for the DataColumn
        /// </summary>
        private RS.MDM.Collections.Generic.List<Parameter> _parameters = new Collections.Generic.List<Parameter>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the Index of the Data column
        /// </summary>
        [XmlAttribute("Index")]
        [Category("Properties")]
        [Description("Represents the Index of the Data column")]
        [TrackChanges()]
        public String Index
        {
            get
            {
                return this._index;
            }
            set
            {
                this._index = value;
            }
        }

        /// <summary>
        /// Represents the type of the data field
        /// </summary>
        [XmlAttribute("DataFieldType")]
        [Category("Properties")]
        [Description("Represents the type of the data field")]
        [TrackChanges()]
        public DataFieldTypeEnum DataFieldType
        {
            get
            {
                return this._dataFieldType;
            }
            set
            {
                this._dataFieldType = value;
            }
        }

        /// <summary>
        /// Represents the Url of data column
        /// </summary>
        [XmlAttribute("DataType")]
        [Category("Properties")]
        [Description("Represents the DataType of data column")]
        [TrackChanges()]
        public DataTypeEnum DataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }

        /// <summary>
        /// Represents the data field. Attribute Id in case of attribute data field type.
        /// </summary>
        [XmlAttribute("DataField")]
        [Category("Properties")]
        [Description("Represents the data field. Attribute Id in case of attribute data field type.")]
        [TrackChanges()]
        public String DataField
        {
            get
            {
                return this._dataField;
            }
            set
            {
                this._dataField = value;
            }
        }

        /// <summary>
        /// Represents whether the column is hidden or visible
        /// </summary>
        [XmlAttribute("Hidden")]
        [Category("Properties")]
        [Description("Represents whether the column is hidden or visible")]
        [TrackChanges()]
        public Boolean Hidden
        {
            get
            {
                return this._hidden;
            }
            set
            {
                this._hidden = value;
            }
        }

        /// <summary>
        /// Represents the width of the data column
        /// </summary>
        [XmlAttribute("Width")]
        [Category("Properties")]
        [Description("Represents the width of the data column")]
        [TrackChanges()]
        public Int32 Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        /// <summary>
        /// Represents the Sortable property of data column
        /// </summary>
        [XmlAttribute("Sortable")]
        [Category("Properties")]
        [Description("Represents the Sortable property of data column")]
        [TrackChanges()]
        public Boolean Sortable
        {
            get
            {
                return this._sortable;
            }
            set
            {
                this._sortable = value;
            }
        }

        /// <summary>
        /// Represents the event handler name on click of the column
        /// </summary>
        [XmlAttribute("OnClick")]
        [Category("Properties")]
        [Description("Represents the event handler name on click of the column")]
        [TrackChanges()]
        public String OnClick
        {
            get
            {
                return this._onClick;
            }
            set
            {
                this._onClick = value;
            }
        }

        /// <summary>
        /// Represents the Url of data column
        /// </summary>
        [XmlAttribute("Url")]
        [Category("Properties")]
        [Description("Represents the Url of data column")]
        [TrackChanges()]
        public String Url
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
        }

        /// <summary>
        /// Represents the column data formatting rules for future displaying to user
        /// </summary>
        [XmlAttribute("DataFormattingRules")]
        [Category("Properties")]
        [Description("Represents the column data formatting rules for displaying to user")]
        [TrackChanges()]
        public String DataFormattingRules
        {
            get
            {
                return this._dataFormattingRules;
            }
            set
            {
                this._dataFormattingRules = value;
            }
        }

        /// <summary>
        /// Represents the column data parsing rules
        /// </summary>
        [XmlAttribute("DataParsingRules")]
        [Category("Properties")]
        [Description("Represents the column data parsing rules")]
        [TrackChanges()]
        public String DataParsingRules
        {
            get
            {
                return this._dataParsingRules;
            }
            set
            {
                this._dataParsingRules = value;
            }
        }

        /// <summary>
        /// Represents the column data formatter name
        /// </summary>
        [XmlAttribute("Formatter")]
        [Category("Properties")]
        [Description("Represents the column data formatter name")]
        [TrackChanges()]
        public String Formatter
        {
            get
            {
                return this._formatter;
            }
            set
            {
                this._formatter = value;
            }
        }

        /// <summary>
        /// Represents the parameter list for the DataColumn
        /// </summary>
        [Category("Parameters")]
        [Description("Represents the parameter list for the DataColumn")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<Parameter> Parameters
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

        #endregion

        #region Constructor

        public DataColumn()
            : base()
        {
        }

        public DataColumn(String xmlValue)
        {
            LoadDataColumn(xmlValue);
        }

        #endregion Constructor

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    param.GenerateNewUniqueIdentifier();
                }
            }
        }

        private void LoadDataColumn(String xmlValue)
        {
            if (!String.IsNullOrWhiteSpace(xmlValue))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(xmlValue, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataColumn")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ClassName"))
                                {
                                    this.ClassName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AssemblyName"))
                                {
                                    this.AssemblyName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Index"))
                                {
                                    this.Index = reader.ReadContentAsString();
                                }


                                if (reader.MoveToAttribute("DataFieldType"))
                                {
                                    DataFieldTypeEnum fieldType = this._dataFieldType;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out fieldType))
                                    {
                                        this.DataFieldType = fieldType;
                                    }
                                }

                                if (reader.MoveToAttribute("DataField"))
                                {
                                    this.DataField = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Hidden"))
                                {
                                    Boolean val = this._hidden;
                                    if (Boolean.TryParse(reader.ReadContentAsString(), out val))
                                    {
                                        this.Hidden = val;
                                    }
                                }

                                if (reader.MoveToAttribute("Width"))
                                {
                                    Int32 val = this._width;
                                    if(Int32.TryParse(reader.ReadContentAsString(), out val))
                                    {
                                    this.Width = val;
                                    }
                                }

                                if (reader.MoveToAttribute("Sortable"))
                                {
                                    Boolean val = this._sortable;
                                    if(Boolean.TryParse(reader.ReadContentAsString(), out val))
                                    {
                                    this.Sortable = val;
                                    }
                                }

                                if (reader.MoveToAttribute("OnClick"))
                                {
                                    this.OnClick = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DataType"))
                                {
                                    DataTypeEnum val = this._dataType;
                                    if(Enum.TryParse(reader.ReadContentAsString(), out val))
                                    {
                                    this.DataType = val;
                                    }
                                }

                                if (reader.MoveToAttribute("DataFormattingRules"))
                                {
                                    this.DataFormattingRules = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DataParsingRules"))
                                {
                                    this.DataParsingRules = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Formatter"))
                                {
                                    this.Formatter = reader.ReadContentAsString();
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Params")
                        {
                            String parmsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(parmsXml))
                            {
                                this.Parameters = LoadParameters(parmsXml);
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

        private RS.MDM.Collections.Generic.List<Parameter> LoadParameters(String parameterAsXml)
        {
            RS.MDM.Collections.Generic.List<Parameter> parameters = new Collections.Generic.List<Parameter>();

            if (!String.IsNullOrWhiteSpace(parameterAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(parameterAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Param")
                        {
                            Parameter parameter = new Parameter();
                            parameter.LoadParameter(reader.ReadOuterXml());

                            parameters.Add(parameter);
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

            return parameters;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Finds and returns a list of child objects for a given unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates an unique identifier of an object that needs to be found</param>
        /// <param name="includeDeletedItems">Indicates if deleted items to be included in the search</param>
        /// <returns>A list of Child objects matching the given unique identifier</returns>
        public override List<RS.MDM.Object> FindChildren(string uniqueIdentifier, bool includeDeletedItems)
        {
            List<RS.MDM.Object> list = new List<Object>();
            list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    list.AddRange(param.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._parameters != null)
            {
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        param.Parent = this;
                        param.InheritedParent = this.InheritedParent;
                    }
                }
            }
        }

        /// <summary>
        /// Accepts the changes to the object
        /// </summary>
        public override void AcceptChanges()
        {
            base.AcceptChanges();

            if (this._parameters != null && this._parameters.Count > 0)
            {
                for (int i = _parameters.Count - 1; i > -1; i--)
                {
                    Parameter param = _parameters[i];

                    if (param != null)
                    {
                        if (param.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._parameters.Remove(param);
                        }
                        else
                        {
                            param.AcceptChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the changes of an object with respect to an instance of an inherited parent
        /// </summary>
        public override void FindChanges()
        {
            base.FindChanges();

            if (this._parameters != null)
            {
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        param.FindChanges();
                    }
                }
            }

            if (this.Parent == null && this.InheritedParent != null)
            {
                this.InheritedParent.FindDeletes(this);
            }
        }

        /// <summary>
        /// Finds deleted children of an inherited child
        /// </summary>
        /// <param name="inheritedChild">Indicates the inherited child</param>
        public override void FindDeletes(Object inheritedChild)
        {
            base.FindDeletes(inheritedChild);

            string previousSibling = string.Empty;

            if (this._parameters != null)
            {
                previousSibling = string.Empty;
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(param.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            Parameter _dataItemClone = RS.MDM.Object.Clone(param, false) as Parameter;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((DataColumn)inheritedChild).Parameters.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            param.FindDeletes(_items[0]);
                        }

                        previousSibling = param.UniqueIdentifier;
                    }
                }
            }
        }

        /// <summary>
        /// Inherits a parent object (instance)
        /// </summary>
        /// <param name="inheritedParent">Indicates an instance of an object that needs to be inherited</param>
        public override void InheritParent(RS.MDM.Object inheritedParent)
        {
            if (inheritedParent != null)
            {
                base.InheritParent(inheritedParent);

                DataColumn _inheritedParent = inheritedParent as DataColumn;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (Parameter param in this._parameters)
                {
                    switch (param.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Parameter _dataItemClone = RS.MDM.Object.Clone(param, false) as Parameter;
                            _inheritedParent.Parameters.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Parameter _inheritedChild = _inheritedParent.Parameters.GetItem(param.UniqueIdentifier);
                            param.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = param.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (Parameter param in this._parameters)
                {
                    if (param.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Parameters.Remove(param.UniqueIdentifier);
                    }
                    else
                    {
                        Parameter _inheritedChild = _inheritedParent.Parameters.GetItem(param.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Get a tree node that represents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                treeNode.ImageKey = "NavigationPane";
                treeNode.SelectedImageKey = treeNode.ImageKey;
            }

            System.Windows.Forms.TreeNode parameters = new System.Windows.Forms.TreeNode("Parameters");
            parameters.ImageKey = "Parameters";
            parameters.SelectedImageKey = parameters.ImageKey;
            parameters.Tag = this.Parameters;
            treeNode.Nodes.Add(parameters);

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    parameters.Nodes.Add(param.GetTreeNode());
                }
            }

            return treeNode;
        }

        /// <summary>
        /// Execute logic related to a given verb
        /// </summary>
        /// <param name="text">Indicate the text that represents a supported verb</param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        public override void OnDesignerVerbClick(string text, Object configObject, object treeView)
        {
            ConfigurationObject configurationObject = null;

            base.OnDesignerVerbClick(text, configObject, treeView);

            switch (text)
            {
                case "Add Parameter":
                    this.Parameters.Add(new Parameter());
                    break;
            }

            if (text != "Find Changes" && text != "Accept Changes" && configObject != null && configObject is ConfigurationObject)
            {
                configurationObject = configObject as ConfigurationObject;
                configurationObject._isConfigDirty = true;
            }

            System.ComponentModel.TypeDescriptor.Refresh(this);
        }

        /// <summary>
        /// Get XML representation of the object
        /// </summary>
        /// <returns>XML representation of Data Column</returns>
        public override String ToXml()
        {
            using (var sw = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(sw))
            {
                #region Data Column Node

                //MenuItem node start
                xmlWriter.WriteStartElement("DataColumn");

                xmlWriter.WriteAttributeString("ClassName", this.ClassName);
                xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
                xmlWriter.WriteAttributeString("Name", this.GetLocaleMessage(this.Name));
                xmlWriter.WriteAttributeString("Index", this.Index);
                xmlWriter.WriteAttributeString("DataFieldType", this.DataFieldType.ToString());
                xmlWriter.WriteAttributeString("DataField", this.DataField);
                xmlWriter.WriteAttributeString("Hidden", this.Hidden.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("Width", this.Width.ToString());
                xmlWriter.WriteAttributeString("Sortable", this.Sortable.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("OnClick", this.OnClick);
                xmlWriter.WriteAttributeString("DataType", this.DataType.ToString());
                xmlWriter.WriteAttributeString("DataFormattingRules", this.DataFormattingRules);
                xmlWriter.WriteAttributeString("DataParsingRules", this.DataParsingRules);
                xmlWriter.WriteAttributeString("Formatter", this.Formatter);

                #region Parameters Node

                xmlWriter.WriteStartElement("Parameters");

                if (this.Parameters != null)
                {
                    foreach (Parameter param in this.Parameters)
                    {
                        xmlWriter.WriteRaw(param.ToXml());
                    }
                }
                xmlWriter.WriteEndElement();

                #endregion Parameters Node

                //Value node end
                xmlWriter.WriteEndElement();

                #endregion Data Column Node

                xmlWriter.Flush();

                //Return the actual XML
                return sw.ToString();
            }
        }

        #endregion

        #region Validations

        /// <summary>
        /// Validates an object and aggregates all the validation exceptions
        /// </summary>
        /// <param name="validationErrors">A container to aggregate all the validation exceptions</param>
        public override void Validate(ref ValidationErrorCollection validationErrors)
        {
            this.SetParent();

            if (validationErrors == null)
            {
                validationErrors = new RS.MDM.Validations.ValidationErrorCollection();
            }

            if (this.Parameters.Count == 0)
            {
                validationErrors.Add("The DataColumn does not contain any Parameter.", ValidationErrorType.Information, "Data Column", this);
            }
        }

        #endregion    

    }
}
