using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Transactions;
using MDM.AdminManager.Business;

namespace MDM.JobManager.Data.SqlClient
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;

    /// <summary>
    /// Specifies job manager Data Access
    /// </summary>
    public class JobDA : SqlClientDataAccessBase
    {
        #region Public Methods

        #region Get Methods

        public JobCollection GetAll(String userName, DBCommandProperties command, DateTime? dateFrom = null, DateTime? dateTo = null, LocaleEnum currentDataLocale = LocaleEnum.UnKnown, Boolean getOnlyUserJobs = false)
        {
            JobCollection jobs = new JobCollection();

            const String storedProcedureName = "usp_JobManager_Job_Get_All";
            SqlDataReader reader = null;

            var sqlBulder = new StringBuilder();

            //And CreateDateTime >=''10/17/2013 12:00:00 AM'' And CreateDateTime <=''10/18/2013 12:00:00 AM''

            if (dateFrom.HasValue)
            {
                sqlBulder.Append(string.Format(" And CreateDateTime >= '{0}'", dateFrom.Value));
            }
            if (dateTo.HasValue)
            {
                sqlBulder.Append(string.Format(" And CreateDateTime <= '{0}'", dateTo.Value));
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("JobManager_Job_Get_All_ParametersArray");

                parameters[0].Value = String.Empty;
                parameters[1].Value = 0;
                parameters[2].Value = userName;
                parameters[3].Value = sqlBulder.ToString();
                parameters[4].Value = 0;
                parameters[5].Value = getOnlyUserJobs;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    jobs = ReadJobs(reader, true, currentDataLocale);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return jobs;
        }

        #endregion

        #region Process Methods

        // todo: ddud this method should replace Process method in future
        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="systemName"></param>
        /// <param name="userName"></param>
        /// <param name="command"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Process(Job job, String systemName, String userName, DBCommandProperties command,
            String programName)
        {
            OperationResult jobProcessOperationResult = new OperationResult();
            using (
                TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    const String storedProcedureName = "usp_JobManager_Job_Process";

                    SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                    SqlParameter[] parameters = generator.GetParameters("JobManager_Job_Process_ParametersArray");

                    String jobData = String.Empty;
                    //if job type is catalog export/BulkOperation then use jobDataxml else jobData object. Reason JobData is not available in catalog export/Entity Export / Bulk operation
                    if (job.JobType == JobType.EntityExport || job.JobType == JobType.BulkOperation)
                    {
                        jobData = job.JobDataXml;
                    }
                    else
                    {
                        jobData = job.JobData.ToXml();
                    }

                    parameters[0].Value = job.Id;
                    parameters[1].Value = Job.GetDBJobType(job.JobType);
                    parameters[2].Value = job.JobSubType.ToString();
                    parameters[3].Value = job.Name;
                    parameters[4].Value = job.Description;
                    parameters[5].Value = jobData;
                    parameters[6].Value = job.JobStatus.ToString();
                    parameters[7].Value = String.Empty;
                    parameters[8].Value = job.ProfileId;
                    parameters[9].Value = job.ProfileName;
                    parameters[10].Value = systemName; // computername..
                    parameters[11].Value = userName;
                    parameters[12].Value = job.Action.ToString();
                    parameters[13].Value = job.Priority;
                    parameters[14].Value = job.ParentJobId;
                    parameters[15].Value = job.FileId;

                    Object value = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                    if (value != null)
                    {
                        job.Id = ValueTypeHelper.Int32TryParse(value.ToString(), job.Id);
                        jobProcessOperationResult.AddOperationResult("", "Job ID: " + job.Id,
                            OperationResultType.Information);
                        jobProcessOperationResult.ReturnValues.Add(job.Id);
                    }
                }
                catch (Exception exception)
                {
                    jobProcessOperationResult.AddOperationResult("", exception.Message, OperationResultType.Error);
                }

                transactionScope.Complete();
            }
            return jobProcessOperationResult;
        }

        #endregion               

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentJobId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable GetChildJobs(int parentJobId, DBCommandProperties command)
        {
            //Invoke stored procedure, code presented as supposition

            SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("JobManager_Job_Get_Hierarchy_ParametersArray");

            parameters[0].Value = parentJobId;

            const string storedProcedureName = "usp_JobManager_JobHierarchy_Get"; // TODO: find proper stored proc

            SqlDataReader reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

            DataTable dt = new DataTable();
            dt.TableName = "Jobs";

            if (reader.HasRows)
            {
                dt.Load(reader);
            }

            return dt;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobType"></param>
        /// <param name="jobSubType"></param>
        /// <param name="jobStatus"></param>
        /// <param name="getKey"></param>
        /// <param name="skipJobDataLoading"></param>
        /// <param name="command"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public JobCollection Get(Int32 jobId, JobType jobType, JobSubType jobSubType, JobStatus jobStatus, Int32 getKey, Boolean skipJobDataLoading, DBCommandProperties command, String userName, DateTime? dateFrom = null, DateTime? dateTo = null, LocaleEnum currentDataLocale = LocaleEnum.UnKnown, Boolean getOnlyUserJobs = false)
        {
            SqlDataReader reader = null;
            JobCollection jobCollection;

            try
            {
                const String storedProcedureName = "usp_JobManager_Job_Get";

                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("JobManager_Job_Get_ParametersArray");

                parameters[0].Value = jobId;
                parameters[1].Value = Job.GetDBJobType(jobType);
                parameters[2].Value = jobSubType.ToString(); // no way to map JobType enum to DB Job sub type..
                parameters[3].Value = jobStatus.ToString();
                parameters[4].Value = getKey;
                parameters[5].Value = skipJobDataLoading;
                parameters[6].Value = userName;
                if (dateFrom == null)
                {
                    parameters[7].Value = DBNull.Value;
                }
                else
                {
                    parameters[7].Value = dateFrom;
                }

                if (dateTo == null)
                {
                    parameters[8].Value = DBNull.Value;
                }
                else
                {
                    parameters[8].Value = dateTo;
                }
                parameters[9].Value = getOnlyUserJobs;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                jobCollection = ReadJobs(reader, skipJobDataLoading, currentDataLocale);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return jobCollection;
        }

        private JobCollection ReadJobs(SqlDataReader reader, Boolean skipJobDataLoading = true, LocaleEnum currentDatalocale = LocaleEnum.UnKnown)
        {
            var jobCollection = new JobCollection();

            var currentThreadCultureName = Thread.CurrentThread.CurrentCulture.Name;
            var currentDataLocaleCultureName = currentDatalocale ==  LocaleEnum.UnKnown ? currentThreadCultureName : currentDatalocale.GetCultureName();

            while (reader.Read())
            {
                Job job = new Job();

                if (reader["JobId"] != null)
                    job.Id = ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), 0);

                if (reader["JobType"] != null)
                    job.JobType = Job.GetJobType(reader["JobType"].ToString());

                if (reader["JobSubType"] != null)
                    job.JobSubType = Job.GetJobSubType(reader["JobsubType"].ToString());

                if (reader["ProfileId"] != null)
                    job.ProfileId = ValueTypeHelper.Int32TryParse(reader["ProfileId"].ToString(), job.Id);

                if (reader["ProfileName"] != null)
                    job.ProfileName = reader["ProfileName"].ToString();

                if (reader["Name"] != null)
                    job.Name = reader["Name"].ToString();

                if (reader["Description"] != null)
                    job.Description = reader["Description"].ToString();

                if (!skipJobDataLoading && reader["JobData"] != null)
                {
                    job.JobDataXml = reader["JobData"].ToString();
                    job.JobData = new JobData(job.JobDataXml);
                }

                if (reader["Status"] != null)
                    job.JobStatus = Job.GetJobStatus(reader["Status"].ToString());

                if (reader["JobAction"] != null)
                {
                    var jobAction = JobAction.None;

                    if (!String.IsNullOrWhiteSpace(reader["JobAction"].ToString()))
                        Enum.TryParse<JobAction>(reader["JobAction"].ToString(), out jobAction);

                    job.JobAction = jobAction;
                }

                if (reader["ParentId"] != null)
                    job.ParentJobId = ValueTypeHelper.Int32TryParse(reader["ParentId"].ToString(), 0);

                if (reader["FileId"] != null)
                    job.FileId = ValueTypeHelper.Int32TryParse(reader["FileId"].ToString(), 0);

                if (reader["ComputerName"] != null)
                    job.ComputerName = reader["ComputerName"].ToString();

                if (reader["UserName"] != null)
                    job.CreatedUser = reader["UserName"].ToString();

                if (reader["CreateDateTime"] != null)
                {
                    String createDateTime = reader["CreateDateTime"].ToString();

                    if (!currentThreadCultureName.Equals(currentDataLocaleCultureName))
                    {
                        createDateTime = FormatHelper.FormatDate(createDateTime, currentThreadCultureName, currentDataLocaleCultureName);
                    }

                    job.CreatedDateTime = createDateTime;
                }

                if (reader["ModDateTime"] != null)
                {
                    String modifiedDateTime = reader["ModDateTime"].ToString();

                    if (!currentThreadCultureName.Equals(currentDataLocaleCultureName))
                    {
                        modifiedDateTime = FormatHelper.FormatDate(modifiedDateTime, currentThreadCultureName, currentDataLocaleCultureName);
                    }

                    job.ModifiedDateTime = modifiedDateTime;
                }

                if (reader["Priority"] != null)
                    job.Priority = ValueTypeHelper.Int32TryParse(reader["Priority"].ToString(), job.Priority);

                if (reader["FileName"] != null)
                    job.FileName = reader["FileName"].ToString();

                jobCollection.Add(job);
            }
            return jobCollection;
        }

        #endregion

        #region Legacy methods

        public class JobLegacyMethods : SqlClientDataAccessBase
        {
            #region Fields

            #endregion

            #region Constructors

            #endregion

            #region Properties

            #endregion

            #region Methods

            /// <summary>
            /// Get all jobs of specific job type.
            /// </summary>
            /// <param name="type">This parameter specifying job type.</param>
            /// <param name="userName">This parameter specifying login user name.</param>
            /// <param name="sql">This parameter is to filter search data based on sql.</param>
            /// <param name="command">This parameter is specifying to which server we have to connect.</param>
            /// <returns>Collection of Job objects based on parameter.</returns>
            public Collection<Job> Get(String type, String userName, String sql, DBCommandProperties command)
            {
                SqlParameter[] parameters;
                SqlDataReader reader = null;
                String storedProcedureName = String.Empty;
                Collection<Job> jobCollection = new Collection<Job>();

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                    parameters = generator.GetParameters("JobManager_Job_Legacy_Get_ParametersArray");

                    parameters[0].Value = type;
                    parameters[1].Value = userName;
                    parameters[2].Value = sql;

                    storedProcedureName = "usp_N_getJobServiceItems";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    while (reader.Read())
                    {
                        String name = String.Empty;
                        Int32 id = 0;
                        String jobType = String.Empty;
                        String subType = String.Empty;
                        String profileName = String.Empty;
                        String shortName = String.Empty;
                        String description = String.Empty;
                        String createdBy = String.Empty;
                        String createdDateTime = String.Empty;
                        String modifiedDateTime = String.Empty;
                        String status = String.Empty;
                        String fileName = String.Empty;
                        Int32 priority = 0;

                        if (reader["JobName"] != null)
                            name = reader["JobName"].ToString();
                        if (reader["PK_JobService"] != null)
                            Int32.TryParse(reader["PK_JobService"].ToString(), out id);
                        if (reader["jobtype"] != null)
                            jobType = reader["jobtype"].ToString();
                        if (reader["jobsubtype"] != null)
                            subType = reader["jobsubtype"].ToString();
                        if (reader["ProfileName"] != null)
                            profileName = reader["ProfileName"].ToString();
                        if (reader["ShortName"] != null)
                            shortName = reader["ShortName"].ToString();
                        if (reader["Description"] != null)
                            description = reader["Description"].ToString();
                        if (reader["Username"] != null)
                            createdBy = reader["Username"].ToString();
                        if (reader["CreateDateTime"] != null)
                            createdDateTime = reader["CreateDateTime"].ToString();
                        if (reader["ModDateTime"] != null)
                            modifiedDateTime = reader["ModDateTime"].ToString();
                        if (reader["Status"] != null)
                            status = reader["Status"].ToString();
                        if (reader["FileName"] != null)
                            fileName = reader["FileName"].ToString();
                        if (reader["Priority"] != null)
                            Int32.TryParse(reader["Priority"].ToString(), out priority);

                        Job job = new Job(id, shortName, name, profileName, description, priority, jobType, subType, createdBy, createdDateTime, modifiedDateTime, status);

                        //Add fileName into JobParameter.
                        job.JobData.JobParameters.Add(new JobParameter("FileName", fileName));
                        //TODO vitaliy: remove comments, when usp_N_getJobServiceItems will be extended

                        if (reader["ParentJobId"] != null)
                            job.ParentJobId = ValueTypeHelper.Int32TryParse(reader["ParentJobId"].ToString(), 0);


                        jobCollection.Add(job);
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
            /// Add job details to Database server
            /// </summary>
            /// <param name="type">This parameter is specifying job type.</param>
            /// <param name="subType">This parameter is specifying job sub type.</param>
            /// <param name="profileName">This parameter is specifying profile name.</param>
            /// <param name="shortName">This parameter is specifying job short name.</param>
            /// <param name="description">This parameter is specifying job description.</param>
            /// <param name="content">This parameter is specifying job content.</param>
            /// <param name="status">This parameter is specifying job status.</param>
            /// <param name="systemName">This parameter is specifying system name.</param>
            /// <param name="loginUser">This parameter is specifying job</param>
            /// <param name="command">This parameter is specifying to which server we have to connect.</param>
            /// <returns>Number of jobs created.</returns>
            public Int32 Add(String type, String subType, String profileName, String shortName, String description, String content, String status, String systemName, String loginUser, DBCommandProperties command)
            {
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;
                Int32 Id = 0;

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    try
                    {
                        SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                        parameters = generator.GetParameters("JobManager_Job_Legacy_Add_ParametersArray");

                        parameters[0].Value = type;
                        parameters[1].Value = subType;
                        parameters[2].Value = profileName;
                        parameters[3].Value = shortName;
                        parameters[4].Value = description;
                        parameters[5].Value = content;
                        parameters[6].Value = status;
                        parameters[7].Value = systemName;
                        parameters[8].Value = loginUser;

                        storedProcedureName = "usp_N_addJobService";

                        Object value = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                        if (value != null)
                        {
                            Int32.TryParse(value.ToString(), out Id);
                        }
                    }
                    finally
                    {
                    }

                    transactionScope.Complete();
                }
                return Id;
            }

            /// <summary>
            /// Update UserAction of Job
            /// </summary>
            /// <param name="jobId">Id of the Job</param>
            /// <param name="userAction">UserAction of Job. UserAction can be Pause/Continue/Abort/Retry/Delete</param>
            /// <param name="command">command having Connection Properties</param>
            /// <returns>True if Update is successful</returns>
            public Boolean UpdateUserAction(Int32 jobId, JobAction userAction, DBCommandProperties command)
            {
                SqlParameter[] parameters;
                Object value = null;
                String storedProcedureName = String.Empty;
                Boolean result = false;
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    try
                    {
                        SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                        parameters = generator.GetParameters("JobManager_Job_Legacy_UpdateUserAction_ParametersArray");

                        parameters[0].Value = jobId;
                        parameters[1].Value = userAction.ToString();

                        storedProcedureName = "usp_N_updateJobUserAction";

                        value = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                        if (value != null)
                        {
                            if (ValueTypeHelper.Int32TryParse(value.ToString(), 0) > 0)
                                result = true;
                        }
                    }
                    finally
                    {
                    }

                    transactionScope.Complete();
                }
                return result;
            }

            /// <summary>
            /// Get Job status details by the id.
            /// </summary>
            /// <param name="id">This parameter is specifying an job</param>
            /// <param name="command">This parameter is specifying to which server we have to connect.</param>
            /// <returns>return a job details in form of XML.</returns>
            public String GetJobItem(Int32 id, DBCommandProperties command)
            {
                SqlParameter[] parameters;
                SqlDataReader reader = null;
                String storedProcedureName = String.Empty;
                StringBuilder jobXml = new StringBuilder();

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                    parameters = generator.GetParameters("JobManager_Job_Legacy_GetItem_ParametersArray");

                    parameters[0].Value = id;

                    storedProcedureName = "usp_N_getJobServiceItem_XML";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    while (reader.Read())
                    {
                        if (reader != null)
                        {
                            jobXml.Append(reader[0].ToString());
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                return jobXml.ToString();
            }

            #endregion
        }

        #endregion
    }
}