using System;

namespace MDM.Imports.Processor
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies arguments for events raise by lookup import job engine
    /// </summary>
    public class LookupImportEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Property denoting job
        /// </summary>
        public Job Job { get; private set; }

        /// <summary>
        /// Property denoting lookup import profile
        /// </summary>
        public LookupImportProfile LookupImportProfile { get; private set; }

        /// <summary>
        /// Property denoting execution status of job
        /// </summary>
        public ExecutionStatus ExecutionStatus { get; private set; }

        /// <summary>
        /// Application name to pass as argument. This is the Enum indicating which application is performing current action 
        /// </summary>
        public MDMCenterApplication MDMApplication { get; private set; }

        /// <summary>
        /// Module name to pass as argument. This is the Enum indicating which module is performing current action 
        /// </summary>
        public MDMCenterModules MDMService { get; private set; }

        /// <summary>
        /// Property denoting MDM Publisher
        /// </summary>
        public MDMPublisher MDMPublisher { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructors which initializes multiple parameters.
        /// </summary>
        /// <param name="job">Indicates instance of job</param>
        /// <param name="importProfile">Indicates import profile</param>
        /// <param name="application">Indicates the type of application</param>
        /// <param name="service">Indicates the service</param>
        /// <param name="publisher">Indicates the publisher</param>
        public LookupImportEventArgs(Job job, LookupImportProfile lookupImportProfile, ExecutionStatus executionStatus, MDMCenterApplication application, MDMCenterModules service, MDMPublisher publisher)
        {
            if (job == null)
                throw new ArgumentNullException("job", "job is null");

            if (lookupImportProfile == null)
                throw new ArgumentNullException("importProfile", "importProfile is null");

            this.Job = job;
            this.LookupImportProfile = lookupImportProfile;
            this.ExecutionStatus = executionStatus;
            this.MDMApplication = application;
            this.MDMService = service;
            this.MDMPublisher = publisher;
        }

        #endregion
    }
}