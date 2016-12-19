using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent class specifying details about the relationship
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(AttributeCollection))]
    [KnownType(typeof(RelationshipCollection))]
    public class Relationship : RelationshipBase, IRelationship
    {
        #region Fields

        /// <summary>
        /// Field for the id of an Entity
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field denoting relationships
        /// </summary>
        [DataMember]
        private RelationshipCollection _relationshipCollection = new RelationshipCollection();

        /// <summary>
        /// Field denoting the relationship attributes
        /// </summary>
        private AttributeCollection _relationshipAttributes = new AttributeCollection();

        /// <summary>
        /// Field denoting source flag of relationship
        /// </summary>
        private AttributeValueSource _sourceFlag = AttributeValueSource.Overridden;

        /// <summary>
        /// Field denoting relationship external id for an entity object
        /// </summary>
        private String _relationshipExternalId = String.Empty;

        /// <summary>
        /// Field denoting a reference id for a relationship object
        /// </summary>
        private Int64 _referenceId = -1;

        /// <summary>
        /// Field denoting relationship type id
        /// </summary>
        private Int32 _relationshipTypeId = -1;

        /// <summary>
        /// Field denoting from EntityId for a relationship
        /// </summary>
        private Int64 _fromEntityId = -1;

        /// <summary>
        /// Field denoting relationship type name
        /// </summary>
        private String _relationshipTypeName = String.Empty;

        /// <summary>
        /// Field denoting row Id for 1 relationship
        /// </summary>
        private Int64 _rowId = -1;

        /// <summary>
        /// Field denoting level for a relationship
        /// </summary>
        private Int16 _level = 1;

        /// <summary>
        /// Field denoting relationship source id for a relationship
        /// </summary>
        private Int64 _relationshipSourceId = 0;

        /// <summary>
        /// Field denoting Id of Source Entity for a relationship
        /// </summary>
        private Int64 _relationshipSourceEntityId = 0;

        /// <summary>
        /// Field denoting relationship Source EntityName for a relationship
        /// </summary>
        private String _relationshipSourceEntityName = String.Empty;

        /// <summary>
        /// Field denoting relationship Source EntityLongName for a relationship
        /// </summary>
        private String _relationshipSourceEntityLongName = String.Empty;

        /// <summary>
        /// Field denoting parent id for a relationship
        /// </summary>
        private Int64 _relationshipParentId = 0;

        /// <summary>
        /// Field denoting Id of Parent Entity for a relationship
        /// </summary>
        private Int64 _relationshipParentEntityId = 0;

        /// <summary>
        /// Field denoting inheritance mode for a relationship
        /// </summary>
        private InheritanceMode _inheritanceMode = InheritanceMode.Direct;

        /// <summary>
        /// Field denoting status for a relationship
        /// </summary>
        private RelationshipStatus _status = RelationshipStatus.Active;

        /// <summary>
        /// Field denoting relationship To external id
        /// </summary>
        private String _toExternalId = String.Empty;

        /// <summary>
        /// Field denoting relationship To Entity Long Name
        /// </summary>
        private String _toLongName = String.Empty;

        /// <summary>
        /// Field denoting the parent Id of related entity
        /// </summary>
        private Int64 _toParentEntityId = 0;

        /// <summary>
        /// Field denoting the parent short name of related entity
        /// </summary>
        private String _toParentEntityName = String.Empty;

        /// <summary>
        /// Field denoting the parent long name of related entity
        /// </summary>
        private String _toParentEntityLongName = String.Empty;

        /// <summary>
        /// Field denoting container Id of To entity
        /// </summary>
        private Int32 _toContainerId = 0;

        /// <summary>
        /// Field denoting relationship container name for an entity object
        /// </summary>
        private String _toContainerName = String.Empty;

        /// <summary>
        /// Field denoting relationship container long name for an entity object
        /// </summary>
        private String _toContainerLongName = String.Empty;

        /// <summary>
        /// Field denoting relationship category path for an entity object
        /// </summary>
        private String _toCategoryPath = String.Empty;

        /// <summary>
        /// Field denoting relationship category long name path for an entity object
        /// </summary>
        private String _toCategoryLongNamePath = String.Empty;

        /// <summary>
        /// Field denoting entity type Id of To entity
        /// </summary>
        private Int32 _toEntityTypeId = 0;

        /// <summary>
        /// Field denoting entity type name of To entity
        /// </summary>
        private String _toEntityTypeName = String.Empty;

        /// <summary>
        /// Field denoting entity type long name of To entity
        /// </summary>
        private String _toEntityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting container Id for FromEntity
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Field denoting original object before change
        /// </summary>
        private Relationship _originalRelatonship = null;

        /// <summary>
        /// Field denoting relationship processing context which is being used for relationship denorm.
        /// </summary>
        private RelationshipProcessingContext _relationshipProcessingContext = null;

        /// <summary>
        /// Field indicates which program is the source of changes of object
        /// </summary>
        private SourceInfo _sourceInfo;

        /// <summary>
        /// Field indicates cross reference id between approved and collaboration of relationship
        /// </summary>
        private Int64 _crossReferenceId;

        /// <summary>
        /// Field indicates cross reference id between approved and collaboration of from entity
        /// </summary>
        private Int64 _fromCrossReferenceId;

        /// <summary>
        /// Field indicates cross reference id between approved and collaboration of related entity
        /// </summary>
        private Int64 _relatedCrossReferenceId;

        #endregion

        #region Properties

        /// <summary>
        /// Property for the id of an Entity
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public override String ObjectType
        {
            get
            {
                return "Relationship";
            }
        }

        /// <summary>
        /// Property denoting the relationship attributes
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public AttributeCollection RelationshipAttributes
        {
            get
            {
                return _relationshipAttributes;
            }
            set
            {
                _relationshipAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting the relationships of the logically related entity
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public new RelationshipCollection RelationshipCollection
        {
            get
            {
                return _relationshipCollection;
            }
            set
            {
                _relationshipCollection = value;
            }
        }

        /// <summary>
        /// Property denoting source flag of relationship
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public AttributeValueSource SourceFlag
        {
            get
            {
                return _sourceFlag;
            }
            set
            {
                _sourceFlag = value;
            }
        }

        /// <summary>
        /// Property denoting relationship type id
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Int32 RelationshipTypeId
        {
            get 
            { 
                return _relationshipTypeId; 
            }
            set 
            { 
                _relationshipTypeId = value; 
            }
        }

        /// <summary>
        /// Property denoting from EntityId for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Int64 FromEntityId
        {
            get { return _fromEntityId; }
            set { _fromEntityId = value; }
        }

        /// <summary>
        /// Property denoting relationship type name
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public String RelationshipTypeName
        {
            get { return _relationshipTypeName; }
            set { _relationshipTypeName = value; }
        }

        /// <summary>
        /// Property denoting the ExternalId of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public String ToExternalId
        {
            get
            {
                String toExternalId = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toExternalId))
                    toExternalId = _toExternalId;
                else if (this.RelatedEntity != null)
                    toExternalId = this.RelatedEntity.Name;

                return toExternalId;
            }
            set { _toExternalId = value; }
        }

        /// <summary>
        /// Property denoting the related entity's LongName
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public String ToLongName
        {
            get
            {
                String toLongName = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toLongName))
                    toLongName = _toLongName;
                else if (this.RelatedEntity != null)
                    toLongName = this.RelatedEntity.LongName;

                return toLongName;
            }
            set { _toLongName = value; }
        }

        /// <summary>
        /// Property denoting the parent Id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public Int64 ToParentEntityId
        {
            get
            {
                Int64 toParentEntityId = 0;

                if (_toParentEntityId > 0)
                    toParentEntityId = _toParentEntityId;
                else if (this.RelatedEntity != null)
                    toParentEntityId = this.RelatedEntity.ParentEntityId;

                return toParentEntityId;
            }
            set
            {
                this._toParentEntityId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent short name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public String ToParentEntityName
        {
            get
            {
                String toParentEntityName = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toParentEntityName))
                    toParentEntityName = _toParentEntityName;
                else if (this.RelatedEntity != null)
                    toParentEntityName = this.RelatedEntity.ParentEntityName;

                return toParentEntityName;
            }
            set
            {
                this._toParentEntityName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public String ToParentEntityLongName
        {
            get
            {
                String toParentEntityLongName = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toParentEntityLongName))
                    toParentEntityLongName = _toParentEntityLongName;
                else if (this.RelatedEntity != null)
                    toParentEntityLongName = this.RelatedEntity.ParentEntityLongName;

                return toParentEntityLongName;
            }
            set
            {
                this._toParentEntityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting container Id of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public Int32 ToContainerId
        {
            get
            {
                Int32 toContainerId = 0;

                if (_toContainerId > 0)
                    toContainerId = _toContainerId;
                else if (this.RelatedEntity != null)
                    toContainerId = this.RelatedEntity.ContainerId;

                return toContainerId;
            }
            set { _toContainerId = value; }
        }

        /// <summary>
        /// Property denoting the container name of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public String ToContainerName
        {
            get
            {
                String toContainerName = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toContainerName))
                    toContainerName = _toContainerName;
                else if (this.RelatedEntity != null)
                    toContainerName = this.RelatedEntity.ContainerName;

                return toContainerName;
            }
            set { _toContainerName = value; }
        }

        /// <summary>
        /// Specifies the container long name of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public String ToContainerLongName
        {
            get
            {
                String toContainerLongName = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toContainerLongName))
                    toContainerLongName = _toContainerLongName;
                else if (this.RelatedEntity != null)
                    toContainerLongName = this.RelatedEntity.ContainerLongName;

                return toContainerLongName;
            }
            set { _toContainerLongName = value; }
        }

        /// <summary>
        /// Property denoting the CategoryPath of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        public String ToCategoryPath
        {
            get
            {
                String toCategoryPath = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toCategoryPath))
                    toCategoryPath = _toCategoryPath;
                else if (this.RelatedEntity != null)
                    toCategoryPath = this.RelatedEntity.CategoryPath;

                return toCategoryPath;
            }
            set { _toCategoryPath = value; }
        }

        /// <summary>
        /// Specifies the Category Long Name Path of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
        public String ToCategoryLongNamePath
        {
            get
            {
                String toCategoryLongNamePath = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toCategoryLongNamePath))
                    toCategoryLongNamePath = _toCategoryLongNamePath;
                else if (this.RelatedEntity != null)
                    toCategoryLongNamePath = this.RelatedEntity.CategoryLongNamePath;

                return toCategoryLongNamePath;
            }
            set { _toCategoryLongNamePath = value; }
        }

        /// <summary>
        /// Property denoting the type Id of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        public Int32 ToEntityTypeId
        {
            get
            {
                Int32 toEntityTypeId = 0;

                if (_toEntityTypeId > 0)
                    toEntityTypeId = _toEntityTypeId;
                else if (this.RelatedEntity != null)
                    toEntityTypeId = this.RelatedEntity.EntityTypeId;

                return toEntityTypeId;
            }
            set { _toEntityTypeId = value; }
        }

        /// <summary>
        /// Property denoting the type name of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(19)]
        public String ToEntityTypeName
        {
            get
            {
                String toEntityTypeName = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toEntityTypeName))
                    toEntityTypeName = _toEntityTypeName;
                else if (this.RelatedEntity != null)
                    toEntityTypeName = this.RelatedEntity.EntityTypeName;

                return toEntityTypeName;
            }
            set { _toEntityTypeName = value; }
        }

        /// <summary>
        /// Specifies the entity type long name of To entity
        /// </summary>
        [DataMember]
        [ProtoMember(20)]
        public String ToEntityTypeLongName
        {
            get
            {
                String toEntityTypeLongName = String.Empty;

                if (!String.IsNullOrWhiteSpace(_toEntityTypeLongName))
                    toEntityTypeLongName = _toEntityTypeLongName;
                else if (this.RelatedEntity != null)
                    toEntityTypeLongName = this.RelatedEntity.EntityTypeLongName;

                return toEntityTypeLongName;
            }
            set { _toEntityTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting row Id for 1 relationship
        /// </summary>
        [DataMember]
        [ProtoMember(21)]
        public Int64 RowId
        {
            get { return _rowId; }
            set { _rowId = value; }
        }

        /// <summary>
        /// Property denoting level for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(22)]
        public Int16 Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// Property denoting relationship source id for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(23)]
        public Int64 RelationshipSourceId
        {
            get { return _relationshipSourceId; }
            set { _relationshipSourceId = value; }
        }

        /// <summary>
        /// Specifies source entityid for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(24)]
        public Int64 RelationshipSourceEntityId
        {
            get { return _relationshipSourceEntityId; }
            set { _relationshipSourceEntityId = value; }
        }

        /// <summary>
        /// Specifies Source EntityName for relationship
        /// </summary>
        [DataMember]
        [ProtoMember(25)]
        public String RelationshipSourceEntityName
        {
            get { return _relationshipSourceEntityName; }
            set { _relationshipSourceEntityName = value; }
        }

        /// <summary>
        /// Specifies SourceEntity LongName for relationship
        /// </summary>
        [DataMember]
        [ProtoMember(26)]
        public String RelationshipSourceEntityLongName
        {
            get { return _relationshipSourceEntityLongName; }
            set { _relationshipSourceEntityLongName = value; }
        }

        /// <summary>
        /// Specifies parent entityid for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(27)]
        public Int64 RelationshipParentEntityId
        {
            get { return _relationshipParentEntityId; }
            set { _relationshipParentEntityId = value; }
        }

        /// <summary>
        /// Property denoting parent id for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(28)]
        public Int64 RelationshipParentId
        {
            get { return _relationshipParentId; }
            set { _relationshipParentId = value; }
        }

        /// <summary>
        /// Property denoting inheritance mode for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(29)]
        public InheritanceMode InheritanceMode
        {
            get { return _inheritanceMode; }
            set { _inheritanceMode = value; }
        }

        /// <summary>
        /// Property denoting status for a relationship
        /// </summary>
        [DataMember]
        [ProtoMember(30)]
        public RelationshipStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Property denoting the relationship external id for an entity object
        /// </summary>
        [DataMember]
        [ProtoMember(31)]
        public String RelationshipExternalId
        {
            get
            {
                return this._relationshipExternalId;
            }
            set
            {
                this._relationshipExternalId = value;
            }
        }

        /// <summary>
        /// Property denoting container Id for FromEntity
        /// </summary>
        [DataMember]
        [ProtoMember(32)]
        public Int32 ContainerId
        {
            get
            {
                return _containerId;
            }
            set
            {
                _containerId = value;
            }
        }

        /// <summary>
        /// Property denoting the original relationship object before changes
        /// </summary>
        public Relationship OriginalRelationship
        {
            get
            {
                return _originalRelatonship;
            }
            set
            {
                _originalRelatonship = value;
            }
        }

        /// <summary>
        /// Specifies relationship processing context which is being used for relationship denorm.
        /// </summary>
        public RelationshipProcessingContext RelationshipProcessingContext
        {
            get
            {
                return _relationshipProcessingContext;
            }
            set
            {
                _relationshipProcessingContext = value;
            }
        }

        /// <summary>
        /// Property defines which program is the source of changes of object
        /// </summary>
        [DataMember]
        [ProtoMember(33)]
        public SourceInfo SourceInfo
        {
            get
            {
                return _sourceInfo;
            }
            set
            {
                _sourceInfo = value;
            }
        }

        /// <summary>
        /// Property denoting the external id for a relationship object
        /// </summary>
        [DataMember]
        [ProtoMember(34)]
        public new Int64 ReferenceId
        {
            get
            {
                return this._referenceId;
            }
            set
            {
                this._referenceId = value;
            }
        }

        /// <summary>
        /// Property denotes cross reference id between approved and collaboration of relationship
        /// </summary>
        [DataMember]
        [ProtoMember(35)]
        public Int64 CrossReferenceId
        {
            get { return this._crossReferenceId; }
            set { this._crossReferenceId = value; }
        }

        /// <summary>
        /// Property denotes cross reference id between approved and collaboration of from entity
        /// </summary>
        [DataMember]
        [ProtoMember(36)]
        public Int64 FromCrossReferenceId
        {
            get { return this._fromCrossReferenceId; }
            set { this._fromCrossReferenceId = value; }
        }

        /// <summary>
        /// Property denotes cross reference id between approved and collaboration of related entity
        /// </summary>
        [DataMember]
        [ProtoMember(37)]
        public Int64 RelatedCrossReferenceId
        {
            get { return this._relatedCrossReferenceId; }
            set { this._relatedCrossReferenceId = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the relationship class
        /// </summary>
        public Relationship()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the  relationship class with the provided parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        /// <param name="type">Indicates the relationship type</param>
        /// <param name="relatedEntityId">Indicates the related entity id</param>
        /// <param name="relationshipTypeName">Name of the relationship type</param>
        /// <param name="relationshipTypeId">Id of the relationship type</param>
        /// <param name="direction">Indicates the direction</param>
        /// <param name="path">Indicates the path of the relationship</param>
        public Relationship(Int32 id, String type, Int32 relationshipTypeId, String relationshipTypeName, Int64 relatedEntityId, RelationshipDirection direction, String path)
            : base(id, type, relatedEntityId, direction, path)
        {
            this._relationshipTypeId = relationshipTypeId;
            this._relationshipTypeName = relationshipTypeName;
        }

        /// <summary>
        /// Initializes a new instance of the  relationship class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="Relationship" Type="[Entity]or[Category]" RelatedEntityId="[Parent]" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        public Relationship(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadRelationshipDetails(valuesAsXml, objectSerialization);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is RelationshipBase)
                {
                    Relationship objectToBeCompared = obj as Relationship;

                    if (this.RelationshipExternalId != objectToBeCompared.RelationshipExternalId)
                        return false;

                    if (this.ToContainerId != objectToBeCompared.ToContainerId)
                        return false;

                    if (this.ToContainerName != objectToBeCompared.ToContainerName)
                        return false;

                    if (this.ToCategoryPath != objectToBeCompared.ToCategoryPath)
                        return false;

                    if (this.ToEntityTypeName != objectToBeCompared.ToEntityTypeName)
                        return false;

                    if (this.ToExternalId != objectToBeCompared.ToExternalId)
                        return false;

                    if (this.RelationshipTypeName != objectToBeCompared.RelationshipTypeName)
                        return false;

                    if (this.RelationshipTypeId != objectToBeCompared.RelationshipTypeId)
                        return false;

                    if (this.Direction != objectToBeCompared.Direction)
                        return false;

                    if (this.FromEntityId != objectToBeCompared.FromEntityId)
                        return false;

                    if (!this.RelationshipCollection.Equals(objectToBeCompared.RelationshipCollection))
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode = base.GetHashCode() ^ this.RelationshipCollection.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Relationship object
        /// </summary>
        /// <returns>Xml representation of Relationship object</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Relationship node start
            xmlWriter.WriteStartElement("Relationship");

            ConvertRelationshipMetadataToXml(xmlWriter);

            #region Write Relationship Attributes

            if (this.RelationshipAttributes != null)
                xmlWriter.WriteRaw(this.RelationshipAttributes.ToXml());

            #endregion

            #region Write Relationships

            if (this.RelationshipCollection != null)
            {
                String childRelationshipsXml = "<Relationships>";

                foreach (Relationship Relationship in this.RelationshipCollection)
                {
                    childRelationshipsXml = String.Concat(childRelationshipsXml, Relationship.ToXml());
                }

                childRelationshipsXml = String.Concat(childRelationshipsXml, "</Relationships>");

                xmlWriter.WriteRaw(childRelationshipsXml);
            }

            #endregion

            //Relationship node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity View based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity View</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                returnXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Relationship node start
                xmlWriter.WriteStartElement("Relationship");

                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LName", this.LongName);

                    String type = String.Empty;
                    if (Type != null)
                        type = Type.LongName;

                    xmlWriter.WriteAttributeString("Type", type);
                    xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
                    xmlWriter.WriteAttributeString("FromEnId", this.FromEntityId.ToString());
                    xmlWriter.WriteAttributeString("RelatedEnId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("ContId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipExtnId", this.RelationshipExternalId);
                    xmlWriter.WriteAttributeString("ToCatPath", this.ToCategoryPath);
                    xmlWriter.WriteAttributeString("ToContId", this.ToContainerId.ToString());
                    xmlWriter.WriteAttributeString("ToContName", this.ToContainerName);
                    xmlWriter.WriteAttributeString("ToEnTypeId", this.ToEntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("ToEnTypeName", this.ToEntityTypeName);
                    xmlWriter.WriteAttributeString("ToExtnId", this.ToExternalId);
                    xmlWriter.WriteAttributeString("ToLName", this.ToLongName);
                    xmlWriter.WriteAttributeString("Level", this.Level.ToString());
                    xmlWriter.WriteAttributeString("RelSourceId", this.RelationshipSourceId.ToString());
                    xmlWriter.WriteAttributeString("RelParentId", this.RelationshipParentId.ToString());
                    xmlWriter.WriteAttributeString("InhMode", this.InheritanceMode.ToString());
                    xmlWriter.WriteAttributeString("Status", this.Status.ToString());

                    #endregion

                    #region Write Related Entity Details

                    if (this.RelatedEntity != null)
                {
                        xmlWriter.WriteRaw(this.RelatedEntity.ToXml(objectSerialization));
                    }

                    #endregion
                }
                else if (objectSerialization == ObjectSerialization.Compact)
                {
                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LN", this.LongName);
                    xmlWriter.WriteAttributeString("ObjType", this.ObjectType);

                    String type = String.Empty;
                    if (Type != null)
                        type = Type.LongName;

                    xmlWriter.WriteAttributeString("Type", type);
                    xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
                    xmlWriter.WriteAttributeString("FromEntityId", this.FromEntityId.ToString());
                    xmlWriter.WriteAttributeString("RelEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Dir", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }
                else if (objectSerialization == ObjectSerialization.External)
                {
                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    
                    //xmlWriter.WriteAttributeString("Name", this.Name);
                    //xmlWriter.WriteAttributeString("LongName", this.LongName);
                    //xmlWriter.WriteAttributeString("ObjectType", this.ObjectType);

                    //String type = String.Empty;
                    //if (Type != null)
                    //    type = Type.LongName;

                    //xmlWriter.WriteAttributeString("Type", type);
                    //xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());

                    xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
                    xmlWriter.WriteAttributeString("FromEntityId", this.FromEntityId.ToString());
                    xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    
                    //xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    //xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    
                    xmlWriter.WriteAttributeString("RelationshipExternalId", this.RelationshipExternalId);

                    xmlWriter.WriteAttributeString("ToExternalId", this.ToExternalId);
                    xmlWriter.WriteAttributeString("ToEntityTypeName", this.ToEntityTypeName);
                    xmlWriter.WriteAttributeString("ToCategoryPath", this.ToCategoryPath);
                    xmlWriter.WriteAttributeString("ToContainerName", this.ToContainerName);
                    
                    xmlWriter.WriteAttributeString("Action", ValueTypeHelper.GetActionString(this.Action));

                    #endregion
                }
                else if (objectSerialization == ObjectSerialization.DataTransfer)
                {
                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("RTId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("RTN", this.RelationshipTypeName);
                    xmlWriter.WriteAttributeString("FEId", this.FromEntityId.ToString());
                    xmlWriter.WriteAttributeString("REId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("RPId", this.RelationshipParentId.ToString());
                    xmlWriter.WriteAttributeString("L", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("SF", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("A", this.Action.ToString());
                    xmlWriter.WriteAttributeString("ContId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ToCatLNP", this.ToCategoryLongNamePath);
                    xmlWriter.WriteAttributeString("ToContId", this.ToContainerId.ToString());
                    xmlWriter.WriteAttributeString("ToContN", this.ToContainerName);
                    xmlWriter.WriteAttributeString("ToETId", this.ToEntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("ToETN", this.ToEntityTypeName);
                    xmlWriter.WriteAttributeString("ToExId", this.ToExternalId);
                    xmlWriter.WriteAttributeString("ToLN", this.ToLongName);
                    xmlWriter.WriteAttributeString("Lvl", this.Level.ToString());
                    xmlWriter.WriteAttributeString("IM", this.InheritanceMode.ToString());
                    xmlWriter.WriteAttributeString("S", this.Status.ToString());
                    xmlWriter.WriteAttributeString("PS", Utility.GetPermissionsAsString(this.PermissionSet));

                    #endregion

                    #region Write Related Entity Details

                    if (this.RelatedEntity != null)
                    {
                        xmlWriter.WriteRaw(this.RelatedEntity.ToXml(objectSerialization));
                    }

                    #endregion
                }
                else
                {
                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("ObjectType", this.ObjectType);

                    String type = String.Empty;
                    if (Type != null)
                        type = Type.LongName;

                    xmlWriter.WriteAttributeString("Type", type);
                    xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
                    xmlWriter.WriteAttributeString("FromEntityId", this.FromEntityId.ToString());
                    xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }

                #region Write Relationship Attributes

                if (this.RelationshipAttributes != null)
                {
                    xmlWriter.WriteRaw(this.RelationshipAttributes.ToXml(objectSerialization));
                }

                #endregion

                #region Write Relationships

                if (this.RelationshipCollection != null)
                {
                    String childRelationshipsXml = "<Relationships>";

                    foreach (Relationship Relationship in this.RelationshipCollection)
                    {
                        childRelationshipsXml = String.Concat(childRelationshipsXml, Relationship.ToXml(objectSerialization));
                    }

                    childRelationshipsXml = String.Concat(childRelationshipsXml, "</Relationships>");

                    xmlWriter.WriteRaw(childRelationshipsXml);
                }

                #endregion

                //Relationship node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return returnXml;
        }

        /// <summary>
        /// Converts Relationship object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of Relationship object</param>
        internal void ConvertRelationshipToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                //Relationship node start
                xmlWriter.WriteStartElement("Relationship");

                ConvertRelationshipMetadataToXml(xmlWriter);

                #region Write Relationship Attributes

                if (this._relationshipAttributes != null)
                {
                    this._relationshipAttributes.ConvertAttributeCollectionToXml(xmlWriter);
                }

                #endregion Write Relationship Attributes

                #region Write Relationships

                if (this._relationshipCollection != null)
                {
                    this._relationshipCollection.ConvertRelationshipCollectionToXml(xmlWriter);
                }

                #endregion Write Relationships

                //Relationship node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write Relationship object.");
            }
        }

        /// <summary>
        /// Loads Relationship object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>		
        /// <param name="context">Indicates context object which specifies what all data to be converted into object from Xml</param>
        internal void LoadRelationshipFromXml(XmlTextReader reader, EntityConversionContext context)
        {
            if (reader.HasAttributes)
            {
                LoadRelationshipMetadataFromXml(reader);
            }

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                {
                    this._relationshipAttributes.LoadAttributeCollectionFromXml(reader);
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                {
                    if (reader.HasAttributes)
                    {
                        this.RelatedEntity = new Entity();
                        this.RelatedEntity.LoadEntityFromXml(reader, context);
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                {
                    this._relationshipCollection.LoadRelationshipCollectionFromXml(reader, context);
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Relationship")
                {
                    return;
                }
            }

        }

        /// <summary>
        /// Gets  relationships
        /// </summary>
        /// <returns> Relationship collection interface</returns>
        public new IRelationshipCollection GetRelationships()
        {
            return (IRelationshipCollection)this.RelationshipCollection;
        }

        /// <summary>
        /// Sets  relationships
        /// </summary>
        /// <param name="iRelationshipCollection"> Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed  relationship collection is null</exception>
        public void SetRelationships(IRelationshipCollection iRelationshipCollection)
        {
            if (iRelationshipCollection == null)
                throw new ArgumentNullException("RelationshipCollection");

            this.RelationshipCollection = (RelationshipCollection)iRelationshipCollection;
        }

        /// <summary>
        /// Gets relationship attributes
        /// </summary>
        /// <returns> Relationship attribute collection</returns>
        public IAttributeCollection GetRelationshipAttributes()
        {
            return (IAttributeCollection)this.RelationshipAttributes;
        }

        /// <summary>
        /// Sets the attributes of the Relationship
        /// </summary>
        /// <param name="iAttributeCollection">Attribute Collection Interface</param>
        public void SetRelationshipAttributes(IAttributeCollection iAttributeCollection)
        {
            this.RelationshipAttributes = (AttributeCollection)iAttributeCollection;
        }

        /// <summary>
        /// Checks whether current object or its child objects have been changed i.e any object having Action flag as Create, Update or Delete
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasObjectChanged()
        {
            Boolean hasObjectUpdated = false;

            if ((this.Action != ObjectAction.Read && this.Action != ObjectAction.Unknown) ||
                this.RelationshipCollection != null && this.RelationshipCollection.HasObjectChanged())
                hasObjectUpdated = true;

            return hasObjectUpdated;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetRelationship">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Relationship subsetRelationship, Boolean compareIds = false)
        {
            if (subsetRelationship != null)
            {
                if (base.IsSuperSetOf(subsetRelationship, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.RelationshipTypeId != subsetRelationship.RelationshipTypeId)
                            return false;

                        if (this.FromEntityId != subsetRelationship.FromEntityId)
                            return false;

                        if (this.RelatedEntityId != subsetRelationship.RelatedEntityId)
                            return false;

                        if (this.ToContainerId != subsetRelationship.ToContainerId)
                            return false;

                        if (this.ToEntityTypeId != subsetRelationship.ToEntityTypeId)
                            return false;

                        if (this.RowId != subsetRelationship.RowId)
                            return false;

                        if (this.RelationshipSourceId != subsetRelationship.RelationshipSourceId)
                            return false;

                        if (this.RelationshipParentId != subsetRelationship.RelationshipParentId)
                            return false;

                        if (this.RelationshipExternalId != subsetRelationship.RelationshipExternalId)
                            return false;

                        if (this.ContainerId != subsetRelationship.ContainerId)
                            return false;

                        if (this.RelationshipSourceEntityId != subsetRelationship.RelationshipSourceEntityId)
                            return false;

                        if (this.RelationshipParentEntityId != subsetRelationship.RelationshipParentEntityId)
                            return false;
                    }

                    if (!this.RelationshipAttributes.IsSuperSetOf(subsetRelationship.RelationshipAttributes))
                        return false;

                    if (!this.RelationshipCollection.IsSuperSetOf(subsetRelationship.RelationshipCollection))
                        return false;

                    if (this.SourceFlag != subsetRelationship.SourceFlag)
                        return false;

                    if (this.RelationshipTypeName != subsetRelationship.RelationshipTypeName)
                        return false;

                    if (this.ToExternalId != subsetRelationship.ToExternalId)
                        return false;

                    if (this.ToLongName != subsetRelationship.ToLongName)
                        return false;

                    if (this.ToContainerName != subsetRelationship.ToContainerName)
                        return false;

                    if (this.ToCategoryPath != subsetRelationship.ToCategoryPath)
                        return false;

                    if (this.ToEntityTypeName != subsetRelationship.ToEntityTypeName)
                        return false;

                    if (this.Level != subsetRelationship.Level)
                        return false;

                    if (this.InheritanceMode != subsetRelationship.InheritanceMode)
                        return false;

                    if (this.Status != subsetRelationship.Status)
                        return false;

                    if (this.ObjectType != subsetRelationship.ObjectType)
                        return false;

                    if (!this.Type.Equals(subsetRelationship.Type))
                        return false;

                    if (this.Direction != subsetRelationship.Direction)
                        return false;

                    if (this.RelatedEntity != null && subsetRelationship.RelatedEntity != null)
                    {
                        if (!this.RelatedEntity.IsSuperSetOf(subsetRelationship.RelatedEntity))
                            return false;
                    }

                    if (this.RelationshipSourceEntityName != subsetRelationship.RelationshipSourceEntityName)
                        return false;

                    if (this.RelationshipSourceEntityLongName != subsetRelationship.RelationshipSourceEntityLongName)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetRelationship">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>If no mismatch found RelationshipOperationResult is sucessful, fail otherwise</returns>
        public RelationshipOperationResult GetSuperSetOfOperationResult(Relationship subsetRelationship, Boolean compareIds = false)
        {

            var relationshipOperationResult = new RelationshipOperationResult();
            
            relationshipOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
            
            var entityOperationResult = new EntityOperationResult();
            
            if (subsetRelationship != null)
            {

                #region compare Ids

                if (compareIds)
                {
                    Utility.BusinessObjectPropertyCompare("RelationshipTypeId", this.RelationshipTypeId, subsetRelationship.RelationshipTypeId, relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("FromEntityId", this.FromEntityId.ToString(), subsetRelationship.FromEntityId.ToString(), relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("RelatedEntityId", this.RelatedEntityId.ToString(), subsetRelationship.RelatedEntityId.ToString(), relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("ToContainerId", this.ToContainerId, subsetRelationship.ToContainerId, relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("ToEntityTypeId", this.ToEntityTypeId, subsetRelationship.ToEntityTypeId, relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("RowId", this.RowId.ToString(), subsetRelationship.RowId.ToString(), relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("RelationshipSourceId", this.RelationshipSourceId.ToString(), subsetRelationship.RelationshipSourceId.ToString(), relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("RelationshipParentId", this.RelationshipParentId.ToString(), subsetRelationship.RelationshipParentId.ToString(), relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("RelationshipExternalId", this.RelationshipExternalId, subsetRelationship.RelationshipExternalId, relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("ContainerId", this.ContainerId, subsetRelationship.ContainerId, relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("RelationshipSourceEntityId", this.RelationshipSourceEntityId.ToString(), subsetRelationship.RelationshipSourceEntityId.ToString(), relationshipOperationResult);
                    Utility.BusinessObjectPropertyCompare("RelationshipParentEntityId", this.RelationshipParentEntityId.ToString(), subsetRelationship.RelationshipParentEntityId.ToString(), relationshipOperationResult);
                }

                #endregion compare Ids

                #region compare relationship attributes

                AttributeOperationResultCollection relationshipAttributeIsSuperSetOfResult = this.RelationshipAttributes.GetSuperSetOperationResult(subsetRelationship.RelationshipAttributes, entityOperationResult);

                if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                {

                    if (relationshipAttributeIsSuperSetOfResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                    {
                        relationshipOperationResult.SetAttributeOperationResult(relationshipAttributeIsSuperSetOfResult);
                    }
                            
                    var rc = this.RelationshipCollection.GetSuperSetOfOperationResult(subsetRelationship.RelationshipCollection, false);

                    if (rc.OperationResultStatus != OperationResultStatusEnum.Successful)
                    {
                        relationshipOperationResult.SetRelationshipOperationResults(rc);
                    }
                }

                #endregion compare relationship attributes

                #region compare properties

                Utility.BusinessObjectPropertyCompare("SourceFlag", this.SourceFlag.ToString(), subsetRelationship.SourceFlag.ToString(), relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("RelationshipTypeName", this.RelationshipTypeName, subsetRelationship.RelationshipTypeName, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("ToExternalId", this.ToExternalId, subsetRelationship.ToExternalId, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("ToLongName", this.ToLongName, subsetRelationship.ToLongName, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("ToContainerName", this.ToContainerName, subsetRelationship.ToContainerName, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("ToCategoryPath", this.ToCategoryPath, subsetRelationship.ToCategoryPath, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("ToEntityTypeName", this.ToEntityTypeName, subsetRelationship.ToEntityTypeName, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("Level", (Int32)this.Level, (Int32)subsetRelationship.Level, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("InheritanceMode", this.InheritanceMode.ToString(), subsetRelationship.InheritanceMode.ToString(), relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("Status", (Int32)this.Status, (Int32)subsetRelationship.Status, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("ObjectType", this.ObjectType, subsetRelationship.ObjectType, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("Type", this.Type.ToString(), subsetRelationship.Type.ToString(), relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("Direction", this.Direction.ToString(), subsetRelationship.Direction.ToString(), relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("RelationshipSourceEntityName", this.RelationshipSourceEntityName, subsetRelationship.RelationshipSourceEntityName, relationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("RelationshipSourceEntityName", this.RelationshipSourceEntityName, subsetRelationship.RelationshipSourceEntityName, relationshipOperationResult);

                #endregion compare properties

                #region compare related entity

                if (this.RelatedEntity != null && subsetRelationship.RelatedEntity != null)
                {
                    var relatedEntityOperationResult = this.RelatedEntity.GetSuperSetOperationResult(subsetRelationship.RelatedEntity);
                    
                    if (relatedEntityOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                    {
                        relationshipOperationResult.AddOperationResult("-1", "Following errors are in related entity:", OperationResultType.Error);
                        relationshipOperationResult.Errors = relatedEntityOperationResult.Errors;
                    }
                }
                else
                {
                    relationshipOperationResult.AddOperationResult("-1", String.Format("Relationship {0} has no relatedEntity to compare", this.Name), OperationResultType.Information);
                }

                #endregion compare related entity

            }

            relationshipOperationResult.RefreshOperationResultStatus();

            return relationshipOperationResult;
        }

        /// <summary>
        /// Create a new relationship object.
        /// </summary>
        /// <returns>New relationship instance.</returns>
        public Relationship Clone()
        {
            Relationship relationship = this.CloneBaseProperties();

            if (this._relationshipCollection != null && this._relationshipCollection.Count > 0)
            {
                RelationshipCollection clonedChildRelationships = new RelationshipCollection();

                foreach (Relationship childRelationship in this._relationshipCollection)
                {
                    Relationship clonedChildRel = childRelationship.Clone(); // Recurse method
                    clonedChildRelationships.Add(clonedChildRel);
                }

                relationship._relationshipCollection = clonedChildRelationships;
            }

            relationship._relationshipAttributes = (AttributeCollection)this._relationshipAttributes.Clone();

            Entity relatedEntity = this.RelatedEntity;

            if (relatedEntity != null)
            {
                relationship.RelatedEntity = relatedEntity.CloneBasicProperties();

                AttributeCollection attributes = this.RelatedEntity.Attributes;

                if (attributes != null)
                {
                    relationship.RelatedEntity.Attributes = (AttributeCollection)attributes.Clone();
                }
            }

            return relationship;
        }

        /// <summary>
        /// Create a new relationship with basic properties of relationship.
        /// </summary>
        /// <returns>New relationship instance having same properties like current relationship</returns>
        public Relationship CloneBaseProperties()
        {
            Relationship relationship = new Relationship();

            relationship._id = this._id;
            relationship.Name = this.Name;
            relationship.LongName = this.LongName;
            relationship.Action = this.Action;
            relationship.AuditRefId = this.AuditRefId;
            relationship.Locale = this.Locale;
            relationship.UserName = this.UserName;

            relationship.Direction = this.Direction;
            relationship.Path = this.Path;
            relationship.RelatedEntityId = this.RelatedEntityId;
            relationship.Type = this.Type;

            relationship._containerId = this._containerId;
            relationship._fromEntityId = this._fromEntityId;
            relationship._inheritanceMode = this._inheritanceMode;
            relationship._relationshipExternalId = this._relationshipExternalId;
            relationship._relationshipTypeName = this._relationshipTypeName;
            relationship._relationshipTypeId = this._relationshipTypeId;
            relationship._relationshipParentId = this._relationshipParentId;
            relationship._relationshipSourceId = this._relationshipSourceId;
            relationship._toExternalId = this._toExternalId;
            relationship._toLongName = this._toLongName;
            relationship._toParentEntityId = this._toParentEntityId;
            relationship._toParentEntityName = this._toParentEntityName;
            relationship._toParentEntityLongName = this._toParentEntityLongName;
            relationship._toContainerId = this._toContainerId;
            relationship._toContainerName = this._toContainerName;
            relationship._toContainerLongName = this._toContainerLongName;
            relationship._toEntityTypeId = this._toEntityTypeId;
            relationship._toEntityTypeName = this.ToEntityTypeName;
            relationship._toEntityTypeLongName = this._toEntityTypeLongName;
            relationship._toCategoryPath = this._toCategoryPath;
            relationship._toCategoryLongNamePath = this._toCategoryLongNamePath;
            relationship._relationshipSourceEntityId = this._relationshipSourceEntityId;
            relationship._relationshipSourceEntityName = this._relationshipSourceEntityName;
            relationship._relationshipSourceEntityLongName = this._relationshipSourceEntityLongName;
            relationship._relationshipParentEntityId = this._relationshipParentEntityId;
            relationship._rowId = this._rowId;
            relationship._sourceFlag = this._sourceFlag;
            relationship._status = this._status;
            relationship._level = this._level;
            if (this._sourceInfo != null)
            {
                relationship._sourceInfo = (SourceInfo)this._sourceInfo.Clone();
            }
            else
            {
                relationship._sourceInfo = null;
            }

            if (this.RelatedEntity != null)
            {
                relationship.RelatedEntity = this.RelatedEntity.CloneBasicProperties();
            }

            relationship._crossReferenceId = this._crossReferenceId;
            relationship._fromCrossReferenceId = this._fromCrossReferenceId;
            relationship._relatedCrossReferenceId = this._relatedCrossReferenceId;

            return relationship;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the  relationship with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="Relationship" Type="[Entity]or[Category]" RelatedEntityId="[Parent]" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        private void LoadRelationshipDetails(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadRelationshipDetailsForDataStorage(valuesAsXml);
                }
                else if (objectSerialization == ObjectSerialization.DataTransfer)
                {
                    LoadRelationshipDetailsForDataTransfer(valuesAsXml);
                }
                else
                {
                    XmlTextReader reader = null;

                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                            {
                                #region Read relationship properties

                                if (reader.HasAttributes)
                                {
                                    LoadRelationshipMetadataFromXml(reader);

                                    reader.Read();
                                }

                                #endregion Read relationship properties
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                            {
                                #region Read attributes

                                String attributeXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(attributeXml))
                                {
                                    AttributeCollection attributeCollection = new AttributeCollection(attributeXml);
                                    if (attributeCollection != null)
                                    {
                                        // Based on the serialization type the unique 
                                        foreach (Attribute attr in attributeCollection)
                                        {
                                            if (!this.RelationshipAttributes.Contains(attr.Name, attr.AttributeParentName, attr.Locale))
                                                this.RelationshipAttributes.Add(attr);
                                        }
                                    }
                                }

                                #endregion Read attributes
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                            {
                                this.RelatedEntity = new Entity(reader.ReadOuterXml());
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                            {
                                #region Read child relationships

                                this.RelationshipCollection = new RelationshipCollection(reader.ReadOuterXml());

                                #endregion Read child relationships
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
        }

        /// <summary>
        /// Loads the  relationship with the XML having values of object when Enum of ObjectSerialization is DataStorage
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="Relationship" Type="[Entity]or[Category]" RelatedEntId="[Parent]" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        private void LoadRelationshipDetailsForDataStorage(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                        {
                            #region Read relationship properties

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

                                if (reader.MoveToAttribute("LName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    String strLocale = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                                    if (locale == LocaleEnum.UnKnown)
                                        throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");

                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("ContId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ContainerId);
                                }

                                if (reader.MoveToAttribute("Type"))
                                {
                                    String type = reader.ReadContentAsString();
                                    this.Type = new RelationshipType(-1, type, type);
                                }

                                if (reader.MoveToAttribute("RelationshipExtnId"))
                                {
                                    this.RelationshipExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToCatPath"))
                                {
                                    this.ToCategoryPath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToContId"))
                                {
                                    this.ToContainerId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ToContName"))
                                {
                                    this.ToContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToEnTypeId"))
                                {
                                    this.ToEntityTypeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ToEnTypeName"))
                                {
                                    this.ToEntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToExtnId"))
                                {
                                    this.ToExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToLName"))
                                {
                                    this.ToLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RelationshipTypeId"))
                                {
                                    this.RelationshipTypeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("RelationshipTypeName"))
                                {
                                    this.RelationshipTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RelatedEnId"))
                                {
                                    this.RelatedEntityId = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("FromEnId"))
                                {
                                    this.FromEntityId = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("Direction"))
                                {
                                    RelationshipDirection relDirection = RelationshipDirection.None;
                                    String strRelDirection = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(strRelDirection))
                                        Enum.TryParse<RelationshipDirection>(strRelDirection, out relDirection);

                                    this.Direction = relDirection;
                                }

                                if (reader.MoveToAttribute("Path"))
                                {
                                    this.Path = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("SourceFlag"))
                                {
                                    this.SourceFlag = Utility.GetSourceFlagEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Level"))
                                {
                                    this.Level = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), this.Level);
                                }

                                if (reader.MoveToAttribute("RelSourceId"))
                                {
                                    this.RelationshipSourceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.RelationshipSourceId);
                                }

                                if (reader.MoveToAttribute("RelParentId"))
                                {
                                    this.RelationshipParentId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.RelationshipParentId);
                                }

                                if (reader.MoveToAttribute("InhMode"))
                                {
                                    InheritanceMode inheritanceMode = InheritanceMode.Direct;
                                    Enum.TryParse<InheritanceMode>(reader.ReadContentAsString(), out inheritanceMode);
                                    this.InheritanceMode = inheritanceMode;
                                }

                                if (reader.MoveToAttribute("Status"))
                                {
                                    RelationshipStatus status = RelationshipStatus.Active;
                                    Enum.TryParse<RelationshipStatus>(reader.ReadContentAsString(), out status);
                                    this.Status = status;
                                }

                                if (reader.MoveToAttribute("SourceId"))
                                {
                                    SourceInfo sourceInfo = new SourceInfo();
                                    sourceInfo.SourceId = reader.ReadContentAsInt();
                                    if (reader.MoveToAttribute("SourceEntityId"))
                                    {
                                        sourceInfo.SourceEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                    }
                                    this.SourceInfo = sourceInfo;
                                }

                                if (reader.MoveToAttribute("CrossReferenceId"))
                                {
                                    this.CrossReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("FromCrossReferenceId"))
                                {
                                    this.FromCrossReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("RelatedCrossReferenceId"))
                                {
                                    this.RelatedCrossReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                reader.Read();
                            }

                            #endregion Read relationship properties
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                        {
                            #region Read attributes

                            String relatedEntityXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(relatedEntityXml))
                            {
                                this.RelatedEntity = new Entity(relatedEntityXml, ObjectSerialization.DataStorage);
                            }

                            #endregion Read attributes
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
                            #region Read attributes

                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                AttributeCollection attributeCollection = new AttributeCollection(attributeXml, false, ObjectSerialization.DataStorage);
                                if (attributeCollection != null)
                                {
                                    // Based on the serialization type the unique 
                                    foreach (Attribute attr in attributeCollection)
                                    {
                                        if (!this.RelationshipAttributes.Contains(attr.Name, attr.AttributeParentName, attr.Locale))
                                            this.RelationshipAttributes.Add(attr);
                                    }
                                }
                            }

                            #endregion Read attributes
                        }

                        #region Read child relationships

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                        {
                            this.RelationshipCollection = new RelationshipCollection(reader.ReadOuterXml(), ObjectSerialization.DataStorage);
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read child relationships
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

        private void LoadRelationshipDetailsForDataTransfer(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                        {
                            #region Read relationship properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("L"))
                                {
                                    String strLocale = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                                    if (locale == LocaleEnum.UnKnown)
                                        throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");

                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("RTId"))
                                {
                                    this.RelationshipTypeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("RTN"))
                                {
                                    this.RelationshipTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FEId"))
                                {
                                    this.FromEntityId = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("REId"))
                                {
                                    this.RelatedEntityId = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("RPId"))
                                {
                                    this.RelationshipParentId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.RelationshipParentId);
                                }

                                if (reader.MoveToAttribute("SF"))
                                {
                                    this.SourceFlag = Utility.GetSourceFlagEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("A"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ContId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ContainerId);
                                }

                                if (reader.MoveToAttribute("ToCatLNP"))
                                {
                                    this.ToCategoryLongNamePath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToContId"))
                                {
                                    this.ToContainerId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ToContN"))
                                {
                                    this.ToContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToETId"))
                                {
                                    this.ToEntityTypeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ToETN"))
                                {
                                    this.ToEntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToExId"))
                                {
                                    this.ToExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ToLN"))
                                {
                                    this.ToLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Lvl"))
                                {
                                    this.Level = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), this.Level);
                                }

                                if (reader.MoveToAttribute("IM"))
                                {
                                    InheritanceMode inheritanceMode = InheritanceMode.Direct;
                                    Enum.TryParse<InheritanceMode>(reader.ReadContentAsString(), out inheritanceMode);
                                    this.InheritanceMode = inheritanceMode;
                                }

                                if (reader.MoveToAttribute("S"))
                                {
                                    RelationshipStatus status = RelationshipStatus.Active;
                                    Enum.TryParse<RelationshipStatus>(reader.ReadContentAsString(), out status);
                                    this.Status = status;
                                }

                                if (reader.MoveToAttribute("PS"))
                                {
                                    this.PermissionSet = Utility.GetPermissionsAsObject(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SourceId"))
                                {
                                    SourceInfo sourceInfo = new SourceInfo();
                                    sourceInfo.SourceId = reader.ReadContentAsInt();
                                    if (reader.MoveToAttribute("SourceEntityId"))
                                    {
                                        sourceInfo.SourceEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                    }
                                    this.SourceInfo = sourceInfo;
                                }

                                reader.Read();
                            }

                            #endregion Read relationship properties
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                        {
                            #region Read attributes

                            String relatedEntityXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(relatedEntityXml))
                            {
                                this.RelatedEntity = new Entity(relatedEntityXml, ObjectSerialization.DataTransfer);
                            }

                            #endregion Read attributes
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
                            #region Read attributes

                            String attributeXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(attributeXml))
                            {
                                AttributeCollection attributeCollection = new AttributeCollection(attributeXml, false, ObjectSerialization.DataTransfer);
                                if (attributeCollection != null)
                                {
                                    // Based on the serialization type the unique 
                                    foreach (Attribute attr in attributeCollection)
                                    {
                                        if (!this.RelationshipAttributes.Contains(attr.Name, attr.AttributeParentName, attr.Locale))
                                            this.RelationshipAttributes.Add(attr);
                                    }
                                }
                            }

                            #endregion Read attributes
                        }

                        #region Read child relationships

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                        {
                            this.RelationshipCollection = new RelationshipCollection(reader.ReadOuterXml(), ObjectSerialization.DataTransfer);
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read child relationships
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

        /// <summary>
        /// Loads properties of Relationship object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>	
        private void LoadRelationshipMetadataFromXml(XmlTextReader reader)
        {
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

            if (reader.MoveToAttribute("Locale"))
            {
                String strLocale = reader.ReadContentAsString();
                LocaleEnum locale = LocaleEnum.UnKnown;
                Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                if (locale == LocaleEnum.UnKnown)
                    throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");

                this.Locale = locale;
            }

            if (reader.MoveToAttribute("ContainerId"))
            {
                this._containerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ContainerId);
            }

            if (reader.MoveToAttribute("Type"))
            {
                String type = reader.ReadContentAsString();
                this.Type = new RelationshipType(-1, type, type);
            }

            if (reader.MoveToAttribute("RelationshipExternalId"))
            {
                this._relationshipExternalId = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ReferenceId"))
            {
                this.ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ReferenceId);
            }

            if (reader.MoveToAttribute("ToCategoryPath"))
            {
                this._toCategoryPath = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ToContainerId"))
            {
                this._toContainerId = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("ToContainerName"))
            {
                this._toContainerName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ToEntityTypeId"))
            {
                this._toEntityTypeId = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("ToEntityTypeName"))
            {
                this._toEntityTypeName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ToExternalId"))
            {
                this._toExternalId = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ToLongName"))
            {
                this._toLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("RelationshipTypeId"))
            {
                this._relationshipTypeId = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("RelationshipTypeName"))
            {
                this._relationshipTypeName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("RelatedEntityId"))
            {
                this.RelatedEntityId = reader.ReadContentAsLong();
            }

            if (reader.MoveToAttribute("FromEntityId"))
            {
                this._fromEntityId = reader.ReadContentAsLong();
            }

            if (reader.MoveToAttribute("Direction"))
            {
                RelationshipDirection relDirection = RelationshipDirection.None;
                String strRelDirection = reader.ReadContentAsString();

                if (!String.IsNullOrWhiteSpace(strRelDirection))
                    Enum.TryParse<RelationshipDirection>(strRelDirection, out relDirection);

                this.Direction = relDirection;
            }

            if (reader.MoveToAttribute("Path"))
            {
                this.Path = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("SourceFlag"))
            {
                this._sourceFlag = Utility.GetSourceFlagEnum(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("Action"))
            {
                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("Level"))
            {
                this._level = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), this.Level);
            }

            if (reader.MoveToAttribute("InheritanceMode"))
            {
                InheritanceMode inheritanceMode = InheritanceMode.Direct;
                String relationshipInheritanceMode = reader.ReadContentAsString();

                if (!String.IsNullOrWhiteSpace(relationshipInheritanceMode))
                    Enum.TryParse<InheritanceMode>(relationshipInheritanceMode, out inheritanceMode);

                this._inheritanceMode = inheritanceMode;
            }

            if (reader.MoveToAttribute("RelationshipSourceEntityName"))
            {
                this._relationshipSourceEntityName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("RelationshipSourceEntityLongName"))
            {
                this._relationshipSourceEntityLongName = reader.ReadContentAsString();
            }
            if (reader.MoveToAttribute("SourceId"))
            {
                SourceInfo sourceInfo = new SourceInfo();
                sourceInfo.SourceId = reader.ReadContentAsInt();
                if (reader.MoveToAttribute("SourceEntityId"))
                {
                    sourceInfo.SourceEntityId = reader.ReadContentAsLong();
                }
                this._sourceInfo = sourceInfo;
            }

            if (reader.MoveToAttribute("CrossReferenceId"))
            {
                this.CrossReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
            }

            if (reader.MoveToAttribute("FromCrossReferenceId"))
            {
                this.FromCrossReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
            }

            if (reader.MoveToAttribute("RelatedCrossReferenceId"))
            {
                this.RelatedCrossReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
            }

        }

        /// <summary>
        /// Converts properties (metadata) of Relationship object into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of Relationship metadata</param>
        private void ConvertRelationshipMetadataToXml(XmlTextWriter xmlWriter)
        {
            #region Write relationship properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);

            String type = String.Empty;
            if (Type != null)
                type = Type.LongName;

            xmlWriter.WriteAttributeString("Type", type);
            xmlWriter.WriteAttributeString("RelationshipTypeId", this._relationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeName", this._relationshipTypeName);
            xmlWriter.WriteAttributeString("FromEntityId", this._fromEntityId.ToString());
            xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
            xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
            xmlWriter.WriteAttributeString("Path", this.Path);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this._sourceFlag));
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this._containerId.ToString());
            xmlWriter.WriteAttributeString("RelationshipExternalId", this._relationshipExternalId);
            xmlWriter.WriteAttributeString("ToCategoryPath", this._toCategoryPath);
            xmlWriter.WriteAttributeString("ToContainerId", this._toContainerId.ToString());
            xmlWriter.WriteAttributeString("ToContainerName", this._toContainerName);
            xmlWriter.WriteAttributeString("ToEntityTypeId", this._toEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToEntityTypeName", this._toEntityTypeName);
            xmlWriter.WriteAttributeString("ToExternalId", this._toExternalId);
            xmlWriter.WriteAttributeString("ToLongName", this._toLongName);
            xmlWriter.WriteAttributeString("RelationshipParentId", this._relationshipParentId.ToString());
            xmlWriter.WriteAttributeString("RelationshipSourceId", this._relationshipSourceId.ToString());
            xmlWriter.WriteAttributeString("RelationshipSourceEntityId", this._relationshipSourceEntityId.ToString());
            xmlWriter.WriteAttributeString("RelationshipSourceEntityName", this._relationshipSourceEntityName);
            xmlWriter.WriteAttributeString("RelationshipSourceEntityLongName", this._relationshipSourceEntityLongName);
            xmlWriter.WriteAttributeString("RelationshipParentEntityId", this._relationshipParentEntityId.ToString());
            xmlWriter.WriteAttributeString("Level", this._level.ToString());
            xmlWriter.WriteAttributeString("InheritanceMode", this._inheritanceMode.ToString());

            if (SourceInfo != null)
            {
                xmlWriter.WriteAttributeString("SourceId", this._sourceInfo.SourceId.ToString());
                xmlWriter.WriteAttributeString("SourceEntityId", this._sourceInfo.SourceEntityId.ToString());
            }

            xmlWriter.WriteAttributeString("CrossReferenceId", this.CrossReferenceId.ToString());
            xmlWriter.WriteAttributeString("FromCrossReferenceId", this.FromCrossReferenceId.ToString());
            xmlWriter.WriteAttributeString("RelatedCrossReferenceId", this.RelatedCrossReferenceId.ToString());

            #endregion Write relationship properties
        }

        #endregion

        #endregion
    }
}