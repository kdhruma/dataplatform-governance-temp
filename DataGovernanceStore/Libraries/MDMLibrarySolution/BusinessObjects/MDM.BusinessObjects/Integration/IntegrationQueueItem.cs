using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class integration message queue.
    /// </summary>
    [DataContract]
    public class IntegrationQueueItem : ObjectBase, IIntegrationQueueItem, IIntegrationItem
    {
        #region Fields

        /// <summary>
        /// Indicates Id of type of integration message
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        private ObjectAction _action = ObjectAction.Read;

        /// <summary>
        /// Indicates Id of connector for which this message will be used
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates name of connector for which this message will be used
        /// </summary>
        private String _connectorLongName = String.Empty;

        /// <summary>
        /// Indicates Id of integration activity log for which this message is created
        /// </summary>
        private Int64 _integrationActivityLogId = -1;

        /// <summary>
        /// Indicates type of integration type
        /// </summary>
        private IntegrationType _integrationType = IntegrationType.Unknown;

        /// <summary>
        /// Indicates type of message
        /// </summary>
        private Int16 _integrationMessageTypeId = -1;

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        private String _integrationMessageTypeLongName = String.Empty;

        /// <summary>
        /// Indicates Id of message to process
        /// </summary>
        private Int64 _integrationMessageId = -1;

        /// <summary>
        /// Indicates if qualifying process in progress
        /// </summary>
        private Boolean _isInProgress = false;

        /// <summary>
        /// Indicates if message processing is processed
        /// </summary>
        private Boolean _isProcessed = false;

        /// <summary>
        /// Indicates time when qualification process started
        /// </summary>
        private DateTime? _startTime = null;

        /// <summary>
        /// Indicates time when qualification process finished
        /// </summary>
        private DateTime? _endTime = null;

        /// <summary>
        /// Indicates which server qualified this record
        /// </summary>
        private Int32 _serverId = -1;

        /// <summary>
        /// Indicates Weitage for qualification
        /// </summary>
        private Int32 _weightage = -1;

        /// <summary>
        /// Indicates any comments qualification or process for given item
        /// </summary>
        private Collection<String> _comments = new Collection<String>();

        /// <summary>
        /// Represents reference to this object while processing .
        /// </summary>
        private String _referenceId = String.Empty;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationQueueItem()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public IntegrationQueueItem(String valuesAsXml)
        {
            throw new NotImplementedException();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of type of integration message
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
        /// Indicates action to be performed on the object
        /// </summary>
        [DataMember]
        public ObjectAction Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Indicates Id of connector for which this message will be used
        /// </summary>
        [DataMember]
        public Int16 ConnectorId
        {
            get
            {
                return _connectorId;
            }
            set
            {
                _connectorId = value;
            }
        }

        /// <summary>
        /// Indicates name of connector for which this message will be used
        /// </summary>
        [DataMember]
        public String ConnectorLongName
        {
            get
            {
                return _connectorLongName;
            }
            set
            {
                _connectorLongName = value;
            }
        }

        /// <summary>
        /// Indicates Id of integration activity log for which this message is created
        /// </summary>
        [DataMember]
        public Int64 IntegrationActivityLogId
        {
            get
            {
                return _integrationActivityLogId;
            }
            set
            {
                _integrationActivityLogId = value;
            }
        }

        /// <summary>
        /// Indicates type of integration type
        /// </summary>
        [DataMember]
        public IntegrationType IntegrationType
        {
            get
            {
                return _integrationType;
            }
            set
            {
                _integrationType = value;
            }
        }

        /// <summary>
        /// Indicates type of message
        /// </summary>
        [DataMember]
        public Int16 IntegrationMessageTypeId
        {
            get
            {
                return _integrationMessageTypeId;
            }
            set
            {
                _integrationMessageTypeId = value;
            }
        }

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        [DataMember]
        public String IntegrationMessageTypeLongName
        {
            get
            {
                return _integrationMessageTypeLongName;
            }
            set
            {
                _integrationMessageTypeLongName = value;
            }
        }

        /// <summary>
        /// Indicates Id of message to process
        /// </summary>
        [DataMember]
        public Int64 IntegrationMessageId
        {
            get { return _integrationMessageId; }
            set { _integrationMessageId = value; }
        }

        /// <summary>
        /// Indicates if qualifying process in progress
        /// </summary>
        [DataMember]
        public Boolean IsInProgress
        {
            get
            {
                return _isInProgress;
            }
            set
            {
                _isInProgress = value;
            }
        }

        /// <summary>
        /// Indicates if message processing is processed
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
        /// Indicates time when qualification process started
        /// </summary>
        [DataMember]
        public DateTime? StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
            }
        }

        /// <summary>
        /// Indicates time when qualification process finished
        /// </summary>
        [DataMember]
        public DateTime? EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
            }
        }

        /// <summary>
        /// Indicates which server qualified this record
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
        /// Indicates Weitage for qualification
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
        /// Indicates Reference field for the object
        /// </summary>
        [DataMember]
        public String ReferenceId
        {
            get { return _referenceId; }
            set { _referenceId = value; }
        }

        /// <summary>
        /// Indicates any comments qualification or process for given item
        /// </summary>
        [DataMember]
        public Collection<String> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        /// <summary>
        /// Indicates Integration Id. In case of exception handling for queues, Integration Id indicates the PK of respective table. 
        /// And based on ProcessorName and Integration Id, record will be marked for reprocessing in respective table.
        /// For example, if ProcessorName = "IntegrationQualifyingQueueLoadProcessor" and IntegrationId = 5, then tb_IntegrationActivityLog with PK = 5 will be marked for re-process.
        /// </summary>
        [DataMember]
        public Int64 IntegrationId
        {
            get
            {
                return this.Id;
            }
            private set
            {
                this.Id = value;
            }
        }

        #endregion Properties

        #region Methods

        
        #endregion Methods
    }
}
