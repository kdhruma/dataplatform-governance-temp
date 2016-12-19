using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class Lookup : Table, ILookup
	{
		#region Fields

		/// <summary>
		/// Field denoting AttributeId for lookup
		/// </summary>
		[DataMember]
		private Int32 _attributeId = 0;

        /// <summary>
        /// Field denoting lookup relationships
        /// </summary>
        [DataMember]
        private LookupRelationshipCollection _lookupRelationships;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        private Boolean _isIndexEnabled = false;

        /// <summary>
        /// Field denotes list lookup display columns.
        /// </summary>
        [DataMember]
        private List<String> _displayColumnsList = new List<String>();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Lookup()
			: base()
		{
		}

		/// <summary>
		/// Constructor with Id, Name and LongName of a Table as input parameters
		/// </summary>
		/// <param name="id">Indicates the Identity of a Table (RSTObjectId)</param>
		/// <param name="name">Indicates the Name of a Table </param>
		/// <param name="longName">Indicates the LongName of a Table </param>
		public Lookup(Int32 id, String name, String longName)
			: base(id, name, longName)
		{
		}

        #pragma warning disable 1570

		/// <summary>
		/// Load table from Xml
		/// </summary>
		/// <param name="valuesAsXml">
		/// Xml having table values
        /// Sample Xml
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
		///                     <Cell ColumnId="3" ColumnName="Code"><![CDATA[000]]></Cell>
		///                     <Cell ColumnId="4" ColumnName="Type"><![CDATA[]]></Cell>
		///                 </Cells>
		///             </Row>
		///         </Rows>
		///     </Table>
		/// 
		/// ]]>
		/// </para>
		/// </param>

        #pragma warning restore 1570

		public Lookup(String valuesAsXml)
		{
			LoadLookup(valuesAsXml);
		}

		/// <summary>
		/// Load table from Datatable
		/// </summary>
		/// <param name="dsModelAndData">Datatable which is to be converted into MDM.BusinessObject.Table</param>
		public Lookup(DataSet dsModelAndData)
		{
			LoadLookup(dsModelAndData);
		}

		#endregion Constructor

		#region Constants

		/// <summary>
		/// 
		/// </summary>
		public const String IdColumnName = "Id";

		/// <summary>
		/// 
		/// </summary>
		public const String ExportFormatColumnName = "ExportFormat";

		/// <summary>
		/// 
		/// </summary>
		public const String DisplayFormatColumnName = "DisplayFormat";

		/// <summary>
		/// 
		/// </summary>
		public const String SearchDataColumnName = "SearchColumns";

		#endregion

		#region Properties

		/// <summary>
		/// Property denoting AttributeId for lookup
		/// </summary>
		public Int32 AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}

        /// <summary>
        /// Property denoting AttributeId for lookup
        /// </summary>
        public LookupRelationshipCollection LookupRelationships
        {
            get
            {
                if (_lookupRelationships == null)
                {
                    _lookupRelationships = new LookupRelationshipCollection();
                }

                return this._lookupRelationships;
            }
            set
            {
                this._lookupRelationships = value;
            }
        }

        /// <summary>
        /// IsIndexEnabled - When true, indexes can be created and used by appropriate Get methods
        /// </summary>
        public Boolean IsIndexEnabled
        {
            get { return _isIndexEnabled; }
            set { _isIndexEnabled = value; }
        }

        /// <summary>
        /// Property denotes list lookup display columns.
        /// </summary>
        [DataMember]
        public List<String> DisplayColumnList
        {
            get { return _displayColumnsList; }
            set { _displayColumnsList = value; }
        }


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
			xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

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

            xmlWriter.WriteStartElement("Relationships");

            if (this.Columns != null)
            {
                foreach (LookupRelationship lookuprelationship in this.LookupRelationships)
                {
                    xmlWriter.WriteRaw(lookuprelationship.ToXml());
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
            return this.ToXml(objectSerialization, true);
        }

        /// <summary>
        /// Represents Table in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <param name="populateSDLValue">Flag to decide whether to populate system data locale value or not. By default
        /// value is true. This flag only used in External ObjectSerialization</param>
        /// <param name="useColumnNamesAsTag">Flag to decide whether the cell object tag start with column name or not.
        /// <param name="exportAuditInfo">Indicates flag whether to export lookup related audit information or not</param>
        /// This flag used only in the case of UIRender ObjectSerialization</param>
        /// <returns>String representing Table in Xml format</returns>
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
                Int32 localeId = (Int32)this.Locale;

                xmlWriter.WriteStartElement("Table");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("LongName", this.LongName);
                xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
				xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
				
                xmlWriter.WriteStartElement("Relationships");

                if (this.Columns != null)
                {
                    foreach (LookupRelationship lookuprelationship in this.LookupRelationships)
                    {
                        xmlWriter.WriteRaw(lookuprelationship.ToXml());
                    }
                }

                //Columns node end
                xmlWriter.WriteEndElement();

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

                            if (row.Cells != null)
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
                                xmlWriter.WriteRaw(row.ToXml(ObjectSerialization.UIRender, populateSDLValue, useColumnNamesAsTag));
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
                                xmlWriter.WriteRaw(row.ToXml(ObjectSerialization.External, populateSDLValue, useColumnNamesAsTag, exportAuditInfo));
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
		public String GetModel(Boolean returnOnlyDisplayColumns = false)
        {
			String xml = String.Empty;
			StringWriter sw = new StringWriter();
			XmlTextWriter xmlWriter = new XmlTextWriter(sw);

			xmlWriter.WriteStartElement("Table");

			#region write Table meta data for Full Xml

			xmlWriter.WriteAttributeString("Id", this.Id.ToString());
			xmlWriter.WriteAttributeString("Name", this.Name);
			xmlWriter.WriteAttributeString("LongName", this.LongName);
			xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());

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
                    //returnOnlyDisplayColumns is false returns all the Metadeta Columns, else returns only display columns along with default columns(Id, DisplayFormat, ExportFormat and Searchcolumn).
                    if (!returnOnlyDisplayColumns || this.DisplayColumnList.Contains(column.Name) || column.Name.ToLower() == "id" || column.Id == 0)
                    {
                        xmlWriter.WriteRaw(column.ToXml());
                    }
				}
			}

			//Columns node end
			xmlWriter.WriteEndElement();

			#endregion write Columns

            xmlWriter.WriteStartElement("Relationships");

            if (this.Columns != null)
            {
                foreach (LookupRelationship lookuprelationship in this.LookupRelationships)
                {
                    xmlWriter.WriteRaw(lookuprelationship.ToXml());
                }
            }

            //Columns node end
            xmlWriter.WriteEndElement();

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

		public void LoadLookup(String valuesAsXml)
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

							if (reader.MoveToAttribute("AttributeId"))
							{
								this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
							}

							if (reader.MoveToAttribute("Name"))
							{
								this.Name = reader.ReadContentAsString();
							}

							if (reader.MoveToAttribute("LongName"))
							{
								this.LongName = reader.ReadContentAsString();
							}

                            if (reader.MoveToAttribute("Locale"))
                            {
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse(reader.ReadContentAsString(), out locale);
                                this.Locale = locale;
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
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                    {
                        #region Read LookupRelationships

                        String propXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(propXml))
                        {
                            LookupRelationshipCollection lookupRelationships = new LookupRelationshipCollection(propXml);
                            if (lookupRelationships != null)
                            {
                                this.LookupRelationships = lookupRelationships;
                            }
                        }

                        #endregion Read LookupRelationships
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
		public void LoadLookup(DataSet dsModelAndData)
		{
			if (dsModelAndData != null)
			{
				LookupCollection lookups = new LookupCollection(dsModelAndData);
				if (lookups != null)
				{
					Lookup currentLookup = lookups.FirstOrDefault();
					this.Rows = currentLookup.Rows;
					this.Columns = currentLookup.Columns;
					this.ExtendedProperties = currentLookup.ExtendedProperties;
                    this.LookupRelationships = currentLookup.LookupRelationships;
				}
			}
		}

	    /// <summary>
	    /// Get lookup record by providing id for the lookup.
	    /// </summary>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public IRow GetRecordById(Int32 id)
	    {
	        IRow row = null;

	        if (id < 0)
	            throw new ArgumentException("Lookup: GetRecordById: Parameter id less than 0");

	        if (!this.Columns.Contains(Lookup.IdColumnName))
	            throw new ArgumentException(String.Format("Lookup: GetRecordById: '{0}' column not found at Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.IdColumnName, this.Name));

	        if (this.Rows != null && this.Rows.Count > 0)
	        {
	            try
	            {
	                row = this.Rows[id];
	            }
	            catch (ArgumentException)
	            {
	                // Eat this up as RowCollection will throw if row not found but thats not necessarily lookup row error
	            }
	        }

	        return row;
	    }

	    /// <summary>
	    /// Get lookup records by providing id list for the lookup. 
	    /// </summary>
	    /// <param name="idList"></param>
	    /// <returns></returns>
	    public IRowCollection GetRecordByIdList(Collection<Int32> idList)
	    {
	        var rows = new RowCollection();

	        if (idList == null || idList.Count < 1)
	            throw new ArgumentException("Lookup: GetRecordByIdList: Parameter idList is null or empty");

	        if (!this.Columns.Contains(Lookup.IdColumnName))
	            throw new ArgumentException(String.Format("Lookup: GetRecordByIdList: '{0}' column not found at Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.IdColumnName, this.Name));

	        foreach (Int32 id in idList.Distinct())
	        {
	            try
	            {
	                rows.Add(this.Rows[id]);
	            }
	            catch (ArgumentException)
	            {
	                // Eat this up as RowCollection will throw if row not found but thats not necessarily lookup row error
	            }
	        }

	        return rows;
	    }

	    /// <summary>
		/// Get lookup record by display format
		/// </summary>
		/// <param name="displayFormat">Display format of the lookup value</param>
		/// <returns></returns>
		public IRow GetRecordByDisplayFormat(String displayFormat)
		{
			return GetRecordByDisplayFormat(displayFormat, null);
		}

		/// <summary>
		/// Get lookup record by display format
		/// </summary>
		/// <param name="displayFormat">Display format of the lookup value</param>
		/// <param name="applicationContext"></param>
		/// <returns></returns>
		public IRow GetRecordByDisplayFormat(String displayFormat, IApplicationContext applicationContext)
		{
			if (String.IsNullOrWhiteSpace(displayFormat))
				throw new ArgumentException("Lookup: GetRecordByDisplayFormat: Parameter displayFormat is null or empty");

			if (!this.Columns.Contains(Lookup.DisplayFormatColumnName))
				throw new ArgumentException(String.Format("Lookup: GetRecordByDisplayFormat: '{0}' column not found in the Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.DisplayFormatColumnName, this.Name));

            IRowCollection rows = null;

            if (this.IsIndexEnabled && this.Rows.IsIndexTableAvailable(Lookup.DisplayFormatColumnName))
            {
                rows = this.GetRowsByIndex(Lookup.DisplayFormatColumnName, displayFormat);
            }
            else
            {
                rows = this.Filter(Lookup.DisplayFormatColumnName, SearchOperator.EqualTo, displayFormat, StringComparison.InvariantCultureIgnoreCase);
            }

			if (rows != null && rows.Any())
			{
				if (applicationContext == null)
					return rows.FirstOrDefault(); //If display format has duplicate values, returning the first match

				foreach (Row filteredRow in rows)
				{
					if (this.CompareWithContext(filteredRow, (ApplicationContext)applicationContext))
						return filteredRow; //Returning the first match based on context
				}
			}

			return null;
		}

		/// <summary>
		/// Get lookup record by export format
		/// </summary>
		/// <param name="exportFormat">Export format of the lookup value</param>
		/// <returns></returns>
		public IRow GetRecordByExportFormat(String exportFormat)
		{
			return GetRecordByExportFormat(exportFormat, null);
		}

		/// <summary>
		/// Get lookup record by export format
		/// </summary>
		/// <param name="exportFormat">Export format of the lookup value</param>
		/// <param name="applicationContext"></param>
		/// <returns></returns>
		public IRow GetRecordByExportFormat(String exportFormat, IApplicationContext applicationContext)
		{
			if (String.IsNullOrWhiteSpace(exportFormat))
				throw new ArgumentException("Lookup: GetRecordByExportFormat: Parameter exportFormat is null or empty");

			if (!this.Columns.Contains(Lookup.ExportFormatColumnName))
				throw new ArgumentException(String.Format("Lookup: GetRecordByExportFormat: '{0}' column not found in the Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.ExportFormatColumnName, this.Name));

            IRowCollection rows = null;

            if (this.IsIndexEnabled && this.Rows.IsIndexTableAvailable(Lookup.ExportFormatColumnName))
            {
                rows = this.GetRowsByIndex(Lookup.ExportFormatColumnName, exportFormat); 
            }
            else
            {
                rows = this.Filter(Lookup.ExportFormatColumnName, SearchOperator.EqualTo, exportFormat, StringComparison.InvariantCultureIgnoreCase);
            }

			if (rows != null && rows.Count() > 0)
			{
				if (applicationContext == null)
					return rows.FirstOrDefault(); //If display format has duplicate values, returning the first match

				foreach (Row filteredRow in rows)
				{
					if (this.CompareWithContext(filteredRow, (ApplicationContext)applicationContext))
						return filteredRow; //Returning the first match based on context
				}
			}

			return null;
		}

		/// <summary>
		/// Gets display format for the requested lookup record Id
		/// </summary>
		/// <param name="id">Id of the lookup record</param>
		/// <returns>Display format</returns>
		public String GetDisplayFormatById(Int32 id)
		{
			String displayFormat = String.Empty;
			IRowCollection rows = null;
			Row row = null;

			#region Validation

			if (id < 1)
				throw new ArgumentException("Lookup: GetDisplayFormatById: Parameter id less than 1");

			if (!this.Columns.Contains(Lookup.IdColumnName))
				throw new ArgumentException(String.Format("Lookup: GetDisplayFormatById: '{0}' column not found in the Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.IdColumnName, this.Name));

			if (!this.Columns.Contains(Lookup.DisplayFormatColumnName))
				throw new ArgumentException(String.Format("Lookup: GetRecordByDisplayFormat: '{0}' column not found in the Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.DisplayFormatColumnName, this.Name));

			#endregion

			rows = this.Filter(Lookup.IdColumnName, SearchOperator.EqualTo, id.ToString());

			if (rows != null && rows.Count() > 0)
			{
				row = rows.First();

				if (row != null)
				{
					Object dispFormat = row[Lookup.DisplayFormatColumnName];

					if (dispFormat != null)
					{
						displayFormat = dispFormat.ToString();
					}
				}
			}

			return displayFormat;
		}

		/// <summary>
		/// Gets export format for the requested lookup record Id
		/// </summary>
		/// <param name="id">Id of the lookup record</param>
		/// <returns>Export format</returns>
		public String GetExportFormatById(Int32 id)
		{
			String exportFormat = String.Empty;
			IRowCollection rows = null;
			Row row = null;

			#region Validation

			if (id < 1)
				throw new ArgumentException("Lookup: GetExportFormatById: Parameter id less than 1");

			if (!this.Columns.Contains(Lookup.IdColumnName))
				throw new ArgumentException(String.Format("Lookup: GetExportFormatById: '{0}' column not found in the Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.IdColumnName, this.Name));

			if (!this.Columns.Contains(Lookup.ExportFormatColumnName))
				throw new ArgumentException(String.Format("Lookup: GetExportFormatById: '{0}' column not found in the Lookup table '{1}'. {0} must be present to perform this operation.", Lookup.ExportFormatColumnName, this.Name));

			#endregion

			rows = this.Filter(Lookup.IdColumnName, SearchOperator.EqualTo, id.ToString());

			if (rows != null && rows.Count() > 0)
			{
				row = rows.First();

				if (row != null)
				{
					Object expFormat = row[Lookup.ExportFormatColumnName];

					if (expFormat != null)
					{
						exportFormat = expFormat.ToString();
					}
				}
			}

			return exportFormat;
		}

		/// <summary>
		/// Convert Lookup Object to DataTable
		/// </summary>
		/// <returns>Datatable of Lookup Object</returns>
		public DataTable ToTable()
		{
			DataTable lookupTable = new DataTable();

			if (this.Columns != null)
			{
				foreach (Column col in this.Columns)
				{
                    DataColumn newColumn = new DataColumn();
                    newColumn.ColumnName = col.Name;
                    newColumn.DataType = typeof(String);
                    lookupTable.Columns.Add(newColumn);
                }

                //By default Caption will be column name only for display columns it should be DisplayColumn.
                for (Int32 i = 0; i < DisplayColumnList.Count; i++)
			    {
			        DataColumn column = lookupTable.Columns[DisplayColumnList[i]];
			        column.Caption = "DisplayColumn";
                    column.SetOrdinal(i);
			    }
            }

			if (this.Rows != null)
			{
				foreach (Row row in this.Rows)
				{
					DataRow newRow = lookupTable.NewRow();

					if (row.Cells != null)
					{
						foreach (Cell cell in row.Cells)
						{
							if (cell.Value != null)
							{
								if (cell.IsSystemLocaleValue == true)
									newRow[cell.ColumnName] = "[{(" + cell.Value.ToString() + ")}]"; //The wrapping of the data is the notation to identify system locale value
								else
									newRow[cell.ColumnName] = cell.Value.ToString();
							}
						}
					}

					lookupTable.Rows.Add(newRow);
				}
			}
            
            return lookupTable;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <param name="applicationContext"></param>
		/// <returns></returns>
		public Boolean CompareWithContext(Row row, ApplicationContext applicationContext)
		{
			Boolean isMatched = true;

			String valueSeparator = "|";

			if (this.Columns.Contains(Constants.LOOKUP_CONTEXT_ORGANIZATION_ID_LIST_COLUMN_NAME))
			{
				Int32 orgId = applicationContext.OrganizationId;

				if (orgId > 0)
                    isMatched = row.CompareValue(Constants.LOOKUP_CONTEXT_ORGANIZATION_ID_LIST_COLUMN_NAME, SearchOperator.Contains, orgId.ToString(), StringComparison.InvariantCultureIgnoreCase, valueSeparator);
			}

            if (isMatched && this.Columns.Contains(Constants.LOOKUP_CONTEXT_CONTAINER_ID_LIST_COLUMN_NAME))
			{
				Int32 containerId = applicationContext.ContainerId;

				if (containerId > 0)
                    isMatched = row.CompareValue(Constants.LOOKUP_CONTEXT_CONTAINER_ID_LIST_COLUMN_NAME, SearchOperator.Contains, containerId.ToString(), StringComparison.InvariantCultureIgnoreCase, valueSeparator);
			}

            if (isMatched && this.Columns.Contains(Constants.LOOKUP_CONTEXT_CATEGORY_PATH_LIST_COLUMN_NAME))
			{
				String categoryPath = HttpUtility.UrlDecode(applicationContext.CategoryPath);

				if (!String.IsNullOrWhiteSpace(categoryPath))
                    isMatched = row.CompareValue(Constants.LOOKUP_CONTEXT_CATEGORY_PATH_LIST_COLUMN_NAME, SearchOperator.SubsetOf, categoryPath, StringComparison.InvariantCultureIgnoreCase, valueSeparator);
			}

			return isMatched;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public new Lookup CopyStructure()
		{
			Lookup lookup = new Lookup();

			lookup.Id = this.Id;
			lookup.AttributeId = this.AttributeId;
			lookup.Name = this.Name;
			lookup.LongName = this.LongName;
            lookup.Locale = this.Locale;
            lookup.IsIndexEnabled = this.IsIndexEnabled;
            lookup.DisplayColumnList = this.DisplayColumnList;

            Hashtable clonedHashtable = new Hashtable();

            foreach (DictionaryEntry entry in this.ExtendedProperties)
            {
                if (entry.Value != null)
                {
                    if (entry.Key is String)
                    {
                        String newKey = new String(((String)entry.Key).ToCharArray());

                        if (entry.Value is String)
                        {
                            String val = entry.Value.ToString();
                            String newVal = new String(val.ToCharArray());
                            clonedHashtable.Add(newKey, newVal);
                        }
                        else if (entry.Value is Int32)
                        {
                            clonedHashtable.Add(newKey, entry.Value);
                        }
                        else // do not know what to do..
                        {
                            clonedHashtable.Add(newKey, entry.Value);
                        }
                    }
                    else
                    {
                        clonedHashtable.Add(entry.Key, entry.Value);
                    }
                }
                else
                {
                    clonedHashtable.Add(entry.Key, entry.Value);
                }
            }

            lookup.ExtendedProperties = clonedHashtable;

            lookup.Columns = (ColumnCollection)this.Columns.Clone();

			return lookup;
		}

        /// <summary>
        /// Property denotes whether the lookup has unique columns or not.
        /// </summary>
        /// <returns>Returns true when lookup has unique column else false</returns>
        public Boolean HasUniqueColumns
        {
            get
            {
                Boolean hasUniqueColumns = false;

                if (this.Columns != null)
                {
                    foreach (Column column in this.Columns)
                    {
                        if (column.IsUnique)
                        {
                            hasUniqueColumns = true;
                            break;
                        }
                    }
                }

                return hasUniqueColumns;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isExportMaskDefined"></param>
        public void UpdateLookupIndexes(Boolean isExportMaskDefined)
        {
            this.Rows.CreateIndexTable(Lookup.DisplayFormatColumnName);

            if (isExportMaskDefined)
            {
                this.Rows.CreateIndexTable(Lookup.ExportFormatColumnName);
            }

            this.Rows.CreateIndexes();

        }

        /// <summary>
        /// Returns Unique Columns available in a Lookup
        /// </summary>
        /// <returns>Collection of Unique Column</returns>
        public Collection<String> GetUniqueColumns()
        {
            Collection<String> lookupUniqueColumnNames = new Collection<String>();

            foreach (Column coln in Columns)
            {
                if (coln.IsUnique)
                {
                    lookupUniqueColumnNames.Add(coln.Name);
                }
            }

            return lookupUniqueColumnNames;
        }

        /// <summary>
        /// Denotes whether display format column is configured for this lookup
        /// </summary>
	    public Boolean IsDisplayFormatConfigured()
	    {
	        return this.Columns.Contains(DisplayFormatColumnName);
	    }

        /// <summary>
        /// Denotes whether export format column is configured for this lookup
        /// </summary>
	    public Boolean IsExportFormatConfigured()
	    {
	        return this.Columns.Contains(ExportFormatColumnName);
	    }

        /// <summary>
        /// Add a new row into the lookup
        /// </summary>
        /// <returns>Indicates the new row</returns>
        public void AddNewRow(IRow row)
        {
            if (row != null)
            {
                this.Rows.Add((Row)row);
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

        private IRowCollection GetRowsByIndex(String indexTable, string indexKey)
        {
            return this.Rows.GetRowsByIndex(indexTable, indexKey);
        }

		#endregion Private Methods
	}
}
