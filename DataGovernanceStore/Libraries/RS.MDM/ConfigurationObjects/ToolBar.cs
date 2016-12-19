using System;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;
    
    /// <summary>
    /// Represents a data item of the Toolbar config
    /// </summary>
    [XmlRoot("ToolBar")]
    [Serializable()]
    public sealed class ToolBar : Object
    {
        #region Fields

        /// <summary>
        /// Represents the render comments dialog of toolbar
        /// </summary>
        private Boolean _renderCommentsDialog = false;

        /// <summary>
        /// Represents the toolbar Items
        /// </summary>
        private RS.MDM.Collections.Generic.List<ToolBarItem> _toolBarItems = new RS.MDM.Collections.Generic.List<ToolBarItem>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the toolbar items
        /// </summary>
        [Category("ToolBarItems")]
        [Description("Represents the list of toolbar items")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<ToolBarItem> ToolBarItems
        {
            get
            {
                this.SetParent();
                return this._toolBarItems;
            }
            set
            {
                this._toolBarItems = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Represents the Render Comments Dialog of toolbar
        /// </summary>
        [XmlAttribute("RenderCommentsDialog")]
        [Category("Properties")]
        [Description("Represents the Render Comments Dialog of toolbar")]
        [TrackChanges()]
        public Boolean RenderCommentsDialog
        {
            get
            {
                return this._renderCommentsDialog;
            }
            set
            {
                this._renderCommentsDialog = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ToolBar class.
        /// </summary>
        public ToolBar()
            : base()
        {
            this.AddVerb("Add ToolBar Item");
        }

       /// <summary>
       /// Constructor with ToolBar as xml as a input parameter
       /// </summary>
       /// <param name="valueAsXml">Indicates the ToolBar as xml </param>
        public ToolBar(String valueAsXml)
            : this()
        {
            LoadToolBar(valueAsXml);

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

            foreach (ToolBarItem item in this._toolBarItems)
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

            foreach (ToolBarItem item in this._toolBarItems)
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
            if (this._toolBarItems != null)
            {
                foreach (ToolBarItem item in this._toolBarItems)
                {
                    if (item != null)
                    {
                        item.Parent = this;
                        item.InheritedParent = this.InheritedParent;
                        item.UILocale = this.UILocale;
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

            if (this._toolBarItems != null && this._toolBarItems.Count > 0)
            {
                for (int i = _toolBarItems.Count - 1; i > -1; i--)
                {
                    ToolBarItem item = _toolBarItems[i];

                    if (item != null)
                    {
                        if (item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._toolBarItems.Remove(item);
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

            if (this._toolBarItems != null)
            {
                foreach (ToolBarItem item in this._toolBarItems)
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

            if (this._toolBarItems != null)
            {
                previousSibling = string.Empty;
                foreach (ToolBarItem item in this._toolBarItems)
                {
                    if (item != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(item.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            ToolBarItem _dataItemClone = RS.MDM.Object.Clone(item, false) as ToolBarItem;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((ToolBar)inheritedChild).ToolBarItems.InsertAfter(previousSibling, _dataItemClone);
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

                ToolBar _inheritedParent = inheritedParent as ToolBar;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (ToolBarItem item in this._toolBarItems)
                {
                    switch (item.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            ToolBarItem _dataItemClone = RS.MDM.Object.Clone(item, false) as ToolBarItem;
                            _inheritedParent.ToolBarItems.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            ToolBarItem _inheritedChild = _inheritedParent.ToolBarItems.GetItem(item.UniqueIdentifier);
                            item.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = item.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (ToolBarItem item in this._toolBarItems)
                {
                    if (item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.ToolBarItems.Remove(item.UniqueIdentifier);
                    }
                    else
                    {
                        ToolBarItem _inheritedChild = _inheritedParent.ToolBarItems.GetItem(item.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode toolBarItems = new System.Windows.Forms.TreeNode("ToolBarItems");
            toolBarItems.ImageKey = "Item";
            toolBarItems.SelectedImageKey = toolBarItems.ImageKey;
            toolBarItems.Tag = this.ToolBarItems;
            treeNode.Nodes.Add(toolBarItems);

            foreach (ToolBarItem item in this._toolBarItems)
            {
                if (item != null)
                {
                    toolBarItems.Nodes.Add(item.GetTreeNode());
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
                case "Add ToolBar Item":
                    this.ToolBarItems.Add(new ToolBarItem());
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
        /// <returns>XML representation of ToolBar</returns>
        public override String ToXml()
        {
            String toolBarXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region ToolBar Node

            //Parameter node start
            xmlWriter.WriteStartElement("ToolBar");

            xmlWriter.WriteAttributeString("ClassName", this.ClassName);
            xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
            xmlWriter.WriteAttributeString("RenderCommentsDialog", this._renderCommentsDialog.ToString().ToLowerInvariant());

            #region ToolBar item Node

            xmlWriter.WriteStartElement("ToolBarItems");

            foreach (ToolBarItem item in this.ToolBarItems)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion ToolBar Item Node

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion ToolBar Node

            xmlWriter.Flush();

            //Get the actual XML
            toolBarXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return toolBarXml;
        }

        /// <summary>
        /// Load the ToolBar object from input xml.
        /// </summary>
        /// <param name="toolBarXml">ToolBar XML</param>
        public void LoadToolBar(String toolBarXml)
        {
            if (!String.IsNullOrWhiteSpace(toolBarXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(toolBarXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ToolBar")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ClassName"))
                                {
                                    this.ClassName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AssemblyName"))
                                {
                                    this.AssemblyName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RenderCommentsDialog"))
                                {
                                    this.RenderCommentsDialog = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._renderCommentsDialog);
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ToolBarItems")
                        {
                            #region Read ToolBarItems

                            String toolBarItemsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(toolBarItemsXml))
                            {
                                this.ToolBarItems = LoadToolBarItem(toolBarItemsXml);
                            }

                            #endregion Read child attributes
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }

                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        private RS.MDM.Collections.Generic.List<ToolBarItem> LoadToolBarItem(String parameterAsXml)
        {
            RS.MDM.Collections.Generic.List<ToolBarItem> toolBarItemList = new Collections.Generic.List<ToolBarItem>();

            if (!String.IsNullOrWhiteSpace(parameterAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(parameterAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ToolBarItem")
                        {
                            ToolBarItem toolBarItem = new ToolBarItem();
                            toolBarItem.LoadToolBarItem(reader.ReadOuterXml());

                            toolBarItemList.Add(toolBarItem);
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

            }

            return toolBarItemList;
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

            if (this.ToolBarItems.Count == 0)
            {
                validationErrors.Add("The ToolBar does not contain any Item.", ValidationErrorType.Warning, "ToolBar", this);
            }
        }

        #endregion
    }
}
