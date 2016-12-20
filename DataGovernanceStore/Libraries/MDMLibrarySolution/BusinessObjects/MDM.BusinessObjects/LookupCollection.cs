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
    /// Specifies class for lookup collection
    /// </summary>
    [DataContract]
    public class LookupCollection : ILookupCollection, ICollection<Lookup>, IEnumerable<Lookup>
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Lookup> _lookupTables = new Collection<Lookup>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LookupCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public LookupCollection( String valueAsXml )
        {
            LoadLookupCollection(valueAsXml);
        }

        /// <summary>
        /// Load table from DataSet
        /// </summary>
        /// <param name="dsModelAndData">DataSet from which table is to created</param>
        public LookupCollection( DataSet dsModelAndData )
        {
            LoadLookupCollection(dsModelAndData);
        }
        
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Find Lookup from LookupCollection based on LookupId
        /// </summary>
        /// <param name="lookupTableId">LookupID to search</param>
        /// <returns>Lookup object having given LookupId</returns>
        public Lookup this[Int32 lookupTableId]
        {
            get
            {
                Lookup lookup = this.GetLookupByLookupId(lookupTableId);

                if ( lookup == null )
                    throw new ArgumentException(String.Format("No Lookup found for Lookup id: {0}", lookupTableId), "lookupTableId");
                else
                    return lookup;
            }
            set
            {
                Lookup lookup = GetLookupByLookupId(lookupTableId);

                if ( lookup == null )
                    throw new ArgumentException(String.Format("No Lookup found for Lookup id: {0}", lookupTableId), "lookupTableId");

                lookup = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Remove table object from TableCollection
        /// </summary>
        /// <param name="lookupId">Id of table which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove( Int32 lookupId )
        {
            Lookup lookup = GetLookupByLookupId(lookupId);

            if ( lookup == null )
                throw new ArgumentException("No lookup found for given lookup id");
            else
                return this.Remove(lookup);
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
                Int32 tablesUnion = this._lookupTables.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 tablesIntersect = this._lookupTables.ToList().Intersect(objectToBeCompared.ToList()).Count();
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
            foreach ( Lookup Lookup in this._lookupTables )
            {
                hashCode += Lookup.GetHashCode();
            }
            return hashCode;
        }

        #pragma warning disable 1570

        /// <summary>
        /// Load table from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having data
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
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
        /// ]]>
        /// </para>
        /// </param>

        #pragma warning restore 1570

        public void LoadLookupCollection(String valuesAsXml)
        {
            //throw new NotImplementedException("Need to correct LookuoCollection's Xml constructor");
            #region Sample Xml
            /*
             *  <Tables>
                  <Table Id="2" Name="" LongName="tblk_Color" AttrtibuteId="3006">
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
                  <Table Id="-1" Name="" LongName="tblk_Pattern" AttrtibuteId="3008">
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                        {
                            String lookupXml = reader.ReadOuterXml();
                            if ( !String.IsNullOrEmpty(lookupXml) )
                            {
                                Lookup lookup = new Lookup(lookupXml);
                                if ( lookup != null )
                                {
                                    this.Add(lookup);
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
        /// <param name="dsModelAndData">DataSet from which table is to created</param>
        public void LoadLookupCollection( DataSet dsModelAndData )
        {
            //TODO:: Currently we are using DataSet / DataTable to populate our objects.
            //Need to try doing the same w/o using DataSet / DataTable .
            if ( dsModelAndData != null && dsModelAndData.Tables != null && dsModelAndData.Tables.Contains("TableModel") )
            {
                //DataSet has a table named "TableModel" which contains definition for all other tables.
                DataTable dtModel = dsModelAndData.Tables["TableModel"];

                //In Model table, there should be a column named "TableName" which will have name which we have given to rest of the tables.
                //If ValueFilterType = "LookupAttribute" then TableName field will have Attribute Ids for which we asked lookup
                //If ValueFilterType = "LookupMaster" then TableName will have name of lookup table.
                if ( dtModel.Rows != null && dtModel.Columns != null && dtModel.Columns.Contains("TableName") )
                {
                    Boolean readLookupName = false;
                    Boolean readAttributeId = false;
                    Boolean readTableId = false;
                    Boolean readModel = false;
                    Boolean readLocale = false;

                    readLookupName = dtModel.Columns.Contains("LookupTableName");
                    readAttributeId = dtModel.Columns.Contains("FK_Attribute");
                    readTableId = dtModel.Columns.Contains("Id");
                    readModel = dtModel.Columns.Contains("Model");
                    readLocale = dtModel.Columns.Contains("LocaleId");

                    //Iterate through the Model table, get the column definition from "Model" column for respective table name (ModelTable.TableName)
                    //Read 1 row from Model table, read respective table from DataSet, read each row and cell and populate Rows and its cells (value)
                    foreach ( DataRow dtRow in dtModel.Rows )
                    {
                        Lookup lookupTable = new Lookup();

                        //Read table Metadata 
                        #region Read table Metadata

                        if ( readLookupName == true && dtRow["LookupTableName"] != null )
                        {
                            lookupTable.Name = dtRow["LookupTableName"].ToString();
                            lookupTable.LongName = dtRow["LookupTableName"].ToString();
                        }

                        if ( readAttributeId == true && dtRow["FK_Attribute"] != null )
                        {
                            //lookupTable.ExtendedProperties.Add("AttributeId", dtRow["FK_Attribute"].ToString());
                            lookupTable.AttributeId = ValueTypeHelper.Int32TryParse(dtRow["FK_Attribute"].ToString(),0);
                        }

                        if ( readTableId == true && dtRow["Id"] != null )
                        {
                            lookupTable.Id = ValueTypeHelper.Int32TryParse(dtRow["Id"].ToString(), 0);
                        }

                        if (readLocale == true && dtRow["LocaleId"] != null)
                        {
                            Int32 localeId = ValueTypeHelper.Int32TryParse(dtRow["LocaleId"].ToString(), 0);
                            lookupTable.Locale = (LocaleEnum)localeId;
                        }

                        #endregion Read table Metadata

                        #region Populate model (columns)

                        if ( readModel == true && dtRow["Model"] != null )
                        {
                            String columnsXml = String.Empty;
                            ColumnCollection columns = null;

                            columnsXml = dtRow["Model"].ToString();
                            if ( String.IsNullOrWhiteSpace(columnsXml) )
                            {
                                throw new Exception(String.Concat("Model is empty "));
                            }
                            columns = new ColumnCollection(columnsXml);
                            lookupTable.Columns = columns;

                            LookupRelationshipCollection lookupRelationships = new LookupRelationshipCollection(columnsXml);

                            //this logic will hold good when we shift to WSID implementation
                            if (lookupRelationships.Count > 0)
                            {
                                lookupTable.LookupRelationships = lookupRelationships;

                                foreach (LookupRelationship lookupRelationship in lookupRelationships)
                                {
                                    Column col = new Column();
                                    col.Id = -1;
                                    col.Name = col.LongName = String.Format(Constants.LOOKUP_RELATIONSHIP_COLUMN_NAME_FORMAT, lookupRelationship.RefTableName, lookupRelationship.ColumnName);
                                    col.DataType = "VARCHAR";
                                    col.IsUnique = false;
                                    lookupTable.Columns.Add(col);
                                }
                            }
                        }

                        #endregion Populate model (columns)

                        #region Read rows

                        String rowsTableName = String.Empty;

                        //Find out name of table from dataset which is to be read for getting rows.
                        if ( dtRow["TableName"] != null )
                        {
                            rowsTableName = dtRow["TableName"].ToString();
                        }

                        DataTable rowsTable = dsModelAndData.Tables[rowsTableName];
                        RowCollection rows = new RowCollection();
                        if ( rowsTable != null && rowsTable.Rows != null && rowsTable.Columns != null )
                        {
                            String idColumnName = "Id";
                            Boolean readId = false;
                            readId = rowsTable.Columns.Contains(idColumnName);

                            //Read each row of DataTable having actual lookup data.
                            //DataTable.Row gives row and Table.Column will give column name.
                            foreach ( DataRow dr in rowsTable.Rows )
                            {
                                Int64 id = 0;
                                #region Read row information like Id

                                if (readId == true && dr[idColumnName] != null)
                                {
                                    id = ValueTypeHelper.Int32TryParse(dr[idColumnName].ToString(), 0);
                                } 

                                #endregion Read row information like Id

                                Row row = lookupTable.NewRow(false, id);
                                
                                #region Read cell from data table

                                foreach ( Column col in lookupTable.Columns )
                                {
                                    row.SetValue(col, dr[col.Name]);
                                }

                                #endregion Read cell from data table
                            }
                        }

                        #endregion Read rows

                        //Now table is finally created. So add it in Table collection
                        //this._lookupTables.Add(lookupTable);
                        this._lookupTables.Add(lookupTable);
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Get Lookup from collection based on Id
        /// </summary>
        /// <param name="lookupId">Id of table to search in current collection</param>
        /// <returns></returns>
        private Lookup GetLookupByLookupId(Int32 lookupId)
        {
            Int32 lookupCount = _lookupTables.Count;
            Lookup lookup = null;

            for (Int32 index = 0; index < lookupCount; index++)
            {
                lookup = _lookupTables[index];
                if (lookup.Id == lookupId)
                    return lookup;
            }
            return null;
        }

        #endregion

        #region ICollection<Lookup> Members

        /// <summary>
        /// Add lookup object in collection
        /// </summary>
        /// <param name="item">lookup to add in collection</param>
        public void Add( Lookup item )
        {
            this._lookupTables.Add(item);
        }

        /// <summary>
        /// Removes all lookups from collection
        /// </summary>
        public void Clear()
        {
            this._lookupTables.Clear();
        }

        /// <summary>
        /// Determines whether the LookupCollection contains a specific lookup.
        /// </summary>
        /// <param name="item">The lookup object to locate in the LookupCollection.</param>
        /// <returns>
        /// <para>true : If lookup found in lookupCollection</para>
        /// <para>false : If lookup found not in lookupCollection</para>
        /// </returns>
        public bool Contains( Lookup item )
        {
            return this._lookupTables.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the LookupCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from LookupCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo( Lookup[] array, int arrayIndex )
        {
            this._lookupTables.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of lookups in LookupCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._lookupTables.Count;
            }
        }

        /// <summary>
        /// Check if LookupCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the LookupCollection.
        /// </summary>
        /// <param name="item">The lookup object to remove from the LookupCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original LookupCollection</returns>
        public bool Remove( Lookup item )
        {
            return this._lookupTables.Remove(item);
        }

        #endregion

        #region IEnumerable<Lookup> Members

        /// <summary>
        /// Returns an enumerator that iterates through a LookupCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Lookup> GetEnumerator()
        {
            return this._lookupTables.GetEnumerator();
        }

        #endregion IEnumerable<Lookup> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a LookupCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( IEnumerator ) this._lookupTables.GetEnumerator();
        }

        #endregion

        #region ILookupCollection Methods

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

            if ( this._lookupTables != null )
            {
                foreach ( Lookup lookupTable in this._lookupTables )
                {
                    xmlWriter.WriteRaw(lookupTable.ToXml());
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

            if ( this._lookupTables != null )
            {
                foreach ( Lookup lookupTable in this._lookupTables )
                {
                    xmlWriter.WriteRaw(lookupTable.ToXml(serialization));
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
        /// Get model of lookups
        /// </summary>
        /// <returns>XML of lookups model</returns>
        public String GetLookupsModel(Boolean returnOnlyDisplayColumns = false)
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Tables");

            if (this._lookupTables != null)
            {
                foreach (Lookup lookupTable in this._lookupTables)
                {
                    xmlWriter.WriteRaw(lookupTable.GetModel(returnOnlyDisplayColumns));
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

        #region Add Lookup Methods

        /// <summary>
        /// Add table in ITableCollection
        /// </summary>
        /// <param name="lookup">Indicates Lookup to add in ILookup</param>
        /// <exception cref="ArgumentNullException">Thrown if table is null</exception>
        public void AddLookup(ILookup lookup)
        {
            if (lookup == null)
            {
                throw new ArgumentNullException("lookup");
            }

            this._lookupTables.Add((Lookup)lookup);
        }

        /// <summary>
        /// Add tables in current table collection
        /// </summary>
        /// <param name="lookups">Indicates ILookupCollection to add in current collection</param>
        /// <exception cref="ArgumentNullException">Thrown if tableCollection is null</exception>
        public void AddLookups(ILookupCollection lookups)
        {
            if (lookups == null)
            {
                throw new ArgumentNullException("lookups");
            }

            foreach (Lookup lookup in lookups)
            {
                this._lookupTables.Add((Lookup)lookup);
            }
        }

        #endregion

        #region GetLookup Methods

        /// <summary>
        /// Get Lookup from collection based on Name and locale
        /// </summary>
        /// <param name="lookupName">Name of table to search in current collection</param>
        /// <param name="locale">Locale to search in current collection</param>
        /// <returns></returns>
        public Lookup GetLookup(String lookupName, LocaleEnum locale)
        {
            lookupName = lookupName.ToLowerInvariant();
            String lookupInternalName = lookupName.StartsWith("tblk") ? lookupName : "tblk_" + lookupName;

            Int32 lookupCount = _lookupTables.Count;
            Lookup lookup = null;
            
            for (Int32 index = 0; index < lookupCount; index++)
            {
                lookup = _lookupTables[index];
                if (((lookup.Name.ToLowerInvariant() == lookupName || lookup.Name.ToLowerInvariant() == lookupInternalName) && lookup.Locale == locale))
                    return lookup;
            }
            return null;
        }

        /// <summary>
        /// Get Lookup from collection based on attribute id and locale
        /// </summary>
        /// <param name="attributeId">Indicates Attribute id to be searched in current collection</param>
        /// <param name="locale">Indicates Locale to be searched in current collection</param>
        /// <returns>Lookup based on attribute Id and locale</returns>
        public Lookup GetLookup(Int32 attributeId, LocaleEnum locale)
        {
            Int32 lookupCount = _lookupTables.Count;
            Lookup lookup = null;

            for (Int32 index = 0; index < lookupCount; index++)
            {
                lookup = _lookupTables[index];
                if (lookup.AttributeId == attributeId && lookup.Locale == locale)
                    return lookup;
            }

            return null;
        }

        #endregion
        
        #endregion ILookupCollection Methods
    }
}
