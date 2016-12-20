using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the definition of an entity variant.
    /// </summary>
    [DataContract]
    public class EntityVariantDefinition : MDMObject, IEntityVariantDefinition, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting root entity type identifier of entity variant
        /// </summary>
        private Int32 _rootEntityTypeId = -1;

        /// <summary>
        /// Field denoting root entity type name of entity variant
        /// </summary>
        private String _rootEntityTypeName = String.Empty;

        /// <summary>
        /// Field denoting root entity type name of entity variant
        /// </summary>
        private String _rootEntityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting the levels of entity variant
        /// </summary>
        private EntityVariantLevelCollection _entityVariantLevels = new EntityVariantLevelCollection();

        /// <summary>
        /// Field denoting the identifier of context for entity variant definition
        /// </summary>
        private Int32 _contextId = -1;

        /// <summary>
        /// Field denoting entity variant definition key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Indicates whether dimension attributes are specified or not
        /// </summary>
        private Boolean _hasDimensionAttributes = true;

        /// <summary>
        /// Field denoting the original entity variant definition
        /// </summary>
        private EntityVariantDefinition _originalEntityVariantDefinition = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityVariantDefinition()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the entity variant definition identifier</param>
        public EntityVariantDefinition(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id and Name of an entity variant definition as input parameters
        /// </summary>
        /// <param name="id">Indicates the entity variant definition identifier</param>
        /// <param name="name">Indicates the name of entity variant definition</param>
        public EntityVariantDefinition(Int32 id, String name)
            : base(id, name)
        {
        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for Entity Variant Definition.</param>
        public EntityVariantDefinition(object[] objectArray)
        {
            int intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);
            this.Id = intId;

            if (objectArray[1] != null)
                this.Name = objectArray[1].ToString();
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="xml">Indicates XML having xml value</param>
        public EntityVariantDefinition(String xml)
        {
            LoadEntityVariantDefinition(xml);
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
                return "EntityVariantDefinition";
            }
        }

        /// <summary>
        /// Property denoting root entity type identifier
        /// </summary>
        [DataMember]
        public Int32 RootEntityTypeId
        {
            get
            {
                return this._rootEntityTypeId;
            }
            set
            {
                this._rootEntityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting root entity type name
        /// </summary>
        [DataMember]
        public String RootEntityTypeName
        {
            get
            {
                return this._rootEntityTypeName;
            }
            set
            {
                this._rootEntityTypeName = value;
            }
        }

        /// <summary>
        /// Property denoting root entity type long name
        /// </summary>
        [DataMember]
        public String RootEntityTypeLongName
        {
            get
            {
                return this._rootEntityTypeLongName;
            }
            set
            {
                this._rootEntityTypeLongName = value;
            }
        }

        /// <summary>
        /// Property denoting levels of the entity variant
        /// </summary>
        [DataMember]
        public EntityVariantLevelCollection EntityVariantLevels
        {
            get
            {
                return this._entityVariantLevels;
            }
            set
            {
                this._entityVariantLevels = value;
            }
        }

        /// <summary>
        /// Property denoting context identifier for entity variant definition
        /// </summary>
        [DataMember]
        public Int32 ContextId
        {
            get
            {
                return this._contextId;
            }
            set
            {
                this._contextId = value;
            }
        }

        /// <summary>
        /// Specifies whether dimension attributes are specified or not
        /// </summary>
        [DataMember]
        public Boolean HasDimensionAttributes
        {
            get
            {
                return this._hasDimensionAttributes;
            }
            set
            {
                this._hasDimensionAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting the original entity variant definition
        /// </summary>
        public EntityVariantDefinition OriginalEntityVariantDefinition
        {
            get
            {
                return _originalEntityVariantDefinition;
            }
            set
            {
                this._originalEntityVariantDefinition = value;
            }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting the ExternalId property for this data model object
        /// </summary>
        public string ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.EntityVariantDefinition;
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Clone Entity Variant Definition object 
        /// </summary>
        /// <returns>Returns cloned copy of Entity variant Definition instance.</returns>
        public IEntityVariantDefinition Clone()
        {
            EntityVariantDefinition clonedEntityVariantDefinition = new EntityVariantDefinition();

            clonedEntityVariantDefinition.Id = this.Id;
            clonedEntityVariantDefinition.Name = this.Name;
            clonedEntityVariantDefinition.LongName = this.LongName;
            clonedEntityVariantDefinition.RootEntityTypeId = this.RootEntityTypeId;
            clonedEntityVariantDefinition.RootEntityTypeName = this.RootEntityTypeName;
            clonedEntityVariantDefinition.RootEntityTypeLongName = this.RootEntityTypeLongName;
            clonedEntityVariantDefinition.HasDimensionAttributes = this.HasDimensionAttributes;

            if (this.EntityVariantLevels != null)
            {
                clonedEntityVariantDefinition.EntityVariantLevels = this.EntityVariantLevels.Clone() as EntityVariantLevelCollection;
            }

            if (this.OriginalEntityVariantDefinition != null)
            {
                clonedEntityVariantDefinition.OriginalEntityVariantDefinition = this.OriginalEntityVariantDefinition.Clone() as EntityVariantDefinition;
            }

            return clonedEntityVariantDefinition;
        }

        /// <summary>
        /// Get Xml representation of entity variant definition object
        /// </summary>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Returns Xml representation of entity variant definition object</returns>
        public String ToXml(Boolean needValues)
        {
            /*
            <EntityVariantDefinition Id="1" Name="Style-SKU Variant" RootEntityTypeId="16" RootEntityTypeName="style" RootEntityTypeLongName="Style" ContextId="1" Action="Create">
                <EntityVariantLevel Rank="1" EntityTypeId="8">
                    <EntityVariantDimensions>
                        <Attribute Id="3001"></Attribute>
                        <Attribute Id="3002"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
                <EntityVariantLevel Rank="2" EntityTypeId="9">
                    <EntityVariantDimensions>
                        <Attribute Id="3003"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
                <EntityVariantLevel Rank="3" EntityTypeId="10">
                    <EntityVariantDimensions>
                        <Attribute Id="3004"></Attribute>
                        <Attribute Id="3005"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
            </EntityVariantDefinition>
             * */
            String xml = string.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region For Entity Variant Definition Metadata

            xmlWriter.WriteStartElement("EntityVariantDefinition");
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("RootEntityTypeId", this.RootEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("RootEntityTypeName", this.RootEntityTypeName);
            xmlWriter.WriteAttributeString("RootEntityTypeLongName", this.RootEntityTypeLongName);
            xmlWriter.WriteAttributeString("ContextId", this.ContextId.ToString());
            xmlWriter.WriteAttributeString("HasDimensionAttributes", this.HasDimensionAttributes.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            foreach (EntityVariantLevel level in this.EntityVariantLevels)
            {
                //Entity Variant Level XML
                xmlWriter.WriteRaw(level.ToXml(needValues));
            }

            xmlWriter.WriteEndElement(); //For Entity Variant Definition

            #endregion For Entity Variant Definition Metadata

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();
            return xml;
        }

        /// <summary>
        /// Append the entity variant levels to entity variant definition.
        /// </summary>
        /// <param name="iEntityVariantLevel">Indicates the entity variant level to append.</param>
        public void AppendEntityVariantLevel(IEntityVariantLevel iEntityVariantLevel)
        {
            if (iEntityVariantLevel == null)
            {
                throw new ArgumentNullException("iEntityVariantLevel");
            }

            this.EntityVariantLevels.Add((EntityVariantLevel)iEntityVariantLevel);
        }

        /// <summary>
        /// Get entity Variant level based on entity variant level attribute name
        /// </summary>
        /// <param name="entityVariantLevelAttributeName">Indicates name of the entity variant level attribute name</param>
        /// <returns>Returns entity variant level based on entity variant level attribute name</returns>
        public EntityVariantLevel GetEntityVariantLevel(String entityVariantLevelAttributeName)
        {
            if (String.IsNullOrWhiteSpace(entityVariantLevelAttributeName))
            {
                return null;
            }

            foreach (EntityVariantLevel entityVariantLevel in this._entityVariantLevels)
            {
                if (entityVariantLevel.Name == entityVariantLevelAttributeName)
                {
                    return entityVariantLevel;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="subSetEntityVariantDefinition">Indicates the object to compare with the current object.</param>
        /// <param name="compareIds">Indicates the flag to determine whether id based comparison to be done or not</param>
        /// <returns>Returns true if the specified object is equal to the current object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityVariantDefinition subSetEntityVariantDefinition, Boolean compareIds = false)
        {
            if (subSetEntityVariantDefinition != null)
            {
                if (base.IsSuperSetOf(subSetEntityVariantDefinition, compareIds))
                {
                    if (this.RootEntityTypeId != subSetEntityVariantDefinition.RootEntityTypeId)
                    {
                        return false;
                    }

                    if (compareIds)
                    {
                        if (this.ContextId != subSetEntityVariantDefinition.ContextId)
                            return false;
                    }

                    if (subSetEntityVariantDefinition._entityVariantLevels != null && subSetEntityVariantDefinition._entityVariantLevels.Count > 0)
                    {
                        foreach (EntityVariantLevel entityVariantLevel in subSetEntityVariantDefinition._entityVariantLevels)
                        {
                            EntityVariantLevel sourceEntityVariantLevel = GetEntityVariantLevel(entityVariantLevel.Name);
                            if (sourceEntityVariantLevel != null)
                            {
                                if (!sourceEntityVariantLevel.IsSuperSetOf(entityVariantLevel, compareIds))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
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
        /// Delta Merge of EntityVariantDefinition
        /// </summary>
        /// <param name="deltaEntityVariantDefinition">Indicates EntityVariantDefinition that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action.</param>
        /// <param name="returnClonedObject">Indicates whether clone merge object or not.</param>
        /// <returns>Returns merged EntityVariantDefinition instance.</returns>
        public EntityVariantDefinition MergeDelta(EntityVariantDefinition deltaEntityVariantDefinition, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            EntityVariantDefinition mergedEntityVariantDefinition = (returnClonedObject == true) ? deltaEntityVariantDefinition.Clone() as EntityVariantDefinition : deltaEntityVariantDefinition;

            mergedEntityVariantDefinition.Action = (mergedEntityVariantDefinition.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedEntityVariantDefinition;
        }

        /// <summary>
        /// Prepares the object from xml string
        /// </summary>
        /// <param name="valuesAsXml">Indicates the string representation of object</param>
        private void LoadEntityVariantDefinition(String valuesAsXml)
        {
            #region Sample Xml

            /*
            <EntityVariantDefinition Id="1" Name="Style-SKU Variant" RootEntityTypeId="16" RootEntityTypeName="style" RootEntityTypeLongName="Style" ContextId="1" Action="Create">
                <EntityVariantLevel Rank="1" EntityTypeId="8">
                    <EntityVariantDimensions>
                        <Attribute Id="3001"></Attribute>
                        <Attribute Id="3002"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
                <EntityVariantLevel Rank="2" EntityTypeId="9">
                    <EntityVariantDimensions>
                        <Attribute Id="3003"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
                <EntityVariantLevel Rank="3" EntityTypeId="10">
                    <EntityVariantDimensions>
                        <Attribute Id="3004"></Attribute>
                        <Attribute Id="3005"></Attribute>
                    </EntityVariantDimensions>
                </EntityVariantLevel>
            </EntityVariantDefinition>
             * */

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityVariantDefinition")
                    {
                        #region Read Entity Variant Level Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                this.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                Int32 action = 1;

                                try
                                {
                                    action = reader.ReadContentAsInt();
                                }
                                catch
                                {
                                }

                                if (Enum.IsDefined(typeof(ObjectAction), action))
                                {
                                    this.Action = (ObjectAction)action;
                                }
                            }

                            if (reader.MoveToAttribute("RootEntityTypeId"))
                            {
                                this.RootEntityTypeId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("RootEntityTypeName"))
                            {
                                this.RootEntityTypeName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RootEntityTypeLongName"))
                            {
                                this.RootEntityTypeLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ContextId"))
                            {
                                this.ContextId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("HasDimensionAttributes"))
                            {
                                this.HasDimensionAttributes = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._hasDimensionAttributes);
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityVariantLevel")
                    {
                        #region Read Entity Variant Dimensions

                        String levelXml = reader.ReadOuterXml();
                        EntityVariantLevel level = new EntityVariantLevel(levelXml);
                        this.EntityVariantLevels.Add(level);

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

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">Indicates the entity variant definition object which needs to be compared.</param>
        /// <returns>Returns the result of the comparison as a boolean value.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is EntityVariantDefinition)
            {
                EntityVariantDefinition objectToBeCompared = obj as EntityVariantDefinition;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.EntityVariantLevels.Count != objectToBeCompared.EntityVariantLevels.Count)
                    return false;

                // Compare levels
                var levels = from p in this.EntityVariantLevels
                             join q in objectToBeCompared.EntityVariantLevels
                             on p.GetHashCode() equals q.GetHashCode()
                             select p;

                if (levels.Count() != this.EntityVariantLevels.Count)
                    return false;

                if (this.HasDimensionAttributes != objectToBeCompared.HasDimensionAttributes)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode() ^ this.RootEntityTypeId.GetHashCode() ^ this.RootEntityTypeName.GetHashCode() ^
                this.RootEntityTypeLongName.GetHashCode() ^ this.ContextId.GetHashCode() ^ this.EntityVariantLevels.GetHashCode() ^
                this.HasDimensionAttributes.GetHashCode();
        }

        #endregion

        #region IDataModelObject

        /// <summary>
        /// Get data model object for the current object.
        /// </summary>
        /// <returns>Returns data model object for the current object</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #endregion Methods
    }
}
