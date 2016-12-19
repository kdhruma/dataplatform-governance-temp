using System;
using System.Xml.Serialization;
using System.ComponentModel;

using MDM.Core;
using RS.MDM.Configuration;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configuration for AttributePanelItem
    /// </summary>
    [XmlRoot("AttributePanelItem")]
    [Serializable()]
    public sealed class AttributePanelItem : Object
    {
        #region Fields

        /// <summary>
        /// field for Attribute Name of the AttributePanelItem
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// field for Attribute Group Name of the AttributePanelItem
        /// </summary>
        private String _attributeGroupName = String.Empty;

        /// <summary>
        /// field for type of the AttributePanelItem
        /// </summary>
        private AttributeDataType _type = AttributeDataType.String;

        /// <summary>
        /// field for source of the AttributePanelItem
        /// </summary>
        private AttributePanelItemSource _source = AttributePanelItemSource.Unknown;

        /// <summary>
        /// field for rule of the AttributePanelItem
        /// </summary>
        private String _rule = String.Empty;

        /// <summary>
        /// field for value of the AttributePanelItem
        /// </summary>
        private Double _value = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates Attribute Name of the AttributePanelItem
        /// </summary>
        [XmlAttribute("AttributeName")]
        [Description("Indicates Attribute Name of the AttributePanelItem.")]
        [Category("AttributePanelItem")]
        [TrackChanges()]
        public String AttributeName
        {
            get
            {
                return this._attributeName;
            }
            set
            {
                this._attributeName = value;
            }
        }

        /// <summary>
        /// Indicates Attribute Group Name of the AttributePanelItem
        /// </summary>
        [XmlAttribute("AttributeGroupName")]
        [Description("Indicates Attribute Group Name of the AttributePanelItem.")]
        [Category("AttributePanelItem")]
        [TrackChanges()]
        public String AttributeGroupName
        {
            get
            {
                return this._attributeGroupName;
            }
            set
            {
                this._attributeGroupName = value;
            }
        }

        /// <summary>
        /// Indicates Type of the AttributePanelItem
        /// </summary>
        [XmlAttribute("Type")]
        [Description("Indicates Type of the AttributePanelItem.")]
        [Category("AttributePanelItem")]
        [TrackChanges()]
        public AttributeDataType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        /// <summary>
        /// Indicates Source of the AttributePanelItem
        /// </summary>
        [XmlAttribute("Source")]
        [Description("Indicates Source of the AttributePanelItem.")]
        [Category("AttributePanelItem")]
        [TrackChanges()]
        public AttributePanelItemSource Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }

        /// <summary>
        /// Indicates Rule of the AttributePanelItem
        /// </summary>
        [XmlAttribute("Rule")]
        [Description("Indicates Rule of the AttributePanelItem.")]
        [Category("AttributePanelItem")]
        [TrackChanges()]
        public String Rule
        {
            get
            {
                return this._rule;
            }
            set
            {
                this._rule = value;
            }
        }

        /// <summary>
        /// Indicates Value of the AttributePanelItem
        /// </summary>
        [XmlAttribute("Value")]
        [Description("Indicates Value of the AttributePanelItem.")]
        [Category("AttributePanelItem")]
        [TrackChanges()]
        public Double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public AttributePanelItem()
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