using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents message qualification result.
    /// Contains the message qualification status, scheduled qualification time and scheduled aggregation time.
    /// </summary>
    public class MessageQualificationResult : IMessageQualificationResult
    {
        #region Fields

        /// <summary>
        /// Indicates if the message is qualified for further processing or not.
        /// </summary>
        private MessageQualificationStatusEnum _messageQualificationStatus = MessageQualificationStatusEnum.Unknown;

        /// <summary>
        /// Indicates the next time slot when message should be picked up for qualification process. 
        /// If value is not given, then it will compute next scheduled time from ConnectorProfile
        /// </summary>
        private DateTime? _scheduledQualificationTime = null;

        /// <summary>
        /// Indicates the next time slot when message should be picked up for aggregation process. 
        /// If value is not given, then it will compute next scheduled time from ConnectorProfile
        /// </summary>
        private DateTime? _scheduledAggregationTime = null;

        /// <summary>
        /// Indicates the comments after the message qualification.
        /// </summary>
        private Collection<String> _comments = new Collection<String>();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MessageQualificationResult()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Values in xml format</param>
        public MessageQualificationResult(String valuesAsXml)
        {
            LoadMessageQualificationResult(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates if the message is qualified for further processing or not.
        /// </summary>
        [DataMember]
        public MessageQualificationStatusEnum MessageQualificationStatus
        {
            get { return _messageQualificationStatus; }
            set { _messageQualificationStatus = value; }
        }

        /// <summary>
        /// Indicates the next time slot when message should be picked up for qualification process. 
        /// If value is not given, then it will compute next scheduled time from ConnectorProfile
        /// </summary>
        [DataMember]
        public DateTime? ScheduledQualificationTime
        {
            get { return _scheduledQualificationTime; }
            set { _scheduledQualificationTime = value; }
        }

        /// <summary>
        /// Indicates the next time slot when message should be picked up for aggregation process. 
        /// If value is not given, then it will compute next scheduled time from ConnectorProfile
        /// </summary>
        [DataMember]
        public DateTime? ScheduledAggregationTime
        {
            get { return _scheduledAggregationTime; }
            set { _scheduledAggregationTime = value; }
        }
        /// <summary>
        /// Indicates the comments after the message qualification.
        /// </summary>
        [DataMember]
        public Collection<String> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents MessageQualificationResult in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("MessageQualificationResult");

            xmlWriter.WriteAttributeString("QualificationStatus", this.MessageQualificationStatus.ToString());
            xmlWriter.WriteAttributeString("ScheduledQualificationTime", this.ScheduledQualificationTime == null ? String.Empty : this.ScheduledQualificationTime.ToString());
            xmlWriter.WriteAttributeString("ScheduledAggregationTime", this.ScheduledAggregationTime == null ? String.Empty : this.ScheduledAggregationTime.ToString());

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

            //MessageQualificationResult end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the MessageQualificationResult and create a new MessageQualificationResult
        /// </summary>
        /// <returns>New MessageQualificationResult having same value as current one.</returns>
        public IMessageQualificationResult Clone()
        {
            MessageQualificationResult messageQualificationResult = new MessageQualificationResult();
            messageQualificationResult.ScheduledQualificationTime = this.ScheduledQualificationTime;
            messageQualificationResult.MessageQualificationStatus = this.MessageQualificationStatus;
            messageQualificationResult.ScheduledAggregationTime = this.ScheduledAggregationTime;

            return messageQualificationResult;
        }

        #endregion Methods

        #region Private Methods

        private void LoadMessageQualificationResult(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MessageQualificationResult" && reader.IsStartElement())
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ScheduledQualificationTime"))
                                {
                                    String value = reader.ReadContentAsString();
                                    if(!String.IsNullOrWhiteSpace(value))
                                    {
                                        this.ScheduledQualificationTime = ValueTypeHelper.ConvertToNullableDateTime(value);
                                    }
                                }

                                if (reader.MoveToAttribute("ScheduledAggregationTime"))
                                {
                                    String value = reader.ReadContentAsString();
                                    if (!String.IsNullOrWhiteSpace(value))
                                    {
                                        this.ScheduledAggregationTime = ValueTypeHelper.ConvertToNullableDateTime(value);
                                    }
                                }

                                if (reader.MoveToAttribute("QualificationStatus"))
                                {
                                    MessageQualificationStatusEnum status = MessageQualificationStatusEnum.Unknown;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out status))
                                    {
                                        this.MessageQualificationStatus = status;
                                    }
                                }
                            }
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

    }
}
