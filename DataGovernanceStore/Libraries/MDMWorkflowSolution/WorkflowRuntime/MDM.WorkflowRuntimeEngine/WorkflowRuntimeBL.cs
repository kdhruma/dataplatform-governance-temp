using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.WorkflowRuntimeEngine
{
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.CacheManager.Business;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.Workflow.Designer.Business;
    using MDM.ParallelizationManager.Processors;
    using MDM.AdminManager.Business;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Interfaces;

    public class WorkflowRuntimeBL : BusinessLogicBase, IWorkflowRuntimeManager
    {
        #region Fields

        private static HashSet<String> participatingMDMObjectsTempSet = new HashSet<String>();

        private TraceSettings _currentTraceSettings = null;

        #endregion

        #region Constructors

        public WorkflowRuntimeBL()
            : base()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            //LoadWorkflowEngineSecurityPrincipal();
        }

        #endregion

        #region Methods

        #region Start Workflow

        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context which will go as part of the instance lifetime</param>
        /// <param name="serviceType">The type of the service which is invoking the workflow</param>
        /// <param name="serviceId">The unique identification of the service</param>
        /// <param name="currentUserName">The user name who is requesting for the start</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>The success count</returns>
        public Int32 StartWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 result = this.StartWorkflow(workflowDataContext, serviceType, serviceId, currentUserName, WorkflowInstanceRunOptions.RunAsMultipleInstances, ref operationResult, callerContext);
            return result;
        }

        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context which will go as part of the instance lifetime</param>
        /// <param name="serviceType">The type of the service which is invoking the workflow</param>
        /// <param name="serviceId">The unique identification of the service</param>
        /// <param name="currentUserName">The user name who is requesting for the start</param>
        /// <param name="workflowInstanceRunOption">The run option which describes the way the workflow needs to be invoked</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>The success count</returns>
        public Int32 StartWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 invokeSuccessCount = 0;
            Guid instanceId = Guid.Empty;
            MDMBOW.WorkflowVersion wfVersion = null;
            Collection<MDMBOW.WorkflowInstance> colGuidForMDMObject = new Collection<MDMBOW.WorkflowInstance>();
            MDMBOW.WorkflowMDMObjectCollection failedMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
            ArrayList mdmObjectsUniqueStringList = new ArrayList();

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity(new ExecutionContext(MDMTraceSource.AdvancedWorkflow));
                diagnosticActivity.Start();
            }

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                    diagnosticActivity.LogInformation(String.Format("Getting version details for workflow '{0}'..", workflowDataContext.WorkflowName));

                //TODO:: try to reduce to one DB call
                //Check for the Workflow Version Id
                //If client is not providing version Id, go and fetch from the DB
                if (workflowDataContext.WorkflowVersionId < 1)
                    workflowDataContext.WorkflowVersionId = GetLatestVersionId(workflowDataContext.WorkflowName, callerContext);

                //Get XAML definition for the latest version
                if (workflowDataContext.WorkflowVersionId > 0)
                    wfVersion = GetVersionDetails(workflowDataContext.WorkflowVersionId, false, callerContext);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                    diagnosticActivity.LogInformation("Version details get completed.");

                if (wfVersion != null)
                {
                    //Update workflow data context details..
                    workflowDataContext.WorkflowName = wfVersion.WorkflowShortName;
                    workflowDataContext.WorkflowLongName = wfVersion.WorkflowLongName;

                    //Check whether the passed MDMObjectIDs are already participating in the requested workflow
                    //If yes, restrict them and throw exception
                    //TODO:: This check may result in performance Hindrance. Come up with some other logic.
                    Int32 countOfPassedObjectsRunning = 0;
                    Int32 iterationCount = 0;
                    Int32 totalObjectsCount = workflowDataContext.MDMObjectCollection.Count;
                    Int32 workflowId = wfVersion.WorkflowId;

                    if (totalObjectsCount > 0)
                    {
                        String mdmObjectType = String.Empty;
                        StringBuilder mdmObjectIDs = new StringBuilder();

                        foreach (MDMBOW.WorkflowMDMObject mdmObj in workflowDataContext.MDMObjectCollection)
                        {
                            iterationCount++;

                            if (mdmObj.MDMObjectId > 0)
                            {
                                //Create the list of MDM objects unique string in order to avoid concurrent invocation
                                String mdmObjectUniqueString = String.Format("{0};{1};{2}", workflowId.ToString(), mdmObj.MDMObjectId.ToString(), mdmObj.MDMObjectType);
                                mdmObjectsUniqueStringList.Add(mdmObjectUniqueString);

                                mdmObjectIDs.Append(mdmObj.MDMObjectId.ToString());

                                if (iterationCount < totalObjectsCount)
                                    mdmObjectIDs.Append(',');
                                else
                                    mdmObjectType = mdmObj.MDMObjectType;
                            }
                        }

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation(String.Format("Start Workflow request is for entities '{0}' for workflow '{1}'", mdmObjectIDs, workflowDataContext.WorkflowName));
                            diagnosticActivity.LogInformation("Checking whether the requested entities are already running in the requested workflow..");
                        }

                        WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                        countOfPassedObjectsRunning = workflowInstanceBL.CheckForRunningInstances(mdmObjectIDs.ToString(), mdmObjectType, wfVersion.WorkflowId, callerContext);

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                            diagnosticActivity.LogInformation("Checking for running instance is completed.");

                        if (countOfPassedObjectsRunning > 0)
                            throw new Exception("Workflow invocation failed as some of the MDMObjects are already participating in the requested workflow.");
                        else
                        {
                            foreach (String mdmObjectUniqueString in mdmObjectsUniqueStringList)
                            {
                                //Add the MDM objects unique string to the participatingMDMObjectsTempSet which will keep track of all the MDM Objects which are starting workflow..
                                //Lock the object so that two threads cannot access the object concurrently..
                                lock (participatingMDMObjectsTempSet)
                                {
                                    //Check whether the requested MDMObject has already been participating..
                                    //If yes, throw exception
                                    if (participatingMDMObjectsTempSet.Contains(mdmObjectUniqueString))
                                        throw new Exception("Workflow invocation failed as some of the MDMObjects are already participating in the requested workflow.");
                                    else
                                        participatingMDMObjectsTempSet.Add(mdmObjectUniqueString);
                                }
                            }
                        }
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                        diagnosticActivity.LogInformation("Workflow invoke option is 'RunAsMultipleInstances'");

                    #region run multiple instances

                    //Get the Workflow.LowPerf.Threshold.Batchsize(AppConfig)
                    String strBatchSize = AppConfigurationHelper.GetAppConfig<String>("Workflow.LowPerf.Threshold.Batchsize");

                    Int32 batchSize = 0;
                    Int32.TryParse(strBatchSize, out batchSize);

                    //Check for the instances count to be invoked. If it is less than Workflow.LowPerf.Threshold.Batchsize, invoke parallely
                    if (workflowDataContext.MDMObjectCollection.Count < batchSize)
                    {
                        MDMBO.OperationResult oprResult = new MDMBO.OperationResult();

                        //Invoke instances parallely
                        Parallel.ForEach(workflowDataContext.MDMObjectCollection, mdmObj =>
                        {
                            instanceId = Guid.Empty;

                            //Create collection of single MDMObject as all activities will expect collection of objects.
                            MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
                            workflowMDMObjectCollection.Add(mdmObj);

                            //Invoke workflow
                            instanceId = InvokeWorkflow(wfVersion, workflowDataContext, workflowMDMObjectCollection, oprResult, callerContext);

                            //Check for InstanceId again
                            if (instanceId != null && instanceId != Guid.Empty)
                            {
                                //hmmm.. Instance got created
                                //Create the workflow instance object to save into tb_Workflow_Instance_Master table
                                MDMBOW.WorkflowInstance wfInstance = new MDMBOW.WorkflowInstance();
                                wfInstance.RuntimeInstanceId = instanceId.ToString();
                                wfInstance.WorkflowVersionId = wfVersion.Id;
                                wfInstance.WorkflowMDMObjects.Add(mdmObj);
                                wfInstance.WorkflowComments = workflowDataContext.WorkflowComments;
                                colGuidForMDMObject.Add(wfInstance);
                            }
                            else
                            {
                                //Sorry.. instance failed to create
                                //Add into failed MDMObject collection
                                failedMDMObjectCollection.Add(mdmObj);
                            }
                        }
                        );

                        operationResult = oprResult;
                    }
                    else
                    {
                        foreach (MDMBOW.WorkflowMDMObject mdmObj in workflowDataContext.MDMObjectCollection)
                        {
                            instanceId = Guid.Empty;

                            //Create collection of single MDMObject as all activities will expect collection of objects.
                            MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
                            workflowMDMObjectCollection.Add(mdmObj);

                            //Invoke workflow
                            instanceId = InvokeWorkflow(wfVersion, workflowDataContext, workflowMDMObjectCollection, operationResult, callerContext);

                            //Check for InstanceId again
                            if (instanceId != null && instanceId != Guid.Empty)
                            {
                                //hmmm.. Instance got created
                                //Create the workflow instance object to save into tb_Workflow_Instance_Master table
                                MDMBOW.WorkflowInstance wfInstance = new MDMBOW.WorkflowInstance();
                                wfInstance.RuntimeInstanceId = instanceId.ToString();
                                wfInstance.WorkflowVersionId = wfVersion.Id;
                                wfInstance.WorkflowMDMObjects.Add(mdmObj);
                                wfInstance.WorkflowComments = workflowDataContext.WorkflowComments;
                                colGuidForMDMObject.Add(wfInstance);
                            }
                            else
                            {
                                //Sorry.. instance failed to create
                                //Add into failed MDMObject collection
                                failedMDMObjectCollection.Add(mdmObj);
                            }
                        }
                    }

                    #endregion run multiple instances

                    #region Commenting RunAsSingleInstance option

                    // Commenting this code because we are going to support RunAsMultipleInstances always
                    // RunAsSingleInstance has been obsoleted

                    //case WorkflowInstanceRunOptions.RunAsSingleInstance:
                    //    #region run single instance

                    //    if (_currentTraceSettings.IsBasicTracingEnabled)
                    //        diagnosticActivity.LogInformation("Workflow invoke option is 'RunAsSingleInstance'");

                    //    //Invoke Workflow
                    //    instanceId = InvokeWorkflow(wfVersion, workflowDataContext, workflowDataContext.MDMObjectCollection, operationResult, callerContext);

                    //    //Check for InstanceId
                    //    if (instanceId != null && instanceId != Guid.Empty)
                    //    {
                    //        //Yes.. Instance got created
                    //        //Create the workflow instance object to save into tb_Workflow_Instance_Master table
                    //        MDMBOW.WorkflowInstance wfInstance = new MDMBOW.WorkflowInstance();
                    //        wfInstance.RuntimeInstanceId = instanceId.ToString();
                    //        wfInstance.WorkflowVersionId = wfVersion.Id;
                    //        wfInstance.WorkflowMDMObjects = workflowDataContext.MDMObjectCollection;
                    //        wfInstance.WorkflowComments = workflowDataContext.WorkflowComments;
                    //        colGuidForMDMObject.Add(wfInstance);
                    //    }
                    //    else
                    //    {
                    //        //Sorry.. instance failed to create
                    //        //Add into failed MDMObject collection
                    //        failedMDMObjectCollection = workflowDataContext.MDMObjectCollection;
                    //    }

                    //    #endregion run single instance

                    #endregion Commenting RunAsSingleInstance option

                    if (failedMDMObjectCollection != null && failedMDMObjectCollection.Count > 0)
                    {
                        //TODO::Send message to Message Inbox regarding failed MDMObjects to get invoke
                    }

                    if (colGuidForMDMObject != null && colGuidForMDMObject.Count > 0)
                    {
                        //Store instance detail in MDM tables.
                        SaveWorkflowInstances(colGuidForMDMObject, workflowInstanceRunOption, serviceType, serviceId, currentUserName, callerContext);

                        //Since the GUID collection is greater than zero at least one workflow has been invoked
                        //Set the invokeSuccessCount to the count of the GUID collection
                        invokeSuccessCount = colGuidForMDMObject.Count;
                    }

                    foreach (String mdmObjectUniqueString in mdmObjectsUniqueStringList)
                    {
                        //Remove the MDM Object details from HashSet
                        lock (participatingMDMObjectsTempSet)
                        {
                            participatingMDMObjectsTempSet.Remove(mdmObjectUniqueString);
                        }
                    }
                }
                else
                {
                    String exceptionMsg = String.Format("The requested workflow '{0}' does not exist in the system or has not been published.", workflowDataContext.WorkflowName);
                    throw new Exception(exceptionMsg);
                }
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogError(ex.Message);
                }
                else
                {
                    diagnosticActivity = new DiagnosticActivity(new ExecutionContext(MDMTraceSource.AdvancedWorkflow));
                    diagnosticActivity.Start();
                    diagnosticActivity.LogError(ex.Message);
                    diagnosticActivity.Stop();
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return invokeSuccessCount;
        }

        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context which will go as part of the instance lifetime</param>
        /// <param name="serviceType">The type of the service which is invoking the workflow</param>
        /// <param name="serviceId">The unique identification of the service</param>
        /// <param name="currentUserName">The user name who is requesting for the start</param>
        /// <param name="workflowInstanceRunOption">The run option which describes the way the workflow needs to be invoked</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>The success count</returns>
        public OperationResult StartWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, EntityOperationResultCollection eorCollection, MDMBO.CallerContext callerContext)
        {
            Int32 invokeSuccessCount = 0;
            Guid instanceId = Guid.Empty;
            MDMBOW.WorkflowVersion wfVersion = null;
            Collection<MDMBOW.WorkflowInstance> colGuidForMDMObject = new Collection<MDMBOW.WorkflowInstance>();
            OperationResult operationResult = new OperationResult();

            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntimeBL.StartWorkflow");
                diagnosticActivity.Start();
            }

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                    diagnosticActivity.LogInformation(String.Format("Getting version details for workflow '{0}'..", workflowDataContext.WorkflowName));

                //Get Workflow Version Id
                //If client is not providing version Id, go and fetch from the DB
                if (workflowDataContext.WorkflowVersionId < 1)
                    workflowDataContext.WorkflowVersionId = GetLatestVersionId(workflowDataContext.WorkflowName, callerContext);

                //Get XAML definition for the latest version
                if (workflowDataContext.WorkflowVersionId > 0)
                    wfVersion = GetVersionDetails(workflowDataContext.WorkflowVersionId, false, callerContext);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                    diagnosticActivity.LogInformation("Version details get completed.");

                if (wfVersion != null)
                {
                    //Update workflow data context details..
                    workflowDataContext.WorkflowName = wfVersion.WorkflowShortName;
                    workflowDataContext.WorkflowLongName = wfVersion.WorkflowLongName;

                    bool isRunningObjectCheckRequired = false;
                    Int32 iterationCount = 0;
                    Int32 totalObjectsCount = workflowDataContext.MDMObjectCollection.Count;
                    Int32 workflowId = wfVersion.WorkflowId;

                    if (totalObjectsCount > 0)
                    {
                        String mdmObjectType = String.Empty;
                        StringBuilder mdmObjectIDs = new StringBuilder();

                        foreach (MDMBOW.WorkflowMDMObject mdmObj in workflowDataContext.MDMObjectCollection)
                        {
                            iterationCount++;

                            //if (mdmObj.MDMObjectId > 0)
                            //{
                            //    mdmObjectIDs.Append(mdmObj.MDMObjectId.ToString());

                            //    if (iterationCount < totalObjectsCount)
                            //        mdmObjectIDs.Append(',');
                            //    else
                            //        mdmObjectType = mdmObj.MDMObjectType;
                            //}

                            if (!String.IsNullOrWhiteSpace(mdmObj.MDMObjectGUID))
                            {
                                mdmObjectIDs.Append(mdmObj.MDMObjectGUID);

                                if (iterationCount < totalObjectsCount)
                                    mdmObjectIDs.Append(',');
                                else
                                    mdmObjectType = mdmObj.MDMObjectType;
                            }
                        }

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation(String.Format("Start Workflow request is for entities '{0}' for workflow '{1}'", mdmObjectIDs, workflowDataContext.WorkflowName));
                            diagnosticActivity.LogInformation("Checking whether the requested entities are already running in the requested workflow..");
                        }
                        WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                        Collection<Int64> runningInstanceDetails = workflowInstanceBL.GetRunningInstanceDetails(mdmObjectIDs.ToString(), mdmObjectType, wfVersion.WorkflowId, callerContext);
                        isRunningObjectCheckRequired = runningInstanceDetails.Count > 0;

                        foreach (Int64 entityId in runningInstanceDetails)
                        {
                            //eorCollection.GetByEntityId(entityId).AddOperationResult("", "Workflow invocation failed as some of the MDMObjects are already participating in the requested workflow.", OperationResultType.Warning);

                            //EntityOperationResult eOR = new EntityOperationResult();
                            eorCollection.AddOperationResult("", "Workflow invocation failed as some of the MDMObjects are already participating in the requested workflow.", OperationResultType.Warning);
                        }

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                            diagnosticActivity.LogInformation("Checking for running instance is completed.");

                        #region Removing already running instances from requested input

                        if (isRunningObjectCheckRequired)
                        {
                            // Removing already running instances from input collection
                            MDMBOW.WorkflowMDMObjectCollection filteredElements = new MDMBOW.WorkflowMDMObjectCollection();
                            foreach (MDMBOW.WorkflowMDMObject obj in workflowDataContext.MDMObjectCollection)
                            {
                                if (!runningInstanceDetails.Contains(obj.MDMObjectId))
                                    filteredElements.Add(obj);
                            }

                            workflowDataContext.MDMObjectCollection = filteredElements;
                        }
                        #endregion

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                            diagnosticActivity.LogInformation("Workflow invoke option is 'RunAsMultipleInstances'");

                        #region run multiple instances

                        //Get the Workflow.LowPerf.Threshold.Batchsize(AppConfig)
                        String strBatchSize = AppConfigurationHelper.GetAppConfig<String>("Workflow.LowPerf.Threshold.Batchsize");

                        Int32 batchSize = 0;
                        Int32.TryParse(strBatchSize, out batchSize);

                        //Check for the instances count to be invoked. If it is less than Workflow.LowPerf.Threshold.Batchsize, invoke in parallel
                        if (workflowDataContext.MDMObjectCollection.Count < batchSize)
                        {
                            //Invoke instances in parallel
                            Parallel.ForEach(workflowDataContext.MDMObjectCollection, mdmObj =>
                            {
                                instanceId = Guid.Empty;

                                //Create collection of single MDMObject as all activities will expect collection of objects.
                                MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
                                workflowMDMObjectCollection.Add(mdmObj);

                                //Invoke workflow
                                instanceId = InvokeWorkflow(wfVersion, workflowDataContext, workflowMDMObjectCollection, operationResult, callerContext);

                                //Check for InstanceId again
                                if (instanceId != null && instanceId != Guid.Empty)
                                {
                                    //hmmm.. Instance got created
                                    //Create the workflow instance object to save into tb_Workflow_Instance_Master table
                                    MDMBOW.WorkflowInstance wfInstance = new MDMBOW.WorkflowInstance();
                                    wfInstance.RuntimeInstanceId = instanceId.ToString();
                                    wfInstance.WorkflowVersionId = wfVersion.Id;
                                    wfInstance.WorkflowMDMObjects.Add(mdmObj);
                                    wfInstance.WorkflowComments = workflowDataContext.WorkflowComments;
                                    colGuidForMDMObject.Add(wfInstance);
                                }
                                else
                                {
                                    eorCollection.AddEntityOperationResult(mdmObj.MDMObjectId, String.Empty, String.Format("Workflow invocation failed for the requested workflow '{0}'", workflowDataContext.WorkflowLongName), OperationResultType.Error);
                                }
                            });
                        }
                        else
                        {
                            foreach (MDMBOW.WorkflowMDMObject mdmObj in workflowDataContext.MDMObjectCollection)
                            {
                                instanceId = Guid.Empty;

                                //Create collection of single MDMObject as all activities will expect collection of objects.
                                MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
                                workflowMDMObjectCollection.Add(mdmObj);

                                //Invoke workflow
                                instanceId = InvokeWorkflow(wfVersion, workflowDataContext, workflowMDMObjectCollection, operationResult, callerContext);

                                //Check for InstanceId again
                                if (instanceId != null && instanceId != Guid.Empty)
                                {
                                    //Create the workflow instance object to save into tb_Workflow_Instance_Master table
                                    MDMBOW.WorkflowInstance wfInstance = new MDMBOW.WorkflowInstance();
                                    wfInstance.RuntimeInstanceId = instanceId.ToString();
                                    wfInstance.WorkflowVersionId = wfVersion.Id;
                                    wfInstance.WorkflowMDMObjects.Add(mdmObj);
                                    wfInstance.WorkflowComments = workflowDataContext.WorkflowComments;
                                    colGuidForMDMObject.Add(wfInstance);
                                }
                                else
                                {
                                    eorCollection.AddEntityOperationResult(mdmObj.MDMObjectId, String.Empty, String.Format("Workflow invocation failed for the requested workflow '{0}'", workflowDataContext.WorkflowLongName), OperationResultType.Error);
                                }
                            }
                        }
                        #endregion run multiple instances

                        if (colGuidForMDMObject != null && colGuidForMDMObject.Count > 0 && workflowDataContext.MDMObjectCollection.Count > 0)
                        {
                            //Store instance detail in MDM tables.
                            SaveWorkflowInstances(colGuidForMDMObject, workflowInstanceRunOption, serviceType, serviceId, currentUserName, callerContext);
                            invokeSuccessCount = workflowDataContext.MDMObjectCollection.Count;
                        }

                        foreach (MDMBOW.WorkflowMDMObject mdmObj in workflowDataContext.MDMObjectCollection)
                        {
                            eorCollection.AddEntityOperationResult(mdmObj.MDMObjectId, String.Empty, String.Format("Workflow '{0}' invocation is successful.", workflowDataContext.WorkflowLongName), OperationResultType.Information);
                        }

                        operationResult.ReturnValues.Add(invokeSuccessCount);
                    }
                }
                else
                {
                    String exceptionMsg = String.Format("The requested workflow '{0}' does not exist in the system or has not been published.", workflowDataContext.WorkflowName);
                    operationResult.AddOperationResult("", exceptionMsg, OperationResultType.Error);
                }
            }
            catch (Exception ex)
            {
                operationResult.AddOperationResult("", ex.Message, OperationResultType.Error);

                //Log exception
                this.LogException(ex);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogError(ex.Message);
                }
                else
                {
                    diagnosticActivity = new DiagnosticActivity(new ExecutionContext(MDMTraceSource.AdvancedWorkflow));
                    diagnosticActivity.Start();
                    diagnosticActivity.LogError(ex.Message);
                    diagnosticActivity.Stop();
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            eorCollection.RefreshOperationResultStatus();

            return operationResult;
        }

        #endregion

        #region Resume Workflow

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="commaSeparatedRuntimeInstanceIds">Instance Ids which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(String commaSeparatedRuntimeInstanceIds, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;

            Collection<MDMBOW.WorkflowInstance> instanceCollection = null;

            try
            {
                WorkflowInstanceBL instanceBL = new WorkflowInstanceBL();
                instanceCollection = instanceBL.GetByRuntimeInstanceIds(commaSeparatedRuntimeInstanceIds, callerContext);

                resumeSuccessCount = ResumeWorkflow(instanceCollection, actionContext, ref operationResult, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="workflowInstance">The Instance object which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowInstance workflowInstance, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;

            try
            {
                // create instance collection with only 1 item within it
                Collection<MDMBOW.WorkflowInstance> instanceCollection = new Collection<MDMBOW.WorkflowInstance>();
                instanceCollection.Add(workflowInstance);

                resumeSuccessCount = ResumeWorkflow(instanceCollection, actionContext, ref operationResult, callerContext);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(Collection<MDMBOW.WorkflowInstance> instanceCollection, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;

            try
            {
                if (instanceCollection != null && instanceCollection.Count > 0)
                {
                    foreach (MDMBOW.WorkflowInstance instance in instanceCollection)
                    {
                        Boolean result = ResumeBookmark(instance.WorkflowVersionId, actionContext, instance.RuntimeInstanceId, instance.WorkflowMDMObjects, operationResult, callerContext);

                        if (result)
                            resumeSuccessCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="mdmObj">The MDM Object for which workflow needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowMDMObject mdmObj, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;
            Collection<MDMBOW.WorkflowInstance> instanceCollection = null;

            try
            {
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                instanceCollection = workflowInstanceBL.GetByMDMObject(mdmObj, actionContext.CurrentActivityName, callerContext);

                if (instanceCollection != null && instanceCollection.Count > 0)
                {
                    foreach (MDMBOW.WorkflowInstance instance in instanceCollection)
                    {
                        if (instance.IsReadyForAction)
                        {
                            Boolean result = ResumeBookmark(instance.WorkflowVersionId, actionContext, instance.RuntimeInstanceId, instance.WorkflowMDMObjects, operationResult, callerContext);

                            if (result)
                                resumeSuccessCount++;
                        }
                        else
                            throw new Exception("The requested workflow activity has already been processed or closed for the requested entity.");
                    }
                }
                else
                    throw new Exception("The requested entity is not being participated actively in the requested workflow.");
            }
            catch (Exception ex)
            {
                operationResult.AddOperationResult("", ex.Message, OperationResultType.Error);

                //Log exception
                this.LogException(ex);
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context holding MDM Objects for which workflow needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 resumeSuccessCount = 0;

            try
            {
                var failedMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();

                //Get the Workflow.LowPerf.Threshold.Batchsize(AppConfig)
                var batchSize = AppConfigurationHelper.GetAppConfig("Workflow.LowPerf.Threshold.Batchsize", 0);

                //Check for the instances count to be invoked. If it is less than Workflow.LowPerf.Threshold.Batchsize, invoke parallel
                if (workflowDataContext.MDMObjectCollection.Count < batchSize)
                {
                    if (actionContext != null)
                    {
                        var oprResult = new OperationResult();

                        var threadSize = AppConfigurationHelper.GetAppConfig("Workflow.LowPerf.Threshold.ThreadSize", 2);

                        var workflowMdmObjects = new Collection<MDMBOW.WorkflowMDMObject>();

                        foreach (var workflowMdmObject in workflowDataContext.MDMObjectCollection)
                        {
                            workflowMdmObjects.Add(workflowMdmObject);
                        }

                        var parallelTaskProcessor = new ParallelTaskProcessor();
                        parallelTaskProcessor.RunInParallel(workflowMdmObjects, mdmObj =>
                        {
                            Collection<MDMBOW.WorkflowInstance> instanceCollection = null;

                            var workflowInstanceManager = new WorkflowInstanceBL();
                            instanceCollection = workflowInstanceManager.GetByMDMObject(mdmObj, actionContext.CurrentActivityName, callerContext);

                            if (instanceCollection != null && instanceCollection.Count > 0)
                            {
                                foreach (MDMBOW.WorkflowInstance instance in instanceCollection)
                                {
                                    if (instance.IsReadyForAction)
                                    {
                                        Boolean result = ResumeBookmark(instance.WorkflowVersionId, actionContext, instance.RuntimeInstanceId, instance.WorkflowMDMObjects, oprResult, callerContext);

                                        if (result)
                                            resumeSuccessCount++;
                                        else
                                            failedMDMObjectCollection.Add(mdmObj);
                                    }
                                    else
                                        failedMDMObjectCollection.Add(mdmObj);
                                }
                            }
                            else
                                failedMDMObjectCollection.Add(mdmObj);

                        }, null, threadSize);

                        operationResult = oprResult;
                    }
                }
                else
                {
                    if (actionContext != null)
                    {
                        foreach (MDMBOW.WorkflowMDMObject mdmObj in workflowDataContext.MDMObjectCollection)
                        {
                            Collection<MDMBOW.WorkflowInstance> instanceCollection = null;

                            WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                            instanceCollection = workflowInstanceBL.GetByMDMObject(mdmObj, actionContext.CurrentActivityName, callerContext);

                            if (instanceCollection != null && instanceCollection.Count > 0)
                            {
                                foreach (MDMBOW.WorkflowInstance instance in instanceCollection)
                                {
                                    if (instance.IsReadyForAction)
                                    {
                                        Boolean result = ResumeBookmark(instance.WorkflowVersionId, actionContext, instance.RuntimeInstanceId, instance.WorkflowMDMObjects, operationResult, callerContext);

                                        if (result)
                                            resumeSuccessCount++;
                                        else
                                            failedMDMObjectCollection.Add(mdmObj);
                                    }
                                    else
                                        failedMDMObjectCollection.Add(mdmObj);
                                }
                            }
                            else
                                failedMDMObjectCollection.Add(mdmObj);
                        }
                    }
                }

                if (failedMDMObjectCollection != null && failedMDMObjectCollection.Count > 0)
                {
                    //TODO::Send message to Message Inbox regarding failed MDMObjects to get resume

                    if (!operationResult.HasError)
                    {
                        String message = String.Empty;

                        if (workflowDataContext.MDMObjectCollection.Count == 1)
                            message = "The requested workflow activity had already been processed or closed for the requested entity.";
                        else if (workflowDataContext.MDMObjectCollection.Count == failedMDMObjectCollection.Count)
                            message = "The requested workflow activity had already been processed or closed for all the selected entities.";
                        else
                            message = "Workflow Action is successful for some entities. The requested workflow activity had already been processed or closed for remaining entities. Please see event log for further details.";

                        MDMBO.Information info = new MDMBO.Information();
                        info.InformationMessage = message;
                        operationResult.Informations.Add(info);

                        message += " Already processed entity Ids:";
                        foreach (MDMBOW.WorkflowMDMObject mdmObject in failedMDMObjectCollection)
                        {
                            message = String.Format("{0} {1};", message, mdmObject.MDMObjectId.ToString());
                        }

                        //Log exception
                        this.LogException(new Exception(message));
                    }
                }
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return resumeSuccessCount;
        }

        /// <summary>
        /// Resumes Aborted Instances
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be resumed</param>
        /// <param name="instanceStatus">The status to which instances needs to be updated</param>
        /// <param name="loginUser">The user who is resuming the workflow</param>
        /// <param name="programName">The name of the program requesting for resume</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 ResumeAbortedWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, String instanceStatus, String loginUser, String programName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;
            Int32 resumeSuccessCount = 0;

            if (instanceCollection != null && instanceCollection.Count > 0)
            {
                foreach (MDMBOW.WorkflowInstance instance in instanceCollection)
                {
                    try
                    {
                        try
                        {
                            //Case 1: When workflow instance has been aborted after any of the Human Activity execution
                            //There will be already an entry in the tb_Workflow_Tracking_RunningInstance for that human activity
                            //Just update the status of that activity so that it will be available for the action for the user
                            WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                            result = workflowInstanceBL.UpdateWorkflowInstances(instance.RuntimeInstanceId, "", "", 0, "", instanceStatus, loginUser, callerContext);

                            if (result)
                                resumeSuccessCount++;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Class == 17)
                            {
                                //Case 2: When workflow instance has been aborted before any of the Human Activity execution
                                //There will not be any entry in the tb_Workflow_Tracking_RunningInstance and so the Case 1 throws exception
                                //This happens when there are some code activities prior to the human activities

                                //In this case procedure raises an error with severity as 10..
                                //So, checking for severity 17 and rerunning the Workflow..
                                result = ResumeAbortedInstance(instance.WorkflowVersionId, instance.RuntimeInstanceId, operationResult, callerContext);

                                if (result)
                                    resumeSuccessCount++;
                            }
                            else
                                throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        MDMBO.Error error = new MDMBO.Error();
                        error.ErrorMessage = ex.Message;
                        operationResult.Errors.Add(error);

                        //Log exception
                        this.LogException(ex);
                    }
                }
            }

            return resumeSuccessCount;
        }

        #endregion

        #region Terminate Workflow

        /// <summary>
        /// Terminates requested workflow instances
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be terminated</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 TerminateWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 terminateSuccessCount = 0;

            try
            {
                if (instanceCollection != null && instanceCollection.Count > 0)
                {
                    foreach (MDMBOW.WorkflowInstance instance in instanceCollection)
                    {
                        Boolean result = TerminateInstance(instance.WorkflowVersionId, instance.RuntimeInstanceId, operationResult, callerContext);

                        if (result)
                            terminateSuccessCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return terminateSuccessCount;
        }

        #endregion

        #region Promote Workflow

        /// <summary>
        /// Promotes requested workflow instances
        /// </summary>
        /// <param name="instanceCollection">The collection of Instance objects which needs to be promoted</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        public Int32 PromoteWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Int32 promoteSuccessCount = 0;

            try
            {
                if (instanceCollection != null && instanceCollection.Count > 0)
                {
                    foreach (MDMBOW.WorkflowInstance instance in instanceCollection)
                    {
                        Boolean result = PromoteInstance(instance.WorkflowVersionId, instance.RuntimeInstanceId, operationResult, callerContext);

                        if (result)
                            promoteSuccessCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return promoteSuccessCount;
        }

        #endregion

        #endregion Methods

        #region Private methods

        /// <summary>
        /// Gets the Latest Version Id of the requested workflow
        /// </summary>
        /// <param name="workflowName">Workflow Name for which latest version is required</param>
        /// <returns>Workflow Latest Version Id</returns>
        private Int32 GetLatestVersionId(String workflowName, MDMBO.CallerContext context)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntimeBL.GetLatestVersionId");
                diagnosticActivity.Start();
            }

            Int32 latestVersionId = 0;

            try
            {
                MDMBOW.Workflow workflow = new MDMBOW.Workflow();
                WorkflowBL workflowBL = new WorkflowBL();

                workflow = workflowBL.GetWorkflowByName(workflowName, context);

                if (workflow != null)
                    latestVersionId = workflow.LatestVersion;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

            return latestVersionId;
        }

        private MDMBOW.WorkflowVersion GetVersionDetails(Int32 versionId, Boolean makeRoundTrip, MDMBO.CallerContext callerContext)
        {

            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntimeBL.GetVersionDetails");
                diagnosticActivity.Start();
            }

            MDMBOW.WorkflowVersion wfVersion = null;

            try
            {

                WorkflowRuntime wfRuntime = new WorkflowRuntime();
                wfVersion = wfRuntime.GetVersionDetails(versionId, makeRoundTrip, callerContext);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

            return wfVersion;
        }

        private Guid InvokeWorkflow(MDMBOW.WorkflowVersion workflowVersion, MDMBOW.WorkflowDataContext datacontext, MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntimeBL.InvokeWorkflow");
                diagnosticActivity.Start();
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogInformation("Workflow invocation starts..");

            }

            Guid instanceId = Guid.Empty;

            try
            {
                // Create input parameter dictionary collection
                Dictionary<String, Object> inputParameters = new Dictionary<string, object>();

                MDMBOW.WorkflowDataContext dataContextForInputParameter = new MDMBOW.WorkflowDataContext();

                if (datacontext != null)
                {
                    dataContextForInputParameter.Id = datacontext.Id;
                    dataContextForInputParameter.WorkflowName = datacontext.WorkflowName;
                    dataContextForInputParameter.WorkflowLongName = datacontext.WorkflowLongName;
                    dataContextForInputParameter.WorkflowVersionId = datacontext.WorkflowVersionId;
                    dataContextForInputParameter.MDMObjectCollection = workflowMDMObjectCollection;
                    dataContextForInputParameter.Module = datacontext.Module;
                    dataContextForInputParameter.Application = datacontext.Application;
                    dataContextForInputParameter.ExtendedProperties = datacontext.ExtendedProperties;
                    dataContextForInputParameter.WorkflowComments = datacontext.WorkflowComments;

                    inputParameters.Add("MDMDataContext", dataContextForInputParameter);

                    // Start new workflow instance
                    WorkflowRuntime wfRuntime = new WorkflowRuntime();

                    instanceId = wfRuntime.StartWorkflow(workflowVersion, inputParameters, operationResult, callerContext);

                    //Check for InstanceId
                    if (instanceId == null || instanceId == Guid.Empty)
                    {
                        //No.. Instance failed to create.
                        //Try once more
                        instanceId = wfRuntime.StartWorkflow(workflowVersion, inputParameters, operationResult, callerContext);
                    }
                }

            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Workflow invocation completed.");

                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

            return instanceId;
        }

        private Boolean ResumeBookmark(Int32 instanceVersionId, MDMBOW.WorkflowActionContext actionContext, String runtimeInstanceId, MDMBOW.WorkflowMDMObjectCollection workflowMDMObjects, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;
            MDMBOW.WorkflowVersion wfVersion = null;

            //Get xaml definition for the latest version
            wfVersion = GetVersionDetails(instanceVersionId, false, callerContext);

            if (wfVersion != null)
            {
                // Resume workflow
                WorkflowRuntime wfRuntime = new WorkflowRuntime();
                result = wfRuntime.ResumeBookmark(wfVersion, actionContext.CurrentActivityName, runtimeInstanceId, actionContext, workflowMDMObjects, operationResult, callerContext);
            }
            else
            {
                String exceptionMsg = String.Format("No version found for the instance : {0}", runtimeInstanceId);
                throw new Exception(exceptionMsg);
            }

            return result;
        }

        private Boolean TerminateInstance(Int32 instanceVersionId, String runtimeInstanceId, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;
            MDMBOW.WorkflowVersion wfVersion = null;

            //Get xaml definition for the latest version
            wfVersion = GetVersionDetails(instanceVersionId, false, callerContext);

            if (wfVersion != null)
            {
                // Terminate Instance
                WorkflowRuntime wfRuntime = new WorkflowRuntime();
                result = wfRuntime.TerminateInstance(wfVersion, runtimeInstanceId, operationResult, callerContext);
            }
            else
            {
                String exceptionMsg = String.Format("No version found for the instance : {0}", runtimeInstanceId);
                throw new Exception(exceptionMsg);
            }

            return result;
        }

        private Boolean PromoteInstance(Int32 instanceVersionId, String runtimeInstanceId, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;
            MDMBOW.WorkflowVersion wfVersion = null;

            //Get xaml definition for the latest version
            wfVersion = GetVersionDetails(instanceVersionId, false, callerContext);

            if (wfVersion != null)
            {
                // Promote Instance
                WorkflowRuntime wfRuntime = new WorkflowRuntime();
                result = wfRuntime.PromoteInstance(wfVersion, runtimeInstanceId, operationResult, callerContext);
            }
            else
            {
                String exceptionMsg = String.Format("No version found for the instance : {0}", runtimeInstanceId);
                throw new Exception(exceptionMsg);
            }

            return result;
        }

        private Boolean ResumeAbortedInstance(Int32 instanceVersionId, String runtimeInstanceId, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;
            MDMBOW.WorkflowVersion wfVersion = null;

            //Get xaml definition for the latest version
            wfVersion = GetVersionDetails(instanceVersionId, false, callerContext);

            if (wfVersion != null)
            {
                // Promote Instance
                WorkflowRuntime wfRuntime = new WorkflowRuntime();
                result = wfRuntime.ResumeAbortedInstance(wfVersion, runtimeInstanceId, operationResult, callerContext);
            }
            else
            {
                String exceptionMsg = String.Format("No version found for the instance : {0}", runtimeInstanceId);
                throw new Exception(exceptionMsg);
            }

            return result;
        }

        private void SaveWorkflowInstances(Collection<MDMBOW.WorkflowInstance> colGuidForMDMObject, WorkflowInstanceRunOptions workflowInstanceRunOption, String serviceType, Int32 serviceId, String currentUserName, MDMBO.CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntimeBL.SaveWorkflowInstances");
                diagnosticActivity.Start();
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogInformation("Saving succeeded workflows into MDM data store..");
            }

            Int32 processingType = Convert.ToInt32(workflowInstanceRunOption);

            try
            {
                WorkflowInstanceBL instanceBL = new WorkflowInstanceBL();
                instanceBL.Process(colGuidForMDMObject, processingType, serviceId, serviceType, currentUserName, callerContext);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Saving succeeded workflows into MDM data store completed.");
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }
        }

        private void StartWorkflowMonitoringService(MDMBO.CallerContext callerContext)
        {
            //Check whether Workflow Monitoring service is running
            if (!WorkflowMonitoringService.IsRunning)
            {
                //No.. Service is not running
                //Start the service
                try
                {
                    WorkflowMonitoringService.Start(callerContext);
                }
                catch (Exception ex)
                {
                    //Log exception
                    this.LogException(ex);
                }
            }
        }

        private MDMBO.SecurityPrincipal LoadWorkflowEngineSecurityPrincipal()
        {
            MDMBO.SecurityPrincipal currentUserSecurityPrincipal = null;

            try
            {
                //Workflow internal would be using username as "system"
                String userName = "cfadmin";

                //Get the message security identity key
                String messageSecurityIdentityKey = AppConfigurationHelper.GetAppConfig<String>("MDM.WCFServices.MessageSecurity.IdentityKey");

                //Cache the message security identity key which will be used as authentication ticket for this security principal
                ICache cacheManager = CacheFactory.GetCache();

                //Set the form auth ticket into cache for further operations..
                cacheManager.Set(MDMBO.CacheKeyGenerator.GetFormAuthenticationTicketCacheKey(userName), messageSecurityIdentityKey, DateTime.Now.AddDays(1));

                //Get security principal..
                //If security principal is not available in the cache, the Get method creates and caches the security principal with the authentication ticket declared above
                SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
                currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, AuthenticationType.Forms, MDMCenterSystem.WcfService, "WithoutTimeStamp");
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.ToString());
            }

            return currentUserSecurityPrincipal;
        }

        /// <summary>
        /// Logs the exception into Event Log
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        private void LogException(Exception ex)
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
    }
}
