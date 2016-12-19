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
    /// Represents a data item of the Menu config
    /// </summary>
    [XmlRoot("Menu")]
    [Serializable()]
    public sealed class Menu : Object
    {
        #region Fields

        /// <summary>
        /// Represents the Menu Items
        /// </summary>
        private RS.MDM.Collections.Generic.List<MenuItem> _menuItems = new RS.MDM.Collections.Generic.List<MenuItem>();

        #endregion

        #region Properties
        /// <summary>
        /// Represents the Menu items
        /// </summary>
        [Category("MenuItems")]
        [Description("Represents the list of Menu Items")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<MenuItem> MenuItems
        {
            get
            {
                this.SetParent();
                return this._menuItems;
            }
            set
            {
                this._menuItems = value;
                this.SetParent();
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Menu class.
        /// </summary>
        public Menu()
            : base()
        {
            this.AddVerb("Add Menu Item");
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

            foreach (MenuItem item in this._menuItems)
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

            foreach (MenuItem item in this._menuItems)
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
            if (this._menuItems != null)
            {
                foreach (MenuItem item in this._menuItems)
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

            if (this._menuItems != null && this._menuItems.Count > 0)
            {
                for (int i = _menuItems.Count - 1; i > -1; i--)
                {
                    MenuItem item = _menuItems[i];

                    if (item != null)
                    {
                        if (item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._menuItems.Remove(item);
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

            if (this._menuItems != null)
            {
                foreach (MenuItem item in this._menuItems)
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

            if (this._menuItems != null)
            {
                previousSibling = string.Empty;
                foreach (MenuItem item in this._menuItems)
                {
                    if (item != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(item.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            MenuItem _dataItemClone = RS.MDM.Object.Clone(item, false) as MenuItem;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((Menu)inheritedChild).MenuItems.InsertAfter(previousSibling, _dataItemClone);
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

                Menu _inheritedParent = inheritedParent as Menu;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (MenuItem item in this._menuItems)
                {
                    switch (item.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            MenuItem _dataItemClone = RS.MDM.Object.Clone(item, false) as MenuItem;
                            _inheritedParent.MenuItems.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            MenuItem _inheritedChild = _inheritedParent.MenuItems.GetItem(item.UniqueIdentifier);
                            item.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = item.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (MenuItem item in this._menuItems)
                {
                    if (item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.MenuItems.Remove(item.UniqueIdentifier);
                    }
                    else
                    {
                        MenuItem _inheritedChild = _inheritedParent.MenuItems.GetItem(item.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode menuItems = new System.Windows.Forms.TreeNode("MenuItems");
            menuItems.ImageKey = "Item";
            menuItems.SelectedImageKey = menuItems.ImageKey;
            menuItems.Tag = this.MenuItems;
            treeNode.Nodes.Add(menuItems);

            foreach (MenuItem item in this._menuItems)
            {
                if (item != null)
                {
                    menuItems.Nodes.Add(item.GetTreeNode());
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
                case "Add Menu Item":
                    this.MenuItems.Add(new MenuItem());
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
        /// <returns>XML representation of Menu</returns>
        public override String ToXml()
        {
            String menuXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region Menu Node

            //Parameter node start
            xmlWriter.WriteStartElement("Menu");

            #region Menu item Node

            xmlWriter.WriteStartElement("MenuItems");

            foreach (MenuItem item in this.MenuItems)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion Menu Item Node

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion Menu Node

            xmlWriter.Flush();

            //Get the actual XML
            menuXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return menuXml;
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

            if (this.MenuItems.Count == 0)
            {
                validationErrors.Add("The Menu does not contain any Item.", ValidationErrorType.Warning, "Menu", this);
            }
        }

        #endregion
    }
}
