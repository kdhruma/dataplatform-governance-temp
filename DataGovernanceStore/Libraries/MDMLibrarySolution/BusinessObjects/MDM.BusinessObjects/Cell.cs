using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class denoting the Cell for a DataTable. This is a placeholder where we can store the values.
    /// </summary>
    [DataContract]
    public class Cell : ObjectBase, ICell
    {
        #region Fields

        /// <summary>
        /// Field denoting column Id for which the cell is. Row has collection of cells and it stores value for each column
        /// </summary>
        private Int32 _columnId = -1;

        /// <summary>
        /// Field denoting column Name for which the cell is. Row has collection of cells and it stores value for each column
        /// </summary>
        private String _columnName = String.Empty;

        /// <summary>
        /// Field denoting value for given row and column
        /// </summary>
        private Object _value = null;

        /// <summary>
        /// Field used to store extra information for cell
        /// </summary>
        private Hashtable _extendedProperties = null;

        /// <summary>
        /// Field denoting whether the cell value is the system locale value
        /// </summary>
        private Boolean _isSystemLocaleValue = false;

        /// <summary>
        /// Field denoting system locale value
        /// </summary>
        private Object _systemLocaleValue = null;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Cell()
            : base()
        {
        }

        /// <summary>
        /// Constructor with columnId, columnName and value as parameter
        /// </summary>
        /// <param name="columnId">Id of column</param>
        /// <param name="columnName">Name of column</param>
        /// <param name="value">Data for given row and column</param>
        public Cell(Int32 columnId, String columnName, Object value)
        {
            this.ColumnId = columnId;
            this.ColumnName = columnName;
            this.Value = value;
        }

        /// <summary>
        /// Constructor with columnId, columnName parameter
        /// </summary>
        /// <param name="columnId">Id of column</param>
        /// <param name="columnName">Name of column</param>
        public Cell(Int32 columnId, String columnName)
        {
            this.ColumnId = columnId;
            this.ColumnName = columnName;
        }

        /// <summary>
        /// Constructor with column object as input parameter
        /// </summary>
        /// <param name="column">Column object</param>        
        /// <exception cref="ArgumentNullException">Thrown If column is null</exception>
        /// <exception cref="Exception">Thrown if column.Id is less than 0 or Column.Name is not provided</exception>
        public Cell(Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column", "Column cannot be null");
            }

            //if ( column.Id <= 0 )
            //{
            //    throw new Exception("Column.Id cannot be less than zero");
            //}

            if (String.IsNullOrWhiteSpace(column.Name))
            {
                throw new Exception("Column.Name is not provided");
            }

            this.ColumnId = column.Id;
            this.ColumnName = column.Name;
        }


#pragma warning disable 1570

        /// <summary>
        /// Populate cell from Xml
        /// </summary>        
        /// <param name="valuesAsXml">
        /// Xml having value of cell
        /// <para>       
        /// Sample Xml: 
        /// <![CDATA[
        /// <Cell ColumnId="127" ColumnName="Value"><![CDATA[]]></Cell>
        /// ]]>
        /// </para>
        /// </param>

