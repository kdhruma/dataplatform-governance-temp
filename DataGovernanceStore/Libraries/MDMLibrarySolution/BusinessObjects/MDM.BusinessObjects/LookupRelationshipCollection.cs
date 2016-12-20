using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;


    /// <summary>
    /// Represents collection of lookup relationships
    /// </summary>
    [DataContract]
    public class LookupRelationshipCollection : ICollection<LookupRelationship>, IEnumerable<LookupRelationship>
    {
         #region Fields

        [DataMember]
        private Collection<LookupRelationship> _LookupRelationships = new Collection<LookupRelationship>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LookupRelationshipCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public LookupRelationshipCollection(String valueAsXml)
        {
            LoadDBRelationshipCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize LookupRelationshipCollection from IList
        /// </summary>
        /// <param name="LookupRelationshipList">Indicates list of lookup relationship</param>
        public LookupRelationshipCollection(IList<LookupRelationship> LookupRelationshipList)
        {
            this._LookupRelationships = new Collection<LookupRelationship>(LookupRelationshipList);
        }

        #endregion

        #region Properties

        #endregion

        #region Public Methods



        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (LookupRelationship DBRelationship in this._LookupRelationships)
            {
                hashCode += DBRelationship.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadDBRelationshipCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <Columns></Columns>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                        {
                            String DBRelationshipXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(DBRelationshipXml))
                            {
                                LookupRelationship DBColumn = new LookupRelationship(DBRelationshipXml);
                                this.Add(DBColumn);
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

        #region ICollection<LookupRelationship> Members

        /// <summary>
        /// Add LookupRelationship object in collection
        /// </summary>
        /// <param name="item">LookupRelationship to add in collection</param>
        public void Add(LookupRelationship item)
        {
            this._LookupRelationships.Add(item);
        }

        /// <summary>
        /// Removes all LookupRelationship from collection
        /// </summary>
        public void Clear()
        {
            this._LookupRelationships.Clear();
        }

        /// <summary>
        /// Determines whether the LookupRelationshipCollection contains a specific DBColumn.
        /// </summary>
        /// <param name="item">The LookupRelationship object to locate in the DBColumnCollection.</param>
        /// <returns>
        /// <para>true : If LookupRelationship found in mappingCollection</para>
        /// <para>false : If LookupRelationship found not in mappingCollection</para>
        /// </returns>
        public bool Contains(LookupRelationship item)
        {
            return this._LookupRelationships.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the LookupRelationshipCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from LookupRelationshipCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(LookupRelationship[] array, int arrayIndex)
        {
            this._LookupRelationships.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of LookupRelationship in LookupRelationshipCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._LookupRelationships.Count;
            }
        }

        /// <summary>
        /// Check if LookupRelationshipCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the LookupRelationshipCollection.
        /// </summary>
        /// <param name="item">The DBColumn object to remove from the LookupRelationshipCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DBColumnCollection</returns>
        public bool Remove(LookupRelationship item)
        {
            return this._LookupRelationships.Remove(item);
        }

        #endregion

        #region IEnumerable<LookupRelationship> Members

        /// <summary>
        /// Returns an enumerator that iterates through a LookupRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<LookupRelationship> GetEnumerator()
        {
            return this._LookupRelationships.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a LookupRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._LookupRelationships.GetEnumerator();
        }

        #endregion

        #region IDBColumnCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of LookupRelationshipCollection object
        /// </summary>
        /// <returns>Xml string representing the LookupRelationshipCollection</returns>
        public String ToXml()
        {
            String lookupRelationshipXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (LookupRelationship lookupRelationship in this._LookupRelationships)
            {
                builder.Append(lookupRelationship.ToXml());
            }

            lookupRelationshipXml = String.Format("<Relationships>{0}</Relationships>", builder.ToString());
            return lookupRelationshipXml;
        }

        #endregion ToXml methods

        #endregion IDBColumnCollection Memebers
    }
}
