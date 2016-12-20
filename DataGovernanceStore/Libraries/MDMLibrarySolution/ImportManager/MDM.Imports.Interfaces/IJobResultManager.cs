using System;

namespace MDM.Imports.Interfaces
{
    using BusinessObjects;

    public interface IJobResultHandler
    {
        /// <summary>
        /// Saves the given entity operation result in the job result table.
        /// </summary>
        /// <param name="operationResults"></param>
        /// <returns></returns>
        Boolean Save(EntityOperationResultCollection operationResults);

        /// <summary>
        /// Saves the given entity operation result in the job result table and adds the number of errors/warning to the total count.
        /// </summary>
        /// <param name="operationResults"></param>
        /// <returns></returns>
        Boolean SaveWithCount(EntityOperationResultCollection operationResults);

        /// <summary>
        /// Resets the item count in the job, for providers to update new counts
        /// </summary>
        void ResetCounts();

    }
}
