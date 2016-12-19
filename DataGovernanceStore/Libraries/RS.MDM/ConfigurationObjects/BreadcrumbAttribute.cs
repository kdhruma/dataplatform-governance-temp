using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using MDM.Core;
using RS.MDM.Configuration;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configurations for Breadcrumb Attirbute settings
    /// </summary>
    [XmlRoot("BreadcrumbAttributes")]
    [Serializable()]
    public class BreadcrumbAttribute : RS.MDM.Object
    {
        #region Private fields
        /// <summary>
        /// Id of an attirbute the value of which is to be displayed on the page.
        /// </summary>
        private Int32 _attributeID = 0;

        /// <summary>
        /// label for attribute value. 
        /// E.g. Product Description = ABC. Here "Product Description" is label
        /// </summary>
        private String _label = String.Empty;

        /// <summary>
        /// Sequence in which given attribute should be displayed on breadcrumb.
        /// Attirbute with Sequence number = 0 comes first
        /// </summary>
        private Int16 _sequenceNumber = 0;

        /// <summary>
        /// Indicates whether attribute is visible on breadcrumb or not.
        /// </summary>
        private Boolean _visible = true;

        /// <summary>
        /// Seperator of attribute label and attribute value.
        /// E.g. "Product Description" = ABC. Here "=" is label seperator.
        /// </summary>
        private String _labelSeparator = "::";

        /// <summary>
        /// Css class name for style for whole section (attribute with value).
        /// </summary>
        private String _cssClassName = String.Empty;

        /// <summary>
        /// Message code or some static text to be added before value. 
        /// Will be separated using space. 
        /// E.g. "Prefix Value"
        /// </summary>
        private String _attributeValuePrefixText = String.Empty;

        /// <summary>
        /// Message code or some static text to be added after value. 
        /// Will be separated using space. 
        /// E.g. "Value Suffix"
        /// </summary>
        private String _attributeValueSuffixText = String.Empty;

        /// <summary>
        /// Css class name for value section.
        /// </summary>
        private String _attributeValueCSSClassName = String.Empty;

        /// <summary>
        /// Css class name for label section.
        /// </summary>
        private String _attributeLabelCSSClassName = String.Empty;

        /// <summary>
        /// Path of css file containing class configured in CSSClassName attribute.
        /// </summary>
        private String _cssFilePath = String.Empty;

        /// <summary>
        /// Collection of additional parameters for breadcrumb attribute
        /// </summary>
        RS.MDM.Collections.Generic.List<Parameter> _parameters = new Collections.Generic.List<Parameter>();

        /// <summary>
        /// Max count of characters to be shown (rest will be hidden by ellipses).
        /// </summary>
        private int _maxCount;

        /// <summary>
        /// Name of callback (javascript) function to be executed on click on node.
        /// </summary>
        private string _onClick = String.Empty;

        #endregion Private fields

        #region Enum
        #endregion Enum

        #region Constructor
        #endregion Constructor

        #region Properties
        
        /// <summary>
        /// Represent the collection of additional parameters for breadcrumb attributes.
        /// </summary>
        [Category("Parameters")]
        [Description("Represent the collection of additional parameters for breadcrumb attributes.")]
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
        /// label for attribute value. 
        /// E.g. Product Description = ABC. Here "Product Description" is label
        /// </summary>
        [XmlAttribute("AttributeID")]
        [Description("Id of an attirbute the value of which is to be displayed on the page.")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public Int32 AttributeID
        {
            get
            {
                return this._attributeID;
            }
            set
            {
                this._attributeID = value;
            }
        }

        /// <summary>
        /// label for attribute value. 
        /// E.g. Product Description = ABC. Here "Product Description" is label
        /// </summary>
        [XmlAttribute("AttributeLabel")]
        [Description("AttributeLabel for attribute value. E.g. Product Description = ABC. Here 'Product Description' is label")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public String AttributeLabel
        {
            get
            {
                return this._label;
            }
            set
            {
                this._label = value;
            }
        }

        /// <summary>
        /// Sequence in which given attribute should be displayed on breadcrumb.
        /// Attirbute with Sequence number = 0 comes first
        /// </summary>
        [XmlAttribute("SequenceNumber")]
        [Description("Sequence in which given attribute should be displayed on breadcrumb. Attirbute with Sequence number = 0 comes first")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public Int16 SequenceNumber
        {
            get
            {
                return this._sequenceNumber;
            }
            set
            {
                this._sequenceNumber = value;
            }
        }

        /// <summary>
        /// Indicates whether attribute is visible on breadcrumb or not.
        /// </summary>
        [XmlAttribute("Visible")]
        [Description("Indicates whether attribute is visible on breadcrumb or not.")]
        [Category("BreadcrumbAttribute")]
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
        /// Seperator of attribute label and attribute value.
        /// E.g. "Product Description" = ABC. Here "=" is label seperator.
        /// </summary>
        [XmlAttribute("LabelSeparator")]
        [Description("Seperator of attribute label and attribute value. E.g. 'Product Description' = ABC. Here '=' is label seperator.")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public String LabelSeparator 
        {
            get
            {
                return this._labelSeparator;
            }
            set
            {
                this._labelSeparator = value;
            }
        }

        /// <summary>
        /// Path of css file containing class configured in CSSClassName attribute.
        /// </summary>
        [XmlAttribute("CSSFilePath")]
        [Description("Path of css file containing class configured in CSSClassName attribute.")]
        [Category("BreadcrumbAttribute")]
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
        /// Css class name for style for whole section (attribute with value).
        /// </summary>
        [XmlAttribute("CSSClassName")]
        [Description("Css class name for style.")]
        [Category("BreadcrumbAttribute")]
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
        /// Message code or some static text to be added before value.
        /// </summary>
        [XmlAttribute("AttributeValuePrefixText")]
        [Description("Message code or some static text to be added before value.")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public String AttributeValuePrefixText
        {
            get
            {
                return _attributeValuePrefixText;
            }
            set
            {
                _attributeValuePrefixText = value;
            }
        }

        /// <summary>
        /// Message code or some static text to be added after value.
        /// </summary>
        [XmlAttribute("AttributeValueSuffixText")]
        [Description("Message code or some static text to be added after value.")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public String AttributeValueSuffixText
        {
            get
            {
                return _attributeValueSuffixText;
            }
            set
            {
                _attributeValueSuffixText = value;
            }
        }

        /// <summary>
        /// Css class name for value section.
        /// </summary>
        [XmlAttribute("AttributeValueCSSClassName")]
        [Description("Css class name for value section.")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public String AttributeValueCSSClassName
        {
            get
            {
                return _attributeValueCSSClassName;
            }
            set
            {
                _attributeValueCSSClassName = value;
            }
        }

        /// <summary>
        /// Css class name for label section.
        /// </summary>
        [XmlAttribute("AttributeLabelCSSClassName")]
        [Description("Css class name for label section.")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public String AttributeLabelCSSClassName
        {
            get
            {
                return _attributeLabelCSSClassName;
            }
            set
            {
                _attributeLabelCSSClassName = value;
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

        /// <summary>
        /// Name of callback (javascript) function to be executed on click on node.
        /// </summary>
        [XmlAttribute("OnClick")]
        [Description("Name of callback (javascript) function to be executed on click on node.")]
        [Category("BreadcrumbAttribute")]
        [TrackChanges()]
        public String OnClick
        {
            get
            {
                return _onClick;
            }
            set
            {
                _onClick = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Define is current attribute from metadata attributes list
        /// </summary>
        /// <returns>[True] in case </returns>
        public Boolean IsMetadataAttribute()
        {
            IEnumerable<Int32> metadataAttributeList = Enum.GetValues(typeof(MetaDataAttributeList)).Cast<Int32>();

            return metadataAttributeList.Contains(this.AttributeID);
        }
        #endregion Methods

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();
            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "BreadcrumbAttribute";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }
            return _treeNode;
        }


        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //BreadcrumbSettings root start
            xmlWriter.WriteStartElement("BreadcrumbAttribute");

            xmlWriter.WriteAttributeString("ClassName", this.ClassName);
            xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Description", this.Description);

            xmlWriter.WriteAttributeString("AttributeID", Convert.ToString(this.AttributeID));
            xmlWriter.WriteAttributeString("AttributeLabel", this.AttributeLabel);
            xmlWriter.WriteAttributeString("SequenceNumber", Convert.ToString(this.SequenceNumber));
            xmlWriter.WriteAttributeString("Visible", Convert.ToString(this.Visible).ToLower());
            xmlWriter.WriteAttributeString("AttributeValuePrefixText", this.AttributeValuePrefixText);
            xmlWriter.WriteAttributeString("AttributeValueSuffixText", this.AttributeValueSuffixText);

            xmlWriter.WriteAttributeString("AttributeValueCSSClassName", this.AttributeValueCSSClassName);
            xmlWriter.WriteAttributeString("AttributeLabelCSSClassName", this.AttributeLabelCSSClassName);
            xmlWriter.WriteAttributeString("OnClick", this.OnClick);

            xmlWriter.WriteAttributeString("CSSFilePath", this.CSSFilePath);
            xmlWriter.WriteAttributeString("CSSClassName", this.CSSClassName);
            xmlWriter.WriteAttributeString("LabelSeparator", this.LabelSeparator);
            xmlWriter.WriteAttributeString("MaxCount", Convert.ToString(this.MaxCount));

            xmlWriter.WriteStartElement("Parameters");

            foreach (var parameter in this.Parameters)
            {
                xmlWriter.WriteRaw(parameter.ToXml());
            }

            //Parameters root node end
            xmlWriter.WriteEndElement();

            //BreadcrumbAttribute root node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String breadcrumbAttributeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return breadcrumbAttributeXml;
        }

        #endregion
    }
}
