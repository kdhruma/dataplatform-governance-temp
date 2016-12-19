using System;
using System.Collections;
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
    /// Specifies class for Dependent Attribute Data Map
    /// </summary>
    public class DependentAttributeDataMap : Table, IDependentAttributeDataMap
    {
        #region Fields

        #endregion Fields

        #region Constructor

        /// <summary>
		/// Parameterless Constructor
		/// </summary>
        public DependentAttributeDataMap()
			: base()
		{
		}

        /// <summary>
		/// Constructor with Id, Name and LongName of a Table as input parameters
		/// </summary>
		/// <param name="id">Indicates the Identity of a Table (RSTObjectId)</param>
		/// <param name="name">Indicates the Name of a Table </param>
		/// <param name="longName">Indicates the LongName of a Table </param>
        public DependentAttributeDataMap(Int32 id, String name, String longName)
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

        public DependentAttributeDataMap(String valuesAsXml)
		{
			LoadDependentAttributeDataMap(valuesAsXml);
		}

		/// <summary>
		/// Load table from Datatable
		/// </summary>
		/// <param name="dsModelAndData">Datatable which is to be converted into MDM.BusinessObject.Table</param>
        public DependentAttributeDataMap(DataSet dsModelAndData)
        {
            LoadDependentAttributeDataMap(dsModelAndData);
        }

        #endregion Constructor

        #region Properties

        #endregion Properties

        #region ToXml Methods

        /// <summary>
        /// Represents Table in Xml format
        /// </summary>
        /// <returns>String representing Table in Xml format</returns>
        new public String ToXml()
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
                    xmlWriter.WriteRaw(column.ToXml());
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
                    xmlWriter.WriteRaw(row.ToXml());
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
        /// <returns>String representing Table in Xml format</returns>
        new public String ToXml(ObjectSerialization objectSerialization)
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
                Int32 localeId = (Int32)this.Locale;

                xmlWriter.WriteStartElement("Table");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("LongName", this.LongName);
                xmlWriter.WriteAttributeString("Locale", localeId.ToString());

                if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write TableRows

                    if (this.Rows != null)
                    {
                        foreach (Row row in this.Rows)
                        {
                            xmlWriter.WriteStartElement("Columns");

                            xmlWriter.WriteAttributeString("Id", row.Id.ToString());
                            xmlWriter.WriteAttributeString("Action", row.Action.ToString());

                            if (row.Cells != null && row.Cells.Count>0)
                            {
                                foreach (Cell cell in row.Cells)
                                {
                                    xmlWriter.WriteStartElement("Column");

                                    xmlWriter.WriteAttributeString("Name", cell.ColumnName);

                                    #region Write CDATA

                                    Boolean isSystemLocaleValue = cell.IsSystemLocaleValue;

                                    if (isSystemLocaleValue == false)
                                    {
                                        if (cell.Value != null)
                                            xmlWriter.WriteCData(cell.Value.ToString());
                                        else
                                            xmlWriter.WriteCData(null);
                                    }
                                    else
                                        xmlWriter.WriteCData(String.Empty);

                                    #endregion

                                    //Column node end
                                    xmlWriter.WriteEndElement();
                                }
                            }
                            else
                            {
                                xmlWriter.WriteStartElement("Column");
                                xmlWriter.WriteEndElement();
                            }

                            //Columns node end
                            xmlWriter.WriteEndElement();

                        }
                    }

                    #endregion
                }
                else
                {
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
                            xmlWriter.WriteRaw(column.ToXml());
                        }
                    }

                    //Columns node end
                    xmlWriter.WriteEndElement();

                    #endregion write Columns

                    if (objectSerialization == ObjectSerialization.UIRender)
                    {
                        #region write Rows

                        xmlWriter.WriteStartElement("Rows");

                        if (this.Rows != null)
                        {
                            foreach (Row row in this.Rows)
                            {
                                xmlWriter.WriteRaw(row.ToXml());
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
                                xmlWriter.WriteRaw(row.ToXml(ObjectSerialization.External));
                            }
                        }

                        //Rows node end
                        xmlWriter.WriteEndElement();

                        #endregion write Rows
                    }
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetModel()
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
                    xmlWriter.WriteRaw(column.ToXml());
                }
            }

            //Columns node end
            xmlWriter.WriteEndElement();

            #endregion write Columns

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();

            sw.Close();
            return xml;
        }

        #endregion ToXml Methods

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
        ///     <Table Id="2" Name="" LongName="tblk_Color" >
        ///         <ExtendedProperties>
        ///             <ExtendedProperty Key="AttributeId"><![CDATA[3006]]></ExtendedProperty>
        ///         </ExtendedProperties>
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

        public void LoadDependentAttributeDataMap(String valuesAsXml)
        {
            #region Sample Xml

            /*
			 * <Table Id="-1" Name="" LongName="tblk_Pattern">
			 *  <ExtendedProperties>
				  <ExtendedProperty Key="AttributeId"><![CDATA[3006]]></ExtendedProperty>
				</ExtendedProperties>
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
			 */

            #endregion Sample Xml

            #region Sample Xml

            /*
			 * <Table Id="-1" Name="" LongName="tblk_Pattern" AttrtibuteId="3008">
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
			 */

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                    {
                        #region Read table Metadata

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                this.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LongName"))
                            {
                                this.LongName = reader.ReadContentAsString();
                            }
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
        /// Load table from Datatable
        /// </summary>
        /// <param name="dsModelAndData">Datatable which is to be converted into MDM.BusinessObject.Table</param>
        public void LoadDependentAttributeDataMap(DataSet dsModelAndData)
        {
            if (dsModelAndData != null)
            {
                DependentAttributeDataMapCollection dependentAttributeMaps = new DependentAttributeDataMapCollection(dsModelAndData);
                if (dependentAttributeMaps != null)
                {
                    DependentAttributeDataMap currentDependentAttributeMap = dependentAttributeMaps.FirstOrDefault();
                    this.Rows = currentDependentAttributeMap.Rows;
                    this.Columns = currentDependentAttributeMap.Columns;
                    this.ExtendedProperties = currentDependentAttributeMap.ExtendedProperties;
                }
            }
        }

        #endregion Public Methods

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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Property")
                        {
                            if (reader.HasAttributes == true)
                            {
                                String key = String.Empty;
                                String value = String.Empty;

                                if (reader.GetAttribute("Key") != null)
                                {
                                    key = reader.GetAttribute("Key");
                                }
                                value = reader.ReadElementContentAsString();

                                if (prop.Contains(key) == false)
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
