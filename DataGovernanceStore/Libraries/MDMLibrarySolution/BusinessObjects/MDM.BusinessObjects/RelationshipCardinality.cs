using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Relationship Cardinality
    /// </summary>
    [DataContract]
    public class RelationshipCardinality : MDMObject, IRelationshipCardinality, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the RelationshipTypeEntityTypeMapping Id
        /// </summary>
        private Int32 _relationshipTypeEntityTypeMappingId = -1;

        /// <summary>
        /// Field denoting the ToEntityTypeId
        /// </summary>
        private Int32 _toEntityTypeId = -1;

        /// <summary>
        /// Field denoting the To EntityType LongName
        /// </summary>
        private String _toEntityTypeName = String.Empty;

        /// <summary>
        /// Field denoting the To EntityType LongName
        /// </summary>
        private String _toEntityTypelongName = String.Empty;

        /// <summary>
        /// Field denoting the Minimum Relationships allowed 
        /// </summary>
        private Int32 _minRelationships = -1;

        /// <summary>
        /// Field denoting the Maximum Relationships allowed 
        /// </summary>
        private Int32 _maxRelationships = -1;

        /// <summary>
        /// Field denoting the RelationshipTypeId
        /// </summary>
        private Int32 _relationshipTypeId = -1;

        /// <summary>
        /// Field denoting the container id
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Field denoting the From EntityTypeId
        /// </summary>
        private Int32 _fromEntityTypeId = -1;

        /// <summary>
        /// Field denoting container key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion
        
        #region Constructors

         /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipCardinality()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipCardinalityId"></param>
        /// <param name="relationshipTypeEntityTypeMappingId"></param>
        /// <param name="toEntityTypeId"></param>
        /// <param name="minRelationships"></param>
        /// <param name="maxRelationships"></param>
        public RelationshipCardinality(Int32 relationshipCardinalityId, Int32 relationshipTypeEntityTypeMappingId, Int32 toEntityTypeId, Int32 minRelationships, Int32 maxRelationships)
            : base(relationshipCardinalityId)
        {
            this._relationshipTypeEntityTypeMappingId = relationshipTypeEntityTypeMappingId;
            this._toEntityTypeId = toEntityTypeId;
            this._minRelationships = minRelationships;
            this._maxRelationships = maxRelationships;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueAsXml"></param>
        public RelationshipCardinality(String valueAsXml)
        {
            LoadRelationshipCardinality(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the RelationshipTypeEntityTypeMapping Id
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeEntityTypeMappingId
        {
            get
            {
                return this._relationshipTypeEntityTypeMappingId;
            }
            set
            {
                this._relationshipTypeEntityTypeMappingId = value;
            }
        }

        /// <summary>
        /// Property denoting the ToEntityTypeId
        /// </summary>
        [DataMember]
        public Int32 ToEntityTypeId
        {
            get
            {
                return this._toEntityTypeId;
            }
            set
            {
                this._toEntityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting the To EntityType LongName
        /// </summary>
        [DataMember]
        public String ToEntityTypeName
        {
            get
            {
                return this._toEntityTypeName;
            }
            set
            {
                this._toEntityTypeName = value;
            }
        }

        /// <summary>
        /// Property denoting the To EntityType LongName
        /// </summary>
        [DataMember]
        public String ToEntityTypeLongName
        {
            get
            {
                return this._toEntityTypelongName;
            }
            set
            {
                this._toEntityTypelongName = value;
            }
        }

        /// <summary>
        /// Property denoting the Minimum Relationships allowed 
        /// </summary>
        [DataMember]
        public Int32 MinRelationships
        {
            get
            {
                return this._minRelationships;
            }
            set
            {
                this._minRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting the Maximum Relationships allowed 
        /// </summary>
        [DataMember]
        public Int32 MaxRelationships
        {
            get
            {
                return this._maxRelationships;
            }
            set
            {
                this._maxRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting the RelationshipTypeId
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get
            {
                return this._relationshipTypeId;
            }
            set
            {
                this._relationshipTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting the container id
        /// </summary>
        [DataMember]
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
        /// Property denoting the From EntityTypeId
        /// </summary>
        [DataMember]
        public Int32 FromEntityTypeId
        {
            get
            {
                return this._fromEntityTypeId;
            }
            set
            {
                this._fromEntityTypeId = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is RelationshipCardinality)
                {
                    RelationshipCardinality objectToBeCompared = obj as RelationshipCardinality;

                    if (!this.RelationshipTypeEntityTypeMappingId.Equals(objectToBeCompared.RelationshipTypeEntityTypeMappingId))
                        return false;

                    if (!this.ContainerId.Equals(objectToBeCompared.ContainerId))
                        return false;

                    if (!this.FromEntityTypeId.Equals(objectToBeCompared.FromEntityTypeId))
                        return false;

                    if (!this.ToEntityTypeId.Equals(objectToBeCompared.ToEntityTypeId))
                        return false;

                    if (!this.ToEntityTypeName.Equals(objectToBeCompared.ToEntityTypeName))
                        return false;

                    if (!this.ToEntityTypeLongName.Equals(objectToBeCompared.ToEntityTypeLongName))
                        return false;

                    if (!this.RelationshipTypeId.Equals(objectToBeCompared.RelationshipTypeId))
                        return false;

                    if (!this.MaxRelationships.Equals(objectToBeCompared.MaxRelationships))
                        return false;

                    if (!this.MinRelationships.Equals(objectToBeCompared.MinRelationships))
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
            hashCode = base.GetHashCode() ^ this.Locale.GetHashCode() ^ this.RelationshipTypeEntityTypeMappingId.GetHashCode() ^ this.ToEntityTypeId.GetHashCode()
                        ^ this.ContainerId.GetHashCode() ^ this.ToEntityTypeLongName.GetHashCode() ^ this.ToEntityTypeName.GetHashCode() ^ this.FromEntityTypeId.GetHashCode()
                        ^ this.RelationshipTypeId.GetHashCode() ^ this.MaxRelationships.GetHashCode() ^ this.MinRelationships.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Represents RelationshipCardinality  in Xml format
        /// </summary>
        /// <returns>String representation of current RelationshipCardinality object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //RelationshipContext node start
            xmlWriter.WriteStartElement("RelationshipCardinality");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeEntityTypeMappingId", this.RelationshipTypeEntityTypeMappingId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("FromEntityTypeId", this.FromEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToEntityTypeId", this.ToEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToEntityTypeName", this.ToEntityTypeName);
            xmlWriter.WriteAttributeString("ToEntityTypeLongName", this.ToEntityTypeLongName);
            xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("MinRelationships", this.MinRelationships.ToString());
            xmlWriter.WriteAttributeString("MixRelationships", this.MaxRelationships.ToString());

            //RelationshipContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Create a new relationship Cardinality object.
        /// </summary>
        /// <returns>New relationship Cardinality instance</returns>
        public RelationshipCardinality Clone()
        {
            RelationshipCardinality clonedRelationshipCardinality = new RelationshipCardinality();

            clonedRelationshipCardinality.Id = this.Id;
            clonedRelationshipCardinality._containerId = this._containerId;
            clonedRelationshipCardinality._relationshipTypeEntityTypeMappingId = this._relationshipTypeEntityTypeMappingId;
            clonedRelationshipCardinality._fromEntityTypeId = this._fromEntityTypeId;
            clonedRelationshipCardinality._toEntityTypeId = this._toEntityTypeId;
            clonedRelationshipCardinality._toEntityTypeName = this._toEntityTypeName;
            clonedRelationshipCardinality._toEntityTypelongName = this._toEntityTypelongName;
            clonedRelationshipCardinality._relationshipTypeId = this._relationshipTypeId;
            clonedRelationshipCardinality._maxRelationships = this._maxRelationships;
            clonedRelationshipCardinality._minRelationships = this._minRelationships;

            return clonedRelationshipCardinality;
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
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
        /// Represents the data model object type.
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get { return MDM.Core.ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality; }
        }

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
        /// 
        /// </summary>
        /// <param name="valueAsXml"></param>
        private void LoadRelationshipCardinality(String valueAsXml)
        {
            #region Sample Xml
            /*
             * <RelationshipCardinality RelationshipTypeEntityTypeMappingId="3001" ToEntityTypeId="16" Level="10" Locale="en_WW"/>
             */
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipCardinality")
                    {
                        #region Read EntityContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                            }

                            if (reader.MoveToAttribute("RelationshipTypeEntityTypeMappingId"))
                            {
                                this.RelationshipTypeEntityTypeMappingId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._relationshipTypeEntityTypeMappingId);
                            }

                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }

                            if (reader.MoveToAttribute("FromEntityTypeId"))
                            {
                                this.FromEntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._fromEntityTypeId);
                            }

                            if (reader.MoveToAttribute("ToEntityTypeId"))
                            {
                                this.ToEntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._toEntityTypeId);
                            }

                            if (reader.MoveToAttribute("ToEntityTypeName"))
                            {
                                this.ToEntityTypeName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ToEntityTypeLongName"))
                            {
                                this.ToEntityTypeLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("MinRelationships"))
                            {
                                this.MinRelationships = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._minRelationships);
                            }

                            if (reader.MoveToAttribute("MaxRelationships"))
                            {
                                this.MaxRelationships = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._maxRelationships);
                            }

                            if (reader.MoveToAttribute("RelationshipTypeId"))
                            {
                                this.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), _relationshipTypeId);
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the XML until we reach expected node.
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

        #endregion

        #endregion        
    }
}
