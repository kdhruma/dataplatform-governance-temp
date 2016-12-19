using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DQMEntityTasksQueueItem
    /// </summary>
    [DataContract]
    [KnownType(typeof(DQMJobType))]
    public class DQMEntityTasksQueueItem : MDMObject, IDQMEntityTasksQueueItem
    {
        #region Fields

        /// <summary>
        /// Field indicates queue item Id
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field indicates parent Job Id
        /// </summary>
        private Int64 _parentJobId = 0;

        /// <summary>
        /// Field for the Id of Entity
        /// </summary>
        private Int64 _entityId;

        /// <summary>
        /// Field  indicates the catalog Id of entity
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field indicates Job type
        /// </summary>
        private DQMJobType _jobType = DQMJobType.Unknown;
        
        /// <summary>
        /// Field indicates whether item is in processing state
        /// </summary>
        private Boolean _isInProgress = false;

        /// <summary>
        /// Field indicates whether item is processed
        /// </summary>
        private Boolean _isProcessed = false;

        /// <summary>
        /// Field indicates weightage of queued entity
        /// </summary>
        private Int16? _weightage = null;

        /// <summary>
        /// Field indicates last modified date time
        /// </summary>
        private DateTime? _lastModifiedDateTime = null;

        /// <summary>
        /// Field indicates server Id who has created this entity task
        /// </summary>
        private Int32? _serverId = null;

        /// <summary>
        /// Field indicates server name who has created this entity task
        /// </summary>
        private String _serverName = null;

        /// <summary>
        /// Field indicates entity Short name
        /// </summary>
        private String _entityName = null;

        /// <summary>
        /// Field indicates entity Long name
        /// </summary>
        private String _entityLongName = null;

        /// <summary>
        /// Field indicates Parent Job Context
        /// </summary>
        private String _parentJobContext = null;

        /// <summary>
        /// Field indicates task Context
        /// </summary>
        private String _context = null;

        /// <summary>
        /// Field indicates DQM Profile id
        /// </summary>
        private Int32? _profileId = null;

        /// <summary>
        /// Field indicates parent job IsSystem status
        /// </summary>
        private Boolean _parentJobIsSystem = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property indicates queue item Id
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
        /// Property indicates parent Job Id
        /// </summary>
        [DataMember]
        public Int64 ParentJobId
        {
            get
            {
                return this._parentJobId;
            }
            set
            {
                this._parentJobId = value;
            }
        }

        /// <summary>
        /// Property indicates the Id of Entity
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
        /// Property indicates the catalog Id of entity
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
        /// Property indicates Job type
        /// </summary>
        [DataMember]
        public DQMJobType JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }

        /// <summary>
        /// Property indicates whether item is in processing state
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
        /// Property indicates whether item is processed
        /// </summary>
        [DataMember]
        public Boolean IsProcessed
        {
            get
            {
                return this._isProcessed;
            }
            set
            {
                this._isProcessed = value;
            }
        }

        /// <summary>
        /// Property indicates weightage of an queued entity
        /// </summary>
        [DataMember]
        public Int16? Weightage
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
        /// Property indicates server Id who has created this entity task
        /// </summary>
        [DataMember]
        public Int32? ServerId
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
        /// Property indicates server name who has created this entity task
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
        /// Property indicates entity Short name
        /// </summary>
        [DataMember]
        public String EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        /// <summary>
        /// Property indicates entity Long name
        /// </summary>
        [DataMember]
        public String EntityLongName
        {
            get { return _entityLongName; }
            set { _entityLongName = value; }
        }

        /// <summary>
        /// Property indicates Parent Job Context
        /// </summary>
        [DataMember]
        public String ParentJobContext
        {
            get { return _parentJobContext; }
            set { _parentJobContext = value; }
        }

        /// <summary>
        /// Property indicates task Context
        /// </summary>
        [DataMember]
        public String Context
        {
            get { return this._context; }
            set { this._context = value; }
        }

        /// <summary>
        /// Property indicates Profile Id
        /// </summary>
        [DataMember]
        public Int32? ProfileId
        {
            get { return this._profileId; }
            set { this._profileId = value; }
        }

        /// <summary>
        /// Property indicates parent job IsSystem status
        /// </summary>
        [DataMember]
        public Boolean ParentJobIsSystem
        {
            get { return this._parentJobIsSystem; }
            set { this._parentJobIsSystem = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DQMEntityTasksQueueItem()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value of DQMEntityTasksQueueItem (except of ParentJobContext and Context properties)</param>
        public DQMEntityTasksQueueItem(String valuesAsxml)
        {
            LoadDQMEntityTasksQueueItem(valuesAsxml);
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadDQMEntityTasksQueueItem(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DQMEntityTasksQueueItem")
                        {
                            #region Read Attribute Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("ParentJobId"))
                                {
                                    this.ParentJobId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("JobType"))
                                {
                                    DQMJobType action = DQMJobType.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.JobType = action;
                                }
                                if (reader.MoveToAttribute("IsInProgress"))
                                {
                                    this.IsInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsProcessed"))
                                {
                                    this.IsProcessed = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("Weightage"))
                                {
                                    this.Weightage = ValueTypeHelper.ConvertToNullableInt16(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("LastModDateTime"))
                                {
                                    this.LastModifiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ServerId"))
                                {
                                    this.ServerId = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ServerName"))
                                {
                                    this.ServerName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("EntityName"))
                                {
                                    this.EntityName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("EntityLongName"))
                                {
                                    this.EntityLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ParentJobIsSystem"))
                                {
                                    this.ParentJobIsSystem = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
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
        /// Get Xml representation of DQMEntityTasksQueueItem (except of ParentJobContext and Context properties)
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String denormXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DQMEntityTasksQueueItem node start
            xmlWriter.WriteStartElement("DQMEntityTasksQueueItem");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ParentJobId", this.ParentJobId.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("JobType", this.JobType.ToString());
            xmlWriter.WriteAttributeString("IsInProgress", this.IsInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsProcessed", this.IsProcessed.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
            xmlWriter.WriteAttributeString("LastModifiedDateTime", this.LastModifiedDateTime.ToString());
            xmlWriter.WriteAttributeString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteAttributeString("ServerName", this.ServerName ?? "");
            xmlWriter.WriteAttributeString("EntityName", this.EntityName);
            xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName);
            xmlWriter.WriteAttributeString("ParentJobIsSystem", this.ParentJobIsSystem.ToString().ToLowerInvariant());

            #endregion

            //DQMEntityTasksQueueItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            denormXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return denormXml;
        }

        /// <summary>
        /// Get Xml representation of DQMEntityTasksQueueItem (except of ParentJobContext and Context properties)
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
