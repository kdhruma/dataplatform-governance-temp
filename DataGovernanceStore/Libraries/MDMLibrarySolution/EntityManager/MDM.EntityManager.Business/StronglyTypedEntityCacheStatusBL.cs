using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MDM.EntityManager.Business
{
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.EntityManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Represents strongly typed entity cache status business logic
    /// </summary>
    public class StronglyTypedEntityCacheStatusBL : BusinessLogicBase
    {

        private StronglyTypedEntityCacheStatusDA _entityCacheStatusDA = new StronglyTypedEntityCacheStatusDA();

        /// <summary>
        /// Updates cache status table for specified entity ids to indicate processor has worked on them
        /// </summary>
        /// <param name="entityIds">Entity ids</param>
        /// <param name="isCacheLoadingRequired">Indicates is cache loading is required.</param>
        /// <param name="callerContext">Caller details</param>
        /// <returns>Result of the cache status process</returns>
        public Boolean Process(Collection<Int64> entityIds, Boolean isCacheLoadingRequired, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.StronglyTypedEntityCacheStatusBL.Process", false);
            }

            #region Validations

            if (entityIds == null || entityIds.Count == 0)
            {
                throw new MDMOperationException("111847", "EntityIdList cannot be null or empty", "EntityManager.StronglyTypedEntityCacheStatusBL", String.Empty, "Process");
            }

            #endregion validations

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            Boolean isProcessed = _entityCacheStatusDA.ProcessCacheStatus(entityIds, isCacheLoadingRequired, callerContext, command, ProcessingMode.AsyncUpdate);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.StronglyTypedEntityCacheStatusBL.Process");
            }

            return isProcessed;
        }

        /// <summary>
        /// Gets next batch for Strongly typed entity cache processor for processing
        /// </summary>
        /// <param name="batchSize">Batch size</param>
        /// <param name="callerContext">Caller details</param>
        /// <returns>Collection of entity ids for the batch</returns>
        public Collection<Int64> GetNextBatch(Int32 batchSize, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.StronglyTypedEntityCacheStatusBL.GetNextBatch", false);
            }

            #region validations

            if (batchSize < 1)
            {
                throw new MDMOperationException("112810", "Batch size for receiving the next batch of entity cache status cannot be less than 1", "EntityManager.StronglyTypedEntityCacheStatusBL", String.Empty, "GetNextBatch");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity cache status get BatchSize : " + batchSize.ToString());
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            Collection<Int64> entityIdCollection = _entityCacheStatusDA.GetNextBatch(null, batchSize, 0, 0, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.StronglyTypedEntityCacheStatusBL.GetNextBatch");
            }

            return entityIdCollection;
        }
    }
}
