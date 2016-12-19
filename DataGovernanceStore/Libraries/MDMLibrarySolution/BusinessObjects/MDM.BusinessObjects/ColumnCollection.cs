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
    using MDM.Core.Exceptions;

    /// <summary>
    /// Represents collection of columns
    /// </summary>
    [DataContract]
    public class ColumnCollection : ICollection<Column>, IEnumerable<Column>, IColumnCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Column> _columns = new Collection<Column>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ColumnCollection() : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ColumnCollection(String valueAsXml)
        {
            LoadColumnCollection(valueAsXml);
        }

        /// <summary>
        /// Constructor which takes List of type Column
        /// </summary>
        /// <param name="columns"></param>
        public ColumnCollection(IList<Column> columns)
        {
            this._columns = new Collection<Column>(columns);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find column from ColumnCollection based on columnId
        /// </summary>
        /// <param name="columnId">ColumnID to search</param>
        /// <returns>Column object having given columnID</returns>
        public Column this[Int32 columnId]
        {
            get
            {
                Column column = GetColumn(columnId);

                if (column == null)
                    throw new ArgumentException(String.Format("No column found for column id: {0}", columnId), "columnId");
                else
                    return column;
            }
            set
            {
                Column column = GetColumn(columnId);

                if (column == null)
                    throw new ArgumentException(String.Format("No column found for column id: {0}", columnId), "columnId");

                column = value;
            }
        }

        /// <summary>
        /// Find column from ColumnCollection based on columnName
        /// </summary>
        /// <param name="columnName">ColumnName to search</param>
        /// <returns>Column object having given columnName</returns>
        public Column this[String columnName]
        {
            get
            {
                Column column = GetColumn(columnName);

                if (column == null)
                    throw new ArgumentException(String.Format("No column found for column Name: {0}", columnName), "columnName");
                else
                    return column;
            }
            set
            {
                Column column = GetColumn(columnName);

                if (column == null)
                    throw new ArgumentException(String.Format("No column found for column Name: {0}", columnName), "columnName");

                column = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Remove column object from ColumnCollection
        /// </summary>
        /// <param name="columnId">Id of column which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 columnId)
        {
            Column column = GetColumn(columnId);

            if (column == null)
                throw new ArgumentException("No column found for given column id");
            else
                return this.Remove(column);
        }

        /// <summary>
        /// Remove column object from ColumnCollection
        /// </summary>
        /// <param name="columnName">Name of column which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(String columnName)
        {
            Column column = GetColumn(columnName);

            if (column == null)
                throw new ArgumentNullException("No column found for given column name");
            else
                return this.Remove(column);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ColumnCollection)
            {
                ColumnCollection objectToBeCompared = obj as ColumnCollection;
                Int32 columnsUnion = this._columns.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 columnsIntersect = this._columns.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (columnsUnion != columnsIntersect)
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
            foreach (Column column in this._columns)
            {
                hashCode += column.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Populate columns from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having columns detail
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <Columns>
        ///       <Column Id="1" Name="Id" DisplayName="Id" />
        ///       <Column Id="2" Name="Street" DisplayName="Street" />
        ///       <Column Id="3" Name="City" DisplayName="City" />
        ///     </Columns>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadColumnCollection(String valuesAsXml)
        {
            #region Sample Xml

            /*
                <Columns>
                  <Column Id="" Name="Id" LongName="Id" />
                  <Column Id="" Name="Street" LongName="Street" />
                  <Column Id="" Name="City" LongName="City" />
                </Columns>
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
                            String columnXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(columnXml))
                            {
                                Column column = new Column(columnXml);
                                if (column != null)
                                {
                                    this.Add(column);
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
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Find a column from current column collection with given ColumnName
        /// </summary>
        /// <param name="columnName">Name of column which is to be searched in current collection</param>
        /// <returns>Column with given name</returns>
        public Column GetColumn(String columnName)
        {
            String columnNameInLowerCase = columnName.ToLowerInvariant();

            Int32 columnCount = _columns.Count;
            Column column = null;

            for (Int32 index = 0; index < columnCount; index++)
            {
                column = _columns[index];
                if (column.Name.ToLowerInvariant() == columnNameInLowerCase)
                    return column;
            }
            return null;
        }

        /// <summary>
        /// Get a column from current column collection based on given index
        /// </summary>
        /// <param name="indexOfColumn">Indicates index of column which is to be searched from current collection</param>
        /// <returns>Returns column by given index</returns>
        public Column GetColumnByIndex(Int32 indexOfColumn)
        {
            Column column = null;

            if (_columns != null)
            {
                if (indexOfColumn < _columns.Count)
                {
                    column = _columns[indexOfColumn];
                }
            }

            return column;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetColumnCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(ColumnCollection subsetColumnCollection, Boolean compareIds = false)
        {
            if (subsetColumnCollection != null && subsetColumnCollection.Count > 0)
            {
                if (this._columns.Count == subsetColumnCollection.Count)
                {
                    foreach (Column column in this._columns)
                    {
                        if (!column.IsSuperSetOf(subsetColumnCollection.GetColumn(column.Name), compareIds))
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

        private Column GetColumn(Int32 columnId)
        {
            Int32 columnCount = _columns.Count;
            Column column = null;

            for (Int32 index = 0; index < columnCount; index++)
            {
                column = _columns[index];
                if (column.Id == columnId)
                    return column;
            }
            return null;
        }

        #endregion

        #region ICollection<Column> Members

        /// <summary>
        /// Add column object in collection
        /// Usage of this method should be avoided to ensure row's consistency for added columns.
        /// Instead use "Table.AddColumn(Column newColumn)" method
        /// </summary>
        /// <param name="column">column to add in collection</param>
        /// <exception cref="Exception">Thrown if column having same name is being added</exception>
        public void Add(IColumn column)
        {
            this.Add((Column) column);
        }

        /// <summary>
        /// Add column object in collection
        /// Usage of this method should be avoided to ensure row's consistency for added columns.
        /// Instead use "Table.AddColumn(Column newColumn)" method
        /// </summary>
        /// <param name="column">column to add in collection</param>
        /// <exception cref="Exception">Thrown if column having same name is being added</exception>
        public void Add(Column column)
        {
            this.Add(column, false);
        }

        /// <summary>
        /// Add column object in collection
        /// Usage of this method should be avoided to ensure row's consistency for added columns.
        /// Instead use "Table.AddColumn(Column newColumn)" method
        /// </summary>
        /// <param name="column">column to add in collection</param>
        /// <param name="ignoreDuplicateCheck">Flag indicates to ignore duplicate check while adding column</param>
        /// <exception cref="Exception">Thrown if column having same name is being added</exception>
        public void Add(Column column, Boolean ignoreDuplicateCheck)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }

            if (String.IsNullOrWhiteSpace(column.Name))
            {
                throw new ArgumentException("Column.Name cannot be null or empty");
            }

            if (!ignoreDuplicateCheck)
            {
                Column duplicateColumn = GetColumn(column.Name);

                if (duplicateColumn != null)
                {
                    throw new DuplicateObjectException("A Column with same Name already exist in table. Please give unique column name");
                }
            }

            this._columns.Add(column);
        }

        /// <summary>
        /// Insert column object in collection at a specific index.
        /// Usage of this method should be avoided to ensure row's consistency for added columns.
        /// Instead use "Table.AddColumn(Column newColumn)" method
        /// </summary>
        /// <param name="index">index at which column to be added in collection</param>
        /// <param name="column">column to add in collection</param>
        /// <exception cref="Exception">Thrown if column having same name is being added</exception>
        public void Insert(Int32 index, Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }

            if (String.IsNullOrWhiteSpace(column.Name))
            {
                throw new ArgumentException("Column.Name cannot be null or empty");
            }

            Column duplicateColumn = GetColumn(column.Name);

            if (duplicateColumn != null)
            {
                throw new DuplicateObjectException("A Column with same Name already exist in table. Please give unique column name");
            }

            this._columns.Insert(index, column);
        }

        /// <summary>
        /// Removes all columns from collection
        /// </summary>
        public void Clear()
        {
            this._columns.Clear();
        }

        /// <summary>
        /// Determines whether the ColumnCollection contains a specific column.
        /// </summary>
        /// <param name="item">The column object to locate in the ColumnCollection.</param>
        /// <returns>
        /// <para>true : If column found in columnCollection</para>
        /// <para>false : If column found not in columnCollection</para>
        /// </returns>
        public bool Contains(Column item)
        {
            return this._columns.Contains(item);
        }

        /// <summary>
        /// Determines whether the ColumnCollection contains a column with specific column name.
        /// </summary>
        /// <param name="columnName">The column object with given Column.Name to locate in the ColumnCollection.</param>
        /// <returns>
        /// <para>true : If column found in columnCollection</para>
        /// <para>false : If column found not in columnCollection</para>
        /// </returns>
        public bool Contains(String columnName)
        {
            Column column = GetColumn(columnName);
            if (column != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the ColumnCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ColumnCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Column[] array, int arrayIndex)
        {
            this._columns.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of columns in ColumnCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._columns.Count;
            }
        }

        /// <summary>
        /// Check if ColumnCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ColumnCollection.
        /// </summary>
        /// <param name="item">The column object to remove from the ColumnCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ColumnCollection</returns>
        public bool Remove(Column item)
        {
            return this._columns.Remove(item);
        }

        #endregion

        #region IEnumerable<Column> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Column> GetEnumerator()
        {
            return this._columns.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) this._columns.GetEnumerator();
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ColumnCollection object
        /// </summary>
        /// <returns>Xml string representing the ColumnCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Columns");

            if (this._columns != null)
            {
                foreach (Column column in this._columns)
                {
                    xmlWriter.WriteRaw(column.ToXml());
                }
            }

            //Column node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get Xml representation of ColumnCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Columns");

            if (this._columns != null)
            {
                foreach (Column column in this._columns)
                {
                    xmlWriter.WriteRaw(column.ToXml(serialization));
                }
            }

            //Column node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Deep clones column collection with each column in the collection
        /// </summary>
        /// <returns></returns>
        public IColumnCollection Clone()
        {
            var clonedColumnCollection = new ColumnCollection();

            if (this._columns != null && _columns.Count > 0)
            {
                foreach (var column in this._columns)
                {
                    if (column != null)
                    {
                        clonedColumnCollection.Add((Column)column.Clone(), true); // This is deep clone so no need to do dup check
                    }
                }
            }

            return clonedColumnCollection;
        }

        #endregion ToXml methods
    }
}
