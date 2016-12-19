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
    /// Specifies Rematch Rule Condition
    /// </summary>
    [DataContract]
    [KnownType(typeof(MatchResultSetAggregateFunction))]
    public class RematchRuleCondition : IRematchRuleCondition
    {
        #region Fields

        /// <summary>
        /// Field for Aggregate Function
        /// </summary>
        private MatchResultSetAggregateFunction _aggregateFunction = MatchResultSetAggregateFunction.Unknown;

        /// <summary>
        /// Field for Operator
        /// </summary>
        private SearchOperator _operator = SearchOperator.EqualTo;

        /// <summary>
        /// Field for Value
        /// </summary>
        private Double _value = 0.0;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs RematchRuleCondition
        /// </summary>
        public RematchRuleCondition()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs RematchRuleCondition using specified instance data
        /// </summary>
        public RematchRuleCondition(RematchRuleCondition source)
            : base()
        {
            this.AggregateFunction = source.AggregateFunction;
            this.Operator = source.Operator;
            this.Value = source.Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Aggregate Function
        /// </summary>
        [DataMember]
        public MatchResultSetAggregateFunction AggregateFunction
        {
            get { return _aggregateFunction; }
            set { _aggregateFunction = value; }
        }

        /// <summary>
        /// Property denoting Operator
        /// </summary>
        [DataMember]
        public SearchOperator Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        /// <summary>
        /// Property denoting Value
        /// </summary>
        [DataMember]
        public Double Value
        {
            get { return _value; }
            set { _value = value; }
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

            //RematchRuleCondition node start
            xmlWriter.WriteStartElement("RematchRuleCondition");

            xmlWriter.WriteAttributeString("AggregateFunction", AggregateFunction.ToString());

            xmlWriter.WriteAttributeString("Operator", Utility.GetOperatorString(Operator));

            xmlWriter.WriteAttributeString("Value", Value.ToString(CultureInfo.InvariantCulture));

            //RematchRuleCondition node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads RematchRuleCondition from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            AggregateFunction = MatchResultSetAggregateFunction.Unknown;
            Operator = SearchOperator.EqualTo;
            Value = 0.0;

            if (node.Attributes != null)
            {
                if (node.Attributes["AggregateFunction"] != null)
                {
                    MatchResultSetAggregateFunction aggregateFunction;
                    if (Enum.TryParse(node.Attributes["AggregateFunction"].Value, true, out aggregateFunction))
                    {
                        AggregateFunction = aggregateFunction;
                    }
                }
                if (node.Attributes["Operator"] != null)
                {
                    Operator = Utility.GetOperatorEnum(node.Attributes["Operator"].Value);
                }
                if (node.Attributes["Value"] != null)
                {
                    Double tmp;
                    if (Double.TryParse(node.Attributes["Value"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        Value = tmp;
                    }
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone RematchRuleCondition
        /// </summary>
        /// <returns>Cloned RematchRuleCondition object</returns>
        public object Clone()
        {
            RematchRuleCondition cloned = new RematchRuleCondition(this);
            return cloned;
        }

        #endregion
    }
}