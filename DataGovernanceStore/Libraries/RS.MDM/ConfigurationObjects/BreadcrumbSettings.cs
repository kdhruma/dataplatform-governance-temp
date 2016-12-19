using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using RS.MDM.Configuration;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configurations for Breadcrumb settings
    /// </summary>
    [XmlRoot("BreadcrumbSettings")]
    [Serializable()]
    public class BreadcrumbSettings : RS.MDM.Object
    {
        #region Private fields

        /// <summary>
        /// Represents config type.
        /// Config type representes what is the use of retuened string of attribute name - value pair
        /// </summary>
        private String _userConfigType = String.Empty;

        /// <summary>
        /// Represent the collection of BreadCrimAttributes which needs to be displayed on breadcrumb
        /// </summary>
        private RS.MDM.Collections.Generic.List<BreadcrumbAttribute> _breadcrumbAttributes = new RS.MDM.Collections.Generic.List<BreadcrumbAttribute>();

        /// <summary>
        /// Path of css file containing class configured in CSSClassName attribute.
        /// </summary>
        private String _cssFilePath = String.Empty;

        /// <summary>
        /// Css class name for style of the whole breadcrumb section
        /// </summary>
        private String _cssClassName = String.Empty;

        /// <summary>
        /// Separator between components. If not sent will fallback to 'ComponentDetail_Separator' from AppConfig.
        /// </summary>
        private String _attributeSeparator = String.Empty;

        /// <summary>
        /// Max count of characters to be shown (rest will be hidden by ellipses).
        /// </summary>
        private int _maxCount;

        #endregion Private fields

        #region Constructor
        #endregion Constructor

        #region Properties

        /// <summary>
        /// Represents config type.
        /// Config type representes what is the use of returned string of attribute name - value pair
        /// </summary>
        [XmlAttribute("UserConfigType")]
        [Description("Represents config type. Config type representes what is the use of returned string of attribute name - value pair")]
        [Category("BreadcrumbSettings")]
        [TrackChanges()]
        public String UserConfigType
        {
            get
            {
                return this._userConfigType;
            }
            set
            {
                this._userConfigType = value;
            }
        }

        /// <summary>
        /// Path of css file containing class configured in CSSClassName attribute.
        /// </summary>
        [XmlAttribute("CSSFilePath")]
        [Description("Path of css file containing class configured in CSSClassName attribute.")]
        [Category("BreadcrumbSettings")]
        [TrackChanges()]
        public String CSSFilePath
        {
            get
            {
                return _cssFilePath;
            }
            set
            {
                _cssFilePath = value;
            }
        }

        /// <summary>
        /// Css class name for style of the whole breadcrumb section.
        /// </summary>
        [XmlAttribute("CSSClassName")]
        [Description("Css class name for style of the whole breadcrumb section.")]
        [Category("BreadcrumbSettings")]
        [TrackChanges()]
        public String CSSClassName
        {
            get
            {
                return _cssClassName;
            }
            set
            {
                _cssClassName = value;
            }
        }

        /// <summary>
        /// Separator between attributes (label - value pairs). If not sent will fallback to 'ComponentDetail_Separator' from AppConfig.
        /// </summary>
        [XmlAttribute("AttributeSeparator")]
        [Description("Separator between attributes (label - value pairs). If not sent will fallback to 'ComponentDetail_Separator' from AppConfig")]
        [Category("BreadcrumbSettings")]
        [TrackChanges()]
        public String AttributeSeparator
        {
            get
            {
                return _attributeSeparator;
            }
            set
            {
                _attributeSeparator = value;
            }
        }

        /// <summary>
        /// Represent the collection of BreadCrimAttributes which needs to be displayed on breadcrumb
        /// </summary>
        [Description("Represent the collection of BreadCrimAttributes which needs to be displayed on breadcrumb")]
        [Category("Attributes")]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<BreadcrumbAttribute> BreadcrumbAttributes
        {
            get
            {
                return this._breadcrumbAttributes;
            }
            set
            {
                this._breadcrumbAttributes = value;
            }
        }

        /// <summary>
        /// Max count of characters to be shown (rest will be hidden by ellipses).
        /// </summary>
        [XmlAttribute("MaxCount")]
        [Description("Max count of characters to be shown (rest will be hidden by ellipses).")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public Int32 MaxCount
        {
            get
            {
                return _maxCount;
            }
            set
            {
                _maxCount = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gives XML representation of Breadcrumb Settings with the attributes in the format expected by SP.
        /// </summary>
        /// <returns>XML representation of BreadcrumbSettings for sending it to DB to get attribute value string</returns>
        public String GetXML()
        {
            String returnValue = String.Empty;
            if (!String.IsNullOrEmpty(this.UserConfigType) && this.BreadcrumbAttributes != null && this.BreadcrumbAttributes.Count > 0)
            {
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlTextWriter.Formatting = Formatting.Indented;

                // Start document
                //xmlTextWriter.WriteStartDocument();
                // Strat UserConfig element
                xmlTextWriter.WriteStartElement("UserConfig");
                // UserConfigType
                xmlTextWriter.WriteAttributeString("UserConfigType", this.UserConfigType);

                foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
                {
                    if (_breadcrumbAttribute != null)
                    {
                        // Attribute element for each breadcrumb attributes
                        // Start Attribtue element
                        xmlTextWriter.WriteStartElement("Attribute");

                        // Attributes of an element
                        xmlTextWriter.WriteAttributeString("Name", _breadcrumbAttribute.AttributeLabel);
                        xmlTextWriter.WriteAttributeString("ID", _breadcrumbAttribute.AttributeID.ToString());
                        xmlTextWriter.WriteAttributeString("SelectedSeqNo", _breadcrumbAttribute.SequenceNumber.ToString());
                        xmlTextWriter.WriteAttributeString("Selected", _breadcrumbAttribute.Visible.ToString().ToLower());
                        xmlTextWriter.WriteAttributeString("LabelSeparator", _breadcrumbAttribute.LabelSeparator);

                        // End Attribute element
                        xmlTextWriter.WriteEndElement();
                    }
                }

                // End UserConfig element
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.Flush();
                xmlTextWriter.Close();
                stringWriter.Flush();
                stringWriter.Close();

                returnValue = stringWriter.ToString();
            }
            else
            {
                throw new ArgumentException("Either UserConfigType or BreadcrumbAttributes not set.");
            }
            return returnValue;
        }

        #endregion Methods

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
            {
                if (_breadcrumbAttribute != null)
                {
                    _breadcrumbAttribute.GenerateNewUniqueIdentifier();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueIdentifier"></param>
        /// <param name="includeDeletedItems"></param>
        /// <returns></returns>
        public override List<RS.MDM.Object> FindChildren(string uniqueIdentifier, bool includeDeletedItems)
        {
            List<RS.MDM.Object> _list = new List<Object>();
            _list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));

            foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
            {
                _list.AddRange(_breadcrumbAttribute.FindChildren(uniqueIdentifier, includeDeletedItems));
            }
            return _list;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetParent()
        {
            if (this.BreadcrumbAttributes != null)
            {
                foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
                {
                    if (_breadcrumbAttribute != null)
                    {
                        _breadcrumbAttribute.Parent = this;
                        _breadcrumbAttribute.InheritedParent = this.InheritedParent;
                    }
                }
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public override void AcceptChanges()
        {
            base.AcceptChanges();
            if (this.BreadcrumbAttributes != null && this.BreadcrumbAttributes.Count > 0)
            {
                for (int i = this.BreadcrumbAttributes.Count - 1; i > -1; i--)
                {
                    BreadcrumbAttribute _breadcrumbAttribute = this.BreadcrumbAttributes[i];
                    if (_breadcrumbAttribute != null)
                    {
                        if (_breadcrumbAttribute.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this.BreadcrumbAttributes.Remove(_breadcrumbAttribute);
                        }
                        else
                        {
                            _breadcrumbAttribute.AcceptChanges();
                        }
                    }
                }
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public override void FindChanges()
        {
            base.FindChanges();

            if (this.BreadcrumbAttributes != null)
            {
                foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
                {
                    if (_breadcrumbAttribute != null)
                    {
                        _breadcrumbAttribute.FindChanges();
                    }
                }
            }



            if (this.Parent == null && this.InheritedParent != null)
            {
                this.InheritedParent.FindDeletes(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inheritedChild"></param>
        public override void FindDeletes(Object inheritedChild)
        {
            base.FindDeletes(inheritedChild);
            string _previousSibling = string.Empty;
            if (this.BreadcrumbAttributes != null)
            {
                foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
                {
                    if (_breadcrumbAttribute != null)
                    {
                        List<RS.MDM.Object> _breadcrumbAttributeLocal = inheritedChild.FindChildren(_breadcrumbAttribute.UniqueIdentifier, true);
                        if (_breadcrumbAttributeLocal.Count == 0)
                        {
                            BreadcrumbAttribute _breadcrumbAttributeClone = RS.MDM.Object.Clone(_breadcrumbAttribute, false) as BreadcrumbAttribute;
                            _breadcrumbAttributeClone.PropertyChanges.Items.Clear();
                            _breadcrumbAttributeClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((BreadcrumbSettings)inheritedChild).BreadcrumbAttributes.InsertAfter(_previousSibling, _breadcrumbAttributeClone);
                        }
                        else
                        {
                            _breadcrumbAttribute.FindDeletes(_breadcrumbAttributeLocal[0]);
                        }
                        _previousSibling = _breadcrumbAttribute.UniqueIdentifier;
                    }
                }
            }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationErrors"></param>
        public override void Validate(ref RS.MDM.Validations.ValidationErrorCollection validationErrors)
        {
            this.SetParent();
            if (validationErrors == null)
            {
                validationErrors = new RS.MDM.Validations.ValidationErrorCollection();
            }

            base.Validate(ref validationErrors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inheritedParent"></param>
        public override void InheritParent(RS.MDM.Object inheritedParent)
        {
            if (inheritedParent != null)
            {
                base.InheritParent(inheritedParent);
                BreadcrumbSettings _inheritedParent = inheritedParent as BreadcrumbSettings;
                string _previousSibling = string.Empty;

                //// Apply all the changes
                foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
                {
                    switch (_breadcrumbAttribute.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            BreadcrumbAttribute _breadcrumbAttributeClone = RS.MDM.Object.Clone(_breadcrumbAttribute, false) as BreadcrumbAttribute;
                            _inheritedParent.BreadcrumbAttributes.InsertAfter(_previousSibling, _breadcrumbAttributeClone);
                            break;
                        case InheritedObjectStatus.Change:
                            BreadcrumbAttribute _inheritedChild = _inheritedParent.BreadcrumbAttributes.GetItem(_breadcrumbAttribute.UniqueIdentifier);
                            _breadcrumbAttribute.InheritParent(_inheritedChild);
                            break;
                    }
                    _previousSibling = _breadcrumbAttribute.UniqueIdentifier;
                }

                //// Cleanup the changes
                foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
                {
                    if (_breadcrumbAttribute.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.BreadcrumbAttributes.Remove(_breadcrumbAttribute.UniqueIdentifier);
                    }
                    else
                    {
                        BreadcrumbAttribute _inheritedChild = _inheritedParent.BreadcrumbAttributes.GetItem(_breadcrumbAttribute.UniqueIdentifier);
                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();
            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "BreadcrumbSettings";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            System.Windows.Forms.TreeNode _breadcrumbAttributeNodes = new System.Windows.Forms.TreeNode("Header Breadcrumb Attributes");
            _treeNode.Nodes.Add(_breadcrumbAttributeNodes);
            _breadcrumbAttributeNodes.ImageKey = "Items";
            _breadcrumbAttributeNodes.SelectedImageKey = _breadcrumbAttributeNodes.ImageKey;
            _breadcrumbAttributeNodes.Tag = this.BreadcrumbAttributes;
            foreach (BreadcrumbAttribute _breadcrumbAttribute in this.BreadcrumbAttributes)
            {
                if (_breadcrumbAttribute != null)
                {
                    _breadcrumbAttributeNodes.Nodes.Add(_breadcrumbAttribute.GetTreeNode());
                }
            }
            return _treeNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        public override void OnDesignerVerbClick(string text, Object configObject, object treeView)
        {
            ConfigurationObject configurationObject = null;
            base.OnDesignerVerbClick(text, configObject, treeView);
            switch (text)
            {
                case "Add Breadcrumb Attribute":
                    this.BreadcrumbAttributes.Add(new BreadcrumbAttribute());
                    break;
            }
            if (text != "Find Changes" && text != "Accept Changes" && configObject != null && configObject is ConfigurationObject)
            {
                configurationObject = configObject as ConfigurationObject;
                configurationObject._isConfigDirty = true;
            }
            System.ComponentModel.TypeDescriptor.Refresh(this);
        }

        public override string ToXml()
        {
            String breadcrumbSettingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //BreadcrumbSettings root start
            xmlWriter.WriteStartElement("BreadcrumbSettings");

            xmlWriter.WriteAttributeString("ClassName", this.ClassName);
            xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
            xmlWriter.WriteAttributeString("Name", this.Name);

            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("UserConfigType", this.UserConfigType);
            xmlWriter.WriteAttributeString("CSSFilePath", this.CSSFilePath);
            xmlWriter.WriteAttributeString("CSSClassName", this.CSSClassName);
            xmlWriter.WriteAttributeString("AttributeSeparator", this.AttributeSeparator);
            xmlWriter.WriteAttributeString("MaxCount", Convert.ToString(this.MaxCount));
            
            xmlWriter.WriteStartElement("BreadcrumbAttributes");

            foreach (var breadcrumbAttribute in this.BreadcrumbAttributes)
            {
                xmlWriter.WriteRaw(breadcrumbAttribute.ToXml());
            }

            //BreadcrumbAttributes root node end
            xmlWriter.WriteEndElement();

            //BreadcrumbSettings root node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            breadcrumbSettingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return breadcrumbSettingXml;
        }

        #endregion
    }
}
