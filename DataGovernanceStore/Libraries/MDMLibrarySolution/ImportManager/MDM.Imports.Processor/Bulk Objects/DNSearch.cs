using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Concurrent;

namespace MDM.Imports.Processor
{
    using MDM.BusinessObjects;
    using MDM.Imports.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// 
    /// </summary>
    public class DNSearch : BaseAttributes, IDNSearch
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        ConcurrentDictionary<Int64, EntitySearchValuesBuilder> dnSearch = new ConcurrentDictionary<Int64, EntitySearchValuesBuilder>();

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ConcurrentDictionary<Int64, EntitySearchValuesBuilder> KeySearchValues
        {
            get
            {
                return dnSearch;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the SQL table name for the component.
        /// </summary>
        /// <returns></returns>
        public override String GetTableName()
        {
            return "tb_DN_Search";
        }

        /// <summary>
        /// Gets the associated data table for the object.
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDataTable(String mdmVersion)
        {
            return CreateDataTable();
        }

        /// <summary>
        /// Fills the given data rows with the attribute object. The entity is need for parent information 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="dataRows"></param>
        /// <param name="mdmVersion"></param>
        /// <param name="auditRefId"></param>
        public Boolean FillDataRow(Entity entity, MDM.BusinessObjects.Attribute attribute, DataRow dataRow, string mdmVersion, Int64 auditRefId)
        {
            Boolean returnStatus = false;

            if (dnSearch.Keys.Contains(entity.Id))
            {
                EntitySearchValuesBuilder ksValue = dnSearch[entity.Id];
                dataRow["FK_Catalog"] = entity.ContainerId;
                dataRow["FK_CNode"] = entity.Id;
                dataRow["ObjectID"] = entity.ExtensionUniqueId;
                dataRow["FK_CNodeParent"] = entity.ParentEntityId;
                dataRow["FK_CatalogBranchLevel"] = entity.BranchLevel;
                dataRow["SearchVal"] = Helpers.ComputeSearchValueWithHeader(entity, ksValue.SearchValue);
                dataRow["KeyVal"] = Helpers.ComputeKeyValueWithHeader(entity, ksValue.KeyValue);
                dataRow["IDPath"] = entity.IdPath;
                returnStatus = true;
            }
            else
            {
                // is this an error??
                Console.WriteLine(string.Format("Fill data for DN Search missing value for entity {0}", entity.Name));
            }

            return returnStatus;
        }

        /// <summary>
        /// Fills the given data rows with the attribute object. The entity is need for parent information 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="dataRows"></param>
        /// <param name="mdmVersion"></param>
        /// <param name="auditRefId"></param>
        public Boolean FillDataRow(Entity entity, MDM.BusinessObjects.Attribute attribute, DataRow[] dataRows, string mdmVersion, Int64 auditRefId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given data rows with the attribute object. The Relationship is need for parent information 
        /// </summary>
        /// <param name="relationship">Indicates relationship to be for which the attribute value row is to be generated.</param>
        /// <param name="attribute">Indicates attribute for which value row is to be generated.</param>
        /// <param name="dataRows">Indicates rows to be filled</param>
        /// <param name="mdmVersion">Indicates version of application</param>
        /// <param name="auditRefId">Indicates audit Id for the record.</param>
        public Boolean FillDataRow(Relationship relationship, MDM.BusinessObjects.Attribute attribute, DataRow[] dataRows, string mdmVersion, Int64 auditRefId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int32 GetEntityCount()
        {
            return (dnSearch != null) ? dnSearch.Keys.Count() : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlBulkCopy"></param>
        /// <param name="mdmVersion"></param>
        /// <returns></returns>
        public bool MapColumns(System.Data.SqlClient.SqlBulkCopy sqlBulkCopy, string mdmVersion)
        {
            sqlBulkCopy.ColumnMappings.Clear();

            SqlBulkCopyColumnMapping FK_CNode = new SqlBulkCopyColumnMapping("FK_CNode", "FK_CNode");
            sqlBulkCopy.ColumnMappings.Add(FK_CNode);

            SqlBulkCopyColumnMapping FK_Catalog = new SqlBulkCopyColumnMapping("FK_Catalog", "FK_Catalog");
            sqlBulkCopy.ColumnMappings.Add(FK_Catalog);

            SqlBulkCopyColumnMapping ObjectID = new SqlBulkCopyColumnMapping("ObjectID", "ObjectID");
            sqlBulkCopy.ColumnMappings.Add(ObjectID);

            SqlBulkCopyColumnMapping FK_CNodeParent = new SqlBulkCopyColumnMapping("FK_CNodeParent", "FK_CNodeParent");
            sqlBulkCopy.ColumnMappings.Add(FK_CNodeParent);

            SqlBulkCopyColumnMapping FK_CatalogBranchLevel = new SqlBulkCopyColumnMapping("FK_CatalogBranchLevel", "FK_CatalogBranchLevel");
            sqlBulkCopy.ColumnMappings.Add(FK_CatalogBranchLevel);

            SqlBulkCopyColumnMapping SearchVal = new SqlBulkCopyColumnMapping("SearchVal", "SearchVal");
            sqlBulkCopy.ColumnMappings.Add(SearchVal);

            SqlBulkCopyColumnMapping KeyVal = new SqlBulkCopyColumnMapping("KeyVal", "KeyVal");
            sqlBulkCopy.ColumnMappings.Add(KeyVal);

            SqlBulkCopyColumnMapping IDPath = new SqlBulkCopyColumnMapping("IDPath", "IDPath");
            sqlBulkCopy.ColumnMappings.Add(IDPath);

            return true;
        }

        /// <summary>
        /// Computes the key value and search value for the given entity and attribute
        /// </summary>
        /// <param name="calRow"></param>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="mdmVersion"></param>
        public void ComputeKeySearchValue(BusinessObjects.Entity entity, BusinessObjects.Attribute item, string mdmVersion)
        {
            try
            {
                long startTime = DateTime.Now.Ticks;
                long actualstartTime = 0;
                EntitySearchValuesBuilder ksValue = null;

                // this dictionary is shared across common and tech attribute threads...
                if (dnSearch.Keys.Contains(entity.Id))
                {
                    ksValue = dnSearch[entity.Id];
                }
                else
                {
                    ksValue = new EntitySearchValuesBuilder();
                }

                actualstartTime = DateTime.Now.Ticks;
                foreach (Value itemValue in item.GetCurrentValuesInvariant())
                {
                    ksValue.AddKeyValue(Helpers.ComputeKeyValue(item.Id, item.AttributeDataType, itemValue, (Int32)item.Locale));
                    ksValue.AddSearchValue(Helpers.ComputeSearchValue(item.Id, item.AttributeDataType, itemValue, (Int32)item.Locale));
                }

                dnSearch[entity.Id] = ksValue;
                long endTime = DateTime.Now.Ticks;
                double timeTaken = new TimeSpan(endTime - startTime).TotalMilliseconds;
                double actualTimeTaken = new TimeSpan(endTime - actualstartTime).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Fill data row threw an exception {0}", ex.Message));
            }
        }

        /// <summary>
        /// For a given entity fetches the key value and search value
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="keyValue"></param>
        /// <param name="searchValue"></param>
        public void AddKeySearchValue(Int64 entityId, string keyValue, string searchValue)
        {
            EntitySearchValuesBuilder ksValue = null;

            if (dnSearch.Keys.Contains(entityId))
            {
                ksValue = dnSearch[entityId];
            }
            else
            {
                ksValue = new EntitySearchValuesBuilder();
            }

            ksValue.AddKeyValue(keyValue);
            ksValue.AddSearchValue(searchValue);
            dnSearch[entityId] = ksValue;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable(GetTableName());

            dataTable.Columns.Add("FK_CNode", System.Type.GetType("System.Int64"));
            dataTable.Columns.Add("FK_Catalog", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("ObjectID", System.Type.GetType("System.Int64"));
            dataTable.Columns.Add("FK_CNodeParent", System.Type.GetType("System.Int64"));
            dataTable.Columns.Add("FK_CatalogBranchLevel", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("SearchVal", System.Type.GetType("System.String"));
            dataTable.Columns.Add("KeyVal", System.Type.GetType("System.String"));
            dataTable.Columns.Add("IDPath", System.Type.GetType("System.String"));

            return dataTable;
        }

        #endregion Private Methods

        #endregion Methods
    }
}