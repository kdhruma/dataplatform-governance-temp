using System;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Data;

namespace MDM.Workflow.Designer.Business
{
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.Workflow.Designer.Data.SqlClient;
    using MDM.Core;
    using MDM.CacheManager.Business;


    /// <summary>
    /// Provides business logic related functions for Workflow Version
    /// </summary>
    public class WorkflowVersionBL : BusinessLogicBase
    {
        #region Fields

        private String _programName = "WorkflowVersionBL";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get collection of workflow versions
        /// </summary>
        /// <param name="workflowId">all workflow versions which are for this workflow</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>return a collection of workflow versions</returns>
        public Collection<WorkflowVersion> Get(Int32 workflowId, CallerContext callerContext)
        {
            //TO DO :: Implement procedure and add an entry into Parameters.Config file
            throw new NotImplementedException("Functionality has not been implemented");

            //WorkflowVersionDA workflowVersionDataManager = new WorkflowVersionDA();
            //return workflowVersionDataManager.Get(workflowId);
        }

        /// <summary>
        /// Gets the WorkflowVersion based on the version id.
        /// </summary>
        /// <param name="versionId">Id of the Workflow Version</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns></returns>
        public WorkflowVersion GetById(Int32 versionId, CallerContext callerContext, Boolean makeRoundTrip = false)
        {
            WorkflowVersion wfVersion = null;

            var cacheManager = CacheFactory.GetCache();

            // Create the cache key name for version object caching
            String cacheKeyName = CacheKeyGenerator.GetWorkflowVersionCacheKey(versionId);

            // Otherwise get the cached version
            if (!makeRoundTrip)
            {
                //Get the Workflow version from cache
                wfVersion = (WorkflowVersion)cacheManager.Get(cacheKeyName);
            }

            if (wfVersion == null)
            {
                //Get Command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

                WorkflowVersionDA workflowVersionDataManager = new WorkflowVersionDA();
                wfVersion = workflowVersionDataManager.GetById(versionId, command);

                if (wfVersion != null)
                {
                    cacheManager.Set(cacheKeyName, wfVersion, DateTime.Now.AddHours(24.0));
                }
            }

            return wfVersion;
        }

        /// <summary>
        /// Process workflow versions, based on Action
        /// </summary>
        /// <param name="workflowVersions">workflow versions to process</param>
        /// <param name="currentUserName">Logged in user name</param>
        /// <returns>returns status of the process</returns>
        public int Process(Collection<WorkflowVersion> workflowVersions, String currentUserName)
        {
            //Not in use..
            
            //TODO :: Check with WorkflowVersionDA process with actual procedure
            WorkflowVersionDA workflowVersionDataManager = new WorkflowVersionDA();
            var success = workflowVersionDataManager.Process(workflowVersions, currentUserName, this._programName);

            var cacheManager = CacheFactory.GetCache();
            
            foreach(var workflowVersion in workflowVersions)
            {
                cacheManager.Remove(CacheKeyGenerator.GetWorkflowVersionCacheKey(workflowVersion.Id));
                cacheManager.Remove(CacheKeyGenerator.GetWorkflowVersionWithTrackingProfileCacheKey(workflowVersion.Id));
            }

            return success;
        }

        #endregion
    }
}
