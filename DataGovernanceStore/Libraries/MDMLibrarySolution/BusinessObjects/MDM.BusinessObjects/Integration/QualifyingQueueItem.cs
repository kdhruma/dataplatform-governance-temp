using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents qualifying queue item
    /// </summary>
    public class QualifyingQueueItem : IntegrationQueueItem, IQualifyingQueueItem
    {
        #region Fields

        /// <summary>
        /// Indicates status of message qualification
        /// </summary>
        private MessageQualificationStatusEnum _messageQualificationStatus = MessageQualificationStatusEnum.Unknown;

        /// <summary>
        /// Indicates when a particular message is to be picked up for qualification process
        /// </summary>
        private DateTime? _scheduledQualifierTime = DateTime.Now;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public QualifyingQueueItem()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public QualifyingQueueItem(String valuesAsXml)
        {
            LoadQualifyingQueueItem(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates status of message qualification
        /// </summary>
        [DataMember]
        public MessageQualificationStatusEnum MessageQualificationStatus
        {
            get
            {
                return _messageQualificationStatus;
            }
            set
            {
                _messageQualificationStatus = value;
            }
        }

        /// <summary>
        /// Indicates when a particular message is to be picked up for qualification process
        /// </summary>
        [DataMember]
        public DateTime? ScheduledQualifierTime
        {
            get
            {
                return _scheduledQualifierTime;
            }
            set
            {
                _scheduledQualifierTime = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents QualifyingQueueItem in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("QualifyingQueueItem");

            xmlWriter.WriteElementString("Action", this.Action.ToString());
            xmlWriter.WriteElementString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteElementString("ConnectorLongName", this.ConnectorLongName);
            xmlWriter.WriteElementString("Id", this.Id.ToString());
            xmlWriter.WriteElementString("IntegrationActivityLogId", this.IntegrationActivityLogId.ToString());
            xmlWriter.WriteElementString("IntegrationMessageId", this.IntegrationMessageId.ToString());
            xmlWriter.WriteElementString("IntegrationMessageTypeId", this.IntegrationMessageTypeId.ToString());
            xmlWriter.WriteElementString("IntegrationMessageTypeLongName", this.IntegrationMessageTypeLongName);
            xmlWriter.WriteElementString("IntegrationType", this.IntegrationType.ToString());
            xmlWriter.WriteElementString("MessageQualificationStatus", this.MessageQualificationStatus.ToString());
            xmlWriter.WriteElementString("IsInProgress", this.IsInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteElementString("EndTime", this.EndTime == null ? String.Empty : this.EndTime.ToString());
            xmlWriter.WriteElementString("StartTime", this.StartTime == null ? String.Empty : this.StartTime.ToString());
            xmlWriter.WriteElementString("ScheduledQualifierTime", this.ScheduledQualifierTime == null ? String.Empty : this.ScheduledQualifierTime.ToString());
            xmlWriter.WriteElementString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteElementString("Weightage", this.Weightage.ToString());
            xmlWriter.WriteElementString("ReferenceId", this.ReferenceId);

            xmlWriter.WriteStartElement("Comments");

            if (this.Comments != null && this.Comments.Count > 0)
            {
                foreach (String comment in this.Comments)
                {
                    xmlWriter.WriteStartElement("Comment");
                    xmlWriter.WriteCData(comment);

                    //End comment
                    xmlWriter.WriteEndElement();
                }
            }

            //End comments
            xmlWriter.WriteEndElement();

            //End QualifyingQueueItem
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();


            return xml;
        }

        /// <summary>
        /// Clone the current instance and create new instance with same values.
        /// </summary>
        /// <returns>Cloned IQualifyingQueueItem</returns>
        public IQualifyingQueueItem Clone()
        {
            QualifyingQueueItem qualifyingQueueItem = new QualifyingQueueItem();
            qualifyingQueueItem.Action = this.Action;
            qualifyingQueueItem.Comments = this.Comments;
            qualifyingQueueItem.ConnectorId = this.ConnectorId;
            qualifyingQueueItem.ConnectorLongName = this.ConnectorLongName;
            qualifyingQueueItem.Id = this.Id;
            qualifyingQueueItem.IntegrationActivityLogId = this.IntegrationActivityLogId;
            qualifyingQueueItem.IntegrationMessageId = this.IntegrationMessageId;
            qualifyingQueueItem.IntegrationMessageTypeId = this.IntegrationMessageTypeId;
            qualifyingQueueItem.IntegrationMessageTypeLongName = this.IntegrationMessageTypeLongName;
            qualifyingQueueItem.IntegrationType = this.IntegrationType;
            qualifyingQueueItem.MessageQualificationStatus = this.MessageQualificationStatus;
            qualifyingQueueItem.IsInProgress = this.IsInProgress;
            qualifyingQueueItem.EndTime = this.EndTime;
            qualifyingQueueItem.StartTime = this.StartTime;
            qualifyingQueueItem.ScheduledQualifierTime = this.ScheduledQualifierTime;
            qualifyingQueueItem.ServerId = this.ServerId;
            qualifyingQueueItem.Weightage = this.Weightage;
            qualifyingQueueItem.ReferenceId = this.ReferenceId;
            return qualifyingQueueItem;
        }

        #region Private Methods

        private void LoadQualifyingQueueItem(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (! reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "QualifyingQueueItem" && reader.IsStartElement())
                        {
                            #region Read QualifyingQueueItem Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    Enum.TryParse<ObjectAction>(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }

                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ConnectorLongName"))
                                    this.ConnectorLongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("IntegrationActivityLogId"))
                                    this.IntegrationActivityLogId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IntegrationMessageId"))
                                    this.IntegrationMessageId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IntegrationMessageTypeId"))
                                    this.IntegrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IntegrationMessageTypeLongName"))
                                    this.IntegrationMessageTypeLongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("IntegrationType"))
                                {
                                    IntegrationType integrationType = Core.IntegrationType.Unknown;
                                    Enum.TryParse<IntegrationType>(reader.ReadContentAsString(), out integrationType);
                                    this.IntegrationType = integrationType;
                                }

                                if (reader.MoveToAttribute("MessageQualificationStatus"))
                                {
                                    MessageQualificationStatusEnum qualificationStatus = Core.MessageQualificationStatusEnum.Unknown;
                                    Enum.TryParse<MessageQualificationStatusEnum>(reader.ReadContentAsString(), out qualificationStatus);
                                    this.MessageQualificationStatus = qualificationStatus;
                                }

                                if (reader.MoveToAttribute("IsInProgress"))
                                    this.IsInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("EndTime"))
                                    this.EndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("StartTime"))
                                    this.StartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("ScheduledQualifierTime"))
                                    this.ScheduledQualifierTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("ServerId"))
                                    this.ServerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Weightage"))
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ReferenceId"))
                                    this.ReferenceId = reader.ReadContentAsString();
                                
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Comment")
                        {
                            String comment = reader.ReadInnerXml();
                            if (!String.IsNullOrWhiteSpace(comment))
                            {
                                this.Comments.Add(comment);
                            }
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

        #endregion Private Methods

        #endregion Methods
    }
}
