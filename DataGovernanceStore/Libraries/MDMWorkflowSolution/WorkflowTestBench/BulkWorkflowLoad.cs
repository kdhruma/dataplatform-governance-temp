using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MDMWS = MDM.Services;
using MDMBO = MDM.BusinessObjects;
using MDMBOW = MDM.BusinessObjects.Workflow;
using MDM.BusinessObjects;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MDM.WorkflowRuntimeEngine;
using MDM.CacheManager.Business;
using MDM.Workflow.Designer.Business;
using MDM.BusinessObjects.Workflow;
using System.Reflection;
using MDM.Core;
using System.Collections.ObjectModel;
using MDM.Workflow.PersistenceManager.Business;
using System.Activities;
using MDM.Workflow.Utility;

using System.Runtime.DurableInstancing;
using MDM.Utility;
using System.Diagnostics;

namespace MDM.Workflow.TestBench
{
    public class BulkWorkflowLoad
    {
        private int InitiateWFSleepTimeIntervalInMillSeconds = 1000;

        private int ResumeWFSleepTimeIntervalInMillSeconds = 500;

        private string workflowName = string.Empty;

        MDMBOW.WorkflowVersion wfVersion = null;

        public BulkWorkflowLoad()
        {
        }

        public void Run()
        {
            String invokeAgain = "no";
            do
            {
                Console.WriteLine("Starting workflow invocation job...");
                if ( Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting workflow invocation job...", MDMTraceSource.AdvancedWorkflow);

                workflowName = System.Configuration.ConfigurationManager.AppSettings.Get("WorkflowName");
                String strInitiateWFSleepTimeIntervalInMillSeconds = System.Configuration.ConfigurationManager.AppSettings.Get("InitiateWFSleepTimeIntervalInMillSeconds");
                String strResumeWFSleepTimeIntervalInMillSeconds = System.Configuration.ConfigurationManager.AppSettings.Get("ResumeWFSleepTimeIntervalInMillSeconds");

                Int32.TryParse(strInitiateWFSleepTimeIntervalInMillSeconds, out InitiateWFSleepTimeIntervalInMillSeconds);
                Int32.TryParse(strResumeWFSleepTimeIntervalInMillSeconds, out ResumeWFSleepTimeIntervalInMillSeconds);
                
                Console.WriteLine("Enter the workflow job id:");
                String strJobId = Console.ReadLine();

                Console.WriteLine("Workflow Job Id:" + strJobId);
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Workflow Job Id:" + strJobId, MDMTraceSource.AdvancedWorkflow);

               // return;

                StartExecution(new CallerContext( MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow));

                Console.WriteLine("Do you want to re-run workflow invocation job? (yes/no)");
                invokeAgain = Console.ReadLine();
                
            } while (invokeAgain.ToLower() == "yes");
        }

        private void StartExecution(CallerContext callerContext)
        {
            try
            {
                Console.WriteLine("Enter the input table name:");
                String tableName = Console.ReadLine();

                Console.WriteLine("Enter the Start Row Number:");
                String strFromRow = Console.ReadLine();

                Console.WriteLine("Enter the End Row Number:");
                String strToRow = Console.ReadLine();

                int fromRow = 0;
                int toRow = 0;

                Int32.TryParse(strFromRow, out fromRow);
                Int32.TryParse(strToRow, out toRow);

                String message = String.Format("Loading workflow input table. TableName: {0}, Start Row: {1}, End Row: {2}", tableName, strFromRow, strToRow);
                Console.WriteLine(message);
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, message, MDMTraceSource.AdvancedWorkflow);

                DataTable dtInputRecords = GetWorkflowData(tableName, fromRow, toRow);

                message = "Workflow input table loaded successfully..";
                Console.WriteLine(message);
                if ( Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, message, MDMTraceSource.AdvancedWorkflow);

                Int32 latestVersionId = GetLatestVersionId(workflowName, callerContext);

                wfVersion = GetVersionDetails(latestVersionId, true, callerContext);

                if (dtInputRecords != null)
                {
                    message = "Total items to be processed:" + dtInputRecords.Rows.Count;
                    Console.WriteLine(message);
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, message, MDMTraceSource.AdvancedWorkflow);

                    #region Initiation & Action Playback

                    DateTime beforeInvoking = DateTime.Now;
                    message = "Workflow initiation and action playback started at - " + beforeInvoking;
                    Console.WriteLine(message);
                    if ( Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, message, MDMTraceSource.AdvancedWorkflow);

                    Boolean performActivityMove = false;
                    String strPerformActivityMove = System.Configuration.ConfigurationManager.AppSettings.Get("PerformActivityMove");

                    Boolean.TryParse(strPerformActivityMove, out performActivityMove);

                    Dictionary<Int64, string> generatednstanceIdColl = new Dictionary<Int64, string>();

                    foreach (DataRow dr in dtInputRecords.Rows)
                    {
                        String strInitiateWFSleepTimeIntervalInMillSeconds = System.Configuration.ConfigurationManager.AppSettings.Get("InitiateWFSleepTimeIntervalInMillSeconds");
                        String strResumeWFSleepTimeIntervalInMillSeconds = System.Configuration.ConfigurationManager.AppSettings.Get("ResumeWFSleepTimeIntervalInMillSeconds");

                        Int32.TryParse(strInitiateWFSleepTimeIntervalInMillSeconds, out InitiateWFSleepTimeIntervalInMillSeconds);
                        Int32.TryParse(strResumeWFSleepTimeIntervalInMillSeconds, out ResumeWFSleepTimeIntervalInMillSeconds);

                        string strCNodeId = dr["PK_CNode"].ToString();
                        Int64 cnodeId = 0;
                        Int64.TryParse(strCNodeId, out cnodeId);

                        MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
                        MDMBOW.WorkflowMDMObject mdmObject = new MDMBOW.WorkflowMDMObject();
                        mdmObject.MDMObjectId = cnodeId;
                        mdmObject.MDMObjectType = "MDM.BusinessObjects.Entity";
                        workflowMDMObjectCollection.Add(mdmObject);

                        Guid instanceId = Guid.Empty;
                        WorkflowApplication wfapp = InitiateWorkflow(workflowMDMObjectCollection, cnodeId, ref instanceId, callerContext);
                        String runtimeInstanceId = instanceId.ToString();

                        generatednstanceIdColl.Add(cnodeId, runtimeInstanceId);

                        Thread.Sleep(InitiateWFSleepTimeIntervalInMillSeconds);
                        
                        if (performActivityMove)
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                string activityNameColumn = "HumanActivity" + i + "StageName";
                                string activityActionColumn = "HumanActivity" + i + "ActionName";

                                String activityName = dr[activityNameColumn] != null ? dr[activityNameColumn].ToString() : string.Empty;
                                String activityAction = dr[activityActionColumn] != null ? dr[activityActionColumn].ToString() : string.Empty;

                                if (!string.IsNullOrEmpty(activityName) && !string.IsNullOrEmpty(activityAction))
                                    ResumeWorkflow(wfapp, runtimeInstanceId, workflowMDMObjectCollection, cnodeId, activityName, activityAction, callerContext);

                                Thread.Sleep(ResumeWFSleepTimeIntervalInMillSeconds);
                            }
                        }
                    }

