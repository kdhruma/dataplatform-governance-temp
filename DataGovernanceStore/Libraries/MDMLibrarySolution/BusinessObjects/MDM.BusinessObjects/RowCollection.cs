using System;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;


namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Represents collection of Rows
    /// </summary>
    [DataContract]
    public class RowCollection : ICollection<Row>, IEnumerable<Row>, IRowCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Dictionary<Int64, Row> _rows = new Dictionary<Int64, Row>();

        [DataMember]
        private Int32 _rowIndex = 0;

        [DataMember]
        private Dictionary<String, Dictionary<String, Collection<Int64>>> _indexTables = new Dictionary<string, Dictionary<string, Collection<Int64>>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public RowCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public RowCollection( String valueAsXml )
        {
            LoadRowCollection(valueAsXml);
        }

        /// <summary>
        /// Constructor which takes a List of type Row
        /// </summary>
        /// <param name="rows"></param>
        public RowCollection(IList<Row> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                this.Add(rows[i]);
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Indexers

        /// <summary>
        /// Find row from RowCollection based on rowId
        /// </summary>
        /// <param name="rowId">Id of row</param>
        /// <returns>Row object at given index</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.-or-index is equal to or greater than RowCollection.Count.</exception>
        public Row this[Int64 rowId]
        {
            get
            {
                Row row = GetRow(rowId);

                if (row == null)
                    throw new ArgumentException(String.Format("No row found for row id: {0}", rowId), "rowId");
                else
                    return row;
            }
        }

        #endregion Indexers

        #region Public Methods

        /// <summary>
        /// Remove row object from RowCollection
        /// </summary>
        /// <param name="rowId">Id of row which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int64 rowId)
        {
            Row row = GetRow(rowId);

            if (row == null)
                throw new ArgumentException("No row found for given row id");
            else
            {
                RemoveIndex(row.Id);
                return this.Remove(row);
            }
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals( object obj )
        {
            if ( obj is RowCollection )
            {
                RowCollection objectToBeCompared = obj as RowCollection;
                Int32 rowsUnion = this._rows.Values.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 rowsIntersect = this._rows.Values.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if ( rowsUnion != rowsIntersect )
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

            // We can't use for loop on keys as keys may not be sequential
            foreach ( Row row in this._rows.Values )
            {
                hashCode += row.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Get row having given RowId
        /// </summary>
        /// <param name="rowId">Id of row to search on rows collection</param>
        /// <returns>Row having given Id</returns>
        public Row GetRow(Int64 rowId)
        {
            if (this._rows.ContainsKey(rowId))
            {
                return this._rows[rowId];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RowCollection Clone()
        {
            RowCollection clonedRowCollection = new RowCollection();

            foreach (var indexTableKey in this._indexTables.Keys)
            {
                clonedRowCollection.CreateIndexTable(indexTableKey);
            }

            foreach (var row in this._rows)
            {
                Row clonedRow = row.Value.Clone();
                clonedRowCollection.Add(clonedRow);
            }

            return clonedRowCollection;
        }

        /// <summary>
        /// Initializes index structure for given key. 
        /// </summary>
        /// <param name="indexName">Column to add index of. Note that this is not validated at this point.</param>
        /// <returns>True - Column name was added to index table list.
        ///          False - Column name was added already.</returns>
        public Boolean CreateIndexTable(String indexName)
        {
            if (_indexTables.ContainsKey(indexName))
            {
                return false;
            }

            _indexTables.Add(indexName, new Dictionary<string, Collection<Int64>>());

            return true;
        }

        /// <summary>
        /// CreateIndexes - For each index table, this method will clear and add index for each row into that table.
        /// </summary>
        public void CreateIndexes()
        {
            foreach (var indexTableKey in _indexTables.Keys)
            {
                var indexTable = _indexTables[indexTableKey];
                
                indexTable.Clear();

                foreach (var row in this._rows.Values)
                {
                    if (row[indexTableKey] != null)
                    {
                        this.AddRowToIndexTable(indexTableKey, row[indexTableKey].ToString(), row.Id);
                    }
                }
            }
        }

        /// <summary>
        /// GetRowsByIndex
        /// </summary>
        /// <param name="indexTable"></param>
        /// <param name="indexKey"></param>
        /// <returns></returns>
        internal IRowCollection GetRowsByIndex(string indexTable, string indexKey)
        {
            RowCollection rows = new RowCollection();

            indexKey = indexKey.ToLowerInvariant();

            if (this._indexTables.ContainsKey(indexTable))
            {
                if (this._indexTables[indexTable].ContainsKey(indexKey))
                {
                    foreach (long lookupId in this._indexTables[indexTable][indexKey])
                    {
                        rows.Add(this._rows[lookupId]);
                    }
                }
            }

            return rows;
        }

        /// <summary>
        /// Checks if index table is available given index name.
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns>True if index table has been created for given index.</returns>
        internal Boolean IsIndexTableAvailable(String indexName)
        {
            if (_indexTables.Keys.Contains(indexName, StringComparer.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetRowCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparission is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(RowCollection subsetRowCollection, Boolean compareIds = false)
        {
            if (subsetRowCollection != null && subsetRowCollection.Count > 0)
            {
                foreach (Row childRow in subsetRowCollection)
                {
                    if (this._rows != null && this._rows.Count > 0)
                    {
                        foreach (Row sourceRow in this._rows.Values)
                        {
                            if (sourceRow.IsSuperSetOf(childRow, compareIds))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add index based on key for given value to lookup id.
        /// </summary>
        /// <param name="table">Index table name - ExportFormat, DisplayFormat</param>
        /// <param name="indexKey">Value of the column/key in gien row- "Bright Red", "Target Inc" </param>
        /// <param name="lookupId">Id of the given row - 1, 2..n</param>
        private void AddRowToIndexTable(String table, String indexKey, Int64 lookupId)
        {
            if (_indexTables.ContainsKey(table))
            {
                Dictionary<String, Collection<Int64>> indexTable = _indexTables[table];

                if (indexTable == null)
                {
                    throw new ArgumentNullException("indexKey", (String.Format("There is no index-table for indexKey {0} in table {1}", indexKey, table)));
                }

                indexKey = indexKey.ToLowerInvariant();

                Collection<long> idCollection;

                indexTable.TryGetValue(indexKey, out idCollection);

                if (idCollection != null)
                {
                    // Add lookupId to collection
                    if (!idCollection.Contains(lookupId))
                    {
                        idCollection.Add(lookupId);
                    }
                }
                else
                {
                    indexTable.Add(indexKey, new Collection<Int64>() { lookupId });
                }
            }
            else
            {
                throw new ArgumentException(String.Format("There is no index defined for key {0}", table));
            }
        }


        /// <summary>
        /// Removes the lookupId from given keyValkue index of key.
        /// </summary>
        /// <param name="lookupId"></param>
        private void RemoveIndex(Int64 lookupId)
        {
            if (lookupId < 0)
            {
                // Negative key indexs are (where caller is not providng id value, is not lookup caller who would need index support.
                // Hence there is nothing to remove from indexes

                return;
            }


            // Iterate over each index table
            foreach (var indexTableKey in _indexTables.Keys)
            {
                var indexTable = _indexTables[indexTableKey];

                // Iterate over each id collection and see if current id exists.
                // Remove id if exists and break to proceed to next index table
                foreach (var lookupIdCollection in indexTable.Values)
                {
                    if (lookupIdCollection.Contains(lookupId))
                    {
                        lookupIdCollection.Remove(lookupId);
                        break;
                    }
                }
            }
        }

        
        /// <summary>
        /// Load rows from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having value of row
        /// <para>
        /// Sample Xml:
        /// <Rows>
        ///     <Row Id="-1" >
        ///         <Cells>
        ///             <Cell ColumnId="1" ColumnName="Column1"><![CDATA[abc]]></Cell>
        ///             <Cell ColumnId="2" ColumnName="Column2"><![CDATA[pqr]]></Cell>
        ///             <Cell ColumnId="3" ColumnName="Column3"><![CDATA[xyz]]></Cell>
        ///             <Cell ColumnId="4" ColumnName="Column4"><![CDATA[mno]]></Cell>
        ///         </Cells>
        ///     </Row>
        ///     <Row Id="-1" >
        ///         <Cells>
        ///             <Cell ColumnId="1" ColumnName="Column1"><![CDATA[abc1]]></Cell>
        ///             <Cell ColumnId="2" ColumnName="Column2"><![CDATA[pqr1]]></Cell>
        ///             <Cell ColumnId="3" ColumnName="Column3"><![CDATA[ira]]></Cell>
        ///             <Cell ColumnId="4" ColumnName="Column4"><![CDATA[mno1]]></Cell>
        ///         </Cells>
        ///     </Row>
        /// </Rows>
        /// </para>
        /// </param>
        private void LoadRowCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             * <Rows>
                <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
                  <Cells>
                    <Cell ColumnId="1" ColumnName="Column1"><![CDATA[abc]]></Cell>
                    <Cell ColumnId="2" ColumnName="Column2"><![CDATA[pqr]]></Cell>
                    <Cell ColumnId="3" ColumnName="Column3"><![CDATA[xyz]]></Cell>
                    <Cell ColumnId="4" ColumnName="Column4"><![CDATA[mno]]></Cell>
                  </Cells>
                </Row>
                <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
                  <Cells>
                    <Cell ColumnId="1" ColumnName="Column1"><![CDATA[abc1]]></Cell>
                    <Cell ColumnId="2" ColumnName="Column2"><![CDATA[pqr1]]></Cell>
                    <Cell ColumnId="3" ColumnName="Column3"><![CDATA[ira]]></Cell>
                    <Cell ColumnId="4" ColumnName="Column4"><![CDATA[mno1]]></Cell>
                  </Cells>
                </Row>
              </Rows>
             */
            #endregion Sample Xml

            if ( !String.IsNullOrWhiteSpace(valuesAsXml) )
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while ( !reader.EOF )
                    {
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Row" )
                        {
                            String rowXml = reader.ReadOuterXml();
                            if ( !String.IsNullOrEmpty(rowXml) )
                            {
                                Row row = new Row(rowXml);
                                if ( row != null )
                                {
                                    this.Add(row);
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
                    if ( reader != null )
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        #region ICollection<Row> Members

        /// <summary>
        /// Add row object in collection
        /// </summary>
        /// <param name="item">row to add in collection</param>
        public void Add( Row item )
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Row item parameter cannot be null");
            }

            if (item.Id > 0)
            {
                if (this._rows.ContainsKey(item.Id))
                {
                    var diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.LogMessageWithData(String.Format("Row with id {0} is already added. Cannot have duplicate rows with same id.", item.Id), item.ToXml(), MessageClassEnum.Warning);
                }
                else
                {
                    this._rows.Add(item.Id, item);
                }
            }
            else
            {
                item.Id = _rowIndex--;
                this._rows.Add(item.Id, item);
            }

            // update lookup index table with current row, Add action
            foreach (var indexeKey in _indexTables.Keys)
            {
                foreach (var cell in item.Cells)
                {
                    if (String.Compare(cell.ColumnName, indexeKey, true) == 0 && cell.Value != null && !String.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        AddRowToIndexTable(indexeKey, cell.ColumnName, item.Id);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all rows from collection
        /// </summary>
        public void Clear()
        {
            this._rows.Clear();
        }

        /// <summary>
        /// Determines whether the RowCollection contains a specific row.
        /// </summary>
        /// <param name="item">The row object to locate in the RowCollection.</param>
        /// <returns>
        /// <para>true : If row found in rowCollection</para>
        /// <para>false : If row found not in rowCollection</para>
        /// </returns>
        public bool Contains( Row item )
        {
            return this._rows.Values.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the RowCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from RowCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo( Row[] array, int arrayIndex )
        {
            foreach (var key in this._rows.Keys)
            {
                array[arrayIndex++] = this._rows[key];

                if (arrayIndex == int.MaxValue)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Get the count of no. of rows in RowCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._rows.Count;
            }
        }

        /// <summary>
        /// Check if RowCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the RowCollection.
        /// </summary>
        /// <param name="item">The row object to remove from the RowCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original RowCollection</returns>
        public bool Remove( Row item )
        {
            Int64 keyIndex = -1;
            Boolean rowFound = false;

            foreach (var key in this._rows.Keys)
            {
                if (this._rows[key] == item)
                {
                    keyIndex = key;
                    rowFound = true;
                    break;
                }
            }

            if (rowFound)
            {
                this._rows.Remove(keyIndex);

                RemoveIndex(keyIndex);

                return true;
            }

            return false;
        }

        #endregion

        #region IEnumerable<Row> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RowCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Row> GetEnumerator()
        {
            return this._rows.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RowCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( IEnumerator ) this._rows.GetEnumerator();
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of RowCollection object
        /// </summary>
        /// <returns>Xml string representing the RowCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Rows");

            if ( this._rows != null )
            {
                foreach ( Row row in this._rows.Values )
                {
                    xmlWriter.WriteRaw(row.ToXml());
                }
            }

            //Row node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get Xml representation of RowCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Rows");

            if ( this._rows != null )
            {
                foreach ( Row row in this._rows.Values )
                {
                    xmlWriter.WriteRaw(row.ToXml(serialization));
                }
            }

            //Row node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion ToXml methods
    }
}
