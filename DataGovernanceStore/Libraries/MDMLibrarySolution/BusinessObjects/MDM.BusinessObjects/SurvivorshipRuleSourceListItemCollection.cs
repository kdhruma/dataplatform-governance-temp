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
    /// Specifies SurvivorshipRuleSourceListItemCollection
    /// </summary>
    public class SurvivorshipRuleSourceListItemCollection : InterfaceContractCollection<ISurvivorshipRuleSourceListItem, SurvivorshipRuleSourceListItem>, ISurvivorshipRuleSourceListItemCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SurvivorshipRuleSourceListItem Collection
        /// </summary>
        public SurvivorshipRuleSourceListItemCollection()
        { }

        /// <summary>
        /// Initialize SurvivorshipRuleSourceListItem collection from IList
        /// </summary>
        /// <param name="sourceListItemList">Source items</param>
        public SurvivorshipRuleSourceListItemCollection(IList<SurvivorshipRuleSourceListItem> sourceListItemList)
        {
            this._items = new Collection<SurvivorshipRuleSourceListItem>(sourceListItemList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of SurvivorshipRuleSourceListItem Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            
            foreach (SurvivorshipRuleSourceListItem item in _items)
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
        /// Loads SurvivorshipRuleSourceListItem Collection from "SurvivorshipRuleSourceListItems" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Source")
                {
                    SurvivorshipRuleSourceListItem item = new SurvivorshipRuleSourceListItem();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleSourceListItem collection
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleSourceListItem collection object</returns>
        public object Clone()
        {
            SurvivorshipRuleSourceListItemCollection clonedRules = new SurvivorshipRuleSourceListItemCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (SurvivorshipRuleSourceListItem item in this._items)
                {
                    SurvivorshipRuleSourceListItem clonedItem = item.Clone() as SurvivorshipRuleSourceListItem;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}
