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
    /// Represents a KeyValuePairs List
    /// </summary>
    [XmlRoot("KeyValuePairsList")]
    [Serializable()]
    public sealed class KeyValuePairsList : Object
    {
        #region Fields

        /// <summary>
        /// Represents the KeyValuePairs List KeyValuePairs
        /// </summary>
        private RS.MDM.Collections.Generic.List<KeyValuePairItem> _items = new RS.MDM.Collections.Generic.List<KeyValuePairItem>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the list of KeyValue pairs
        /// </summary>
        [Category("KeyValuePairs")]
        [Description("Represents the list of KeyValue pairs")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<KeyValuePairItem> KeyValuePairs
        {
            get
            {
                this.SetParent();
                return this._items;
            }
            set
            {
                this._items = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the KeyValuePairsList class
        /// </summary>
        public KeyValuePairsList()
            : base()
        {
            this.AddVerb("Add KeyValue Pair");
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

            foreach (KeyValuePairItem item in this._items)
            {
                if (item != null)
                {
                    item.GenerateNewUniqueIdentifier();
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

            foreach (KeyValuePairItem item in this._items)
            {
                if (item != null)
                {
                    list.AddRange(item.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._items != null)
            {
                foreach (KeyValuePairItem item in this._items)
                {
                    if (item != null)
                    {
                        item.Parent = this;
                        item.InheritedParent = this.InheritedParent;
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

            if (this._items != null && this._items.Count > 0)
            {
                for (int i = _items.Count - 1; i > -1; i--)
                {
                    KeyValuePairItem item = _items[i];

                    if (item != null)
                    {
                        if (item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._items.Remove(item);
                        }
                        else
                        {
                            item.AcceptChanges();
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

            if (this._items != null)
            {
                foreach (KeyValuePairItem item in this._items)
                {
                    if (item != null)
                    {
                        item.FindChanges();
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

            if (this._items != null)
            {
                previousSibling = string.Empty;
                foreach (KeyValuePairItem item in this._items)
                {
                    if (item != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(item.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            KeyValuePairItem _dataItemClone = RS.MDM.Object.Clone(item, false) as KeyValuePairItem;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((KeyValuePairsList)inheritedChild).KeyValuePairs.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            item.FindDeletes(_items[0]);
                        }

                        previousSibling = item.UniqueIdentifier;
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

                KeyValuePairsList _inheritedParent = inheritedParent as KeyValuePairsList;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (KeyValuePairItem item in this._items)
                {
                    switch (item.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            KeyValuePairItem _dataItemClone = RS.MDM.Object.Clone(item, false) as KeyValuePairItem;
                            _inheritedParent.KeyValuePairs.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            KeyValuePairItem _inheritedChild = _inheritedParent.KeyValuePairs.GetItem(item.UniqueIdentifier);
                            item.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = item.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (KeyValuePairItem item in this._items)
                {
                    if (item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.KeyValuePairs.Remove(item.UniqueIdentifier);
                    }
                    else
                    {
                        KeyValuePairItem _inheritedChild = _inheritedParent.KeyValuePairs.GetItem(item.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode KeyValuePairItems = new System.Windows.Forms.TreeNode("KeyValuePairs");
            KeyValuePairItems.ImageKey = "Item";
            KeyValuePairItems.SelectedImageKey = KeyValuePairItems.ImageKey;
            KeyValuePairItems.Tag = this.KeyValuePairs;
            treeNode.Nodes.Add(KeyValuePairItems);

            foreach (KeyValuePairItem item in this._items)
            {
                if (item != null)
                {
                    KeyValuePairItems.Nodes.Add(item.GetTreeNode());
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
                case "Add KeyValue Pair":
                    this.KeyValuePairs.Add(new KeyValuePairItem());
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
        /// <returns>XML representation of KeyValuePairsList</returns>
        public override String ToXml()
        {
            String KeyValuePairsListXml = String.Empty;

            using(var sw = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(sw))
            {
                #region KeyValuePairsList Node

                //KeyValuePairsList node start
                xmlWriter.WriteStartElement("KeyValuePairsList");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("Description", this.Description);
                xmlWriter.WriteAttributeString("Tag", this.Tag);
                xmlWriter.WriteAttributeString("ClassName", this.ClassName);
                xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
                xmlWriter.WriteAttributeString("InheritedParentUId", this.InheritedParentUId);

                #region KeyValuePairs Node

                xmlWriter.WriteStartElement("KeyValuePairs");

                foreach (KeyValuePairItem item in this.KeyValuePairs)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }

                xmlWriter.WriteEndElement();

                #endregion KeyValuePairs Node

                //KeyValuePairsList node end
                xmlWriter.WriteEndElement();

                #endregion KeyValuePairsList Node

                xmlWriter.Flush();

                //Get the actual XML
                KeyValuePairsListXml = sw.ToString();
            }

            return KeyValuePairsListXml;
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

            if (this.KeyValuePairs.Count > 0)
            {
                var names = new HashSet<string>();
                foreach (var pair in KeyValuePairs)
                {
                    if (string.IsNullOrEmpty(pair.Name))
                    {
                        validationErrors.Add("The KeyValuePairsList contains item with null or empty name", ValidationErrorType.Error, "KeyValuePairsList", this);
                        continue;
                    }
                    if (names.Contains(pair.Name))
                    {validationErrors.Add(string.Format("The KeyValuePairsList contains more than one item with the same name: \"{0}\"", pair.Name), ValidationErrorType.Error, "KeyValuePairsList", this);
                    }
                    else
                    {
                        names.Add(pair.Name);
                    }
                }
            }
        }

        #endregion
    }
}
