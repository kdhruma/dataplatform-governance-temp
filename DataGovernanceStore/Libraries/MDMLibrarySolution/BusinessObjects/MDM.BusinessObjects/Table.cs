using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Data;
using System.Collections;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Represents table which is a matrix of rows and columns.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Hashtable))]
    public class Table : MDMObject, ITable
    {
        #region Fields

        /// <summary>
        /// Field denoting columns. Column defines the structure of table
        /// </summary>
        private ColumnCollection _columns = new ColumnCollection();

        /// <summary>
        /// Field denoting Rows in table.
        /// </summary>
        private RowCollection _rows = new RowCollection();

        /// <summary>
        /// Field used for storing extra information for table
        /// </summary>
        private Hashtable _extendedProperties = new Hashtable();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Table()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id, Name and LongName of a Table as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Table (RSTObjectId)</param>
        /// <param name="name">Indicates the Name of a Table </param>
        /// <param name="longName">Indicates the LongName of a Table </param>
        public Table(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        #pragma warning disable 1570

        /// <summary>
        /// Load table from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having table values
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// 
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
        /// 
        /// ]]>
        /// </para>
        /// </param>

        #pragma warning restore 1570

        public Table(String valuesAsXml)
        {
            LoadTable(valuesAsXml);
        }

        /// <summary>
        /// Load table from Datatable
        /// </summary>
        /// <param name="dt">Datatable which is to be converted into MDM.BusinessObject.Table</param>
        /// <param name="rowIdColumnName">Indicates column name from one of the columns of table which is to be treated as value for RowId</param>
        public Table(DataTable dt, String rowIdColumnName = "")
        {
            LoadTable(dt, rowIdColumnName);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting columns. Column defines the structure of table
        /// </summary>
        [DataMember]
        public ColumnCollection Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }

        /// <summary> 
        /// Property denoting Rows in table.
        /// </summary>
        [DataMember]
        public RowCollection Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
            }
        }

        /// <summary>
        /// Property used for storing extra information for table
        /// </summary>
        [DataMember]
        public new Hashtable ExtendedProperties
        {
            get
            {
                return this._extendedProperties;
            }
            set
            {
                this._extendedProperties = value;
            }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Get row from current table's row collection based on row Id
        /// </summary>
        /// <param name="rowId">Id of row</param>
        /// <returns>Row at given Id</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.-or-index is equal to or greater than RowCollection.Count.</exception>
        public Row this[Int64 rowId]
        {
            get
            {
                if (this.Rows == null)
                {
                    throw new Exception("No row found in table");
                }

                return this.Rows[rowId];
            }
        }

        /// <summary>
        /// Get value of given row and given column
        /// </summary>
        /// <param name="rowId">Id of row of which data is to be fetched</param>
        /// <param name="columnId">Id of column of which data is to be fetched</param>
        /// <returns>Value at given given row and column intersection</returns>
        public Object this[Int64 rowId, Int32 columnId]
        {
            get
            {
                if (this.Rows == null)
                {
                    throw new Exception("No row found in table");
                }

                Object returnValue = null;

                Row row = this.Rows[rowId];

                if (row != null)
                {
                    returnValue = row[columnId];
                }

                return returnValue;
            }
        }

        #endregion Indexers

        #region Public Methods

        #pragma warning disable 1570

        /// <summary>
        /// Load table from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having table values
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// 
        ///     <Table Id="2" Name="" LongName="tblk_Color" AttrtibuteId="3006">
        ///         <Columns>
        ///             <Column Id="1" Name="LookupKey" LongName="" DefaultValue="" />
        ///             <Column Id="2" Name="Value" LongName="" DefaultValue="" />
        ///             <Column Id="3" Name="Code" LongName="" DefaultValue="" />
        ///             <Column Id="4" Name="Type" LongName="" DefaultValue="" />
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
        /// 
        /// ]]>
        /// </para>
        /// </param>

        #pragma warning restore 1570

        public void LoadTable(String valuesAsXml)
        {
            #region Sample Xml

            /*
             * <Table Id="-1" Name="" LongName="tblk_Pattern" AttrtibuteId="3008">
                <Columns>
                  <Column Id="0" Name="LookupKey" LongName="" DefaultValue="" />
                  <Column Id="0" Name="Value" LongName="" DefaultValue="" />
                  <Column Id="0" Name="Code" LongName="" DefaultValue="" />
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
             */

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table" && reader.HasAttributes)
                    {
                        #region Read table Metadata

                        if (reader.MoveToAttribute("Id"))
                        {
                            this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                        }

                        //if ( reader.MoveToAttribute("AttributeId") )
                        //{
                        //    this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                        //}

                        if (reader.MoveToAttribute("Name"))
                        {
                            this.Name = reader.ReadContentAsString();
                        }

                        if (reader.MoveToAttribute("LongName"))
                        {
                            this.LongName = reader.ReadContentAsString();
                        }

                        #endregion Read table Metadata
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Columns")
                    {
                        #region Read Columns

                        String columnsXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(columnsXml))
                        {
                            ColumnCollection columns = new ColumnCollection(columnsXml);
                            if (columns != null)
                            {
                                foreach (Column col in columns)
                                {
                                    this.Columns.Add(col);
                                }
                            }
                        }

                        #endregion Read Columns
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Rows")
                    {
                        #region Read Rows

                        String rowsXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(rowsXml))
                        {
                            RowCollection rows = new RowCollection(rowsXml);
                            if (rows != null)
                            {
                                foreach (Row row in rows)
                                {
                                    this.Rows.Add(row);
                                }
                            }
                        }

                        #endregion Read Rows
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperties")
                    {
                        #region Read Extended Properties

                        String propXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(propXml))
                        {
                            Hashtable props = this.ReadExtendedProperties(propXml);
                            if (props != null)
                            {
                                this.ExtendedProperties = props;
                            }
                        }

                        #endregion Read Extended Properties
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

        /// <summary>
        /// Load Table from DataTable
        /// </summary>
        /// <param name="datatable">DataTable having value for Table</param>
        /// <param name="rowIdColumnName">Indicates column name from one of the columns of table which is to be treated as value for RowId</param>
        public void LoadTable(DataTable datatable, String rowIdColumnName)
        {
            if (datatable != null)
            {
                this.Name = datatable.TableName;
                this.LongName = datatable.TableName;

                if (datatable.Columns != null && datatable.Columns.Count > 0)
                {
                    //Add columns
                    this.AddColumns(datatable.Columns);

                    //Add Rows
                    if (datatable.Rows != null)
                    {
                        //Loop through the row
                        foreach (DataRow row in datatable.Rows)
                        {
                            Row rsRow = this.NewRow();
                            foreach (DataColumn column in datatable.Columns)
                            {
                                //Read value from DataTable and populate Table object with value.
                                Object cellValue = row[column];
                                rsRow[column.ColumnName] = cellValue;
                                if (!String.IsNullOrWhiteSpace(rowIdColumnName) &&
                                    column.ColumnName.ToLower().Equals(rowIdColumnName.ToLower()) &&
                                    cellValue != null)
                                {
                                    rsRow.Id = ValueTypeHelper.Int32TryParse(cellValue.ToString(), 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add columns in Table from DataColumnCollection
        /// </summary>
        /// <param name="columns">Columns from DataTable which are to be added in Table</param>
        public void AddColumns(DataColumnCollection columns)
        {
            if (columns != null)
            {
                Int32 index = 0;
                foreach (DataColumn column in columns)
                {

                    Column rsColumn = new Column(index++, column.ColumnName, column.ColumnName, null);
                    AddColumn(rsColumn);
                }
            }
        }

        /// <summary>
        /// Add new row in table. This row will have structure defined.
        /// </summary>
        /// <returns>Newly added row.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no columns are defined for table and we are trying to add row</exception>
        public Row NewRow()
        {
            return this.NewRow(true);
        }

        /// <summary>
        /// Add new row in table. This row will have structure defined.
        /// </summary>
        /// <param name="id">Row Id</param>
        /// <returns>Newly added row.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no columns are defined for table and we are trying to add row</exception>

        public Row NewRow(Int64 id)
        {
            return NewRow(true, id);
        }

        /// <summary>
        /// Add new row in table. This row will have structure defined.
        /// </summary>
        /// <returns>Newly added row.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no columns are defined for table and we are trying to add row</exception>
        public Row NewRow(Boolean populateDefaultValue, Int64 id = 0)
        {
            Row row = new Row(id);
            
            if (this.Rows == null)
            {
                this.Rows = new RowCollection();
            }

            if (this.Columns == null || (this.Columns.Count <= 0))
            {
                throw new InvalidOperationException("No columns are defined for given table. Please define model for table first and then try to add new row");
            }

            foreach (Column col in this.Columns)
            {
                row.AddCell(col, populateDefaultValue);
            }

            this.Rows.Add(row);

            return row;
        }

        /// <summary>
        /// Add column in Current table. This method will make sure all existing rows has structure setup for newly added column
        /// </summary>
        /// <param name="newColumn">Column to add</param>
        /// <returns>Indicates whether column is successfully added or not</returns>
        public Boolean AddColumn(Column newColumn)
        {
            Boolean returnVal = true;

            this.Columns.Add(newColumn);

            if (this.Rows != null)
            {
                foreach (Row row in this.Rows)
                {
                    row.AddCell(newColumn);
                }
            }

            return returnVal;
        }

        /// <summary>
        /// Add column in Current table. This method will make sure all existing rows has structure setup for newly added column
        /// </summary>
        /// <param name="newColumn">Column to add</param>
        /// <returns>Indicates whether column is successfully added or not</returns>
        public Boolean AddColumn(IColumn newColumn)
        {
            if (newColumn == null)
            {
                throw new ArgumentNullException("newColumn");
            }
            Boolean returnValue = true;
            returnValue = this.AddColumn((Column)newColumn);
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Table CopyStructure()
        {
            Table newTable = new Table();
            newTable.Id = this.Id;
            newTable.Name = this.Name;
            newTable.LongName = this.LongName;
            newTable.ExtendedProperties = this.ExtendedProperties;
            newTable.Columns = (ColumnCollection)this.Columns.Clone();
            return newTable;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetTable">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Table subsetTable, Boolean compareIds = false)
        {
            if (subsetTable != null)
            {
                if (base.IsSuperSetOf(subsetTable, compareIds))
                {
                    if (!this.Columns.IsSuperSetOf(subsetTable.Columns))
                        return false;

                    if (!this.Rows.IsSuperSetOf(subsetTable.Rows))
                        return false;
                }
            }

            return true;
        }

        #endregion Public Methods

        #region ITable Methods

        #region ToXml Methods

        /// <summary>
        /// Represents Table in Xml format
        /// </summary>
        /// <returns>String representing Table in Xml format</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Table");

            #region write Table meta data for Full Xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);

            #endregion write Table meta data for Full Xml

            #region write Extended properties

            xmlWriter.WriteStartElement("ExtendedProperties");

            if (this.ExtendedProperties != null)
            {
                foreach (String key in this.ExtendedProperties.Keys)
                {
                    //ExtendedProperty node start
                    xmlWriter.WriteStartElement("ExtendedProperty");

                    xmlWriter.WriteAttributeString("Key", key);
                    if (this.ExtendedProperties[key] != null)
                    {
                        xmlWriter.WriteCData(this.ExtendedProperties[key].ToString());
                    }
                    //ExtendedProperty node end
                    xmlWriter.WriteEndElement();
                }
            }

            //ExtendedProperties node end
            xmlWriter.WriteEndElement();

            #endregion write Extended properties

            #region write Columns

            xmlWriter.WriteStartElement("Columns");

            if (this.Columns != null)
            {
                foreach (Column column in this.Columns)
                {
                    try
                    {
                        xmlWriter.WriteRaw(column.ToXml());
                    }
                    catch
                    {
                        //TODO - Log Exceptions
                    }
                }
            }

            //Columns node end
            xmlWriter.WriteEndElement();

            #endregion write Columns

            #region write Rows

            xmlWriter.WriteStartElement("Rows");

            if (this.Rows != null)
            {
                foreach (Row row in this.Rows)
                {
                    try
                    {
                        xmlWriter.WriteRaw(row.ToXml());
                    }
                    catch
                    {
                        // TODO : Log exceptions
                    }
                }
            }

            //Rows node end
            xmlWriter.WriteEndElement();

            #endregion write Rows

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
        /// Represents Table in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <param name="useColumnNamesAsTag">Flag to decide whether the cell object tag start with column name or not.
        /// This flag used only in the case of UIRender ObjectSerialization</param>
        /// <returns>String representing Table in Xml format</returns>
        public String ToXml(ObjectSerialization objectSerialization, Boolean useColumnNamesAsTag = false)
        {
            String xml = String.Empty;
            if (objectSerialization == ObjectSerialization.Full)
            {
                return this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                xmlWriter.WriteStartElement("Table");

                if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write Table meta data for Processing Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);

                    #endregion write Table meta data for Processing Xml

                    #region write Extended properties for Processing Xml

                    xmlWriter.WriteStartElement("ExtendedProperties");

                    if (this.ExtendedProperties != null)
                    {
                        foreach (String key in this.ExtendedProperties.Keys)
                        {
                            //ExtendedProperty node start
                            xmlWriter.WriteStartElement("ExtendedProperty");

                            xmlWriter.WriteAttributeString("Key", key);
                            if (this.ExtendedProperties[key] != null)
                            {
                                xmlWriter.WriteCData(this.ExtendedProperties[key].ToString());
                            }

                            //ExtendedProperty node end
                            xmlWriter.WriteEndElement();
                        }
                    }

                    //ExtendedProperties node end
                    xmlWriter.WriteEndElement();

                    #endregion write Extended properties for Processing Xml
                }
                else if (objectSerialization == ObjectSerialization.UIRender)
                {
                    #region write Table meta data for UIRendering Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);

                    #endregion write Table meta data for UIRendering Xml

                    #region write Extended properties for UIRendering Xml

                    if (this.ExtendedProperties != null)
                    {
                        if (this.ExtendedProperties.Contains("AttributeId") && this.ExtendedProperties["AttributeId"] != null)
                        {
                            xmlWriter.WriteAttributeString("AttributeId", this.ExtendedProperties["AttributeId"].ToString());
                        }
                    }


                    #endregion write Extended properties for UIRendering Xml
                }

                #region write Columns

                xmlWriter.WriteStartElement("Columns");

                if (this.Columns != null)
                {
                    foreach (Column column in this.Columns)
                    {
                        try
                        {
                            xmlWriter.WriteRaw(column.ToXml());
                        }
                        catch 
                        {
                            //TODO - Log Exceptions
                        }
                    }
                }

                //Columns node end
                xmlWriter.WriteEndElement();

                #endregion write Columns

                if (objectSerialization == ObjectSerialization.UIRender || objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write Rows

                    xmlWriter.WriteStartElement("Rows");

                    if (this.Rows != null)
                    {
                        foreach (Row row in this.Rows)
                        {
                            try
                            {
                                xmlWriter.WriteRaw(row.ToXml(objectSerialization, true, useColumnNamesAsTag));
                            }
                            catch
                            {
                                // TODO : Log exceptions
                            }
                        }
                    }

                    //Rows node end
                    xmlWriter.WriteEndElement();

                    #endregion write Rows
                }
                else if (objectSerialization == ObjectSerialization.External)
                {
                    #region write Rows

                    xmlWriter.WriteStartElement("Rows");

                    if (this.Rows != null)
                    {
                        foreach (Row row in this.Rows)
                        {
                            try
                            {
                                xmlWriter.WriteRaw(row.ToXml(objectSerialization));
                            }
                            catch
                            {
                                // TODO : Log exceptions
                            }
                        }
                    }

                    //Rows node end
                    xmlWriter.WriteEndElement();

                    #endregion write Rows
                }

                //Table node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return xml;
        }

        #endregion ToXml Methods

        #region Get Methods

        /// <summary>
        /// Get rows of current table
        /// </summary>
        /// <returns>Rows collection interface</returns>
        public IRowCollection GetRows()
        {
            IRowCollection rows = null;
            if (this.Rows != null)
            {
                rows = (IRowCollection)this.Rows;
            }
            return rows;
        }

        /// <summary>
        /// Get columns of current table
        /// </summary>
        /// <returns>Columns collection interface</returns>
        public IColumnCollection GetColumns()
        {
            IColumnCollection columns = null;
            if (this.Columns != null)
            {
                columns = (IColumnCollection)this.Columns;
            }
            return columns;
        }

        /// <summary>
        /// Get column by ColumnName
        /// </summary>
        /// <returns>Column having given name</returns>
        public IColumn GetColumn(String columnName)
        {
            IColumn column = null;
            if (this.Columns != null)
            {
                Column col = this.Columns.GetColumn(columnName);
                if (col != null)
                {
                    column = (IColumn)col;
                }
            }
            return column;
        }

        /// <summary>
        /// Get Extended properties from current table
        /// </summary>
        /// <returns>Hashtable having key - value pair of extended properties</returns>
        public Hashtable GetExtendedProperties()
        {
            return this.ExtendedProperties;
        }

        #endregion Get Methods

        #region Filter lookup rows methods

        /// <summary>
        /// Filter lookup rows for the given column name with provided searchValue
        /// </summary>
        /// <param name="columnName">Name of the column on which we have to filter</param>
        /// <param name="searchOperator">Search operator which is selected</param>
        /// <param name="filterValue">Value which has to be filered</param>
        /// <param name="valueSeparator">Separator of the value</param>
        /// <returns></returns>
        public IRowCollection Filter(String columnName, SearchOperator searchOperator, String filterValue, String valueSeparator = "")
        {
            StringComparison comparisionType = StringComparison.InvariantCulture;
            return Filter(columnName, searchOperator, filterValue, comparisionType, valueSeparator);
        }

        /// <summary>
        /// Filter lookup rows for the given column name with provided searchValue
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="searchOperator"></param>
        /// <param name="filterValue"></param>
        /// <param name="comparisionType"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        public IRowCollection Filter(String columnName, SearchOperator searchOperator, String filterValue, StringComparison comparisionType, String valueSeparator = "")
        {
            Int32 cellNumber = -1;

            if (!this.Columns.Contains(columnName))
            {
                throw new ArgumentException(String.Format("Table: Filter: Provided column name '{0}' does not exist in Table '{1}'", columnName, this.Name));
            }

            RowCollection rowCollection = new RowCollection();
            
            foreach (Row row in this.Rows)
            {
                if (cellNumber == -1)
                {
                    Int32 position = 0;
                    foreach (Cell cell in row.Cells)
                    {
                        if (String.Compare(columnName, cell.ColumnName, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            cellNumber = position;
                            break;
                        }
                        position++;
                    }
                }

                Boolean isMatched = row.CompareValue(cellNumber, searchOperator, filterValue, comparisionType, valueSeparator);

                if (isMatched)
                    rowCollection.Add(row);
            }

            return rowCollection;
        }

        /// <summary>
        /// Filter lookup rows for the given column name with provided searchValue
        /// </summary>
        /// <param name="lookupSearchRuleCollection">lookupSearchRuleCollection object having all search parameters</param>
        /// <returns></returns>
        public IRowCollection Filter(LookupSearchRuleCollection lookupSearchRuleCollection)
        {
            RowCollection filteredRow = new RowCollection();
            RowCollection singleRuleFilteredRowCollection = new RowCollection();
            RowCollection originalRows = null;

            if (lookupSearchRuleCollection != null && lookupSearchRuleCollection.Count > 0)
            {
                foreach (LookupSearchRule lookupSearchRule in lookupSearchRuleCollection)
                {
                    SearchOperator searchOperator = SearchOperator.None;

                    switch (lookupSearchRule.LookupSearchOperator)
                    {
                        case LookupSearchOperatorEnum.EqualTo: 
                            searchOperator = MDM.Core.SearchOperator.EqualTo;
                            break;

                        case LookupSearchOperatorEnum.Contains: 
                            searchOperator = MDM.Core.SearchOperator.Contains;
                            break;

                        case LookupSearchOperatorEnum.NotContains: 
                            searchOperator = MDM.Core.SearchOperator.NotContains;
                            break;
                    }

                    if (singleRuleFilteredRowCollection != null && singleRuleFilteredRowCollection.Count > 0 && lookupSearchRuleCollection.GroupOperator.ToLowerInvariant() == "and")
                    {
                        //Clone the original lookup values before modifying.
                        if (originalRows == null || originalRows.Count <= 0)
                        {
                            originalRows = this.Rows;
                        }

                        this.Rows = singleRuleFilteredRowCollection;
                    }

                    singleRuleFilteredRowCollection = (RowCollection)Filter(lookupSearchRule.SearchColumnName, searchOperator, lookupSearchRule.SearchValue, StringComparison.InvariantCultureIgnoreCase);

                    if (lookupSearchRuleCollection.GroupOperator.ToLowerInvariant() == "or")
                    {
                        foreach (Row singleRuleFilredRow in singleRuleFilteredRowCollection)
                        {
                            if (filteredRow.GetRow(singleRuleFilredRow.Id) == null)
                                filteredRow.Add(singleRuleFilredRow);
                        }
                    }
                    else
                    {
                        filteredRow = singleRuleFilteredRowCollection;
                    }
                }
            }

            //Assign original values back for the next time.
            if (originalRows != null && originalRows.Count > 0 && originalRows.Count != this.Rows.Count)
            {
                this.Rows = originalRows;
            }

            return filteredRow;
        }

        /// <summary>
        /// Filter lookup rows for the given column name with provided searchValue
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public IRowCollection Filter(String filterExpression)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ToSystemDataTable

        /// <summary>
        /// Convert Table to System DataTable
        /// </summary>
        /// <returns>Returns the DataTable equivalent of Table</returns>
        public DataTable ToSystemDataTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = this.Name;

            if (this.Columns != null && this.Columns.Count > 0)
            {
                //Add the columns from Table to DataTable
                foreach (Column column in this.Columns)
                {
                    dataTable.Columns.Add(column.Name);
                }

                if (this.Rows != null && this.Rows.Count > 0)
                {
                    //Loop through all the rows 
                    foreach (Row row in this.Rows)
                    {
                        DataRow dr = dataTable.NewRow();

                        //Loop through all the cell in a particular row
                        foreach (Cell cell in row.Cells)
                        {
                            if (cell.Value != null)
                            {
                                dr[cell.ColumnName] = cell.Value.ToString();
                            }
                        }

                        //Add data row in data table
                        dataTable.Rows.Add(dr);
                    }
                }
            }

            return dataTable;
        }

        #endregion

        #endregion ITable Methods

        #region Private Methods

        

        #pragma warning disable 1570

        /// <summary>
        /// Read ExtendedProperties from Xml and populate NameValueCollection of those properties
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having value
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <ExtendedProperties>
        ///     <ExtendedProperty Key="AttributeId">
        ///         <![CDATA[3006]]>
        ///     </ExtendedProperty>
        /// </ExtendedProperties>
        /// ]]>
        /// </para>
        /// </param>
        /// <returns>Extended properties in form of NameValueCollection</returns>

        #pragma warning restore 1570

        private Hashtable ReadExtendedProperties(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <ExtendedProperties>
                    <ExtendedProperty Key="AttributeId"><![CDATA[3006]]></ExtendedProperty>
               </ExtendedProperties>
             */
            #endregion Sample Xml

            Hashtable prop = null;
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    prop = new Hashtable();
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperty")
                        {
                            if (reader.HasAttributes == true)
                            {
                                String key = String.Empty;
                                Object value = null;

                                if (reader.GetAttribute("Key") != null)
                                {
                                    key = reader.GetAttribute("Key");
                                }
                                value = reader.ReadElementContentAsObject();

                                if (prop.Contains(key) == false && value != null)
                                {
                                    prop.Add(key, value);
                                }
                            }
                        }
                        reader.Read();
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
            return prop;
        }

        #endregion Private Methods
    }
}
