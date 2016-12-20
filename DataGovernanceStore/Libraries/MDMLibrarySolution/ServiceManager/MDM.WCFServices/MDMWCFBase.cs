using System;
using System.ServiceModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MDM.WCFServices
{
    using Core;
    using Core.Exceptions;
    using BusinessObjects;
    using AdminManager.Business;
    using ExceptionManager;
    using Utility;
    using WCFServiceInterfaces;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public class MDMWCFBase : IMDMWCFBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// 
        /// </summary>
        private static Object _lockObject = new Object();

        #endregion

        #region Static Constructor

        /// <summary>
        /// This static constructor is used to start perform WCF application start activities...
        /// </summary>
        static MDMWCFBase()
        {
            if (!GlobalEvents.IsApplicationStarted)
            {
                lock (_lockObject)
                {
                    if (!GlobalEvents.IsApplicationStarted)
                        GlobalEvents.OnApplicationStart();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public MDMWCFBase()
            : this(true)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadSecurityPrincipal"></param>
        public MDMWCFBase(Boolean loadSecurityPrincipal = true)
        {
            try
            {
                if (loadSecurityPrincipal) //TODO: rename tihs flag to indicate skipping of all normal API call intialization step when called from external process instead of just saying to load security principal
                {
                    LoadOperationContext();

                    _securityPrincipal = LoadSecurityPrincipal();
                }
            }
            catch (Exception ex)
            {
                new ExceptionHandler(ex);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public SecurityPrincipal SecurityPrincipal
        {
            get
            {
                return _securityPrincipal;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Makes calls of KnowledgeBase Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <param name="initializeContext">Optional action to initialize execution context of diagnostic activity</param>
        /// <param name="onCallCompleted">Optional action to log additional data to current diagnostic activity after call of business logic method completed</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <returns>The value returned by business logic or default</returns>
        protected TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(Func<TBusinessLogic, TResult> call, Action<ExecutionContext> initializeContext = null, Action<DiagnosticActivity> onCallCompleted = null, MDMTraceSource traceSource = MDMTraceSource.APIFramework, [CallerMemberName] String methodName = "")
            where TBusinessLogic : BusinessLogicBase, new()
        {
            return MakeBusinessLogicCall(call, new TBusinessLogic(), initializeContext, onCallCompleted, traceSource, methodName);
        }

        /// <summary>
        /// Makes calls of KnowledgeBase Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <param name="initializeContext">Optional action to initialize execution context of diagnostic activity</param>
        /// <param name="onCallCompleted">Optional action to log additional data to current diagnostic activity after call of business logic method completed</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <returns>The value returned by business logic or default</returns>
        protected TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(Func<TBusinessLogic, TResult> call, TBusinessLogic businessLogicInstance, Action<ExecutionContext> initializeContext = null, Action<DiagnosticActivity> onCallCompleted = null, MDMTraceSource traceSource = MDMTraceSource.APIFramework, [CallerMemberName] String methodName = "") 
            where TBusinessLogic : BusinessLogicBase, new()
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity;

            if (isTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(methodName, traceSource, filePath: GetType().FullName);

                if (initializeContext != null)
                {
                    initializeContext(diagnosticActivity.ExecutionContext);
                }

                diagnosticActivity.Start();
            }
            else
            {
                diagnosticActivity = new DiagnosticActivity();
            }

            TResult operationResult;

            try
            {
                diagnosticActivity.LogVerbose(String.Format("{0} receives {1} request message.", GetType().Name, methodName));

                operationResult = call(businessLogicInstance);

                diagnosticActivity.LogVerbose(String.Format("{0} receives {1} response message.", GetType().Name, methodName));
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    if (onCallCompleted != null)
                    {
                        onCallCompleted(diagnosticActivity);
                    }

                    diagnosticActivity.Stop();
                }
            }

            return operationResult;
        }

        /// <summary>
        /// Wrap the normal exception into a WCF fault
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The Fault Exception of type WcfException</returns>
        protected FaultException<MDMExceptionDetails> WrapException(Exception ex)
        {
            //TODO add the service url in the faultcode
            MDMExceptionDetails fault = null;
            FaultReason reason = null;
            FaultException<MDMExceptionDetails> exception = null;

            //Get message code
            String messageCode = String.Empty;
            Object[] messageArguments = null;

            if (ex is MDMOperationException)
            {
                MDMOperationException mdmException = ex as MDMOperationException;

                messageCode = mdmException.MessageCode;
                messageArguments = mdmException.MessageArguments;
            }

            fault = new MDMExceptionDetails(messageCode, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), messageArguments);

            reason = new FaultReason(ex.Message);

            exception = new FaultException<MDMExceptionDetails>(fault, reason);

            return exception;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected SecurityPrincipal LoadSecurityPrincipal()
        {
            return LoadSecurityPrincipal(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reCreate"></param>
        /// <returns></returns>
        protected SecurityPrincipal LoadSecurityPrincipal(Boolean reCreate)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            SecurityPrincipal currentUserSecurityPrincipal = null;
            String userName = String.Empty;

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StartTraceActivity(MDMTraceSource.APIFramework);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Starting load security prinicipal...");

                if (OperationContext.Current != null && OperationContext.Current.ServiceSecurityContext != null &&
                    OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name != null)
                {
                    userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose,
                            "Operation request primary identity name is:" + userName);

                    if (!String.IsNullOrWhiteSpace(userName) && !userName.Equals("superuser"))
                    {
                        SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();

                        AuthenticationType authenticationType = AuthenticationType.Windows;

                        //How to identify if request came is of type form auth or windows auth...
                        //Right now default authentication type is set as windows auth..
                        //And we are using cache key set at WCFCustomAuthentication level to find out if it is forms authentication..

                        if (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.AuthenticationType ==
                            "WCFMessageAuthenticationValidator")
                            authenticationType = AuthenticationType.Forms;

                        currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, authenticationType,
                            MDMCenterSystem.WcfService);

                        if (currentUserSecurityPrincipal != null)
                        {
                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose,
                                    "Security principal is loaded for user:" + userName);
                        }
                        else
                        {
                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning,
                                    "Security principal load failed for user:" + userName);
                        }
                    }
                }
                else
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning,
                            "WCFBase:LoadSecurityPrincipal:OperationContext.PrimaryIdentity.Name is not available in WCF service request. Operation would failed in lack of security prinicipal");
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Done with load security prinicipal");
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error,
                    "Security load failed for the provided user:" + userName + ". Error is: " + ex.Message);

                UnauthorizedAccessException unAuthException =
                    new UnauthorizedAccessException(
                        "Security load failed for the provided user:" + userName +
                        ". Please verify user settings in WCF request.", ex);

                LogException(unAuthException);
                throw WrapException(unAuthException);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }

            return currentUserSecurityPrincipal;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void LoadOperationContext()
        {
            try
            {
                MDMOperationContextHelper.LoadComponentTracingSettings();
            }
            catch
            {
                //TODO:: Need to work on exception mangement at this level..
            }
        }

        #region Utility Methods

        /// <summary>
        /// Logs the exception into Event Log
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        protected void LogException(Exception ex)
        {
            try
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Unhandled exception occurred during service execution. Error is:" + ex.Message);
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
            catch
            {
                //Do not throw
            }
        }

        #endregion

        #endregion
    }
}
