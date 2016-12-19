using System;

namespace MDM.JobManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Denorm;
    using MDM.ConfigurationManager.Business;
    using MDM.JobManager.Data.SqlClient;

    /// <summary>
    /// Specifies denorm class
    /// </summary>
    public class DenormBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting denorm data access
        /// </summary>
        private DenormDA _denormDA = new DenormDA();

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DenormBL()
        {
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get each job step details based on job id.
        /// </summary>
        /// <param name="jobId">This parameter specifying jobId.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Job object with each execution step</returns>
        public Job GetJobStepDetails(Int32 jobId,MDMCenterApplication application,MDMCenterModules module)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            return _denormDA.GetJobStepDetails(jobId, command);
        }

        /// <summary>
        /// Get job type
        /// </summary>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Collection of job type</returns>
        public JobCollection GetJobType(MDMCenterApplication application,MDMCenterModules module)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            return _denormDA.GetJobType(command);
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
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Collection of job</returns>
        public JobCollection GetJobList(JobType jobType, String fromDate, String toDate, Boolean displayNonEmptyJobs, Boolean displayErrorJobs, Int32 displayRows, MDMCenterApplication application, MDMCenterModules module)
        { 
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            return _denormDA.GetJobList(jobType, fromDate, toDate, displayNonEmptyJobs, displayErrorJobs, displayRows, command);
        }

        /// <summary>
        /// Get Collection of job with execution step details & result of each step.
        /// </summary>
        /// <param name="mode">This parameter specifying mode.</param>
        /// <param name="jobId">This parameter specifying type jobId.</param>
        /// <param name="displayRows">This parameter specifying how many job user want to display.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Collection of Job & it's result.</returns>
        public Tuple<JobCollection, DenormResultCollection, Int32> GetJobErrorDetails(String mode, Int32 jobId, Int32 displayRows, MDMCenterApplication application, MDMCenterModules module)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            return _denormDA.GetJobErrorDetails(mode, jobId, displayRows, command);
        }

        /// <summary>
        /// Get current job status. 
        /// </summary>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Current Job object</returns>
        public Job GetCurrentJobStatus(MDMCenterApplication application, MDMCenterModules module)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            return _denormDA.GetCurrentJobStatus(command);
        }

        /// <summary>
        /// release denorm lock.
        /// </summary>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        public void ReleaseLock(MDMCenterApplication application, MDMCenterModules module)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            _denormDA.ReleaseLock(command);
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}