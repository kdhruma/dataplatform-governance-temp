using System;
using System.Xml.Serialization;
using System.ComponentModel;

using RS.MDM.Configuration;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configuration for ContainerPanelItem
    /// </summary>
    [XmlRoot("ContainerPanelItem")]
    [Serializable()]
    public sealed class ContainerPanelItem : Object
    {
        #region Fields

        /// <summary>
        /// field for Container Id of the ContainerPanelItem
        /// </summary>
        private String _containerId = "0";

        /// <summary>
        /// field for Container Name of the ContainerPanelItem
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// field for include Org Ids of the ContainerPanelItem
        /// </summary>
        private String _includeOrgIds = String.Empty;

        /// <summary>
        /// field for exclude Container Ids of the ContainerPanelItem
        /// </summary>
        private String _excludeContainerIds = String.Empty;

        /// <summary>
        /// field for min occurs of the ContainerPanelItem
        /// </summary>
        private String _minOccurs = "0";

        /// <summary>
        /// field for max occurs of the ContainerPanelItem
        /// </summary>
        private String _maxOccurs = "1";

        /// <summary>
        /// field for multi select of the ContainerPanelItem
        /// </summary>
        private Boolean _multiSelect = false;

        /// <summary>
        /// field for filter column of the ContainerPanelItem
        /// </summary>
        private String _filterColumn = String.Empty;

        /// <summary>
        /// field for display columns of the ContainerPanelItem
        /// </summary>
        private String _displayColumns = String.Empty;

        /// <summary>
        /// field for text field of the ContainerPanelItem
        /// </summary>
        private String _textField = String.Empty;

        /// <summary>
        /// field for value field of the ContainerPanelItem
        /// </summary>
        private String _valueField = String.Empty;

        /// <summary>
        /// field for showing level of the ContainerPanelItem
        /// </summary>
        private String _showLevel = "ALL";
       
        /// <summary>
        /// field for RS table width of the ContainerPanelItem
        /// </summary>
        private String _rsTableWidth = "0";

        /// <summary>
        /// field for RS table height of the ContainerPanelItem
        /// </summary>
        private String _rsTableHeight = "0";

        /// <summary>
        /// field for width of the ContainerPanelItem
        /// </summary>
        private String _width = "0";

        /// <summary>
        /// field for allow delete of the ContainerPanelItem
        /// </summary>
        private Boolean _allowDelete = false;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates ContainerId of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("ContainerId")]
        [Description("Indicates ContainerId of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                this._containerId = value;
            }
        }

        /// <summary>
        /// Indicates ContainerName of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("ContainerName")]
        [Description("Indicates ContainerName of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String ContainerName
        {
            get
            {
                return this._containerName;
            }
            set
            {
                this._containerName = value;
            }
        }

        /// <summary>
        /// Indicates IncludeOrgIds of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("IncludeOrgIds")]
        [Description("Indicates IncludeOrgIds of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String IncludeOrgIds
        {
            get
            {
                return this._includeOrgIds;
            }
            set
            {
                this._includeOrgIds = value;
            }
        }

        /// <summary>
        /// Indicates ExcludeContainerIds of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("ExcludeContainerIds")]
        [Description("Indicates ExcludeContainerIds of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String ExcludeContainerIds
        {
            get
            {
                return this._excludeContainerIds;
            }
            set
            {
                this._excludeContainerIds = value;
            }
        }

        /// <summary>
        /// Indicates MinOccurs of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("MinOccurs")]
        [Description("Indicates MinOccurs of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String MinOccurs
        {
            get
            {
                return this._minOccurs;
            }
            set
            {
                this._minOccurs = value;
            }
        }

        /// <summary>
        /// Indicates MaxOccurs of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("MaxOccurs")]
        [Description("Indicates MaxOccurs of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String MaxOccurs
        {
            get
            {
                return this._maxOccurs;
            }
            set
            {
                this._maxOccurs = value;
            }
        }

        /// <summary>
        /// Indicates MultiSelect of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("MultiSelect")]
        [Description("Indicates MultiSelect of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
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
        /// Indicates FilterColumn of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("FilterColumn")]
        [Description("Indicates FilterColumn of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String FilterColumn
        {
            get
            {
                return this._filterColumn;
            }
            set
            {
                this._filterColumn = value;
            }
        }

        /// <summary>
        /// Indicates DisplayColumns of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("DisplayColumns")]
        [Description("Indicates DisplayColumns of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String DisplayColumns
        {
            get
            {
                return this._displayColumns;
            }
            set
            {
                this._displayColumns = value;
            }
        }

        /// <summary>
        /// Indicates TextField of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("TextField")]
        [Description("Indicates TextField of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String TextField
        {
            get
            {
                return this._textField;
            }
            set
            {
                this._textField = value;
            }
        }

        /// <summary>
        /// Indicates ValueField of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("ValueField")]
        [Description("Indicates ValueField of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String ValueField
        {
            get
            {
                return this._valueField;
            }
            set
            {
                this._valueField = value;
            }
        }

        /// <summary>
        /// Indicates ShowLevel of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("ShowLevel")]
        [Description("Indicates ShowLevel of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String ShowLevel
        {
            get
            {
                return this._showLevel;
            }
            set
            {
                this._showLevel = value;
            }
        }

        /// <summary>
        /// Indicates RS Table Width of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("RS_TableWidth")]
        [Description("Indicates RS Table Width of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String RSTableWidth
        {
            get
            {
                return this._rsTableWidth;
            }
            set
            {
                this._rsTableWidth = value;
            }
        }

        /// <summary>
        /// Indicates RS Table Height of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("RS_TableHeight")]
        [Description("Indicates RS Table sHeight of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public String RSTableHeight
        {
            get
            {
                return this._rsTableHeight;
            }
            set
            {
                this._rsTableHeight = value;
            }
        }

        /// <summary>
        /// Indicates Width of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("Width")]
        [Description("Indicates Width of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
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
        /// Indicates AllowDelete of the ContainerPanelItem
        /// </summary>
        [XmlAttribute("AllowDelete")]
        [Description("Indicates AllowDelete of the ContainerPanelItem.")]
        [Category("ContainerPanelItem")]
        [TrackChanges()]
        public Boolean AllowDelete
        {
            get
            {
                return this._allowDelete;
            }
            set
            {
                this._allowDelete = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public ContainerPanelItem()
            : base()
        {
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Overrides

        /// <summary>
        /// Get a tree node that represents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "UIColumn";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            return _treeNode;
        }

        #endregion

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}