using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.DataModelManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;

    /// <summary>
    /// class for data model process orchestrator.
    /// </summary>
    public class DataModelProcessOrchestrator
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private static LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private static LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Filed denoting DataLocale
        /// </summary>
        private static LocaleEnum _systemUILocale = GlobalizationHelper.GetSystemUILocale();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor.
        /// </summary>
        public DataModelProcessOrchestrator()
        {

        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Create - Update or Delete given dataModelObjects
        /// </summary>
        /// <param name="dataModelManager">DataModel Manager </param>
        /// <param name="dataModelObjects">Collection of dataModelObjects to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public static IDataModelOperationResultCollection Validate(IDataModelManager dataModelManager, IDataModelObjectCollection dataModelObjects, ICallerContext iCallerContext)
        {
            String activityName = "DataModelProcessOrchestrator.Validate";
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper completeDataModelProcessDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(activityName, MDMTraceSource.DataModel, false);
            }

            DataModelOperationResultCollection operationResults = new DataModelOperationResultCollection();
            dataModelManager.PrepareOperationResultsSchema(dataModelObjects, operationResults);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Prepared data model operation results for {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
            }

            try
            {
                #region Initial Setup

                Object userName = String.Empty;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (callerContext != null && callerContext.AdditionalProperties != null)
                {
                    callerContext.AdditionalProperties.TryGetValue("UserName", out userName);
                }

                #endregion

                #region Parameter Validations

                dataModelManager.Validate(dataModelObjects, operationResults, iCallerContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Validation process completed for {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                }

                operationResults.RefreshOperationResultStatus();
                if (!ScanAndFilterDataModelObjectsBasedOnResults(dataModelObjects, operationResults))
                {
                    return operationResults;
                }

                #endregion

                #region Populate Original dataModelObjects objects

                dataModelManager.LoadOriginal(dataModelObjects, operationResults, iCallerContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Original data models loaded for {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                }

                #endregion

                #region Fill dataModelObjects objects

                dataModelManager.FillDataModel(dataModelObjects, iCallerContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Data models fill process completed for {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                }

                #endregion

                #region Step: Compare, Merge and Calculate Actions

                dataModelManager.CompareAndMerge(dataModelObjects, operationResults, iCallerContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Data model action re-calculated for {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                }

                operationResults.RefreshOperationResultStatus();
                if (!ScanAndFilterDataModelObjectsBasedOnResults(dataModelObjects, operationResults))
                {
                    return operationResults;
                }

                // Update Performed Action 
                foreach (IDataModelObject dataModelObject in dataModelObjects)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(dataModelObject.ReferenceId);
                    operationResult.PerformedAction = dataModelObject.Action;
                    dataModelObject.UserName = userName as String;
                }

                #endregion

            }
            catch (Exception ex)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, "113689", "{0} failed with {1}", new Object[] { activityName, ex.Message }, false, iCallerContext as CallerContext);
                operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, OperationResultType.Error);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message, MDMTraceSource.DataModel);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall data model validate processing time for {1}", completeDataModelProcessDurationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(activityName, MDMTraceSource.DataModel);
                }
            }

            return operationResults;
        }

        /// <summary>
        /// Create - Update or Delete given dataModelObjects
        /// </summary>
        /// <param name="dataModelManager">DataModel Manager </param>
        /// <param name="dataModelObjects">Collection of dataModelObjects to process</param>
        /// <param name="operationResults"></param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public static IDataModelOperationResultCollection Process(IDataModelManager dataModelManager, IDataModelObjectCollection dataModelObjects, IDataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            String activityName = "DataModelProcessOrchestrator.Process";
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper completeDataModelDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(activityName, MDMTraceSource.DataModel, false);
            }

            try
            {
                if (operationResults != null)
                {
                    #region Step: Process

                    dataModelManager.Process(dataModelObjects, operationResults as DataModelOperationResultCollection, iCallerContext);

                    if (operationResults != null)
                    {
                        foreach (OperationResult operationResult in operationResults)
                        {
                            operationResult.RefreshOperationResultStatus();
                        }
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for {1} database process", durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                    }

                    #endregion

                    #region Update Cache

                    dataModelManager.InvalidateCache(dataModelObjects, operationResults as DataModelOperationResultCollection, iCallerContext);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - data model cache status updated for {1}.", durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                    }

                    #endregion
                }
                else
                {
                    throw new ArgumentNullException("operationResults", "OperationResults parameter is null");
                }
            }
            catch (Exception ex)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, "113689", "{0} failed with {1}", new Object[] { activityName, ex.Message }, false, iCallerContext as CallerContext);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message, MDMTraceSource.DataModel);
                if (operationResults != null)
                {
                    operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, OperationResultType.Error);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall data model processing time for {1}", completeDataModelDurationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjects.DataModelObjectType), MDMTraceSource.DataModel);
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(activityName, MDMTraceSource.DataModel);
                }
            }

            return operationResults;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataModelObjects"></param>
        /// <param name="operationResults"></param>
        /// <returns></returns>
        private static Boolean ScanAndFilterDataModelObjectsBasedOnResults(IDataModelObjectCollection dataModelObjects, DataModelOperationResultCollection operationResults)
        {
            Boolean continueProcess = true;

            if (dataModelObjects != null && operationResults != null)
            {
                continueProcess = operationResults.OperationResultStatus != OperationResultStatusEnum.Failed;

                if (continueProcess)
                {
                    var failedReferenceIds = new Collection<String>();

                    foreach (var operationResult in operationResults)
                    {
                        if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                        {
                            failedReferenceIds.Add(operationResult.ReferenceId);
                        }
                    }

                    if (failedReferenceIds.Count > 0)
                    {
                        dataModelObjects.Remove(failedReferenceIds);
                    }
                }

                if (dataModelObjects.Count < 1)
                {
                    continueProcess = false;
                }
            }

            return continueProcess;
        }

        #endregion

        #endregion
    }
}