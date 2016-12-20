using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Core.Exceptions;


    /// <summary>
    /// Specifies the Attribute Value Instance for the Object
    /// </summary>
    [DataContract]
    public class AttributeProcessingOptions : ObjectBase,IAttributeProcessingOptions
    {
        #region Fields
        /// <summary>
        ///
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        ///
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private Boolean _canDeleteAttribute = true;

        /// <summary>
        ///
        /// </summary>
        private Boolean _canUpdateAttribute = true;

        /// <summary>
        ///
        /// </summary>
        private Boolean _canAddAttribute = true;

        /// <summary>
        ///
        /// </summary>
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        /// <summary>
        /// 
        /// </summary>
        private AttributeModelType _attributeModelType = AttributeModelType.Unknown;

        #endregion

        #region Properties

        /// <summary>
        /// Id of the attribute which will added/updated/deleted based on Attribute Action flag in Import Profile
        /// </summary>
        [DataMember]
        public int AttributeId
        {
            get
            {
                return _attributeId;
            }
            set
            {
                _attributeId = value;
            }
        }

        /// <summary>
        /// Name of the attribute which will added/updated/deleted based on Attribute Action flag in Import Profile
        /// </summary>
        [DataMember]
        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        /// <summary>
        /// Flag which will decide whether the attribute will be added
        /// </summary>
        [DataMember]
        public Boolean CanAddAttribute
        {
            get { return _canAddAttribute; }
            set { _canAddAttribute = value; }
        }

        /// <summary>
        /// Flag which will decide whether the attribute will be deleted
        /// </summary>
        [DataMember]
        public Boolean CanDeleteAttribute
        {
            get { return _canDeleteAttribute; }
            set { _canDeleteAttribute = value; }
        }

        /// <summary>
        /// Flag which will decide whether the attribute will be updated
        /// </summary>
        [DataMember]
        public Boolean CanUpdateAttribute
        {
            get { return _canUpdateAttribute; }
            set { _canUpdateAttribute = value; }
        }

        /// <summary>
        /// Locale of the Attribute
        /// </summary>
        [DataMember]
        public LocaleEnum Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }
        
        /// <summary>
        /// ModelType of the Attribute For. e.g. Common
        /// </summary>
        [DataMember]
        public AttributeModelType AttributeModelType
        {
            get { return _attributeModelType; }
            set { _attributeModelType = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor to initialize attributeprocessingoptions
        /// </summary>
        public AttributeProcessingOptions()
        {

        }

        /// <summary>
        /// Construct Attribute Processing Options from XML
        /// </summary>
        public AttributeProcessingOptions(String attributeProcessingOptionsXml)
        {
            LoadAttributeModelFromXml(attributeProcessingOptionsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Attribute Processing Options
        /// </summary>
        /// <returns>Xml representation of Attribute Processing Options</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //AttributeProcessingOptions node start
                    xmlWriter.WriteStartElement("AttributeProcessingOptions");

                    xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
                    xmlWriter.WriteAttributeString("AttributeName", this.AttributeName);
                    xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                    xmlWriter.WriteAttributeString("CanAddAttribute", this.CanAddAttribute.ToString());
                    xmlWriter.WriteAttributeString("CanUpdateAttribute", this.CanUpdateAttribute.ToString());
                    xmlWriter.WriteAttributeString("CanDeleteAttribute", this.CanDeleteAttribute.ToString());

                    //AttributeProcessingOptions node end
                    xmlWriter.WriteEndElement();
                }

                //get the actual XML
                returnXml = sw.ToString();
            }

            return returnXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the object from XML
        /// </summary>
        /// <param name="attributeProcessingOptionsXml">Attribute Processing Options XML</param>
        private void LoadAttributeModelFromXml(String attributeProcessingOptionsXml)
        {
            using (XmlTextReader reader = new XmlTextReader(attributeProcessingOptionsXml, XmlNodeType.Element, null))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("AttributeId"))
                            {
                                this._attributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._attributeId);
                            }

                            if (reader.MoveToAttribute("AttributeName"))
                            {
                                this._attributeName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("AttributeModelType"))
                            {
                                ValueTypeHelper.EnumTryParse<AttributeModelType>(reader.ReadContentAsString(), true, out this._attributeModelType);
                            }

                            if (reader.MoveToAttribute("CanAddAttribute"))
                            {
                                this._canAddAttribute = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._canAddAttribute);
                            }

                            if (reader.MoveToAttribute("CanUpdateAttribute"))
                            {
                                this._canUpdateAttribute = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._canUpdateAttribute);
                            }

                            if (reader.MoveToAttribute("CanDeleteAttribute"))
                            {
                                this._canDeleteAttribute = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._canDeleteAttribute);
                            }
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
