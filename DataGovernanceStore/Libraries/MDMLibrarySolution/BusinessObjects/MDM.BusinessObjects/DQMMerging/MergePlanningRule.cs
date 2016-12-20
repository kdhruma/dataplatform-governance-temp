using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMMerging
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies MergePlanning Rule
    /// </summary>
    [DataContract]
    public class MergePlanningRule : IMergePlanningRule
    {
        #region Fields

        /// <summary>
        /// Field for Default MergeAction
        /// </summary>
        private MergeAction _defaultAction = MergeAction.Unknown;

        /// <summary>
        /// Field for RangeRules Collection
        /// </summary>
        private RangeRuleCollection _rangeRules = new RangeRuleCollection();

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
        /// Filed indicates merging profile id
        /// </summary>
        private Int32? _mergingProfileId;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs MergePlanningRule
        /// </summary>
        public MergePlanningRule()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs MergePlanningRule using specified instance data
        /// </summary>
        public MergePlanningRule(MergePlanningRule source)
            : base()
        {
            this.DefaultAction = source.DefaultAction;
            this.RangeRules = (RangeRuleCollection)source.RangeRules.Clone();
            this.RelationshipTypeId = source.RelationshipTypeId;
            this.TargetContainerId = source.TargetContainerId;
            this.WorkflowName = source.WorkflowName;
            this.ActionLabelMessageCode = source.ActionLabelMessageCode;
            this.MergingProfileId = source.MergingProfileId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Default MergeAction
        /// </summary>
        [DataMember]
        public MergeAction DefaultAction
        {
            get { return _defaultAction; }
            set { _defaultAction = value; }
        }

        /// <summary>
        /// Property denoting RangeRules Collection
        /// </summary>
        [DataMember]
        public RangeRuleCollection RangeRules
        {
            get { return _rangeRules; }
            set { _rangeRules = value; }
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
        /// Property denoting target Container Id
        /// </summary>
        [DataMember]
        public Int32? TargetContainerId
        {
            get { return _targetContainerId; }
            set { _targetContainerId = value; }
        }

        /// <summary>
        /// Property denoting Worflow Name
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
        /// Property denotes merging profile id
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
        /// Get Xml representation of MergePlanningRule
        /// </summary>
        public String ToXml(Boolean withOuterNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //MergePlanningRules node start
                xmlWriter.WriteStartElement("MergePlanningRule");
            }

            xmlWriter.WriteStartElement("DefaultAction");
            xmlWriter.WriteValue(DefaultAction.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RangeRules");
            xmlWriter.WriteRaw(RangeRules.ToXml(false));
            xmlWriter.WriteEndElement();

            if (RelationshipTypeId.HasValue)
            {
                xmlWriter.WriteStartElement("RelationshipTypeId");
                xmlWriter.WriteValue(RelationshipTypeId.ToString());
                xmlWriter.WriteEndElement();                
            }

            if (MergingProfileId.HasValue)
            {
                xmlWriter.WriteStartElement("MergingProfileId");
                xmlWriter.WriteValue(MergingProfileId.ToString());
                xmlWriter.WriteEndElement();
            }

            if (TargetContainerId.HasValue)
            {
                xmlWriter.WriteStartElement("TargetContainerId");
                xmlWriter.WriteValue(TargetContainerId.ToString());
                xmlWriter.WriteEndElement();                
            }

            xmlWriter.WriteStartElement("WorkflowName");
            xmlWriter.WriteValue(WorkflowName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ActionLabelMessageCode");
            xmlWriter.WriteValue(ActionLabelMessageCode);
            xmlWriter.WriteEndElement();

            if (withOuterNode)
            {
                //MergePlanningRule node end
                xmlWriter.WriteEndElement();
            }

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MergePlanningRule from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            DefaultAction = MergeAction.Unknown;
            RangeRules.Clear();

            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "DefaultAction":
                        MergeAction mergeActionTmp;
                        if (Enum.TryParse(child.InnerText, true, out mergeActionTmp))
                        {
                            DefaultAction = mergeActionTmp;
                        }
                        break;
                    case "RangeRules":
                        RangeRules.LoadFromXml(child);
                        break;
                    case "RelationshipTypeId":
                        RelationshipTypeId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                        break;
                    case "MergingProfileId":
                        MergingProfileId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                        break;
                    case "TargetContainerId":
                        TargetContainerId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                        break;
                    case "WorkflowName":
                        WorkflowName = child.InnerText;
                        break;
                    case "ActionLabelMessageCode":
                        ActionLabelMessageCode = child.InnerText;
                        break;
                }
            }
        }

        /// <summary>
        /// Loads MergePlanningRule from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("MergePlanningRule");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }


        /// <summary>
        /// Gets the range rule based on merge action.
        /// </summary>
        /// <param name="mergeAction">The merge action.</param>
        /// <returns>Range rule based on the merge action</returns>
        public RangeRule GetRangeRule(MergeAction mergeAction)
        {
            if (_rangeRules != null && _rangeRules.Count > 0)
            {
                foreach (RangeRule rangeRule in _rangeRules)
                {
                    if (rangeRule.Action == mergeAction)
                    {
                        return rangeRule;
                    }
                }
            }

            return null;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MergePlanningRule
        /// </summary>
        /// <returns>Cloned MergePlanningRule object</returns>
        public object Clone()
        {
            MergePlanningRule cloned = new MergePlanningRule(this);
            return cloned;
        }

        #endregion
    }
}