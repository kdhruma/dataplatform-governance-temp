using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.EntityManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.EntityManager.Data;
    using MDM.ConfigurationManager.Business;


    using MDM.Core.Exceptions;

    /// <summary>
    /// Represents the BL layer for Entity Cache status.
    /// </summary>
    public class EntityCacheStatusBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies the Data access class for entity cache status 
        /// </summary>
        private EntityCacheStatusDA _entityCacheStatusDA = new EntityCacheStatusDA();

        #endregion

        #region Constructor
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityCacheStatusBL()
        {            
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Returns the EntityCacheStatusCollection for the EntityCacheProcessor based on the batch size. 
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCacheStatusCollection GetNextBatch(Int32 batchSize, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityCacheStatusBL.GetNextBatch", false);
            }

            #region validations

            if (batchSize < 1)
            {
                throw new MDMOperationException("112810", "Batch size for receiving the next batch of entity cache status cannot be less than 1", "EntityManager.EntityCacheStatusBL", String.Empty, "GetNextBatch");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity cache status get BatchSize : " + batchSize.ToString());
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            EntityCacheStatusCollection entityCacheStatusCollection = _entityCacheStatusDA.GetNextBatch(null, batchSize, 0, 0, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityCacheStatusBL.GetNextBatch");
            }

            return entityCacheStatusCollection;
        }

        /// <summary>
        /// Returns the EntityCacheStatusCollection for the EntityCacheProcessor based on the request specified. 
        /// </summary>
        /// <param name="loadRequestCollection"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCacheStatusCollection GetEntityCacheStatusCollection(Collection<EntityCacheStatusLoadRequest> loadRequestCollection, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityCacheStatusBL.GetEntityCacheStatusCollection", false);
            }

            #region validations

            if (loadRequestCollection != null && loadRequestCollection.Count == 0)
            {
                throw new MDMOperationException("112811", "Entity Id list for retrieving cache status cannot be null or empty", "EntityManager.EntityCacheStatusBL", String.Empty, "GetEntityCacheStatusCollection");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            EntityCacheStatusCollection entityCacheStatusCollection = _entityCacheStatusDA.GetEntityCacheStatusCollection(loadRequestCollection, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityCacheStatusBL.GetEntityCacheStatusCollection");
            }

            return entityCacheStatusCollection;
        }

        /// <summary>
        /// Updates the EntityCacheStatus table with the value specified in the EntityCacheStatusCollection.
        /// </summary>
        /// <param name="entityCacheStatusCollection">Specifies the entityCacheStatusCollection to be processed</param>
        /// <param name="callerContext">Represents the caller of the API</param>
        /// <param name="isCacheProcessorUpdate">Specifies if the API is invoked by the cache processor</param>
        /// <returns></returns>
        public Boolean Process(EntityCacheStatusCollection entityCacheStatusCollection, CallerContext callerContext, Boolean isCacheProcessorUpdate = false)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityCacheStatusBL.Process", false);                
            }

            #region Validations

            if (entityCacheStatusCollection == null || entityCacheStatusCollection.Count == 0)
            {
                throw new MDMOperationException("112812", "Entity cache status collection cannot be null or empty", "EntityManager.EntityCacheStatusBL", String.Empty, "Process");
            }

            #endregion validations

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            Boolean isProcessed = _entityCacheStatusDA.ProcessCacheStatus(entityCacheStatusCollection, callerContext, command, isCacheProcessorUpdate);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityCacheStatusBL.Process");
            }

            return isProcessed;
        }

        /// <summary>
        /// Loads the EntityCache context from the EntityActivityLog table and populates the Entity cache table.
        /// </summary>
        /// <param name="entityActivityLog"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean LoadEntityCacheContextForProcess(EntityActivityLog entityActivityLog, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityCacheStatusBL.LoadEntityCacheContextForProcess", false);
            }

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            Boolean isProcessed = _entityCacheStatusDA.LoadEntityCacheContextForProcess(entityActivityLog, callerContext, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityCacheStatusBL.LoadEntityCacheContextForProcess");
            }

            return isProcessed;
        }

        #endregion
    }
}