                    //for (int i = 1; i <= 5; i++)
                    //{
                    //    String strStageWiseResumeTimeIntervalInMillSeconds = ConfigurationManager.AppSettings.Get("StageWiseResumeTimeIntervalInMillSeconds");
                    //    int StageWiseResumeTimeIntervalInMillSeconds = 20000;
                    //    Int32.TryParse(strStageWiseResumeTimeIntervalInMillSeconds, out StageWiseResumeTimeIntervalInMillSeconds);
                    //    Thread.Sleep(StageWiseResumeTimeIntervalInMillSeconds);
                    //    ResumeWorkflowForGivenStageSeqNo(dtInputRecords, generatednstanceIdColl, i,callerContext);
                    //}

                    DateTime afterInvoking = DateTime.Now;
                    message = "Workflow initiation and action playback completed at - " + afterInvoking;
                    Console.WriteLine(message);
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, message, MDMTraceSource.AdvancedWorkflow);

                    message = "Total time taken: " + afterInvoking.Subtract(beforeInvoking).ToString();
                    Console.WriteLine(message);
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, message, MDMTraceSource.AdvancedWorkflow);

                    #endregion
                }
                else
                {
                    message = "No records found in input table to process....";
                    Console.WriteLine(message);
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, message, MDMTraceSource.AdvancedWorkflow);
                }
            }
            catch (Exception ex)
            {
                string message = "Unhandled exception occurred. Exception:" + ex.ToString();
                Console.WriteLine(message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.AdvancedWorkflow);
            }
        }

        private void ResumeWorkflowForGivenStageSeqNo(DataTable dtInputRecords, Dictionary<Int64, string>  generatednstanceIdColl, int StageNo, CallerContext callerContext)
        {

            // activity 1
            foreach (DataRow dr in dtInputRecords.Rows)
            {
                String strInitiateWFSleepTimeIntervalInMillSeconds = System.Configuration.ConfigurationManager.AppSettings.Get("InitiateWFSleepTimeIntervalInMillSeconds");
                String strResumeWFSleepTimeIntervalInMillSeconds = System.Configuration.ConfigurationManager.AppSettings.Get("ResumeWFSleepTimeIntervalInMillSeconds");

                Int32.TryParse(strInitiateWFSleepTimeIntervalInMillSeconds, out InitiateWFSleepTimeIntervalInMillSeconds);
                Int32.TryParse(strResumeWFSleepTimeIntervalInMillSeconds, out ResumeWFSleepTimeIntervalInMillSeconds);

                string strCNodeId = dr["PK_CNode"].ToString();
                Int64 cnodeId = 0;
                Int64.TryParse(strCNodeId, out cnodeId);

                MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
                MDMBOW.WorkflowMDMObject mdmObject = new MDMBOW.WorkflowMDMObject();
                mdmObject.MDMObjectId = cnodeId;
                mdmObject.MDMObjectType = "MDM.BusinessObjects.Entity";
                workflowMDMObjectCollection.Add(mdmObject);

                //Guid instanceId = Guid.Empty;
                //WorkflowApplication wfapp = InitiateWorkflow(workflowMDMObjectCollection, cnodeId, ref instanceId);
                //String runtimeInstanceId = instanceId.ToString();
                //Thread.Sleep(InitiateWFSleepTimeIntervalInMillSeconds);

                WorkflowApplication wfapp = null;
                String runtimeInstanceId = generatednstanceIdColl[cnodeId];

                string activityNameColumn = "HumanActivity" + StageNo + "StageName";
                string activityActionColumn = "HumanActivity" + StageNo + "ActionName";

                String activityName = dr[activityNameColumn] != null ? dr[activityNameColumn].ToString() : string.Empty;
                String activityAction = dr[activityActionColumn] != null ? dr[activityActionColumn].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(activityName) && !string.IsNullOrEmpty(activityAction))
                    ResumeWorkflow(wfapp, runtimeInstanceId, workflowMDMObjectCollection, cnodeId, activityName, activityAction, callerContext);

                Thread.Sleep(ResumeWFSleepTimeIntervalInMillSeconds);
            }
        }

        private DataTable GetWorkflowData(string tableName, int fromRow, int toRow)
        {
            DataTable result = null;

            try
            {
                string connectionString = MDM.Utility.AppConfigurationHelper.ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                String query = String.Format("SELECT * FROM {0} WHERE RowNum >= {1} AND RowNum <= {2}", tableName, fromRow, toRow);

                // Assumes that customerConnection is a valid SqlConnection object.
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                DataSet dsInputRecords = new DataSet();

                dataAdapter.Fill(dsInputRecords, "WorkflowInputDetails");

                if (dsInputRecords != null && dsInputRecords.Tables.Count > 0)
                    result = dsInputRecords.Tables[0];
            }
            catch (Exception ex)
            {
                String message = "Exception on GetWorkflowData: " + ex.ToString();
                Console.WriteLine(message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.AdvancedWorkflow);
            }

            return result;
        }

        private Int32 GetLatestVersionId(String workflowName, CallerContext callerContext)
        {
            Int32 latestVersionId = 0;

            MDMBOW.Workflow workflow = new MDMBOW.Workflow();
            WorkflowBL workflowBL = new WorkflowBL();

            workflow = workflowBL.GetWorkflowByName(workflowName, callerContext);

            if (workflow != null)
                latestVersionId = workflow.LatestVersion;

            return latestVersionId;
        }

        private MDMBOW.WorkflowVersion GetVersionDetails(Int32 versionId, Boolean makeRoundTrip,CallerContext callerContext)
        {
            ICache cache = null;
            MDMBOW.WorkflowVersion wfVersion = null;

            // Create the cache key name for version object caching
            String cacheKeyName = String.Format("RS_WorkflowVersionId_{0}", versionId);

            try
            {
                cache = CacheFactory.GetCache();
            }
            catch
            {
                //Ignore exception..
            }

            // If cache is null then always make DB call to get the version detail.
            // Otherwise get the cached version
            if (cache != null || makeRoundTrip == true)
            {
                //Get the Workflow version from cache
                object cachedVersion = cache[cacheKeyName];

                // If workflow version returned from cache IS NOT NULL then cast it into WorkflowVersion object
                if (cachedVersion != null)
                {
                    wfVersion = (MDMBOW.WorkflowVersion)cachedVersion;
                }
                else
                {
                    // If workflow version returned from cache IS NULL then take version detail from DB and put it into cache
                    // cache is set to expire after 2 hr.
                    wfVersion = GetWorkflowVerisonDetailsFromDB(versionId, callerContext);

                    if (wfVersion != null)
                    {
                        //Load the tracking profile object
                        wfVersion.TrackingProfileObject = TrackingProfileLoader.LoadFromInputXML(wfVersion.TrackingProfile);

                        cache.Set(cacheKeyName, wfVersion, DateTime.Now.AddHours(24.0));
                    }
                }
            }
            else
            {
                //If cache is null then always make DB call
                wfVersion = GetWorkflowVerisonDetailsFromDB(versionId, callerContext);

                //Load the tracking profile object
                wfVersion.TrackingProfileObject = TrackingProfileLoader.LoadFromInputXML(wfVersion.TrackingProfile);
            }

            return wfVersion;
        }

        private MDMBOW.WorkflowVersion GetWorkflowVerisonDetailsFromDB(Int32 versionId, CallerContext callerContext)
        {
            MDMBOW.WorkflowVersion wfVersion = null;
            WorkflowVersionBL wfVersionBL = new WorkflowVersionBL();

            //Get the version object through Id
            wfVersion = wfVersionBL.GetById(versionId, callerContext);

            return wfVersion;
        }

        private WorkflowApplication InitiateWorkflow(WorkflowMDMObjectCollection workflowMDMObjectCollection, Int64 cnodeId, ref Guid instanceId, CallerContext callerContext)
        {
            WorkflowApplication wfapp = null;
            Collection<MDMBOW.WorkflowInstance> colGuidForMDMObject = new Collection<MDMBOW.WorkflowInstance>();

            MDMBO.OperationResult operationResult = new MDMBO.OperationResult();

            MDMBOW.Workflow workflow = new MDMBOW.Workflow();
            workflow.LongName = workflowName;

            //MDMWS.WorkflowService wfService = new MDMWS.WorkflowService();
            //wfService.StartWorkflow(workflow, workflowMDMObjectCollection, "TestBench", 1, "system", MDM.Core.WorkflowInstanceRunOptions.RunAsMultipleInstances, ref operationResult);

            wfapp = InvokeWorkflow(wfVersion, workflowMDMObjectCollection, operationResult, ref instanceId, callerContext);

            //Check for InstanceId
            if (instanceId != null && instanceId != Guid.Empty)
            {
                //Yes.. Instance got created
                //Create the workflow instance object to save into tb_Workflow_Instance_Master table
                MDMBOW.WorkflowInstance wfInstance = new MDMBOW.WorkflowInstance();
                wfInstance.RuntimeInstanceId = instanceId.ToString();
                wfInstance.WorkflowVersionId = wfVersion.Id;
                wfInstance.WorkflowMDMObjects = workflowMDMObjectCollection;
                colGuidForMDMObject.Add(wfInstance);

                //Store instance detail in MDM tables.
                SaveWorkflowInstances(colGuidForMDMObject, WorkflowInstanceRunOptions.RunAsMultipleInstances, "system", 1, "cfadmin", callerContext);
            }

            if (operationResult.HasError)
            {
                String msg = String.Format("Error occurred while invoking workflow for item:{0} \n: Error: ", cnodeId);

                String errorString = msg;
                foreach (Error error in operationResult.Errors)
                {
                    errorString += error.ErrorMessage;
                }

                Console.WriteLine(errorString);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorString, MDMTraceSource.AdvancedWorkflow);
            }
            else
            {
                String msg = String.Format("Workflow invoked successfully for an item:{0}.", cnodeId);
                Console.WriteLine(msg);
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, msg, MDMTraceSource.AdvancedWorkflow);
            }

            return wfapp;
        }

        private WorkflowApplication InvokeWorkflow(MDMBOW.WorkflowVersion workflowVersion, MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection, MDMBO.OperationResult operationResult, ref Guid instanceId, CallerContext callerContext)
        {

            WorkflowApplication wfapp = null;

            // Prepare workflow data context
            MDMBOW.WorkflowDataContext datacontext = new MDMBOW.WorkflowDataContext();
            datacontext.WorkflowName = workflowName;
            datacontext.WorkflowVersionId = wfVersion.Id;

            instanceId = Guid.Empty;

            // Create input parameter dictionary collection
            Dictionary<String, Object> inputParameters = new Dictionary<string, object>();

            MDMBOW.WorkflowDataContext dataContextForInputParameter = new MDMBOW.WorkflowDataContext();

            if (datacontext != null)
            {
                dataContextForInputParameter.Id = datacontext.Id;
                dataContextForInputParameter.WorkflowName = datacontext.WorkflowName;
                dataContextForInputParameter.WorkflowVersionId = datacontext.WorkflowVersionId;
                dataContextForInputParameter.MDMObjectCollection = workflowMDMObjectCollection;

                inputParameters.Add("MDMDataContext", dataContextForInputParameter);

                // Start new workflow instance
                WorkflowRuntime wfRuntime = new WorkflowRuntime();

                wfapp = wfRuntime.StartWorkflow(workflowVersion, inputParameters, operationResult, ref instanceId, callerContext);

                //Check for InstanceId
                if (instanceId == null || instanceId == Guid.Empty)
                {
                    //No.. Instance failed to create.
                    //Try once more
                    wfapp = wfRuntime.StartWorkflow(workflowVersion, inputParameters, operationResult, ref instanceId, callerContext);
                }
            }

            return wfapp;
        }

        private void ResumeWorkflow(WorkflowApplication wfapp, string runtimeInstanceId, WorkflowMDMObjectCollection workflowMDMObjectCollection, Int64 cnodeId, string activityName, string action, CallerContext callerContext)
        {
            MDMBOW.WorkflowDataContext workflowDataContext = null;

            String actingUserName = "cfadmin";
            int actingUserId = 1;
            String comments = String.Empty;

            if (!String.IsNullOrEmpty(activityName))
            {
                workflowDataContext = new MDMBOW.WorkflowDataContext();
                workflowDataContext.MDMObjectCollection = workflowMDMObjectCollection;

                OperationResult operationResult = new OperationResult();

                MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                actionContext.UserAction = action;
                actionContext.Comments = comments;
                actionContext.CurrentActivityName = activityName;
                actionContext.ActingUserName = actingUserName;
                actionContext.ActingUserId = actingUserId;

                //Resume workflow
                //workflowService.ResumeWorkflow(workflowDataContext, activityName, action, "", actingUserName, actingUserId, ref operationResult);

                ResumeBookmark(null, runtimeInstanceId, actionContext, workflowMDMObjectCollection, operationResult, callerContext);
                //ResumeBookmark1(wfapp, runtimeInstanceId, actionContext, operationResult);

                if (operationResult.HasError)
                {
                    String msg = String.Format("Error occurred while performing workflow activity action for item:{0}, activity:{1}, action:{2}. Error: ", cnodeId, activityName, action);

                    String errorString = msg;
                    foreach (Error error in operationResult.Errors)
                    {
                        errorString += error.ErrorMessage;
                    }

                    Console.WriteLine(errorString);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, errorString, MDMTraceSource.AdvancedWorkflow);
                }
                else
                {
                    String msg = String.Format("Activity action performed successfully for item:{0}, activity:{1}, action:{2}.", cnodeId, activityName, action);
                    Console.WriteLine(msg);
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, msg, MDMTraceSource.AdvancedWorkflow);
                }
            }
        }

        private void ResumeBookmark(WorkflowApplication wfapp, string runtimeInstanceId, WorkflowActionContext actionContext, WorkflowMDMObjectCollection workflowMDMObjects, OperationResult operationResult, CallerContext callerContext)
        {
            //try
            //{
            //    //Get bookmark name based on activity name and mdm object information
            //    String bookmarkName = WorkflowHelper.GetBookmarkName(actionContext.CurrentActivityName, runtimeInstanceId);

            //    //Resume the bookmark in the loaded instance by passing the input parameters to the ResumeBookmark call back method
            //    BookmarkResumptionResult resumeResult = wfapp.ResumeBookmark(bookmarkName, actionContext);
            //}
            //catch (InstanceLockedException ex)
            //{
            //    MDMBO.Error error = new MDMBO.Error();
            //    //error.ErrorMessage = "The requested workflow is under process by another user for some of the entities. Please wait for some time. This action might have aborted the workflow for those entities. In that case resume the workflow to proceed.";
            //    error.ErrorMessage = ex.Message;
            //    operationResult.Errors.Add(error);
            //}
            //catch (Exception ex)
            //{
            //    MDMBO.Error error = new MDMBO.Error();
            //    error.ErrorMessage = ex.Message;
            //    operationResult.Errors.Add(error);

            //}
            WorkflowRuntime wfRuntime = new WorkflowRuntime();
            Boolean result = wfRuntime.ResumeBookmark(wfVersion, actionContext.CurrentActivityName, runtimeInstanceId, actionContext, workflowMDMObjects, operationResult, callerContext);
        }

        private void ResumeBookmark1(WorkflowApplication wfapp, string runtimeInstanceId, WorkflowActionContext actionContext, OperationResult operationResult)
        {
            try
            {
                //Get bookmark name based on activity name and mdm object information
                String bookmarkName = WorkflowHelper.GetBookmarkName(actionContext.CurrentActivityName, runtimeInstanceId);

                //Resume the bookmark in the loaded instance by passing the input parameters to the ResumeBookmark call back method
                BookmarkResumptionResult resumeResult = wfapp.ResumeBookmark(bookmarkName, actionContext);
            }
            catch (InstanceLockedException ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                //error.ErrorMessage = "The requested workflow is under process by another user for some of the entities. Please wait for some time. This action might have aborted the workflow for those entities. In that case resume the workflow to proceed.";
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

            }
            //WorkflowRuntime wfRuntime = new WorkflowRuntime();
            //Boolean result = wfRuntime.ResumeBookmark(wfVersion.WorkflowDefinition, wfVersion.TrackingProfileObject, actionContext.CurrentActivityName, runtimeInstanceId, actionContext, operationResult);
        }

        private void SaveWorkflowInstances(Collection<MDMBOW.WorkflowInstance> colGuidForMDMObject, WorkflowInstanceRunOptions workflowInstanceRunOption, String serviceType, Int32 serviceId, String currentUserName, CallerContext callerContext)
        {
            Int32 processingType = Convert.ToInt32(workflowInstanceRunOption);

            WorkflowInstanceBL instanceBL = new WorkflowInstanceBL();
            instanceBL.Process(colGuidForMDMObject, processingType, serviceId, serviceType, currentUserName, callerContext);
        }
    }
}
