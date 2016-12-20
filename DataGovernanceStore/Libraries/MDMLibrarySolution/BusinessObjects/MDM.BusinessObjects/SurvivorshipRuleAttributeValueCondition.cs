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
    /// Specifies SurvivorshipRuleAttributeValueCondition
    /// </summary>
    [DataContract]
    public class SurvivorshipRuleAttributeValueCondition : ISurvivorshipRuleAttributeValueCondition
    {
        #region Fields
        
        /// <summary>
        /// Field for TargetAttributeId
        /// </summary>
        private Int64 _targetAttributeId = 0;

        /// <summary>
        /// Field for Operator
        /// </summary>
        private SearchOperator _operator = SearchOperator.EqualTo;

        /// <summary>
        /// Field for search value CDATA
        /// </summary>
        private String _value = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Property denotes TargetAttributeId
        /// </summary>
        [DataMember]
        public Int64 TargetAttributeId
        {
            get { return _targetAttributeId; }
            set { _targetAttributeId = value; }
        }

        /// <summary>
        /// Property denotes Operator
        /// </summary>
        [DataMember]
        public SearchOperator Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        /// <summary>
        /// Property denotes search value CDATA
        /// </summary>
        [DataMember]
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs SurvivorshipRuleAttributeValueCondition
        /// </summary>
        public SurvivorshipRuleAttributeValueCondition()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs SurvivorshipRuleAttributeValueCondition using specified instance data
        /// </summary>
        public SurvivorshipRuleAttributeValueCondition(SurvivorshipRuleAttributeValueCondition source)
            : base()
        {
            this.TargetAttributeId = source.TargetAttributeId;
            this.Operator = source.Operator;
            this.Value = source.Value;
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

            //SurvivorshipRuleAttributeValueCondition node start
            xmlWriter.WriteStartElement("AttributeValueCondition");

            xmlWriter.WriteAttributeString("TgtAttrId", TargetAttributeId.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("Operator", Operator.ToString());

            xmlWriter.WriteCData(Value);

            //SurvivorshipRuleAttributeValueCondition node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads SurvivorshipRuleAttributeValueCondition from "AttributeValueCondition" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            TargetAttributeId = 0;
            Operator = SearchOperator.EqualTo;
            Value = String.Empty;

            if (node.Attributes != null)
            {
                if (node.Attributes["TgtAttrId"] != null)
                {
                    Int64 tmp;
                    if (Int64.TryParse(node.Attributes["TgtAttrId"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        TargetAttributeId = tmp;
                    }
                }
                if (node.Attributes["Operator"] != null)
                {
                    SearchOperator operatorTmp;
                    if (Enum.TryParse(node.Attributes["Operator"].Value, true, out operatorTmp))
                    {
                        Operator = operatorTmp;
                    }
                }
            }
            if (node.ChildNodes.Count == 1)
            {
                XmlNode childNode = node.ChildNodes[0];
                if (childNode.NodeType == XmlNodeType.Text || childNode.NodeType == XmlNodeType.CDATA)
                {
                    Value = childNode.Value;
                }
            }

        }

        /// <summary>
        /// Check whether condition is passed.
        /// Works for simple attributes only
        /// </summary>
        /// <param name="targetAttribute">Attribute to be examinated for condition</param>
        /// <returns>Boolean flag indicates whether attribute satisfies condition or not</returns>
        public Boolean IsSatisfiedFor(Attribute targetAttribute)
        {
            // Ignoring lookups, complex attributes and collections
            if (targetAttribute == null || targetAttribute.IsLookup || targetAttribute.IsComplex ||
                targetAttribute.IsCollection)
            {
                return false;
            }

            var result = this.TargetAttributeId == targetAttribute.Id &&
                         GetCompareFunc(targetAttribute)(this.Value, targetAttribute.GetCurrentValue().ToString());

            return result; 
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns comparison function according to operator
        /// </summary>
        /// <param name="targetAttribute">Target attribute</param>
        /// <returns>Function to be executed against target attribute with specific values</returns>
        private Func<String, String, Boolean> GetCompareFunc(Attribute targetAttribute)
        {
            Boolean isNumber = targetAttribute.AttributeDataType == AttributeDataType.Integer ||
                               targetAttribute.AttributeDataType == AttributeDataType.Decimal ||
                               targetAttribute.AttributeDataType == AttributeDataType.Fraction;

            Func<String, String, Boolean> result = (attrVal, valToCompare) => false;

            switch (this.Operator)
            {
                case SearchOperator.Like:
                    // TODO[IN] Check comparison logic for like
                case SearchOperator.Contains:
                    result = (attrVal, valToCompare) => attrVal.Contains(valToCompare);
                    break;

                case SearchOperator.GreaterThanOrEqualTo:
                    
                    result = (attrVal, valToCompare) => isNumber && ValueTypeHelper.DecimalTryParse(attrVal, 0) >= ValueTypeHelper.DecimalTryParse(valToCompare, 0);
                    break;

                case SearchOperator.GreaterThan:

                    result = (attrVal, valToCompare) => isNumber && ValueTypeHelper.DecimalTryParse(attrVal, 0) > ValueTypeHelper.DecimalTryParse(valToCompare, 0);
                    break;

                case SearchOperator.HasNoValue:

                    result = (attVal, valToCompare) => String.IsNullOrEmpty(attVal);
                    break;

                case SearchOperator.HasValue:
                    result = (attVal, valToCompare) => !String.IsNullOrEmpty(attVal);
                    break;

                case SearchOperator.SubsetOf:
                case SearchOperator.In:

                    result = (attVal, valToCompare) => valToCompare.Contains(attVal);
                    break;

                case SearchOperator.LessThan:

                    result = (attrVal, valToCompare) => isNumber && ValueTypeHelper.DecimalTryParse(attrVal, 0) < ValueTypeHelper.DecimalTryParse(valToCompare, 0);
                    break;

                case SearchOperator.LessThanOrEqualTo:

                    result = (attrVal, valToCompare) => isNumber && ValueTypeHelper.DecimalTryParse(attrVal, 0) <= ValueTypeHelper.DecimalTryParse(valToCompare, 0);
                    break;

                case SearchOperator.None:

                    result = (attVal, valToCompare) => false;
                    break;

                case SearchOperator.NotContains:

                    result = (attVal, valToCompare) => !attVal.Contains(valToCompare);
                    break;

                case SearchOperator.NotIn:

                    result = (attVal, valToCompare) => !valToCompare.Contains(attVal);
                    break;
               
                case SearchOperator.EqualTo:
                default:
                    result = (attrVal, valToCompare) => attrVal.Equals(valToCompare);
                    break;
            }

            return result;
        }

        #endregion Private Methods

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleAttributeValueCondition
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleAttributeValueCondition object</returns>
        public object Clone()
        {
            SurvivorshipRuleAttributeValueCondition cloned = new SurvivorshipRuleAttributeValueCondition(this);
            return cloned;
        }

        #endregion
    }
}
