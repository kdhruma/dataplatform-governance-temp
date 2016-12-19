using System;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the AttributeMap Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class AttributeMapCollection : ICollection<AttributeMap>, IEnumerable<AttributeMap>
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<AttributeMap> _attributeMaps = new Collection<AttributeMap>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AttributeMapCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public AttributeMapCollection(String valueAsXml)
        {
            LoadAttributeMapCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize AttributeMapCollection from IList
        /// </summary>
        /// <param name="attributeMapsList">IList of attributeMaps</param>
        public AttributeMapCollection(IList<AttributeMap> attributeMapsList)
        {
            this._attributeMaps = new Collection<AttributeMap>(attributeMapsList);
        }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is AttributeMapCollection)
            {
                AttributeMapCollection objectToBeCompared = obj as AttributeMapCollection;
                Int32 attributeMapsUnion = this._attributeMaps.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributeMapsIntersect = this._attributeMaps.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (attributeMapsUnion != attributeMapsIntersect)
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
            foreach (AttributeMap attr in this._attributeMaps)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        ///<summary>
        /// Load AttributeMapCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadAttributeMapCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <AttributeMaps></AttributeMaps>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeMap")
                        {
                            String attributeMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeMapXml))
                            {
                                AttributeMap attributeMap = new AttributeMap(attributeMapXml);
                                this.Add(attributeMap);
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

        #region Private Methods


        #endregion

        #region ICollection<AttributeMap> Members

        /// <summary>
        /// Add attributeMap object in collection
        /// </summary>
        /// <param name="item">attributeMap to add in collection</param>
        public void Add(AttributeMap item)
        {
            this._attributeMaps.Add(item);
        }

        /// <summary>
        /// Removes all attributeMaps from collection
        /// </summary>
        public void Clear()
        {
            this._attributeMaps.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeMapCollection contains a specific attributeMap.
        /// </summary>
        /// <param name="item">The attributeMap object to locate in the AttributeMapCollection.</param>
        /// <returns>
        /// <para>true : If attributeMap found in attributeMapCollection</para>
        /// <para>false : If attributeMap found not in attributeMapCollection</para>
        /// </returns>
        public bool Contains(AttributeMap item)
        {
            return this._attributeMaps.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the AttributeMapCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from AttributeMapCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(AttributeMap[] array, int arrayIndex)
        {
            this._attributeMaps.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of attributeMaps in AttributeMapCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributeMaps.Count;
            }
        }

        /// <summary>
        /// Check if AttributeMapCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the AttributeMapCollection.
        /// </summary>
        /// <param name="item">The attributeMap object to remove from the AttributeMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original AttributeMapCollection</returns>
        public bool Remove(AttributeMap item)
        {
            return this._attributeMaps.Remove(item);
        }

        #endregion

        #region IEnumerable<AttributeMap> Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeMap> GetEnumerator()
        {
            return this._attributeMaps.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeMaps.GetEnumerator();
        }

        #endregion

        #region IAttributeMapCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of AttributeMapCollection object
        /// </summary>
        /// <returns>Xml string representing the AttributeMapCollection</returns>
        public String ToXml()
        {
            String attributeMapsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( AttributeMap attributeMap in this._attributeMaps )
            {
                builder.Append(attributeMap.ToXml());
            }

            attributeMapsXml = String.Format("<AttributeMaps>{0}</AttributeMaps>", builder.ToString());
            return attributeMapsXml;
        }

        /// <summary>
        /// Get Xml representation of AttributeMapCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String attributeMapsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (AttributeMap attributeMap in this._attributeMaps)
            {
                builder.Append(attributeMap.ToXml(serialization));
            }

            attributeMapsXml = String.Format("<AttributeMaps>{0}</AttributeMaps>", builder.ToString());
            return attributeMapsXml;
        }

        #endregion ToXml methods

        #region AttributeMap Get

        #endregion AttributeMap Get
       

        #endregion IAttributeMapCollection Memebers
    }
}
