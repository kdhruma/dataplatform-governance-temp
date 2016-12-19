using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Imports.Processor
{
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.JobManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.Interfaces;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// 
    /// </summary>
    public class JobImportResultHandler : IJobResultHandler
    {
        #region Fields

        private IImportProgressHandler _progressHandler = null;

        private IImportProgressHandler _relationshipProgressHandler = null;

        /// <summary>
        /// 
        /// </summary>
        private JobImportResultBL _jobImportResultManager = new JobImportResultBL();

        private Int32 _jobId = 0;

        private String _userName = string.Empty;

        private String _programName = string.Empty;

        private Int64 _auditRefId = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        public JobImportResultHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        public JobImportResultHandler(Int32 jobId)
        {
            this._jobId = jobId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public JobImportResultBL JobImportResultManager
        {
            get
            {
                return _jobImportResultManager;
            }
        }

        public IImportProgressHandler ProgressHandler
        {
            get { return this._progressHandler; }
            set { this._progressHandler = value; }
        }

        public IImportProgressHandler RelationshipProgressHandler
        {
            get { return this._relationshipProgressHandler; }
            set { this._relationshipProgressHandler = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProgramName
        {
            get { return this._programName; }
            set { this._programName = value; }
        }

        /// <summary>
        /// Job Id
        /// </summary>
        public Int32 JobId
        {
            get { return this._jobId; }
            set { this._jobId = value; }
        }

        /// <summary>
        /// Job Id
        /// </summary>
        public Int64 AuditRefId
        {
            get { return this._auditRefId; }
            set { this._auditRefId = value; }
        }
        #endregion

        #region Public methods
        
        /// <summary>
        /// Converts the Entity operation result to Import job result collection
        /// </summary>
        /// <param name="operationResults"></param>
        /// <returns></returns>
        public bool Save(EntityOperationResultCollection operationResults)
        {
            return Save(operationResults, false);
        }

        /// <summary>
        /// Converts the Entity operation result to Import job result collection
        /// </summary>
        /// <param name="operationResults">Entity Operation Result</param>
        /// <param name="saveSuccessEntities">True to save success Entities</param>
        /// <returns>true if saved </returns>
        public Boolean Save(EntityOperationResultCollection operationResults, Boolean saveSuccessEntities)
        {
            if (operationResults == null || operationResults.Count <= 0)
            {
                return false;
            }

            JobImportResultCollection importResults = new JobImportResultCollection();
            foreach (EntityOperationResult operationResult in operationResults)
            {
                JobImportResult importResult = new JobImportResult();

                importResult.ExternalId = operationResult.ExternalId;
                importResult.InternalId = operationResult.EntityId;
                importResult.PerformedAction = operationResult.PerformedAction;
                importResult.JobId = JobId;
                importResult.AuditRefId = AuditRefId;
                importResult.Status = (operationResult.OperationResultStatus == Core.OperationResultStatusEnum.Successful) ? "Success" : "Failed";

                // Store the XML only if result is not success
                if (operationResult.OperationResultStatus != Core.OperationResultStatusEnum.Successful)
                {
                    importResult.OperationResultXML = operationResult.ToXml(Core.ObjectSerialization.External);
                }
                else if (saveSuccessEntities && (operationResult.OperationResultStatus == Core.OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == Core.OperationResultStatusEnum.None))
                {
                    importResult.OperationResultXML = operationResult.ToXml(Core.ObjectSerialization.External);
                }
                importResults.Add(importResult);
            }

            return _jobImportResultManager.Save(importResults, Core.MDMCenterApplication.JobService, Core.MDMCenterModules.Import, ProgramName, UserName);
        }

        /// <summary>
        /// Converts the Entity operation result to Import job result collection
        /// </summary>
        /// <param name="operationResults">Entity Operation Result</param>
        /// <param name="saveSuccessEntities">True to save success Entities</param>
        /// <returns>true if saved </returns>
        public Boolean Save(OperationResultCollection objOperationResults, Boolean bSaveSuccessEntities)
        {
            if (objOperationResults == null || objOperationResults.Count <= 0)
            {
                return false;
            }

            JobImportResultCollection importResults = new JobImportResultCollection();

            bool resultSuccessOrNone = false;

            foreach (OperationResult objOperationResult in objOperationResults)
            {
                JobImportResult importResult = new JobImportResult();

                importResult.ExternalId = Convert.ToString(objOperationResult.ReferenceId);
                importResult.InternalId = objOperationResult.Id;
                importResult.JobId = JobId;
                importResult.AuditRefId = AuditRefId;
                resultSuccessOrNone = ( objOperationResult.OperationResultStatus == Core.OperationResultStatusEnum.Successful ||
                                        objOperationResult.OperationResultStatus == Core.OperationResultStatusEnum.None); 
                //importResult.ImportObjectType = objOperationResult.ImportObjectType;

                importResult.Status = (resultSuccessOrNone)? "Success" : "Failed";

                // Store the XML only if result is not success
                if (!resultSuccessOrNone || (bSaveSuccessEntities && (resultSuccessOrNone)))
                {
                    importResult.OperationResultXML = objOperationResult.ToXml(Core.ObjectSerialization.External);
                }

                importResults.Add(importResult);
            }

            return _jobImportResultManager.Save(importResults, Core.MDMCenterApplication.JobService, Core.MDMCenterModules.Import, ProgramName, UserName);
        }

        /// Converts the Entity operation result to Import job result collection
        /// </summary>
        /// <param name="operationResults">Entity Operation Result</param>
        /// <param name="saveSuccessEntities">True to save success Entities</param>
        /// <returns>true if saved </returns>
        public Boolean Save(DataModelOperationResultCollection objDataModelOperationResults, Boolean bSaveSuccessEntities)
        {
            if (objDataModelOperationResults == null || objDataModelOperationResults.Count <= 0)
            {
                return false;
            }

            JobImportResultCollection importResults = new JobImportResultCollection();

            bool resultSuccessOrNone = false;

            foreach (DataModelOperationResult operationResult in objDataModelOperationResults)
            {
                JobImportResult importResult = new JobImportResult();

                importResult.ExternalId = String.Format("[{0}] - {1}", operationResult.ReferenceId, operationResult.ExternalId);
                importResult.InternalId = operationResult.Id;
                importResult.JobId = JobId;
                importResult.AuditRefId = AuditRefId;
                resultSuccessOrNone = (operationResult.OperationResultStatus == Core.OperationResultStatusEnum.Successful ||
                                       operationResult.OperationResultStatus == Core.OperationResultStatusEnum.None);

                importResult.ObjectType = operationResult.DataModelObjectType;

                importResult.PerformedAction = operationResult.PerformedAction;
                importResult.Status = (resultSuccessOrNone) ? "Success" : "Failed";

                // Store the XML only if result is not success
                if (!resultSuccessOrNone || (bSaveSuccessEntities && (resultSuccessOrNone)))
                {
                    importResult.OperationResultXML = operationResult.ToXml(Core.ObjectSerialization.External);
                }

                importResults.Add(importResult);
            }

            return _jobImportResultManager.Save(importResults, Core.MDMCenterApplication.JobService, Core.MDMCenterModules.Import, ProgramName, UserName);
        }

        /// <summary>
        /// Converts DataModelOperationResultSummary to Import job result collection
        /// </summary>
        /// <param name="dataModelOperationResultSummaryCollection"></param>
        /// <param name="_application"></param>
        /// <param name="_module"></param>
        /// <param name="importProgram"></param>
        /// <param name="importUser"></param>
        /// <returns></returns>
        public Boolean Save(DataModelOperationResultSummaryCollection dataModelOperationResultSummaryCollection, Core.ObjectAction jobResultAction)
        {
            if (dataModelOperationResultSummaryCollection == null || dataModelOperationResultSummaryCollection.Count <= 0)
            {
                return false;
            }

            JobImportResultCollection importResults = new JobImportResultCollection();

            ObjectType objectType;

            foreach (DataModelOperationResultSummary objDataModelOperationResult in dataModelOperationResultSummaryCollection)
            {
                JobImportResult importResult = new JobImportResult();

                importResult.Action = jobResultAction;
                importResult.ObjectType = ObjectType.None;

                if (Enum.TryParse<Core.ObjectType>(objDataModelOperationResult.ObjectType, true, out objectType))
                {
                    importResult.ObjectType = objectType;
                }

                importResult.ExternalId = objDataModelOperationResult.ExternalId;
                importResult.InternalId = objDataModelOperationResult.InternalId;
                importResult.JobId = JobId;
                importResult.AuditRefId = AuditRefId;
                importResult.PerformedAction = Core.ObjectAction.Unknown;
                importResult.Status = objDataModelOperationResult.SummaryStatus.ToString();

                importResult.OperationResultXML = objDataModelOperationResult.ToXml(Core.ObjectSerialization.External);

                importResults.Add(importResult);
            }

            return _jobImportResultManager.Save(importResults, Core.MDMCenterApplication.JobService, Core.MDMCenterModules.Import, ProgramName, UserName);
        }

        /// Converts the DDG operation result to Import job result collection
        /// </summary>
        /// <param name="operationResults">Indicates the business rule operation Result</param>
        /// <param name="saveSuccessEntities">Indicates a boolean value specifying true to save success entries</param>
        /// <returns>Returns true if the value is saved successfully; otherwise false.</returns>
        public Boolean Save(BusinessRuleOperationResultCollection ddgOperationResults, Boolean bSaveSuccessEntities)
        {
            if (ddgOperationResults == null || ddgOperationResults.Count <= 0)
            {
                return false;
            }

            JobImportResultCollection importResults = new JobImportResultCollection();

            Boolean resultSuccessOrNone = false;

            foreach (BusinessRuleOperationResult operationResult in ddgOperationResults)
            {
                JobImportResult importResult = new JobImportResult();

                importResult.ExternalId = operationResult.ReferenceId.ToString();//TODO: To check
                importResult.InternalId = operationResult.Id;
                importResult.JobId = JobId;
                importResult.AuditRefId = AuditRefId;
                resultSuccessOrNone = (operationResult.OperationResultStatus == Core.OperationResultStatusEnum.Successful ||
                                       operationResult.OperationResultStatus == Core.OperationResultStatusEnum.None);

                importResult.ObjectType = operationResult.DDGObjectType;//TODO: Need to check

                importResult.PerformedAction = operationResult.PerformedAction;
                importResult.Status = (resultSuccessOrNone) ? "Success" : "Failed";

                // Store the XML only if result is not success
                if (!resultSuccessOrNone || (bSaveSuccessEntities && (resultSuccessOrNone)))
                {
                    importResult.OperationResultXML = operationResult.ToXml(Core.ObjectSerialization.External);
                }

                importResults.Add(importResult);
            }

            return _jobImportResultManager.Save(importResults, Core.MDMCenterApplication.JobService, Core.MDMCenterModules.Import, ProgramName, UserName);
        }

        /// <summary>
        /// Converts DDG operation result summary collection to Import job result collection
        /// </summary>
        /// <param name="ddgOperationResultSummaryCollection">Indicates the DDG operation result summary collection</param>
        /// <param name="jobResultAction">Indicates the job result action</param>
        /// <returns>Returns true if value is saved successfully; otherwise false.</returns>
        public Boolean Save(DDGOperationResultSummaryCollection ddgOperationResultSummaryCollection, Core.ObjectAction jobResultAction)
        {
            if (ddgOperationResultSummaryCollection == null || ddgOperationResultSummaryCollection.Count <= 0)
            {
                return false;
            }

            JobImportResultCollection importResults = new JobImportResultCollection();

            ObjectType objectType;

            foreach (DDGOperationResultSummary operationResultSummary in ddgOperationResultSummaryCollection)
            {
                JobImportResult importResult = new JobImportResult();

                importResult.Action = jobResultAction;
                importResult.ObjectType = ObjectType.None;

                if (Enum.TryParse<Core.ObjectType>(operationResultSummary.ObjectType, true, out objectType))
                {
                    importResult.ObjectType = objectType;
                }

                importResult.ExternalId = operationResultSummary.ExternalId;
               // importResult.InternalId = objDataModelOperationResult.InternalId;//TODO: Need to check
                importResult.JobId = JobId;
                importResult.AuditRefId = AuditRefId;
                importResult.PerformedAction = Core.ObjectAction.Unknown;
                importResult.Status = operationResultSummary.SummaryStatus.ToString();

                importResult.OperationResultXML = operationResultSummary.ToXml();//TODO: Need to check

                importResults.Add(importResult);
            }

            return _jobImportResultManager.Save(importResults, Core.MDMCenterApplication.JobService, Core.MDMCenterModules.Import, ProgramName, UserName);
        }

        /// <summary>
        /// Converts the Entity operation result to Import job result collection and adds the number of errors/warning to the total count.
        /// The caller is expected to set the individual status of each operation result to 'Failed'
        /// </summary>
        /// <param name="operationResults"></param>
        /// <returns></returns>
        public bool SaveWithCount(EntityOperationResultCollection operationResults)
        {
            if (Save(operationResults))
            {
                Int32 failedCount = 0;
                Int32 partiallySuccessfulCount = 0;
                Int32 SuccessfulCount = 0;

                if (operationResults != null)
                {
                    var failedEntities = from original in operationResults
                                         where
                                         (original.OperationResultStatus == Core.OperationResultStatusEnum.Failed)
                                         select original;

                    if (failedEntities.Any())
                    {
                        failedCount = failedEntities.Count();
                    }
                }

                if (operationResults != null)
                {
                    var partiallySuccessfullEntities = from original in operationResults
                                         where
                                         (original.OperationResultStatus == Core.OperationResultStatusEnum.CompletedWithWarnings)
                                         select original;

                    if (partiallySuccessfullEntities.Any())
                    {
                        partiallySuccessfulCount = partiallySuccessfullEntities.Count();
                    }
                }

                if (operationResults != null)
                {
                    var SuccessfullEntities = from original in operationResults
                                                       where
                                                       (original.OperationResultStatus == Core.OperationResultStatusEnum.None)
                                                       select original;

                    if (SuccessfullEntities.Any())
                    {
                        SuccessfulCount = SuccessfullEntities.Count();
                    }
                }

                ProgressHandler.UpdateFailedEntities(failedCount);
                ProgressHandler.UpdatePartialSuccessFulEntities(partiallySuccessfulCount);
                ProgressHandler.UpdateSuccessFulEntities(SuccessfulCount);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void ResetCounts()
        {
            ProgressHandler.ResetEntityCounts();
        }

        #endregion
    }
}
