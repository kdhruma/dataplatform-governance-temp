using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies SurvivorshipRuleStrategyPriority
    /// </summary>
    [DataContract]
    public class SurvivorshipRuleStrategyPriority : ISurvivorshipRuleStrategyPriority
    {
        #region Fields

        /// <summary>
        /// Field for Strategy
        /// </summary>
        private RulesetStrategy _strategy = RulesetStrategy.Unknown;

        /// <summary>
        /// Field for Priority
        /// </summary>
        private Int32 _priority = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Property denotes Strategy
        /// </summary>
        [DataMember]
        public RulesetStrategy Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }
        
        /// <summary>
        /// Property denotes Priority
        /// </summary>
        [DataMember]
        public Int32 Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs SurvivorshipRuleStrategyPriority
        /// </summary>
        public SurvivorshipRuleStrategyPriority()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs SurvivorshipRuleStrategyPriority using specified instance data
        /// </summary>
        public SurvivorshipRuleStrategyPriority(SurvivorshipRuleStrategyPriority source)
            : base()
        {
            this.Strategy = source.Strategy;
            this.Priority = source.Priority;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MatchingRule
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //SurvivorshipRuleStrategyPriority node start
            xmlWriter.WriteStartElement("StrategyPriority");

            xmlWriter.WriteAttributeString("Strategy", Strategy.ToString());

            xmlWriter.WriteAttributeString("Priority", Priority.ToString(CultureInfo.InvariantCulture));

            //SurvivorshipRuleStrategyPriority node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads SurvivorshipRuleStrategyPriority from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            Strategy = RulesetStrategy.Unknown;
            Priority = 0;
            if (node.Attributes != null)
            {
                if (node.Attributes["Strategy"] != null)
                {
                    RulesetStrategy rulesetStrategyTmp;
                    if (Enum.TryParse(node.Attributes["Strategy"].Value, true, out rulesetStrategyTmp))
                    {
                        Strategy = rulesetStrategyTmp;
                    }
                }
                if (node.Attributes["Priority"] != null)
                {
                    Int32 tmp;
                    if (Int32.TryParse(node.Attributes["Priority"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        Priority = tmp;
                    }
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleStrategyPriority
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleStrategyPriority object</returns>
        public object Clone()
        {
            SurvivorshipRuleStrategyPriority cloned = new SurvivorshipRuleStrategyPriority(this);
            return cloned;
        }

        #endregion
    }
}
