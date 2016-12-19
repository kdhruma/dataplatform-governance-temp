using System;
using System.Diagnostics;

namespace MDM.SearchManager.Business
{
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.SearchManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Represents the category search data Business logic manager
    /// </summary>
    public class CategorySearchDataBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Process Category Search Data Collection.
        /// </summary>
        /// <param name="categorySearchDataCollection">Provides SearchData to be processed.</param>
        /// <param name="entityOperationResults">Indicates entityOperationResult.</param>
        /// <param name="callerContext">Indicates application and method which called this method.</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns>True if serachData have been processed successfully.</returns>
        public Boolean Process(EntitySearchDataCollection categorySearchDataCollection, EntityOperationResultCollection entityOperationResults, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntitySearchManager.CategorySearchDataBL.Process", MDMTraceSource.DenormProcess, false);
            }

            Boolean result = false;

            try
            {
                #region Validations

                if (categorySearchDataCollection == null || categorySearchDataCollection.Count < 1)
                {
                    throw new MDMOperationException("111839", "EntitySearchData Collection is null or empty.", "EntitySearchManager", String.Empty, "Process");
                }

                foreach (EntitySearchData entitySearchData in categorySearchDataCollection)
                {
                    if (entitySearchData.EntityId <= 0)
                    {
                        throw new MDMOperationException("111795", "EntityId must be greater than 0.", "EntitySearchManager", String.Empty, "Process");
                    }

                    if (entitySearchData.ContainerId <= 0)
                    {
                        throw new MDMOperationException("111821", "ContainerId must be greater than 0.", "EntitySearchManager", String.Empty, "Process");
                    }

                    if (String.IsNullOrWhiteSpace(entitySearchData.SearchValue))
                    {
                        throw new MDMOperationException("111837", "SearchValue cannot be null.", "EntitySearchManager", String.Empty, "Process");
                    }

                    if (String.IsNullOrWhiteSpace(entitySearchData.KeyValue))
                    {
                        throw new MDMOperationException("111838", "KeyValue cannot be null.", "EntitySearchManager", String.Empty, "Process");
                    }
                }

                #endregion

                #region Prepare Entity OperationResult object for missing ones

                foreach (EntitySearchData categorySearchData in categorySearchDataCollection)
                {
                    EntityOperationResult entityOperationResult = entityOperationResults.GetEntityOperationResult(categorySearchData.EntityId);

                    if (entityOperationResult == null)
                    {
                        entityOperationResult = new EntityOperationResult();
                        entityOperationResult.EntityId = categorySearchData.EntityId;
                        entityOperationResults.Add(entityOperationResult);
                    }
                }

                #endregion

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Search);

                //Get Data from DataBase
                CategorySearchDataDA categorySearchDataDA = new CategorySearchDataDA();
                result = categorySearchDataDA.Process(categorySearchDataCollection, entityOperationResults, command, processingMode);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntitySearchManager.CategorySearchDataBL.Process", MDMTraceSource.DenormProcess);
                }
            }

            return result;
        }

        /// <summary>
        /// Get CategorySearchData for given entities.
        /// </summary>
        /// <param name="entities">Provides Collection of entities for which searchData needs to be found.</param>
        /// <param name="callerContext">Indicates application and method which called this method</param>
        /// <returns>EntitySearchData Collection</returns>
        public EntitySearchDataCollection Get(EntityCollection entities, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntitySearchManager.CategorySearchDataBL.Get", MDMTraceSource.DenormProcess, false);
            }

            EntitySearchDataCollection categorySearchDataCollection = new EntitySearchDataCollection();

            try
            {
                #region Validations

                if (entities == null || entities.Count < 1)
                {
                    throw new MDMOperationException("111840", "Entity Collection is null or empty.", "EntitySearchManager", String.Empty, "Get");
                }

                foreach (Entity entity in entities)
                {
                    if (entity.Id <= 0)
                    {
                        throw new MDMOperationException("111795", "EntityId must be greater than 0.", "EntitySearchManager", String.Empty, "Get");
                    }
                }

                #endregion

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Search);

                //Get Data from DataBase
                CategorySearchDataDA categorySearchDataDA = new CategorySearchDataDA();
                categorySearchDataCollection = categorySearchDataDA.Get(entities, command);

                if (categorySearchDataCollection == null || categorySearchDataCollection.Count < 1)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No SearchData found for given categories.");
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntitySearchManager.CategorySearchDataBL.Get", MDMTraceSource.DenormProcess);
                }
            }

            return categorySearchDataCollection;
        }

        #endregion 
    }
}
