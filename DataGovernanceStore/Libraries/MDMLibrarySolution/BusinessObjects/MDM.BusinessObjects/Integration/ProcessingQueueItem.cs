using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents processing queue item
    /// </summary>
    public class ProcessingQueueItem : IntegrationQueueItem, IProcessingQueueItem
    {
        #region Fields

        /// <summary>
        /// Indicates when a particular message is to be picked up for processing process
        /// </summary>
        private DateTime? _scheduledProcessTime = DateTime.Now;

        /// <summary>
        /// Indicates current processing queue item came from which qualifying queue item
        /// </summary>
        private Int64 _qualifyingQueueItemId = -1;

        /// <summary>
        /// Indicates what should be the name of Name while reading/writing Xml for queue item. This is needed because InboundQueueItem and OutboundItem are inheriting this class.
        /// </summary>
        private String QueueItemNodeName = String.Empty;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ProcessingQueueItem(IntegrationType integrationType)
            : base()
        {
            GetNodeNameForQueueItem(integrationType);
        }

        /// <summary>
        /// Constructor which takes integrationType and valuesAsXml as input parametrs
        /// </summary>
        /// <param name="integrationType">Indicates type of Integration - Unknown|Inbound|Outbound </param>
        /// <param name="valuesAsXml">ProcessingQueueItem in Xml representation</param>
        public ProcessingQueueItem(IntegrationType integrationType, String valuesAsXml)
        {
            GetNodeNameForQueueItem(integrationType);
            LoadProcessingQueueItem(integrationType, valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates when a particular message is to be picked up for processing process
        /// </summary>
        [DataMember]
        public DateTime? ScheduledProcessTime
        {
            get { return _scheduledProcessTime; }
            set { _scheduledProcessTime = value; }
        }

        /// <summary>
        /// Indicates current processing queue item came from which qualifying queue item
        /// </summary>
        [DataMember]
        public Int64 QualifyingQueueItemId
        {
            get { return _qualifyingQueueItemId; }
            set { _qualifyingQueueItemId = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents ProcessingQueueItem in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement(this.QueueItemNodeName);

            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("ConnectorLongName", this.ConnectorLongName);
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("IntegrationActivityLogId", this.IntegrationActivityLogId.ToString());
            xmlWriter.WriteAttributeString("IntegrationMessageId", this.IntegrationMessageId.ToString());
            xmlWriter.WriteAttributeString("IntegrationMessageTypeId", this.IntegrationMessageTypeId.ToString());
            xmlWriter.WriteAttributeString("IntegrationMessageTypeLongName", this.IntegrationMessageTypeLongName);
            xmlWriter.WriteAttributeString("IntegrationType", this.IntegrationType.ToString());
            xmlWriter.WriteAttributeString("IsProcessed", this.IsProcessed.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsInProgress", this.IsInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("EndTime", this.EndTime == null ? String.Empty : this.EndTime.ToString());
            xmlWriter.WriteAttributeString("StartTime", this.StartTime == null ? String.Empty : this.StartTime.ToString());
            xmlWriter.WriteAttributeString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
            xmlWriter.WriteAttributeString("QualifyingQueueItemId", this.QualifyingQueueItemId.ToString());
            xmlWriter.WriteAttributeString("ScheduledProcessTime", this.ScheduledProcessTime == null ? String.Empty : this.ScheduledProcessTime.ToString());


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

            //End ProcessingQueueItem
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
        /// <returns>Cloned IProcessingQueueItem</returns>
        public IProcessingQueueItem Clone()
        {
            ProcessingQueueItem processingQueueItem = new ProcessingQueueItem(this.IntegrationType);
            processingQueueItem.Action = this.Action;
            processingQueueItem.Comments = this.Comments;
            processingQueueItem.ConnectorId = this.ConnectorId;
            processingQueueItem.ConnectorLongName = this.ConnectorLongName;
            processingQueueItem.Id = this.Id;
            processingQueueItem.IntegrationActivityLogId = this.IntegrationActivityLogId;
            processingQueueItem.IntegrationMessageId = this.IntegrationMessageId;
            processingQueueItem.IntegrationMessageTypeId = this.IntegrationMessageTypeId;
            processingQueueItem.IntegrationMessageTypeLongName = this.IntegrationMessageTypeLongName;
            processingQueueItem.IntegrationType = this.IntegrationType;
            processingQueueItem.IsProcessed = this.IsProcessed;
            processingQueueItem.IsInProgress = this.IsInProgress;
            processingQueueItem.EndTime = this.EndTime;
            processingQueueItem.StartTime = this.StartTime;
            processingQueueItem.ScheduledProcessTime = this.ScheduledProcessTime;
            processingQueueItem.ServerId = this.ServerId;
            processingQueueItem.Weightage = this.Weightage;
            processingQueueItem.ReferenceId = this.ReferenceId;
            processingQueueItem.QualifyingQueueItemId = this.QualifyingQueueItemId;

            return processingQueueItem;
        }

        #region Private Methods

        private void LoadProcessingQueueItem(IntegrationType integrationType, String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == this.QueueItemNodeName && reader.IsStartElement())
                        {
                            #region Read ProcessingQueueItem Attributes

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

                                //if (reader.MoveToAttribute("IntegrationType"))
                                //{
                                //    IntegrationType iType = Core.IntegrationType.Unknown;
                                //    Enum.TryParse<IntegrationType>(reader.ReadContentAsString(), out integrationType);
                                //    this.IntegrationType = iType;
                                //}

                                this.IntegrationType = integrationType;

                                if (reader.MoveToAttribute("IsProcessed"))
                                    this.IsProcessed = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("IsInProgress"))
                                    this.IsInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("EndTime"))
                                    this.EndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("StartTime"))
                                    this.StartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("ServerId"))
                                    this.ServerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Weightage"))
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("QualifyingQueueItemId"))
                                    this.QualifyingQueueItemId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ScheduledProcessTime"))
                                    this.ScheduledProcessTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                            }
                            #endregion

                            reader.Read();
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

        /// <summary>
        /// From the IntegrationType, decide what should be the nodename for xml.
        /// This class is inherited by both InboundQueueItem and OutboundQueueItem. So Xml serialization/deserialization, this will be used.
        /// </summary>
        /// <param name="integrationType">Indicates type of queue.</param>
        private void GetNodeNameForQueueItem(IntegrationType integrationType)
        {
            if (integrationType == Core.IntegrationType.Inbound)
            {
                this.QueueItemNodeName = "InboundQueueItem";
                this.IntegrationType = Core.IntegrationType.Inbound;
            }
            else if (integrationType == Core.IntegrationType.Outbound)
            {
                this.QueueItemNodeName = "OutboundQueueItem";
                this.IntegrationType = Core.IntegrationType.Outbound;
            }
            else
            {
                throw new Exception("IntegrationType = '" + integrationType + "' is not supported. Please provide valid IntegrationType");
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
