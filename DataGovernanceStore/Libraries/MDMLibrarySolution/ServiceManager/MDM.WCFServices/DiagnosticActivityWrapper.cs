using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Dispatcher;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;

    public class DiagnosticActivityWrapper : IParameterInspector
    {
        private readonly String typeName;

        public DiagnosticActivityWrapper(String typeName)
        {
            this.typeName = typeName;
        }

        #region Implementation of IParameterInspector

        /// <summary>
        /// Called before client calls are sent and after service responses are returned.
        /// </summary>
        /// <returns>
        /// The correlation state that is returned as the <paramref name="correlationState"/> parameter in <see cref="M:System.ServiceModel.Dispatcher.IParameterInspector.AfterCall(System.String,System.Object[],System.Object,System.Object)"/>. Return null if you do not intend to use correlation state.
        /// </returns>
        /// <param name="operationName">The name of the operation.</param><param name="inputs">The objects passed to the method by the client.</param>
        public object BeforeCall(string operationName, object[] inputs)
        {
            CallerContext callerContext = null;
            foreach (Object input in inputs)
            {
                callerContext = input as CallerContext;
                if (callerContext != null)
                {
                    break;
                }
            }

            if (callerContext != null &&
                callerContext.TraceSettings != null)
            {
                MDMOperationContextHelper.LoadOperationTracingSettings(callerContext.OperationId, callerContext.TraceSettings);
            }

            if (!MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled)
            {
                return null;
            }

            SecurityPrincipal currentSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            SecurityContext securityContext =
                new SecurityContext(currentSecurityPrincipal.UserPreferences.LoginId,
                                    currentSecurityPrincipal.UserPreferences.LoginName,
                                    currentSecurityPrincipal.UserPreferences.DefaultRoleId,
                                    currentSecurityPrincipal.UserPreferences.DefaultRoleName);

            if (callerContext == null ||
                callerContext.ActivityId == Guid.Empty)
            {
                DiagnosticActivity diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.APIFramework,
                                                                                           operationName,
                                                                                           typeName);
                diagnosticActivity.ExecutionContext.SecurityContext = securityContext;
                diagnosticActivity.Start();
                return null;
            }

            #region Context preparing

            CallDataContext callDataContext = new CallDataContext();

            ExecutionContext executionContext = new ExecutionContext(callerContext,
                                                                     callDataContext,
                                                                     securityContext,
                                                                     String.Empty);

            #endregion

            DiagnosticActivity clientDiagnosticActivity = new DiagnosticActivity(callerContext.ActivityId,
                                                                                 callerContext.OperationId,
                                                                                 executionContext);

            clientDiagnosticActivity.DoNotPersist = true;
            clientDiagnosticActivity.Start();
            MDMTraceHelper.StartTraceActivity(MDMTraceSource.APIFramework, operationName, typeName);
            return clientDiagnosticActivity;
        }

        /// <summary>
        /// Called after client calls are returned and before service responses are sent.
        /// </summary>
        /// <param name="operationName">The name of the invoked operation.</param><param name="outputs">Any output objects.</param><param name="returnValue">The return value of the operation.</param><param name="correlationState">Any correlation state returned from the <see cref="M:System.ServiceModel.Dispatcher.IParameterInspector.BeforeCall(System.String,System.Object[])"/> method, or null. </param>
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            if (MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled)
            {
                MDMTraceHelper.StopTraceActivity();
                if (correlationState != null)
                {
                    DiagnosticActivity diagnosticActivity = (DiagnosticActivity)correlationState;
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion
    }
}
