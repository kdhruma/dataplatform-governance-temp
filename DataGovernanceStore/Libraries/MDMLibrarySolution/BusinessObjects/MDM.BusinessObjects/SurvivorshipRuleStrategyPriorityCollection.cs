using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies SurvivorshipRuleStrategyPriorityCollection
    /// </summary>
    [DataContract]
    public class SurvivorshipRuleStrategyPriorityCollection :
        InterfaceContractCollection<ISurvivorshipRuleStrategyPriority, SurvivorshipRuleStrategyPriority>,
        ISurvivorshipRuleStrategyPriorityCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SurvivorshipRuleStrategyPriority Collection
        /// </summary>
        public SurvivorshipRuleStrategyPriorityCollection()
        {
        }

        /// <summary>
        /// Initialize SurvivorshipRuleStrategyPriority collection from IList
        /// </summary>
        /// <param name="survivorshipRuleList">Source items</param>
        public SurvivorshipRuleStrategyPriorityCollection(IList<SurvivorshipRuleStrategyPriority> survivorshipRuleList)
        {
            this._items = new Collection<SurvivorshipRuleStrategyPriority>(survivorshipRuleList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of SurvivorshipRuleStrategyPriority Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            foreach (SurvivorshipRuleStrategyPriority item in _items)
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
        /// Loads SurvivorshipRuleStrategyPriority Collection from "StrategyPriorities" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "StrategyPriority")
                {
                    SurvivorshipRuleStrategyPriority item = new SurvivorshipRuleStrategyPriority();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleStrategyPriority collection
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleStrategyPriority collection object</returns>
        public object Clone()
        {
            SurvivorshipRuleStrategyPriorityCollection clonedRules = new SurvivorshipRuleStrategyPriorityCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (SurvivorshipRuleStrategyPriority item in this._items)
                {
                    SurvivorshipRuleStrategyPriority clonedItem = item.Clone() as SurvivorshipRuleStrategyPriority;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}
