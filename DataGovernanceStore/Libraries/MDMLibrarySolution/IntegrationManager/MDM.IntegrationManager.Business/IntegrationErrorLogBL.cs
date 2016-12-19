using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace MDM.IntegrationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Data;
    using MDM.Interfaces;
    using MDM.Utility;
    using System.Collections.ObjectModel;

    public class IntegrationErrorLogBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;
        private IntegrationErrorLogDA _integrationErrorLogDA = new IntegrationErrorLogDA();
        private const String _processMethodName = "MDM.IntegrationManager.Business.IntegrationErrorLogBL.Process";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationErrorLogBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD Methods

        /// <summary>
        /// Create error log entry for given IIntegrationItem and exception detail.
        /// </summary>
        /// <param name="iIntegrationItem">Integration item for which error occurred.</param>
        /// <param name="processorName">Name of processor for which error occurred.</param>
        /// <param name="errorMessage">Exception detail.</param>
        /// <param name="callerContext">Context of caller making call to this API</param>
        /// <returns>Result of operation</returns>
        public OperationResult WriteErrorLog(IIntegrationItem iIntegrationItem, CoreDataProcessorList processorName, String errorMessage, CallerContext callerContext)
        {
            IntegrationErrorLog errorLog = new IntegrationErrorLog();
            errorLog.ConnectorId = iIntegrationItem.ConnectorId;
            errorLog.CoreDataProcessorName = processorName;
            errorLog.IntegrationId = iIntegrationItem.Id;
            errorLog.IntegrationMessageTypeId = iIntegrationItem.IntegrationMessageTypeId;
            errorLog.IntegrationType = iIntegrationItem.IntegrationType;
            errorLog.MessageText = errorMessage;
            errorLog.MessageType = OperationResultType.Error;

            OperationResult orCreate = this.Create(errorLog, callerContext);

            if (orCreate != null)
            {
                if (!( orCreate.OperationResultStatus == OperationResultStatusEnum.Successful || orCreate.OperationResultStatus == OperationResultStatusEnum.None ))
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Recording error for '" + processorName.ToString() + "' processor (IntegrationErrorLogBL.WriteErrorLog) was not successful. OperationResult.ToXml : " + orCreate.ToXml(), MDMTraceSource.Integration);
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Recording error for '" + processorName.ToString() + "' processor (IntegrationErrorLogBL.WriteErrorLog) was successful", MDMTraceSource.ParallelProcessingEngine);
                    }
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Recording error for '" + processorName.ToString() + "' processor (IntegrationErrorLogBL.WriteErrorLog) returned null OperationResult", MDMTraceSource.Integration);
            }

            return orCreate;
        }

        /// <summary>
        /// Create error log entry for given IIntegrationItems and exception detail.
        /// </summary>
        /// <param name="iIntegrationItemList">The integration item list.</param>
        /// <param name="processorName">Name of processor for which error occurred.</param>
        /// <param name="errorMessage">Exception detail.</param>
        /// <param name="callerContext">Context of caller making call to this API</param>
        /// <returns>
        /// Result of operation
        /// </returns>
        public OperationResultCollection WriteErrorLogCollection(List<IIntegrationItem> iIntegrationItemList, CoreDataProcessorList processorName, String errorMessage, CallerContext callerContext)
        {
            IntegrationErrorLogCollection integrationErrorLogCollection = new IntegrationErrorLogCollection();

            foreach (IIntegrationItem iIntegrationItem in iIntegrationItemList)
            {
                IntegrationErrorLog errorLog = new IntegrationErrorLog();
                errorLog.ConnectorId = iIntegrationItem.ConnectorId;
                errorLog.CoreDataProcessorName = processorName;
                errorLog.IntegrationId = iIntegrationItem.Id;
                errorLog.IntegrationMessageTypeId = iIntegrationItem.IntegrationMessageTypeId;
                errorLog.IntegrationType = iIntegrationItem.IntegrationType;
                errorLog.MessageText = errorMessage;
                errorLog.MessageType = OperationResultType.Error;
                errorLog.Action = ObjectAction.Create;

                integrationErrorLogCollection.Add(errorLog);
            }


            OperationResultCollection orCreate = this.Process(integrationErrorLogCollection, callerContext);

            if (orCreate != null)
            {
                if (!(orCreate.OperationResultStatus == OperationResultStatusEnum.Successful || orCreate.OperationResultStatus == OperationResultStatusEnum.None))
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Recording error for '" + processorName.ToString() + "' processor (IntegrationErrorLogBL.WriteErrorLogCollection) was not successful. OperationResult.ToXml : " + orCreate.ToXml(), MDMTraceSource.Integration);
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Recording error for '" + processorName.ToString() + "' processor (IntegrationErrorLogBL.WriteErrorLogCollection) was successful", MDMTraceSource.ParallelProcessingEngine);
                    }
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Recording error for '" + processorName.ToString() + "' processor (IntegrationErrorLogBL.WriteErrorLogCollection) returned null OperationResult", MDMTraceSource.Integration);
            }

            return orCreate;
        }


        ///// <summary>
        ///// Handle exception for MDMMessagePackage. This is called mostly for Feeder of processors
        ///// </summary>
        ///// <param name="failedMDMMessagePackageCollection">MDMMessagePackages for which error occurred.</param>
        ///// <param name="exception">Exception detail.</param>
        ///// <param name="coreDataProcessor">Name of processor for which error occurred.</param>
        ///// <param name="callerContext">Context of caller making call to this API</param>
        //public void HandleException(MDMMessagePackageCollection failedMDMMessagePackageCollection, Exception exception, CoreDataProcessorList coreDataProcessor, CallerContext callerContext)
        //{
        //    try
        //    {
        //        foreach (MDMMessagePackage messagePackage in failedMDMMessagePackageCollection)
        //        {
        //            Object data = messagePackage.Data;
        //            IIntegrationItem integrationItem = ( IIntegrationItem )data;

        //            this.WriteErrorLog(integrationItem, coreDataProcessor, exception.ToString(), callerContext);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("DataProcessor: {0}:ProduceMessages failed. Exception:{1}", CoreDataProcessorList.EntityActivityLogProcessor.ToString(), ex.ToString()), MDMTraceSource.ParallelProcessingEngine);
        //    }
        //}

        /// <summary>
        /// Create a integration activity log
        /// </summary>
        /// <param name="integrationErrorLog">Integration activity log to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(IntegrationErrorLog integrationErrorLog, CallerContext callerContext)
        {
            this.Validate(integrationErrorLog, "Create");
            integrationErrorLog.Action = Core.ObjectAction.Create;
            return this.Process(integrationErrorLog, callerContext);
        }

        #endregion CUD Methods

        #region Private Methods

        /// <summary>
        /// Process (create / update / delete) integration activity logs
        /// </summary>
        /// <param name="integrationErrorLogCollection">Integration activity logs to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        private OperationResult Process(IntegrationErrorLog integrationErrorLog, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            #endregion Parameter Validation

            try
            {
                IntegrationErrorLogCollection integrationErrorLogCollection = new IntegrationErrorLogCollection();
                integrationErrorLogCollection.Add(integrationErrorLog);
                OperationResultCollection resultCollection = Process(integrationErrorLogCollection, callerContext);
                if (resultCollection!= null && resultCollection.Count > 0)
                {
                    operationResult = resultCollection[0];
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResult;
        }


        /// <summary>
        /// Process (create / update / delete) integration activity logs
        /// </summary>
        /// <param name="integrationErrorLogCollection">Integration activity logs to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        private OperationResultCollection Process(IntegrationErrorLogCollection integrationErrorLogCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            #endregion Parameter Validation

            try
            {
                String userName = String.Empty;
                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = _processMethodName;
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);
                operationResultCollection = CreateOperationResultCollection(integrationErrorLogCollection);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _integrationErrorLogDA.Process(integrationErrorLogCollection, callerContext.ProgramName, userName, operationResultCollection, command);

                    if (operationResultCollection.OperationResultStatus == OperationResultStatusEnum.Successful || operationResultCollection.OperationResultStatus == OperationResultStatusEnum.None)
                    {
                        transactionScope.Complete();
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResultCollection;
        }

        /// <summary>
        /// Get security principal for currently logged in user
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// Validate input parameters
        /// </summary>
        private void Validate(IntegrationErrorLog integrationErrorLog, String methodName)
        {
            if (integrationErrorLog == null)
            {
                String message = "IntegrationErrorLog must not be null";
                throw new MDMOperationException("C1022", message, "IntegrationErrorLogBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// Validate caller context if it is null or not.
        /// </summary>
        /// <param name="callerContext">Caller context to validate</param>
        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "IntegrationErrorLogBL.Process", String.Empty, "Process");
            }
        }
        
        private OperationResultCollection CreateOperationResultCollection(IntegrationErrorLogCollection integrationErrorLogCollection)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            foreach (IntegrationErrorLog integrationErrorLog in integrationErrorLogCollection)
            {
                OperationResult operationResult = new OperationResult();
                operationResult.OperationResultStatus = OperationResultStatusEnum.None;
                operationResultCollection.Add(operationResult);
            }
            return operationResultCollection;
        }

        #endregion Private Methods
    }
}
