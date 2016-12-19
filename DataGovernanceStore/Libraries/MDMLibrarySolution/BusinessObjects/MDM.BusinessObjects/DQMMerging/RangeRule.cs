using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMMerging
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Range Rule
    /// </summary>
    [DataContract]
    public class RangeRule : IRangeRule
    {
        #region Fields

        /// <summary>
        /// Field for Order
        /// </summary>
        private Int32 _order = 0;

        /// <summary>
        /// Field for lower limit (more or equal to this value)
        /// </summary>
        private Double _from = 0.0;

        /// <summary>
        /// Field for upper limit (less or equal than this value)
        /// </summary>
        private Double _to = 0.0;

        /// <summary>
        /// Field for Merge Action
        /// </summary>
        private MergeAction _action = MergeAction.Unknown;

        /// <summary>
        /// Field for Realtionship type Id
        /// </summary>
        private Int32? _relationshipTypeId;

        /// <summary>
        /// Field for target Container Id
        /// </summary>
        private Int32? _targetContainerId;

        /// <summary>
        /// Field for Workflow Name
        /// </summary>
        private String _workflowName;

        /// <summary>
        /// Property denoting MessageCode to be used for button text while generating the action button using given MergePlanningProfile
        /// </summary>
        private String _actionLabelMessageCode;

        /// <summary>
        /// Field for merging profile identifier
        /// </summary>
        private Int32? _mergingProfileId;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs RangeRule
        /// </summary>
        public RangeRule()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs RangeRule using specified instance data
        /// </summary>
        public RangeRule(RangeRule source)
            : base()
        {
            this.Order = source.Order;
            this.From = source.From;
            this.To = source.To;
            this.Action = source.Action;
            this.RelationshipTypeId = source.RelationshipTypeId;
            this.TargetContainerId = source.TargetContainerId;
            this.WorkflowName = source.WorkflowName;
            this.ActionLabelMessageCode = source.ActionLabelMessageCode;
            this.MergingProfileId = source.MergingProfileId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Order
        /// </summary>
        [DataMember]
        public Int32 Order
        {
            get { return _order; }
            set { _order = value; }
        }

        /// <summary>
        /// Property denoting lower limit (more or equal to this value)
        /// </summary>
        [DataMember]
        public Double From
        {
            get { return _from; }
            set { _from = value; }
        }

        /// <summary>
        /// Property denoting upper limit (less or equal to this value)
        /// </summary>
        [DataMember]
        public Double To
        {
            get { return _to; }
            set { _to = value; }
        }

        /// <summary>
        /// Property denoting Merge Action
        /// </summary>
        [DataMember]
        public MergeAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        /// <summary>
        /// Property denoting Relationship Type
        /// </summary>
        [DataMember]
        public Int32? RelationshipTypeId
        {
            get { return _relationshipTypeId; }
            set { _relationshipTypeId = value; }
        }

        /// <summary>
        /// Property denoting target Container Id
        /// </summary>
        [DataMember]
        public Int32? TargetContainerId
        {
            get { return _targetContainerId; }
            set { _targetContainerId = value; }
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
        /// Property denoting MessageCode to be used for button text while generating the action button using given MergePlanningProfile
        /// </summary>
        [DataMember]
        public String ActionLabelMessageCode
        {
            get { return _actionLabelMessageCode; }
            set { _actionLabelMessageCode = value; }
        }

        /// <summary>
        /// Property denoting the merging profile identifier which will be set when the action is merge. This provides the merging profile details which needs to be used for creating the merge copy context.
        /// </summary>
        [DataMember]
        public Int32? MergingProfileId
        {
            get
            {
                return _mergingProfileId;
            }
            set
            {
                _mergingProfileId = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RangeRule
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //RangeRule node start
            xmlWriter.WriteStartElement("RangeRule");

            xmlWriter.WriteAttributeString("Order", Order.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("From", From.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("To", To.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("Action", Action.ToString());

            xmlWriter.WriteAttributeString("ActionLabelMessageCode", ActionLabelMessageCode);

            if (RelationshipTypeId.HasValue)
            {
                xmlWriter.WriteAttributeString("RelationshipTypeId", RelationshipTypeId.ToString());
            }

            if (TargetContainerId.HasValue)
            {
                xmlWriter.WriteAttributeString("TargetContainerId", TargetContainerId.ToString());
            }

            if (MergingProfileId.HasValue)
            {
                xmlWriter.WriteAttributeString("MergingProfileId", MergingProfileId.ToString());
            }

            xmlWriter.WriteAttributeString("WorkflowName", WorkflowName);

            //RangeRule node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads RangeRule from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            Int32 int32Tmp;
            Order = 0;
            if (node.Attributes != null && node.Attributes["Order"] != null && Int32.TryParse(node.Attributes["Order"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int32Tmp))
            {
                Order = int32Tmp;
            }

            Double doubleTmp;
            From = 0.0;
            if (node.Attributes != null && node.Attributes["From"] != null && Double.TryParse(node.Attributes["From"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTmp))
            {
                From = doubleTmp;
            }

            To = 0.0;
            if (node.Attributes != null && node.Attributes["To"] != null && Double.TryParse(node.Attributes["To"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTmp))
            {
                To = doubleTmp;
            }

            MergeAction mergeActionTmp;
            Action = MergeAction.Unknown;
            if (node.Attributes != null && node.Attributes["Action"] != null && Enum.TryParse(node.Attributes["Action"].Value, true, out mergeActionTmp))
            {
                Action = mergeActionTmp;
            }

            if (node.Attributes != null && node.Attributes["RelationshipTypeId"] != null)
            {
                RelationshipTypeId = ValueTypeHelper.ConvertToNullableInt32(node.Attributes["RelationshipTypeId"].Value);
            }

            if (node.Attributes != null && node.Attributes["TargetContainerId"] != null)
            {
                TargetContainerId = ValueTypeHelper.ConvertToNullableInt32(node.Attributes["TargetContainerId"].Value);
            }

            if (node.Attributes != null && node.Attributes["MergingProfileId"]!= null)
            {
                MergingProfileId = ValueTypeHelper.ConvertToNullableInt32(node.Attributes["MergingProfileId"].Value);
            }

            if (node.Attributes != null && node.Attributes["WorkflowName"] != null && !String.IsNullOrEmpty(node.Attributes["WorkflowName"].Value))
            {
                WorkflowName = node.Attributes["WorkflowName"].Value;
            }

            if (node.Attributes != null && node.Attributes["ActionLabelMessageCode"] != null && !String.IsNullOrEmpty(node.Attributes["ActionLabelMessageCode"].Value))
            {
                ActionLabelMessageCode = node.Attributes["ActionLabelMessageCode"].Value;
            }

        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone RangeRule
        /// </summary>
        /// <returns>Cloned RangeRule object</returns>
        public object Clone()
        {
            RangeRule cloned = new RangeRule(this);
            return cloned;
        }

        #endregion
    }
}
