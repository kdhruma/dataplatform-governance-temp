using System;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies mapping object for EntityTypeAttributeMapping
    /// </summary>
    [DataContract]
    public class EntityTypeAttributeMapping : MDMObject, IEntityTypeAttributeMapping, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the entity type id
        /// </summary>
        private Int32 _entityTypeId = -1;

        /// <summary>
        /// Field denoting the attribute id
        /// </summary>
        private Int32 _attributeId = -1;

        /// <summary>
        /// Field denoting the attribute parent id
        /// </summary>
        private Int32 _attributeParentId = -1;

        /// <summary>
        /// Field denoting the short name of entity type 
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field denoting the long name of entity type 
        /// </summary>
        private String _entityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting the short name of attribute
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field denoting the long name of attribute
        /// </summary>
        private String _attributeLongName = String.Empty;

        /// <summary>
        /// Field denoting the short name of attribute parent
        /// </summary>
        private String _attributeParentName = String.Empty;

        /// <summary>
        /// Field denoting the long name of attribute parent
        /// </summary>
        private String _attributeParentLongName = String.Empty;

        /// <summary>
        /// Field denoting the required property of mapping
        /// </summary>
        private Boolean _required = false;

        /// <summary>
        /// Field denoting the read only property of mapping
        /// </summary>
        private Boolean _readOnly = false;

        /// <summary>
        /// Field denoting the show at creation property of mapping
        /// </summary>
        private Boolean _showAtCreation = false;

        /// <summary>
        /// Field denoting the extension property of mapping
        /// </summary>
        private String _extension = string.Empty;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private Int32 _sortOrder = 0;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private EntityTypeAttributeMapping _originalEntityTypeAttributeMapping = null;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityTypeAttributeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an EntityType - Attribute Mapping</param>
        public EntityTypeAttributeMapping(Int32 id) :
            base(id)
        {

        }

        /// <summary>
        /// Constructor with EntityTypeId and AttributeId as input parameters
        /// </summary>
        /// <param name="entityTypeId">Indicates EntityType id in the mapping</param>
        /// <param name="attributeId">Indicates Attribute Id in the mapping</param>
        public EntityTypeAttributeMapping(Int32 entityTypeId, Int32 attributeId)
        {
            this.EntityTypeId = entityTypeId;
            this.AttributeId = attributeId;
        }

        /// <summary>
        /// Initialize new instance of EntityTypeAttributeMapping from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for EntityTypeAttributeMapping</param>
        public EntityTypeAttributeMapping(String valuesAsXml)
        {
            LoadEntityTypeAttributeMapping(valuesAsXml);
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
                return "EntityTypeAttributeMapping";
            }
        }

        /// <summary>
        /// Property denoting mapped EntityType Id
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get { return _entityTypeId; }
            set { _entityTypeId = value; }
        }

        /// <summary>
        /// Property denoting mapped Attribute Id
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        /// <summary>
        /// Property denoting mapped Attribute Parent Id
        /// </summary>
        [DataMember]
        public Int32 AttributeParentId
        {
            get { return _attributeParentId; }
            set { _attributeParentId = value; }
        }

        /// <summary>
        /// Property denoting the mapped EntityType Short Name
        /// </summary>
        [DataMember]
        public String EntityTypeName
        {
            get { return _entityTypeName; }
            set { _entityTypeName = value; }
        }

        /// <summary>
        /// Property denoting the mapped EntityType Long Name
        /// </summary>
        [DataMember]
        public String EntityTypeLongName
        {
            get { return _entityTypeLongName; }
            set { _entityTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Short Name
        /// </summary>
        [DataMember]
        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Long Name
        /// </summary>
        [DataMember]
        public String AttributeLongName
        {
            get { return _attributeLongName; }
            set { _attributeLongName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Parent Short Name
        /// </summary>
        [DataMember]
        public String AttributeParentName
        {
            get { return _attributeParentName; }
            set { _attributeParentName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Parent Long Name
        /// </summary>
        [DataMember]
        public String AttributeParentLongName
        {
            get { return _attributeParentLongName; }
            set { _attributeParentLongName = value; }
        }

        /// <summary>
        /// Property denoting the Required property for this Mapping
        /// </summary>
        [DataMember]
        public Boolean Required
        {
            get { return _required; }
            set { _required = value; }
        }

        /// <summary>
        /// Property denoting the Read Only property for this Mapping
        /// </summary>
        [DataMember]
        public Boolean ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        /// <summary>
        /// Property denoting the Show At Creation property for this Mapping
        /// </summary>
        [DataMember]
        public Boolean ShowAtCreation
        {
            get { return _showAtCreation; }
            set { _showAtCreation = value; }
        }

        /// <summary>
        /// Property denoting the Extension property for this Mapping
        /// </summary>
        [DataMember]
        public String Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        /// <summary>
        /// Property denoting the Sort Order property for this Mapping
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        /// <summary>
        /// Property denoting the OriginalEntityTypeAttributeMapping
        /// </summary>
        public EntityTypeAttributeMapping OriginalEntityTypeAttributeMapping
        {
            get { return _originalEntityTypeAttributeMapping; }
            set { _originalEntityTypeAttributeMapping = value; }
        }

        #region IDataModelObject Properties
        /// <summary>
        /// Property denoting the ExternalId property for this Mapping
        /// </summary>
        public string ExternalId
        {
            get{ return _externalId; }
            set{ _externalId = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.EntityTypeAttributeMapping;
            }
        }
        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of EntityTypeAttributeMapping object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXML()
        {
            String xml = string.Empty;

            xml = "<EntityTypeAttribute Id=\"{0}\" EntityTypeId=\"{1}\" AttributeId = \"{2}\" SortOrder = \"{3}\" FK_InheritanceMode = \"{4}\" Required = \"{5}\" ReadOnly = \"{6}\" ShowAtCreation = \"{7}\" Extension = \"{8}\" Action=\"{9}\" />    ";
                
            string retXML = string.Format(xml, this.Id, this.EntityTypeId, this.AttributeId, this.SortOrder, "2", ((this.Required)?'Y':'N'), this.ReadOnly, this.ShowAtCreation, this.Extension, this.Action);

            return retXML;
        }

        /// <summary>
        /// Clone EntityTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of EntityTypeAttributeMapping object.</returns>
        public IEntityTypeAttributeMapping Clone()
        {
            EntityTypeAttributeMapping clonedEntityTypeAttributeMapping = new EntityTypeAttributeMapping();

            clonedEntityTypeAttributeMapping.Id = this.Id;
            clonedEntityTypeAttributeMapping.Name = this.Name;
            clonedEntityTypeAttributeMapping.LongName = this.LongName;
            clonedEntityTypeAttributeMapping.Locale = this.Locale;
            clonedEntityTypeAttributeMapping.Action = this.Action;
            clonedEntityTypeAttributeMapping.AuditRefId = this.AuditRefId;
            clonedEntityTypeAttributeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedEntityTypeAttributeMapping.EntityTypeId = this.EntityTypeId;
            clonedEntityTypeAttributeMapping.EntityTypeName = this.EntityTypeName;
            clonedEntityTypeAttributeMapping.EntityTypeLongName = this.EntityTypeLongName;
            clonedEntityTypeAttributeMapping.AttributeId = this.AttributeId;
            clonedEntityTypeAttributeMapping.AttributeName = this.AttributeName;
            clonedEntityTypeAttributeMapping.AttributeLongName = this.AttributeLongName;
            clonedEntityTypeAttributeMapping.AttributeParentId = this.AttributeParentId;
            clonedEntityTypeAttributeMapping.AttributeParentName = this.AttributeParentName;
            clonedEntityTypeAttributeMapping.AttributeParentLongName = this.AttributeParentLongName;
            clonedEntityTypeAttributeMapping.Required = this.Required;
            clonedEntityTypeAttributeMapping.ReadOnly = this.ReadOnly;
            clonedEntityTypeAttributeMapping.ShowAtCreation = this.ShowAtCreation;
            clonedEntityTypeAttributeMapping.SortOrder = this.SortOrder;
            clonedEntityTypeAttributeMapping.Extension = this.Extension;
            clonedEntityTypeAttributeMapping.SortOrder = this.SortOrder;

            return clonedEntityTypeAttributeMapping;

        }

        /// <summary>
        /// Delta Merge of EntityTypeAttributeMapping
        /// </summary>
        /// <param name="deltaEntityTypeAttributeMapping">EntityTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged EntityTypeAttributeMapping instance</returns>
        public IEntityTypeAttributeMapping MergeDelta(IEntityTypeAttributeMapping deltaEntityTypeAttributeMapping, ICallerContext iCallerContext, bool returnClonedObject = true)
        {
            IEntityTypeAttributeMapping mergedEntityTypeAttributeMapping = (returnClonedObject == true) ? deltaEntityTypeAttributeMapping.Clone() : deltaEntityTypeAttributeMapping;
            mergedEntityTypeAttributeMapping.Action = (mergedEntityTypeAttributeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedEntityTypeAttributeMapping;

        }

        /// <summary>
        /// Compare entity type attribute mapping with current entity type attribute mapping .
        /// This method will compare entity type attribute mapping .
        /// </summary>
        /// <param name="subsetEntityTypeAttributeMapping">Entity type attribute mapping to be compared with current entity type attribute mapping.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(EntityTypeAttributeMapping subsetEntityTypeAttributeMapping, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetEntityTypeAttributeMapping.Id)
                    return false;

                if (this.EntityTypeId != subsetEntityTypeAttributeMapping.EntityTypeId)
                    return false;

                if (this.AttributeId != subsetEntityTypeAttributeMapping.AttributeId)
                    return false;
            }

            if (this.EntityTypeName != subsetEntityTypeAttributeMapping.EntityTypeName)
                return false;

            if (this.AttributeName != subsetEntityTypeAttributeMapping.AttributeName)
                return false;

            if (this.AttributeParentName != subsetEntityTypeAttributeMapping.AttributeParentName)
                return false;

            if (this.ReadOnly != subsetEntityTypeAttributeMapping.ReadOnly)
                return false;

            if (this.Required != subsetEntityTypeAttributeMapping.Required)
                return false;

            if (this.Action != subsetEntityTypeAttributeMapping.Action)
                return false;

            if (this.ShowAtCreation != subsetEntityTypeAttributeMapping.ShowAtCreation)
                return false;

            return true;
        }

        #region override
        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">EntityTypeAttributeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityTypeAttributeMapping)
            {
                EntityTypeAttributeMapping objectToBeCompared = obj as EntityTypeAttributeMapping;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                    return false;

                if (this.EntityTypeName != objectToBeCompared.EntityTypeName)
                    return false;

                if (this.EntityTypeLongName != objectToBeCompared.EntityTypeLongName)
                    return false;

                if (this.AttributeId != objectToBeCompared.AttributeId)
                    return false;

                if (this.AttributeName != objectToBeCompared.AttributeName)
                    return false;

                if (this.AttributeLongName != objectToBeCompared.AttributeLongName)
                    return false;

                if (this.AttributeParentId != objectToBeCompared.AttributeParentId)
                    return false;

                if (this.AttributeParentName != objectToBeCompared.AttributeParentName)
                    return false;

                if (this.AttributeParentLongName != objectToBeCompared.AttributeParentLongName)
                    return false;

                if (this.Required != objectToBeCompared.Required)
                    return false;

                if (this.ReadOnly != objectToBeCompared.ReadOnly)
                    return false;

                if (this.ShowAtCreation != objectToBeCompared.ShowAtCreation)
                    return false;

                if (this.Extension != objectToBeCompared.Extension)
                    return false;

                if (this.SortOrder != objectToBeCompared.SortOrder)
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
            return base.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.EntityTypeName.GetHashCode() ^ this.EntityTypeLongName.GetHashCode() ^ this.AttributeId.GetHashCode() ^ this.AttributeName.GetHashCode() ^ this.AttributeLongName.GetHashCode() ^ this.AttributeParentId.GetHashCode()
                   ^ this.AttributeParentName.GetHashCode() ^ this.AttributeParentLongName.GetHashCode() ^ this.Required.GetHashCode() ^ this.ReadOnly.GetHashCode() ^ this.ShowAtCreation.GetHashCode() ^ this.SortOrder.GetHashCode() ^ this.Extension.GetHashCode();
        }
        #endregion

        #region IDataModelObject

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current entity type attribute mapping
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <EntityTypeAttribute Id="-1" EntityTypeId="29" AttributeId = "4079" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" Action="Create" />
        /// ]]>
        /// </para>
        /// </param>
        private void LoadEntityTypeAttributeMapping(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <EntityTypeAttribute Id="-1" EntityTypeId="29" AttributeId = "4079" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" Action="Create" />
             */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityTypeAttribute")
                        {
                            #region Read ContainerEntityType attributes

                            if (reader.HasAttributes)
                            {

                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("EntityTypeId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("EntityTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeId"))
                                {
                                    this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("AttributeName"))
                                {
                                    this.AttributeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeParentName"))
                                {
                                    this.AttributeParentName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ReadOnly"))
                                {
                                    this.ReadOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Required"))
                                {
                                    this.Required = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ShowAtCreation"))
                                {
                                    this.ShowAtCreation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                            }

                            #endregion
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

        #endregion

        #endregion

    }
}
