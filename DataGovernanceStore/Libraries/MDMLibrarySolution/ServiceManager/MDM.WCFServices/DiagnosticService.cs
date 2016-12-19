using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.WCFServiceInterfaces;
    using MDM.DiagnosticManager.Business;
    using MDM.DiagnosticData.Business;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ProfileManager.Business;
    using File = MDM.BusinessObjects.File;
    using MDM.Utility;

    /// <summary>
    /// 
    /// </summary>
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class DiagnosticService : MDMWCFBase, IDiagnosticService
    {
        #region Fields
        #endregion

        #region Constructors

        public DiagnosticService()
            : base(true)
        {

        }

        public DiagnosticService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Retrieves the application related diagnostic information
        /// </summary>
        /// <param name="applicationDiagnosticType">Indicates the application diagnostic type</param>
        /// <param name="startDateTime">Indicates start date time to get information from date time</param>
        /// <param name="entityId">Indicates the Id of an entity</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns>Returns JSON String</returns>
        public String GetApplicationDiagnostic(ApplicationDiagnosticType applicationDiagnosticType, DateTime startDateTime, Int64 entityId, Int64 count, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticBL, String>("GetApplicationDiagnostic", callerContext, 
                                                                     businessLogic => businessLogic.GetApplicationDiagnostic(applicationDiagnosticType, startDateTime, entityId, count, callerContext));
        }

        /// <summary>
        /// Retrieves the system related diagnostic information
        /// </summary>
        /// <param name="systemDiagnosticType">Indicates the system diagnostic type</param>
        /// <param name="systemDiagnosticSubType">Indicates the system diagnostic sub type</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns>Returns JSON String</returns>
        public String GetSystemDiagnostic(SystemDiagnosticType systemDiagnosticType, SystemDiagnosticSubType systemDiagnosticSubType, Int64 count, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticBL, String>("GetSystemDiagnostic", callerContext, 
                                                                     businessLogic => businessLogic.GetSystemDiagnostic(systemDiagnosticType, systemDiagnosticSubType, count, callerContext));
        }

        /// <summary>
        /// Process DiagnosticActivity and Records collection
        /// </summary>
        /// <param name="diagnosticActivities"></param>
        /// <param name="diagnosticRecords"></param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns></returns>
        public Boolean ProcessDiagnosticData(DiagnosticActivityCollection diagnosticActivities, DiagnosticRecordCollection diagnosticRecords, CallerContext callerContext)
        {
            ExecutionContext executionContext = new ExecutionContext();

            if (!executionContext.LegacyMDMTraceSources.Contains(MDMTraceSource.DiagnosticService))
            {
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.DiagnosticService);
            }

            Boolean operationResult;

            try
            {
                DiagnosticDataBL businessLogic = new DiagnosticDataBL();
                operationResult = businessLogic.Process(diagnosticActivities, diagnosticRecords, callerContext);
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }

            return operationResult;
        }

        /// <summary>
        /// Get DiagnosticActivity collection
        /// </summary>
        /// <param name="diagnosticReportSettings">Indicates Diagnostic Report settings</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns>DiagnosticActivityCollection</returns>
        public DiagnosticActivityCollection GetActivities(DiagnosticReportSettings diagnosticReportSettings, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticDataBL, DiagnosticActivityCollection>("GetActivities", callerContext, 
                businessLogic => businessLogic.GetActivities(diagnosticReportSettings, callerContext), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// Get DiagnosticRecord collection
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DiagnosticRecordCollection GetRecords(Guid operationId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticDataBL, DiagnosticRecordCollection>("GetRecords", callerContext, 
                businessLogic => businessLogic.GetRecords(operationId, callerContext), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// Retrieves the related diagnostic record data based on context.
        /// </summary>
        /// <param name="relativeDataReferanceId">Indicates relative data reference id for diagnostic record.</param>
        /// <param name="diagnosticRelativeDataType">Indicates relative data type for diagnostic record.</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns>returns related diagnostic record data as string</returns>
        public String GetRelatedDiagnosticRecordData(Int64 relativeDataReferanceId, DiagnosticRelativeDataType diagnosticRelativeDataType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticBL, String>("GetRelatedDiagnosticRecordData", callerContext, 
                businessLogic => businessLogic.GetRelatedDiagnosticRecordData(relativeDataReferanceId, diagnosticRelativeDataType, callerContext), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// Starts the diagnostic traces.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context which contains the MDM application and MDM module</param>
        /// <returns>Returns the operation result based on the result</returns>
        public OperationResult StartDiagnosticTraces(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticDataBL, OperationResult>("StartDiagnosticTraces", callerContext, 
                businessLogic => businessLogic.StartDiagnosticTraces(callerContext), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// Stop the diagnostic traces
        /// </summary>
        /// <param name="callerContext">Indicates the caller context which contains the MDM application and MDM module</param>
        /// <returns>Returns the operation result based on the result</returns>
        public OperationResult StopDiagnosticTraces(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticDataBL, OperationResult>("StopDiagnosticTraces", callerContext, 
                businessLogic => businessLogic.StopDiagnosticTraces(callerContext), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// make calls to generate excel report based on reporttype and sub type
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubtype"></param>
        /// <param name="inputXml"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DiagnosticToolsReportResultWrapper ProcessDiagnosticToolsReport(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubtype, String inputXml, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticBL, DiagnosticToolsReportResultWrapper>("ProcessDiagnosticToolsReport", callerContext,
                                                                     businessLogic => businessLogic.ProcessDiagnosticToolsReport(reportType, reportSubtype, inputXml, callerContext));
        }

        /// <summary>
        /// retrieve the report template xml based on reporttype and reportsubtype
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public String GetReportTemplate(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticBL, String>("GetReportTemplate", callerContext,
                                                                     businessLogic => businessLogic.GetReportTemplate(reportType, reportSubType, callerContext));
        }

        #region CRUD operations on DiagnosticReportProfile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult CreateDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticReportProfileBL, OperationResult>("Create", callerContext,
                                                                     businessLogic => businessLogic.Create(diagnosticReportProfile, callerContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult UpdateDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticReportProfileBL, OperationResult>("Update", callerContext,
                                                                     businessLogic => businessLogic.Update(diagnosticReportProfile, callerContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DiagnosticReportProfile GetDiagnosticReportProfileByName(String profileName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticReportProfileBL, DiagnosticReportProfile>("Get", callerContext,
                                                                     businessLogic => businessLogic.Get(profileName, callerContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult DeleteDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DiagnosticReportProfileBL, OperationResult>("Delete", callerContext,
                                                                     businessLogic => businessLogic.Delete(diagnosticReportProfile, callerContext));
        }


        #endregion CRUD operations on DiagnosticReportProfile


        #endregion

        #region Private Methods

        /// <summary>
        /// Makes calls of Diagnostic Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <param name="traceSource"></param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(string methodName, CallerContext callerContext, Func<TBusinessLogic, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General) where TBusinessLogic : BusinessLogicBase, new()
        {
            ExecutionContext executionContext = new ExecutionContext();
            executionContext.LegacyMDMTraceSources.Add(traceSource);

            if (!executionContext.LegacyMDMTraceSources.Contains(MDMTraceSource.DiagnosticService))
            {
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.DiagnosticService);
            }

            if (callerContext != null &&
                callerContext.TraceSettings != null)
            {
                MDMOperationContextHelper.LoadOperationTracingSettings(callerContext.OperationId, callerContext.TraceSettings);
            }

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            DiagnosticActivity clientDiagnosticActivity = null;
            if (isTracingEnabled &&
                callerContext != null &&
                callerContext.ActivityId != Guid.Empty)
            {
                #region Context preparing

                CallDataContext callDataContext = new CallDataContext();

                SecurityPrincipal currentSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                SecurityContext securityContext =
                    new SecurityContext(currentSecurityPrincipal.UserPreferences.LoginId,
                                        currentSecurityPrincipal.UserPreferences.LoginName,
                                        currentSecurityPrincipal.UserPreferences.DefaultRoleId,
                                        String.Empty);

                ExecutionContext clientExecutionContext = new ExecutionContext(callerContext,
                                                                               callDataContext,
                                                                               securityContext,
                                                                               String.Empty);

                #endregion

                clientDiagnosticActivity = new DiagnosticActivity(callerContext.ActivityId,
                                                                  callerContext.OperationId,
                                                                  clientExecutionContext);

                clientDiagnosticActivity.DoNotPersist = true;
                clientDiagnosticActivity.Start(clientExecutionContext);
            }

            DiagnosticActivity activity = new DiagnosticActivity(null, "DiagnosticService." + methodName);

            //Start trace activity
            if (isTracingEnabled)
            {
                activity.Start();
            }

            TResult operationResult;

            try
            {
                if (isTracingEnabled)
                {
                    activity.LogVerbose("DiagnosticService receives" + methodName + " request message.");
                }

                operationResult = call(new TBusinessLogic());

                if (isTracingEnabled)
                {
                    activity.LogVerbose("DiagnosticService receives" + methodName + " response message.");
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    activity.Stop();

                    if (clientDiagnosticActivity != null)
                    {
                        clientDiagnosticActivity.Stop();
                    }
                }
            }

            return operationResult;
        }

        #endregion #region Private Methods

        #endregion
    }
}
