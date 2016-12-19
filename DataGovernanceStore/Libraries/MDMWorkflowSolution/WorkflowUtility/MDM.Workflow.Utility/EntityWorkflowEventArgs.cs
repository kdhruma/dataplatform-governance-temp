using System;

namespace MDM.Workflow.Utility
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies arguments for events raise by entity object
    /// </summary>
    public class EntityWorkflowEventArgs : EventArgs, IEntityEventArgs
    {
        #region Properties

        /// <summary>
        /// Entity object to be passed as argument
        /// </summary>
        public EntityCollection EntityInstances { get; private set; }

        /// <summary>
        /// EntityManager object to be passed as argument
        /// </summary>
        public IEntityManager EntityManagerInstance { get; private set; }

        /// <summary>
        /// Collection of EntityOperationResult objects to be passed as argument. This can be used to keep the log of operations
        /// </summary>
        public EntityOperationResultCollection EntityOperationResults { get; private set; }

        /// <summary>
        /// UserId to pass as argument. This is the Id of user who is performing current action
        /// </summary>
        public Int32 UserId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public CallerContext CallerContext { get; private set; }

        /// <summary>
        /// Entity processing options to be passed as arguments
        /// </summary>
        public IEntityProcessingOptions EntityProcessingOptions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean PopulateDiagnosticsReport { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityOperationDiagnosticReport EntityOperationDiagnosticReport { get; private set; }

        #endregion
        
        #region Constructors

        /// <summary>
        /// Constructor taking Entity, EntityManager, OperationResult, UserId and programName as input parameters
        /// </summary>
        /// <param name="entityInstances">Entity object to be passed as argument</param>
        /// <param name="entityManagerInstance">EntityManager object to be passed as argument</param>
        /// <param name="entityOperationResults">Entity Operation Result Collection to put the log of operations</param>
        /// <param name="userId">UserId (who is performing the current action) to pass as argument</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="programName">Modified ProgramName</param>
        public EntityWorkflowEventArgs(EntityCollection entityInstances, IEntityManager entityManagerInstance, EntityOperationResultCollection entityOperationResults, Int32 userId, CallerContext callerContext)
        {
            if (entityInstances == null)
                throw new ArgumentNullException("entityInstances", "EntityInstances is null");

            if (entityManagerInstance == null)
                throw new ArgumentNullException("entityManagerInstance", "EntityManagerInstance is null");

            if (entityOperationResults == null)
                throw new ArgumentNullException("entityOperationResults", "EntityOperationResultCollection is null");

            EntityInstances = entityInstances;
            EntityManagerInstance = entityManagerInstance;
            EntityOperationResults = entityOperationResults;
            UserId = userId;
            CallerContext = callerContext;
        }

        #endregion
    }
}