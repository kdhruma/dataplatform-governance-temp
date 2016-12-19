using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using MDM.BusinessObjects;
using MDM.Core;
using MDM.Imports.Interfaces;
using MDM.ExceptionManager;
using MDM.Utility;
using System.Diagnostics;
using MDM.ExceptionManager.Handlers;
using MDM.BusinessObjects.Diagnostics;

namespace MDM.Imports.Processor
{
    public class Util
    {
        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        public Util()
        {
            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        /// <summary>
        /// Bulk copies the given datatable with the specified options.
        /// </summary>
        /// <param name="caltable"></param>
        /// <param name="bulkInsert"></param>
        /// <returns></returns>
        public bool BulkCopyDataTable(DataTable caltable, IBulkInsert bulkInsert, string mdmVersion, EventLogHandler LogHandler, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();

            bool success = false;
            int retryCount = 1;
            int numberOfRetries = bulkInsert.NumberOfRetries();
            while (numberOfRetries > 0)
            {
                string sqlConnectionString = bulkInsert.TargetConnectionString(application, module);
                bool useTableLock = bulkInsert.UseTableLock();
                bool useTriggers = bulkInsert.FireTriggers;
                SqlBulkCopy sqlBulkCopy = null;
                try
                {
                    using (SqlConnection mdmConnection = new SqlConnection(sqlConnectionString))
                    {
                        mdmConnection.Open();

                        SqlBulkCopyOptions bulkCopyOptions = SqlBulkCopyOptions.Default;
                        if (useTableLock)
                        {
                            bulkCopyOptions = bulkCopyOptions | SqlBulkCopyOptions.TableLock;
                        }
                        if (useTriggers)
                        {
                            bulkCopyOptions = bulkCopyOptions | SqlBulkCopyOptions.FireTriggers;
                        }

                        //bulkCopyOptions = bulkCopyOptions | SqlBulkCopyOptions.KeepIdentity;

                        sqlBulkCopy = new SqlBulkCopy(sqlConnectionString, bulkCopyOptions);
                        // Map the columns
                        if (bulkInsert.MapColumns(sqlBulkCopy, mdmVersion) == false)
                        {
                            message = string.Format("Bulk copy - Mapping of columns for table {0} failed.", bulkInsert.GetTableName());

                            if (_traceSettings.IsBasicTracingEnabled)
                            {
                                activity.LogError(message);
                            }
                            LogHandler.WriteErrorLog(message, 100);
                            return false;
                        }

                        sqlBulkCopy.DestinationTableName = bulkInsert.GetTableName();
                        sqlBulkCopy.BatchSize = bulkInsert.GetBatchSize();
                        sqlBulkCopy.BulkCopyTimeout = bulkInsert.TimeOutSeconds();

                        sqlBulkCopy.WriteToServer(caltable);
                        //update status
                        success = true;
                        numberOfRetries = 0;
                    }
                }
                catch (SqlException ex)
                {
                    // For sql exception, when the error is due to a dead lock in bulk copy, we retry. Any other error..we log and quit.
                    success = false;
                    // dead lock error code is 1205
                    if (ex.Number != 1205)
                    {
                        // a sql exception that is not a deadlock 
                        string errorMessage = String.Format("Bulk copy of table {0} failed with the following exception. The message is {1}", bulkInsert.GetTableName(), ex.Message);
                        // dump the table to a file and 
                        HandleErrorData(caltable, bulkInsert, LogHandler, errorMessage);
                        numberOfRetries = 1;
                    }
                    else
                    {
                        // for dead lock errors, just retry..
                        string errorMessage = String.Format("Bulk copy of table {0} failed due to dead lock issues. Retry attempt number {1} will happen now.", bulkInsert.GetTableName(), retryCount++);

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            activity.LogWarning(errorMessage);
                        }

                        LogHandler.WriteErrorLog(errorMessage, 50);
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = String.Format("Bulk copy of table {0} failed with the following exception. The message is {1}", bulkInsert.GetTableName(), ex.Message);
                    // dump the table to a file and 
                    HandleErrorData(caltable, bulkInsert, LogHandler, errorMessage);
                    success = false;
                    numberOfRetries = 1;
                }
                finally
                {
                    if (sqlBulkCopy != null)
                        sqlBulkCopy.Close();

                    // reduce the number of retries..if we failed..
                    if (!success)
                    numberOfRetries--;
                }
            }
            return success;
        }

        /// <summary>
        /// After we exhausted all our retry attempts, dump the data in a text file.
        /// </summary>
        /// <param name="caltable"></param>
        /// <param name="bulkInsert"></param>
        /// <param name="LogHandler"></param>
        private void HandleErrorData(DataTable caltable, IBulkInsert bulkInsert, EventLogHandler LogHandler, string message)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogError(message);
            }
            LogHandler.WriteErrorLog(message, 100);

            if (bulkInsert.DumpFailedDataInTextFile())
            {
                string folderName = bulkInsert.FolderForFailedData();

                if (string.IsNullOrEmpty(folderName))
                {
                    message = string.Format("Bulk Copy - Handle error data failed as the folder name is empty for table {0}", bulkInsert.GetTableName());

                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogError(message);
                    }
                    LogHandler.WriteErrorLog(message, 100);
                    return;
                }

                string fileName = System.IO.Path.Combine(folderName, string.Format("{0}-{1}", bulkInsert.GetTableName(), Guid.NewGuid()));
                try
                {
                    caltable.WriteXml(fileName);
                }
                catch (Exception ex)
                {
                    message = String.Format("Dumping table {0} to XML failed with the following exception. The message is {1}", bulkInsert.GetTableName(), ex.Message);

                    activity.LogError(message);
                    LogHandler.WriteErrorLog(message, 100);
                }
            }
        }
    }
}
