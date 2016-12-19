using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Transactions;

namespace MDM.JobManager.Data.SqlClient
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using Microsoft.SqlServer.Server;

    /// <summary>
    /// Specifies job Import Result Data Access
    /// </summary>
    public class JobImportResultDA : SqlClientDataAccessBase
    {
        #region Public Methods

        /// <summary>
        /// Saves the given set of import job results in to the database.
        /// </summary>
        /// <param name="jobResults"></param>
        /// <param name="userName"></param>
        /// <param name="programName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public Boolean Save(JobImportResultCollection jobResults, String userName, String programName, DBCommandProperties command)
        {
            Boolean result = true;
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_JobImportResult_Add_ParametersArray");

                storedProcedureName = "usp_JobManager_Job_Result_Import_Process";

                #region Populate table value parameters

                List<SqlDataRecord> jobResultList = new List<SqlDataRecord>();
                SqlMetaData[] importResultMetadata = generator.GetTableValueMetadata("JobManager_JobImportResult_Add_ParametersArray", parameters[0].ParameterName);

                SqlDataRecord importResultRecord = null;

                foreach (JobImportResult jobResult in jobResults)
                {
                    importResultRecord = new SqlDataRecord(importResultMetadata);
                    importResultRecord.SetValues(jobResult.JobId, jobResult.Status, ((Byte)jobResult.ObjectType).ToString(), jobResult.ExternalId, jobResult.InternalId, jobResult.OperationResultXML, jobResult.Description, 1, jobResult.Action.ToString(), (Byte)jobResult.PerformedAction);
                    jobResultList.Add(importResultRecord);
                }
                
                parameters[0].Value = jobResultList;


                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                #endregion
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return result;
        }

        /// <summary>
        /// Gets the import job results for a given job id
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public JobImportResultCollection Get(Int32 jobId, DBCommandProperties command, String lookupTableName = "")
        {
            JobImportResultCollection jobImportResultCollection = new JobImportResultCollection();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_JobImportResult_GetParametersArray");

                storedProcedureName = "usp_JobManager_Job_Result_Import_Get";

                parameters[0].Value = jobId;
                parameters[1].Value = lookupTableName;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        String status = String.Empty;
                        ObjectType jobtype = ObjectType.None;
                        ObjectAction performedAction = ObjectAction.Unknown;
                        String externalid = String.Empty;
                        Int64 internalid = 0;
                        String description = String.Empty;
                        Int32 auditrefid = 0;
                        String resultXML = String.Empty;

                        if (reader["Status"] != null)
                            status = reader["Status"].ToString();

                        if (reader["ObjectType"] != null)
                            Enum.TryParse(reader["ObjectType"].ToString(), out jobtype);

                        if (reader["PerformedAction"] != null)
                            performedAction = (ObjectAction)Convert.ToByte(reader["PerformedAction"]);

                        if (reader["ExternalID"] != null)
                            externalid = reader["ExternalID"].ToString();

                        if (reader["InternalID"] != null)
                            Int64.TryParse(reader["InternalID"].ToString(), out internalid);

                        if (reader["Description"] != null)
                            description = reader["Description"].ToString();

                        if (reader["OperationResultXml"] != null)
                            resultXML = reader["OperationResultXml"].ToString();

                        if (reader["FK_Audit_Ref"] != null)
                            Int32.TryParse(reader["FK_Audit_Ref"].ToString(), out auditrefid);

                        JobImportResult importresult = new JobImportResult(-1, jobId, status, externalid, internalid, description, resultXML, 0, jobtype, performedAction);

                        jobImportResultCollection.Add(importresult);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return jobImportResultCollection;
        }

        /// <summary>
        /// Gets the import job results for a given job id, objectType and externalId
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="command"></param>
        /// <param name="objectType">ObjectType can be empty string</param>
        /// <param name="externalId">ExternalId can be empty string</param>
        /// <returns>JobImportResultCollection</returns>
        public JobImportResultCollection GetImportResults(Int32 jobId, DBCommandProperties command, JobResultsReturnType resultType, ObjectType objectType, String externalId)
        {
            JobImportResultCollection jobImportResultCollection = new JobImportResultCollection();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_JobImportResult_Summary_GetParametersArray");

                storedProcedureName = "usp_JobManager_Job_Result_Import_Summary_Get";

                parameters[0].Value = jobId;
                parameters[1].Value = resultType;
                parameters[2].Value = ((Byte)objectType).ToString();
                parameters[3].Value = externalId;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        String status = String.Empty;
                        ObjectType resultObjectType = ObjectType.None;
                        ObjectAction performedAction = ObjectAction.Unknown;
                        String externalid = String.Empty;
                        Int64 internalid = 0;
                        String description = String.Empty;
                        Int32 auditrefid = 0;
                        String resultXML = String.Empty;

                        if (reader["Status"] != null)
                            status = reader["Status"].ToString();
                        
                        if (reader["ObjectType"] != null)
                        {
                            try
                            {
                                resultObjectType = (ObjectType)Convert.ToByte(reader["ObjectType"]);
                            }
                            catch
                            {
                            }
                        }

                        if (reader["PerformedAction"] != null)
                            performedAction = (ObjectAction)Convert.ToByte(reader["PerformedAction"]);

                        if (reader["ExternalID"] != null)
                            externalid = reader["ExternalID"].ToString();

                        if (reader["InternalID"] != null)
                            Int64.TryParse(reader["InternalID"].ToString(), out internalid);

                        if (reader["Description"] != null)
                            description = reader["Description"].ToString();

                        if (reader["OperationResultXml"] != null)
                            resultXML = reader["OperationResultXml"].ToString();

                        if (reader["FK_Audit_Ref"] != null)
                            Int32.TryParse(reader["FK_Audit_Ref"].ToString(), out auditrefid);

                        JobImportResult importresult = new JobImportResult(-1, jobId, status, externalid, internalid, description, resultXML, 0, resultObjectType, performedAction);

                        jobImportResultCollection.Add(importresult);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return jobImportResultCollection;
        }

        #endregion
    }

}
