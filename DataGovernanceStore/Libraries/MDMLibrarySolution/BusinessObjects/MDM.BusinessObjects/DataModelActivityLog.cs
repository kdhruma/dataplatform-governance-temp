using System;
using System.Linq;


namespace MDM.BusinessObjects
{
    using Core;
    using System.Runtime.Serialization;
    using System.IO;
    using System.Xml;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents class for entity activity log
    /// </summary>
    [DataContract]
    [KnownType(typeof(DataModelActivityList))]
    public class DataModelActivityLog : MDMObject
    {
        #region Fields

        /// <summary>
        ///  Unique Id representing table
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// unique id representing organization
        /// </summary>
        private Int32 _orgId = -1;

        /// <summary>
        /// Indicates the catalog name of the Impacted entity
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Indicates the catalog name of the Impacted entity
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// indicate impacted node type
        /// </summary>
        private Int32 _entityTypeId = -1;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _relationshipTypeId = -1;

        /// <summary>
        /// Field indicates list of all the attributes which has been modified
        /// </summary>
        private Collection<Int32> _attributeIdList = new Collection<Int32>();

        /// <summary>
        /// 
        /// </summary>
        private DataModelActivityList _dataModelActivityLogAction = DataModelActivityList.Unknown;

        /// <summary>
        /// Field indicates weightage
        /// </summary>
        private Int32 _weightage;

        /// <summary>
        /// Field indicates whether loading of impacted entities is in progress
        /// </summary>
        private Boolean _isLoadingInProgress;

        /// <summary>
        /// Field indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        private Boolean _isLoaded;

        /// <summary>
        /// Field indicates whether all the loaded impacted entites in the impacted Entity table has processed
        /// </summary>
        private Boolean _isProcessed;

        /// <summary>
        /// Field indicates the time when the impacted entities loading in to impacted Entity table for processing started
        /// </summary>
        private DateTime? _loadStartTime;

        /// <summary>
        /// Field indicates the time when the impacted entities loading in to impacted Entity table for processing ended
        /// </summary>
        private DateTime? _loadEndTime;

        /// <summary>
        /// Field indicates the processing start time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        private DateTime? _processStartTime;

        /// <summary>
        /// Field indicates the processing end time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        private DateTime? _processEndTime;

        /// <summary>
        /// Field indicates the Created Date time of the entites in the Activity log table 
        /// </summary>
        private DateTime? _createdDateTime;

        /// <summary>
        /// number impacted entities effected by the entity change
        /// </summary>
        private Int64 _impactedCount;

        /// <summary>
        /// defines server id who has created this entity activity
        /// </summary>
        private Int32 _serverId;

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
        private Int32 _parentActivityLogId;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _mdmObjectId;

        /// <summary>
        /// 
        /// </summary>
        private PerformedAction _performedAction;

        /// <summary>
        /// Field denoting xml form of any single chage being done for data model
        /// </summary>
        private String _changedData = String.Empty;

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
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// representing organization id in which attribute model falls under
        /// </summary>
        [DataMember]
        public Int32 OrgId 
        {
            get
            {
                return _orgId;
            }
            set
            {
                _orgId = value;
            }
        }


        /// <summary>
        /// Indicates the catalog id
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
        /// 
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
        /// 
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get
            {
                return _relationshipTypeId;
            }
            set
            {
                _relationshipTypeId = value;
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
        public DataModelActivityList DataModelActivityLogAction
        {
            get { return _dataModelActivityLogAction; }
            set { _dataModelActivityLogAction = value; }
        }

        /// <summary>
        /// indicates weightage 
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get
            {
                return _weightage;
            }
            set
            {
                _weightage = value;
            }
        }


