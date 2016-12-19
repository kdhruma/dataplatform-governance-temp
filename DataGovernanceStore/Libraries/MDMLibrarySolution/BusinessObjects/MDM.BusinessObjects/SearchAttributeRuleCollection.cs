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
    /// Specifies the specifications for AttributeRules for Search 
    /// </summary>
    public class SearchAttributeRuleCollection : ISearchAttributeRuleCollection, ICollection<SearchAttributeRule>, IEnumerable<SearchAttributeRule>
    {
        #region Fields

        /// <summary>
        /// Collection of SearchAttributeRule Object
        /// </summary>
        [DataMember]
        Collection<SearchAttributeRule> _searchAttributeRules = new Collection<SearchAttributeRule>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search Attribute Rule class.
        /// </summary>
        public SearchAttributeRuleCollection()
            : base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchAttributeRuleCollection object</param>
        public SearchAttributeRuleCollection(String valuesAsXml)
        {
            LoadSearchAttributeRules(valuesAsXml);
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="source">SearchAttributeRuleCollection to be copied</param>
        public SearchAttributeRuleCollection(SearchAttributeRuleCollection source)
            : base()
        {
            foreach (SearchAttributeRule searchAttributeRule in source)
            {
                this.Add((SearchAttributeRule)searchAttributeRule.Clone());
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region ISearchAttributeRuleCollection

        /// <summary>
        /// Get the XML presentation of an SearchAttributeRuleCollection
        /// </summary>
        /// <returns>Xml representation of Search Attribute Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when SearchAttributeRuleCollection object passed is null</exception>
        public String ToXml()
        {
            if (this._searchAttributeRules == null)
                throw new ArgumentNullException("SearchAttributeRuleCollection");

            String searchAttributeRuleXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("SearchAttributeRules");

            foreach (SearchAttributeRule searchAttributeRule in this._searchAttributeRules)
            {
                searchAttributeRuleXml = String.Concat(searchAttributeRuleXml, searchAttributeRule.ToXml());
            }
            xmlWriter.WriteRaw(searchAttributeRuleXml);
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            searchAttributeRuleXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchAttributeRuleXml;
        }

        /// <summary>
        /// Get All Attribute Ids
        /// </summary>
        /// <returns>Attribute Ids</returns>
        public Collection<Int32> GetAllAttributeIds()
        {
            Collection<Int32> attributeIds = null;

            if (this._searchAttributeRules != null)
            {
                attributeIds = new Collection<Int32>();

                foreach (SearchAttributeRule rule in this._searchAttributeRules)
                {
                    if (rule.Attribute != null && !attributeIds.Contains(rule.Attribute.Id))
                    {
                        attributeIds.Add(rule.Attribute.Id);
                    }
                }
            }

            return attributeIds;
        }

        /// <summary>
        /// Get search attribute rule by identifier
        /// </summary>
        /// <returns>Attribute Ids</returns>
        public SearchAttributeRule GetById(Int32 id)
        {
            if (this._searchAttributeRules != null)
            {
                foreach (SearchAttributeRule rule in this._searchAttributeRules)
                {
                    if (rule.Id == id)
                    {
                        return rule;
                    }
                }
            }

            return null;
        }

        #endregion

        #region IEnumerable<SearchAttributeRule>

        /// <summary>
        /// Returns an enumerator that iterates through a SearchAttributeRuleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<SearchAttributeRule> GetEnumerator()
        {
            return this._searchAttributeRules.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a SearchAttributeRuleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._searchAttributeRules.GetEnumerator();
        }

        #endregion

        #region ICollection<SearchAttributeRule>

        /// <summary>
        /// Add SearchAttributeRule Object in collection
        /// </summary>
        /// <param name="item">SearchAttributeRule to add in collection</param>
        public void Add(SearchAttributeRule item)
        {
            this._searchAttributeRules.Add(item);
        }

        /// <summary>
        /// Removes all searchAttributeRule from Collection
        /// </summary>
        public void Clear()
        {
            this._searchAttributeRules.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeModelCollection contains a specific attribute search rule.
        /// </summary>
        /// <param name="item">The SearchAttributeRule object to locate in the SearchAttributeRuleCollection.</param>
        /// <returns>
        /// <para>true : If item found in SearchAttributeRuleCollection</para>
        /// <para>false : If item found not in SearchAttributeRuleCollection</para>
        /// </returns>
        public bool Contains(SearchAttributeRule item)
        {
            return this._searchAttributeRules.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the SearchAttributeRuleCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from SearchAttributeRuleCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(SearchAttributeRule[] array, int arrayIndex)
        {
            this._searchAttributeRules.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of attributes in SearchAttributeRuleCollection
        /// </summary>
        public int Count
        {
            get { return this._searchAttributeRules.Count; }
        }

        /// <summary>
        /// Check if SearchAttributeRuleCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific attribute search rule from the SearchAttributeRuleCollection.
        /// </summary>
        /// <param name="item">The attribute searchRule object to remove from the SearchAttributeRuleCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(SearchAttributeRule item)
        {
            return this._searchAttributeRules.Remove(item);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current SearchAttributeRuleCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchAttributeRuleCollection
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        ///    <SearchAttributeCollection>
        ///               <Attribute AttributeId="24" Value="aa" Operator="like"/>
        ///               <SystemAttribute WhereClause="##84##='s*'" />
        ///    </SearchAttributeCollection>
        /// ]]>
        /// </para>
        /// </param>
        private void LoadSearchAttributeRules(String valueAsXml)
        {
            #region Sample XML

            //<SearchAttributeCollection>
            //           <Attribute AttributeId="24" Value="aa" Operator="like"/>
            //           <SystemAttribute WhereClause="##84##='s*'" />
            //</SearchAttributeCollection>

            #endregion

            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && (reader.Name == "Attribute" || reader.Name == "SystemAttribute"))
                        {
                            String searchAttributeRulesXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(searchAttributeRulesXml))
                            {
                                SearchAttributeRule searchAttributeRule = new SearchAttributeRule(searchAttributeRulesXml);

                                if (searchAttributeRule != null && searchAttributeRule.Attribute != null && searchAttributeRule.Attribute.Id > 0)
                                {
                                    this.Add(searchAttributeRule);
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