using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies the Collection of lookup search rule
    /// </summary>
    [DataContract]
    public class LookupSearchRuleCollection : ICollection<LookupSearchRule>, IEnumerable<LookupSearchRule>, ILookupSearchRuleCollection
    {
        #region Fields

        [DataMember]
        private Collection<LookupSearchRule> _lookupSearchRules = new Collection<LookupSearchRule>();

        /// <summary>
        /// Group Operator for more than one rule combination
        /// </summary>
        private String _groupOperator = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if engine is started or not
        /// </summary>
        [DataMember]
        public String GroupOperator
        {
            get { return _groupOperator; }
            set { _groupOperator = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LookupSearchRuleCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public LookupSearchRuleCollection(String valueAsXml)
        {
            LoadLookupSearchRuleCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize LookupSearchRuleCollection from IList
        /// </summary>
        /// <param name="lookupSearchRuleList">IList of denorm result</param>
        public LookupSearchRuleCollection(IList<LookupSearchRule> lookupSearchRuleList)
        {
            this._lookupSearchRules = new Collection<LookupSearchRule>(lookupSearchRuleList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is LookupSearchRuleCollection)
            {
                LookupSearchRuleCollection objectToBeCompared = obj as LookupSearchRuleCollection;
                Int32 lookupSearchRuleUnion = this._lookupSearchRules.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 lookupSearchRuleIntersect = this._lookupSearchRules.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (lookupSearchRuleUnion != lookupSearchRuleIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (LookupSearchRule attr in this._lookupSearchRules)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Load lookupSearhRuleCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">xml having xml values for an object</param>
        public void LoadLookupSearchRuleCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
                <LookupSearchRuleCollection></LookupSearchRuleCollection>
            */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "LookupSearchRuleCollection")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("GroupOperator"))
                                {
                                    this.GroupOperator = reader.ReadContentAsString();
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "LookupSearchRule")
                        {
                            String lookupSearchRuleXML = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(lookupSearchRuleXML))
                            {
                                LookupSearchRule lookupSearchRule = new LookupSearchRule(lookupSearchRuleXML);
                                this.Add(lookupSearchRule);
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

        #region ICollection<LookupSearchRule> Members

        /// <summary>
        /// Add LookupSearchRule object in collection
        /// </summary>
        /// <param name="item">LookupSearchRule to add in collection</param>
        public void Add(ILookupSearchRule item)
        {
            this._lookupSearchRules.Add((LookupSearchRule)item);
        }

        /// <summary>
        /// Add LookupSearchRule object in collection
        /// </summary>
        /// <param name="item">LookupSearchRule to add in collection</param>
        public void Add(LookupSearchRule item)
        {
            this._lookupSearchRules.Add(item);
        }

        /// <summary>
        /// Removes all LookupSearchRule from collection
        /// </summary>
        public void Clear()
        {
            this._lookupSearchRules.Clear();
        }

        /// <summary>
        /// Determines whether the LookupSearchRuleCollection contains a specific LookupSearchRule.
        /// </summary>
        /// <param name="item">The LookupSearchRule object to locate in the LookupSearchRuleCollection.</param>
        /// <returns>
        /// <para>true : If LookupSearchRule found in mappingCollection</para>
        /// <para>false : If LookupSearchRule found not in mappingCollection</para>
        /// </returns>
        public bool Contains(LookupSearchRule item)
        {
            return this._lookupSearchRules.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the LookupSearchRuleCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from LookupSearchRuleCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(LookupSearchRule[] array, int arrayIndex)
        {
            this._lookupSearchRules.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of LookupSearchRule in LookupSearchRuleCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._lookupSearchRules.Count;
            }
        }

        /// <summary>
        /// Check if LookupSearchRuleCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the LookupSearchRuleCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the LookupSearchRuleCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original LookupSearchRuleCollection</returns>
        public bool Remove(LookupSearchRule item)
        {
            return this._lookupSearchRules.Remove(item);
        }

        #endregion

        #region IEnumerable<LookupSearchRule> Members

        /// <summary>
        /// Returns an enumerator that iterates through a LookupSearchRuleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<LookupSearchRule> GetEnumerator()
        {
            return this._lookupSearchRules.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a LookupSearchRuleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._lookupSearchRules.GetEnumerator();
        }

        #endregion

        #region XML Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of LookupSearchRuleCollection object
        /// </summary>
        /// <returns>Xml string representing the LookupSearchRuleCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Lookup Search Rule node start
            xmlWriter.WriteStartElement("LookupSearchRuleCollection");

            #region Write Properties

            xmlWriter.WriteAttributeString("GroupOperator", this.GroupOperator.ToLowerInvariant());
            StringBuilder stringBuilder = new StringBuilder();

            foreach (LookupSearchRule lookupSearchRule in this._lookupSearchRules)
            {
                stringBuilder.Append(lookupSearchRule.ToXml());
            }

            xmlWriter.WriteRaw(stringBuilder.ToString());

            #endregion

            //node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion ToXml methods

        #endregion

    }
}