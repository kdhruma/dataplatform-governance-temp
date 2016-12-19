using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MDM.BusinessObjects
{
    using MDM.Core;
    using System.Runtime.Serialization;
    using System.IO;
    using System.Xml;
    using System.Collections.ObjectModel;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for entity activity log
    /// </summary>
    [DataContract]
    [KnownType(typeof(EntityActivityList))]
    public class EntityActivityLog : MDMObject, IDataProcessorEntity
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
        /// Indicates the catalog name of the Impacted entity
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private EntityActivityList _performedAction = EntityActivityList.UnKnown;

        /// <summary>
        /// Field indicates weightage
        /// </summary>
        private Int32 _weightage = 0;

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
        private Collection<Int64> _relationshipIdList = new Collection<Int64>();

        /// <summary>
        /// Field denoting relationship process mode which is converted decimal of binary indication of processing settings
        /// <para>
        /// 0th index -> WhereUsed denormalized relationships - Weightage 1
        /// 1st index -> Hierarchy denormalized relationships - Weightage 2
        /// 2nd index -> Extension denormalized relationships - Weightage 4
        /// 3rd index -> Relationships of Relationship Tree - Weightage 8
        /// 0110 stands for Hierarchy and Extension relationship processing and will be having a value 6
        /// </para>
        /// </summary>
        private Int32 _relationshipProcessMode = 0;

        /// <summary>
        /// Field indicates contexual entity data associated with this log entry
        /// </summary>
        private String _entityData = String.Empty;

        /// <summary>
        /// Field indicates whether loading of impacted entities is in progress
        /// </summary>
        private Boolean _isLoadingInProgress = false;

        /// <summary>
        /// Field indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        private Boolean _isLoaded = false;

        /// <summary>
        /// Field indicates whether all the loaded impacted entites in the impacted Entity table has processed
        /// </summary>
        private Boolean _isProcessed = false;

        /// <summary>
        /// Field indicates the time when the impacted entities loading in to impacted Entity table for processing started
        /// </summary>
        private DateTime? _loadStartTime = null;

        /// <summary>
        /// Field indicates the time when the impacted entities loading in to impacted Entity table for processing ended
        /// </summary>
        private DateTime? _loadEndTime = null;

        /// <summary>
        /// Field indicates the processing start time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        private DateTime? _processStartTime = null;

        /// <summary>
        /// Field indicates the processing end time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        private DateTime? _processEndTime = null;

        /// <summary>
        /// Field indicates the Created Date time of the entites in the Activity log table 
        /// </summary>
        private DateTime? _createdDateTime = null;

        /// <summary>
        /// number impacted entities effected by the entity change
        /// </summary>
        private Int64 _impactedCount = 0;

        /// <summary>
        /// number impacted entities effected by the entity change to be processed
        /// </summary>
        private Int64 _pendingImpactedEntityCount = 0;

        /// <summary>
        /// defines server id who has created this entity activity
        /// </summary>
        private Int32 _serverId = 0;

        /// <summary>
        /// defines server name who has created this entity activity
        /// </summary>
        private String _serverName = String.Empty;

        /// <summary>
        /// defines user name who has created this entity activity
        /// </summary>
        private String _userName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private String _context = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private Int64 _parentActivityLogId = default(Int64);

        /// <summary>
        /// Field denoting whether the current entity is having direct changes or having changes impacted from related entity
        /// </summary>
        private Boolean _isDirectChange = true;

        /// <summary>
        /// Field denoting the rule map context name
        /// </summary>
        private String _mdmRuleMapContextName = String.Empty;

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
        /// Indicates the catalog name of the Impacted entity
        /// </summary>
        [DataMember]
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
        /// Indicates the activity action
        /// </summary>
        [DataMember]
        public EntityActivityList PerformedAction
        {
            get { return _performedAction; }
            set { _performedAction = value; }
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
                if (_attributeLocaleIdList != null && _attributeLocaleIdList.Count > 0)
                {
                    _attributeLocaleIdList = new Collection<LocaleEnum>(_attributeLocaleIdList.Where(l => l != LocaleEnum.UnKnown).Distinct().ToList<LocaleEnum>());
                }
                else
                {
                    _attributeLocaleIdList = new Collection<LocaleEnum>();
                }

                return _attributeLocaleIdList;
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
        public Collection<Int64> RelationshipIdList
        {
            get
            {
                if (_relationshipIdList != null && _relationshipIdList.Count > 0)
                {
                    _relationshipIdList = new Collection<Int64>(_relationshipIdList.Distinct().ToList<Int64>());
                }
                else
                {
                    _relationshipIdList = new Collection<Int64>();
                }
                return _relationshipIdList;
            }
            set
            {
                _relationshipIdList = value;
            }
        }

        /// <summary>
        /// Field denoting relationship process mode which is converted decimal of binary indication of processing settings
        /// <para>
        /// 0th index -> WhereUsed denormalized relationships - Weightage 1
        /// 1st index -> Hierarchy denormalized relationships - Weightage 2
        /// 2nd index -> Extension denormalized relationships - Weightage 4
        /// 3rd index -> Relationships of Relationship Tree - Weightage 8
        /// 0110 stands for Hierarchy and Extension relationship processing and will be having a value 6
        /// </para>
        /// </summary>
        [DataMember]
        public Int32 RelationshipProcessMode
        {
            get
            {
                return _relationshipProcessMode;
            }
            set
            {
                _relationshipProcessMode = value;
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
        public Boolean IsLoadingInProgress
        {
            get
            {
                return this._isLoadingInProgress;
            }
            set
            {
                this._isLoadingInProgress = value;

            }
        }

        /// <summary>
        /// Property indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        [DataMember]
        public Boolean IsLoaded
        {
            get
            {
                return this._isLoaded;
            }
            set
            {
                this._isLoaded = value;

            }
        }

        /// <summary>
        ///  Property indicates whether all the loaded impacted entites in the impacted Entity table has processed
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
        /// Property indicates the time when the impacted entities loading in to impacted Entity table for processing started
        /// </summary>
        [DataMember]
        public DateTime? LoadStartTime
        {
            get
            {
                return this._loadStartTime;
            }
            set
            {
                this._loadStartTime = value;
            }
        }

        /// <summary>
        /// Property indicates the time when the impacted entities loading in to impacted Entity table for processing ended
        /// </summary>
        [DataMember]
        public DateTime? LoadEndTime
        {
            get
            {
                return this._loadEndTime;
            }
            set
            {
                this._loadEndTime = value;
            }
        }

        /// <summary>
        /// Property indicates the processing start time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        [DataMember]
        public DateTime? ProcessStartTime
        {
            get
            {
                return this._processStartTime;
            }
            set
            {
                this._processStartTime = value;
            }
        }

        /// <summary>
        ///  Property indicates the processing end time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        [DataMember]
        public DateTime? ProcessEndTime
        {
            get
            {
                return this._processEndTime;
            }
            set
            {
                this._processEndTime = value;
            }
        }

        /// <summary>
        /// Property indicates the Created Date time of the entites in the Activity log table 
        /// </summary>
        [DataMember]
        public DateTime? CreatedDateTime
        {
            get
            {
                return this._createdDateTime;
            }
            set
            {
                this._createdDateTime = value;
            }
        }

        /// <summary>
        /// number impacted entities effected by the entity change
        /// </summary>
        [DataMember]
        public Int64 ImpactedCount
        {
            get
            {
                return this._impactedCount;
            }
            set
            {
                this._impactedCount = value;
            }
        }

        /// <summary>
        /// number of impacted entities effected by the entity change to be processed
        /// </summary>
        [DataMember]
        public Int64 PendingImpactedEntityCount
        {
            get
            {
                return this._pendingImpactedEntityCount;
            }
            set
            {
                this._pendingImpactedEntityCount = value;
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
        /// user name
        /// </summary>
        [DataMember]
        public new String UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }

        /// <summary>
        /// Unique Id representing table
        /// Note:This is used only for logging exception in the Processor
        /// </summary>
        [DataMember]
        public Int64 EntityActivityLogId
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
        /// Unique Id of the parent Entity representing table, of which the current entity came in for process
        /// Note:This is used only for logging exception in the Processor
        /// </summary>
        [DataMember]
        public Int64 ParentEntityActivityLogId
        {
            get
            {
                return this._parentActivityLogId;
            }

            set
            {
                this._parentActivityLogId = value;
            }
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

        /// <summary>
        /// Property denoting whether the current entity is having direct changes or having changes impacted from related entity
        /// </summary>
        public Boolean IsDirectChange
        {
            get { return _isDirectChange; }
            set { _isDirectChange = value; }
        }

        /// <summary>
        /// Property denoting rule map context name
        /// </summary>
        [DataMember]
        public String MDMRuleMapContextName
        {
            get
            {
                return this._mdmRuleMapContextName;
            }
            set
            {
                this._mdmRuleMapContextName = value;
            }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EntityActivityLog()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public EntityActivityLog(String valuesAsxml)
        {
            LoadEntityActivityLog(valuesAsxml);
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityActivityLog(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityActivityLog")
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
                                if (reader.MoveToAttribute("ContainerName"))
                                {
                                    this.ContainerName = reader.ReadContentAsString();
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
                                    this.RelationshipIdList = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("RelationshipProcessMode"))
                                {
                                    this.RelationshipProcessMode = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("EntityData"))
                                {
                                    this.EntityData = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("IsLoadingInProgress"))
                                {
                                    this.IsLoadingInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsLoaded"))
                                {
                                    this.IsLoaded = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsProcessed"))
                                {
                                    this.IsProcessed = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("ProgramName"))
                                {
                                    this.ProgramName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LoadStartTime"))
                                {
                                    this.LoadStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("LoadEndTime"))
                                {
                                    this.LoadEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ProcessStartTime"))
                                {
                                    this.ProcessStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ProcessEndTime"))
                                {
                                    this.ProcessEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ImpactedCount"))
                                {
                                    this.ImpactedCount = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("PendingCount"))
                                {
                                    this.PendingImpactedEntityCount = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("UserName"))
                                {
                                    this.UserName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ServerId"))
                                {
                                    this.ServerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("ServerName"))
                                {
                                    this.ServerName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Context"))
                                {
                                    this.Context = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ParentEntityActivityLogId"))
                                {
                                    this.ParentEntityActivityLogId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), default(Int64));
                                }

                                if (reader.MoveToAttribute("MDMRuleMapContextName"))
                                {
                                    this.MDMRuleMapContextName = reader.ReadContentAsString().ToString();
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
        /// Get Xml representation of EntityActivityLog
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
                attributeLocaleIdList = ValueTypeHelper.JoinCollectionGetLocaleIdList(this.AttributeLocaleIdList, ",");
            }

            if (this.RelationshipIdList != null && this.RelationshipIdList.Count > 0)
            {
                relationshipIdList = ValueTypeHelper.JoinCollection(this.RelationshipIdList, ",");
            }

            //EntityActivityLog node start
            xmlWriter.WriteStartElement("EntityActivityLog");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
            xmlWriter.WriteAttributeString("PerformedAction", this.PerformedAction.ToString());
            xmlWriter.WriteAttributeString("EntityName", this.EntityName);
            xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName);
            xmlWriter.WriteAttributeString("AttributeIdList", attributeIdList);
            xmlWriter.WriteAttributeString("AttributeLocaleIdList", attributeLocaleIdList);
            xmlWriter.WriteAttributeString("RelationshipIdList", relationshipIdList);
            xmlWriter.WriteAttributeString("RelationshipProcessMode", this.RelationshipProcessMode.ToString());
            xmlWriter.WriteAttributeString("EntityData", this.EntityData);
            xmlWriter.WriteAttributeString("IsLoadingInProgress", this.IsLoadingInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsLoaded", this.IsLoaded.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsProcessed", this.IsProcessed.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ProgramName", this.ProgramName);
            xmlWriter.WriteAttributeString("LoadStartTime", this.LoadStartTime.ToString());
            xmlWriter.WriteAttributeString("LoadEndTime", this.LoadEndTime.ToString());
            xmlWriter.WriteAttributeString("ProcessStartTime", this.ProcessStartTime.ToString());
            xmlWriter.WriteAttributeString("ProcessEndTime", this.ProcessEndTime.ToString());
            xmlWriter.WriteAttributeString("CreateDateTime", this.CreatedDateTime.ToString());
            xmlWriter.WriteAttributeString("ImpactedCount", this.ImpactedCount.ToString());
            xmlWriter.WriteAttributeString("PendingCount", this.PendingImpactedEntityCount.ToString());
            xmlWriter.WriteAttributeString("UserName", this.UserName);
            xmlWriter.WriteAttributeString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteAttributeString("ServerName", this.ServerName.ToString());
            xmlWriter.WriteAttributeString("Context", this.Context);
            xmlWriter.WriteAttributeString("ParentEntityActivityLogId", this.ParentEntityActivityLogId.ToString());
            xmlWriter.WriteAttributeString("MDMRuleMapContextName", this.MDMRuleMapContextName.ToString());
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
        /// Get Xml representation of EntityActivityLog
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
