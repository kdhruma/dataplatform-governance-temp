using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections;
using System.IO;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Cell Collection for Table
    /// </summary>
    [DataContract]
    public class CellCollection : ICollection<Cell>, IEnumerable<Cell>, ICellCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Cell> _cells = new Collection<Cell>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public CellCollection() : base() { }

        #pragma warning disable 1570

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Xml having value for CellCollection
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Cells>
        ///     <Cell ColumnId="127" ColumnName="Value"><![CDATA[]]></Cell>
        ///     <Cell ColumnId="128" ColumnName="Code"><![CDATA[000]]></Cell>
        ///     <Cell ColumnId="129" ColumnName="Type"><![CDATA[]]></Cell>
        ///     <Cell ColumnId="0" ColumnName="StorageFormat"><![CDATA[000]]></Cell>
        ///     <Cell ColumnId="0" ColumnName="SearchColumns"><![CDATA[000,,]]></Cell>
        ///     <Cell ColumnId="0" ColumnName="DisplayFormat"><![CDATA[000]]></Cell>
        /// </Cells>
        /// ]]>
        /// </para>
        /// </param>

        #pragma warning restore 1570
        public CellCollection( String valueAsXml )
        {
            LoadCellCollection(valueAsXml);
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
        public override bool Equals( object obj )
        {
            if ( obj is CellCollection )
            {
                CellCollection objectToBeCompared = obj as CellCollection;
                Int32 CellsUnion = this._cells.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 CellsIntersect = this._cells.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if ( CellsUnion != CellsIntersect )
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
            foreach ( Cell cell in this._cells )
            {
                hashCode += cell.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Get cell from column Id
        /// </summary>
        /// <param name="columnId">Column id</param>
        /// <returns>Cell having given ColumnId</returns>
        /// <exception cref="Exception">Thrown if no cell is found with given ColumnId</exception>
        public Cell GetCell( Int32 columnId )
        {
            Cell cell = null;
            foreach (Cell cellColumn in this._cells)
            {
                if (cellColumn.ColumnId.Equals(columnId))
                {
                    cell = cellColumn;
                    break;
                }
            }

            if (cell == null)
            {
                throw new Exception(String.Concat("No column found with Columnid = ", columnId));
            }
            return cell;
        }

        /// <summary>
        /// Get cell from column Name
        /// </summary>
        /// <param name="columnName">Column Name</param>
        /// <returns>Cell having given columnName</returns>
        /// <exception cref="Exception">Thrown if no cell is found with given ColumnName</exception>
        public Cell GetCell( String columnName )
        {
            Cell cell = null;

            foreach (Cell cellColumn in this._cells)
            {
                if(String.Compare(cellColumn.ColumnName, columnName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    cell = cellColumn;
                    break;
                }
            }

            if (cell == null)
            {
                throw new Exception(String.Concat("No column found with ColumnName = ", columnName));
            }
        
            return cell;
        }

        /// <summary>
        /// Get cell from given columnId and ColumnName
        /// </summary>
        /// <param name="column">Column of which cell we are looking for</param>
        /// <returns>Cell having given column Id and column Name</returns>
        /// <exception cref="Exception">Thrown if no cell is found with given ColumnName and ColumnId</exception>
        public Cell GetCell( Column column )
        {
            if ( column == null )
            {
                throw new ArgumentNullException("column");
            }

            Int32 cellCount = _cells.Count;
            Cell cell = null;

            for (Int32 index = 0; index < cellCount; index++)
            {
                cell = _cells[index];
                if(cell.ColumnId == column.Id && cell.ColumnName == column.Name)
                    return cell;
            }

            throw new Exception(String.Concat("No column found with ColumnId = ", column.Id, " and ColumnName = ", column.Name));
        }

        #pragma warning disable 1570

        /// <summary>
        /// Populate current object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for CellCollection
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Cells>
        ///     <Cell ColumnId="127" ColumnName="Value"><![CDATA[]]></Cell>
        ///     <Cell ColumnId="128" ColumnName="Code"><![CDATA[000]]></Cell>
        ///     <Cell ColumnId="129" ColumnName="Type"><![CDATA[]]></Cell>
        ///     <Cell ColumnId="0" ColumnName="StorageFormat"><![CDATA[000]]></Cell>
        ///     <Cell ColumnId="0" ColumnName="SearchColumns"><![CDATA[000,,]]></Cell>
        ///     <Cell ColumnId="0" ColumnName="DisplayFormat"><![CDATA[000]]></Cell>
        /// </Cells>
        /// ]]>
        /// </para>
        /// </param>

        #pragma warning restore 1570
        public void LoadCellCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             *  <Cells>
                  <Cell ColumnId="127" ColumnName="Value"><![CDATA[]]></Cell>
                  <Cell ColumnId="128" ColumnName="Code"><![CDATA[000]]></Cell>
                  <Cell ColumnId="129" ColumnName="Type"><![CDATA[]]></Cell>
                  <Cell ColumnId="0" ColumnName="StorageFormat"><![CDATA[000]]></Cell>
                  <Cell ColumnId="0" ColumnName="SearchColumns"><![CDATA[000,,]]></Cell>
                  <Cell ColumnId="0" ColumnName="DisplayFormat"><![CDATA[000]]></Cell>
                </Cells>
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
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Cell" )
                        {
                            String cellXml = reader.ReadOuterXml();
                            if ( !String.IsNullOrEmpty(cellXml) )
                            {
                                Cell cell = new Cell(cellXml);
                                if ( cell != null )
                                {
                                    this.Add(cell);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public CellCollection Clone()
        {
            CellCollection cellCollection = new CellCollection();
            foreach (Cell cell in this._cells)
            {
                Cell clonedCell = cell.Clone();
                cellCollection._cells.Add(clonedCell);
            }

            return cellCollection;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetCellCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparission is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(CellCollection subsetCellCollection, Boolean compareIds = false)
        {
            if (subsetCellCollection != null && subsetCellCollection.Count > 0)
            {
                if (this._cells.Count == subsetCellCollection.Count)
                {
                    foreach (Cell cell in this._cells)
                    {
                        if (!cell.IsSuperSetOf(subsetCellCollection.GetCell(cell.ColumnName), compareIds))
                            return false;
                    }
                }
                else
                    return false;
            }
            
            return true;
        }

        #endregion

        #region Private Methods

        
        #endregion

        #region ICollection<Cell> Members

        /// <summary>
        /// Add cell object in collection
        /// </summary>
        /// <param name="item">cell to add in collection</param>
        public void Add(ICell item)
        {
            this._cells.Add((Cell)item);
        }

        /// <summary>
        /// Add cell object in collection
        /// </summary>
        /// <param name="item">cell to add in collection</param>
        public void Add( Cell item )
        {
            this._cells.Add(item);
        }

        /// <summary>
        /// Add cell object in collection at specific index.
        /// </summary>
        /// <param name="item">cell to add in collection</param>
        /// <param name="index">index at which cell to be added in collection</param>
        public void Insert(Int32 index,Cell item)
        {
            this._cells.Insert(index, item);
        }


        /// <summary>
        /// Removes all cells from collection
        /// </summary>
        public void Clear()
        {
            this._cells.Clear();
        }

        /// <summary>
        /// Determines whether the CellCollection contains a specific cell.
        /// </summary>
        /// <param name="item">The cell object to locate in the CellCollection.</param>
        /// <returns>
        /// <para>true : If cell found in cellCollection</para>
        /// <para>false : If cell found not in cellCollection</para>
        /// </returns>
        public bool Contains( Cell item )
        {
            return this._cells.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the CellCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from CellCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo( Cell[] array, int arrayIndex )
        {
            this._cells.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of cells in CellCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._cells.Count;
            }
        }

        /// <summary>
        /// Check if CellCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the CellCollection.
        /// </summary>
        /// <param name="item">The cell object to remove from the CellCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original CellCollection</returns>
        public bool Remove( Cell item )
        {
            return this._cells.Remove(item);
        }

        #endregion

        #region IEnumerable<Cell> Members

        /// <summary>
        /// Returns an enumerator that iterates through a CellCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            return this._cells.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a CellCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( IEnumerator ) this._cells.GetEnumerator();
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of CellCollection object
        /// </summary>
        /// <returns>Xml string representing the CellCollection</returns>
        public String ToXml()
        {
            String cellsXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Cells");

            foreach ( Cell cell in this._cells )
            {
                xmlWriter.WriteRaw(cell.ToXml());
            }

            //Cell node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            cellsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return cellsXml;
        }

        /// <summary>
        /// Get Xml representation of CellCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String cellsXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Cells");

            foreach ( Cell cell in this._cells )
            {
                xmlWriter.WriteRaw(cell.ToXml(serialization));
            }

            //Cell node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            cellsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return cellsXml;
        }

        #endregion ToXml methods
    }
}
