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
    /// Represents a data item of the SearchAttributeRules config
    /// </summary>
    [XmlRoot("SearchPanel")]
    [Serializable()]
    public sealed class SearchPanel : Object
    {
        #region Fields

        /// <summary>
        /// Represents the collection of SearchAttributeRules
        /// </summary>
        private RS.MDM.Collections.Generic.List<SearchPanelItem> _searchPanelItems = new RS.MDM.Collections.Generic.List<SearchPanelItem>();

        #endregion

        #region Properties
        /// <summary>
        /// Represents the SearchAttributeRules
        /// </summary>
        [Category("SearchPanelItems")]
        [Description("Represents the list of Search Panel Items")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<SearchPanelItem> SearchPanelItems
        {
            get
            {
                this.SetParent();
                return this._searchPanelItems;
            }
            set
            {
                this._searchPanelItems = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SearchPanel class.
        /// </summary>
        public SearchPanel()
            : base()
        {
            this.AddVerb("Add Search Panel Item");
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

            foreach (SearchPanelItem searchPanelItem in this._searchPanelItems)
            {
                if (searchPanelItem != null)
                {
                    searchPanelItem.GenerateNewUniqueIdentifier();
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

            foreach (SearchPanelItem searchPanelItem in this._searchPanelItems)
            {
                if (searchPanelItem != null)
                {
                    list.AddRange(searchPanelItem.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._searchPanelItems != null)
            {
                foreach (SearchPanelItem searchPanelItem in this._searchPanelItems)
                {
                    if (searchPanelItem != null)
                    {
                        searchPanelItem.Parent = this;
                        searchPanelItem.InheritedParent = this.InheritedParent;
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

            if (this._searchPanelItems != null && this._searchPanelItems.Count > 0)
            {
                for (int i = _searchPanelItems.Count - 1; i > -1; i--)
                {
                    SearchPanelItem searchPanelItem = _searchPanelItems[i];

                    if (searchPanelItem != null)
                    {
                        if (searchPanelItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._searchPanelItems.Remove(searchPanelItem);
                        }
                        else
                        {
                            searchPanelItem.AcceptChanges();
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

            if (this._searchPanelItems != null)
            {
                foreach (SearchPanelItem searchPanelItem in this._searchPanelItems)
                {
                    if (searchPanelItem != null)
                    {
                        searchPanelItem.FindChanges();
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

            if (this._searchPanelItems != null)
            {
                previousSibling = string.Empty;
                foreach (SearchPanelItem searchPanelItem in this._searchPanelItems)
                {
                    if (searchPanelItem != null)
                    {
                        List<RS.MDM.Object> _searchPanelItem = inheritedChild.FindChildren(searchPanelItem.UniqueIdentifier, true);

                        if (_searchPanelItem.Count == 0)
                        {
                            SearchPanelItem _dataItemClone = RS.MDM.Object.Clone(searchPanelItem, false) as SearchPanelItem;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((SearchPanel)inheritedChild).SearchPanelItems.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            searchPanelItem.FindDeletes(_searchPanelItem[0]);
                        }

                        previousSibling = searchPanelItem.UniqueIdentifier;
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

                SearchPanel _inheritedParent = inheritedParent as SearchPanel;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (SearchPanelItem searchPanelItem in this._searchPanelItems)
                {
                    switch (searchPanelItem.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            SearchPanelItem _dataItemClone = RS.MDM.Object.Clone(searchPanelItem, false) as SearchPanelItem;
                            _inheritedParent.SearchPanelItems.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            SearchPanelItem _inheritedChild = _inheritedParent.SearchPanelItems.GetItem(searchPanelItem.UniqueIdentifier);
                            searchPanelItem.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = searchPanelItem.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (SearchPanelItem searchPanelItem in this._searchPanelItems)
                {
                    if (searchPanelItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.SearchPanelItems.Remove(searchPanelItem.UniqueIdentifier);
                    }
                    else
                    {
                        SearchPanelItem _inheritedChild = _inheritedParent.SearchPanelItems.GetItem(searchPanelItem.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode searchPanelItems = new System.Windows.Forms.TreeNode("SearchPanelItems");
            searchPanelItems.ImageKey = "Item";
            searchPanelItems.SelectedImageKey = searchPanelItems.ImageKey;
            searchPanelItems.Tag = this.SearchPanelItems;
            treeNode.Nodes.Add(searchPanelItems);

            foreach (SearchPanelItem item in this._searchPanelItems)
            {
                if (item != null)
                {
                    searchPanelItems.Nodes.Add(item.GetTreeNode());
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
                case "Add Search Attribute Rule Item":
                    this.SearchPanelItems.Add(new SearchPanelItem());
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
        /// <returns>XML representation of SearchAttributeRulesCollection</returns>
        public override String ToXml()
        {
            String searchPanelXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region SearchPanel Node

            //Parameter node start
            xmlWriter.WriteStartElement("SearchPanel");

            #region SearchPanelItem Node

            xmlWriter.WriteStartElement("SearchPanelItems");

            if (this.SearchPanelItems != null)
            {
                foreach (SearchPanelItem item in this.SearchPanelItems)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
            }

            xmlWriter.WriteEndElement();

            #endregion SearchPanelItem Node

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion SearchPanel Node

            xmlWriter.Flush();

            //Get the actual XML
            searchPanelXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchPanelXml;
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

            if (this.SearchPanelItems.Count == 0)
            {
                validationErrors.Add("The SearchPanel does not contain any SearchPanelItem.", ValidationErrorType.Warning, "SearchPanel", this);
            }
        }

        #endregion

    }
}
