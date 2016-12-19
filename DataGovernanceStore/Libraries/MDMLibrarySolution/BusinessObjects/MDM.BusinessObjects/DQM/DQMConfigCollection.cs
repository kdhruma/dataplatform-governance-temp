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
    /// Represent class for collection of DQM config
    /// </summary>
    [DataContract]
    public class DQMConfigCollection : IDQMConfigCollection, ICloneable
    {
        private readonly Collection<IDQMConfig> _items;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Matching Profile Collection
        /// </summary>
        public DQMConfigCollection()
        {
            _items = new Collection<IDQMConfig>();
        }

        /// <summary>
        /// Initialize Matching Profiles collection from IList
        /// </summary>
        /// <param name="DQMConfigsList">Source items</param>
        public DQMConfigCollection(IList<IDQMConfig> DQMConfigsList)
        {
            this._items = new Collection<IDQMConfig>(DQMConfigsList);
        }

        /// <summary>
        /// Initialize Matching Profiles collection from xml
        /// </summary>
        /// <param name="xml">Xml String</param>
        public DQMConfigCollection(String xml)
            : this()
        {
            LoadFromXml(xml);
        }

        #endregion
        
        #region ICloneable Members

        /// <summary>
        /// Clone DQMConfig collection
        /// </summary>
        /// <returns>Cloned DQMConfig collection object</returns>
        public object Clone()
        {
            DQMConfigCollection clonedItems = new DQMConfigCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DQMConfig item in this._items)
                {
                    DQMConfig clonedItem = item.Clone() as DQMConfig;
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;
        }

        #endregion

        /// <summary>
        /// Indexer to getting IDQMConfig by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IDQMConfig this[Int32 index]
        {
            get { return _items[index]; }
        }

        /// <summary>
        /// Get an enumerator that iterates through DQM config collection
        /// </summary>
        /// <returns>Returns an enumerator that iterates through DQM config collection</returns>
        public IEnumerator<IDQMConfig> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        /// <summary>
        /// Add DQM config object in collection
        /// </summary>
        /// <param name="item">Indicates DQM config object to add in collection</param>
        public void Add(IDQMConfig item)
        {
            this._items.Add(item);
        }

        /// <summary>
        /// Removes all DQM config from collection
        /// </summary>
        public void Clear()
        {
            this._items.Clear();
        }

        /// <summary>
        /// Determines whether the DQM config collection contains a specific DQM config
        /// </summary>
        /// <param name="item">Indicates the DQM config object to locate in the DQM config collection</param>
        /// <returns>
        /// <para>true : If specified DQM config is found in DQM config collection</para>
        /// <para>false : If specified DQM config is not found in DQM config collection</para>
        /// </returns>
        public bool Contains(IDQMConfig item)
        {
            return this._items.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DQM config collection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DQM config collection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(IDQMConfig[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific DQM config from the DQM config collection
        /// </summary>
        /// <param name="item">Indicates the DQM config object to remove from the DQM config collection</param>
        /// <returns>Returns true if specified DQM config is successfully removed; otherwise false
        /// This method also returns false if DQM config is not found in the original collection
        /// </returns>
        public bool Remove(IDQMConfig item)
        {
            return this._items.Remove(item);
        }

        /// <summary>
        /// Get the count of DQM config in DQM config collection
        /// </summary>
        public int Count
        {
            get { return this._items.Count; }
        }

        /// <summary>
        /// Get the count of DQM config in DQM config collection
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        /// <summary>
        /// Get an enumerator that iterates through DQM config collection
        /// </summary>
        /// <returns>Returns an enumerator that iterates through DQM config collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #region Xml Serialization
        
        /// <summary>
        /// Gets Xml representation of DQMConfig Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("DQMConfigCollection");

            foreach (DQMConfig item in _items)
            {
                xmlWriter.WriteRaw(item.ToXmlWithOuterNode());
            }

            // DQMConfigCollection node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads current object from provided XmlNode
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
                if (child.Name == "DQMConfig")
                {
                    DQMConfig item = new DQMConfig();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads current object from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("DQMConfigCollection");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion
    }
}