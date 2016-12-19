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
    /// Specifies class for Dependent Attribute Data Map Collection
    /// </summary>
    [DataContract]
    public class DependentAttributeDataMapCollection : IDependentAttributeDataMapCollection, ICollection<DependentAttributeDataMap>, IEnumerable<DependentAttributeDataMap>
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<DependentAttributeDataMap> _dependentAttributeMapTables = new Collection<DependentAttributeDataMap>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DependentAttributeDataMapCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DependentAttributeDataMapCollection( String valueAsXml )
        {
            LoadDependentAttributeDataMapCollection(valueAsXml);
        }

        /// <summary>
        /// Load table from DataSet
        /// </summary>
        /// <param name="dsModelAndData">DataSet from which table is to created</param>
        public DependentAttributeDataMapCollection(DataSet dsModelAndData)
        {
            LoadDependentAttributeDataMapCollection(dsModelAndData);
        }
        
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Find DependentAttributeDataMap from DependentAttributeDataMapCollection based on DependentAttributeDataMapId
        /// </summary>
        /// <param name="dependentAttributeDataMapTableId">DependentAttributeDataMapID to search</param>
        /// <returns>DependentAttributeDataMapTableId object having given dependentAttributeDataMapTableId</returns>
        public DependentAttributeDataMap this[Int32 dependentAttributeDataMapTableId]
        {
            get
            {
                DependentAttributeDataMap dependentAttributeDataMap = this._dependentAttributeMapTables[dependentAttributeDataMapTableId];

                if (dependentAttributeDataMap == null)
                    throw new ArgumentException(String.Format("No DependentAttribute found for DependentAttributeDataMap id: {0}", dependentAttributeDataMapTableId), "dependentAttributeDataMapId");
                else
                    return dependentAttributeDataMap;
            }
            set
            {
                DependentAttributeDataMap dependentAttributeDataMap = GetDependentAttributeDataMapById(dependentAttributeDataMapTableId);

                if (dependentAttributeDataMap == null)
                    throw new ArgumentException(String.Format("No dependentAttributeDataMap found for dependentAttributeDataMap id: {0}", dependentAttributeDataMapTableId), "dependentAttributeDataMapId");

                dependentAttributeDataMap = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Remove table object from TableCollection
        /// </summary>
        /// <param name="dependentAttributeDataMapId">Id of table which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 dependentAttributeDataMapId)
        {
            DependentAttributeDataMap dependentAttributeDataMap = GetDependentAttributeDataMapById(dependentAttributeDataMapId);

            if (dependentAttributeDataMap == null)
                throw new ArgumentException("No dependentAttributeDataMap found for given dependentAttributeDataMap id");
            else
                return this.Remove(dependentAttributeDataMap);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is TableCollection)
            {
                TableCollection objectToBeCompared = obj as TableCollection;
                Int32 tablesUnion = this._dependentAttributeMapTables.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 tablesIntersect = this._dependentAttributeMapTables.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (tablesUnion != tablesIntersect)
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
            foreach (DependentAttributeDataMap dependentAttrDataMap in this._dependentAttributeMapTables)
            {
                hashCode += dependentAttrDataMap.GetHashCode();
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

        public void LoadDependentAttributeDataMapCollection(String valuesAsXml)
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

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                        {
                            String dependentAttributeDataMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(dependentAttributeDataMapXml))
                            {
                                DependentAttributeDataMap dependentAttributeDataMap = new DependentAttributeDataMap(dependentAttributeDataMapXml);
                                if (dependentAttributeDataMap != null)
                                {
                                    this.Add(dependentAttributeDataMap);
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
        /// Load table from DataSet
        /// </summary>
        /// <param name="dsModelAndData">DataSet from which table is to created</param>
        public void LoadDependentAttributeDataMapCollection(DataSet dsModelAndData)
        {
            //TODO:: Currently we are using DataSet / DataTable to populate our objects.
            //Need to try doing the same w/o using DataSet / DataTable .
            if (dsModelAndData != null && dsModelAndData.Tables != null && dsModelAndData.Tables.Contains("Model"))
            {
                //DataSet has a table named "TableModel" which contains definition for all other tables.
                DataTable dtModel = dsModelAndData.Tables["Model"];

                //In Model table, there should be a column named "TableName" which will have name which we have given to rest of the tables.
                //If ValueFilterType = "LookupAttribute" then TableName field will have Attribute Ids for which we asked lookup
                //If ValueFilterType = "LookupMaster" then TableName will have name of lookup table.
                if (dtModel.Rows != null && dtModel.Columns != null && dtModel.Columns.Contains("LinkTableName"))
                {
                    Boolean readLookupName = false;
                    Boolean readTableId = false;
                    Boolean readModel = false;
                   
                    readTableId = dtModel.Columns.Contains("Id");
                    readLookupName = dtModel.Columns.Contains("LinkTableName");
                    readModel = dtModel.Columns.Contains("Model");
                    
                    //Iterate through the Model table, get the column definition from "Model" column for respective table name (ModelTable.TableName)
                    //Read 1 row from Model table, read respective table from DataSet, read each row and cell and populate Rows and its cells (value)
                    foreach (DataRow dtRow in dtModel.Rows)
                    {
                        DependentAttributeDataMap dependentAttributeDataMapTable = new DependentAttributeDataMap();

                        //Read table Metadata 
                        #region Read table Metadata

                        if (readLookupName == true && dtRow["LinkTableName"] != null)
                        {
                            dependentAttributeDataMapTable.Name = dtRow["LinkTableName"].ToString();
                            dependentAttributeDataMapTable.LongName = dtRow["LinkTableName"].ToString();
                        }

                        if (readTableId == true && dtRow["Id"] != null)
                        {
                            dependentAttributeDataMapTable.Id = ValueTypeHelper.Int32TryParse(dtRow["Id"].ToString(), 0);
                        }

                        #endregion Read table Metadata

                        #region Populate model (columns)

                        if (readModel == true && dtRow["Model"] != null)
                        {
                            String columnsXml = String.Empty;
                            ColumnCollection columns = null;

                            columnsXml = dtRow["Model"].ToString();
                            if (String.IsNullOrWhiteSpace(columnsXml))
                            {
                                throw new Exception(String.Concat("Model is empty "));
                            }
                            columns = new ColumnCollection(columnsXml);
                            dependentAttributeDataMapTable.Columns = columns;
                        }

                        #endregion Populate model (columns)

                        #region Read rows

                        String rowsTableName = String.Empty;
                        String columnName = string.Empty;

                        //Find out name of table from dataset which is to be read for getting rows.
                        if (dtRow["LinkTableName"] != null)
                        {
                            rowsTableName = dtRow["LinkTableName"].ToString();
                            columnName = rowsTableName.Replace("tblink", "PK") ;
                        }

                        DataTable rowsTable = dsModelAndData.Tables[rowsTableName];
                        RowCollection rows = new RowCollection();
                        if (rowsTable != null && rowsTable.Rows != null && rowsTable.Columns != null)
                        {
                            
                            Boolean readId = true;
                           
                            //Read each row of DataTable having actual lookup data.
                            //DataTable.Row gives row and Table.Column will give column name.
                            foreach (DataRow dr in rowsTable.Rows)
                            {
                                Row row = dependentAttributeDataMapTable.NewRow(false);

                                #region Read row information like Id

                                if (readId == true && !String.IsNullOrEmpty(columnName) && dr[columnName]!=null)
                                {
                                    row.Id = ValueTypeHelper.Int32TryParse(dr[columnName].ToString(), 0);
                                }

                                #endregion Read row information like Id

                                #region Read cell from data table

                                foreach (Column col in dependentAttributeDataMapTable.Columns)
                                {
                                    row.SetValue(col, dr[col.Name]);
                                }

                                #endregion Read cell from data table
                            }
                        }

                        #endregion Read rows

                        //Now table is finally created. So add it in Table collection
                        //this._lookupTables.Add(lookupTable);
                        this._dependentAttributeMapTables.Add(dependentAttributeDataMapTable);
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Get DependentAttributeDataMap from collection based on Id
        /// </summary>
        /// <param name="dependentAttributeDataMapId">Id of table to search in current collection</param>
        /// <returns></returns>
        private DependentAttributeDataMap GetDependentAttributeDataMapById(Int32 dependentAttributeDataMapId)
        {
            var filteredDependentAttributeDataMaps = from dependentAttributeDataMap in this._dependentAttributeMapTables
                                                     where dependentAttributeDataMap.Id == dependentAttributeDataMapId
                                  select dependentAttributeDataMap;

            if (filteredDependentAttributeDataMaps.Any())
                return filteredDependentAttributeDataMaps.First();
            else
                return null;
        }

        #endregion

        #region ICollection<DependentAttributeDataMap> Members

        /// <summary>
        /// Add lookup object in collection
        /// </summary>
        /// <param name="item">dependentAttributeDataMap to add in collection</param>
        public void Add(DependentAttributeDataMap item)
        {
            this._dependentAttributeMapTables.Add(item);
        }

        /// <summary>
        /// Removes all dependentAttributeDataMaps from collection
        /// </summary>
        public void Clear()
        {
            this._dependentAttributeMapTables.Clear();
        }

        /// <summary>
        /// Determines whether the dependentAttributeDataMapCollection contains a specific dependentAttributeDataMap.
        /// </summary>
        /// <param name="item">The dependentAttributeDataMap object to locate in the dependentAttributeDataMapCollection.</param>
        /// <returns>
        /// <para>true : If dependentAttributeDataMap found in dependentAttributeDataMapCollection</para>
        /// <para>false : If dependentAttributeDataMap found not in dependentAttributeDataMapCollection</para>
        /// </returns>
        public bool Contains(DependentAttributeDataMap item)
        {
            return this._dependentAttributeMapTables.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the dependentAttributeDataMapCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from dependentAttributeDataMapCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DependentAttributeDataMap[] array, int arrayIndex)
        {
            this._dependentAttributeMapTables.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of dependentAttributeDataMaps in DependentAttributeDataMapCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._dependentAttributeMapTables.Count;
            }
        }

        /// <summary>
        /// Check if DependentAttributeDataMapCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DependentAttributeDataMapCollection.
        /// </summary>
        /// <param name="item">The dependentAttributeDataMap object to remove from the dependentAttributeDataMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original dependentAttributeDataMapCollection</returns>
        public bool Remove(DependentAttributeDataMap item)
        {
            return this._dependentAttributeMapTables.Remove(item);
        }

        #endregion

        #region IEnumerable<DependentAttributeDataMap> Members

        /// <summary>
        /// Returns an enumerator that iterates through a dependentAttributeDataMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DependentAttributeDataMap> GetEnumerator()
        {
            return this._dependentAttributeMapTables.GetEnumerator();
        }

        #endregion IEnumerable<DependentAttributeDataMap> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a dependentAttributeDataMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._dependentAttributeMapTables.GetEnumerator();
        }

        #endregion

        #region IDependentAttributeDataMapCollection Methods

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

            if (this._dependentAttributeMapTables != null)
            {
                foreach (DependentAttributeDataMap dependentAttributeDataMap in this._dependentAttributeMapTables)
                {
                    xmlWriter.WriteRaw(dependentAttributeDataMap.ToXml());
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
        /// <param name="serialization">Indicates serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Tables");

            if (this._dependentAttributeMapTables != null)
            {
                foreach (DependentAttributeDataMap dependentAttributeDataMapTable in this._dependentAttributeMapTables)
                {
                    xmlWriter.WriteRaw(dependentAttributeDataMapTable.ToXml(serialization));
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
        /// Get the xml representation of dependent attribute data map model based on current object
        /// </summary>
        /// <returns>Returns xml representation of dependent attribute data map model based on current object</returns>
        public String GetDependentAttributeDataMapsModel()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Tables");

            if (this._dependentAttributeMapTables != null)
            {
                foreach (DependentAttributeDataMap dependentAttributeDataMap in this._dependentAttributeMapTables)
                {
                    xmlWriter.WriteRaw(dependentAttributeDataMap.GetModel());
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

        #region Add DependentAttributeDataMap Methods

        /// <summary>
        /// Add dependent attribute data map to add in collection
        /// </summary>
        /// <param name="dependentAttributeDataMap">Indicates dependent attribute data map which is to be added in collection</param>
        /// <exception cref="ArgumentNullException">Thrown null exception if the object is null</exception>
        public void AddDependentAttributeDataMap(IDependentAttributeDataMap dependentAttributeDataMap)
        {
            if (dependentAttributeDataMap == null)
            {
                throw new ArgumentNullException("dependentAttributeDataMap");
            }

            this._dependentAttributeMapTables.Add((DependentAttributeDataMap)dependentAttributeDataMap);
        }

        /// <summary>
        /// Add dependent attribute data map collection to add in current collection
        /// </summary>
        /// <param name="dependentAttributeDataMaps">Indicates dependent attribute data map collection which is to be added in collection</param>
        /// <exception cref="ArgumentNullException">Thrown null exception if the object is null</exception>
        public void AddDependentAttributeDataMaps(IDependentAttributeDataMapCollection dependentAttributeDataMaps)
        {
            if (dependentAttributeDataMaps == null)
            {
                throw new ArgumentNullException("dependentAttributeDataMaps");
            }

            foreach (DependentAttributeDataMap dependentAttributeDataMap in dependentAttributeDataMaps)
            {
                this._dependentAttributeMapTables.Add((DependentAttributeDataMap)dependentAttributeDataMap);
            }
        }

        #endregion

        #endregion IDependentAttributeDataMapCollection Methods
    }
}
