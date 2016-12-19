using System;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents object for TreeView config
    /// </summary>
    [XmlRoot("TreeView")]
    [Serializable()]
    public sealed class TreeView : Object
    {
        #region Fields

        /// <summary>
        /// Represents the title of the tree view
        /// </summary>
        private String _title = String.Empty;

        /// <summary>
        /// Represents the default selected tree node id
        /// </summary>
        private Int32 _defaultSelectedNodeId = 1;

        /// <summary>
        /// Represents collapsibility of the tree view
        /// </summary>
        private Boolean _collapsible = true;

        /// <summary>
        /// Represents the list of tree nodes
        /// </summary>
        private RS.MDM.Collections.Generic.List<TreeNode> _treeNodes = new RS.MDM.Collections.Generic.List<TreeNode>();

        /// <summary>
        /// Represents the onComplete event for Treeview.
        /// </summary>
        private String _onComplete = String.Empty;

        
        #endregion

        #region Properties

        /// <summary>
        /// Represents the title of the tree view
        /// </summary>
        [XmlAttribute("Title")]
        [Category("Properties")]
        [Description("Represents the title of the tree view")]
        [TrackChanges()]
        public String Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        /// <summary>
        /// Represents collapsibility of the tree view
        /// </summary>
        [XmlAttribute("Collapsible")]
        [Category("Properties")]
        [Description("Represents collapsibility of the tree view")]
        [TrackChanges()]
        public Boolean Collapsible
        {
            get
            {
                return this._collapsible;
            }
            set
            {
                this._collapsible = value;
            }
        }

        /// <summary>
        /// Represents the default selected tree node id
        /// </summary>
        [XmlAttribute("DefaultSelectedNodeId")]
        [Category("Properties")]
        [Description("Represents the default selected tree node id")]
        [TrackChanges()]
        public Int32 DefaultSelectedNodeId
        {
            get
            {
                return this._defaultSelectedNodeId;
            }
            set
            {
                this._defaultSelectedNodeId = value;
            }
        }


        /// <summary>
        /// Represents OnComplete event of Treeview
        /// </summary>
        [XmlAttribute("OnComplete")]
        [Category("Properties")]
        [Description("Represents OnComplete event of Treeview")]
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
        /// Represents the list of tree nodes
        /// </summary>
        [Category("TreeNodes")]
        [Description("Represents the list of tree nodes")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<TreeNode> TreeNodes
        {
            get
            {
                this.SetParent();
                return this._treeNodes;
            }
            set
            {
                this._treeNodes = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TreeView class.
        /// </summary>
        public TreeView()
            : base()
        {
            this.AddVerb("Add TreeNode");
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

        #endregion

        #region Overrides

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (TreeNode treeNode in this._treeNodes)
            {
                if (treeNode != null)
                {
                    treeNode.GenerateNewUniqueIdentifier();
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

            foreach (TreeNode treeNode in this._treeNodes)
            {
                if (treeNode != null)
                {
                    list.AddRange(treeNode.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._treeNodes != null)
            {
                foreach (TreeNode treeNode in this._treeNodes)
                {
                    if (treeNode != null)
                    {
                        treeNode.Parent = this;
                        treeNode.InheritedParent = this.InheritedParent;
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

            if (this._treeNodes != null && this._treeNodes.Count > 0)
            {
                for (int i = _treeNodes.Count - 1; i > -1; i--)
                {
                    TreeNode treeNode = _treeNodes[i];

                    if (treeNode != null)
                    {
                        if (treeNode.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._treeNodes.Remove(treeNode);
                        }
                        else
                        {
                            treeNode.AcceptChanges();
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

            if (this._treeNodes != null)
            {
                foreach (TreeNode treeNode in this._treeNodes)
                {
                    if (treeNode != null)
                    {
                        treeNode.FindChanges();
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

            if (this._treeNodes != null)
            {
                previousSibling = string.Empty;
                foreach (TreeNode treeNode in this._treeNodes)
                {
                    if (treeNode != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(treeNode.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            TreeNode _dataItemClone = RS.MDM.Object.Clone(treeNode, false) as TreeNode;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((TreeView)inheritedChild).TreeNodes.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            treeNode.FindDeletes(_items[0]);
                        }

                        previousSibling = treeNode.UniqueIdentifier;
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

                TreeView _inheritedParent = inheritedParent as TreeView;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (TreeNode treeNode in this._treeNodes)
                {
                    switch (treeNode.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            TreeNode _dataItemClone = RS.MDM.Object.Clone(treeNode, false) as TreeNode;
                            _inheritedParent.TreeNodes.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                        case InheritedObjectStatus.None:
                            TreeNode _inheritedChild = _inheritedParent.TreeNodes.GetItem(treeNode.UniqueIdentifier);
                            treeNode.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = treeNode.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (TreeNode treeNode in this._treeNodes)
                {
                    if (treeNode.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.TreeNodes.Remove(treeNode.UniqueIdentifier);
                    }
                    else
                    {
                        TreeNode _inheritedChild = _inheritedParent.TreeNodes.GetItem(treeNode.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode treeNodes = new System.Windows.Forms.TreeNode("TreeNodes");
            treeNodes.ImageKey = "TreeNodes";
            treeNodes.SelectedImageKey = treeNodes.ImageKey;
            treeNodes.Tag = this.TreeNodes;
            treeNode.Nodes.Add(treeNodes);

            foreach (TreeNode tNode in this._treeNodes)
            {
                if (tNode != null)
                {
                    treeNodes.Nodes.Add(tNode.GetTreeNode());
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
                case "Add TreeNode":
                    this.TreeNodes.Add(new TreeNode());
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
        /// <returns>XML representation of Entity View Panel</returns>
        public override String ToXml()
        {
            String entityViewPanelXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region TreeView Node

            //Parameter node start
            xmlWriter.WriteStartElement("TreeView");

            xmlWriter.WriteAttributeString("Title", this.Title);
            xmlWriter.WriteAttributeString("DefaultSelectedNodeId", this.DefaultSelectedNodeId.ToString());
            xmlWriter.WriteAttributeString("Collapsible", this.Collapsible.ToString());
            xmlWriter.WriteAttributeString("OnComplete", this.OnComplete);
            
            #region TreeNodes Node

            xmlWriter.WriteStartElement("TreeNodes");

            foreach (TreeNode treeNode in this.TreeNodes)
            {
                xmlWriter.WriteRaw(treeNode.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion EntityViews Node

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion TreeView Node

            xmlWriter.Flush();

            //Get the actual XML
            entityViewPanelXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityViewPanelXml;
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

            if (this.TreeNodes.Count == 0)
            {
                validationErrors.Add("The Tree View does not contain any TreeNode.", ValidationErrorType.Warning, "Tree View", this);
            }
        }

        #endregion
    }
}
