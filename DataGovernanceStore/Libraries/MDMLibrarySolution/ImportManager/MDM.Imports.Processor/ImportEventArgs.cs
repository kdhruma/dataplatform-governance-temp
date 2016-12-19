using System;

namespace MDM.Imports.Processor
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies arguments for events raise by import job engine
    /// </summary>
    public class ImportEventArgs: EventArgs
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public EntityCollection EntityCollection { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityOperationResultCollection EntityOperationResultCollection { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Job Job { get;private set;}
       
        /// <summary>
        /// 
        /// </summary>
        public ImportProfile ImportProfile {get; private set;}

        /// <summary>
        /// 
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
        /// MDM Publisher
        /// </summary>
        public MDMPublisher MDMPublisher{ get; private set; }

        #endregion
        
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <param name="application"></param>
        /// <param name="service"></param>
        /// <param name="publisher"></param>
        public ImportEventArgs(Job job, ImportProfile importProfile, ExecutionStatus executionStatus, MDMCenterApplication application, MDMCenterModules service, MDMPublisher publisher)
        {
            if (job == null)
                throw new ArgumentNullException("job", "job is null");

            if (importProfile == null)
                throw new ArgumentNullException("importProfile", "importProfile is null");

            this.Job = job;
            this.ImportProfile = importProfile;
            this.ExecutionStatus = executionStatus;
            this.MDMApplication = application;
            this.MDMService = service;
            this.MDMPublisher = publisher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <param name="application"></param>
        /// <param name="service"></param>
        /// <param name="publisher"></param>
        public ImportEventArgs(EntityCollection entityCollection, EntityOperationResultCollection entityOperationResultCollection, Job job, ImportProfile importProfile, MDMCenterApplication application, MDMCenterModules service, MDMPublisher publisher)
        {
            if (entityCollection == null)
                throw new ArgumentNullException("EntityCollection", "entityCollection is null");

            if (entityOperationResultCollection == null)
                throw new ArgumentNullException("EntityOperationResultCollection", "EntityOperationResultCollection is null");

            if (job == null)
                throw new ArgumentNullException("job", "job is null");

            if (importProfile == null)
                throw new ArgumentNullException("importProfile", "importProfile is null");

            this.EntityCollection = entityCollection;
            this.EntityOperationResultCollection = entityOperationResultCollection;
            this.Job = job;
            this.ImportProfile = importProfile;
            this.MDMApplication = application;
            this.MDMService = service;
            this.MDMPublisher = publisher;
        }

        #endregion
    }
}