using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for queued entity
    /// </summary>
    [DataContract]
    public class QueuedEntity : MDMObject, IDataProcessorEntity
    {
        #region Fields

        /// <summary>
        ///  Unique Id representing table
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field for the id of Entity for which impacted entities are to be processed
        /// </summary>
        private Int64 _entityId;

        /// <summary>
        /// Indicates the catalog Id of the Impacted entity
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// 
        /// </summary>
        private EntityActivityList _performedAction = EntityActivityList.UnKnown;
        
        /// <summary>
        /// Field indicating entity short name
        /// </summary>
        private String _entityName = String.Empty;

        /// <summary>
        /// Field indicating entity Long name
        /// </summary>
        private String _entityLongName = String.Empty;

        /// <summary>
        /// Field indicates list of all the attributes which has been modified
        /// </summary>
        private Collection<Int32> _attributeIdList = new Collection<Int32>();

        /// <summary>
        /// Field indicates locale list for attribute which are changed
        /// </summary>
        private Collection<LocaleEnum> _attributeLocaleIdList = new Collection<LocaleEnum>();

        /// <summary>
        /// Field indicates locale list for attribute which are changed
        /// </summary>
        private Collection<Int32> _relationshipIdList = new Collection<Int32>();

        /// <summary>
        /// Field indicates contexual entity data associated with this log entry
        /// </summary>
        private String _entityData = String.Empty;

        /// <summary>
        /// Field indicates whether loading of impacted entities is in progress
        /// </summary>
        private Boolean _isInProgress = false;

        /// <summary>
        /// Field indicates weightage of queued entity
        /// </summary>
        private Int32 _weightage = 0;

        /// <summary>
        /// indicates the PK of Entity activity Log which is referred to impacted entity
        /// </summary>
        private Int64 _entityActivityLogId = 0;

        /// <summary>
        /// defines server id who has created this entity queue
        /// </summary>
        private Int32 _serverId = 0;

        /// <summary>
        /// defines server name who has created this entity activity
        /// </summary>
        private String _serverName = String.Empty;

        /// <summary>
        /// defines last modified date time
        /// </summary>
        private DateTime? _lastModifiedDateTime= null;

        /// <summary>
        /// Defines, current entity has its own activtylogid or got it from the parent.
        /// returns true if the activityLogId is got from its parent else false
        /// </summary>
        private Boolean _isDirectChange = true;

        /// <summary>
        /// Entity Context
        /// </summary>
        private String _context = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Unique Id representing table
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Property indicates the id of Entity for which impacted entities are to be processed
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
        /// Indicates the catalog Id of the Impacted entity
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
        /// Indicates the activity action
        /// </summary>
        [DataMember]
        public EntityActivityList PerformedAction
        {
            get { return _performedAction; }
            set { _performedAction = value; }
        }

        /// <summary>
        /// Property indicating entity short name
        /// </summary>
        [DataMember]
        public String EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        /// <summary>
        /// Property indicating entity Long name
        /// </summary>
        [DataMember]
        public String EntityLongName
        {
            get { return _entityLongName; }
            set { _entityLongName = value; }
        }

        /// <summary>
        /// Property indicates list of all the attributes which has been modified
        /// </summary>
        [DataMember]
        public Collection<Int32> AttributeIdList
        {
            get
            {
                if (_attributeIdList != null && _attributeIdList.Count > 0)
                {
                    _attributeIdList = new Collection<Int32>(_attributeIdList.Distinct().ToList<Int32>());
                }
                else
                {
                    _attributeIdList = new Collection<Int32>();
                }

                if (_attributeIdList.Contains(0))
                    _attributeIdList.Remove(0);

                return _attributeIdList;
            }
            set
            {
                _attributeIdList = value;
            }
        }

        /// <summary>
        /// Property indicates locale list for attribute which are changed
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> AttributeLocaleIdList
        {
            get
            {
                if(this._attributeLocaleIdList != null && this._attributeLocaleIdList.Count > 0)
                {
                    this._attributeLocaleIdList = new Collection<LocaleEnum>(this._attributeLocaleIdList.Where(l => l != LocaleEnum.UnKnown).Distinct().ToList<LocaleEnum>());
                }
                else
                {
                    this._attributeLocaleIdList = new Collection<LocaleEnum>();
                }

                return this._attributeLocaleIdList;
            }
            set
            {
                _attributeLocaleIdList = value;

            }
        }

        /// <summary>
        /// Property indicates list of all the relationships which has been modified
        /// </summary>
        [DataMember]
        public Collection<Int32> RelationshipIdList
        {
            get
            {
                if (_relationshipIdList != null && _relationshipIdList.Count > 0)
                {
                    _relationshipIdList = new Collection<Int32>(_relationshipIdList.Distinct().ToList<Int32>());
                }
                else
                {
                    _relationshipIdList = new Collection<Int32>();
                }
                return _relationshipIdList;
            }
            set
            {
                _relationshipIdList = value;

            }
        }

        /// <summary>
        /// Property indicates contexual entity data associated with this entity activity log entry
        /// </summary>
        [DataMember]
        public String EntityData
        {
            get
            {
                return this._entityData;
            }
            set
            {
                this._entityData = value;

            }
        }

        /// <summary>
        /// Property indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        [DataMember]
        public Boolean IsInProgress
        {
            get
            {
                return this._isInProgress;
            }
            set
            {
                this._isInProgress = value;

            }
        }

        /// <summary>
        /// indicates weightage of an queued entity
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get
            {
                return this._weightage;
            }
            set
            {
                this._weightage = value;
            }
        }

        /// <summary>
        /// indicates the PK of Impacted Entity Log which is referred to queued entity
        /// </summary>
        [DataMember]
        public Int64 EntityActivityLogId
        {
            get
            {
                return this._entityActivityLogId;
            }
            set
            {
                this._entityActivityLogId = value;
            }
        }

        /// <summary>
        /// server id
        /// </summary>
        [DataMember]
        public Int32 ServerId
        {
            get
            {
                return this._serverId;
            }
            set
            {
                this._serverId = value;
            }
        }

        /// <summary>
        /// server name
        /// </summary>
        [DataMember]
        public String ServerName
        {
            get
            {
                return this._serverName;
            }
            set
            {
                this._serverName = value;
            }
        }

        /// <summary>
        ///  Property indicates the last modified time of this queued entity
        /// </summary>
        [DataMember]
        public DateTime? LastModifiedDateTime
        {
            get
            {
                return this._lastModifiedDateTime;
            }
            set
            {
                this._lastModifiedDateTime = value;
            }
        }

        /// <summary>
        /// Defines, current entity has its own activtylogid or got it from the parent.
        /// returns true if the activityLogId is got from its parent else false
        /// </summary>
        [DataMember]
        public Boolean IsDirectChange
        {
            get { return _isDirectChange; }
            set { _isDirectChange = value; }
        }


        /// <summary>
        /// Entity Context
        /// </summary>
        [DataMember]
        public String Context
        {
            get
            {
                return this._context;
            }
            set
            {
                this._context = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public QueuedEntity()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public QueuedEntity(String valuesAsxml)
        {
            LoadQueuedEntity(valuesAsxml);
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadQueuedEntity(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "QueuedEntity")
                        {
                            #region Read Attribute Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("PerformedAction"))
                                {
                                    EntityActivityList action = EntityActivityList.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.PerformedAction = action;
                                }
                                if (reader.MoveToAttribute("EntityName"))
                                {
                                    this.EntityName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("EntityLongName"))
                                {
                                    this.EntityLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("AttributeIdList"))
                                {
                                    this.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("AttributeLocaleIdList"))
                                {
                                    this.AttributeLocaleIdList = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("RelationshipIdList"))
                                {
                                    this.RelationshipIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("EntityData"))
                                {
                                    this.EntityData = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("IsInProgress"))
                                {
                                    this.IsInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("Weightage"))
                                {
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("EntityActivityLogId"))
                                {
                                    this.EntityActivityLogId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("ServerId"))
                                {
                                    this.ServerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("ServerName"))
                                {
                                    this.ServerName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LastModDateTime"))
                                {
                                    this.LastModifiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                            }

                            #endregion
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

        #endregion Private Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of QueuedEntity
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String denormXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String attributeIdList = String.Empty;
            String attributeLocaleIdList = String.Empty;
            String relationshipIdList = String.Empty;

            if (this.AttributeIdList != null && this.AttributeIdList.Count > 0)
            {
                attributeIdList = ValueTypeHelper.JoinCollection(this.AttributeIdList, ",");
            }

            if (this.AttributeLocaleIdList != null && this.AttributeLocaleIdList.Count > 0)
            {
                attributeIdList = ValueTypeHelper.JoinCollectionGetLocaleIdList(this.AttributeLocaleIdList, ",");
            }

            if (this.RelationshipIdList != null && this.RelationshipIdList.Count > 0)
            {
                relationshipIdList = ValueTypeHelper.JoinCollection(this.RelationshipIdList, ",");
            }

            //QueuedEntity node start
            xmlWriter.WriteStartElement("QueuedEntity");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("PerformedAction", this.PerformedAction.ToString());
            xmlWriter.WriteAttributeString("EntityName", this.EntityName);
            xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName);
            xmlWriter.WriteAttributeString("AttributeIdList", attributeIdList);
            xmlWriter.WriteAttributeString("AttributeLocaleIdList", attributeLocaleIdList);
            xmlWriter.WriteAttributeString("RelationshipIdList", relationshipIdList);
            xmlWriter.WriteAttributeString("EntityData", this.EntityData);
            xmlWriter.WriteAttributeString("IsInProgress", this.IsInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
            xmlWriter.WriteAttributeString("EntityActivityLogId", this.EntityActivityLogId.ToString());
            xmlWriter.WriteAttributeString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteAttributeString("ServerName", this.ServerName.ToString());
            xmlWriter.WriteAttributeString("LastModifiedDateTime", this.LastModifiedDateTime.ToString());

            #endregion

            //Denorm Result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            denormXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return denormXml;
        }

        /// <summary>
        /// Get Xml representation of QueuedEntity
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            // No serialization implemented for now...
            return this.ToXml();
        }
 
        #endregion
    }
}
