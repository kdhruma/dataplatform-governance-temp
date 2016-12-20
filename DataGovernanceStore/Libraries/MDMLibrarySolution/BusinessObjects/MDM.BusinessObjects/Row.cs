using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents Row collection for table
    /// </summary>
    [DataContract]
    [KnownType(typeof(Hashtable))]
    public class Row : MDMObject, IRow
    {
        #region Fields

        /// <summary>
        /// Field denoting Id of row.
        /// </summary>
        private Int64 _id = 0;

        /// <summary>
		/// Field denoting cells in current row. Cell is the intersection point of row and column where we store the value
		/// </summary>
		private CellCollection _cells = new CellCollection();

        /// <summary>
        /// Field denoting whether row is having system locale values
        /// </summary>
        private Boolean _isSystemLocaleRow = false;

        /// <summary>
        /// Field used to store extra information for cell
        /// </summary>
        private Hashtable _extendedProperties = null;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Row()
              : base()
        {
        }

        /// <summary>
        /// Id based Constructor
        /// </summary>
        public Row(Int64 id)
            : base()
        {
            this.Id = id;
        }

        /// <summary>
        /// Load Row from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml representing row value
        /// <para>
        /// Sample Xml :
        /// <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
        ///     <Cells>
        ///         <Cell ColumnId="0" ColumnName="LookupKey"><![CDATA[101 Dalmations           ]]></Cell>
        ///         <Cell ColumnId="0" ColumnName="Value"><![CDATA[101 Dalmations           ]]></Cell>
        ///         <Cell ColumnId="0" ColumnName="Code"><![CDATA[101 Dalmations           ]]></Cell>
        ///     </Cells>
        /// </Row>
        /// </para>
        /// </param>
        public Row(String valuesAsXml)
        {
            LoadRow(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Field denoting Id of row.
        /// </summary>
        [DataMember]
        new public Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Cells in current row
        /// </summary>
        [DataMember]
        public CellCollection Cells
        {
            get
            {
                return _cells;
            }
            set
            {
                _cells = value;
            }
        }

        /// <summary>
        /// Property used to store extra information for cell
        /// </summary>
        [DataMember]
        public new Hashtable ExtendedProperties
        {
            get
            {
                return _extendedProperties;
            }
            set
            {
                _extendedProperties = value;
            }
        }

        /// <summary>
        /// Property denoting whether row is having system locale values
        /// </summary>
        [DataMember]
        public Boolean IsSystemLocaleRow
        {
            get
            {
                return _isSystemLocaleRow;
            }
            set
            {
                _isSystemLocaleRow = value;
            }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Get/Set value for current row and given column Id
        /// </summary>
        /// <param name="columnId">Id of column of which value we want to set</param>
        /// <returns>Value of cell at current row and given column Id</returns>
        public Object this[Int32 columnId]
        {
            get
            {
                return this.GetValue(columnId);
            }
            set
            {
                this.SetValue(columnId, value);
            }
        }

        /// <summary>
        /// Get/Set value for current row and given column Name
        /// </summary>
        /// <param name="columnName">Name of column of which value we want to set</param>
        /// <returns>Value of cell at current row and given column Name</returns>
        public Object this[String columnName]
        {
            get
            {
                return this.GetValue(columnName);
            }
            set
            {
                this.SetValue(columnName, value);
            }
        }

        /// <summary>
        /// Get/Set value for current row and given column
        /// </summary>
        /// <param name="column">Column of which value we want to set</param>
        /// <returns>Value of cell at current row and given column</returns>
        public Object this[Column column]
        {
            get
            {
                return this.GetValue(column);
            }
            set
            {
                this.SetValue(column, value);
            }
        }

        #endregion Indexers

        #region Public Methods

        /// <summary>
        /// Load Row from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml representing row value
        /// <para>
        /// Sample Xml :
        /// <Row Id="-1" SearchValue="" DisplayValue="" StorageValue="">
        ///     <Cells>
        ///         <Cell ColumnId="0" ColumnName="LookupKey"><![CDATA[101 Dalmations           ]]></Cell>
        ///         <Cell ColumnId="0" ColumnName="Value"><![CDATA[101 Dalmations           ]]></Cell>
        ///         <Cell ColumnId="0" ColumnName="Code"><![CDATA[101 Dalmations           ]]></Cell>
        ///     </Cells>
        /// </Row>
        /// </para>
        /// </param>
        public void LoadRow(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <Row Id="1" SearchValue="" DisplayValue="" StorageValue="">
             */
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Row" && reader.HasAttributes)
                    {
                        #region Read Row Properties

                        if (reader.MoveToAttribute("Id"))
                        {
                            this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                        }
                        if (reader.MoveToAttribute("Action"))
                        {
                            ObjectAction action = ObjectAction.Read;
                            Enum.TryParse(reader.ReadContentAsString(), true, out action);
                            this.Action = action;
                        }
                        if (reader.MoveToAttribute("IsSystemLocaleRow"))
                        {
                            this.IsSystemLocaleRow = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                        }

                        #endregion Read Row Properties
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                    {
                        String key = String.Empty;
                        String value = String.Empty;

                        if (reader.MoveToAttribute("Key"))
                        {
                            key = reader.ReadContentAsString();
                        }

                        if (reader.MoveToAttribute("Value"))
                        {
                            value = reader.ReadContentAsString();
                        }

                        this.ExtendedProperties.Add(key, value);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Cells")
                    {
                        #region Read cells

                        String cellsXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(cellsXml))
                        {
                            CellCollection cells = new CellCollection(cellsXml);
                            if (cells != null)
                            {
                                foreach (Cell cell in cells)
                                {
                                    this.Cells.Add(cell);
                                }
                            }
                        }

                        #endregion Read cells
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
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetRow">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Row subsetRow, Boolean compareIds = false)
        {
            if (subsetRow != null)
            {
                if (compareIds)
                {
                    if (this.Id != subsetRow.Id)
                        return false;
                }

                if (this.IsSystemLocaleRow != subsetRow.IsSystemLocaleRow)
                    return false;

                if (!this.Cells.IsSuperSetOf(subsetRow.Cells))
                    return false;
            }

            return true;
        }

        #endregion Public Methods

        #region IRow Methods

        #region ToXml

        /// <summary>
		/// Represents Row in Xml format
		/// </summary>
        /// <returns>String representing Row in Xml format</returns>
		public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Row");

            #region write Row meta data for Full Xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("IsSystemLocaleRow", this.IsSystemLocaleRow.ToString());

            #endregion write Row meta data for Full Xml

            #region Write cell attributes

            if (this.ExtendedProperties != null && this.ExtendedProperties.Count > 0)
            {
                xmlWriter.WriteStartElement("Attributes");

                foreach (DictionaryEntry attribute in ExtendedProperties)
                {
                    xmlWriter.WriteStartElement("Attribute");

                    xmlWriter.WriteAttributeString("Key", attribute.Key.ToString());
                    xmlWriter.WriteAttributeString("Value", (attribute.Value == null) ? String.Empty : attribute.Value.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }

            #endregion

            #region write Cells

            xmlWriter.WriteStartElement("Cells");

            if (this.Cells != null)
            {
                foreach (Cell cell in this.Cells)
                {
                    xmlWriter.WriteRaw(cell.ToXml());
                }
            }

            //Overridden Values node end
            xmlWriter.WriteEndElement();

            #endregion write Cells

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
        /// Represents Row in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing Row in Xml format</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml(objectSerialization, true);
        }

        /// <summary>
        /// Represents Row in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <param name="populateSDLValue">Flag to decide whether to populate system data locale value or not. By default
        /// value is true. This flag only used in External ObjectSerialization</param>
        /// <param name="useColumnNamesAsTag">Flag to decide whether the cell object tag start with column name or not.
        /// <param name="exportAuditInfo">Indicates flag whether to export lookup related audit information or not</param>
        /// This flag used only in the case of UIRender ObjectSerialization</param>
        /// <returns>String representing Row in Xml format</returns>
        public String ToXml(ObjectSerialization objectSerialization, Boolean populateSDLValue = true, Boolean useColumnNamesAsTag = false, Boolean exportAuditInfo = false)
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

                xmlWriter.WriteStartElement("Row");

                #region write Row meta data for Processing/UIRendering Xml

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("IsSystemLocaleRow", this.IsSystemLocaleRow.ToString());

                #endregion write Row meta data for Full Xml

                #region Write cell attributes

                if (this.ExtendedProperties != null && this.ExtendedProperties.Count > 0)
                {
                    if (!exportAuditInfo)
                    {
                        xmlWriter.WriteStartElement("Attributes");

                        foreach (DictionaryEntry attribute in ExtendedProperties)
                        {
                            xmlWriter.WriteStartElement("Attribute");

                            xmlWriter.WriteAttributeString("Key", attribute.Key.ToString());
                            xmlWriter.WriteAttributeString("Value", (attribute.Value == null) ? String.Empty : attribute.Value.ToString());

                            xmlWriter.WriteEndElement();
                        }

                        xmlWriter.WriteEndElement();
                    }
                    else
                    {
                        foreach (DictionaryEntry attribute in ExtendedProperties)
                        {
                            xmlWriter.WriteAttributeString(attribute.Key.ToString(), (attribute.Value == null) ? String.Empty : attribute.Value.ToString());
                        }
                    }
                }

                #endregion

                if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write Cells

                    xmlWriter.WriteStartElement("Cells");

                    if (this.Cells != null)
                    {
                        foreach (Cell cell in this.Cells)
                        {
                            xmlWriter.WriteRaw(cell.ToXml(objectSerialization));
                        }
                    }

                    //Overridden Values node end
                    xmlWriter.WriteEndElement();

                    #endregion write Cells
                }
                else if (objectSerialization == ObjectSerialization.UIRender)
                {
                    #region write Cells

                    xmlWriter.WriteStartElement("Cells");

                    if (this.Cells != null)
                    {
                        foreach (Cell cell in this.Cells)
                        {
                            xmlWriter.WriteRaw(cell.ToXml(objectSerialization, populateSDLValue, useColumnNamesAsTag));
                        }
                    }

                    //Overridden Values node end
                    xmlWriter.WriteEndElement();

                    #endregion write Cells
                }
                else if (objectSerialization == ObjectSerialization.External)
                {
                    #region write ColumnName

                    xmlWriter.WriteStartElement("Cells");

                    if (this.Cells != null)
                    {
                        foreach (Cell cell in this.Cells)
                        {
                            xmlWriter.WriteRaw(cell.ToXml(objectSerialization, populateSDLValue));
                        }
                    }

                    xmlWriter.WriteEndElement();

                    #endregion write ColumnName
                }

                //Row node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return xml;

        }

        #endregion ToXml

        #region Set values

        /// <summary>
        /// Set value for given column in current row
        /// </summary>
        /// <param name="column">Column for which value is to be added</param>
        /// <param name="value">Value to set for current Row and Column</param>
        /// <exception cref="ArgumentNullException">Thrown if column is null</exception>
        /// <exception cref="ArgumentException">thrown if Column.Name is not provided</exception>
        /// <exception cref="Exception">Thrown if there is no cell available for given Column.Id and Column.Name</exception>
        public void SetValue(Column column, Object value)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }

            //if ( column != null && column.Id <= 0 )
            //{
            //    throw new ArgumentException("Column.Id cannot be less than zero"); ;
            //}

            if (column != null && String.IsNullOrWhiteSpace(column.Name))
            {
                throw new ArgumentException("Column.Name cannot be null/empty"); ;
            }

            Cell cell = this.Cells.GetCell(column);
            if (cell == null)
            {
                throw new Exception(String.Concat("No cell found for column Id = ", column.Id, " and column name = ", column.Name, ". Please verify table model."));
            }

            if (value != DBNull.Value)
            {
                cell.Value = value;
            }
        }

        /// <summary>
        /// Set value for given column id in current row
        /// </summary>
        /// <param name="columnId">Column id for which value is to be added</param>
        /// <param name="value">Value to set for current Row and Column id</param>
        /// <exception cref="Exception">Thrown if there is no cell available for given ColumnId</exception>
        public void SetValue(Int32 columnId, Object value)
        {
            Cell cell = this.Cells.GetCell(columnId);
            if (cell == null)
            {
                throw new Exception(String.Concat("No cell found for column Id = ", columnId, ". Please verify table model."));
            }

            if (value != DBNull.Value)
            {
                cell.Value = value;
            }
        }

        /// <summary>
        /// Set value for given column Name in current row
        /// </summary>
        /// <param name="columnName">Column Name for which value is to be added</param>
        /// <param name="value">Value to set for current Row and Column id</param>
        /// <exception cref="Exception">Thrown if there is no cell available for given ColumnName</exception>
        public void SetValue(String columnName, Object value)
        {
            Cell cell = this.Cells.GetCell(columnName);
            if (cell == null)
            {
                throw new Exception(String.Concat("No cell found for column Name = ", columnName, ". Please verify table model."));
            }

            if (value != DBNull.Value)
            {
                cell.Value = value;
            }
        }

        #endregion Set values

        #region Get values

        /// <summary>
        /// Get value for given column in current row
        /// </summary>
        /// <param name="column">Column for which value is to be fetched</param>
        /// <returns>Value at current row and given column</returns>
        /// <exception cref="ArgumentNullException">Thrown if Column is null</exception>
        /// <exception cref="ArgumentException">Thrown if Column.Id is less than 0 or Column.Name is not provided</exception>
        public Object GetValue(Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }

            //if ( column != null && column.Id <= 0 )
            //{
            //    throw new ArgumentException("Column.Id cannot be less than zero"); ;
            //}

            if (column != null && String.IsNullOrWhiteSpace(column.Name))
            {
                throw new ArgumentException("Column.Name cannot be null/empty"); ;
            }

            Object returnValue = null;

            Cell cell = this.Cells.GetCell(column);
            if (cell != null)
            {
                returnValue = cell.Value;
            }
            return returnValue;
        }

        /// <summary>
        /// Get value for given column in current row
        /// </summary>
        /// <param name="columnId">Column id for which value is to be fetched</param>
        /// <returns>Value at current row and given column id</returns>
        public Object GetValue(Int32 columnId)
        {
            Object returnValue = null;
            Cell cell = this.Cells.GetCell(columnId);
            if (cell != null)
            {
                returnValue = cell.Value;
            }
            return returnValue;
        }

        /// <summary>
        /// Get value for given column in current row
        /// </summary>
        /// <param name="columnName">Column Name for which value is to be fetched</param>
        /// <returns>Value at current row and given column Name</returns>
        public Object GetValue(String columnName)
        {
            Object returnValue = null;
            Cell cell = this.Cells.GetCell(columnName);
            if (cell != null)
            {
                returnValue = cell.Value;
            }
            return returnValue;
        }

        #endregion Get values

        /// <summary>
        /// Gets cell for the requested column Id
        /// </summary>
        /// <param name="columnId">Column Id of the cell</param>
        /// <returns>Cell object</returns>
        public Cell GetCell(Int32 columnId)
        {
            return this.Cells.GetCell(columnId);
        }

        /// <summary>
        /// Gets cell for the requested column name
        /// </summary>
        /// <param name="columnName">Column Name of the cell</param>
        /// <returns>Cell object</returns>
        public Cell GetCell(String columnName)
        {
            return this.Cells.GetCell(columnName);
        }

        /// <summary>
        /// Compares row for given column, search operator and filter value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="searchOperator"></param>
        /// <param name="filterValue"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        public Boolean CompareValue(String columnName, SearchOperator searchOperator, String filterValue, String valueSeparator = "")
        {
            StringComparison comparisionType = StringComparison.InvariantCulture;
            return CompareValue(columnName, searchOperator, filterValue, comparisionType, valueSeparator);
        }


        /// <summary>
        /// Compares row for the given cell number, search operator and filter value
        /// </summary>
        /// <param name="cellNumber"></param>
        /// <param name="searchOperator"></param>
        /// <param name="filterValue"></param>
        /// <param name="comparisionType"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        public Boolean CompareValue(Int32 cellNumber, SearchOperator searchOperator, String filterValue, StringComparison comparisionType, String valueSeparator = "")
        {
            Cell cell = this.Cells.ElementAt(cellNumber);

            Object objValue = cell.Value;

            String columnName = cell.ColumnName;
            String value = objValue == null ? String.Empty : objValue.ToString();

            if (columnName == Constants.LOOKUP_CONTEXT_CONTAINER_ID_LIST_COLUMN_NAME || columnName == Constants.LOOKUP_CONTEXT_ORGANIZATION_ID_LIST_COLUMN_NAME || columnName == Constants.LOOKUP_CONTEXT_CATEGORY_PATH_LIST_COLUMN_NAME)
            {
                if (String.IsNullOrEmpty(value) || value == "0")
                    return true;
            }

            return InternalCompareValue(value, searchOperator, filterValue, comparisionType, valueSeparator);
        }

        /// <summary>
        /// Compares row for given column, search operator and filter value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="searchOperator"></param>
        /// <param name="filterValue"></param>
        /// <param name="comparisionType"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        public Boolean CompareValue(String columnName, SearchOperator searchOperator, String filterValue, StringComparison comparisionType, String valueSeparator = "")
        {
            Object objValue = this.GetValue(columnName);
            String value = objValue == null ? String.Empty : objValue.ToString();

            if (columnName == Constants.LOOKUP_CONTEXT_CONTAINER_ID_LIST_COLUMN_NAME || columnName == Constants.LOOKUP_CONTEXT_ORGANIZATION_ID_LIST_COLUMN_NAME || columnName == Constants.LOOKUP_CONTEXT_CATEGORY_PATH_LIST_COLUMN_NAME)
            {
                if (String.IsNullOrEmpty(value) || value == "0")
                    return true;
            }

            return InternalCompareValue(value, searchOperator, filterValue, comparisionType, valueSeparator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Row Clone()
        {
            Row clonedRow = new Row();
            clonedRow.Cells = this._cells.Clone();
            clonedRow._extendedProperties = this._extendedProperties;
            clonedRow._id = this._id;
            clonedRow.Action = this.Action;
            clonedRow._isSystemLocaleRow = this._isSystemLocaleRow;

            clonedRow.Name = this.Name;
            clonedRow.LongName = this.LongName;
            clonedRow.Locale = this.Locale;
            clonedRow.AuditRefId = this.AuditRefId;
            clonedRow.ProgramName = this.ProgramName;
            clonedRow.UserName = this.UserName;

            return clonedRow;
        }

        #endregion IRow Methods

        #region Internal Methods

        /// <summary>
        /// Add cell to current row. This method is not available for access other than table. 
        /// If user wants to add a row, He need to CallTable.NewRow(). This row will have structure defined.
        /// </summary>
        /// <param name="column">Column for which cell is to be added.</param>
        internal void AddCell(Column column)
        {
            this.AddCell(column, true);
        }

        /// <summary>
        /// Add cell to current row. This method is not available for access other than table. 
        /// If user wants to add a row, He need to CallTable.NewRow(). This row will have structure defined.
        /// </summary>
        /// <param name="column">Column for which cell is to be added.</param>
        /// <param name="populateDefaultValue">Indicates if we have to populate default values or not</param>
        internal void AddCell(Column column, Boolean populateDefaultValue)
        {
            if (this.Cells == null)
            {
                this.Cells = new CellCollection();
            }
            this.Cells.Add(new Cell(column));
            //Set the default value
            if (populateDefaultValue == true)
            {
                this.SetValue(column, column.DefaultValue);
            }
        }

        /// <summary>
        /// Compares row for given column, search operator and filter value
        /// </summary>
        /// <param name="value">Indicates the value to be compared</param>
        /// <param name="searchOperator">Indicates the search operator used for comparison</param>
        /// <param name="filterValue">Indicates the filtered value which is compared with the value</param>
        /// <param name="comparisionType">Indicates the type of string comparison to be done</param>
        /// <param name="valueSeparator">Indicates the value seperator</param>
        /// <returns>Returns true if the value matches; else return false</returns>
        internal Boolean InternalCompareValue(String value, SearchOperator searchOperator, String filterValue, StringComparison comparisionType, String valueSeparator)
        {
            Boolean isMatched = false;

            String concatenatedFilterValue = filterValue;

            if (!String.IsNullOrWhiteSpace(valueSeparator))
            {
                concatenatedFilterValue = String.Concat(valueSeparator, filterValue, valueSeparator);
                value = String.Concat(valueSeparator, value, valueSeparator);
            }

            switch (searchOperator)
            {
                case SearchOperator.EqualTo:
                    if (String.Compare(value, concatenatedFilterValue, comparisionType) == 0)
                        isMatched = true;
                    break;
                case SearchOperator.Contains:
                    if (comparisionType == StringComparison.InvariantCultureIgnoreCase)
                    {
                        if (value.ToLowerInvariant().Contains(concatenatedFilterValue.ToLowerInvariant()))
                        {
                            isMatched = true;
                        }
                    }
                    else if (comparisionType == StringComparison.CurrentCultureIgnoreCase)
                    {
                        if (value.ToLower().Contains(concatenatedFilterValue.ToLower()))
                        {
                            isMatched = true;
                        }
                    }
                    else if (value.Contains(concatenatedFilterValue))
                        isMatched = true;
                    break;
                case SearchOperator.NotContains:
                    if (comparisionType == StringComparison.InvariantCultureIgnoreCase)
                    {
                        if (!(value.ToLowerInvariant().Contains(concatenatedFilterValue.ToLowerInvariant())))
                        {
                            isMatched = true;
                        }
                    }
                    else if (comparisionType == StringComparison.CurrentCultureIgnoreCase)
                    {
                        if (!(value.ToLower().Contains(concatenatedFilterValue.ToLower())))
                        {
                            isMatched = true;
                        }
                    }
                    else if (!value.Contains(concatenatedFilterValue))
                        isMatched = true;
                    break;
                case SearchOperator.SubsetOf:
                    {
                        String[] splitedValues = null;
                        if (valueSeparator.Length > 0)
                        {
                            splitedValues = value.Split(new string[] { valueSeparator }, StringSplitOptions.RemoveEmptyEntries);

                            int i = 0;

                            foreach (string s in splitedValues)
                                splitedValues[i] = valueSeparator + splitedValues[i++];
                        }
                        else
                        {
                            splitedValues = new string[] { value };
                        }

                        if (splitedValues != null && splitedValues.Length > 0)
                        {
                            //This fix is done only for the Husky Customers
                            if (concatenatedFilterValue.Contains(",") || concatenatedFilterValue.Contains("%2C"))
                            {
                                String filteredValue = concatenatedFilterValue.ToLower().Replace("%2c", "").Replace("%20", "").Replace("|", "").Replace(",", "").Replace(" ", "");

                                foreach (String categoryPath in splitedValues)
                                {
                                    String formattedCategoryPath = categoryPath.ToLower().Replace("|", "").Replace(",", "").Replace("%2", "").Replace("%2c", "").Replace(" ", "").Replace("%20", "");

                                    if (categoryPath.Contains(",") && filteredValue.Equals(formattedCategoryPath))
                                    {
                                        return true; // Value matched
                                    }
                                }

                                return false;
                            }
                            //End of Husky Fix
                            else
                            {
                                foreach (string val in splitedValues)
                                {
                                    if (concatenatedFilterValue.StartsWith(val, comparisionType))
                                        isMatched = true;
                                }
                            }
                        }
                        break;
                    }
                default:
                    throw new NotSupportedException(String.Format("Table: Filter: Provide search operator '{0}' is not supported", searchOperator.ToString()));
            }

            return isMatched;
        }

        #endregion Internal Methods
    }
}
