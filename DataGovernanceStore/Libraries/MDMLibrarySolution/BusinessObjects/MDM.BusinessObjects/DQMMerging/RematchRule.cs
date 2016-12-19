using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMMerging
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Rematch Rule Condition
    /// </summary>
    [DataContract]
    public class RematchRule : IRematchRule
    {
        #region Fields

        /// <summary>
        /// Field for Ordr
        /// </summary>
        private Int32 _order = 0;

        /// <summary>
        /// Field for RematchRule Conditions
        /// </summary>
        private RematchRuleConditionCollection _conditions = new RematchRuleConditionCollection();

        /// <summary>
        /// Field for MatchingProfileId
        /// </summary>
        private Int32 _matchingProfileId = 0;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs RematchRule
        /// </summary>
        public RematchRule()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs RematchRule using specified instance data
        /// </summary>
        public RematchRule(RematchRule source)
            : base()
        {
            this.Order = source.Order;
            this.Conditions = (RematchRuleConditionCollection)source.Conditions.Clone();
            this.MatchingProfileId = source.MatchingProfileId;
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
        /// Property denoting RematchRule Conditions
        /// </summary>
        [DataMember]
        public RematchRuleConditionCollection Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }

        /// <summary>
        /// Property denoting MatchingProfileId
        /// </summary>
        [DataMember]
        public Int32 MatchingProfileId
        {
            get { return _matchingProfileId; }
            set { _matchingProfileId = value; }
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

            //RematchRule node start
            xmlWriter.WriteStartElement("RematchRule");

            xmlWriter.WriteStartElement("Order");
            xmlWriter.WriteValue(Order.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RematchRuleConditions");
            xmlWriter.WriteRaw(Conditions.ToXml(false));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MatchingProfileId");
            xmlWriter.WriteValue(MatchingProfileId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            //RematchRule node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads RematchRule from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            Order = 0;
            Conditions.Clear();
            MatchingProfileId = 0;
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Order")
                {
                    Int32 tmp;
                    if (Int32.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        Order = tmp;
                    }
                }
                else if (child.Name == "RematchRuleConditions")
                {
                    Conditions.LoadFromXml(child);
                }
                else if (child.Name == "MatchingProfileId")
                {
                    Int32 tmp;
                    if (Int32.TryParse(child.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        MatchingProfileId = tmp;
                    }
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone RematchRule
        /// </summary>
        /// <returns>Cloned RematchRule object</returns>
        public object Clone()
        {
            RematchRule cloned = new RematchRule(this);
            return cloned;
        }

        #endregion
    }
}