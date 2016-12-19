using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDMBO = MDM.BusinessObjects;

    /// <summary>
    /// Specifies the level in an entity variant
    /// </summary>
    [DataContract]
    public class EntityVariantLevel : MDMObject, IEntityVariantLevel
    {
        #region Fields

        /// <summary>
        /// Field denoting the dimension attributes for current level
        /// </summary>
        private Collection<MDMBO.Attribute> _dimensionAttributes = new Collection<MDMBO.Attribute>();

        /// <summary>
        /// Field denoting the rule attributes for current level
        /// </summary>
        private EntityVariantRuleAttributeCollection _ruleAttributes = new EntityVariantRuleAttributeCollection();

        /// <summary>
        /// Field denoting the rank of current entity variant level
        /// </summary>
        private Int32 _rank = -1;

        /// <summary>
        /// Field denoting the entity type id of current level
        /// </summary>
        private Int32 _entityTypeId = -1;

        /// <summary>
        /// Field denoting entity type name of current variant level
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field denoting entity type long name of current variant level
        /// </summary>
        private String _entityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting a table object which hold dimension values
        /// </summary>
        private Table _dimensionValues = new Table();

        /// <summary>
        /// Field denoting identifier of parent level
        /// </summary>
        private Int32 _parentLevelId = -1;

        /// <summary>
        /// Field denoting denormalized list of dimension attribute identifier
        /// </summary>
        private Collection<Int32> _denormalizedDimensionAttributeIdList = new Collection<Int32>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityVariantLevel()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the identifier of entity variant level</param>
        public EntityVariantLevel(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of an entity variant level as input parameters
        /// </summary>
        /// <param name="id">Indicates the identifier of an entity variant level</param>
        /// <param name="name">Indicates the name of an entity variant level</param>
        /// <param name="longName">Indicates the long name of an entity variant level</param>
        public EntityVariantLevel(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for entity variant level. </param>
        public EntityVariantLevel(object[] objectArray)
        {
            int intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);
            this.Id = intId;

            if (objectArray[1] != null)
                this.Name = objectArray[1].ToString();

            if (objectArray[2] != null)
                this.LongName = objectArray[2].ToString();
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="xml">Indicates XML having xml value</param>
        public EntityVariantLevel(String xml)
        {
            LoadEntityVariantLevel(xml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "EntityVariantLevel";
            }
        }

        //TODO: To remove the reference of this dimension attribute property as this is moved inside the Entity Variant rule Attribute Collection
        /// <summary>
        /// Property denoting the dimension attributes of this entity variant level
        /// </summary>
        [DataMember]
        public Collection<MDMBO.Attribute> DimensionAttributes
        {
            get
            {
                return this._dimensionAttributes;
            }
            set
            {
                this._dimensionAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting the rule attributes of this entity variant level
        /// </summary>
        [DataMember]
        public EntityVariantRuleAttributeCollection RuleAttributes
        {
            get
            {
                return this._ruleAttributes;
            }
            set
            {
                this._ruleAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting the dimension values for this level
        /// </summary>
        [DataMember]
        public Table DimensionValues
        {
            get
            {
                return this._dimensionValues;
            }
            set
            {
                this._dimensionValues = value;
            }
        }

        /// <summary>
        /// Property denoting the rank of the entity variant level
        /// </summary>
        [DataMember]
        public Int32 Rank
        {
            get
            {
                return this._rank;
            }
            set
            {
                this._rank = value;
            }
        }

        /// <summary>
        /// Property denoting the entity type id for the entity variant level
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get
            {
                return this._entityTypeId;
            }
            set
            {
                this._entityTypeId = value;
            }
        }

        /// <summary>
        /// Field denoting entity type name of current variant level
        /// </summary>
        [DataMember]
        public String EntityTypeName
        {
            get
            {
                return this._entityTypeName;
            }
            set
            {
                this._entityTypeName = value;
            }
        }

        /// <summary>
        /// Field denoting entity type long name of current variant level
        /// </summary>
        [DataMember]
        public String EntityTypeLongName
        {
            get
            {
                return this._entityTypeLongName;
            }
            set
            {
                this._entityTypeLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the parentlevelid
        /// </summary>
        [DataMember]
        public Int32 ParentLevelId
        {
            get
            {
                return this._parentLevelId;
            }
            set
            {
                this._parentLevelId = value;
            }
        }

        //TODO: To check whether this can be removed.
        /// <summary>
        /// Property denoting the Denormalized Dimension AttributeIdList
        /// </summary>
        [DataMember]
        public Collection<Int32> DenormalizedDimensionAttributeIdList
        {
            get
            {
                return this._denormalizedDimensionAttributeIdList;
            }
            set
            {
                this._denormalizedDimensionAttributeIdList = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of entity variant level object
        /// </summary>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Returns Xml representation of entity variant level object</returns>
        public String ToXml(bool needValues)
        {
            /*
                <EntityVariantLevel Id="1" Name="color" LongName="Color" Rank="1" EntityTypeId="8" Action="Create" ParentLevelId="0">
                    <EntityVariantDimensions>
                        <Attribute Id="3001"></Attribute>
                        <Attribute Id="3002"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
                <EntityVariantLevel Id="2" Name="sku" LongName="SKU" Rank="2" EntityTypeId="9" Action="Create" ParentLevelId="1">
                    <EntityVariantDimensions>
                        <Attribute Id="3003"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
             */

            String xml = string.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("EntityVariantLevel");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Rank", this.Rank.ToString());
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
            xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ParentLevelId", this._parentLevelId.ToString());

            xmlWriter.WriteStartElement("RuleAttributes");

            foreach (EntityVariantRuleAttribute ruleAttribute in this.RuleAttributes)
            {
                xmlWriter.WriteRaw(ruleAttribute.ToXml(needValues));
            }

            xmlWriter.WriteEndElement(); //For Rule Attributes

            xmlWriter.WriteStartElement("DimensionAttributes"); // xml += "<DimensionAttributes>";

            foreach (MDMBO.Attribute attribute in this.DimensionAttributes)
            {
                xmlWriter.WriteStartElement("Attribute");
                xmlWriter.WriteAttributeString("Id", attribute.Id.ToString());
                xmlWriter.WriteAttributeString("IsLookup", attribute.IsLookup.ToString());
                xmlWriter.WriteEndElement(); //Attribute
            }

            xmlWriter.WriteEndElement(); //DimensionAttributes

            xmlWriter.WriteStartElement("DimensionValues"); // xml += "<DimensionAttributes>";

            xmlWriter.WriteRaw(this.DimensionValues.ToXml());

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement(); // entity variant level
            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Prepares the object from xml string
        /// </summary>
        /// <param name="valuesAsXml">Indicates string representation of object</param>
        private void LoadEntityVariantLevel(String valuesAsXml)
        {
            #region Sample Xml

            /*
              <EntityVariantLevel Id="1" Name="color" LongName="Color" Rank="1" EntityTypeId="8" Action="Create" ParentLevelId="0">
                    <EntityVariantDimensions>
                        <Attribute Id="3001"></Attribute>
                        <Attribute Id="3002"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
                <EntityVariantLevel Id="2" Name="sku" LongName="SKU" Rank="2" EntityTypeId="9" Action="Create" ParentLevelId="1">
                    <EntityVariantDimensions>
                        <Attribute Id="3003"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
             */

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityVariantLevel" && reader.HasAttributes)
                    {
                        #region Read Entity Variant Level Properties


                        if (reader.MoveToAttribute("Id"))
                        {
                            this.Id = reader.ReadContentAsInt();
                        }

                        if (reader.MoveToAttribute("Name"))
                        {
                            this.Name = reader.ReadContentAsString();
                        }

                        if (reader.MoveToAttribute("LongName"))
                        {
                            this.LongName = reader.ReadContentAsString();
                        }

                        if (reader.MoveToAttribute("Action"))
                        {
                            Int32 action = 1;
                            try
                            {
                                action = reader.ReadContentAsInt();
                            }
                            catch { }
                            if (Enum.IsDefined(typeof(ObjectAction), action))
                            {
                                this.Action = (ObjectAction)action;
                            }
                        }

                        if (reader.MoveToAttribute("Rank"))
                        {
                            this.Rank = reader.ReadContentAsInt();
                        }

                        if (reader.MoveToAttribute("EntityTypeId"))
                        {
                            this.EntityTypeId = reader.ReadContentAsInt();
                        }

                        if (reader.MoveToAttribute("EntityTypeName"))
                        {
                            this.EntityTypeName = reader.ReadContentAsString();
                        }
                        if (reader.MoveToAttribute("EntityTypeLongName"))
                        {
                            this.EntityTypeLongName = reader.ReadContentAsString();
                        }

                        if (reader.MoveToAttribute("ParentLevelId"))
                        {
                            //it may be blank
                            Int32.TryParse(reader.ReadContentAsString(), out this._parentLevelId);
                        }


                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DimensionAttributes")
                    {
                        #region Read DimensionAttributes

                        this.DimensionAttributes = this.ParseAttributes(reader.ReadInnerXml());

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RuleAttributes")
                    {
                        #region Read RuleAttributes

                        this.RuleAttributes = this.ParseRuleAttributes(reader.ReadInnerXml());

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DimensionValues")
                    {
                        #region Read RuleAttributes

                        this.DimensionValues = new Table(reader.ReadInnerXml());

                        #endregion
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

        /// <summary>
        /// Parse the Attribute from xml
        /// </summary>
        /// <param name="attributesXml">Indicates the attributes object in xml format</param>
        /// <returns>Returns the parsed collection of attribute object from xml</returns>
        private Collection<MDMBO.Attribute> ParseAttributes(String attributesXml)
        {
            Collection<MDMBO.Attribute> attributes = new Collection<MDMBO.Attribute>();
            XmlTextReader reader = new XmlTextReader(attributesXml, XmlNodeType.Element, null);

            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                {
                    String attributeXml = reader.ReadOuterXml();
                    MDMBO.Attribute attribute = new MDMBO.Attribute(attributeXml);
                    attributes.Add(attribute);
                }
                else
                {
                    reader.Read();
                }
            }

            return attributes;
        }

        /// <summary>
        /// Parse the rule attributes from xml
        /// </summary>
        /// <param name="attributesXml">Indicate the attribute collection in xml format</param>
        /// <returns>Returns the parsed collection of rule attributes from xml</returns>
        private EntityVariantRuleAttributeCollection ParseRuleAttributes(String attributesXml)
        {
            #region Sample Xml
            /*
             * <RuleAttributes>
                <Attribute Id="5076" TargetAttrId="5079" IsOptional="false" />
                <Attribute Id="5078" TargetAttrId="5080" IsOptional="true" />
              </RuleAttributes>
             * 
             */
            #endregion Sample Xml

            EntityVariantRuleAttributeCollection ruleAttributes = new EntityVariantRuleAttributeCollection();
            XmlTextReader reader = new XmlTextReader(attributesXml, XmlNodeType.Element, null);

            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute" && reader.HasAttributes)
                {
                    MDMBO.Attribute attribute = new MDMBO.Attribute();
                    Int32 targetAttrId = 0;
                    Boolean isOptional = false;
                    Boolean isLookup = false;
                    EntityVariantRuleAttribute ruleAttr = new EntityVariantRuleAttribute();

                    if (reader.MoveToAttribute("Id"))
                    {
                        attribute.Id = reader.ReadContentAsInt();
                    }

                    if (reader.MoveToAttribute("TargetAttrId"))
                    {
                        targetAttrId = reader.ReadContentAsInt();
                    }

                    if (reader.MoveToAttribute("IsOptional"))
                    {
                        String strOptional = "false";
                        strOptional = reader.ReadContentAsString();
                        Boolean.TryParse(strOptional, out isOptional);
                    }

                    if (reader.MoveToAttribute("IsLookup"))
                    {
                        isLookup = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), isLookup);
                    }

                    ruleAttr.RuleAttribute = attribute;
                    ruleAttr.TargetAttributeId = targetAttrId;
                    ruleAttr.IsOptional = isOptional;
                    ruleAttr.RuleAttribute.IsLookup = isLookup;

                    ruleAttributes.Add(ruleAttr);
                }
                else
                {
                    reader.Read();
                }
            }

            return ruleAttributes;
        }

        /// <summary>
        /// Append dimension Attributes
        /// </summary>
        /// <param name="iAttribute">Indicate the attribute interface object</param>
        public void AppendDimensionAttribute(IAttribute iAttribute)
        {
            if (iAttribute == null)
            {
                throw new ArgumentNullException("iAttribute");
            }
            this.DimensionAttributes.Add((Attribute)iAttribute);
        }

        /// <summary>
        /// Append Dimension Values
        /// </summary>
        /// <param name="iTable">Indicates the table interface object</param>
        public void SetDimensionValues(ITable iTable)
        {
            if (iTable == null)
            {
                throw new ArgumentNullException("iTable");
            }
            this.DimensionValues = (Table)iTable;
        }

        /// <summary>
        /// Append Rule Attribute
        /// </summary>
        /// <param name="iAttribute">Indicates Rule Attribute</param>
        /// <param name="targetAttrId">Indicates Target attribute Id</param>
        /// <param name="isOptional">Indicates Append rule attribute is optional or not</param>
        public void AppendRuleAttribute(IAttribute iAttribute, Int32 targetAttrId, Boolean isOptional)
        {
            if (iAttribute == null)
            {
                throw new ArgumentNullException("iAttribute");
            }

            EntityVariantRuleAttribute ruleAttribute = new EntityVariantRuleAttribute();
            ruleAttribute.RuleAttribute = (Attribute)iAttribute;
            ruleAttribute.TargetAttributeId = targetAttrId;
            ruleAttribute.IsOptional = isOptional;
            this.RuleAttributes.Add(ruleAttribute);
        }

        /// <summary>
        /// Get rule attribute by target attribute Id
        /// </summary>
        /// <param name="targetAttributeId">Indicates identifier of target attribute</param>
        /// <returns>Returns entity variant rule attribute based on target attribute Id</returns>
        public EntityVariantRuleAttribute GetRuleAttributeByTargetAttributeId(Int32 targetAttributeId)
        {
            EntityVariantRuleAttribute entityVariantRuleAttribute = null;

            if (this.RuleAttributes != null)
            {
                entityVariantRuleAttribute = this.RuleAttributes.Where(ruleAttribute => ruleAttribute.TargetAttributeId == targetAttributeId).FirstOrDefault();
            }

            return entityVariantRuleAttribute;
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="subSetEntityVariantLevel">Indicates the object to compare with the current object.</param>
        /// <param name="compareIds">Indicates the flag to determine whether id based comparison is true or not</param>
        /// <returns>Returns true if the specified object is equal to the current object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityVariantLevel subSetEntityVariantLevel, Boolean compareIds = false)
        {
            if (subSetEntityVariantLevel != null)
            {
                if (base.IsSuperSetOf(subSetEntityVariantLevel, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.Rank != subSetEntityVariantLevel.Rank)
                            return false;

                        if (this.EntityTypeId != subSetEntityVariantLevel.EntityTypeId)
                            return false;

                        if (this.ParentLevelId != subSetEntityVariantLevel.ParentLevelId)
                            return false;
                    }

                    if (!this.RuleAttributes.IsSuperSetOf(subSetEntityVariantLevel.RuleAttributes))
                        return false;

                    if (this.EntityTypeName != subSetEntityVariantLevel.EntityTypeName)
                        return false;

                    if (this.EntityTypeLongName != subSetEntityVariantLevel.EntityTypeLongName)
                        return false;

                    if (!this.DimensionValues.IsSuperSetOf(subSetEntityVariantLevel.DimensionValues))
                        return false;

                    if (subSetEntityVariantLevel.DimensionAttributes != null && subSetEntityVariantLevel.DimensionAttributes.Count > 0)
                    {
                        foreach (Attribute childDimensionAttribute in subSetEntityVariantLevel.DimensionAttributes)
                        {
                            if (this._dimensionAttributes != null && this._dimensionAttributes.Count > 0)
                            {
                                foreach (Attribute sourceDimensionAttribute in this._dimensionAttributes)
                                {
                                    if (compareIds)
                                    {
                                        if (sourceDimensionAttribute.Id != childDimensionAttribute.Id)
                                            return false;
                                    }

                                    if (sourceDimensionAttribute.LongName != childDimensionAttribute.LongName)
                                        return false;

                                    if (sourceDimensionAttribute.IsLookup != childDimensionAttribute.IsLookup)
                                        return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clone Entity Variant Level object
        /// </summary>
        /// <returns>Returns cloned Entity Variant Level instance</returns>
        public IEntityVariantLevel Clone()
        {
            EntityVariantLevel clonedEntityVariantLevel = new EntityVariantLevel();

            clonedEntityVariantLevel.Id = this.Id;
            clonedEntityVariantLevel.EntityTypeId = this.EntityTypeId;
            clonedEntityVariantLevel.EntityTypeName = this.EntityTypeName;
            clonedEntityVariantLevel.EntityTypeLongName = this.EntityTypeLongName;
            clonedEntityVariantLevel.Rank = this.Rank;
            clonedEntityVariantLevel.ParentLevelId = this.ParentLevelId;

            if (clonedEntityVariantLevel.RuleAttributes != null)
            {
                clonedEntityVariantLevel.RuleAttributes = this.RuleAttributes.Clone() as EntityVariantRuleAttributeCollection;
            }

            return clonedEntityVariantLevel;
        }

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">Indicates the entity variant level object which needs to be compared.</param>
        /// <returns>Returns the result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityVariantLevel)
            {
                EntityVariantLevel objectToBeCompared = obj as EntityVariantLevel;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.RuleAttributes.Count != objectToBeCompared.RuleAttributes.Count)
                    return false;

                // Compare rules
                var matchedRuleAttributes = from p in this.RuleAttributes
                                            join q in objectToBeCompared.RuleAttributes
                                            on p.GetHashCode() equals q.GetHashCode()
                                            select p;

                if (matchedRuleAttributes.Count() != this.RuleAttributes.Count)
                    return false;

                if (this.DimensionAttributes.Count != objectToBeCompared.DimensionAttributes.Count)
                    return false;

                // Compare dimensions
                var matchedDimensionAttributes = from p in this.DimensionAttributes
                                                 join q in objectToBeCompared.DimensionAttributes
                                                 on p.GetHashCode() equals q.GetHashCode()
                                                 select p;

                if (matchedDimensionAttributes.Count() != this.DimensionAttributes.Count)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.DimensionAttributes.GetHashCode()  ^ this.EntityTypeId.GetHashCode() ^
                this.EntityTypeName.GetHashCode() ^ this.EntityTypeLongName.GetHashCode() ^ this.Rank.GetHashCode() ^ this.RuleAttributes.GetHashCode();
        }

        #endregion

        #endregion Methods
    }
}
