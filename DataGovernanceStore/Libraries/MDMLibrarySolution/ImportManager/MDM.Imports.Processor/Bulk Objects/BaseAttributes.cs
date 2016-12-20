using System;
using System.Data;
using System.Data.SqlClient;

namespace MDM.Imports.Processor
{
    using System.Configuration;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.ConfigurationManager.Business;

    /// <summary>
    /// 
    /// </summary>
    public class BaseAttributes
    {
        #region Fields

        /// <summary>
        /// Indicates if we need to fire triggers during the bulk insert process
        /// </summary>
        private Boolean _fireTriggers = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Indicates if we need to fire triggers during the bulk insert process
        /// </summary>
        public Boolean FireTriggers
        {
            get
            {
                return this._fireTriggers;
            }
            set
            {
                this._fireTriggers = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the SQL table name for the component.
        /// </summary>
        /// <returns></returns>
        public virtual String GetTableName()
        {
            return String.Empty;
        }

        /// <summary>
        /// Give the batch size to be used for the table
        /// </summary>
        /// <returns></returns>
        public Int32 GetBatchSize()
        {
            return 0;
        }

        /// <summary>
        /// Indicates if table lock is required during the bulk insert process
        /// </summary>
        /// <returns></returns>
        public Boolean UseTableLock()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["TableLockInBulkInsert"]);
        }

        /// <summary>
        /// Number of times to retry the bulk insert in case of failures
        /// </summary>
        /// <returns></returns>
        public Int32 NumberOfRetries()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfRetriesInBulkInsert"]);
        }

        /// <summary>
        /// Number of seconds before the bulk insert times out.
        /// </summary>
        /// <returns></returns>
        public Int32 TimeOutSeconds()
        {
            return 0;
        }

        /// <summary>
        /// Indicates if we want to dump the failed bulk data in a text file.
        /// </summary>
        /// <returns></returns>
        public Boolean DumpFailedDataInTextFile()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["DumpFailedDataInTextFile"]);
        }

        /// <summary>
        /// The folder where we want to dump the data.
        /// </summary>
        /// <returns></returns>
        public String FolderForFailedData()
        {
            return ConfigurationManager.AppSettings["FolderForFailedData"].ToString();
        }

        /// <summary>
        /// Gets the associated data table for the object.
        /// </summary>
        /// <returns></returns>
        public virtual DataTable CreateBaseDataTable(String mdmVersion)
        {
            DataTable dataTable = new DataTable(GetTableName());

            dataTable.Columns.Add("FK_Attribute", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("FK_Locale", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("AttrVal", System.Type.GetType("System.String"));
            dataTable.Columns.Add("FK_UOM", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("WSID", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("FK_Audit_Ref", System.Type.GetType("System.Int64"));
            dataTable.Columns.Add("Seq", System.Type.GetType("System.Double"));
            dataTable.Columns.Add("NumVal", System.Type.GetType("System.Double"));
            dataTable.Columns.Add("DateVal", System.Type.GetType("System.DateTime"));

            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="dataRow"></param>
        /// <param name="auditRefId"></param>
        public void FillBaseDataRow(Attribute attribute, Value value, DataRow dataRow, Int64 auditRefId)
        {
            dataRow["FK_Attribute"] = attribute.Id;
            dataRow["FK_Locale"] = (Int32)attribute.Locale;
            dataRow["AttrVal"] = value.AttrVal;

            if (value.UomId < 1)
            {
                dataRow["FK_UOM"] = DBNull.Value;
            }
            else
            {
                dataRow["FK_UOM"] = value.UomId;
            }

            dataRow["WSID"] = value.ValueRefId;
            dataRow["FK_Audit_Ref"] = auditRefId;
            dataRow["Seq"] = value.Sequence;

            if (attribute.AttributeDataType == AttributeDataType.Integer
                            || attribute.AttributeDataType == AttributeDataType.Decimal
                            || attribute.AttributeDataType == AttributeDataType.Fraction)
            {
                if (value.NumericVal != null)
                {
                    dataRow["NumVal"] = value.NumericVal;
                }
            }

            if (attribute.AttributeDataType == AttributeDataType.Date || attribute.AttributeDataType == AttributeDataType.DateTime)
            {
                if (value.DateVal != null)
                {
                    dataRow["DateVal"] = value.DateVal;
                }
            }
        }

        /// <summary>
        /// Map the db column to the bulk copy object
        /// </summary>
        /// <param name="sqlBulkCopy"></param>
        /// <param name="mdmVersion"></param>
        public void FillMapColumns(SqlBulkCopy sqlBulkCopy, String mdmVersion)
        {
            SqlBulkCopyColumnMapping FK_Attribute = new SqlBulkCopyColumnMapping("FK_Attribute", "FK_Attribute");
            sqlBulkCopy.ColumnMappings.Add(FK_Attribute);

            SqlBulkCopyColumnMapping FK_Locale = new SqlBulkCopyColumnMapping("FK_Locale", "FK_Locale");
            sqlBulkCopy.ColumnMappings.Add(FK_Locale);

            SqlBulkCopyColumnMapping AttrVal = new SqlBulkCopyColumnMapping("AttrVal", "AttrVal");
            sqlBulkCopy.ColumnMappings.Add(AttrVal);

            SqlBulkCopyColumnMapping FK_UOM = new SqlBulkCopyColumnMapping("FK_UOM", "FK_UOM");
            sqlBulkCopy.ColumnMappings.Add(FK_UOM);

            SqlBulkCopyColumnMapping WSID = new SqlBulkCopyColumnMapping("WSID", "WSID");
            sqlBulkCopy.ColumnMappings.Add(WSID);

            SqlBulkCopyColumnMapping FK_Audit_Ref = new SqlBulkCopyColumnMapping("FK_Audit_Ref", "FK_Audit_Ref");
            sqlBulkCopy.ColumnMappings.Add(FK_Audit_Ref);

            SqlBulkCopyColumnMapping Seq = new SqlBulkCopyColumnMapping("Seq", "Seq");
            sqlBulkCopy.ColumnMappings.Add(Seq);

            SqlBulkCopyColumnMapping NumVal = new SqlBulkCopyColumnMapping("NumVal", "NumVal");
            sqlBulkCopy.ColumnMappings.Add(NumVal);

            SqlBulkCopyColumnMapping DateVal = new SqlBulkCopyColumnMapping("DateVal", "DateVal");
            sqlBulkCopy.ColumnMappings.Add(DateVal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdmVersion"></param>
        /// <returns></returns>
        public Boolean ValidateDatabaseSchema(String mdmVersion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies the target server for the bulk insert
        /// </summary>
        /// <returns></returns>
        public String TargetConnectionString(MDMCenterApplication application, MDMCenterModules module)
        {
            return DBCommandHelper.Get(application, module, MDMCenterModuleAction.Create).ConnectionString;
        }

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}