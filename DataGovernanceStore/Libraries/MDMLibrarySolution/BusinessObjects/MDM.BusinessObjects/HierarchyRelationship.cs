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
    /// Specifies the Hierarchy Relationship
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class HierarchyRelationship : RelationshipBase, IHierarchyRelationship
    {
        #region Fields

        /// <summary>
        /// Field denoting hierarchy relationships
        /// </summary>
        private HierarchyRelationshipCollection _relationshipCollection = new HierarchyRelationshipCollection();

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public override String ObjectType
        {
            get
            {
                return "HierarchyRelationship";
            }
        }

        /// <summary>
        /// Property denoting the hierarchy relationships of the related entity
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public new HierarchyRelationshipCollection RelationshipCollection
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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship class
        /// </summary>
        public HierarchyRelationship()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship class with the provided parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Relationship</param>
        /// <param name="type">Indicates the relationship type</param>
        /// <param name="relatedEntityId">Indicates the related entity id</param>
        /// <param name="direction">Indicates the direction</param>
        /// <param name="path">Indicates the path of the relationship</param>
        public HierarchyRelationship(Int32 id, String type, Int32 relatedEntityId, RelationshipDirection direction, String path)
            : base(id, type, relatedEntityId, direction, path)
        {

        }

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="HierarchyRelationship" Type="[Entity]or[Category]" RelatedEntityId="[Parent]" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        public HierarchyRelationship(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadHierarchyRelationshipDetails(valuesAsXml, objectSerialization);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Loads the hierarchy relationship with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjectType="HierarchyRelationship" Type="[Entity]or[Category]" RelatedEntityId="[Parent]" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        public void LoadHierarchyRelationshipDetails(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadHierarchyRelationshipDetailsForDataStorage(valuesAsXml);
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
                                #region Read Hierarchy relationship properties

                                if (reader.HasAttributes)
                                {
                                    LoadHierarchyRelationshipMetadataFromXml(reader);
                                }

                                #endregion Read Hierarchy relationship properties
                            }

                            #region Read child hierarchy relationships

                            if (reader.ReadToFollowing("Relationships"))
                            {
                                this.RelationshipCollection = new HierarchyRelationshipCollection(reader.ReadOuterXml());
                            }

                            #endregion Read child hierarchy relationships
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
        /// Loads the hierarchy relationship with the XML having values of object when Enum of ObjectSerialization is DataStorage
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Relationship Id="126" ObjType="HierarchyRelationship" Type="[Entity]or[Category]" RelatedEnId="[Parent]" Direction="Up" Path="[Current]-[Parent]" Action=="Read" &gt;
        ///             &lt;RelationshipAttributes /&gt;
        ///             &lt;Relationships /&gt;       
        ///         &lt;/Relationship&gt;
        ///     </para>
        /// </example>
        public void LoadHierarchyRelationshipDetailsForDataStorage(String valuesAsXml)
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
                            #region Read Hierarchy relationship properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
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
                            }

                            #endregion Read Hierarchy relationship properties
                        }

                        #region Read child hierarchy relationships

                        if (reader.ReadToFollowing("Relationships"))
                        {
                            this.RelationshipCollection = new HierarchyRelationshipCollection(reader.ReadOuterXml(), ObjectSerialization.DataStorage);
                        }

                        #endregion Read child hierarchy relationships
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
        /// Loads the hierarchy relationship from the XML
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        internal void LoadHierarchyRelationshipFromXml(XmlTextReader reader, EntityConversionContext context)
        {
            if (reader.HasAttributes)
            {
                LoadHierarchyRelationshipMetadataFromXml(reader);
            }

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                {
                    // If RelatedEntity is expected to be loaded from xml, then only load it.
                    if (context.LoadRelatedEntityInHierarchyRelationship)
                    {
                        if (reader.HasAttributes)
                        {
                            this.RelatedEntity = new Entity();
                            this.RelatedEntity.LoadEntityFromXml(reader, context);
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships") // child hierarchy relationship
                {
                    // If RelatedEntity is not expected to be loaded from xml, then only load the child hierarchy relationships.
                    if (!context.LoadRelatedEntityInHierarchyRelationship)
                    {
                        this.RelationshipCollection.LoadHierarchyRelationshipCollectionFromXml(reader, context);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Relationship")
                {
                    return;
                }
            }
        }

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
                    HierarchyRelationship objectToBeCompared = obj as HierarchyRelationship;

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

            ConvertHierarchyRelationshipMetadataToXml(xmlWriter);

            #region Write Relationships

            if (this.RelationshipCollection != null)
            {
                String childRelationshipsXml = "<Relationships>";

                foreach (HierarchyRelationship hierarchyRelationship in this.RelationshipCollection)
                {
                    childRelationshipsXml = String.Concat(childRelationshipsXml, hierarchyRelationship.ToXml());
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
                    xmlWriter.WriteAttributeString("ObjTypeId", this.ObjectTypeId.ToString());
                    xmlWriter.WriteAttributeString("ObjType", this.ObjectType);

                    String type = String.Empty;
                    if (Type != null)
                        type = Type.LongName;

                    xmlWriter.WriteAttributeString("Type", type);
                    xmlWriter.WriteAttributeString("RelatedEnId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

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
                    xmlWriter.WriteAttributeString("RelEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("Dir", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
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
                    xmlWriter.WriteAttributeString("Direction", this.Direction.ToString());
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Action", ValueTypeHelper.GetActionString(this.Action));

                    #endregion
                }
                else //TODO::Add for ObjectSerialization.DataTransfer
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
                {
                    String childRelationshipsXml = "<Relationships>";

                    foreach (HierarchyRelationship hierarchyRelationship in this.RelationshipCollection)
                    {
                        childRelationshipsXml = String.Concat(childRelationshipsXml, hierarchyRelationship.ToXml(objectSerialization));
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
        /// Converts HierarchyRelationship object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of HierarchyRelationship object</param>
        /// <param name="context">Indicates context object which specifies what all data of entity to be converted into Xml</param>
        internal void ConvertHierarchyRelationshipToXml(XmlTextWriter xmlWriter, EntityConversionContext context)
        {
            if (xmlWriter != null)
            {
                //Relationship node start
                xmlWriter.WriteStartElement("Relationship");

                ConvertHierarchyRelationshipMetadataToXml(xmlWriter);

                if (context.LoadRelatedEntityInHierarchyRelationship)
                {
                    #region Write Related Entity

                    if (this.RelatedEntity != null)
                    {
                        this.RelatedEntity.ConvertEntityToXml(xmlWriter, context);
                    }

                    #endregion Write Related Entity
                }
                else
                {
                    #region Write Relationships

                    if (this._relationshipCollection != null)
                    {
                        //Relationships node start
                        xmlWriter.WriteStartElement("Relationships");

                        foreach (HierarchyRelationship hierarchyRelationship in this._relationshipCollection)
                        {
                            hierarchyRelationship.ConvertHierarchyRelationshipToXml(xmlWriter, context);
                        }

                        //Relationships node end
                        xmlWriter.WriteEndElement();
                    }

                    #endregion Write Relationships
                }

                //Relationship node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write HierarchyRelationship object.");
            }
        }

        /// <summary>
        /// Gets hierarchy relationships
        /// </summary>
        /// <returns>Hierarchy Relationship collection interface</returns>
        public new IHierarchyRelationshipCollection GetRelationships()
        {
            return (IHierarchyRelationshipCollection)this.RelationshipCollection;
        }

        /// <summary>
        /// Sets hierarchy relationships
        /// </summary>
        /// <param name="iHierarchyRelationshipCollection">Hierarchy Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed hierarchy relationship collection is null</exception>
        public void SetRelationships(IHierarchyRelationshipCollection iHierarchyRelationshipCollection)
        {
            if (iHierarchyRelationshipCollection == null)
                throw new ArgumentNullException("iHierarchyRelationshipCollection");

            this.RelationshipCollection = (HierarchyRelationshipCollection)iHierarchyRelationshipCollection;
        }

        /// <summary>
        /// Checks whether current object or its child objects have been changed i.e any object having Action flag as Create, Update or Delete
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasObjectChanged()
        {
            bool hasObjectUpdated = (this.Action != ObjectAction.Read && this.Action != ObjectAction.Unknown) ||
                this.RelationshipCollection != null && this.RelationshipCollection.HasObjectChanged();

            return hasObjectUpdated;
        }

        /// <summary>
        /// Clone HierarchyRelationship object
        /// </summary>
        /// <param name="cloneRelatedEntity">Flag to indicate if we need to clone related entity also</param>
        /// <returns>Returns cloned copy of HierarchyRelationship</returns>
        public HierarchyRelationship Clone(Boolean cloneRelatedEntity = true)
        {
            HierarchyRelationship clonedHierarchyRelationship = CloneBasicProperties();

            if (this._relationshipCollection != null && this._relationshipCollection.Count > 0)
            {
                HierarchyRelationshipCollection clonedChildHierarchyRelationship = new HierarchyRelationshipCollection();

                foreach (HierarchyRelationship childHierarchyRelationship in this._relationshipCollection)
                {
                    HierarchyRelationship clonedChildHierarchyRel = childHierarchyRelationship.Clone(cloneRelatedEntity); // Recurse method
                    clonedChildHierarchyRelationship.Add(clonedChildHierarchyRel);
                }

                clonedHierarchyRelationship._relationshipCollection = clonedChildHierarchyRelationship;
            }

            clonedHierarchyRelationship.RelatedEntity = cloneRelatedEntity && this.RelatedEntity != null ? this.RelatedEntity.Clone() : null;

            return clonedHierarchyRelationship;
        }

        /// <summary>
        /// Clone HierarchyRelationship object
        /// </summary>
        /// <returns>Returns cloned copy of HierarchyRelationship</returns>
        public HierarchyRelationship CloneBasicProperties(Boolean cloneRelatedEntity = true)
        {
            var clonedHierarchyRelationship = new HierarchyRelationship
            {
                Id = this.Id,
                Name = this.Name,
                LongName = this.LongName,
                Action = this.Action,
                AuditRefId = this.AuditRefId,
                Locale = this.Locale,
                UserName = this.UserName,
                Direction = this.Direction,
                Path = this.Path,
                RelatedEntityId = this.RelatedEntityId,
                Type = this.Type
            };

            clonedHierarchyRelationship.RelatedEntity = cloneRelatedEntity && this.RelatedEntity != null ? RelatedEntity.Clone() : null;

            return clonedHierarchyRelationship;
        }

        /// <summary>
        /// Compares HierarchyRelationship object with current HierarchyRelationship object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetHierarchyRelationship">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(HierarchyRelationship subsetHierarchyRelationship, Boolean compareIds = false)
        {
            if (subsetHierarchyRelationship != null)
            {
                if (base.IsSuperSetOf(subsetHierarchyRelationship, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.RelatedEntityId != subsetHierarchyRelationship.RelatedEntityId)
                        {
                            return false;
                        }

                        if (String.Compare(this.Path, subsetHierarchyRelationship.Path) != 0)
                        {
                            return false;
                        }
                    }

                    if (this.ObjectType != subsetHierarchyRelationship.ObjectType)
                    {
                        return false;
                    }

                    if (!this.RelationshipCollection.IsSuperSetOf(subsetHierarchyRelationship.RelationshipCollection))
                    {
                        return false;
                    }

                    if (!this.Type.IsSuperSetOf(subsetHierarchyRelationship.Type))
                    {
                        return false;
                    }

                    if (this.Direction != subsetHierarchyRelationship.Direction)
                    {
                        return false;
                    }

                    if (subsetHierarchyRelationship.RelatedEntity != null)
                    {
                        if (this.RelatedEntity != null)
                        {
                            if (!this.RelatedEntity.IsSuperSetOf(subsetHierarchyRelationship.RelatedEntity))
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

            return true;
        }

        /// <summary>
        /// compare hierarchyrelationship
        /// </summary>
        /// <param name="subsetHierarchyRelationship"></param>
        /// <param name="hierarchyRelationshipOperationResult"></param>
        /// <param name="compareByRelatedEntityId"></param>
        /// <returns></returns>
        public Boolean GetSuperSetOperationResult(HierarchyRelationship subsetHierarchyRelationship, OperationResult hierarchyRelationshipOperationResult, Boolean compareByRelatedEntityId = false)
        {
            if (subsetHierarchyRelationship != null)
            {
                #region compare related entity ID

                if (compareByRelatedEntityId)
                {
                    Utility.BusinessObjectPropertyCompare("RelatedEntityId", RelatedEntityId, subsetHierarchyRelationship.RelatedEntityId, hierarchyRelationshipOperationResult);
                }

                #endregion compare related entity ID

                #region compare child hierarchyRelationshipCollection

                var subsetHierachyRelationshipOperationResult = new EntityOperationResult();

                Utility.BusinessObjectHierarchyRelationshipOperationResultCompare(this.RelationshipCollection, subsetHierarchyRelationship.RelationshipCollection, subsetHierachyRelationshipOperationResult);

                foreach (var error in subsetHierachyRelationshipOperationResult.Errors)
                {
                    hierarchyRelationshipOperationResult.Errors.Add(error);
                }

                #endregion compare child hierarchyRelationCollection

                #region compare base properties

                Utility.BusinessObjectPropertyCompare("HierarchyRelationship Name", this.Name, subsetHierarchyRelationship.Name, hierarchyRelationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("HierarchyRelationship LongName", this.LongName, subsetHierarchyRelationship.LongName, hierarchyRelationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("HierarchyRelationship Locale", this.Locale, subsetHierarchyRelationship.Locale, hierarchyRelationshipOperationResult);

                #endregion compare base properties

                #region compare extended properties

                Utility.BusinessObjectPropertyCompare("ExtensionRelationship Type", this.GetRelationshipType().Name, subsetHierarchyRelationship.GetRelationshipType().Name, hierarchyRelationshipOperationResult);

                //Utility.BusinessObjectPropertyCompare("HierarchyRelationship Path", this.Path, subsetHierarchyRelationship.Path, hierarchyRelationshipOperationResult);

                Utility.BusinessObjectPropertyCompare("HierarchyRelationship Direction", this.Direction, subsetHierarchyRelationship.Direction, hierarchyRelationshipOperationResult);

                #endregion compare properties

                #region compare RelationshipCollection

                this.RelationshipCollection.GetSuperSetOfOperationResult(subsetHierarchyRelationship.RelationshipCollection, hierarchyRelationshipOperationResult);

                #endregion compare RelationshipCollection


                #region compare target entity

                if (subsetHierarchyRelationship.RelatedEntity != null)
                {
                    if (this.RelatedEntity != null)
                    {
                        var relatedEntityOperationResult = this.RelatedEntity.GetSuperSetOperationResult(subsetHierarchyRelationship.RelatedEntity);

                        foreach (var error in relatedEntityOperationResult.GetAllErrors())
                        {
                            hierarchyRelationshipOperationResult.Errors.Add(error);
                        }
                    }
                    else
                    {
                        hierarchyRelationshipOperationResult.AddOperationResult("-1", "RelatedEntity of superset HierarchyRelationship is null", OperationResultType.Error);
                    }
                }
                else
                {
                    hierarchyRelationshipOperationResult.AddOperationResult("-1", "RelatedEntity of subsetHierarchyRelationship is null", OperationResultType.Information);
                }

                #endregion compare target entity
            }
            else //subset hierarchy relationship is null
            {
                hierarchyRelationshipOperationResult.AddOperationResult("-1", "SubsetHierarchyRelationship is null - nothing to compare", OperationResultType.Information);
            }

            #region refresh and return

            hierarchyRelationshipOperationResult.RefreshOperationResultStatus();

            if (hierarchyRelationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }

            #endregion refresh and return
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads properties of WorkflowActionContext from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        private void LoadHierarchyRelationshipMetadataFromXml(XmlTextReader reader)
        {
            if (reader.MoveToAttribute("Id"))
            {
                this.Id = reader.ReadContentAsInt();
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
        }

        /// <summary>
        /// Converts properties (metadata) of HierarchyRelationship object into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of HierarchyRelationship metadata</param>
        private void ConvertHierarchyRelationshipMetadataToXml(XmlTextWriter xmlWriter)
        {
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

            #endregion Write relationship properties
        }

        #endregion Private Methods

        #endregion
    }
}
