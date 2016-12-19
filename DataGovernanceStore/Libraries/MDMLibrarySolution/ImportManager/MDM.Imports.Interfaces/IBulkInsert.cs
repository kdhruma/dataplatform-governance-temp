using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace MDM.Imports.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    public interface IBulkInsert
    {
        #region Properties

        /// <summary>
        /// Indicates if we need to fire triggers during the bulk insert process
        /// </summary>
        Boolean FireTriggers { get; set; }

        #endregion

        /// <summary>
        /// Returns the SQL table name for the component.
        /// </summary>
        /// <returns></returns>
        string GetTableName();

        /// <summary>
        /// Give the batch size to be used for the table
        /// </summary>
        /// <returns></returns>
        int GetBatchSize();

        /// <summary>
        /// Indicates if table lock is required during the bulk insert process
        /// </summary>
        /// <returns></returns>
        bool UseTableLock();

        /// <summary>
        /// Number of times to retry the bulk insert in case of failures
        /// </summary>
        /// <returns></returns>
        int NumberOfRetries();

        /// <summary>
        /// Number of seconds before the bulk insert times out.
        /// </summary>
        /// <returns></returns>
        int TimeOutSeconds();

        /// <summary>
        /// Indicates if we want to dump the failed bulk data in a text file.
        /// </summary>
        /// <returns></returns>
        bool DumpFailedDataInTextFile();

        /// <summary>
        /// The folder where we want to dump the data.
        /// </summary>
        /// <returns></returns>
        string FolderForFailedData();

        /// <summary>
        /// Gets the associated data table for the object.
        /// </summary>
        /// <returns></returns>
        DataTable CreateDataTable(string mdmVersion);

        /// <summary>
        /// Fills the given data rows with the attribute object. The entity is need for parent information 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="dataRows"></param>
        /// <param name="mdmVersion"></param>
        /// <param name="auditRefId"></param>
        Boolean FillDataRow(Entity entity, MDM.BusinessObjects.Attribute attribute, DataRow dataRow, string mdmVersion, Int64 auditRefId);

        /// <summary>
        /// Fills the given data rows with the attribute object. The entity is need for parent information 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="dataRows"></param>
        /// <param name="mdmVersion"></param>
        /// <param name="auditRefId"></param>
        Boolean FillDataRow(Entity entity, MDM.BusinessObjects.Attribute attribute, DataRow[] dataRows, string mdmVersion, Int64 auditRefId);

        /// <summary>
        /// Fills the given data rows with the attribute object. The Relationship is need for parent information 
        /// </summary>
        /// <param name="relationship">Indicates relationship to be for which the attribute value row is to be generated.</param>
        /// <param name="attribute">Indicates attribute for which value row is to be generated.</param>
        /// <param name="dataRows">Indicates rows to be filled</param>
        /// <param name="mdmVersion">Indicates version of application</param>
        /// <param name="auditRefId">Indicates audit Id for the record.</param>
        Boolean FillDataRow(Relationship relationship, MDM.BusinessObjects.Attribute attribute, DataRow[] dataRows, string mdmVersion, Int64 auditRefId);

        /// <summary>
        /// Map the db column to the bulk copy object
        /// </summary>
        /// <param name="sqlBulkCopy"></param>
        /// <returns></returns>
        bool MapColumns(SqlBulkCopy sqlBulkCopy, string mdmVersion);

        /// <summary>
        /// Make sure the input schema and output schema matches..
        /// </summary>
        /// <returns></returns>
        bool ValidateDatabaseSchema(string mdmVersion);

        /// <summary>
        /// Specifies the target server for the bulk insert
        /// </summary>
        /// <returns></returns>
        string TargetConnectionString(MDMCenterApplication application, MDMCenterModules module);
    }
}
