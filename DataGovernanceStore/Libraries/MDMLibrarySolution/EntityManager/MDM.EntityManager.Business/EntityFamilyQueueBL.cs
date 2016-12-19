using System;
using System.Transactions;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.EntityManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.EntityManager.Data;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the business operations for entity family queue.
    /// </summary>
    public class EntityFamilyQueueBL : BusinessLogicBase,IEntityFamilyQueueManager
    {
        #region Fields

        /// <summary>
        /// Field denoting system UI locale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting LocaleMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityFamilyQueueBL()
        {
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        #endregion Constructor

        #region Properties
        #endregion Properties

        #region  Methods

        #region Public Methods

        /// <summary>
        /// Process entity family queue
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates entity family queue to be processed</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns operation result</returns>
        public OperationResult Process(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext)
        {
            OperationResult operationResult = null;
            EntityFamilyQueueCollection queueCollection = new EntityFamilyQueueCollection() { entityFamilyQueue };

            OperationResultCollection operationResults = Process(queueCollection, callerContext);

            if (operationResults != null && operationResults.Count > 0)
            {
                operationResult = operationResults.ElementAt(0);
            }

            return operationResult;
        }

        /// <summary>
        /// Process entity family queue collection
        /// </summary>
        /// <param name="entityFamilyQueues">Indicates entity family queue collection to be processed</param>
        /// <param name="callerContext">Indicates the caller context</param></param>
        /// <returns>Returns operation result collection</returns>
        public OperationResultCollection Process(EntityFamilyQueueCollection entityFamilyQueues, CallerContext callerContext)
        {
            return ProcessInternal(entityFamilyQueues, callerContext);
        }

        /// <summary>
        /// Get entity family queue collection based on given batch size
        /// </summary>
        /// <param name="batchSize">Indicates no. of entity family to be fetched</param>
        /// <param name="entityActivityList">Indicates entity activity list</param>
        /// <param name="callerContext">Indicates the caller context</param></param>
        /// <returns>Returns entity family queue collection</returns>
        public EntityFamilyQueueCollection Get(Int32 batchSize, List<EntityActivityList> entityActivityList, CallerContext callerContext)
        {
            PopulateDefaultProgramName(callerContext, "EntityFamilyQueueBL.Get");

            if (batchSize < 1)
            {
                throw new MDMOperationException(String.Empty, "Batch size must be greater than 0.", callerContext.ProgramName, String.Empty, "Get");
            }

            return Get(0, entityActivityList, batchSize, callerContext);
        }

        /// <summary>
        /// Get entity family queue collection based on given entity family id
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates specific entity family to be fetched</param>
        /// <param name="entityActivityList">Indicates entity activity list</param>
        /// <param name="callerContext">Indicates the caller context</param></param>
        /// <returns>Returns entity family queue collection</returns>
        public EntityFamilyQueueCollection Get(Int64 entityGlobalFamilyId, List<EntityActivityList> entityActivityList, CallerContext callerContext)
        {
            PopulateDefaultProgramName(callerContext, "EntityFamilyQueueBL.Get");
            ValidteEntityGlobalFamilyId(entityGlobalFamilyId, callerContext);

            return Get(entityGlobalFamilyId, entityActivityList, 0, callerContext);
        }

        /// <summary>
        /// Releases lock and mark IsProcssed as true for a given entity global family id
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates specific entity family to be unlocked</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns operation result</returns>
        public OperationResult MarkCompleteAndReleaseLock(Int64 entityGlobalFamilyId, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            PopulateDefaultProgramName(callerContext, "EntityFamilyQueueBL.MarkCompleteAndReleaseLock");
            ValidteEntityGlobalFamilyId(entityGlobalFamilyId, callerContext);

            EntityFamilyQueue entityFamilyQueue = new EntityFamilyQueue()
            {
                EntityGlobalFamilyId = entityGlobalFamilyId,
                EntityFamilyId = entityGlobalFamilyId,
                IsProcessed = true,
                Action = ObjectAction.Update
            };

            OperationResultCollection operationResults = ProcessInternal(new EntityFamilyQueueCollection() { entityFamilyQueue }, callerContext);

            if (operationResults != null && operationResults.Count > 0)
            {
                operationResult = operationResults.FirstOrDefault();
            }

            return operationResult;
        }

        #endregion Public Methods

        #region Private Methods

        #region Get/Process Methods

        /// <summary>
        /// Process entity family queue collection
        /// </summary>
        /// <param name="entityFamilyQueues">Indicates entity family queue collection to be processed</param>
        /// <param name="callerContext">Indicates the caller context</param></param>
        /// <returns>Returns operation result collection</returns>
        private OperationResultCollection ProcessInternal(EntityFamilyQueueCollection entityFamilyQueues, CallerContext callerContext)
        {
            OperationResultCollection operationResults = new OperationResultCollection();

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            #endregion

            try
            {
                #region Step : Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Step : Initial Setup

                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

                PopulateDefaultProgramName(callerContext, "EntityFamilyQueueBL.Process");

                #endregion

                #endregion Step : Diagnostics & Tracing

                #region Step : Data Validations

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is started...");
                }

                ValidateInputParameters(entityFamilyQueues, operationResults, callerContext);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is completed.");
                }

                #endregion Step : Data Validations

                #region Step : Scan and filter based on results

                Boolean canContinue = ScanAndFilterEntityFamilyQueueBasedOnResults(entityFamilyQueues, operationResults);

                if (!canContinue)
                {
                    return operationResults;
                }

                #endregion Step : Scan and filter based on results

                #region Step : Perform updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Starting entity family queue database process...");
                    }

                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);
                    EntityFamilyQueueDA entityFamilyQueueDA = new EntityFamilyQueueDA();

                    EntityFamilyQueue entityFamilyQueue = null;

                    if (entityFamilyQueues != null)
                    {
                        entityFamilyQueue = entityFamilyQueues.ElementAt(0);
                    }

                    //If the entity family queue to be revalidated based on context, then there will be only one entity family queue.
                    //Also, Revalidation context will be set. So based on that, the respective stored procedure can be called.
                    if (entityFamilyQueues.Count == 1 && entityFamilyQueue != null && entityFamilyQueue.RevalidateContext != null && entityFamilyQueue.RevalidateContext.RevalidateMode == RevalidateMode.Delta)
                    {
                        entityFamilyQueueDA.Process(entityFamilyQueue, callerContext, userName, command);
                    }
                    else
                    {
                        entityFamilyQueueDA.Process(entityFamilyQueues, callerContext, operationResults, userName, command);
                    }

                    transactionScope.Complete();

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity family queue database process completed");
                    }
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Localizing errors for operation results started...");
                }

                foreach (OperationResult operationResult in operationResults)
                {
                    LocalizeErrors(callerContext, operationResult);
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Localizing errors for operation results completed.");
                }

                #endregion Step : Perform updates in database
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("EntityFamily processing is completed.");
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return operationResults;
        }

        /// <summary>
        /// Get entity family queue collection based on given parameters
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates specific entity family to be fetched</param>
        /// <param name="entityActivityList">Indicates entity activity list </param>
        /// <param name="batchSize">Indicates no. of entity family to be fetched</param>
        /// <param name="callerContext">Indicates the caller context</param></param>
        /// <returns>Returns entity family queue collection</returns>
        private EntityFamilyQueueCollection Get(Int64 entityGlobalFamilyId, List<EntityActivityList> entityActivityList, Int32 batchSize, CallerContext callerContext)
        {
            EntityFamilyQueueCollection mergedEntityFamilyQueues = null;

            #region Diagnostics & tracing

            var diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Entity family Get is started.");
            }

            #endregion Diagnostics & tracing

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                EntityFamilyQueueDA entityFamilyQueueDA = new EntityFamilyQueueDA();
                EntityFamilyQueueCollection entityFamilyQueues = entityFamilyQueueDA.Get(entityGlobalFamilyId, entityActivityList, batchSize, command);

                if (entityFamilyQueues != null && entityFamilyQueues.Count > 0)
                {
                    mergedEntityFamilyQueues = MergeEntityFamilyChangeContext(entityFamilyQueues);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity family Get is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return mergedEntityFamilyQueues;
        }

        #endregion Get/Process Methods

        #region Helper Methods

        /// <summary>
        /// Localize errors in operation result
        /// </summary>
        /// <param name="callerContext">Indicates the called context</param>
        /// <param name="operationResult">Indicates operation result that contains errors to be localized.</param>
        private void LocalizeErrors(CallerContext callerContext, OperationResult operationResult)
        {
            foreach (Error error in operationResult.Errors)
            {
                if (!String.IsNullOrEmpty(error.ErrorCode))
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                    if (_localeMessage != null)
                    {
                        error.ErrorMessage = _localeMessage.Message;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="defaultProgramName"></param>
        private void PopulateDefaultProgramName(CallerContext callerContext, String defaultProgramName)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext();
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = defaultProgramName;
            }
        }

        /// <summary>
        ///  Performs scan and filter of entity family queue collection based on results and returns boolean flag to continue process.
        /// </summary>
        /// <param name="entityFamilyQueues">>Specifies all entity family queue collection</param>
        /// <param name="operationResults">Specifies  operation result collection</param>
        /// <returns>'true' if operation is successful;otherwise 'false'</returns>
        private Boolean ScanAndFilterEntityFamilyQueueBasedOnResults(EntityFamilyQueueCollection entityFamilyQueues, OperationResultCollection operationResults)
        {
            Boolean continueProcess = true;

            if (operationResults.OperationResultStatus == OperationResultStatusEnum.Failed)
            {
                //Operation result says 'Failed'. Stop processing
                continueProcess = false;
            }
            else if (operationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                //Behavior says process partial.. Filter out the failed ones and continue with succeeded ones..
                IEnumerable<OperationResult> failedOperationResults = operationResults.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.Failed);

                foreach (OperationResult operationResult in failedOperationResults)
                {
                    if (entityFamilyQueues != null)
                    {
                        EntityFamilyQueue entityFamilyQueueInFilteredCollection = entityFamilyQueues.SingleOrDefault(e => e.Id == operationResult.Id);

                        if (entityFamilyQueueInFilteredCollection != null)
                        {
                            entityFamilyQueues.Remove(entityFamilyQueueInFilteredCollection);
                        }
                    }
                }

                if (entityFamilyQueues != null)
                {
                    if (entityFamilyQueues.Count < 1)
                    {
                        continueProcess = false;
                    }
                }
            }

            return continueProcess;
        }

        #endregion Helper Methods

        #region Data Validation Methods

        /// <summary>
        /// Validates given parameters
        /// </summary>
        /// <param name="entityFamilyQueues">Indicates entity family queue collection to be validated</param>
        /// <param name="operationResults">Indicates operation result collection to log validation errors</param>
        /// <param name="callerContext">Indicates caller context to be validated</param>
        private void ValidateInputParameters(EntityFamilyQueueCollection entityFamilyQueues, OperationResultCollection operationResults, CallerContext callerContext)
        {
            if (entityFamilyQueues == null || entityFamilyQueues.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "114341", false, callerContext); //Entity Family Queue collection is not available or empty.
                throw new MDMOperationException(_localeMessage.Code, _localeMessage.Message, "EntityFamilyQueueBL.Process", String.Empty, "Process");
            }

            Int64 entityFamilyToBeCreated = -1;

            foreach (EntityFamilyQueue entityFamilyQueue in entityFamilyQueues)
            {
                entityFamilyQueue.Id = entityFamilyToBeCreated--;
                entityFamilyQueue.Action = ObjectAction.Create;
                Boolean isRevalidatingContext = false;

                if (entityFamilyQueue.RevalidateContext != null && entityFamilyQueue.RevalidateContext.RevalidateMode == RevalidateMode.Delta)
                {
                    isRevalidatingContext = true;
                }

                OperationResult operationResult = new OperationResult
                {
                    ReferenceId = entityFamilyQueue.EntityFamilyId.ToString()
                };

                if (!isRevalidatingContext)
                {
                    if (entityFamilyQueue.EntityFamilyId < 0)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "114342", false, callerContext); //EntityFamilyId must be greater than 0.
                        operationResult.Errors.Add(new Error(_localeMessage.Code, _localeMessage.Message));
                    }

                    if (entityFamilyQueue.EntityGlobalFamilyId < 0)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "114340", false, callerContext); //EntityGlobalFamilyId must be greater than 0.
                        operationResult.Errors.Add(new Error(_localeMessage.Code, _localeMessage.Message));
                    }
                }
                else
                {
                    if (entityFamilyQueue.RevalidateContext.RuleMapContextIds == null || entityFamilyQueue.RevalidateContext.RuleMapContextIds.Count < 1)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "114563", false, callerContext); //Failed to perform delta revalidation as the rule map context is not provided.
                        operationResult.Errors.Add(new Error(_localeMessage.Code, _localeMessage.Message));
                    }
                }

                operationResult.RefreshOperationResultStatus();

                operationResults.Add(operationResult);
            }

            operationResults.RefreshOperationResultStatus();
        }

        /// <summary>
        /// Validates entity global family id
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates entity global family is validated</param>
        /// <param name="callerContext">Indicates caller context</param>
        private void ValidteEntityGlobalFamilyId(Int64 entityGlobalFamilyId, CallerContext callerContext)
        {
            if (entityGlobalFamilyId < 1)
            {
                throw new MDMOperationException(String.Empty, "EntityGlobalFamilyId must be greater than 0.", callerContext.ProgramName, String.Empty, "Get");
            }
        }

        #endregion Data Validation Methods

        #region Merge Entity Family Change Context

        /// <summary>
        /// Merge entity family change context and creates master change context record
        /// </summary>
        /// <param name="entityFamilyQueues">Indicates entity family queues</param>
        /// <returns>Returns merged entity family queues</returns>
        private EntityFamilyQueueCollection MergeEntityFamilyChangeContext(EntityFamilyQueueCollection entityFamilyQueues)
        {
            EntityFamilyQueueCollection mergedEntityFamilyQueues = new EntityFamilyQueueCollection();

            foreach (EntityFamilyQueue entityFamilyQueue in entityFamilyQueues)
            {
                Boolean isMasterCollaborationRecord = false;
                EntityFamilyQueue filteredEntityFamilyQueue = null;

                if (entityFamilyQueue.EntityActivityList == EntityActivityList.EntityRevalidate)
                {
                    RevalidateContext revalidateContext = entityFamilyQueue.RevalidateContext;

                    //If revalidate context is not found then add it into merged entity family queues and continue... 
                    if (revalidateContext == null)
                    {
                        mergedEntityFamilyQueues.Add(entityFamilyQueue);
                    }
                    else
                    {
                        isMasterCollaborationRecord = revalidateContext.IsMasterCollaborationRecord;

                        //if entity family queue has master collaboration record is set to true then uniqueness is based on EntityGlobalFamilyId & IsMasterCollaborationRecord
                        if (isMasterCollaborationRecord)
                        {
                            filteredEntityFamilyQueue = mergedEntityFamilyQueues.GetByEntityGlobalFamilyId(entityFamilyQueue.EntityGlobalFamilyId);
                        }
                        else
                        {
                            filteredEntityFamilyQueue = mergedEntityFamilyQueues.GetByEntityFamilyId(entityFamilyQueue.EntityFamilyId);
                        }

                        if (filteredEntityFamilyQueue == null)
                        {
                            mergedEntityFamilyQueues.Add(entityFamilyQueue);
                        }
                        else
                        {
                            filteredEntityFamilyQueue.RevalidateContext.Merge(entityFamilyQueue.RevalidateContext);
                        }
                    }
                }
                else
                {
                    EntityFamilyChangeContext entityFamilyChangeContext = entityFamilyQueue.EntityFamilyChangeContexts.FirstOrDefault();
                    isMasterCollaborationRecord = entityFamilyChangeContext.IsMasterCollaborationRecord;
                    
                    //if entity family queue has master collaboration record is set to true then uniqueness is based on EntityGlobalFamilyId & IsMasterCollaborationRecord
                    if (isMasterCollaborationRecord)
                    {
                        filteredEntityFamilyQueue = mergedEntityFamilyQueues.GetByEntityGlobalFamilyId(entityFamilyQueue.EntityGlobalFamilyId);
                    }
                    else
                    {
                        filteredEntityFamilyQueue = mergedEntityFamilyQueues.GetByEntityFamilyId(entityFamilyQueue.EntityFamilyId);
                    }

                    if (filteredEntityFamilyQueue == null)
                    {
                        mergedEntityFamilyQueues.Add(entityFamilyQueue);
                    }
                    else
                    {
                        if (entityFamilyChangeContext.EntityActivityList == EntityActivityList.EntityAsyncWorkflowActivityBusinessRules)
                        {
                            filteredEntityFamilyQueue.EntityFamilyChangeContexts.Add(entityFamilyChangeContext);
                        }
                        else
                        {
                            filteredEntityFamilyQueue.EntityFamilyChangeContexts.Merge(entityFamilyQueue.EntityFamilyChangeContexts);
                        }
                    }
                }
            }

            return mergedEntityFamilyQueues;
        }

        #endregion Merge Entity Family Change Context

        #endregion Private Methods

        #endregion Methods
    }
}