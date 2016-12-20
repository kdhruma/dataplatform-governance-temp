using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    /// <summary>
    /// RelationshipTypeMaps
    /// </summary>
    [DataContract]
    public class RelationshipTypeMaps : ICollection<RelationshipTypeMap>, IEnumerable<RelationshipTypeMap>
    {
        #region Fields

        /// <summary>
        /// Field denoting the collection of RelationshipTypeMap
        /// </summary>
        [DataMember]
        private Collection<RelationshipTypeMap> _RelationshipTypeMaps = new Collection<RelationshipTypeMap>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipTypeMaps()
            : base()
        {
        }

        /// <summary>
        ///  Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">XML having xml values</param>
        public RelationshipTypeMaps(String valueAsXml)
        {
            LoadRelationshipTypeMaps(valueAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of RelationshipTypeMaps
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String relationshipTypeMapsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (RelationshipTypeMap relationshipTypeMap in this._RelationshipTypeMaps)
            {
                builder.Append(relationshipTypeMap.ToXml());
            }

            relationshipTypeMapsXml = String.Format("<RelationshipTypeMaps>{0}</RelationshipTypeMaps>", builder.ToString());

            return relationshipTypeMapsXml;
        }

        #endregion ToXml Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RelationshipTypeMaps)
            {
                RelationshipTypeMaps objectToBeCompared = obj as RelationshipTypeMaps;
                Int32 relationshipTypeMapsUnion = this._RelationshipTypeMaps.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 relationshipTypeMapsIntersect = this._RelationshipTypeMaps.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (relationshipTypeMapsUnion != relationshipTypeMapsIntersect)
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
            foreach (RelationshipTypeMap relTypeMap in this._RelationshipTypeMaps)
            {
                hashCode += relTypeMap.GetHashCode();
            }
            return hashCode;
        }

        #endregion Public Methods

        #region ICollection<RelationshipTypeMap> Members

        /// <summary>
        /// Add RelationshipTypeMap object in collection
        /// </summary>
        /// <param name="item">RelationshipTypeMap to add in collection</param>
        public void Add(RelationshipTypeMap item)
        {
            this._RelationshipTypeMaps.Add(item);
        }

        /// <summary>
        /// Removes all RelationshipTypeMaps from collection
        /// </summary>
        public void Clear()
        {
            this._RelationshipTypeMaps.Clear();
        }

        /// <summary>
        /// Determines whether the RelationshipTypeMaps contains a specific RelationshipTypeMap.
        /// </summary>
        /// <param name="item">The RelationshipTypeMap object to locate in the RelationshipTypeMaps.</param>
        /// <returns>
        /// <para>true : If RelationshipTypeMap found in RelationshipTypeMaps</para>
        /// <para>false : If RelationshipTypeMap found not in RelationshipTypeMaps</para>
        /// </returns>
        public bool Contains(RelationshipTypeMap item)
        {
            return this._RelationshipTypeMaps.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the RelationshipTypeMaps to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from RelationshipTypeMaps. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(RelationshipTypeMap[] array, int arrayIndex)
        {
            this._RelationshipTypeMaps.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of RelationshipTypeMap in RelationshipTypeMaps
        /// </summary>
        public int Count
        {
            get
            {
                return this._RelationshipTypeMaps.Count;
            }
        }

        /// <summary>
        /// Check if RelationshipTypeMaps is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the RelationshipTypeMaps.
        /// </summary>
        /// <param name="item">The RelationshipTypeMap object to remove from the RelationshipTypeMaps.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original RelationshipTypeMaps</returns>
        public bool Remove(RelationshipTypeMap item)
        {
            return this._RelationshipTypeMaps.Remove(item);
        }

        #endregion

        #region IEnumerable<RelationshipTypeMap> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeMaps.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<RelationshipTypeMap> GetEnumerator()
        {
            return this._RelationshipTypeMaps.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeMaps.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._RelationshipTypeMaps.GetEnumerator();
        }

        #endregion

        #region Private Methods

        private void LoadRelationshipTypeMaps(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeMap")
                        {
                            if (reader.HasAttributes)
                            {
                                String relationshipTypeMapXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(relationshipTypeMapXml))
                                {
                                    RelationshipTypeMap relationshipTypeMap = new RelationshipTypeMap(relationshipTypeMapXml);

                                    if (relationshipTypeMap != null)
                                    {
                                        this._RelationshipTypeMaps.Add(relationshipTypeMap);
                                    }
                                }
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
                        }
                        {
                            //Keep on reading the xml until we reach expected node.
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

        #endregion Private Methods

        #endregion
    }
}
