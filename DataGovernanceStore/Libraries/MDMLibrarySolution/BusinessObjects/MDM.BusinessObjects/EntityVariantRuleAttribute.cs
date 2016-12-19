using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies a rule attribute for entity variant definition.
    /// </summary>
    [DataContract]
    public class EntityVariantRuleAttribute : MDMObject, IEntityVariantRuleAttribute
    {
        #region Fields

        /// <summary>
        /// Field denoting Rule attribute
        /// </summary>
        private Attribute _ruleAttribute = new Attribute();

        //TODO: remove this and its references
        /// <summary>
        /// Field denoting target attribute Id
        /// </summary>
        private Int32 _targetAttribute = -1;

        /// <summary>
        /// Field denoting source attribute identifier
        /// </summary>
        private Int32 _sourceAttributeId = -1;

        /// <summary>
        /// Field denoting source attribute name
        /// </summary>
        private String _sourceAttributeName = String.Empty;

        /// <summary>
        /// Field denoting source attribute long name
        /// </summary>
        private String _sourceAttributeLongName = String.Empty;

        /// <summary>
        /// Field denoting target attribute identifier
        /// </summary>
        private Int32 _targetAttributeId = -1;

        /// <summary>
        /// Field denoting target attribute name
        /// </summary>
        private String _targetAttributeName = String.Empty;

        /// <summary>
        /// Field denoting target attribute long name
        /// </summary>
        private String _targetAttributeLongName = String.Empty;

        /// <summary>
        /// Field denoting if rule attribute is optional
        /// </summary>
        private Boolean _isOptional = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting Rule attribute
        /// </summary>
        [DataMember]
        public Attribute RuleAttribute
        {
            get { return _ruleAttribute; }
            set { _ruleAttribute = value; }
        }

        /// <summary>
        /// Property denoting target attribute Id
        /// </summary>
        [DataMember]
        public Int32 TargetAttribute
        {
            get { return _targetAttribute; }
            set { _targetAttribute = value; }
        }

        /// <summary>
        /// Property denoting source attribute identifier
        /// </summary>
        [DataMember]
        public Int32 SourceAttributeId
        {
            get { return _sourceAttributeId; }
            set { _sourceAttributeId = value; }
        }

        /// <summary>
        /// Property denoting source attribute name
        /// </summary>
        [DataMember]
        public String SourceAttributeName
        {
            get { return _sourceAttributeName; }
            set { _sourceAttributeName = value; }
        }

        /// <summary>
        /// Property denoting source attribute long name
        /// </summary>
        [DataMember]
        public String SourceAttributeLongName
        {
            get { return _sourceAttributeLongName; }
            set { _sourceAttributeLongName = value; }
        }

        /// <summary>
        /// Property denoting target attribute identifier
        /// </summary>
        [DataMember]
        public Int32 TargetAttributeId
        {
            get { return _targetAttributeId; }
            set { _targetAttributeId = value; }
        }

        /// <summary>
        /// Property denoting target attribute name
        /// </summary>
        [DataMember]
        public String TargetAttributeName
        {
            get { return _targetAttributeName; }
            set { _targetAttributeName = value; }
        }

        /// <summary>
        /// Property denoting target attribute long name
        /// </summary>
        [DataMember]
        public String TargetAttributeLongName
        {
            get { return _targetAttributeLongName; }
            set { _targetAttributeLongName = value; }
        }

        /// <summary>
        /// Property denoting if rule attribute is optional
        /// </summary>
        [DataMember]
        public Boolean IsOptional
        {
            get { return _isOptional; }
            set { _isOptional = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityVariantRuleAttribute() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleAttribute">Indicates rule attributes.</param>
        /// <param name="sourceAttributeId">Indicates identifier of source attribute.</param>
        /// <param name="sourceAttributeName">Indicates name of source attribute.</param>
        /// <param name="sourceAttributeLongname">Indicates long name of source attribute.</param>
        /// <param name="targetAttributeId">Indicates identifier of target attribute.</param>
        /// <param name="targetAttributeName">Indicates name of target attribute.</param>
        /// <param name="targetAttributelongName">Indicates long name of target attribute.</param>
        /// <param name="isOptional">Indicates if it is optional.</param>
        public EntityVariantRuleAttribute(Attribute ruleAttribute, Int32 sourceAttributeId, String sourceAttributeName, String sourceAttributeLongname, 
            Int32 targetAttributeId, String targetAttributeName, String targetAttributelongName, Boolean isOptional)
        {
            this._ruleAttribute = ruleAttribute;
            this._sourceAttributeId = sourceAttributeId;
            this._sourceAttributeName = sourceAttributeName;
            this._sourceAttributeLongName = sourceAttributeLongname;
            this._targetAttributeId = targetAttributeId;
            this._targetAttributeName = targetAttributeName;
            this._targetAttributeLongName = targetAttributelongName;
            this._isOptional = isOptional;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get XML representation of entity variant rule attribute object 
        /// </summary>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Returns XML representation of entity variant rule attribute object</returns>
        public String ToXml(bool needValues)
        {
            String xml = String.Empty;
            Attribute attribute = this.RuleAttribute;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region Write Attribute Metadata

            if (needValues)
            {
                String attributeXml = attribute.ToXml(ObjectSerialization.UIRender);
                attributeXml = attributeXml.Replace("<Attribute Id=\"" + attribute.Id + "\"", "<Attribute Id=\"" + attribute.Id + "\" TargetAttrId=\"" + this.TargetAttribute + "\" IsOptional=\"" + this.IsOptional.ToString() + "\"");
                xmlWriter.WriteRaw(attributeXml);
            }
            else
            {
                xmlWriter.WriteStartElement("Attribute");
                xmlWriter.WriteAttributeString("Id", attribute.Id.ToString());

                //TODO: Need to remove this
                xmlWriter.WriteAttributeString("TargetAttrId",this.TargetAttribute.ToString());

                xmlWriter.WriteAttributeString("SourceAttributeId", this.SourceAttributeId.ToString());
                xmlWriter.WriteAttributeString("SourceAttributeName", this.SourceAttributeName);
                xmlWriter.WriteAttributeString("SourceAttributeLongName", this.SourceAttributeLongName);
                xmlWriter.WriteAttributeString("TargetAttributeId", this.TargetAttributeId.ToString());
                xmlWriter.WriteAttributeString("TargetAttributeName", this.TargetAttributeName);
                xmlWriter.WriteAttributeString("TargetAttributeLongName", this.TargetAttributeLongName);
                xmlWriter.WriteAttributeString("IsOptional", this.IsOptional.ToString());
                xmlWriter.WriteAttributeString("IsLookup", attribute.IsLookup.ToString());
                xmlWriter.WriteEndElement();
            }
            
            #endregion Write Attribute Metadata

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();
            return xml;
        }

        /// <summary>
        /// Get XML representation of entity variant rule attribute object
        /// </summary>
        /// <param name="objectSerialization">Indicates object serialization</param>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Returns XML representation of entity variant rule attribute object</returns>
        public String ToXml(ObjectSerialization objectSerialization, bool needValues)
        {
            return this.ToXml(needValues);   
        }

        /// <summary>
        /// Get rule attribute
        /// </summary>
        /// <returns>Returns rule attribute interface</returns>
        public IAttribute GetRuleAttribute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set rule attribute
        /// </summary>
        /// <param name="ruleAttribute">Indicates rule attribute interface to set</param>
        public void SetRuleAttribute(IAttribute ruleAttribute)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="subSetEntityVariantRuleAttribute">Indicates the object to compare with the current object.</param>
        /// <param name="compareIds">Indicates the flag to determine whether id based comparison is true or not</param>
        /// <returns>Returns true if the specified object is equal to the current object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityVariantRuleAttribute subSetEntityVariantRuleAttribute, Boolean compareIds = false)
        {
            if (subSetEntityVariantRuleAttribute != null)
            {
                if (base.IsSuperSetOf(subSetEntityVariantRuleAttribute, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.TargetAttribute != subSetEntityVariantRuleAttribute.TargetAttribute)
                            return false;
                        if (this.SourceAttributeId != subSetEntityVariantRuleAttribute.SourceAttributeId)
                            return false;
                        if (this.TargetAttributeId != subSetEntityVariantRuleAttribute.TargetAttributeId)
                            return false;
                    }

                    if (this.SourceAttributeName != subSetEntityVariantRuleAttribute.SourceAttributeName)
                        return false;

                    if (this.SourceAttributeLongName != subSetEntityVariantRuleAttribute.SourceAttributeLongName)
                        return false;

                    if (this.TargetAttributeName != subSetEntityVariantRuleAttribute.TargetAttributeName)
                        return false;

                    if (this.TargetAttributeLongName != subSetEntityVariantRuleAttribute.TargetAttributeLongName)
                        return false;

                    if (this.Action != subSetEntityVariantRuleAttribute.Action)
                        return false;

                    if (this.IsOptional != subSetEntityVariantRuleAttribute.IsOptional)
                        return false;

                    if (!this.RuleAttribute.IsSuperSetOf(subSetEntityVariantRuleAttribute.RuleAttribute))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clone entity variant rule attribute object
        /// </summary>
        /// <returns>Returns cloned entity variant rule attribute instance</returns>
        public IEntityVariantRuleAttribute Clone()
        {
            EntityVariantRuleAttribute clonedEntityVariantRuleAttribute = new EntityVariantRuleAttribute();

            clonedEntityVariantRuleAttribute.SourceAttributeId = this.SourceAttributeId;
            clonedEntityVariantRuleAttribute.SourceAttributeName = this.SourceAttributeName;
            clonedEntityVariantRuleAttribute.SourceAttributeLongName = this.SourceAttributeLongName;
            clonedEntityVariantRuleAttribute.TargetAttributeId = this.TargetAttributeId;
            clonedEntityVariantRuleAttribute.TargetAttributeName = this.TargetAttributeName;
            clonedEntityVariantRuleAttribute.TargetAttributeLongName = this.TargetAttributeLongName;
            clonedEntityVariantRuleAttribute.IsOptional = this.IsOptional;

            return clonedEntityVariantRuleAttribute;
        }

        #endregion Methods
    }
}
