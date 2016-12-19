using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Represent class for collection of DQM rule
    /// </summary>
    [DataContract]
    public class DQMRuleCollection : IDQMRuleCollection, ICloneable
    {
        private readonly Collection<IDQMRule> _items;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Matching Profile Collection
        /// </summary>
        public DQMRuleCollection()
        {
            _items = new Collection<IDQMRule>();
        }

        /// <summary>
        /// Initialize Matching Profiles collection from IList
        /// </summary>
        /// <param name="DQMRulesList">Source items</param>
        public DQMRuleCollection(IList<IDQMRule> DQMRulesList)
        {
            this._items = new Collection<IDQMRule>(DQMRulesList);
        }

        /// <summary>
        /// Initialize Matching Profiles collection from Xml
        /// </summary>
        /// <param name="xml">Xml String</param>
        public DQMRuleCollection(String xml)
            : this()
        {
            LoadFromXml(xml);
        }

        #endregion
        
        #region ICloneable Members

        /// <summary>
        /// Clone DQMRule collection
        /// </summary>
        /// <returns>Cloned DQMRule collection object</returns>
        public object Clone()
        {
            DQMRuleCollection clonedRules = new DQMRuleCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DQMRule rule in this._items)
                {
                    DQMRule clonedRule = rule.Clone() as DQMRule;
                    clonedRules.Add(clonedRule);
                }
            }

            return clonedRules;
        }

        #endregion

        /// <summary>
        /// Find IDQM rule from DQM collection based on index passed in parameter
        /// </summary>
        /// <param name="index">Indicates the index based on which IDQM rule is returned</param>
        /// <returns>Returns IDQM rule from DQM collection based on index passed in parameter</returns>
        public IDQMRule this[Int32 index]
        {
            get { return _items[index]; }
        }

        /// <summary>
        /// Get an enumerator that iterates through a DQM rule collection 
        /// </summary>
        /// <returns>Returns an IEnumerator object that iterate through a DQM collection</returns>
        public IEnumerator<IDQMRule> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        /// <summary>
        /// Add DQM rule object in collection
        /// </summary>
        /// <param name="item">Indicates the DQM rule object to add in collection</param>
        public void Add(IDQMRule item)
        {
            this._items.Add(item);
        }

        /// <summary>
        /// Removes all DQM rule from its collection
        /// </summary>
        public void Clear()
        {
            this._items.Clear();
        }

        /// <summary>
        /// Determines whether the DQM rule collection contains a specific DQM rule
        /// </summary>
        /// <param name="item">Indicates the DQM rule object to locate in the DQM rule collection</param>
        /// <returns>
        /// <para>true : If DQM rule is found in DQM rule collection</para>
        /// <para>false : If DQM rule is not found in DQM rule collection</para>
        /// </returns>
        public bool Contains(IDQMRule item)
        {
            return this._items.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DQM rule collection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">Indicates the one-dimensional System.Array that is the destination of the elements
        ///  copied from DQM rule collection. The System.Array must have zero-based indexing</param>
        /// <param name="arrayIndex">Indicates the zero-based index in array at which copying begins</param>
        public void CopyTo(IDQMRule[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DQM rule collection
        /// </summary>
        /// <param name="item">Indicates the DQM rule object to remove from the DQM rule collection</param>
        /// <returns>
        /// Returns true if item is successfully removed; otherwise, false. 
        /// This method also returns false if item was not found in the original DQM rule collection
        /// </returns>
        public bool Remove(IDQMRule item)
        {
            return this._items.Remove(item);
        }

        /// <summary>
        /// Get the count of DQM rule in DQM rule collection
        /// </summary>
        public int Count
        {
            get { return this._items.Count; }
        }

        /// <summary>
        /// Check if DQM rule is read only
        /// </summary>
        public bool IsReadOnly 
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        /// <summary>
        /// Get an enumerator that iterates through a DQM rule collection
        /// </summary>
        /// <returns>Returns an enumerator that iterates through a DQM rule collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current collection
        /// </summary>
        /// <returns>Returns Xml representation of current collection as string</returns>
        public String ToXml()
        {
            String result = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("DQMRuleCollection");
                foreach (DQMRule item in _items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                //Get the actual XML
                result = sw.ToString();
            }

            return result;
        }

        /// <summary>
        /// Loads current collection from provided XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            if (node == null)
            {
                return;
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "DQMRule")
                {
                    DQMRule item = new DQMRule();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads current collection from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("DQMRuleCollection");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

    }
}
