using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the details of search data for an Entity
    /// </summary>
    [DataContract]
    public class EntitySearchData : MDMObject, IEntitySearchData
    {
        #region Fields

        /// <summary>
        /// Field denoting Id of EntitySerachData
        /// </summary>
        Int64 _id = 0;

        /// <summary>
        /// Field denoting Id of an Entity.
        /// </summary>
        Int64 _entityId = 0;
        
        /// <summary>
        /// Field denoting Id of catalog id
        /// </summary>
        Int32 _containerId = 0;

        /// <summary>
        /// Field denoting value that needs to be searched.
        /// </summary>
        String _searchValue = String.Empty;

        /// <summary>
        /// Field denoting Keyvalue for search.
        /// </summary>
        String _keyValue = String.Empty;

        /// <summary>
        /// Field denoting IdPath for Serach.
        /// </summary>
        String _idPath = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the new instance of EntitySearchData
        /// </summary>
        public EntitySearchData()
        {
            
        }

        /// <summary>
        /// Instantiates the new instance of EntitySearchData
        /// </summary>
        /// <param name="id">Indicates Id of an EntitySearchData.</param>
        /// <param name="entityId">Indicates Id of an Entity.</param>
        /// <param name="containerId">Indicates container id of entity</param>
        /// <param name="searchValue">Indicates Value that needs to be searched.</param>
        /// <param name="keyValue">Indicates KeyValue for Search.</param>
        /// <param name="idPath">Indicates Id Path for Search</param>
        public EntitySearchData(Int64 id, Int64 entityId,Int32 containerId, String searchValue, String keyValue, String idPath)
            : base()
        {
            this._id = id;
            this._entityId = entityId;
            this._containerId = containerId;
            this._searchValue = searchValue;
            this._keyValue = keyValue;
            this._idPath = idPath;
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public EntitySearchData(String valuesAsXml)
        {
            LoadEntitySearchData(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Id of an EntitySearchData
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Property denoting Id of an Entity.
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get
            {
                return this._entityId;
            }
            set
            {
                this._entityId = value;
            }
        }

        /// <summary>
        /// Property denoting Id of an Catalog.
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
        /// Property denoting value that needs to be searched.
        /// </summary>
        [DataMember]
        public String SearchValue
        {
            get
            {
                return _searchValue;
            }
            set
            {
                this._searchValue = value;
            }
        }

        /// <summary>
        /// Property denoting Keyvalue for search.
        /// </summary>
        [DataMember]
        public String KeyValue
        {
            get
            {
                return _keyValue;
            }
            set
            {
                this._keyValue = value;
            }
        }

        /// <summary>
        /// Property denoting IdPath for Serach.
        /// </summary>
        [DataMember]
        public String IdPath
        {
            get
            {
                return _idPath;
            }
            set
            {
                this._idPath = value;
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
        public override Boolean Equals(Object obj)
        {
            if (obj != null && obj is EntitySearchData)
            {
                EntitySearchData objectToBeCompared = obj as EntitySearchData;

                if (this.Id != objectToBeCompared.Id)
                    return false;

                if (this.EntityId != objectToBeCompared.EntityId)
                    return false;

                if (this.ContainerId != objectToBeCompared.ContainerId)
                    return false;

                if (this.SearchValue != objectToBeCompared.SearchValue)
                    return false;

                if (this.KeyValue != objectToBeCompared.KeyValue)
                    return false;

                if (this.IdPath != objectToBeCompared.IdPath)
                    return false;

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
            //TODO
            //Put NULL checks for string properties
            Int32 hashCode = base.GetHashCode() ^
                             this.Id.GetHashCode() ^
                             this.EntityId.GetHashCode() ^
                             this.ContainerId.GetHashCode();

            if (!String.IsNullOrWhiteSpace(this.SearchValue))
                hashCode = hashCode ^ this.SearchValue.GetHashCode();

            if (!String.IsNullOrWhiteSpace(this.KeyValue))
                hashCode = hashCode ^ this.KeyValue.GetHashCode();

            if (!String.IsNullOrWhiteSpace(this.IdPath))
                hashCode = hashCode ^ this.IdPath.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of EntitySearchData
        /// </summary>
        /// <returns>Xml representation of EntitySearchData</returns>
        public override String ToXml()
        {
            String EntitySearchDataXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntitySearchData node start
            xmlWriter.WriteStartElement("EntitySearchData");

            #region Write Entity Map properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("CatalogId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("SearchValue", this.SearchValue);
            xmlWriter.WriteAttributeString("KeyValue", this.KeyValue);
            xmlWriter.WriteAttributeString("IdPath", this.IdPath);
            
            #endregion

            //EntitySearchData node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            EntitySearchDataXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return EntitySearchDataXml;
        }

        /// <summary>
        /// Get Xml representation of EntitySearchData based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of EntitySearchData</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String entitySearchDataXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                entitySearchDataXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //EntitySearchData node start
                xmlWriter.WriteStartElement("EntitySearchData");

                #region Write Entity Map properties

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                xmlWriter.WriteAttributeString("CatalogId", this.ContainerId.ToString());
                xmlWriter.WriteAttributeString("SearchValue", this.SearchValue);
                xmlWriter.WriteAttributeString("KeyValue", this.KeyValue);
                xmlWriter.WriteAttributeString("IdPath", this.IdPath);

                #endregion

                //EntitySearchData node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                entitySearchDataXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return entitySearchDataXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load EntitySearchData Object from Xml as String
        /// </summary>
        /// <param name="valuesAsXml">Specifies ObjectXml String</param>
        private void LoadEntitySearchData(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntitySearchData")
                    {
                        #region Read EntitySearchData Properties

                        if (reader.HasAttributes)
                        {
                            //TODO :: User ValutTypeHelper for type conversion
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(),0);
                            }

                            if (reader.MoveToAttribute("EntityId"))
                            {
                                this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("CatalogId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("SearchValue"))
                            {
                                this.SearchValue = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("KeyValue"))
                            {
                                this.KeyValue = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("IdPath"))
                            {
                                this.IdPath = reader.ReadContentAsString();
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
