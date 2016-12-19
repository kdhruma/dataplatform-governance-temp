using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DQMProcessorErrorLogItem
    /// </summary>
    [DataContract]
    [KnownType(typeof(DQMJobType))]
    public class DQMProcessorErrorLogItem : MDMObject, IDQMProcessorErrorLogItem
    {
        #region Fields

        /// <summary>
        /// Field indicates error log item Id
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field indicates Job Id
        /// </summary>
        private Int64? _jobId = 0;

        /// <summary>
        /// Field indicates Job type
        /// </summary>
        private DQMJobType? _jobType = DQMJobType.Unknown;

        /// <summary>
        /// Field for the Id of Entity
        /// </summary>
        private Int64? _entityId;

        /// <summary>
        /// Field indicates the catalog Id of entity
        /// </summary>
        private Int32? _containerId = 0;

        /// <summary>
        /// Field indicates modified date time. Please set to Null if you want to use SQL Server's current date and time.
        /// </summary>
        private DateTime? _modifiedDateTime = null;

        /// <summary>
        /// Field indicates error message
        /// </summary>
        private String _errorMessage = null;

        /// <summary>
        /// Field indicates processor name
        /// </summary>
        private String _processorName = null;

        /// <summary>
        /// Field indicates user
        /// </summary>
        private String _modifiedUser = null;

        /// <summary>
        /// Field indicates program
        /// </summary>
        private String _modifiedProgram = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property indicates error log item Id
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
        /// Property indicates Job Id
        /// </summary>
        [DataMember]
        public Int64? JobId
        {
            get
            {
                return this._jobId;
            }
            set
            {
                this._jobId = value;
            }
        }

        /// <summary>
        /// Property indicates Job type
        /// </summary>
        [DataMember]
        public DQMJobType? JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }

        /// <summary>
        /// Property indicates the Id of Entity
        /// </summary>
        [DataMember]
        public Int64? EntityId
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
        public Int32? ContainerId
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
        ///  Property indicates the last modified time of this queued entity. Please set to Null if you want to use SQL Server's current date and time.
        /// </summary>
        [DataMember]
        public DateTime? ModifiedDateTime
        {
            get
            {
                return this._modifiedDateTime;
            }
            set
            {
                this._modifiedDateTime = value;
            }
        }

        /// <summary>
        /// Property indicates error message
        /// </summary>
        [DataMember]
        public String ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// Property indicates processor name
        /// </summary>
        [DataMember]
        public String ProcessorName
        {
            get { return _processorName; }
            set { _processorName = value; }
        }

        /// <summary>
        /// Property indicates user
        /// </summary>
        [DataMember]
        public String ModifiedUser
        {
            get
            {
                return this._modifiedUser;
            }
            set
            {
                this._modifiedUser = value;
            }
        }

        /// <summary>
        /// Property indicates program
        /// </summary>
        [DataMember]
        public String ModifiedProgram
        {
            get { return _modifiedProgram; }
            set { _modifiedProgram = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DQMProcessorErrorLogItem()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public DQMProcessorErrorLogItem(String valuesAsxml)
        {
            LoadDQMProcessorErrorLogItem(valuesAsxml);
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadDQMProcessorErrorLogItem(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DQMProcessorErrorLogItem")
                        {
                            #region Read Attribute Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("JobId"))
                                {
                                    this.JobId = ValueTypeHelper.ConvertToNullableInt64(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    this.EntityId = ValueTypeHelper.ConvertToNullableInt64(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("JobType"))
                                {
                                    DQMJobType action = DQMJobType.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.JobType = action;
                                }
                                if (reader.MoveToAttribute("ModifiedDateTime"))
                                {
                                    this.ModifiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ModifiedUser"))
                                {
                                    this.ModifiedUser = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ModifiedProgram"))
                                {
                                    this.ModifiedProgram = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ErrorMessage"))
                                {
                                    this.ErrorMessage = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ProcessorName"))
                                {
                                    this.ProcessorName = reader.ReadContentAsString();
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
        /// Get Xml representation of DQMProcessorErrorLogItem
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String denormXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DQMProcessorErrorLogItem node start
            xmlWriter.WriteStartElement("DQMProcessorErrorLogItem");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("JobId", this.JobId.ToString());
            xmlWriter.WriteAttributeString("JobType", this.JobType.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ErrorMessage", this.ErrorMessage ?? "");
            xmlWriter.WriteAttributeString("ProcessorName", this.ProcessorName ?? "");
            xmlWriter.WriteAttributeString("ModifiedDateTime", this.ModifiedDateTime.ToString());
            xmlWriter.WriteAttributeString("ModifiedUser", this.ModifiedUser ?? "");
            xmlWriter.WriteAttributeString("ModifiedProgram", this.ModifiedProgram ?? "");

            #endregion

            //DQMProcessorErrorLogItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            denormXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return denormXml;
        }

        /// <summary>
        /// Get Xml representation of DQMProcessorErrorLogItem
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