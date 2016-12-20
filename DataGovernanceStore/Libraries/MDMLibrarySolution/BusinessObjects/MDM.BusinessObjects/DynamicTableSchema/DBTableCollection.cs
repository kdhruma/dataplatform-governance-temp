using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DynamicTableSchema
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the DBTable Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class DBTableCollection : ICollection<DBTable>, IEnumerable<DBTable>, IDBTableCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<DBTable> _DBTables = new Collection<DBTable>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DBTableCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DBTableCollection(String valueAsXml)
        {
            LoadDBTableCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DBTableCollection from IList
        /// </summary>
        /// <param name="DBTableList">IList of DBTables</param>
        public DBTableCollection(IList<DBTable> DBTableList)
        {
            this._DBTables = new Collection<DBTable>(DBTableList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.LookupModel;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DBTableCollection)
            {
                DBTableCollection objectToBeCompared = obj as DBTableCollection;
                Int32 DBTableUnion = this._DBTables.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 DBTableIntersect = this._DBTables.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (DBTableUnion != DBTableIntersect)
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
            foreach (DBTable DBTable in this._DBTables)
            {
                hashCode += DBTable.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Gets the DBTable for given TableName.
        /// </summary>
        /// <param name="tableName">TableName to be searched in the collection</param>
        /// <returns>DBTable for given TableName</returns>
        public DBTable Get(String tableName)
        {
            tableName = tableName.ToLowerInvariant();

            Int32 tableCount = _DBTables.Count;
            DBTable dbTable = null;

            for (Int32 index = 0; index < tableCount; index++)
            {
                dbTable = _DBTables[index];

                if (dbTable.Name.ToLowerInvariant() == tableName)
                    return dbTable;
            }

            return null;
        }

        #endregion

        #region ICollection<DBTable> Members

        /// <summary>
        /// Add DBTable object in collection
        /// </summary>
        /// <param name="item">DBTable to add in collection</param>
        public void Add(DBTable item)
        {
            this._DBTables.Add(item);
        }

        /// <summary>
        /// Removes all DBTables from collection
        /// </summary>
        public void Clear()
        {
            this._DBTables.Clear();
        }

        /// <summary>
        /// Determines whether the DBTableCollection contains a specific DBTable.
        /// </summary>
        /// <param name="item">The DBTable object to locate in the DBTableCollection.</param>
        /// <returns>
        /// <para>true : If DBTable found in mappingCollection</para>
        /// <para>false : If DBTable found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DBTable item)
        {
            return this._DBTables.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DBTableCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DBTableCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DBTable[] array, int arrayIndex)
        {
            this._DBTables.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DBTables in DBTableCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._DBTables.Count;
            }
        }

        /// <summary>
        /// Check if DBTableCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DBTableCollection.
        /// </summary>
        /// <param name="item">The DBTable object to remove from the DBTableCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DBTableCollection</returns>
        public bool Remove(DBTable item)
        {
            return this._DBTables.Remove(item);
        }

        #endregion

        #region IEnumerable<DBTable> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBTableCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DBTable> GetEnumerator()
        {
            return this._DBTables.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBTableCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._DBTables.GetEnumerator();
        }

        #endregion

        #region IDBTableCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DBTableCollection object
        /// </summary>
        /// <returns>Xml string representing the DBTableCollection</returns>
        public String ToXml()
        {
            String DBTablesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DBTable DBTable in this._DBTables)
            {
                builder.Append(DBTable.ToXml());
            }

            DBTablesXml = String.Format("<Tables>{0}</Tables>", builder.ToString());
            return DBTablesXml;
        }

        #endregion ToXml methods

        #endregion IDBTableCollection Memebers

        #region IDBTableCollection

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of a dbTable model which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            DBTableCollection dbTables = Get(referenceIds);

            if (dbTables != null && dbTables.Count > 0)
            {
                foreach (DBTable dbTable in dbTables)
                {
                    result = result && this.Remove(dbTable);
                }
            }

            return result;
        }

        #endregion

        #region IDataModelObjectCollection Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> spliteDBTables = null;

            if (this._DBTables != null)
            {
                spliteDBTables = Utility.Split(this, batchSize);
            }

            return spliteDBTables;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as DBTable);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadDBTableCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <Tables></Tables>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                        {
                            String DBTableXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(DBTableXml))
                            {
                                DBTable DBTable = new DBTable(DBTableXml);
                                this.Add(DBTable);
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
        ///  Gets the DBTable collection using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of a db table which is to be fetched.</param>
        /// <returns>Returns filtered db Table collection</returns>
        private DBTableCollection Get(Collection<String> referenceIds)
        {
            DBTableCollection dbTables = new DBTableCollection();
            Int32 counter = 0;

            if (this._DBTables != null && this._DBTables.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (DBTable dbTable in this._DBTables)
                {
                    if (referenceIds.Contains(dbTable.ReferenceId))
                    {
                        dbTables.Add(dbTable);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return dbTables;
        }

        #endregion

        #endregion
    }
}