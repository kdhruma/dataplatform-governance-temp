using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies WordElements Collection
    /// </summary>
    [DataContract]
    public class WordElementsCollection : InterfaceContractCollection<IWordElement, WordElement>, IWordElementsCollection, IEquatable<WordElementsCollection>, IDataModelObjectCollection
    {
        #region Fields

        private Int32? _totalWordsCount = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public WordElementsCollection()
        {
        }

        /// <summary>
        /// Initialize WordElementsCollection from IList
        /// </summary>
        /// <param name="wordElementsList">IList of WordElements</param>
        public WordElementsCollection(IList<WordElement> wordElementsList)
        {
            this._items = new Collection<WordElement>(wordElementsList);
        }

        /// <summary>
        /// Initialize WordElementsCollection from String in Xml format
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public WordElementsCollection(String valuesAsXml)
        {
            LoadFromXmlWithOuterNode(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates TotalWordsCount
        /// </summary>
        [DataMember]
        public Int32? TotalWordsCount
        {
            get { return _totalWordsCount; }
            set { _totalWordsCount = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an DataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return ObjectType.WordElement;

            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Denotes method that clone Word elements collection
        /// </summary>
        /// <returns>Cloned Word elements collection object</returns>
        public Object Clone()
        {
            WordElementsCollection clonedElements = new WordElementsCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (WordElement item in this._items)
                {
                    WordElement clonedElement = item.Clone() as WordElement;
                    clonedElements.Add(clonedElement);
                }
            }
            clonedElements.TotalWordsCount = this.TotalWordsCount;

            return clonedElements;
        }

        /// <summary>
        /// Denotes method for xml serialization
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("WordElementsCollection");

            if (TotalWordsCount.HasValue)
            {
                xmlWriter.WriteStartElement("TotalWordsCount");

                xmlWriter.WriteRaw(TotalWordsCount.ToString());

                xmlWriter.WriteEndElement();
            }

            foreach (WordElement wordElement in this)
            {
                xmlWriter.WriteRaw(wordElement.ToXml());
            }

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Denotes method for xml deserialization
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("WordElementsCollection");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        /// <summary>
        /// Denotes method for xml deserialization
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _totalWordsCount = null;
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "TotalWordsCount":
                        String nodeValue = child.InnerXml;
                        TotalWordsCount = ValueTypeHelper.ConvertToNullableInt32(nodeValue);
                        break;
                    case "WordElement":
                        WordElement item = new WordElement();
                        item.LoadFromXml(child);
                        _items.Add(item);
                        break;
                }

            }
        }

        /// <summary>
        /// Denotes method for comparing word elements collections
        /// </summary>
        public Boolean Equals(WordElementsCollection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.TotalWordsCount == other.TotalWordsCount &&
                   this.Count() == other.Count() &&
                   !this.Except(other).Any() &&
                   !other.Except(this).Any();
        }

        /// <summary>
        /// Denotes method for comparing word elements collections
        /// </summary>
        public override Boolean Equals(Object obj)
        {
            return Equals(obj as WordElementsCollection);
        }

        /// <summary>
        /// Denotes method for getting hashcode
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = _totalWordsCount.GetHashCode();
                foreach (WordElement wordElement in this)
                {
                    hashCode += (wordElement != null ? wordElement.GetHashCode() : 0);
                }
                return hashCode;
            }
        }

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Remove DataModelObject based on reference id
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an Word Elements which should be fetched</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            WordElementsCollection wordElements = GetWordElements(referenceIds);

            if (!wordElements.IsNullOrEmpty())
            {
                foreach (WordElement wordElement in wordElements)
                {
                    result = result && Remove(wordElement);
                }
            }

            return result;
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> wordElementsInBatch = null;

            if (this._items != null)
            {
                wordElementsInBatch = Utility.Split(this, batchSize);
            }

            return wordElementsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as WordElement);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        ///  Gets the word elements using the reference id
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of word element which should be fetched</param>
        /// <returns>Returns filtered word elements</returns>
        private WordElementsCollection GetWordElements(Collection<String> referenceIds)
        {
            WordElementsCollection wordElements = new WordElementsCollection();

            if (!_items.IsNullOrEmpty() && !referenceIds.IsNullOrEmpty())
            {
                HashSet<String> ids = new HashSet<String>(referenceIds);

                foreach (WordElement wordElement in _items)
                {
                    if (ids.Contains(wordElement.ReferenceId))
                    {
                        wordElements.Add(wordElement);
                    }
                }
            }

            return wordElements;
        }

        #endregion

        #endregion
    }
}