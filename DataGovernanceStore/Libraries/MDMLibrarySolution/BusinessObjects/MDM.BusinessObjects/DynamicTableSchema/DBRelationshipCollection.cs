using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DynamicTableSchema
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the DBRelationship Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class DBRelationshipCollection : ICollection<DBRelationship>, IEnumerable<DBRelationship>
    {
        #region Fields

        [DataMember]
        private Collection<DBRelationship> _DBRelationships = new Collection<DBRelationship>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DBRelationshipCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DBRelationshipCollection(String valueAsXml)
        {
            LoadDBRelationshipCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DBColumnCollection from IList
        /// </summary>
        /// <param name="DBColumnList">IList of DBColumns</param>
        public DBRelationshipCollection(IList<DBRelationship> DBColumnList)
        {
            this._DBRelationships = new Collection<DBRelationship>(DBColumnList);
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
            foreach (DBRelationship DBRelationship in this._DBRelationships)
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
                                DBRelationship DBColumn = new DBRelationship(DBRelationshipXml);
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

        #region ICollection<DBRelationship> Members

        /// <summary>
        /// Add DBRelationship object in collection
        /// </summary>
        /// <param name="item">DBRelationship to add in collection</param>
        public void Add(DBRelationship item)
        {
            this._DBRelationships.Add(item);
        }

        /// <summary>
        /// Removes all DBRelationships from collection
        /// </summary>
        public void Clear()
        {
            this._DBRelationships.Clear();
        }

        /// <summary>
        /// Determines whether the DBRelationshipCollection contains a specific DBColumn.
        /// </summary>
        /// <param name="item">The DBRelationship object to locate in the DBColumnCollection.</param>
        /// <returns>
        /// <para>true : If DBRelationship found in mappingCollection</para>
        /// <para>false : If DBRelationship found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DBRelationship item)
        {
            return this._DBRelationships.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DBRelationshipCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DBRelationshipCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DBRelationship[] array, int arrayIndex)
        {
            this._DBRelationships.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DBRelationship in DBRelationshipCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._DBRelationships.Count;
            }
        }

        /// <summary>
        /// Check if DBRelationshipCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DBRelationshipCollection.
        /// </summary>
        /// <param name="item">The DBColumn object to remove from the DBRelationshipCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DBRelationshipCollection</returns>
        public bool Remove(DBRelationship item)
        {
            return this._DBRelationships.Remove(item);
        }

        #endregion

        #region IEnumerable<DBRelationship> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DBRelationship> GetEnumerator()
        {
            return this._DBRelationships.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._DBRelationships.GetEnumerator();
        }

        #endregion

        #region DBRelationshipCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DBRelationshipCollection object
        /// </summary>
        /// <returns>Xml string representing the DBRelationshipCollection</returns>
        public String ToXml()
        {
            String DBRelationshipsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DBRelationship DBRelationship in this._DBRelationships)
            {
                builder.Append(DBRelationship.ToXml());
            }

            DBRelationshipsXml = String.Format("<Relationships>{0}</Relationships>", builder.ToString());
            return DBRelationshipsXml;
        }

        #endregion ToXml methods

        #endregion DBRelationshipCollection Memebers
    }
}
