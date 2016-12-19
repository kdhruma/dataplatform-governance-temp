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
    /// Specifies whether to show dialog on item action and the type of the dialog
    /// </summary>
    public enum ShowDialogEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Comments = 1,

        /// <summary>
        /// 
        /// </summary>
        CommentsRequired = 2,

        /// <summary>
        /// 
        /// </summary>
        Rejection = 3,

        /// <summary>
        /// 
        /// </summary>
        RejectionCommentsRequried = 4,

        /// <summary>
        /// 
        /// </summary>
        None = 5
    }

    /// <summary>
    /// Represents an Item of ToolBar
    /// </summary>
    [XmlRoot("ToolBarItem")]
    [Serializable()]
    public sealed class ToolBarItem : Object
    {
        #region Fields

        /// <summary>
        /// Represents the type of the ToolBar Item
        /// </summary>
        private String _itemType = String.Empty;

        /// <summary>
        /// Represents the caption of the ToolBar Item
        /// </summary>
        private String _caption = String.Empty;

        /// <summary>
        /// Represents on before click event of toolbar item
        /// </summary>
        private String _onBeforeClick = String.Empty;

        /// <summary>
        /// Represents on click event of toolbar item
        /// </summary>
        private String _onClick = String.Empty;

        /// <summary>
        /// Represents the Icon of toolbar item
        /// </summary>
        private String _icon = String.Empty;
        
        /// <summary>
        /// Represents the ShortCut of toolbar item
        /// </summary>
        private String _shortCut = String.Empty;

        /// <summary>
        /// Represents the Tooltip of toolbar item
        /// </summary>
        private String _toolTip = String.Empty;

        /// <summary>
        /// Specifies whether to show dialog on item action and the type of the dialog
        /// </summary>
        private ShowDialogEnum _showDialog = ShowDialogEnum.None;

        /// <summary>
        /// Represents the visible property of toolbar item
        /// </summary>
        private Boolean _visible = true;

        /// <summary>
        /// Specifies whether toolbar button enabled or not
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        /// Represents if BusinessRule should be triggered on Pre.
        /// </summary>
        private Boolean _triggerBusinessRuleOnPreProcess = false;

        /// <summary>
        /// Represents if BusinessRule should be triggered on Post
        /// </summary>
        private Boolean _triggerBusinessRuleOnPostProcess = false;

        /// <summary>
        /// Represents the parameter list for the toolbar item
        /// </summary>
        private RS.MDM.Collections.Generic.List<Parameter> _parameters = new Collections.Generic.List<Parameter>();

        /// <summary>
        /// Specifies if the toolbar item has to displayed as part of More Actions dropdown
        /// </summary>
        private Boolean _showInMoreActions = false;


        /// <summary>
        /// Specifies if the toolbar item has to show text on the button.
        /// </summary>
        private Boolean _showTextOnButton = false;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the type of the toolbar item
        /// </summary>
        [XmlAttribute("Type")]
        [Category("Properties")]
        [Description("Represents the type of the toolbar item")]
        [TrackChanges()]
        public String Type
        {
            get
            {
                return this._itemType;
            }
            set
            {
                this._itemType = value;
            }
        }

        /// <summary>
        /// Represents the caption of the toolbar item
        /// </summary>
        [XmlAttribute("Caption")]
        [Category("Properties")]
        [Description("Represents the caption of the toolbar item")]
        [TrackChanges()]
        public String Caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                this._caption = value;
            }
        }

        /// <summary>
        /// Represents on click event of toolbar item
        /// </summary>
        [XmlAttribute("OnClick")]
        [Category("Properties")]
        [Description("Represents on click event of toolbar item")]
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
        /// Represents on before click event of toolbar item
        /// </summary>
        [XmlAttribute("OnBeforeClick")]
        [Category("Properties")]
        [Description("Represents on before click event of toolbar item")]
        [TrackChanges()]
        public String OnBeforeClick
        {
            get
            {
                return this._onBeforeClick;
            }
            set
            {
                this._onBeforeClick = value;
            }
        }

        /// <summary>
        /// Represents the Icon of toolbar item
        /// </summary>
        [XmlAttribute("Icon")]
        [Category("Properties")]
        [Description("Represents the Icon of toolbar item")]
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
        /// Represents the Icon of toolbar item
        /// </summary>
        [XmlAttribute("ShortCut")]
        [Category("Properties")]
        [Description("Represents the ShortCut of toolbar item")]
        [TrackChanges()]
        public String ShortCut
        {
            get
            {
                return this._shortCut;
            }
            set
            {
                this._shortCut = value;
            }
        }

        /// <summary>
        /// Represents the Icon of toolbar item
        /// </summary>
        [XmlAttribute("Tooltip")]
        [Category("Properties")]
        [Description("Represents the Tooltip of toolbar item")]
        [TrackChanges()]
        public String Tooltip
        {
            get
            {
                return this._toolTip;
            }
            set
            {
                this._toolTip = value;
            }
        }

        /// <summary>
        /// Specifies whether to show dialog on item action and the type of the dialog
        /// </summary>
        [XmlAttribute("ShowDialog")]
        [Category("Properties")]
        [Description("Specifies whether to show dialog on item action and the type of the dialog")]
        [TrackChanges()]
        public ShowDialogEnum ShowDialog
        {
            get
            {
                return this._showDialog;
            }
            set
            {
                this._showDialog = value;
            }
        }

        /// <summary>
        /// Represents the visible property of toolbar item
        /// </summary>
        [XmlAttribute("Visible")]
        [Category("Properties")]
        [Description("Represents the visible property of toolbar item")]
        [TrackChanges()]
        public Boolean Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ToolBarItem"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("Enabled")]
        [Category("Properties")]
        [Description("Represents the enabled property of toolbar item")]
        [TrackChanges()]
        public Boolean Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                this._enabled = value;
            }
        }

        /// <summary>
        /// Represents the Businessrule to be triggered
        /// </summary>

        [XmlAttribute("TriggerBusinessRuleOnPreProcess")]
        [Category("Properties")]
        [Description("Represents the Businessrule to be triggered")]
        [TrackChanges()]
        public Boolean TriggerBusinessRuleOnPreProcess
        {
            get
            {
                return this._triggerBusinessRuleOnPreProcess;
            }
            set
            {
                this._triggerBusinessRuleOnPreProcess = value;
            }
        }

        /// <summary>
        /// Represents if Businessrule to be triggered on Post
        /// </summary>
        [XmlAttribute("TriggerBusinessRuleOnPostProcess")]
        [Category("Properties")]
        [Description("Represents the Businessrule to be triggered")]
        [TrackChanges()]
        public Boolean TriggerBusinessRuleOnPostProcess
        {
            get
            {
                return this._triggerBusinessRuleOnPostProcess;
            }
            set
            {
                this._triggerBusinessRuleOnPostProcess = value;
            }
        }

        /// <summary>
        /// Represents the parameter list for the toolbar item
        /// </summary>
        [Category("Parameters")]
        [Description("Represents the parameter list for the toolbar item")]
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
        /// Specifies if the toolbar item has to displayed as part of More Actions dropdown
        /// </summary>
        [XmlAttribute("ShowInMoreActions")]
        [Category("Properties")]
        [Description("Specifies if the toolbar item has to displayed as part of More Actions dropdown")]
        [TrackChanges()]
        public Boolean ShowInMoreActions
        {
            get
            {
                return this._showInMoreActions;
            }
            set
            {
                this._showInMoreActions = value;
            }
        }


        /// <summary>
        /// Specifies if the toolbar item has to show text on the button
        /// </summary>
        [XmlAttribute("ShowTextOnButton")]
        [Category("Properties")]
        [Description("Specifies if the toolbar item has to text on the button")]
        [TrackChanges()]
        public Boolean ShowTextOnButton
        {
            get
            {
                return this._showTextOnButton;
            }
            set
            {
                this._showTextOnButton = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ToolBarItem class.
        /// </summary>
        public ToolBarItem()
            : base()
        {
            this.AddVerb("Add Parameter");
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
                            ((ToolBarItem)inheritedChild).Parameters.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            param.FindDeletes(_items[0]);
                        }

                        previousSibling = param.UniqueIdentifier;
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

                ToolBarItem _inheritedParent = inheritedParent as ToolBarItem;

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
        /// <returns>XML representation of ToolBar Item</returns>
        public override String ToXml()
        {
            String ToolBarItemXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region ToolBar Item Node

            //Parameter node start
            xmlWriter.WriteStartElement("ToolBarItem");

            xmlWriter.WriteAttributeString("ClassName", this.ClassName);
            xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
            xmlWriter.WriteAttributeString("Type", this.Type);
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Caption", this.GetLocaleMessage(this.Caption));
            xmlWriter.WriteAttributeString("Description", this.GetLocaleMessage(this.Description));
            xmlWriter.WriteAttributeString("OnClick", this.OnClick);
            xmlWriter.WriteAttributeString("OnBeforeClick", this.OnBeforeClick);
            xmlWriter.WriteAttributeString("Icon", this.Icon);
            xmlWriter.WriteAttributeString("Tooltip", this.Tooltip);
            xmlWriter.WriteAttributeString("ShortCut", this.ShortCut);
            xmlWriter.WriteAttributeString("ShowDialog", this.ShowDialog.ToString());
            xmlWriter.WriteAttributeString("Visible", this.Visible.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("TriggerBusinessRuleOnPreProcess", this.TriggerBusinessRuleOnPreProcess.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("TriggerBusinessRuleOnPostProcess", this.TriggerBusinessRuleOnPostProcess.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ShowInMoreActions", this.ShowInMoreActions.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ShowTextOnButton", this.ShowTextOnButton.ToString().ToLowerInvariant());

            #region Params Node

            xmlWriter.WriteStartElement("Params");

            foreach (Parameter param in this.Parameters)
            {
                xmlWriter.WriteRaw(param.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion Params Node

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion ToolBar Item Node

            xmlWriter.Flush();

            //Get the actual XML
            ToolBarItemXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return ToolBarItemXml;
        }

        /// <summary>
        /// Load the ToolBar Item object from the input xml
        /// </summary>
        /// <param name="toolBarItemXml">Tool Bar item xml</param>
        public void LoadToolBarItem(String toolBarItemXml)
        {
            if (!String.IsNullOrWhiteSpace(toolBarItemXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(toolBarItemXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ToolBarItem")
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

                                if (reader.MoveToAttribute("Type"))
                                {
                                    this.Type = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Caption"))
                                {
                                    this.Caption = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Description"))
                                {
                                    this.Description = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("OnClick"))
                                {
                                    this.OnClick = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("OnBeforeClick"))
                                {
                                    this.OnBeforeClick = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Icon"))
                                {
                                    this.Icon = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Tooltip"))
                                {
                                    this.Tooltip = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ShortCut"))
                                {
                                    this.ShortCut = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ShowDialog"))
                                {
                                    ShowDialogEnum showDialogEnum = ShowDialogEnum.None;
                                    Enum.TryParse(reader.ReadContentAsString(), out showDialogEnum);

                                    this.ShowDialog = showDialogEnum;
                                }
                                if (reader.MoveToAttribute("Visible"))
                                {
                                    this.Visible = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._visible);
                                }
                                if (reader.MoveToAttribute("Enabled"))
                                {
                                    this.Enabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._enabled);
                                }
                                if (reader.MoveToAttribute("TriggerBusinessRuleOnPreProcess"))
                                {
                                    this.TriggerBusinessRuleOnPreProcess = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._triggerBusinessRuleOnPreProcess);
                                }
                                if (reader.MoveToAttribute("TriggerBusinessRuleOnPostProcess"))
                                {
                                    this.TriggerBusinessRuleOnPostProcess = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._triggerBusinessRuleOnPostProcess);
                                }
                                if (reader.MoveToAttribute("ShowInMoreActions"))
                                {
                                    this.ShowInMoreActions = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._showInMoreActions);
                                }
                                if (reader.MoveToAttribute("ShowTextOnButton"))
                                {
                                    this.ShowTextOnButton = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._showTextOnButton);
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Params")
                        {
                            String parmsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(parmsXml))
                            {
                                this.Parameters = LoadParameters(parmsXml);
                            }
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

        private RS.MDM.Collections.Generic.List<Parameter> LoadParameters(String parameterAsXml)
        {
            RS.MDM.Collections.Generic.List<Parameter> parameters = new Collections.Generic.List<Parameter>();

            if (!String.IsNullOrWhiteSpace(parameterAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(parameterAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Param")
                        {
                            Parameter parameter = new Parameter();
                            parameter.LoadParameter(reader.ReadOuterXml());

                            parameters.Add(parameter);
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

            return parameters;
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
                validationErrors.Add("The ToolBar item does not contain any parameter.", ValidationErrorType.Warning, "ToolBar Item", this);
            }
        }

        #endregion
    }
}
