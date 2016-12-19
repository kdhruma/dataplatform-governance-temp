using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies SurvivorshipRuleAttributeValueConditionCollection
    /// </summary>
    public class SurvivorshipRuleAttributeValueConditionCollection : InterfaceContractCollection<ISurvivorshipRuleAttributeValueCondition, SurvivorshipRuleAttributeValueCondition>, ISurvivorshipRuleAttributeValueConditionCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SurvivorshipRuleAttributeValueCondition Collection
        /// </summary>
        public SurvivorshipRuleAttributeValueConditionCollection()
        { }

        /// <summary>
        /// Initialize SurvivorshipRuleAttributeValueCondition collection from IList
        /// </summary>
        /// <param name="attributeValueConditionList">Source items</param>
        public SurvivorshipRuleAttributeValueConditionCollection(IList<SurvivorshipRuleAttributeValueCondition> attributeValueConditionList)
        {
            this._items = new Collection<SurvivorshipRuleAttributeValueCondition>(attributeValueConditionList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of SurvivorshipRuleAttributeValueCondition Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            foreach (SurvivorshipRuleAttributeValueCondition item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads Condition Collection from "Conditions" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "AttributeValueCondition")
                {
                    SurvivorshipRuleAttributeValueCondition item = new SurvivorshipRuleAttributeValueCondition();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleAttributeValueCondition collection
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleAttributeValueCondition collection object</returns>
        public object Clone()
        {
            SurvivorshipRuleAttributeValueConditionCollection clonedRules = new SurvivorshipRuleAttributeValueConditionCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (SurvivorshipRuleAttributeValueCondition item in this._items)
                {
                    SurvivorshipRuleAttributeValueCondition clonedItem = item.Clone() as SurvivorshipRuleAttributeValueCondition;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}
