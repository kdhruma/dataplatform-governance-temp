using System;
using System.Data;
using System.Data.SqlClient;

namespace MDM.Imports.Processor
{
    using System.Configuration;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Imports.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class RelationshipAttributes : BaseAttributes, IBulkInsert
    {
        #region Fields
        #endregion Fields

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the SQL table name for the relationship AttributeTable.
        /// </summary>
        /// <returns></returns>
        public override String GetTableName()
        {
            return "tb_RelationshipAttrVal";
        }

        /// <summary>
        ///  Gets the associated data table for the object.
        /// </summary>
        /// <param name="mdmVersion"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(String mdmVersion)
        {
            DataTable dataTable = base.CreateBaseDataTable(mdmVersion);

            dataTable.Columns.Add("FK_Relationship", System.Type.GetType("System.Int64"));
            dataTable.Columns.Add("SRCFlag", System.Type.GetType("System.String"));
            dataTable.Columns.Add("IsInvalidData", System.Type.GetType("System.Boolean"));
            dataTable.Columns.Add("DeleteFlag", System.Type.GetType("System.Boolean"));

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given data rows with the attribute object. The entity is need for parent information 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="dataRows"></param>
        /// <param name="mdmVersion"></param>
        /// <param name="auditRefId"></param>
        public Boolean FillDataRow(Relationship relationship, MDM.BusinessObjects.Attribute attribute, DataRow[] dataRows, String mdmVersion, Int64 auditRefId)
        {
            Int32 currentValIndex = 0;

            foreach (Value value in attribute.GetCurrentValuesInvariant())
            {
                DataRow dataRow = dataRows[currentValIndex++];

                dataRow["FK_Relationship"] = relationship.Id;
                dataRow["SRCFlag"] = "O";
                dataRow["IsInvalidData"] = attribute.HasInvalidValues;
                dataRow["DeleteFlag"] = false;

                base.FillBaseDataRow(attribute, value, dataRow, auditRefId);
            }

            return true;
        }

        /// <summary>
        /// Map the db column to the bulk copy object
        /// </summary>
        /// <param name="sqlBulkCopy"></param>
        /// <returns></returns>
        public bool MapColumns(SqlBulkCopy sqlBulkCopy, String mdmVersion)
        {
            sqlBulkCopy.ColumnMappings.Clear();

            SqlBulkCopyColumnMapping FK_Relationship = new SqlBulkCopyColumnMapping("FK_Relationship", "FK_Relationship");
            sqlBulkCopy.ColumnMappings.Add(FK_Relationship);

            SqlBulkCopyColumnMapping SRCFlag = new SqlBulkCopyColumnMapping("SRCFlag", "SRCFlag");
            sqlBulkCopy.ColumnMappings.Add(SRCFlag);

            SqlBulkCopyColumnMapping IsInvalidData = new SqlBulkCopyColumnMapping("IsInvalidData", "IsInvalidData");
            sqlBulkCopy.ColumnMappings.Add(IsInvalidData);

            SqlBulkCopyColumnMapping DeleteFlag = new SqlBulkCopyColumnMapping("DeleteFlag", "DeleteFlag");
            sqlBulkCopy.ColumnMappings.Add(DeleteFlag);

            base.FillMapColumns(sqlBulkCopy, mdmVersion);

            return true;
        }

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}