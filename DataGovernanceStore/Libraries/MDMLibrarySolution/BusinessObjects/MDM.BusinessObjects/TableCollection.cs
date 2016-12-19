using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents collection of columns
    /// </summary>
    [DataContract]
    public class TableCollection : ICollection<Table>, IEnumerable<Table>, ITableCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Table> _tables = new Collection<Table>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public TableCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public TableCollection( String valueAsXml )
        {
            LoadTableCollection(valueAsXml);
        }

        /// <summary>
        /// Load table from DataSet
        /// </summary>
        /// <param name="dataset">DataSet from which table is to created</param>
        public TableCollection( DataSet dataset )
        {
            LoadTableCollection(dataset);
        }
        
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Find table from TableCollection based on tableId
        /// </summary>
        /// <param name="tableId">TableID to search</param>
        /// <returns>Table object having given tableID</returns>
        public Table this[Int32 tableId]
        {
            get
            {
                Table table = GetTable(tableId);

                if ( table == null )
                    throw new ArgumentException(String.Format("No table found for table id: {0}", tableId), "tableId");
                else
                    return table;
            }
            set
            {
                Table table = GetTable(tableId);

                if ( table == null )
                    throw new ArgumentException(String.Format("No table found for table id: {0}", tableId), "tableId");

                table = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Remove table object from TableCollection
        /// </summary>
        /// <param name="tableId">Id of table which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove( Int32 tableId )
        {
            Table table = GetTable(tableId);

            if ( table == null )
                throw new ArgumentException("No table found for given table id");
            else
                return this.Remove(table);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals( object obj )
        {
            if ( obj is TableCollection )
            {
                TableCollection objectToBeCompared = obj as TableCollection;
                Int32 tablesUnion = this._tables.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 tablesIntersect = this._tables.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if ( tablesUnion != tablesIntersect )
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
            foreach ( Table table in this._tables )
            {
                hashCode += table.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Load table from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having data
        /// <para>
        /// Sample Xml:
        /// <Tables>
        ///     <Table Id="2" Name="" LongName="tblk_Color" AttrtibuteId="3006">
        ///         <Columns>
        ///             <Column Id="1" Name="LookupKey" LongName="" />
        ///             <Column Id="2" Name="Value" LongName="" />
        ///             <Column Id="3" Name="Code" LongName="" />
        ///             <Column Id="4" Name="Type" LongName="" />
        ///         </Columns>
        ///         <Rows>
        ///             <Row Id="1" SearchValue="" DisplayValue="" StorageValue="">
        ///                 <Cells>
        ///                     <Cell ColumnId="1" ColumnName="LookupKey"><![CDATA[000]]></Cell>
        ///                     <Cell ColumnId="2" ColumnName="Value"><![CDATA[]]></Cell>
        ///                  <Cell ColumnId="3" ColumnName="Code"><![CDATA[000]]></Cell>
        ///                  <Cell ColumnId="4" ColumnName="Type"><![CDATA[]]></Cell>
        ///                 </Cells>
        ///             </Row>
        ///         </Rows>
        ///     </Table>
        ///     <Table Id="-1" Name="" LongName="tblk_Pattern" AttrtibuteId="3008">
        ///         <Columns>
        ///             <Column Id="0" Name="LookupKey" LongName="" />
        ///             <Column Id="0" Name="Value" LongName="" />
        ///             <Column Id="0" Name="Code" LongName="" />
        ///         </Columns>
        ///         <Rows>
        ///             <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
        ///                 <Cells>
        ///                     <Cell ColumnId="0" ColumnName="LookupKey"><![CDATA[101 Dalmations           ]]></Cell>
        ///                     <Cell ColumnId="0" ColumnName="Value"><![CDATA[101 Dalmations           ]]></Cell>
        ///                     <Cell ColumnId="0" ColumnName="Code"><![CDATA[101 Dalmations           ]]></Cell>
        ///                 </Cells>
        ///             </Row>
        ///             <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
        ///                 <Cells>
        ///                     <Cell ColumnId="0" ColumnName="LookupKey"><![CDATA[10K  Yellow Gold  7" Herr]]></Cell>
        ///                     <Cell ColumnId="0" ColumnName="Value"><![CDATA[10K  Yellow Gold  7" Herr]]></Cell>
        ///                     <Cell ColumnId="0" ColumnName="Code"><![CDATA[10K  Yellow Gold  7" Herr]]></Cell>
        ///                 </Cells>
        ///             </Row>
        ///         </Rows>
        ///     </Table>
        /// </Tables>
        /// </para>
        /// </param>
        public void LoadTableCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             *  <Tables>
                  <Table Id="2" Name="" LongName="tblk_Color">
                    <Columns>
                      <Column Id="1" Name="LookupKey" LongName="" />
                      <Column Id="2" Name="Value" LongName="" />
                      <Column Id="3" Name="Code" LongName="" />
                      <Column Id="4" Name="Type" LongName="" />
                    </Columns>
                    <Rows>
                      <Row Id="1" SearchValue="" DisplayValue="" StorageValue="">
                        <Cells>
                          <Cell ColumnId="1" ColumnName="LookupKey"><![CDATA[000]]></Cell>
                          <Cell ColumnId="2" ColumnName="Value"><![CDATA[]]></Cell>
                          <Cell ColumnId="3" ColumnName="Code"><![CDATA[000]]></Cell>
                          <Cell ColumnId="4" ColumnName="Type"><![CDATA[]]></Cell>
                        </Cells>
                      </Row>
                    </Rows>
                  </Table>
                  <Table Id="-1" Name="" LongName="tblk_Pattern" >
                    <Columns>
                      <Column Id="0" Name="LookupKey" LongName="" />
                      <Column Id="0" Name="Value" LongName="" />
                      <Column Id="0" Name="Code" LongName="" />
                    </Columns>
                    <Rows>
                      <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
                        <Cells>
                          <Cell ColumnId="0" ColumnName="LookupKey"><![CDATA[101 Dalmations           ]]></Cell>
                          <Cell ColumnId="0" ColumnName="Value"><![CDATA[101 Dalmations           ]]></Cell>
                          <Cell ColumnId="0" ColumnName="Code"><![CDATA[101 Dalmations           ]]></Cell>
                        </Cells>
                      </Row>
                      <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
                        <Cells>
                          <Cell ColumnId="0" ColumnName="LookupKey"><![CDATA[10K  Yellow Gold  7" Herr]]></Cell>
                          <Cell ColumnId="0" ColumnName="Value"><![CDATA[10K  Yellow Gold  7" Herr]]></Cell>
                          <Cell ColumnId="0" ColumnName="Code"><![CDATA[10K  Yellow Gold  7" Herr]]></Cell>
                        </Cells>
                      </Row>
                    </Rows>
                  </Table>
                </Tables>
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
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Table" )
                        {
                            String tableXml = reader.ReadOuterXml();
                            if ( !String.IsNullOrEmpty(tableXml) )
                            {
                                Table table = new Table(tableXml);
                                if ( table != null )
                                {
                                    this.Add(table);
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

        /// <summary>
        /// Load table from DataSet
        /// </summary>
        /// <param name="dataset">DataSet from which table is to created</param>
        public void LoadTableCollection( DataSet dataset )
        {
            if ( dataset != null && dataset.Tables != null )
            {
                foreach ( DataTable dt in dataset.Tables )
                {
                    this._tables.Add(new Table(dt));
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get table from collection based on Id
        /// </summary>
        /// <param name="tableId">Id of table to search in current collection</param>
        /// <returns></returns>
        private Table GetTable( Int32 tableId )
        {
            var filteredTables = from table in this._tables
                                  where table.Id == tableId
                                  select table;

            if ( filteredTables.Any() )
                return filteredTables.First();
            else
                return null;
        }

        #endregion

        #region ICollection<Table> Members

        /// <summary>
        /// Add table object in collection
        /// </summary>
        /// <param name="item">table to add in collection</param>
        public void Add( Table item )
        {
            this._tables.Add(item);
        }

        /// <summary>
        /// Removes all tables from collection
        /// </summary>
        public void Clear()
        {
            this._tables.Clear();
        }

        /// <summary>
        /// Determines whether the TableCollection contains a specific table.
        /// </summary>
        /// <param name="item">The table object to locate in the TableCollection.</param>
        /// <returns>
        /// <para>true : If table found in tableCollection</para>
        /// <para>false : If table found not in tableCollection</para>
        /// </returns>
        public bool Contains( Table item )
        {
            return this._tables.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the TableCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from TableCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo( Table[] array, int arrayIndex )
        {
            this._tables.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of tables in TableCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._tables.Count;
            }
        }

        /// <summary>
        /// Check if TableCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the TableCollection.
        /// </summary>
        /// <param name="item">The table object to remove from the TableCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original TableCollection</returns>
        public bool Remove( Table item )
        {
            return this._tables.Remove(item);
        }

        #endregion

        #region IEnumerable<Table> Members

        /// <summary>
        /// Returns an enumerator that iterates through a TableCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Table> GetEnumerator()
        {
            return this._tables.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a TableCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( IEnumerator ) this._tables.GetEnumerator();
        }

        #endregion

        #region ITableCollection Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of TableCollection object
        /// </summary>
        /// <returns>Xml string representing the TableCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Tables");

            if ( this._tables != null )
            {
                foreach ( Table table in this._tables )
                {
                    xmlWriter.WriteRaw(table.ToXml());
                }
            }

            //Table node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get Xml representation of TableCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Tables");

            if ( this._tables != null )
            {
                foreach ( Table table in this._tables )
                {
                    xmlWriter.WriteRaw(table.ToXml(serialization));
                }
            }

            //Table node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion ToXml methods

        /// <summary>
        /// Add table in ITableCollection
        /// </summary>
        /// <param name="table">Table to add in ITableCollection</param>
        /// <exception cref="ArgumentNullException">Thrown if table is null</exception>
        public void AddTable( ITable table )
        {
            if ( table == null )
            {
                throw new ArgumentNullException("table");
            }

            this._tables.Add(( Table ) table);
        }

        /// <summary>
        /// Add tables in current table collection
        /// </summary>
        /// <param name="tables">ITableCollection to add in current collection</param>
        /// <exception cref="ArgumentNullException">Thrown if tableCollection is null</exception>
        public void AddTables( ITableCollection tables )
        {
            if ( tables == null )
            {
                throw new ArgumentNullException("tables");
            }

            foreach ( Table table in tables )
            {
                this._tables.Add(( Table ) table);
            }
        }

        #endregion ITableCollection Methods
    }
}
