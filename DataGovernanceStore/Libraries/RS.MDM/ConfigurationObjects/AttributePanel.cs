using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections.Generic;

using RS.MDM.Validations;
using RS.MDM.Configuration;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configuration for AttributePanel
    /// </summary>
    [XmlRoot("AttributePanel")]
    [Serializable()]
    [XmlInclude(typeof(AttributePanelItem))]
    public sealed class AttributePanel : Panel
    {
        #region Fields

        /// <summary>
        /// field for width of the AttributePanel
        /// </summary>
        private RS.MDM.Collections.Generic.List<AttributePanelItem> _attributePanelItems = new RS.MDM.Collections.Generic.List<AttributePanelItem>();

        /// <summary>
        /// field for allow delete of the AttributePanel
        /// </summary>
        private Boolean _readOnly = false;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates Width of the AttributePanel
        /// </summary>
        [Description("Indicates Attribute Panel Item of the AttributePanel.")]
        [Category("AttributePanelItems")]
        [TrackChanges()]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<AttributePanelItem> AttributePanelItems
        {
            get
            {
                this.SetParent();
                return this._attributePanelItems;
            }
            set
            {
                this._attributePanelItems = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Indicates ReadOnly of the AttributePanel
        /// </summary>
        [XmlAttribute("ReadOnly")]
        [Description("Indicates AllowDelete of the AttributePanel.")]
        [Category("AttributePanel")]
        [TrackChanges()]
        public Boolean ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public AttributePanel()
            : base()
        {
            this.AddVerb("Add Attribute Panel Item");
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Overrides

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
            {
                if (attrPanelItem != null)
                {
                    attrPanelItem.GenerateNewUniqueIdentifier();
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
            List<RS.MDM.Object> _list = new List<Object>();
            _list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));

            foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
            {
                if (attrPanelItem != null)
                {
                    _list.AddRange(attrPanelItem.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return _list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._attributePanelItems != null)
            {
                foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
                {
                    if (attrPanelItem != null)
                    {
                        attrPanelItem.Parent = this;
                        attrPanelItem.InheritedParent = this.InheritedParent;
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

            if (this._attributePanelItems != null && this._attributePanelItems.Count > 0)
            {
                for (int i = _attributePanelItems.Count - 1; i > -1; i--)
                {
                    AttributePanelItem attrPanelItem = _attributePanelItems[i];

                    if (attrPanelItem != null)
                    {
                        if (attrPanelItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._attributePanelItems.Remove(attrPanelItem);
                        }
                        else
                        {
                            attrPanelItem.AcceptChanges();
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

            if (this.PropertyChanges.ObjectStatus != InheritedObjectStatus.Delete && this.PropertyChanges.ObjectStatus != InheritedObjectStatus.Add)
            {
                RS.MDM.Object inheritedParentObject = null;

                if (this.InheritedParent != null)
                {
                    if (this.Parent == null)
                    {
                        inheritedParentObject = this.InheritedParent;
                    }
                    else
                    {
                        List<RS.MDM.Object> _inheritedParentObjects = this.InheritedParent.FindChildren(this.UniqueIdentifier, false);
                        if (_inheritedParentObjects.Count > 0)
                        {
                            foreach (RS.MDM.Object _object in _inheritedParentObjects)
                            {
                                if (_object != null)
                                {
                                    inheritedParentObject = _object;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (inheritedParentObject != null && inheritedParentObject is AttributePanel)
                {
                    Boolean isAttributePanelChanged = false;

                    AttributePanel inheritedParentPanel = inheritedParentObject as AttributePanel;

                    if (this._attributePanelItems != null && this._attributePanelItems.Count > 0)
                    {
                        if (inheritedParentPanel.AttributePanelItems == null || inheritedParentPanel.AttributePanelItems.Count != this._attributePanelItems.Count)
                        {
                            isAttributePanelChanged = true;
                        }
                    }
                    else if (inheritedParentPanel.AttributePanelItems != null && inheritedParentPanel.AttributePanelItems.Count > 0)
                    {
                        isAttributePanelChanged = true;
                    }

                    if (isAttributePanelChanged)
                    {
                        this.PropertyChanges.ObjectStatus = InheritedObjectStatus.Change;
                    }
                }
            }

            if (this._attributePanelItems != null)
            {
                foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
                {
                    if (attrPanelItem != null)
                    {
                        attrPanelItem.FindChanges();
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

            string _previousSibling = string.Empty;

            if (this._attributePanelItems != null)
            {
                _previousSibling = string.Empty;
                foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
                {
                    if (attrPanelItem != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(attrPanelItem.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            AttributePanelItem _dataItemClone = RS.MDM.Object.Clone(attrPanelItem, false) as AttributePanelItem;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((AttributePanel)inheritedChild).AttributePanelItems.InsertAfter(_previousSibling, _dataItemClone);
                        }
                        else
                        {
                            attrPanelItem.FindDeletes(_items[0]);
                        }

                        _previousSibling = attrPanelItem.UniqueIdentifier;
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

                AttributePanel _inheritedParent = inheritedParent as AttributePanel;

                string _previousSibling = string.Empty;

                foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
                {
                    switch (attrPanelItem.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            AttributePanelItem _dataItemClone = RS.MDM.Object.Clone(attrPanelItem, false) as AttributePanelItem;
                            _inheritedParent.AttributePanelItems.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            AttributePanelItem _inheritedChild = _inheritedParent.AttributePanelItems.GetItem(attrPanelItem.UniqueIdentifier);
                            attrPanelItem.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = attrPanelItem.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
                {
                    if (attrPanelItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.AttributePanelItems.Remove(attrPanelItem.UniqueIdentifier);
                    }
                    else
                    {
                        AttributePanelItem _inheritedChild = _inheritedParent.AttributePanelItems.GetItem(attrPanelItem.UniqueIdentifier);

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
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "NavigationPane";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            System.Windows.Forms.TreeNode attrPanelItemsNodes = new System.Windows.Forms.TreeNode("AttributePanelItems");
            attrPanelItemsNodes.ImageKey = "Items";
            attrPanelItemsNodes.SelectedImageKey = attrPanelItemsNodes.ImageKey;
            attrPanelItemsNodes.Tag = this.AttributePanelItems;
            _treeNode.Nodes.Add(attrPanelItemsNodes);

            foreach (AttributePanelItem attrPanelItem in this._attributePanelItems)
            {
                if (attrPanelItem != null)
                {
                    attrPanelItemsNodes.Nodes.Add(attrPanelItem.GetTreeNode());
                }
            }

            return _treeNode;
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
                case "Add Attribute Panel Item":
                    this.AttributePanelItems.Add(new AttributePanelItem());
                    break;
            }

            if (text != "Find Changes" && text != "Accept Changes" && configObject != null && configObject is ConfigurationObject)
            {
                configurationObject = configObject as ConfigurationObject;
                configurationObject._isConfigDirty = true;
            }

            System.ComponentModel.TypeDescriptor.Refresh(this);
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
        }

        #endregion

        #endregion

        #region Private Methods

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

        #endregion

        #endregion
    }
}