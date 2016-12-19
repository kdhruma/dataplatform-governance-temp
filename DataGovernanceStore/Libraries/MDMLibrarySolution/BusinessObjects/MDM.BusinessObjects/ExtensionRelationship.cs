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
    /// Specifies extension relationship(MDL) object 
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class ExtensionRelationship : RelationshipBase, IExtensionRelationship
    {
        #region Field

        /// <summary>
        /// Field denoting the container id of the extended entity
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting the container name of the extended entity
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field denoting the container long name of the extended entity
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field denoting the type of the Container 
        /// </summary>
        private ContainerType _containerType = ContainerType.Unknown;

        /// <summary>
        /// Field denoting the category ID of the extended entity
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting the category name of the extended entity
        /// </summary>
        private String _categoryName = String.Empty;

        /// <summary>
        /// Field denoting the category long name of the extended entity
        /// </summary>
        private String _categoryLongName = String.Empty;

        /// <summary>
        /// Field denoting the external id of the extended entity
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field denoting the category path of the of the extended entity
        /// </summary>
        private String _categoryPath = String.Empty;

        /// <summary>
        /// Field denoting the category ln path of the of the extended entity
        /// </summary>
        private String _categoryLongNamePath = String.Empty;

        /// <summary>
        /// Field denoting the parent extension entity id
        /// </summary>
        private Int64 _parentExtensionEntityId = 0;

        /// <summary>
        /// Field denoting extension relationships
        /// </summary>
        private ExtensionRelationshipCollection _relationshipCollection = new ExtensionRelationshipCollection();

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the container id of the extended entity
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32 ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                this._containerId = value;
            }
        }

        /// <summary>
        /// Property denoting the container name of the extended entity
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String ContainerName
        {
            get
            {
                return this._containerName;
            }
            set
            {
                this._containerName = value;
            }
        }

        /// <summary>
        /// Property denoting the container long name of the extended entity
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public String ContainerLongName
        {
            get
            {
                return this._containerLongName;
            }
            set
            {
                this._containerLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the type of the Container 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public ContainerType ContainerType
        {
            get
            {
                return _containerType;
            }
            set
            {
                _containerType = value;
            }
        }

        /// <summary>
        /// Property denoting the category id of the extended entity
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Int64 CategoryId
        {
            get
            {
                return this._categoryId;
            }
            set
            {
                this._categoryId = value;
            }
        }

        /// <summary>
        /// Property denoting the category name of the extended entity
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }

        /// <summary>
        /// Property denoting the category long name of the extended entity
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public String CategoryLongName
        {
            get
            {
                return _categoryLongName;
            }
            set
            {
                _categoryLongName = value;
            }
        }

        /// <summary>
        /// Property denoty the category path of the extended entity.
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public String CategoryPath
        {
            get
            {
                return _categoryPath;
            }
            set
            {
                _categoryPath = value;
            }
        }

        /// <summary>
        /// Property denotes the category longname path of the extended entity.
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public String CategoryLongNamePath
        {
            get
            {
                return _categoryLongNamePath;
            }
            set
            {
                _categoryLongNamePath = value;
            }
        }

        /// <summary>
        /// Property denoty the external id of the extended entity.
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        /// <summary>
        /// Property denoting the extension relationships of the related entity
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public new ExtensionRelationshipCollection RelationshipCollection
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
        /// Property denoting the parent extension entity id
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public Int64 ParentExtensionEntityId
        {
            get
            {
                return this._parentExtensionEntityId;
            }
            set
            {
                this._parentExtensionEntityId = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the extension relationship class
        /// </summary>
        public ExtensionRelationship()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the extension relationship class with the provided parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        /// <param name="type">Indicates the relationship type</param>
        /// <param name="relatedEntityId">Indicates the related entity id</param>
        /// <param name="containerId">Indicates the Id of the container to which the item has been extended</param>
        /// <param name="containerName">Indicates name of the container to which the item has been extended</param>
        /// <param name="containerLongName">Indicates long name of the container to which the item has been extended</param>
        /// <param name="categoryId">Indicates id of the category to which the item has been extended</param>
        /// <param name="categoryName">Indicates name of the category to which the item has been extended</param>
        /// <param name="categoryLongName">Indicates long name of the category to which the item has been extended</param>
        /// <param name="direction">Indicates the direction</param>
        /// <param name="path">Indicates the path of the relationship</param>
        public ExtensionRelationship(Int32 id, String type, Int64 relatedEntityId, Int32 containerId, String containerName, String containerLongName, Int64 categoryId, String categoryName, String categoryLongName, RelationshipDirection direction, String path)
            : base(id, type, relatedEntityId, direction, path)
        {
            this.ContainerId = containerId;
            this.ContainerName = containerName;
            this.ContainerLongName = containerLongName;
            this.CategoryId = categoryId;
            this.CategoryName = categoryName;
            this.CategoryLongName = categoryLongName;
        }

        /// <summary>
        /// Initializes a new instance of the extension relationship class with the provided parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        /// <param name="type">Indicates the relationship type</param>
        /// <param name="relatedEntityId">Indicates the related entity id</param>
        /// <param name="containerId">Indicates the Id of the container to which the item has been extended</param>
        /// <param name="containerName">Indicates name of the container to which the item has been extended</param>
        /// <param name="containerLongName">Indicates long name of the container to which the item has been extended</param>
        /// <param name="categoryId">Indicates id of the category to which the item has been extended</param>
        /// <param name="categoryName">Indicates name of the category to which the item has been extended</param>
        /// <param name="categoryLongName">Indicates long name of the category to which the item has been extended</param>
        /// <param name="direction">Indicates the direction</param>
        /// <param name="path">Indicates the path of the relationship</param>
        /// <param name="categoryPath">Indicates the category path of the relationship</param>
        /// <param name="categoryLongNamePath">Indicates the category long name path of the relationship</param>
        public ExtensionRelationship(Int32 id, String type, Int64 relatedEntityId, Int32 containerId, String containerName, String containerLongName, Int64 categoryId, String categoryName, String categoryLongName, RelationshipDirection direction, String path, String categoryPath, String categoryLongNamePath)
            : base(id, type, relatedEntityId, direction, path)
        {
            this.ContainerId = containerId;
            this.ContainerName = containerName;
            this.ContainerLongName = containerLongName;
            this.CategoryId = categoryId;
            this.CategoryName = categoryName;
            this.CategoryLongName = categoryLongName;
            this.CategoryPath = categoryPath;
            this.CategoryLongNamePath = categoryLongNamePath;
        }

        /// <summary>
        /// Initializes a new instance of the extension relationship class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="ExtensionRelationship" Type="[Entity]" RelatedEntityId="[Parent]" ContainerId="" ContainerName="" ContainerLongName="" CategoryId="" CategoryName="" CategoryLongName="" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        public ExtensionRelationship(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadExtensionRelationshipDetails(valuesAsXml, objectSerialization);
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
                    ExtensionRelationship objectToBeCompared = obj as ExtensionRelationship;

                    if (this.ContainerId != objectToBeCompared.ContainerId)
                        return false;

                    if (this.ContainerName != objectToBeCompared.ContainerName)
                        return false;

                    if (this.ContainerLongName != objectToBeCompared.ContainerLongName)
                        return false;

                    if (this.ContainerType != objectToBeCompared.ContainerType)
                        return false;

                    if (this.CategoryId != objectToBeCompared.CategoryId)
                        return false;

                    if (this.CategoryName != objectToBeCompared.CategoryName)
                        return false;

                    if (this.CategoryLongName != objectToBeCompared.CategoryLongName)
                        return false;

                    if (this.CategoryPath != objectToBeCompared.CategoryPath)
                        return false;

                    if (this.CategoryLongNamePath != objectToBeCompared.CategoryLongNamePath)
                        return false;

                    if (this.ExternalId != objectToBeCompared.ExternalId)
                        return false;

                    if (this.ParentExtensionEntityId != objectToBeCompared.ParentExtensionEntityId)
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
            hashCode = base.GetHashCode() ^ this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^ this.ContainerLongName.GetHashCode()
                       ^ this.ContainerType.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.CategoryName.GetHashCode() ^ this.CategoryLongName.GetHashCode()
                       ^ this.CategoryPath.GetHashCode() ^ this.ExternalId.GetHashCode() ^ this.RelationshipCollection.GetHashCode()
                       ^ this.CategoryLongNamePath.GetHashCode() ^ this.ParentExtensionEntityId.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Extension Relationship object
        /// </summary>
        /// <returns>Xml representation of Extension Relationship object</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Relationship node start
            xmlWriter.WriteStartElement("Relationship");

            ConvertExtensionMetadataToXml(xmlWriter);

            #region Write Relationships

            if (this.RelationshipCollection != null)
            {
                String childRelationshipsXml = "<Relationships>";

                foreach (ExtensionRelationship extensionRelationship in this.RelationshipCollection)
                {
                    childRelationshipsXml = String.Concat(childRelationshipsXml, extensionRelationship.ToXml());
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
        /// Get Xml representation of Extension Relationship based on requested object serialization
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
                    xmlWriter.WriteAttributeString("RelatedEnId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("ContId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContName", this.ContainerName.ToString());
                    xmlWriter.WriteAttributeString("ContLName", this.ContainerLongName.ToString());
                    xmlWriter.WriteAttributeString("ContType", this.ContainerType.ToString());
                    xmlWriter.WriteAttributeString("CatId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("CatName", this.CategoryName.ToString());
                    xmlWriter.WriteAttributeString("CatLName", this.CategoryLongName.ToString());
                    xmlWriter.WriteAttributeString("CatPath", this.CategoryPath.ToString());
                    xmlWriter.WriteAttributeString("CatLNamePath", this.CategoryLongNamePath.ToString());
                    xmlWriter.WriteAttributeString("ExtnlId", this.ExternalId.ToString());
                    xmlWriter.WriteAttributeString("PExtEntityId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion Write relationship properties
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
                    xmlWriter.WriteAttributeString("RelEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContainerName", this.ContainerName.ToString());
                    xmlWriter.WriteAttributeString("ContainerLN", this.ContainerLongName.ToString());
                    xmlWriter.WriteAttributeString("CatId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("CatName", this.CategoryName.ToString());
                    xmlWriter.WriteAttributeString("CategoryLN", this.CategoryLongName.ToString());
                    xmlWriter.WriteAttributeString("CatPath", this.CategoryPath.ToString());
                    xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
                    xmlWriter.WriteAttributeString("PExtEntityId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("Dir", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("CategoryLongNamePath", this.CategoryLongNamePath);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }
                else if (objectSerialization == ObjectSerialization.External)
                {
                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("ObjectType", this.ObjectType);

                    String type = String.Empty;
                    if (Type != null)
                        type = Type.LongName;

                    xmlWriter.WriteAttributeString("Type", type);
                    xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContainerName", this.ContainerName.ToString());
                    xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName.ToString());
                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("CategoryName", this.CategoryName.ToString());
                    xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName.ToString());
                    xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath.ToString());
                    xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("CategoryLongNamePath", this.CategoryLongNamePath);
                    xmlWriter.WriteAttributeString("Action", ValueTypeHelper.GetActionString(this.Action));

                    #endregion
                }
                else //TODO::Add for ObjectSerialization.DataTransfer
                {
                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name.ToString());
                    xmlWriter.WriteAttributeString("ObjectType", this.ObjectType);

                    String type = String.Empty;
                    if (Type != null)
                        type = Type.LongName;

                    xmlWriter.WriteAttributeString("Type", type);
                    xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContainerName", this.ContainerName.ToString());
                    xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName.ToString());
                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("CategoryName", this.CategoryName.ToString());
                    xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName.ToString());
                    xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath.ToString());
                    xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
                    xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("CategoryLongNamePath", this.CategoryLongNamePath);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }

                #region Write Relationships

                if (this.RelationshipCollection != null)
                {
                    String childRelationshipsXml = "<Relationships>";

                    foreach (ExtensionRelationship extensionRelationship in this.RelationshipCollection)
                    {
                        childRelationshipsXml = String.Concat(childRelationshipsXml, extensionRelationship.ToXml(objectSerialization));
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
        /// Converts Extension object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml representation of Extension object</param>
        internal void ConvertExtensionRelationshipToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                // Extension node start
                xmlWriter.WriteStartElement("Relationship");

                ConvertExtensionMetadataToXml(xmlWriter);

                #region Write Relationships

                if (this.RelationshipCollection != null)
                {
                    this.RelationshipCollection.ConvertExtensionRelationshipCollectionToXml(xmlWriter);
                }

                #endregion Write Relationships

                // Extension node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write ExtensionRelationship object.");
            }
        }

        /// <summary>
        /// Loads ExtensionRelationship object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadExtensionRelationshipFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.IsStartElement())
                {
                    if (reader.HasAttributes)
                    {
                        LoadExtensionRelationshipMetadataFromXml(reader);

                        reader.Read();
                    }
                }

                #region Read child extension relationships

                if (reader.ReadToFollowing("Relationships"))
                {
                    this._relationshipCollection.LoadExtensionRelationshipCollectionFromXml(reader);
                }

                #endregion Read child extension relationships
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read ExtensionRelationship object.");
            }
        }

        /// <summary>
        /// Gets extension relationships
        /// </summary>
        /// <returns>Extension Relationship collection interface</returns>
        public new IExtensionRelationshipCollection GetRelationships()
        {
            return (IExtensionRelationshipCollection)this.RelationshipCollection;
        }

        /// <summary>
        /// Sets extension relationships
        /// </summary>
        /// <param name="iExtensionRelationshipCollection">Extension Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed extension relationship collection is null</exception>
        public void SetRelationships(IExtensionRelationshipCollection iExtensionRelationshipCollection)
        {
            if (iExtensionRelationshipCollection == null)
                throw new ArgumentNullException("ExtensionRelationshipCollection");

            this.RelationshipCollection = (ExtensionRelationshipCollection)iExtensionRelationshipCollection;
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
        /// Clone ExtensionRelationship object
        /// </summary>
        /// <param name="cloneRelatedEntity">Indicates whether to clone related entity or not</param>
        /// <returns>Returns cloned copy of ExtensioRelationship</returns>
        public ExtensionRelationship Clone(Boolean cloneRelatedEntity = true)
        {
            ExtensionRelationship extensionRelationship = CloneBasicProperties();

            if (this._relationshipCollection != null && this._relationshipCollection.Count > 0)
            {
                ExtensionRelationshipCollection clonedChildExtensionRelationships = new ExtensionRelationshipCollection();

                foreach (ExtensionRelationship childExtensionRelationship in this._relationshipCollection)
                {
                    ExtensionRelationship clonedChildRel = childExtensionRelationship.Clone(); // Recurse method
                    clonedChildExtensionRelationships.Add(clonedChildRel);
                }

                extensionRelationship._relationshipCollection = clonedChildExtensionRelationships;
            }

            if (this.RelatedEntity != null)
            {
                extensionRelationship.RelatedEntity = cloneRelatedEntity && this.RelatedEntity != null ? this.RelatedEntity.Clone() : null;
            }

            return extensionRelationship;
        }

        /// <summary>
        /// Compare 2 extension relationship using get supersetoperationresult need discussion on which properpies are needed to compare. For now comparing path, direction, category
        /// </summary>
        /// <param name="subsetExtensionRelationship"></param>
        /// <param name="extensionRelationshipOperationResult"></param>
        /// <param name="compareById"></param>
        /// <returns></returns>
        public Boolean GetSuperSetOperationResult(ExtensionRelationship subsetExtensionRelationship, OperationResult extensionRelationshipOperationResult, Boolean compareById = false)
        {
            if (subsetExtensionRelationship != null)
            {
                #region compare by Ids

                if (compareById)
                {
                    Utility.BusinessObjectPropertyCompare("RelatedEntityId", RelatedEntityId, subsetExtensionRelationship.RelatedEntityId, extensionRelationshipOperationResult);

                    //Extended property
                    Utility.BusinessObjectPropertyCompare("ExtensionRelationship ContainerId", this.ContainerId, subsetExtensionRelationship.ContainerId, extensionRelationshipOperationResult);
                }

                #endregion compare by Ids

                #region compare base properties

                Utility.BusinessObjectPropertyCompare("ExtensionRelationship Name", this.Name, subsetExtensionRelationship.Name, extensionRelationshipOperationResult);
                Utility.BusinessObjectPropertyCompare("ExtensionRelationshipLongName", this.LongName, subsetExtensionRelationship.LongName, extensionRelationshipOperationResult);
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship Locale", this.LongName, subsetExtensionRelationship.LongName, extensionRelationshipOperationResult);

                #endregion compare base properties

                #region extended properties

                //if (String.Compare (this.GetRelationshipType().Name, subsetExtensionRelationship.GetRelationshipType().Name) != 0)
                //{
                //    extensionRelationshipOperationResult.AddOperationResult("-1", "ExtensionRelationship Type mismatch", OperationResultType.Error);
                //}
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship Type", this.GetRelationshipType().Name, subsetExtensionRelationship.GetRelationshipType().Name, extensionRelationshipOperationResult);
                //Utility.BusinessObjectPropertyCompare("ExtensionRelationship Path", this.Path, subsetExtensionRelationship.Path, extensionRelationshipOperationResult);
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship Direction", this.Direction, subsetExtensionRelationship.Direction, extensionRelationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("ExtensionRelationship CategoryName", this._categoryName, subsetExtensionRelationship._categoryName, extensionRelationshipOperationResult);
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship CategoryLongName", this.CategoryLongName, subsetExtensionRelationship.CategoryLongName, extensionRelationshipOperationResult);
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship CategoryLongNamePath", this.CategoryLongNamePath, subsetExtensionRelationship.CategoryLongNamePath, extensionRelationshipOperationResult);
                
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship ContainerName", this.ContainerName, subsetExtensionRelationship.ContainerName, extensionRelationshipOperationResult);
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship ContainerLongName", this.ContainerLongName, subsetExtensionRelationship.ContainerLongName, extensionRelationshipOperationResult);
                Utility.BusinessObjectPropertyCompare("ExtensionRelationship ContainerType", this.ContainerType, subsetExtensionRelationship.ContainerType, extensionRelationshipOperationResult);

                #endregion extended properties

                #region compare extensionrelationship collection

                this.RelationshipCollection.GetSuperSetOperationResult(subsetExtensionRelationship.RelationshipCollection, extensionRelationshipOperationResult, compareById);

                #endregion compare extensionrelationship collection

                #region compare relatedEntityId

                if (subsetExtensionRelationship.RelatedEntity != null)
                {
                    if (this.RelatedEntity != null)
                    {
                        var relatedEntityOperationResult = this.RelatedEntity.GetSuperSetOperationResult(subsetExtensionRelationship.RelatedEntity);

                        foreach (var error in relatedEntityOperationResult.GetAllErrors())
                        {
                            extensionRelationshipOperationResult.Errors.Add(error);
                        }
                    }
                    else
                    {
                        extensionRelationshipOperationResult.AddOperationResult("-1", "RelatedEntity of superset ExtensionRelationship is null - make sure entity has xtensionrelationship and entityConext.LoadExtensionRelationship = true", OperationResultType.Error);
                    }
                }
                else
                {
                    extensionRelationshipOperationResult.AddOperationResult("-1", "RelatedEntity of subsetExtensionRelationship is null", OperationResultType.Information);
                }

                #endregion compare relatedEntityId

                #region refresh and return

                extensionRelationshipOperationResult.RefreshOperationResultStatus();
                if (extensionRelationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                #endregion refresh and return
            }
            else
            {
                extensionRelationshipOperationResult.AddOperationResult("-1", "subsetextensionRelationship is null - nothing to compare to - check input xml", OperationResultType.Error);
                return false;
            }
            
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Loads properties of ExtensionRelationship object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        private void LoadExtensionRelationshipMetadataFromXml(XmlTextReader reader)
        {
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

                if (reader.MoveToAttribute("Type"))
                {
                    String type = reader.ReadContentAsString();

                    this.Type = new RelationshipType(-1, type, type);
                }

                if (reader.MoveToAttribute("RelatedEntityId"))
                {
                    this.RelatedEntityId = reader.ReadContentAsLong();
                }

                if (reader.MoveToAttribute("ContainerId"))
                {
                    this._containerId = reader.ReadContentAsInt();
                }

                if (reader.MoveToAttribute("ContainerName"))
                {
                    this._containerName = reader.ReadContentAsString();
                }

                if (reader.MoveToAttribute("ContainerLongName"))
                {
                    this._containerLongName = reader.ReadContentAsString();
                }

                if (reader.MoveToAttribute("ContainerType"))
                {
                    ContainerType containerType = ContainerType.Unknown;
                    String strContainerType = reader.ReadContentAsString();

                    if (!String.IsNullOrWhiteSpace(strContainerType))
                        Enum.TryParse<ContainerType>(strContainerType, out containerType);

                    this._containerType = containerType;
                }

                if (reader.MoveToAttribute("CategoryId"))
                {
                    this._categoryId = reader.ReadContentAsInt();
                }

                if (reader.MoveToAttribute("CategoryName"))
                {
                    this._categoryName = reader.ReadContentAsString();
                }

                if (reader.MoveToAttribute("CategoryLongName"))
                {
                    this._categoryLongName = reader.ReadContentAsString();
                }

                if (reader.MoveToAttribute("CategoryPath"))
                {
                    this._categoryPath = reader.ReadContentAsString();
                }

                if (reader.MoveToAttribute("CategoryLongNamePath"))
                {
                    this._categoryLongNamePath = reader.ReadContentAsString();
                }

                if (reader.MoveToAttribute("ExternalId"))
                {
                    this._externalId = reader.ReadContentAsString();
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

                if (reader.MoveToAttribute("Action"))
                {
                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                }

                if (reader.MoveToAttribute("ParentExtensionEntityId"))
                {
                    this._parentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._parentExtensionEntityId);
                }
            }
        }

        /// <summary>
        /// Loads the extension relationship with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="ExtensionRelationship" Type="[Entity]" RelatedEntityId="[Parent]" ContainerId="" ContainerName="" ContainerLongName="" CategoryId="" CategoryName="" CategoryLongName="" CategoryPath="" ExternalId="" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        private void LoadExtensionRelationshipDetails(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadExtensionRelationshipDetailsForDataStorage(valuesAsXml);
                }
                else
                {
                    XmlTextReader reader = null;

                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                #region Read Extension relationship properties

                                if (reader.HasAttributes)
                                {
                                    LoadExtensionRelationshipMetadataFromXml(reader);
                                }

                                #endregion Read Hierarchy relationship properties
                            }

                            #region Read child extension relationships

                            if (reader.ReadToFollowing("Relationships"))
                            {
                                this.RelationshipCollection = new ExtensionRelationshipCollection(reader.ReadOuterXml());
                            }

                            #endregion Read child extension relationships
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
        /// Loads the extension relationship with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="ExtensionRelationship" Type="[Entity]" RelatedEnId="[Parent]" ContId="" ContName="" ContLName="" CatId="" CatName="" CatLName="" CatPath="" ExtnId="" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        private void LoadExtensionRelationshipDetailsForDataStorage(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            #region Read Extension relationship properties

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

                                if (reader.MoveToAttribute("Type"))
                                {
                                    String type = reader.ReadContentAsString();

                                    this.Type = new RelationshipType(-1, type, type);
                                }

                                if (reader.MoveToAttribute("RelatedEnId"))
                                {
                                    this.RelatedEntityId = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("ContId"))
                                {
                                    this.ContainerId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ContName"))
                                {
                                    this.ContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContLName"))
                                {
                                    this.ContainerLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContType"))
                                {
                                    ContainerType containerType = ContainerType.Unknown;
                                    String strContainerType = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(strContainerType))
                                        Enum.TryParse<ContainerType>(strContainerType, out containerType);

                                    this.ContainerType = containerType;
                                }

                                if (reader.MoveToAttribute("CatId"))
                                {
                                    this.CategoryId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("CatName"))
                                {
                                    this.CategoryName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatLName"))
                                {
                                    this.CategoryLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatPath"))
                                {
                                    this.CategoryPath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatLNamePath"))
                                {
                                    this.CategoryLongNamePath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ExtnId"))
                                {
                                    this.ExternalId = reader.ReadContentAsString();
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

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("PExtEntityId"))
                                {
                                    this.ParentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), _parentExtensionEntityId);
                                }
                            }

                            #endregion Read Hierarchy relationship properties
                        }

                        #region Read child extension relationships

                        if (reader.ReadToFollowing("Relationships"))
                        {
                            this.RelationshipCollection = new ExtensionRelationshipCollection(reader.ReadOuterXml(), ObjectSerialization.DataStorage);
                        }

                        #endregion Read child extension relationships
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
        /// Clone ExtensionRelationship object
        /// </summary>
        /// <returns>Returns cloned copy of ExtensionRelationship</returns>
        private ExtensionRelationship CloneBasicProperties()
        {
            ExtensionRelationship extensionRelationship = new ExtensionRelationship();

            extensionRelationship.Id = this.Id;
            extensionRelationship.Name = this.Name;
            extensionRelationship.LongName = this.LongName;
            extensionRelationship.Action = this.Action;
            extensionRelationship.AuditRefId = this.AuditRefId;
            extensionRelationship.Locale = this.Locale;
            extensionRelationship.UserName = this.UserName;

            extensionRelationship.Direction = this.Direction;
            extensionRelationship.Path = this.Path;
            extensionRelationship.RelatedEntityId = this.RelatedEntityId;
            extensionRelationship.Type = this.Type;

            extensionRelationship._containerId = this.ContainerId;
            extensionRelationship._containerName = this.ContainerName;
            extensionRelationship._containerLongName = this.ContainerLongName;
            extensionRelationship._containerType = this.ContainerType;
            extensionRelationship._parentExtensionEntityId = this.ParentExtensionEntityId;
            extensionRelationship._externalId = this.ExternalId;
            extensionRelationship._categoryId = this.CategoryId;
            extensionRelationship._categoryName = this.CategoryName;
            extensionRelationship._categoryLongName = this.CategoryLongName;
            extensionRelationship._categoryLongNamePath = this.CategoryLongNamePath;
            extensionRelationship._categoryPath = this.CategoryPath;

            return extensionRelationship;
        }

        /// <summary>
        /// Converts properties (metadata) of Extension object into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of Extension metadata</param>
        private void ConvertExtensionMetadataToXml(XmlTextWriter xmlWriter)
        {
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
            xmlWriter.WriteAttributeString("ContainerId", this._containerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this._containerName.ToString());
            xmlWriter.WriteAttributeString("ContainerLongName", this._containerLongName.ToString());
            xmlWriter.WriteAttributeString("ContainerType", this._containerType.ToString());
            xmlWriter.WriteAttributeString("CategoryId", this._categoryId.ToString());
            xmlWriter.WriteAttributeString("CategoryName", this._categoryName.ToString());
            xmlWriter.WriteAttributeString("CategoryLongName", this._categoryLongName.ToString());
            xmlWriter.WriteAttributeString("CategoryPath", this._categoryPath.ToString());
            xmlWriter.WriteAttributeString("CategoryLongNamePath", this._categoryLongNamePath.ToString());
            xmlWriter.WriteAttributeString("ExternalId", this._externalId.ToString());
            xmlWriter.WriteAttributeString("ParentExtensionEntityId", this._parentExtensionEntityId.ToString());
            xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
            xmlWriter.WriteAttributeString("Path", this.Path);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
        }

        #endregion

        #endregion
    }
}