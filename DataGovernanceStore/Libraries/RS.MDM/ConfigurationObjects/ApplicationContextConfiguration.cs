using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using MDM.Core;
using Newtonsoft.Json.Linq;

namespace RS.MDM.ConfigurationObjects
{
    
    using RS.MDM.Configuration;
    using RS.MDM.Validations;

    /// <summary>
    /// Specifies layout for application context configuration object
    /// </summary>
    public enum LayoutEnum
    {
        Horizontal = 0,
        Vertical = 1,
    }

    /// <summary>
    /// Provides configuration for application context configuration
    /// </summary>
    [XmlRoot("ApplicationContextConfiguration")]
    [Serializable()]
    [XmlInclude(typeof(ApplicationContextConfigurationItem))]
    public class ApplicationContextConfiguration: Object
    {
        #region Fields

        /// <summary>
        /// field for the layout of the ApplicationContextConfiguration
        /// </summary>
        private LayoutEnum _layout = LayoutEnum.Horizontal;

        /// <summary>
        /// field to denote if the ApplicationContextConfiguration is visible
        /// </summary>
        private Boolean _showLabel = false;

        /// <summary>
        /// field for ApplicationContextConfigurationItems
        /// </summary>
        private RS.MDM.Collections.Generic.List<ApplicationContextConfigurationItem> _applicationContextConfigurationItems = new RS.MDM.Collections.Generic.List<ApplicationContextConfigurationItem>();

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the title of the EntityHierarchyPanelConfigItem
        /// </summary>
        [XmlAttribute("Layout")]
        [Description("ApplicationContextConfigurationItem layout.")]
        [Category("ApplicationContextConfigurationItem")]
        [TrackChanges()]
        public LayoutEnum Layout
        {
            get
            {
                return this._layout;
            }
            set
            {
                this._layout = value;
            }
        }

        /// <summary>
        /// Indicates if the EntityHierarchyPanelConfigItem is visible
        /// </summary>
        [XmlAttribute("ShowLabel")]
        [Description("ApplicationContextConfigurationItem is visible or not")]
        [Category("ApplicationContextConfigurationItem")]
        [TrackChanges()]
        public Boolean ShowLabel
        {
            get
            {
                return this._showLabel;
            }
            set
            {
                this._showLabel = value;
            }
        }

