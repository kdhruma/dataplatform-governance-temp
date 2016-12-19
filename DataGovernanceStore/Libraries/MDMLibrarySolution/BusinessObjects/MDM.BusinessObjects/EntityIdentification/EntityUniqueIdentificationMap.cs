using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.EntityIdentification
{
    using MDM.Core;
    using MDM.Interfaces.EntityIdentification;

    /// <summary>
    /// Specifies EntityUniqueIdentification Values 
    /// </summary>
    [DataContract]
    public class EntityUniqueIdentificationMap : IEntityUniqueIdentificationMap
    {
        #region Fields

        /// <summary>
        /// Field denoting id of the entity identifier map used for internal operations
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field denoting entity external Id
        /// </summary>
        private String _externalId = String.Empty;

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
        /// Property denoting entity identifier 1
        /// </summary>
        private String _entityIdentifier1Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 2
        /// </summary>
        private String _entityIdentifier2Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 3
        /// </summary>
        private String _entityIdentifier3Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 4
        /// </summary>
        private String _entityIdentifier4Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 5
        /// </summary>
        private String _entityIdentifier5Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 1
        /// </summary>
        private Int64 _entityIdentifier1Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 2
        /// </summary>
        private Int64 _entityIdentifier2Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 3
        /// </summary>
        private Int64 _entityIdentifier3Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 4
        /// </summary>
        private Int64 _entityIdentifier4Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 5
        /// </summary>
        private Int64 _entityIdentifier5Result { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// Property denoting id of the entity identifier map used for internal operations
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
        /// Property denoting entity identifier 1
        /// </summary>
        public String EntityIdentifier1Value
        {
            get
            {
                return _entityIdentifier1Value;
            }
            set
            {
                _entityIdentifier1Value = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier 2
        /// </summary>
        public String EntityIdentifier2Value
        {
            get
            {
                return _entityIdentifier2Value;
            }
            set
            {
                _entityIdentifier2Value = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier 3
        /// </summary>
        public String EntityIdentifier3Value
        {
            get
            {
                return _entityIdentifier3Value;
            }
            set
            {
                _entityIdentifier3Value = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier 4
        /// </summary>
        public String EntityIdentifier4Value
        {
            get
            {
                return _entityIdentifier4Value;
            }
            set
            {
                _entityIdentifier4Value = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier 5
        /// </summary>
        public String EntityIdentifier5Value
        {
            get
            {
                return _entityIdentifier5Value;
            }
            set
            {
                _entityIdentifier5Value = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier result 1
        /// </summary>
        public Int64 EntityIdentifier1Result
        {
            get
            {
                return _entityIdentifier1Result;
            }
            set
            {
                _entityIdentifier1Result = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier result 2
        /// </summary>
        public Int64 EntityIdentifier2Result
        {
            get
            {
                return _entityIdentifier2Result;
            }
            set
            {
                _entityIdentifier2Result = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier result 3
        /// </summary>
        public Int64 EntityIdentifier3Result
        {
            get
            {
                return _entityIdentifier3Result;
            }
            set
            {
                _entityIdentifier3Result = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier result 4
        /// </summary>
        public Int64 EntityIdentifier4Result
        {
            get
            {
                return _entityIdentifier4Result;
            }
            set
            {
                _entityIdentifier4Result = value;
            }
        }

        /// <summary>
        /// Property denoting entity identifier result 5
        /// </summary>
        public Int64 EntityIdentifier5Result
        {
            get
            {
                return _entityIdentifier5Result;
            }
            set
            {
                _entityIdentifier5Result = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiates the new instance of entity identifier map
        /// </summary>
        public EntityUniqueIdentificationMap()
        {

        }

        /// <summary>
        /// Instantiates the new instance of entity identifier map
        /// </summary>
        /// <param name="id">Indicates Id of an entity</param>
        /// <param name="externalId">Indicates Entity Id in the external system</param>
        /// <param name="containerId">Indicates Id of container</param>
        /// <param name="categoryId">Indicates Id of category</param>
        /// <param name="entityTypeId">Indicates Id of entity type</param>        
        public EntityUniqueIdentificationMap(Int64 id, String externalId, Int32 containerId, Int64 categoryId, Int32 entityTypeId)
        {
            _id = id;
            _externalId = externalId;
            _containerId = containerId;
            _categoryId = categoryId;
            _entityTypeId = entityTypeId;
        }

        /// <summary>
        /// Instantiates the new instance of entity identifier map
        /// </summary>
        /// <param name="id">Indicates Id of an entity</param>
        /// <param name="externalId">Indicates Entity Id in the external system</param>
        /// <param name="containerId">Indicates Id of container</param>
        /// <param name="categoryId">Indicates Id of category</param>
        /// <param name="entityTypeId">Indicates Id of entity type</param>        
        /// <param name="entityIdentifier1Value">Indicates entityIdentifier1Value</param>
        /// <param name="entityIdentifier2Value">Indicates entityIdentifier2Value</param>
        /// <param name="entityIdentifier3Value">Indicates entityIdentifier3Value</param>
        /// <param name="entityIdentifier4Value">Indicates entityIdentifier4Value</param>
        /// <param name="entityIdentifier5Value">Indicates entityIdentifier5Value</param>
        public EntityUniqueIdentificationMap(Int64 id, String externalId, Int32 containerId, Int64 categoryId, Int32 entityTypeId, String entityIdentifier1Value, String entityIdentifier2Value, String entityIdentifier3Value, String entityIdentifier4Value, String entityIdentifier5Value)
        {
            _id = id;
            _externalId = externalId;
            _containerId = containerId;
            _categoryId = categoryId;
            _entityTypeId = entityTypeId;

            _entityIdentifier1Value = entityIdentifier1Value;
            _entityIdentifier2Value = entityIdentifier2Value;
            _entityIdentifier3Value = entityIdentifier3Value;
            _entityIdentifier4Value = entityIdentifier4Value;
            _entityIdentifier5Value = entityIdentifier5Value;
        }

        /// <summary>
        /// Instantiates the new instance of entity identifier map
        /// </summary>
        /// <param name="id">Indicates Id of an entity</param>
        /// <param name="externalId">Indicates Entity Id in the external system</param>
        /// <param name="containerId">Indicates Id of container</param>
        /// <param name="categoryId">Indicates Id of category</param>
        /// <param name="entityTypeId">Indicates Id of entity type</param>
        /// <param name="entityIdentifier1Value">Indicates entityIdentifier1Value</param>
        /// <param name="entityIdentifier2Value">Indicates entityIdentifier2Value</param>
        /// <param name="entityIdentifier3Value">Indicates entityIdentifier3Value</param>
        /// <param name="entityIdentifier4Value">Indicates entityIdentifier4Value</param>
        /// <param name="entityIdentifier5Value">Indicates entityIdentifier5Value</param>
        /// <param name="entityIdentifier1Result">Indicates entityIdentifier1Result</param>
        /// <param name="entityIdentifier2Result">Indicates entityIdentifier2Result</param>
        /// <param name="entityIdentifier3Result">Indicates entityIdentifier3Result</param>
        /// <param name="entityIdentifier4Result">Indicates entityIdentifier4Result</param>
        /// <param name="entityIdentifier5Result">Indicates entityIdentifier5Result</param>
        public EntityUniqueIdentificationMap(Int64 id, String externalId, Int32 containerId, Int64 categoryId, Int32 entityTypeId, String entityIdentifier1Value, String entityIdentifier2Value, String entityIdentifier3Value, String entityIdentifier4Value, String entityIdentifier5Value, Int64 entityIdentifier1Result, Int64 entityIdentifier2Result, Int64 entityIdentifier3Result, Int64 entityIdentifier4Result, Int64 entityIdentifier5Result)
        {
            _id = id;
            _externalId = externalId;
            _containerId = containerId;
            _categoryId = categoryId;
            _entityTypeId = entityTypeId;

            _entityIdentifier1Value = entityIdentifier1Value;
            _entityIdentifier2Value = entityIdentifier2Value;
            _entityIdentifier3Value = entityIdentifier3Value;
            _entityIdentifier4Value = entityIdentifier4Value;
            _entityIdentifier5Value = entityIdentifier5Value;

            _entityIdentifier1Result = entityIdentifier1Result;
            _entityIdentifier2Result = entityIdentifier2Result;
            _entityIdentifier3Result = entityIdentifier3Result;
            _entityIdentifier4Result = entityIdentifier4Result;
            _entityIdentifier5Result = entityIdentifier5Result;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj != null && obj is EntityUniqueIdentificationMap)
            {
                EntityUniqueIdentificationMap objectToBeCompared = obj as EntityUniqueIdentificationMap;

                if (this.Id != objectToBeCompared.Id)
                {
                    return false;
                }

                if (this.ExternalId != objectToBeCompared.ExternalId)
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

                if (this.EntityIdentifier1Value != objectToBeCompared.EntityIdentifier1Value)
                {
                    return false;
                }

                if (this.EntityIdentifier2Value != objectToBeCompared.EntityIdentifier2Value)
                {
                    return false;
                }

                if (this.EntityIdentifier3Value != objectToBeCompared.EntityIdentifier3Value)
                {
                    return false;
                }

                if (this.EntityIdentifier4Value != objectToBeCompared.EntityIdentifier4Value)
                {
                    return false;
                }

                if (this.EntityIdentifier5Value != objectToBeCompared.EntityIdentifier5Value)
                {
                    return false;
                }

                if (this.EntityIdentifier1Result != objectToBeCompared.EntityIdentifier1Result)
                {
                    return false;
                }

                if (this.EntityIdentifier2Result != objectToBeCompared.EntityIdentifier2Result)
                {
                    return false;
                }

                if (this.EntityIdentifier3Result != objectToBeCompared.EntityIdentifier3Result)
                {
                    return false;
                }

                if (this.EntityIdentifier4Result != objectToBeCompared.EntityIdentifier4Result)
                {
                    return false;
                }

                if (this.EntityIdentifier5Result != objectToBeCompared.EntityIdentifier5Result)
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
            Int32 hashCode = base.GetHashCode() ^ this.Id.GetHashCode() ^
                this.ExternalId.GetHashCode() ^ this.ContainerId.GetHashCode() ^ this.CategoryId.GetHashCode() ^
                this.EntityTypeId.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of entity identifier map
        /// </summary>
        /// <returns>Xml representation of entity identifier map</returns>
        public String ToXml()
        {
            String entityIdentifierMapXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //entity identifier map node start
            xmlWriter.WriteStartElement("EntityUniqueIdentificationMap");

            #region Write entity identifier map properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier1Value", this.EntityIdentifier1Value.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier2Value", this.EntityIdentifier2Value.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier3Value", this.EntityIdentifier3Value.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier4Value", this.EntityIdentifier4Value.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier5Value", this.EntityIdentifier5Value.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier1Result", this.EntityIdentifier1Result.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier2Result", this.EntityIdentifier2Result.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier3Result", this.EntityIdentifier3Result.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier4Result", this.EntityIdentifier4Result.ToString());
            xmlWriter.WriteAttributeString("EntityIdentifier5Result", this.EntityIdentifier5Result.ToString());

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            entityIdentifierMapXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityIdentifierMapXml;
        }

        /// <summary>
        /// Get Xml representation of entity identifier map based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of entity identifier map</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String entityIdentifierMapXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                entityIdentifierMapXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //entity identifier map node start
                xmlWriter.WriteStartElement("EntityUniqueIdentification");

                #region Write entity identifier map properties

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
                xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier1Value", this.EntityIdentifier1Value.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier2Value", this.EntityIdentifier2Value.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier3Value", this.EntityIdentifier3Value.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier4Value", this.EntityIdentifier4Value.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier5Value", this.EntityIdentifier5Value.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier1Result", this.EntityIdentifier1Result.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier2Result", this.EntityIdentifier2Result.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier3Result", this.EntityIdentifier3Result.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier4Result", this.EntityIdentifier4Result.ToString());
                xmlWriter.WriteAttributeString("EntityIdentifier5Result", this.EntityIdentifier5Result.ToString());

                #endregion

                //Operation result node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                entityIdentifierMapXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return entityIdentifierMapXml;
        }

        /// <summary>
        /// Indicates if current Entity has any Identifier Result
        /// </summary>
        /// <returns></returns>
        public Boolean HasAnyResults()
        {
            bool result = false;
            if ((this.EntityIdentifier1Result > 0 || this.EntityIdentifier1Result == -100) || (this.EntityIdentifier2Result > 0 || this.EntityIdentifier2Result == -100)
                || (this.EntityIdentifier3Result > 0 || this.EntityIdentifier3Result == -100) || (this.EntityIdentifier4Result > 0 || this.EntityIdentifier4Result == -100)
                || (this.EntityIdentifier5Result > 0 || this.EntityIdentifier5Result == -100))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}
