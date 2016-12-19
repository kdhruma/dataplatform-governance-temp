using System;
using System.Data.SqlClient;
using System.Transactions;

namespace MDM.JobManager.Data.SqlClient
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Denorm;
    using MDM.Utility;

    /// <summary>
    /// Specifies denorm Data Access
    /// </summary>
    public class DenormDA : SqlClientDataAccessBase
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get each job step details based on job id.
        /// </summary>
        /// <param name="jobId">This parameter specifying jobId.</param>
        /// <param name="command">Object having command properties</param>
        /// <returns></returns>
        public Job GetJobStepDetails(Int32 jobId, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            Job job = new Job();
            JobExecutionStepCollection jobExecutionStepCollection = new JobExecutionStepCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_Denorm_GetJobStep_GetParametersArray");

                parameters[0].Value = jobId;

                storedProcedureName = "usp_JobManager_Denorm_JobStep_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["JobId"] != null)
                            job.Id = ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), job.Id);

                        #region Adding Job Execution Step

                        JobExecutionStep jobExecutionStep = new JobExecutionStep();

                        if (reader["StepName"] != null)
                            jobExecutionStep.Name = reader["StepName"].ToString();

                        if (reader["StepNumber"] != null)
                            jobExecutionStep.Id = ValueTypeHelper.Int32TryParse(reader["StepNumber"].ToString(), jobExecutionStep.Id);

                        #endregion

                        #region Adding  Execution Status for each Step

                        ExecutionStatus executionStatus = new ExecutionStatus();

                        if (reader["FinishedRecordCount"] != null)
                            executionStatus.TotalElementsProcessed = ValueTypeHelper.Int64TryParse(reader["FinishedRecordCount"].ToString(), executionStatus.TotalElementsProcessed);

                        if (reader["TotalRecordCount"] != null)
                            executionStatus.TotalElementsToProcess = ValueTypeHelper.Int64TryParse(reader["TotalRecordCount"].ToString(), executionStatus.TotalElementsToProcess);

                        if (reader["Progress"] != null)
                            executionStatus.OverAllProgress = ValueTypeHelper.Int32TryParse(reader["Progress"].ToString(), executionStatus.OverAllProgress);

                        if (reader["HasError"] != null)
                            executionStatus.CurrentStatusMessage = reader["HasError"].ToString();

                        if (reader["StartDateTime"] != null)
                            executionStatus.StartTime = reader["StartDateTime"].ToString();

                        if (reader["FinishDateTime"] != null)
                            executionStatus.EndTime = reader["FinishDateTime"].ToString();

                        #endregion

                        jobExecutionStep.ExecutionStatus = executionStatus;

                        jobExecutionStepCollection.Add(jobExecutionStep);
                    }

                    JobData jobData = new JobData();
                    //Assigning Collection of Execution step.
                    jobData.JobExecutionStep = jobExecutionStepCollection;
                    job.JobData = jobData;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return job;
        }

        /// <summary>
        /// Get job type
        /// </summary>
        /// <param name="command">Object having command properties</param>
        /// <returns>Collection of job type</returns>
        public JobCollection GetJobType(DBCommandProperties command)
        {
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            JobCollection jobCollection = new JobCollection();

            try
            {
                storedProcedureName = "usp_JobManager_Denorm_JobType_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, null, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Job job = new Job();

                        if (reader["JobType"] != null)
                            job.JobType = Job.GetJobType(reader["JobType"].ToString());

                        if (reader["JobName"] != null)
                            job.Name = reader["JobName"].ToString();

                        jobCollection.Add(job);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return jobCollection;
        }

        /// <summary>
        /// Get all collection of job based on job type,fromDate,toDate etc.
        /// </summary>
        /// <param name="jobType">This parameter specifying type of job.</param>
        /// <param name="fromDate">This parameter specifying fromDate.</param>
        /// <param name="toDate">This parameter specifying toDate.</param>
        /// <param name="displayNonEmptyJobs">This parameter specifying whether display empty job or not.</param>
        /// <param name="displayErrorJobs">This parameter specifying whether display error job or not.</param>
        /// <param name="displayRows">This parameter specifying how many job user want to display.</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Collection of job</returns>
        public JobCollection GetJobList(JobType jobType, String fromDate, String toDate, Boolean displayNonEmptyJobs, Boolean displayErrorJobs, Int32 displayRows, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            JobCollection jobCollection = new JobCollection();
            JobParameter jobParameter;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_Denorm_GetJobList_GetParametersArray");

                parameters[0].Value = Job.GetDBJobType(jobType);
                parameters[1].Value = fromDate;
                parameters[2].Value = toDate;
                parameters[3].Value = displayNonEmptyJobs;
                parameters[4].Value = displayErrorJobs;
                parameters[5].Value = displayRows;

                storedProcedureName = "usp_JobManager_Denorm_Job_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        #region Adding Job details into Job object

                        Job job = new Job();


                        if (reader["JobId"] != null)
                            job.Id = ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), job.Id);

                        if (reader["JobName"] != null)
                            job.Name = reader["JobName"].ToString();

                        if (reader["IsRunning"] != null)
                        {
                            if (reader["IsRunning"].ToString() == "False")
                                job.JobStatus = JobStatus.Completed;
                            else
                                job.JobStatus = JobStatus.Running;
                        }

                        #endregion

                        #region Adding additional parameter to Job object

                        JobParameterCollection jobParameterCollection = new JobParameterCollection();

                        jobParameter = new JobParameter("CNodeCount", reader["CNodeCount"].ToString());
                        jobParameterCollection.Add(jobParameter);

                        jobParameter = new JobParameter("CNodeAttrCount", reader["CNodeAttrCount"].ToString());
                        jobParameterCollection.Add(jobParameter);

                        jobParameter = new JobParameter("RelationshipCount", reader["RelationshipCount"].ToString());
                        jobParameterCollection.Add(jobParameter);

                        #endregion

                        #region Adding Exectuion status of each job

                        ExecutionStatus executionStatus = new ExecutionStatus();

                        if (reader["HasError"] != null)
                            executionStatus.CurrentStatusMessage = reader["HasError"].ToString();

                        if (reader["StartDateTime"] != null)
                            executionStatus.StartTime = reader["StartDateTime"].ToString();

                        if (reader["FinishDateTime"] != null)
                            executionStatus.EndTime = reader["FinishDateTime"].ToString();

                        #endregion

                        JobData jobData = new JobData();
                        jobData.ExecutionStatus = executionStatus;
                        jobData.JobParameters = jobParameterCollection;

                        job.JobData = jobData;

                        jobCollection.Add(job);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return jobCollection;
        }

        /// <summary>
        /// Get Collection of job with execution step details & result of each step.
        /// </summary>
        /// <param name="mode">This parameter specifying mode.</param>
        /// <param name="jobId">This parameter specifying type jobId.</param>
        /// <param name="displayRows">This parameter specifying how many job user want to display.</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Collection of Job & it's result.</returns>
        public Tuple<JobCollection, DenormResultCollection, Int32> GetJobErrorDetails(String mode, Int32 jobId, Int32 displayRows, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            Tuple<JobCollection, DenormResultCollection, Int32> tupleCollection = null;
            JobCollection jobCollection = new JobCollection();
            DenormResultCollection denormResultCollection = denormResultCollection = new DenormResultCollection();
            JobExecutionStepCollection jobExecutionStepCollection = new JobExecutionStepCollection();
            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();
            EntityOperationResult entityOpeartionResult = new EntityOperationResult();
            Int32 recordCount = 0;
            Boolean result;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_Denorm_GetJobErrorDetails_GetParametersArray");

                parameters[0].Value = mode;

                if (jobId == 0)
                    parameters[1].Value = null;
                else
                    parameters[1].Value = jobId;
                parameters[2].Value = displayRows;

                storedProcedureName = "usp_JobManager_Denorm_JobError_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 entityId = 0;
                        Int32 stepId = 0;
                        Int32 errorId = 0;

                        if (reader["CNodeID"] != null)
                            entityId = ValueTypeHelper.Int64TryParse(reader["CNodeID"].ToString(), entityOpeartionResult.EntityId);

                        if (reader["StepID"] != null)
                            stepId = ValueTypeHelper.Int32TryParse(reader["StepID"].ToString(), 0);

                        if (reader["ErrorID"] != null)
                            errorId = ValueTypeHelper.Int32TryParse(reader["ErrorID"].ToString(), 0);

                        #region Add Jobs in Job Collection

                        Job job = new Job();

                        if (reader["JobID"] != null)
                            job.Id = ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), job.Id);

                        //Check jobId is already exists or not in JobCollection.
                        result = jobCollection.Contains(ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), 0));

                        if (!result)
                        {
                            //No Job Id found means add job & it's step details

                            JobExecutionStep jobExecutionStep = new JobExecutionStep();
                            jobExecutionStepCollection = new JobExecutionStepCollection();

                            jobExecutionStep.Id = stepId;

                            if (reader["StepName"] != null)
                                jobExecutionStep.Name = reader["StepName"].ToString();

                            jobExecutionStepCollection.Add(jobExecutionStep);

                            JobData jobData = new JobData();

                            jobData.JobExecutionStep = jobExecutionStepCollection;
                            job.JobData = jobData;

                            jobCollection.Add(job);
                        }
                        else
                        {

                            Job filteredJob = jobCollection[job.Id];

                            //Check executionStepId is already exists or not in ExecutionStepCollection of filtered job.
                            result = filteredJob.JobData.JobExecutionStep.Contains(ValueTypeHelper.Int32TryParse(reader["StepID"].ToString(), 0));

                            if (!result)
                            {
                                JobExecutionStep jobExecutionStep = new JobExecutionStep();

                                jobExecutionStep.Id = stepId;

                                if (reader["StepName"] != null)
                                    jobExecutionStep.Name = reader["StepName"].ToString();

                                filteredJob.JobData.JobExecutionStep.Add(jobExecutionStep);
                            }
                        }

                        #endregion

                        #region Add Denorm Result Per job

                        DenormResult denormResult = new DenormResult();

                        //Check for specific jobId  is exists in DenormResultCollection or not.
                        result = denormResultCollection.Contains(job.Id);
                        if (!result)
                        {
                            entityOpeartionResult = new EntityOperationResult();
                            entityOperationResultCollection = new EntityOperationResultCollection();

                            denormResult.JobId = job.Id;

                            entityOpeartionResult.EntityId = entityId;

                            if (reader["CNodeShortName"] != null)
                                entityOpeartionResult.EntityLongName = reader["CNodeShortName"].ToString();

                            Error error = new Error();
                            error.ErrorCode = errorId.ToString();
                            String errorMessage = String.Concat(reader["ErrorMessage"].ToString(), "#",
                                                                   reader["RetryCount"].ToString(), "#",
                                                                   reader["Status"].ToString(), "#",
                                                                   reader["ModDateTime"].ToString(), "#",
                                                                   reader["CatalogID"].ToString());

                            error.ErrorMessage = errorMessage;

                            entityOpeartionResult.Errors.Add(error);
                            entityOperationResultCollection.Add(entityOpeartionResult);

                            denormResult.StepResult[stepId] = entityOperationResultCollection;

                            denormResultCollection.Add(denormResult);
                        }
                        else
                        {
                            result = denormResultCollection.Contains(job.Id);
                            if (result)
                            {
                                denormResult = denormResultCollection[job.Id];

                                //Check StepId is exists as key in dictionary or not.
                                if (denormResult.StepResult.ContainsKey(stepId))
                                {
                                    // Check entity Id is exists in Entity Operation Result Collection or not.
                                    result = entityOperationResultCollection.Contains(entityId);
                                    if (!result)
                                    {
                                        entityOpeartionResult = new EntityOperationResult();

                                        entityOpeartionResult.EntityId = entityId;

                                        if (reader["CNodeShortName"] != null)
                                            entityOpeartionResult.EntityLongName = reader["CNodeShortName"].ToString();

                                        Error error = new Error();
                                        error.ErrorCode = errorId.ToString();
                                        String errorMessage = String.Concat(reader["ErrorMessage"].ToString(), "#",
                                                                               reader["RetryCount"].ToString(), "#",
                                                                               reader["Status"].ToString(), "#",
                                                                               reader["ModDateTime"].ToString(), "#",
                                                                               reader["CatalogID"].ToString());

                                        error.ErrorMessage = errorMessage;

                                        entityOpeartionResult.Errors.Add(error);
                                        entityOperationResultCollection.Add(entityOpeartionResult);
                                    }
                                    else
                                    {
                                        Error error = new Error();
                                        entityOpeartionResult = new EntityOperationResult();
                                        entityOpeartionResult = entityOperationResultCollection[entityId];

                                        if (reader["ErrorID"] != null)
                                        {
                                            error.ErrorCode = reader["ErrorID"].ToString();
                                            String errorMessage = String.Concat(reader["ErrorMessage"].ToString(), "#",
                                                                               reader["RetryCount"].ToString(), "#",
                                                                               reader["Status"].ToString(), "#",
                                                                               reader["ModDateTime"].ToString(), "#",
                                                                               reader["CatalogID"].ToString());

                                            error.ErrorMessage = errorMessage;
                                            entityOpeartionResult.Errors.Add(error);
                                        }
                                    }
                                }
                                else
                                {
                                    //No StepId is exists in dictionary so directly add new key & value.

                                    entityOpeartionResult = new EntityOperationResult();
                                    entityOperationResultCollection = new EntityOperationResultCollection();

                                    entityOpeartionResult.EntityId = entityId;

                                    if (reader["CNodeShortName"] != null)
                                        entityOpeartionResult.EntityLongName = reader["CNodeShortName"].ToString();

                                    Error error = new Error();
                                    error.ErrorCode = errorId.ToString();
                                    String errorMessage = String.Concat(reader["ErrorMessage"].ToString(), "#",
                                                                           reader["RetryCount"].ToString(), "#",
                                                                           reader["Status"].ToString(), "#",
                                                                           reader["ModDateTime"].ToString(), "#",
                                                                           reader["CatalogID"].ToString());

                                    error.ErrorMessage = errorMessage;

                                    entityOpeartionResult.Errors.Add(error);
                                    entityOperationResultCollection.Add(entityOpeartionResult);

                                    denormResult.StepResult[stepId] = entityOperationResultCollection;
                                }
                            }
                        }

                        #endregion
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (reader[0] != null)
                                recordCount = ValueTypeHelper.Int32TryParse(reader[0].ToString(), 0);
                        }
                    }
                }
                tupleCollection = new Tuple<JobCollection, DenormResultCollection, Int32>(jobCollection, denormResultCollection, recordCount);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return tupleCollection;
        }

        /// <summary>
        /// Get current job status.
        /// </summary>
        /// <param name="command">Object having command properties</param>
        /// <returns>Current Job object</returns>
        public Job GetCurrentJobStatus(DBCommandProperties command)
        {
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            Job job = new Job();
            ExecutionStatus executionStatus = new ExecutionStatus();
            JobData jobData = new JobData();
            JobExecutionStepCollection jobExecutionStepCollection = new JobExecutionStepCollection();

            try
            {
                storedProcedureName = "usp_JobManager_Denorm_JobStatus_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, null, storedProcedureName);

                if (reader != null)
                {
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            #region Adding Job details into Job object

                            if (reader["JobId"] != null)
                                job.Id = ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), job.Id);

                            if (reader["JobName"] != null)
                                job.Name = reader["JobName"].ToString();

                            if (reader["Locked"] != null)
                                job.IsEnable = ValueTypeHelper.ConvertToBoolean(reader["Locked"].ToString());

                            if (reader["IsRunning"] != null)
                            {
                                if (reader["IsRunning"].ToString() == "False")
                                    job.JobStatus = JobStatus.Completed;
                                else
                                    job.JobStatus = JobStatus.Running;
                            }

                            #endregion

                            #region Adding Exectuion status of each job

                            executionStatus = new ExecutionStatus();

                            if (reader["HasError"] != null)
                                executionStatus.CurrentStatusMessage = reader["HasError"].ToString();

                            if (reader["StartDateTime"] != null)
                                executionStatus.StartTime = reader["StartDateTime"].ToString();

                            if (reader["FinishDateTime"] != null)
                                executionStatus.EndTime = reader["FinishDateTime"].ToString();

                            #endregion

                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            #region Adding Execution step for Current Job

                            JobExecutionStep jobExecutionStep = new JobExecutionStep();

                            if (reader["PK_DN_Step"] != null)
                                jobExecutionStep.PK_DN_Step = ValueTypeHelper.Int32TryParse(reader["PK_DN_Step"].ToString(), jobExecutionStep.PK_DN_Step);

                            if (reader["StepName"] != null)
                                jobExecutionStep.Name = reader["StepName"].ToString();

                            if (reader["StepNumber"] != null)
                                jobExecutionStep.Id = ValueTypeHelper.Int32TryParse(reader["StepNumber"].ToString(), jobExecutionStep.Id);

                            #endregion

                            #region Adding Execution Status of each step for current job

                            executionStatus = new ExecutionStatus();

                            if (reader["TotalSteps"] != null)
                                executionStatus.TotalSteps = ValueTypeHelper.Int32TryParse(reader["TotalSteps"].ToString(), executionStatus.TotalSteps);

                            if (reader["FinishedRecordCount"] != null)
                                executionStatus.TotalElementsProcessed = ValueTypeHelper.Int64TryParse(reader["FinishedRecordCount"].ToString(), executionStatus.TotalElementsProcessed);

                            if (reader["TotalRecordCount"] != null)
                                executionStatus.TotalElementsToProcess = ValueTypeHelper.Int64TryParse(reader["TotalRecordCount"].ToString(), executionStatus.TotalElementsToProcess);

                            if (reader["HasError"] != null)
                                executionStatus.CurrentStatusMessage = reader["HasError"].ToString();

                            if (reader["StartDateTime"] != null)
                                executionStatus.StartTime = reader["StartDateTime"].ToString();

                            if (reader["FinishDateTime"] != null)
                                executionStatus.EndTime = reader["FinishDateTime"].ToString();

                            #endregion

                            jobExecutionStep.ExecutionStatus = executionStatus;

                            jobExecutionStepCollection.Add(jobExecutionStep);
                        }

                        jobData.JobExecutionStep = jobExecutionStepCollection;
                        job.JobData = jobData;

                        job.JobData.ExecutionStatus = executionStatus;

                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return job;
        }

        /// <summary>
        /// release denorm lock.
        /// </summary>
        /// <param name="command">Object having command properties</param>
        public void ReleaseLock(DBCommandProperties command)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlDataReader reader = null;
                String storedProcedureName = String.Empty;

                try
                {
                    storedProcedureName = "usp_JobManager_Denorm_Lock_Release";

                    reader = ExecuteProcedureReader(command.ConnectionString, null, storedProcedureName);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();
            }
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}