        /// <summary>
        /// Indicates ApplicationContextConfigurationItemw
        /// </summary>
        [Category("ApplicationContextConfigurationItems")]
        [TrackChanges()]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<ApplicationContextConfigurationItem> ApplicationContextConfigurationItems
        {
            get
            {
                this.SetParent();
                return this._applicationContextConfigurationItems;
            }
            set
            {
                this._applicationContextConfigurationItems = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public ApplicationContextConfiguration()
            : base()
        {
        }

        public ApplicationContextConfiguration(String valuesAsXml)
        {
            LoadParameter(valuesAsXml);
        }

        #endregion

        #region Serialization & De-serialization

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

        #region Public Methods

        /// <summary>
        /// Get XML representation of the ApplicationContextConfiguration object
        /// </summary>
        /// <returns>XML representation of ApplicationContextConfiguration</returns>
        public override String ToXml()
        {
            String parameterXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ApplicationContextConfiguration node start
            xmlWriter.WriteStartElement("ApplicationContextConfiguration");

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("Layout", this.Layout.ToString());
            xmlWriter.WriteAttributeString("ShowLabel", this.ShowLabel.ToString());

            #region ApplicationContextConfigurationItems Node

            xmlWriter.WriteStartElement("ApplicationContextConfigurationItems");

            foreach (ApplicationContextConfigurationItem applicationContextConfigurationItem in this.ApplicationContextConfigurationItems)
            {
                xmlWriter.WriteRaw(applicationContextConfigurationItem.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion ApplicationContextConfigurationItems Node

            //ApplicationContextConfiguration node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            parameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return parameterXml;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (ApplicationContextConfigurationItem _item in this._applicationContextConfigurationItems)
            {
                if (_item != null)
                {
                    _item.GenerateNewUniqueIdentifier();
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

            foreach (ApplicationContextConfigurationItem _item in this._applicationContextConfigurationItems)
            {
                _list.AddRange(_item.FindChildren(uniqueIdentifier, includeDeletedItems));
            }

            return _list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._applicationContextConfigurationItems != null)
            {
                foreach (ApplicationContextConfigurationItem _item in this._applicationContextConfigurationItems)
                {
                    if (_item != null)
                    {
                        _item.Parent = this;
                        _item.InheritedParent = this.InheritedParent;
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

            if (this._applicationContextConfigurationItems != null && this._applicationContextConfigurationItems.Count > 0)
            {
                for (int i =  this._applicationContextConfigurationItems.Count - 1; i > -1; i--)
                {
                    ApplicationContextConfigurationItem _item = this._applicationContextConfigurationItems[i];

                    if (_item != null)
                    {
                        if (_item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._applicationContextConfigurationItems.Remove(_item);
                        }
                        else
                        {
                            _item.AcceptChanges();
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

            if (this._applicationContextConfigurationItems != null)
            {
                foreach (ApplicationContextConfigurationItem _item in this._applicationContextConfigurationItems)
                {
                    if (_item != null)
                    {
                        _item.FindChanges();
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

            if (this._applicationContextConfigurationItems != null)
            {
                _previousSibling = string.Empty;
                foreach (ApplicationContextConfigurationItem _item in this._applicationContextConfigurationItems)
                {
                    if (_item != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(_item.UniqueIdentifier, true);
                        if (_items.Count == 0)
                        {
                            Panel _panelClone = RS.MDM.Object.Clone(_item, false) as Panel;
                            _panelClone.PropertyChanges.Items.Clear();
                            _panelClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((PanelBar)inheritedChild).Panels.InsertAfter(_previousSibling, _panelClone);
                        }
                        else
                        {
                            _item.FindDeletes(_items[0]);
                        }
                        _previousSibling = _item.UniqueIdentifier;
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
                foreach (ApplicationContextConfigurationItem _item in this._applicationContextConfigurationItems)
                {
                    switch (_item.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Panel _panelClone = RS.MDM.Object.Clone(_item, false) as Panel;
                            _inheritedParent.Panels.InsertAfter(_previousSibling, _panelClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Panel _inheritedChild = _inheritedParent.Panels.GetItem(_item.UniqueIdentifier);
                            _item.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = _item.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (ApplicationContextConfigurationItem _item in this._applicationContextConfigurationItems)
                {
                    if (_item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Panels.Remove(_item.UniqueIdentifier);
                    }
                    else
                    {
                        Panel _inheritedChild = _inheritedParent.Panels.GetItem(_item.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Get JSON representation of application context object
        /// </summary>
        /// <returns>Returns the json objects as result</returns>
        public new JObject ToJSON()
        {
            return this.ToApplicationContextJSON();
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

            if (this._applicationContextConfigurationItems.Count == 0)
            {
                validationErrors.Add("The application context configuration item does not contain any items", ValidationErrorType.Warning, "ApplicationContextConfigurationItems", this);
            }
        }

        #endregion

        #region Private Methods

        private JObject ToApplicationContextJSON()
        {
            return new JObject(
                new JProperty("Items",
                    new JArray(
                        from item in this.ApplicationContextConfigurationItems
                        select new JObject(
                            new JProperty("name", item.Name),
                            new JProperty("objectType", item.ObjectType.ToString()),
                            new JProperty("visible", item.Visible.ToString())))),
                new JProperty("Options",
                    new JObject(
                        new JProperty("layout", this.Layout.ToString()),
                        new JProperty("showLabel", this.ShowLabel.ToString()))));
        }

        /// <summary>
        /// Load the ApplicationContextConfiguration object from the input xml
        /// </summary>
        /// <param name="valueAsXml">ApplicationContextConfiguration object representation as xml</param>
        private void LoadParameter(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContextConfiguration")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Description"))
                                {
                                    this.Description = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ShowLabel"))
                                {
                                    this.ShowLabel = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.ShowLabel);
                                }
                                if (reader.MoveToAttribute("Layout"))
                                {
                                    LayoutEnum layoutEnum = LayoutEnum.Horizontal;
                                    Enum.TryParse(reader.ReadContentAsString(), out layoutEnum);
                                    this.Layout = layoutEnum;
                                }
                            }
                        }
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContextConfigurationItem")
                        {
                            //Read ApplicationContextConfigurationItems
                            #region Read ApplicationContextConfigurationItems

                            String applicationContextConfigurationXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(applicationContextConfigurationXml))
                            {
                                ApplicationContextConfigurationItem applicationContextConfigurationItem = new ApplicationContextConfigurationItem(applicationContextConfigurationXml);
                                if (applicationContextConfigurationItem != null)
                                {
                                    this.ApplicationContextConfigurationItems.Add(applicationContextConfigurationItem);
                                }
                            }

                            #endregion Read ApplicationContextConfigurationItems
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

        #endregion
    }
}