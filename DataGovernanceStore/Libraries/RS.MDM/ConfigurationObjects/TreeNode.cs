using System;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using MC = MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;
    

    /// <summary>
    /// Represents a view of tree node Panel
    /// </summary>
    [XmlRoot("TreeNode")]
    [Serializable()]
    public sealed class TreeNode : Object
    {
        #region Fields

        /// <summary>
        /// Represents the title of the tree node
        /// </summary>
        private String _title = String.Empty;

        /// <summary>
        /// Represents the icon name of the tree node
        /// </summary>
        private String _icon = String.Empty;

        /// <summary>
        /// Represents the tree node click event handler name
        /// </summary>
        private String _onClick = String.Empty;

        /// <summary>
        /// Represents the type of node. This can be used for generic purpose.
        /// </summary>
        private String _nodeType = String.Empty;

        /// <summary>
        /// Represents node help data
        /// </summary>
        private String _helpText = String.Empty;

        /// <summary>
        /// Represents the parameter list for the tree node
        /// </summary>
        private RS.MDM.Collections.Generic.List<Parameter> _parameters = new Collections.Generic.List<Parameter>();

        /// <summary>
        /// Represents the child tree node list
        /// </summary>
        private RS.MDM.Collections.Generic.List<TreeNode> _childTreeNodes = new Collections.Generic.List<TreeNode>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the caption of the tree node
        /// </summary>
        [XmlAttribute("Title")]
        [Category("Properties")]
        [Description("Represents the title of the tree node")]
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
        /// Represents the sequence of the tree node
        /// </summary>
        [XmlAttribute("Icon")]
        [Category("Properties")]
        [Description("Represents the icon name of the tree node")]
        [TrackChanges()]
        public String Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        /// <summary>
        /// Represents the type of node. This can be used for generic purpose.
        /// </summary>
        [XmlAttribute("NodeType")]
        [Category("Properties")]
        [Description("Represents the type of node.")]
        [TrackChanges()]
        public String NodeType
        {
            get
            {
                return this._nodeType;
            }
            set
            {
                this._nodeType = value;
            }
        }

        /// <summary>
        /// Represents the sequence of the tree node
        /// </summary>
        [XmlAttribute("OnClick")]
        [Category("Properties")]
        [Description("Represents the tree node click event handler name")]
        [TrackChanges()]
        public String OnClick
        {
            get
            {
                return this._onClick;
            }
            set
            {
                this._onClick = value;
            }
        }

        /// <summary>
        /// Represents the parameter list for the view
        /// </summary>
        [Category("Parameters")]
        [Description("Represents the parameter list for the tree node")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<Parameter> Parameters
        {
            get
            {
                this.SetParent();
                return this._parameters;
            }
            set
            {
                this._parameters = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Represents the parameter list for the view
        /// </summary>
        [Category("ChildTreeNodes")]
        [Description("Represents the child tree node list")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<TreeNode> ChildTreeNodes
        {
            get
            {
                this.SetParent();
                return this._childTreeNodes;
            }
            set
            {
                this._childTreeNodes = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Represents node help data
        /// </summary>
        [Category("Properties")]
        [Description("Represents node help data")]
        [TrackChanges()]
        public String HelpText
        {
            get
            {
                return this._helpText;
            }
            set
            {
                this._helpText = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TreeNode class.
        /// </summary>
        public TreeNode()
            : base()
        {
            this.AddVerb("Add Parameter");
            this.AddVerb("Add Child Node");
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

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    param.GenerateNewUniqueIdentifier();
                }
            }

            foreach (TreeNode treeNode in this._childTreeNodes)
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

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    list.AddRange(param.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            foreach (TreeNode treeNode in this._childTreeNodes)
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
            if (this._parameters != null)
            {
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        param.Parent = this;
                        param.InheritedParent = this.InheritedParent;
                    }
                }
            }

            if (this._childTreeNodes != null)
            {
                foreach (TreeNode treeNode in this._childTreeNodes)
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

            if (this._parameters != null && this._parameters.Count > 0)
            {
                for (int i = _parameters.Count - 1; i > -1; i--)
                {
                    Parameter param = _parameters[i];

                    if (param != null)
                    {
                        if (param.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._parameters.Remove(param);
                        }
                        else
                        {
                            param.AcceptChanges();
                        }
                    }
                }
            }

            if (this._childTreeNodes != null && this._childTreeNodes.Count > 0)
            {
                for (int i = _childTreeNodes.Count - 1; i > -1; i--)
                {
                    TreeNode treeNode = _childTreeNodes[i];

                    if (treeNode != null)
                    {
                        if (treeNode.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._childTreeNodes.Remove(treeNode);
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

            if (this._parameters != null)
            {
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        param.FindChanges();
                    }
                }
            }

            if (this._childTreeNodes != null)
            {
                foreach (TreeNode treeNode in this._childTreeNodes)
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

            if (this._parameters != null)
            {
                previousSibling = string.Empty;
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(param.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            Parameter _dataItemClone = RS.MDM.Object.Clone(param, false) as Parameter;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((TreeNode)inheritedChild).Parameters.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            param.FindDeletes(_items[0]);
                        }

                        previousSibling = param.UniqueIdentifier;
                    }
                }
            }

            if (this._childTreeNodes != null)
            {
                previousSibling = string.Empty;
                foreach (TreeNode treeNode in this._childTreeNodes)
                {
                    if (treeNode != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(treeNode.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            TreeNode _dataItemClone = RS.MDM.Object.Clone(treeNode, false) as TreeNode;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((TreeNode)inheritedChild).ChildTreeNodes.InsertAfter(previousSibling, _dataItemClone);
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

                TreeNode _inheritedParent = inheritedParent as TreeNode;

                string _previousSibling = string.Empty;

                // Apply all the changes
                foreach (Parameter param in this._parameters)
                {
                    switch (param.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Parameter _dataItemClone = RS.MDM.Object.Clone(param, false) as Parameter;
                            _inheritedParent.Parameters.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Parameter _inheritedChild = _inheritedParent.Parameters.GetItem(param.UniqueIdentifier);
                            param.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = param.UniqueIdentifier;
                }

                foreach (TreeNode treeNode in this._childTreeNodes)
                {
                    switch (treeNode.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            TreeNode _dataItemClone = RS.MDM.Object.Clone(treeNode, false) as TreeNode;
                            _inheritedParent.ChildTreeNodes.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            TreeNode _inheritedChild = _inheritedParent.ChildTreeNodes.GetItem(treeNode.UniqueIdentifier);
                            treeNode.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = treeNode.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (Parameter param in this._parameters)
                {
                    if (param.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Parameters.Remove(param.UniqueIdentifier);
                    }
                    else
                    {
                        Parameter _inheritedChild = _inheritedParent.Parameters.GetItem(param.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }

                foreach (TreeNode treeNode in this._childTreeNodes)
                {
                    if (treeNode.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.ChildTreeNodes.Remove(treeNode.UniqueIdentifier);
                    }
                    else
                    {
                        TreeNode _inheritedChild = _inheritedParent.ChildTreeNodes.GetItem(treeNode.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode parameters = new System.Windows.Forms.TreeNode("Parameters");
            parameters.ImageKey = "Parameters";
            parameters.SelectedImageKey = parameters.ImageKey;
            parameters.Tag = this.Parameters;
            treeNode.Nodes.Add(parameters);

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    parameters.Nodes.Add(param.GetTreeNode());
                }
            }

            System.Windows.Forms.TreeNode treeNodes = new System.Windows.Forms.TreeNode("ChildTreeNodes");
            treeNodes.ImageKey = "ChildTreeNodes";
            treeNodes.SelectedImageKey = parameters.ImageKey;
            treeNodes.Tag = this.Parameters;
            treeNode.Nodes.Add(treeNodes);

            foreach (TreeNode tNode in this._childTreeNodes)
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
                case "Add Parameter":
                    this.Parameters.Add(new Parameter());
                    break;
                case "Add Child Node":
                    this.ChildTreeNodes.Add(new TreeNode());
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
        /// <returns>XML representation of tree node</returns>
        public override String ToXml()
        {
            String treeNodeXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region TreeNode Node

            //Parameter node start
            xmlWriter.WriteStartElement("TreeNode");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
            xmlWriter.WriteAttributeString("Name", this.GetLocaleMessage(this.Name));
            xmlWriter.WriteAttributeString("Title", this.GetLocaleMessage(this.Title));
            xmlWriter.WriteAttributeString("Icon", this.Icon);
            xmlWriter.WriteAttributeString("NodeType", this.NodeType);
            xmlWriter.WriteAttributeString("OnClick", this.OnClick);

            #region Params Node

            xmlWriter.WriteStartElement("Params");

            foreach (Parameter param in this.Parameters)
            {
                xmlWriter.WriteRaw(param.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion Params Node

            #region Child Tree Node

            xmlWriter.WriteStartElement("TreeNodes");

            foreach (TreeNode treeNode in this.ChildTreeNodes)
            {
                xmlWriter.WriteRaw(treeNode.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion Child Tree Node

            #region HelpText Node

            xmlWriter.WriteStartElement("HelpText");

            xmlWriter.WriteCData(HelpText);

            xmlWriter.WriteEndElement();

            #endregion HelpText Node

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion TreeNode Node

            xmlWriter.Flush();

            //Get the actual XML
            treeNodeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return treeNodeXml;
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

            if (this.Parameters.Count == 0)
            {
                validationErrors.Add("The tree node does not contain any parameter.", ValidationErrorType.Warning, "Tree Node", this);
            }

            if (this.ChildTreeNodes.Count == 0)
            {
                validationErrors.Add("The tree node does not contain any child node.", ValidationErrorType.Warning, "Tree Node", this);
            }
        }

        #endregion
    }
}
