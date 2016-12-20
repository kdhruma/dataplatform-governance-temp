using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping to uniquely identifier the MDM entity
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityUniqueIdentifier : ObjectBase, IEntityUniqueIdentifier
    {
        #region Fields

        /// <summary>
        /// Field denoting entity id
        /// </summary>
        [DataMember]
        [ProtoMember(1), DefaultValue(0)]
        private Int64 _entityId = 0;

        /// <summary>
        /// Field defining the external id / shortname of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private String _externalId = String.Empty;

        /// <summary>
        /// Field denoting the category id of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(3), DefaultValue(0)]
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting category path of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        private String _categoryPath = String.Empty;

        /// <summary>
        /// Field denoting container id of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(5), DefaultValue(0)]
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting container name of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        private String _containerName = String.Empty;

        /// <summary>
        /// Field denoting container qualifier name of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        private String _containerQualifierName = String.Empty;

        /// <summary>
        /// Field denoting container level of container
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        private Int32 _containerLevel = 0;

        /// <summary>
        /// Field denoting entity type Id of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(9), DefaultValue(0)]
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field denoting entity name of the entity in the context
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        private String _entityTypeName = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting id of the entity identifier 
        /// </summary>
        public Int64 EntityId
        {
            get
            {
                return _entityId;
            }
            set
            {
                _entityId = value;
            }
        }

        /// <summary>
        /// Property defining the short name of the MDM object
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
        /// Property defining the category id of the MDM object
        /// </summary>
        public Int64 CategoryId
        {
            get
            {
                return _categoryId;
            }
            set
            {
                _categoryId = value;
            }
        }

        /// <summary>
        /// Property denoting the category path of the MDM Object
        /// </summary>
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
        /// Property denoting entity catalog Id of the MDM Object
        /// </summary>
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
        /// Property denoting catalog Name of MDM Object
        /// </summary>
        public String ContainerName
        {
            get
            {
                return _containerName;
            }
            set
            {
                _containerName = value;
            }
        }

        /// <summary>
        /// Property denoting catalog Name of MDM Object
        /// </summary>
        public String ContainerQualifierName
        {
            get
            {
                return _containerQualifierName;
            }
            set
            {
                _containerQualifierName = value;
            }
        }

        /// <summary>
        /// Property denoting container level of container
        /// </summary>
        public Int32 ContainerLevel
        {
            get
            {
                return this._containerLevel;
            }
            set
            {
                this._containerLevel = value;
            }
        }

        /// <summary>
        /// Property denoting Entity type Id of the entity
        /// </summary>
        public Int32 EntityTypeId
        {
            get
            {
                return _entityTypeId;
            }
            set
            {
                _entityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting entity type name of the entity in the context
        /// </summary>
        public String EntityTypeName
        {
            get
            {
                return _entityTypeName;
            }
            set
            {
                _entityTypeName = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityUniqueIdentifier()
        {

        }

        /// <summary>
        /// Default constructor with internal entity Id(PK_CNode)
        /// </summary>
        public EntityUniqueIdentifier(Int64 entityId)
        {
            _entityId = entityId;
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public EntityUniqueIdentifier(String valuesAsXml)
        {
            LoadEntityUniqueIdentifiers(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is EntityUniqueIdentifier)
            {
                var objectToBeCompared = obj as EntityUniqueIdentifier;

                if (this.EntityId != objectToBeCompared.EntityId)
                {
                    return false;
                }
                if (this.ExternalId != objectToBeCompared.ExternalId)
                {
                    return false;
                }
                if (this.CategoryId != objectToBeCompared.CategoryId)
                {
                    return false;
                }
                if (this.CategoryPath != objectToBeCompared.CategoryPath)
                {
                    return false;
                }
                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }
                if (this.ContainerName != objectToBeCompared.ContainerName)
                {
                    return false;
                }
                if (this.ContainerQualifierName != objectToBeCompared.ContainerQualifierName)
                {
                    return false;
                }
                if (this.ContainerLevel != objectToBeCompared.ContainerLevel)
                {
                    return false;
                }
                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                {
                    return false;
                }
                if (this.EntityTypeName != objectToBeCompared.EntityTypeName)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.EntityId.GetHashCode() ^ this.ExternalId.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.CategoryPath.GetHashCode() ^
                this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^ this.ContainerQualifierName.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.EntityTypeName.GetHashCode() ^
                this.ContainerLevel.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Entity Identifier
        /// </summary>
        /// <returns>Xml representation of Entity Identifier</returns>
        public String ToXml()
        {
            String entityUniqueIdentifierXml;

            var sw = new StringWriter();
            var xmlWriter = new XmlTextWriter(sw);

            //Entity Identifier node start
            xmlWriter.WriteStartElement("EntityUniqueIdentifier");

            #region Write Entity Identifier properties

            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("Externalid", this.ExternalId);
            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath);
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
            xmlWriter.WriteAttributeString("ContainerQualifierName", this.ContainerQualifierName);
            xmlWriter.WriteAttributeString("ContainerLevel", this.ContainerLevel.ToString());
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            entityUniqueIdentifierXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityUniqueIdentifierXml;
        }

        /// <summary>
        /// Get Xml representation of Entity Identifier based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Identifier</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String entityUniqueIdentifierXml;

            if (objectSerialization == ObjectSerialization.Full)
            {
                entityUniqueIdentifierXml = this.ToXml();
            }
            else
            {
                var sw = new StringWriter();
                var xmlWriter = new XmlTextWriter(sw);

                //Entity Identifier node start
                xmlWriter.WriteStartElement("EntityUniqueIdentifier");

                #region Write Entity Identifier properties

                xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                xmlWriter.WriteAttributeString("Externalid", this.ExternalId);
                xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath);
                xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
                xmlWriter.WriteAttributeString("ContainerQualifierName", this.ContainerQualifierName);
                xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);

                #endregion

                //Operation result node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                entityUniqueIdentifierXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return entityUniqueIdentifierXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityUniqueIdentifiers(String valuesAsXml)
        {

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityUniqueIdentifier")
                    {
                        #region Read EntityUniqueIdentifiers Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("EntityId"))
                            {
                                this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("ExternalId"))
                            {
                                this.ExternalId = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CategoryId"))
                            {
                                this.CategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("CategoryPath"))
                            {
                                this.CategoryPath = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("ContainerName"))
                            {
                                this.ContainerName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ContainerQualifierName"))
                            {
                                this.ContainerQualifierName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ContainerLevel"))
                            {
                                this.ContainerLevel = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("EntityTypeId"))
                            {
                                this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("EntityTypeName"))
                            {
                                this.EntityTypeName = reader.ReadContentAsString();
                            }
                        }

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

        #endregion

        #endregion
    }
}