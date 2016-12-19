using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the validation states
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class ValidationStates : MDMObject, IValidationStates
    {
        #region Fields

        /// <summary>
        /// Field denoting isSelfValid validation state attribute value
        /// </summary>
        private ValidityStateValue _isSelfValid = ValidityStateValue.NotChecked;

        /// <summary>
        /// Field denoting isMetaDataValid validation state attribute value
        /// </summary>
        private ValidityStateValue _isMetaDataValid = ValidityStateValue.NotChecked;

        /// <summary>
        /// Field denoting isCommonAttributesValid validation state attribute value
        /// </summary>
        private ValidityStateValue _isCommonAttributesValid = ValidityStateValue.NotChecked;

        /// <summary>
        /// Field denoting isCategoryAttributesValid validation state attribute value
        /// </summary>
        private ValidityStateValue _isCategoryAttributesValid = ValidityStateValue.NotChecked;

        /// <summary>
        /// Field denoting isRelationshipsValid validation state attribute value
        /// </summary>
        private ValidityStateValue _isRelationshipsValid = ValidityStateValue.NotChecked;

        /// <summary>
        /// Field denoting isEntityVariantValid validation state attribute value
        /// </summary>
        private ValidityStateValue _isEntityVariantValid = ValidityStateValue.NotChecked;

        /// <summary>
        /// Field denoting isExtensionsValid validation state attribute value
        /// </summary>
        private ValidityStateValue _isExtensionsValid = ValidityStateValue.NotChecked;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less Constructor
        /// </summary>
        public ValidationStates()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public ValidationStates(String valuesAsXml)
        {
            LoadValidationStates(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denoting  isSelfValid validation state attribute value 
        /// </summary> 
        [DataMember]
        [ProtoMember(1)]
        public ValidityStateValue IsSelfValid
        {
            get { return this._isSelfValid; }
            set { this._isSelfValid = value; }
        }

        /// <summary>
        /// Property denoting  IsMetaDataValid validation state attribute value 
        /// </summary> 
        [DataMember]
        [ProtoMember(2)]
        public ValidityStateValue IsMetaDataValid
        {
            get { return this._isMetaDataValid; }
            set { this._isMetaDataValid = value; }
        }

        /// <summary>
        /// Property denoting  IsCommonAttributesValid validation state attribute value 
        /// </summary> 
        [DataMember]
        [ProtoMember(3)]
        public ValidityStateValue IsCommonAttributesValid
        {
            get { return this._isCommonAttributesValid; }
            set { this._isCommonAttributesValid = value; }
        }

        /// <summary>
        /// Property denoting  IsCategoryAttributesValid validation state attribute value 
        /// </summary> 
        [DataMember]
        [ProtoMember(4)]
        public ValidityStateValue IsCategoryAttributesValid
        {
            get { return this._isCategoryAttributesValid; }
            set { this._isCategoryAttributesValid = value; }
        }

        /// <summary>
        /// Property denoting  IsRelationshipsValid validation state attribute value 
        /// </summary> 
        [DataMember]
        [ProtoMember(5)]
        public ValidityStateValue IsRelationshipsValid
        {
            get { return this._isRelationshipsValid; }
            set { this._isRelationshipsValid = value; }
        }

        /// <summary>
        /// Property denoting  IsEntityVariantValid validation state attribute value 
        /// </summary> 
        [DataMember]
        [ProtoMember(6)]
        public ValidityStateValue IsEntityVariantValid
        {
            get { return this._isEntityVariantValid; }
            set { this._isEntityVariantValid = value; }
        }

        /// <summary>
        /// Property denoting  IsExtensionsValid validation state attribute value 
        /// </summary> 
        [DataMember]
        [ProtoMember(7)]
        public ValidityStateValue IsExtensionsValid
        {
            get { return this._isExtensionsValid; }
            set { this._isExtensionsValid = value; }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //ValidationStates node start
                    xmlWriter.WriteStartElement("ValidationStates");

                    #region write ValidationStates

                    xmlWriter.WriteAttributeString("IsSelfValid", this.IsSelfValid.ToString());
                    xmlWriter.WriteAttributeString("IsMetaDataValid", this.IsMetaDataValid.ToString());
                    xmlWriter.WriteAttributeString("IsCommonAttributesValid", this.IsCommonAttributesValid.ToString());
                    xmlWriter.WriteAttributeString("IsCategoryAttributesValid", this.IsCategoryAttributesValid.ToString());
                    xmlWriter.WriteAttributeString("IsRelationshipsValid", this.IsRelationshipsValid.ToString());
                    xmlWriter.WriteAttributeString("IsEntityVariantValid", this.IsEntityVariantValid.ToString());
                    xmlWriter.WriteAttributeString("IsExtensionsValid", this.IsExtensionsValid.ToString());

                    #endregion write EntityValidationState

                    //ValidationStates node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Clone validation state object
        /// </summary>
        /// <returns>Returns cloned validation state object</returns>
        public IValidationStates Clone()
        {
            ValidationStates clonedValidationStates = new ValidationStates();

            clonedValidationStates._isSelfValid = this._isSelfValid;
            clonedValidationStates._isMetaDataValid = this._isMetaDataValid;
            clonedValidationStates._isCommonAttributesValid = this._isCommonAttributesValid;
            clonedValidationStates._isCategoryAttributesValid = this._isCategoryAttributesValid;
            clonedValidationStates._isRelationshipsValid = this._isRelationshipsValid;
            clonedValidationStates._isEntityVariantValid = this._isEntityVariantValid;
            clonedValidationStates._isExtensionsValid = this._isExtensionsValid;

            return clonedValidationStates;
        }

        /// <summary>
        /// Get validation state value based on given attribute identifier.
        /// </summary>
        /// <param name="attributeId">Indicates attribute identifier</param>
        /// <returns></returns>
        public ValidityStateValue GetStateValue(Int32 attributeId)
        {
            if(attributeId > 0)
            {
                SystemAttributes systemAttribute = (SystemAttributes)attributeId;
                
                switch(systemAttribute)
                {
                    case SystemAttributes.EntitySelfValid:
                        return this.IsSelfValid;

                    case SystemAttributes.EntityCommonAttributesValid:
                        return this.IsCommonAttributesValid;

                    case SystemAttributes.EntityCategoryAttributesValid:
                        return this.IsCategoryAttributesValid;

                    case SystemAttributes.EntityMetaDataValid:
                        return this.IsMetaDataValid;

                    case SystemAttributes.EntityRelationshipsValid:
                        return this.IsRelationshipsValid;

                    case SystemAttributes.EntityExtensionsValid:
                        return this.IsExtensionsValid;

                    case SystemAttributes.EntityVariantValid:
                        return this.IsEntityVariantValid;
                }
            }

            return ValidityStateValue.Unknown;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadValidationStates(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ValidationStates")
                        {
                            #region Read ValidationStates

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("IsSelfValid"))
                                {
                                    ValidityStateValue valueStateValue = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out valueStateValue);
                                    this._isSelfValid = valueStateValue;
                                }
                                if (reader.MoveToAttribute("IsMetaDataValid"))
                                {
                                    ValidityStateValue valueStateValue = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out valueStateValue);
                                    this._isMetaDataValid = valueStateValue;
                                }
                                if (reader.MoveToAttribute("IsCommonAttributesValid"))
                                {
                                    ValidityStateValue valueStateValue = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out valueStateValue);
                                    this._isCommonAttributesValid = valueStateValue;
                                }
                                if (reader.MoveToAttribute("IsCategoryAttributesValid"))
                                {
                                    ValidityStateValue valueStateValue = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out valueStateValue);
                                    this._isCategoryAttributesValid = valueStateValue;
                                }
                                if (reader.MoveToAttribute("IsRelationshipsValid"))
                                {
                                    ValidityStateValue valueStateValue = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out valueStateValue);
                                    this._isRelationshipsValid = valueStateValue;
                                }
                                if (reader.MoveToAttribute("IsEntityVariantValid"))
                                {
                                    ValidityStateValue valueStateValue = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out valueStateValue);
                                    this._isEntityVariantValid = valueStateValue;
                                }
                                if (reader.MoveToAttribute("IsExtensionsValid"))
                                {
                                    ValidityStateValue valueStateValue = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out valueStateValue);
                                    this._isExtensionsValid = valueStateValue;
                                }
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion Read ValidationStates
                        }
                        else
                        {
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

        #endregion Private Methods

        #endregion Methods
    }
}