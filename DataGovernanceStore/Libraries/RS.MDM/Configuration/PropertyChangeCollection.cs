using System;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Windows.Forms;


namespace RS.MDM.Configuration
{

    #region InheritedObjectStatus

    /// <summary>
    /// Indicates the status of an inherited object instance
    /// </summary>
    public enum InheritedObjectStatus
    {
        /// <summary>
        /// Indicates an object that is added to the inherited child
        /// </summary>
        Add,

        /// <summary>
        /// Indicates a deleted object in the inherited child
        /// </summary>
        Delete,

        /// <summary>
        /// Indicates a changed object in the inherited child
        /// </summary>
        Change,

        /// <summary>
        /// Indicates an object that has not changed in the inherited child
        /// </summary>
        None

    }

    #endregion

    /// <summary>
    /// Provides functionality for aggregating property changes of an inherited object (instance)
    /// </summary>
    [XmlRoot("PropertyChanges")]
    [Serializable()]
    public sealed class PropertyChangeCollection
    {
        #region Fields

        /// <summary>
        /// field for the object status
        /// </summary>
        private InheritedObjectStatus _objectStatus = InheritedObjectStatus.None;

        /// <summary>
        /// field for the list of property changes
        /// </summary>
        private List<PropertyChange> _items = new List<PropertyChange>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public PropertyChangeCollection()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the status of Inherited Object in the inheritance heirarchy
        /// </summary>
        [Description("Indicates the status of Inherited Object in the inheritance heirarchy")]
        [Category("Changes")]
        [XmlAttribute("ObjectStatus")]
        public InheritedObjectStatus ObjectStatus
        {
            get
            {
                return this._objectStatus;
            }
            set
            {
                this._objectStatus = value;
            }
        }

        /// <summary>
        /// Indicates the property change collection
        /// </summary>
        [Description("Indicates the property change collection")]
        [Category("Changes")]
        [XmlElement("PropertyChange")]
        public List<PropertyChange> Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a property change to the collection
        /// </summary>
        /// <param name="name">Indicates the name of the property</param>
        /// <param name="parentValue">Indicates the value of the parent property</param>
        /// <param name="childValue">Indicates the value of the child property</param>
        public void Add(string name, string parentValue, string childValue)
        {
            PropertyChange _propertyChange = new PropertyChange(name, parentValue, childValue);
            this._items.Add(_propertyChange);
        }

        /// <summary>
        /// Resets the property change collection by clearing it.
        /// </summary>
        public void Reset()
        {
            this._objectStatus = InheritedObjectStatus.None;
            this._items.Clear();
        }

        /// <summary>
        /// Get a TreeNode that represents the property change collection
        /// </summary>
        /// <returns>A TreeNode that represents the property change collection</returns>
        public TreeNode GetTreeNode()
        {
            TreeNode _treeNode = new TreeNode();
            _treeNode.Tag = this;
            _treeNode.ImageKey = this.ObjectStatus.ToString();
            _treeNode.SelectedImageKey = _treeNode.ImageKey;
            _treeNode.Text = this.GetType().Name;
            _treeNode.ToolTipText = this.GetType().Name;

            foreach (PropertyChange _propertyChange in this._items)
            {
                _treeNode.Nodes.Add(_propertyChange.GetTreeNode()); 
            }
            return _treeNode;
        }

        #endregion
    }
}
