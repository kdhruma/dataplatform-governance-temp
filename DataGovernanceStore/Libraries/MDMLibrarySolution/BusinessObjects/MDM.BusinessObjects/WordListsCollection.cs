using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies WordLists Collection
    /// </summary>
    [DataContract]
    public class WordListsCollection : InterfaceContractCollection<IWordList, WordList>, IWordListsCollection, IEquatable<WordListsCollection>
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public WordListsCollection()
        {
        }

        /// <summary>
        /// Initialize WordListCollection from xml
        /// </summary>
        /// <param name="xml"></param>
        public WordListsCollection(String xml)
        {
            LoadFromXmlWithOuterNode(xml);
        }

        /// <summary>
        /// Initialize WordListsCollection from IList
        /// </summary>
        /// <param name="wordLists">IList of WordLists</param>
        public WordListsCollection(IList<WordList> wordLists)
        {
            this._items = new Collection<WordList>(wordLists);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Denotes method for cloning word lists collection
        /// </summary>
        /// <returns>Cloned Word lists collection object</returns>
        public Object Clone()
        {
            WordListsCollection clonedLists = new WordListsCollection();

            if (!_items.IsNullOrEmpty())
            {
                foreach (WordList item in _items)
                {
                    WordList clonedList = item.Clone() as WordList;
                    clonedLists.Add(clonedList);
                }
            }

            return clonedLists;
        }

        /// <summary>
        /// Denotes method for xml serialization
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("WordListsCollection");

            foreach (WordList wordList in this)
            {
                xmlWriter.WriteRaw(wordList.ToXml());
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

                XmlNode node = doc.SelectSingleNode("WordListsCollection");
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
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "WordList")
                {
                    WordList item = new WordList();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Denotes method for comparing word lists collections
        /// </summary>
        public Boolean Equals(WordListsCollection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.Count() == other.Count() &&
                   !this.Except(other).Any() &&
                   !other.Except(this).Any();
        }

        /// <summary>
        /// Denotes method for comparing word lists collections
        /// </summary>
        public override Boolean Equals(Object obj)
        {
            return Equals(obj as WordListsCollection);
        }

        /// <summary>
        /// Denotes method for getting hashcode
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = 0;
                foreach (WordList wordList in this)
                {
                    hashCode += (wordList != null ? wordList.GetHashCode() : 0);
                }
                return hashCode;
            }
        }

        #endregion

        #endregion
    }
}