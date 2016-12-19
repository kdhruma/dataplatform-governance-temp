using System;
using System.CodeDom;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MDM.AdminManager.Business;
using MDM.BusinessObjects.Exports;
using MDM.Core.Extensions;

namespace MDM.DiagnosticReportProcessor
{
    using BusinessObjects;
    using BusinessObjects.Jobs;
    using Core;
    using JobManager.Business;
    using ProfileManager.Business;
    using Utility;
    using Riversand.JobService.Interfaces;
    using BusinessObjects.Diagnostics;
    using ExceptionManager.Handlers;
    using Services;
    using Core.Exceptions;


    public class DiagnosticReportProcessor : IJob
    {

        #region Constants

        #endregion Constants

        #region Fields

        private Job _job;

        /// <summary>
        /// Indicates instance of JobBL
        /// </summary>
        private JobBL _jobManager = new JobBL();


        /// <summary>
        /// 
        /// </summary>
        private DiagnosticService _diagnosticService = new DiagnosticService();

        /// <summary>
        /// 
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        /// <summary>
        /// Root activity
        /// </summary>
        private DiagnosticActivity _rootActivity;

        /// <summary>
        /// Operation id
        /// </summary>
        string _operationId = "";
        
        /// <summary>
        /// 
        /// </summary>
        private DiagnosticReportProfile _diagnosticReportProfile = new DiagnosticReportProfile();

