using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping of MDM Internal Entity Id and the External System Entity Id
    /// </summary>
    [DataContract]
    public class EntityMap : IEntityMap
    {
        #region Fields

        /// <summary>
        /// Field denoting id of the entity map used for internal operations
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field defining the type id of the object being mapped
        /// </summary>
        private Int32 _objectTypeId = 0;

        /// <summary>
        /// Field defining the type of the object being mapped
        /// </summary>
        private String _objectType = String.Empty;

        /// <summary>
        /// Field denoting the Id of the system
        /// </summary>
        private String _systemId = String.Empty;

        /// <summary>
        /// Field denoting entity external Id
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field denoting entity internal Id
        /// </summary>
        private Int64 _internalId = 0;

        /// <summary>
        /// Field denoting container Id of the entity in the context
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting category Id of the entity in the context
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting entity type Id of the 
        /// </summary>
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field denoting the parent extension Id of an entity map
        /// </summary>
        private Int64 _parentExtensionEntityId = 0;

        /// <summary>
        /// Property denoting custom data that can be used for passing attribute values
        /// </summary>
        private String _customData = String.Empty;

        /// <summary>
        /// Field denoting entity family id
        /// </summary>
        private Int64 _entityFamilyId = 0;

        /// <summary>
        /// Field denoting entity family group id
        /// </summary>
        private Int64 _entityGlobalFamilyId = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting id of the entity map used for internal operations
        /// </summary>
        [DataMember]
        public Int64 Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// Property defining the type id of the MDM object
        /// </summary>
        [DataMember]
        public Int32 ObjectTypeId
        {
            get
            {
                return _objectTypeId;
            }
            set
            {
                _objectTypeId = value;
            }
        }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        [DataMember]
        public String ObjectType
        {
            get
            {
                return _objectType;
            }
            set
            {
                _objectType = value;
            }
        }

        /// <summary>
        /// Property denoting the Id of the system
        /// </summary>
        [DataMember]
        public String SystemId
        {
            get
            {
                return _systemId;
            }
            set
            {
                _systemId = value;
            }
        }

        /// <summary>
        /// Property denoting entity external Id
        /// </summary>
        [DataMember]
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
        /// Property denoting entity internal Id
        /// </summary>
        [DataMember]
        public Int64 InternalId
        {
            get
            {
                return _internalId;
            }
            set
            {
                _internalId = value;
            }
        }

        /// <summary>
        /// Property denoting container Id of the entity in the context
        /// </summary>
        [DataMember]
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
        /// Property denoting category Id of the entity in the context
        /// </summary>
        [DataMember]
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
        /// Property denoting entity type Id of the entity in the context
        /// </summary>
        [DataMember]
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
        /// Property denoting the parent extension id of an entity map
        /// </summary>
        [DataMember]
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

        /// <summary>
        /// Property denoting id of the entity map used for internal operations
        /// </summary>
        [DataMember]
        public String CustomData
        {
            get
            {
                return _customData;
            }
            set
            {
                _customData = value;
            }
        }

        /// <summary>
        /// Specifies entity family id for a variant tree
        /// </summary>
        [DataMember]
        public Int64 EntityFamilyId
        {
            get
            {
                return this._entityFamilyId;
            }
            set
            {
                this._entityFamilyId = value;
            }
        }

        /// <summary>
        /// Specifies entity global family id across parent(including extended families)
        /// </summary>
        [DataMember]
        public Int64 EntityGlobalFamilyId
        {
            get
            {
                return this._entityGlobalFamilyId;
            }
            set
            {
                this._entityGlobalFamilyId = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the new instance of Entity Map
        /// </summary>
        public EntityMap()
        {
            
        }

        /// <summary>
        /// Instantiates the new instance of Entity Map
        /// </summary>
        /// <param name="id">Indicates Id of an entity</param>
        /// <param name="systemId">Indicates Id of the system from where entity has been imported</param>
        /// <param name="objectTypeId">Indicates Id of the object type</param>
        /// <param name="objectType">Indicates Type of the object type</param>
        /// <param name="externalId">Indicates Entity Id in the external system</param>
        /// <param name="internalId">Indicates Entity Internal Id</param>
        /// <param name="containerId">Indicates Id of container</param>
        /// <param name="categoryId">Indicates Id of category</param>
        /// <param name="entityTypeId">Indicates Id of entity type</param>
        public EntityMap(Int64 id, String systemId, Int32 objectTypeId, String objectType, String externalId, Int64 internalId, Int32 containerId, Int64 categoryId, Int32 entityTypeId)
            : this(id, systemId, objectTypeId, objectType, externalId, internalId, containerId, categoryId, entityTypeId, 0)
        {
        }

        /// <summary>
        /// Instantiates the new instance of Entity Map
        /// </summary>
        /// <param name="id">Indicates Id of an entity</param>
        /// <param name="systemId">Indicates Id of the system from where entity has been imported</param>
        /// <param name="objectTypeId">Indicates Id of the object type</param>
        /// <param name="objectType">Indicates Type of the object type</param>
        /// <param name="externalId">Indicates Entity Id in the external system</param>
        /// <param name="internalId">Indicates Entity Internal Id</param>
        /// <param name="containerId">Indicates Id of container</param>
        /// <param name="categoryId">Indicates Id of category</param>
        /// <param name="entityTypeId">Indicates Id of entity type</param>
        /// <param name="parentExtensionEntityId">Indicates Id of parent extension entity id</param>
        public EntityMap(Int64 id, String systemId, Int32 objectTypeId, String objectType, String externalId, Int64 internalId, Int32 containerId, Int64 categoryId, Int32 entityTypeId, Int64 parentExtensionEntityId)
        {
            _id = id;
            _systemId = systemId;
            _objectTypeId = objectTypeId;
            _objectType = objectType;
            _externalId = externalId;
            _internalId = internalId;
            _containerId = containerId;
            _categoryId = categoryId;
            _entityTypeId = entityTypeId;
            _parentExtensionEntityId = parentExtensionEntityId;
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
            if (obj != null && obj is EntityMap)
            {
                EntityMap objectToBeCompared = obj as EntityMap;

                if (this.Id != objectToBeCompared.Id)
                {
                    return false;
                }

                if (this.SystemId != objectToBeCompared.SystemId)
                {
                    return false;
                }

                if (this.ObjectTypeId != objectToBeCompared.ObjectTypeId)
                {
                    return false;
                }

                if (this.ObjectType != objectToBeCompared.ObjectType)
                {
                    return false;
                }

                if (this.ExternalId != objectToBeCompared.ExternalId)
                {
                    return false;
                }

                if (this.InternalId != objectToBeCompared.InternalId)
                {
                    return false;
                }

                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }

                if (this.CategoryId != objectToBeCompared.CategoryId)
                {
                    return false;
                }

                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                {
                    return false;
                }

                if (this.ParentExtensionEntityId != objectToBeCompared.ParentExtensionEntityId)
                {
                    return false;
                }

                if (this.CustomData != objectToBeCompared.CustomData)
                {
                    return false;
                }

                if (this.EntityFamilyId != objectToBeCompared.EntityFamilyId)
                {
                    return false;
                }

                if (this.EntityGlobalFamilyId != objectToBeCompared.EntityGlobalFamilyId)
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
            Int32 hashCode = base.GetHashCode() ^ this.Id.GetHashCode() ^ this.SystemId.GetHashCode() ^ this.ObjectTypeId.GetHashCode() ^ this.ObjectType.GetHashCode() ^
                this.ExternalId.GetHashCode() ^ this.InternalId.GetHashCode() ^ this.ContainerId.GetHashCode() ^ this.CategoryId.GetHashCode() ^
                this.EntityTypeId.GetHashCode() ^ this.ParentExtensionEntityId.GetHashCode() ^ this.CustomData.GetHashCode() ^ this.EntityFamilyId.GetHashCode() ^ this.EntityGlobalFamilyId.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Entity Map
        /// </summary>
        /// <returns>Xml representation of Entity Map</returns>
        public String ToXml()
        {
            String entityMapXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Entity Map node start
            xmlWriter.WriteStartElement("EntityMap");

            #region Write Entity Map properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("SystemId", this.SystemId);
            xmlWriter.WriteAttributeString("ObjectTypeId", this.ObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("ObjectType", this.ObjectType);
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
            xmlWriter.WriteAttributeString("InternalId", this.InternalId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
            xmlWriter.WriteAttributeString("CustomData", this.CustomData);
            xmlWriter.WriteAttributeString("EntityFamilyId", this.EntityFamilyId.ToString());
            xmlWriter.WriteAttributeString("EntityGlobalFamilyId", this.EntityGlobalFamilyId.ToString());

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            entityMapXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityMapXml;
        }

        /// <summary>
        /// Get Xml representation of Entity Map based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity map</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String entityMapXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                entityMapXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Entity Map node start
                xmlWriter.WriteStartElement("EntityMap");

                #region Write Entity Map properties

                xmlWriter.WriteAttributeString("SystemId", this.SystemId);
                xmlWriter.WriteAttributeString("ObjectTypeId", this.ObjectTypeId.ToString());
                xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());
                xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
                xmlWriter.WriteAttributeString("InternalId", this.InternalId.ToString());
                xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
                xmlWriter.WriteAttributeString("CustomData", this.CustomData.ToString());
                xmlWriter.WriteAttributeString("EntityFamilyId", this.EntityFamilyId.ToString());
                xmlWriter.WriteAttributeString("EntityGlobalFamilyId", this.EntityGlobalFamilyId.ToString());

                #endregion

                //Operation result node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                entityMapXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return entityMapXml;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
