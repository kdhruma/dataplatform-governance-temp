using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMMerging
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies MatchReviewProfile
    /// </summary>
    [DataContract]
    public class MatchReviewProfile : MDMObject, IMatchReviewProfile
    {
        #region Fields

        /// <summary>
        /// Field for MatchResultSetRules
        /// </summary>
        private RematchRuleCollection _matchResultSetRules = new RematchRuleCollection();

        /// <summary>
        /// Field for MatchResultsRule
        /// </summary>
        private MergePlanningRule _matchResultsRule = new MergePlanningRule();

        /// <summary>
        /// Field for match review workflow name
        /// </summary>
        private String _matchReviewWorkflowName;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs MatchReviewProfile
        /// </summary>
        public MatchReviewProfile()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs MatchReviewProfile using specified instance data
        /// </summary>
        public MatchReviewProfile(MatchReviewProfile source)
            : base(source.Id, source.Name, source.LongName, source.Locale, source.AuditRefId, source.ProgramName)
        {
            this.Action = source.Action;
            this.MatchResultSetRules = (RematchRuleCollection)source.MatchResultSetRules.Clone();
            this.MatchResultsRule = (MergePlanningRule)source.MatchResultsRule.Clone();
            this.MatchReviewWorkflowName = source.MatchReviewWorkflowName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting MatchResultSetRules
        /// </summary>
        [DataMember]
        public RematchRuleCollection MatchResultSetRules
        {
            get { return _matchResultSetRules; }
            set { _matchResultSetRules = value; }
        }

        /// <summary>
        /// Property denoting MatchResultsRule
        /// </summary>
        [DataMember]
        public MergePlanningRule MatchResultsRule
        {
            get { return _matchResultsRule; }
            set { _matchResultsRule = value; }
        }

        /// <summary>
        /// Property denoting the name of the match review workflow which needs to be invoked when multiple entities or suspects are found.
        /// </summary>
        [DataMember]
        public String MatchReviewWorkflowName
        {
            get
            {
                return _matchReviewWorkflowName;
            }
            set
            {
                _matchReviewWorkflowName = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation (excluding MDMObject's properties) of MatchingRuleset
        /// </summary>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MergePlanningProfile node start
            xmlWriter.WriteStartElement("MatchReviewProfile");

            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("Action", Action.ToString());
            xmlWriter.WriteAttributeString("MatchReviewWorkflowName", MatchReviewWorkflowName);

            xmlWriter.WriteStartElement("MatchResultSetRules");
            xmlWriter.WriteRaw(MatchResultSetRules.ToXml(false));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MatchResultsRule");
            xmlWriter.WriteRaw(MatchResultsRule.ToXml(false));
            xmlWriter.WriteEndElement();

            //MergePlanningProfile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MatchReviewProfile (excluding MDMObject's properties) from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            MatchResultSetRules.Clear();
            MatchResultsRule = new MergePlanningRule();

            if (node.Attributes != null)
            {
                if (node.Attributes["Name"] != null && !String.IsNullOrEmpty(node.Attributes["Name"].Value))
                {
                    Name = node.Attributes["Name"].Value;
                }

                ObjectAction objectActionTmp;
                if (node.Attributes["Action"] != null && Enum.TryParse(node.Attributes["Action"].Value, true, out objectActionTmp))
                {
                    Action = objectActionTmp;
                }

                if (node.Attributes["MatchReviewWorkflowName"] != null && !String.IsNullOrEmpty(node.Attributes["MatchReviewWorkflowName"].Value))
                {
                    MatchReviewWorkflowName = node.Attributes["MatchReviewWorkflowName"].Value;
                }
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "MatchResultSetRules")
                {
                    MatchResultSetRules.LoadFromXml(child);
                }
                else if (child.Name == "MatchResultsRule")
                {
                    MatchResultsRule.LoadFromXml(child);
                }
            }
        }

        /// <summary>
        /// Loads MatchReviewProfile (excluding MDMObject's properties) from XML with outer node
        /// </summary>
        public virtual void LoadFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlWithOuterNode);
            XmlNode node = doc.SelectSingleNode("MatchReviewProfile");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MatchReviewProfile
        /// </summary>
        /// <returns>Cloned MatchReviewProfile object</returns>
        public object Clone()
        {
            MatchReviewProfile cloned = new MatchReviewProfile(this);
            return cloned;
        }

        #endregion
    }
}