        #endregion Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Job Job
        {
            get { return _job; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool cancelJob
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        /// <summary>
        /// Property denoting identifier of _job
        /// </summary>
        public int id
        {
            get { return _job.Id; }
            set { _job.Id = value; }
        }


        /// <summary>
        /// Property denoting description about _job
        /// </summary>
        public string description
        {
            get
            {
                return _job.Description;
            }
            set
            {
                _job.Description = description;
            }
        }

        /// <summary>
        /// Proprty denoting user name who created _job
        /// </summary>
        public string username
        {
            get { return _job.CreatedUser; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        public DiagnosticReportProcessor(Job job)
        {
            if (job == null)
                throw new ArgumentNullException("Job is null");

            _job = job;
        }

        #endregion Constructors

        #region Public Methods

        public void Execute()
        {
            #region Initialize

            DiagnosticActivity activity = new DiagnosticActivity();
            String programName = "MDMCenter.Diagnostic.DiagnosticReportProcessor";

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
                activity.LogInformation(String.Format("JobEngine Execute() started _job id ({0}), _job name ({1})", _job.Id, _job.Name));
            }

            CallerContext callerContext = new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(JobType.DiagnosticReportExport), "MDMCenter.Diagnostic.DiagnosticReportProcessor", _job.Id, _job.JobData.ProfileId, _job.ProfileName);

            try
            {
                if (_job == null)
                    throw new ArgumentNullException("Job instance is not loaded");

                InitializeLogging();

                var executionContext = new ExecutionContext(callerContext, new CallDataContext(),
                    new SecurityContext(0, _job.CreatedUser, 0, ""), "");

                _rootActivity.Start(executionContext);

                //if _job is not in pending or queued status
                if (_job.JobStatus != JobStatus.Pending && _job.JobStatus != JobStatus.Queued)
                {
                    string message =
                        String.Format("Job can only be in Pending or Queued status, _job ({0}) current status is {1}",
                            _job.Id, _job.JobStatus.ToString());
                    activity.LogError(message);
                    throw new ApplicationException(message);
                }


                //TODO change subscribers from getall to profiler subscribers

            #endregion Initialize

                #region update job status to running

                _job.JobStatus = JobStatus.Running;
                _job.JobData.ExecutionStatus.StartTime = DateTime.Now.ToString();
                _jobManager.Update(_job,
                    new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));

                #endregion update job status to running

                #region Load and lock Job Profile

                DiagnosticReportExecutionStepCollection executionSteps;

                switch (_job.JobType)
                {
                    case JobType.DiagnosticReportExport:
                        _diagnosticReportProfile = GetDiagnosticReportProfile(_job.JobData.ProfileId);
                        executionSteps = _diagnosticReportProfile.ExecutionSteps;
                        _diagnosticService = new DiagnosticService();
                        break;

                    default:
                        throw new MDMOperationException("Only DiagnosticReportExport Jobtype is supported");
                }

                #endregion Load  and lock Job Profile

                #region execution

                foreach (var executionStep in executionSteps)
                {
                    if (executionStep.StepType == ExecutionStepType.Core)
                    {
                        switch (executionStep.Name.ToLower())
                        {
                            case "process":
                                // dispatch to API, no diag report engine needed, pass on excel file to subscribers as stream
                                {
                                    if (_job.JobData.JobParameters["InputXml"] == null)
                                    {
                                        _job.JobData.OperationResult.AddOperationResult("-1", String.Format("Job {0}: {1} does not contain parameter 'InputXml'", _job.Id, _job.Name), OperationResultType.Error);
                                        UpdateJobStatus(JobStatus.Aborted);
                                        break;
                                    }
                                    DiagnosticToolsReportResultWrapper resultWrapper =
                                        _diagnosticService.ProcessDiagnosticToolsReport(_diagnosticReportProfile.DiagnosticToolsReportType, _diagnosticReportProfile.DiagnosticToolsReportSubType, _job.JobData.JobParameters["InputXml"].Value, callerContext);

                                    //check returned value
                                    if (!ValidateReturnValue(resultWrapper, _job.JobData.OperationResult))
                                    {
                                        break;
                                    }

                                    //pass errors from api level to job level
                                    if (resultWrapper.OperationResult.HasError)
                                    {
                                        foreach (var error in resultWrapper.OperationResult.Errors)
                                        {
                                            _job.JobData.OperationResult.AddOperationResult("-1", String.Format("DiagnosticService.ProcessDiagnosticToolsReport: {0}", error.ErrorMessage), OperationResultType.Error);
                                        }
                                    }

                                    var excelFile = resultWrapper.File;

                                    try
                                    {
                                        //SubscriberBL subscriberManager = new SubscriberBL();
                                        //Collection<String> subscriberCollectionAsString = ValueTypeHelper.SplitStringToStringCollection(_diagnosticReportProfile.DataSubscribers, ',');
                                        //if (subscriberCollectionAsString.IsNullOrEmpty())
                                        //{
                                        //    throw new Exception(
                                        //        String.Format("Profile {0} cannot find any subscribers. Excel file cannot be exported", _diagnosticReportProfile.Name));
                                        //}

                                        //foreach (var subscriberName in subscriberCollectionAsString)
                                        //{
                                        //    ExportSubscriber subscriber = subscriberManager.GetByName(subscriberName, callerContext);
                                        //    if (subscriber == null)
                                        //    {
                                        //        _job.JobData.OperationResult.AddOperationResult("-1",
                                        //            String.Format("DiagnosticReportProfile {0} contains subscriber {1}, which is not found in the current system", _diagnosticReportProfile.Name, subscriberName), OperationResultType.Error);
                                        //    }
                                        //    else if (String.IsNullOrEmpty(subscriber.GetConfigurationValue("FileDirectory")))
                                        //    {
                                        //        _job.JobData.OperationResult.AddOperationResult("-1", String.Format(@"Entry 'FileDirectory' is not found or not valid in Subscriber {0}", subscriber.Name), OperationResultType.Error);
                                        //    }
                                        //    else if (String.IsNullOrEmpty(subscriber.GetConfigurationValue("FailoverDirectory")))
                                        //    {
                                        //        _job.JobData.OperationResult.AddOperationResult("-1", String.Format(@"Entry 'FailoverDirectory' is not found or not valid in Subscriber {0}", subscriber.Name), OperationResultType.Error);
                                        //    }
                                        //    else if (!Directory.Exists(subscriber.GetConfigurationValue("FileDirectory")))
                                        //    {
                                        //        _job.JobData.OperationResult.AddOperationResult("-1", String.Format("FileDirectory '{0}' of Subscriber '{1}' of Profile '{2}' does not exist or not accessible",subscriber.GetConfigurationValue("FileDirectory"), subscriber.Name, _diagnosticReportProfile.Name), OperationResultType.Error);
                                        //    }
                                        //    else if (!Directory.Exists(subscriber.GetConfigurationValue("FailoverDirectory")))
                                        //    {
                                        //        _job.JobData.OperationResult.AddOperationResult("-1", String.Format("FailoverDirectory '{0}' of Subscriber '{1}' of Profile '{2}' does not exist or not accessible", subscriber.GetConfigurationValue("FailoverDirectory"), subscriber.Name, _diagnosticReportProfile.Name), OperationResultType.Error);
                                        //    }
                                        //    else
                                        //    {
                                        //        _job.ProfileId = _job.JobData.ProfileId;
                                        //        _job.ProfileType = _diagnosticReportProfile.DiagnosticToolsReportType.ToString();
                                        //        using (Stream excelStream = new MemoryStream(excelFile.FileData))
                                        //        {
                                        //            ProccessFile(excelStream, "xlsx",
                                        //                subscriber.GetConfigurationValue("FileDirectory"),
                                        //                subscriber.GetConfigurationValue("FailoverDirectory"), subscriberName);
                                        //        }
                                        //    }
                                        //}
                                    }
                                    catch (Exception subscriberException)
                                    {
                                        description = "Error occurred during file transfer to subscribers";
                                        _job.JobData.OperationResult.AddOperationResult("-1",
                                            String.Format(
                                                "Exception occurred for diagnostic report export job Id {0} : {1}. Additional information: {2}",
                                                _job.Id, description, subscriberException.Message),
                                            OperationResultType.Error);
                                        UpdateJobStatus(JobStatus.Aborted);
                                    }
                                    break;
                                }
                        }
                    }
                    else if (executionStep.StepType == ExecutionStepType.Custom)
                    {
                        RunExternalDiagnosticReportStep(executionStep);
                    }
                }

                #endregion execution

                #region job finalizing

            }
            catch (Exception ex)
            {
                description = String.Format("Diagnostic Report Export Job ID {0} failed", _job.Id);
                _job.JobData.OperationResult.AddOperationResult("-1", String.Format("Exception occurred for diagnostic report export job Id {0} : {1} \nAdditional information {2}", _job.Id, description, ex.Message), OperationResultType.Error);
                UpdateJobStatus(JobStatus.Aborted);
            }
            finally
            {
                //update job status to completed
                _job.JobData.OperationResult.RefreshOperationResultStatus();
                if (_job.JobData.OperationResult.HasError)
                {
                    UpdateJobStatus(JobStatus.CompletedWithErrors);
                }
                if (!_job.JobData.OperationResult.HasError)
                {
                    UpdateJobStatus(JobStatus.Completed);
                }
            }

                #endregion job finalizing
        }

        /// <summary>
        /// entry point for diagnostic report job
        /// </summary>
        /// <param name="currentExecutionStep"></param>
        /// <returns></returns>
        private Boolean RunExternalDiagnosticReportStep(DiagnosticReportExecutionStep currentExecutionStep)
        {
            string message;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            String assemblyFileName = currentExecutionStep.AssemblyFileName;
            String classFullName = currentExecutionStep.ClassFullName;
            String methodName = "RunStep";

            //Check if method exists..
            if (!ExtensionHelper.CheckMethod(assemblyFileName, classFullName, methodName))
            {
                message = String.Format("Unable to run external diagnostic report step. Provided assembly does not exist. ExecutionStep: {0}, Assembly Name: {1}, ClassName: {2}, Method: {3}", currentExecutionStep.Name, assemblyFileName, classFullName, methodName);
                throw new ApplicationException(message);
            }

            try
            {
                object[] parameters = new object[] { currentExecutionStep, Job, _diagnosticReportProfile };
                ExtensionHelper.InvokeMethod(assemblyFileName, classFullName, methodName, parameters);

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = string.Format("Invoked external diagnostic report step - assembly ({0}), classname ({1}), methodname ({2})",
                        assemblyFileName, classFullName, methodName);

                    activity.LogInformation(message);
                }
            }
            catch (Exception ex)
            {
                message = String.Format("Unhandled exception occurred while running execution step. ExecutionStep: {0}, Assembly Name: {1}, ClassName: {2}, Method: {3}", currentExecutionStep.Name, assemblyFileName, classFullName, methodName);
                throw new ApplicationException(message, ex);
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        private DiagnosticReportProfile GetDiagnosticReportProfile(int profileId)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (profileId < 1)
            {
                string message = string.Format("Invalid diagnostic report profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            DiagnosticReportProfileBL diagnosticReportProfileManager = new DiagnosticReportProfileBL();

            DiagnosticReportProfile reportProfile = diagnosticReportProfileManager.Get(profileId,
                new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Unknown));

            if (reportProfile == null)
            {
                string message = string.Format("Unable to retrieve diagnostic report profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Retrieved diagnostic report profile - Id ({0})", profileId));
                activity.Stop();
            }

            return reportProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        private DiagnosticReportProfile GetDiagnosticReportProfile(String profileName)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (String.IsNullOrEmpty(profileName))
            {
                string message = "Invalid diagnostic report profile name";
                throw new ApplicationException(message);
            }

            DiagnosticReportProfileBL diagnosticReportProfileManager = new DiagnosticReportProfileBL();

            DiagnosticReportProfile reportProfile = diagnosticReportProfileManager.Get(profileName, new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Instrumentation));

            if (reportProfile == null)
            {
                string message = string.Format("Profile {0} not found", profileName);
                throw new ApplicationException(message);
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Retrieved diagnostic report profile [{0}]", profileName));
                activity.Stop();
            }

            return reportProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CleanUp()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            if (_job.JobStatus == JobStatus.Completed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsIgnored()
        {
            if (_job.JobStatus == JobStatus.Ignored)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveJob()
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            String programName = String.Format("JobEngine: JobId: {0}, Name:{1} updated", _job.Id, _job.Name);
            _jobManager.Update(_job, new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));
            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        public OperationResult CreateJob()
        {
            String programName = String.Format("JobEngine: JobId: {0}, Name:{1} created", _job.Id, _job.Name);
            return _jobManager.Create(_job, new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        void InitializeLogging()
        {
            if (_job.JobData == null) return;

            bool isDiagnostics = false;
            if (_job.JobData.JobParameters.Contains("ShowTrace"))
            {
                JobParameter showTrace = _job.JobData.JobParameters["ShowTrace"];
                isDiagnostics = ValueTypeHelper.BooleanTryParse(showTrace.Value, false);
            }

            if (_job.JobData.JobParameters.Contains("OperationId"))
            {
                JobParameter jobParam = _job.JobData.JobParameters["OperationId"];
                _operationId = jobParam.Value;
            }

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            Guid operationIdGuid = string.IsNullOrEmpty(_operationId) ? Guid.Empty : Guid.Parse(_operationId);
            _rootActivity = DiagnosticActivity.GetRootActivity("DiagnosticProcessorRootActivity", _traceSettings, operationIdGuid, isDiagnostics);
        }

        /// <summary>
        /// stream handler - if failed to write to fileDirectory, method will write to failover directory; if failed again raise error for that subscriber - should the job deeemed failed at this point ?.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="extension"></param>
        /// <param name="fileDirectory"></param>
        /// <param name="failoverDirectory"></param>
        /// <param name="currentSubscriber"></param>
        /// <param name="persistFileToDB"></param>
        private void ProccessFile(Stream inputStream, String extension, String fileDirectory, String failoverDirectory, String currentSubscriber)
        {
            String time = DateTime.Now.ToString("MMddyyyy_HH");
            try
            {
                var fileName = String.Format(@"{0}\{1}_{2}.{3}", fileDirectory,
                    _diagnosticReportProfile.DiagnosticToolsReportType,
                    time, extension);
                
                //save filepath just in case
                JobParameter filePathParameter = new JobParameter("filePath", fileName);
                _job.JobData.JobParameters.Add(filePathParameter);
                
                using (FileStream output = System.IO.File.OpenWrite(fileName))
                {
                    inputStream.CopyTo(output);
                }

            }
            catch (IOException)
            {
                _job.JobData.OperationResult.AddOperationResult("-1", String.Format("Job ID: {0} {1} failed to write excel report file to directory {2}", _job.Id, _job.Name, fileDirectory), OperationResultType.Error);


                try // job failed to write to filedirectory - here try again on failoverdirectory
                {
                    var fileName = String.Format(@"{0}\{1}_{2}.{3}", failoverDirectory, _diagnosticReportProfile.DiagnosticToolsReportType, time, extension);
                    if (inputStream.CanSeek) inputStream.Position = 0;
                    using (FileStream output = System.IO.File.OpenWrite(fileName))
                    {
                        inputStream.CopyTo(output);
                    }

                    // now register excel file with tb_File
                    FileBL fileManager = new FileBL();
                    _job.FileId = fileManager.UploadFile(fileName,
                        new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Instrumentation,
                            "Diagnostic Report Export", _job.Id, _job.ProfileId, _job.ProfileName));
                }
                catch (IOException)
                {
                    _job.JobData.OperationResult.AddOperationResult("-1", String.Format("Job ID: {0} '{1}' is aborted because subscriber '{2}' cannot write data to both FileDirectory and Failover Directory", _job.Id, _job.Name, currentSubscriber), OperationResultType.Error);
                    UpdateJobStatus(JobStatus.Aborted);
                }
            }

            inputStream.Close();
        }

        #region email subscribers

        /// <summary>
        /// 
        /// </summary>
        private void SendBeginMail()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendCompleteMail()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendFailureMail()
        {
            //throw new NotImplementedException();
        }

        #endregion email subscribers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        private void UpdateJobStatus(JobStatus status)
        {
            switch (status)
            {
                case JobStatus.Running:
                    SendBeginMail();
                    _job.JobStatus = JobStatus.Running;
                    break;

                case JobStatus.Completed:
                    _job.JobData.ExecutionStatus.EndTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                    _job.JobStatus = JobStatus.Completed;
                    _job.JobData.ExecutionStatus.CurrentStatusMessage = status.ToString();
                    _job.JobData.OperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    SendCompleteMail();
                    SaveJob();
                    break;

                case JobStatus.Aborted:
                    _job.JobData.ExecutionStatus.EndTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                    _job.JobStatus = JobStatus.Aborted;
                    _job.JobData.ExecutionStatus.CurrentStatusMessage = status.ToString();
                    _job.JobData.OperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    SendFailureMail();
                    SaveJob();
                    break;

                case JobStatus.CompletedWithErrors:
                    _job.JobData.ExecutionStatus.EndTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                    _job.JobStatus = JobStatus.CompletedWithErrors;
                    _job.JobData.ExecutionStatus.CurrentStatusMessage = status.ToString();
                    _job.JobData.OperationResult.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
                    SendFailureMail();
                    SaveJob();
                    break;

                case JobStatus.CompletedWithWarnings:
                case JobStatus.CompletedWithWarningsAndErrors:
                    break;
            }
        }


        /// <summary>
        /// if no excel file is generated or DiagnosticService.ProcessDiagnosticToolsReport return value doesnot contain operationresult, abort the job. 
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="jobOperationResult"></param>
        /// <returns></returns>
        private Boolean ValidateReturnValue(DiagnosticToolsReportResultWrapper returnValue, OperationResult jobOperationResult)
        {
            Boolean result = true;

            if (returnValue.File == null || returnValue.OperationResult == null)
            {
                jobOperationResult.AddOperationResult("-1",
                    "DiagnosticService.ProcessDiagnosticToolsReport encountered a problem during API call.", OperationResultType.Error);
                UpdateJobStatus(JobStatus.Aborted);
                result = false;
            }

            return result;
        }

        #endregion Private Methods
    }
}
