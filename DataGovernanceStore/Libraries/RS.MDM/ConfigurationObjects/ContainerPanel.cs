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
    /// Provides configuration for ContainerPanel
    /// </summary>
    [XmlRoot("ContainerPanel")]
    [Serializable()]
    [XmlInclude(typeof(ContainerPanelItem))]
    public sealed class ContainerPanel : Panel
    {
        #region Fields

        /// <summary>
        /// field for ContainerPanelItems of the ContainerPanel
        /// </summary>
        private RS.MDM.Collections.Generic.List<ContainerPanelItem> _containerPanelItems = new RS.MDM.Collections.Generic.List<ContainerPanelItem>();

        /// <summary>
        /// field for read only of the ContainerPanel
        /// </summary>
        private Boolean _readOnly = false;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates ContainerPanelItems of the ContainerPanel
        /// </summary>
        [Description("Indicates Container Panel Item of the ContainerPanel.")]
        [Category("ContainerPanelItems")]
        [TrackChanges()]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<ContainerPanelItem> ContainerPanelItems
        {
            get
            {
                this.SetParent();
                return this._containerPanelItems;
            }
            set
            {
                this._containerPanelItems = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Indicates ReadOnly of the ContainerPanel
        /// </summary>
        [XmlAttribute("ReadOnly")]
        [Description("Indicates ReadOnly of the ContainerPanel.")]
        [Category("ContainerPanel")]
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
        public ContainerPanel()
            : base()
        {
            this.AddVerb("Add Container Panel Item");
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

            foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
            {
                if (containerPanelItem != null)
                {
                    containerPanelItem.GenerateNewUniqueIdentifier();
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

            foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
            {
                if (containerPanelItem != null)
                {
                    _list.AddRange(containerPanelItem.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return _list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._containerPanelItems != null)
            {
                foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
                {
                    if (containerPanelItem != null)
                    {
                        containerPanelItem.Parent = this;
                        containerPanelItem.InheritedParent = this.InheritedParent;
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

            if (this._containerPanelItems != null && this._containerPanelItems.Count > 0)
            {
                for (int i = _containerPanelItems.Count - 1; i > -1; i--)
                {
                    ContainerPanelItem containerPanelItem = _containerPanelItems[i];

                    if (containerPanelItem != null)
                    {
                        if (containerPanelItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._containerPanelItems.Remove(containerPanelItem);
                        }
                        else
                        {
                            containerPanelItem.AcceptChanges();
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

                if (inheritedParentObject != null && inheritedParentObject is ContainerPanel)
                {
                    Boolean isCategoryPanelChanged = false;

                    ContainerPanel inheritedParentPanel = inheritedParentObject as ContainerPanel;

                    if (this._containerPanelItems != null && this._containerPanelItems.Count > 0)
                    {
                        if (inheritedParentPanel == null || inheritedParentPanel.ContainerPanelItems.Count != this._containerPanelItems.Count)
                        {
                            isCategoryPanelChanged = true;
                        }
                    }
                    else if (inheritedParentPanel.ContainerPanelItems != null && inheritedParentPanel.ContainerPanelItems.Count > 0)
                    {
                        isCategoryPanelChanged = true;
                    }

                    if (isCategoryPanelChanged)
                    {
                        this.PropertyChanges.ObjectStatus = InheritedObjectStatus.Change;
                    }
                }
            }

            if (this._containerPanelItems != null)
            {
                foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
                {
                    if (containerPanelItem != null)
                    {
                        containerPanelItem.FindChanges();
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

            if (this._containerPanelItems != null)
            {
                _previousSibling = string.Empty;
                foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
                {
                    if (containerPanelItem != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(containerPanelItem.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            ContainerPanelItem _dataItemClone = RS.MDM.Object.Clone(containerPanelItem, false) as ContainerPanelItem;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((ContainerPanel)inheritedChild).ContainerPanelItems.InsertAfter(_previousSibling, _dataItemClone);
                        }
                        else
                        {
                            containerPanelItem.FindDeletes(_items[0]);
                        }

                        _previousSibling = containerPanelItem.UniqueIdentifier;
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

                ContainerPanel _inheritedParent = inheritedParent as ContainerPanel;

                string _previousSibling = string.Empty;

                foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
                {
                    switch (containerPanelItem.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            ContainerPanelItem _dataItemClone = RS.MDM.Object.Clone(containerPanelItem, false) as ContainerPanelItem;
                            _inheritedParent.ContainerPanelItems.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            ContainerPanelItem _inheritedChild = _inheritedParent.ContainerPanelItems.GetItem(containerPanelItem.UniqueIdentifier);
                            containerPanelItem.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = containerPanelItem.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
                {
                    if (containerPanelItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.ContainerPanelItems.Remove(containerPanelItem.UniqueIdentifier);
                    }
                    else
                    {
                        ContainerPanelItem _inheritedChild = _inheritedParent.ContainerPanelItems.GetItem(containerPanelItem.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode containerPanelItemsNodes = new System.Windows.Forms.TreeNode("ContainerPanelItems");
            containerPanelItemsNodes.ImageKey = "Items";
            containerPanelItemsNodes.SelectedImageKey = containerPanelItemsNodes.ImageKey;
            containerPanelItemsNodes.Tag = this.ContainerPanelItems;
            _treeNode.Nodes.Add(containerPanelItemsNodes);

            foreach (ContainerPanelItem containerPanelItem in this._containerPanelItems)
            {
                if (containerPanelItem != null)
                {
                    containerPanelItemsNodes.Nodes.Add(containerPanelItem.GetTreeNode());
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
                case "Add Container Panel Item":
                    this.ContainerPanelItems.Add(new ContainerPanelItem());
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