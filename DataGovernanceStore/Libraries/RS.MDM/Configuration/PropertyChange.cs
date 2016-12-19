using System;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RS.MDM.Configuration
{
    /// <summary>
    /// Provides functionality to capture changes of an object w.r.t inherited parent (instance)
    /// </summary>
    [XmlRoot("PropertyChange"), Serializable()]
    public sealed class PropertyChange
    {
        #region Fields

        /// <summary>
        /// field for the name of the property
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// field for the value of the parent property
        /// </summary>
        private string _parentValue = null;

        /// <summary>
        /// field for the value fo the child property
        /// </summary>
        private string _childValue = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public PropertyChange()
        {
        }

        /// <summary>
        /// Constructor with property name, parentvalue and childvalue as input properties
        /// </summary>
        /// <param name="name">Indicates the name of the property</param>
        /// <param name="parentValue">Indicates the value of the parent property</param>
        /// <param name="childValue">Indicates the value of the child property</param>
        public PropertyChange(string name, string parentValue, string childValue)
        {
            this._name = name;
            this._parentValue = parentValue;
            this._childValue = childValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the Name of changed property
        /// </summary>
        [Description("Indicates the Name of changed property")]
        [Category("Property")]
        [XmlAttribute("Name")]
        [ReadOnly(true)]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// Indicates the value of parent property
        /// </summary>
        [Description("Indicates the value of parent property")]
        [Category("Property")]
        [XmlAttribute("ParentValue")]
        [ReadOnly(true)]
        public string ParentValue
        {
            get
            {
                return this._parentValue;
            }
            set
            {
                this._parentValue = value;
            }
        }

        /// <summary>
        /// Indicates the value of child property
        /// </summary>
        [Description("Indicates the value of child property")]
        [Category("Property")]
        [XmlAttribute("ChildValue")]
        [ReadOnly(true)]
        public string ChildValue
        {
            get
            {
                return this._childValue;
            }
            set
            {
                this._childValue = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a treenode that represents the property change
        /// </summary>
        /// <returns>A treenode that represents the property change</returns>
        public TreeNode GetTreeNode()
        {
            TreeNode _treeNode = new TreeNode();
            _treeNode.Tag = this;
            _treeNode.ImageKey = "Property"; 
            _treeNode.SelectedImageKey = _treeNode.ImageKey;
            _treeNode.Text = this.GetType().Name;
            _treeNode.ToolTipText = this.GetType().Name;
            return _treeNode;
        }

        #endregion
    }
}
