using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.EntityManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.EntityManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Business layer which contains logic for work with SourceValues
    /// </summary>
    public class SourceValueBL : BusinessLogicBase
    {
        #region Constants

        private const String ProcessSourceValueProcessName = "Process";
        private const String GetSourceValueProcessName = "Get";
        private const String GetSourceValueByEntityProcessName = "GetByEntity";

        private const String EmptyProgramNameReplacementPrefix = "MDM.EntityManager.Business.SourceValueBL.";
        private const String TracingPrefix = "MDM.EntityManager.Business.SourceValueBL.";

        #endregion

        #region Fields
        
        private SecurityPrincipal _currentSecurityPrincipal = null;

        #endregion

        #region Constructors

        #endregion 

        #region Properties

        public String UserLogin
        {
            get
            {
                try
                {
                    if (_currentSecurityPrincipal == null)
                    {
                        _currentSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                    }
                    return _currentSecurityPrincipal.CurrentUserName;
                }
                catch
                {
                    TraceError("Unable to fetch user login", MDMTraceSource.EntityProcess);
                }
                return null;
            }
        }

        #endregion

        #region Public Methods

        #region CUD operations

        /// <summary>
        /// 
        /// </summary>
        public OperationResult Process(EntityCollection entities, CallerContext callerContext)
        {
            OperationResult operationResult = null;
            
            StartTraceActivity(ProcessSourceValueProcessName, MDMTraceSource.EntityProcess);
            try
            {
                ValidateContext(callerContext, "Process", MDMTraceSource.EntityProcess);

                ValidateEntityCollection(entities, "Process", MDMTraceSource.EntityProcess);

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Create);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResult = new SourceValueDA().Process(entities, PrepareCallerContext(callerContext, "Process"), command);

                    TraceInformation("Processing entities", MDMTraceSource.EntityProcess);

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        transactionScope.Complete();
                    }
                }
            }
            finally
            {
                StopTraceActivity(ProcessSourceValueProcessName, MDMTraceSource.EntityProcess);
            }

            return operationResult;
        } 

        #endregion
        
        #region Get operations

        /// <summary>
        /// 
        /// </summary>
        public OperationResult Get(EntityCollection entities, CallerContext callerContext)
        {

            OperationResult result = null;
            
            StartTraceActivity(GetSourceValueProcessName, MDMTraceSource.EntityGet);
            try
            {
                ValidateContext(callerContext, "Get", MDMTraceSource.EntityGet);

                SourceValueDA sourceValueDA = new SourceValueDA();

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                result = sourceValueDA.Get(entities, callerContext, command);
            }
            finally
            {
                StopTraceActivity(GetSourceValueProcessName, MDMTraceSource.EntityGet);
            }

            return result;
        }

        /// <summary>
        /// Get by Entity
        /// </summary>
        public OperationResult GetByEntity(Entity entity, CallerContext callerContext)
        {
            OperationResult result = null;

            StartTraceActivity(GetSourceValueByEntityProcessName, MDMTraceSource.EntityGet);
            try
            {
                EntityCollection entityCollection = new EntityCollection(new Collection<Entity>
                {
                    entity
                });
                result = Get(entityCollection, PrepareCallerContext(callerContext, "GetByEntity"));
            }
            finally
            {
                StopTraceActivity(GetSourceValueByEntityProcessName, MDMTraceSource.EntityGet);
            }

            return result;
        } 

        #endregion

        #endregion

        #region Private Methods

        private void ValidateContext(CallerContext callerContext, String methodName, MDMTraceSource traceSourceValue)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.SourceValueBL." + methodName, String.Empty, methodName);
            }
        }
        
        private void ValidateEntityCollection(EntityCollection entities, String methodName, MDMTraceSource traceSourceValue)
        {
            if (entities == null)
            {
                throw new MDMOperationException(String.Empty, "EntityCollection is null", "EntityManager.SourceValueBL." + methodName, String.Empty, methodName);
            }
            if (!entities.Any())
            {
                throw new MDMOperationException(String.Empty, "EntityCollection is empty", "EntityManager.SourceValueBL." + methodName, String.Empty, methodName);
            }
        }

        private CallerContext PrepareCallerContext(CallerContext callerContext, String methodName)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                CallerContext context = (CallerContext)callerContext.Clone();
                context.ProgramName = EmptyProgramNameReplacementPrefix + methodName;
                return context;
            }
            return callerContext;
        }

        private static Boolean StartTraceActivity(String record, MDMTraceSource traceSourceValue)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StartTraceActivity(PopulateTraceRecord(record), traceSourceValue, false) : true;
        }

        private static Boolean StopTraceActivity(String record, MDMTraceSource traceSourceValue)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StopTraceActivity(PopulateTraceRecord(record), traceSourceValue) : true;
        }

        private static Boolean TraceInformation(String record, MDMTraceSource traceSourceValue)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, PopulateTraceRecord(record), traceSourceValue) : true;
        }

        private static Boolean TraceError(String record, MDMTraceSource traceSourceValue)
        {
            return MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, PopulateTraceRecord(record), traceSourceValue);
        }

        private static String PopulateTraceRecord(String record)
        {
            return TracingPrefix + record;
        }

        #endregion
    }
}