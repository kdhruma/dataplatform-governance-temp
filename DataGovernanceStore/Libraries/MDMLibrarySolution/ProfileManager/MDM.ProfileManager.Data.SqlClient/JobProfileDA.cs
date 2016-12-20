using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Transactions;

namespace MDM.ProfileManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.BusinessObjects.Imports;

    public class JobProfileDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobCollection"></param>
        /// <param name="operationResult"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Process(JobProfile jobProfile, String userName, DBCommandProperties command, String programName)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            OperationResult operationResult = new OperationResult();

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("ProfileManager_SqlParameters");

                    parameters = generator.GetParameters("ProfileManager_JobProfile_Process_ParametersArray");

                    parameters[0].Value = jobProfile.Id;
                    parameters[1].Value = jobProfile.Name;
                    parameters[2].Value = jobProfile.ProfileDomain;
                    parameters[3].Value = jobProfile.ProfileDataXml;
                    parameters[4].Value = jobProfile.FileType;
                    parameters[5].Value = jobProfile.OrganizationId;
                    parameters[6].Value = jobProfile.OrganizationName;
                    parameters[7].Value = jobProfile.ContainerId;
                    parameters[8].Value = jobProfile.ContainerName;
                    parameters[9].Value = jobProfile.Action.ToString();
                    parameters[10].Value = userName;
                    parameters[11].Value = programName;

                    storedProcedureName = "usp_ProfileManager_JobProfile_Process";

                    Object value = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                    if (value != null)
                    {
                        jobProfile.Id = ValueTypeHelper.Int32TryParse(value.ToString(), jobProfile.Id);
                        operationResult.Id = jobProfile.Id;
                        operationResult.ReferenceId = !String.IsNullOrWhiteSpace(jobProfile.ReferenceId) ? jobProfile.ReferenceId : jobProfile.Name;
                    }
                }
                catch (Exception ex)
                {
                    operationResult.Errors.Add(new Error(ex.Message, ex.Message));
                }
                finally
                {
                }

                transactionScope.Complete();
            }
            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="profileName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public JobProfile Get(Int32 profileId, String profileName, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            JobProfile jobProfile = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ProfileManager_SqlParameters");

                parameters = generator.GetParameters("ProfileManager_JobProfile_Get_ParametersArray");

                parameters[0].Value = profileId;
                parameters[1].Value = profileName;

                storedProcedureName = "usp_ProfileManager_JobProfile_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    jobProfile = GetObject(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return jobProfile;
        }

        /// <summary>
        /// Get all profiles based on requested job type.
        /// Job type parameter is optional. By default is unknown.
        /// If it is unknown it will return all available profile else requested job type profiles will be return.
        /// </summary>
        /// <param name="command">Indicates the DB command properties</param>
        /// <param name="jobType">Indicates the job type</param>
        /// <returns>Return the Job Profiles collections</returns>
        public JobProfileCollection GetAll(DBCommandProperties command, String jobType)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            JobProfileCollection jobProfileCollection = new JobProfileCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ProfileManager_SqlParameters");

                parameters = generator.GetParameters("ProfileManager_JobProfile_Get_ParametersArray");

                String jobTypeValue = String.Empty;

                parameters[0].Value = 0;
                parameters[1].Value = String.Empty;
                parameters[2].Value = jobType;

                storedProcedureName = "usp_ProfileManager_JobProfile_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    jobProfileCollection.Add(GetObject(reader));
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return jobProfileCollection;
        }

        #endregion

        #region Private Methods

        private JobProfile GetObject(SqlDataReader reader)
        {
            JobProfile jobProfile = new JobProfile();

            if (reader["Id"] != null)
                jobProfile.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), jobProfile.Id);

            if (reader["Name"] != null)
                jobProfile.Name = reader["Name"].ToString();

            if (reader["ProfileDomain"] != null)
                jobProfile.ProfileDomain = reader["ProfileDomain"].ToString();

            if (reader["ProfileDataXml"] != null)
                jobProfile.ProfileDataXml = reader["ProfileDataXml"].ToString();

            if (reader["FileType"] != null)
                jobProfile.FileType = reader["FileType"].ToString();

            if (reader["ContainerId"] != null)
                jobProfile.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);

            if (reader["ContainerName"] != null)
                jobProfile.ContainerName = reader["ContainerName"].ToString();

            if (reader["OrganizationId"] != null)
                jobProfile.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), 0);

            if (reader["OrganizationName"] != null)
                jobProfile.OrganizationName = reader["OrganizationName"].ToString();

            if (reader["IsSystemProfile"] != null)
                jobProfile.IsSystemProfile = ValueTypeHelper.BooleanTryParse(reader["IsSystemProfile"].ToString(), false);

            if (reader["ProfileTypeName"] != null)
            {
                JobType jobType = JobType.UnKnown;
                ValueTypeHelper.EnumTryParse<JobType>(reader["ProfileTypeName"].ToString(), true, out jobType);
                jobProfile.JobType = jobType;
            }

            if (reader["ModDateTime"] != null)
            {
                jobProfile.LastModified = ValueTypeHelper.ConvertToNullableDateTime(reader["ModDateTime"].ToString());
            }

            return jobProfile;
        }

        #endregion

        #endregion
    }
}
