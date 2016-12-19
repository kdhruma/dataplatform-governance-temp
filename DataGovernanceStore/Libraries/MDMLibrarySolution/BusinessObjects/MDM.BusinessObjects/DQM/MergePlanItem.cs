using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies MergePlanItem
    /// </summary>
    [DataContract]
    public class MergePlanItem : MDMObject, IMergePlanItem
    {
        #region Fields

        /// <summary>
        /// Field denoting Merge Plan Item Id
        /// </summary>
        private Int64 _id;

        /// <summary>
        /// Field denoting Merge Planning Job Id
        /// </summary>
        private Int64? _mergePlanningJobId;

        /// <summary>
        /// Field denoting Matching Job Id
        /// </summary>
        private Int64? _matchingJobId;

        /// <summary>
        /// Field denoting Source Entity Id
        /// </summary>
        private Int64 _sourceEntityId;

        /// <summary>
        /// Field denoting Chosen Suspect Entity Id
        /// </summary>
        private Int64 _chosenSuspectEntityId;

        /// <summary>
        /// Field for ChosenSuspectScore
        /// </summary>
        private Double _chosenSuspectScore = 0.0;

        /// <summary>
        /// Field for _chosenSuspectUserName
        /// </summary>
        private String _chosenSuspectUserName;

        /// <summary>
        /// Field denoting Merge Actions
        /// </summary>
        private Collection<MergeAction> _mergeActions = new Collection<MergeAction>();

        /// <summary>
        /// Field denoting User Review Status
        /// </summary>
        private MergePlanUserReviewStatus _userReviewStatus = MergePlanUserReviewStatus.Unknown;

        /// <summary>
        /// Field denoting Reviewing User
        /// </summary>
        private String _reviewingUser;

        /// <summary>
        /// Field denoting Merge Status
        /// </summary>
        private MergeStatus? _mergeStatus = null;

        /// <summary>
        /// Field denoting Final Entity Id
        /// </summary>
        private Int64? _finalTargetEntityId;

        /// <summary>
        /// Field denoting Last Modification
        /// </summary>
        private DateTime? _lastModification;

        /// <summary>
        /// Field denoting Status Message
        /// </summary>
        private String _statusMessage;

        /// <summary>
        /// Field denoting Workflow Name
        /// </summary>
        private String _workflowName;

        /// <summary>
        /// Field denoting target container id
        /// </summary>
        private Int32? _targetContainerId;

        /// <summary>
        /// Field denoting relationship type id
        /// </summary>
        private Int32? _relationshipTypeId;

        /// <summary>
        /// Field denoting merge profile identifier
        /// </summary>
        private Int32? _mergingProfileId;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs MergePlanItem
        /// </summary>
        public MergePlanItem()
            : base()
        {
        }

        /// <summary>
        /// Constructs MergePlanItem from xml string
        /// </summary>
        public MergePlanItem(String xml)
            : base()
        {
            LoadFromXmlWithOuterNode(xml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Merge Plan Item Id
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property denoting Merge Planning Job Id
        /// </summary>
        [DataMember]
        public Int64? MergePlanningJobId
        {
            get { return _mergePlanningJobId; }
            set { _mergePlanningJobId = value; }
        }

        /// <summary>
        /// Property denoting Merge Planning Job Id
        /// </summary>
        [DataMember]
        public Int64? MatchingJobId
        {
            get { return _matchingJobId; }
            set { _matchingJobId = value; }
        }

        /// <summary>
        /// Property denoting Source Entity Id
        /// </summary>
        [DataMember]
        public Int64 SourceEntityId
        {
            get { return _sourceEntityId; }
            set { _sourceEntityId = value; }
        }

        /// <summary>
        /// Property denoting Chosen Suspect Entity Id
        /// </summary>
        [DataMember]
        public long ChosenSuspectEntityId
        {
            get { return _chosenSuspectEntityId; }
            set { _chosenSuspectEntityId = value; }
        }

        /// <summary>
        /// Property denoting ChosenSuspectScore
        /// </summary>
        [DataMember]
        public Double ChosenSuspectScore
        {
            get { return _chosenSuspectScore; }
            set { _chosenSuspectScore = value; }
        }

        /// <summary>
        /// Property denoting ChosenSuspectUserName
        /// </summary>
        [DataMember]
        public String ChosenSuspectUserName
        {
            get { return _chosenSuspectUserName; }
            set { _chosenSuspectUserName = value; }
        }

        /// <summary>
        /// Property denoting Merge Actions
        /// </summary>
        [DataMember]
        public Collection<MergeAction> MergeActions
        {
            get { return _mergeActions; }
            set { _mergeActions = value; }
        }

        /// <summary>
        /// Property denoting User Review Status
        /// </summary>
        [DataMember]
        public MergePlanUserReviewStatus UserReviewStatus
        {
            get { return _userReviewStatus; }
            set { _userReviewStatus = value; }
        }

        /// <summary>
        /// Property denoting Reviewing User
        /// </summary>
        [DataMember]
        public String ReviewingUser
        {
            get { return _reviewingUser; }
            set { _reviewingUser = value; }
        }

        /// <summary>
        /// Property denoting Merge Status
        /// </summary>
        [DataMember]
        public MergeStatus? MergeStatus
        {
            get { return _mergeStatus; }
            set { _mergeStatus = value; }
        }

        /// <summary>
        /// Property denoting Final Entity Id
        /// </summary>
        [DataMember]
        public Int64? FinalTargetEntityId
        {
            get { return _finalTargetEntityId; }
            set { _finalTargetEntityId = value; }
        }

        /// <summary>
        /// Property denoting Last Modification
        /// </summary>
        [DataMember]
        public DateTime? LastModification
        {
            get { return _lastModification; }
            set { _lastModification = value; }
        }

        /// <summary>
        /// Property denoting Status Message
        /// </summary>
        [DataMember]
        public String StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage = value; }
        }

        /// <summary>
        /// Property denoting Workflow Name
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// Property denoting Target Container Id
        /// </summary>
        [DataMember]
        public Int32? TargetContainerId
        {
            get { return _targetContainerId; }
            set { _targetContainerId = value; }
        }

        /// <summary>
        /// Property denoting Relationship Type Id
        /// </summary>
        [DataMember]
        public Int32? RelationshipTypeId
        {
            get { return _relationshipTypeId; }
            set { _relationshipTypeId = value; }
        }

        /// <summary>
        /// Property denoting merge profile identifier
        /// </summary>
        [DataMember]
        public Int32? MergingProfileId
        {
            get { return _mergingProfileId; }
            set { _mergingProfileId = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MergePlanItem
        /// </summary>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MergePlanItem node start
            xmlWriter.WriteStartElement("MergePlanItem");

            xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Action", Action.ToString());

            xmlWriter.WriteStartElement("MergePlanningJobId");
            if (MergePlanningJobId.HasValue)
            {
                xmlWriter.WriteValue(MergePlanningJobId.Value.ToString(CultureInfo.InvariantCulture));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MatchingJobId");
            if (MatchingJobId != null)
            {
                xmlWriter.WriteValue(MatchingJobId.Value.ToString(CultureInfo.InvariantCulture));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SourceEntityId");
            xmlWriter.WriteValue(SourceEntityId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ChosenSuspectEntityId");
            xmlWriter.WriteValue(ChosenSuspectEntityId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ChosenSuspectScore");
            xmlWriter.WriteValue(ChosenSuspectScore.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ChosenSuspectUserName");
            xmlWriter.WriteValue(ChosenSuspectUserName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MergeActions");
            foreach (MergeAction mergeAction in MergeActions)
            {
                xmlWriter.WriteStartElement("MergeAction");
                xmlWriter.WriteValue(mergeAction.ToString());
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            
            xmlWriter.WriteStartElement("UserReviewStatus");
            xmlWriter.WriteValue(UserReviewStatus.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ReviewingUser");
            xmlWriter.WriteValue(ReviewingUser);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MergeStatus");
            xmlWriter.WriteValue(MergeStatus.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("FinalTargetEntityId");
            if (FinalTargetEntityId.HasValue)
            {
                xmlWriter.WriteValue(FinalTargetEntityId.Value.ToString(CultureInfo.InvariantCulture));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("LastModification");
            if (LastModification.HasValue)
            {
                xmlWriter.WriteValue(LastModification.Value.ToUniversalTime());
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("WorkflowName");
            xmlWriter.WriteValue(WorkflowName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("StatusMessage");
            xmlWriter.WriteValue(StatusMessage);
            xmlWriter.WriteEndElement();

            if (TargetContainerId.HasValue)
            {
                xmlWriter.WriteStartElement("TargetContainerId");
                xmlWriter.WriteValue(TargetContainerId);
                xmlWriter.WriteEndElement();
            }

            if (RelationshipTypeId.HasValue)
            {
                xmlWriter.WriteStartElement("RelationshipTypeId");
                xmlWriter.WriteValue(RelationshipTypeId);
                xmlWriter.WriteEndElement();
            }

            if(MergingProfileId.HasValue)
            {
                xmlWriter.WriteStartElement("MergingProfileId");
                xmlWriter.WriteValue(MergingProfileId);
                xmlWriter.WriteEndElement();
            }

            //MergePlanItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MergePlanItem from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            if (node.Attributes != null)
            {
                Int64 int64Tmp = 0;
                if (node.Attributes["Id"] != null && Int64.TryParse(node.Attributes["Id"].Value, out int64Tmp))
                {
                    Id = int64Tmp;
                }

                ObjectAction objectActionTmp;
                if (node.Attributes["Action"] != null && Enum.TryParse(node.Attributes["Action"].Value, true, out objectActionTmp))
                {
                    Action = objectActionTmp;
                }
            }
                
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "MergePlanningJobId")
                {
                    Int64 tmp;
                    if (Int64.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        MergePlanningJobId = tmp;
                    }
                }
                else if (child.Name == "MatchingJobId")
                {
                    Int64 tmp;
                    if (Int64.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        MatchingJobId = tmp;
                    }
                }
                else if (child.Name == "SourceEntityId")
                {
                    Int64 tmp;
                    if (Int64.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        SourceEntityId = tmp;
                    }
                }
                else if (child.Name == "ChosenSuspectEntityId")
                {
                    Int64 tmp;
                    if (Int64.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        ChosenSuspectEntityId = tmp;
                    }
                }
                else if (child.Name == "MergeActions")
                {
                    MergeActions = new Collection<MergeAction>();
                    foreach (XmlNode mergeAction in child.ChildNodes)
                    {
                        if (mergeAction.Name == "MergeAction")
                        {
                            MergeAction mergeActionTmp;
                            if (Enum.TryParse(mergeAction.InnerText, true, out mergeActionTmp))
                            {
                                MergeActions.Add(mergeActionTmp);
                            }
                        }
                    }
                }
                else if (child.Name == "ChosenSuspectScore")
                {
                    Double tmp;
                    if (Double.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        ChosenSuspectScore = tmp;
                    }
                }
                else if (child.Name == "ChosenSuspectUserName")
                {
                    if (String.IsNullOrEmpty(child.InnerText))
                    {
                        ChosenSuspectUserName = child.InnerText;
                    }
                }
                else if (child.Name == "UserReviewStatus")
                {
                    MergePlanUserReviewStatus userStatusTmp;
                    if (Enum.TryParse(child.InnerText, true, out userStatusTmp))
                    {
                        UserReviewStatus = userStatusTmp;
                    }
                }
                else if (child.Name == "ReviewingUser")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        ReviewingUser = child.InnerText;
                    }
                }
                else if (child.Name == "MergeStatus")
                {
                    MergeStatus mergeStatusTmp;
                    if (Enum.TryParse(child.InnerText, true, out mergeStatusTmp))
                    {
                        MergeStatus = mergeStatusTmp;
                    }
                }
                else if (child.Name == "FinalTargetEntityId")
                {
                    Int64 tmp;
                    if (Int64.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        FinalTargetEntityId = tmp;
                    }
                }
                else if (child.Name == "LastModification")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        LastModification = ValueTypeHelper.ConvertToDateTime(child.InnerText).ToUniversalTime();
                    }
                }
                else if (child.Name == "StatusMessage")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        StatusMessage = child.InnerText;
                    }
                }
                else if (child.Name == "WorkflowName")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        WorkflowName = child.InnerText;
                    }
                }
                else if (child.Name == "TargetContainerId")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        TargetContainerId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    }
                }
                else if (child.Name == "RelationshipTypeId")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        RelationshipTypeId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    }
                }
                else if (child.Name == "MergingProfileId")
                {
                    if(!String.IsNullOrEmpty(child.InnerText))
                    {
                        MergingProfileId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    }
                }
            }

        }

        /// <summary>
        /// Loads MergePlanItem (excluding MDMObject's properties) from XML with outer node
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlWithOuterNode);
            XmlNode node = doc.SelectSingleNode("MergePlanItem");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion
    }
}