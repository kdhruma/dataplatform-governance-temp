using System;
using System.Collections.ObjectModel;

namespace MDM.DataModelManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.DataModelManager.Data;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;

    /// <summary>
    /// Specifies Replication class
    /// </summary>
    public class ReplicationBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting Replication data access
        /// </summary>
        private ReplicationDA _replicationDA = new ReplicationDA();

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReplicationBL()
        {
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        ///  Modify Article of replication
        /// </summary>
        /// <param name="tableNames">This parameter is specifying table names to be replicate.</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamicTableType</param>
        /// <param name="action">This parameter is specifying action of table for replication</param>
        /// <param name="moduleId">This parameter is specifying module id</param>
        /// <returns>If Operation performed Successful, returns Job Name, else returns Empty String</returns>
        public String ModifyArticle(Collection<String> tableNames, DynamicTableType dynamicTableType, ReplicationType action, Int32 moduleId)
        {
            String jobName = String.Empty;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.ReplicationBL.ModifyArticle", false);

            jobName = _replicationDA.ModifyArticle(tableNames, dynamicTableType, action, moduleId);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DataModelManager.ReplicationBL.ModifyArticle");

            return jobName;
        }

        /// <summary>
        ///  Adds Job to execute on the Distributor in order to replicate Lookup and Complex Attribute Tables to Subscribers
        /// </summary>
        /// <param name="jobName">The Job Name to be run on the Distributor DB</param>
        /// <param name="callerContext">Name of the Application and Module which calls this BL method</param>
        public void AddJobToDistributor(String jobName, CallerContext callerContext)
        {
            #region Step : Initial Setup

            DBCommandProperties command;

            #region Parameter validations

            if (String.IsNullOrEmpty(jobName))
                throw new ArgumentNullException("JobName", "JobName is null or empty");

            #endregion Parameter validations

            command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Create);

            #endregion

            _replicationDA.AddJobToDistributor(jobName, command);

        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
