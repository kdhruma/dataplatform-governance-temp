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
    /// Specifies the DBColumn Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class DBColumnCollection : ICollection<DBColumn>, IEnumerable<DBColumn>,IDBColumnCollection
    {
        #region Fields

        [DataMember]
        private Collection<DBColumn> _DBColumns = new Collection<DBColumn>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DBColumnCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DBColumnCollection(String valueAsXml)
        {
            LoadDBColumnCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DBColumnCollection from IList
        /// </summary>
        /// <param name="DBColumnList">IList of DBColumns</param>
        public DBColumnCollection(IList<DBColumn> DBColumnList)
        {
            this._DBColumns = new Collection<DBColumn>(DBColumnList);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is DBColumnCollection)
            {
                DBColumnCollection objectToBeCompared = obj as DBColumnCollection;
                Int32 DBColumnsUnion = this._DBColumns.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 DBColumnsIntersect = this._DBColumns.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (DBColumnsUnion != DBColumnsIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (DBColumn DBColumn in this._DBColumns)
            {
                hashCode += DBColumn.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DBColumnCollection based on column Name.
        /// </summary>
        /// <param name="columnNames">The collection of column names to remove from the DBColumnCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DBColumnCollection</returns>
        public Boolean Remove(Collection<String> columnNames)
        {
            Boolean result = false;

            DBColumnCollection dbColumns = Get(columnNames);

            if (dbColumns != null && dbColumns.Count > 0)
            {
                result = true;

                foreach (DBColumn dbColumn in dbColumns)
                {
                    result = result && this._DBColumns.Remove(dbColumn);
                }
            }

            return result;
        }

        /// <summary>
        /// Clone DBColumnsCollection object
        /// </summary>
        /// <returns>Cloned copy of IDBColumnCollection object.</returns>
        public IDBColumnCollection Clone()
        {
            DBColumnCollection clonedDBColumns = new DBColumnCollection();

            if (this._DBColumns != null && this._DBColumns.Count > 0)
            {
                foreach (DBColumn dbColumn in this._DBColumns)
                {
                    clonedDBColumns.Add((DBColumn)dbColumn.Clone());
                }
            }

            return clonedDBColumns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public DBColumn Get(String columnName)
        {
            return Get(new Collection<String>() { columnName }).SingleOrDefault();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadDBColumnCollection(String valuesAsXml)
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Column")
                        {
                            String DBColumnXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(DBColumnXml))
                            {
                                DBColumn DBColumn = new DBColumn(DBColumnXml);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        private DBColumnCollection Get(Collection<String> columnNames)
        {
            DBColumnCollection dbColumns = new DBColumnCollection();
            Int32 counter = 0;

            if (this._DBColumns != null && this._DBColumns.Count > 0 && columnNames != null && columnNames.Count > 0)
            {
                foreach (DBColumn dbColumn in this._DBColumns)
                {
                    if (columnNames.Contains(dbColumn.Name))
                    {
                        dbColumns.Add(dbColumn);
                        counter++;
                    }

                    if (columnNames.Count.Equals(counter))
                        break;
                }
            }

            return dbColumns;
        }

        #endregion

        #endregion

        #region ICollection<DBColumn> Members

        /// <summary>
        /// Add DBColumn object in collection
        /// </summary>
        /// <param name="item">DBColumn to add in collection</param>
        public void Add(DBColumn item)
        {
            this._DBColumns.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbColumns"></param>
        public void AddRange(DBColumnCollection dbColumns)
        {
            if (dbColumns != null && dbColumns.Count > 0)
            {
                foreach (DBColumn dbColumn in dbColumns)
                {
                    this._DBColumns.Add(dbColumn);
                }
            }
        }

        /// <summary>
        /// Removes all DBColumns from collection
        /// </summary>
        public void Clear()
        {
            this._DBColumns.Clear();
        }

        /// <summary>
        /// Determines whether the DBColumnCollection contains a specific DBColumn.
        /// </summary>
        /// <param name="item">The DBColumn object to locate in the DBColumnCollection.</param>
        /// <returns>
        /// <para>true : If DBColumn found in mappingCollection</para>
        /// <para>false : If DBColumn found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DBColumn item)
        {
            return this._DBColumns.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DBColumnCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DBColumnCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DBColumn[] array, int arrayIndex)
        {
            this._DBColumns.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DBColumns in DBColumnCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._DBColumns.Count;
            }
        }

        /// <summary>
        /// Check if DBColumnCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DBColumnCollection.
        /// </summary>
        /// <param name="item">The DBColumn object to remove from the DBColumnCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DBColumnCollection</returns>
        public Boolean Remove(DBColumn item)
        {
            return this._DBColumns.Remove(item);
        }

        #endregion

        #region IEnumerable<DBColumn> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DBColumn> GetEnumerator()
        {
            return this._DBColumns.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._DBColumns.GetEnumerator();
        }

        #endregion

        #region IDBColumnCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DBColumnCollection object
        /// </summary>
        /// <returns>Xml string representing the DBColumnCollection</returns>
        public String ToXml()
        {
            String DBColumnsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DBColumn DBColumn in this._DBColumns)
            {
                builder.Append(DBColumn.ToXml());
            }

            DBColumnsXml = String.Format("<Columns>{0}</Columns>", builder.ToString());
            return DBColumnsXml;
        }

        #endregion ToXml methods

        #endregion IDBColumnCollection Memebers
    }
}