        /// <summary>
        /// Property indicates list of all the attributes which has been afffected
        /// </summary>
        [DataMember]
        public Collection<Int32> AttributeIdList
        {
            get
            {
                if (_attributeIdList != null && _attributeIdList.Count > 0)
                {
                    _attributeIdList = new Collection<Int32>(_attributeIdList.Distinct().ToList());
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
        /// Property indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        [DataMember]
        public Boolean IsLoadingInProgress
        {
            get
            {
                return _isLoadingInProgress;
            }
            set
            {
                _isLoadingInProgress = value;

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
                return _isLoaded;
            }
            set
            {
                _isLoaded = value;

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
                return _isProcessed;
            }
            set
            {
                _isProcessed = value;
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
                return _loadStartTime;
            }
            set
            {
                _loadStartTime = value;
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
                return _loadEndTime;
            }
            set
            {
                _loadEndTime = value;
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
                return _processStartTime;
            }
            set
            {
                _processStartTime = value;
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
                return _processEndTime;
            }
            set
            {
                _processEndTime = value;
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
                return _createdDateTime;
            }
            set
            {
                _createdDateTime = value;
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
                return _impactedCount;
            }
            set
            {
                _impactedCount = value;
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
                return _serverId;
            }
            set
            {
                _serverId = value;
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
                return _serverName;
            }
            set
            {
                _serverName = value;
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
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        /// <summary>
        /// Unique Id representing table
        /// Note:This is used only for logging exception in the Processor
        /// </summary>
        [DataMember]
        public Int64 DataModelActivityLogId
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
        /// Unique Id of the parent Entity representing table, of which the current entity came in for process
        /// Note:This is used only for logging exception in the Processor
        /// </summary>
        [DataMember]
        public Int32 ParentDataModelActivityLogId
        {
            get
            {
                return _parentActivityLogId;
            }

            set
            {
                _parentActivityLogId = value;
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
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 MDMObjectId
        {
            get
            {
                return _mdmObjectId;
            }
            set
            {
                _mdmObjectId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PerformedAction PerformedAction
        {
            get
            {
                return _performedAction;
            }
            set
            {
                _performedAction = value;
            }
        }

        /// <summary>
        /// Property indicating xml form of any single change being done for data model
        /// </summary>
        [DataMember]
        public String ChangedData
        {
            get 
            { 
                return this._changedData;
            }
            set 
            { 
                this._changedData = value; 
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataModelActivityLog()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public DataModelActivityLog(String valuesAsxml)
        {
            LoadDataModelActivityLog(valuesAsxml);
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadDataModelActivityLog(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelActivityLog")
                        {
                            #region Read Attribute Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ContainerName"))
                                {
                                    ContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PerformedAction"))
                                {
                                    PerformedAction action;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                }

                                if (reader.MoveToAttribute("AttributeIdList"))
                                {
                                    AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("IsLoadingInProgress"))
                                {
                                    IsLoadingInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("IsLoaded"))
                                {
                                    IsLoaded = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("IsProcessed"))
                                {
                                    IsProcessed = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("ProgramName"))
                                {
                                    ProgramName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LoadStartTime"))
                                {
                                    LoadStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("LoadEndTime"))
                                {
                                    LoadEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ProcessStartTime"))
                                {
                                    ProcessStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ProcessEndTime"))
                                {
                                    ProcessEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ImpactedCount"))
                                {
                                    ImpactedCount = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("UserName"))
                                {
                                    UserName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ServerId"))
                                {
                                    ServerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ServerName"))
                                {
                                    ServerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Context"))
                                {
                                    Context = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentDataModelActivityLogId"))
                                {
                                    ParentDataModelActivityLogId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), default(Int32));
                                }
                                
                                if (reader.MoveToAttribute("ChangedData"))
                                {
                                    ChangedData = reader.ReadContentAsString();
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
        /// Get Xml representation of DataModelActivityLog
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String denormXml;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String attributeIdList = String.Empty;
            String attributeLocaleIdList = String.Empty;
            String relationshipIdList = String.Empty;

            if (AttributeIdList != null && AttributeIdList.Count > 0)
            {
                attributeIdList = ValueTypeHelper.JoinCollection(AttributeIdList, ",");
            }

            //DataModelActivityLog node start
            xmlWriter.WriteStartElement("DataModelActivityLog");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", Id.ToString());
            xmlWriter.WriteAttributeString("ContainerId", ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", ContainerName);
            xmlWriter.WriteAttributeString("PerformedAction", Action.ToString());
            xmlWriter.WriteAttributeString("AttributeIdList", attributeIdList);
            xmlWriter.WriteAttributeString("AttributeLocaleIdList", attributeLocaleIdList);
            xmlWriter.WriteAttributeString("RelationshipIdList", relationshipIdList);
            xmlWriter.WriteAttributeString("IsLoadingInProgress", IsLoadingInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsLoaded", IsLoaded.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsProcessed", IsProcessed.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ProgramName", ProgramName);
            xmlWriter.WriteAttributeString("LoadStartTime", LoadStartTime.ToString());
            xmlWriter.WriteAttributeString("LoadEndTime", LoadEndTime.ToString());
            xmlWriter.WriteAttributeString("ProcessStartTime", ProcessStartTime.ToString());
            xmlWriter.WriteAttributeString("ProcessEndTime", ProcessEndTime.ToString());
            xmlWriter.WriteAttributeString("CreateDateTime", CreatedDateTime.ToString());
            xmlWriter.WriteAttributeString("ImpactedCount", ImpactedCount.ToString());
            xmlWriter.WriteAttributeString("UserName", UserName);
            xmlWriter.WriteAttributeString("ServerId", ServerId.ToString());
            xmlWriter.WriteAttributeString("ServerName", ServerName);
            xmlWriter.WriteAttributeString("Context", Context);
            xmlWriter.WriteAttributeString("ParentDataModelActivityLogId", ParentDataModelActivityLogId.ToString());
            xmlWriter.WriteAttributeString("ChangedData", ChangedData);

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
        /// Get Xml representation of DataModelActivityLog
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return ToXml();
        }

        #endregion
    }
}
