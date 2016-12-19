using System;

namespace MDM.Imports.Processor
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;

    /// <summary>
    /// Specifies arguments for events raise by DDG import engine
    /// </summary>
    public class DDGImportEventArgs : EventArgs
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting job
        /// </summary>
        public Job Job { get; private set; }

        ///// <summary>
        ///// Property denoting DDG import profile
        ///// </summary>
        public DDGImportProfile DDGImportImportProfile { get; private set; }

        /// <summary>
        /// Property denoting execution status of job
        /// </summary>
        public ExecutionStatus ExecutionStatus { get; private set; }

        /// <summary>
        /// Property denoting the caller context
        /// </summary>
        public CallerContext callerContext { get; private set; }

        /// <summary>
        /// Property denoting MDM Publisher
        /// </summary>
        public MDMPublisher MDMPublisher { get; private set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructors which initializes multiple parameters.
        /// </summary>
        /// <param name="job">Indicates instance of job</param>
        /// <param name="importProfile">Indicates import profile</param>
        /// <param name="callerContext">Indicates the name of Application and Module which perform the Action</param>
        /// <param name="publisher">Indicates the publisher</param>
        public DDGImportEventArgs(Job job, DDGImportProfile ddgImportProfile, ExecutionStatus executionStatus, CallerContext callerContext, MDMPublisher publisher)
        {
            if (job == null)
                throw new ArgumentNullException("job", "job is null");

            if (ddgImportProfile == null)
                throw new ArgumentNullException("importProfile", "importProfile is null");

            this.Job = job;
            this.DDGImportImportProfile = ddgImportProfile;
            this.ExecutionStatus = executionStatus;
            this.callerContext = callerContext;
            this.MDMPublisher = publisher;
        }

        #endregion Constructor

        #region Methods

        #endregion Methods
    }
}
