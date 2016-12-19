
using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Configuration;
using MDM.Core;
using MDM.Utility;

namespace Riversand.StoredProcedures
{
    /// <summary>
    /// 
    /// </summary>
    public class Aggregation
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private Aggregation()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static void ImportProduct(SqlInt32 FK_JobService, SqlString UserLogin, SqlInt32 FK_Supplier, SqlBoolean ToLoadCategories, SqlBoolean SchemaValidation)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Aggregation.ImportProduct(FK_JobService, UserLogin, FK_Supplier, ToLoadCategories, SchemaValidation, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ImportProduct(SqlInt32 FK_JobService, SqlString UserLogin, SqlInt32 FK_Supplier, SqlBoolean ToLoadCategories, SqlBoolean SchemaValidation, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Aggregation.ImportProduct(FK_JobService, UserLogin, FK_Supplier, ToLoadCategories, SchemaValidation, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ImportProduct(SqlInt32 FK_JobService, SqlString UserLogin, SqlInt32 FK_Supplier, SqlBoolean ToLoadCategories, SqlBoolean SchemaValidation, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlAggregation.ImportProduct(FK_JobService, UserLogin, FK_Supplier, ToLoadCategories, SchemaValidation, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Aggregation.ImportProduct for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Aggregation.ImportProduct for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageData(SqlString FileNameList, SqlString TableNameList, SqlInt32 JobID, SqlString FieldTerminator, SqlString RowTerminator)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Aggregation.StageData(FileNameList, TableNameList, JobID, FieldTerminator, RowTerminator, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageData(SqlString FileNameList, SqlString TableNameList, SqlInt32 JobID, SqlString FieldTerminator, SqlString RowTerminator, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Aggregation.StageData(FileNameList, TableNameList, JobID, FieldTerminator, RowTerminator, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageData(SqlString FileNameList, SqlString TableNameList, SqlInt32 JobID, SqlString FieldTerminator, SqlString RowTerminator, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlAggregation.StageData(FileNameList, TableNameList, JobID, FieldTerminator, RowTerminator, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Aggregation.StageData for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Aggregation.StageData for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageDataFromXML(SqlXml dataXml, SqlInt32 jobId, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            SqlAggregation.StageDataFromXML(dataXml, jobId, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageDataFromXML(SqlXml dataXml, SqlInt32 jobId, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            SqlAggregation.StageDataFromXML(dataXml, jobId, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StageDataFromXML(SqlXml dataXml, SqlInt32 jobId, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlAggregation.StageDataFromXML(dataXml, jobId, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of SqlAggregation.StageDataFromXML for this provider: " + providerName);
                    throw new ApplicationException("No implementation of SqlAggregation.StageDataFromXML for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable sp_help(SqlString objname)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Aggregation.sp_help(objname, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable sp_help(SqlString objname, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Aggregation.sp_help(objname, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable sp_help(SqlString objname, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlAggregation.sp_help(objname, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Aggregation.sp_help for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Aggregation.sp_help for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateStageTableFromTable(SqlString SourceTableName, SqlBoolean dropStaging)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Aggregation.CreateStageTableFromTable(SourceTableName, dropStaging, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateStageTableFromTable(SqlString SourceTableName, SqlBoolean dropStaging, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Aggregation.CreateStageTableFromTable(SourceTableName, dropStaging, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateStageTableFromTable(SqlString SourceTableName, SqlBoolean dropStaging, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlAggregation.CreateStageTableFromTable(SourceTableName, dropStaging, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Aggregation.CreateStageTableFromTable for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Aggregation.CreateStageTableFromTable for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ImportWorkerTableCreation(SqlInt32 WorkerCount)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Aggregation.ImportWorkerTableCreation(WorkerCount, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ImportWorkerTableCreation(SqlInt32 WorkerCount, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Aggregation.ImportWorkerTableCreation(WorkerCount, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ImportWorkerTableCreation(SqlInt32 WorkerCount, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlAggregation.ImportWorkerTableCreation(WorkerCount, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Aggregation.ImportWorkerTableCreation for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Aggregation.ImportWorkerTableCreation for this provider: " + providerName);
            }
        }

    }
}
