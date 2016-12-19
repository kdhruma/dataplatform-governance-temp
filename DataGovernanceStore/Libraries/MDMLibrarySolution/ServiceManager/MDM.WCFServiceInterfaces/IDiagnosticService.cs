using System;
using System.Data;
using System.ServiceModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.BusinessObjects.Diagnostics;
    using System.IO;
    using File = MDM.BusinessObjects.File;

    /// <summary>
    /// Defines operation contracts for MDM Diagnostic related operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IDiagnosticService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetApplicationDiagnostic(ApplicationDiagnosticType type, DateTime startDateTime, Int64 entityId, Int64 count, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetSystemDiagnostic(SystemDiagnosticType type, SystemDiagnosticSubType subType, Int64 count, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessDiagnosticData(DiagnosticActivityCollection diagnosticActivities, DiagnosticRecordCollection diagnosticRecords, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DiagnosticActivityCollection GetActivities(DiagnosticReportSettings diagnosticReportSettings, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DiagnosticRecordCollection GetRecords(Guid operationId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetRelatedDiagnosticRecordData(Int64 relativeDataReferanceId, DiagnosticRelativeDataType diagnosticRelativeDataType, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult StartDiagnosticTraces(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult StopDiagnosticTraces(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DiagnosticToolsReportResultWrapper ProcessDiagnosticToolsReport(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubType, String inputXml, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetReportTemplate(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubType, CallerContext callerContext);

        #region CRUD operations on DiagnosticReportProfile

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DiagnosticReportProfile GetDiagnosticReportProfileByName(String profileName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext);

        #endregion CRUD operations on DiagnosticReportProfile



    }
}
