using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Relationship
    /// </summary>
    [DataContract]
    [ProtoContract]
    [ProtoInclude(ProtoBufConstants.RELATIONSHIPBASE + 1, typeof(Relationship))]
    [ProtoInclude(ProtoBufConstants.RELATIONSHIPBASE + 2, typeof(ExtensionRelationship))]
    [ProtoInclude(ProtoBufConstants.RELATIONSHIPBASE + 3, typeof(HierarchyRelationship))]
    public class RelationshipBase : MDMObject, IRelationshipBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the relationship type
        /// </summary>
        private RelationshipType _type = new RelationshipType();

        /// <summary>
        /// Field denoting the path of the relationship object
        /// </summary>
        private String _path = String.Empty;

        /// <summary>
        /// Field denoting the direction of the relationship
        /// </summary>
        private RelationshipDirection _direction = RelationshipDirection.None;

        /// <summary>
        /// Field denoting the id of the related entity
        /// </summary>
        private Int64 _relatedEntityId = 0;

        /// <summary>
        /// Field denoting the related entity object
        /// </summary>
        private Entity _relatedEntity = null;

        /// <summary>
        /// Field denoting the relationships of the related entity
        /// </summary>
        private RelationshipBaseCollection _relationshipCollection = new RelationshipBaseCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the relationship class
        /// </summary>
        public RelationshipBase()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the relationship class with the provided Id
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        public RelationshipBase(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the relationship class with the provided Id, Name and LongName
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        /// <param name="name">Indicates the Name of a Relationship</param>
        /// <param name="longName">Indicates the Description of a Relationship</param>
        public RelationshipBase(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the relationship class with the provided Id, Name, LongName and Locale
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        /// <param name="name">Indicates the Name of a Relationship</param>
        /// <param name="longName">Indicates the LongName of a Relationship</param>
        /// <param name="locale">Indicates the Locale of a Relationship</param>
        public RelationshipBase(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {

        }

        /// <summary>
        /// Initializes a new instance of the relationship class with the provided parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        /// <param name="type">Indicates the relationship type</param>
        /// <param name="relatedEntityId">Indicates the related entity id</param>
        /// <param name="direction">Indicates the direction</param>
        /// <param name="path">Indicates the path of the relationship</param>
        public RelationshipBase(Int32 id, String type, Int64 relatedEntityId, RelationshipDirection direction, String path)
            : base(id)
        {
            if (_type == null)
                _type = new RelationshipType();

            _type.Name = type;
            _type.LongName = type;

            _relatedEntityId = relatedEntityId;
            _direction = direction;
            _path = path;
        }

        #endregion

        #region Properties

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
        /// Property denoting the Relationship Type
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public RelationshipType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        /// <summary>
        /// Property denoting the path of the relationship object
        /// </summary>
        [DataMember]
        [System.Xml.Serialization.XmlAttribute()]
        [ProtoMember(2)]
        public String Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }

        /// <summary>
        /// Property denoting the direction of the relationship
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public RelationshipDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        /// <summary>
        /// Property denoting the id of the related entity
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Int64 RelatedEntityId
        {
            get
            {
                return _relatedEntityId;
            }
            set
            {
                _relatedEntityId = value;
            }
        }

        /// <summary>
        /// Property denoting the related entity object
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Entity RelatedEntity
        {
            get
            {
                return _relatedEntity;
            }
            set
            {
                _relatedEntity = value;
            }
        }

        /// <summary>
        /// Property denoting the relationships of the related entity
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public virtual RelationshipBaseCollection RelationshipCollection
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

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if(base.Equals(obj))
            {
                if (obj is RelationshipBase)
                {
                    RelationshipBase objectToBeCompared = obj as RelationshipBase;

                    if (!this.Type.Equals(objectToBeCompared.Type))
                        return false;

                    if (this.Path != objectToBeCompared.Path)
                        return false;

                    if (this.Direction != objectToBeCompared.Direction)
                        return false;

                    if (this.RelatedEntityId != objectToBeCompared.RelatedEntityId)
                        return false;

                    if (this.RelatedEntity != null)
                    {
                        if (!this.RelatedEntity.Equals(objectToBeCompared.RelatedEntity))
                            return false;
                    }
                    else if (objectToBeCompared.RelatedEntity != null)
                    {
                        return false;
                    }

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
            hashCode = base.GetHashCode() ^ this.Type.GetHashCode() ^ this.Path.GetHashCode() ^ this.Direction.GetHashCode() ^ this.RelatedEntityId.GetHashCode() ^ ((this.RelatedEntity == null)?0:this.RelatedEntity.GetHashCode()) ^ this.RelationshipCollection.GetHashCode();
            return hashCode;
        }

        #region IRelationship Methods

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

            #region Write relationship properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("ObjectTypeId", this.ObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("ObjectType", this.ObjectType);

            String type = String.Empty;
            if (Type != null)
                type = Type.LongName;

            xmlWriter.WriteAttributeString("Type", type);
            xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
            xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
            xmlWriter.WriteAttributeString("Path", this.Path);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            #endregion

            #region Write Relationships

            if (this.RelationshipCollection != null)
                xmlWriter.WriteRaw(this.RelationshipCollection.ToXml());

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

                if (objectSerialization == ObjectSerialization.Compact)
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
                    xmlWriter.WriteAttributeString("RelEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Dir", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

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
                    xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }

                #region Write Relationships

                if (this.RelationshipCollection != null)
                    xmlWriter.WriteRaw(this.RelationshipCollection.ToXml(objectSerialization));

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
        /// Gets the relationship type
        /// </summary>
        /// <returns>Relationship type interface</returns>
        public IRelationshipType GetRelationshipType()
        {
            return (IRelationshipType)this.Type;
        }

        /// <summary>
        /// Sets the relationship type
        /// </summary>
        /// <param name="iRelationshipType">Relationship type interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship type is null</exception>
        public void SetRelationshipType(IRelationshipType iRelationshipType)
        {
            if (iRelationshipType == null)
                throw new ArgumentNullException("RelationshipType");

            this.Type = (RelationshipType)iRelationshipType;
        }

        /// <summary>
        /// Gets relationships
        /// </summary>
        /// <returns>Relationship collection interface</returns>
        public virtual IRelationshipBaseCollection GetRelationships()
        {
            return (IRelationshipBaseCollection)this.RelationshipCollection;
        }

        /// <summary>
        /// Sets relationships
        /// </summary>
        /// <param name="iRelationshipCollection">Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship collection is null</exception>
        public virtual void SetRelationships(IRelationshipBaseCollection iRelationshipCollection)
        {
            if (iRelationshipCollection == null)
                throw new ArgumentNullException("RelationshipCollection");

            this.RelationshipCollection = (RelationshipBaseCollection)iRelationshipCollection;
        }

        /// <summary>
        /// Gets the related entity
        /// </summary>
        /// <returns>Related entity interface</returns>
        public IEntity GetRelatedEntity()
        {
            return (IEntity)this.RelatedEntity;
        }

        /// <summary>
        /// Sets the related entity
        /// </summary>
        /// <param name="iEntity">Related entity interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed entity is null</exception>
        public void SetRelatedEntity(IEntity iEntity)
        {
            if (iEntity == null)
                throw new ArgumentNullException("Entity");

            this.RelatedEntity = (Entity)iEntity;
        }

        #endregion

        #endregion
    }
}
