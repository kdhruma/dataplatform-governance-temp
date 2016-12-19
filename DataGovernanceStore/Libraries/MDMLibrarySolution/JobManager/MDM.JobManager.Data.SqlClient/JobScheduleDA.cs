using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.JobManager.Data.SqlClient
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.BusinessObjects.DQM;
    using MDM.BusinessObjects.DQMNormalization;
    using MDM.BusinessObjects.Exports;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Specifies job manager Data Access
    /// </summary>
    public class JobScheduleDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Get Job schedule

        /// <summary>
        /// Get all the job Schedules
        /// </summary>
        /// <param name="scheduleId">This parameter is specifying the Schedule Id</param>
        /// <param name="command">This parameter is specifying to which server we have to connect.</param>
        /// <returns>Returns job collections and profile collections</returns>
        public Tuple<Collection<Job>, Collection<ExportProfile>> GetSchedule(Int32 scheduleId, DBCommandProperties command)
        {
            Tuple<Collection<Job>, Collection<ExportProfile>> scheduleTuple = null;
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            Collection<ExportProfile> jobProfileCollection = new Collection<ExportProfile>();
            Collection<Job> jobScheduleCollection = new Collection<Job>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_JobSchedule_Get_ParametersArray");

                parameters[0].Value = scheduleId;

                storedProcedureName = "usp_JobManager_JobSchedule_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Int32 id = 0;
                    String shortName = String.Empty;
                    String longName = String.Empty;
                    String content = String.Empty;
                    Boolean isEnable = false;
                    String modifiedDateTime = String.Empty;
                    String createdBy = String.Empty;
                    String nextRunDate = String.Empty;
                    String lastRunStatus = String.Empty;
                    String computerName = String.Empty;

                    if (reader["PK_JobSchedule"] != null)
                        Int32.TryParse(reader["PK_JobSchedule"].ToString(), out id);
                    if (reader["ShortName"] != null)
                        shortName = reader["ShortName"].ToString();
                    if (reader["LongName"] != null)
                        longName = reader["LongName"].ToString();
                    if (reader["ScheduleData"] != null)
                        content = reader["ScheduleData"].ToString();
                    if (reader["Enabled"] != null)
                        isEnable = Convert.ToBoolean(reader["Enabled"]);
                    if (reader["ModDateTime"] != null)
                        modifiedDateTime = reader["ModDateTime"].ToString();
                    if (reader["CreateUser"] != null)
                        createdBy = reader["CreateUser"].ToString();
                    if (reader["NextRunDate"] != null)
                        nextRunDate = reader["NextRunDate"].ToString();
                    if (reader["LastRunStatus"] != null)
                        lastRunStatus = reader["LastRunStatus"].ToString();
                    if (reader["ComputerName"] != null)
                        computerName = reader["ComputerName"].ToString();

                    Job job = new Job(id, shortName, longName, content, isEnable, modifiedDateTime, createdBy, nextRunDate, lastRunStatus, computerName);
                    jobScheduleCollection.Add(job);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        Int32 jobScheduleId = 0;
                        Int32 profileId = 0;
                        String profileName = String.Empty;
                        String content = String.Empty;
                        ExportProfileType profileType = ExportProfileType.Unknown;

                        if (reader["FK_JobSchedule"] != null)
                            Int32.TryParse(reader["FK_JobSchedule"].ToString(), out jobScheduleId);
                        if (reader["FK_Profiles"] != null)
                            Int32.TryParse(reader["FK_Profiles"].ToString(), out profileId);
                        if (reader["ProfileName"] != null)
                            profileName = reader["ProfileName"].ToString();
                        if (reader["ProfileType"] != null)
                            Enum.TryParse<ExportProfileType>(reader["ProfileType"].ToString(), out profileType);

                        ExportProfile profile = new ExportProfile() { Id = profileId, Name = profileName, ProfileType = profileType };
                        jobProfileCollection.Add(profile);
                    }
                }
                scheduleTuple = new Tuple<Collection<Job>, Collection<ExportProfile>>(jobScheduleCollection, jobProfileCollection);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return scheduleTuple;
        }

        /// <summary>
        /// Get All jobSchedules from the system
        /// </summary>
        /// <param name="command">Connection string and other command related property</param>
        /// <returns>Collection of jobSchedules</returns>
        public JobScheduleCollection Get(Int32 scheduleId, DBCommandProperties command)
        {
            const String LegacyProfileType = "Catalog Export";
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            JobScheduleCollection jobScheduleCollection = new JobScheduleCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                parameters = generator.GetParameters("JobManager_JobSchedule_Get_ParametersArray");

                parameters[0].Value = scheduleId;

                storedProcedureName = "usp_JobManager_JobSchedule_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Int32 id = 0;
                    String shortName = String.Empty;
                    String longName = String.Empty;
                    String content = String.Empty;
                    Boolean isEnable = false;
                    String modifiedDateTime = String.Empty;
                    String createdBy = String.Empty;
                    String nextRunDate = String.Empty;
                    String lastRunStatus = String.Empty;
                    String computerName = String.Empty;

                    if (reader["PK_JobSchedule"] != null)
                        Int32.TryParse(reader["PK_JobSchedule"].ToString(), out id);
                    if (reader["ShortName"] != null)
                        shortName = reader["ShortName"].ToString();
                    if (reader["LongName"] != null)
                        longName = reader["LongName"].ToString();
                    if (reader["ScheduleData"] != null)
                        content = reader["ScheduleData"].ToString();
                    if (reader["Enabled"] != null)
                        isEnable = Convert.ToBoolean(reader["Enabled"]);
                    if (reader["ModDateTime"] != null)
                        modifiedDateTime = reader["ModDateTime"].ToString();
                    if (reader["CreateUser"] != null)
                        createdBy = reader["CreateUser"].ToString();
                    if (reader["NextRunDate"] != null)
                        nextRunDate = reader["NextRunDate"].ToString();
                    if (reader["LastRunStatus"] != null)
                        lastRunStatus = reader["LastRunStatus"].ToString();
                    if (reader["ComputerName"] != null)
                        computerName = reader["ComputerName"].ToString();

                    //Job job = new Job(id, shortName, longName, content, isEnable, modifiedDateTime, createdBy,nextRunDate,lastRunStatus,computerName);
                    JobSchedule schedule = new JobSchedule();
                    schedule.Id = id;
                    schedule.Name = shortName;
                    schedule.LongName = longName;
                    schedule.ScheduleCriteria = ScheduleCriteria.CreateFromXml(content);
                    schedule.Enabled = isEnable;
                    schedule.LastModofiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(modifiedDateTime);
                    schedule.CreateUserName = createdBy;
                    schedule.NextRunDate = ValueTypeHelper.ConvertToNullableDateTime(nextRunDate);
                    schedule.LastRunStatus = lastRunStatus;
                    schedule.ComputerName = computerName;

                    jobScheduleCollection.Add(schedule);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        Int32 jobScheduleId = 0;
                        Int32 profileId = 0;
                        String profileName = String.Empty;
                        String profileLongName = String.Empty;
                        String profileType = null;

                        if (reader["FK_JobSchedule"] != null)
                            Int32.TryParse(reader["FK_JobSchedule"].ToString(), out jobScheduleId);
                        if (reader["FK_Profiles"] != null)
                            Int32.TryParse(reader["FK_Profiles"].ToString(), out profileId);
                        if (reader["ProfileName"] != null)
                            profileName = reader["ProfileName"].ToString();
                        if (reader["ProfileLongName"] != null)
                            profileLongName = reader["ProfileLongName"].ToString();
                        if (reader["ProfileType"] != null)
                            profileType = reader["ProfileType"].ToString();

                        BaseProfile profile = CreateProfile(profileType);
                        profile.Id = profileId;
                        profile.Name = profileName;
                        profile.LongName = profileLongName;

                        if (profile is ExportProfile)
                        {
                            profile.ExtendedProperties = profileType ?? LegacyProfileType;
                        }

                        IJobSchedule schedule = jobScheduleCollection.GetJobSchedule(jobScheduleId);

                        if (schedule != null)
                        {
                            schedule.AddProfile(profile);
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return jobScheduleCollection;
        }

        #endregion Get Job schedule

        #region CUD Job schedule

        /// <summary>
        /// Process schedule criteria
        /// </summary>
        /// <param name="jobSchedule">Schedule criteria to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of schedule criteria</returns>
        public OperationResult Process(JobSchedule jobSchedule, String programName, String userName, DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("JobScheduleDA.Process", false);

            SqlDataReader reader = null;
            OperationResult jobScheduleProcessOperationResult = new OperationResult();

            SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

            const String storedProcedureName = "usp_JobManager_JobSchedule_Process";


            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    SqlParameter[] parameters = generator.GetParameters("JobManager_JobSchedule_Process_ParametersArray");

                    #region Populate profile table value parameters

                    List<SqlDataRecord> profileTableList = new List<SqlDataRecord>();
                    SqlMetaData[] profileMetadata = generator.GetTableValueMetadata("JobManager_JobSchedule_Process_ParametersArray", parameters[9].ParameterName);

                    SqlDataRecord profileResultRecord = null;

                    foreach (BaseProfile profile in jobSchedule.Profiles)
                    {
                        profileResultRecord = new SqlDataRecord(profileMetadata);
                        profileResultRecord.SetValue(0, Convert.ToString(profile.Id));
                        profileResultRecord.SetValue(1, Convert.ToString(GetProfileTypeName(profile)));
                        profileTableList.Add(profileResultRecord);
                    }

                    if (profileTableList.Count < 1)
                    {
                        profileTableList = null;
                    }

                    #endregion Populate profile table value parameters

                    parameters[0].Value = jobSchedule.Id;
                    parameters[1].Value = jobSchedule.Name;
                    parameters[2].Value = jobSchedule.LongName;
                    parameters[3].Value = jobSchedule.ScheduleCriteria.GenerateXml();
                    parameters[4].Value = jobSchedule.Enabled.ToString();
                    parameters[5].Value = jobSchedule.ComputerName;
                    parameters[6].Value = userName;
                    parameters[7].Value = programName;
                    parameters[8].Value = jobSchedule.Action.ToString();
                    parameters[9].Value = profileTableList;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update jobSchedule object with Actual Id in case of create
                    UpdateJobScheduleAndOperationResult(reader, jobSchedule, jobScheduleProcessOperationResult);

                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "JobSchedule Process Failed." + exception.Message);
                    jobScheduleProcessOperationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return jobScheduleProcessOperationResult;
        }

        /// <summary>
        /// Add the job schedule details.
        /// </summary>
        /// <param name="scheduleName">This parameter is specifying the Schedule Name</param>
        /// <param name="scheduleLongName">This parameter is specifying the Schedule Long name</param>
        /// <param name="content">This parameter is specifying the Schedule content</param>
        /// <param name="isEnable">This parameter is specifying whether it is enable or not</param>
        /// <param name="systemName">This parameter is specifying the system name</param>
        /// <param name="profiles">This parameter is specifying the profiles</param>
        /// <param name="loginUser">This parameter is specifying the login user</param>
        /// <param name="programName">This parameter is specifying the program Name</param>
        /// <param name="command">This parameter is specifying to which server we have to connect.</param>
        public void AddSchedule(String scheduleName, String scheduleLongName, String content, Boolean isEnable, String systemName, String profiles, String loginUser, String programName, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                    parameters = generator.GetParameters("JobManager_JobSchedule_Add_ParametersArray");

                    parameters[0].Value = scheduleName;
                    parameters[1].Value = scheduleLongName;
                    parameters[2].Value = content;
                    parameters[3].Value = isEnable;
                    parameters[4].Value = systemName;
                    parameters[5].Value = profiles;
                    parameters[6].Value = loginUser;
                    parameters[7].Value = programName;

                    storedProcedureName = "usp_JobSchedule_Add";

                    ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);
                }
                finally
                {
                }

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Update the job schedule details
        /// </summary>
        /// <param name="scheduleId">This parameter is specifying the schedule Id</param>
        /// <param name="scheduleName">This parameter is specifying the schedule Name</param>
        /// <param name="scheduleLongName">This parameter is specifying the schedule long Name</param>
        /// <param name="content">This parameter is specifying the schedule content</param>
        /// <param name="isEnable">This parameter is specifying whether it is enable or not</param>
        /// <param name="systemName">This parameter is specifying the system name</param>
        /// <param name="profiles">This parameter is specifying the profiles</param>
        /// <param name="loginUser">This parameter is specifying the login user</param>
        /// <param name="programName">This parameter is specifying the program Name</param>
        /// <param name="command">This parameter is specifying to which server we have to connect.</param>
        public void UpdateSchedule(Int32 scheduleId, String scheduleName, String scheduleLongName, String content, Boolean isEnable, String systemName, String profiles, String loginUser, String programName, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                    parameters = generator.GetParameters("JobManager_JobSchedule_Update_ParametersArray");

                    parameters[0].Value = scheduleId;
                    parameters[1].Value = scheduleName;
                    parameters[2].Value = scheduleLongName;
                    parameters[3].Value = content;
                    parameters[4].Value = isEnable;
                    parameters[5].Value = systemName;
                    parameters[6].Value = profiles;
                    parameters[7].Value = loginUser;
                    parameters[8].Value = programName;

                    storedProcedureName = "usp_JobSchedule_Update";

                    ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);
                }
                finally
                {
                }

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Delete the job schedule.
        /// </summary>
        /// <param name="scheduleId">This parameter is specifying the schedule Id</param>
        /// <param name="command">This parameter is specifying to which server we have to connect.</param>
        public void DeleteSchedule(Int32 scheduleId, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("JobManager_SqlParameters");

                    parameters = generator.GetParameters("JobManager_JobSchedule_Delete_ParametersArray");

                    parameters[0].Value = scheduleId;

                    storedProcedureName = "usp_JobSchedule_Delete";

                    ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);
                }
                finally
                {
                }

                transactionScope.Complete();
            }
        }

        #endregion CUD Job schedule

        #region Private methods

        private void UpdateJobScheduleAndOperationResult(SqlDataReader reader, JobSchedule jobSchedule, OperationResult jobScheduleProcessOperationResult)
        {
            Boolean hasError = false;
            String errorCode = String.Empty;
            Int32 id;

            jobScheduleProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

            while (reader.Read())
            {
                if (reader["HasError"] != null)
                    hasError = ValueTypeHelper.Int32TryParse(reader["HasError"].ToString(), 0) != 0;

                if (reader["ErrorMessage"] != null)
                    errorCode = reader["ErrorMessage"].ToString();

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        jobScheduleProcessOperationResult.AddOperationResult(errorCode, String.Empty,
                            OperationResultType.Error);
                    }
                    jobScheduleProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    if (reader["Id"] != null && Int32.TryParse(reader["Id"].ToString(), out id))
                    {
                        jobSchedule.Id = id;
                        jobScheduleProcessOperationResult.Id = id;
                    jobScheduleProcessOperationResult.ReferenceId = !String.IsNullOrWhiteSpace(jobSchedule.ReferenceId) ? jobSchedule.ReferenceId : jobSchedule.Name;
                        jobScheduleProcessOperationResult.ReturnValues.Add(id);
                    }
                    else
                    {
                        jobScheduleProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    }
                }
            }
        }

        private JobScheduleCollection PrepareJobScheduleCollection(SqlDataReader reader)
        {
            JobScheduleCollection jobSchedules = null;

            if (reader != null)
            {
                jobSchedules = new JobScheduleCollection();
                while (reader.Read())
                {
                    #region Local variables

                    Int32 jobScheduleId = -1;
                    String name = String.Empty;
                    String longName = String.Empty;

                    #endregion Local variables

                    #region Read values

                    if (reader["PK_JobSchedule"] != null)
                    {
                        jobScheduleId = ValueTypeHelper.Int32TryParse(reader["PK_JobSchedule"].ToString(), jobScheduleId);
                    }

                    if (reader["Name"] != null)
                    {
                        name = reader["Name"].ToString();
                    }

                    if (reader["LongName"] != null)
                    {
                        name = reader["LongName"].ToString();
                    }


                    #endregion Read values

                    #region Create collection

                    JobSchedule jobSchedule = new JobSchedule();
                    jobSchedule.Id = jobScheduleId;
                    jobSchedule.Name = name;
                    jobSchedule.LongName = longName;

                    jobSchedules.Add(jobSchedule);

                    #endregion Create collection
                }
            }

            return jobSchedules;
        }

        private BaseProfile CreateProfile(String profileType)
        {
            BaseProfile profile = null;
            DQMJobType dqmJobType;
            ExportProfileType exportProfileType;

            if (Enum.TryParse(profileType, out dqmJobType))
            {
                switch (dqmJobType)
                {
                    case DQMJobType.Merging:
                        profile = new MergingProfile();
                        break;
                    case DQMJobType.Validation:
                        profile = new ValidationProfile();
                        break;
                    case DQMJobType.Normalization:
                        profile = new NormalizationProfile();
                        break;
                    case DQMJobType.Matching:
                        profile = new MatchingProfile();
                        break;
                    default:
                        throw new MDMOperationException(String.Empty, String.Format("Profile type {0} is not supported", dqmJobType), "JobScheduleDA.CreateProfile", String.Empty, "CreateProfile"); //TODO: localize
                        //break;
                }
            }
            else if (Enum.TryParse(profileType, out exportProfileType))
            {
                profile = new ExportProfile();
            }
            else
            {
                throw new MDMOperationException(String.Empty, String.Format("Profile type {0} is not supported", profileType), "JobScheduleDA.CreateProfile", String.Empty, "CreateProfile"); //TODO: localize
            }

            return profile;
        }

        private String GetProfileTypeName(BaseProfile profile)
        {
            String profileTypeName = String.Empty;

            if (profile is ExportProfile)
            {
                profileTypeName = ((ExportProfile)profile).ProfileType.ToString();
            }
            else if (profile is MergingProfile)
            {
                profileTypeName = DQMJobType.Merging.ToString();
            }
            else if (profile is MatchingProfile)
            {
                profileTypeName = DQMJobType.Matching.ToString();
            }
            else if (profile is DQMJobProfile)
            {
                profileTypeName = ((DQMJobProfile)profile).JobType.ToString();
            }
            else
            {
                throw new MDMOperationException(String.Empty, String.Format("The profile type {0} is not supported", profile.GetType().Name), "JobScheduleDA.GetProfileTypeName", String.Empty, "GetProfileTypeName"); //TODO: localize
            }

            return profileTypeName;
        }

        #endregion Private methods

    }
}
