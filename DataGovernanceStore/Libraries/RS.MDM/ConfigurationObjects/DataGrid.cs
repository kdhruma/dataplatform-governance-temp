using System;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Web.UI.WebControls;


namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents a data of the DataGrid config
    /// </summary>
    [XmlRoot("DataGrid")]
    [Serializable()]
    public sealed class DataGrid : Object
    {
        #region Fields

        /// <summary>
        /// Represents the OnComplete property of data grid
        /// </summary>
        private String _onComplete = String.Empty;

        /// <summary>
        /// Represents the multi select of data grid
        /// </summary>
        private Boolean _multiSelect = true;

        /// <summary>
        /// Represents width of the data grid
        /// </summary>
        private String _width = String.Empty;

        /// <summary>
        /// Represents height of the data grid
        /// </summary>
        private String _height = String.Empty;

        /// <summary>
        /// Represents Records Per Page of the data grid
        /// </summary>
        private Int32 _recordsPerPage = 0;

        /// <summary>
        /// Represents the Pager Size Options of the data grid
        /// </summary>
        private String _pagerSizeOptions = String.Empty;

        /// <summary>
        /// Represents the maximum number of records to be fetched
        /// </summary>
        private Int32 _maxResultsCount = 200;

        /// <summary>
        /// Represents the Data Columns
        /// </summary>
        private RS.MDM.Collections.Generic.List<DataColumn> _dataColumns = new RS.MDM.Collections.Generic.List<DataColumn>();

        /// <summary>
        /// Indicates default sort column for grid
        /// </summary>
        private String _defaultSortColumn = String.Empty;

        /// <summary>
        /// Indicates default grouping column for grid
        /// </summary>
        private String _defaultGroupingColumn = String.Empty;

        /// <summary>
        /// Represents the multi sorting of data grid
        /// </summary>
        private Boolean _multiSort = true;

        /// <summary>
        /// Indicates sort direction for default sort column
        /// </summary>
        private SortDirection _defaultSortOrder = SortDirection.Ascending;

        /// <summary>
        /// Represents the parameter list for the DataGrid
        /// </summary>
        private RS.MDM.Collections.Generic.List<Parameter> _parameters = new Collections.Generic.List<Parameter>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the Data Columns
        /// </summary>
        [Category("DataColumns")]
        [Description("Represents the list of Data Columns")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<DataColumn> DataColumns
        {
            get
            {
                this.SetParent();
                return this._dataColumns;
            }
            set
            {
                this._dataColumns = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Represents the On Complete property of data grid
        /// </summary>
        [XmlAttribute("OnComplete")]
        [Category("Properties")]
        [Description("Represents the On Complete property of data grid")]
        [TrackChanges()]
        public String OnComplete
        {
            get
            {
                return this._onComplete;
            }
            set
            {
                this._onComplete = value;
            }
        }

        /// <summary>
        /// Represents multiSelect property of data grid
        /// </summary>
        [XmlAttribute("MultiSelect")]
        [Category("Properties")]
        [Description("Represents multiSelect property of data grid")]
        [TrackChanges()]
        public Boolean MultiSelect
        {
            get
            {
                return this._multiSelect;
            }
            set
            {
                this._multiSelect = value;
            }
        }

        /// <summary>
        /// Represents multiSort property of data grid
        /// </summary>
        [XmlAttribute("MultiSort")]
        [Category("Properties")]
        [Description("Represents multiSort property of data grid")]
        [TrackChanges()]
        public Boolean MultiSort
        {
            get
            {
                return this._multiSort;
            }
            set
            {
                this._multiSort = value;
            }
        }

        /// <summary>
        /// Represents width of the data grid
        /// </summary>
        [XmlAttribute("Width")]
        [Category("Properties")]
        [Description("Represents width of the data grid")]
        [TrackChanges()]
        public String Width
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
        /// Represents height of the data grid
        /// </summary>
        [XmlAttribute("Height")]
        [Category("Properties")]
        [Description("Represents height of the data grid")]
        [TrackChanges()]
        public String Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        /// <summary>
        /// Represents records per page of the data grid
        /// </summary>
        [XmlAttribute("RecordsPerPage")]
        [Category("Properties")]
        [Description("Represents records per page of the data grid")]
        [TrackChanges()]
        public Int32 RecordsPerPage
        {
            get
            {
                return this._recordsPerPage;
            }
            set
            {
                this._recordsPerPage = value;
            }
        }

        /// <summary>
        /// Represents pager size options of the data grid
        /// </summary>
        [XmlAttribute("PagerSizeOptions")]
        [Category("Properties")]
        [Description("Represents pager size options of the data grid")]
        [TrackChanges()]
        public String PagerSizeOptions
        {
            get
            {
                return this._pagerSizeOptions;
            }
            set
            {
                this._pagerSizeOptions = value;
            }
        }

        /// <summary>
        /// Represents the maximum number of records to be fetched
        /// </summary>
        [XmlAttribute("MaxResultsCount")]
        [Category("Properties")]
        [Description("Represents the maximum number of records to be fetched")]
        [TrackChanges()]
        public Int32 MaxResultsCount
        {
            get
            {
                return this._maxResultsCount;
            }
            set
            {
                this._maxResultsCount = value;
            }
        }

        /// <summary>
        /// Represents the default sort column of grid
        /// </summary>
        [XmlAttribute("DefaultSortColumn")]
        [Category("Properties")]
        [Description("Represents the default sort column of grid")]
        [TrackChanges()]
        public String DefaultSortColumn
        {
            get
            {
                return this._defaultSortColumn;
            }
            set
            {
                this._defaultSortColumn = value;
            }
        }

        /// <summary>
        /// Represents the default sort column of grid
        /// </summary>
        [XmlAttribute("DefaultGroupingColumn")]
        [Category("Properties")]
        [Description("Represents the default grouping column of grid")]
        [TrackChanges()]
        public String DefaultGroupingColumn
        {
            get
            {
                return this._defaultGroupingColumn;
            }
            set
            {
                this._defaultGroupingColumn = value;
            }
        }

        /// <summary>
        /// Represents the default sort column of grid
        /// </summary>
        [XmlAttribute("SortOrder")]
        [Category("Properties")]
        [Description("Represents the default sort order for of grid")]
        [TrackChanges()]
        public SortDirection DefaultSortOrder
        {
            get
            {
                return this._defaultSortOrder;
            }
            set
            {
                this._defaultSortOrder = value;
            }
        }

        /// <summary>
        /// Represents the parameter list for the DataGrid
        /// </summary>
        [Category("Parameters")]
        [Description("Represents the parameter list for the DataGrid")]
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

        /// <summary>
        /// Initializes a new instance of the DataGrid class.
        /// </summary>
        public DataGrid()
            : base()
        {
            this.AddVerb("Add Data Column");
            this.AddVerb("Add Parameter");
        }

        /// <summary>
        /// Initializes a new instance of the DataGrid class using Xml (generated by ToXml() method)
        /// </summary>
        public DataGrid(String xmlValue)
            : base()
        {
            this.AddVerb("Add Data Column");
            this.AddVerb("Add Parameter");
            LoadDataGrid(xmlValue);
        }

        #endregion

        #region Methods

        #endregion

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

        private void LoadDataGrid(String xmlValue)
        {
            if (!String.IsNullOrWhiteSpace(xmlValue))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(xmlValue, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataGrid")
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

                                if (reader.MoveToAttribute("OnComplete"))
                                {
                                    this.OnComplete = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("MultiSelect"))
                                {
                                    Boolean val = true;
                                    if (Boolean.TryParse(reader.ReadContentAsString(),out val))
                                    {
                                        this.MultiSelect = val;
                                    }
                                }

                                if (reader.MoveToAttribute("MultiSort"))
                                {
                                    Boolean val = true;
                                    if (Boolean.TryParse(reader.ReadContentAsString(), out val))
                                    {
                                        this.MultiSort = val;
                                    }
                                }

                                if (reader.MoveToAttribute("Width"))
                                {
                                    this.Width = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Height"))
                                {
                                    this.Height = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RecordsPerPage"))
                                {
                                    Int32 val = this._recordsPerPage;
                                    if(Int32.TryParse(reader.ReadContentAsString(), out val))
                                    {
                                        this.RecordsPerPage =  val;
                                    }
                                }

                                if (reader.MoveToAttribute("PagerSizeOptions"))
                                {
                                    this.PagerSizeOptions =  reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PagerSizeOptions"))
                                {
                                    this.PagerSizeOptions =  reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DefaultSortColumn"))
                                {
                                    this.DefaultSortColumn =  reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DefaultSortColumn"))
                                {
                                    this.DefaultSortColumn =  reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DefaultGroupingColumn"))
                                {
                                    this.DefaultGroupingColumn =  reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DefaultSortOrder"))
                                {
                                    SortDirection val = this._defaultSortOrder;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out val))
                                    {
                                        this.DefaultSortOrder = val;
                                    }
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataColumns")
                        {
                            #region Read DataColumns

                            String dataColumnsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dataColumnsXml))
                            {
                                this.DataColumns = LoadDataColumns(dataColumnsXml);
                            }

                            #endregion Read DataColumns
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Parameters")
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

        private RS.MDM.Collections.Generic.List<DataColumn> LoadDataColumns(String parameterAsXml)
        {
            RS.MDM.Collections.Generic.List<DataColumn> dataColumnList = new Collections.Generic.List<DataColumn>();

            if (!String.IsNullOrWhiteSpace(parameterAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(parameterAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataColumn")
                        {
                            DataColumn dataColumn = new DataColumn(reader.ReadOuterXml());

                            dataColumnList.Add(dataColumn);
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

            return dataColumnList;
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
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (DataColumn column in this._dataColumns)
            {
                if (column != null)
                {
                    column.GenerateNewUniqueIdentifier();
                }
            }

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    param.GenerateNewUniqueIdentifier();
                }
            }
        }

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

            foreach (DataColumn column in this._dataColumns)
            {
                if (column != null)
                {
                    list.AddRange(column.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

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
            if (this._dataColumns != null)
            {
                foreach (DataColumn column in this._dataColumns)
                {
                    if (column != null)
                    {
                        column.Parent = this;
                        column.InheritedParent = this.InheritedParent;
                    }
                }
            }

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

            if (this._dataColumns != null && this._dataColumns.Count > 0)
            {
                for (int i = _dataColumns.Count - 1; i > -1; i--)
                {
                    DataColumn column = _dataColumns[i];

                    if (column != null)
                    {
                        if (column.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._dataColumns.Remove(column);
                        }
                        else
                        {
                            column.AcceptChanges();
                        }
                    }
                }
            }

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

            if (this._dataColumns != null)
            {
                foreach (DataColumn column in this._dataColumns)
                {
                    if (column != null)
                    {
                        column.FindChanges();
                    }
                }
            }

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

            if (this._dataColumns != null)
            {
                previousSibling = string.Empty;
                foreach (DataColumn column in this._dataColumns)
                {
                    if (column != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(column.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            DataColumn _dataItemClone = RS.MDM.Object.Clone(column, false) as DataColumn;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((DataGrid)inheritedChild).DataColumns.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            column.FindDeletes(_items[0]);
                        }

                        previousSibling = column.UniqueIdentifier;
                    }
                }
            }

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
                            ((TreeNode)inheritedChild).Parameters.InsertAfter(previousSibling, _dataItemClone);
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

                DataGrid _inheritedParent = inheritedParent as DataGrid;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (DataColumn column in this._dataColumns)
                {
                    switch (column.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            DataColumn _dataItemClone = RS.MDM.Object.Clone(column, false) as DataColumn;
                            _inheritedParent.DataColumns.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            DataColumn _inheritedChild = _inheritedParent.DataColumns.GetItem(column.UniqueIdentifier);
                            column.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = column.UniqueIdentifier;
                }

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

                foreach (DataColumn column in this._dataColumns)
                {
                    if (column.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.DataColumns.Remove(column.UniqueIdentifier);
                    }
                    else
                    {
                        DataColumn _inheritedChild = _inheritedParent.DataColumns.GetItem(column.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }

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

            System.Windows.Forms.TreeNode datacolumns = new System.Windows.Forms.TreeNode("DataColumns");
            datacolumns.ImageKey = "DataColumn";
            datacolumns.SelectedImageKey = datacolumns.ImageKey;
            datacolumns.Tag = this.DataColumns;
            treeNode.Nodes.Add(datacolumns);

            foreach (DataColumn column in this._dataColumns)
            {
                if (column != null)
                {
                    datacolumns.Nodes.Add(column.GetTreeNode());
                }
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
                case "Add Data Column":
                    this.DataColumns.Add(new DataColumn());
                    break;
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
        /// <returns>XML representation of DataGrid</returns>
        public override String ToXml()
        {
            using (var sw = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(sw))
            {
                #region DataGrid Node

                //Parameter node start
                xmlWriter.WriteStartElement("DataGrid");

                xmlWriter.WriteAttributeString("ClassName", this.ClassName);
                xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
                xmlWriter.WriteAttributeString("OnComplete", this.OnComplete);
                xmlWriter.WriteAttributeString("MultiSelect", this.MultiSelect.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("MultiSort", this.MultiSort.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("Width", this.Width.ToString());
                xmlWriter.WriteAttributeString("Height", this.Height.ToString());
                xmlWriter.WriteAttributeString("RecordsPerPage", this.RecordsPerPage.ToString());
                xmlWriter.WriteAttributeString("PagerSizeOptions", this.PagerSizeOptions);
                xmlWriter.WriteAttributeString("MaxResultCount", this.MaxResultsCount.ToString());
                xmlWriter.WriteAttributeString("DefaultSortColumn", this.DefaultSortColumn.ToString());
                xmlWriter.WriteAttributeString("DefaultGroupingColumn", this.DefaultGroupingColumn.ToString());
                xmlWriter.WriteAttributeString("DefaultSortOrder", this.DefaultSortOrder.ToString());

                #region Data Column Node

                xmlWriter.WriteStartElement("DataColumns");

                foreach (DataColumn column in this.DataColumns)
                {
                    xmlWriter.WriteRaw(column.ToXml());
                }

                xmlWriter.WriteEndElement();

                #endregion Data Column Node

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

                #endregion DataGrid Node

                xmlWriter.Flush();

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

            if (this.DataColumns.Count == 0)
            {
                validationErrors.Add("The Data Grid does not contain any Column.", ValidationErrorType.Warning, "Data Grid", this);
            }

            if (this.Parameters.Count == 0)
            {
                validationErrors.Add("The DataGrid does not contain any Parameter.", ValidationErrorType.Information, "Data Grid", this);
            }
        }

        #endregion
    }
}
