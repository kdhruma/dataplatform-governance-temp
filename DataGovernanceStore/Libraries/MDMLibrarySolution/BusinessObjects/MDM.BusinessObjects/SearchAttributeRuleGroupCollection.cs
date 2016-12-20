using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the specifications for AttributeRules for export syndication profile. 
    /// </summary>
    [DataContract]
    public class SearchAttributeRuleGroupCollection : ISearchAttributeRuleGroupCollection, ICollection<SearchAttributeRuleGroup>, IEnumerable<SearchAttributeRuleGroup>
    {
        #region Fields

        /// <summary>
        /// Collection of SearchAttributeRuleGroup Object
        /// </summary>
        [DataMember]
        Collection<SearchAttributeRuleGroup> _searchAttributeRuleGroups = new Collection<SearchAttributeRuleGroup>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search Attribute Rule Group class.
        /// </summary>
        public SearchAttributeRuleGroupCollection()
            : base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchAttributeRuleGroupCollection object</param>
        public SearchAttributeRuleGroupCollection(String valuesAsXml)
        {
            LoadSearchAttributeRuleGroups(valuesAsXml);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region ISearchAttributeRuleGroupCollection

        /// <summary>
        /// Get the XML presentation of an SearchAttributeRuleGroupCollection
        /// </summary>
        /// <returns>Xml representation of Search Attribute Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when SearchAttributeRuleGroupCollection object passed is null</exception>
        public String ToXml()
        {
            if (this._searchAttributeRuleGroups == null)
                throw new ArgumentNullException("SearchAttributeRuleGroupCollection");

            String searchAttributeRuleGroupXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("SearchAttributeRuleGroups");

            foreach (SearchAttributeRuleGroup searchAttributeRuleGroup in this._searchAttributeRuleGroups)
            {
                searchAttributeRuleGroupXml = String.Concat(searchAttributeRuleGroupXml, searchAttributeRuleGroup.ToXml());
            }
            xmlWriter.WriteRaw(searchAttributeRuleGroupXml);
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            searchAttributeRuleGroupXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchAttributeRuleGroupXml;
        }

        #endregion

        #region IEnumerable<SearchAttributeRuleGroup>

        /// <summary>
        /// Returns an enumerator that iterates through a SearchAttributeRuleGroupCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<SearchAttributeRuleGroup> GetEnumerator()
        {
            return this._searchAttributeRuleGroups.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a SearchAttributeRuleGroupCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._searchAttributeRuleGroups.GetEnumerator();
        }

        #endregion

        #region ICollection<SearchAttributeRuleGroup>

        /// <summary>
        /// Add SearchAttributeRuleGroup Object in collection
        /// </summary>
        /// <param name="item">SearchAttributeRuleGroup to add in collection</param>
        public void Add(SearchAttributeRuleGroup item)
        {
            this._searchAttributeRuleGroups.Add(item);
        }

        /// <summary>
        /// Removes all searchAttributeRuleGroup from Collection
        /// </summary>
        public void Clear()
        {
            this._searchAttributeRuleGroups.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeModelCollection contains a specific attribute search rule.
        /// </summary>
        /// <param name="item">The SearchAttributeRuleGroup object to locate in the SearchAttributeRuleGroupCollection.</param>
        /// <returns>
        /// <para>true : If item found in SearchAttributeRuleGroupCollection</para>
        /// <para>false : If item found not in SearchAttributeRuleGroupCollection</para>
        /// </returns>
        public bool Contains(SearchAttributeRuleGroup item)
        {
            return this._searchAttributeRuleGroups.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the SearchAttributeRuleGroupCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from SearchAttributeRuleGroupCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(SearchAttributeRuleGroup[] array, int arrayIndex)
        {
            this._searchAttributeRuleGroups.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of attributes in SearchAttributeRuleGroupCollection
        /// </summary>
        public int Count
        {
            get { return this._searchAttributeRuleGroups.Count; }
        }

        /// <summary>
        /// Check if SearchAttributeRuleGroupCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific attribute search rule from the SearchAttributeRuleGroupCollection.
        /// </summary>
        /// <param name="item">The attribute searchRule object to remove from the SearchAttributeRuleGroupCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(SearchAttributeRuleGroup item)
        {
            return this._searchAttributeRuleGroups.Remove(item);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current SearchAttributeRuleGroupCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchAttributeRuleGroupCollection
        /// </param>
        private void LoadSearchAttributeRuleGroups(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchAttributeRuleGroups")
                        {
                            String searchAttributeRuleGroupsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(searchAttributeRuleGroupsXml))
                            {
                                SearchAttributeRuleGroup searchAttributeRuleGroup = new SearchAttributeRuleGroup(searchAttributeRuleGroupsXml);

                                if (searchAttributeRuleGroup != null)
                                {
                                    this.Add(searchAttributeRuleGroup);
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}