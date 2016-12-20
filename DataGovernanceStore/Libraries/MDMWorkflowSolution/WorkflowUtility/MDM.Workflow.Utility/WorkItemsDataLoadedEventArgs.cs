using System;

namespace MDM.Workflow.Utility
{
    using MDM.BusinessObjects;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Specifies arguments for events raised for work items data loaded
    /// </summary>
    public class WorkItemsDataLoadedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Property denoting the current entity id
        /// </summary>
        public Int64 EntityId { get; private set; }

        /// <summary>
        /// Property denoting the work items data
        /// </summary>
        public Table WorkItemsData { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the object
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <param name="workItemsData">Indicates work items data</param>
        public WorkItemsDataLoadedEventArgs(Int64 entityId, Table workItemsData)
        {
            if (entityId < 1)
            {
                throw new MDMOperationException("Entity Id Must be greater than 0");
            }

            if (workItemsData == null)
            {
                throw new ArgumentNullException("workItemsData");
            }

            EntityId = entityId;
            WorkItemsData = workItemsData;
        }

        #endregion
    }
}