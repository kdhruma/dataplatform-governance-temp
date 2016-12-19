using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Diagnostics;

using RS.MDM.Validations;
using RS.MDM.Configuration;
using MDM.Interfaces;
using MDM.Utility;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Specifies positions for PanelBar
    /// </summary>
    public enum PanelBarPosition
    {
        Left,
        Right,
        Undefined
    }

    /// <summary>
    /// Specifies positions for PanelBar
    /// </summary>
    public enum DockMode
    {
        Floating = 1,
        Docked = 2,
        Default = 3
    }

    /// <summary>
    /// Provides configuration for the PanelBar
    /// </summary>
    [XmlRoot("PanelBar")]
    [Serializable()]
    [XmlInclude(typeof(Panel))]
    [XmlInclude(typeof(AttributePanel))]
    [XmlInclude(typeof(ContainerPanel))]
    public sealed class PanelBar : Object
    {
        #region Fields

        /// <summary>
        /// field for the title of the panelbar
        /// </summary>
        private string _title = String.Empty;

        /// <summary>
        /// field for position of the panel bar
        /// </summary>
        private PanelBarPosition _position = PanelBarPosition.Undefined;

        /// <summary>
        /// field which indicates Unit Of Measure
        /// </summary>
        private UnitType _unitOfMeasure = UnitType.Percentage;

        /// <summary>
        /// field for the minimum width of the panelbar
        /// </summary>
        private string _minWidth = "100";

        /// <summary>
        /// field for expand mode
        /// </summary>
        private DockMode _dockMode = DockMode.Docked;

        /// <summary>
        /// field for the items in the panelbar
        /// </summary>
        private RS.MDM.Collections.Generic.List<Panel> _panels = new RS.MDM.Collections.Generic.List<Panel>();

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the title of the panel bar
        /// </summary>
        [XmlAttribute("Title")]
        [Description("Indicates the title of the panel bar.")]
        [Category("PanelBar")]
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
        /// Indicates the position of the panelbar
        /// </summary>
        [XmlAttribute("Position")]
        [Description("Indicates the Position of the PanelBar.")]
        [Category("PanelBar")]
        [TrackChanges()]
        public PanelBarPosition Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        /// <summary>
        /// Indicates Unit Of Measure(Pixel or Percentage) to be used for the Panels.
        /// </summary>
        [XmlAttribute("UnitOfMeasure")]
        [Description("Indicates Unit Of Measure(Pixel or Percentage) to calculate Panel width.")]
        [Category("PanelBar")]
        [TrackChanges()]
        public UnitType UnitOfMeasure
        {
            get
            {
                return this._unitOfMeasure;
            }
            set
            {
                if (value == UnitType.Pixel || value == UnitType.Percentage) // Restrict the Unit Types to be only Pixel and percentage.
                    this._unitOfMeasure = value;
                else
                    this._unitOfMeasure = UnitType.Percentage;
            }
        }

        /// <summary>
        /// Indicates the minimum width of the panelbar
        /// </summary>
        [XmlAttribute("MinWidth")]
        [Description("Indicates the Minimum Width for the PanelBar.")]
        [Category("PanelBar")]
        [TrackChanges()]
        public string MinWidth
        {
            get
            {
                return this._minWidth;
            }
            set
            {
                this._minWidth = value;
            }
        }

        /// <summary>
        /// Indicates the expand mode for the panelbar
        /// </summary>
        [XmlAttribute("DockMode")]
        [Description("Indicates the Dockmode for the PanelBar.")]
        [Category("PanelBar")]
        [TrackChanges()]
        public DockMode DockMode
        {
            get
            {
                return this._dockMode;
            }
            set
            {
                this._dockMode = value;
            }
        }

        /// <summary>
        /// Indicates the panels in the panelbar
        /// </summary>
        [Description("Indicates the Panels for the PanelBar.")]
        [Category("PanelBar")]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<Panel> Panels
        {
            get
            {
                this.SetParent();
                return this._panels;
            }
            set
            {
                this._panels = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public PanelBar()
            : base()
        {
            this.AddVerb("Add Panel");
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Mapped Attribute Name.
        /// </summary>
        /// <param name="name">Possible values for Name: ShortName or LongName</param>
        /// <returns>Attribute Unique Identifier</returns>
        public IAttributeUniqueIdentifier GetMappedAttributeIdentifier(String name)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading of Mapped Attribute as '{0}' from MetaDataPage - Config started...", name));

            IAttributeUniqueIdentifier iAttributeUniqueIdentifier = null;

            foreach (Panel panel in this.Panels)
            {
                if (panel is AttributePanel)
                {
                    AttributePanel attributePanel = (AttributePanel)panel;

                    foreach (AttributePanelItem attributePanelItem in attributePanel.AttributePanelItems)
                    {
                        if (!attributePanelItem.Name.Equals(name))
                            continue;

                        iAttributeUniqueIdentifier = MDMObjectFactory.GetIAttributeUniqueIdentifier(attributePanelItem.AttributeName, attributePanelItem.AttributeGroupName);
                        break;
                    }
                }
            }

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading of Mapped Attribute as '{0}' from MetaDataPage - Config completed.", name));

            return iAttributeUniqueIdentifier;
        }

        #endregion

        #region Private Methods
        #endregion

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

            foreach (Panel _panel in this._panels)
            {
                if (_panel != null)
                {
                    _panel.GenerateNewUniqueIdentifier();
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

            foreach (Panel _panel in this._panels)
            {
                _list.AddRange(_panel.FindChildren(uniqueIdentifier, includeDeletedItems));
            }

            return _list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._panels != null)
            {
                foreach (Panel _panel in this._panels)
                {
                    if (_panel != null)
                    {
                        _panel.Parent = this;
                        _panel.InheritedParent = this.InheritedParent;
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

            if (this._panels != null && this._panels.Count > 0)
            {
                for (int i = _panels.Count - 1; i > -1; i--)
                {
                    Panel _panel = _panels[i];

                    if (_panel != null)
                    {
                        if (_panel.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._panels.Remove(_panel);
                        }
                        else
                        {
                            _panel.AcceptChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the changes of an object wrt an instance of an inherited parent
        /// </summary>
        public override void FindChanges()
        {
            base.FindChanges();

            if (this._panels != null)
            {
                foreach (Panel _panel in this._panels)
                {
                    if (_panel != null)
                    {
                        _panel.FindChanges();
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

            if (this._panels != null)
            {
                _previousSibling = string.Empty;
                foreach (Panel _panel in this._panels)
                {
                    if (_panel != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(_panel.UniqueIdentifier, true);
                        if (_items.Count == 0)
                        {
                            Panel _panelClone = RS.MDM.Object.Clone(_panel, false) as Panel;
                            _panelClone.PropertyChanges.Items.Clear();
                            _panelClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((PanelBar)inheritedChild).Panels.InsertAfter(_previousSibling, _panelClone);
                        }
                        else
                        {
                            _panel.FindDeletes(_items[0]);
                        }
                        _previousSibling = _panel.UniqueIdentifier;
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
                PanelBar _inheritedParent = inheritedParent as PanelBar;
                string _previousSibling = string.Empty;

                // Apply all the changes
                foreach (Panel _panel in this._panels)
                {
                    switch (_panel.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Panel _panelClone = RS.MDM.Object.Clone(_panel, false) as Panel;
                            _inheritedParent.Panels.InsertAfter(_previousSibling, _panelClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Panel _inheritedChild = _inheritedParent.Panels.GetItem(_panel.UniqueIdentifier);
                            _panel.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = _panel.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (Panel _panel in this._panels)
                {
                    if (_panel.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Panels.Remove(_panel.UniqueIdentifier);
                    }
                    else
                    {
                        Panel _inheritedChild = _inheritedParent.Panels.GetItem(_panel.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Get a tree node that reprents an object and its aggregated children
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

            System.Windows.Forms.TreeNode panelNodes = new System.Windows.Forms.TreeNode("Panels");
            panelNodes.ImageKey = "Items";
            panelNodes.SelectedImageKey = panelNodes.ImageKey;
            panelNodes.Tag = this._panels;
            _treeNode.Nodes.Add(panelNodes);

            foreach (Panel _panel in this._panels)
            {
                if (_panel != null)
                {
                    panelNodes.Nodes.Add(_panel.GetTreeNode());
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
                case "Add Panel":
                    this.Panels.Add(new Panel());
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

            if (this._panels.Count == 0)
            {
                validationErrors.Add("The Panel Bar does not contain any items", ValidationErrorType.Warning, "Panels", this);
            }
        }

        #endregion
    }
}
