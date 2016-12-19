using System;
using System.Data.SqlClient;
using System.Data;

namespace MDM.Imports.Processor
{
    using System.Configuration;
    using MDM.Core;
    using MDM.ConfigurationManager.Business;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects;

    /// <summary>
    /// 
    /// </summary>
    public class TechnicalAttributes : BaseAttributes, IBulkInsert
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the SQL table name for the component.
        /// </summary>
        /// <returns></returns>
        public override String GetTableName()
        {
            return "tb_ComponentCharacteristic";
        }

        /// <summary>
        /// Gets the associated data table for the object.
        /// </summary>
        /// <param name="mdmVersion"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(String mdmVersion)
        {
            DataTable dataTable = base.CreateBaseDataTable(mdmVersion);

            dataTable.Columns.Add("FK_CNode", System.Type.GetType("System.Int64"));
            dataTable.Columns.Add("FK_Catalog", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("FK_CNodeParent", System.Type.GetType("System.Int64"));
            dataTable.Columns.Add("SRCFlag", System.Type.GetType("System.String"));

            return dataTable;
        }

        /// <summary>
        /// Fills the given data rows with the attribute object. The entity is need for parent information 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="dataRows"></param>
        /// <param name="mdmVersion"></param>
        /// <param name="auditRefId"></param>
        public Boolean FillDataRow(Entity entity, MDM.BusinessObjects.Attribute attribute, DataRow dataRow, String mdmVersion, Int64 auditRefId)
        {
            DataRow[] dataRows = new DataRow[1];
            dataRows[0] = dataRow;
            return FillDataRow(entity, attribute, dataRows, mdmVersion, auditRefId);
        }

        /// <summary>
        /// Fills the given data rows with the attribute object. The entity is need for parent information 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="dataRows"></param>
        /// <param name="mdmVersion"></param>
        /// <param name="auditRefId"></param>
        public Boolean FillDataRow(Entity entity, MDM.BusinessObjects.Attribute attribute, DataRow[] dataRows, String mdmVersion, Int64 auditRefId)
        {
            Int32 currentValIndex = 0;

            foreach (Value value in attribute.GetCurrentValuesInvariant())
            {
                DataRow dataRow = dataRows[currentValIndex++];

                dataRow["FK_CNode"] = entity.Id;
                dataRow["FK_Catalog"] = entity.ContainerId;
                dataRow["FK_CNodeParent"] = entity.ParentEntityId;
                dataRow["SRCFlag"] = "O";

                base.FillBaseDataRow(attribute, value, dataRow, auditRefId);
            }

            return true;
        }

        /// <summary>
        /// Fills the given data rows with the attribute object. The Relationship is need for parent information 
        /// </summary>
        /// <param name="relationship">Indicates relationship to be for which the attribute value row is to be generated.</param>
        /// <param name="attribute">Indicates attribute for which value row is to be generated.</param>
        /// <param name="dataRows">Indicates rows to be filled</param>
        /// <param name="mdmVersion">Indicates version of application</param>
        /// <param name="auditRefId">Indicates audit Id for the record.</param>
        public Boolean FillDataRow(Relationship relationship, MDM.BusinessObjects.Attribute attribute, DataRow[] dataRows, String mdmVersion, Int64 auditRefId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Map the db column to the bulk copy object
        /// </summary>
        /// <param name="sqlBulkCopy"></param>
        /// <returns></returns>
        public Boolean MapColumns(SqlBulkCopy sqlBulkCopy, String mdmVersion)
        {
            sqlBulkCopy.ColumnMappings.Clear();

            SqlBulkCopyColumnMapping FK_CNode = new SqlBulkCopyColumnMapping("FK_CNode", "FK_CNode");
            sqlBulkCopy.ColumnMappings.Add(FK_CNode);

            SqlBulkCopyColumnMapping FK_Catalog = new SqlBulkCopyColumnMapping("FK_Catalog", "FK_Catalog");
            sqlBulkCopy.ColumnMappings.Add(FK_Catalog);

            SqlBulkCopyColumnMapping FK_CNodeparent = new SqlBulkCopyColumnMapping("FK_CNodeParent", "FK_CNodeParent");
            sqlBulkCopy.ColumnMappings.Add(FK_CNodeparent);

            SqlBulkCopyColumnMapping SRCFlag = new SqlBulkCopyColumnMapping("SRCFlag", "SRCFlag");
            sqlBulkCopy.ColumnMappings.Add(SRCFlag);

            base.FillMapColumns(sqlBulkCopy, mdmVersion);

            return true;
        }

        #endregion

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}