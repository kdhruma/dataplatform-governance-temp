using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies entity level history for single entity
    /// </summary>
    [DataContract]
    public class EntityHistory : InterfaceContractCollection<IEntityHistoryRecord, EntityHistoryRecord>, IEntityHistory
    {
        #region Fields

        /// <summary>
        /// Field denotes entity id for current history 
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Field denotes entity long name for current history 
        /// </summary>
        private String _entityLongName = String.Empty;

        /// <summary>
        /// Field denoting current entity's entity type long name for which history is requested.
        /// </summary>
        private String _entityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting current entity's catalog long name for which history is requested.        
        /// </summary>
        private String _entityCatalogLongName = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityHistory() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityHistory(String valuesAsXml)
        {
            LoadEntityHistory(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting current entity id for which history is requested.
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Property denoting current entity long name for which history is requested.
        /// </summary>
        [DataMember]
        public String EntityLongName
        {
            get { return _entityLongName; }
            set { _entityLongName = value; }
        }

        /// <summary>
        /// Property denoting current entity's entity type long name for which history is requested.
        /// </summary>
        [DataMember]
        public String EntityTypeLongName
        {
            get { return _entityTypeLongName; }
            set { _entityTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting current entity's catalog long name for which history is requested.        
        /// </summary>
        [DataMember]
        public String EntityCatalogLongName
        {
            get { return _entityCatalogLongName; }
            set { _entityCatalogLongName = value; }
        }
        #endregion

        #region Methods
        
        #region Public Methods
        
        /// <summary>
        /// Replaces old EntityHistoryRecords and sets EntityHistoryRecord collection to current entity history.
        /// </summary>
        /// <param name="entityHistoryRecords">Collection of EntityHistoryRecords</param>
        public void SetEntityHistoryRecords(Collection<EntityHistoryRecord> entityHistoryRecords)
        {
            if (entityHistoryRecords != null)
            {
                this._items = entityHistoryRecords;
            }
        }

        /// <summary>
        /// Get Xml representation of EntityHistory object
        /// </summary>
        /// <returns>Xml string representing the EntityHistory</returns>
        public String ToXml()
        {
            String entityHistoryXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            xmlWriter.WriteStartElement("EntityHistory");

            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName.ToString());
            xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName.ToString());
            xmlWriter.WriteAttributeString("EntityCatalogLongName", this.EntityCatalogLongName.ToString());

            foreach (EntityHistoryRecord entityHistoryRecord in this._items)
            {
                xmlWriter.WriteRaw(entityHistoryRecord.ToXml());
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            entityHistoryXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityHistoryXml;
        }

        /// <summary>
        /// Clone entity history.
        /// </summary>
        /// <returns>cloned entity history object.</returns>
        public IEntityHistory Clone()
        {
            EntityHistory clonedEntityHistory = new EntityHistory();

            if (this._items != null && this._items.Count > 0)
            {
                clonedEntityHistory.EntityId = this.EntityId;
                clonedEntityHistory.EntityLongName = this.EntityLongName;
                clonedEntityHistory.EntityTypeLongName = this.EntityTypeLongName;
                clonedEntityHistory.EntityCatalogLongName = this.EntityCatalogLongName;

                foreach (EntityHistoryRecord entityHistoryRecord in this._items)
                {
                    EntityHistoryRecord clonedEntityHistoryRecord = entityHistoryRecord.Clone();
                    clonedEntityHistory.Add(clonedEntityHistoryRecord);
                }
            }
            return clonedEntityHistory;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">EntityHistory Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(EntityHistory objectToBeCompared, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.EntityId != objectToBeCompared.EntityId)
                    return false;
            }

            if (this.EntityLongName != objectToBeCompared.EntityLongName)
                return false;

            if (this.EntityTypeLongName != objectToBeCompared.EntityTypeLongName)
                return false;

            if (this.EntityCatalogLongName != objectToBeCompared.EntityCatalogLongName)
                return false;

            if (this._items.Count == objectToBeCompared._items.Count)
            {
                for (Int32 i = 0; i < this._items.Count; i++)
                {
                    if (!this._items[i].Equals(objectToBeCompared._items[i],compareIds))
                        return false;
                }
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetEntityHistory">Indicates EntityHistory Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates flag denoting whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>Returns true if the specified Object is equal to the current object; otherwise false.</returns>
        public Boolean IsSuperSetOf(EntityHistory subsetEntityHistory, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.EntityId != subsetEntityHistory.EntityId)
                    return false;
            }

            if (this.EntityLongName != subsetEntityHistory.EntityLongName)
                return false;

            if (this.EntityTypeLongName != subsetEntityHistory.EntityTypeLongName)
                return false;

            if (this.EntityCatalogLongName != subsetEntityHistory.EntityCatalogLongName)
                return false;

            if (subsetEntityHistory._items != null && subsetEntityHistory._items.Count > 0)
            {
                foreach (EntityHistoryRecord childEntityHistory in subsetEntityHistory)
                {
                    if (this._items != null && this._items.Count > 0)
                    {
                        foreach (EntityHistoryRecord sourceEntityHistory in this._items)
                        {
                            if (sourceEntityHistory.IsSuperSetOf(childEntityHistory, compareIds))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }

            return true;
        }
        
        /// <summary>
        /// Gets entity history record based in audit ref id and attribute id.
        /// </summary>
        /// <param name="auditRefId">modification Reference id </param>
        /// <param name="modifiedAttributeId">Id of an attribute which is modified.</param>
        /// <returns>EntityHistoryRecord</returns>
        public EntityHistoryRecord GetEntityHistoryRecord(Int64 auditRefId, Int32 modifiedAttributeId)
        {
            EntityHistoryRecord entityHistoryRecord = null;

            if (_items != null)
            {
                foreach (EntityHistoryRecord items in this._items)
                {
                    if (items.AuditRefId == auditRefId && items.ChangedData_AttributeId == modifiedAttributeId)
                    {
                        entityHistoryRecord = items;
                        break;
                    }
                }
            }

            return entityHistoryRecord;
        }

        #endregion

        #region Private Methods

        private void LoadEntityHistory(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistory")
                        {
                            #region Read EntityHistory Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    Int64 entityId = -1;
                                    Int64.TryParse(reader.ReadContentAsString(),out entityId);
                                    this.EntityId = entityId;
                                }

                                if (reader.MoveToAttribute("EntityLongName"))
                                {
                                    this.EntityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityTypeLongName"))
                                {
                                    this.EntityTypeLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityCatalogLongName"))
                                {
                                    this.EntityCatalogLongName = reader.ReadContentAsString();
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryRecord")
                        {
                            #region Read EntityHistoryRecord Properties

                            String entityHistoryRecordXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityHistoryRecordXml))
                            {
                                EntityHistoryRecord entityHistoryRecord = new EntityHistoryRecord(entityHistoryRecordXml);
                                if (entityHistoryRecord != null)
                                {
                                    this.Add(entityHistoryRecord);
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