#pragma warning restore 1570

        public Cell(String valuesAsXml)
        {
            LoadCell(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting column Id for which the cell is. Row has collection of cells and it stores value for each column
        /// </summary>
        [DataMember]
        public Int32 ColumnId
        {
            get
            {
                return _columnId;
            }
            set
            {
                _columnId = value;
            }
        }

        /// <summary>
        /// Property denoting column Name for which the cell is. Row has collection of cells and it stores value for each column
        /// </summary>
        [DataMember]
        public String ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        /// <summary>
        /// Property denoting value for given row and column
        /// </summary>
        [DataMember]
        public Object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Property denoting whether the cell value is the system locale value
        /// </summary>
        [DataMember]
        public Boolean IsSystemLocaleValue
        {
            get
            {
                return _isSystemLocaleValue;
            }
            set
            {
                _isSystemLocaleValue = value;
            }
        }

        /// <summary>
        /// Property used to store extra information for cell
        /// </summary>
        [DataMember]
        public Hashtable ExtendedProperties
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
        /// Property denoting system locale value
        /// </summary>
        [DataMember]
        public Object SystemLocaleValue
        {
            get
            {
                return _systemLocaleValue;
            }
            set
            {
                _systemLocaleValue = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents Cell in Xml format
        /// </summary>
        /// <returns>String representing Cell Xml</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Cell");

            #region write Cell meta data for Full Xml

            xmlWriter.WriteAttributeString("ColumnId", this.ColumnId.ToString());
            xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);

            if (this.Value != null && !String.IsNullOrEmpty(this.Value.ToString()))
            {
                xmlWriter.WriteCData(this.Value.ToString());
            }

            #endregion write Cell meta data for Full Xml

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

            //Cell node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents Cell in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Serialization option for Cell object</param>
        /// <returns>String representing Cell Xml</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml(objectSerialization, true);
        }

        /// <summary>
        /// Represents Cell in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Serialization option for Cell object</param>
        /// <param name="populateSDLValue">Flag to decide whether to populate system data locale value or not. By default
        /// value is true. This flag only used in External ObjectSerialization</param>
        /// <param name="useColumnNamesAsTag">Flag to decide whether the cell object tag start with column name or not.
        /// This flag used only in the case of UIRender ObjectSerialization</param>
        /// <returns>String representing Cell Xml</returns>
        public String ToXml(ObjectSerialization objectSerialization, Boolean populateSDLValue = true, Boolean useColumnNamesAsTag = false)
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

                if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write Cell meta data for Processing Xml

                    xmlWriter.WriteStartElement("Cell");

                    xmlWriter.WriteAttributeString("ColumnId", this.ColumnId.ToString());
                    xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
                    if (this.Value != null)
                    {
                        xmlWriter.WriteCData(this.Value.ToString());
                    }

                    //Cell node end
                    xmlWriter.WriteEndElement();

                    #endregion write Cell meta data for Full Xml
                }
                else if (objectSerialization == ObjectSerialization.UIRender)
                {
                    #region write Cell meta data for UIRendering Xml

                    if (useColumnNamesAsTag)
                    {
                        xmlWriter.WriteStartElement(this.ColumnName);
                    }
                    else
                    {
                        xmlWriter.WriteStartElement("Cell");
                    }

                    xmlWriter.WriteAttributeString("ColumnId", this.ColumnId.ToString());
                    xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
                    xmlWriter.WriteAttributeString("IsSystemLocaleValue", this.IsSystemLocaleValue.ToString());

                    if (this.Value != null)
                    {
                        xmlWriter.WriteCData(this.Value.ToString());
                    }

                    //Cell node end
                    xmlWriter.WriteEndElement();

                    #endregion write Cell meta data for UIRendering Xml
                }
                else if (objectSerialization == ObjectSerialization.External)
                {
                    #region write Cell meta data for External Xml

                    xmlWriter.WriteStartElement("Cell");

                    xmlWriter.WriteAttributeString("ColumnId", this.ColumnId.ToString());
                    xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
                    xmlWriter.WriteAttributeString("IsSystemLocaleValue", this.IsSystemLocaleValue.ToString());

                    if (this.Value != null && (populateSDLValue || !this.IsSystemLocaleValue)) //Do populate the cell value, Always if populateSDLValue is true or if system locale value is false and value is not a system locale value
                    {
                        xmlWriter.WriteCData(this.Value.ToString());
                    }

                    //Cell node end
                    xmlWriter.WriteEndElement();

                    #endregion write Cell meta data for External Xml
                }

                xmlWriter.Flush();

                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return xml;

        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is Cell)
                {
                    Cell objectToBeCompared = obj as Cell;

                    if (this.ColumnId != objectToBeCompared.ColumnId)
                        return false;

                    if (this.Value != objectToBeCompared.Value)
                        return false;

                    if (this.IsSystemLocaleValue != objectToBeCompared.IsSystemLocaleValue)
                        return false;

                    if (this.SystemLocaleValue != objectToBeCompared.SystemLocaleValue)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.ColumnId.GetHashCode() ^ this.Value.GetHashCode() ^ this.IsSystemLocaleValue.GetHashCode() ^ this.SystemLocaleValue.GetHashCode();

            return hashCode;
        }


#pragma warning disable 1570

        /// <summary>
        /// Populate cell from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having value of cell
        /// <para>
        /// Sample Xml: 
        /// <![CDATA[
        /// <Cell ColumnId="127" ColumnName="Value"><![CDATA[]]></Cell>
        /// ]]>
        /// </para>
        /// </param>

#pragma warning restore 1570

        public void LoadCell(String valuesAsXml)
        {
            #region Sample Xml

            /*
             * <Cell ColumnId="127" ColumnName="Value"><![CDATA[]]></Cell>
             */

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Cell" && reader.HasAttributes)
                    {
                        #region Read Cell Properties

                        if (reader.GetAttribute("ColumnId") != null)
                        {
                            this.ColumnId = ValueTypeHelper.Int32TryParse(reader.GetAttribute("ColumnId"), 0);
                        }
                        if (reader.GetAttribute("ColumnName") != null)
                        {
                            this.ColumnName = reader.GetAttribute("ColumnName");
                        }
                        this.Value = reader.ReadElementContentAsString();

                        #endregion Read Cell Properties
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Cell Clone()
        {
            Cell clonedCell = new Cell();
            clonedCell._columnId = this._columnId;
            clonedCell._columnName = this._columnName;
            clonedCell._extendedProperties = this._extendedProperties;
            clonedCell._value = this._value;
            clonedCell._systemLocaleValue = this._systemLocaleValue;
            clonedCell._isSystemLocaleValue = this._isSystemLocaleValue;

            return clonedCell;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetCell">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparission is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Cell subsetCell, Boolean compareIds = false)
        {
            if (subsetCell != null)
            {
                if (compareIds)
                {
                    if (this.ColumnId != subsetCell.ColumnId)
                        return false;
                }

                if (this.ColumnName != subsetCell.ColumnName)
                    return false;

                if (this.Value != null)
                {
                    if (subsetCell.Value == null || String.Compare(this.Value.ToString(), subsetCell.Value.ToString()) != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (subsetCell.Value != null)
                    {
                        return false;
                    }
                }

                if (this.IsSystemLocaleValue != subsetCell.IsSystemLocaleValue)
                    return false;

                if (this.SystemLocaleValue != subsetCell.SystemLocaleValue)
                    return false;
            }

            return true;
        }

        #endregion Methods
    }
}
